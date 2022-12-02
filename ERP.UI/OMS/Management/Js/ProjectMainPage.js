$(document).ready(function () {

    $.ajax({
        type: "POST",
        url: "ProjectMainPage.aspx/GetMyAnnouncement",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            console.log('annou', msg)
            var Anc = msg.d;
            var Html = '';
            var HtmlonPage = '';
            
            for (var i = 0; i < Anc.length; i++) {
                
                    Html = Html + '<div class="ti_news"><a href="#announcementDiv"><span class="t2title">' + Anc[i].title + '"</span>'
                    Html = Html + '<span>' + Anc[i].msg + '</span></a></div>'
                
                


                //Html = Html + '<div class="col-md-12 cont" id="ancDiv' + Anc[i].AncId + '">'
                //Html = Html + '<div class="alert-message ">'
                //Html = Html + '<h4>' + Anc[i].title + '</h4>'
                //Html = Html + '  <p>' + Anc[i].msg + '</p>'
                //if (Anc[i].allowCmnt)
                //    Html = Html + '<a href="#" class=" comntLink" onclick="loadComment(' + Anc[i].AncId + ',event,this)">Comment</a>'
                //Html = Html + '</div></div>'


            }
            $('#ttn2').html(Html);
            //console.log(Html)
            if (Anc.length > 0) {
                $('#newsSettingTicker').show();
                _Ticker = $("#T2").newsTicker();
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

            } else {
                $('#newsSettingTicker').hide();
            }

            // previous
            for (var i = 0; i < Anc.length; i++) {

                HtmlonPage = HtmlonPage + '<div class="col-md-12 cont" id="ancDiv' + Anc[i].AncId + '">'
                HtmlonPage = HtmlonPage + '<div class="alert-message ">'
                HtmlonPage = HtmlonPage + '<h4>' + Anc[i].title + '</h4>'
                if (Anc[i].anninHtml == "" || Anc[i].anninHtml == null) {
                    HtmlonPage = HtmlonPage + '  <p>' + Anc[i].msg + '</p>'
                }else {
                     HtmlonPage = HtmlonPage + '  <p>' + Anc[i].anninHtml + '</p>'
                }
                
                if (Anc[i].allowCmnt)
                    HtmlonPage = HtmlonPage + '<a href="#" class=" comntLink" onclick="loadComment(' + Anc[i].AncId + ',event,this)">Comment</a>'
                HtmlonPage = HtmlonPage + '</div></div>'
            }
            if (Anc.length > 0) {
                $('#mainAnc').show();
            }
            $('#announcementDiv').html(HtmlonPage)
        }
    });

    if ($("#hdnDefaultDashboardService").val() == "True") {
        $("#divPopHead").removeClass("hide");
    }

    //if ($("#dhnDashBoardSession").val() == "SRV") {
    //    PopupOk();
    //}

});


function SalesDbButtonClick() {
    //window.open('/DashBoard/Sales/SalesDashboard.aspx', '_blank');
    document.getElementsByClassName('navbar-minimalize')[0].click();
    createCookie("MenuCloseOpen", "0", 30);
    $('#fullpageDiv').remove();
    document.getElementById('loadeddiv').src = '/DashBoard/Sales/SalesDashboard.aspx';
    $('#loadeddiv').show();

}

function PurchaseDbButtonClick() {
    //    window.open('/DashBoard/Purchase/PurchaseDb.aspx', '_blank');
    document.getElementsByClassName('navbar-minimalize')[0].click();
    createCookie("MenuCloseOpen", "0", 30);
    $('#fullpageDiv').remove();
    document.getElementById('loadeddiv').src = '/DashBoard/Purchase/PurchaseDb.aspx';
    $('#loadeddiv').show();

}

