
var chatHub = $.connection.chatHub;
$(document).ready(function () {
    $('<audio id="chatAudio"><source src="' + srp + 'images/notify.ogg" type="audio/ogg"><source src="' + srp + 'images/notify.mp3" type="audio/mpeg"><source src="' + srp + 'images/notify.wav" type="audio/wav"></audio>').appendTo('body');
    // Declare a proxy to reference the hub. 
    var chatHub = $.connection.chatHub;
    registerClientMethods(chatHub);
    // Start Hub
    $.connection.hub.start().done(function () {
        registerEvents(chatHub);
    });

    $("#chat_min_button").click(function () {
        if ($(this).html() == "<i class=\"fa fa-minus-square\"></i>") {
            $(this).html("<i class=\"fa fa-plus-square\"></i>");
        }
        else {
            $(this).html("<i class=\"fa fa-minus-square\"></i>");
        }
        $("#chat_box").slideToggle();
    });
    $('#wrapSearchUser').hide();
    setInterval(ResetTypingFlag, 6000);
});


function registerEvents(chatHub) {
    var UserName = $("[id$=hdnCurrentUserName]").val();
    var UserID = parseInt($("[id$=hdnCurrentUserID]").val());
    var User_Group = $("#hdnCurrentGroup").val();

    chatHub.server.connect(UserName, UserID, User_Group);
}
var userList;
var chatHubI;
var userIDS;
function registerClientMethods(chatHub) {
    // Calls when user successfully logged in
    chatHub.client.onConnected = function (id, userName, allUsers, messages, userid) {
        $('#hdId').val(id);
        $('#hdUserName').val(userName);
        userList = allUsers;
        userIDS = userid;
        chatHubI = chatHub;

        //const result = data.reduce((acc, x) =>
        //   acc.concat(acc.find(y => y.User_Group === x.User_Group) ? [] : [x])
        // , []);
        //
        console.log(allUsers)
        //const unique = [...new Set(allUsers.map(item => item.User_Group))]; 
        const unique = Array.from(new Set(allUsers.map(s => s.User_Group)))
            .map(User_Group => {
                return {
                    User_Group: User_Group,
                    UserName: allUsers.find(s => s.User_Group === User_Group).UserName,
                    UserID: allUsers.find(s => s.User_Group === User_Group).UserID,
                    ConnectionId: allUsers.find(s => s.User_Group === User_Group).ConnectionId
                }
            })
        console.log(unique)
        var groupHtml = '';
        for (i = 0; i < unique.length; i++) {
            //AddUser(chatHub, allUsers[i].ConnectionId, allUsers[i].UserName, userid);
            groupHtml += '<div id="' + unique[i].ConnectionId + '" data-group="' + unique[i].User_Group  +' " class="col-sm-12 bg-success groupClick" > <img src="/assests/images/user1.png" class="userImg" /> <span class="uName">' + unique[i].User_Group + '</span><span class="onlineFlag"></span></div>'

        }
        $('#chat_boxpre').html(groupHtml);
        // Add All Users
        for (i = 0; i < allUsers.length; i++) {
            //AddUser(chatHub, allUsers[i].ConnectionId, allUsers[i].UserName, userid);

            //AddUser(chatHub, allUsers[i].UserID, allUsers[i].UserName, userid);
        }

        // Add Existing Messages
        for (i = 0; i < messages.length; i++) {
            AddMessage(messages[i].UserName, messages[i].Message);
        }

    }
}
$("body").on("click", ".groupClick", function(){
    var groupName = $(this).data("group").trim()
    //console.log(groupName);
    var dataList = userList;
    var inHtml= '';
    $('#chat_box').html('');
    for (i = 0; i < dataList.length; i++) {
        var s = dataList[i].User_Group;
        var TempUGrp = s.trim();

        if(TempUGrp == groupName){
            AddUser(chatHubI, dataList[i].UserID, dataList[i].UserName, userIDS);
            $('#wrapSearchGRP').hide();
            $('#wrapSearchUser').show();
            $('.chatBack').show();
           // inHtml += '<div id="' + dataList[i].UserID + '" class="col-sm-12 bg-success groupClick" ><img src="/assests/images/user1.png" class="userImg" /> <span class="uName">' + dataList[i].UserName + '</span><span class="onlineFlag"></span></div>'
        }    
    }
    setTimeout(function () {
        var options = {
            valueNames: ['uName']
        };
        var userSearch = new List('wrapSearchUser', options);
    },5000)
    //$('#chat_box').html()
   
});


