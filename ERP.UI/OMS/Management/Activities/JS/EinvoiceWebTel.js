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

function updateGridByDateCancelledSI() {
    if (cFormDateCancelledSI.GetDate() == null) {
        jAlert('Please select from date.', 'Alert', function () { cFormDate.Focus(); });
    }
    else if (ctoDateCancelledSI.GetDate() == null) {
        jAlert('Please select to date.', 'Alert', function () { ctoDate.Focus(); });
    }
    else if (ccmbBranchfilterCancelledSI.GetValue() == null) {
        jAlert('Please select Branch.', 'Alert', function () { ccmbBranchfilter.Focus(); });
    }
    else {

        //cGrdQuotation.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue())
        $("#hfFromDate").val(cFormDateCancelledSI.GetDate().format('yyyy-MM-dd'));
        $("#hfToDate").val(ctoDateCancelledSI.GetDate().format('yyyy-MM-dd'));
        $("#hfBranchID").val(ccmbBranchfilterCancelledSI.GetValue());
        $("#hfIsFilter").val("Y");
        cGrdQuotationCancelledSI.Refresh();
        cGrdQuotationCancelledSI.Refresh();
    }
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

function updateGridByDateTSI() {
    if (cFormDateTSI.GetDate() == null) {
        jAlert('Please select from date.', 'Alert', function () { cFormDate.Focus(); });
    }
    else if (ctoDateTSI.GetDate() == null) {
        jAlert('Please select to date.', 'Alert', function () { ctoDate.Focus(); });
    }
    else if (ccmbBranchfilterTSI.GetValue() == null) {
        jAlert('Please select Branch.', 'Alert', function () { ccmbBranchfilter.Focus(); });
    }
    else {

        //cGrdQuotation.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue())
        $("#hfFromDate").val(cFormDateTSI.GetDate().format('yyyy-MM-dd'));
        $("#hfToDate").val(ctoDateTSI.GetDate().format('yyyy-MM-dd'));
        $("#hfBranchID").val(ccmbBranchfilterTSI.GetValue());
        $("#hfIsFilter").val("Y");
        cGrdQuotationTSI.Refresh();
        cGrdQuotationTSI.Refresh();
    }
}

function updateGridByDatePendingTSI() {
    if (cFormDatePendingTSI.GetDate() == null) {
        jAlert('Please select from date.', 'Alert', function () { cFormDate.Focus(); });
    }
    else if (ctoDatePendingTSI.GetDate() == null) {
        jAlert('Please select to date.', 'Alert', function () { ctoDate.Focus(); });
    }
    else if (ccmbBranchfilterPendingTSI.GetValue() == null) {
        jAlert('Please select Branch.', 'Alert', function () { ccmbBranchfilter.Focus(); });
    }
    else {

        //cGrdQuotation.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue())
        $("#hfFromDate").val(cFormDatePendingTSI.GetDate().format('yyyy-MM-dd'));
        $("#hfToDate").val(ctoDatePendingTSI.GetDate().format('yyyy-MM-dd'));
        $("#hfBranchID").val(ccmbBranchfilterPendingTSI.GetValue());
        $("#hfIsFilter").val("Y");
        cGrdQuotationPendingTSI.Refresh();
        cGrdQuotationPendingTSI.Refresh();
    }
}
function updateGridByDateCancelTSI() {
    if (cFormDateCancelTSI.GetDate() == null) {
        jAlert('Please select from date.', 'Alert', function () { cFormDate.Focus(); });
    }
    else if (ctoDateCancelTSI.GetDate() == null) {
        jAlert('Please select to date.', 'Alert', function () { ctoDate.Focus(); });
    }
    else if (ccmbBranchfilterCancelTSI.GetValue() == null) {
        jAlert('Please select Branch.', 'Alert', function () { ccmbBranchfilter.Focus(); });
    }
    else {

        //cGrdQuotation.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue())
        $("#hfFromDate").val(cFormDateCancelTSI.GetDate().format('yyyy-MM-dd'));
        $("#hfToDate").val(ctoDateCancelTSI.GetDate().format('yyyy-MM-dd'));
        $("#hfBranchID").val(ccmbBranchfilterCancelTSI.GetValue());
        $("#hfIsFilter").val("Y");
        cGrdQuotationCancelTSI.Refresh();
        cGrdQuotationCancelTSI.Refresh();
    }
}



function updateGridByDateCR() {
    if (cFormDateCR.GetDate() == null) {
        jAlert('Please select from date.', 'Alert', function () { cFormDate.Focus(); });
    }
    else if (ctoDateCR.GetDate() == null) {
        jAlert('Please select to date.', 'Alert', function () { ctoDate.Focus(); });
    }
    else if (ccmbBranchfilterCR.GetValue() == null) {
        jAlert('Please select Branch.', 'Alert', function () { ccmbBranchfilter.Focus(); });
    }
    else {

        //cGrdQuotation.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue())
        $("#hfFromDate").val(cFormDateCR.GetDate().format('yyyy-MM-dd'));
        $("#hfToDate").val(ctoDateCR.GetDate().format('yyyy-MM-dd'));
        $("#hfBranchID").val(ccmbBranchfilterCR.GetValue());
        $("#hfIsFilter").val("Y");
        cGrdQuotationCR.Refresh();
        cGrdQuotationCR.Refresh();
    }
}

function updateGridByDatePendingCR() {
    if (cFormDatePendingCR.GetDate() == null) {
        jAlert('Please select from date.', 'Alert', function () { cFormDate.Focus(); });
    }
    else if (ctoDatePendingCR.GetDate() == null) {
        jAlert('Please select to date.', 'Alert', function () { ctoDate.Focus(); });
    }
    else if (ccmbBranchfilterPendingCR.GetValue() == null) {
        jAlert('Please select Branch.', 'Alert', function () { ccmbBranchfilter.Focus(); });
    }
    else {

        //cGrdQuotation.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue())
        $("#hfFromDate").val(cFormDatePendingCR.GetDate().format('yyyy-MM-dd'));
        $("#hfToDate").val(ctoDatePendingCR.GetDate().format('yyyy-MM-dd'));
        $("#hfBranchID").val(ccmbBranchfilterPendingCR.GetValue());
        $("#hfIsFilter").val("Y");
        cGrdQuotationPendingCR.Refresh();
        cGrdQuotationPendingCR.Refresh();
    }
}


function updateGridByDateCancelCR() {
    if (cFormDateCancelCR.GetDate() == null) {
        jAlert('Please select from date.', 'Alert', function () { cFormDate.Focus(); });
    }
    else if (ctoDateCancelCR.GetDate() == null) {
        jAlert('Please select to date.', 'Alert', function () { ctoDate.Focus(); });
    }
    else if (ccmbBranchfilterCancelCR.GetValue() == null) {
        jAlert('Please select Branch.', 'Alert', function () { ccmbBranchfilter.Focus(); });
    }
    else {

        //cGrdQuotation.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue())
        $("#hfFromDate").val(cFormDateCancelCR.GetDate().format('yyyy-MM-dd'));
        $("#hfToDate").val(ctoDateCancelCR.GetDate().format('yyyy-MM-dd'));
        $("#hfBranchID").val(ccmbBranchfilterCancelCR.GetValue());
        $("#hfIsFilter").val("Y");
        cGrdQuotationCancelCR.Refresh();
        cGrdQuotationCancelCR.Refresh();
    }
}



function updateGridByDateewaybillSI() {
    if (cFormDateewaybillSI.GetDate() == null) {
        jAlert('Please select from date.', 'Alert', function () { cFormDate.Focus(); });
    }
    else if (ctoDateewaybillSI.GetDate() == null) {
        jAlert('Please select to date.', 'Alert', function () { ctoDate.Focus(); });
    }
    else if (ccmbBranchfilterewaybillSI.GetValue() == null) {
        jAlert('Please select Branch.', 'Alert', function () { ccmbBranchfilter.Focus(); });
    }
    else {

        //cGrdQuotation.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue())
        $("#hfFromDate").val(cFormDateewaybillSI.GetDate().format('yyyy-MM-dd'));
        $("#hfToDate").val(ctoDateewaybillSI.GetDate().format('yyyy-MM-dd'));
        $("#hfBranchID").val(ccmbBranchfilterewaybillSI.GetValue());
        $("#hfIsFilter").val("Y");
        cGrdQuotationewaybillSI.Refresh();
        cGrdQuotationewaybillSI.Refresh();
    }
}


function updateGridByDateCancelewaybill() {
    if (cFormDateCancelewaybill.GetDate() == null) {
        jAlert('Please select from date.', 'Alert', function () { cFormDate.Focus(); });
    }
    else if (ctoDateCancelewaybill.GetDate() == null) {
        jAlert('Please select to date.', 'Alert', function () { ctoDate.Focus(); });
    }
    else if (ccmbBranchfilterCancelewaybill.GetValue() == null) {
        jAlert('Please select Branch.', 'Alert', function () { ccmbBranchfilter.Focus(); });
    }
    else {

        //cGrdQuotation.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue())
        $("#hfFromDate").val(cFormDateCancelewaybill.GetDate().format('yyyy-MM-dd'));
        $("#hfToDate").val(ctoDateCancelewaybill.GetDate().format('yyyy-MM-dd'));
        $("#hfBranchID").val(ccmbBranchfilterCancelewaybill.GetValue());
        $("#hfIsFilter").val("Y");
        cGrdQuotationCancelewaybill.Refresh();
        cGrdQuotationCancelewaybill.Refresh();
    }
}


function updateGridByDateewaybillTSI() {
    if (cFormDateewaybillTSI.GetDate() == null) {
        jAlert('Please select from date.', 'Alert', function () { cFormDate.Focus(); });
    }
    else if (ctoDateewaybillTSI.GetDate() == null) {
        jAlert('Please select to date.', 'Alert', function () { ctoDate.Focus(); });
    }
    else if (ccmbBranchfilterewaybillTSI.GetValue() == null) {
        jAlert('Please select Branch.', 'Alert', function () { ccmbBranchfilter.Focus(); });
    }
    else {

        //cGrdQuotation.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue())
        $("#hfFromDate").val(cFormDateewaybillTSI.GetDate().format('yyyy-MM-dd'));
        $("#hfToDate").val(ctoDateewaybillTSI.GetDate().format('yyyy-MM-dd'));
        $("#hfBranchID").val(ccmbBranchfilterewaybillTSI.GetValue());
        $("#hfIsFilter").val("Y");
        cGrdQuotationewaybillTSI.Refresh();
        cGrdQuotationewaybillTSI.Refresh();
    }
}

function updateGridByDateCancelewaybillTSI() {
    if (cFormDateCancelewaybillTSI.GetDate() == null) {
        jAlert('Please select from date.', 'Alert', function () { cFormDate.Focus(); });
    }
    else if (ctoDateCancelewaybillTSI.GetDate() == null) {
        jAlert('Please select to date.', 'Alert', function () { ctoDate.Focus(); });
    }
    else if (ccmbBranchfilterCancelewaybillTSI.GetValue() == null) {
        jAlert('Please select Branch.', 'Alert', function () { ccmbBranchfilter.Focus(); });
    }
    else {

        //cGrdQuotation.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue())
        $("#hfFromDate").val(cFormDateCancelewaybillTSI.GetDate().format('yyyy-MM-dd'));
        $("#hfToDate").val(ctoDateCancelewaybillTSI.GetDate().format('yyyy-MM-dd'));
        $("#hfBranchID").val(ccmbBranchfilterCancelewaybillTSI.GetValue());
        $("#hfIsFilter").val("Y");
        cGrdQuotationCancelewaybillTSI.Refresh();
        cGrdQuotationCancelewaybillTSI.Refresh();
    }
}


function updateGridByDateewaybillSR() {
    if (cFormDateewaybillSR.GetDate() == null) {
        jAlert('Please select from date.', 'Alert', function () { cFormDate.Focus(); });
    }
    else if (ctoDateewaybillSR.GetDate() == null) {
        jAlert('Please select to date.', 'Alert', function () { ctoDate.Focus(); });
    }
    else if (ccmbBranchfilterewaybillSR.GetValue() == null) {
        jAlert('Please select Branch.', 'Alert', function () { ccmbBranchfilter.Focus(); });
    }
    else {

        //cGrdQuotation.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue())
        $("#hfFromDate").val(cFormDateewaybillSR.GetDate().format('yyyy-MM-dd'));
        $("#hfToDate").val(ctoDateewaybillSR.GetDate().format('yyyy-MM-dd'));
        $("#hfBranchID").val(ccmbBranchfilterewaybillSR.GetValue());
        $("#hfIsFilter").val("Y");
        cGrdQuotationewaybillSR.Refresh();
        cGrdQuotationewaybillSR.Refresh();
    }
}


function updateGridByDateCancelewaybillSR() {
    if (cFormDateCancelewaybillSR.GetDate() == null) {
        jAlert('Please select from date.', 'Alert', function () { cFormDate.Focus(); });
    }
    else if (ctoDateCancelewaybillSR.GetDate() == null) {
        jAlert('Please select to date.', 'Alert', function () { ctoDate.Focus(); });
    }
    else if (ccmbBranchfilterCancelewaybillSR.GetValue() == null) {
        jAlert('Please select Branch.', 'Alert', function () { ccmbBranchfilter.Focus(); });
    }
    else {

        //cGrdQuotation.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue())
        $("#hfFromDate").val(cFormDateCancelewaybillSR.GetDate().format('yyyy-MM-dd'));
        $("#hfToDate").val(ctoDateCancelewaybillSR.GetDate().format('yyyy-MM-dd'));
        $("#hfBranchID").val(ccmbBranchfilterCancelewaybillSR.GetValue());
        $("#hfIsFilter").val("Y");
        cGrdQuotationCancelewaybillSR.Refresh();
        cGrdQuotationCancelewaybillSR.Refresh();
    }
}



function grdEndcallback(s, e) {
    if (s.cpJson == "Yes") {
        $.ajax({
            type: "POST",
            url: "EinvoiceWebTel.aspx/generateMultiEinvoiceJSON",
            //data: "{'ProductName':'" + ProductName + "'}",
            //data: JSON.stringify(otherdet),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                console.log(msg.d);
                downloadObjectAsJson(msg.d, "Invoice_JSON");
                var json = JSON.stringify(msg.d);
                console.log(json);
                // WriteToFile(json);
            }
        });
        s.cpJson = null;
    }
    else if (cGrdQuotation.cpJSON == 'Cancel') {
        cGrdQuotation.cpJSON = null;
        var msg = "";
        if (s.cpSuccessMsg != "") {
            msg = msg + 'Cancelled successfully : ' + s.cpSuccessMsg.substr(1) + '<br/>'
        }
        if (s.cpFalureMsg != "") {
            msg = msg + 'Cancellation Failed : ' + s.cpFalureMsg.substr(1)
        }
        s.cpSuccessMsg = null;
        s.cpFalureMsg = null;

        jAlert(msg, 'Alert', function () {
            s.Refresh();
        })
    }


    else if (cGrdQuotation.cpJSON == 'Ewaybill') {
        cGrdQuotation.cpJSON = null;
        var msg = "";
        if (s.cpSuccessMsg != "") {
            msg = msg + 'E-Way bill genererated successfully : ' + s.cpSuccessMsg.substr(1) + '<br/>'
        }
        if (s.cpFalureMsg != "") {
            msg = msg + 'E-Way bill genereration Failed : ' + s.cpFalureMsg.substr(1)
        }
        s.cpSuccessMsg = null;
        s.cpFalureMsg = null;

        jAlert(msg, 'Alert', function () {
            s.Refresh();
        })
    }

}

