function gridRowclick(s, e) {
    $('#grid').find('tr').removeClass('rowActive');
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
    $("#hdnAction").val("Add");
})


function SaveClick() {
    //$("#hdnAction").val("Add");
    $("#hdnHSNSACType").val("HSN");

    grid.PerformCallback("Save");


}

function LastCall(s, e) {

    if (grid.cpOutput != null && grid.cpOutput != "") {
        jAlert(grid.cpOutput, 'Alert');
        $("#hdnAction").val("Add");
        $("#hdnID").val("");
    }
}


function OnEdit(id) {
    var obj = {};
    obj.id = id;

    $.ajax({
        type: "POST",
        url: "TaxExceptionRules.aspx/EdtExceptionRule",
        data: JSON.stringify(obj),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {
            console.log(msg);
            var obj = msg.d;
            if (obj.APPLICABLE_FROMDATE != null) {
                cdtFromdate.SetDate(new Date(parseInt(obj.APPLICABLE_FROMDATE.substr(6))));
            }
            if (obj.APPLICABLE_TODATE != null) {
                cdtTodate.SetDate(new Date(parseInt(obj.APPLICABLE_TODATE.substr(6))));
            }

            $("#dllBasedOn").val(obj.BASED_ON);
            $("#dllEntityType").val(obj.ENTITY_TYPE);
            $("#ddlOperator").val(obj.OPERATOR);
            ctxtVoucherAmount.SetText(obj.CRITERIA);


            CcmbCGST.SetValue(obj.INPUT_CGST_TAXRATESID);
            CcmbIGST.SetValue(obj.INPUT_IGST_TAXRATESID);
            CcmbSGST.SetValue(obj.INPUT_SGST_TAXRATESID);
            CcmbUTGST.SetValue(obj.INPUT_UTGST_TAXRATESID);


            CcmbSaleCGST.SetValue(obj.OUTPUT_CGST_TAXRATESID);
            CcmbSaleSGST.SetValue(obj.OUTPUT_SGST_TAXRATESID);
            CcmbSaleUTGST.SetValue(obj.OUTPUT_UTGST_TAXRATESID);
            CcmbSaleIGST.SetValue(obj.OUTPUT_IGST_TAXRATESID);
            $("#hdnID").val(id);
            $("#hdnAction").val("Update");

        }
    });
}

function DeleteRow(id) {
    var obj = {};
    obj.id = id;

    $.ajax({
        type: "POST",
        url: "TaxExceptionRules.aspx/DeleteExceptionRule",
        data: JSON.stringify(obj),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {
            jAlert('Deleted Successfully', 'Alert');
            grid.Refresh();
        }
    });
}