var CanCallback = true;
var debitOldValue = 0;
var debitNewValue = 0;
var Note_Id;
var globalRowIndex;
var RowIndex;


function AllControlInitilize() {
    if (CanCallback) {


        CanCallback = false;

        if ($('#hdAddEdit').val() == "Add") {
            ctxtVoucherNo.SetText("");
            document.getElementById("divNumberingScheme").style.display = "block";
            cdtTDate.SetEnabled(true);
            ctxtCustName.SetEnabled(true);
            cbtnSaveNew.SetVisible(true);
            cbtnSaveRecords.SetVisible(true);
            $("#DoEdit").val("1");
            SetNumberingSchemeDataSource();
            cddlBranch.SetEnabled(false);
            setTimeout(function () {
                cCmbScheme.Focus();
            }, 500);


        } else if ($('#hdAddEdit').val() == "Edit") {

            document.getElementById("divNumberingScheme").style.display = "none";
            cdtTDate.SetEnabled(false);
            ctxtVoucherNo.SetEnabled(false);
            cCmbScheme.SetEnabled(false);
            cddlBranch.SetEnabled(false);
            cddlNoteType.SetEnabled(false);

            document.getElementById("TxtHeaded").innerHTML = "Modify Customer Debit/Credit Note";
            $("#DoEdit").val("2");
            //ctxtCustName.SetEnabled(false);
            cbtnSaveNew.SetVisible(false);
            Note_Id = grid.GetVisibleRowsOnPage();
            SuffleRows();
            SetTotalSummary();

            if ($("#hdnTagCount").val() != "0") {
                cbtnSaveRecords.SetVisible(false);
                $("#DoEdit").val("");
            }
            var customerId = GetObjectID('hdnCustomerId').value;
            if ($('#hdnDocumentSegmentSettings').val() == "1") {

                $.ajax({
                    type: "POST",
                    url: "SalesOrderAdd.aspx/GetSegmentDetails",
                    data: JSON.stringify({ CustomerId: customerId }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (msg) {
                        OutStandingAmount = msg.d;
                        if (OutStandingAmount != null) {


                            if (OutStandingAmount.Segment1 != "") {
                                var Segment1 = OutStandingAmount.Segment1;
                                var Segment2 = OutStandingAmount.Segment2;
                                var Segment3 = OutStandingAmount.Segment3;
                                var Segment4 = OutStandingAmount.Segment4;
                                var Segment5 = OutStandingAmount.Segment5;

                                if (Segment1 == "0") {
                                    var div = document.getElementById('DivSegment1');
                                    div.style.display = 'none';
                                    $('#hdnValueSegment1').val("0");
                                }
                                else {
                                    $('#ModuleSegment1header').text(OutStandingAmount.SegmentName1);
                                    $('#hdnValueSegment1').val("1");
                                }
                                if (Segment2 == "0") {
                                    var div = document.getElementById('DivSegment2');
                                    div.style.display = 'none';
                                    $('#hdnValueSegment2').val("0");
                                }
                                else {
                                    $('#ModuleSegment2Header').text(OutStandingAmount.SegmentName2);
                                    $('#hdnValueSegment2').val("1");
                                }

                                if (Segment3 == "0") {
                                    var div = document.getElementById('DivSegment3');
                                    div.style.display = 'none';
                                    $('#hdnValueSegment3').val("0");
                                }
                                else {
                                    $('#ModuleSegment3Header').text(OutStandingAmount.SegmentName3);
                                    $('#hdnValueSegment3').val("1");
                                }

                                if (Segment4 == "0") {
                                    var div = document.getElementById('DivSegment4');
                                    div.style.display = 'none';
                                    $('#hdnValueSegment4').val("0");
                                }
                                else {
                                    $('#ModuleSegment4Header').text(OutStandingAmount.SegmentName4);
                                    $('#hdnValueSegment4').val("1");
                                }

                                if (Segment5 == "0") {
                                    var div = document.getElementById('DivSegment5');
                                    div.style.display = 'none';
                                    $('#hdnValueSegment5').val("0");
                                }
                                else {
                                    $('#ModuleSegment5Header').text(OutStandingAmount.SegmentName5);
                                    $('#hdnValueSegment5').val("1");
                                }

                            }
                        }
                        else {

                            document.getElementById('DivSegment1').style.display = 'none';
                            document.getElementById('DivSegment2').style.display = 'none';
                            document.getElementById('DivSegment3').style.display = 'none';
                            document.getElementById('DivSegment4').style.display = 'none';
                            document.getElementById('DivSegment5').style.display = 'none';
                        }
                    }
                });
            }
            

        }
        else if ($('#hdAddEdit').val() == "View") {

            document.getElementById("divNumberingScheme").style.display = "none";
            cdtTDate.SetEnabled(false);
            ctxtVoucherNo.SetEnabled(false);
            cCmbScheme.SetEnabled(false);
            cddlBranch.SetEnabled(false);
            cddlNoteType.SetEnabled(false);

            document.getElementById("TxtHeaded").innerHTML = "View Customer Debit/Credit Note";

            //ctxtCustName.SetEnabled(false);
            cbtnSaveNew.SetVisible(false);
            cbtnSaveRecords.SetVisible(false);
            Note_Id = grid.GetVisibleRowsOnPage();
            SuffleRows();
            SetTotalSummary();
        }



        GridAddnewRow();

    }
}

function Posting_LostFocus()
{
    if ((new Date($("#hdnLockFromDate").val()) <= cdtTDate.GetDate()) && (cdtTDate.GetDate() <= new Date($("#hdnLockToDate").val()))) {
        jAlert("DATA is Freezed between   " + $("#hdnLockFromDateCon").val() + " to " + $("#hdnLockToDateCon").val() + " for Add.");
    }
    ctxtCustName.SetFocus();

}

function HeaderClear() {
    cdtTDate.SetEnabled(true);
    ctxtCustName.SetEnabled(true);
    ctxtCustName.SetText("");
    GetObjectID('hdnCustomerId').value = "";
    $("#txtNarration").val("");
    txtPartyInvoice.SetText('');
    cPLPartyDate.SetText('');
    cddlInvoice.SetValue('0');
    cddl_Reason.SetValue('0');
    $('#hdAddEdit').val("Add");
    SetDefaultBillingShippingAddress("");
    Note_Id = 1;
}

function SetNumberingSchemeDataSource() {
    var OtherDetails = {}
    OtherDetails.VoucherType = cddlNoteType.GetValue();
    $.ajax({
        type: "POST",
        url: "../Activities/Services/CustomerNote.asmx/GetNumberingSchemeByType",
        data: JSON.stringify(OtherDetails),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var returnObject = msg.d;

            if (returnObject.NumberingSchema) {
                SetDataSourceOnComboBox(cCmbScheme, returnObject.NumberingSchema);
            }
            if (returnObject.ForBranch) {
                SetDataSourceOnComboBox(cddlBranch, returnObject.ForBranch);
            }
            if (returnObject.Currency) {

                SetDataSourceOnComboBox(cddlCurrency, returnObject.Currency);
            }
            if (returnObject.reason) {

                SetDataSourceOnComboBox(cddl_Reason, returnObject.reason);
            }
            if (returnObject.SatrtDate) {
                var dtStart = new Date(parseInt(returnObject.SatrtDate.substr(6)));
                cdtTDate.SetMinDate(dtStart);
                //cInstDate.SetMinDate(dtStart);
            }

            if (returnObject.EndDate) {
                var dtEnd = new Date(parseInt(returnObject.EndDate.substr(6)));
                cdtTDate.SetMaxDate(dtEnd);
                //cInstDate.SetMaxDate(dtEnd);
                var today = new Date();
                if (dtEnd > today) {
                    cdtTDate.SetDate(today);
                }
                else {
                    cdtTDate.SetDate(dtEnd);
                }
            }
            if (returnObject.SysSetting) {
                $("#SysSetting").val(returnObject.SysSetting);
            }

            if (returnObject.UDFCount) {
                $("#IsUdfpresent").val(returnObject.UDFCount);
            }




        }
    });
}

function disp_prompt(name) {
    $("#shippingcustomer").css("display", "none");
    if (name == "tab0") {
        ctxtCustName.Focus();
    }
    if (name == "tab1") {
    }
}

