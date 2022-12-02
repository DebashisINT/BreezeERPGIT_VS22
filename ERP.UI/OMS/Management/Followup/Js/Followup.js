var canCallBack = true;
ModuleNameForHelp = "PaymentFollowup";



function cmbMainBranchChange(s, e) {

    var OtherDetails = {}
    OtherDetails.BranchId = ccmbMainUnit.GetValue();
    $.ajax({
        type: "POST",
        url: "Followup.aspx/GetAllDetailsByBranch",
        data: JSON.stringify(OtherDetails),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var returnObject = msg.d;
             
            SetDataSourceOnComboBox(ccmbSubunit, returnObject);
            
        }
    });
     
}

$(document).ready(function () {
    $('[data-toggle="tooltip"]').tooltip();
    $('[data-toggle="popover"]').popover({ html: true });
});


function AllControlInitilize() {
    if (canCallBack) {
        canCallBack = false;
        ccmbMainUnit.Focus();
        ccmbMainUnit.ShowDropDown();
        cerrorDD.PerformCallback();
        
    }
}


function SetDataSourceOnComboBox(ControlObject, Source) {
    ControlObject.ClearItems();
    for (var count = 0; count < Source.length; count++) {
        ControlObject.AddItem(Source[count].Name, Source[count].Id);
    }
    ControlObject.SetSelectedIndex(0);
}


function onError() {
    alert('error');
}


function CustomerButnClick(s, e) {
    $('#CustModel').modal('show');
}


function CustomerKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
        $('#CustModel').modal('show');
    }
}



function Customerkeydown(e) {

    var OtherDetails = {}
    OtherDetails.SearchKey = $("#txtCustSearch").val();

    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var HeaderCaption = [];
        HeaderCaption.push("Customer Name");
        HeaderCaption.push("Unique Id");
        HeaderCaption.push("Address");
        if ($("#txtCustSearch").val() != '') {
            callonServer("../Activities/Services/Master.asmx/GetEntity", OtherDetails, "CustomerTable", HeaderCaption, "customerIndex", "SetCustomer");
        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[customerindex=0]"))
            $("input[customerindex=0]").focus();
    }

}




function ValueSelected(e, indexName) {
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var Id = e.target.parentElement.parentElement.cells[0].innerText;
        var name = e.target.parentElement.parentElement.cells[1].children[0].value;
        if (Id) { 
                SetCustomer(Id, name);
        }

    }

    else if (e.code == "ArrowDown") {
        thisindex = parseFloat(e.target.getAttribute('customerIndex'));
        thisindex++;
        if (thisindex < 10)
            $("input[customerIndex=" + thisindex + "]").focus();
    }
    else if (e.code == "ArrowUp") {
        thisindex = parseFloat(e.target.getAttribute('customerIndex'));
        thisindex--;
        if (thisindex > -1)
            $("input[customerIndex=" + thisindex + "]").focus();
        else {
            
                $('#txtCustSearch').focus();
        }
    }

}



function SetCustomer(Id, Name) {
    if (Id) {
        $('#CustModel').modal('hide');
        ctxtCustName.SetText(Name);

        GetObjectID('hdnCustomerId').value = Id;
    }
}



function setBranch() {
    if (ccmbSubunit.GetValue() == "0") {
        var BranchList = "";
        for (var i = 0; i < ccmbSubunit.GetItemCount() ; i++) {
            BranchList = BranchList + ',' + ccmbSubunit.GetItem(i).value
        }
        BranchList = BranchList.substr(1, BranchList.length);
        GetObjectID('hdBranchList').value = BranchList;
    }
    else {
        GetObjectID('hdBranchList').value = ccmbSubunit.GetValue();
    }
}



function GenerateFollowup() {

    if (ccmbSubunit.GetText() == "") {
        jAlert('Please Select Unit First.');
        return;
    } else if (!cchkShowAll.GetChecked() && ctxtCustName.GetText() == "") {
        jAlert('Please Select Customer First.');
        return;
    }



    setBranch();
    cGrid.PerformCallback();
  
}


