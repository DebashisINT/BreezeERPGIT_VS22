var CanCallback = true;
var PaymentObj = [];
var DocObj = {};
var globalRowIndex;
var PickedDocument = [];
function PutDate(s, e) {
    var dt = new Date();
    cInstDate.SetDate(dt);
}
function AllControlInitilize() {
    if (CanCallback) {


        CanCallback = false;
        PaymentDetail_ID = 1;
        var obj = new Object;
        obj.ID = 'AdvancePayment';
        obj.TypeName = 'Advance Receipt';
        PaymentObj.push(obj);

        obj = new Object;
        obj.ID = 'OnAccountPay';
        obj.TypeName = 'On Account';
        PaymentObj.push(obj);


        obj = new Object;
        obj.ID = 'CustCrNote';
        obj.TypeName = 'Credit Note';
        PaymentObj.push(obj);


        if ($('#hdAddEdit').val() == "Add") {
            ctxtVoucherNo.SetText("");
            document.getElementById("divNumberingScheme").style.display = "block";
            cdtTDate.SetEnabled(true);
            ctxtCustName.SetEnabled(true);
            cbtnSaveNew.SetVisible(true);
            $("#DoEdit").val("1");
            //var dt = new Date();            
            //cInstDate.SetDate(dt);         

           
            SetNumberingSchemeDataSource();

            setTimeout(function () {
                cCmbScheme.Focus();
            }, 500);


        }
            /*Rev Work Date:-21.03.2022 -Copy Function add*/
        else if ($('#hdAddEdit').val() == "Copy") {
            ctxtVoucherNo.SetText("");
            clookup_Project.SetEnabled(true);
            document.getElementById("divNumberingScheme").style.display = "block";
            cdtTDate.SetEnabled(true);
            ctxtCustName.SetEnabled(true);
            cbtnSaveNew.SetVisible(true);
            $("#DoEdit").val("1");
            SetNumberingSchemeDataSource();

            LoadDocument();
            CreateDocumentList();
            CashBank_SelectedIndexChanged();
            cComboInstrumentTypee.SetValue($("#hdnInstrumentType").val().trim());
            ShowRunningTotal();
            selectValue();
            SuffleRows();
            PaymentDetail_ID = grid.GetVisibleRowsOnPage();
            ShowCustomerBalance();
            cddlBranch.SetEnabled(false);
        }
            /*Close of Rev Work Date:-21.03.2022 -Copy Function add*/
        else {
            clookup_Project.SetEnabled(true);
            document.getElementById("divNumberingScheme").style.display = "none";
            cdtTDate.SetEnabled(false);
            //ctxtCustName.SetEnabled(false);
            cbtnSaveNew.SetVisible(false);
            document.getElementById("TxtHeaded").innerHTML = "Modify Customer Payment";
            LoadDocument();
            CreateDocumentList();
            CashBank_SelectedIndexChanged();
            cComboInstrumentTypee.SetValue($("#hdnInstrumentType").val().trim());
            ShowRunningTotal();
            selectValue();
            SuffleRows();
            PaymentDetail_ID = grid.GetVisibleRowsOnPage();
            ShowCustomerBalance();
            cddlBranch.SetEnabled(false);
        }


        GridAddnewRow();

    }
}
function SetLostFocusonDemand(e) {
    if ((new Date($("#hdnLockFromDate").val()) <= cdtTDate.GetDate()) && (cdtTDate.GetDate() <= new Date($("#hdnLockToDate").val()))) {
        jAlert("DATA is Freezed between  " + $("#hdnLockFromDateCon").val() + " to " + $("#hdnLockToDateCon").val() + " for Add.");
    }
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

function cAdjDateChange() {

}

function ShowHideTab(evt, cityName) {
    var i, tabcontent, tablinks;
    tabcontent = document.getElementsByClassName("tabcontent");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }
    tablinks = document.getElementsByClassName("tablinks");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" active", "");
    }
    document.getElementById(cityName).style.display = "block";
    evt.currentTarget.className += " active";
}