// On New User Connected
chatHub.client.onNewUserConnected = function (id, name, userid) {
    AddUser(chatHub, id, name, userid);
}

// On User Disconnected
chatHub.client.onUserDisconnected = function (id, userName) {
    $('#' + id).remove();

    //var ctrId = 'private_' + id;
    //$('#' + ctrId).remove();
}

chatHub.client.messageReceived = function (userName, message) {
    AddMessage(userName, message);
}


chatHub.client.sendPrivateMessage = function (windowId, fromUserName, chatTitle, message) {
    var a = fromUserName.split("-")
    var uname = a[0];
    var uuID = a[1];
    var ctrId = 'private_' + windowId;
    if ($('#' + ctrId).length == 0) {
        createPrivateChatWindow(chatHub, windowId, ctrId, uname, chatTitle);
        $('#chatAudio')[0].play();
    }
    else {
        var rType = CheckHiddenWindow();
        if ($('#' + ctrId).parent().css('display') == "none") {
            $('#' + ctrId).parent().parent().effect("shake", { times: 2 }, 1000);
            rType = true;
        }
        if (rType == true) {
            $('#chatAudio')[0].play();
        }
    }
    $('#' + ctrId).chatbox("option", "boxManager").addMsg(uname, message, uuID);
    $('#typing_' + windowId).hide();
}


chatHub.client.send = function (windowId, fromUserName, chatTitle, message, Group) {
    var a = fromUserName.split("-")
    var uname = a[0];
    var uuID = a[1];
    var ctrId = 'private_' + windowId;
    if ($('#' + ctrId).length == 0) {
        createGroupChatWindow(chatHub, windowId, ctrId, Group, Group);
        $('#chatAudio')[0].play();
    }
    else {
        var rType = CheckHiddenWindow();
        if ($('#' + ctrId).parent().css('display') == "none") {
            $('#' + ctrId).parent().parent().effect("shake", { times: 2 }, 1000);
            rType = true;
        }
        if (rType == true) {
            $('#chatAudio')[0].play();
        }
    }
    $('#' + ctrId).chatbox("option", "boxManager").addMsg(uname, message, uuID);
    $('#typing_' + windowId).hide();
}

