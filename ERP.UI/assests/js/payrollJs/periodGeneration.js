
var checksum = true;
function SaveActivePrev() {
    checksum = true;
    var url = '/PeriodGeneration/setActivePrevNext/';
    var PayClassID = document.getElementById('hdnPayClassID').value;

    $.ajax({
        type: "POST",
        url: url,
        data: JSON.stringify({ 'ActionType': 'activePrev', 'PayClassID': PayClassID }),
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            console.log(response);
            if (response.response_msg == "Success") {

                PeriodgridView.Refresh();
            }
            else {
                jAlert(response.response_msg);
            }
        },
        error: function (response) {
            console.log(response);
        }
    });


}

function SaveActiveNxt() {
    checksum = true;
    var url = '/PeriodGeneration/setActivePrevNext/';
    var PayClassID = document.getElementById('hdnPayClassID').value;

    $.ajax({
        type: "POST",
        url: url,
        data: JSON.stringify({ 'ActionType': 'activeNxt', 'PayClassID': PayClassID }),
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            console.log(response);

            if (response.response_msg == "Success") {

                PeriodgridView.Refresh();
            }
            else {
                jAlert(response.response_msg);
            }
        },
        error: function (response) {
            console.log(response);
        }
    });
}

    function btnPayClass_KeyDown(s, e) {
        if (e.htmlEvent.key == "Enter") {
            s.OnButtonClick(0);
        }
    }

function btnPayClass_Click(s, e) {
    checksum = true;
    var searchkey = btnPayClassName.GetText();
    if (searchkey == '' || searchkey==null)
        BindPayClass('');
    else
    {
        BindPayClass(searchkey);
    }
    setTimeout(function () { $("#txtPayClass").focus(); }, 500);
    $('#PayClassModel').modal('show');
}

function PayClasskeydown(e) {
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        if ($("#txtPayClass").val() != '') {
            BindPayClass($("#txtPayClass").val());
        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[PayClassIndex=0]"))
            $("input[PayClassIndex=0]").focus();
    }
}
function BindPayClass(SearchKey) {
    var OtherDetails = {}
    OtherDetails.SearchKey = SearchKey;

    var HeaderCaption = [];
    HeaderCaption.push("Pay Class Name");
    HeaderCaption.push("Period From");
    HeaderCaption.push("Period To");

    callonServerScroll("../Models/p_WebServiceList.asmx/GetPayClassList", OtherDetails, "PayClassTable", HeaderCaption, "PayClassIndex", "SetPayClass");
}

function SetPayClass(Id, Name) {
    var PayClassID = Id;
    var PayClassName = Name;

    if (PayClassID != "") {
        $('#PayClassModel').modal('hide');
        btnPayClassName.SetText(PayClassName);
        document.getElementById('hdnPayClassID').value = Id;
        PeriodgridView.Refresh();
        PeriodgridView.Refresh();
        var ParentObj = $.grep(mycallonServerObj, function (e) { return e.PayrollClassID == Id })
        $("#txt_frmDt").val(ParentObj[0]["PeriodFrom"]);
        $("#txt_toDt").val(ParentObj[0]["PeriodTo"]);
        $("#txt_actvmnth").val('@ViewBag.Activated');
        //for(var i=0;i<PeriodgridView.GetVisibleRowsOnPage();i++)
        //{
        //    console.log(PeriodgridView.GetRow(i).children[1].innerText)
        //}


    }
}

function ValueSelected(e, indexName) {
    if (e.code == "Enter") {
        var Id = e.target.parentElement.parentElement.cells[0].innerText;
        var name = e.target.parentElement.parentElement.cells[1].children[0].value;
        if (Id) {
            if (indexName == "PayClassIndex") {
                SetPayClass(Id, name);
            }
        }
    }

    else if (e.code == "ArrowDown") {
        thisindex = parseFloat(e.target.getAttribute(indexName));
        thisindex++;
        if (thisindex < 10)
            $("input[" + indexName + "=" + thisindex + "]").focus();
    }
    else if (e.code == "ArrowUp") {
        thisindex = parseFloat(e.target.getAttribute(indexName));
        thisindex--;
        if (thisindex > -1)
            $("input[" + indexName + "=" + thisindex + "]").focus();
        else {
            if (indexName == "PayClassIndex")
                $('#txtPayClass').focus();
        }
    }

}
function OnStartCallback(s, e) {
    e.customArgs["PayClassID"] = document.getElementById('hdnPayClassID').value;
}
    
function OnEndCallback()
{
    for(var i=0;i<PeriodgridView.GetVisibleRowsOnPage();i++)
    {
        if (PeriodgridView.GetRow(i).children[1].innerText == "True" && checksum)
        {
            //PeriodgridView.GetRow(1).children[0].children[0].className.includes("CheckBoxchecked")
            console.log(PeriodgridView.GetRow(i).children[2].innerText  )
            $("#txt_actvmnth").val(PeriodgridView.GetRow(i).children[2].innerText);
            checksum = false;
        }
                
    }
}