function SetNumberingSchemeDataSource() {
    var OtherDetails = {}
    OtherDetails.VoucherType = "P";
    /*Rev Work Date:-21.03.2022 -Copy Function add*/
    var CopyType = $('#hrCopy').val();/*Get the request string value from hidden field of copy*/
    /*Close of Rev Work Date:-21.03.2022 -Copy Function add*/
    $.ajax({
        type: "POST",
        url: "../Activities/Services/CustomerReceiptPayment.asmx/GetNumberingSchemeByType",
        data: JSON.stringify(OtherDetails),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var returnObject = msg.d;

            if (returnObject.NumberingSchema) {
                SetDataSourceOnComboBox(cCmbScheme, returnObject.NumberingSchema);
            }
            /*Rev Work Date:-21.03.2022 -Copy Function add*/
            /* if (returnObject.ForBranch) {
                 SetDataSourceOnComboBox(cddlBranch, returnObject.ForBranch);
             }*/
            if (CopyType != 'Copy') {
                if (returnObject.ForBranch) {
                    SetDataSourceOnComboBox(cddlBranch, returnObject.ForBranch);
                }
            }
            /*Close of Rev Work Date:-21.03.2022 -Copy Function add*/
            if (returnObject.Currency) {

                SetDataSourceOnComboBox(cddlCurrency, returnObject.Currency);
            }
            if (returnObject.SatrtDate) {
                var dtStart = new Date(parseInt(returnObject.SatrtDate.substr(6)));
                cdtTDate.SetMinDate(dtStart);
               // cInstDate.SetMinDate(dtStart);
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


function CmbScheme_ValueChange() {
    /* Rev Work Date:-21.03.2022 -Copy Function add*/
    var CopyType = $('#hrCopy').val();/*Get the request string value from hidden field of copy*/
    if (CopyType != 'Copy') {
        deleteAllRows();
    }
    // deleteAllRows();
    /*Close of Rev Work Date:-21.03.2022 -Copy Function add*/


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
        cddlBranch.SetEnabled(false);
        $("#hdnEnterBranch").val(branchID);
        if (branchID != null && branchID != "" && branchID != "undefined") {
            loadMainAccountByBranchIdForPayDet(branchID);

            var OtherDetails = {}
            OtherDetails.userbranch = branchID;
            $.ajax({
                type: "POST",
                url: "../Activities/Services/CustomerReceiptPayment.asmx/BindCashBankAccountJson",
                data: JSON.stringify(OtherDetails),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var returnObject = msg.d;

                    if (returnObject) {
                        SetDataSourceOnComboBox(cddlCashBank, returnObject);
                        CashBank_SelectedIndexChanged();
                    }

                }
            });
        }
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
        clookup_Project.gridView.Refresh();
        //cddlCashBank.PerformCallback(branchID);
    }

}


//function deleteAllRows() {
//    var frontRow = 0;
//    var backRow = -1;
//    for (var i = 0; i <= grid.GetVisibleRowsOnPage() + 100 ; i++) {
//        grid.DeleteRow(frontRow);
//        grid.DeleteRow(backRow);
//        backRow--;
//        frontRow++;
//    }
//    grid.AddNewRow();
//   // ctxtTotalPayment.SetValue(0);
//    c_txt_Debit.SetValue(0);
//    ctxtVoucherAmount.SetValue(0);
//}



function SetDataSourceOnComboBox(ControlObject, Source) {
    ControlObject.ClearItems();
    for (var count = 0; count < Source.length; count++) {
        ControlObject.AddItem(Source[count].Name, Source[count].Id);
    }
    ControlObject.SetSelectedIndex(0);
}


function CashBank_SelectedIndexChanged() {
    $('#MandatoryCashBank').hide();
    var CashBankText = cddlCashBank.GetText();

    if (CashBankText == "") return;

    var SpliteDetails = CashBankText.split(']');
    var WithDrawType = SpliteDetails[1].trim();
    if (WithDrawType == "Cash") {
        var comboitem = cComboInstrumentTypee.FindItemByValue('C');
        if (comboitem != undefined && comboitem != null) {
            cComboInstrumentTypee.RemoveItem(comboitem.index);
        }
        var comboitem = cComboInstrumentTypee.FindItemByValue('D');
        if (comboitem != undefined && comboitem != null) {
            cComboInstrumentTypee.RemoveItem(comboitem.index);
        }
        var comboitem = cComboInstrumentTypee.FindItemByValue('E');
        if (comboitem != undefined && comboitem != null) {
            cComboInstrumentTypee.RemoveItem(comboitem.index);

        }
        var comboitem = cComboInstrumentTypee.FindItemByValue('CRD');
        if (comboitem != undefined && comboitem != null) {
            cComboInstrumentTypee.RemoveItem(comboitem.index);

        }
        var comboitem = cComboInstrumentTypee.FindItemByValue('CH');
        if (comboitem == undefined || comboitem == null) {
            cComboInstrumentTypee.AddItem("Cash", "CH");
        }
        cComboInstrumentTypee.SetValue("CH");
        InstrumentTypeSelectedIndexChanged();
    }
    else {
        var comboitem = cComboInstrumentTypee.FindItemByValue('C');
        if (comboitem == undefined || comboitem == null) {
            cComboInstrumentTypee.AddItem("Cheque", "C");
        }
        var comboitem = cComboInstrumentTypee.FindItemByValue('D');
        if (comboitem == undefined || comboitem == null) {
            cComboInstrumentTypee.AddItem("Draft", "D");
        }
        var comboitem = cComboInstrumentTypee.FindItemByValue('E');
        if (comboitem == undefined || comboitem == null) {
            cComboInstrumentTypee.AddItem("E.Transfer", "E");
        }
        var comboitem = cComboInstrumentTypee.FindItemByValue('CRD');
        if (comboitem == undefined || comboitem == null) {
            cComboInstrumentTypee.AddItem("Card", "CRD");
        }
        var comboitem = cComboInstrumentTypee.FindItemByValue('CH');
        if (comboitem != undefined && comboitem != null) {
            cComboInstrumentTypee.RemoveItem(comboitem.index);
            cComboInstrumentTypee.SetValue("C");
            InstrumentTypeSelectedIndexChanged();
        }
    }
}

function InstrumentTypeSelectedIndexChanged() {
    $("#MandatoryInstrumentType").hide();
    var InstType = cComboInstrumentTypee.GetValue();

    if (InstType == "CH") {

        document.getElementById("divInstrumentNo").style.display = 'none';
        document.getElementById("tdIDateDiv").style.display = 'none';
        document.getElementById("divDrawnOn").style.display = 'none';
    }
    else if (InstType == "C") {

        document.getElementById("divInstrumentNo").style.display = 'block';
        document.getElementById("tdIDateDiv").style.display = 'block';
        document.getElementById("divDrawnOn").style.display = 'block';
    }
    else {

        document.getElementById("divInstrumentNo").style.display = 'block';
        document.getElementById("tdIDateDiv").style.display = 'block';
        document.getElementById("divDrawnOn").style.display = 'none';
    }
}

function ProductButnClick() {

    var HeaderCaption = [];
    HeaderCaption.push("Code");
    HeaderCaption.push("Name");
    HeaderCaption.push("Hsn");
    var txt = "";
    txt += "<table border='1' width='100%' class='dynamicPopupTbl'><tr class='HeaderStyle'> <th >Select</th> <th class='hide'>id</th>";// <th>Customer Name</th><th>Unique Id</th> <th>Address</th> </tr>"
    for (var i = 0 ; i < HeaderCaption.length; i++) {
        txt += "<th>" + HeaderCaption[i] + "</th>";
    }

    ProductTable.innerHTML = txt;



    $('#ProdModel').modal('show');
}

function btnProduct_KeyDown(s, e) {
    if (e.htmlEvent.key == "Enter" || e.htmlEvent.key == "NumpadEnter") {
        s.OnButtonClick(0);
    }
}

function lostFocusVoucherAmount(s, e) {
    //var totPayment = ctxtTotalPayment.GetValue();
    //var totDebit = c_txt_Debit.GetValue();
    //var RunningBalance = clblRunningBalanceCapsul.GetValue();
    //var VoucherAmount = ctxtVoucherAmount.GetValue();
    //if (totPayment != "0.0" && totPayment != "0" && totPayment != "0.00") {
    //    clblRunningBalanceCapsul.SetValue(parseFloat(VoucherAmount) - parseFloat(totPayment));
    //}
    //else {
    //    clblRunningBalanceCapsul.SetValue(parseFloat(VoucherAmount) - parseFloat(totDebit));
    //}
    ShowRunningTotal();

}

function GetVisibleIndex(s, e) {
    globalRowIndex = e.visibleIndex;
}

function OnCustomButtonClick(s, e) {
    if (e.buttonID == 'CustomDelete') {
        if (grid.GetVisibleRowsOnPage() > 1) {
            grid.batchEditApi.StartEdit(e.visibleIndex);
            PopOnPicked(grid.GetEditor("TypeId").GetText() + grid.GetEditor("IsOpening").GetText() + grid.GetEditor("DocId").GetText());
            grid.DeleteRow(e.visibleIndex);
            SuffuleSerialNumber();
            ShowRunningTotal();
            setTimeout(function () {
                grid.batchEditApi.StartEdit(globalRowIndex, 2);
            }, 200);
        }
    }
    if (e.buttonID == 'AddNew') {
        grid.batchEditApi.StartEdit(e.visibleIndex);

        if (grid.GetEditor("TypeId").GetText() == "") return;
        if (grid.GetEditor("Payment").GetText() == "0.00" || grid.GetEditor("Payment").GetText() == "") return;

        if ( grid.GetEditor("TypeId").GetText().toUpperCase() != "ONACCOUNTPAY") {
            if (grid.GetEditor("DocId").GetText() == "") return;
        }

        GridAddnewRow();
    }
}
function PopOnPicked(uniqueId) {
    for (var i = 0; i < PickedDocument.length; i++) {
        if (PickedDocument[i] == uniqueId) {
            PickedDocument.splice(i, 1);
            return;
        }
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
    PickedDocument = [];
    //GridAddnewRow();
    grid.AddNewRow();
    c_txt_Debit.SetValue(0);
    ctxtVoucherAmount.SetValue(0);
    grid.GetEditor("SrlNo").SetText(1);

    if (PaymentDetail_ID == "" || PaymentDetail_ID == null) {
        PaymentDetail_ID = 1;
    }
    else {
        PaymentDetail_ID = PaymentDetail_ID + 1;
    }
    grid.GetEditor("UpdateEdit").SetText(PaymentDetail_ID + 1);
    grid.GetEditor("PaymentDetail_ID").SetText(parseInt(PaymentDetail_ID));
}



function grid_SalesInvoiceOnEndCallback(s, e) {
}

function OnEndCallback(s, e) {

    cLoadingPanelCRP.Hide();
    if (grid.cpGridBlank == "GridBlank")
    {
        grid.batchEditApi.StartEdit(e.visibleIndex);

        //if (grid.GetEditor("TypeId").GetText() == "") return;
        //if (grid.GetEditor("Payment").GetText() == "0.00" || grid.GetEditor("Payment").GetText() == "") return;

        //if (grid.GetEditor("TypeId").GetText().toUpperCase() != "ONACCOUNTPAY") {
        //    if (grid.GetEditor("DocId").GetText() == "") return;
        //}

        GridAddnewRow();
        //GridAddnewRow();
    }
    if (grid.cpInsert != null) {
        var Output = grid.cpInsert;
        var outputText = Output.split("~")[0];
        var outputValue = Output.split("~")[1];
        var refreshType = Output.split("~")[2];

        if (parseFloat(outputValue) > 0) {
            if (refreshType == "N") {

                jAlert(outputText);

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
                jAlert(outputText, "Success", function () {
                    window.location.href = 'CustomerReceiptPaymentList.aspx';
                });

            }
        }
        else if (outputText == "-9") {
            jAlert("DATA is Freezed between " + grid.cpAddLockStatus+" for Add.");
        }
        else {
            SuffleRows();
            jAlert(outputText);
        }


    }


}

function HeaderClear() {
    cdtTDate.SetEnabled(true);
    ctxtCustName.SetEnabled(true);
    ctxtCustName.SetText("");
    GetObjectID('hdnCustomerId').value = "";
    cddlContactPerson.ClearItems();
    ctxtInstNobth.SetText("");
    cInstDate.SetText("");
    ctxtDrawnOn.SetText("");
    $("#txtNarration").val("");
    ctxtVoucherAmount.SetText("0.00");
    $("#CB_GSTApplicable").prop("checked", false);
    cbtnProduct.SetText("");
    $("#txtTotalAmount").val("0.00");
    $("#lblRunningBalanceCapsulCrp").val("0.00");
    $('#hdAddEdit').val("Add");
    PaymentDetail_ID = 1;
}


function OpenUdf() {
    if (document.getElementById('IsUdfpresent').value == '0') {
        jAlert("UDF not define.");
    }
    else {
        var keyVal = document.getElementById('Keyval_internalId').value;
        var url = '/OMS/management/Master/frm_BranchUdfPopUp.aspx?Type=CRP&&KeyVal_InternalID=' + keyVal;
        popup.SetContentUrl(url);
        popup.Show();
    }
    return true;
}

function DocumentNoButnClick() {





    var newobj = [];
    txt = "<table border='1' width='100%' class='dynamicPopupTbl'><tr class='HeaderStyle'><th style='display:none;' >DocId</th><th>Document Number</th><th>Document Date</th><th>Unit</th><th>Unpaid Amount</th><th style='display:none;' >ProjectId</th><th style='display:none;' >Project_Code</th></tr>";
    DocTable.innerHTML = txt;
    $("#txtDocSearch").val("");

    grid.batchEditApi.StartEdit(globalRowIndex);
    var DocType = grid.GetEditor("TypeId").GetText();



    if ( DocType == "OnAccountPay" || DocType == "") {
        return;
    }
    LoadDocument();
    var Paymentdetails = $.grep(DocObj, function (e) { return e.Type == DocType && !PickedDocument.includes(e.UniqueId);; });





    for (var i = 0; i < Paymentdetails.length; i++) {

        var obj = {};
        obj.Invoice_Id = Paymentdetails[i]["Invoice_Id"];
        obj.DocumentNumber = Paymentdetails[i]["DocumentNumber"];
        obj.DocumentType = Paymentdetails[i]["DocumentType"];
        obj.DocDate = Paymentdetails[i]["DocDate"];
        obj.branch = Paymentdetails[i]["branch"];
        obj.UnPaidAmount = Paymentdetails[i]["UnPaidAmount"];
        obj.ProjectId = Paymentdetails[i]["ProjectId"];
        obj.Project_Code = Paymentdetails[i]["Project_Code"];

        newobj.push(obj);

    }
    DocTable.innerHTML = "<table border='1' width=\"100%\" id='floatedTbl'><tr class=\"HeaderStyle\"><th>Ledger Code</th><th>Ledger Name</th></tr><table>";
    DocTable.innerHTML = MakeTableFromArray(newobj, "SetDoc", "DocIndex");




    $("#DocModel").modal('show');
}



function DocumentNoKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter" || e.htmlEvent.key == "NumpadEnter") {


        s.OnButtonClick(0);
    }
}

function TypeKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter" || e.htmlEvent.key == "NumpadEnter") {

        s.OnButtonClick(0);
    }
}