function OnEndCallback() {
    cLoadingPanelCRP.Hide();
    if (grid.cpInsert != null) {
        var Output = grid.cpInsert;
        var outputText = Output.split("~")[0];
        var outputValue = Output.split("~")[1];
        var refreshType = Output.split("~")[2];
        //Rev v1.0.101  subhra  07-01-2019  0019425
        var AutoPrint = document.getElementById('hdnAutoPrint').value;
        var DCNoteID = grid.cpCrDrNoteId;
        //End Of Rev
        if (parseFloat(outputText) > 0) {
            if (refreshType == "N") {
                //Rev v1.0.101  subhra  07-01-2019  0019425
                if (AutoPrint == "Yes") {
                    if (grid.cpTransactionType=='Dr') {
                        var reportName = 'CustDrNote-Branch~D'
                        var module = 'CUSTDRCRNOTE'
                        window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + DCNoteID, '_blank')
                    }
                    else if (grid.cpTransactionType == 'Cr') {
                        var reportName = 'CustCrNote-Branch~D'
                        var module = 'CUSTDRCRNOTE'
                        window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + DCNoteID, '_blank')
                    }
                  
                }
                //End Of Rev
                jAlert(outputValue);

                HeaderClear();
                deleteAllRows();
                var schemetypeValue = cCmbScheme.GetValue();
                var startNo = schemetypeValue.split("~")[1];

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
               
                    //Rev v1.0.101  subhra  07-01-2019  0019425
                    if (AutoPrint == "Yes") {
                        if (grid.cpTransactionType == 'Dr') {
                            var reportName = 'CustDrNote-Branch~D'
                            var module = 'CUSTDRCRNOTE'
                            window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + DCNoteID, '_blank')
                        }
                        else if (grid.cpTransactionType == 'Cr') {
                            var reportName = 'CustCrNote-Branch~D'
                            var module = 'CUSTDRCRNOTE'
                            window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + DCNoteID, '_blank')
                        }

                    }
                    //End Of Rev
                    jAlert(outputValue, "Success", function () {
                        window.location.href = 'CustomerNoteList.aspx';
                    });

            }
        }
        else if (parseInt(outputText) == -12)
        {
            SuffleRows();
            jAlert("Please select project");
        }
        else if (outputText == "-9") {
           jAlert("DATA is Freezed between " + grid.cpAddLockStatus + " for Add.");
        }
        else if (parseInt(outputText) == 0)
        {
            if (grid.cpSaveSuccessOrFail != null)
            {
                if (grid.cpSaveSuccessOrFail = "checkAcurateTaxAmount")
                {
                    grid.cpSaveSuccessOrFail = null;
                   // jAlert('Check GST Calculated for Main Account ' + grid.cpProductName + ' at line ' + grid.cpSerialNo);
                    jAlert('Check GST Calculated for Main Account ' + grid.cpProductName);
                    grid.cpSaveSuccessOrFail = '';
                    grid.cpSerialNo = '';
                    grid.cpProductName = '';
                }
            }
            
        }
        else {
            SuffleRows();
            jAlert(outputValue);
        }


    }
}

function OnBatchEditStartEditing(s, e) {
    globalRowIndex = e.visibleIndex;
}

function OnCustomButtonClick() {

}

function GetVisibleIndex() {

}

function OnInit() {

}
function MainAccountKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter") {
        shouldCheck = 0;
        s.OnButtonClick(0);
    }
    //if (e.htmlEvent.key == "Tab") {

    //    s.OnButtonClick(0);
    //}
}
function SubAccountKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter") {
        s.OnButtonClick(0);
    }
    if (e.htmlEvent.key == "Delete") {
        var subAccountText = "";
        var subAccountID = "";

        grid.batchEditApi.StartEdit(globalRowIndex);


        var VoucherType = document.getElementById('rbtnType').value;
        if (VoucherType == "P") {
            setTimeout(function () { grid.batchEditApi.StartEdit(globalRowIndex, 5); }, 500);

        }
        else {
            setTimeout(function () { grid.batchEditApi.StartEdit(globalRowIndex, 4); }, 500);
        }
        grid.GetEditor("bthSubAccount").SetText(subAccountText);
        grid.GetEditor("gvColSubAccount").SetText(subAccountID);



    }
}
function MainAccountButnClick(s, e) {
    RowIndex = globalRowIndex;
    if (e.buttonIndex == 0) {

       var txt = "<table border='1' width=\"100%\"  class=\"dynamicPopupTbl\"><tr class=\"HeaderStyle\" ><th>Main Account Name</th><th>Subledger Type</th><th>Reverse Applicable</th><th>HSN/SAC</th></tr><table>";
        document.getElementById("MainAccountTable").innerHTML = txt;
        $('#MainAccountModel').modal('show');

        $("#txtMainAccountSearch").focus();

    }
}

function closeModal() {
    $('#MainAccountModel').modal('hide');
    grid.batchEditApi.StartEdit(globalRowIndex, 2);
}

function SubAccountButnClick(s, e) {




    RowIndex = globalRowIndex;
    txt = " <table border='1' width=\"100%\"  class=\"dynamicPopupTbl\"><tr class=\"HeaderStyle\"><th>Sub Account Name [Unique Id]</th><th>Sub Account Code</th></tr></table>";
    document.getElementById("SubAccountTable").innerHTML = txt;
    grid.batchEditApi.StartEdit(e.visibleIndex);
    IsSubAccount = (grid.GetEditor('IsSubledger').GetText() != null) ? grid.GetEditor('IsSubledger').GetText() : "None";
    $("#mainActMsgSub").hide();
    if (IsSubAccount != 'None') {
        
        var strMainAccountID = (grid.GetEditor('MainAccount').GetText() != null) ? grid.GetEditor('MainAccount').GetText() : "0";
        var MainAccountID = (grid.GetEditor('gvColMainAccount').GetValue() != null) ? grid.GetEditor('gvColMainAccount').GetValue() : "0";
        if (e.buttonIndex == 0) {
            if (strMainAccountID.trim() != "") {

                $('#SubAccountModel').modal('show');

            }
        }
    }
}
function SubAccountNewkeydown(e) {
    grid.batchEditApi.StartEdit(e.visibleIndex);
    var strMainAccountID = (grid.GetEditor('MainAccount').GetText() != null) ? grid.GetEditor('MainAccount').GetText() : "0";
    var MainAccountID = (grid.GetEditor('gvColMainAccount').GetValue() != null) ? grid.GetEditor('gvColMainAccount').GetValue() : "0";

    var OtherDetails = {}
    OtherDetails.SearchKey = $("#txtSubAccountSearch").val();
    OtherDetails.MainAccountCode = MainAccountID;
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        if ($("#txtSubAccountSearch").val() == "")
            return;
        var HeaderCaption = [];
        HeaderCaption.push("Sub Account Name [Unique Id]");
        HeaderCaption.push("Subledger Type");

        callonServer("/OMS/Management/Activities/Services/Master.asmx/GetSubAccountJournal", OtherDetails, "SubAccountTable", HeaderCaption, "SubAccountIndex", "SetSubAccount");
    }
    else if (e.code == "ArrowDown") {
        if ($("input[SubAccountIndex=0]"))
            $("input[SubAccountIndex=0]").focus();
    }
    else if (e.code == "Escape") {
        $('#SubAccountModel').modal('hide');
        grid.batchEditApi.StartEdit(globalRowIndex, 3);
    }
}
function SetSubAccount(Id, name) {
    $('#SubAccountModel').modal('hide');
    GetSubAcountComboBox(Id, name);
    grid.batchEditApi.StartEdit(globalRowIndex, 4);
}

function DebitGotFocus(s, e) {
    debitOldValue = s.GetText();
    rowIndex = globalRowIndex;

    var indx = debitOldValue.indexOf(',');
    if (indx != -1) {
        debitOldValue = debitOldValue.replace(/,/g, '');
    }
}

function DebitLostFocus(s, e) {
    debitNewValue = s.GetText();

    var indx = debitNewValue.indexOf(',');
    if (indx != -1) {
        debitNewValue = debitNewValue.replace(/,/g, '');
    }
    if (debitOldValue != debitNewValue) {
        grid.GetEditor('TaxAmount').SetValue("0");

        var Note_id = grid.GetEditor('Note_Id').GetText();

        cdeleteTax.PerformCallback("DeleteInlineTax~" + Note_id);

    }


    var Amount = grid.GetEditor("btnRecieve").GetText();
    var amountAfterDiscount = grid.GetEditor("btnRecieve").GetText();
    var stateCode = $('#hdStateIdShipping').val();

    if (stateCode == "" && grid.GetEditor("HSNCODE").GetText() != "") {

        jAlert('Please select valid shipping state to proceed.', function () {
            return;
        });

    }

    caluculateAndSetGST(grid.GetEditor("btnRecieve"), grid.GetEditor("TaxAmount"), grid.GetEditor("NetAmount"), grid.GetEditor("HSNCODE").GetText(), Amount, amountAfterDiscount, "E", stateCode, cddlBranch.GetValue())
    grid.GetEditor('NetAmount').SetValue(parseFloat(debitNewValue) + parseFloat(grid.GetEditor("TaxAmount").GetText()));

    SetTotalSummary();

    setTimeout(function () { grid.batchEditApi.StartEdit(globalRowIndex, 5); }, 200);

}

