/*****************
Global variable*/
var ReceiptList = [];
var globalRowindex = 0;
var DocumentList = [];
var canCallBack = true;
var PickedDocument = [];
var saveNewOrExit = '';
var alertShow = false;
var RowCount = 0;
$(document).ready(function () {
    // GridAddnewRow();
    $('#AdvanceModel').on('shown.bs.modal', function () {
        if ($("input[receiptIndex=0]"))
            $("input[receiptIndex=0]").focus();
    })

    $('#DocumentModel').on('shown.bs.modal', function () {
        if ($("input[DocumentIndex=0]"))
            $("input[DocumentIndex=0]").focus();
    })

    $('#CustModel').on('shown.bs.modal', function () {
        $('#txtCustSearch').focus();
    })
    $('#VendModel').on('shown.bs.modal', function () {
        $("#txtVendSearch").focus();
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
        setTimeout(function () { $("#txtVendSearch").focus(); }, 500);
        
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
            setTimeout(function () { $("#txtVendSearch").focus(); }, 500);
        }
    }
}

//***********************Header Document Function Start********************

function DocumentNumberBtnClick(s, e) {
    if (GetObjectID('hdnCustomerId').value) {
        $('#AdvanceModel').modal('show');
        setTimeout(function () { document.getElementById("txtAdvanceSearch").focus(); }, 500);
        var newobj = [];
        var receiptdetails = $.grep(ReceiptList, function (e) { return !PickedDocument.includes(e.ArId); });
        var ProjectSelectInEntry = GetObjectID('hdnProjectSelectInEntryModule').value;        
        if (ProjectSelectInEntry == 1) {

            var htmlScript = "<table class='dynamicPopupTbl' border='1' width='100%'> <tr class='HeaderStyle'>  <th>Journal No.</th> <th>Journal Date</th> <th>Amount</th> <th>Balance</th><th>Project Code</th></tr>";


            htmlScript += ' </table>';
            document.getElementById('AdvRecDocTbl').innerHTML = htmlScript;

            for (var i = 0; i < ReceiptList.length; i++) {
                var obj = {};

                obj.ArId = receiptdetails[i]["ArId"];
                obj.id = receiptdetails[i]["DocId"];
                obj.No = receiptdetails[i]["docNo"];
                obj.docDt = receiptdetails[i]["docDt"];
                obj.ActAmt = receiptdetails[i]["ActAmt"];
                obj.avlAmt = receiptdetails[i]["avlAmt"];
                obj.Cur = receiptdetails[i]["Cur"];
                obj.CurRate = receiptdetails[i]["CurRate"];
                obj.AdvType = receiptdetails[i]["AdvType"];
                obj.Proj_Id = receiptdetails[i]["Proj_Id"];
                obj.Proj_Code = receiptdetails[i]["Proj_Code"];
                obj.HIERARCHY_NAME = receiptdetails[i]["HIERARCHY_NAME"];
                newobj.push(obj);

            }
        }
        else {
            var htmlScript = "<table class='dynamicPopupTbl' border='1' width='100%'> <tr class='HeaderStyle'>  <th>Journal No.</th> <th>Journal Date</th> <th>Amount</th> <th>Balance</th></tr>";
            htmlScript += ' </table>';
            document.getElementById('AdvRecDocTbl').innerHTML = htmlScript;

            for (var i = 0; i < ReceiptList.length; i++) {
                var obj = {};

                obj.ArId = receiptdetails[i]["ArId"];
                obj.id = receiptdetails[i]["DocId"];
                obj.No = receiptdetails[i]["docNo"];
                obj.docDt = receiptdetails[i]["docDt"];
                obj.ActAmt = receiptdetails[i]["ActAmt"];
                obj.avlAmt = receiptdetails[i]["avlAmt"];
                obj.Cur = receiptdetails[i]["Cur"];
                obj.CurRate = receiptdetails[i]["CurRate"];
                obj.AdvType = receiptdetails[i]["AdvType"];
                newobj.push(obj);

            }
        }
        document.getElementById('AdvRecDocTbl').innerHTML = MakeTableFromArrayForJournal(newobj, "SetDocForJournal", "JournalIndex");

    } else {
        jAlert("Please select Vendor first.", "Alert", function () { ctxtVendName.Focus(); });
    }
}
function MakeTableFromArrayForJournal(myObj, onSelect, UniqueIndex) {
    var myObj = myObj;
    mycallonServerObj = myObj;
    var txt = '';
    var count = 0;
    var ProjectSelectInEntry = GetObjectID('hdnProjectSelectInEntryModule').value;        
    if (ProjectSelectInEntry == 1) {
        txt = "<table class='dynamicPopupTbl' border='1' width='100%'> <tr class='HeaderStyle'>  <th>Journal No.</th> <th>Journal Date</th> <th>Amount</th> <th>Balance</th><th>Project Code</th></tr>";

        for (x in myObj) {
            txt += "<tr onclick='RowclickJournal(event," + onSelect + ")'>";
            var PropertyCount = 0;

            for (key in myObj[0]) {

                if (PropertyCount == 0)
                    txt += " <td class='hide'>" + myObj[x]["ArId"] + "</td>";
                else if (PropertyCount == 1)
                    txt += " <td class='hide'>" + myObj[x]["id"] + "</td>";
                else if (PropertyCount == 6 || PropertyCount == 7 || PropertyCount == 8)
                    txt += " <td class='hide'>" + myObj[x]["AdvType"] + "</td>";
                else if (PropertyCount == 9)
                    txt += " <td class='hide'>" + myObj[x]["Proj_Id"] + "</td>";
                else if (PropertyCount == 11)
                    txt += " <td class='hide'>" + myObj[x]["HIERARCHY_NAME"] + "</td>";
                else

                    txt += " <td style='width:220px'><input onclick='PopupTextClickJournal(event," + onSelect + ")' type='text' style='background-color: #3399520a;'" + UniqueIndex + "='" + count + "'onfocus='ReceiptGetFocus(event)'  onblur='ReceiptlostFocus(event)' onkeydown=ReceiptSelected(event,'" + UniqueIndex.toString() + "',event.target) width='100%'  readonly  />" + myObj[x][key] + "</td>";
                PropertyCount++;
            }
            txt += "</tr>";
            count++;
        }
        txt += "</table>"
    }
    else
    {
        txt = "<table class='dynamicPopupTbl' border='1' width='100%'> <tr class='HeaderStyle'>  <th>Journal No.</th> <th>Journal Date</th> <th>Amount</th> <th>Balance</th></tr>";

        for (x in myObj) {
            txt += "<tr onclick='RowclickJournal(event," + onSelect + ")'>";
            var PropertyCount = 0;

            for (key in myObj[0]) {

                if (PropertyCount == 0)
                    txt += " <td class='hide'>" + myObj[x]["ArId"] + "</td>";
                else if (PropertyCount == 1)
                    txt += " <td class='hide'>" + myObj[x]["id"] + "</td>";
                else if (PropertyCount == 6 || PropertyCount == 7 || PropertyCount == 8)
                    txt += " <td class='hide'>" + myObj[x]["AdvType"] + "</td>";
                else if (PropertyCount == 9)
                    txt += " <td class='hide'>" + myObj[x]["Proj_Id"] + "</td>";
                else if (PropertyCount == 10)
                    txt += " <td class='hide'>" + myObj[x]["Proj_Code"] + "</td>";
                else if (PropertyCount == 11)
                    txt += " <td class='hide'>" + myObj[x]["HIERARCHY_NAME"] + "</td>";
                else

                    txt += " <td style='width:220px'><input onclick='PopupTextClickJournal(event," + onSelect + ")' type='text' style='background-color: #3399520a;'" + UniqueIndex + "='" + count + "'onfocus='ReceiptGetFocus(event)'  onblur='ReceiptlostFocus(event)' onkeydown=ReceiptSelected(event,'" + UniqueIndex.toString() + "',event.target) width='100%'  readonly  />" + myObj[x][key] + "</td>";
                PropertyCount++;
            }
            txt += "</tr>";
            count++;
        }
        txt += "</table>"
    }
    return txt;
}
function RowclickJournal(e, OnSelect) {
    if (e.target.type != "text") {
        var Id = e.target.parentElement.children[1].innerText;
        var name = e.target.parentElement.children[2].innerText;
        OnSelect.call(this, Id, name, e.target);
    }
}
function PopupTextClickJournal(e, OnSelect) {
    var Id = e.target.parentElement.parentElement.children[0].innerText;
    var name = e.target.parentElement.parentElement.cells[1].children[0].value;
    OnSelect.call(this, Id, name, e.target.parentElement);
}
function DocumentNumberBtn(s, e) {
    if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
        DocumentNumberBtnClick();
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
                    obj.id = receiptdetails[i]["DocId"];
                    obj.No = receiptdetails[i]["docNo"];
                    obj.docDt = receiptdetails[i]["docDt"];
                    obj.ActAmt = receiptdetails[i]["ActAmt"];
                    obj.avlAmt = receiptdetails[i]["avlAmt"];
                    obj.Cur = receiptdetails[i]["Cur"];
                    obj.CurRate = receiptdetails[i]["CurRate"];
                    obj.AdvType = receiptdetails[i]["AdvType"];
                    newobj.push(obj);

                }
            }
            document.getElementById('AdvRecDocTbl').innerHTML = "";
            document.getElementById('AdvRecDocTbl').innerHTML = MakeTableFromArrayForJournal(newobj, "SetDocForJournal", "JournalIndex");
        }
        else {
            document.getElementById('AdvRecDocTbl').innerHTML = "";
            document.getElementById('AdvRecDocTbl').innerHTML = MakeTableFromArrayForJournal(ReceiptList, "SetDocForJournal", "JournalIndex");
        }

        setTimeout(function () { document.getElementById("txtGridDocSearch").focus(); }, 500);
    }
    else if (e.code == "ArrowDown") {
        if ($("input[JournalIndex=0]"))
            $("input[JournalIndex=0]").focus();
    }
}
function SetDocForJournal(id, Name, e) {
    DeleteAllRows();

    var Uniqueid = e.parentElement.cells[0].innerText;
    var JournalID = e.parentElement.cells[1].innerText;
    var No = e.parentElement.cells[2].innerText;

    var docDt = e.parentElement.cells[3].innerText;
    var ActAmt = e.parentElement.cells[4].innerText;
    var avlAmt = e.parentElement.cells[5].innerText;
    var AdvType = e.parentElement.cells[6].innerText;
    var Cur = e.parentElement.cells[7].innerText;
    var CurRate = e.parentElement.cells[8].innerText;
    var ProjectSelectInEntry = GetObjectID('hdnProjectSelectInEntryModule').value;
    if (ProjectSelectInEntry == 1) {
        var Proj_Id = e.parentElement.cells[9].innerText;
        var Proj_Code = e.parentElement.cells[10].innerText;
        var HIERARCHY_NAME = e.parentElement.cells[11].innerText;
    }

    $('#AdvanceModel').modal('hide');
    cbtntxtDocNo.SetText(No);
    cDocAmt.SetValue(ActAmt);

    cExchRate.SetValue(CurRate);

    if (CurRate == 0)
        cBaseAmt.SetValue(ActAmt);
    else
        cBaseAmt.SetValue(ActAmt * CurRate);


    cOsAmt.SetValue(avlAmt);
    GetObjectID('hdAdvanceDocNo').value = JournalID;
    GetObjectID('hdAdjustmentType').value = AdvType;
    GetObjectID('hdAdjustmentId').value = JournalID;
    var ProjectSelectInEntry = GetObjectID('hdnProjectSelectInEntryModule').value;
    if (ProjectSelectInEntry == 1) {
        GetObjectID('hddnProjectId').value = Proj_Id;
        ctxtProject.SetText(Proj_Code);
        ctxtHierarchy.SetText(HIERARCHY_NAME);
    }
    else {
        GetObjectID('hddnProjectId').value = "0";
    }
    showDocumentList();
    cRemarks.Focus();
}
//***********************Header Document Function End********************