function CRMButtonClick() {
    //  window.open('/DashBoard/CRM/CRMDb.aspx', '_blank');
    document.getElementsByClassName('navbar-minimalize')[0].click();
    createCookie("MenuCloseOpen", "0", 30);
    $('#fullpageDiv').remove();
    document.getElementById('loadeddiv').src = '/DashBoard/CRM/CRMDb.aspx';
    $('#loadeddiv').show();

}

function AttbtnButtonClick() {
    //  window.open('/DashBoard/Attendance/AttendanceDb.aspx', '_blank');
    document.getElementsByClassName('navbar-minimalize')[0].click();
    createCookie("MenuCloseOpen", "0", 30);
    $('#fullpageDiv').remove();
    document.getElementById('loadeddiv').src = '/DashBoard/Attendance/AttendanceDb.aspx';
    $('#loadeddiv').show();

}


function FollowupButtonClick() {
    //window.open('/DashBoard/FollowUp/FollowupDb.aspx', '_blank');
    document.getElementsByClassName('navbar-minimalize')[0].click();
    createCookie("MenuCloseOpen", "0", 30);
    $('#fullpageDiv').remove();
    document.getElementById('loadeddiv').src = '/DashBoard/FollowUp/FollowupDb.aspx';
    $('#loadeddiv').show();

}
function showDashboard() {
    //window.open('/DashBoard/FollowUp/FollowupDb.aspx', '_blank');
    document.getElementsByClassName('navbar-minimalize')[0].click();
    createCookie("MenuCloseOpen", "0", 30);
    $('#fullpageDiv').remove();
    document.getElementById('loadeddiv').src = '/DashBoard/Accounts/AccountsDb.aspx';
    $('#loadeddiv').show();

}
function showTasklist() {
    //window.open('/DashBoard/FollowUp/FollowupDb.aspx', '_blank');
    document.getElementsByClassName('navbar-minimalize')[0].click();
    createCookie("MenuCloseOpen", "0", 30);
    $('#fullpageDiv').remove();
    document.getElementById('loadeddiv').src = '/DashBoard/TaskList/TaskList.aspx';
    $('#loadeddiv').show();

}

function showCSTProfile() {
    document.getElementsByClassName('navbar-minimalize')[0].click();
    createCookie("MenuCloseOpen", "0", 30);
    $('#fullpageDiv').remove();
    document.getElementById('loadeddiv').src = '/DashBoard/CustomerProfile/customerProfile.aspx';
    $('#loadeddiv').show();
}

