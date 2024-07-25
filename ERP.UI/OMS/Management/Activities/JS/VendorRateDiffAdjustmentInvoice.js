//====================================================Revision History =========================================================================
//1.0   v2.0.44	    Priti	    24-07-2024	    0027592: Auto Calculation Required for the following Vendor  adjustment modules
//====================================================End Revision History=====================================================================
/*****************
Global variable*/

var ReceiptList = [];
var globalRowindex = 0;
var DocumentList = [];
var canCallBack = true;
var PickedDocument = [];
var saveNewOrExit = '';
var alertShow = false;
var AdvanceType = '';
var GridBranchId = '';

$(document).ready(function () {
    // GridAddnewRow();


    $('#AdvanceModel').on('shown.bs.modal', function () {
        //if ($("input[receiptIndex=0]"))
        //    $("input[receiptIndex=0]").focus();
        
        $('#txtAdvanceSearch').focus();
    })

    $('#DocumentModel').on('shown.bs.modal', function () {
        //if ($("input[DocumentIndex=0]"))
        //    $("input[DocumentIndex=0]").focus();
        $('#txtGridDocSearch').focus();
        
    })

    $('#VendModel').on('shown.bs.modal', function () {
        $('#txtVendSearch').focus();
    })


});
function VendorButnClick(s, e) {
    if (cCmbScheme.GetValue() == "0~1~0") {
        cCmbScheme.Focus();

    }
    else if ($("#ddlBranch").val() == "0") {
        $("#ddlBranch").focus();

    }
    else {
        $('#VendModel').modal('show');
    }
}
function VendorKeyDown(s, e) {


    if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {

        if (cCmbScheme.GetValue() == "0~1~0") {
            jAlert("Please Select Numbering Scheme.", "Alert", function () {
                cCmbScheme.Focus();
            });
        }
        else if ($("#ddlBranch").val() == "0") {
            jAlert("Please Select Branch.", "Alert", function () {
                $("#ddlBranch").focus();
            });
        }
        else {
            $('#VendModel').modal('show');
        }
    }
}