function TypeTextChange(s,e)
{
    var Type = (grid.GetEditor('Type').GetValue() != null) ? grid.GetEditor('Type').GetValue() : "0";
    if ($("#hdnProjectSelectInEntryModule").val() == "1") {

        if (Type == "On Account")
        {
            if ((clookup_Project.GetGridView().GetRowKey(clookup_Project.GetGridView().GetFocusedRowIndex()) != null) &&  clookup_Project.GetText()!="") {
                LoadDocument();
                DocumentNoButnClick();
            }
        }
    }
}


function TypeButnClick(s, e) {

    if ($("#hdnCustomerId").val() != "" && $("#hdnCustomerId").val() != null && $("#hdnCustomerId").val() != "undefined") {
        if (e.buttonIndex == 0) {
            PopulateType(PaymentObj, "TypeTable", "TypeIndex", "SetType")
            $('#TypeModal').modal('show');
        }
    }
    else {

        jAlert("Please select customer to proceed further.", "Alert", function () {
            ctxtCustName.SetFocus();
        });
    }


}


function PopulateType(obj, TargetID, UniqueIndex, onSelect) {
    var myObj = obj;
    mycallonServerObj = obj;
    var txt = '';
    var count = 0;
    txt += "<table border='1' width='100%' class='dynamicPopupTbl'><tr class='HeaderStyle'> <th class='hide'>id</th>";// <th>Customer Name</th><th>Unique Id</th> <th>Address</th> </tr>"



    txt += "<th class='" + UniqueIndex + "_" + '1' + "'>Type</th>";


    for (x in myObj) {
        txt += "<tr onclick='Rowclick(event," + onSelect + ")'>";
        var PropertyCount = 0;

        for (key in myObj[0]) {

            if (PropertyCount == 0)
                txt += " <td class='hide'>" + myObj[x][key] + "</td>";
            else if (PropertyCount == 1)
                txt += " <td><input onclick='PopupTextClick(event," + onSelect + ")' CustomTypeindex=" + count + "  type='text' style='background-color: #3399520a;'" + UniqueIndex + "='" + count + "'onfocus='searchElementGetFocus(event)'  onblur='searchElementlostFocus(event)' onkeydown=SetTypeValue(event,'CustomTypeindex') width='100%' readonly value='" + myObj[x][key] + "'/></td>";
            else
                txt += "<td>" + myObj[x][key] + "</td>"
            PropertyCount++;
        }
        txt += "</tr>";
        count++;
    }
    txt += "</table>"
    document.getElementById(TargetID).innerHTML = txt;
}


function Rowclick(e, OnSelect) {
    if (e.target.type != "text") {
        var Id = e.target.parentElement.children[0].innerText;
        var name = e.target.parentElement.cells[1].children[0].value;
        OnSelect.call(this, Id, name, e.target);
    }
}

function searchElementlostFocus(e) {
    e.target.parentElement.parentElement.className = "";
    // e.target.style = "background: transparent";
}


function searchElementGetFocus(e) {
    e.target.parentElement.parentElement.className = "focusrow";
    // e.target.style = "background: transparent";

}

function PopupTextClick(e, OnSelect) {
    var Id = e.target.parentElement.parentElement.children[0].innerText;
    var name = e.target.parentElement.parentElement.cells[1].children[0].value;
    OnSelect.call(this, Id, name, e.target.parentElement);
}

function SetTypeValue(e, indexName) {
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var Id = e.target.parentElement.parentElement.children[0].innerText;
        var name = e.target.parentElement.parentElement.cells[1].children[0].value;
        SetType(Id, name);
    } else if (e.code == "ArrowDown") {
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

    }

}

function SetType(id, name) {
    grid.batchEditApi.StartEdit(globalRowIndex);

                    grid.GetEditor("ProjectId").SetValue("0");
                    grid.GetEditor("Project_Code").SetValue("");
                   grid.GetEditor("Type").SetValue(name);
                    grid.GetEditor("TypeId").SetValue(id);

                    grid.GetEditor("DocumentNo").SetValue("");
                    grid.GetEditor("Payment").SetValue("0.00");
                    grid.GetEditor("IsOpening").SetValue("");
                    grid.GetEditor("ActualAmount").SetValue("0.00");
                    grid.GetEditor("DocId").SetValue("");

                    var Type = (grid.GetEditor('Type').GetValue() != null) ? grid.GetEditor('Type').GetValue() : "0";
                    if ($("#hdnProjectSelectInEntryModule").val() == "1") {

                        if (Type == "On Account") {
                            if ((clookup_Project.GetGridView().GetRowKey(clookup_Project.GetGridView().GetFocusedRowIndex()) != null) && clookup_Project.GetText() != "") {
                                var id = (clookup_Project.GetGridView().GetRowKey(clookup_Project.GetGridView().GetFocusedRowIndex()));
                                var text = clookup_Project.GetValue();
                                grid.GetEditor("ProjectId").SetValue(id);
                                grid.GetEditor("Project_Code").SetValue(text);
                            }
                        }
                    }
                   
                    ShowRunningTotal();
                    CreateDocumentList();
                    grid.batchEditApi.EndEdit();

                    CloseSubModal();

                
                    //clookup_Project.SetEnabled(false);
}


function CheckedChange() {

    if ($("#CB_GSTApplicable").is(':checked')) {
        cbtnProduct.SetEnabled(true);
    }
    else {
        cbtnProduct.SetEnabled(false);
        //return;
    }
    var proMsg = 'Selected Product (s) to be cleaned if you unchecked this option.';
    if (cbtnProduct.GetText() != '') {
        if ($("#CB_GSTApplicable").is(':checked')) {

        }
        else {
            jAlert(proMsg, 'Alert Dialog: [CustomerReceipt/Payment]', function (r) {
                if (r == true) {

                    jConfirm('Are You Sure?', 'Confirmation Dialog', function (r) {
                        if (r == true) {
                            cbtnProduct.SetText("");

                            $("#hdnProductId").val("");
                            $("#hdtHsnCode").val("");

                            $("#CB_GSTApplicable").prop("checked", false);
                        }
                        else {
                            $("#CB_GSTApplicable").prop("checked", true);
                        }
                    });
                }
                else {
                    $("#CB_GSTApplicable").prop("checked", true);
                }
            });
        }
    }

}

function CloseSubModal() {
    $('#TypeModal').modal('hide');
}

