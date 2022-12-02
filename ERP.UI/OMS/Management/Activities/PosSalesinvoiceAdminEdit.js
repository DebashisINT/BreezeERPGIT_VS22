validateInvoiceNumber = function () {
    var invoiceNumber = $("#SearchInv").val();
    if(invoiceNumber==""){
        return;
    }
    var jsonData = {};
    jsonData.invoiceNumber = invoiceNumber;
    $.ajax({
        type: "POST",
        url: "PosSalesinvoiceAdminEdit.aspx/GetInvoiceDetails",
        data: JSON.stringify(jsonData),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: "true",
        cache: "false",
        success: function (msg) {
            var obj = msg.d;
            if (obj.status == "Ok") {

                if (obj.invoiceId == "")
                    ShowMsg("This Invoice Number Does not exists");
                else
                    window.location.href = 'PosSalesinvoiceAdminEdit.aspx?id=' + obj.invoiceId;

            } else {
                console.log(obj.Msg);
            }
        },
        Error: function (x, e) { 
        }
    });


}

ShowMsg = function (msg) {
    jAlert(msg);
}

UpdateClientClick = function () {
    if ($("#UpdateField").val() == "PaymnetDetails") {
        SelectAllData();
    }
}
 

$(document).ready(function () {
    hideAllcontrol();
    $('#UpdateField').change(function () {
        var UpdatedField = $(this).val();
        hideAllcontrol();
        if (UpdatedField == "docNo") {
            $("#divInvoiceNumber").show();
        }
        else if (UpdatedField == "PaymnetDetails") {
            $("#divPaymentDetails").show();
        }

    });
});

hideAllcontrol = function () {
    $("#divInvoiceNumber").hide();
    $("#divPaymentDetails").hide();

}

SearchManualReceipt = function () {

    cManualReceipt.PerformCallback('validateReceiptNumber');

}

ShowManualReceiptPopup = function () {
    cManualReceipt.Show();
}