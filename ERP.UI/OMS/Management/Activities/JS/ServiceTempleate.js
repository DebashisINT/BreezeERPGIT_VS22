/*****************
Global variable*/

var ReceiptList = [];
var globalRowindex = 0;
var DocumentList = [];
var canCallBack = true;
var PickedDocument = [];
var saveNewOrExit = '';
var alertShow = false;
$(document).ready(function () {

    if ($("#hdAddEdit").val() == "Add")
    {
        GridAddnewRow();
    }
     


    //$('#AdvanceModel').on('shown.bs.modal', function () {
    //    if ($("input[receiptIndex=0]"))
    //        $("input[receiptIndex=0]").focus();
    //})

    $('#ProductModel').on('shown.bs.modal', function () {
        if ($("input[ProdIndex=0]"))
            $("input[ProdIndex=0]").focus();
    })

     $('#ServiceItemModel').on('shown.bs.modal', function () {
         $('#txtServiceItemSearch').focus();
    })


});
function AddBatchNew(s, e) {
    var keyCode = ASPxClientUtils.GetKeyCode(e.htmlEvent);
    if ((keyCode === 13)) {
        grid.batchEditApi.StartEdit(globalRowindex);
        var Product = (grid.GetEditor('ProductName').GetValue() != null) ? grid.GetEditor('ProductName').GetValue() : "";      

        if (Product != "") {
            grid.batchEditApi.EndEdit();
            grid.AddNewRow();
           // RowCount = RowCount + 1;
            grid.GetEditor("SrlNo").SetText(grid.GetVisibleItemsOnPage());
            grid.GetEditor("ActualSL").SetText(grid.GetVisibleItemsOnPage());

        }
        else {
            jAlert("Select Product Name.");
        }        
    }

}

function SetLostFocusonDemand(e) {
    if ((new Date($("#hdnLockFromDate").val()) <= cdtTDate.GetDate()) && (cdtTDate.GetDate() <= new Date($("#hdnLockToDate").val()))) {
        jAlert("DATA is Freezed between   " + $("#hdnLockFromDateCon").val() + " to " + $("#hdnLockToDateCon").val() + " for Add.");
    }
}

function ServiceItemButnClick(s, e) {    
    $('#ServiceItemModel').modal('show');
}
function ServiceItemKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
        $('#ServiceItemModel').modal('show');
    }
}

