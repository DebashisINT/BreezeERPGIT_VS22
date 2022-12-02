<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TaskList.aspx.cs" Inherits="DashBoard.DashBoard.TaskList.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <title>Task List Dashboard</title>
    <link href="../css/bootstrap.min.css" rel="stylesheet" />
    <link href="../css/main.css" rel="stylesheet" />
    <link href="../css/font-awesome/css/font-awesome.min.css" rel="stylesheet" />
    <link href="../css/SearchPopup.css" rel="stylesheet" />
    
    <script src="../Js/jquery.3.3.1.js"></script>
    <link href="../Js/datatable/jquery.dataTables.css" rel="stylesheet" />
    <script src="../Js/datatable/jquery.dataTables.js"></script>
   
    <script src="../Js/bootstrap.min.js"></script>
    <link href="../css/newtheme.css" rel="stylesheet" />


    <link href="../Js/styles.css" rel="stylesheet" />
    <script src="../Js/apexcharts.min.js"></script>

    <link href="../Js/driver/driver.min.css" rel="stylesheet" />
    <script src="../Js/driver/driver.min.js"></script>
    <link href="TaskList.css" rel="stylesheet" />

    
    <script>
        $(document).ready(function () {
            $('body').on('click', '.panelAnchor', function () {
                $('#panelRight').toggleClass('open');
                //if( $('#panelRight').hasClass('open') ){
                //    $('body').append('<div class="overlay"></div>');
                //}else{
                //    $('.overlay').remove();
                //}
            });

            var driver = new Driver();

            // Define the steps for introduction
            //driver.defineSteps([
            //  {
            //      element: '#branchSelect',
            //      popover: {
            //          className: 'first-step-popover-class',
            //          title: 'Guide tour',
            //          description: 'You can choose branch from there. Upon choosing only the selected branch data will be shown throughout the page otherwise all branches will be selected by default',
            //          position: 'left'
            //      }
            //  },
            //  {
            //      element: '#Expensethismonth',
            //      popover: {
            //          title: 'Title on Popover',
            //          description: 'Body of the popover',
            //          position: 'bottom'
            //      }
            //  },
            //  {
            //      element: '#bankBalance',
            //      popover: {
            //          title: 'Title on Popover',
            //          description: 'Body of the popover',
            //          position: 'bottom'
            //      }
            //  },
            //]);

            //// Start the introduction
            //driver.start();
        });
        function theFunction() {
            //$("#Dashboard_Div").addClass("hide");
            $("#fullpageDiv").removeClass("hide");
        }
</script>


    <link href="https://fonts.googleapis.com/css?family=Fira+Sans:200,300,400,500,600|Montserrat:300,400,500,600,700|Open+Sans+Condensed:300,700|Open+Sans:300,400,600,700" rel="stylesheet" />

    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/jquery-confirm/3.3.2/jquery-confirm.min.css"/>
