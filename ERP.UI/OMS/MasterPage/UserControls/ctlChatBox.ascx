<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctlChatBox.ascx.cs" Inherits="BreezeChat.controls.ctlChatBox" %>
<!--EmojiOneArea -->
    <%--<link href="/Styles/emojionearea.min.css" rel="stylesheet" />
    <script src="/Scripts/emojionearea.js"></script>--%>
<style>
    #chat_widnow {
        /*border: solid 1px #f1f1f1 !important;*/
    }
    .ui-chatbox-titlebar {
     
        border: 1px solid #003eff !important;
        background: #007fff !important;
        font-weight: normal;
        color: #ffffff !important;
        text-transform:capitalize
    }
    .minimized-view {
            position: relative;
            cursor: pointer;
            display: flex;
            align-items: center;
            justify-content: center;
            border-radius: 8px;
            width: calc(100% - 16px);
            height: 35px;
            -webkit-box-shadow: 0 6px 10px 0;
            -moz-box-shadow: 0 6px 10px 0;
            -ms-box-shadow: 0 6px 10px 0;
            -o-box-shadow: 0 6px 10px 0;
            box-shadow: 0 6px 10px 0;
            color: #fff;
            text-align: center;
            font-weight: 500;
            font-size: 14px;
            animation: slide-in-from-bottom 0.2s ease-out backwards;
            background-color: #0E4498;
            box-sizing: border-box;
            margin-bottom: 16px;
            margin-right: 16px;
            margin-top: 8px;
            margin-left: 8px;
            transition: transform 0.2s ease-out, width .4s ease-out;
            overflow: visible;
            
        }
        .minimized-view:hover {
            transform: translateY(-2px) !important;
            -webkit-box-shadow: 0 8px 15px 0;
            -moz-box-shadow: 0 8px 15px 0;
            -ms-box-shadow: 0 8px 15px 0;
            -o-box-shadow: 0 8px 15px 0;
            box-shadow: 0 8px 15px 0;
        }
        .minimized-view.open{
                width: 38px;
        }
        .minimized-view-icon {
            opacity: 1;
            transition: 0.2s ease-out;
            display: inherit;
        }
        .minimized-view-text {
            color: #FFF;
            margin-left: 8px;
            opacity: 1;
            transition: 0.2s ease-out;
            transition-delay: 0.1s;
            white-space: nowrap;
        }
        .minimized-view-cross-icon {
            position: absolute;
            top: 50%;
            left: 50%;
            transition: 0.2s ease-out;
            transform: translate3d(-50%, -50%, 0) rotate(-90deg);
            transition-delay: 0s;
            opacity: 0;
        }
        .minimized-view-cross-icon.open {
            transform: translate3d(-50%, -50%, 0) rotate(0deg);
            opacity: 1;
            transition-delay: 0.1s;
        }
        .minimized-view-text.open {
            opacity: 0;
            transform: translateY(2px);
        }
        .minimized-view-icon.open {
            opacity: 0;
            transform: translateY(2px);
        }

        .extended-view {
            display: -webkit-box;
            display: -moz-box;
            display: -webkit-flex;
            display: -ms-flexbox;
            display: flex;
            -webkit-box-direction: normal;
            -webkit-box-orient: vertical;
            -moz-box-direction: normal;
            -moz-box-orient: vertical;
            -webkit-flex-direction: column;
            -ms-flex-direction: column;
            flex-direction: column;
            -webkit-box: 1;
            -moz-box: 1;
            -webkit-flex: 1 0 auto;
            -ms-flexbox: 1 0 auto;
            flex: 1 0 auto;
            position: absolute;
            bottom: 55px;
            right: 0px;
            overflow: hidden;
            width: 300px;
            height: 350px;
            border-radius: 8px;
            overflow: hidden;
            box-shadow: 0 2px 8px 0 rgba(0,0,0,0.2);
            border-radius: 8px;
            margin: 4px;
            margin-right: 8px;
            margin-left: 8px;
            margin-bottom: 10px;
            background-color: #fff;
            opacity: 0;
            transform-origin: 50% 100%;
            transition: opacity 0.2s linear,transform 0.2s cubic-bezier(0.165, 0.84, 0.44, 1);
            transition-delay: 0.1s;
            will-change: opacity, transform;
            transform: scale3d(0, 0, 1) translate3d(0, 30px, 0);
        }
        .extended-view.active {
            opacity: 1;
            transform: scale3d(1, 1, 1) translate3d(0, 0, 0);
        }
        .header-v2-top {
            height: 55px;
            display: flex;
            justify-content: space-between;
            padding: 12px 14px 10px 12px;
            box-sizing: border-box;
            transition: 0.3s cubic-bezier(0.19, 1, 0.22, 1);
            background-color: #FAFAFA;
            border-bottom: 1.5px solid #F2F2F2;
            background:#fff;
            flex-direction: column;
        }
        .header-v2-top .tl {
            color: #3c23ca;
            font-weight: 500;
            font-size: 14px;
        }
        .userListing-area {
            padding:15px;
            min-height: 266px;
            overflow-y: auto;
            background: #f5f9ff;
        }
        .composer-branding {
            text-align: center;
            background: #f3f3f3;
            padding: 6px 3px;
        }
        .spchBtn {
            position: absolute;
            right: 10px;
            top: 17px;
            width: 30px;
            height: 30px;
            border-radius: 50px;
            background: #f3eeee;
            border: none;
        }
        .spchBtn > img {
            max-width: 100%;
        }
        .spchBtn:hover {
            background: #d4d0d0;
        }

        #chat_boxpre, #chat_box {
            margin-bottom:5px;
            height: 230px;
            overflow-y: auto;
            padding-bottom: 22px;
        }
        .scroll::-webkit-scrollbar {
            width: 3px;
        }

        .scroll::-webkit-scrollbar-track {
            -webkit-box-shadow: inset 0 0 6px rgba(0,0,0,0.3); 
            border-radius: 10px;
        }

        .scroll::-webkit-scrollbar-thumb {
            border-radius: 10px;
            -webkit-box-shadow: inset 0 0 6px rgba(0,0,0,0.12); 
        }
        #chat_boxpre>div {
            margin-bottom: 5px;
            border-radius: 4px;
            overflow: hidden;
            background: #fff;
            box-shadow: 0px 2px 5px rgb(43 42 42 / 8%);
            -webkit-transition: all 0.3s ease-in;
            transition: all 0.3s ease-in;
            padding: 7px 8px;
        }
        .chatBack {
            position: absolute;
            right: 16px;
            top: 23px;
            cursor:pointer;
        }
        .search-user-wrap {
            padding: 5px;
            background: #f5f9ff;
            position: absolute;
            width: 100%;
            left: 0;
            bottom: 0;
        }
        .search-user-wrap >input {
            margin: 0;
            border-radius: 22px;
            min-height: 33px;
            padding: 0 15px;
            border-color: #dadada;
            box-shadow: none;
        }
        .search-user-wrap >input:focus {
            border-color:#c2b8ff;
            background:#e0e3ff
        }
