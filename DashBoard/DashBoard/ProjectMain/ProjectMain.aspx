<%@ Page Title="" Language="C#" MasterPageFile="~/oms/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="ProjectMain.aspx.cs" Inherits="DashBoard.DashBoard.ProjectMain.ProjectMain" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        ModuleNameForHelp = "Dashboard"

    </script>
    <link href="https://fonts.googleapis.com/css?family=Poppins:300,300i,400,400i,500,600,700,800&display=swap" rel="stylesheet"/>
    
    <script src="../../assests/pluggins/ticker/jquery.tickerNews.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

   
    <%--<link rel="stylesheet" href="https://daneden.github.io/animate.css/animate.min.css" />--%>
    <script>
        $(document).ready(function () {
            $('.rightSide').addClass('dashboardRight');
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
            getAllData();


        });

        function getAllData() {
            var dt = {};
            dt.action = "ALL";
            $.ajax({
                type: "POST",
                url: "ProjectMainPage.aspx/GetAllNotificationData",
                data: JSON.stringify(dt),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (data) {

                    //var obj = { a: "1", b: "2", c: "3" };
                    var obj = data.d[0];
                    console.log('obje', obj)
                    var total = 0;
                    for (var el in obj) {
                        if (obj.hasOwnProperty(el)) {
                            total += parseFloat(obj[el]);
                        }
                    }
                    $('#NotLight').text(total);
                    //console.log('total', total)
                },
                error: function (data) {
                    console.log(data);
                }
            });
        }


        function sum(obj) {
            var sum = 0;
            for (var el in obj) {
                if (obj.hasOwnProperty(el)) {
                    sum += parseFloat(obj[el]);
                }
            }
            return sum;
        }
        function SwitchtoServiceManagement() {

            var OtherDetails = {}
            OtherDetails.comment = "SRV";
            $.ajax({
                type: "POST",
                url: "ProjectMainPage.aspx/SrvSession",
                data: JSON.stringify(OtherDetails),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    //  loadCommentSection(lastAncId);
                }
            });

            //window.open('/DashBoard/FollowUp/FollowupDb.aspx', '_blank');
            document.getElementsByClassName('navbar-minimalize')[0].click();
            createCookie("MenuCloseOpen", "0", 30);
            $('#fullpageDiv').remove();
            document.getElementById('loadeddiv').src = '/DashBoard/ServiceManagement/SvcMgmtDshbrd.aspx';
            $('#loadeddiv').show();
            $("#divPopHead").addClass("hide");

        }
    </script>
    
    <script src="https://cdn.rawgit.com/abdmob/x2js/master/xml2json.js"></script>
    
    <script type="text/javascript">
        var x2js = new X2JS();

        function convertXml2JSon() {
            $("#jsonArea").val();
        }
        //$(function () {
        //    const RSS_URL = 'https://news.abplive.com/news/india/feed';
        //    //const RSS_URL = 'http://goidirectory.nic.in/rss/new_additions_rss.php';
        //    $.ajax(RSS_URL, {
        //        accepts: {
        //                xml: "application/rss+xml"
        //            },

        //            dataType: "xml",

        //            success: function (data) {
        //                var $XML = $(data);
        //                var toNItem = "";
        //                $XML.find("item").each(function () {
        //                    var $this = $(this),
        //                        item = {
        //                            title: $this.find("title").text(),
        //                            link: $this.find("link").text(),
        //                            description: $this.find("description").text(),
        //                            pubDate: $this.find("pubDate").text(),
        //                            author: $this.find("author").text()
        //                        };
        //                    toNItem += "<div class='ti_news'>"
        //                    toNItem += "<a href='" + item.link + "' target='_blank'>" + item.title + "</a>"
        //                    toNItem +="</div>"
        //                    //etc...
        //                });
        //                console.log(toNItem);
        //                $('#ttn').html(toNItem);
        //                $("#T1").newsTicker();

        //            }
        //    });

        $('#newsSetting').hide();
        var totalFeed = '';
        var todoObj = {};
        todoObj.links = 'https://www.incometaxindia.gov.in/_layouts/15/Dit/Pages/Rss.aspx?List=Latest%20Tax%20Updates';
        $.ajax({
            type: "POST",
            url: "ProjectMainPage.aspx/ParseRssFile",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(todoObj),
            dataType: "json",
            success: function (data) {
                var msg = data.d;
                //console.log(msg)
                if (msg == null) {
                    $('#newsSetting').hide();
                } else {
                    //$('#ttn').html(data.d);
                    totalFeed = totalFeed + data.d;
                    todoObj = {};
                    todoObj.links = 'https://www.incometaxindia.gov.in/_layouts/15/Dit/Pages/Rss.aspx?List=Press%20Release';
                    $.ajax({
                        type: "POST",
                        url: "ProjectMainPage.aspx/ParseRssFile",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(todoObj),
                        dataType: "json",
                        success: function (data) {
                            var msg = data.d;

                            if (msg == null) {
                                $('#newsSetting').hide();
                            } else {
                                totalFeed = totalFeed + data.d;
                                //console.log(totalFeed)
                                $('#ttn').html(totalFeed);
                                _Ticker = $("#T1").newsTicker();
                                _Ticker.on("mouseenter", function () {
                                    var __self = this;
                                    timer = setTimeout(function () {
                                        __self.pauseTicker();
                                    }, 200);
                                });
                                _Ticker.on("mouseleave", function () {
                                    clearTimeout(timer);
                                    if (!timer) return !1;
                                    this.startTicker();
                                });
                                $('#newsSetting').show();

                            }
                        },
                        error: function (msg) {
                            console.log(msg)
                            $('#newsSetting').hide();
                        }
                    });
                    //$("#T1").newsTicker();
                    //$('#newsSetting').show();
                }
            },
            error: function (msg) {
                console.log(msg)
                $('#newsSetting').hide();
            }
        });





        /*########################################################
            before the use of $.getJSON you need to set {crossOrigin: true} through $.ajaxSetup
            
            $.ajaxSetup({
                crossOrigin: true
            });
            
            $.getJSON(url, null, function(data) {
                $( '#test' ).html(data);
            });
        ########################################################*/




        $(document).ready(function () {
            $('.noticon').click(function (e) {
                $('.notWindow').toggleClass('active');
                $(this).toggleClass('active');
            });
            $('.tgl').click(function (e) {
                $('.tickerNws').slideToggle();
                $(this).toggleClass('active');
            });


            showAll();
        });
        function showAll() {
            $('.countLbl').removeClass('active');
            $('#showAllLbl').addClass('active');

            $('.notWraper').hide();
            var todoObj = {};
            todoObj.userid = '378';
            $.ajax({
                type: "POST",
                url: "service/general.asmx/TodoData",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(todoObj),
                dataType: "json",
                success: function (data) {
                    var mainDta = data.d;
                    // watchlistDataLoop(data.d);

                    var tureCount = $.grep(mainDta, function (e) { return e.ISCOMPLETED == true });
                    var falseCount = $.grep(mainDta, function (e) { return e.ISCOMPLETED == false && e.WARNING == false });
                    var overdueCount = $.grep(mainDta, function (e) { return e.WARNING == true });
                    console.log(falseCount)
                    if (falseCount.length > 0 || overdueCount > 0) {
                        $('#pendingCount').html(falseCount.length);
                        $('#overDueCount').html(overdueCount.length);
                        $('.notWraper').show();
                    } else {
                        $('.notWraper').hide();
                    }

                    //console.log(falseCount);
                    //console.log(overdueCount);
                },
                error: function (data) {
                    $('.notWraper').hide();
                }
            });
        }


        $(function () {
            //var timer = !1;
            //_Ticker = $("#T1").newsTicker();
            //_Ticker.on("mouseenter", function () {
            //    var __self = this;
            //    timer = setTimeout(function () {
            //        __self.pauseTicker();
            //    }, 200);
            //});
            //_Ticker.on("mouseleave", function () {
            //    clearTimeout(timer);
            //    if (!timer) return !1;
            //    this.startTicker();
            //});
        });
    </script>
    
    <script type="text/javascript">


        $(document).ready(function () {
            $('#loaderP').hide();
            $('.nBoxes').click(function () {
                $('#loaderP').show();
            });
            $('#loadeddiv').load(function () {
                $('#loaderP').hide();
            })
        });
    </script>
    <style>
        #NotLight{
            background: #2dbd88;
            position: absolute;
            display: block;
            min-width: 25px;
            height: 25px;
            line-height: 26px;
            font-size: 14px;
            border-radius: 50%;
            right: 5px;
            top: 5px;
            color: #fff;
            font-weight: 500;
        }
    </style>
    <link href="CSS/ProjectManagementPage.css" rel="stylesheet" />
    <!-- POPUP -->
    <div class="popupWraper hide" id="divPopHead" runat="server">
        <div class="popBox">
            <img src="/assests/images/pop_notification.png" class="mBot10" style="width: 50px;" />
            <h1 id="h1heading" runat="server"><%--Discussion heading--%></h1>
            <p id="pParagraph" runat="server">
                <%--Then system moves to ‘Dashboard’ or landing page where the entire control and
                    activity component will be available for service management.--%>
            </p>
            <button type="button" class="btn btn-sign" onclick="return PopupOk()">OK</button>
        </div>

    </div>
    <!-- POPUP -->

    <!-- Notifiaction area-->
            <div class="notWraper ">
                <div class="noticon">
                    <img src="../../assests/images/bell.png" class="nt" />
                    <img src="../../assests/images/nCros.png" class="cross" />
                </div>
                <div class="notWindow">
                    <div class="notHeader">Notifications</div>
                    <div class="notContent">
                        <div class="tasks">
                            <div onclick="showTasklist()">
                                <div class="numb" id="pendingCount" >0</div>
                                <div class="txt">Tasks Pending</div>
                            </div>
                            <div class="od" onclick="showTasklist()">
                                <div class="numb" id="overDueCount">0</div>
                                <div class="txt">Tasks Overdue</div>
                            </div>
                        </div>
                        <div id="content-div"></div>
                    </div>
                </div>
            </div>
     <!-- Notifiaction area-->
    <div class="clearfix relative" id="newsSetting" >
        <span class="tgl">
            <i class="fa fa-arrow-up"></i>
            <i class="fa fa-arrow-down"></i>
        </span>
        <div class="tickerNws ">
            <div class="tag"><i class="fa fa-rss" aria-hidden="true"></i> UPDATES</div>
            <div class="TickerNews" id="T1">
		        <div class="ti_wrapper">
		            <div class="ti_slide">
		                <div class="ti_content" id="ttn">
		                
						
		                </div>
		            </div>
		        </div>
	        </div>
            
        </div>
    </div>
    <div id="loaderP">
        <img src="../../assests/images/Svg/bars.svg" style="max-width: 45px;" />
    </div>
    <iframe id="loadeddiv" style="position: absolute; height: 100%; width: 98%; border: none; display: none;"></iframe>
    <button type="button " class="hide" data-target="#slideSider" data-function="pushSlide">Open from Side</button>
    <div class="pushslider col-md-8 hidden " id="slideSider">
        <div class="pushslider-header">Header </div>
        <div class="pushslider-close" data-close="#slideSider"><i class="fa fa-close"></i></div>
        <div class="pushslider-content">
            <p >Something from here</p>
        </div>
    </div>
    <div style="padding:3px 0  7px" id="divSwitchtoServiceManagement" runat="server">
        <div class="switchToService" onclick="SwitchtoServiceManagement()"><i class="fa fa-arrow-right"></i> Switch to Service Management</div>
    </div>
    <div class="padding " id="fullpageDiv">
        <script src="Js/ProjectMainPage.js?v=0.17"></script>
        
       

        <div class=" dashitems ">
            <div class="widget" runat="server" id="SalesDbButton" visible="false" onclick="SalesDbButtonClick()">
                <div class="nBoxes" >
                    <div class="icon-part"><img src="../../assests/images/webP/DashboardIcons/sale.webp" /></div>
                    <div class="Ntilt">Sales Analytics</div>
                </div>
            </div>
            <div class="widget" runat="server" id="PurchaseDbButton" visible="false" onclick="PurchaseDbButtonClick()">
                <div class="nBoxes" >
                    <img src="../../assests/images/webP/DashboardIcons/analytics%20(2).webp" />
                    <div class="Ntilt"> Purchase Analytics</div>
                </div>
            </div>
            <div class="widget" runat="server" id="CRMButton" visible="false" onclick="CRMButtonClick()">
                <div class="nBoxes" >
                    <img src="../../assests/images/webP/DashboardIcons/crm.webp" />
                    <div class="Ntilt">CRM Analytics </div>
                </div>
            </div>
            <div class="widget" runat="server" id="Attbtn" visible="false" onclick="AttbtnButtonClick()">
                <div class="nBoxes" >
                    <img src="../../assests/images/webP/DashboardIcons/attendance.webp" />
                    <div class="Ntilt">Today's Attendance</div>
                </div>
            </div>
            <div class="widget" runat="server" id="followupBtn" visible="false" onclick="FollowupButtonClick()">
                <div class="nBoxes" >
                    <img src="../../assests/images/webP/DashboardIcons/analytics%20(1).webp" />
                    <div class="Ntilt">Payment Followup</div>
                </div>
            </div>
            <div class="widget" runat="server" id="AccountsBtn" visible="false" onclick="showDashboard()">
                <div class="nBoxes" >
                    <img src="../../assests/images/webP/DashboardIcons/accounting.webp" />
                    <div class="Ntilt">Account Dashboard</div>
                </div>
            </div>
            <div class="widget" runat="server" id="tasklistbtn" visible="false" onclick="showTasklist()">
                <div class="nBoxes" >
                    <img src="../../assests/images/webP/DashboardIcons/scheduling.webp" />
                    <div class="Ntilt">Task List </div>
                </div>
            </div>
            <div class="widget" onclick="showProjectDB()" visible="false" id="PMSButton" runat="server">
                <div class="nBoxes" >
                    <img src="../../assests/images/webP/DashboardIcons/project.webp" />
                    <div class="Ntilt">Project Management </div>
                </div>
            </div>
            <div class="widget hide" onclick="showFinancialDB()" visible="false" id="FinancialButton" runat="server">
                <div class="nBoxes" >
                    <img src="../../assests/images/webP/DashboardIcons/stock.webp" />
                    <div class="Ntilt">Financial</div>
                </div>
            </div>
            <div class="widget" onclick="showCustomerRMDB()" id="CustRMButton" runat="server" visible="false">
                <div class="nBoxes" >
                    <img src="../../assests/images/webP/DashboardIcons/customer.webp" />
                    <div class="Ntilt">Customer Relations...</div>
                </div>
            </div>

            <div class="widget hide" onclick="showProjectlist()">
                <div class="nBoxes" >
                    <img src="../../assests/images/webP/DashboardIcons/stock.webp" />
                    <div class="Ntilt">Project Analytics</div>
                </div>
            </div>
            <div class="widget hide" onclick="showquickReports()">
                <div class="nBoxes" >
                    <img src="../../assests/images/webP/DashboardIcons/customer.webp" />
                    <div class="Ntilt">Quick Reports</div>
                </div>
            </div>
            <div class="widget hide" onclick="showkpi()" id="dvKPISummary" runat="server"  visible="false">
                <div class="nBoxes">
                    <img src="../../assests/images/webP/DashboardIcons/kpi.webp" />
                    <div class="Ntilt">KPI Summary</div>
                </div>
            </div>
            <div class="widget " onclick="showInventory()" id="dvInveDashboard" runat="server" visible="false">
                <div class="nBoxes">
                    <img src="../../assests/images/webP/DashboardIcons/inventory.webp" />
                    <div class="Ntilt">Inventory dashboard</div>
                </div>
            </div>
            <div class="widget" onclick="showMngNot()">
                    <div class="nBoxes">
                        <span id="NotLight">0</span>
                        <img src="../../assests/images/webP/DashboardIcons/mangementNotify.webp" />
                        <div class="Ntilt">Management Notification</div>
                    </div>
            </div>
            
        </div>


        <div class="mTop5 col-md-12 pTop10" style="display: none" id="mainAnc">
            <h3 style="font-family: 'Poppins', sans-serif !IMPORTANT;font-size:18px;font-weight: 400;">Today's Announcement <img style="width:20px;margin-left:8px"  src="../../assests/svg/speaker.svg"></h3>
            <div class="row" id="announcementDiv" style="font-family: 'Poppins', sans-serif !IMPORTANT;font-size:13px;font-weight: 400;">
            </div>
        </div>
    </div>

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

    <asp:HiddenField ID="dhnDashBoardSession" runat="server" />

</asp:Content>
