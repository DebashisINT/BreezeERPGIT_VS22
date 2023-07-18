/**********************************************************************************************************************************
 1.0      Sanchita  V2.0.38   30-05-2023      ERP - Listing Views - Vendor Debit/Credit Note. refer: 26589  
***********************************************************************************************************************************/
        function StopDefaultAction(e) {
            if (e.preventDefault) { e.preventDefault() }
            else { e.stop() };
            e.returnValue = false;
            e.stopPropagation();
        }
document.onkeydown = function (e) {
    if ((event.keyCode == 120 || event.keyCode == 65) && event.altKey == true) { //run code for alt+A -- ie, Add New  
        StopDefaultAction(e);
        AddButtonClick();
    }
}
function AddButtonClick() {
    var url = 'VendorDrCrNoteAdd.aspx?key=' + 'ADD';
    window.location.href = url;
}
function OnViewClick(keyValue) {
    var url = 'VendorDrCrNoteAdd.aspx?key=' + keyValue + '&req=V' + '&type=VN';
    window.location.href = url;
}
function OnMoreInfoClick(keyValue) {           
    var url = 'VendorDrCrNoteAdd.aspx?key=' + keyValue + '&type=VN';
    window.location.href = url;           
}
function OnClickDelete(keyValue) {
           
    jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
        if (r == true) {
            cGvJvSearch.PerformCallback("PCB_DeleteBtnOkE~" + keyValue);
        }
    });
            
}

function GvJvSearch_EndCallBack() {
    if (cGvJvSearch.cpJVDelete != undefined && cGvJvSearch.cpJVDelete != null) {
        jAlert(cGvJvSearch.cpJVDelete);
        cGvJvSearch.cpJVDelete = null;
        updateGridByDate();
        //cGvJvSearch.PerformCallback('PCB_BindAfterDelete');
    }
}
function onPrintJv(id, RowIndex) {
    DCNoteID = id;
    cDocumentsPopup.Show();
    $('#HdCrDrNoteType').val(cGvJvSearch.GetRow(RowIndex).children[0].innerText);
    cCmbDesignName.SetSelectedIndex(0);
    cSelectPanel.PerformCallback('Bindalldesignes');
    $('#btnOK').focus();
}
function PerformCallToGridBind() {
    cSelectPanel.PerformCallback('Bindsingledesign');
    cDocumentsPopup.Hide();
    return false;
}
function cSelectPanelEndCall(s, e) {           
    if (cSelectPanel.cpSuccess != null) {
        var reportName = cCmbDesignName.GetValue();
        var module = 'VENDDRCRNOTE';
        window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + DCNoteID, '_blank')
    }
    cSelectPanel.cpSuccess = null
    if (cSelectPanel.cpSuccess == null) {
        cCmbDesignName.SetSelectedIndex(0);
    }
}
function updateGridByDate() {

    if (cFormDate.GetDate() == null) {
        jAlert('Please select from date.', 'Alert', function () { cFormDate.Focus(); });
    }
    else if (ctoDate.GetDate() == null) {
        jAlert('Please select to date.', 'Alert', function () { ctoDate.Focus(); });
    }
    else if (ccmbBranchfilter.GetValue() == null) {
        jAlert('Please select Branch.', 'Alert', function () { ccmbBranchfilter.Focus(); });
    }
    else {
        localStorage.setItem("FromDateVendorCrDrNote", cFormDate.GetDate().format('yyyy-MM-dd'));
        localStorage.setItem("ToDateVendorCrDrNote", ctoDate.GetDate().format('yyyy-MM-dd'));
        localStorage.setItem("BranchVendorCrDrNote", ccmbBranchfilter.GetValue());

        $("#hfFromDate").val(cFormDate.GetDate().format('yyyy-MM-dd'));
        $("#hfToDate").val(ctoDate.GetDate().format('yyyy-MM-dd'));
        $("#hfBranchID").val(ccmbBranchfilter.GetValue());
        $("#hfIsFilter").val("Y");
        // Rev 1.0
        //cGvJvSearch.Refresh();
        $("#hFilterType").val("All");
        cCallbackPanel.PerformCallback("");
        // End of Rev 1.0
    }
}
function gridRowclick(s, e) {
    $('#GvJvSearch').find('tr').removeClass('rowActive');
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
                $(value).css({ 'opacity': '1' });
            }, 100);
        });
    }, 200);
}
$(document).ready(function () {
    //Toggle fullscreen expandEntryGrid
    $("#expandcGvJvSearch").click(function (e) {
        e.preventDefault();
        var $this = $(this);
        if ($this.children('i').hasClass('fa-expand')) {
            $this.removeClass('hovered half').addClass('full');
            $this.attr('title', 'Minimize Grid');
            $this.children('i').removeClass('fa-expand');
            $this.children('i').addClass('fa-arrows-alt');
            var gridId = $(this).attr('data-instance');
            $(this).closest('.makeFullscreen').addClass('panel-fullscreen');
            var cntWidth = $(this).parent('.makeFullscreen').width();
            var browserHeight = document.documentElement.clientHeight;
            var browserWidth = document.documentElement.clientWidth;
            cGvJvSearch.SetHeight(browserHeight - 150);
            cGvJvSearch.SetWidth(cntWidth);
        }
        else if ($this.children('i').hasClass('fa-arrows-alt')) {
            $this.children('i').removeClass('fa-arrows-alt');
            $this.removeClass('full').addClass('hovered half');
            $this.attr('title', 'Maximize Grid');
            $this.children('i').addClass('fa-expand');
            var gridId = $(this).attr('data-instance');
            $(this).closest('.makeFullscreen').removeClass('panel-fullscreen');
            var browserHeight = document.documentElement.clientHeight;
            var browserWidth = document.documentElement.clientWidth;
            cGvJvSearch.SetHeight(300);
            var cntWidth = $this.parent('.makeFullscreen').width();
            cGvJvSearch.SetWidth(cntWidth);
        }
    });
});


    $(document).ready(function () {
        if ($('body').hasClass('mini-navbar')) {
            var windowWidth = $(window).width();
            var cntWidth = windowWidth - 90;
            cGvJvSearch.SetWidth(cntWidth);
        } else {
            var windowWidth = $(window).width();
            var cntWidth = windowWidth - 220;
            cGvJvSearch.SetWidth(cntWidth);
        }
        $('.navbar-minimalize').click(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                cGvJvSearch.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                cGvJvSearch.SetWidth(cntWidth);
            }

        });
    });