function PopupOk() {
    if ($("#hdnDefaultDashboardService").val() == "True") {
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
    else if ($("#hdnDefaultDashboardSTB").val() == "True") {
        var OtherDetails = {}
        OtherDetails.comment = "STB";
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
        document.getElementById('loadeddiv').src = '/DashBoard/STBMng/StbMangement.aspx';
        $('#loadeddiv').show();
        $("#divPopHead").addClass("hide");
    }
    else {
        $("#divPopHead").addClass("hide");
    }
}


function showProjectDB() {
    //window.open('/DashBoard/FollowUp/FollowupDb.aspx', '_blank');
    document.getElementsByClassName('navbar-minimalize')[0].click();
    createCookie("MenuCloseOpen", "0", 30);
    $('#fullpageDiv').remove();
    document.getElementById('loadeddiv').src = '/DashBoard/ProjectDB/projectDB.aspx';
    $('#loadeddiv').show();

}
function showFinancialDB() {
    //window.open('/DashBoard/FollowUp/FollowupDb.aspx', '_blank');
    document.getElementsByClassName('navbar-minimalize')[0].click();
    createCookie("MenuCloseOpen", "0", 30);
    $('#fullpageDiv').remove();
    document.getElementById('loadeddiv').src = '/DashBoard/FinancialDB/FinancialDB.aspx';
    $('#loadeddiv').show();

}
function showquickReports() {
    //window.open('/DashBoard/FollowUp/FollowupDb.aspx', '_blank');
    document.getElementsByClassName('navbar-minimalize')[0].click();
    createCookie("MenuCloseOpen", "0", 30);
    $('#fullpageDiv').remove();
    document.getElementById('loadeddiv').src = '/DashBoard/QcReportsLinks/qcReports.aspx';
    $('#loadeddiv').show();

}
function showkpi() {
    //window.open('/DashBoard/FollowUp/FollowupDb.aspx', '_blank');
    document.getElementsByClassName('navbar-minimalize')[0].click();
    createCookie("MenuCloseOpen", "0", 30);
    $('#fullpageDiv').remove();
    document.getElementById('loadeddiv').src = '/DashBoard/kpiSummary/kpisummary.aspx';
    $('#loadeddiv').show();

}
function showInventory() {
    //window.open('/DashBoard/FollowUp/FollowupDb.aspx', '_blank');
    document.getElementsByClassName('navbar-minimalize')[0].click();
    createCookie("MenuCloseOpen", "0", 30);
    $('#fullpageDiv').remove();
    document.getElementById('loadeddiv').src = '/DashBoard/Inventory/InventoryDB.aspx';
    $('#loadeddiv').show();

}
function showMngNot() {
    //window.open('/DashBoard/FollowUp/FollowupDb.aspx', '_blank');
    document.getElementsByClassName('navbar-minimalize')[0].click();
    createCookie("MenuCloseOpen", "0", 30);
    $('#fullpageDiv').remove();
    document.getElementById('loadeddiv').src = '/DashBoard/managementNotification/mngnotification.aspx';
    $('#loadeddiv').show();

}
function showappWaiting() {
    //window.open('/DashBoard/FollowUp/FollowupDb.aspx', '_blank');
    document.getElementsByClassName('navbar-minimalize')[0].click();
    createCookie("MenuCloseOpen", "0", 30);
    $('#fullpageDiv').remove();
    document.getElementById('loadeddiv').src = '/DashBoard/ApprovalWaiting/ApprovalWaiting.aspx';
    $('#loadeddiv').show();

}
function showSTB() {
    //window.open('/DashBoard/FollowUp/FollowupDb.aspx', '_blank');
    document.getElementsByClassName('navbar-minimalize')[0].click();
    createCookie("MenuCloseOpen", "0", 30);
    $('#fullpageDiv').remove();
    document.getElementById('loadeddiv').src = '/DashBoard/STBMng/StbMangement.aspx';
    $('#loadeddiv').show();
}
function showCustomerRMDB() {
    document.getElementsByClassName('navbar-minimalize')[0].click();
    createCookie("MenuCloseOpen", "0", 30);
    $('#fullpageDiv').remove();
    document.getElementById('loadeddiv').src = '/DashBoard/CRMDASH/crmDash.aspx';
    $('#loadeddiv').show();
}

function getCommentBox(AncId) {
    //var html = '<textarea   placeholder="Comments..." rows="3" onkeydown="postcomment(event,' + AncId + ',this)" ></textarea>';

    var html = ' <textarea rows="4"   cols="50" style="height:50px" class="CommentaddBox"  onkeydown="postcomment(event,' + AncId + ',this)"   placeholder="Type Comment and press Enter..." ></textarea>'
    return html;
}

var lastAncId;
function postcomment(e, id, input) {
    if (e.key == "Enter" && e.shiftKey == false) {
        var comment = input.value.replace(/\n/g, '<br />');
        var OtherDetails = {}
        OtherDetails.comment = comment;
        OtherDetails.AncId = id;

        lastAncId = id


        $.ajax({
            type: "POST",
            url: "ProjectMainPage.aspx/SaveComment",
            data: JSON.stringify(OtherDetails),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                loadCommentSection(lastAncId);
            }
        });

        e.preventDefault();
    }
}


