
$(document).ready(function () {
    //$('select[multiple]').multiselect();
})
$(function () {
    $('select[multiple]').multiselect({
        columns: 1,
        placeholder: 'Select ',
        search: true,
        searchOptions: {
            'default': 'Search'
        },
        selectAll: true
    });
    $('#MainAccountCode, #ddlSubaccdount, #ddlPostingType').select2();
});

function SaveSetting() {
    var action = "";

    if ($("#hdnID").val() == "") {
        action = "Add";
    }
    else {
        action = "update";
    }

    var payHeads = $('#ddlPayhead').val();
    var payheadids = "";
    if (payHeads == null) {
        jAlert('You must select atleast one Payhead to proceed.', 'Alert');
    }
    else {
        payheadids = payHeads.join();


        var MainAccountCode = $("#MainAccountCode").val();
        var SubAccountCode = $("#ddlSubaccdount").val();
        var PostingType = $("#ddlPostingType").val();
        var obj = {};

        obj.Action = action;
        obj.AccountMapCode = $("#hdnID").val();
        obj.AccountCode = MainAccountCode;
        obj.Subaacount = SubAccountCode;
        obj.PostingType = PostingType;
        obj.Payheadids = payheadids;


        $.ajax({
            type: "POST",
            url: "/PayrollSettings/Save",
            data: JSON.stringify(obj),
            async: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                console.log(response);
                grid.Refresh();
            },
            error: function (response) {
                jAlert("Please try again later");
            }
        });
    }
}

function gridclick(s, e) {
    $('#gridcrmAccount').find('tr').removeClass('rowActive');
    $('.floatedBtnArea').removeClass('insideGrid');
    $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea').addClass('insideGrid');
    $(s.GetRow(e.visibleIndex)).addClass('rowActive');
    setTimeout(function () {
        //alert('delay');
        var lists = $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a');
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

function ClearData() {
    $("#hdnID").val("");
    var arr = [];
    $("#ddlPayhead").find("option").each(function (e) {

        var o = {};
        var id = $(this).val();
        var text = $(this).text();

        o.name = text;
        o.value = id;
        o.checked = false;

        arr.push(o);
    });

    $('#ddlPayhead').multiselect('loadOptions', arr);
    $("#MainAccountCode").prop("selectedIndex", 0).val();
    $("#ddlSubaccdount").prop("selectedIndex", 0).val();
    $("#ddlPostingType").prop("selectedIndex", 0).val();
}

function EditClick(id) {
    $("#hdnID").val(id);
    var obj = {};
    obj.Action = "";
    obj.AccountMapCode = $("#hdnID").val();
    obj.AccountCode = "";
    obj.Subaacount = "";
    obj.PostingType = "";
    obj.Payheadids = "";


    $.ajax({
        type: "POST",
        url: "/PayrollSettings/Edit",
        data: JSON.stringify(obj),
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {

            var poayheadids = response.Payheadids.split(',');
            console.log(poayheadids);
            var arr = [];
            $("#ddlPayhead").find("option").each(function (e) {

                var o = {};
                var id = $(this).val();
                var text = $(this).text();

                if (poayheadids.includes(id)) {
                    o.name = text;
                    o.value = id;
                    o.checked = true;
                }
                else {
                    o.name = text;
                    o.value = id;
                    o.checked = false;
                }

                arr.push(o);
            });

            $('#ddlPayhead').multiselect('loadOptions', arr);
            $("#MainAccountCode").val(response.AccountCode);
            $("#ddlSubaccdount").val(response.Subaacount);
            $("#ddlPostingType").val(response.PostingType);

            grid.Refresh();
        },
        error: function (response) {
            jAlert("Please try again later");
        }
    });


}
function DeleteClick(id) {
    jConfirm("Confirm Delete?", "Confirmation Dialog", function (ret) {
        if (ret == true) {
            var obj = {};
            obj.Action = "";
            obj.AccountMapCode = id;
            obj.AccountCode = "";
            obj.Subaacount = "";
            obj.PostingType = "";
            obj.Payheadids = "";


            $.ajax({
                type: "POST",
                url: "/PayrollSettings/DeleteRow",
                data: JSON.stringify(obj),
                async: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    jAlert('Deleted Successfully.', 'Alert', function () {
                        grid.Refresh();
                    });

                }
            });
        }
    });
}
$(function () {
    $('#ddlExport').on('change', function () {
        if ($("#ddlExport option:selected").index() > 0) {
            var selectedValue = $(this).val();
            $('#ddlExport').prop("selectedIndex", 0);

            var url = $('#hdnExportLink').val();
            window.location.href = url.replace("_type_", selectedValue);
        }
    });
});