<script src="https://cdnjs.cloudflare.com/ajax/libs/jquery-confirm/3.3.2/jquery-confirm.min.js"></script>
    <script>
        function reloadParent() {
            parent.document.location.href = '/oms/management/projectmainpage.aspx'
        }
        $(document).ready(function () {
            function createCalender(data) {
                var evt = [];
                
                for (var i = 0; i < data.length; i++) {

                    var obj = {};
                    obj.title = data[i].TASK_SUBJECT;
                    obj.start = data[i].TASK_DUEDATE;
                    obj.description = data[i].TASK_DESCRIPTION;
                    evt.push(obj);
       

                    
                }
                console.log(evt);
                
                $('#EventCal').fullCalendar({
                    themeSystem: 'bootstrap3',
                    header: {
                        left: 'prev,next today',
                        center: 'title',
                        right: 'month,listMonth'
                    },
                    weekNumbers: true,
                    eventLimit: true,
                    events: evt,
                    eventRender: function (event, element) {
                        element.qtip({
                            content: event.description,
                            position: {
                                my: 'top left',  // Position my top left...
                                at: 'left bottom', // at the bottom right of...
                            }
                        });
                    }
                });
            }
            
            var todoObj = {};
            todoObj.userid = '378';
            $.ajax({
                type: "POST",
                url: "../service/general.asmx/TodoData",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(todoObj),
                dataType: "json",
                success: function (data) {
                    // watchlistDataLoop(data.d);
                    createTodoList(data.d);
                    createCalender(data.d);
                }
            });

            
               
        });

        function resizer() {
            var calParentSize = $('#calParentSize').height();

        }
        function completeTask(id,s) {
            //alert(id);
            //if ($(this).attr(':checked')) {
            //    alert('You Un-Checked it');
            //} else {
            //    alert('You have Checked it');
            //}


            if ($(s).is(':checked')) {
                $.confirm({
                    title: 'Do you want to Complete this task!',
                    content: '',
                    buttons: {
                        confirm: function () {
                            var taskEdit = {};
                            taskEdit.taskId = id;
                            taskEdit.Status = "0";
                            $.ajax({
                                type: "POST",
                                url: "../service/general.asmx/UpdateTask",
                                contentType: "application/json; charset=utf-8",
                                data: JSON.stringify(taskEdit),
                                dataType: "json",
                                success: function (data) {
                                    function createCalender(data) {
                                        var evt = [];

                                        for (var i = 0; i < data.length; i++) {

                                            var obj = {};
                                            obj.title = data[i].TASK_SUBJECT;
                                            obj.start = data[i].TASK_DUEDATE;
                                            obj.description = data[i].TASK_DESCRIPTION;
                                            evt.push(obj);



                                        }
                                        console.log(evt);

                                        $('#EventCal').fullCalendar({
                                            themeSystem: 'bootstrap3',
                                            header: {
                                                left: 'prev,next today',
                                                center: 'title',
                                                right: 'month,listMonth'
                                            },
                                            weekNumbers: true,
                                            eventLimit: true,
                                            events: evt,
                                            eventRender: function (event, element) {
                                                element.qtip({
                                                    content: event.description,
                                                    position: {
                                                        my: 'top left',  // Position my top left...
                                                        at: 'left bottom', // at the bottom right of...
                                                    }
                                                });
                                            }
                                        });
                                    }

                                    var todoObj = {};
                                    todoObj.userid = '378';
                                    $.ajax({
                                        type: "POST",
                                        url: "../service/general.asmx/TodoData",
                                        contentType: "application/json; charset=utf-8",
                                        data: JSON.stringify(todoObj),
                                        dataType: "json",
                                        success: function (data) {
                                            // watchlistDataLoop(data.d);
                                            createTodoList(data.d);
                                            createCalender(data.d);
                                        }
                                    });


                                }
                            });
                        },
                        cancel: function () {
                            //$.alert('Canceled!');
                            $(s).filter(':checkbox').prop('checked', false);
                        }   
                    }
                });

            } else {
                $.confirm({
                    title: 'Do you want to Uncomplete this task!',
                    content: '',
                    buttons: {
                        confirm: function () {
                            var taskEdit = {};
                            taskEdit.taskId = id;
                            taskEdit.Status = "1";
                            $.ajax({
                                type: "POST",
                                url: "../service/general.asmx/UpdateTask",
                                contentType: "application/json; charset=utf-8",
                                data: JSON.stringify(taskEdit),
                                dataType: "json",
                                success: function (data) {
                                    function createCalender(data) {
                                        var evt = [];

                                        for (var i = 0; i < data.length; i++) {

                                            var obj = {};
                                            obj.title = data[i].TASK_SUBJECT;
                                            obj.start = data[i].TASK_DUEDATE;
                                            obj.description = data[i].TASK_DESCRIPTION;
                                            evt.push(obj);



                                        }
                                        console.log(evt);

                                        $('#EventCal').fullCalendar({
                                            themeSystem: 'bootstrap3',
                                            header: {
                                                left: 'prev,next today',
                                                center: 'title',
                                                right: 'month,listMonth'
                                            },
                                            weekNumbers: true,
                                            eventLimit: true,
                                            events: evt,
                                            eventRender: function (event, element) {
                                                element.qtip({
                                                    content: event.description,
                                                    position: {
                                                        my: 'top left',  // Position my top left...
                                                        at: 'left bottom', // at the bottom right of...
                                                    }
                                                });
                                            }
                                        });
                                    }

                                    var todoObj = {};
                                    todoObj.userid = '378';
                                    $.ajax({
                                        type: "POST",
                                        url: "../service/general.asmx/TodoData",
                                        contentType: "application/json; charset=utf-8",
                                        data: JSON.stringify(todoObj),
                                        dataType: "json",
                                        success: function (data) {
                                            // watchlistDataLoop(data.d);
                                            createTodoList(data.d);
                                            createCalender(data.d);
                                        }
                                    });


                                }
                            });
                        },
                        cancel: function () {
                            //$.alert('Canceled!');
                            $(s).filter(':checkbox').prop('checked', true);
                        }   
                    }
                });
            }

            //if ($(this).attr(':checked')) {
                //$.confirm({
                //    title: 'Do you want to Uncomplete this task!',
                //    content: 'Simple confirm!',
                //    buttons: {
                //        confirm: function () {
                //            var taskEdit = {};
                //            taskEdit.taskId = id;
                //            $.ajax({
                //                type: "POST",
                //                url: "../service/general.asmx/UpdateTask",
                //                contentType: "application/json; charset=utf-8",
                //                data: JSON.stringify(taskEdit),
                //                dataType: "json",
                //                success: function (data) {
                //                    function createCalender(data) {
                //                        var evt = [];

                //                        for (var i = 0; i < data.length; i++) {

                //                            var obj = {};
                //                            obj.title = data[i].TASK_SUBJECT;
                //                            obj.start = data[i].TASK_DUEDATE;
                //                            obj.description = data[i].TASK_DESCRIPTION;
                //                            evt.push(obj);



                //                        }
                //                        console.log(evt);

                //                        $('#EventCal').fullCalendar({
                //                            themeSystem: 'bootstrap3',
                //                            header: {
                //                                left: 'prev,next today',
                //                                center: 'title',
                //                                right: 'month,listMonth'
                //                            },
                //                            weekNumbers: true,
                //                            eventLimit: true,
                //                            events: evt,
                //                            eventRender: function (event, element) {
                //                                element.qtip({
                //                                    content: event.description,
                //                                    position: {
                //                                        my: 'top left',  // Position my top left...
                //                                        at: 'left bottom', // at the bottom right of...
                //                                    }
                //                                });
                //                            }
                //                        });
                //                    }

                //                    var todoObj = {};
                //                    todoObj.userid = '378';
                //                    $.ajax({
                //                        type: "POST",
                //                        url: "../service/general.asmx/TodoData",
                //                        contentType: "application/json; charset=utf-8",
                //                        data: JSON.stringify(todoObj),
                //                        dataType: "json",
                //                        success: function (data) {
                //                            // watchlistDataLoop(data.d);
                //                            createTodoList(data.d);
                //                            createCalender(data.d);
                //                        }
                //                    });


                //                }
                //            });
                //        },
                //        cancel: function () {
                //            //$.alert('Canceled!');
                //        }   
                //    }
                //});
            //} else {
                //$.confirm({
                //    title: 'Do you want to Complete this task!',
                //    content: 'Simple confirm!',
                //    buttons: {
                //        confirm: function () {
                //            var taskEdit = {};
                //            taskEdit.taskId = id;
                //            $.ajax({
                //                type: "POST",
                //                url: "../service/general.asmx/UpdateTask",
                //                contentType: "application/json; charset=utf-8",
                //                data: JSON.stringify(taskEdit),
                //                dataType: "json",
                //                success: function (data) {
                //                    function createCalender(data) {
                //                        var evt = [];

                //                        for (var i = 0; i < data.length; i++) {

                //                            var obj = {};
                //                            obj.title = data[i].TASK_SUBJECT;
                //                            obj.start = data[i].TASK_DUEDATE;
                //                            obj.description = data[i].TASK_DESCRIPTION;
                //                            evt.push(obj);



                //                        }
                //                        console.log(evt);

                //                        $('#EventCal').fullCalendar({
                //                            themeSystem: 'bootstrap3',
                //                            header: {
                //                                left: 'prev,next today',
                //                                center: 'title',
                //                                right: 'month,listMonth'
                //                            },
                //                            weekNumbers: true,
                //                            eventLimit: true,
                //                            events: evt,
                //                            eventRender: function (event, element) {
                //                                element.qtip({
                //                                    content: event.description,
                //                                    position: {
                //                                        my: 'top left',  // Position my top left...
                //                                        at: 'left bottom', // at the bottom right of...
                //                                    }
                //                                });
                //                            }
                //                        });
                //                    }

                //                    var todoObj = {};
                //                    todoObj.userid = '378';
                //                    $.ajax({
                //                        type: "POST",
                //                        url: "../service/general.asmx/TodoData",
                //                        contentType: "application/json; charset=utf-8",
                //                        data: JSON.stringify(todoObj),
                //                        dataType: "json",
                //                        success: function (data) {
                //                            // watchlistDataLoop(data.d);
                //                            createTodoList(data.d);
                //                            createCalender(data.d);
                //                        }
                //                    });


                //                }
                //            });
                //        },
                //        cancel: function () {
                //            //$.alert('Canceled!');
                //        }
                //    }
                //});

           // }
            
        }

        //function abc(id) {
        //    //alert(id);
        //    if ($(this).attr(':checked')) {
        //        $(this).filter(':checkbox').prop('checked', true);
        //    } else {
        //        $(this).filter(':checkbox').prop('checked', true);

        //    }
        //}
        

        function createTodoList(data) {
            //console.log(data.length);
            var mainDta = data;
            console.log('createTodoList', mainDta);
            var tureCount = $.grep(mainDta, function (e) { return e.ISCOMPLETED == true });
            var falseCount = $.grep(mainDta, function (e) { return e.ISCOMPLETED == false && e.WARNING == false });
            var overdueCount = $.grep(mainDta, function (e) { return e.WARNING == true });

            
            $('#completeCount').html(tureCount.length);
            $('#pendingCount').html(falseCount.length);
            $('#overDueCount').html(overdueCount.length);
            $('#allCount').html(mainDta.length);

            var toDoItem = ""

            for (var i = 0; i < mainDta.length; i++) {
                var tskStatus = "";
                if(mainDta[i]['task_status'] == "" || mainDta[i]['task_status'] == undefined) {
                    tskStatus = "NONE";
                }else {
                    tskStatus = mainDta[i]['task_status']
                }
                toDoItem += "<div class='todolistItem clearfix' id='users'><div class='forIconStatus'>"
                if (mainDta[i]['ISCOMPLETED'] == true) {
                    toDoItem += "<img src='../images/EV1.png' />"
                } else if (mainDta[i]['ISCOMPLETED'] == false && mainDta[i]['WARNING'] == true) {
                    toDoItem += "<img src='../images/EV2.png' />"
                } else if (mainDta[i]['ISCOMPLETED'] == false && mainDta[i]['WARNING'] == false) {
                    toDoItem += "<img src='../images/EV3.png' />"
                }

                toDoItem += "</div><div class='forContent'><div class='col-sm-12'><div class='row'>"

                toDoItem += "<h5 class='TodoSub col-sm-12'>" + mainDta[i]['TASK_SUBJECT'] + "</h5>"
                toDoItem += "<p class='col-sm-12'>" + mainDta[i]['TASK_DESCRIPTION'] + "</p>"

                toDoItem += " </div></div><div class='col-sm-12'><div><ul class='todoInfo'><li>"

                if (mainDta[i]['ISCOMPLETED'] == true) {
                    toDoItem += "<div class='hd'>Due on</div><div class='data'>" + mainDta[i]['TASK_DUEDATEFor'] + "</div>"
                } else if (mainDta[i]['ISCOMPLETED'] == false && mainDta[i]['WARNING'] == true) {
                    toDoItem += "<div class='hd'>Due on</div><div class='data red'>" + mainDta[i]['TASK_DUEDATEFor'] + "</div>"
                } else if (mainDta[i]['ISCOMPLETED'] == false && mainDta[i]['WARNING'] == false) {
                    toDoItem += "<div class='hd'>Due on</div><div class='data'>" + mainDta[i]['TASK_DUEDATEFor'] + "</div>"
                }

                

                toDoItem += "</li><li>"
                if (mainDta[i]['TASK_PRIORITY'] == "High") {
                    toDoItem += "<div class='hd'>Priority</div><div class='data cHigh'>" + mainDta[i]['TASK_PRIORITY'] + "</div></li><li>"
                } else if (mainDta[i]['TASK_PRIORITY'] == 'Low') {
                    toDoItem += "<div class='hd'>Priority</div><div class='data cLow'>" + mainDta[i]['TASK_PRIORITY'] + "</div></li><li>"
                } else if (mainDta[i]['TASK_PRIORITY'] == 'Normal') {
                    toDoItem += "<div class='hd'>Priority</div><div class='data cNormal'>" + mainDta[i]['TASK_PRIORITY'] + "</div></li><li>"
                }
                if (mainDta[i]['CompletedBy'] == "") {
                    toDoItem += "<div class='hd'>Completed by</div><div class='data'>&nbsp; </div></li><li>"
                } else {
                    toDoItem += "<div class='hd'>Completed by</div><div class='data'>" + mainDta[i]['CompletedBy'] + "</div></li><li>"
                }

                //toDoItem += "<div class='hd'>Completed by</div><div class='data'>" + mainDta[i]['CompletedBy'] + "</div></li><li>"
                if (mainDta[i]['completedon'] == null) {
                    toDoItem += "<div class='hd'>Completed on</div><div class='data'>&nbsp;</div></li>"
                } else {
                    toDoItem += "<div class='hd'>Completed on</div><div class='data'>" + mainDta[i]['completedonFor'] + "</div></li>"
                }
                toDoItem += "<li><div class='hd'>Status</div><div class='data stD'>" + mainDta[i]['task_status'] + "</div></li>"
                toDoItem += "</ul></div></div>"
                toDoItem += "<div class='col-sm-12'><button type='button' class='btn btn-info btn-xs' onclick='openTaskHistory(" + '"' + mainDta[i]['SCHEDULE_id'] + '"' +")'>View History</button>"
                toDoItem += "<button class='btn btn-primary btn-xs' type='button' onclick='openUpdateTask(" + '"' + mainDta[i]['SCHEDULE_id'] + '"' + ', ' + '"' + tskStatus + '"' + ', ' + '"' + mainDta[i]['TASK_SUBJECT'] + '"' + ', ' + '"' + mainDta[i]['HELP_ID'] + '"' + ', ' + '"' + mainDta[i]['HELP_REASON'] + '"' + ', ' + '"' + mainDta[i]['REMARKS'] + '"' + ")'>Update</button></div>"
                toDoItem += "</div>" // end of forContent


                if (mainDta[i]['ISCOMPLETED'] == true) {
                    toDoItem += "<div class='forAction hide'><label class='switch'><input type='checkbox' onclick='completeTask(" + mainDta[i]['SCHEDULE_id'] + ",this);' id='checkMeOut" + mainDta[i]['SCHEDULE_id'] + "' checked><span class='slider round'></span></label></div></div>"
                } else {
                    toDoItem += "<div class='forAction hide'><label class='switch'><input type='checkbox' onclick='completeTask(" + mainDta[i]['SCHEDULE_id'] + ",this);' id='checkMeOut" + mainDta[i]['SCHEDULE_id'] + "'><span class='slider round'></span></label></div></div>"
                }
                

            }
            $('#todolistWraper').html(toDoItem);

        }

        function openUpdateTask(s, t, d, hid, hreason, REMARKS) {
            var SCHEDULE_id = s,
                task_status = t,
                TASK_SUBJECT = d;
                
            $("#taksSelectedId").val(SCHEDULE_id);
            $("#TaskUpdate").modal("show");
            $("#tskStatusDrop").val(task_status);
            $("#tskTitle").val(TASK_SUBJECT);
            $("#helpSelect").val(hid);
            $("#helpReason").val(hreason);
            $("#updateRemarks").val(REMARKS);
            if ($("#tskStatusDrop").val() == "Asked for Help") {
                $("#forHelp").show();
            }
        }
        function updateTaskinPopup() {
            var SCHEDULE_id = $("#taksSelectedId").val();
            var tskStatusDrop = $("#tskStatusDrop").val();
            var updateRemarks = $("#updateRemarks").val();
            var tskTitle = $("#tskTitle").val();
            var HELP_ID = $("#helpSelect").val();
            var HELP_REASON = $("#helpReason").val();
            //var SCHEDULE_id = $("#taksSelectedId").val();
           
                var updateArr = {};
                updateArr.taskId = SCHEDULE_id;
                //updateArr.Status = "1";
                updateArr.tskStatusDrop = tskStatusDrop;
                updateArr.updateRemarks = updateRemarks;
                updateArr.TASK_TITLE = tskTitle;
                updateArr.HELP_ID = HELP_ID;
                updateArr.HELP_REASON = HELP_REASON;
                $.ajax({
                    type: "POST",
                    url: "../service/general.asmx/UpdateTask",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(updateArr),
                    dataType: "json",
                    success: function (data) {
                        $("#TaskUpdate").modal("hide");
                        showAll();
                    }
                });
                      
        }
        function openTaskHistory(taskId) {
            var updateArr = {};
            updateArr.taskId = taskId;
            $.ajax({
                type: "POST",
                url: "../service/general.asmx/GetTaskChangeHistory",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(updateArr),
                dataType: "json",
                success: function (res) {
                    $("#TaskHistoryModal").modal("show");
                    var data = res.d;
                    var tblContent = "<thead><tr><th>Subject</th><th>Created on</th><th>Task Status</th><th>Remarks</th></tr></thead><tbody>";
                    for (var i = 0; i < data.length; i++) {
                        tblContent += "<tr>"
                        tblContent += "<td>" + data[i].TASK_TITLE + "</td>"
                        tblContent += "<td>" + data[i].CREATED_ON + "</td>"
                        tblContent += "<td>" + data[i].TSTATUS + "</td>"
                        tblContent += "<td>" + data[i].REMARKS + "</td>"
                        tblContent += "</tr>"
                    }
                    tblContent += "</tbody>"
                    $("#TaskHistoryTable").html(tblContent)
                }
            });
        }
        $(document).ready(function () {
            getUserList();
            $("#forHelp, #forResign").hide();
            $("#tskStatusDrop").change(function () {
                if ($(this).val() == "Asked for Help") {
                    $("#forHelp").show();
                } else {
                    $("#forHelp").hide();
                    $("#helpSelect, #helpReason").val("");
                    
                }
            });

            $("#chkAssigntoO").change(function () {
                if (this.checked) {
                    $("#forResign").show();
                } else {
                    $("#forResign").hide();
                    $("#assignSelect").val("");
                }
            })
        });
        function getUserList() {
            $.ajax({
                type: "POST",
                url: "../service/general.asmx/GetUsersAll",
                contentType: "application/json; charset=utf-8",
                //data: JSON.stringify(todoObj),
                dataType: "json",
                success: function (res) {
                    console.log("allUser", res.d);
                    var data = res.d;
                    var userOptions="<option value=''>Select</option>"
                    for (var i = 0; i < data.length; i++) {
                        userOptions += "<option value='"+ data[i].user_id + "'> " + data[i].user_name +"</option>"
                    }
                    $("#helpSelect, #assignSelect").html(userOptions);
                }
            }); 
        }