function OnKeyDown(s, e) {
    if (e.htmlEvent.keyCode == 40 || e.htmlEvent.keyCode == 38)
        return ASPxClientUtils.PreventEvent(e.htmlEvent);
}



function TaxAmountKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter") {
        s.OnButtonClick(0);
    }
}

function CmbScheme_ValueChange() {
    deleteAllRows();


    var schemetypeValue = cCmbScheme.GetValue();
    var schemeID;
    var schemetype;
    var schemelength;
    var branchID;
    var Type;
    if (schemetypeValue != "" && schemetypeValue != null) {
        schemeID = schemetypeValue.toString().split('~')[0];
        schemetype = schemetypeValue.toString().split('~')[1];
        schemelength = schemetypeValue.toString().split('~')[2];
        branchID = schemetypeValue.toString().split('~')[3];
        Type = schemetypeValue.toString().split('~')[4];


        var fromdate = schemetypeValue.toString().split('~')[4];
        var todate = schemetypeValue.toString().split('~')[5];

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



        cddlBranch.SetValue(branchID);
        $("#hdnEnterBranch").val(branchID);


        if (schemetype == '0') {


            ctxtVoucherNo.SetEnabled(true);
            ctxtVoucherNo.SetText("");

            setTimeout(function () {
                ctxtVoucherNo.SetFocus();
            }, 200);




        }
        else if (schemetype == '1') {

            ctxtVoucherNo.SetEnabled(false);
            ctxtVoucherNo.SetText("Auto");
            cdtTDate.Focus();
        }
        else if (schemetype == '2') {

            ctxtVoucherNo.SetEnabled(true);
            ctxtVoucherNo.SetText("Datewise");
        }
        else if (schemetype == 'n') {
            ctxtVoucherNo.SetEnabled(true);
            ctxtVoucherNo.SetText("");
        }
        else {
            ctxtVoucherNo.SetEnabled(false);
            ctxtVoucherNo.SetText("");

            setTimeout(function () {
                cCmbScheme.SetFocus();
            }, 200);
        }

        //cddlCashBank.PerformCallback(branchID);
    }
    clookup_Project.gridView.Refresh();
}

function SetDataSourceOnComboBox(ControlObject, Source) {
    ControlObject.ClearItems();
    for (var count = 0; count < Source.length; count++) {
        ControlObject.AddItem(Source[count].Name, Source[count].Id);
    }
    ControlObject.SetSelectedIndex(0);
}

function GridAddnewRow() {
    grid.AddNewRow();
    grid.GetEditor("SrlNo").SetText(grid.GetVisibleItemsOnPage());



    if (Note_Id == "" || Note_Id == null) {
        Note_Id = 1;
    }
    else {
        Note_Id = parseInt(Note_Id) + 1;
    }

    grid.GetEditor("Note_Id").SetText(parseInt(Note_Id));

    grid.batchEditApi.EndEdit();

    setTimeout(function () {
        grid.batchEditApi.EndEdit();
        grid.batchEditApi.StartEdit(globalRowIndex, 2);
    }, 200);
}

function ddlBranch_Change() {

}

function Type_Changed() {
    SetNumberingSchemeDataSource();
    var val = cddlNoteType.GetValue();

    if (val == "Cr") {
        document.getElementById('div_InvoiceNo').style.display = 'block';
        document.getElementById('div_InvoiceDate').style.display = 'block';
    }
    else {
        document.getElementById('div_InvoiceNo').style.display = 'none';
        document.getElementById('div_InvoiceDate').style.display = 'none';
    }
}


function deleteAllRows() {
    var TotRowNumber = grid.GetVisibleItemsOnPage();

    for (var i = 0; i < 1000; i++) {
        if (grid.GetRow(i)) {
            if (grid.GetRow(i).style.display != "none") {
                grid.DeleteRow(i);
            }
        }
    }

    for (i = -1; i > -1000; i--) {
        if (grid.GetRow(i)) {
            if (grid.GetRow(i).style.display != "none") {
                grid.DeleteRow(i);
            }
        }
    }
    grid.AddNewRow();
    c_txt_Debit.SetValue(0);
    grid.GetEditor("SrlNo").SetText(1);

    if (Note_Id == "" || Note_Id == null) {
        Note_Id = 1;
    }
    else {
        Note_Id = Note_Id + 1;
    }
    grid.GetEditor("UpdateEdit").SetText(Note_Id + 1);
    grid.GetEditor("Note_Id").SetText(parseInt(Note_Id));
    SetTotalSummary();
}

function Customer_KeyDown(s, e) {
    if (e.htmlEvent.key == "Enter" || e.htmlEvent.key == "NumpadEnter") {
        if ((cCmbScheme.GetValue() != "0" && cCmbScheme.GetValue() != null && cCmbScheme.GetValue() != "undefined") || $('#hdAddEdit').val() != "Add") {
            var txt = "<table border='1' width='100%' class='dynamicPopupTbl'> <tr class='HeaderStyle'><th class='hide'>id</th><th>Name</th><th>Unique Id</th><th>Address</th></tr></table>";
            document.getElementById("CustomerTable").innerHTML = txt;

            var Contacttype = "CL";
            if (Contacttype.toUpperCase() == "CL")
                $('#CustModel').find('.modal-title').text("Customer Search");
            else
                $('#CustModel').find('.modal-title').text("Vendor Search");

            $('#CustModel').modal('show');
        }
        else
            setTimeout(function () { jAlert("Please select numbering scheme to select customer."); }, 500);
    }
}
function CustomerButnClick(s, e) {

    if ((cCmbScheme.GetValue() != "0" && cCmbScheme.GetValue() != null && cCmbScheme.GetValue() != "undefined") || $('#hdAddEdit').val() != "Add") {
        var Contacttype = "CL";
        if (Contacttype.toUpperCase() == "CL")
            $('#CustModel').find('.modal-title').text("Customer Search");
        else
            $('#CustModel').find('.modal-title').text("Vendor Search");

        var txt = "<table border='1' width='100%' class='dynamicPopupTbl'> <tr class='HeaderStyle'><th class='hide'>id</th><th>Name</th><th>Unique Id</th><th>Address</th></tr></table>";
        document.getElementById("CustomerTable").innerHTML = txt;

        $('#CustModel').modal('show');
    }
    else
        setTimeout(function () { jAlert("Please select numbering scheme to select customer."); }, 500);
}


