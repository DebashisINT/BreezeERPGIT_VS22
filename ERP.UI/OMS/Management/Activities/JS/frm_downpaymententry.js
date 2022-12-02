
        document.onkeydown = function (e) {
            if (event.keyCode == 65 && event.altKey == true) { //run code for alt+a -- ie, Add
                StopDefaultAction(e);
                AddnewFinance();
            }
        }

function StopDefaultAction(e) {
    if (e.preventDefault) { e.preventDefault() }
    else { e.stop() };

    e.returnValue = false;
    e.stopPropagation();
}



function Clear() {
    cdtEntryDate.SetEnabled(true);
    cdtDpDate.SetEnabled(true);
    ccmbFinancer.SetEnabled(true);
    ccmbBranch.SetEnabled(true);
    ctxtBillAmount.SetEnabled(true);
    cdtBilldate.SetEnabled(true);
    cCustomerComboBox.SetEnabled(true);
    ctxtFinanceAmount.SetEnabled(true);
    ctxtDownPay1.SetEnabled(true);
    ctxtDownPay2.SetEnabled(true);
    ctxtDivestmentAmt1.SetEnabled(true);
    ctxtDivestmentAmt2.SetEnabled(true);
    ctxtDivestmentAmt3.SetEnabled(true);
    ctxtfinalPayment.SetEnabled(true);
    ctxtDbdPercentage.SetEnabled(true);
    ctxtDbdAmount.SetEnabled(true);
    ctxtMbdPercentage.SetEnabled(true);
    ctxtMbdAmount.SetEnabled(true);
    ctxtProcessingFee.SetEnabled(true);
    ctxtTotalPay.SetEnabled(true);
    ctxtbalance.SetEnabled(true);
    ccmbStatus.SetEnabled(true);

    document.getElementById('txtproduct').disabled = false;
    document.getElementById('txtdownPayNo').disabled = false;
    document.getElementById('txtBillNo').disabled = false;
    document.getElementById('txtChallanNo').disabled = false;
    document.getElementById('txtSfCode').disabled = false;
    document.getElementById('txtAdjmntNo').disabled = false;
    document.getElementById('txtAdjmntDt').disabled = false;
    document.getElementById('txtModeofPayment').disabled = false;
    document.getElementById('txtfinalMr').disabled = false;
    document.getElementById('txtNaration').disabled = false;
    document.getElementById('txtDivestmentNo1').disabled = false;
    document.getElementById('txtDivestmentDT1').disabled = false;
    document.getElementById('txtDivestmentNo2').disabled = false;
    document.getElementById('txtDivestmentDT2').disabled = false;
    document.getElementById('txtDivestmentNo3').disabled = false;
    document.getElementById('txtDivestmentDT3').disabled = false;

    $('#txtdownPayNo').val("");
    $('#txtBillNo').val("");
    $('#txtChallanNo').val("");
    $('#txtSfCode').val("");
    $('#txtModeofPayment').val("");
    $('#txtproduct').val("");
    $('#txtAdjmntNo').val("");
    $('#txtAdjmntDt').val("");
    $('#txtfinalMr').val("");
    $('#txtNaration').val("");
    $('#txtDivestmentNo1').val("");
    $('#txtDivestmentDT1').val("");
    $('#txtDivestmentNo2').val("");
    $('#txtDivestmentDT2').val("");
    $('#txtDivestmentNo3').val("");
    $('#txtDivestmentDT3').val("");

    ctxtDivestmentAmt1.SetValue("0");
    ctxtDivestmentAmt2.SetValue("0");
    ctxtDivestmentAmt3.SetValue("0");
    ctxtFinanceAmount.SetValue("0");
    ctxtDownPay1.SetValue("0");
    ctxtDownPay2.SetValue("0");
    ctxtfinalPayment.SetValue("0");
    ctxtDbdPercentage.SetValue("0");
    ctxtDbdAmount.SetValue("0");
    ctxtMbdPercentage.SetValue("0");
    ctxtMbdAmount.SetValue("0");
    ctxtProcessingFee.SetValue("0");
    ctxtTotalPay.SetValue("0");
    ctxtbalance.SetValue("0");
    ccmbStatus.SetValue("");
    ctxtBillAmount.SetValue("0");

    var strDate = new Date();
    cdtEntryDate.SetDate(strDate);
    cdtDpDate.SetDate(strDate);
    cdtBilldate.SetDate(strDate);

    ccmbFinancer.SetValue("");
    cCustomerComboBox.SetValue("");
    $("#drdExport").val(0);
}

function GridEndCallback() {
    if (cdownpaygrid.cpMsg != null) {
        if (cdownpaygrid.cpMsg != '') {
            var _Msg = cdownpaygrid.cpMsg;
            cdownpaygrid.cpMsg = null;

            jAlert(_Msg, 'Alert Dialog: [Downpayment Entry]', function (r) {
                if (r == true) {
                    //window.location.reload();
                    $('#hdfID').val("");
                    $('#hdfInvoiceID').val("");

                    Clear();
                    cfinancePopup.Hide();
                    updateGridAfterDelete();
                }
            });
        }
    }
    $("#drdExport").val(0);
}

function CallbackPanelEndCall(s, e) {
    cfinancePopup.Show();
    ccmbBranch.Focus();
    LoadingPanel.Hide();
    $("#drdExport").val(0);
}

function BranchChange() {
    ccmbFinancer.PerformCallback();
    $("#drdExport").val(0);
}

function gridRowclick(s, e) {
    //alert('hi');
    $('#downpaygrid').find('tr').removeClass('rowActive');
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
        if ($('body').hasClass('mini-navbar')) {
            var windowWidth = $(window).width();
            var cntWidth = windowWidth - 90;
            cdownpaygrid.SetWidth(cntWidth);
        } else {
            var windowWidth = $(window).width();
            var cntWidth = windowWidth - 220;
            cdownpaygrid.SetWidth(cntWidth);
        }
        $('.navbar-minimalize').click(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                cdownpaygrid.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                cdownpaygrid.SetWidth(cntWidth);
            }
        });
    });