//***********************Grid Document Function Start********************

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
                txt += " <td style='width:220px'><input onclick='PopupTextClickDoc(event," + onSelect + ")' type='text' style='background-color: #3399520a;'" + UniqueIndex + "='" + count + "'onfocus='DocumentGetFocus(event)'  onblur='DocumentlostFocus(event)' onkeydown=DocumnetSelected(event,'" + UniqueIndex.toString() + "',event.target) width='100%'  readonly  />" + myObj[x][key] + "</td>";
            PropertyCount++;
        }
        txt += "</tr>";
        count++;
    }
    txt += "</table>"

    return txt;
}
function gridDocNobuttonClick() {
    var newobj = [];
    var tempDocumentList = $.grep(DocumentList, function (e) { return !PickedDocument.includes(e.uniqueid); });

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
function SetDoc(id, Name, e) {

    grid.batchEditApi.StartEdit(globalRowindex, 8);
    var No = e.parentElement.cells[1].innerText;
    var doctype = e.parentElement.cells[3].innerText;
    var actAmt = e.parentElement.cells[4].innerText;
    var unPdAmt = e.parentElement.cells[6].innerText;
    var Uniqueid = e.parentElement.cells[5].innerText;
    var cur = e.parentElement.cells[7].innerText;

    grid.GetEditor("DocNo").SetText(No);
    grid.GetEditor("DocAmt").SetValue(actAmt);
    grid.GetEditor("OsAmt").SetValue(unPdAmt);
    grid.GetEditor("DocumentId").SetText(id);
    grid.GetEditor("DocumentType").SetText(doctype);
    grid.GetEditor("Currency").SetText(cur);

    CreateDocumentList();
    //grid.batchEditApi.EndEdit();

    $('#DocumentModel').modal('hide');
}
function RowclickDoc(e, OnSelect) {
    if (e.target.type != "text") {
        var Id = e.target.parentElement.children[0].innerText;
        var name = e.target.parentElement.cells[1].children[0].value;
        OnSelect.call(this, Id, name, e.target);
    }
}
function PopupTextClickDoc(e, OnSelect) {
    var Id = e.target.parentElement.parentElement.children[0].innerText;
    var name = e.target.parentElement.parentElement.cells[1].children[0].value;
    OnSelect.call(this, Id, name, e.target.parentElement);
}

//***********************Grid Document Function End********************

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
        cOsAmt.SetValue(receiptdetails[0].avlAmt);
        GetObjectID('hdAdvanceDocNo').value = receiptdetails[0].DocId;
        GetObjectID('hdAdjustmentType').value = receiptdetails[0].AdvType;
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
            //callonServer("Services/Master.asmx/GetOnlyVendor", OtherDetails, "VendorTable", HeaderCaption, "VendorIndex", "SetCustomer");
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
            if (indexName == "customerIndex")
                $('#txtCustSearch').focus();
        }
    }
}
function SetCustomer(Id, Name) {
    if (Id) {
        $('#VendModel').modal('hide');
        ctxtVendName.SetText(Name);
        GetObjectID('hdnCustomerId').value = Id;
        LoadCustomerReceipt();
        cbtntxtDocNo.SetText('');
        DocumentList = [];
        DeleteAllRows();
        cbtntxtDocNo.Focus();
    }
}
function LoadCustomerReceipt() {
    var CustomerId = GetObjectID('hdnCustomerId').value;
    var transDate = cdtTDate.date.format('yyyy-MM-dd');
    var numbSchm = cCmbScheme.GetValue();
    var splitData = numbSchm.split('~');
    var OtherDetails = {}
    OtherDetails.Mode = "Add";
    OtherDetails.CustomerId = CustomerId;
    OtherDetails.date = transDate;
    OtherDetails.Branch = splitData[2];
    $.ajax({
        type: "POST",
        url: "/OMS/Management/Activities/Services/CustomerReceiptAdjustment.asmx/GetJournalDetorsVRVoucharList",
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
        //jAlert("Adjusted Amount can not be more than O/S Amount.", "Alert", function () { cAdjAmt.SetValue(cOsAmt.GetValue()); });
        cAdjAmt.SetValue(cOsAmt.GetValue());
        jAlert("Adjusted Amount can not be more than O/S Amount.", "Alert");
    }
}

function gridDocNoKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
        gridDocNobuttonClick();
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
    var OtherDetails = {}
    OtherDetails.Mode = $('#hdAddEdit').val();
    OtherDetails.ReceiptId = GetObjectID('hdAdvanceDocNo').value;
    OtherDetails.customerId = GetObjectID('hdnCustomerId').value;
    OtherDetails.TransDate = cdtTDate.date.format('yyyy-MM-dd');
    OtherDetails.AdjId = GetObjectID('hdAdjustmentId').value;
    OtherDetails.AdvType = GetObjectID('hdAdjustmentType').value;
    OtherDetails.BranchId = $("#ddlBranch").val();
    OtherDetails.ProjectId = GetObjectID('hddnProjectId').value;
    $.ajax({
        type: "POST",
        url: "/OMS/Management/Activities/Services/CustomerReceiptAdjustment.asmx/GetVendorReceiptOnAccountDocumentList",
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



    }
    grid.GetEditor("RemainingBalance").SetValue(parseFloat(grid.GetEditor("OsAmt").GetValue()) - s.GetValue());

}

function GridAddnewRow() {
    grid.AddNewRow();
    RowCount = RowCount + 1;
    grid.GetEditor("SrlNo").SetText(grid.GetVisibleItemsOnPage());
    grid.GetEditor("ActualSL").SetText(RowCount);
}

function gridCustomButtonClick(s, e) {
    if (e.buttonID == 'CustomDelete') {
        if (grid.GetVisibleRowsOnPage() > 1) {
            grid.batchEditApi.StartEdit(e.visibleIndex);
            PopOnPicked(grid.GetEditor("DocumentType").GetText() + grid.GetEditor("DocumentId").GetText());
            grid.DeleteRow(e.visibleIndex);
            SuffuleSerialNumber();
        }
    }
    else if (e.buttonID == 'AddNew') {
        GridAddnewRow();
    }
}

function AllControlInitilize() {
    if (canCallBack) {

        if ($('#hdAddEdit').val() == "Add") {
            cCmbScheme.Focus();
            GridAddnewRow();
            setTimeout(function () { cCmbScheme.Focus(); }, 500);
            cbtnSaveRecords.SetVisible(true)
            cbtn_SaveRecords.SetVisible(true)

        } else {
            RowCount = parseInt($("#HiddenRowCount").val());
            GridAddnewRow();
            showDocumentList();
            CreateDocumentList();
            cRemarks.Focus();

            cbtnSaveRecords.SetVisible(false);
            cbtn_SaveRecords.SetVisible(true);
        }

        canCallBack = false;
    }
}

function SuffuleSerialNumber() {
    var TotRowNumber = grid.GetVisibleItemsOnPage();
    var SlnoCount = 1;
    for (var i = 0; i < 1000; i++) {
        if (grid.GetRow(i)) {
            if (grid.GetRow(i).style.display != "none") {
                grid.batchEditApi.StartEdit(i, 2);
                grid.GetEditor("SrlNo").SetText(SlnoCount);
                SlnoCount++;
            }
        }
    }

    for (i = -1; i > -1000; i--) {
        if (grid.GetRow(i)) {
            if (grid.GetRow(i).style.display != "none") {
                grid.batchEditApi.StartEdit(i, 2);
                grid.GetEditor("SrlNo").SetText(SlnoCount);
                SlnoCount++;
            }
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

    if (GetObjectID('hdnCustomerId').value.trim() == "") {
        $('#MandatoryCustomer').show();
        return false;
    } else {
        $('#MandatoryCustomer').hide();
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
    $('#ddlBranch').val(splitData[2]);
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
    if (startNo == "1") {
        ctxtVoucherNo.SetText("Auto");
        ctxtVoucherNo.SetEnabled(false);
    } else {
        ctxtVoucherNo.SetText("");
        ctxtVoucherNo.SetEnabled(true);
    }
}

function PushOnPicked(uniqueId) {

    uniqueId = uniqueId.replace("Opening Invoice", "opInv");
    uniqueId = uniqueId.replace("Transit Invoice", "TInvoice");
    PickedDocument.push(uniqueId);
}

function PopOnPicked(uniqueId) {
    uniqueId = uniqueId.replace("Opening Invoice", "opInv");
    uniqueId = uniqueId.replace("Transit Invoice", "TInvoice");
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
    if (ValidateEntry()) {
        if (!grid.InCallback()) {
            grid.UpdateEdit();
        }
    }
}


function HeaderClear() {
    ctxtCustName.SetText("");
    GetObjectID('hdnCustomerId').value = "";
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
        window.location.href = 'JournalDetorsAdjustVenRec.aspx?Key=Add';
        $('#hdAddEdit').val("Add");        
    }
    else {
        window.location.href = 'JournalDetorsAdjustVenRecList.aspx';
    }
}



function GetTotalAdjustedAmount() {
    var TotaAdj = 0;
    for (var i = 0; i < 1000; i++) {
        if (grid.GetRow(i)) {
            if (grid.GetRow(i).style.display != "none") {
                grid.batchEditApi.StartEdit(i, 2);
                TotaAdj = TotaAdj + parseFloat(grid.GetEditor("AdjAmt").GetValue());
            }
        }
    }

    for (i = -1; i > -1000; i--) {
        if (grid.GetRow(i)) {
            grid.batchEditApi.StartEdit(i, 2);
            TotaAdj = TotaAdj + parseFloat(grid.GetEditor("AdjAmt").GetValue())

        }
    }

    var RoundValue = (TotaAdj).toFixed(2);

    return RoundValue;
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
    ctxtCustName.SetEnabled(true);
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
    if (ValidateEntry()) {
        if (!grid.InCallback()) {
            grid.UpdateEdit();
        }
    }
}

function getUrlVars() {
    var vars = [], hash;
    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');
        vars.push(hash[0]);
        vars[hash[0]] = hash[1];
    }
    return vars;
} 
document.onkeydown = function (e) {
    var urlKeys = getUrlVars();
    if (event.keyCode == 83 && event.altKey == true && !alertShow) { //run code for Alt + n -- ie, Save & New  
        if ($('#hdAddEdit').val() == "Add") {
            SaveButtonClick();
        }
    }
    else if (event.keyCode == 88 && event.altKey == true && !alertShow) { //run code for Ctrl+X -- ie, Save & Exit!     
        if(urlKeys.req != 'V')
        {
            SaveExitButtonClick();
        }        
    }

}


function GetTwodecimalValue(val) {
    return parseFloat((Math.round(val * 100)) / 100).toFixed(2);
}