</script>

  <%--  <link href="../Js/tempuststyle.css" rel="stylesheet" />
    <script src="../Js/tempust.js"></script>--%>

    <link href="../css/fullcalendar.min.css" rel="stylesheet" />
    <link href="../css/jquery.qtip.min.css" rel="stylesheet" />
    <script src="../Js/moment.min.js"></script>
    
    
    
    <script src="../Js/fullcalendar.min.js"></script>
    <script src="../Js/jquery.qtip.min.js"></script>
    <link href="/assests/css/custom/PMSStyles.css" rel="stylesheet" />   
    <script type="text/javascript">

        function showAll() {
            $('.countLbl').removeClass('active');
            $('#showAllLbl').addClass('active');
            console.log(ele.target);
            $(ele).addClass('active');
            var todoObj = {};
            todoObj.userid = '378';
            $.ajax({
                type: "POST",
                url: "../service/general.asmx/TodoData",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(todoObj),
                dataType: "json",
                success: function (data) {
                    var mainDta = data;
                    // watchlistDataLoop(data.d);
                    createTodoList(data.d);
                }
            });
        }
        function showComplete() {
            $('.countLbl').removeClass('active');
            $('#showCompleteLbl').addClass('active');
            var todoObj = {};
            todoObj.userid = '378';
            $.ajax({
                type: "POST",
                url: "../service/general.asmx/TodoData",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(todoObj),
                dataType: "json",
                success: function (data) {
                    // watchlistDataLoop(data.d);
                    var mainDta = data.d;
                    var tureCount = $.grep(mainDta, function (e) { return e.ISCOMPLETED == true });
                    console.log("tureCount", tureCount)
                    generateFilter(tureCount);
                    
                    //filterTodoList(data.d);
                }
            });
        }
        function showPending() {
            $('.countLbl').removeClass('active');
            $('#showPendingLbl').addClass('active');
            var todoObj = {};
            todoObj.userid = '378';
            $.ajax({
                type: "POST",
                url: "../service/general.asmx/TodoData",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(todoObj),
                dataType: "json",
                success: function (data) {
                    
                    // watchlistDataLoop(data.d);
                    var PendingData = data.d;
                    console.log(PendingData)
                    var falseCount = $.grep(PendingData, function (e) { return e.ISCOMPLETED == false && e.WARNING == false });
                    console.log("falseCount", falseCount)
                    generateFilter(falseCount);
                    
                    //filterTodoList(data.d);
                }
            });
        }
        function showOverDue() {
            $('.countLbl').removeClass('active');
            $('#showOverDueLbl').addClass('active');
            var todoObj = {};
            todoObj.userid = '378';
            $.ajax({
                type: "POST",
                url: "../service/general.asmx/TodoData",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(todoObj),
                dataType: "json",
                success: function (data) {
                    // watchlistDataLoop(data.d);
                    var OverDueData = data.d;
                    
                    var overdueCount = $.grep(OverDueData, function (e) { return e.WARNING == true });
                    console.log("overdueCount", overdueCount)
                    generateFilter(overdueCount);
                    
                    //filterTodoList(data.d);
                }
            });
        }
        function showAll() {
            $('.countLbl').removeClass('active');
            $('#showAllLbl').addClass('active');
            var todoObj = {};
            todoObj.userid = '378';
            $.ajax({
                type: "POST",
                url: "../service/general.asmx/TodoData",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(todoObj),
                dataType: "json",
                success: function (data) {
                    // watchlistDataLoop(data.d);
                    createTodoList(data.d);
                }
            });
        }

        function generateFilter(DATA) {
            //console.log(DATA);
            var toDoFilterItem = "";

            for (var i = 0; i < DATA.length; i++) {
                var tskStatus = "";
                if (DATA[i]['task_status'] == "" || DATA[i]['task_status'] == undefined) {
                    tskStatus = "NONE";
                } else {
                    tskStatus = DATA[i]['task_status']
                }

                toDoFilterItem += "<div class='todolistItem clearfix' id='users'><div class='forIconStatus'>"
                if (DATA[i]['ISCOMPLETED'] == true) {
                    toDoFilterItem += "<img src='../images/EV1.png' />"
                } else if (DATA[i]['ISCOMPLETED'] == false && DATA[i]['WARNING'] == true) {
                    toDoFilterItem += "<img src='../images/EV2.png' />"
                } else if (DATA[i]['ISCOMPLETED'] == false && DATA[i]['WARNING'] == false) {
                    toDoFilterItem += "<img src='../images/EV3.png' />"
                }

                toDoFilterItem += "</div><div class='forContent'><div class='col-sm-12'><div class='row'>"

                toDoFilterItem += "<h5 class='TodoSub col-sm-12'>" + DATA[i]['TASK_SUBJECT'] + "</h5>"
                toDoFilterItem += "<p class='col-sm-12'>" + DATA[i]['TASK_DESCRIPTION'] + "</p>"

                toDoFilterItem += " </div></div><div class='col-sm-12'><div><ul class='todoInfo'><li>"

                if (DATA[i]['ISCOMPLETED'] == true) {
                    toDoFilterItem += "<div class='hd'>Due on</div><div class='data'>" + DATA[i]['TASK_DUEDATEFor'] + "</div>"
                } else if (DATA[i]['ISCOMPLETED'] == false && DATA[i]['WARNING'] == true) {
                    toDoFilterItem += "<div class='hd'>Due on</div><div class='data red'>" + DATA[i]['TASK_DUEDATEFor'] + "</div>"
                } else if (DATA[i]['ISCOMPLETED'] == false && DATA[i]['WARNING'] == false) {
                    toDoFilterItem += "<div class='hd'>Due on</div><div class='data'>" + DATA[i]['TASK_DUEDATEFor'] + "</div>"
                }



                toDoFilterItem += "</li><li>"

                if (DATA[i]['TASK_PRIORITY'] == "High") {
                    toDoFilterItem += "<div class='hd'>Priority</div><div class='data cHigh'>" + DATA[i]['TASK_PRIORITY'] + "</div></li><li>"
                } else if (DATA[i]['TASK_PRIORITY'] == 'Low') {
                    toDoFilterItem += "<div class='hd'>Priority</div><div class='data cLow'>" + DATA[i]['TASK_PRIORITY'] + "</div></li><li>"
                } else if (DATA[i]['TASK_PRIORITY'] == 'Normal') {
                    toDoFilterItem += "<div class='hd'>Priority</div><div class='data cNormal'>" + DATA[i]['TASK_PRIORITY'] + "</div></li><li>"
                }
                if (DATA[i]['CompletedBy'] == "") {
                    toDoFilterItem += "<div class='hd'>Completed by</div><div class='data'>&nbsp; </div></li><li>"
                } else {
                    toDoFilterItem += "<div class='hd'>Completed by</div><div class='data'>" + DATA[i]['CompletedBy'] + "</div></li><li>"
                }

                //toDoItem += "<div class='hd'>Completed by</div><div class='data'>" + mainDta[i]['CompletedBy'] + "</div></li><li>"
                if (DATA[i]['completedon'] == null) {
                    toDoFilterItem += "<div class='hd'>Completed on</div><div class='data'>&nbsp;</div></li>"
                } else {
                    toDoFilterItem += "<div class='hd'>Completed on</div><div class='data'>" + DATA[i]['completedonFor'] + "</div></li>"
                }
                toDoFilterItem += "<li><div class='hd'>Status</div><div class='data stD'>" + DATA[i]['task_status'] + "</div></li>"
                toDoFilterItem += "</ul></div></div>"
                toDoFilterItem += "<div class='col-sm-12'><button type='button' class='btn btn-info btn-xs'>View History</button>"
                toDoFilterItem += "<button class='btn btn-primary btn-xs' type='button' onclick='openUpdateTask(" + '"' + DATA[i]['SCHEDULE_id'] + '"' + ', ' + '"' + tskStatus + '"' + ', ' + '"' + DATA[i]['TASK_SUBJECT'] + '"' + ', ' + '"' + DATA[i]['HELP_ID'] + '"' + ', ' + '"' + DATA[i]['HELP_REASON'] + '"' + ', ' + '"' + DATA[i]['REMARKS'] + '"' + ")'>Update</button></div>"
                toDoFilterItem += "</div>" // end of forContent


                if (DATA[i]['ISCOMPLETED'] == true) {
                    toDoFilterItem += "<div class='forAction hide'><label class='switch'><input type='checkbox' onclick='completeTask(" + DATA[i]['SCHEDULE_id'] + ",this);' id='checkMeOut" + DATA[i]['SCHEDULE_id'] + "' checked><span class='slider round'></span></label></div></div>"
                } else {
                    toDoFilterItem += "<div class='forAction hide'><label class='switch'><input type='checkbox' onclick='completeTask(" + DATA[i]['SCHEDULE_id'] + ",this);' id='checkMeOut" + DATA[i]['SCHEDULE_id'] + "'><span class='slider round'></span></label></div></div>"
                }


            }
            $('#todolistWraper').html(toDoFilterItem);
        }
        $(function () {
            $('[data-toggle="tooltip"]').tooltip()
        })
    </script>
    <style>
        .countLbl.active {
            background: #aeb8ff;
            color: #4c4b4b;
        }
        .mainLabel .tooltip {
            font-weight:500 !important
        }
        #TaskHistoryTable {
            width:100%;
            border:1px solid #ccc;
        }
        #TaskHistoryTable>thead>tr>th {
            background:#5150b7;
            padding:8px 5px;
            color:#fff
        }
        #TaskHistoryTable>tbody>tr>td {
            padding:2px 5px;
        }
        #TaskHistoryTable>tbody>tr:nth-child(odd)>td {
            background:#f5f5f5
        }
    </style>

