validateInvoiceNumber = function () {
    var invoiceNumber = $("#SearchInv").val();
    if (invoiceNumber == "") {
        return;
    }
    var jsonData = {};
    jsonData.invoiceNumber = invoiceNumber;
    $.ajax({
        type: "POST",
        url: "Financer-Specialedit.aspx/GetInvoiceDetails",
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

                    ///window.location.href = 'PosSalesinvoiceAdminEdit.aspx?id=' + obj.invoiceId;
                    $("#dvfinancer").show();
                    $("#txtoldInvoicenumber").val(obj.invoceNumbr);
                    $("#id_invoice").val(obj.invoiceId);

                   cProductfinancerPanel.PerformCallback('Finacerbind~' + obj.invoiceId);

            } else {
                console.log(obj.Msg);
            }
        },
        Error: function (x, e) {
        }
    });


}



ShowManualReceiptPopup = function () {
    cManualReceipt.Show();
}


UpdateManualReceipt = function () 
{

    var key = gridfinancerLookup.GetGridView().GetRowKey(gridfinancerLookup.GetGridView().GetFocusedRowIndex());
   // alert(key);
    var financer = key;
    var invoiceid = $("#id_invoice").val();
  //  alert(financer + ' ' + invoiceid)

    if (invoiceid == "" || financer == "") {
        return;
    }
    var jsonData = {};
    jsonData.invoiceid = invoiceid;
    jsonData.financerid = financer;
    $.ajax({
        type: "POST",
        url: "Financer-Specialedit.aspx/ChangeFinancerInvoice",
        data: JSON.stringify(jsonData),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: "true",
        cache: "false",
        success: function (msg) {
            var obj = msg.d;
            if (obj.status == "Ok") {

                if (obj.invoiceId == "")

                    ShowMsg("Successfully Financer Changed");

                else

                ///window.location.href = 'PosSalesinvoiceAdminEdit.aspx?id=' + obj.invoiceId;
                $("#dvfinancer").show();
                $("#txtoldInvoicenumber").val(obj.invoceNumbr);
                cProductfinancerPanel.PerformCallback('Finacerbind~' + obj.invoiceId);


            } else {
                console.log(obj.Msg);
            }
        },
        Error: function (x, e) {
        }
    });

}