function loadCommentSection(id) {
    var OtherDetails = {}
    OtherDetails.AncId = id;
    lastAncId = id;

    $.ajax({
        type: "POST",
        url: "ProjectMainPage.aspx/Gettop5Comment",
        data: JSON.stringify(OtherDetails),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var cmtobj = msg.d.cmt;
            var Html = '<ul class="comntList">';
            for (var i = 0; i < cmtobj.length; i++) {
                Html = Html + '<li><div>       <span class="pull-left iconHolder"><i class="fa fa-user"></i></span>';
                Html = Html + '<div class="commentSecOld">'


                Html = Html + '<h5>' + cmtobj[i].user_name + '<span class="timeClass">' + cmtobj[i].CommentOn + '<span></h5>';
                Html = Html + '<div class="msgbody">' + cmtobj[i].Comment + '</div></div>';
                Html = Html + '</div></li>'

            }
            Html = Html + '</ul>'

            if (msg.d.totalCount > 3) {
                Html = Html + '<div class="col-md-12 CommentaddBox loadallcmt" onclick=loadAllComment(' + lastAncId + ')> Load all comments(' + msg.d.totalCount + ')...</div>'
            }

            Html = Html + getCommentBox(lastAncId);

            $('.comntList').remove();
            $('.CommentaddBox').remove();
            $('#ancDiv' + lastAncId).append(Html);

        }
    });
}


function loadComment(id, e, input) {

    if (input && input.innerText == 'Hide Comment(s)') {
        $('.comntList').remove();
        $('.CommentaddBox').remove();
        $('.comntLink').text('Comment');
        return;
    }

    $('.comntLink').text('Comment');
    if (input) {
        input.innerText = 'Hide Comment(s)';
    }

    loadCommentSection(id);
    if (e)
        e.preventDefault();
}

//function RefereshCommnetsFeed() {

//    setTimeout(function () {
//        RefereshCommnetsFeed();
//    }, 10000);

//    if ($('.comntLink').text().includes('Hide Comment(s)')){
//        loadCommentSection(lastAncId);
//    }
//}

//setTimeout(function () {
//    RefereshCommnetsFeed();
//}, 10000);


function loadAllComment(id) {



    $('#allComment').modal('show');
    var OtherDetails = {}
    OtherDetails.AncId = id;
    lastAncId = id;

    $.ajax({
        type: "POST",
        url: "ProjectMainPage.aspx/GetAllComment",
        data: JSON.stringify(OtherDetails),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var cmtobj = msg.d;
            var Html = '<ul class="comntList1">';
            for (var i = 0; i < cmtobj.length; i++) {
                Html = Html + '<li><div>       <span class="pull-left iconHolder"><i class="fa fa-user"></i></span>';
                Html = Html + '<div class="commentSecOld">'


                Html = Html + '<h5>' + cmtobj[i].user_name + '<span  class="timeClass">' + cmtobj[i].CommentOn + '<span></h5>';
                Html = Html + '<div class="msgbody">' + cmtobj[i].Comment + '</div></div>';
                Html = Html + '</div></li>'

            }
            Html = Html + '</ul>'



            //Html = Html + getCommentBox(lastAncId);

            //$('.comntList').remove();
            //$('.CommentaddBox').remove();
            //$('#ancDiv' + lastAncId).append(Html);
            $('#allcmt').html(Html);
        }
    });


}



function showWeekFunctionAlert() {

    showCustomNewNotify(' <div style="text-align: center"  > <img src="../../assests/images/exclamation-mark.png" style="margin: 5px 0 15px 0;"/> <div> ERP system detected that you may face security threat. <br/> Reason : Your login password is found very weak. <br />Wish to change it now? <a href="/OMS/Management/ToolsUtilities/frmchangepassword.aspx" style="color:#f9f210"> <strong>Click Here...</strong></a> </div></div>');
}

function showCustomNewNotify(text) {
    var x = document.getElementById("CustomNewNotify");
    x.innerHTML = text;
    x.className = "show";
    setTimeout(function () { x.className = x.className.replace("show", ""); }, 16000);
}