</head>
<body>
    <!-- Modal Task Update-->
<div id="TaskUpdate" class="modal fade pmsModal w30" role="dialog">
  <div class="modal-dialog">

    <!-- Modal content-->
    <div class="modal-content">
      <div class="modal-header">
        <button type="button" class="close" data-dismiss="modal">&times;</button>
        <h4 class="modal-title">Update Task</h4>
      </div>
      <div class="modal-body">
        <div>
            <label>Task Subject</label>
          <input type="text" id="tskTitle" disabled="disabled" />
        </div>
        <div>
            <label>Status</label>
            <div>
                <select id="tskStatusDrop" class="form-control">
                    <option value="NONE">Select</option>
                    <option value="Work In progress">Work In progress</option>
                    <option value="Waiting for Linked Task to Complete">Waiting for Linked Task to Complete</option>
                    <option value="Asked for Help">Asked for Help</option>
                    <option value="Completed">Completed</option>
                </select>
            </div>
        </div>
          <div id="forHelp">
            <label>Seek help from</label>
            <div>
                <select id="helpSelect" class="form-control">
                    <option>Select</option>
                </select>
            </div>
             <div>
                <label>Reason</label>
                  <div>
                      <textarea id="helpReason" rows="3" style="height:50px"></textarea>
                  </div>
             </div>
          </div>
        <div class="checkbox hide">
          <label><input type="checkbox" id="chkAssigntoO" value="" />Assign to Other</label>
        </div>
          <div id="forResign">
            <label>Assign to</label>
            <div>
                <select id="assignSelect" class="form-control">
                    <option>Select</option>
                </select>
            </div>
             <div>
                <label>Reason</label>
                  <div>
                      <textarea id="assignReason" rows="3" style="height:50px"></textarea>
                  </div>
             </div>
          </div>
          <div>
              <label>Remarks</label>
              <div>
                 <textarea id="updateRemarks" rows="3" style="height:50px"></textarea>
              </div>
          </div>
      </div>
      <div class="modal-footer">
          <input type="hidden" id="taksSelectedId" value="" />
          <button type="button" class="btn btn-success" onclick="updateTaskinPopup()">Save</button>
        <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
      </div>
    </div>

  </div>