function Customerkeydown(e) {
    var OtherDetails = {}


    OtherDetails.SearchKey = $("#txtCustSearch").val();
    OtherDetails.contactType = "CL";


    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var HeaderCaption = [];
        HeaderCaption.push("Customer Name");
        HeaderCaption.push("Unique Id");
        HeaderCaption.push("Address");
        if ($("#txtCustSearch").val() != "") {
            callonServer("Services/Master.asmx/GetCustomerCRP", OtherDetails, "CustomerTable", HeaderCaption, "customerIndex", "SetCustomer");
        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[customerindex=0]"))
            $("input[customerindex=0]").focus();
    }
}
function SetCustomer(Id, Name) {
    var key = Id;
    if (key != null && key != '') {
        deleteAllRows();
        ctxtCustName.SetText(Name);
        GetObjectID('hdnCustomerId').value = key;
        $('#CustModel').modal('hide');
        SetDefaultBillingShippingAddress(GetObjectID('hdnCustomerId').value);
        page.SetActiveTabIndex(1);

        if ($("#hdnProjectSelectInEntryModule").val() == "1")
        {
            var ProjectId = (clookup_Project.GetGridView().GetRowKey(clookup_Project.GetGridView().GetFocusedRowIndex()));
            if (ProjectId != null && clookup_Project.GetText() != "") {
                getSaleInvoiceForCustomerWithProject(GetObjectID('hdnCustomerId').value, ProjectId);
            }
            else
            {
                getSaleInvoiceForCustomer(GetObjectID('hdnCustomerId').value);
            }
        }
        else {
            getSaleInvoiceForCustomer(GetObjectID('hdnCustomerId').value);
        }
        if ($('#hdnDocumentSegmentSettings').val() == "1") {


            $.ajax({
                type: "POST",
                url: "SalesOrderAdd.aspx/GetSegmentDetails",
                data: JSON.stringify({ CustomerId: Id }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {
                    OutStandingAmount = msg.d;
                    if (OutStandingAmount != null) {
                        if (OutStandingAmount.Segment1 != "") {
                            var Segment1 = OutStandingAmount.Segment1;
                            var Segment2 = OutStandingAmount.Segment2;
                            var Segment3 = OutStandingAmount.Segment3;
                            var Segment4 = OutStandingAmount.Segment4;
                            var Segment5 = OutStandingAmount.Segment5;

                            if (Segment1 == "0") {
                                var div = document.getElementById('DivSegment1');
                                div.style.display = 'none';
                                $('#hdnValueSegment1').val("0");
                            }
                            else {
                                $('#ModuleSegment1header').text(OutStandingAmount.SegmentName1);
                                $('#hdnValueSegment1').val("1");
                            }
                            if (Segment2 == "0") {
                                var div = document.getElementById('DivSegment2');
                                div.style.display = 'none';
                                $('#hdnValueSegment2').val("0");
                            }
                            else {
                                $('#ModuleSegment2Header').text(OutStandingAmount.SegmentName2);
                                $('#hdnValueSegment2').val("1");
                            }

                            if (Segment3 == "0") {
                                var div = document.getElementById('DivSegment3');
                                div.style.display = 'none';
                                $('#hdnValueSegment3').val("0");
                            }
                            else {
                                $('#ModuleSegment3Header').text(OutStandingAmount.SegmentName3);
                                $('#hdnValueSegment3').val("1");
                            }

                            if (Segment4 == "0") {
                                var div = document.getElementById('DivSegment4');
                                div.style.display = 'none';
                                $('#hdnValueSegment4').val("0");
                            }
                            else {
                                $('#ModuleSegment4Header').text(OutStandingAmount.SegmentName4);
                                $('#hdnValueSegment4').val("1");
                            }

                            if (Segment5 == "0") {
                                var div = document.getElementById('DivSegment5');
                                div.style.display = 'none';
                                $('#hdnValueSegment5').val("0");
                            }
                            else {
                                $('#ModuleSegment5Header').text(OutStandingAmount.SegmentName5);
                                $('#hdnValueSegment5').val("1");
                            }
                        }
                    }
                    else {

                        document.getElementById('DivSegment1').style.display = 'none';
                        document.getElementById('DivSegment2').style.display = 'none';
                        document.getElementById('DivSegment3').style.display = 'none';
                        document.getElementById('DivSegment4').style.display = 'none';
                        document.getElementById('DivSegment5').style.display = 'none';
                    }
                }

            });

            $('#hdnCustomerId').val(Id);
            var OtherDetails = {}
            OtherDetails.SearchKey = $("#txtSegment1Search").val();
            OtherDetails.CustomerIds = Id;

            var HeaderCaption = [];
            HeaderCaption.push("Code");
            HeaderCaption.push("Name");
            callonServer("Services/Master.asmx/GetSegment1", OtherDetails, "Segment1Table", HeaderCaption, "segment1Index", "Setsegment1");
        }

        setTimeout(function () {
            // txtPartyInvoiceNo.SetFocus();
        }, 200);


    }
    clookup_Project.gridView.Refresh();
}


function getSaleInvoiceForCustomerWithProject(custId, ProjectId) {
    var branch = cddlBranch.GetValue();
    var OtherDetails = {};
    OtherDetails.customerId = custId;
    OtherDetails.TransDate = cdtTDate.GetDate();
    OtherDetails.BranchID = branch;
    OtherDetails.ProjectId = ProjectId;
    $.ajax({
        type: "POST",
        url: "../Activities/Services/CustomerNote.asmx/GetSaleInvoiceForCustomerWithProject",
        data: JSON.stringify(OtherDetails),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var returnObject = msg.d;
            SetDataSourceOnComboBox(cddlInvoice, returnObject);

        }
    });
}



function getSaleInvoiceForCustomer(custId) {
    var branch = cddlBranch.GetValue();
    var OtherDetails = {};
    OtherDetails.customerId = custId;
    OtherDetails.TransDate = cdtTDate.GetDate();
    OtherDetails.BranchID = branch;

    $.ajax({
        type: "POST",
        url: "../Activities/Services/CustomerNote.asmx/GetSaleInvoiceForCustomer",
        data: JSON.stringify(OtherDetails),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var returnObject = msg.d;
            SetDataSourceOnComboBox(cddlInvoice, returnObject);
       
        }
    });
}


function ValueSelected(e, indexName) {

    if (e.code == "Enter" || e.code == "NumpadEnter") {
        if (indexName == "customerIndex") {

            var Id = e.target.parentElement.parentElement.cells[0].innerText;
            var name = e.target.parentElement.parentElement.cells[1].children[0].value;
            if (Id) {
                SetCustomer(Id, name);

            }
        }
        else if (indexName == "MainAccountIndex") {
            var Id = e.target.parentElement.parentElement.cells[0].innerText;
            var name = e.target.parentElement.parentElement.cells[1].children[0].value;
            SetMainAccount(Id, name, e.target.parentElement);
        }

        else if (indexName == "SubAccountIndex") {
            var Id = e.target.parentElement.parentElement.cells[0].innerText;
            var name = e.target.parentElement.parentElement.cells[1].children[0].value;
            GetSubAcountComboBox(Id, name);
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
        if (thisindex > -1) {
            $("input[" + indexName + "=" + thisindex + "]").focus();
        }
        else {

            if (indexName == "customerIndex")
                $('#txtCustSearch').focus();
            else if (indexName == "MainAccountIndex")
                $('#txtMainAccountSearch').focus();
            else if (indexName == "SubAccountIndex")
                $('#txtSubAccountSearch').focus();
        }
    }
}


var IsSubAccount = '';
function MainAccountNewkeydown(e) {
    var OtherDetails = {}
    OtherDetails.SearchKey = $("#txtMainAccountSearch").val();
    OtherDetails.branchId = $("#ddlBranch").val();
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        if ($("#txtMainAccountSearch").val() == "")
            return;
        var HeaderCaption = [];
        HeaderCaption.push("Main Account Name");
        HeaderCaption.push("Subledger Type");
        HeaderCaption.push("Reverse Applicable");
        HeaderCaption.push("HSN/SAC");

        callonServer("/OMS/Management/Activities/Services/Master.asmx/GetMainAccountCashBank", OtherDetails, "MainAccountTable", HeaderCaption, "MainAccountIndex", "SetMainAccount");
    }
    else if (e.code == "ArrowDown") {
        if ($("input[MainAccountIndex=0]"))
            $("input[MainAccountIndex=0]").focus();
    }
    else if (e.code == "Escape") {
        //  
        $('#MainAccountModel').modal('hide');
        grid.batchEditApi.StartEdit(globalRowIndex, 1);

    }
}


function SetMainAccount(Id, name, e) {

    $('#MainAccountModel').modal('hide');

    var IsSub = e.parentElement.cells[2].innerText;
    var RevApp = e.parentElement.cells[3].innerText;
    if (RevApp == 'Yes') {
        RevApp = '1';
    }
    else {
        RevApp = '0';
    }
    var TaxAble = e.parentElement.cells[4].innerText;
    GetMainAcountComboBox(Id, name, IsSub, RevApp, TaxAble);
    grid.batchEditApi.StartEdit(globalRowIndex, 2);
   
}

function GetMainAcountComboBox(Id, name, IsSub, RevApp, TaxAble) {
    var MainAccountText = name;

    IsSubAccount = IsSub;
    var MainAccountID = Id;//cMainAccountComboBox.GetValue();
    var ReverseApplicable = RevApp; //cMainAccountComboBox.GetSelectedItem().texts[2];
    var TaxApplicable = TaxAble;// cMainAccountComboBox.GetSelectedItem().texts[3];
    grid.batchEditApi.StartEdit(globalRowIndex);
    grid.batchEditApi.StartEdit(globalRowIndex, 3);
    grid.GetEditor("MainAccount").SetText(MainAccountText);
    grid.GetEditor("gvColMainAccount").SetText(MainAccountID);
    shouldCheck = 0;
    grid.GetEditor("bthSubAccount").SetValue("");
    grid.GetEditor("btnRecieve").SetValue("");
    grid.GetEditor("btnRecieve").SetValue("");




    var prevAmount = grid.GetEditor("NetAmount").GetText();
    grid.GetEditor("TaxAmount").SetValue("0.00");
    grid.GetEditor("NetAmount").SetValue("0.00");
    grid.GetEditor("gvColSubAccount").SetValue("");
    grid.GetEditor("IsSubledger").SetValue(IsSubAccount);
    grid.GetEditor("HSNCODE").SetValue(TaxApplicable);
    SetTotalSummary();


    setTimeout(function () {
        grid.batchEditApi.StartEdit(RowIndex, 3);
    }, 200);

}