function DocumentNumberBtnClick(s, e) {
    if (GetObjectID('hdnCustomerId').value) {
        $('#AdvanceModel').modal('show');
        var ProjectSelectInEntry = GetObjectID('hdnProjectSelectInEntryModule').value;
        if (ProjectSelectInEntry == 1) {
            var htmlScript = "<table class='dynamicPopupTbl' border='1' width='100%'> <tr class='HeaderStyle'>  <th>Journal No.</th> <th>Journal Date</th> <th>Amount</th> <th>Balance</th><th>Project Code</th></tr>";
            for (var rp = 0; rp < ReceiptList.length; rp++) {
                htmlScript += "<tr> <td><input readonly onclick='ReceiptonClick(" + ReceiptList[rp].ArId + ")' type='text' style='background-color: #3399520a;'receiptIndex=" + rp + " onfocus='ReceiptGetFocus(event)'  onblur='ReceiptlostFocus(event)' onkeydown=ReceiptSelected(event," + ReceiptList[rp].ArId + ") width='100%' readonly value='" + ReceiptList[rp].docNo + "'/></td><td  onclick='ReceiptonClick(" + ReceiptList[rp].ArId + ")'>" + ReceiptList[rp].docDt + "</td><td onclick='ReceiptonClick(" + ReceiptList[rp].ArId + ")'>" + GetTwodecimalValue(ReceiptList[rp].ActAmt) + "</td><td onclick='ReceiptonClick(" + ReceiptList[rp].ArId + ")'>" + GetTwodecimalValue(ReceiptList[rp].avlAmt) + "</td><td onclick='ReceiptonClick(" + ReceiptList[rp].ArId + ")'>" + ReceiptList[rp].Proj_Code + "</td></tr>";
            }
        }
        else {
            var htmlScript = "<table class='dynamicPopupTbl' border='1' width='100%'> <tr class='HeaderStyle'>  <th>Journal No.</th> <th>Journal Date</th> <th>Amount</th> <th>Balance</th></tr>";
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

function ServiceItemkeydown(e) {

    var OtherDetails = {}
    OtherDetails.SearchKey = $("#txtServiceItemSearch").val();

    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var HeaderCaption = [];
        HeaderCaption.push("Product Code");
        HeaderCaption.push("Product Description");
        HeaderCaption.push("Inventory");
        HeaderCaption.push("HSN/SAC");
        HeaderCaption.push("Class");
        HeaderCaption.push("Brand");
        if ($("#txtCustSearch").val() != '') {
            callonServer("Services/Master.asmx/GetServiceProduct", OtherDetails, "ServiceItemTable", HeaderCaption, "customerIndex", "SetServiceItem");
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
                SetServiceItem(Id, name);
            else if (indexName == "ProdIndex") {
                SetProduct(Id, code, name);
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
            if (indexName == "customerIndex")
                $('#txtCustSearch').focus();
        }
    }

}
function SetServiceItem(Id, Name) {
    if (Id) {
        $('#ServiceItemModel').modal('hide');
        ctxServiceItemName.SetText(Name);
        GetObjectID('hdnServiceProductId').value = Id;
        //LoadCustomerReceipt();
        //cbtntxtDocNo.SetText('');
        //DocumentList = [];
        DeleteAllRows();
        ctxtQuantity.Focus();
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
        url: "/OMS/Management/Activities/Services/CustomerReceiptAdjustment.asmx/GetJournalVoucharList",
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
        // jAlert("Adjusted Amount can not be more than O/S Amount.", "Alert", function () { cAdjAmt.SetValue(cOsAmt.GetValue()); });
        cAdjAmt.SetValue(cOsAmt.GetValue());
        jAlert("Adjusted Amount can not be more than O/S Amount.", "Alert");

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
    grid.GetEditor("SrlNo").SetText(grid.GetVisibleItemsOnPage());
    grid.GetEditor("ActualSL").SetText(grid.GetVisibleItemsOnPage());
    
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
            ctxtServiceDescription.Focus();

        } else {
            //showDocumentList();
            //CreateDocumentList();
            SuffleRows();
            cbtnSaveRecords.SetVisible(false);
            cbtn_SaveRecords.SetVisible(true);
            ctxtNotes.Focus();
        }

        canCallBack = false;
    }
}
function SuffleRows() {
    for (var i = 0; i < 1000; i++) {
        if (grid.GetRow(i)) {
            if (grid.GetRow(i).style.display != "none") {
                grid.batchEditApi.StartEdit(i, 2);
                grid.GetEditor("ActualSL").SetText(grid.GetEditor("ActualSL").GetText() + i);
            }
        }
    }

    for (i = -1; i > -1000; i--) {
        if (grid.GetRow(i)) {
            if (grid.GetRow(i).style.display != "none") {
                grid.batchEditApi.StartEdit(i, 2);
                grid.GetEditor("ActualSL").SetText(grid.GetEditor("ActualSL").GetText() + i);
            }
        }
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
            grid.GetEditor("ActualSL").SetText(SlnoCount);
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
    grid.GetEditor("ActualSL").SetText(1);
}


function ValidateEntry() {
    var ReturnValue = true;
   

    if (GetObjectID('hdnServiceProductId').value.trim() == "") {
        $('#MandatoryVendor').show();
        return false;
    } else {
        $('#MandatoryVendor').hide();
    }

   
    for (var i = 0; i < 1000; i++) {
        if (grid.GetRow(i)) {
            if (grid.GetRow(i).style.display != "none") {
                grid.batchEditApi.StartEdit(i, 2);
                if (grid.GetEditor("ProductID").GetText() == "") {
                    //cLoadingPanelCRP.Hide();
                    jAlert("Please select a valid Product to proceed.");
                    return false;
                }              

                if ((grid.GetEditor("Quantity").GetText() == "0.0000" || grid.GetEditor("Quantity").GetText() == "")) {
                    //cLoadingPanelCRP.Hide();
                    jAlert("Please enter a valid Quantity to proceed.");
                    return false;
                }              

            }
        }
    }

    for (i = -1; i > -1000; i--) {
        if (grid.GetRow(i)) {
            if (grid.GetRow(i).style.display != "none") {
                grid.batchEditApi.StartEdit(i, 2);
                if (grid.GetEditor("ProductID").GetText() == "") {
                    //cLoadingPanelCRP.Hide();
                    jAlert("Please select a valid Product to proceed.");
                    return false;
                }
                
                if ((grid.GetEditor("Quantity").GetText() == "0.0000" || grid.GetEditor("Quantity").GetText() == "")) {
                    // cLoadingPanelCRP.Hide();
                    jAlert("Please enter a valid Quantity to proceed.");
                    return false;
                }
               
            }
        }
    }

    return ReturnValue;
}

function CmbScheme_ValueChange(s, e) {
    var numbSchm = s.GetValue();
    var splitData = numbSchm.split('~');
    var startNo = splitData[1];
    $('#ddlBranch').val(splitData[2]);

    //Cut Off  Valid from To Date Sudip

    var fromdate = numbSchm.toString().split('~')[3];
    var todate = numbSchm.toString().split('~')[4];
    //  alert(fromdate + '   ' + todate);
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


    //Cut Off  Valid from To Date Sudip


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
    if (ValidateEntry()) {
        if (!grid.InCallback()) {
            grid.AddNewRow();            
            grid.GetEditor("SrlNo").SetText(grid.GetVisibleItemsOnPage());
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
        window.location.href = 'ServiceTempleateAdd.aspx?Key=Add';       
        $('#hdAddEdit').val("Add");
       
    }
    else {
        window.location.href = 'ServiceTempleateList.aspx';
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

    return TotaAdj;
}

function GridEndCallBack(s, e) {
 
    alertShow = true;
    if (grid.cpErrorCode == "0") {
        jAlert("Document  Saved Successfully ", "Alert", function () { afterSave(); alertShow = false; });
    }    
    else {
        jAlert("Internal Server Error", "Alert", function () { grid.batchEditApi.StartEdit(-1, 2); grid.batchEditApi.StartEdit(0, 2); alertShow = false; });
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
    //HeaderClear();
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
            grid.AddNewRow();
            grid.GetEditor("SrlNo").SetText(grid.GetVisibleItemsOnPage());
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

function ProductButnClick(s, e) {
    if (e.buttonIndex == 0) {
        setTimeout(function () { $("#txtProdSearch").focus(); }, 500);
        $('#txtProdSearch').val('');
        $('#ProductModel').modal('show');
    }
}
function ProductKeyDown(s, e) {
    console.log(e.htmlEvent.key);
    if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
        s.OnButtonClick(0);
    }
    //if (e.htmlEvent.key == "Tab") {
    //    s.OnButtonClick(0);
    //}
}
function prodkeydown(e) {

    var OtherDetails = {}
    OtherDetails.SearchKey = $("#txtProdSearch").val();

    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var HeaderCaption = [];
        HeaderCaption.push("Product Code");
        HeaderCaption.push("Product Description");
        HeaderCaption.push("Inventory");
        HeaderCaption.push("HSN/SAC");
        HeaderCaption.push("Class");
        HeaderCaption.push("Brand");
        if ($("#txtProdSearch").val() != '') {
            callonServer("Services/Master.asmx/GetProductForWHStockTransfer", OtherDetails, "ProductTable", HeaderCaption, "ProdIndex", "SetProduct");
        }       

    }
    else if (e.code == "ArrowDown") {
        if ($("input[ProdIndex=0]"))
            $("input[ProdIndex=0]").focus();
    }
    else if (e.code == "Escape") {
       
        grid.batchEditApi.StartEdit(globalRowIndex, 2);
    }
}
function SetProduct(Id, code, name) {
    var ProductIDS = Id;
    var splitData = ProductIDS.split('||@||');
    $('#ProductModel').modal('hide');
    var strProductID = splitData[0];
    var LookUpData = Id;
    var ProductDescription = splitData[1];
    var ProductCode = code;
    if (!ProductCode) {
        LookUpData = null;
    }
    var SaleUOMname = splitData[2];

    grid.batchEditApi.StartEdit(globalRowindex);
    grid.GetEditor("ProductID").SetText(LookUpData);
    grid.GetEditor("ProductName").SetText(ProductCode);
    grid.GetEditor("Discription").SetText(ProductDescription);
    grid.GetEditor("UOM").SetText(SaleUOMname);

    grid.batchEditApi.EndEdit();
    grid.batchEditApi.StartEdit(globalRowindex, 4);
}
function RateTextChange(s, e) {
    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    var Rate = (grid.GetEditor('Rate').GetValue() != null) ? grid.GetEditor('Rate').GetValue() : "0";
    var amount = (QuantityValue * Rate);
    grid.GetEditor('Value').SetValue(amount);

    grid.batchEditApi.EndEdit();
    grid.batchEditApi.StartEdit(globalRowindex, 8);
}

function QuantityTextChange() {

    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    var Rate = (grid.GetEditor('Rate').GetValue() != null) ? grid.GetEditor('Rate').GetValue() : "0";
    var amount = (QuantityValue * Rate);
    grid.GetEditor('Value').SetValue(amount);

    grid.batchEditApi.EndEdit();
    grid.batchEditApi.StartEdit(globalRowindex, 6);
}