function grdEndcallbackCR(s, e) {
    if (s.cpJson == "Yes") {
        $.ajax({
            type: "POST",
            url: "EinvoiceWebTel.aspx/generateMultiEinvoiceJSON",
            //data: "{'ProductName':'" + ProductName + "'}",
            //data: JSON.stringify(otherdet),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                console.log(msg.d);
                downloadObjectAsJson(msg.d, "Invoice_JSON");
                var json = JSON.stringify(msg.d);
                console.log(json);
                // WriteToFile(json);
            }
        });
        s.cpJson = null;
    }
    else if (s.cpJson == 'No') {
        s.cpJson = null;
        var msg = "";
        if (s.cpSuccessMsg != "") {
            msg = msg + 'Cancelled successfully : ' + s.cpSuccessMsg.substr(1) + '<br/>'
        }
        if (s.cpFalureMsg != "") {
            msg = msg + 'Cancellation Failed : ' + s.cpFalureMsg.substr(1)
        }
        s.cpSuccessMsg = null;
        s.cpFalureMsg = null;

        jAlert(msg, 'Alert', function () {
            s.Refresh();
        })
    }
    else if (s.cpJSON == 'Ewaybill') {
        cGrdQuotation.cpJSON = null;
        var msg = "";
        if (s.cpSuccessMsg != "") {
            msg = msg + 'E-Way bill genererated successfully : ' + s.cpSuccessMsg.substr(1) + '<br/>'
        }
        if (s.cpFalureMsg != "") {
            msg = msg + 'E-Way bill genereration Failed : ' + s.cpFalureMsg.substr(1)
        }
        s.cpSuccessMsg = null;
        s.cpFalureMsg = null;

        jAlert(msg, 'Alert', function () {
            s.Refresh();
        })
    }
}
function grdEndcallbackPendingCR(s, e) {
    if (s.cpJson == "Yes") {
        $.ajax({
            type: "POST",
            url: "EinvoiceWebTel.aspx/generateMultiEinvoiceJSON",
            //data: "{'ProductName':'" + ProductName + "'}",
            //data: JSON.stringify(otherdet),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                console.log(msg.d);
                downloadObjectAsJson(msg.d, "Invoice_JSON");
                var json = JSON.stringify(msg.d);
                console.log(json);
                // WriteToFile(json);
            }
        });
        s.cpJson = null;
    }
    else if (s.cpJson == 'No') {
        s.cpJson = null;
        var msg = "";
        if (s.cpSuccessMsg != "") {
            msg = msg + 'Generated successfully : ' + s.cpSuccessMsg.substr(1) + '<br/>'
        }
        if (s.cpFalureMsg != "") {
            msg = msg + 'Generation Failed : ' + s.cpFalureMsg.substr(1)
        }
        s.cpSuccessMsg = null;
        s.cpFalureMsg = null;

        jAlert(msg, 'Alert', function () {
            s.Refresh();
        })
    }
}