function DocumentNumberBtnClick(s, e) {
    //if (GetObjectID('hdnVendorId').value) {
    //    $('#AdvanceModel').modal('show');
    //    var htmlScript = "<table class='dynamicPopupTbl' border='1' width='100%'> <tr class='HeaderStyle'>  <th>Rate Difference Number</th> <th>Rate Difference Date</th> <th>Amount</th> <th>Balance</th></tr>";
    //    for (var rp = 0; rp < ReceiptList.length; rp++) {
    //        htmlScript += "<tr> <td><input readonly onclick='ReceiptonClick(" + ReceiptList[rp].ArId + ")' type='text' style='background-color: #3399520a;'receiptIndex=" + rp + " onfocus='ReceiptGetFocus(event)'  onblur='ReceiptlostFocus(event)' onkeydown=ReceiptSelected(event," + ReceiptList[rp].ArId + ") width='100%' readonly value='" + ReceiptList[rp].docNo + "'/></td><td  onclick='ReceiptonClick(" + ReceiptList[rp].ArId + ")'>" + ReceiptList[rp].docDt + "</td><td onclick='ReceiptonClick(" + ReceiptList[rp].ArId + ")'>" + GetTwodecimalValue(ReceiptList[rp].ActAmt) + "</td><td onclick='ReceiptonClick(" + ReceiptList[rp].ArId + ")'>" + GetTwodecimalValue(ReceiptList[rp].avlAmt) + "</td></tr>";
    //    }
    //    htmlScript += ' </table>';
    //    document.getElementById('AdvPayDocTbl').innerHTML = htmlScript;

    //} else {
    //    jAlert("Please select Vendor.", "Alert", function () { ctxtVendName.Focus(); });
    //}

    if (GetObjectID('hdnVendorId').value) {
        var newobj = [];
        $('#AdvanceModel').modal('show');
        var htmlScript = "<table class='dynamicPopupTbl' border='1' width='100%'> <tr class='HeaderStyle'>  <th>Rate Difference Number</th> <th>Rate Difference Date</th> <th>Amount</th> <th>Balance</th></tr>";
       
        htmlScript += ' </table>';
        document.getElementById('AdvPayDocTbl').innerHTML = htmlScript;

        var receiptdetails = $.grep(ReceiptList, function (e) { return !PickedDocument.includes(e.ArId); });

        for (var i = 0; i < ReceiptList.length; i++) {
            var obj = {};

            obj.ArId = receiptdetails[i]["ArId"];
            obj.DocId = receiptdetails[i]["DocId"];
            obj.docNo = receiptdetails[i]["docNo"];
            obj.docDt = receiptdetails[i]["docDt"];
            obj.doctype = receiptdetails[i]["AdvType"];
            obj.ActAmt = receiptdetails[i]["ActAmt"];
            obj.avlAmt = receiptdetails[i]["avlAmt"];
            obj.Cur = receiptdetails[i]["Cur"];
            obj.CurRate = receiptdetails[i]["CurRate"];
            newobj.push(obj);

        }
        document.getElementById('AdvPayDocTbl').innerHTML = MakeTableFromArrayForAdvance(newobj, "SetDocForAdvance", "receiptIndex");
        $('#AdvanceModel').modal('show');

    } else {
        jAlert("Please select Vendor.", "Alert", function () { ctxtVendName.Focus(); });
    }
}
function MakeTableFromArrayForAdvance(myObj, onSelect, UniqueIndex) {
    var myObj = myObj;
    mycallonServerObj = myObj;
    var txt = '';
    var count = 0;
    
    txt = "<table class='dynamicPopupTbl' border='1' width='100%'> <tr class='HeaderStyle'>  <th>Rate Difference Number</th> <th>Rate Difference Date</th> <th>Amount</th> <th>Balance</th></tr>";

    for (x in myObj) {
        txt += "<tr onclick='RowclickDoc(event," + onSelect + ")'>";
        var PropertyCount = 0;

        for (key in myObj[0]) {

            if (PropertyCount == 0)
                txt += " <td class='hide'>" + myObj[x]["ArId"] + "</td>";
            else if (PropertyCount == 1)
                txt += " <td class='hide'>" + myObj[x]["DocId"] + "</td>";
            else if (PropertyCount == 4)
                txt += " <td class='hide'>" + myObj[x]["doctype"] + "</td>";
            else if (PropertyCount == 7)
                txt += " <td class='hide'>" + myObj[x]["Cur"] + "</td>";
            else if (PropertyCount == 8)
                txt += " <td class='hide'>" + myObj[x]["CurRate"] + "</td>";
            
            else if (PropertyCount == 5 || PropertyCount == 6)

                txt += " <td style='width:220px'><input onclick='PopupTextClickDoc(event," + onSelect + ")' type='text' style='background-color: #3399520a;'" + UniqueIndex + "='" + count + "'onfocus='ReceiptGetFocus(event)'  onblur='ReceiptlostFocus(event)' onkeydown=ReceiptSelected(event,'" + UniqueIndex.toString() + "',event.target) width='100%'  readonly  />" + GetTwodecimalValue(myObj[x][key]) + "</td>";

            else

                txt += " <td style='width:220px'><input onclick='PopupTextClickDoc(event," + onSelect + ")' type='text' style='background-color: #3399520a;'" + UniqueIndex + "='" + count + "'onfocus='ReceiptGetFocus(event)'  onblur='ReceiptlostFocus(event)' onkeydown=ReceiptSelected(event,'" + UniqueIndex.toString() + "',event.target) width='100%'  readonly  />" + myObj[x][key] + "</td>";

            PropertyCount++;
        }
        txt += "</tr>";
        count++;
    }
    txt += "</table>"

    return txt;
}
function PopupTextClickDoc(e, OnSelect) {
    var Id = e.target.parentElement.parentElement.children[1].innerText;
    var name = e.target.parentElement.parentElement.cells[2].children[0].value;
    OnSelect.call(this, Id, name, e.target.parentElement);
}
function SetDocForAdvance(Id, Name, e) {
    DeleteAllRows();
    var ArId = e.parentElement.cells[0].innerText;
    var docId = e.parentElement.cells[1].innerText;
    var docNo = e.parentElement.cells[2].innerText;
    var docDt = e.parentElement.cells[3].innerText;
    var doctype = e.parentElement.cells[4].innerText;
    var ActAmt = e.parentElement.cells[5].innerText;
    var avlAmt = e.parentElement.cells[6].innerText;
    var Cur = e.parentElement.cells[7].innerText;
    var CurRate = e.parentElement.cells[8].innerText;


    $('#AdvanceModel').modal('hide');
    cbtntxtDocNo.SetText(docNo);
    cDocAmt.SetValue(ActAmt);

    cExchRate.SetValue(CurRate);

    if (CurRate == 0)
        cBaseAmt.SetValue(ActAmt);
    else
        cBaseAmt.SetValue(ActAmt * CurRate);


    cOsAmt.SetValue(avlAmt);
    GetObjectID('hdAdvanceDocNo').value = docId;
    GetObjectID('hdAdjustmentType').value = doctype;
    GetObjectID('hdAdjustmentId').value = docId;
    showDocumentList();
    cRemarks.Focus();
}
function RowclickDoc(e, OnSelect) {
    if (e.target.type != "text") {
        var Id = e.target.parentElement.children[1].innerText;
        var name = e.target.parentElement.cells[2].children[0].value;
        OnSelect.call(this, Id, name, e.target);
    }
}
function AdvanceNewkeydown(e) {
    var newobj = [];
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var receiptdetails = $.grep(ReceiptList, function (e) { return !PickedDocument.includes(e.ArId); });
        var SearchObj = $("#txtAdvanceSearch").val();
        if (SearchObj != "") {
            for (var i = 0; i < receiptdetails.length; i++) {
                var obj = {};
                var NewCode = receiptdetails[i]["docNo"];
                if (NewCode.toUpperCase().includes(SearchObj.toUpperCase())) {

                    obj.ArId = receiptdetails[i]["ArId"];
                    obj.DocId = receiptdetails[i]["DocId"];
                    obj.docNo = receiptdetails[i]["docNo"];
                    obj.docDt = receiptdetails[i]["docDt"];
                    obj.doctype = receiptdetails[i]["AdvType"];
                    obj.ActAmt = receiptdetails[i]["ActAmt"];
                    obj.avlAmt = receiptdetails[i]["avlAmt"];

                    obj.Cur = receiptdetails[i]["Cur"];
                    obj.CurRate = receiptdetails[i]["CurRate"];

                    newobj.push(obj);
                }
            }
            document.getElementById('AdvPayDocTbl').innerHTML = "";
            document.getElementById('AdvPayDocTbl').innerHTML = MakeTableFromArrayForAdvance(newobj, "SetDocForAdvance", "receiptIndex");
        }
        else {
            document.getElementById('AdvPayDocTbl').innerHTML = "";
            document.getElementById('AdvPayDocTbl').innerHTML = MakeTableFromArrayForAdvance(DocObj, "SetDocForAdvance", "receiptIndex");
        }

        setTimeout(function () { document.getElementById("txtGridDocSearch").focus(); }, 500);
    }
    else if (e.code == "ArrowDown") {
        if ($("input[receiptIndex=0]"))
            $("input[receiptIndex=0]").focus();
    }
}