function GetVisibleIndex(s, e) {
    globalRowIndex = e.visibleIndex;
}
function OnCustomButtonClick(s, e) {

    if (e.buttonID == 'AddNew') {
        grid.batchEditApi.StartEdit(e.visibleIndex);
        var mainAccountValue = (grid.GetEditor('MainAccount').GetValue() != null) ? grid.GetEditor('MainAccount').GetValue() : "";
        var btnRecieve = (grid.GetEditor('btnRecieve').GetValue() != null) ? grid.GetEditor('btnRecieve').GetValue() : "";
        if (mainAccountValue != "" && btnRecieve != "0.0") {
            grid.SetFocusedRowIndex();
            GridAddnewRow();
        }
    }
    if (e.buttonID == 'CustomDelete') {
        if (grid.GetVisibleRowsOnPage() > 1) {
            grid.batchEditApi.StartEdit(e.visibleIndex);
            grid.DeleteRow(e.visibleIndex);
            var IndexNo = globalRowIndex;
            SuffuleSerialNumber();
            SetTotalSummary();
            setTimeout(function () {
                S
                grid.batchEditApi.StartEdit(globalRowIndex, 2);
            }, 200);

        }
    }
}
function AfterSaveBillingShipiing(validate) {
    if (validate) {
        page.SetActiveTabIndex(0);
        page.tabs[0].SetEnabled(true);
        $("#divcross").show();

    }
    else {
        page.SetActiveTabIndex(1);
        page.tabs[0].SetEnabled(false);
        $("#divcross").hide();
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


function SetTotalSummary() {

    var TotRowNumber = grid.GetVisibleItemsOnPage() + 100;
    var TaxableAmount = 0;
    var TaxAmount = 0;
    var NetAmount = 0;



    for (var i = 0; i < TotRowNumber; i++) {
        if (grid.GetRow(i)) {
            if (grid.GetRow(i).style.display != "none") {
                grid.batchEditApi.StartEdit(i, 2);

                
                TaxAmount = TaxAmount + DecimalRoundoff(grid.GetEditor("TaxAmount").GetText(), 2);

                if (DecimalRoundoff(grid.GetEditor("TaxAmount").GetText(), 2) > 0)
                TaxableAmount = TaxableAmount + DecimalRoundoff(grid.GetEditor("btnRecieve").GetText(), 2);

                NetAmount = NetAmount + DecimalRoundoff(grid.GetEditor("NetAmount").GetText(), 2);
            }
        }
    }

    for (i = -1; i > -TotRowNumber; i--) {
        if (grid.GetRow(i)) {
            if (grid.GetRow(i).style.display != "none") {
                grid.batchEditApi.StartEdit(i, 2);

                TaxAmount = TaxAmount + DecimalRoundoff(grid.GetEditor("TaxAmount").GetText(), 2);

                if (DecimalRoundoff(grid.GetEditor("TaxAmount").GetText(), 2) > 0)
                    TaxableAmount = TaxableAmount + DecimalRoundoff(grid.GetEditor("btnRecieve").GetText(), 2);



                NetAmount = NetAmount + DecimalRoundoff(grid.GetEditor("NetAmount").GetText(), 2);
            }
        }
    }

    c_txtTaxableAmount.SetText(TaxableAmount.toString());
    c_txtTaxAmount.SetText(TaxAmount.toString());
    c_txt_Debit.SetText(NetAmount.toString());
}

function GetSubAcountComboBox(Id, Name) {
    var subAccountText = Name;
    var subAccountID = Id;
    grid.batchEditApi.StartEdit(globalRowIndex, 3);
    grid.GetEditor("bthSubAccount").SetText(subAccountText);
    grid.GetEditor("gvColSubAccount").SetText(subAccountID);
    grid.batchEditApi.StartEdit(globalRowIndex, 3);
    CloseSubModal();

    setTimeout(function () {
        grid.batchEditApi.StartEdit(globalRowIndex, 4);
    }, 200);

}

function CloseSubModal() {
    $('#SubAccountModel').modal('hide');
    grid.batchEditApi.StartEdit(globalRowIndex, 2);

}

$(document).ready(function () {
    $('#MainAccountModel').on('shown.bs.modal', function () {
        $('#txtMainAccountSearch').val("");
        $('#txtMainAccountSearch').focus();
    })
    $('#SubAccountModel').on('shown.bs.modal', function () {
        $('#txtSubAccountSearch').val("");
        $('#txtSubAccountSearch').focus();
    })
    $('#SubAccountModel').on('hide.bs.modal', function () {

        //grid.batchEditApi.StartEdit(globalRowIndex, 2);
    })
});


//TAX SECTION
function taxAmtButnClick(s, e) {


    if (grid.GetEditor("HSNCODE").GetText() == "") {
        return;
    }


    var TaxAmountOngrid = grid.GetEditor("TaxAmount").GetValue();
    $("#TaxAmountOngrid").val(TaxAmountOngrid);
    $("#VisibleIndexForTax").val(globalRowIndex);
    if (e.buttonIndex == 0) {
        grid.batchEditApi.StartEdit(e.visibleIndex);
        var shippingStCode = '';


        //  shippingStCode = cbsSCmbState.GetText();

        shippingStCode = GeteShippingStateCode();
        document.getElementById('HdSerialNo1').value = grid.GetEditor('Note_Id').GetValue();

        if (shippingStCode != '') {
            showTax();
        }
        else {

            if ($("#IsTaxApplicable").val() != "" && $("#IsTaxApplicable").val() != null) {
                jAlert("Please Enter Billing/Shipping Details to Calculate GST.", "Alert !!", function () {
                    page.SetActiveTabIndex(1);
                    cbsSave_BillingShipping.Focus();
                    page.tabs[0].SetEnabled(false);
                    $("#divcross").hide();
                });
            }

            else {
                showTax();
            }

        }
    }
}
function RecalCulateTaxTotalAmountInline() {
    var totalInlineTaxAmount = 0;
    for (var i = 0; i < taxJson.length; i++) {
        cgridTax.batchEditApi.StartEdit(i, 3);
        var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
        var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
        if (sign == '(+)') {
            totalInlineTaxAmount = totalInlineTaxAmount + parseFloat(cgridTax.GetEditor("Amount").GetValue());
        } else {
            totalInlineTaxAmount = totalInlineTaxAmount - parseFloat(cgridTax.GetEditor("Amount").GetValue());
        }

        cgridTax.batchEditApi.EndEdit();
    }
    totalInlineTaxAmount = totalInlineTaxAmount + parseFloat(0);
    ctxtTaxTotAmt.SetValue(totalInlineTaxAmount, 2);

}
function taxAmountLostFocus(s, e) {
    var finalTaxAmt = parseFloat(s.GetValue());
    var totAmt = parseFloat(ctxtTaxTotAmt.GetText());
    var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
    var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
    if (sign == '(+)') {
        ctxtTaxTotAmt.SetValue(((totAmt + finalTaxAmt - taxAmountGlobal), 2));
    } else {
        ctxtTaxTotAmt.SetValue(((totAmt + (finalTaxAmt * -1) - (taxAmountGlobal * -1)), 2));
    }
    // SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''));

    SetRunningTotal();
    RecalCulateTaxTotalAmountInline();
}
function txtPercentageLostFocus(s, e) {

    var ProdAmt = parseFloat(cgridTax.GetEditor("calCulatedOn").GetValue());
    if (s.GetText().trim() != '') {
        if (!isNaN(parseFloat(ProdAmt * s.GetText()) / 100)) {
            var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
            var taxNameWithSign = cgridTax.GetEditor("TaxField").GetText();
            var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
            if (sign == '(+)') {
                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * s.GetText()) / 100);
                ctxtTaxTotAmt.SetValue((parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt), 2);
                GlobalCurTaxAmt = 0;
            }
            else {

                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                cgridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * s.GetText()) / 100) * -1);
                ctxtTaxTotAmt.SetValue((parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) * -1) - (GlobalCurTaxAmt * -1), 2));
                GlobalCurTaxAmt = 0;
            }
            //  SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''));


            SetRunningTotal();
        } else {
            s.SetText("");
        }
    }
    RecalCulateTaxTotalAmountInline();
}
function showTax() {
    SrlNo = grid.batchEditApi.GetCellValue(globalRowIndex, 'Note_Id');
    var strMainAccountID = (grid.GetEditor('MainAccount').GetText() != null) ? grid.GetEditor('MainAccount').GetText() : "0";
    var MainAccountID = (grid.GetEditor('gvColMainAccount').GetValue() != null) ? grid.GetEditor('gvColMainAccount').GetValue() : "0";
    var StrAmount = "0";

    var NetAmount = (grid.GetEditor('NetAmount').GetValue() != null) ? parseFloat(grid.GetEditor('NetAmount').GetValue()) : "0";
    var TaxAmount = (grid.GetEditor('TaxAmount').GetValue() != null) ? parseFloat(grid.GetEditor('TaxAmount').GetValue()) : "0";
    var ActualAmount = (grid.GetEditor('btnRecieve').GetValue() != null) ? parseFloat(grid.GetEditor('btnRecieve').GetValue()) : "0";
    //StrAmount = parseFloat(NetAmount) - parseFloat(TaxAmount);
    StrAmount = ActualAmount;
    if (strMainAccountID.trim() != "") {
        globalNetAmount = parseFloat(StrAmount);
        document.getElementById('setCurrentProdCode').value = MainAccountID;
        document.getElementById('HdSerialNo').value = SrlNo + 1;
        var strSrlNo = SrlNo + 1;
        SrlNo = strSrlNo;
        ctxtTaxTotAmt.SetValue(0);
        $('.RecalculateInline').hide();
        caspxTaxpopUp.Show();
        var Amount = (Math.round(StrAmount * 100) / 100).toFixed(2);
        clblTaxProdGrossAmt.SetText(Amount);
        clblProdNetAmt.SetText(Amount);
        document.getElementById('HdProdGrossAmt').value = Amount;
        document.getElementById('HdProdNetAmt').value = Amount;
        clblTaxDiscount.SetText('0.00');

        $('.GstCstvatClass').show();
        $('.gstGrossAmount').hide();
        $('.gstNetAmount').hide();
        clblTaxableGross.SetText("");
        clblTaxableNet.SetText("");
        var shippingStCode = '';
        // shippingStCode = cbsSCmbState.GetText();

        shippingStCode = GeteShippingStateCode();
        //shippingStCode = shippingStCode.substring(shippingStCode.lastIndexOf('(')).replace('(State Code:', '').replace(')', '').trim();

       

        if (globalRowIndex > -1) {
            cgridTax.PerformCallback('New~' + "1");
        }
        else {
            cgridTax.PerformCallback('New~' + "1");
        }
        ctxtprodBasicAmt.SetValue(Amount);
    }
}