function grdEndcallbackPendingSI(s, e) {
    if (s.cpJson == "Yes") {
        $.ajax({
            type: "POST",
            url: "EinvoiceWebTel.aspx/generateMultiEinvoiceJSON",
            //data: "{'ProductName':'" + ProductName + "'}",
            //data: JSON.stringify(otherdet),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                console.log(msg.d);
                downloadObjectAsJson(msg.d, "Invoice_JSON");
                var json = JSON.stringify(msg.d);
                console.log(json);
                // WriteToFile(json);
            }
        });
        s.cpJson = null;
    }
    else if (s.cpJson == 'No') {
        s.cpJson = null;
        var msg = "";
        if (s.cpSuccessMsg != "") {
            msg = msg + 'Generated successfully : ' + s.cpSuccessMsg.substr(1) + '<br/>'
        }
        if (s.cpFalureMsg != "") {
            msg = msg + 'Generation Failed : ' + s.cpFalureMsg.substr(1) + '<br/>'
        }




        if (s.cpIRNSuccessMsg != "") {
            msg = msg + 'E-Way Bill Generated successfully : ' + s.cpIRNSuccessMsg.substr(1) + '<br/>'
        }
        if (s.cpIRNFalureMsg != "") {
            msg = msg + 'E-Way Bill Generation Failed : ' + s.cpIRNFalureMsg.substr(1)
        }


        s.cpSuccessMsg = null;
        s.cpFalureMsg = null;
        s.cpIRNSuccessMsg = null;
        s.cpIRNFalureMsg = null;

        jAlert(msg, 'Alert', function () {
            s.Refresh();
        })
    }
}

function grdEndcallbackPendingTSI(s, e) {
    if (s.cpJson == "Yes") {
        $.ajax({
            type: "POST",
            url: "EinvoiceWebTel.aspx/generateMultiEinvoiceJSON",
            //data: "{'ProductName':'" + ProductName + "'}",
            //data: JSON.stringify(otherdet),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                console.log(msg.d);
                downloadObjectAsJson(msg.d, "Invoice_JSON");
                var json = JSON.stringify(msg.d);
                console.log(json);
                // WriteToFile(json);
            }
        });
        s.cpJson = null;
    }
    else if (s.cpJson == 'No') {
        s.cpJson = null;
        var msg = "";
        if (s.cpSuccessMsg != "") {
            msg = msg + 'Generated successfully : ' + s.cpSuccessMsg.substr(1) + '<br/>'
        }
        if (s.cpFalureMsg != "") {
            msg = msg + 'Generation Failed : ' + s.cpFalureMsg.substr(1)
        }
        s.cpSuccessMsg = null;
        s.cpFalureMsg = null;

        jAlert(msg, 'Alert', function () {
            s.Refresh();
        })
    }
}


function DoownLoadJson(id) {
    var otherdet = {};
    otherdet.id = id;
    $.ajax({
        type: "POST",
        url: "EinvoiceWebTel.aspx/generateEinvoiceJSON",
        //data: "{'ProductName':'" + ProductName + "'}",
        data: JSON.stringify(otherdet),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            console.log(msg.d);
            downloadObjectAsJson(msg.d, id);
            var json = JSON.stringify(msg.d);
            console.log(json);
            //  WriteToFile(json);
        }
    });

}

function CancelIRN(irn) {
    $("#hdnCancelIRNType").val("SILine");
    $("#hdnCancelIRNNo").val(irn);
    $("#modal1").modal('show');
}


function UpdatePinIRN(id) {

    $("#hdnInvoiceId").val(id);
    var otherdet = {};
    otherdet.ID = $("#hdnInvoiceId").val();
    $.ajax({
        type: "POST",
        url: "EinvoiceWebTel.aspx/GetDistancePin",
        data: JSON.stringify(otherdet),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            ctxtPINtoPINDistance.SetValue(msg.d);

            $("#modalUpdatePin").modal('show');
        }
    });

}