$(document).ready(function () {
    var isCtrl = false;



    $('#TypeModal').on('shown.bs.modal', function () {
        $("input[customtypeindex=0]").focus();

    })
    $('#CustModel').on('hide.bs.modal', function () {
        setTimeout(function () {
            grid.batchEditApi.StartEdit(globalRowIndex, 2);
        }, 200);
    })

    $('#TypeModal').on('hide.bs.modal', function () {
        setTimeout(function () {
            IndexNo = globalRowIndex;
            grid.batchEditApi.EndEdit();
            setTimeout(function () {
                grid.batchEditApi.StartEdit(IndexNo, 3);
            }, 200);
        }, 200);
    })



    $('#DocModel').on('shown.bs.modal', function () {
        document.getElementById("txtDocSearch").focus();
    })
    $('#DocModel').on('hide.bs.modal', function () {
        setTimeout(function () {
            grid.batchEditApi.EndEdit();
            grid.batchEditApi.StartEdit(globalRowIndex, 4);
        }, 200);
    })

    $('#ProdModel').on('shown.bs.modal', function () {
        $('#txtProdSearch').focus();
    })
    document.onkeydown = function (e) {
        if (event.keyCode == 83 && event.altKey == true && $("#DoEdit").val()=="1") {
            // StopDefaultAction(e);
            SaveButtonClickNew();//........Alt+N
        }
        else if (event.keyCode == 88 && event.altKey == true && $("#DoEdit").val() == "1") {
            SaveButtonClick();//........Alt+X
        }
        else if (event.keyCode == 85 && event.altKey == true) {
            OpenUdf();
        }
        else if (event.keyCode == 84 && event.altKey == true) {
            Save_TaxesClick();
        }
    }
    $("#openlink").on("click", function () {
        //window.open('../master/Contact_general.aspx?id=ADD', '_blank');
        if ((cCmbScheme.GetValue() != "0" && cCmbScheme.GetValue() != null && cCmbScheme.GetValue() != "undefined") || $('#hdAddEdit').val() != "Add") {
            AddcustomerClick();
        }
        else {
            setTimeout(function () { jAlert("Please select numbering scheme to select customer."); }, 500);
        }
    });
});

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

function AddcustomerClick() {
    var url = '/OMS/management/Master/Customer_general.aspx';
    AspxDirectAddCustPopup.SetContentUrl(url);
    AspxDirectAddCustPopup.Show();
}
function ParentCustomerOnClose(newCustId,CustomerName,Unique) {
    AspxDirectAddCustPopup.Hide();
    $("input[name='ctl00$ContentPlaceHolder1$ASPxPageControl1$rdl_Contact'][value='CL']").prop('checked', true);
    SetCustomer(newCustId, CustomerName);
}

function selectValue() {
    var type = ($("[id$='rdl_MultipleType']").find(":checked").val() != null) ? $("[id$='rdl_MultipleType']").find(":checked").val() : "";
    if (type == 'S') {
        $('#Multipletype').hide();
        $('#singletype').show();
        $('#tdCashBankLabel').show();
        $("#ClearSingle").hide();
    }
    else if (type == 'M') {

        $('#tdCashBankLabel').hide();

        $('#Multipletype').show();
        $('#singletype').hide();
        $("#ClearSingle").show();
    }
}

function Customer_KeyDown(s, e) {
    if (e.htmlEvent.key == "Enter" || e.htmlEvent.key == "NumpadEnter") {
        if ((cCmbScheme.GetValue() != "0" && cCmbScheme.GetValue() != null && cCmbScheme.GetValue() != "undefined") || $('#hdAddEdit').val() != "Add") {

            var txt = "<table border='1' width='100%' class='dynamicPopupTbl'> <tr class='HeaderStyle'><th class='hide'>id</th><th>Name</th><th>Unique Id</th><th>Address</th></tr></table>";
            document.getElementById("CustomerTable").innerHTML = txt;

            var Contacttype = ($("[id$='rdl_Contact']").find(":checked").val() != null) ? $("[id$='rdl_Contact']").find(":checked").val() : "CL";
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
        var Contacttype = ($("[id$='rdl_Contact']").find(":checked").val() != null) ? $("[id$='rdl_Contact']").find(":checked").val() : "CL";
        if (Contacttype.toUpperCase() == "CL")
            $('#CustModel').find('.modal-title').text("Customer Search");
        else
            $('#CustModel').find('.modal-title').text("Vendor Search");

        var txt = "<table border='1' width='100%' class='dynamicPopupTbl'> <tr class='HeaderStyle'><th class='hide'>id</th><th>Name</th><th>Unique Id</th><th>Address</th></tr></table>";
        document.getElementById("CustomerTable").innerHTML = txt;
        $('#CustModel').modal('show');
        setTimeout(function () { $('#txtCustSearch').focus(); }, 500);
    }
    else
        setTimeout(function () { jAlert("Please select numbering scheme to select customer."); }, 500);
}


function Customerkeydown(e) {
    var OtherDetails = {}
    var Contacttype = ($("[id$='rdl_Contact']").find(":checked").val() != null) ? $("[id$='rdl_Contact']").find(":checked").val() : "";
    OtherDetails.SearchKey = $("#txtCustSearch").val();
    OtherDetails.contactType = Contacttype;

    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var HeaderCaption = [];

        if (Contacttype == "CL")
            HeaderCaption.push("Customer Name");
        else
            HeaderCaption.push("Vendor Name");

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
        GetContactPerson();
        $('#CustModel').modal('hide');
        GetProformaInvoice();
        SetDefaultBillingShippingAddress(GetObjectID('hdnCustomerId').value);
       // page.SetActiveTabIndex(1);

        setTimeout(function () {
            cddlContactPerson.SetFocus();
        }, 200);
        ShowCustomerBalance();
        clookup_Project.gridView.Refresh();
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
            $('#txtCustSearch').focus();
        }
    }
}

function GetContactPerson() {
    var OtherDetails = {}
    OtherDetails.CustomerId = $("#hdnCustomerId").val();
    $.ajax({
        type: "POST",
        url: "../Activities/Services/CustomerReceiptPayment.asmx/GetContactPerson",
        data: JSON.stringify(OtherDetails),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var returnObject = msg.d;

            if (returnObject) {
                SetDataSourceOnComboBox(cddlContactPerson, returnObject);
            }

        }
    });

    LoadDocument();
}
function GetProformaInvoice() {
    var OtherDetails = {}
    OtherDetails.CustomerId = $("#hdnCustomerId").val();
    $.ajax({
        type: "POST",
        url: "../Activities/Services/CustomerReceiptPayment.asmx/GetProformaInvoice",
        data: JSON.stringify(OtherDetails),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var returnObject = msg.d;

            if (returnObject) {
                SetDataSourceOnComboBox(cddlProformaInvoice, returnObject);
            }

        }
    });
}
function LoadDocument() {
    if ($("#hdnCustomerId").val() != "") {
        var OtherDetails = {}
        OtherDetails.VoucherType = "P";
        OtherDetails.CustomerId = $("#hdnCustomerId").val();
        OtherDetails.BranchId = cddlBranch.GetValue();
        OtherDetails.ReceiptPaymentId = $("#ReceiptPaymentId").text();
        OtherDetails.TransDate = cdtTDate.GetText();
      

        if ($("#hdnProjectSelectInEntryModule").val() == "1")
        {
            if ((clookup_Project.GetGridView().GetRowKey(clookup_Project.GetGridView().GetFocusedRowIndex()) != null) && clookup_Project.GetText() != "")
            {
                OtherDetails.ProjectId = (clookup_Project.GetGridView().GetRowKey(clookup_Project.GetGridView().GetFocusedRowIndex()));

                $.ajax({
                    type: "POST",
                    url: "../Activities/Services/CustomerReceiptPayment.asmx/GetAllDocumentPaymentWithProject",
                    data: JSON.stringify(OtherDetails),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (msg) {
                        var returnObject = msg.d;

                        if (returnObject) {
                            console.log(returnObject);
                            DocObj = returnObject;
                        }

                    }
                });
            }
            else
            {
                $.ajax({
                    type: "POST",
                    url: "../Activities/Services/CustomerReceiptPayment.asmx/GetAllDocumentPayment",
                    data: JSON.stringify(OtherDetails),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (msg) {
                        var returnObject = msg.d;

                        if (returnObject) {
                            console.log(returnObject);
                            DocObj = returnObject;
                        }

                    }
                });
            }
        }
        else
        {
            $.ajax({
                type: "POST",
                url: "../Activities/Services/CustomerReceiptPayment.asmx/GetAllDocumentPayment",
                data: JSON.stringify(OtherDetails),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {
                    var returnObject = msg.d;

                    if (returnObject) {
                        console.log(returnObject);
                        DocObj = returnObject;
                    }

                }
            });
        }
    }
}

function Currency_Rate() {

    var Currency_ID = cddlCurrency.GetValue();
    var obj = {};
    obj.Currency_ID = Currency_ID;

    var basedCurrency = $("#hdnLocalCurrency").text();

    if ($("#ddl_Currency").text().trim() == basedCurrency.split("~")[1]) {
        ctxtRate.SetValue("");
        ctxtRate.SetEnabled(false);
    }
    else {
        $.ajax({
            type: "POST",
            url: "../Activities/Services/CustomerReceiptPayment.asmx/GetRate",
            data: JSON.stringify(obj),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var data = msg.d;
                if (data != "") {
                    ctxtRate.SetValue(data);
                    ctxtRate.SetEnabled(true);
                }
                else {
                    ctxtRate.SetValue("0.00");
                    ctxtRate.SetEnabled(false);
                }
            }
        });
        //ctxtRate.SetEnabled(true);
    }
}

