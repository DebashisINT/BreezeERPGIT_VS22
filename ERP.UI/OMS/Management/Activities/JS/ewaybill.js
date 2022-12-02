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

        //cGrdQuotation.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue())
        $("#hfFromDate").val(cFormDate.GetDate().format('yyyy-MM-dd'));
        $("#hfToDate").val(ctoDate.GetDate().format('yyyy-MM-dd'));
        $("#hfBranchID").val(ccmbBranchfilter.GetValue());
        $("#hfIsFilter").val("Y");
        cGrdQuotation.Refresh();
        cGrdQuotation.Refresh();
    }
}


function grdEndcallback(s, e) {

}



function grdEndcallbackPendingSI(s, e) {

}

function grdEndcallbackTSI(s, e) {

}



function grdEndcallbackPendingTSI(s, e) {

}


function grdEndcallbackCR(s, e) {

}



function grdEndcallbackPendingCR(s, e) {

}

function gridcrmCampaignclick(s, e) {
    //alert('hi');
    //IconChange();
    $('#gridcrmCampaign').find('tr').removeClass('rowActive');
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


function updateGridByDatePendinSI() {
    if (cFormDatePendinSI.GetDate() == null) {
        jAlert('Please select from date.', 'Alert', function () { cFormDate.Focus(); });
    }
    else if (ctoDatePendinSI.GetDate() == null) {
        jAlert('Please select to date.', 'Alert', function () { ctoDate.Focus(); });
    }
    else if (ccmbBranchfilterPendinSI.GetValue() == null) {
        jAlert('Please select Branch.', 'Alert', function () { ccmbBranchfilter.Focus(); });
    }
    else {

        //cGrdQuotation.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue())
        $("#hfFromDate").val(cFormDatePendinSI.GetDate().format('yyyy-MM-dd'));
        $("#hfToDate").val(ctoDatePendinSI.GetDate().format('yyyy-MM-dd'));
        $("#hfBranchID").val(ccmbBranchfilterPendinSI.GetValue());
        $("#hfIsFilter").val("Y");
        cGrdQuotationPendinSI.Refresh();
        cGrdQuotationPendinSI.Refresh();
    }
}


function DoownLoadEwaybill(ewaybill)
{
    cGrdQuotation.PerformCallback('Download~' + ewaybill);
}

function UploadEwayBill(id) {
    cGrdQuotationPendinSI.PerformCallback('GenerateEwaybill~' + id);
}