chatHub.client.GetLastMessages = function (TouserID, CurrentChatMessages) {
    //debugger;
    var ctrId = 'private_' + TouserID;
    var AllmsgHtml = "";
    for (i = 0; i < CurrentChatMessages.length; i++) {
        var a = CurrentChatMessages[i].FromUserName.split("-")
        var uname = a[0];
        AllmsgHtml += "";
        if (i == CurrentChatMessages.length - 1) {
            if ($('#' + ctrId).children().last().html() != "<b>" + uname + ": </b><span>" + CurrentChatMessages[i].Message + "</span>") {
                var currentUserId = $('#hdnCurrentUserIDS').val();
                if (CurrentChatMessages[i].FromUserID != currentUserId) {
                    AllmsgHtml += "<div style=\"display: block; \" class=\"ui-chatbox-msg userEnd\"><img src='/assests/images/user1.png' class='userImg' /><div class='uiCnt'><div class='uiCntHd'>" + uname + ": </div><div>" + CurrentChatMessages[i].Message + "</div></div></div>";
                } else {
                    AllmsgHtml += "<div style=\"display: block; float:right;\" class=\"ui-chatbox-msg self\"><div class='uiCnt'><div class='uiCntHd'>" + uname + ": </div><div>" + CurrentChatMessages[i].Message + "</div></div><img src='/assests/images/user2.png' class='userImg' style='margin-left:5px' /></div>";
                }
            }
        }
        else {
            var currentUserId = $('#hdnCurrentUserIDS').val();
            if (CurrentChatMessages[i].FromUserID != currentUserId) {
                AllmsgHtml += "<div style=\"display: block; \" class=\"ui-chatbox-msg userEnd\"><img src='/assests/images/user1.png' class='userImg' /><div class='uiCnt'><div class='uiCntHd'>" + uname + ": </div><div>" + CurrentChatMessages[i].Message + "</div></div></div>";
            } else {
                AllmsgHtml += "<div style=\"display: block; float:right;\" class=\"ui-chatbox-msg self\"><div class='uiCnt'><div class='uiCntHd'>" + uname + ": </div><div>" + CurrentChatMessages[i].Message + "</div></div><img src='/assests/images/user2.png' class='userImg' style='margin-left:5px' /></div>";
            }
            
        }
        AllmsgHtml += "";
    }
    $('#' + ctrId).prepend(AllmsgHtml);
    var box = $('#' + ctrId);
    box.scrollTop(box.get(0).scrollHeight);
}

function CheckHiddenWindow() {
    var hidden, state;

    if (typeof document.hidden !== "undefined") {
        state = "visibilityState";
    } else if (typeof document.mozHidden !== "undefined") {
        state = "mozVisibilityState";
    } else if (typeof document.msHidden !== "undefined") {
        state = "msVisibilityState";
    } else if (typeof document.webkitHidden !== "undefined") {
        state = "webkitVisibilityState";
    }

    if (document[state] == "hidden")
        return true;
    else
        return false;

}

function AddUser(chatHub, id, name, userid) {
    var currentuserid = parseInt($("[id$=hdnCurrentUserID]").val());
    var connectionid = $('#hdId').val();
    var code = "";
    console.log('name', name)
    var nameObj = name.split("-");
    var nameS = nameObj[0];
    if (connectionid == "") {
        if (userid == currentuserid) {
            $('#hdId').val(id);
            connectionid = id;
            $('#hdUserName').val(name);
        }
    }
    if (connectionid != id) {
        if ($('#' + id).length == 0) {
            code = $('<a id="' + id + '" class="col-sm-12 bg-success" > <img src="/assests/images/user1.png" class="userImg" /> <span class="uName">' + nameS + '</span><span class="onlineFlag"></span><a>');
            $(code).click(function () {
                var id = $(this).attr('id');
                if (connectionid != id) {
                    OpenPrivateChatWindow(chatHub, id, nameS);
                }
            });
        }
    }
    else {
        if ($('#curruser_' + id).length == 0) {
            code = $('<div id="curruser_' + id + '" class="col-sm-12 bg-info"> <img src="/assests/images/user2.png" class="userImg" /> <span class="uName">' + nameS + '</span><div>');

        }
    }
    $("#chat_box").append(code);
}

function OpenPrivateChatWindow(chatHub, id, userName) {
    var ctrId = 'private_' + id;
    if ($('#' + ctrId).length > 0) return;
    createPrivateChatWindow(chatHub, id, ctrId, userName, userName);
}