function CreateDocumentList() {

    PickedDocument = [];

    for (var i = 0; i < 1000; i++) {
        if (grid.GetRow(i)) {
            if (grid.GetRow(i).style.display != "none") {
                grid.batchEditApi.StartEdit(i, 2);
                PushOnPicked(grid.GetEditor("TypeId").GetText() + grid.GetEditor("IsOpening").GetText() + grid.GetEditor("DocId").GetText());
            }
        }
    }

    for (i = -1; i > -1000; i--) {
        if (grid.GetRow(i)) {
            if (grid.GetRow(i).style.display != "none") {
                grid.batchEditApi.StartEdit(i, 2);
                PushOnPicked(grid.GetEditor("TypeId").GetText() + grid.GetEditor("IsOpening").GetText() + grid.GetEditor("DocId").GetText());

            }
        }
    }
}


function PushOnPicked(uniqueId) {
    PickedDocument.push(uniqueId);
}
function GridAddnewRow() {
    grid.AddNewRow();
    grid.GetEditor("SrlNo").SetText(grid.GetVisibleItemsOnPage());


    if (PaymentDetail_ID == "" || PaymentDetail_ID == null) {
        PaymentDetail_ID = 1;
    }
    else {
        PaymentDetail_ID = parseInt(PaymentDetail_ID) + 1;
    }

    grid.GetEditor("PaymentDetail_ID").SetText(parseInt(PaymentDetail_ID));


    grid.batchEditApi.EndEdit();

    setTimeout(function () {
        grid.batchEditApi.EndEdit();
        grid.batchEditApi.StartEdit(globalRowIndex, 2);
    }, 200);
}


function GetInvoiceMsg(s, e) {

    var VoucherAmount = document.getElementById('hdAddEdit').value;
    if (VoucherAmount == 'Edit') {

    }
    else {
        var salesInvoice = document.getElementById('SysSetting').value;
        var vouchAmt = parseFloat(ctxtVoucherAmount.GetValue());
        // var totPay = parseFloat(ctxtTotalPayment.GetValue());
        var totDebit = c_txt_Debit.GetValue();
        var totalRunningValue = "0.00";
        if (totDebit == "0.0" && totDebit == "0" && totDebit == "0.00") {
            var totalRunningValue = vouchAmt;
        }

        if (salesInvoice == "Yes") {
            clblRunningBalanceCapsul.SetValue(totalRunningValue);
            jConfirm('Wish to auto adjust amount with sale Invoice(s)?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    cPopup_invoice.Show();
                    var amount = ctxtVoucherAmount.GetValue();
                    var customerval = GetObjectID('hdnCustomerId').value;
                    //var customerval = (cCustomerComboBox.GetValue() != null) ? cCustomerComboBox.GetValue() : "";

                }
                else {
                    grid.batchEditApi.StartEdit(-1, 1);
                }
            });
        }
    }
    var amount = ctxtVoucherAmount.GetValue();
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

function disp_prompt(name) {

    if (name == "tab0") {
        ctxtCustName.Focus();
    }
    if (name == "tab1") {
        var custID = GetObjectID('hdnCustomerId').value;
        if (custID == null && custID == '') {
            jAlert('Please select a customer');
            page.SetActiveTabIndex(0);
            return;
        }
        else {
            page.GetTabByName('General').SetEnabled(false);
            page.SetActiveTabIndex(1);
        }
    }
}

var ProdArr = new Array();

function SetSelectedValues(Id, Name, ArrName) {
    if (ArrName == 'CustomerSource') {
        var key = Id;
        if (key != null && key != '') {

            $('#ProductModal').modal('hide');
            cbtnProduct.SetText(Name);
        }
        else {

            cbtnProduct.SetText('');
        }
    }

}



function DocNewkeydown(e) {

    var newobj = [];

    if (e.code == "Enter" || e.code == "NumpadEnter") {




        grid.batchEditApi.StartEdit(globalRowIndex);
        var DocType = grid.GetEditor("TypeId").GetText();
        var Paymentdetails = $.grep(DocObj, function (e) { return e.Type == DocType && !PickedDocument.includes(e.UniqueId); });

        var SearchObj = $("#txtDocSearch").val();
        if (SearchObj != "") {
            for (var i = 0; i < Paymentdetails.length; i++) {

                var obj = {};
                var NewCode = Paymentdetails[i]["DocumentNumber"];
                if (NewCode.toUpperCase().includes(SearchObj.toUpperCase())) {
                    //NewCode = NewCode.toUpperCase().replace(SearchObj.toUpperCase(), '<mark><b>' + SearchObj.toUpperCase() + '</b></mark>');


                    obj.Invoice_Id = Paymentdetails[i]["Invoice_Id"];
                    obj.DocumentNumber = NewCode;
                    obj.DocumentType = Paymentdetails[i]["DocumentType"];
                    obj.DocDate = Paymentdetails[i]["DocDate"];
                    obj.branch = Paymentdetails[i]["branch"];
                    obj.UnPaidAmount = Paymentdetails[i]["UnPaidAmount"];
                    obj.ProjectId = Paymentdetails[i]["ProjectId"];
                    obj.Project_Code = Paymentdetails[i]["Project_Code"];

                    newobj.push(obj);

                }



            }
            DocTable.innerHTML = "";
            DocTable.innerHTML = MakeTableFromArray(newobj, "SetDoc", "DocIndex");
        }
        else {
            DocTable.innerHTML = "";
            DocTable.innerHTML = MakeTableFromArray(DocObj, "SetDoc", "DocIndex");
        }

        setTimeout(function () { document.getElementById("txtDocSearch").focus(); }, 500);

    }
    else if (e.code == "ArrowDown") {
        if ($("input[DocIndex=0]"))
            $("input[DocIndex=0]").focus();
    }
}
function MakeTableFromArray(myObj, onSelect, UniqueIndex) {
    var myObj = myObj;
    mycallonServerObj = myObj;
    var txt = '';
    var count = 0;

    txt = "<table border='1' width='100%'  class='dynamicPopupTbl' id='floatedTbl'><tbody><tr class='HeaderStyle'><th style='display:none;' >DocId</th><th width='250px'>Document Number</th><th width='25%'>Document Date</th><th width='25%'>Unit</th><th width='25%'>Unpaid Amount</th></tr>";


    for (x in myObj) {
        txt += "<tr onclick='RowclickDoc(event," + onSelect + ")'>";
        var PropertyCount = 0;

        for (key in myObj[0]) {

            if (PropertyCount == 0)
                txt += " <td class='hide'>" + myObj[x][key] + "</td>";
            else if (PropertyCount == 1)
                txt += " <td style='width:25%'><input onclick='PopupTextClickDoc(event," + onSelect + ")' type='text' style='background-color: #3399520a;'" + UniqueIndex + "='" + count + "'onfocus='searchElementGetFocusDoc(event)'  onblur='searchElementlostFocusDoc(event)' onkeydown=ValueSelectedDoc(event,'" + UniqueIndex.toString() + "',event.target) width='100%' value=" + myObj[x][key] + " readonly  /></td>";
            else if (PropertyCount == 2)
                txt += "<td class='hide'>" + myObj[x][key] + "</td>"
            else if (PropertyCount == 6)
                txt += "<td class='hide'>" + myObj[x][key] + "</td>"
            else if (PropertyCount == 7)
                txt += "<td class='hide'>" + myObj[x][key] + "</td>"
            else
                txt += "<td style='width:220px'>" + myObj[x][key] + "</td>"
            PropertyCount++;
        }
        txt += "</tr>";
        count++;
    }
    txt += "</tbody></table>"
    //document.getElementById(TargetID).innerHTML = txt;

    return txt;
}

function closeDocModal() {
    $("#DocModel").modal('hide');
}


function RowclickDoc(e, OnSelect) {
    if (e.target.type != "text") {
        var Id = e.target.parentElement.children[0].innerText;
        var name = e.target.parentElement.cells[1].children[0].value;
        OnSelect.call(this, Id, name, e.target);
    }
}

function searchElementlostFocusDoc(e) {
    e.target.parentElement.parentElement.className = "";
    // e.target.style = "background: transparent";
}


function searchElementGetFocusDoc(e) {
    e.target.parentElement.parentElement.className = "focusrow";
    // e.target.style = "background: transparent";

}

function PopupTextClickDoc(e, OnSelect) {
    var Id = e.target.parentElement.parentElement.children[0].innerText;
    var name = e.target.parentElement.parentElement.cells[1].children[0].value;
    OnSelect.call(this, Id, name, e.target.parentElement);
}