function showAllChkChange() {
    if (cchkShowAll.GetChecked()) {
        ctxtCustName.SetText('');
        GetObjectID('hdnCustomerId').value = '';
        ctxtCustName.SetEnabled(false);
    } else {
        ctxtCustName.SetEnabled(true);
    }
}



function OnView(key) {
     
    var url = 'FollowupDetails.aspx?CustId=' + key + '&&BranchId=' + GetObjectID('hdBranchList').value + '&&FromDt=' + cFormDate.GetDate().format('yyyy-MM-dd') + '&&ToDt=' + ctoDate.GetDate().format('yyyy-MM-dd');
    cDetailPopup.SetContentUrl(url);
    cDetailPopup.SetHeaderText('Follow-up Detail');
    cDetailPopup.SetWidth(window.screen.width - 50);
    cDetailPopup.SetHeight(433);
    cDetailPopup.Show();

}


function OnWaitingGridKeyPress(e) {

    if (e.code == "Enter") {
        var index = cGrid.GetFocusedRowIndex(); 
        cGrid.GetRow(index).children[6].children[0].onclick();
    }

}

function popupClosing() {
    cGrid.Focus();
    if ($(".DocumentDetails").hasClass('on')) {
        var e = cGrid.GetFocusedRowIndex();

        var str = cGrid.GetRow(e).children[0].innerText;
        $('#divProductDetails').hide();
        if (str.trim() == "") {
            $(".DocumentDetails").animate({ right: (($(".DocumentDetails").width() + 50) * -1) + 'px' });
            $(".DocumentDetails").addClass('on');
            $('#DocumentDetails').html('');
            return;
        }
        var dateStr = str.split('-')[2] + '-' + str.split('-')[1] + '-' + str.split('-')[0]
        var CustId = cGrid.GetRow(e).children[2].innerHTML;


        var OtherDetails = {}
        OtherDetails.dateStr = dateStr;
        OtherDetails.CustId = CustId;

        $.ajax({
            type: "POST",
            url: "Followup.aspx/GetDocumentDetailsForCustomer",
            data: JSON.stringify(OtherDetails),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var returnObject = msg.d.documentDetails;
                detailObj = msg.d.documentProductDetails;

                var htmlString = "<table class='tablStyl'><thead><tr><th>Branch</th><th>Doc No.</th><th>Date</th><th>Total Amt.</th><th>Unpaid Amt.</th><th>Adj. Amt.</th><th>Followed By</th><th>Date</th><th>Details</th></tr></thead><tbody style=' background: #7b7d73;'>";
                for (var i = 0; i < returnObject.length; i++) {
                    htmlString = htmlString + '<tr style="color: white;"><td>' + returnObject[i].Branch + '</td><td>' + returnObject[i].DocNo + '</td><td>' + returnObject[i].DocDate + '</td><td>' + returnObject[i].TotalAmount + '</td><td>' + returnObject[i].UnPaidAmount + '</td><td>' + returnObject[i].adjAmount + '</td><td>' + returnObject[i].followedByname + '</td><td>' + returnObject[i].FollowedOn + '</td><td><span style="font-size: 17px;    cursor: pointer;    color: #f30000;" onclick=showdetails("' + returnObject[i].docId + returnObject[i].DocType + '")><i class="fa fa-angle-down"></i></span></td></tr>';

                    //Create Detail
                    var detObj = $.grep(detailObj, function (e) { return e.docId == returnObject[i].docId && e.DocType == returnObject[i].DocType && e.Prod != ""; })

                    var DetailHtml = '<table class="tablStyl" style="width: 100%;"><thead><tr><th>Product</th><th>Qty</th><th>Price</th><th>Tax</th><th>Amount</th></tr></thead>';
                    for (var j = 0; j < detObj.length; j++) {
                        DetailHtml = DetailHtml + '<tbody style=" background: #282f13;"><tr class="DetailProductRowFont"><td>' + detObj[j].Prod + '</td><td>' + detObj[j].qty + '</td><td>' + detObj[j].Price + '</td><td>' + detObj[j].tax + '</td><td>' + detObj[j].totAmt + '</td></tr></tbody>';
                    }
                    DetailHtml = DetailHtml + ' </table>';




                    htmlString = htmlString + '<tr id="' + returnObject[i].docId + returnObject[i].DocType + '" style="display:none" class="DetailProductRow"><td></td> <td colspan=8><div style="display:none" class="DetailProductRowDiv" id="div' + returnObject[i].docId + returnObject[i].DocType + '">' + DetailHtml + '</div></td></tr>'
                }
                htmlString = htmlString + '</tbody></table>';
                 
                $('#DocumentDetails').html(htmlString);
                 
            }
        });


    }
}


