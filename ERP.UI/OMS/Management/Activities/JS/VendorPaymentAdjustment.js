//====================================================Revision History =========================================================================
//1.0   v2.0.39	    Priti	    27-06-2023	    0026412: Auto calculation of Adjusted amount during Adjustment of Document Entries-Advance with Invoice for Vendor
//2.0   V2.0.40     Sanchita    25-10-2023      26915: Party Invoice Date required in the Document Search window of the Invoice for the module 
//                                              Adjustment of Documents - Advance With Invoice
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
        if ($("input[receiptIndex=0]"))
            $("input[receiptIndex=0]").focus();
    })

    $('#DocumentModel').on('shown.bs.modal', function () {
        if ($("input[DocumentIndex=0]"))
            $("input[DocumentIndex=0]").focus();
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
    //    var htmlScript = "<table class='dynamicPopupTbl' border='1' width='100%'> <tr class='HeaderStyle'>  <th>Payment Number</th> <th>Payment Date</th> <th>Amount</th> <th>Balance</th></tr>";
    //    for (var rp = 0; rp < ReceiptList.length; rp++) {
    //        htmlScript += "<tr> 
    //<td><input readonly onclick='ReceiptonClick(" + ReceiptList[rp].ArId + ")' type='text' style='background-color: #3399520a;'receiptIndex=" + rp + " onfocus='ReceiptGetFocus(event)'  onblur='ReceiptlostFocus(event)' onkeydown=ReceiptSelected(event," + ReceiptList[rp].ArId + ") width='100%' readonly value='" + ReceiptList[rp].docNo + "'/></td><td  onclick='ReceiptonClick(" + ReceiptList[rp].ArId + ")'>" + ReceiptList[rp].docDt + "</td><td onclick='ReceiptonClick(" + ReceiptList[rp].ArId + ")'>" + GetTwodecimalValue(ReceiptList[rp].ActAmt) + "</td><td onclick='ReceiptonClick(" + ReceiptList[rp].ArId + ")'>" + GetTwodecimalValue(ReceiptList[rp].avlAmt) + "</td></tr>";
    //    }
    //    htmlScript += ' </table>';
    //    document.getElementById('AdvPayDocTbl').innerHTML = htmlScript;

    //} else {
    //    jAlert("Please select Vendor.", "Alert", function () { ctxtVendName.Focus(); });
    //}

        if (GetObjectID('hdnVendorId').value) {
            var newobj = [];

            var ProjectSelectInEntry = GetObjectID('hdnProjectSelectInEntryModule').value;        
            if (ProjectSelectInEntry == 1)
            {
                var htmlScript = "<table class='dynamicPopupTbl' border='1' width='100%'> <tr class='HeaderStyle'>  <th>Payment Number</th> <th>Payment Date</th> <th>Amount</th> <th>Balance</th><th>Project Code</th></tr>";
            }
            else {
                var htmlScript = "<table class='dynamicPopupTbl' border='1' width='100%'> <tr class='HeaderStyle'>  <th>Payment Number</th> <th>Payment Date</th> <th>Amount</th> <th>Balance</th></tr>";
            }



       
        htmlScript += ' </table>';
        document.getElementById('AdvPayDocTbl').innerHTML = htmlScript;

        var receiptdetails = $.grep(ReceiptList, function (e) { return !PickedDocument.includes(e.ArId); });

        if (ProjectSelectInEntry == 1)
        {
            for (var i = 0; i < ReceiptList.length; i++) {
                var obj = {};
                obj.id = receiptdetails[i]["DocId"];
                obj.No = receiptdetails[i]["docNo"];
                obj.ArId = receiptdetails[i]["ArId"];
                obj.docDt = receiptdetails[i]["docDt"];
                obj.ActAmt = receiptdetails[i]["ActAmt"];
                obj.avlAmt = receiptdetails[i]["avlAmt"];
                obj.AdvType = receiptdetails[i]["AdvType"];
                obj.Proj_Id = receiptdetails[i]["Proj_Id"];
                obj.Proj_Code = receiptdetails[i]["Proj_Code"];
                obj.HIERARCHY_NAME = receiptdetails[i]["HIERARCHY_NAME"];
                newobj.push(obj);
            }
        }
        else {
            for (var i = 0; i < ReceiptList.length; i++) {
                var obj = {};
                obj.id = receiptdetails[i]["DocId"];
                obj.No = receiptdetails[i]["docNo"];
                obj.ArId = receiptdetails[i]["ArId"];
                obj.docDt = receiptdetails[i]["docDt"];
                obj.ActAmt = receiptdetails[i]["ActAmt"];
                obj.avlAmt = receiptdetails[i]["avlAmt"];
                obj.AdvType = receiptdetails[i]["AdvType"];
                newobj.push(obj);
            }
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
    var ProjectSelectInEntry = GetObjectID('hdnProjectSelectInEntryModule').value;        
    if (ProjectSelectInEntry == 1) {
        txt = "<table class='dynamicPopupTbl' border='1' width='100%'> <tr class='HeaderStyle'>  <th>Payment Number</th> <th>Payment Date</th> <th>Amount</th> <th>Balance</th><th>Project Code</th></tr>";
        for (x in myObj) {
            txt += "<tr onclick='RowclickDoc(event," + onSelect + ")'>";
            var PropertyCount = 0;

            for (key in myObj[0]) {

                if (PropertyCount == 0)
                    txt += " <td class='hide'>" + myObj[x]["id"] + "</td>";
                else if (PropertyCount == 2)
                    txt += " <td class='hide'>" + myObj[x]["ArId"] + "</td>";
                else if (PropertyCount == 6)
                    txt += " <td class='hide'>" + myObj[x]["AdvType"] + "</td>";
                else if (PropertyCount == 7)
                    txt += " <td class='hide'>" + myObj[x]["Proj_Id"] + "</td>";
                else if (PropertyCount == 9)
                    txt += " <td class='hide'>" + myObj[x]["HIERARCHY_NAME"] + "</td>";
                else

                    txt += " <td style='width:220px'><input onclick='PopupTextClickDoc(event," + onSelect + ")' type='text' style='background-color: #3399520a;'" + UniqueIndex + "='" + count + "'onfocus='ReceiptGetFocus(event)'  onblur='ReceiptlostFocus(event)' onkeydown=ReceiptSelected(event,'" + UniqueIndex.toString() + "',event.target) width='100%'  readonly  />" + myObj[x][key] + "</td>";
                PropertyCount++;
            }
            txt += "</tr>";
            count++;
        }

    }
    else {
        txt = "<table class='dynamicPopupTbl' border='1' width='100%'> <tr class='HeaderStyle'>  <th>Payment Number</th> <th>Payment Date</th> <th>Amount</th> <th>Balance</th></tr>";
        for (x in myObj) {
            txt += "<tr onclick='RowclickDoc(event," + onSelect + ")'>";
            var PropertyCount = 0;

            for (key in myObj[0]) {

                if (PropertyCount == 0)
                    txt += " <td class='hide'>" + myObj[x]["id"] + "</td>";
                else if (PropertyCount == 2)
                    txt += " <td class='hide'>" + myObj[x]["ArId"] + "</td>";
                else if (PropertyCount == 6)
                    txt += " <td class='hide'>" + myObj[x]["AdvType"] + "</td>";
                else if (PropertyCount == 7)
                    txt += " <td class='hide'>" + myObj[x]["Proj_Id"] + "</td>";
                else if (PropertyCount == 8)
                    txt += " <td class='hide'>" + myObj[x]["Proj_Code"] + "</td>";
                else if (PropertyCount == 9)
                    txt += " <td class='hide'>" + myObj[x]["HIERARCHY_NAME"] + "</td>";
                else

                    txt += " <td style='width:220px'><input onclick='PopupTextClickDoc(event," + onSelect + ")' type='text' style='background-color: #3399520a;'" + UniqueIndex + "='" + count + "'onfocus='ReceiptGetFocus(event)'  onblur='ReceiptlostFocus(event)' onkeydown=ReceiptSelected(event,'" + UniqueIndex.toString() + "',event.target) width='100%'  readonly  />" + myObj[x][key] + "</td>";
                PropertyCount++;
            }
            txt += "</tr>";
            count++;
        }
    }

    




    txt += "</table>"

    return txt;
}

function SetDocForAdvance(id, Name, e)
{
    DeleteAllRows();

    var No  = e.parentElement.cells[1].innerText;
    var Uniqueid = e.parentElement.cells[2].innerText;
    var docDt = e.parentElement.cells[3].innerText;
    var ActAmt = e.parentElement.cells[4].innerText;
    var avlAmt = e.parentElement.cells[5].innerText;
    var AdvType = e.parentElement.cells[6].innerText;

    var ProjectSelectInEntry = GetObjectID('hdnProjectSelectInEntryModule').value;        
    if (ProjectSelectInEntry == 1) {

        var Proj_Id = e.parentElement.cells[7].innerText;
        var Proj_Code = e.parentElement.cells[8].innerText;
        var HIERARCHY_NAME = e.parentElement.cells[9].innerText;

    }
   
        $('#AdvanceModel').modal('hide');
        cbtntxtDocNo.SetText(No);
        cDocAmt.SetValue(ActAmt);

        //cExchRate.SetValue(receiptdetails[0].CurRate);
        
        //if (receiptdetails[0].CurRate == 0)
        //    cBaseAmt.SetValue(receiptdetails[0].ActAmt);
        //else
        //    cBaseAmt.SetValue(receiptdetails[0].ActAmt * receiptdetails[0].CurRate);

      
        cOsAmt.SetValue(avlAmt);
        GetObjectID('hdAdvanceDocNo').value = id;
        GetObjectID('hdAdjustmentType').value = AdvType;
        GetObjectID('hdAdjustmentId').value = id;

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

function AdvanceNewkeydown(e)
{
    var newobj = [];
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var receiptdetails = $.grep(ReceiptList, function (e) { return !PickedDocument.includes(e.ArId); });
        var SearchObj = $("#txtAdvanceSearch").val();
        if (SearchObj != "") {

            var ProjectSelectInEntry = GetObjectID('hdnProjectSelectInEntryModule').value;
            if (ProjectSelectInEntry == 1) {

                for (var i = 0; i < receiptdetails.length; i++) {
                    var obj = {};
                    var NewCode = receiptdetails[i]["docNo"];
                    if (NewCode.toUpperCase().includes(SearchObj.toUpperCase())) {

                        obj.id = receiptdetails[i]["DocId"];
                        obj.No = receiptdetails[i]["docNo"];
                        obj.ArId = receiptdetails[i]["ArId"];
                        obj.docDt = receiptdetails[i]["docDt"];
                        obj.ActAmt = receiptdetails[i]["ActAmt"];
                        obj.avlAmt = receiptdetails[i]["avlAmt"];
                        obj.AdvType = receiptdetails[i]["AdvType"];
                        obj.Proj_Id = receiptdetails[i]["Proj_Id"];
                        obj.Proj_Code = receiptdetails[i]["Proj_Code"];
                        obj.HIERARCHY_NAME = receiptdetails[i]["HIERARCHY_NAME"];
                        newobj.push(obj);
                    }
                }               
            }
            else {
                for (var i = 0; i < receiptdetails.length; i++) {
                    var obj = {};
                    var NewCode = receiptdetails[i]["docNo"];
                    if (NewCode.toUpperCase().includes(SearchObj.toUpperCase())) {

                        obj.id = receiptdetails[i]["DocId"];
                        obj.No = receiptdetails[i]["docNo"];
                        obj.ArId = receiptdetails[i]["ArId"];
                        obj.docDt = receiptdetails[i]["docDt"];
                        obj.ActAmt = receiptdetails[i]["ActAmt"];
                        obj.avlAmt = receiptdetails[i]["avlAmt"];
                        obj.AdvType = receiptdetails[i]["AdvType"];

                        newobj.push(obj);
                    }
                }
            }


            document.getElementById('AdvPayDocTbl').innerHTML = "";
            document.getElementById('AdvPayDocTbl').innerHTML = MakeTableFromArrayForAdvance(newobj, "SetDocForAdvance", "receiptIndex");
        }
        else {
            document.getElementById('AdvPayDocTbl').innerHTML = "";
            document.getElementById('AdvPayDocTbl').innerHTML = MakeTableFromArrayForAdvance(ReceiptList, "SetDocForAdvance", "receiptIndex");
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
function DocNewkeydown(e) {
    var VendorID = GetObjectID('hdnVendorId').value;
    var transDate = cdtTDate.date.format('yyyy-MM-dd');
    var numbSchm = cCmbScheme.GetValue();
    var splitData = numbSchm.split('~');
    var OtherDetails = {}
    OtherDetails.Mode = "Add";
    OtherDetails.VendorID = VendorID;
    OtherDetails.date = transDate;
    OtherDetails.Branch = splitData[2];  
    OtherDetails.SearchKey = $("#txtDocSearch").val();   
    
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var HeaderCaption = [];
        HeaderCaption.push("Payment Number");
        HeaderCaption.push("Payment Date");
        HeaderCaption.push("Amount");
        HeaderCaption.push("Balance");
        HeaderCaption.push("Type");
        if ($("#txtDocSearch").val() != '') {
            callonServer("Services/VendorPaymentAdjustment.asmx/GetAdvancePaymentList", OtherDetails, "AdvPayDocTbl", HeaderCaption, "AdvDocumentIndex", "SetDocument");
        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[AdvDocumentIndex=0]"))
            $("input[AdvDocumentIndex=0]").focus();
    }

}
function SetDocument(Id, Name,e)
{
    if (Id) {
        $('#AdvanceModel').modal('hide');
        cbtntxtDocNo.SetText(Name);
        GetObjectID('hdAdvanceDocNo').value = Id;

        var ActAmt = e.parentElement.cells[3].innerText;
        var avlAmt = e.parentElement.cells[4].innerText;
       
        cDocAmt.SetValue(ActAmt);
        //cExchRate.SetValue(receiptdetails[0].CurRate);

        //Set Value in base Currency
        //if (receiptdetails[0].CurRate == 0)
        //    cBaseAmt.SetValue(ActAmt);
        //else
        //    cBaseAmt.SetValue(ActAmt * receiptdetails[0].CurRate);

        var AdvanceType = e.parentElement.cells[5].innerText;
        if (AdvanceType == "Opening Advance")
        {
            AdvanceType = 'OP_ADV';
        }
        else
        {
            AdvanceType = 'CUR_ADV';
        }
        cOsAmt.SetValue(avlAmt);
       
        GetObjectID('hdAdjustmentType').value = AdvanceType;
        GetObjectID('hdAdjustmentId').value = Id;
        showDocumentList();
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
            //if (indexName == "AdvDocumentIndex")
            //    SetDocument(Id, name);
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
            //if (indexName == "AdvDocumentIndex")
            //    $('#txtDocSearch').focus();
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
    OtherDetails.Branch =splitData[2];
    $.ajax({
        type: "POST",
        url: "/OMS/Management/Activities/Services/VendorPaymentAdjustment.asmx/GetAdvancePaymentList",
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
    if (tempDocumentList.length == 0) {
        jAlert("No document is available for adjustment", "Alert");
    } else {
        $('#DocumentModel').modal('show');

        var newobj = [];
        // Rev rev 2.0 [ <th>Party Invoice Number</th> added ]
        //var htmlScript = "<table class='dynamicPopupTbl' border='1' width='100%'> <tr class='HeaderStyle'>  <th>Document Number</th> <th>Document Date</th> <th>Document Type</th><th>Party Invoice Number</th><th>Document Amount</th><th>Balance Amount</th></tr>";
        var htmlScript = "<table class='dynamicPopupTbl' border='1' width='100%'> <tr class='HeaderStyle'>  <th>Document Number</th> <th>Document Date</th> <th>Document Type</th><th>Party Invoice Number</th><th>Party Invoice Date</th><th>Document Amount</th><th>Balance Amount</th></tr>";
        // End of Rev rev 2.0
       
        htmlScript += ' </table>';
        document.getElementById('DocNoDocTbl').innerHTML = htmlScript;
        $("#txtDocSearch").val("");

        for (var i = 0; i < tempDocumentList.length; i++) {
            var obj = {};
            obj.id = tempDocumentList[i]["id"];
            obj.No = tempDocumentList[i]["No"];
            obj.docDate = tempDocumentList[i]["docDate"];
            obj.doctype = tempDocumentList[i]["doctype"];
            obj.PartyInvoiceNo = tempDocumentList[i]["PartyInvoiceNo"];
            // Rev rev 2.0
            obj.PartyInvoiceDate = tempDocumentList[i]["PartyInvoiceDate"];
            // End of Rev rev 2.0
            obj.actAmt = tempDocumentList[i]["actAmt"];
            obj.uniqueid = tempDocumentList[i]["uniqueid"];
            obj.unPdAmt = tempDocumentList[i]["unPdAmt"];
            obj.cur = tempDocumentList[i]["cur"];
            
            newobj.push(obj);
        }

        document.getElementById('DocNoDocTbl').innerHTML = MakeTableFromArray(newobj, "SetDoc", "DocumentIndex");
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
                // Rev rev 2.0
                //if (NewCode.toUpperCase().includes(SearchObj.toUpperCase())) {
                var PartyInvoiceNo = receiptdetails[i]["PartyInvoiceNo"];
                
                if (NewCode.toUpperCase().includes(SearchObj.toUpperCase()) || PartyInvoiceNo.toUpperCase().includes(SearchObj.toUpperCase())) {
                    // End of Rev rev 2.0
                    
                    obj.id = receiptdetails[i]["id"];
                    obj.No = receiptdetails[i]["No"];
                    obj.docDate = receiptdetails[i]["docDate"];
                    obj.doctype = receiptdetails[i]["doctype"];
                    obj.PartyInvoiceNo = receiptdetails[i]["PartyInvoiceNo"];
                    // Rev rev 2.0
                    obj.PartyInvoiceDate = receiptdetails[i]["PartyInvoiceDate"];
                    // End of Rev rev 2.0
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

    // Rev rev 2.0 [ </th><th>Party Invoice Number</th> added ]
    //txt = "<table class='dynamicPopupTbl' border='1' width='100%'> <tr class='HeaderStyle'><th>Document Number</th> <th>Document Date</th> <th>Document Type</th><th>Party Invoice Number</th><th>Document Amount</th><th>Balance Amount</th></tr>";
    txt = "<table class='dynamicPopupTbl' border='1' width='100%'> <tr class='HeaderStyle'><th>Document Number</th> <th>Document Date</th> <th>Document Type</th><th>Party Invoice Number</th></th><th>Party Invoice Date</th><th>Document Amount</th><th>Balance Amount</th></tr>";
    // End of Rev rev 2.0


    for (x in myObj) {
        txt += "<tr onclick='RowclickDoc(event," + onSelect + ")'>";
        var PropertyCount = 0;  

        for (key in myObj[0]) {

            if (PropertyCount == 0)
                txt += " <td class='hide'>" + myObj[x]["id"] + "</td>";    
                // Rev 2.0
            //else if (PropertyCount == 6)
            else if (PropertyCount == 7)
            // End of Rev 2.0
                txt += " <td class='hide'>" + myObj[x]["uniqueid"] + "</td>";
                // Rev 2.0
            //else if (PropertyCount == 8)
            else if (PropertyCount == 9)
            // End of Rev 2.0
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
function SetDoc(id, Name, e) {
       
        grid.batchEditApi.StartEdit(globalRowindex, 8);  
        var No  = e.parentElement.cells[1].innerText;
    var doctype = e.parentElement.cells[3].innerText;
        //Rev 2.0 --Rework V2.0.41
        //var actAmt = e.parentElement.cells[5].innerText;
        //var unPdAmt = e.parentElement.cells[7].innerText;
        //var Uniqueid = e.parentElement.cells[6].innerText;
        //var cur = e.parentElement.cells[8].innerText;
        var actAmt = e.parentElement.cells[6].innerText;
        var unPdAmt = e.parentElement.cells[8].innerText;
        var Uniqueid = e.parentElement.cells[7].innerText;
        var cur = e.parentElement.cells[9].innerText;
        //Rev 2.0 --Rework V2.0.41

        var PartyInvoiceNo = e.parentElement.cells[4].innerText;
        //obj.PartyInvoiceNo = tempDocumentList[i]["PartyInvoiceNo"];





        grid.GetEditor("DocNo").SetText(No);
        grid.GetEditor("DocAmt").SetValue(actAmt);
        grid.GetEditor("OsAmt").SetValue(unPdAmt);
        grid.GetEditor("DocumentId").SetText(id);
        grid.GetEditor("DocumentType").SetText(doctype);
        grid.GetEditor("Currency").SetText(cur);

        //REV 1.0
        var AutocalculationAdjustmentInvoice = GetObjectID('hdnAutocalculationAdjustmentInvoice').value;
        if (AutocalculationAdjustmentInvoice == 1) {
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
        }
        //REV 1.0 END

        CreateDocumentList();
        //grid.batchEditApi.EndEdit();       
        $('#DocumentModel').modal('hide');
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
    var numbSchm = cCmbScheme.GetValue();
    var splitData = numbSchm.split('~');


    var OtherDetails = {}
    OtherDetails.Mode = $('#hdAddEdit').val();
    OtherDetails.ReceiptId = GetObjectID('hdAdvanceDocNo').value;
    OtherDetails.VendorId = GetObjectID('hdnVendorId').value;
    OtherDetails.TransDate = cdtTDate.date.format('yyyy-MM-dd');
    OtherDetails.AdjId = GetObjectID('hdAdjustmentId').value;
    OtherDetails.BranchId = $('#ddlBranch').val();//splitData[2];
    OtherDetails.ProjectId = GetObjectID('hddnProjectId').value;
    OtherDetails.AdvType = AdvanceType;
    $.ajax({
        type: "POST",
        url: "/OMS/Management/Activities/Services/VendorPaymentAdjustment.asmx/GetDocumentList",
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

}

function GridAddnewRow() {
    grid.AddNewRow();
    grid.GetEditor("SrlNo").SetText(grid.GetVisibleItemsOnPage());
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
        DeleteAllRows();
        DocumentList = [];
        var numbSchm = cCmbScheme.GetValue();
        var splitData = numbSchm.split('~');
        var startNo = splitData[1];
        HeaderClear();
        $('#hdAddEdit').val("Add");
        enabledHeader();


        if (startNo != "1") {
            ctxtVoucherNo.SetText("");
            ctxtVoucherNo.Focus();
        }
        else {
            ctxtVoucherNo.SetText("Auto");
            ctxtVoucherNo.SetEnabled(false);
            ctxtVendName.Focus();
        }

    }
    else {
        window.location.href = 'VendorPaymentAdjustmentList.aspx';
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