function DocumentNumberBtn(s, e) {
    if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
        DocumentNumberBtnClick();
    }
}

function ReceiptonClick(id) {
    PopulateReceiptDetails(id);
}


function ReceiptGetFocus(e) {
    e.target.parentElement.parentElement.className = "focusrow";
    e.target.style = "background: #0000ff3d";
}
function ReceiptlostFocus(e) {
    e.target.parentElement.parentElement.className = "";
    e.target.style = "background-color: #3399520a";
}
function ReceiptSelected(e, id) {
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        if (id) {
            PopulateReceiptDetails(id);
        }
    }

    else if (e.code == "ArrowDown") {
        thisindex = parseFloat(e.target.getAttribute('receiptIndex'));
        thisindex++;
        if (thisindex < 10)
            $("input[receiptIndex=" + thisindex + "]").focus();
    }
    else if (e.code == "ArrowUp") {
        thisindex = parseFloat(e.target.getAttribute('receiptIndex'));
        thisindex--;
        if (thisindex > -1)
            $("input[receiptIndex=" + thisindex + "]").focus();
    }

}

function PopulateReceiptDetails(receiptId) {
    DeleteAllRows();
    var receiptdetails = $.grep(ReceiptList, function (e) { return e.ArId == receiptId; });
    if (receiptdetails.length > 0) {
        $('#AdvanceModel').modal('hide');
        cbtntxtDocNo.SetText(receiptdetails[0].docNo);
        cDocAmt.SetValue(receiptdetails[0].ActAmt);
        cExchRate.SetValue(receiptdetails[0].CurRate);
        //Set Value in base Currency
        if (receiptdetails[0].CurRate == 0)
            cBaseAmt.SetValue(receiptdetails[0].ActAmt);
        else
            cBaseAmt.SetValue(receiptdetails[0].ActAmt * receiptdetails[0].CurRate);

        AdvanceType = receiptdetails[0].AdvType;
        cOsAmt.SetValue(receiptdetails[0].avlAmt);
        GetObjectID('hdAdvanceDocNo').value = receiptdetails[0].DocId;
        GetObjectID('hdAdjustmentType').value = AdvanceType;
        GetObjectID('hdAdjustmentId').value = receiptdetails[0].DocId;
        showDocumentList();
        cRemarks.Focus();
    }

}

function Vendorkeydown(e) {

    var OtherDetails = {}
    OtherDetails.SearchKey = $("#txtVendSearch").val();
    OtherDetails.type = 'DV';

    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var HeaderCaption = [];
        HeaderCaption.push("Vendor/Transporter Name");
        HeaderCaption.push("Unique Id");
        HeaderCaption.push("Type");

        if ($("#txtVendSearch").val() != '') {
            callonServer("Services/Master.asmx/GetVendorForVendorPayRec", OtherDetails, "VendorTable", HeaderCaption, "VendorIndex", "SetCustomer");
        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[VendorIndex=0]"))
            $("input[VendorIndex=0]").focus();
    }

}
function ValueSelected(e, indexName) {
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var Id = e.target.parentElement.parentElement.cells[0].innerText;
        var name = e.target.parentElement.parentElement.cells[1].children[0].value;
        if (Id) {
            if (indexName == "VendorIndex")
                SetCustomer(Id, name);
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
            if (indexName == "VendorIndex")
                $('#txtVendSearch').focus();
        }
    }

}
function SetCustomer(Id, Name) {
    if (Id) {
        $('#VendModel').modal('hide');
        ctxtVendName.SetText(Name);
        GetObjectID('hdnVendorId').value = Id;
        LoadVendorPayment();
        cbtntxtDocNo.SetText('');
        DocumentList = [];
        DeleteAllRows();
        cbtntxtDocNo.Focus();
    }
}