function OnFolloupAdd(key) {
    var showZeroBal = 0
    if (cchkzeroBal.GetChecked()) {
        showZeroBal = 1;
    }

    var url = 'FollowupEntry.aspx?custId=' + key + '&&BranchId=' + GetObjectID('hdBranchList').value + '&&FromDt=' + cFormDate.GetDate().format('yyyy-MM-dd') + '&&ToDt=' + ctoDate.GetDate().format('yyyy-MM-dd') + '&&showZeroBal=' + showZeroBal + '&&CustName=' + ctxtCustName.GetText();
    cDetailPopup.SetContentUrl(url);
    cDetailPopup.SetHeaderText('Follow-up Add/Edit');
    cDetailPopup.SetWidth(window.screen.width - 50);
    cDetailPopup.SetHeight(window.screen.height-200);
    cDetailPopup.Show();
    console.log(url);
}



function View_Customer(keyValue) {
     
    var url = '/OMS/management/master/View/ViewCustomer.html?ActiveTabName=Correspondence&&id=' + keyValue;
    cDetailPopup.SetContentUrl(url);
    cDetailPopup.SetHeaderText('Customer View');
    cDetailPopup.SetWidth(window.screen.width - 50);
    cDetailPopup.SetHeight(window.screen.height - 200);
    cDetailPopup.Show();
}



function FocusedRowDoubleClick(s, e) { 
    var str = cGrid.GetRow(e.visibleIndex).children[0].innerText;
    $('#divProductDetails').hide();
    if (str.trim() == "") {
        $(".DocumentDetails").animate({ right: (($(".DocumentDetails").width() + 50) * -1) + 'px' });
        $(".DocumentDetails").addClass('on');
        $('#DocumentDetails').html('');
        return;
    }
    var dateStr = str.split('-')[2].trim() + '-' + str.split('-')[1].trim() + '-' + str.split('-')[0].trim();
    var CustId = cGrid.GetRow(e.visibleIndex).children[2].innerHTML;


    var OtherDetails = {}
    OtherDetails.dateStr = dateStr;
    OtherDetails.CustId = CustId;
    loadChild(OtherDetails);
}

function loadDrawer(e) {
    var str = cGrid.GetRow(e).children[0].innerText;
    $('#divProductDetails').hide();
    if (str.trim() == "") {
        $(".DocumentDetails").animate({ right: (($(".DocumentDetails").width() + 50) * -1) + 'px' });
        $(".DocumentDetails").addClass('on');
        $('#DocumentDetails').html('');
        return;
    }
    var dateStr = str.split('-')[2].trim() + '-' + str.split('-')[1].trim() + '-' + str.split('-')[0].trim();
    var CustId = cGrid.GetRow(e).children[2].innerHTML;


    var OtherDetails = {}
    OtherDetails.dateStr = dateStr;
    OtherDetails.CustId = CustId;
    loadChild(OtherDetails);

}



