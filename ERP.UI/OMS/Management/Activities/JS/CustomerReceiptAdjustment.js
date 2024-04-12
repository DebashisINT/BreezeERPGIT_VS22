//====================================================Revision History =========================================================================
//1.0   v2.0.38	Priti	22-06-2023	0025222: Auto calculation of Adjusted amount during Adjustment of Document Entries-Advance with Invoice
//2.0   v2.0.43	Priti	12-04-2024	0027361: Customer Adjustment of Documents - Advance With Invoice showing mismatch.
//====================================================End Revision History=====================================================================


/*****************
Global variable*/

var ReceiptList = [];
var globalRowindex = 0;
var DocumentList = [];
var canCallBack = true;
var PickedDocument = [];
var saveNewOrExit='';
var alertShow = false;
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
    

});

function SetLostFocusonDemand(e) {
    if ((new Date($("#hdnLockFromDate").val()) <= cdtTDate.GetDate()) && (cdtTDate.GetDate() <= new Date($("#hdnLockToDate").val()))) {
        jAlert("DATA is Freezed between   " + $("#hdnLockFromDateCon").val() + " to " + $("#hdnLockToDateCon").val() + " for Add.");
    }
}

function CustomerButnClick(s, e) {
    if (cCmbScheme.GetValue() == "0~1~0") {
            cCmbScheme.Focus();
        
    }
    else if ($("#ddlBranch").val() == "0") {
           $("#ddlBranch").focus();
       
    }
    else {
        $('#CustModel').modal('show');
    }
}
function CustomerKeyDown(s, e) {


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
            $('#CustModel').modal('show');
        }
    }
}

function DocumentNumberBtnClick(s, e) {
    if (GetObjectID('hdnCustomerId').value) {
        $('#AdvanceModel').modal('show');
        var ProjectSelectInEntry = GetObjectID('hdnProjectSelectInEntryModule').value;        
        if (ProjectSelectInEntry == 1) {
            var htmlScript = "<table class='dynamicPopupTbl' border='1' width='100%'> <tr class='HeaderStyle'>  <th>Receipt Number</th> <th>Receipt Date</th> <th>Amount</th> <th>Balance</th><th>Project Code</th></tr>";
            for (var rp = 0; rp < ReceiptList.length; rp++) {
                htmlScript += "<tr> <td><input readonly onclick='ReceiptonClick(" + ReceiptList[rp].ArId + ")' type='text' style='background-color: #3399520a;'receiptIndex=" + rp + " onfocus='ReceiptGetFocus(event)'  onblur='ReceiptlostFocus(event)' onkeydown=ReceiptSelected(event," + ReceiptList[rp].ArId + ") width='100%' readonly value='" + ReceiptList[rp].docNo + "'/></td><td  onclick='ReceiptonClick(" + ReceiptList[rp].ArId + ")'>" + ReceiptList[rp].docDt + "</td><td onclick='ReceiptonClick(" + ReceiptList[rp].ArId + ")'>" + GetTwodecimalValue(ReceiptList[rp].ActAmt) + "</td><td onclick='ReceiptonClick(" + ReceiptList[rp].ArId + ")'>" + GetTwodecimalValue(ReceiptList[rp].avlAmt) + "</td><td onclick='ReceiptonClick(" + ReceiptList[rp].ArId + ")'>" + ReceiptList[rp].Proj_Code + "</td></tr>";
            }

        }
        else {
            var htmlScript = "<table class='dynamicPopupTbl' border='1' width='100%'> <tr class='HeaderStyle'>  <th>Receipt Number</th> <th>Receipt Date</th> <th>Amount</th> <th>Balance</th></tr>";
            for (var rp = 0; rp < ReceiptList.length; rp++) {
                htmlScript += "<tr> <td><input readonly onclick='ReceiptonClick(" + ReceiptList[rp].ArId + ")' type='text' style='background-color: #3399520a;'receiptIndex=" + rp + " onfocus='ReceiptGetFocus(event)'  onblur='ReceiptlostFocus(event)' onkeydown=ReceiptSelected(event," + ReceiptList[rp].ArId + ") width='100%' readonly value='" + ReceiptList[rp].docNo + "'/></td><td  onclick='ReceiptonClick(" + ReceiptList[rp].ArId + ")'>" + ReceiptList[rp].docDt + "</td><td onclick='ReceiptonClick(" + ReceiptList[rp].ArId + ")'>" + GetTwodecimalValue(ReceiptList[rp].ActAmt) + "</td><td onclick='ReceiptonClick(" + ReceiptList[rp].ArId + ")'>" + GetTwodecimalValue(ReceiptList[rp].avlAmt) + "</td></tr>";
            }
        }
        htmlScript += ' </table>';
        document.getElementById('AdvRecDocTbl').innerHTML = htmlScript;

    } else {
        jAlert("Please select customer first.", "Alert", function () { ctxtCustName.Focus(); });
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
        cOsAmt.SetValue(receiptdetails[0].avlAmt);
        GetObjectID('hdAdvanceDocNo').value = receiptdetails[0].DocId;
        GetObjectID('hdAdjustmentType').value = receiptdetails[0].AdvType;
        GetObjectID('hddnProjectId').value = receiptdetails[0].Proj_Id;
        ctxtProject.SetText(receiptdetails[0].Proj_Code);
        ctxtHierarchy.SetText(receiptdetails[0].HIERARCHY_NAME);

        showDocumentList();
        cRemarks.Focus();
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
            callonServer("Services/Master.asmx/GetCustomer", OtherDetails, "CustomerTable", HeaderCaption, "customerIndex", "SetCustomer");
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
            if (indexName == "customerIndex")
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
        $('#CustModel').modal('hide');
        ctxtCustName.SetText(Name);
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
        url: "/OMS/Management/Activities/Services/CustomerReceiptAdjustment.asmx/GetAdvanceReceiptList",
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
        jAlert("Adjusted Amount can not be more than O/S Amount.", "Alert", function () { cAdjAmt.SetValue(cOsAmt.GetValue());  });
    }
}