function UpdatePinIRNTSI(id) {

    $("#hdnInvoiceId").val(id);
    var otherdet = {};
    otherdet.ID = $("#hdnInvoiceId").val();
    $.ajax({
        type: "POST",
        url: "EinvoiceWebTel.aspx/GetDistancePinTSI",
        data: JSON.stringify(otherdet),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            ctxtPINtoPINDistanceTSI.SetValue(msg.d);

            $("#modalUpdatePinTSI").modal('show');
        }
    });

}

function genEwaybill(irn, id) {
    cGrdQuotation.PerformCallback('GenEwayBill~' + irn + '~' + id);
}

function UploadEwaybillSI() {
    cGrdQuotation.PerformCallback('GenEwayBillBulk');
}

function genEwaybillTSI(irn, id) {
    cGrdQuotationTSI.PerformCallback('GenEwayBill~' + irn + '~' + id);
}

function UploadEwaybillTSI() {
    cGrdQuotationTSI.PerformCallback('GenEwayBillBulk~' + irn + '~' + id);
}

function genEwaybillSR(irn, id) {
    cGrdQuotationCR.PerformCallback('GenEwayBill~' + irn + '~' + id);
}

function UploadEwaybillSR() {
    cGrdQuotationCR.PerformCallback('GenEwayBillBulk~' + irn + '~' + id);
}


function CancelBulkIRN() {

    $("#hdnCancelIRNType").val("SIBulk");


    $("#modal1").modal('show');

}


function CancelIRNSR(irn) {

    $("#hdnCancelIRNType").val("SILineSR");
    $("#hdnCancelIRNNo").val(irn);

    $("#modal1").modal('show');

}


function CancelBulkIRNSR() {

    $("#hdnCancelIRNType").val("SIBulkSR");

    $("#modal1").modal('show');

}



function grdEndcallbackTSI(s, e) {
    if (s.cpJson == "Yes") {
        $.ajax({
            type: "POST",
            url: "EinvoiceWebTel.aspx/generateMultiEinvoiceJSON",
            //data: "{'ProductName':'" + ProductName + "'}",
            //data: JSON.stringify(otherdet),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                console.log(msg.d);
                downloadObjectAsJson(msg.d, "Invoice_JSON");
                var json = JSON.stringify(msg.d);
                console.log(json);
                // WriteToFile(json);
            }
        });
        s.cpJson = null;
    }
    else if (s.cpJson == 'No') {
        s.cpJson = null;
        var msg = "";
        if (s.cpSuccessMsg != "" && s.cpSuccessMsg != null) {
            msg = msg + 'Cancelled successfully : ' + s.cpSuccessMsg.substr(1) + '<br/>'
        }
        if (s.cpFalureMsg != "" && s.cpFalureMsg != null) {
            msg = msg + 'Cancellation Failed : ' + s.cpFalureMsg.substr(1)
        }
        s.cpSuccessMsg = null;
        s.cpFalureMsg = null;

        jAlert(msg, 'Alert', function () {
            s.Refresh();
        })
    }
    else if (s.cpJSON == 'Ewaybill') {
        cGrdQuotation.cpJSON = null;
        var msg = "";
        if (s.cpSuccessMsg != "") {
            msg = msg + 'E-Way bill genererated successfully : ' + s.cpSuccessMsg.substr(1) + '<br/>'
        }
        if (s.cpFalureMsg != "") {
            msg = msg + 'E-Way bill genereration Failed : ' + s.cpFalureMsg.substr(1)
        }
        s.cpSuccessMsg = null;
        s.cpFalureMsg = null;

        jAlert(msg, 'Alert', function () {
            s.Refresh();
        })
    }
}

function DoownLoadJsonTSI(id) {
    var otherdet = {};
    otherdet.id = id;
    $.ajax({
        type: "POST",
        url: "EinvoiceWebTel.aspx/generateEinvoiceJSONTSI",
        //data: "{'ProductName':'" + ProductName + "'}",
        data: JSON.stringify(otherdet),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            console.log(msg.d);
            downloadObjectAsJson(msg.d, id);
            var json = JSON.stringify(msg.d);
            console.log(json);
            //  WriteToFile(json);
        }
    });

}


function DoownLoadJsonSR(id) {
    var otherdet = {};
    otherdet.id = id;
    $.ajax({
        type: "POST",
        url: "EinvoiceWebTel.aspx/generateEinvoiceJSONSR",
        //data: "{'ProductName':'" + ProductName + "'}",
        data: JSON.stringify(otherdet),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            console.log(msg.d);
            downloadObjectAsJson(msg.d, id);
            var json = JSON.stringify(msg.d);
            console.log(json);
            //  WriteToFile(json);
        }
    });

}


function CancelIRNTSI(irn) {

    $("#hdnCancelIRNType").val("SILineTSI");
    $("#hdnCancelIRNNo").val(irn);

    $("#modal1").modal('show');

}

function CancelBulkIRNTSI() {

    $("#hdnCancelIRNType").val("SIBulkTSI");


    $("#modal1").modal('show');

}

function PINtoPINDistanceSubmit() {
    var otherdet = {};
    otherdet.PINtoPINDistance = ctxtPINtoPINDistance.GetValue();
    otherdet.ID = $("#hdnInvoiceId").val();
    $.ajax({
        type: "POST",
        url: "EinvoiceWebTel.aspx/UpdatePin",
        data: JSON.stringify(otherdet),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            if (msg.d > 0) {
                jAlert("Update Successfully.");
                cGrdQuotation.Refresh();
                $("#modalUpdatePin").modal('hide');
            }

        }
    });
}

function PINtoPINDistanceSubmitTSI() {
    var otherdet = {};
    otherdet.PINtoPINDistance = ctxtPINtoPINDistanceTSI.GetValue();
    otherdet.ID = $("#hdnInvoiceId").val();
    $.ajax({
        type: "POST",
        url: "EinvoiceWebTel.aspx/UpdatePinTSI",
        data: JSON.stringify(otherdet),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            if (msg.d > 0) {
                jAlert("Update Successfully.");
                cGrdQuotationTSI.Refresh();
                $("#modalUpdatePinTSI").modal('hide');
            }

        }
    });
}