function loadChild(OtherDetails) {
    $.ajax({
        type: "POST",
        url: "Followup.aspx/GetDocumentDetailsForCustomer",
        data: JSON.stringify(OtherDetails),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var returnObject = msg.d.documentDetails;
            detailObj = msg.d.documentProductDetails;

            var htmlString = "<table class='tablStyl'><thead><tr><th>Branch</th><th>Doc No.</th><th>Due Date</th><th>Total Amt.</th><th>Unpaid Amt.</th><th>Adj. Amt.</th><th>Followed By</th><th>Date</th><th>Details</th></tr></thead><tbody style=' background: #7b7d73;'>";
            for (var i = 0; i < returnObject.length; i++) {
                htmlString = htmlString + '<tr style="color: white;"><td>' + returnObject[i].Branch + '</td><td>' + returnObject[i].DocNo + '</td><td>' + returnObject[i].DocDate + '</td><td>' + returnObject[i].TotalAmount + '</td><td>' + returnObject[i].UnPaidAmount + '</td><td>' + returnObject[i].adjAmount + '</td><td>' + returnObject[i].followedByname + '</td><td>' + returnObject[i].FollowedOn + '</td><td><span style="font-size: 17px;    cursor: pointer;    color: #f30000;" onclick=showdetails("' + returnObject[i].docId + returnObject[i].DocType + '")><i class="fa fa-angle-down"></i></span></td></tr>';

                //Create Detail
                var detObj = $.grep(detailObj, function (e) { return e.docId == returnObject[i].docId && e.DocType == returnObject[i].DocType && e.Prod != ""; })

                var DetailHtml = '<table class="tablStyl" style="width: 100%;"><thead><tr><th>Product</th><th>Qty</th><th>Price</th><th>Tax</th><th>Amount</th></tr></thead>';
                for (var j = 0; j < detObj.length; j++) {
                    DetailHtml = DetailHtml + '<tbody style=" background: #282f13;"><tr class="DetailProductRowFont"><td>' + detObj[j].Prod + '</td><td>' + detObj[j].qty + '</td><td>' + detObj[j].Price + '</td><td>' + detObj[j].tax + '</td><td>' + detObj[j].totAmt + '</td></tr></tbody>';
                }
                DetailHtml = DetailHtml + ' </table>';




                htmlString = htmlString + '<tr id="' + returnObject[i].docId + returnObject[i].DocType + '" style="display:none" class="DetailProductRow"><td></td> <td colspan=8><div style="display:none" class="DetailProductRowDiv" id="div' + returnObject[i].docId + returnObject[i].DocType + '">' + DetailHtml + '</div></td></tr>'
            }
            htmlString = htmlString + '</tbody></table>';

            $('#MainDivDetails').show();
            $('#DocumentDetails').html(htmlString);

            $(".DocumentDetails").css({ 'right': (($(".DocumentDetails").width() + 50) * -1) + 'px' })
            $(".DocumentDetails").animate({ right: '0px' });
            $(".DocumentDetails").removeClass('on');
            var holdyDiv = $('<div></div>').attr('class', 'layer');
            holdyDiv.appendTo('body');
        }
    });

}

function SlideChange() {
    if ($(".DocumentDetails").hasClass('on')) {
        $(".DocumentDetails").animate({ right: '0px' });
        $(".DocumentDetails").removeClass('on');
        var holdyDiv = $('<div></div>').attr('class', 'layer');
        holdyDiv.appendTo('body');
    } else {
        $(".DocumentDetails").animate({ right: (($(".DocumentDetails").width() + 50) * -1) + 'px' });
        $(".DocumentDetails").addClass('on');
        $(".layer").hide();
    }
}



function showdetails(RowId) {
    //var detObj = $.grep(detailObj, function (e) { return e.docId == docId && e.DocType == docType && e.Prod != ""; })

    //var DetailHtml = '<table class="tablStyl"><thead><tr><th>Product</th><th>Qty</th><th>Price</th><th>Tax</th><th>Amount</th></tr></thead>';
    //for (var i = 0; i < detObj.length; i++) {
    //    DetailHtml = DetailHtml + '<tbody><tr><td>' + detObj[i].Prod + '</td><td>' + detObj[i].qty + '</td><td>' + detObj[i].Price + '</td><td>' + detObj[i].tax + '</td><td>' + detObj[i].totAmt + '</td></tr></tbody>';
    //}
    //DetailHtml = DetailHtml + ' </table>';
    //console.log(RowId);
   // $('#'+RowId).show();
    //$('#divProductDetails').html(DetailHtml);
    $('.DetailProductRow').hide();
    $('.DetailProductRowDiv').hide();
    $('#' + RowId).slideDown();
    $('#div' + RowId).slideDown();

}