function LoadVendorPayment() {
    var VendorID = GetObjectID('hdnVendorId').value;
    var transDate = cdtTDate.date.format('yyyy-MM-dd');
    var numbSchm = cCmbScheme.GetValue();
    var splitData = numbSchm.split('~');
    var OtherDetails = {}
    OtherDetails.Mode = "Add";
    OtherDetails.VendorID = VendorID;
    OtherDetails.date = transDate;
    OtherDetails.Branch = splitData[2];
    $.ajax({
        type: "POST",
        url: "/OMS/Management/Activities/Services/VendorPaymentAdjustment.asmx/GetDocumentListRatediff",
        data: JSON.stringify(OtherDetails),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            ReceiptList = msg.d;

        }
    });
}


function adjAmountLostFocus() {
    if (parseFloat(cAdjAmt.GetValue()) > parseFloat(cOsAmt.GetValue())) {
        jAlert("Adjusted Amount can not be more than O/S Amount.", "Alert");
        cAdjAmt.SetValue(cOsAmt.GetValue());
        //jAlert("Adjusted Amount can not be more than O/S Amount.", "Alert", function () { cAdjAmt.SetValue(cOsAmt.GetValue()); });
    }
}

function gridDocNobuttonClick() {
    var tempDocumentList = $.grep(DocumentList, function (e) { return !PickedDocument.includes(e.uniqueid); });
    var newobj = [];
    if (tempDocumentList.length == 0) {
        jAlert("No document is available for adjustment", "Alert");
    } else {
        $('#DocumentModel').modal('show');

        var htmlScript = "<table class='dynamicPopupTbl' border='1' width='100%'> <tr class='HeaderStyle'>  <th>Document Number</th> <th>Document Date</th> <th>Document Type</th><th>Document Amount</th><th>Balance Amount</th></tr>";

        //for (var rp = 0; rp < tempDocumentList.length; rp++) {
        //    htmlScript += "<tr> <td><input readonly onclick=DocuementClick('" + tempDocumentList[rp].uniqueid + "') type='text' style='background-color: #3399520a;'DocumentIndex=" + rp + " onfocus='DocumentGetFocus(event)'  onblur='DocumentlostFocus(event)' onkeydown=DocumnetSelected(event,'" + tempDocumentList[rp].uniqueid + "') width='100%' readonly value='" + tempDocumentList[rp].No + "'/></td><td  onclick=DocuementClick('" + tempDocumentList[rp].uniqueid + "')>" + tempDocumentList[rp].docDate + "</td><td onclick=DocuementClick('" + tempDocumentList[rp].uniqueid + "')>" + tempDocumentList[rp].doctype + "</td><td onclick=DocuementClick('" + tempDocumentList[rp].uniqueid + "')>" + GetTwodecimalValue(tempDocumentList[rp].actAmt) + "</td><td onclick=DocuementClick('" + tempDocumentList[rp].uniqueid + "')>" + GetTwodecimalValue(tempDocumentList[rp].unPdAmt) + "</td></tr>";
        //}

        htmlScript += ' </table>';
        document.getElementById('DocNoDocTbl').innerHTML = htmlScript;

        $("#txtDocSearch").val("");

        for (var i = 0; i < tempDocumentList.length; i++) {
            var obj = {};
            obj.id = tempDocumentList[i]["id"];
            obj.No = tempDocumentList[i]["No"];
            obj.docDate = tempDocumentList[i]["docDate"];
            obj.doctype = tempDocumentList[i]["doctype"];
            obj.actAmt = tempDocumentList[i]["actAmt"];
            obj.uniqueid = tempDocumentList[i]["uniqueid"];
            obj.unPdAmt = tempDocumentList[i]["unPdAmt"];
            obj.cur = tempDocumentList[i]["cur"];
            newobj.push(obj);
        }
        document.getElementById('DocNoDocTbl').innerHTML = MakeTableFromArray(newobj, "SetDoc", "DocumentIndex");
    }
}
function MakeTableFromArray(myObj, onSelect, UniqueIndex) {
    var myObj = myObj;
    mycallonServerObj = myObj;
    var txt = '';
    var count = 0;

    txt = "<table class='dynamicPopupTbl' border='1' width='100%'> <tr class='HeaderStyle'><th>Document Number</th> <th>Document Date</th> <th>Document Type</th><th>Document Amount</th><th>Balance Amount</th></tr>";


    for (x in myObj) {
        txt += "<tr onclick='RowclickDoc(event," + onSelect + ")'>";
        var PropertyCount = 0;

        for (key in myObj[0]) {

            if (PropertyCount == 0)
                txt += " <td class='hide'>" + myObj[x]["id"] + "</td>";
            else if (PropertyCount == 5)
                txt += " <td class='hide'>" + myObj[x]["uniqueid"] + "</td>";
            else if (PropertyCount == 7)
                txt += " <td class='hide'>" + myObj[x]["cur"] + "</td>";
            else
                txt += " <td style='width:220px'><input onclick='PopupTextClickDocgrid(event," + onSelect + ")' type='text' style='background-color: #3399520a;'" + UniqueIndex + "='" + count + "'onfocus='DocumentGetFocus(event)'  onblur='DocumentlostFocus(event)' onkeydown=DocumnetSelected(event,'" + UniqueIndex.toString() + "',event.target) width='100%'  readonly  />" + myObj[x][key] + "</td>";
            PropertyCount++;
        }
        txt += "</tr>";
        count++;
    }
    txt += "</table>"

    return txt;
}
function PopupTextClickDocgrid(e, OnSelect) {
    var Id = e.target.parentElement.parentElement.children[0].innerText;
    //var name = e.target.parentElement.parentElement.cells[1].children[0].value;
    var  name=e.target.parentElement.parentElement.children[1].innerText;
    OnSelect.call(this, Id, name, e.target.parentElement);
}