</style>
<script>
    $(function () {
        $('.minimized-view').click(function () {
            $(this).toggleClass('open');
            $(this).find('span').toggleClass('open');
            $('.extended-view').toggleClass('active');
        });
        $('.chatBack').hide();
        $('.chatBack').click(function () {
            $(this).hide();
            $('#wrapSearchGRP').show();
            $('#wrapSearchUser').hide();
        });
    })
    </script>
<%--chat button new--%>
<div >
    <div style="position:fixed;width:140px;right:25px;bottom:25px;z-index:99;display: flex; justify-content: flex-end;" id="dragUnit">
        <div class="minimized-view"  style="background: rgb(60 35 202);color: rgba(0, 103, 221, 0.4);">
            <span class="minimized-view-icon "><svg width="16" height="16" viewBox="0 0 16 16" version="1.1" xmlns="http://www.w3.org/2000/svg"><g id="Page-1" stroke="none" stroke-width="1" fill="none" fill-rule="evenodd"><path d="M16,2 L16,10.4444444 C16,11.5490139 15.1045695,12.4444444 14,12.4444444 L11.9109137,12.4444444 L11.9109137,16 L7.74039,12.4444444 L2,12.4444444 C0.8954305,12.4444444 0,11.5490139 0,10.4444444 L0,2 C0,0.8954305 0.8954305,0 2,0 L14,0 C15.1045695,0 16,0.8954305 16,2 Z" id="chat" fill="#FFFFFF"></path></g></svg></span>
            <span class="minimized-view-text ">Let's Chat!</span>
            <span class="minimized-view-cross-icon "><svg width="14" height="14" viewBox="0 0 14 14" version="1.1" xmlns="http://www.w3.org/2000/svg"><g id="Page-1" stroke="none" stroke-width="1" fill="none" fill-rule="evenodd"><polygon fill="#FFFFFF" id="cross-button-polygon" points="14 1.41 12.59 0 7 5.59 1.41 0 0 1.41 5.59 7 0 12.59 1.41 14 7 8.41 12.59 14 14 12.59 8.41 7"></polygon></g></svg></span>
         </div>
        <div class="extended-view " id="chat_widnow">
    <div class="header-v2-top">
        <div class="tl" id="chat_title_bar">BreezeERP</div>
        <div>Chat user to user Encrypted</div>
        <span class="chatBack"><i class="fa fa-arrow-left"></i></span>
    </div>
    <div >

    <div class="userListing-area relative">
        <div id="wrapSearchGRP">
            <div id="chat_boxpre" class="list scroll"></div>
            <div class="search-user-wrap">
                  <input type="text" class="form-control search" placeholder="Search here" onkeyup="isNumberKey(event)">
            </div>
        </div>
        <div id="wrapSearchUser">
            <div id="chat_box" class="list"></div>
            <div class="search-user-wrap">
                  <input type="text" class="form-control search" placeholder="Search here" onkeyup="isNumberKey(event)">
            </div>
        </div>
        
    </div>
    
    </div>
    <div class="composer-branding">Powered by <strong>Breeze</strong><span id="posX"></span><span id="posY"></span></div>