function CmbtaxClick(s, e) {
    
    gstcstvatGlobalName = s.GetText();
}
function GetTaxVisibleIndex(s, e) {
    globalTaxRowIndex = e.visibleIndex;
}
function taxAmountGotFocus(s, e) {
    taxAmountGlobal = parseFloat(s.GetValue());
}
function chargeCmbtaxClick(s, e) {
    ChargegstcstvatGlobalName = s.GetText();
}
function ShowTaxPopUp(type) {
    if (type == "IY") {
        $('#ContentErrorMsg').hide();
        $('#content-6').show();
        document.getElementById('calculateTotalAmountOK').style.display = 'block';
        
        if (cgridTax.GetVisibleRowsOnPage() < 1) {
            $('.cgridTaxClass').hide();

        } else {
            $('.cgridTaxClass').show();
        }

        
    }
    if (type == "IN") {
        $('#ErrorMsgCharges').hide();
        $('#content-5').show();

        
        if (gridTax.GetVisibleRowsOnPage() < 1) {
            $('.gridTaxClass').hide();

        } else {
            $('.gridTaxClass').show();
        }

    }
}
function cgridTax_EndCallBack(s, e) {
    $("#TaxAmountOngrid").val("");
    $("#VisibleIndexForTax").val("");

    //cgridTax.batchEditApi.StartEdit(0);

    cgridTax.batchEditApi.StartEdit(0, 4);
    cgridTax.GetEditor('Amount').SetValue(cgridTax.GetEditor('Amount').GetValue());

    $('.cgridTaxClass').show();
    //cgridTax.batchEditApi.StartEdit(0,1);
    //check Json data
    if (cgridTax.cpJsonData) {
        if (cgridTax.cpJsonData != "") {
            taxJson = JSON.parse(cgridTax.cpJsonData);
            cgridTax.cpJsonData = null;
        }
    }
    //End Here

    if (cgridTax.cpUpdated.split('~')[0] == 'ok') {
        ctxtTaxTotAmt.SetValue(cgridTax.cpUpdated.split('~')[1]);
        var gridValue = parseFloat(cgridTax.cpUpdated.split('~')[1]);
        var ddValue = 0;
        ctxtTaxTotAmt.SetValue(gridValue + ddValue);
        cgridTax.cpUpdated = "";
        RecalCulateTaxTotalAmountInline();
    }
    else {
        var totAmt = ctxtTaxTotAmt.GetValue();
        caspxTaxpopUp.Hide();
        cgridTax.CancelEdit();
    }
    
    //Debjyoti Check where any Gst Present or not
    // If Not then hide the hole section
    SetRunningTotal();
    ShowTaxPopUp("IY");
}

function SetRunningTotal() {
    var runningTot = parseFloat(clblProdNetAmt.GetValue());
    for (var i = 0; i < taxJson.length; i++) {
        cgridTax.batchEditApi.StartEdit(i, 3);
        if (taxJson[i].applicableOn == "R") {
            cgridTax.GetEditor("calCulatedOn").SetValue(runningTot);
            var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
            var taxNameWithSign = cgridTax.GetEditor("TaxField").GetText();
            var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
            var ProdAmt = parseFloat(cgridTax.GetEditor("calCulatedOn").GetValue());
            var thisRunningAmt = 0;
            if (sign == '(+)') {
                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * cgridTax.GetEditor("TaxField").GetValue()) / 100);

                ctxtTaxTotAmt.SetValue(Math.round((parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(cgridTax.GetEditor("TaxField").GetValue())) / 100) - GlobalCurTaxAmt), 2));
                GlobalCurTaxAmt = 0;
            }
            else {

                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                cgridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * cgridTax.GetEditor("TaxField").GetValue()) / 100) * -1);

                ctxtTaxTotAmt.SetValue(Math.round((parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(cgridTax.GetEditor("TaxField").GetValue())) / 100) * -1) - (GlobalCurTaxAmt * -1)), 2));
                GlobalCurTaxAmt = 0;
            }
            //  SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''));
        }
        runningTot = runningTot + parseFloat(cgridTax.GetEditor("Amount").GetValue());
        cgridTax.batchEditApi.EndEdit();
    }
}

function calculateTotalAmount() {

    var TaxAmount = ctxtTaxTotAmt.GetValue();
    var Amount = grid.GetEditor("btnRecieve").GetValue();
    grid.batchEditApi.StartEdit(globalRowIndex, 7);
    var strNetAmount = parseFloat(TaxAmount) + parseFloat(Amount);
    var cashBankGridTaxAmount = grid.GetEditor("TaxAmount");
    cashBankGridTaxAmount.SetValue(TaxAmount);
    var NetAmountGrid = grid.GetEditor("NetAmount");
    NetAmountGrid.SetValue(strNetAmount.toFixed(2));
    c_txt_Debit.SetValue(strNetAmount.toFixed(2));
    document.getElementById('HdSerialNo').val = grid;
    if (cgridTax.GetVisibleRowsOnPage() > 0) {
        document.getElementById('HdSerialNo1').value = grid.GetEditor('Note_Id').GetText();
        grid.batchEditApi.EndEdit();
        cgridTax.batchEditApi.StartEdit(0, 4);
        cgridTax.GetEditor('Taxes_Name').SetValue(cgridTax.GetEditor('Taxes_Name').GetText() + '  ');
        cgridTax.UpdateEdit();
    }
   
    return false;
}

function deleteTaxEndCallBack() {

}


document.onkeydown = function (e) {
    if (event.keyCode == 83 && event.altKey == true && ($("#DoEdit").val() == "1")) {
        // StopDefaultAction(e);
        SaveButtonClickNew();//........Alt+N
    }
    else if (event.keyCode == 88 && event.altKey == true && ($("#DoEdit").val() == "1" || $("#DoEdit").val() == "2")) {
        SaveButtonClick();//........Alt+X
    }
    else if (event.keyCode == 85 && event.altKey == true) {
        OpenUdf();
    }
    else if (event.keyCode == 84 && event.altKey == true) {
        Save_TaxesClick();
    }
}

function SaveButtonClickNew() {

    var ProjectCode = clookup_Project.GetText();
    if ($("#hdnProjectSelectInEntryModule").val() == "1" && $("#hdnProjectMandatory").val() == "1" && ProjectCode == "") {
        jAlert("Please Select Project.");
       return false;
    }

    if (validation()) {
        $("#hdnRefreshType").val("N");
         grid.UpdateEdit();
                 }
}

function SaveButtonClick() {
    var ProjectCode = clookup_Project.GetText();
    if ($("#hdnProjectSelectInEntryModule").val() == "1" && $("#hdnProjectMandatory").val() == "1" && ProjectCode == "") {
        jAlert("Please Select Project.");
        return false;
    }

    if (validation()) {
        $("#hdnRefreshType").val("E");
        
                    grid.UpdateEdit();
       }
}