function CancelIRNSubmit() {
    if ($("#hdnCancelIRNType").val() == 'SILine') {
        var otherdet = {};
        otherdet.irn = $("#hdnCancelIRNNo").val();
        otherdet.type = $("#hdnCancelIRNType").val();
        otherdet.cancelReason = $("#ddlCancelReason").val();
        otherdet.cancelRemarks = $("#txtCancelRemarks").val();



        $.ajax({
            type: "POST",
            url: "EinvoiceWebTel.aspx/CancelIRN",
            //data: "{'ProductName':'" + ProductName + "'}",
            data: JSON.stringify(otherdet),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                jAlert(msg.d);
                cGrdQuotation.Refresh();
            }
        });
    }
    else if ($("#hdnCancelIRNType").val() == 'SIBulk') {
        cGrdQuotation.PerformCallback('CancelIRN~' + $("#ddlCancelReason").val() + '~' + $("#txtCancelRemarks").val());
    }
    else if ($("#hdnCancelIRNType").val() == 'SIBulkTSI') {
        cGrdQuotationTSI.PerformCallback('CancelIRN~' + $("#ddlCancelReason").val() + '~' + $("#txtCancelRemarks").val());
    }
    else if ($("#hdnCancelIRNType").val() == 'SILineTSI') {
        var otherdet = {};
        otherdet.irn = $("#hdnCancelIRNNo").val();
        otherdet.type = $("#hdnCancelIRNType").val();
        otherdet.cancelReason = $("#ddlCancelReason").val();
        otherdet.cancelRemarks = $("#txtCancelRemarks").val();



        $.ajax({
            type: "POST",
            url: "EinvoiceWebTel.aspx/CancelIRNTSI",
            //data: "{'ProductName':'" + ProductName + "'}",
            data: JSON.stringify(otherdet),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                jAlert(msg.d);
                cGrdQuotationTSI.Refresh();
            }
        });
    }
    else if ($("#hdnCancelIRNType").val() == 'SIBulkSR') {
        cGrdQuotationCR.PerformCallback('CancelIRN~' + $("#ddlCancelReason").val() + '~' + $("#txtCancelRemarks").val());
    }
    else if ($("#hdnCancelIRNType").val() == 'SILineSR') {
        var otherdet = {};
        otherdet.irn = $("#hdnCancelIRNNo").val();
        otherdet.type = $("#hdnCancelIRNType").val();
        otherdet.cancelReason = $("#ddlCancelReason").val();
        otherdet.cancelRemarks = $("#txtCancelRemarks").val();



        $.ajax({
            type: "POST",
            url: "EinvoiceWebTel.aspx/CancelIRNSR",
            //data: "{'ProductName':'" + ProductName + "'}",
            data: JSON.stringify(otherdet),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                jAlert(msg.d);
                cGrdQuotationCR.Refresh();
            }
        });
    }
}


function UploadIRNSI(id) {
    cGrdQuotationPendinSI.PerformCallback('UploadSingleIRN~SI~' + id);
}

function UploadBulkIRNSI() {
    cGrdQuotationPendinSI.PerformCallback('UploadBulkIRN~SI');
}


function UploadIRNSR(id) {
    cGrdQuotationPendingCR.PerformCallback('UploadSingleIRN~SI~' + id);
}

function UploadBulkIRNTSR() {
    cGrdQuotationPendingCR.PerformCallback('UploadBulkIRN~SI');
}


function UploadIRNTSI(id) {
    cGrdQuotationPendingTSI.PerformCallback('UploadSingleIRN~SI~' + id);
}