function gridDocNobuttonClick() {
    var tempDocumentList = $.grep(DocumentList, function (e) { return !PickedDocument.includes(e.uniqueid); });

    if (tempDocumentList.length == 0) {
        jAlert("No document is available for adjustment", "Alert");
    } else {
        $('#DocumentModel').modal('show');



        var htmlScript = "<table class='dynamicPopupTbl' border='1' width='100%'> <tr class='HeaderStyle'>  <th>Document Number</th> <th>Document Date</th> <th>Document Type</th><th>Document Amount</th><th>Balance Amount</th></tr>";
        for (var rp = 0; rp < tempDocumentList.length; rp++) {
            htmlScript += "<tr> <td><input readonly onclick=DocuementClick('" + tempDocumentList[rp].uniqueid + "') type='text' style='background-color: #3399520a;'DocumentIndex=" + rp + " onfocus='DocumentGetFocus(event)'  onblur='DocumentlostFocus(event)' onkeydown=DocumnetSelected(event,'" + tempDocumentList[rp].uniqueid + "') width='100%' readonly value='" + tempDocumentList[rp].No + "'/></td><td  onclick=DocuementClick('" + tempDocumentList[rp].uniqueid + "')>" + tempDocumentList[rp].docDate + "</td><td onclick=DocuementClick('" + tempDocumentList[rp].uniqueid + "')>" + tempDocumentList[rp].doctype + "</td><td onclick=DocuementClick('" + tempDocumentList[rp].uniqueid + "')>" + GetTwodecimalValue(tempDocumentList[rp].actAmt) + "</td><td onclick=DocuementClick('" + tempDocumentList[rp].uniqueid + "')>" + GetTwodecimalValue(tempDocumentList[rp].unPdAmt) + "</td></tr>";
        }
        htmlScript += ' </table>';
        document.getElementById('DocNoDocTbl').innerHTML = htmlScript;


    }
}
function gridDocNoKeyDown(s,e) {
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
    OtherDetails.ReceiptId =  GetObjectID('hdAdvanceDocNo').value;
    OtherDetails.customerId = GetObjectID('hdnCustomerId').value;
    OtherDetails.TransDate = cdtTDate.date.format('yyyy-MM-dd');
    OtherDetails.AdjId = GetObjectID('hdAdjustmentId').value;
    OtherDetails.AdvType = GetObjectID('hdAdjustmentType').value;
    OtherDetails.BranchId = $("#ddlBranch").val();
    OtherDetails.ProjectId = GetObjectID('hddnProjectId').value;
    $.ajax({
        type: "POST",
        url: "/OMS/Management/Activities/Services/CustomerReceiptAdjustment.asmx/GetDocumentList",
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
    grid.GetEditor("AdjAmt").SetValue(0);
    ShowRunningTotal();
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
                    //grid.GetEditor("AdjAmt").SetValue(parseFloat(grid.GetEditor("OsAmt").GetValue() + parseFloat(cAdjAmt.GetValue()) - parseFloat(cOsAmt.GetValue())));
                    grid.GetEditor("AdjAmt").SetValue(parseFloat(cOsAmt.GetValue()));
                }
            }
            else {
                if (parseFloat(grid.GetEditor("OsAmt").GetValue()) < (parseFloat(cOsAmt.GetValue()) - parseFloat(cAdjAmt.GetValue()))) {
                    /* grid.GetEditor("AdjAmt").SetValue((parseFloat(cOsAmt.GetValue()) + parseFloat(cAdjAmt.GetValue())) -parseFloat(cOsAmt.GetValue()));*/
                    grid.GetEditor("AdjAmt").SetValue(parseFloat(grid.GetEditor("OsAmt").GetValue()));
                }
                else if (parseFloat(grid.GetEditor("OsAmt").GetValue()) > (parseFloat(cOsAmt.GetValue()) - parseFloat(cAdjAmt.GetValue()))) {
                    grid.GetEditor("AdjAmt").SetValue(parseFloat(cOsAmt.GetValue()) - parseFloat(cAdjAmt.GetValue()));

                    //if (parseFloat(cOsAmt.GetValue()) > (parseFloat(cOsAmt.GetValue()) + parseFloat(cAdjAmt.GetValue()))) {
                    //    grid.GetEditor("AdjAmt").SetValue(parseFloat(cOsAmt.GetValue()) - parseFloat(cAdjAmt.GetValue()));
                    //}
                    //else {
                    //    grid.GetEditor("AdjAmt").SetValue(parseFloat(grid.GetEditor("OsAmt").GetValue()) - (parseFloat(cOsAmt.GetValue()) + parseFloat(cAdjAmt.GetValue())));
                    //}
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
}


function GetVisibleIndex(s, e) {
    globalRowindex = e.visibleIndex;
}
function gridFocusedRowChanged(s, e) {
    globalRowindex = e.visibleIndex;
}
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
    //if (parseFloat(cAdjAmt.GetValue()) < parseFloat(cOsAmt.GetValue())) {
        var TotAmt = DecimalRoundoff(GetTotalAmount(), 2);
        cAdjAmt.SetValue(TotAmt.toString());
    //}
}
function gridAdjustAmtLostFocus(s, e) {
    console.log('1');
    if (parseFloat(grid.GetEditor("OsAmt").GetValue()) < s.GetValue()) {

        grid.batchEditApi.StartEdit(globalRowindex, 8);
        var NewAmt = grid.GetEditor("OsAmt");
        s.SetValue(NewAmt.GetValue());
        jAlert("Adjusted Amount can not be more than O/S Amount.", "Alert");
       

       
    }
    grid.GetEditor("RemainingBalance").SetValue(parseFloat(grid.GetEditor("OsAmt").GetValue()) - s.GetValue());

    var AutocalculationAdjustmentInvoice = GetObjectID('hdnAutocalculationAdjustmentInvoice').value;
    if (AutocalculationAdjustmentInvoice == 1) {
        ShowRunningTotal();
    }
    
}

function GridAddnewRow() {
    grid.AddNewRow();
    grid.GetEditor("SrlNo").SetText(grid.GetVisibleItemsOnPage());
}

function gridCustomButtonClick(s,e) {
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
     
    for (i = -1; i >-1000; i--) {
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
    if (parseFloat(cAdjAmt.GetValue())<=0) {
      
        jAlert("Adjusted Amount must be greater than zero.", "Alert", function () { cAdjAmt.Focus(); });
        return false;
    }
    //REV 2.0
    //if (parseFloat(cAdjAmt.GetValue()) != GetTotalAdjustedAmount()) {
    if (parseFloat(cAdjAmt.GetValue()) != DecimalRoundoff(GetTotalAmount(), 2)) {  
    //REV 2.0 END
        jAlert("Mismatch detected in Adjusted Amount and Adjustment Amount.", "Alert", function () { cAdjAmt.Focus(); });
        return false;
    }
    
    return ReturnValue;
}

function CmbScheme_ValueChange(s,e) {
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
    if (ValidateEntry())
    {
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
            ctxtCustName.Focus();
        }
        
    }
    else {
        window.location.href = 'CustomerReceiptAdjustmentList.aspx';
    }
}



function GetTotalAdjustedAmount() {
    var TotaAdj = 0; 
    for (var i = 0; i < 1000; i++) {
        if (grid.GetRow(i)) {
            if (grid.GetRow(i).style.display != "none") {
                grid.batchEditApi.StartEdit(i, 2);
                TotaAdj = TotaAdj + parseFloat(grid.GetEditor("AdjAmt").GetValue()).toFixed(2);
            }
        }
    }

    for (i = -1; i > -1000; i--) {
        if (grid.GetRow(i)) {
            grid.batchEditApi.StartEdit(i, 2);
            TotaAdj = TotaAdj + parseFloat(grid.GetEditor("AdjAmt").GetValue()).toFixed(2);
            
        }
    }
     
    return TotaAdj;
}

function GridEndCallBack(s, e) {

    console.log(s.cpErrorCode.toString());
    alertShow = true;
    if (grid.cpErrorCode == "0") {
        jAlert(grid.cpadjustmentNumber, "Alert", function () { afterSave(); alertShow = false;});
    }
    else if(grid.cpErrorCode == "-9")
    {
        jAlert('DATA is Freezed between ' + grid.cpAddLockStatus + ' for Add.');
        grid.cpAddLockStatus = null;
        grid.cpErrorCode = null;
    }
    else {
        jAlert(grid.cpadjustmentNumber, "Alert", function () { grid.batchEditApi.StartEdit(-1, 2); grid.batchEditApi.StartEdit(0, 2); alertShow = false;});
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
    saveNewOrExit = 'E';
    if (ValidateEntry()) {
        if (!grid.InCallback()) {
            grid.UpdateEdit();
        }
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