function validation() {
    var Valid = true;
    cLoadingPanelCRP.Show();

    if (ctxtVoucherNo.GetText().trim() == "") {
        cLoadingPanelCRP.Hide();
        $("#MandatoryBillNo").show();
        return false;
    }
    else {
        $("#MandatoryBillNo").hide();
    }
    var TransDate = cdtTDate.GetDate();
    if (TransDate == null) {
        cLoadingPanelCRP.Hide();
        $("#MandatoryTransDate").show();
        return false;
    }
    else {
        $("#MandatoryTransDate").hide();
    }
    var branch = cddlBranch.GetValue();
    if (branch == "") {
        cLoadingPanelCRP.Hide();
        $("#MandatoryBranch").show();
        return false;
    }
    else {
        $("#MandatoryBranch").hide();
    }
    var customerId = GetObjectID('hdnCustomerId').value;
    if (customerId == '' || customerId == null) {
        cLoadingPanelCRP.Hide();
        $('#MandatorysCustomer').attr('style', 'display:block');
        return false;
    }
    else {
        $('#MandatorysCustomer').attr('style', 'display:none');
    }





    if ((ctxtbillingPin.GetText() == "" || ctxtShippingPin.GetText() == "" || ctxtAddress1.GetText() == "" || ctxtsAddress1.GetText() == "")) {
        jAlert("Please select a valid address to  proceed.", function () {
            cLoadingPanelCRP.Hide();
            page.SetActiveTabIndex(1);

        });

        return false;
    }


    grid.batchEditApi.EndEdit();
    var gridCount = grid.GetVisibleRowsOnPage();
    var TotRowNumber = grid.GetVisibleItemsOnPage();

    for (var i = 0; i < 1000; i++) {
        if (grid.GetRow(i)) {
            if (grid.GetRow(i).style.display != "none") {
                grid.batchEditApi.StartEdit(i, 2);
                if (grid.GetEditor("MainAccount").GetText() == "" && grid.GetEditor("btnRecieve").GetText() != "0.00" && grid.GetEditor("btnRecieve").GetText() != "") {
                    cLoadingPanelCRP.Hide();
                    jAlert("Please select a valid Main Account to proceed.");
                    return false;
                }

                if (grid.GetEditor("MainAccount").GetText() != "" && (grid.GetEditor("btnRecieve").GetText() == "0.00" || grid.GetEditor("btnRecieve").GetText() == "")) {
                    cLoadingPanelCRP.Hide();
                    jAlert("Please enter a valid amount to proceed.");
                    return false;
                }

                
            }
        }
    }

    for (i = -1; i > -1000; i--) {
        if (grid.GetRow(i)) {
            if (grid.GetRow(i).style.display != "none") {
                grid.batchEditApi.StartEdit(i, 2);
                if (grid.GetEditor("MainAccount").GetText() == "" && grid.GetEditor("btnRecieve").GetText() != "0.00" && grid.GetEditor("btnRecieve").GetText() != "") {
                    cLoadingPanelCRP.Hide();
                    jAlert("Please select a valid Main Account to proceed.");
                    return false;
                }

                if (grid.GetEditor("MainAccount").GetText() != "" && (grid.GetEditor("btnRecieve").GetText() == "0.00" || grid.GetEditor("btnRecieve").GetText() == "")) {
                    cLoadingPanelCRP.Hide();
                    jAlert("Please enter a valid amount to proceed.");
                    return false;
                }
            }
        }
    }

    var rowCount = grid.GetVisibleRowsOnPage();
    if (rowCount == 1) {
        for (var i = 0; i < 1000; i++) {
            if (grid.GetRow(i)) {
                if (grid.GetRow(i).style.display != "none") {
                    grid.batchEditApi.StartEdit(i, 2);
                    if (grid.GetEditor("MainAccount").GetText() == "") {
                        cLoadingPanelCRP.Hide();
                        jAlert("Please select atleast one main account to proceed.");
                        return false;
                    }
                    else {
                        if (grid.GetEditor("btnRecieve").GetText() == "0.00") {
                            cLoadingPanelCRP.Hide();
                            jAlert("Please enter a valid amount to proceed.");
                            return false;
                        }
                    }


                }
            }
        }

        for (i = -1; i > -1000; i--) {
            if (grid.GetRow(i)) {
                if (grid.GetRow(i).style.display != "none") {
                    grid.batchEditApi.StartEdit(i, 2);
                    if (grid.GetEditor("MainAccount").GetText() == "" ) {
                        cLoadingPanelCRP.Hide();
                        jAlert("Please select atleast one main account to proceed.");
                        return false;
                    }
                    else {
                        if (grid.GetEditor("btnRecieve").GetText() == "0.00") {
                            cLoadingPanelCRP.Hide();
                            jAlert("Please enter a valid amount to proceed.");
                            return false;
                        }
                    }
                }
            }
        }
    }

    if (!GridValidation())
    {
        cLoadingPanelCRP.Hide();
        jAlert("Duplicate Cutomer Ledger cannot be selected.");
        return false;
    }
    return Valid;
}

function SuffleRows() {
    for (var i = 0; i < 1000; i++) {
        if (grid.GetRow(i)) {
            if (grid.GetRow(i).style.display != "none") {
                grid.batchEditApi.StartEdit(i, 2);
                grid.GetEditor("UpdateEdit").SetText(grid.GetEditor("UpdateEdit").GetText() + i);
            }
        }
    }

    for (i = -1; i > -1000; i--) {
        if (grid.GetRow(i)) {
            if (grid.GetRow(i).style.display != "none") {
                grid.batchEditApi.StartEdit(i, 2);
                grid.GetEditor("UpdateEdit").SetText(grid.GetEditor("UpdateEdit").GetText() + i);
            }
        }
    }
}


function GridValidation()
{
  var  RepeatedRow = [];
  var validgrid = true;
       

    for (var i = 0; i < 1000; i++) {
        if (grid.GetRow(i)) {
            if (grid.GetRow(i).style.display != "none") {
                grid.batchEditApi.StartEdit(i, 2);
                if ((grid.GetEditor("gvColMainAccount").GetText().trim() != "") && (grid.GetEditor("gvColSubAccount").GetText().trim() != "")) {
                    var RepeatedRowCount = {};
                    RepeatedRowCount.MainAccount = grid.GetEditor("gvColMainAccount").GetText().trim();
                    RepeatedRowCount.SubAccount = grid.GetEditor("gvColSubAccount").GetText().trim();
                    RepeatedRowCount.IsSubLedger = grid.GetEditor("IsSubledger").GetText().trim();
                    RepeatedRow.push(RepeatedRowCount);
                }
               
            }
        }
    }

    for (i = -1; i > -1000; i--) {
        if (grid.GetRow(i)) {
            if (grid.GetRow(i).style.display != "none") {
                grid.batchEditApi.StartEdit(i, 2);
                if ((grid.GetEditor("gvColMainAccount").GetText().trim() != "") && (grid.GetEditor("gvColSubAccount").GetText().trim() != "")) {
                    var RepeatedRowCount = {};
                    RepeatedRowCount.MainAccount = grid.GetEditor("gvColMainAccount").GetText().trim();
                    RepeatedRowCount.SubAccount = grid.GetEditor("gvColSubAccount").GetText().trim();
                    RepeatedRowCount.IsSubLedger = grid.GetEditor("IsSubledger").GetText().trim();
                    RepeatedRow.push(RepeatedRowCount);
                }
            }
        }
    }

    var result = groupBy(RepeatedRow, function (item) {
        return [item.MainAccount, item.SubAccount];
    });


    if (result) {
        for (var i = 0; i < result.length; i++) {
            if (result[0].length > 1) {
                validgrid= false;
                break;
            }
        }
    }
    return validgrid;

}

function groupBy(array, f) {
    var groups = {};
    array.forEach(function (o) {
        var group = JSON.stringify(f(o));
        groups[group] = groups[group] || [];
        groups[group].push(o);
    });
    return Object.keys(groups).map(function (group) {
        return groups[group];
    })
}
function Segment1ButnClick(s, e) {
    if ($("#hdnCustomerId").val() != "") {
        $('#Segment1Model').modal('show');
    }
    else {
        jAlert("Please Select Customer");
    }

}
function Segment1keydown(e) {

    var OtherDetails = {}
    OtherDetails.SearchKey = $("#txtSegment1Search").val();
    OtherDetails.CustomerIds = $("#hdnCustomerId").val();
    if (e.code == "Enter" || e.code == "NumpadEnter") {

        var HeaderCaption = [];
        HeaderCaption.push("Code");
        HeaderCaption.push("Name");
        callonServer("Services/Master.asmx/GetSegment1", OtherDetails, "Segment1Table", HeaderCaption, "segment1Index", "Setsegment1");
    }
    else if (e.code == "ArrowDown") {
        if ($("input[segment1Index=0]"))
            $("input[segment1Index=0]").focus();
    }
    else if (e.code == "Escape") {
        ctxtSegment1.Focus();
    }
}

function Segment1_KeyDown(s, e) {
    if (e.htmlEvent.key == "Enter") {
        $('#Segment1Model').modal('show');
        $("#txtSegment1Search").focus();
    }
}