function createPrivateChatWindow(chatHub, userId, ctrId, userName, chatTitle) {
    $("#chat_div").append("<div id=\"" + ctrId + "\"></div>")
    showList.push(ctrId);
    $('#' + ctrId).chatbox({
        id: ctrId,
        title: userName,
        user: userName,
        offset: getNextOffset(),
        width: 200,
        messageSent: function (id, user, msg) {
            chatHub.server.sendPrivateMessage(userId, msg);
            TypingFlag = true;
        },
        boxClosed: function (removeid) {
            $('#' + removeid).remove();
            var idx = showList.indexOf(removeid);
            if (idx != -1) {
                showList.splice(idx, 1);
                diff = config.width + config.gap;
                for (var i = idx; i < showList.length; i++) {
                    offset = $("#" + showList[i]).chatbox("option", "offset");
                    $("#" + showList[i]).chatbox("option", "offset", offset - diff);
                }
            }
        }

    });
    $('#' + ctrId).siblings().css("position", "relative");
    $('#' + ctrId).siblings().append("<div id=\"typing_" + userId + "\" style=\"width:20px; height:20px; display:none; position:absolute; right:25px; top:8px\"><span style=\"font-size:8px\">typing...</span></div>");
    $('#' + ctrId).siblings().find('textarea').on('input', function (e) {
        if (TypingFlag == true) {
            chatHub.server.sendUserTypingRequest(userId);
        }
        TypingFlag = false;
    });

    var FromUserID = parseInt($("[id$=hdnCurrentUserID]").val());
    var ToUserID = userId;
    chatHub.server.requestLastMessage(FromUserID, ToUserID);
    //var msgTextbox = $('#' + ctrId).siblings().find('textarea');
    
    //$(msgTextbox).emojioneArea({
    //    pickerPosition: "top"
    //});

}

function createGroupChatWindow(chatHub, userId, ctrId, userName, chatTitle) {
    $("#chat_div").append("<div id=\"" + ctrId + "\"></div>")
    showList.push(ctrId);
    $('#' + ctrId).chatbox({
        id: ctrId,
        title: userName,
        user: userName,
        offset: getNextOffset(),
        width: 200,
        messageSent: function (id, user, msg) {
            chatHub.server.send(userId, msg, userName);
            TypingFlag = true;
        },
        boxClosed: function (removeid) {
            $('#' + removeid).remove();
            var idx = showList.indexOf(removeid);
            if (idx != -1) {
                showList.splice(idx, 1);
                diff = config.width + config.gap;
                for (var i = idx; i < showList.length; i++) {
                    offset = $("#" + showList[i]).chatbox("option", "offset");
                    $("#" + showList[i]).chatbox("option", "offset", offset - diff);
                }
            }
        }

    });
    $('#' + ctrId).siblings().css("position", "relative");
    $('#' + ctrId).siblings().append("<div id=\"typing_" + userId + "\" style=\"width:20px; height:20px; display:none; position:absolute; right:25px; top:8px\"><span style=\"font-size:8px\">typing...</span></div>");
    $('#' + ctrId).siblings().find('textarea').on('input', function (e) {
        if (TypingFlag == true) {
            chatHub.server.sendUserTypingRequest(userId);
        }
        TypingFlag = false;
    });

    var FromUserID = parseInt($("[id$=hdnCurrentUserID]").val());
    var ToUserID = userId;
    chatHub.server.requestLastMessage(FromUserID, ToUserID);
}

chatHub.client.ReceiveTypingRequest = function (userId) {
    var ctrId = 'private_' + userId;
    if ($('#' + ctrId).length > 0) {
        jQuery('#typing_' + userId).show();
        jQuery('#typing_' + userId).delay(6000).fadeOut("slow");
    }
}

// list of boxes shown on the page
var showList = new Array();
var config = {
    width: 200, //px
    gap: 20,
    maxBoxes: 5
};

var getNextOffset = function () {
    return (config.width + config.gap) * showList.length;
};

var TypingFlag = true;

function ResetTypingFlag() {
    TypingFlag = true;
}

function AddMessage(userName, message) {
    //$('#divChatWindow').append('<div class="message"><span class="userName">' + userName + '</span>: ' + message + '</div>');
    //var height = $('#divChatWindow')[0].scrollHeight;
    //$('#divChatWindow').scrollTop(height);
}