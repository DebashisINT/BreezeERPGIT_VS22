 
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
                                  } ,
                                  Cancel: {
                                      text: "No",
                                      id: "popup_no",
                                      click: function () {
                                          CallBack(false); 
                                          $(this).dialog("close");
                                      }
                                  },
                                 
                              },
                              close: function (event, ui) {
                                  $(this).remove();
                              }
                          });
    }

    function jAlert(message, textTitle, CallBack) {
        $('<div></div>').appendTo('body')
                          .html('<div><h6>' + message + '</h6></div>')
                          .dialog({
                              modal: true, title: textTitle, zIndex: 10000, autoOpen: true,
                              width: 'auto', resizable: false,
                              buttons: {
                                  OK: function () {
                                           
                                      CallBack(); 
                                      $(this).dialog("close");


                                  },
                                      
                              },
                              close: function (event, ui) {
                                  $(this).remove();
                              }
                          });
    }
function jAlert(message) {
    $('<div></div>').appendTo('body')
                      .html('<div><h6>' + message + '</h6></div>')
                      .dialog({
                          modal: true, title: 'Alert', zIndex: 10000, autoOpen: true,
                          width: 'auto', resizable: false,
                          buttons: {
                              OK: function () {

                                          
                                  $(this).dialog("close");


                              },

                          },
                          close: function (event, ui) {
                              $(this).remove();
                          }
                      });
}

 