</div>

<div id="TaskHistoryModal" class="modal fade pmsModal w50" role="dialog">
  <div class="modal-dialog">

    <!-- Modal content-->
    <div class="modal-content">
      <div class="modal-header">
        <button type="button" class="close" data-dismiss="modal">&times;</button>
        <h4 class="modal-title">View Task History</h4>
      </div>
      <div class="modal-body">
         <div>
             <table id="TaskHistoryTable"></table>
         </div>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
      </div>
    </div>

  </div>
</div>

    <form id="form1" runat="server">
       <div class="panel-heading clearfix ">
           <div class="col-md-12">
                <div class="panel-title pull-left ">
                    <h3 >Task List </h3>
                </div>
               <div class="pull-right" >
                    <table>
                        <tr>
                            <td><a href="#" onclick="reloadParent()" class="reloadParent"><i class="fa fa-times"></i></a></td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
        <div class="containerPad clearfix form_main">
            <div class="row">
                <div class="col-md-6"  id="TodoPermission">
                    <div class="boxWidget overflHidden noContent">
                        <h1 class="mainLabel clearfix">Task    
                            <span class="countLbl  pull-right" onclick="showComplete()" id="showCompleteLbl" data-toggle="tooltip" data-placement="top" title="Completed"> <img src="../images/emoS.png" style="max-width: 22px;margin-right: 5px;" /> <span id="completeCount" class="success"></span></span>
                            <span class="countLbl  pull-right" onclick="showPending()" id="showPendingLbl" data-toggle="tooltip" data-placement="top" title="Pending"> <img src="../images/emoW.png" style="max-width: 22px;margin-right: 5px;" /> <span id="pendingCount" class="pending"></span></span>
                            <span class="countLbl  pull-right" onclick="showOverDue()" id="showOverDueLbl" data-toggle="tooltip" data-placement="top" title="OverDue">
                                <img src="../images/emoD.png" style="max-width: 22px;margin-right: 5px;" />  
                                <span id="overDueCount" class="overDue"></span>
                            </span>
                            <span class="countLbl  pull-right active" onclick="showAll()" id="showAllLbl"><span style="display: inline-block;height: 21px;line-height: 17px;">All</span>  <span id="allCount" class="all"></span></span>
                        </h1>
                        <div class="todolistWraper" id="todolistWraper">
                       
                        </div>
                    </div>
                </div>
                <div class="col-md-6"  id="">
                    <div class="boxWidget overflHidden noContent">
                        <h1 class="mainLabel ">Task Calender</h1>
                        <div class="" id="EventCal">
                       
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                
            </div>


        </div>


    </form>
</body>
</html>