function SetDoc(id, Name, e) {

    grid.batchEditApi.StartEdit(globalRowindex, 8);
    //Below line added by chinmoy   
    //MantisId: 0019632
    var Documntid = e.parentElement.children[0].innerHTML;
    //End
    var No = e.parentElement.cells[1].innerText;
    var doctype = e.parentElement.cells[3].innerText;
    var actAmt = e.parentElement.cells[4].innerText;
    var unPdAmt = e.parentElement.cells[6].innerText;
    var Uniqueid = e.parentElement.cells[5].innerText;
    var cur = e.parentElement.cells[7].innerText;

    grid.GetEditor("DocNo").SetText(No);
    grid.GetEditor("DocAmt").SetValue(actAmt);
    grid.GetEditor("OsAmt").SetValue(unPdAmt);
     //Below line added by chinmoy   
    //MantisId: 0019632
    grid.GetEditor("DocumentId").SetText(Documntid);
    //End
    grid.GetEditor("DocumentType").SetText(doctype);
    grid.GetEditor("Currency").SetText(cur);

    //REV 1.0

    var OsAmt = cOsAmt.GetValue();
    var AdjAmt = cAdjAmt.GetValue();
    if (parseFloat(cAdjAmt.GetValue()) < parseFloat(cOsAmt.GetValue())) {
        if (parseFloat(cAdjAmt.GetValue()) == 0) {
            if (parseFloat(grid.GetEditor("OsAmt").GetValue()) <= parseFloat(cOsAmt.GetValue())) {
                grid.GetEditor("AdjAmt").SetValue(grid.GetEditor("OsAmt").GetValue());
            }
            else {
                grid.GetEditor("AdjAmt").SetValue(parseFloat(cOsAmt.GetValue()));
            }
        }
        else {
            if (parseFloat(grid.GetEditor("OsAmt").GetValue()) < (parseFloat(cOsAmt.GetValue()) - parseFloat(cAdjAmt.GetValue()))) {
                grid.GetEditor("AdjAmt").SetValue(parseFloat(grid.GetEditor("OsAmt").GetValue()));
            }
            else if (parseFloat(grid.GetEditor("OsAmt").GetValue()) > (parseFloat(cOsAmt.GetValue()) - parseFloat(cAdjAmt.GetValue()))) {
                grid.GetEditor("AdjAmt").SetValue(parseFloat(cOsAmt.GetValue()) - parseFloat(cAdjAmt.GetValue()));
            }
        }
        ShowRunningTotal();
    }
    else {
        if (grid.GetVisibleRowsOnPage() > 1) {

            PopOnPicked(grid.GetEditor("DocumentType").GetText() + grid.GetEditor("DocumentId").GetText());
            grid.DeleteRow(globalRowindex);
            SuffuleSerialNumber();
            ShowRunningTotal();
        }
    }

    //REV 1.0 END

    CreateDocumentList();
    //grid.batchEditApi.EndEdit();

    $('#DocumentModel').modal('hide');
}
//Rev 1.0
function GetTotalAmount() {

    var totalAmount = 0;

    for (var i = 0; i < 1000; i++) {
        if (grid.GetRow(i)) {
            if (grid.GetRow(i).style.display != "none") {
                grid.batchEditApi.StartEdit(i, 2);
                totalAmount = totalAmount + DecimalRoundoff(grid.GetEditor("AdjAmt").GetText(), 2);
            }
        }
    }

    for (i = -1; i > -1000; i--) {
        if (grid.GetRow(i)) {
            if (grid.GetRow(i).style.display != "none") {
                grid.batchEditApi.StartEdit(i, 2);
                totalAmount = totalAmount + DecimalRoundoff(grid.GetEditor("AdjAmt").GetText(), 2);
            }
        }
    }

    return totalAmount;
}
function ShowRunningTotal() {
    var TotAmt = DecimalRoundoff(GetTotalAmount(), 2);
    cAdjAmt.SetValue(TotAmt.toString());
}
//REv 1.0 End
function gridDocNoKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
        gridDocNobuttonClick();
    }
}
function gridDocumentNewkeydown(e) {
    var newobj = [];
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        grid.batchEditApi.StartEdit(globalRowindex);
        var receiptdetails = $.grep(DocumentList, function (e) { return !PickedDocument.includes(e.uniqueid); });
        var SearchObj = $("#txtGridDocSearch").val();
        if (SearchObj != "") {
            for (var i = 0; i < receiptdetails.length; i++) {

                var obj = {};
                var NewCode = receiptdetails[i]["No"];
                if (NewCode.toUpperCase().includes(SearchObj.toUpperCase())) {

                    obj.id = receiptdetails[i]["id"];
                    obj.No = receiptdetails[i]["No"];
                    obj.docDate = receiptdetails[i]["docDate"];
                    obj.doctype = receiptdetails[i]["doctype"];
                    obj.actAmt = receiptdetails[i]["actAmt"];
                    obj.uniqueid = receiptdetails[i]["uniqueid"];
                    obj.unPdAmt = receiptdetails[i]["unPdAmt"];
                    obj.cur = receiptdetails[i]["cur"];

                    newobj.push(obj);
                }
            }
            document.getElementById('DocNoDocTbl').innerHTML = "";
            document.getElementById('DocNoDocTbl').innerHTML = MakeTableFromArray(newobj, "SetDoc", "DocumentIndex");
        }
        else {
            document.getElementById('DocNoDocTbl').innerHTML = "";
            document.getElementById('DocNoDocTbl').innerHTML = MakeTableFromArray(DocObj, "SetDoc", "DocumentIndex");
        }

        setTimeout(function () { document.getElementById("txtGridDocSearch").focus(); }, 500);

    }
    else if (e.code == "ArrowDown") {
        if ($("input[DocumentIndex=0]"))
            $("input[DocumentIndex=0]").focus();
    }
}