function SetDoc(id, Name, e) {

    var row = e.parentElement.children;

    var NewId = row[0].innerText;
    var NewDoc = row[1].children[0].value;
    var DocType = row[2].innerText;
    var Unpaid = DecimalRoundoff(row[5].innerText, 2);
    var ProjectId = row[6].innerText;
    var Project_Code = row[7].innerText;


    grid.batchEditApi.StartEdit(globalRowIndex);
    grid.GetEditor("DocumentNo").SetValue(NewDoc);
    grid.GetEditor("Payment").SetValue(Unpaid);
    grid.GetEditor("IsOpening").SetValue(DocType);
    grid.GetEditor("ActualAmount").SetValue(Unpaid);
    grid.GetEditor("DocId").SetValue(NewId);
    grid.GetEditor("ProjectId").SetValue(ProjectId);
    grid.GetEditor("Project_Code").SetValue(Project_Code);
    ShowRunningTotal();
    CreateDocumentList();
    grid.batchEditApi.EndEdit();
    closeDocModal();

}
function ValueSelectedDoc(e, indexName) {
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var Id = e.target.parentElement.parentElement.cells[0].innerText;
        var name = e.target.parentElement.parentElement.cells[1].children[0].value;
        if (Id) {
            SetDoc(Id, name, e.target.parentElement);

        }
    }
    else if (e.code == "ArrowDown") {
        thisindex = parseFloat(e.target.getAttribute(indexName));
        thisindex++;
        if (thisindex < 100)
            $("input[" + indexName + "=" + thisindex + "]").focus();
    }
    else if (e.code == "ArrowUp") {
        thisindex = parseFloat(e.target.getAttribute(indexName));
        thisindex--;
        if (thisindex > -1) {
            $("input[" + indexName + "=" + thisindex + "]").focus();
        }
        else {
            $('#txtDocSearch').focus();
        }
    }
}

function GetServerDateFormat(today) {
    if (today != "" && today != null) {
        var dd = today.getDate();
        var mm = today.getMonth() + 1;
        var yyyy = today.getFullYear();

        if (dd < 10) {
            dd = '0' + dd;
        }
        if (mm < 10) {
            mm = '0' + mm;
        }
        today = yyyy + '-' + mm + '-' + dd;
    }
    else {
        today = "";
    }

    return today;
}