function UploadBulkIRNTSI() {
    cGrdQuotationPendingTSI.PerformCallback('UploadBulkIRN~SI');
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

function downloadObjectAsJson(exportObj, exportName) {
    var dataStr = "data:text/json;charset=utf-8," + encodeURIComponent(JSON.stringify(exportObj, 0, 4));
    var downloadAnchorNode = document.createElement('a');
    downloadAnchorNode.setAttribute("href", dataStr);
    downloadAnchorNode.setAttribute("download", exportName + ".json");
    document.body.appendChild(downloadAnchorNode); // required for firefox
    downloadAnchorNode.click();
    downloadAnchorNode.remove();
}

function UploadExcel() {



    var formData = new FormData();
    formData.append('file', $('#flEcel')[0].files[0]);
    $.ajax({
        type: 'post',
        url: 'fileUploader.ashx',
        data: formData,
        success: function (status) {
            if (status != 'error') {

            }
        },
        processData: false,
        contentType: false,
        error: function (msg) {
            alert("Whoops something went wrong!");
        }
    });
}

function DownloadBulkJSONSI() {
    cGrdQuotation.PerformCallback('DownloadJSON');
}

function DownloadBulkJSONPendingSI() {
    cGrdQuotationPendinSI.PerformCallback('DownloadJSON');
}
function DownloadBulkJSONTSI() {
    cGrdQuotationTSI.PerformCallback('DownloadJSON');
}

function DownloadBulkJSONPendingTSI() {
    cGrdQuotationPendingTSI.PerformCallback('DownloadJSON');
}

function DownloadBulkJSONSR() {
    cGrdQuotationCR.PerformCallback('DownloadJSON');
}

function DownloadBulkJSONPendingSR() {
    cGrdQuotationPendingCR.PerformCallback('DownloadJSON');
}

$(document).ready(function () {
    $('#modal1').on('shown.bs.modal', function () {

        $("#upload-widget").dropzone({ url: "/file/post" });
    });
});
function ShowInfoN(DocId, DocType, ErrType, Irn, InvoiceNo, CustomerName, ChallanNo, NetAmount, AckDt, AckNo) {

    console.log(Irn, InvoiceNo, CustomerName, ChallanNo, NetAmount, AckDt, AckNo);

    var dt = {}
    dt.DocId = DocId;
    dt.DocType = DocType;
    dt.ErrType = ErrType;
    $.ajax({
        type: "POST",
        url: "Activities/Services/Master.asmx/GetEInvErrorMsg",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(dt),
        dataType: "json",
        success: function (data) {
            console.log('GetEInvErrorMsg', data);
            var data = data.d;
            if (data.length > 0) {
                var mapped = data.map(function (item) {
                    return { a: item.DocumentDate, b: item.DocumentNo, c: item.ErrorType, d: item.Errorcode, e: item.Errormsg };
                });
                showalertTable(mapped, "Info");
            } else {
                if (ErrType == "IRN_CANCEL") {
                    var LogedUser = $("#LogedUser").text();
                    $("#SetLogedUser").text(LogedUser);
                    var st = AckDt.split(" ");
                    var dt = st[0];
                    console.log(st)
                    var dateF = chageDateFormat(dt) + " &nbsp;  " + st[1] + st[2];

                    $(".popupSuc, .bcShad").addClass("in");
                    $("#IrnNumber").val(Irn);
                    $("#IrnlblInvNUmber").text(InvoiceNo);
                    $("#IrnlblInvDate").text(CustomerName);
                    $("#IrnlblCust").text(ChallanNo);
                    $("#IrnlblAmount").text(NetAmount);
                    $("#AckNum").text(AckNo);
                    $("#AckDate").html(dateF);
                } else {
                    $(".popupSuc, .bcShad").removeClass("in");
                    jAlert("No Data Found");
                }

            }

        }
    });
}
function chageDateFormat(date) {
    var split = date.split("/");

    var day = split[1];
    var d = "";
    if (day.length < 2) {
        d = "0" + day
    } else {
        d = day
    }

    var month = split[0];
    var m = "";
    if (month.length < 2) {
        m = "0" + month
    } else {
        m = month
    }
    var year = split[2]
    return d + "-" + m + "-" + year;

}
function chageDateFormatY(date) {
    var split = date.split("/");

    var day = split[1];
    var d = "";
    if (day.length < 2) {
        d = "0" + day
    } else {
        d = day
    }

    var month = split[0];
    var m = "";
    if (month.length < 2) {
        m = "0" + month
    } else {
        m = month
    }
    var year = split[2]
    return year + "-" + m + "-" + d;

}
function rmPop() {
    $(".popupSuc, .bcShad").removeClass("in");
}

function myFunction() {
    var copyText = document.getElementById("IrnNumber");
    copyText.select();
    copyText.setSelectionRange(0, 99999);
    document.execCommand("copy");

    var tooltip = document.getElementById("myTooltip");
    tooltip.innerHTML = "Copied";
}

function outFunc() {
    var tooltip = document.getElementById("myTooltip");
    tooltip.innerHTML = "Copy to clipboard";
}
function tsiBoxRefresh(frmdate, todate) {
    var dt = {};
    dt.frmdate = frmdate;
    dt.todate = todate;
    $.ajax({
        type: "POST",
        url: "EinvoiceWebTel.aspx/GetTSITabBox",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(dt),
        dataType: "json",
        success: function (data) {
            console.log('GetTSITabBox', data);
            $("#TSTOTAL_AMOUNT").text(numberWithCommas(data.d[0].TOTAL_AMOUNT));
            $("#TSTOTAL_COUNTS").text(data.d[0].TOTAL_COUNTS);
            $("#TSTOTAL_GENERATED").text(data.d[0].TOTAL_GENERATED);
            $("#TSTOTAL_GENERATED_AMOUNT").text(numberWithCommas(data.d[0].TOTAL_GENERATED_AMOUNT));
            $("#TSTOTAL_PENDING").text(data.d[0].TOTAL_PENDING);
            $("#TSTOTAL_PENDING_AMOUNT").text(numberWithCommas(data.d[0].TOTAL_PENDING_AMOUNT));
            $("#TSTOTAL_CANCEL").text(data.d[0].TOTAL_CANCEL);
            $("#TSTOTAL_CANCEL_AMOUNT").text(numberWithCommas(data.d[0].TOTAL_CANCEL_AMOUNT));
        }
    });
}
function SRBoxRefresh(frmdate, todate) {
    var dt = {};
    dt.frmdate = frmdate;
    dt.todate = todate;
    $.ajax({
        type: "POST",
        url: "EinvoiceWebTel.aspx/GetSRTabBox",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(dt),
        dataType: "json",
        success: function (data) {
            console.log('GetTSITabBox', data);
            $("#SRTOTAL_AMOUNT").text(numberWithCommas(data.d[0].TOTAL_AMOUNT));
            $("#SRTOTAL_COUNTS").text(data.d[0].TOTAL_COUNTS);
            $("#SRTOTAL_GENERATED").text(data.d[0].TOTAL_GENERATED);
            $("#SRTOTAL_GENERATED_AMOUNT").text(numberWithCommas(data.d[0].TOTAL_GENERATED_AMOUNT));
            $("#SRTOTAL_PENDING").text(data.d[0].TOTAL_PENDING);
            $("#SRTOTAL_PENDING_AMOUNT").text(numberWithCommas(data.d[0].TOTAL_PENDING_AMOUNT));
            $("#SRTOTAL_CANCEL").text(data.d[0].TOTAL_CANCEL);
            $("#SRTOTAL_CANCEL_AMOUNT").text(numberWithCommas(data.d[0].TOTAL_CANCEL_AMOUNT));
        }
    });
}
function SIBoxRefresh(frmdate, todate) {
    var dt = {};
    dt.frmdate = frmdate;
    dt.todate = todate;
    $.ajax({
        type: "POST",
        url: "EinvoiceWebTel.aspx/GetSITabBox",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(dt),
        dataType: "json",
        success: function (data) {
            console.log('GetTSITabBox', data);
            $("#SITOTAL_AMOUNT").text(numberWithCommas(data.d[0].TOTAL_AMOUNT));
            $("#SITOTAL_COUNTS").text(data.d[0].TOTAL_COUNTS);
            $("#SITOTAL_GENERATED").text(data.d[0].TOTAL_GENERATED);
            $("#SITOTAL_GENERATED_AMOUNT").text(numberWithCommas(data.d[0].TOTAL_GENERATED_AMOUNT));
            $("#SITOTAL_PENDING").text(data.d[0].TOTAL_PENDING);
            $("#SITOTAL_PENDING_AMOUNT").text(numberWithCommas(data.d[0].TOTAL_PENDING_AMOUNT));
            $("#SITOTAL_CANCEL").text(data.d[0].TOTAL_CANCEL);
            $("#SITOTAL_CANCEL_AMOUNT").text(numberWithCommas(data.d[0].TOTAL_CANCEL_AMOUNT));
        }
    });
}
$(function () {
    $('[data-toggle="tooltip"]').tooltip();
    cFormDateFilter1.SetValue(new Date());
    cToDateFilter1.SetValue(new Date());
    cFormDateFilter2.SetValue(new Date());
    cToDateFilter2.SetValue(new Date());
    cFormDateFilter3.SetValue(new Date());
    cToDateFilter3.SetValue(new Date());
    $("#filterOneok").click(function () {
        var getFDat = cFormDateFilter1.GetDate().format("yyyy-MM-dd");
        var getTDat = cToDateFilter1.GetDate().format("yyyy-MM-dd");
        SIBoxRefresh(getFDat, getTDat);
        $("#homeFilter").modal("hide");
        //Irn gen
        cFormDate.SetDate(cFormDateFilter1.GetDate());
        ctoDate.SetDate(cToDateFilter1.GetDate());
        //Irn pending
        cFormDatePendinSI.SetDate(cFormDateFilter1.GetDate());
        ctoDatePendinSI.SetDate(cToDateFilter1.GetDate());
        //Irn cancel 
        cFormDateCancelledSI.SetDate(cFormDateFilter1.GetDate());
        ctoDateCancelledSI.SetDate(cToDateFilter1.GetDate());
        //Irn Ewaybill
        cFormDateewaybillSI.SetDate(cFormDateFilter1.GetDate());
        ctoDateewaybillSI.SetDate(cToDateFilter1.GetDate());
    })
    $("#filterTwook").click(function () {
        var getFDat = cFormDateFilter2.GetDate().format("yyyy-MM-dd");
        var getTDat = cToDateFilter2.GetDate().format("yyyy-MM-dd");
        tsiBoxRefresh(getFDat, getTDat);
        $("#profileFilter").modal("hide");
        //Irn gen
        cFormDateTSI.SetDate(cFormDateFilter2.GetDate());
        ctoDateTSI.SetDate(cToDateFilter2.GetDate());
        //Irn pending
        cFormDatePendingTSI.SetDate(cFormDateFilter2.GetDate());
        ctoDatePendingTSI.SetDate(cToDateFilter2.GetDate());
        //Irn cancel 
        cFormDateCancelTSI.SetDate(cFormDateFilter2.GetDate());
        ctoDateCancelTSI.SetDate(cToDateFilter2.GetDate());
        //Irn Ewaybill
        cFormDateewaybillTSI.SetDate(cFormDateFilter2.GetDate());
        ctoDateewaybillTSI.SetDate(cToDateFilter2.GetDate());
    });
    $("#filterThreeok").click(function () {
        var getFDat = cFormDateFilter3.GetDate().format("yyyy-MM-dd");
        var getTDat = cToDateFilter3.GetDate().format("yyyy-MM-dd");
        SRBoxRefresh(getFDat, getTDat);
        $("#messagesFilter").modal("hide");
        //Irn gen
        cFormDateCR.SetDate(cFormDateFilter3.GetDate());
        ctoDateCR.SetDate(cToDateFilter3.GetDate());
        //Irn pending
        cFormDatePendingCR.SetDate(cFormDateFilter3.GetDate());
        ctoDatePendingCR.SetDate(cToDateFilter3.GetDate());
        //Irn cancel 
        cFormDateCancelCR.SetDate(cFormDateFilter3.GetDate());
        ctoDateCancelCR.SetDate(cToDateFilter3.GetDate());
        //Irn Ewaybill
        cFormDateewaybillSR.SetDate(cFormDateFilter3.GetDate());
        ctoDateewaybillSR.SetDate(cToDateFilter3.GetDate());
    });
});
$(document).ready(function () {
    var dateToday = new Date().toLocaleDateString();
    var fDate = chageDateFormatY(dateToday);
    console.log(fDate)
    SIBoxRefresh(fDate, fDate);
    $(".clickTrig").click(function () {
        var id = $(this).data("triggreid")
        console.log(id);
        if (id == "#pills-home-tab") {
            $("#SIgenBut").trigger("click");
        } else if (id == "#pills-profile-tab") {
            $("#SIpendindBut").trigger("click");
        } else if (id == "#pills-cancel-tab") {
            $("#SIcancelBut").trigger("click");
        } else if (id == "#pills-homeTSI-tab") {
            $("#TSIgenBut").trigger("click");
        } else if (id == "#pills-profileTSI-tab") {
            $("#TSIpendingBut").trigger("click");
        } else if (id == "#pills-CancelTSI-tab") {
            $("#TSIcancelBut").trigger("click");
        } else if (id == "#pills-homeSR-tab") {
            $("#SRgenBut").trigger("click");
        } else if (id == "#pills-profileSR-tab") {
            $("#SRpendingBut").trigger("click");
        } else if (id == "#pills-cancelSR-tab") {
            $("#SRcancelBut").trigger("click");
        }
        //console.log(id);
        $(id).trigger("click");
    })

    $("#TsiTabLoad").click(function () {
        var dateToday = new Date().toLocaleDateString();
        var fDate = chageDateFormatY(dateToday);
        tsiBoxRefresh(fDate, fDate)
    });
    $("#SrTabLoad").click(function () {
        var dateToday = new Date().toLocaleDateString();
        var fDate = chageDateFormatY(dateToday);
        SRBoxRefresh(fDate, fDate)
    });
});



function CancelEwayBillSI(ewaybill) {

    $("#hdnEwayBillType").val("SI");
    $("#hdnEwayBillNo").val(ewaybill);

    $("#EwayBillcancelModal").modal('show')
}

function CancelEwayBillTSI(ewaybill) {

    $("#hdnEwayBillType").val("TSI");
    $("#hdnEwayBillNo").val(ewaybill);

    $("#EwayBillcancelModal").modal('show')
}
function CancelEwayBillSR(ewaybill) {

    $("#hdnEwayBillType").val("SR");
    $("#hdnEwayBillNo").val(ewaybill);

    $("#EwayBillcancelModal").modal('show')
}
function UpdateEwayBillSI(ewaybill) {

    $("#hdnEwayBillType").val("UPDATESI");
    $("#hdnEwayBillNo").val(ewaybill);

    $("#EwayBillcancelModal").modal('show')

}


function UpdateTransporterEwayBillSI(ewaybill) {
    cGrdQuotationewaybillSI.PerformCallback('UpdateTransporterEwayBill~' + ewaybill)
}


function DownloadEwayBillSI(ewaybill) {
    cGrdQuotationewaybillSI.PerformCallback('DownloadEwayBill~' + ewaybill)
}
//Mantis Issue 24237
function DownloadEwayBillTSI(ewaybill) {
    cGrdQuotationewaybillTSI.PerformCallback('DownloadEwayBill~' + ewaybill)
}
//End of Mantis Issue 24237

function grdEndcallbackEwaybillSI(s, e) {

    if (s.cpJson == "cancelEwayBill") {
        s.cpJson = null;
        var msg = "";
        if (s.cpSuccessMsg != "" && s.cpSuccessMsg != null) {
            msg = msg + 'Cancelled successfully : ' + s.cpSuccessMsg.substr(1) + '<br/>'
        }
        if (s.cpFalureMsg != "" && s.cpFalureMsg != null) {
            msg = msg + 'Cancellation Failed : ' + s.cpFalureMsg.substr(1)
        }
        s.cpSuccessMsg = null;
        s.cpFalureMsg = null;

        jAlert(msg, 'Alert', function () {
            s.Refresh();
        })
    }
    else if (s.cpJson == "UpdateEwaybill") {
        s.cpJson = null;
        var msg = "";
        if (s.cpSuccessMsg != "" && s.cpSuccessMsg != null) {
            msg = msg + 'Updated successfully : ' + s.cpSuccessMsg.substr(1) + '<br/>'
        }
        if (s.cpFalureMsg != "" && s.cpFalureMsg != null) {
            msg = msg + 'Updation Failed : ' + s.cpFalureMsg.substr(1)
        }
        s.cpSuccessMsg = null;
        s.cpFalureMsg = null;

        jAlert(msg, 'Alert', function () {
            s.Refresh();
        })
    }
    else if (s.cpJson == "UpdateEwaybillTr") {
        s.cpJson = null;
        var msg = "";
        if (s.cpSuccessMsg != "" && s.cpSuccessMsg != null) {
            msg = msg + 'Updated successfully : ' + s.cpSuccessMsg.substr(1) + '<br/>'
        }
        if (s.cpFalureMsg != "" && s.cpFalureMsg != null) {
            msg = msg + 'Updation Failed : ' + s.cpFalureMsg.substr(1)
        }
        s.cpSuccessMsg = null;
        s.cpFalureMsg = null;

        jAlert(msg, 'Alert', function () {
            s.Refresh();
        })
    }
        //else if (s.cpJson == "DownloadEwaybill") {
        //    var billNumber = s.cpeWaybillNumber;
        //    ///var dataStr = "data:text/json;charset=utf-8," + encodeURIComponent(JSON.stringify(exportObj, 0, 4));
        //    var downloadAnchorNode = document.createElement('a');
        //    //downloadAnchorNode.setAttribute("href", dataStr);
        //    downloadAnchorNode.setAttribute("download", billNumber + ".pdf");
        //    document.body.appendChild(downloadAnchorNode); // required for firefox
        //    downloadAnchorNode.click();
        //    downloadAnchorNode.remove();
        //}
    else if (s.cpJson == "DownloadEwaybill") {

        s.cpJson = null;
        var pathbillNumber = s.cpeWaybillNumber;
        var link = document.createElement('a');
        link.href = pathbillNumber;
        link.download = pathbillNumber;

        window.open(pathbillNumber);
        link.dispatchEvent(new MouseEvent('click'));
        s.cpeWaybillNumber = null;

    }

}


function grdEndcallbackewaybillTSI(s, e) {




    if (s.cpJson == "cancelEwayBill") {
        s.cpJson = null;
        var msg = "";
        if (s.cpSuccessMsg != "" && s.cpSuccessMsg != null) {
            msg = msg + 'Cancelled successfully : ' + s.cpSuccessMsg.substr(1) + '<br/>'
        }
        if (s.cpFalureMsg != "" && s.cpFalureMsg != null) {
            msg = msg + 'Cancellation Failed : ' + s.cpFalureMsg.substr(1)
        }
        s.cpSuccessMsg = null;
        s.cpFalureMsg = null;

        jAlert(msg, 'Alert', function () {
            s.Refresh();
        })
    }
    else if (s.cpJson == "UpdateEwaybill") {
        s.cpJson = null;
        var msg = "";
        if (s.cpSuccessMsg != "" && s.cpSuccessMsg != null) {
            msg = msg + 'Updated successfully : ' + s.cpSuccessMsg.substr(1) + '<br/>'
        }
        if (s.cpFalureMsg != "" && s.cpFalureMsg != null) {
            msg = msg + 'Updation Failed : ' + s.cpFalureMsg.substr(1)
        }
        s.cpSuccessMsg = null;
        s.cpFalureMsg = null;

        jAlert(msg, 'Alert', function () {
            s.Refresh();
        })
    }
    else if (s.cpJson == "UpdateEwaybillTr") {
        s.cpJson = null;
        var msg = "";
        if (s.cpSuccessMsg != "" && s.cpSuccessMsg != null) {
            msg = msg + 'Updated successfully : ' + s.cpSuccessMsg.substr(1) + '<br/>'
        }
        if (s.cpFalureMsg != "" && s.cpFalureMsg != null) {
            msg = msg + 'Updation Failed : ' + s.cpFalureMsg.substr(1)
        }
        s.cpSuccessMsg = null;
        s.cpFalureMsg = null;

        jAlert(msg, 'Alert', function () {
            s.Refresh();
        })
    }
    else if (s.cpJson == "DownloadEwaybill") {
       
        s.cpJson = null;
        var pathbillNumber = s.cpeWaybillNumber;
        var link = document.createElement('a');
        link.href = pathbillNumber;
        link.download = pathbillNumber;    
        
        window.open(pathbillNumber);
        link.dispatchEvent(new MouseEvent('click'));
        s.cpeWaybillNumber = null;
    }
}

function grdEndcallbackewaybillSR(s, e) {




    if (s.cpJson == "cancelEwayBill") {
        s.cpJson = null;
        var msg = "";
        if (s.cpSuccessMsg != "" && s.cpSuccessMsg != null) {
            msg = msg + 'Cancelled successfully : ' + s.cpSuccessMsg.substr(1) + '<br/>'
        }
        if (s.cpFalureMsg != "" && s.cpFalureMsg != null) {
            msg = msg + 'Cancellation Failed : ' + s.cpFalureMsg.substr(1)
        }
        s.cpSuccessMsg = null;
        s.cpFalureMsg = null;

        jAlert(msg, 'Alert', function () {
            s.Refresh();
        })
    }
    else if (s.cpJson == "UpdateEwaybill") {
        s.cpJson = null;
        var msg = "";
        if (s.cpSuccessMsg != "" && s.cpSuccessMsg != null) {
            msg = msg + 'Updated successfully : ' + s.cpSuccessMsg.substr(1) + '<br/>'
        }
        if (s.cpFalureMsg != "" && s.cpFalureMsg != null) {
            msg = msg + 'Updation Failed : ' + s.cpFalureMsg.substr(1)
        }
        s.cpSuccessMsg = null;
        s.cpFalureMsg = null;

        jAlert(msg, 'Alert', function () {
            s.Refresh();
        })
    }
    else if (s.cpJson == "UpdateEwaybillTr") {
        s.cpJson = null;
        var msg = "";
        if (s.cpSuccessMsg != "" && s.cpSuccessMsg != null) {
            msg = msg + 'Updated successfully : ' + s.cpSuccessMsg.substr(1) + '<br/>'
        }
        if (s.cpFalureMsg != "" && s.cpFalureMsg != null) {
            msg = msg + 'Updation Failed : ' + s.cpFalureMsg.substr(1)
        }
        s.cpSuccessMsg = null;
        s.cpFalureMsg = null;

        jAlert(msg, 'Alert', function () {
            s.Refresh();
        })
    }
    else if (s.cpJson == "DownloadEwaybill") {
        s.cpJson = null;
        var pathbillNumber = s.cpeWaybillNumber;
        var link = document.createElement('a');
        link.href = pathbillNumber;
        link.download = pathbillNumber;

        window.open(pathbillNumber);
        link.dispatchEvent(new MouseEvent('click'));
        s.cpeWaybillNumber = null;
    }
}

function CancelEwaSubmit() {
    if ($("#hdnEwayBillType").val() == 'SI') {
        cGrdQuotationewaybillSI.PerformCallback('CancelEwayBill~' + $("#hdnEwayBillNo").val() + "~" + $("#ddlEwaybillCancelReason").val() + "~" + $("#txtEwayCancelRemarks").val());
    }
    else if ($("#hdnEwayBillType").val() == 'UPDATESI') {
        cGrdQuotationewaybillSI.PerformCallback('UpdateEwayBill~' + $("#hdnEwayBillNo").val() + "~" + $("#ddlEwaybillCancelReason").val() + "~" + $("#txtEwayCancelRemarks").val());
    }
    else if ($("#hdnEwayBillType").val() == 'TSI') {
        cGrdQuotationewaybillTSI.PerformCallback('CancelEwayBill~' + $("#hdnEwayBillNo").val() + "~" + $("#ddlEwaybillCancelReason").val() + "~" + $("#txtEwayCancelRemarks").val());
    }
    else if ($("#hdnEwayBillType").val() == 'SR') {
        cGrdQuotationewaybillCR.PerformCallback('CancelEwayBill~' + $("#hdnEwayBillNo").val() + "~" + $("#ddlEwaybillCancelReason").val() + "~" + $("#txtEwayCancelRemarks").val());
    }

    $("#hdnEwayBillType").val("");
    $("#hdnEwayBillNo").val("");
}

function grdEndcallbackewaybillSI(s, e) {
    if (s.cpJson == "DownloadEwaybill") {

        s.cpJson = null;
        var pathbillNumber = s.cpeWaybillNumber;
        var link = document.createElement('a');
        link.href = pathbillNumber;
        link.download = pathbillNumber;

        window.open(pathbillNumber);
        link.dispatchEvent(new MouseEvent('click'));
        s.cpeWaybillNumber = null;

    }
    else if (s.cpJson == "cancelEwayBill") {
        s.cpJson = null;
        var msg = "";
        if (s.cpSuccessMsg != "" && s.cpSuccessMsg != null) {
            msg = msg + 'Cancelled successfully : ' + s.cpSuccessMsg.substr(1) + '<br/>'
        }
        if (s.cpFalureMsg != "" && s.cpFalureMsg != null) {
            msg = msg + 'Cancellation Failed : ' + s.cpFalureMsg.substr(1)
        }
        s.cpSuccessMsg = null;
        s.cpFalureMsg = null;
        $("#EwayBillcancelModal").modal('hide');
        jAlert(msg, 'Alert', function () {
            s.Refresh();
        })
    }
    else if (s.cpJson == "UpdateEwaybill") {
        s.cpJson = null;
        var msg = "";
        if (s.cpSuccessMsg != "" && s.cpSuccessMsg != null) {
            msg = msg + 'Updated successfully : ' + s.cpSuccessMsg.substr(1) + '<br/>'
        }
        if (s.cpFalureMsg != "" && s.cpFalureMsg != null) {
            msg = msg + 'Updation Failed : ' + s.cpFalureMsg.substr(1)
        }
        s.cpSuccessMsg = null;
        s.cpFalureMsg = null;

        jAlert(msg, 'Alert', function () {
            s.Refresh();
        })
    }
    else if (s.cpJson == "UpdateEwaybillTr") {
        s.cpJson = null;
        var msg = "";
        if (s.cpSuccessMsg != "" && s.cpSuccessMsg != null) {
            msg = msg + 'Updated successfully : ' + s.cpSuccessMsg.substr(1) + '<br/>'
        }
        if (s.cpFalureMsg != "" && s.cpFalureMsg != null) {
            msg = msg + 'Updation Failed : ' + s.cpFalureMsg.substr(1)
        }
        s.cpSuccessMsg = null;
        s.cpFalureMsg = null;

        jAlert(msg, 'Alert', function () {

            s.Refresh();

        })
    }
}

function grdEndcallbackewaybillSR(s, e) {

}

//function grdEndcallbackewaybillTSI(s, e) {

//}