function DocuementClick(uniqueid) {
    populateDocument(uniqueid);
}
function DocumentGetFocus(e) {
    e.target.parentElement.parentElement.className = "focusrow";
    e.target.style = "background: #0000ff3d";
}
function DocumentlostFocus(e) {
    e.target.parentElement.parentElement.className = "";
    e.target.style = "background-color: #3399520a";
}

function DocumnetSelected(e, id) {
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        if (id) {
            populateDocument(id);
        }
    }

    else if (e.code == "ArrowDown") {
        thisindex = parseFloat(e.target.getAttribute('DocumentIndex'));
        thisindex++;
        //if (thisindex < 10)
        $("input[DocumentIndex=" + thisindex + "]").focus();
    }
    else if (e.code == "ArrowUp") {
        thisindex = parseFloat(e.target.getAttribute('DocumentIndex'));
        thisindex--;
        if (thisindex > -1)
            $("input[DocumentIndex=" + thisindex + "]").focus();
    }

}



function showDocumentList() {
    var numbSchm = cCmbScheme.GetValue();
    var splitData = numbSchm.split('~');


    var OtherDetails = {}
    OtherDetails.Mode = $('#hdAddEdit').val();
    OtherDetails.ReceiptId = GetObjectID('hdAdvanceDocNo').value;
    OtherDetails.VendorId = GetObjectID('hdnVendorId').value;
    OtherDetails.TransDate = cdtTDate.date.format('yyyy-MM-dd');
    OtherDetails.AdjId = GetObjectID('hdAdjustmentId').value;
    OtherDetails.BranchId = $('#ddlBranch').val();//splitData[2];
    OtherDetails.AdvType = AdvanceType;
    $.ajax({
        type: "POST",
        url: "/OMS/Management/Activities/Services/VendorPaymentAdjustment.asmx/GetInvoiceDocumentListRateDiff",
        data: JSON.stringify(OtherDetails),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            DocumentList = msg.d;

        }
    });
}

function populateDocument(id) {

    $('#DocumentModel').modal('hide');
    grid.batchEditApi.StartEdit(globalRowindex, 8);

    var SelectedDocument = $.grep(DocumentList, function (e) { return e.uniqueid == id; });
    if (SelectedDocument.length > 0) {
        var setObj = SelectedDocument[0];
        PopOnPicked(grid.GetEditor("DocumentType").GetText() + grid.GetEditor("DocumentId").GetText());
        grid.GetEditor("DocNo").SetText(setObj.No);
        grid.GetEditor("DocAmt").SetValue(setObj.actAmt);
        grid.GetEditor("OsAmt").SetValue(setObj.unPdAmt);
        grid.GetEditor("DocumentId").SetText(setObj.id);
        grid.GetEditor("DocumentType").SetText(setObj.doctype);
        grid.GetEditor("Currency").SetText(setObj.cur);
        PushOnPicked(id);

    }
}


function GetVisibleIndex(s, e) {
    globalRowindex = e.visibleIndex;
}
function gridFocusedRowChanged(s, e) {
    globalRowindex = e.visibleIndex;
}

