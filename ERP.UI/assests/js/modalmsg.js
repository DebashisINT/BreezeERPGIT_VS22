 

function jConfirm(message, textTitle, CallBack) {
    
        $('<div></div>').appendTo('body')
                          .html('<div><h6>' + message + '</h6></div>')
                          .dialog({
                              modal: true, title: textTitle, zIndex: 10000, autoOpen: true,
                              width: 'auto', resizable: false,
                              buttons: {
                                  "Yes":{
                                      text: "Yes",
                                      id: "popup_ok",
                                      click: function(){
                                          CallBack(true);
                                          $(this).dialog("close");
                                      }   
                                  },
                                  Cancel: {
                                      text: "No",
                                      id: "popup_no",
                                      click: function () {
                                          CallBack(false); 
                                          $(this).dialog("close");
                                      }
                                  },
                                 
                              },
                              create: function () {
                                  $('.ui-dialog-titlebar-close').addClass('hide');
                                  //$('.ui-dialog-buttonset').children().addClass('btn btn-primary');
                                  //$('.ui-dialog-buttonset').children('#popup_no').addClass('btn btn-danger');
                                  $('#popup_ok').addClass('btn btn-primary');
                                  $('#popup_no').addClass('btn btn-danger');
                              },
                              close: function (event, ui) {
                                  $(this).remove();
                              }
                          });
    }

function jAlert(message, textTitle, CallBack) {
    
    var title = 'Alert';
    if (textTitle)
        title = textTitle;

    $('<div></div>').appendTo('body')
                      .html('<div><h6>' + message + '</h6></div>')
                      .dialog({
                          
                          modal: true, title: title, zIndex: 10000, autoOpen: true,
                              width: 'auto', resizable: false,
                              buttons: {
                                  OK: function () { 
                                      if (CallBack)
                                        CallBack(true);
                                      $(this).dialog("close");


                                  },
                                      
                              },
                              create:function () {
                                  $('.ui-dialog-buttonset').children().addClass('btn btn-primary');
                                  $('.ui-dialog-titlebar-close').addClass('hide')
                              },
                              close: function (event, ui) {
                                  $(this).remove();
                              }
                          });
}


function showalertTable(data, title, CallBack) {
    var myObj = data;
    if (myObj == "" || myObj == undefined || myObj == null) {

    } else {
        
        var html = "";
        html += "<table class='mainAlertTable'><thead><tr><th>Doc Date</th><th>Doc No</th><th>Type</th><th>Error Code</th><th>Error Msg</th></thead>";
        html += "<tbody>";
        for (var i = 0; i < myObj.length; i++) {
            
            var obj = myObj[i];
            html += "<tr>";
            html += "<td>" + obj.a + "</td>";
            html += "<td>" + obj.b + "</td>";
            html += "<td>" + obj.c + "</td>";
            html += "<td>" + obj.d + "</td>";
            html += "<td>" + obj.e + "</td>";
            html += "</tr>";
        }
        html += "</tbody><table>";

        $('<div class="myAlertTbl"></div>').appendTo('body')
            .html(html)
            .dialog({
                modal: true, title: title, zIndex: 10000, autoOpen: true,
                width: '900px !important', resizable: false,
                buttons: {
                    OK: function () {
                        if (CallBack)
                            CallBack(true);
                        $(this).dialog("close");
                    },
                },
                create: function () {
                    $('.ui-dialog-buttonset').children().addClass('btn btn-primary');
                    $('.ui-dialog-titlebar-close').addClass('hide')
                },
                close: function (event, ui) {
                    $(this).remove();
                }
            });
    }
}
//function jAlert(message) {
//    $('<div></div>').appendTo('body')
//                      .html('<div><h6>' + message + '</h6></div>')
//                      .dialog({
//                          modal: true, title: 'Alert', zIndex: 10000, autoOpen: true,
//                          width: 'auto', resizable: false,
//                          buttons: {
//                              OK: function () {

//                                  console.log('jalert 1 para');
//                                  $(this).dialog("close");


//                              },

//                          },
//                          close: function (event, ui) {
//                              $(this).remove();
//                          }
//                      });
//}

 