function Setsegment1(Id, Name, e) {

    var LookUpData = Id;
    var ProductCode = Name;
    if (!ProductCode) {
        LookUpData = null;
    }
    $('#Segment1Model').modal('hide');
    ctxtSegment1.SetText(ProductCode);
    $('#hdnSegment1').val(LookUpData);
    SetDefaultSegmentBillingShippingAddress($("#hdnCustomerId").val(), Id);

    if ($('#hdnValueSegment2').val() == "1") {
        var OtherDetails = {}
        OtherDetails.SearchKey = $("#txtSegment2Search").val();
        OtherDetails.CustomerIds = $("#hdnCustomerId").val();
        var HeaderCaption = [];
        HeaderCaption.push("Code");
        HeaderCaption.push("Name");
        callonServer("Services/Master.asmx/GetSegment2", OtherDetails, "Segment2Table", HeaderCaption, "segment2Index", "Setsegment2");
        $('#Segment2Model').modal('show');
    }




}
function Segment2ButnClick(s, e) {
    if ($("#hdnCustomerId").val() != "") {
        $('#Segment2Model').modal('show');
    }
    else {
        jAlert("Please Select Customer");
    }
}
function Segment2keydown(e) {

    var OtherDetails = {}
    OtherDetails.SearchKey = $("#txtSegment1Search").val();
    OtherDetails.CustomerIds = $("#hdnCustomerId").val();
    if (e.code == "Enter" || e.code == "NumpadEnter") {

        var HeaderCaption = [];
        HeaderCaption.push("Code");
        HeaderCaption.push("Name");
        callonServer("Services/Master.asmx/GetSegment2", OtherDetails, "Segment2Table", HeaderCaption, "segment2Index", "Setsegment2");
    }
    else if (e.code == "ArrowDown") {
        if ($("input[segment2Index=0]"))
            $("input[segment2Index=0]").focus();
    }
    else if (e.code == "Escape") {
        ctxtSegment2.Focus();
    }
}
function Segment2_KeyDown(s, e) {
    if (e.htmlEvent.key == "Enter") {
        $('#Segment2Model').modal('show');
        $("#txtSegment2Search").focus();
    }
}
function Setsegment2(Id, Name, e) {

    var LookUpData = Id;
    var ProductCode = Name;
    if (!ProductCode) {
        LookUpData = null;
    }
    $('#Segment2Model').modal('hide');
    ctxtSegment2.SetText(ProductCode);
    $('#hdnSegment2').val(LookUpData);

    SetDefaultSegmentBillingShippingAddress($("#hdnCustomerId").val(), Id);
    if ($('#hdnValueSegment3').val() == "1") {
        var OtherDetails = {}
        OtherDetails.SearchKey = $("#txtSegment1Search").val();
        OtherDetails.CustomerIds = $("#hdnCustomerId").val();
        var HeaderCaption = [];
        HeaderCaption.push("Code");
        HeaderCaption.push("Name");
        callonServer("Services/Master.asmx/GetSegment3", OtherDetails, "Segment3Table", HeaderCaption, "segment3Index", "Setsegment3");
        $('#Segment3Model').modal('show');
    }


}
function Segment3ButnClick(s, e) {
    if ($("#hdnCustomerId").val() != "") {
        $('#Segment3Model').modal('show');
    }
    else {
        jAlert("Please Select Customer");
    }

}
function Segment3keydown(e) {

    var OtherDetails = {}
    OtherDetails.SearchKey = $("#txtSegment3Search").val();
    OtherDetails.CustomerIds = $("#hdnCustomerId").val();
    if (e.code == "Enter" || e.code == "NumpadEnter") {

        var HeaderCaption = [];
        HeaderCaption.push("Code");
        HeaderCaption.push("Name");
        callonServer("Services/Master.asmx/GetSegment3", OtherDetails, "Segment3Table", HeaderCaption, "segment3Index", "Setsegment3");
    }
    else if (e.code == "ArrowDown") {
        if ($("input[segment3Index=0]"))
            $("input[segment3Index=0]").focus();
    }
    else if (e.code == "Escape") {
        ctxtSegment3.Focus();
    }
}
function Setsegment3(Id, Name, e) {

    var LookUpData = Id;
    var ProductCode = Name;
    if (!ProductCode) {
        LookUpData = null;
    }
    $('#Segment3Model').modal('hide');
    ctxtSegment3.SetText(ProductCode);
    $('#hdnSegment3').val(LookUpData);

    SetDefaultSegmentBillingShippingAddress($("#hdnCustomerId").val(), Id);
    if ($('#hdnValueSegment4').val() == "1") {
        var OtherDetails = {}
        OtherDetails.SearchKey = $("#txtSegment4Search").val();
        OtherDetails.CustomerIds = $("#hdnCustomerId").val();
        var HeaderCaption = [];
        HeaderCaption.push("Code");
        HeaderCaption.push("Name");
        callonServer("Services/Master.asmx/GetSegment4", OtherDetails, "Segment4Table", HeaderCaption, "segment4Index", "Setsegment4");
        $('#Segment4Model').modal('show');
    }


}
function Segment3_KeyDown(s, e) {
    if (e.htmlEvent.key == "Enter") {
        $('#Segment3Model').modal('show');
        $("#txtSegment3Search").focus();
    }
}
function Segment4ButnClick(s, e) {
    if ($("#hdnCustomerId").val() != "") {
        $('#Segment4Model').modal('show');
    }
    else {
        jAlert("Please Select Customer");
    }

}
function Segment4keydown(e) {

    var OtherDetails = {}
    OtherDetails.SearchKey = $("#txtSegment4Search").val();
    OtherDetails.CustomerIds = $("#hdnCustomerId").val();
    if (e.code == "Enter" || e.code == "NumpadEnter") {

        var HeaderCaption = [];
        HeaderCaption.push("Code");
        HeaderCaption.push("Name");
        callonServer("Services/Master.asmx/GetSegment4", OtherDetails, "Segment4Table", HeaderCaption, "segment4Index", "Setsegment4");
    }
    else if (e.code == "ArrowDown") {
        if ($("input[segment4Index=0]"))
            $("input[segment4Index=0]").focus();
    }
    else if (e.code == "Escape") {
        ctxtSegment4.Focus();
    }
}
function Setsegment4(Id, Name, e) {

    var LookUpData = Id;
    var ProductCode = Name;
    if (!ProductCode) {
        LookUpData = null;
    }
    $('#Segment4Model').modal('hide');
    ctxtSegment4.SetText(ProductCode);
    $('#hdnSegment4').val(LookUpData);
    SetDefaultSegmentBillingShippingAddress($("#hdnCustomerId").val(), Id);
    if ($('#hdnValueSegment5').val() == "1") {

        var OtherDetails = {}
        OtherDetails.SearchKey = $("#txtSegment5Search").val();
        OtherDetails.CustomerIds = $("#hdnCustomerId").val();
        var HeaderCaption = [];
        HeaderCaption.push("Code");
        HeaderCaption.push("Name");
        callonServer("Services/Master.asmx/GetSegment5", OtherDetails, "Segment5Table", HeaderCaption, "segment5Index", "Setsegment5");
        $('#Segment5Model').modal('show');
    }


}
function Segment4_KeyDown(s, e) {
    if (e.htmlEvent.key == "Enter") {
        $('#Segment4Model').modal('show');
        $("#txtSegment4Search").focus();
    }
}
function Segment5_KeyDown(s, e) {
    if (e.htmlEvent.key == "Enter") {
        $('#Segment5Model').modal('show');
        $("#txtSegment5Search").focus();
    }
}
function Segment5ButnClick(s, e) {
    if ($("#hdnCustomerId").val() != "") {
        $('#Segment5Model').modal('show');
    }
    else {
        jAlert("Please Select Customer");
    }

}
function Setsegment5(Id, Name, e) {

    var LookUpData = Id;
    var ProductCode = Name;
    if (!ProductCode) {
        LookUpData = null;
    }
    $('#Segment5Model').modal('hide');
    ctxtSegment5.SetText(ProductCode);
    $('#hdnSegment5').val(LookUpData);

    SetDefaultSegmentBillingShippingAddress($("#hdnCustomerId").val(), Id);
}
function Segment5keydown(e) {

    var OtherDetails = {}
    OtherDetails.SearchKey = $("#txtSegment5Search").val();
    OtherDetails.CustomerIds = $("#hdnCustomerId").val();
    if (e.code == "Enter" || e.code == "NumpadEnter") {

        var HeaderCaption = [];
        HeaderCaption.push("Code");
        HeaderCaption.push("Name");
        callonServer("Services/Master.asmx/GetSegment5", OtherDetails, "Segment5Table", HeaderCaption, "segment5Index", "Setsegment5");
    }
    else if (e.code == "ArrowDown") {
        if ($("input[segment5Index=0]"))
            $("input[segment5Index=0]").focus();
    }
    else if (e.code == "Escape") {
        ctxtSegment5.Focus();
    }
}

