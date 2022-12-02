
        function ClickOnMoreInfo(keyValue) {
            var url = 'frm_TdsTcsPopUp.aspx?id=' + keyValue;
            //  OnMoreInfoClick(url, "Modify TDS/TCS Details", '940px', '500px', 'Y');
            window.location.href = url;
        }
function OnAddButtonClick() {
    var url = 'frm_TdsTcsPopUp.aspx?id=' + 'ADD';
    // OnMoreInfoClick(url, "Modify TDS/TCS Details", '940px', '500px', 'Y');
    window.location.href = url;
}
function callback() {
    gridTdsTcs.PerformCallback();
}

function fn_DeleteTDSTCS(keyValue) {
             
    jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
        if (r == true) {
            gridTdsTcs.PerformCallback('Delete~' + keyValue);
        }
    });
}
function grid_EndCallBack() {
            
    if (gridTdsTcs.cpDelete != null) {
        if (gridTdsTcs.cpDelete == 'Success')
            jAlert('Deleted Successfully');
        else if (gridTdsTcs.cpDelete == 'Deletion Failed')
            jAlert('Used in other modules. Cannot Delete.');
        else
            jAlert("Error on deletion\n'Please Try again!!'")
    }
}
                 
                 
                 
           
                
//function EndCall(obj) {
//    alert('hi');
//    if (gridTdsTcs.cpDelete != null)
//        jAlert(gridTdsTcs.cpDelete);
//}
//}
function gridRowclick(s, e) {
    $('#gridTdsTcs').find('tr').removeClass('rowActive');
    $('.floatedBtnArea').removeClass('insideGrid');
    //$('.floatedBtnArea a .ico').css({ 'opacity': '0' });
    $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea').addClass('insideGrid');
    $(s.GetRow(e.visibleIndex)).addClass('rowActive');
    setTimeout(function () {
        //alert('delay');
        var lists = $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a');
        //$(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a .ico').css({'opacity': '1'});
        //$(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a').each(function (e) {
        //    setTimeout(function () {
        //        $(this).fadeIn();
        //    }, 100);
        //});    
        $.each(lists, function (index, value) {
            //console.log(index);
            //console.log(value);
            setTimeout(function () {
                console.log(value);
                $(value).css({ 'opacity': '1' });
            }, 100);
        });
    }, 200);
}

$(document).ready(function () {
    $('.navbar-minimalize').click(function () {
        gridTdsTcs.Refresh();
    });
    gridTdsTcs.Refresh();
});