function SaveButtonClick() {

    var ProjectCode = clookup_Project.GetText();
    if ($("#hdnProjectSelectInEntryModule").val() == "1" && $("#hdnProjectMandatory").val() == "1" && ProjectCode == "") {
        jAlert("Please Select Project.");
        return false;
    }
    if (validation()) {
        $("#hdnRefreshType").val("E");
        SelectAllData();

        var CashBank = cddlCashBank.GetValue();
        var TotalPaymentAmount = DecimalRoundoff(ctxtVoucherAmount.GetValue(), 2);
        var urlKeys = getUrlVars();
        var CustomerPaymentID;
        if (urlKeys.key != 'Add') {
            CustomerPaymentID = urlKeys.id;
        }
        else {
            CustomerPaymentID = 0;
        }

        var PostingDate = GetServerDateFormat(cdtTDate.GetValue());

        $.ajax({
            type: "POST",
            url: "CustomerPayment.aspx/GetTotalBalanceByCashBankID",
            data: JSON.stringify({ CashBankID: CashBank, CustomerPaymentID: CustomerPaymentID, PostingDate: PostingDate }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (msg) {
                var data = msg.d;
                var VoucherAmount = data.toString().split('~')[0];
                var BalanceLimit = data.toString().split('~')[1];
                var BalanceExceed = data.toString().split('~')[2];
                var closingBalnc = data.toString().split('~')[3];
                if (BalanceLimit != '0.00') {
                    //Rev Tanmoy
                    var TotalVoucherAmount = parseFloat(TotalPaymentAmount);// + parseFloat(VoucherAmount);
                    var closingAmount = parseFloat(closingBalnc) - parseFloat(TotalVoucherAmount);
                    if (parseFloat(closingAmount) < parseFloat(BalanceLimit)) {
                   // if (parseFloat(TotalVoucherAmount) > parseFloat(BalanceLimit)) {
                        //End Rev Tanmoy
                        if (BalanceExceed.trim() == 'W') {

                            jConfirm('Cash/Bank Balance - Limit is exceed do you want to proceed?', 'Confirmation Dialog', function (r) {
                                if (r == true) {
                                    grid.AddNewRow();
                                    grid.GetEditor("SrlNo").SetText(grid.GetVisibleItemsOnPage());

                                    grid.UpdateEdit();
                                    
                                }
                                else {
                                    cLoadingPanelCRP.Hide();
                                }
                            });
                        }
                        else if (BalanceExceed.trim() == 'B') {
                            jAlert('Cash/Bank Balance - Limit is exceed can not proceed');
                            cLoadingPanelCRP.Hide();

                        }
                        else if (BalanceExceed.trim() == 'I') {
                            grid.AddNewRow();
                            grid.GetEditor("SrlNo").SetText(grid.GetVisibleItemsOnPage());                            
                            grid.UpdateEdit();
                           
                        }
                    }
                    else {
                        grid.AddNewRow();
                        grid.GetEditor("SrlNo").SetText(grid.GetVisibleItemsOnPage());
                        grid.UpdateEdit();
                       
                    }
                }
                else {
                    grid.AddNewRow();
                    grid.GetEditor("SrlNo").SetText(grid.GetVisibleItemsOnPage());
                    grid.UpdateEdit();
                    
                }

            }
        });

       /// grid.UpdateEdit();
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
        SelectAllData();

        var CashBank = cddlCashBank.GetValue();
        var TotalPaymentAmount = DecimalRoundoff(ctxtVoucherAmount.GetValue(), 2);
        var urlKeys = getUrlVars();
        var CustomerPaymentID;
        if (urlKeys.key != 'Add') {
            CustomerPaymentID = urlKeys.id;
        }
        else {
            CustomerPaymentID = 0;
        }

        var PostingDate = GetServerDateFormat(cdtTDate.GetValue());

        $.ajax({
            type: "POST",
            url: "CustomerPayment.aspx/GetTotalBalanceByCashBankID",
            data: JSON.stringify({ CashBankID: CashBank, CustomerPaymentID: CustomerPaymentID, PostingDate: PostingDate }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (msg) {
                var data = msg.d;
                var VoucherAmount = data.toString().split('~')[0];
                var BalanceLimit = data.toString().split('~')[1];
                var BalanceExceed = data.toString().split('~')[2];
                var closingBalnc = data.toString().split('~')[3];
                if (BalanceLimit != '0.00') {
                    //Rev Tanmoy
                    var TotalVoucherAmount = parseFloat(TotalPaymentAmount); //+ parseFloat(VoucherAmount);
                  
                    var closingAmount = parseFloat(closingBalnc) - parseFloat(TotalVoucherAmount);
                    // if (parseFloat(TotalVoucherAmount) > parseFloat(BalanceLimit)) {
                    if (parseFloat(closingAmount) < parseFloat(BalanceLimit)) {
                        //End Rev Tanmoy
                        if (BalanceExceed.trim() == 'W') {

                            jConfirm('Cash/Bank Balance - Limit is exceed do you want to proceed?', 'Confirmation Dialog', function (r) {
                                if (r == true) {
                                    grid.AddNewRow();
                                    grid.GetEditor("SrlNo").SetText(grid.GetVisibleItemsOnPage());

                                    grid.UpdateEdit();

                                }
                                else {
                                    cLoadingPanelCRP.Hide();
                                }
                            });
                        }
                        else if (BalanceExceed.trim() == 'B') {
                            jAlert('Cash/Bank Balance - Limit is exceed can not proceed');
                            cLoadingPanelCRP.Hide();

                        }
                        else if (BalanceExceed.trim() == 'I') {
                            grid.AddNewRow();
                            grid.GetEditor("SrlNo").SetText(grid.GetVisibleItemsOnPage());
                            grid.UpdateEdit();

                        }
                    }
                    else {
                        grid.AddNewRow();
                        grid.GetEditor("SrlNo").SetText(grid.GetVisibleItemsOnPage());
                        grid.UpdateEdit();

                    }
                }
                else {
                    grid.AddNewRow();
                    grid.GetEditor("SrlNo").SetText(grid.GetVisibleItemsOnPage());
                    grid.UpdateEdit();

                }

            }
        });




       // grid.UpdateEdit();
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


    var type = ($("[id$='rdl_MultipleType']").find(":checked").val() != null) ? $("[id$='rdl_MultipleType']").find(":checked").val() : "";
    var CashBank = cddlCashBank.GetValue();

    var VoucherAmount = ctxtVoucherAmount.GetValue();
    if (type == 'S') {
        if (CashBank == null) {
            cLoadingPanelCRP.Hide();
            $("#MandatoryCashBank").show();
            return false;
        }
        else {
            $("#MandatoryCashBank").hide();
        }
        var comboitem = cComboInstrumentTypee.GetValue('CH');


    }

    if (VoucherAmount == "0.00") {

        jAlert("Voucher amount must be greater then ZERO.");
        cLoadingPanelCRP.Hide();
        return false;
    }
    var type = ($("[id$='rdl_MultipleType']").find(":checked").val() != null) ? $("[id$='rdl_MultipleType']").find(":checked").val() : "";
    if (type == 'M') {
        var totalAmount = GetPaymentTotalEnteredAmount();
        var VoucherAmount = parseFloat(ctxtVoucherAmount.GetValue());
        if (totalAmount != VoucherAmount) {
            jAlert("Voucher amount And Multiple Payment amount must be Same.");
            cLoadingPanelCRP.Hide();
            return false;
        }

        var EnteredCashAmount = parseFloat($('#cmbUcpaymentCashLedgerAmt').val());


    }
    var type = ($("[id$='rdl_MultipleType']").find(":checked").val() != null) ? $("[id$='rdl_MultipleType']").find(":checked").val() : "";
    if (type == 'M') {
        var retValueonpayDet;
        if (document.getElementById('PaymentTable')) {
            var table = document.getElementById('PaymentTable');
            if (table.rows[table.rows.length - 1]) {
                if (table.rows[table.rows.length - 1].children[0].children[1].value != "-Select-") {
                    retValueonpayDet = validatePaymentDetails(table.rows[table.rows.length - 1]);
                    if (!retValueonpayDet) {
                        cLoadingPanelCRP.Hide();
                        return false;
                    }
                }
            }
        }
    }
    grid.batchEditApi.EndEdit();
    var gridCount = grid.GetVisibleRowsOnPage();


    var VoucherAmount = DecimalRoundoff(ctxtVoucherAmount.GetValue(), 2);

    var TotAmt = DecimalRoundoff(GetTotalAmount(), 2);

    if (VoucherAmount != TotAmt) {
        cLoadingPanelCRP.Hide();
        jAlert("Voucher amount and Total amount is not same.Can not proceed.");
        return false;
    }


    var TotRowNumber = grid.GetVisibleItemsOnPage();
    var PaymentDetail_ID = grid.GetEditor("PaymentDetail_ID").GetText();
    for (var i = 0; i < 1000; i++) {
        if (grid.GetRow(i)) {
            if (grid.GetRow(i).style.display != "none") {
                grid.batchEditApi.StartEdit(i, 2);
                if (grid.GetEditor("TypeId").GetText() == "" && grid.GetEditor("Payment").GetText() != "0.00" && grid.GetEditor("Payment").GetText() != "") {
                    cLoadingPanelCRP.Hide();
                    jAlert("Please select a valid type to proceed.");
                    return false;
                }

                if (grid.GetEditor("TypeId").GetText() != "" && (grid.GetEditor("Payment").GetText() == "0.00" || grid.GetEditor("Payment").GetText() == "")) {
                    cLoadingPanelCRP.Hide();
                    jAlert("Please enter a valid amount to proceed.");
                    return false;
                }

                if (grid.GetEditor("TypeId").GetText().toUpperCase() != "ADVANCE" && grid.GetEditor("TypeId").GetText().toUpperCase() != "ONACCOUNTPAY" && grid.GetEditor("TypeId").GetText().toUpperCase() != "") {

                    if (grid.GetEditor("DocId").GetText() == "") {
                        cLoadingPanelCRP.Hide();
                        jAlert("Please select a valid document to proceed.");
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
                if (grid.GetEditor("TypeId").GetText() == "" && grid.GetEditor("Payment").GetText() != "0.00" && grid.GetEditor("Payment").GetText() != "") {
                    cLoadingPanelCRP.Hide();
                    jAlert("Please select a valid type to proceed.");
                    return false;
                }

                if (grid.GetEditor("TypeId").GetText() != "" && (grid.GetEditor("Payment").GetText() == "0.00" || grid.GetEditor("Payment").GetText() == "")) {
                    cLoadingPanelCRP.Hide();
                    jAlert("Please enter a valid amount to proceed.");
                    return false;
                }

                if (grid.GetEditor("TypeId").GetText().toUpperCase() != "ADVANCE" && grid.GetEditor("TypeId").GetText().toUpperCase() != "ONACCOUNTPAY" && grid.GetEditor("TypeId").GetText().toUpperCase() !="") {

                    if (grid.GetEditor("DocId").GetText() == "") {
                        cLoadingPanelCRP.Hide();
                        jAlert("Please select a valid document to proceed.");
                        return false;
                    }
                }
            }
        }
    }



    return Valid;
}




function GetTotalAmount() {

    var totalAmount = 0;

    for (var i = 0; i < 1000; i++) {
        if (grid.GetRow(i)) {
            if (grid.GetRow(i).style.display != "none") {
                grid.batchEditApi.StartEdit(i, 2);
                totalAmount = totalAmount + DecimalRoundoff(grid.GetEditor("Payment").GetText(), 2);
            }
        }
    }

    for (i = -1; i > -1000; i--) {
        if (grid.GetRow(i)) {
            if (grid.GetRow(i).style.display != "none") {
                grid.batchEditApi.StartEdit(i, 2);
                totalAmount = totalAmount + DecimalRoundoff(grid.GetEditor("Payment").GetText(), 2);
            }
        }
    }

    return totalAmount;
}


function Payment_LostFocus(s, e) {

    grid.batchEditApi.StartEdit(globalRowIndex, 4);
    var TypeID = grid.GetEditor("TypeId").GetText();
    var DocID = grid.GetEditor("DocId").GetText();
    var ActualAmount = grid.GetEditor("ActualAmount").GetText();
    var Payment = grid.GetEditor("Payment").GetText();

    if (TypeID.toUpperCase() != "ONACCOUNTPAY") {
        if (DocID != "") {
            if (parseFloat(Payment) > parseFloat(ActualAmount)) {
                var NewAmt = grid.GetEditor("ActualAmount");
                s.SetValue(NewAmt.GetValue());
                jAlert("Payment amount can not be greater than unpaid amount.");
                setTimeout(function () {
                    grid.batchEditApi.EndEdit();
                    grid.batchEditApi.StartEdit(globalRowIndex, 4);
                }, 200);
            }
        }
        else {
            s.SetValue("0.00");
                       
        }
    }

    ShowRunningTotal();

    setTimeout(function () {
        grid.batchEditApi.EndEdit();
        grid.batchEditApi.StartEdit(globalRowIndex, 5);
    }, 300);

}

function ShowRunningTotal() {
    var VoucherAmount = ctxtVoucherAmount.GetValue();

    var TotAmt = DecimalRoundoff(GetTotalAmount(), 2);
    c_txt_Debit.SetText(TotAmt.toString());
    ctxtVoucherAmount.SetText(TotAmt.toString());

    clblRunningBalanceCapsul.SetValue(DecimalRoundoff(parseFloat(VoucherAmount) - parseFloat(TotAmt), 2));
}


function ddlBranch_Change() {
    deleteAllRows();
    LoadDocument();
    CreateDocumentList();
    clookup_Project.gridView.Refresh();
}

function ValidateGrid() {


}

function Productkeydown(e) {
    var OtherDetails = {}

    if ($.trim($("#txtProdSearch").val()) == "" || $.trim($("#txtProdSearch").val()) == null) {
        return false;
    }
    OtherDetails.SearchKey = $("#txtProdSearch").val();
    OtherDetails.HSNID = $("#hdnHSNId").val();

    if (e.code == "Enter" || e.code == "NumpadEnter") {

        var HeaderCaption = [];
        HeaderCaption.push("Code");
        HeaderCaption.push("Name");
        HeaderCaption.push("Hsn");


        if ($("#txtProdSearch").val() != "") {
            callonServerM("../Activities/Services/CustomerReceiptPayment.asmx/GetHSNWiseProduct", OtherDetails, "ProductTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "ProductSource");
        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[dPropertyIndex=0]"))
            $("input[dPropertyIndex=0]").focus();
    }
}

function SetSelectedValues(Id, Name, ArrName) {
    if (ArrName == 'ClassSource') {
        var key = Id;
        if (key != null && key != '') {
            $('#ClassModel').modal('hide');
            cbtnProduct.SetText(Name);
            GetObjectID('hdnHSNId').value = key;


            cbtnProduct.SetText('');
            $('#txtProdSearch').val('')

            var OtherDetailsProd = {}
            OtherDetailsProd.SearchKey = 'undefined text';
            OtherDetailsProd.HSNID = '';
            var HeaderCaption = [];
            HeaderCaption.push("Code");
            HeaderCaption.push("Name");
            HeaderCaption.push("Hsn");






            callonServerM("../Activities/Services/CustomerReceiptPayment.asmx/GetHSNWiseProduct", OtherDetailsProd, "ProductTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "ProductSource");

        }
        else {
            ctxtClass.SetText('');
            GetObjectID('hdnHSNId').value = '';
        }
    }
    else if (ArrName == 'ProductSource') {
        var key = Id;
        if (key != null && key != '') {
            $('#ProdModel').modal('hide');
            cbtnProduct.SetText(Name);
            $("#hdnProductId").val(key);
            var hsnlist = MultiHSNCRP.split(",");
            var hsn = "";
            if (hsnlist) {
                hsnlist.forEach(function (element) {
                    if (element != "") {
                        if (hsn != "") {
                            if (hsn != element) {

 
                                jAlert("HSN mismatch detected.Product selection will clear now.", "Alert", function () {

                                    cbtnProduct.SetFocus();
                                    return;

                                });
                            }
                            else {

                            }
                        }
                        else {
                            hsn = element;
                            $("#hdtHsnCode").val(hsn);
                        }
                    }
                });
            }

            GetObjectID('hdncWiseProductId').value = key;
        }
        else {
            cbtnProduct.SetText('');
            GetObjectID('hdncWiseProductId').value = '';
        }
    }
    else if (ArrName == 'BrandSource') {
        var key = Id;
        if (key != null && key != '') {
            $('#BrandModel').modal('hide');
            ctxtBrandName.SetText(Name);
            GetObjectID('hdnBranndId').value = key;
        }
        else {
            ctxtBrandName.SetText('');
            GetObjectID('hdnBranndId').value = '';
        }
    }

}

function selectContactValue() {
    deleteAllRows();
    ctxtCustName.SetText("");
    GetObjectID('hdnCustomerId').value = "";
    GetContactPerson();
    SetDefaultBillingShippingAddress(GetObjectID('hdnCustomerId').value);
    // page.SetActiveTabIndex(1);
    SuffleRows();
    setTimeout(function () {
        ctxtCustName.SetFocus();
    }, 200);
}


$(document).ready(function () {

    $('#idOutstanding').on("click", function () {


        cOutstandingPopup.Show();
        var CustomerId = $("#hdnCustomerId").val();
        var BranchId = cddlBranch.GetValue();
        $("#hddnBranchId").val(BranchId);
        var AsOnDate = cdtTDate.GetDate().format('yyyy-MM-dd');
        $("#hddnAsOnDate").val(AsOnDate);
        $("#hddnOutStandingBlock").val('1');
        //Clear Row
        var rw = $("[id$='CustomerOutstanding_DXMainTable']").find("tr")
        for (var RowClount = 0; RowClount < rw.length; RowClount++) {
            rw[RowClount].remove();
        }
        var CheckUniqueCode = false;
        $.ajax({
            type: "POST",
            url: "SalesOrderAdd.aspx/GetCustomerOutStanding",
            data: JSON.stringify({ strAsOnDate: AsOnDate, strCustomerId: CustomerId, BranchId: BranchId }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            //async:false,
            success: function (msg) {

                CheckUniqueCode = msg.d;
                if (CheckUniqueCode == true) {
                    cCustomerOutstanding.Refresh();

                }
            }
        });
    });
});

function ShowCustomerBalance() {
    //Ajax Started
    var CustomerId = GetObjectID('hdnCustomerId').value;
    var BranchId = cddlBranch.GetValue();
    var AsOnDate = cdtTDate.GetDate().format('yyyy-MM-dd');
    $.ajax({
        type: "POST",
        url: "SalesOrderAdd.aspx/GetCustomerOutStandingAmount",
        data: JSON.stringify({ strAsOnDate: AsOnDate, strCustomerId: CustomerId, BranchId: BranchId }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        // async:false,
        success: function (msg) {

            
            OutStandingAmount = msg.d;
            if (OutStandingAmount === "") {
                $('#lblOutstanding').text('0.00');
            }
            else {
                $('#lblOutstanding').text(OutStandingAmount);
            }

        }
    });

    //End
}

function ProjectValuechangedValue(s, e) {
    var previousValue = e.previousValue;
    var newValue = e.value;
}

function ProjectListKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter") {
        s.OnButtonClick(0);
    }
}

function ProjectListButnClick(s, e) {
    //ctaggingGrid.PerformCallback('BindComponentGrid');
    clookup_Project.ShowDropDown();
}

function ProjectCodeSelected(s, e) {
    debugger;
    if (clookupPopup_ProjectCode.GetGridView().GetFocusedRowIndex() == -1) {
        cProjectCodePopup.Hide();
        //var vouchertype = $("#ComboVoucherType").val(); //cComboVoucherType.GetValue();
        //if (vouchertype == 'P') {
        //    grid.batchEditApi.StartEdit(globalRowIndex, 5);
        //}
        //else {
        //    grid.batchEditApi.StartEdit(globalRowIndex, 4);
        //}
        return;
    }
    var ProjectInlineLookUpData = clookupPopup_ProjectCode.GetGridView().GetRowKey(clookupPopup_ProjectCode.GetGridView().GetFocusedRowIndex());
    var ProjectInlinedata = ProjectInlineLookUpData.split('~')[0];
    grid.batchEditApi.StartEdit(globalRowIndex);
    var ProjectCode = clookupPopup_ProjectCode.GetValue();
    cProjectCodePopup.Hide();

    grid.GetEditor("Project_Code").SetText(ProjectCode);
    grid.GetEditor("ProjectId").SetText(ProjectInlinedata);

}


function lookup_ProjectCodeKeyDown(s, e) {
    if (e.htmlEvent.key == "Escape") {
        cProjectCodePopup.Hide();

    }
}
function ProjectCodeCallback_endcallback() {

    clookupPopup_ProjectCode.ShowDropDown();;
    clookupPopup_ProjectCode.Focus();
    clookupPopup_ProjectCode.Clear()

}
function ProjectCodeinlineSelectedPayment(s, e) {
    if ($("#hdnProjectSelectInEntryModule").val() == "1") {
        if ((clookup_Project.GetGridView().GetRowKey(clookup_Project.GetGridView().GetFocusedRowIndex()) != null)) {
            LoadDocument();
        }
    }
}

function ProjectCodeButnClick(s, e) {
    if (e.buttonIndex == 0) {
        if ($("#hdnAllowProjectInDetailsLevel").val() != "0") {
            clookupPopup_ProjectCode.Clear();
            var Type = (grid.GetEditor('Type').GetValue() != null) ? grid.GetEditor('Type').GetValue() : "0";
            var InvoiceNo = (grid.GetEditor('DocumentNo').GetValue() != null) ? grid.GetEditor('DocumentNo').GetValue() : "0";
            if (clookupPopup_ProjectCode.Clear()) {
                cProjectCodePopup.Show();
                clookupPopup_ProjectCode.Focus();
            }
            var id = (clookup_Project.GetGridView().GetRowKey(clookup_Project.GetGridView().GetFocusedRowIndex()));
            if ((Type == "On Account") && (id != null)) {

                cProjectCodeCallback.PerformCallback('Type~' + Type + "~" + id);
            }
            else {
                cProjectCodeCallback.PerformCallback('Type~' + Type + "~" + InvoiceNo);
            }
        }
    }
}
function ProjectCodeKeyDown(s, e) {


    if (e.htmlEvent.key == "Enter" || e.htmlEvent.key == "NumpadEnter") {


        s.OnButtonClick(0);
    }
}

function Project_LostFocus() {

    var gridVal = "";
    if (grid.GetVisibleRowsOnPage() > 0) {
        if ($("#hdAddEdit").val() == "Add") {
            grid.batchEditApi.StartEdit(-1);
            gridVal = grid.GetEditor("Type").GetValue();
            grid.batchEditApi.EndEdit();
        }
      else  {
            grid.batchEditApi.StartEdit(0);
            gridVal = grid.GetEditor("Type").GetValue();
            grid.batchEditApi.EndEdit();
        }
    }
    //&& Gotprojid != ""
    var Gotprojid = $("#hdnEditProjId").val();
    if (grid.GetVisibleRowsOnPage() > 0 && gridVal != "" && gridVal != null) {
        debugger;
        jConfirm('Project Change will  blank  the grid. Confirm ?', 'Confirmation Dialog', function (r) {
            if (r == true) {
                LoadDocument();
                grid.PerformCallback("GridBlank");

            }
            else {
                clookup_Project.gridView.SelectItemsByKey($("#hdnEditProjId").val())
            }
        });
    }

    var projID = clookup_Project.GetValue();
    if (projID == null || projID == "") {
        $("#ddlHierarchy").val(0);
    }
    // var projID = clookup_Project.GetValue();
    $.ajax({
        type: "POST",
        url: 'CustomerPayment.aspx/getHierarchyID',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ ProjID: projID }),
        async: false,
        success: function (msg) {
            var data = msg.d;
            $("#ddlHierarchy").val(data);
        }
    });


}

function cProject_GotFocus(s, e) {
    var ProjectId = clookup_Project.GetGridView().GetRowKey(clookup_Project.GetGridView().GetFocusedRowIndex());
    $("#hdnEditProjId").val(ProjectId);
    // clookup_Project.ShowDropDown();

}

function ProjectValueChange(s, e) {

    var projID = clookup_Project.GetValue();
    $.ajax({
        type: "POST",
        url: 'CustomerPayment.aspx/getHierarchyID',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ ProjID: projID }),
        async: false,
        success: function (msg) {
            var data = msg.d;
            $("#ddlHierarchy").val(data);
        }
    });
}