function gridAdjustAmtLostFocus(s, e) {
    if (parseFloat(grid.GetEditor("OsAmt").GetValue()) < s.GetValue()) {

        grid.batchEditApi.StartEdit(globalRowindex, 8);

        var NewAmt = grid.GetEditor("OsAmt");
        s.SetValue(NewAmt.GetValue());
        jAlert("Adjusted Amount can not be more than O/S Amount.", "Alert");
        //return;
    }
    grid.GetEditor("RemainingBalance").SetValue(parseFloat(grid.GetEditor("OsAmt").GetValue()) - s.GetValue());
    //REV 1.0
    ShowRunningTotal();
    //REV 1.0 End
}

function GridAddnewRow() {
    grid.AddNewRow();
    grid.GetEditor("SrlNo").SetText(grid.GetVisibleItemsOnPage());
}

function gridCustomButtonClick(s, e) {
    if (e.buttonID == 'CustomDelete') {
        if (grid.GetVisibleRowsOnPage() > 1) {
            grid.batchEditApi.StartEdit(e.visibleIndex);
            PopOnPicked(grid.GetEditor("DocumentType").GetText() + grid.GetEditor("DocumentId").GetText());
            grid.DeleteRow(e.visibleIndex);
            SuffuleSerialNumber();
            //Rev 1.0
            ShowRunningTotal();
            //Rev 1.0 End
        }
    }
    else if (e.buttonID == 'AddNew') {
        GridAddnewRow();
    }
}

function AllControlInitilize() {
    if (canCallBack) {
        GridAddnewRow();
        if ($('#hdAddEdit').val() == "Add") {
            cCmbScheme.Focus();

        } else {
            showDocumentList();
            CreateDocumentList();
            cRemarks.Focus();
        }

        canCallBack = false;
    }
}

function SuffuleSerialNumber() {
    var TotRowNumber = grid.GetVisibleItemsOnPage();
    var SlnoCount = 1;
    for (var i = 0; i < 1000; i++) {
        if (grid.GetRow(i)) {
            grid.batchEditApi.StartEdit(i, 2);
            grid.GetEditor("SrlNo").SetText(SlnoCount);
            SlnoCount++;
        }
    }

    for (i = -1; i > -1000; i--) {
        if (grid.GetRow(i)) {
            grid.batchEditApi.StartEdit(i, 2);
            grid.GetEditor("SrlNo").SetText(SlnoCount);
            SlnoCount++;
        }
    }
}


function DeleteAllRows() {
    var TotRowNumber = grid.GetVisibleItemsOnPage();
    for (var i = 0; i < 1000; i++) {
        if (grid.GetRow(i)) {
            grid.DeleteRow(i);
        }
    }

    for (i = -1; i > -1000; i--) {
        if (grid.GetRow(i)) {
            grid.DeleteRow(i);
        }
    }
    PickedDocument = [];
    //GridAddnewRow();
    grid.AddNewRow();
    grid.GetEditor("SrlNo").SetText(1);
}


function ValidateEntry() {
    var ReturnValue = true;
    if (ctxtVoucherNo.GetText().trim() == "") {
        $('#MandatoryAdjNo').show();
        return false;
    } else {
        $('#MandatoryAdjNo').hide();
    }

    if (GetObjectID('hdnVendorId').value.trim() == "") {
        $('#MandatoryVendor').show();
        return false;
    } else {
        $('#MandatoryVendor').hide();
    }

    if (cbtntxtDocNo.GetText().trim() == "") {
        $('#MandatoryDocNo').show()
        return false;
    } else {
        $('#MandatoryDocNo').hide();
    }
    if (parseFloat(cAdjAmt.GetValue()) <= 0) {

        jAlert("Adjusted Amount must be greater than zero.", "Alert", function () { cAdjAmt.Focus(); });
        return false;
    }
    if (parseFloat(cAdjAmt.GetValue()) != GetTotalAdjustedAmount()) {
        jAlert("Mismatch detected in Adjusted Amount and Adjustment Amount.", "Alert", function () { cAdjAmt.Focus(); });
        return false;
    }

    return ReturnValue;
}


function CmbScheme_ValueChange(s, e) {
    var numbSchm = s.GetValue();
    var splitData = numbSchm.split('~');
    var startNo = splitData[1];
    GridBranchId = splitData[2];
    var fromdate = numbSchm.toString().split('~')[3];
    var todate = numbSchm.toString().split('~')[4];

    var dt = new Date();

    cdtTDate.SetDate(dt);

    if (dt < new Date(fromdate)) {
        cdtTDate.SetDate(new Date(fromdate));
    }

    if (dt > new Date(todate)) {
        cdtTDate.SetDate(new Date(todate));
    }




    cdtTDate.SetMinDate(new Date(fromdate));
    cdtTDate.SetMaxDate(new Date(todate));

    $('#ddlBranch').val(splitData[2]);
    if (startNo == "1") {
        ctxtVoucherNo.SetText("Auto");
        ctxtVoucherNo.SetEnabled(false);
    } else {
        ctxtVoucherNo.SetText("");
        ctxtVoucherNo.SetEnabled(true);
    }

}

