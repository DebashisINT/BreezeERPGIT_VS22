validateSaleorderNumber = function () {

    var saleordernumber = $("#inp_saleorder").val();

    // alert(saleordernumber);

    if (saleordernumber == "") {
        $("#dvusersd").attr('style', 'display:none;');
        return;
    }


    var jsonData = {};
    jsonData.saleordernumber = saleordernumber;
    $.ajax({
        type: "POST",
        url: "SalesOrderSpecialEdit.aspx/SaleorderDetails",
        data: JSON.stringify(jsonData),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: "true",
        cache: "false",
        success: function (msg) {
            var obj = msg.d;
            if (obj.status == "Ok") {

                if (obj.Oredernumber == "") {
                    jAlert("This Sale Oreder Number Does not exists");
                    $("#inp_saleorder").val('');

                    $("#dvusersd").attr('style','display:none;');
                    cuseridpanel.PerformCallback('Userbind~' + 0);

                 
                }
                else
                    $("#dvusersd").attr('style', 'display:block;');
                    cuseridpanel.PerformCallback('Userbind~' + obj.Oredernumber);
              

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


UpdateSaleorder = function () {

    var key = griduserloginLookup.GetGridView().GetRowKey(griduserloginLookup.GetGridView().GetFocusedRowIndex());
    // alert(key);
    var userid = key;
    var ordernumber = $("#inp_saleorder").val();

    if (ordernumber == "" || userid == "") {
        return;
    }
    var jsonData = {};
    jsonData.ordernumber = ordernumber;
    jsonData.userId = userid;
    $.ajax({
        type: "POST",
        url: "SalesOrderSpecialEdit.aspx/ChangeSaleOrderInvoice",
        data: JSON.stringify(jsonData),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: "true",
        cache: "false",
        success: function (msg) {
            var obj = msg.d;
            if (obj.status == "Ok") {

                if (obj.invoiceId == "")

                    jAlert("Successfully Sale order User Changed");

                else

                    ///window.location.href = 'PosSalesinvoiceAdminEdit.aspx?id=' + obj.invoiceId;

                    cuseridpanel.PerformCallback('Userbind~' + obj.invoiceId);

            }

            else {
                console.log(obj.Msg);
            }
        },
        Error: function (x, e) {
        }
    });

}