</div>
     </div>
    <%--chat button new--%>

</div>
<%--<div id="chat_widnow">
    <div id="chat_title_bar"> <span class="col-sm-9 text-primary" style="padding-top:4px"><strong>Online Users</strong></span>
        <div id="chat_min_button" style="margin-top: 4px;"><i class="fa fa-plus-square"></i></div>
    </div>
    <div id="chat_box" style="display: none;overflow-y:auto;">
    </div>
</div>--%>
<div id="chat_div"></div> 
<input id="hdId" type="hidden" />
<input id="hdUserName" type="hidden" />
<asp:HiddenField ID="hdnCurrentUserName" runat="server" />
<asp:HiddenField ID="hdnCurrentUserID" runat="server" />


<script src="/Scripts/jquery.signalR-2.2.0.min.js"></script>
<!--Reference the autogenerated SignalR hub script.<link href="/styles/jquery.ui.chatbox.css" rel="stylesheet" /> -->
<script src="/signalr/hubs"></script>

<script src="/scripts/jquery.ui.chatbox.js"></script>
<script src="/scripts/chatboxManager.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/list.js/1.0.2/list.min.js" integrity="sha512-MT5YyrGWqMGkIbwkVUZEWGrRDjlNx8loukEdFyzLo4s8INKVxnDQy2eFcpmnWGrFwJ+X8mRTQOJpWCayXz7+Og==" crossorigin="anonymous"></script>
<script type="text/javascript">
    function isNumberKey(evt) {
        console.log(event.keyCode)
        var charCode = event.keyCode;
        if (charCode == 13) {
            return false;
        }
        return true;
    }
    setTimeout(function () {
        var options = {
            valueNames: ['uName']
        };

        var groupSearch = new List('wrapSearchGRP', options);
        var userSearch = new List('wrapSearchUser', options);

    }, 5000)
    $(document).ready(function () {
        $('#dragUnit').draggable({
            drag: function () {
                $(this).css({ 'bottom': 'auto' });
                //var offset = $(this).offset();
                //var xPos = offset.left;
                //var yPos = offset.top;
                //$('#posX').text('x: ' + xPos);
                //$('#posY').text('y: ' + yPos);
                //$('#chat_widnow').css({
                //    "left": xPos + "px",
                //    "top": yPos + "px",
                //    "bottom": 'auto'
                //})
            }
        });

    });
    // Make the DIV element draggable:



</script>