function PushOnPicked(uniqueId) {
    PickedDocument.push(uniqueId);
}

function PopOnPicked(uniqueId) {
    for (var i = 0; i < PickedDocument.length; i++) {
        if (PickedDocument[i] == uniqueId) {
            PickedDocument.splice(i, 1);
            return;
        }
    }
}


function SaveButtonClick() {
    saveNewOrExit = 'N';
    $('#HiddenSaveButton').val("N");
    if (ValidateEntry() && !grid.InCallback()) {

        grid.UpdateEdit();
    }
}


function HeaderClear() {
    ctxtVendName.SetText("");
    GetObjectID('hdnVendorId').value = "";
    GetObjectID('hdAdvanceDocNo').value = "";
    cbtntxtDocNo.SetText("");
    cDocAmt.SetValue(0);
    cExchRate.SetValue(0);
    cBaseAmt.SetValue(0);
    cRemarks.SetText("");
    cOsAmt.SetValue(0);
    cAdjAmt.SetValue(0);
}


function afterSave() {

    if (saveNewOrExit == 'N') {
        window.location.href = 'VendorRateDiffAdjustmentInvoice.aspx?Key=Add';
        //DeleteAllRows();
        //DocumentList = [];
        //var numbSchm = cCmbScheme.GetValue();
        //var splitData = numbSchm.split('~');
        //var startNo = splitData[1];
        //HeaderClear();
        $('#hdAddEdit').val("Add");
        //enabledHeader();


        //if (startNo != "1") {
        //    ctxtVoucherNo.SetText("");
        //    ctxtVoucherNo.Focus();
        //}
        //else {
        //    ctxtVoucherNo.SetText("Auto");
        //    ctxtVoucherNo.SetEnabled(false);
        //    ctxtVendName.Focus();
        //}

    }
    else {
        window.location.href = 'VendorRateDiffAdjustmentInvoiceList.aspx';
    }
}



function GetTotalAdjustedAmount() {
    var TotaAdj = 0;
    for (var i = 0; i < 1000; i++) {
        if (grid.GetRow(i)) {
            if (grid.GetRow(i).style.display != 'none') {
                grid.batchEditApi.StartEdit(i, 2);
                TotaAdj = TotaAdj + parseFloat(grid.GetEditor("AdjAmt").GetValue());
            }
        }
    }

    for (i = -1; i > -1000; i--) {
        if (grid.GetRow(i)) {
            if (grid.GetRow(i).style.display != 'none') {
                grid.batchEditApi.StartEdit(i, 2);
                TotaAdj = TotaAdj + parseFloat(grid.GetEditor("AdjAmt").GetValue())
            }
        }
    }

    return TotaAdj;
}

function GridEndCallBack(s, e) {

    console.log(s.cpErrorCode.toString());
    alertShow = true;
    if (grid.cpErrorCode == "0") {
        jAlert(grid.cpadjustmentNumber, "Alert", function () { afterSave(); alertShow = false; });
    } else {
        jAlert(grid.cpadjustmentNumber, "Alert", function () { grid.batchEditApi.StartEdit(-1, 2); grid.batchEditApi.StartEdit(0, 2); alertShow = false; });
    }
}


function CreateDocumentList() {
    for (var i = 0; i < 1000; i++) {
        if (grid.GetRow(i)) {
            grid.batchEditApi.StartEdit(i, 2);
            PushOnPicked(grid.GetEditor("DocumentType").GetText() + grid.GetEditor("DocumentId").GetText());
        }
    }

    for (i = -1; i > -1000; i--) {
        if (grid.GetRow(i)) {
            grid.batchEditApi.StartEdit(i, 2);
            PushOnPicked(grid.GetEditor("DocumentType").GetText() + grid.GetEditor("DocumentId").GetText());

        }
    }
}


function enabledHeader() {
    document.getElementById('ddlBranch').disabled = false;
    cCmbScheme.SetEnabled(true);
    ctxtVoucherNo.SetEnabled(true);
    cdtTDate.SetEnabled(true);
    ctxtVendName.SetEnabled(true);
    cbtntxtDocNo.SetEnabled(true);

}


function ddlBranch_SelectedIndexChanged() {
    HeaderClear();
    $('#ddlBranch').focus();
}


function cAdjDateChange() {
    HeaderClear();
}


function SaveExitButtonClick() {
    $('#HiddenSaveButton').val("E");
    saveNewOrExit = 'E';
    if (ValidateEntry() && !grid.InCallback()) {
        grid.UpdateEdit();
    }
}



document.onkeydown = function (e) {
    if (event.keyCode == 83 && event.altKey == true && !alertShow) { //run code for Alt + n -- ie, Save & New  

        SaveButtonClick();
    }
    else if (event.keyCode == 88 && event.altKey == true && !alertShow) { //run code for Ctrl+X -- ie, Save & Exit!     

        SaveExitButtonClick();
    }

}


function GetTwodecimalValue(val) {
    return parseFloat((Math.round(val * 100)) / 100).toFixed(2);
}


