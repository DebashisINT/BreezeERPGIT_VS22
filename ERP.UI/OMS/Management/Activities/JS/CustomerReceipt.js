//====================================================Revision History =========================================================================
//1.0   V2.0.41     Sanchita    01-11-2023      26952: Instrument No. field in Cash/Bank Voucher will be mandatory if Bank selected in Cash/Bank
//====================================================End Revision History=====================================================================

var CanCallback = true;
var ReceiptObj = [];
var ReceiptObjTax = [];
var DocObj = {};
var globalRowIndex;
var PickedDocument = [];
var CRPBranchstatecode = "";
function PutDate(s, e) {
    var dt = new Date();
    cInstDate.SetDate(dt);
}
function AllControlInitilize() {
   
    if (CanCallback) {
        //$("#hdAddEdit").val("Add");

        CanCallback = false;
        
        ReceiptDetail_ID = 1;
        var obj = new Object;
        obj.ID = 'Advance';
        obj.TypeName = 'Advance';
        ReceiptObj.push(obj);
        ReceiptObjTax.push(obj);

        obj = new Object;
        obj.ID = 'Invoice';
        obj.TypeName = 'Invoice';
        ReceiptObj.push(obj);


        obj = new Object;
        obj.ID = 'CustDbNote';
        obj.TypeName = 'Debit Note';
        ReceiptObj.push(obj);

        obj = new Object;
        obj.ID = 'OnAccount';
        obj.TypeName = 'On Account';
        ReceiptObj.push(obj);

        if ($('#hdAddEdit').val() == "Add") {
            ctxtVoucherNo.SetText("");
            document.getElementById("divNumberingScheme").style.display = "block";
            cdtTDate.SetEnabled(true);
            ctxtCustName.SetEnabled(true);
            cbtnSaveNew.SetVisible(true);
            //var dt = new Date();
            //cInstDate.SetDate(dt);
            SetNumberingSchemeDataSource();
            $("#DoEdit").val("1");
            setTimeout(function () {
                cCmbScheme.Focus();
            }, 500);


        }
            /*Rev Work Date:-21.03.2022 -Copy Function add*/
        else if ($('#hdAddEdit').val() == "Copy")
        {
            clookup_Project.SetEnabled(true);
            ctxtVoucherNo.SetText("");
            document.getElementById("divNumberingScheme").style.display = "block";
            cdtTDate.SetEnabled(true);
            ctxtCustName.SetEnabled(true);
            cbtnSaveNew.SetVisible(true);
            SetNumberingSchemeDataSource();

            arrMultiPopup = [];
            var ProdObjEdit = new Object();
            var ProdArrEdit = new Object();
            ProdArrEdit = JSON.parse($("#jsonProducts").text());
            ProdObjEdit.Name = "ProductSource";
            ProdObjEdit.ArraySource = ProdArrEdit;
            arrMultiPopup.push(ProdObjEdit);


            LoadDocument();
            CreateDocumentList();
            CashBank_SelectedIndexChanged();
            cComboInstrumentTypee.SetValue($("#hdnInstrumentType").val().trim());
            ShowRunningTotal();
            selectValue();
            SuffleRows();
            EnableDisableGST();
            ReceiptDetail_ID = grid.GetVisibleRowsOnPage();
            ShowCustomerBalance();
            cddlBranch.SetEnabled(false);

            var CustomerID = GetObjectID('hdnCustomerId').value;

            if ($('#hdnDocumentSegmentSettings').val() == "1") {
                $.ajax({
                    type: "POST",
                    url: "CustomerReceipt.aspx/GetSegmentDetails",
                    data: JSON.stringify({ CustomerId: CustomerID }),
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

                                var Module_Id = OutStandingAmount.Module_Id;
                                var SegmentNo = OutStandingAmount.SegmentNo;

                                if (SegmentNo != "") {

                                    if ("1" == SegmentNo) {
                                        if (Segment1 == "0") {
                                            var div = document.getElementById('DivSegment1');
                                            div.style.display = 'none';
                                            $('#hdnValueSegment1').val("0");
                                        }
                                        else {
                                            $('#lblSegment1').text(OutStandingAmount.SegmentName1);
                                            $('#ModuleSegment1header').text(OutStandingAmount.SegmentName1);
                                            $('#hdnValueSegment1').val("1");
                                        }
                                    }
                                    else {
                                        var div = document.getElementById('DivSegment1');
                                        div.style.display = 'none';
                                        $('#hdnValueSegment1').val("0");
                                    }


                                    if ("2" == SegmentNo) {
                                        if (Segment2 == "0") {
                                            var div = document.getElementById('DivSegment2');
                                            div.style.display = 'none';
                                            $('#hdnValueSegment2').val("0");
                                        }
                                        else {
                                            $('#lblSegment2').text(OutStandingAmount.SegmentName2);
                                            $('#ModuleSegment2Header').text(OutStandingAmount.SegmentName2);
                                            $('#hdnValueSegment2').val("1");
                                        }
                                    }
                                    else {
                                        var div = document.getElementById('DivSegment2');
                                        div.style.display = 'none';
                                        $('#hdnValueSegment2').val("0");
                                    }

                                    if ("3" == SegmentNo) {
                                        if (Segment3 == "0") {
                                            var div = document.getElementById('DivSegment3');
                                            div.style.display = 'none';
                                            $('#hdnValueSegment3').val("0");
                                        }
                                        else {
                                            $('#ModuleSegment3Header').text(OutStandingAmount.SegmentName3);
                                            $('#hdnValueSegment3').val("1");
                                        }
                                    }
                                    else {
                                        var div = document.getElementById('DivSegment3');
                                        div.style.display = 'none';
                                        $('#hdnValueSegment3').val("0");
                                    }

                                    if ("4" == SegmentNo) {
                                        if (Segment4 == "0") {
                                            var div = document.getElementById('DivSegment4');
                                            div.style.display = 'none';
                                            $('#hdnValueSegment4').val("0");
                                        }
                                        else {
                                            $('#ModuleSegment4Header').text(OutStandingAmount.SegmentName4);
                                            $('#hdnValueSegment4').val("1");
                                        }
                                    }
                                    else {
                                        var div = document.getElementById('DivSegment4');
                                        div.style.display = 'none';
                                        $('#hdnValueSegment4').val("0");
                                    }

                                    if ("5" == SegmentNo) {
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
                                    else {
                                        var div = document.getElementById('DivSegment5');
                                        div.style.display = 'none';
                                        $('#hdnValueSegment5').val("0");
                                    }
                                }
                                else {

                                    if (Segment1 == "0") {
                                        var div = document.getElementById('DivSegment1');
                                        div.style.display = 'none';
                                        $('#hdnValueSegment1').val("0");
                                    }
                                    else {
                                        $('#lblSegment1').text(OutStandingAmount.SegmentName1);
                                        $('#ModuleSegment1header').text(OutStandingAmount.SegmentName1);
                                        $('#hdnValueSegment1').val("1");
                                    }

                                    if (Segment2 == "0") {
                                        var div = document.getElementById('DivSegment2');
                                        div.style.display = 'none';
                                        $('#hdnValueSegment2').val("0");
                                    }
                                    else {
                                        $('#lblSegment2').text(OutStandingAmount.SegmentName2);
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
            /*Close of Rev Work Date:-21.03.2022 -Copy Function add*/
        else {
           
            clookup_Project.SetEnabled(true);
            document.getElementById("divNumberingScheme").style.display = "none";
            cdtTDate.SetEnabled(false);
            cbtnSaveNew.SetVisible(false);
            //ctxtCustName.SetEnabled(false);
            document.getElementById("TxtHeaded").innerHTML = "Modify Customer Receipt";
            arrMultiPopup = [];
            var ProdObjEdit = new Object();
            var ProdArrEdit = new Object();
            ProdArrEdit = JSON.parse($("#jsonProducts").text());
            ProdObjEdit.Name = "ProductSource";
            ProdObjEdit.ArraySource = ProdArrEdit;
            arrMultiPopup.push(ProdObjEdit);


            LoadDocument();
            CreateDocumentList();
            CashBank_SelectedIndexChanged();
            cComboInstrumentTypee.SetValue($("#hdnInstrumentType").val().trim());
            ShowRunningTotal();
            selectValue();
            SuffleRows();
            EnableDisableGST();
            ReceiptDetail_ID = grid.GetVisibleRowsOnPage();
            ShowCustomerBalance();
            cddlBranch.SetEnabled(false);
            var CustomerID = GetObjectID('hdnCustomerId').value;

            if ($('#hdnDocumentSegmentSettings').val() == "1") {
                $.ajax({
                    type: "POST",
                    url: "CustomerReceipt.aspx/GetSegmentDetails",
                    data: JSON.stringify({ CustomerId: CustomerID }),
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

                                var Module_Id = OutStandingAmount.Module_Id;
                                var SegmentNo = OutStandingAmount.SegmentNo;

                                if (SegmentNo != "") {

                                    if ("1" == SegmentNo) {
                                        if (Segment1 == "0") {
                                            var div = document.getElementById('DivSegment1');
                                            div.style.display = 'none';
                                            $('#hdnValueSegment1').val("0");
                                        }
                                        else {
                                            $('#lblSegment1').text(OutStandingAmount.SegmentName1);
                                            $('#ModuleSegment1header').text(OutStandingAmount.SegmentName1);
                                            $('#hdnValueSegment1').val("1");
                                        }
                                    }
                                    else {
                                        var div = document.getElementById('DivSegment1');
                                        div.style.display = 'none';
                                        $('#hdnValueSegment1').val("0");
                                    }


                                    if ("2" == SegmentNo) {
                                        if (Segment2 == "0") {
                                            var div = document.getElementById('DivSegment2');
                                            div.style.display = 'none';
                                            $('#hdnValueSegment2').val("0");
                                        }
                                        else {
                                            $('#lblSegment2').text(OutStandingAmount.SegmentName2);
                                            $('#ModuleSegment2Header').text(OutStandingAmount.SegmentName2);
                                            $('#hdnValueSegment2').val("1");
                                        }
                                    }
                                    else {
                                        var div = document.getElementById('DivSegment2');
                                        div.style.display = 'none';
                                        $('#hdnValueSegment2').val("0");
                                    }

                                    if ("3" == SegmentNo) {
                                        if (Segment3 == "0") {
                                            var div = document.getElementById('DivSegment3');
                                            div.style.display = 'none';
                                            $('#hdnValueSegment3').val("0");
                                        }
                                        else {
                                            $('#ModuleSegment3Header').text(OutStandingAmount.SegmentName3);
                                            $('#hdnValueSegment3').val("1");
                                        }
                                    }
                                    else {
                                        var div = document.getElementById('DivSegment3');
                                        div.style.display = 'none';
                                        $('#hdnValueSegment3').val("0");
                                    }

                                    if ("4" == SegmentNo) {
                                        if (Segment4 == "0") {
                                            var div = document.getElementById('DivSegment4');
                                            div.style.display = 'none';
                                            $('#hdnValueSegment4').val("0");
                                        }
                                        else {
                                            $('#ModuleSegment4Header').text(OutStandingAmount.SegmentName4);
                                            $('#hdnValueSegment4').val("1");
                                        }
                                    }
                                    else {
                                        var div = document.getElementById('DivSegment4');
                                        div.style.display = 'none';
                                        $('#hdnValueSegment4').val("0");
                                    }

                                    if ("5" == SegmentNo) {
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
                                    else {
                                        var div = document.getElementById('DivSegment5');
                                        div.style.display = 'none';
                                        $('#hdnValueSegment5').val("0");
                                    }
                                }
                                else {

                                    if (Segment1 == "0") {
                                        var div = document.getElementById('DivSegment1');
                                        div.style.display = 'none';
                                        $('#hdnValueSegment1').val("0");
                                    }
                                    else {
                                        $('#lblSegment1').text(OutStandingAmount.SegmentName1);
                                        $('#ModuleSegment1header').text(OutStandingAmount.SegmentName1);
                                        $('#hdnValueSegment1').val("1");
                                    }

                                    if (Segment2 == "0") {
                                        var div = document.getElementById('DivSegment2');
                                        div.style.display = 'none';
                                        $('#hdnValueSegment2').val("0");
                                    }
                                    else {
                                        $('#lblSegment2').text(OutStandingAmount.SegmentName2);
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


        GridAddnewRow();

    }
}

function SetLostFocusonDemand(e) {
    if ((new Date($("#hdnLockFromDate").val()) <= cdtTDate.GetDate()) && (cdtTDate.GetDate() <= new Date($("#hdnLockToDate").val()))) {
        jAlert("DATA is Freezed between   " + $("#hdnLockFromDateCon").val() + " to " + $("#hdnLockToDateCon").val() + " for Add.");
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
    OtherDetails.VoucherType = "R";
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
        //$("#ddlBranch").val(branchID);
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
    // Rev 1.0
    $("#MandatoryInstNo").hide();
    // End of Rev 1.0

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
            var IndexNo = globalRowIndex;
            SuffuleSerialNumber();
            EnableDisableGST();
            ShowRunningTotal();
            setTimeout(function () {
                grid.batchEditApi.StartEdit(globalRowIndex, 2);
            }, 200);

        }
    }
    if (e.buttonID == 'AddNew') {
        grid.batchEditApi.StartEdit(e.visibleIndex);

        if (grid.GetEditor("TypeId").GetText() == "") return;
        if (grid.GetEditor("Receipt").GetText() == "0.00" || grid.GetEditor("Receipt").GetText() == "") return;

        if (grid.GetEditor("TypeId").GetText().toUpperCase() != "ADVANCE" && grid.GetEditor("TypeId").GetText().toUpperCase() != "ONACCOUNT") {
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

function EnableDisableGST() {
    var TotRowNumber = grid.GetVisibleItemsOnPage();
    var SlnoCount = 1;
    for (var i = 0; i < 1000; i++) {
        if (grid.GetRow(i)) {
            if (grid.GetRow(i).style.display != "none") {
                grid.batchEditApi.StartEdit(i, 2);
                if (grid.GetEditor("TypeId").GetText().toUpperCase() == "INVOICE" || grid.GetEditor("TypeId").GetText().toUpperCase() == "ONACCOUNT" || grid.GetEditor("TypeId").GetText().toUpperCase() == "CUSTDBNOTE") {
                    document.getElementById("CB_GSTApplicable").checked = false;
                    document.getElementById("CB_GSTApplicable").disabled = true;
                    return;
                }
                else {
                    document.getElementById("CB_GSTApplicable").disabled = false;
                }
            }
        }
    }

    for (i = -1; i > -1000; i--) {
        if (grid.GetRow(i)) {
            if (grid.GetRow(i).style.display != "none") {
                grid.batchEditApi.StartEdit(i, 2);
                if (grid.GetEditor("TypeId").GetText().toUpperCase() == "INVOICE" || grid.GetEditor("TypeId").GetText().toUpperCase() == "ONACCOUNT" || grid.GetEditor("TypeId").GetText().toUpperCase() == "CUSTDBNOTE") {
                    document.getElementById("CB_GSTApplicable").checked = false;
                    document.getElementById("CB_GSTApplicable").disabled = true;
                    return;
                }
                else {
                    document.getElementById("CB_GSTApplicable").disabled = false;
                }
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

    if (ReceiptDetail_ID == "" || ReceiptDetail_ID == null) {
        ReceiptDetail_ID = 1;
    }
    else {
        ReceiptDetail_ID = ReceiptDetail_ID + 1;
    }
    grid.GetEditor("UpdateEdit").SetText(ReceiptDetail_ID + 1);
    grid.GetEditor("ReceiptDetail_ID").SetText(parseInt(ReceiptDetail_ID));
}



function grid_SalesInvoiceOnEndCallback(s, e) {
}

function OnEndCallback(s, e) {

    cLoadingPanelCRP.Hide();

    if (grid.cpGridBlank == "GridBlank") {
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
            jAlert("DATA is Freezed between " + grid.cpAddLockStatus +" for Add.");
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
    ReceiptDetail_ID = 1;
    document.getElementById('txtCustSearch').value = "";
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


    debugger;


    var newobj = [];
    txt = "<table border='1' width='100%' class='dynamicPopupTbl'><tr class='HeaderStyle'><td style='display:none;' >DocId</td><td>Document Number</td><td>Document Date</td><td>Unit</td><td>Unpaid Amount</td><td style='display:none;' >ProjectId</td><td style='display:none;' >Project_Code</td></tr>";
    DocTable.innerHTML = txt;
    $("#txtDocSearch").val("");

    grid.batchEditApi.StartEdit(globalRowIndex);
    var DocType = grid.GetEditor("TypeId").GetText();



    if (DocType == "Advance" || DocType == "OnAccount" || DocType == "") {
        return;
    }
    LoadDocument();

    var SegmentCode1 = "";
    var SegmentCode2 = "";
    var SegmentCode3 = "";
    var SegmentCode4= "";
    var SegmentCode5 = "";

    if ($("#hdnSegment1").val() != "")
    {
        SegmentCode1 = $("#hdnSegment1").val();
    }
    else if ($("#hdnSegment2").val() != "")
    {
        SegmentCode2 = $("#hdnSegment2").val();
    }
    else if ($("#hdnSegment3").val() != "") {
        SegmentCode3 = $("#hdnSegment3").val();
    }
    else if ($("#hdnSegment4").val() != "") {
        SegmentCode4 = $("#hdnSegment4").val();
    }
    else if ($("#hdnSegment5").val() != "") {
        SegmentCode5 = $("#hdnSegment5").val();
    }
    
    if (DocType == "Invoice")
    {
        var receiptdetails = $.grep(DocObj, function (e) { return e.Type == DocType && !PickedDocument.includes(e.UniqueId) && e.Segment1 == SegmentCode1 && e.Segment2 == SegmentCode2 && e.Segment3 == SegmentCode3 && e.Segment4 == SegmentCode4 && e.Segment5 == SegmentCode5 });

    }
    else
    {
        var receiptdetails = $.grep(DocObj, function (e) { return e.Type == DocType && !PickedDocument.includes(e.UniqueId);; });

    }





    for (var i = 0; i < receiptdetails.length; i++) {

        var obj = {};
        obj.Invoice_Id = receiptdetails[i]["Invoice_Id"];
        obj.DocumentNumber = receiptdetails[i]["DocumentNumber"];
        obj.DocumentType = receiptdetails[i]["DocumentType"];
        obj.DocDate = receiptdetails[i]["DocDate"];
        obj.branch = receiptdetails[i]["branch"];
        obj.UnPaidAmount = receiptdetails[i]["UnPaidAmount"];
        obj.ProjectId = receiptdetails[i]["ProjectId"];
        obj.Project_Code = receiptdetails[i]["Project_Code"];

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




function TypeButnClick(s, e) {
    debugger;
    if ($("#hdnCustomerId").val() != "" && $("#hdnCustomerId").val() != null && $("#hdnCustomerId").val() != "undefined") {
        if (e.buttonIndex == 0) {

            if (!document.getElementById("CB_GSTApplicable").checked)
                PopulateType(ReceiptObj, "TypeTable", "TypeIndex", "SetType");
            else
                PopulateType(ReceiptObjTax, "TypeTable", "TypeIndex", "SetType");



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
    var IndexNo = globalRowIndex;
    if (name.toUpperCase() == "INVOICE" || name.toUpperCase() == "ON ACCOUNT" || name.toUpperCase() == "DEBIT NOTE") {
        if (cbtnProduct.GetText() != "") {
            jConfirm("selecting " + name + " type will clear product details are you sure?", "Confirmation Dialog", function (r) {
                if (r == true) {

                    cbtnProduct.SetText("");
                    $("#hdnProductId").val("");
                    $("#hdtHsnCode").val("");




                    arrMultiPopup[0].ArraySource = []
                 

                    grid.GetEditor("Type").SetValue(name);
                    grid.GetEditor("TypeId").SetValue(id);

                    grid.GetEditor("DocumentNo").SetValue("");
                    grid.GetEditor("Receipt").SetValue("0.00");
                    grid.GetEditor("IsOpening").SetValue("");
                    grid.GetEditor("ActualAmount").SetValue("0.00");
                    grid.GetEditor("DocId").SetValue("");
                    grid.GetEditor("ProjectId").SetValue("0");
                    grid.GetEditor("Project_Code").SetValue("");

                    //var Type = (grid.GetEditor('Type').GetValue() != null) ? grid.GetEditor('Type').GetValue() : "0";
                    //if ($("#hdnProjectSelectInEntryModule").val() == "1") {

                    //    if ((Type == "On Account") || (Type=="Advance")) {
                    //        if ((clookup_Project.GetGridView().GetRowKey(clookup_Project.GetGridView().GetFocusedRowIndex()) != null)) {
                    //            var id = (clookup_Project.GetGridView().GetRowKey(clookup_Project.GetGridView().GetFocusedRowIndex()));
                    //            var text = clookup_Project.GetValue();
                    //            grid.GetEditor("ProjectId").SetValue(id);
                    //            grid.GetEditor("Project_Code").SetValue(text);
                    //        }
                    //    }
                    //}


                    ShowRunningTotal();
                    CreateDocumentList();

                    document.getElementById("CB_GSTApplicable").checked = false;
                    document.getElementById("CB_GSTApplicable").disabled = true;

                    EnableDisableGST();
                    grid.batchEditApi.EndEdit();
                    globalRowIndex = IndexNo;
                    CloseSubModal();

                }
                else {
                    return;
                }
            });
        }
        else {
            document.getElementById("CB_GSTApplicable").checked = false;
            document.getElementById("CB_GSTApplicable").disabled = true;
            grid.GetEditor("Type").SetValue(name);
            grid.GetEditor("TypeId").SetValue(id);

            grid.GetEditor("DocumentNo").SetValue("");
            grid.GetEditor("Receipt").SetValue("0.00");
            grid.GetEditor("IsOpening").SetValue("");
            grid.GetEditor("ActualAmount").SetValue("0.00");
            grid.GetEditor("DocId").SetValue("");
            grid.GetEditor("ProjectId").SetValue("0");
            grid.GetEditor("Project_Code").SetValue("");

            var Type = (grid.GetEditor('Type').GetValue() != null) ? grid.GetEditor('Type').GetValue() : "0";
            if ($("#hdnProjectSelectInEntryModule").val() == "1") {

                if ((Type == "On Account") || (Type == "Advance")) {
                    if ((clookup_Project.GetGridView().GetRowKey(clookup_Project.GetGridView().GetFocusedRowIndex()) != null)) {
                        var id = (clookup_Project.GetGridView().GetRowKey(clookup_Project.GetGridView().GetFocusedRowIndex()));
                        var text = clookup_Project.GetValue();
                        grid.GetEditor("ProjectId").SetValue(id);
                        grid.GetEditor("Project_Code").SetValue(text);
                    }
                }
            }


            ShowRunningTotal();
            CreateDocumentList();
            EnableDisableGST();
            grid.batchEditApi.EndEdit();
            globalRowIndex = IndexNo;
            CloseSubModal();

        }
    }
    else {
        grid.GetEditor("Type").SetValue(name);
        grid.GetEditor("TypeId").SetValue(id);

        grid.GetEditor("DocumentNo").SetValue("");
        grid.GetEditor("Receipt").SetValue("0.00");
        grid.GetEditor("IsOpening").SetValue("");
        grid.GetEditor("ActualAmount").SetValue("0.00");
        grid.GetEditor("DocId").SetValue("");
        grid.GetEditor("ProjectId").SetValue("0");
        grid.GetEditor("Project_Code").SetValue("");

        var Type = (grid.GetEditor('Type').GetValue() != null) ? grid.GetEditor('Type').GetValue() : "0";
        if ($("#hdnProjectSelectInEntryModule").val() == "1") {

            if ((Type == "On Account") || (Type == "Advance")) {
                if ((clookup_Project.GetGridView().GetRowKey(clookup_Project.GetGridView().GetFocusedRowIndex()) != null)) {
                    var id = (clookup_Project.GetGridView().GetRowKey(clookup_Project.GetGridView().GetFocusedRowIndex()));
                    var text = clookup_Project.GetValue();
                    grid.GetEditor("ProjectId").SetValue(id);
                    grid.GetEditor("Project_Code").SetValue(text);
                }
            }
        }


        ShowRunningTotal();
        CreateDocumentList();

        EnableDisableGST();
        grid.batchEditApi.EndEdit();
        globalRowIndex = IndexNo;
        CloseSubModal();
    }
   
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



    var ProdObj = new Object();
    ProdObj.Name = "ProductSource";
    ProdObj.ArraySource = ProdArr;
    arrMultiPopup.push(ProdObj);

    $('#TypeModal').on('shown.bs.modal', function () {
        $("input[customtypeindex=0]").focus();

    })

    $('#TypeModal').on('hide.bs.modal', function () {
        setTimeout(function () {
            IndexNo = globalRowIndex;
            EnableDisableGST();

            setTimeout(function () {
                grid.batchEditApi.EndEdit();
                grid.batchEditApi.StartEdit(IndexNo, 3);
            }, 200);
        }, 200);
    })
    $('#CustModel').on('hide.bs.modal', function () {
        setTimeout(function () {
            grid.batchEditApi.StartEdit(globalRowIndex, 2);
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
        if (event.keyCode == 83 && event.altKey == true && $("#DoEdit").val() == "1") {
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
function ParentCustomerOnClose(newCustId, CustomerName, Unique) {
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
        if ((cCmbScheme.GetValue() != "0" && cCmbScheme.GetValue() != null && cCmbScheme.GetValue() != "undefined") || $('#hdAddEdit').val() != "Add"){
        var txt= "<table border='1' width='100%' class='dynamicPopupTbl'> <tr class='HeaderStyle'><th class='hide'>id</th><th>Name</th><th>Unique Id</th><th>Address</th></tr></table>";                               
        document.getElementById("CustomerTable").innerHTML = txt;

        var Contacttype = ($("[id$='rdl_Contact']").find(":checked").val() != null) ? $("[id$='rdl_Contact']").find(":checked").val() : "CL";
        if (Contacttype.toUpperCase() == "CL")
            $('#CustModel').find('.modal-title').text("Customer Search");
        else
            $('#CustModel').find('.modal-title').text("Vendor Search");

        $('#CustModel').modal('show');
        setTimeout(function () { $('#txtCustSearch').focus(); }, 500);
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
        if (Contacttype=="CL")
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
        // GetProformaInvoice();
        var startDate = new Date();
        startDate = cdtTDate.GetValueString();
        cCBPProformaInvoice.PerformCallback(key + '~' + startDate);
        SetDefaultBillingShippingAddress(GetObjectID('hdnCustomerId').value);
        // page.SetActiveTabIndex(1);
        setTimeout(function () {
            cddlContactPerson.SetFocus();
        }, 200);

        ShowCustomerBalance();
        clookup_Project.gridView.Refresh();


        if ($('#hdnDocumentSegmentSettings').val() == "1") {
            $.ajax({
                type: "POST",
                url: "CustomerReceipt.aspx/GetSegmentDetails",
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

                            var Module_Id = OutStandingAmount.Module_Id;
                            var SegmentNo = OutStandingAmount.SegmentNo;

                            if (SegmentNo != "") {

                                if ("1" == SegmentNo) {
                                    if (Segment1 == "0") {
                                        var div = document.getElementById('DivSegment1');
                                        div.style.display = 'none';
                                        $('#hdnValueSegment1').val("0");
                                    }
                                    else {
                                        $('#lblSegment1').text(OutStandingAmount.SegmentName1);
                                        $('#ModuleSegment1header').text(OutStandingAmount.SegmentName1);
                                        $('#hdnValueSegment1').val("1");
                                    }
                                }
                                else {
                                    var div = document.getElementById('DivSegment1');
                                    div.style.display = 'none';
                                    $('#hdnValueSegment1').val("0");
                                }


                                if ("2" == SegmentNo) {
                                    if (Segment2 == "0") {
                                        var div = document.getElementById('DivSegment2');
                                        div.style.display = 'none';
                                        $('#hdnValueSegment2').val("0");
                                    }
                                    else {
                                        $('#lblSegment2').text(OutStandingAmount.SegmentName2);
                                        $('#ModuleSegment2Header').text(OutStandingAmount.SegmentName2);
                                        $('#hdnValueSegment2').val("1");
                                    }
                                }
                                else {
                                    var div = document.getElementById('DivSegment2');
                                    div.style.display = 'none';
                                    $('#hdnValueSegment2').val("0");
                                }

                                if ("3" == SegmentNo) {
                                    if (Segment3 == "0") {
                                        var div = document.getElementById('DivSegment3');
                                        div.style.display = 'none';
                                        $('#hdnValueSegment3').val("0");
                                    }
                                    else {
                                        $('#ModuleSegment3Header').text(OutStandingAmount.SegmentName3);
                                        $('#hdnValueSegment3').val("1");
                                    }
                                }
                                else {
                                    var div = document.getElementById('DivSegment3');
                                    div.style.display = 'none';
                                    $('#hdnValueSegment3').val("0");
                                }

                                if ("4" == SegmentNo) {
                                    if (Segment4 == "0") {
                                        var div = document.getElementById('DivSegment4');
                                        div.style.display = 'none';
                                        $('#hdnValueSegment4').val("0");
                                    }
                                    else {
                                        $('#ModuleSegment4Header').text(OutStandingAmount.SegmentName4);
                                        $('#hdnValueSegment4').val("1");
                                    }
                                }
                                else {
                                    var div = document.getElementById('DivSegment4');
                                    div.style.display = 'none';
                                    $('#hdnValueSegment4').val("0");
                                }

                                if ("5" == SegmentNo) {
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
                                else {
                                    var div = document.getElementById('DivSegment5');
                                    div.style.display = 'none';
                                    $('#hdnValueSegment5').val("0");
                                }
                            }
                            else {
                               
                                    if (Segment1 == "0") {
                                        var div = document.getElementById('DivSegment1');
                                        div.style.display = 'none';
                                        $('#hdnValueSegment1').val("0");
                                    }
                                    else {
                                        $('#lblSegment1').text(OutStandingAmount.SegmentName1);
                                        $('#ModuleSegment1header').text(OutStandingAmount.SegmentName1);
                                        $('#hdnValueSegment1').val("1");
                                    }                              

                                    if (Segment2 == "0") {
                                        var div = document.getElementById('DivSegment2');
                                        div.style.display = 'none';
                                        $('#hdnValueSegment2').val("0");
                                    }
                                    else {
                                        $('#lblSegment2').text(OutStandingAmount.SegmentName2);
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
}
function ValueSelected(e, indexName) {
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var Id = e.target.parentElement.parentElement.cells[0].innerText;
        var name = e.target.parentElement.parentElement.cells[1].children[0].value;
        if (Id) {
            SetCustomer(Id, name);

        }
        else if (indexName == "segment1Index") {
            Setsegment1(Id, name);
        }
        else if (indexName == "segment2Index") {
            Setsegment2(Id, name);
        }
        else if (indexName == "segment3Index") {
            Setsegment3(Id, name);
        }
        else if (indexName == "segment4Index") {
            Setsegment4(Id, name);
        }
        else if (indexName == "segment5Index") {
            Setsegment5(Id, name);
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
function CloseGridQuotationLookup() {
    
    clookup_ProformaInvoice.ConfirmCurrentSelection();
    clookup_ProformaInvoice.HideDropDown();
   
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
    OtherDetails.AsOnDate = cdtTDate.GetDate().format('yyyy-MM-dd');
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

function LoadDocument()
{
    debugger;
    if ($("#hdnCustomerId").val() != "")
    {
        var OtherDetails = {}
        OtherDetails.VoucherType = "R";
        OtherDetails.CustomerId = $("#hdnCustomerId").val();
        OtherDetails.BranchId = cddlBranch.GetValue();
        OtherDetails.ReceiptPaymentId = $("#ReceiptPaymentId").text();
        OtherDetails.TransDate = cdtTDate.GetText();



        if ($("#hdnProjectSelectInEntryModule").val() == "1")
        {
            if ((clookup_Project.GetGridView().GetRowKey(clookup_Project.GetGridView().GetFocusedRowIndex()) != null) && clookup_Project.GetText()!="")
            {
                OtherDetails.ProjectId = (clookup_Project.GetGridView().GetRowKey(clookup_Project.GetGridView().GetFocusedRowIndex()));

                $.ajax({
                    type: "POST",
                    url: "../Activities/Services/CustomerReceiptPayment.asmx/GetAllDocumentWithProject",
                    data: JSON.stringify(OtherDetails),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async:false,
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
                    url: "../Activities/Services/CustomerReceiptPayment.asmx/GetAllDocument",
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
                url: "../Activities/Services/CustomerReceiptPayment.asmx/GetAllDocument",
                data: JSON.stringify(OtherDetails),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
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



    if (ReceiptDetail_ID == "" || ReceiptDetail_ID == null) {
        ReceiptDetail_ID = 1;
    }
    else {
        ReceiptDetail_ID = parseInt(ReceiptDetail_ID) + 1;
    }

    grid.GetEditor("ReceiptDetail_ID").SetText(parseInt(ReceiptDetail_ID));

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
        var receiptdetails = $.grep(DocObj, function (e) { return e.Type == DocType && !PickedDocument.includes(e.UniqueId); });

        var SearchObj = $("#txtDocSearch").val();
        if (SearchObj != "") {
            for (var i = 0; i < receiptdetails.length; i++) {

                var obj = {};
                var NewCode = receiptdetails[i]["DocumentNumber"];
                if (NewCode.toUpperCase().includes(SearchObj.toUpperCase())) {
                    //NewCode = NewCode.toUpperCase().replace(SearchObj.toUpperCase(), '<mark><b>' + SearchObj.toUpperCase() + '</b></mark>');


                    obj.Invoice_Id = receiptdetails[i]["Invoice_Id"];
                    obj.DocumentNumber = NewCode;
                    obj.DocumentType = receiptdetails[i]["DocumentType"];
                    obj.DocDate = receiptdetails[i]["DocDate"];
                    obj.branch = receiptdetails[i]["branch"];
                    obj.UnPaidAmount = receiptdetails[i]["UnPaidAmount"];
                    obj.ProjectId = receiptdetails[i]["ProjectId"];
                    obj.Project_Code = receiptdetails[i]["Project_Code"];

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

    txt = "<table border='1' width='100%'  class='dynamicPopupTbl' id='floatedTbl'><tbody><tr class='HeaderStyle'><th style='display:none;' >DocId</th><th>Document Number</th><th >Document Date</th><th >Unit</th><th >Unpaid Amount</th><th style='display:none;' >ProjectId</th><th style='display:none;' >Project_Code</th></tr>";


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
                txt += "<td>" + myObj[x][key] + "</td>"
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
    grid.GetEditor("Receipt").SetValue(Unpaid);
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


function SaveButtonClick() {
    var ProjectCode = clookup_Project.GetText();
    if ($("#hdnProjectSelectInEntryModule").val() == "1" && $("#hdnProjectMandatory").val() == "1" && ProjectCode == "") {
        jAlert("Please Select Project.");
        return false;
    }

    if (validation()) {
        $("#hdnRefreshType").val("E");
        if (document.getElementById("CB_GSTApplicable").checked && $("#hdtHsnCode").val() != "") {
            LoadBranchStateCode();
            if (CRPBranchstatecode != GeteShippingStateCode()) {
                var msg = confirm("Branch address and Customer shipping address is different. Do you want proceed with IGST calculation?");
                if (msg == false) {
                    cLoadingPanelCRP.Hide();
                    return false;
                }
                else {
                    SelectAllData();
                    grid.UpdateEdit();
                }
            }
            else {
                SelectAllData();
                grid.UpdateEdit();
            }
        }
        else {
            SelectAllData();
            grid.UpdateEdit();
        }



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

        if (document.getElementById("CB_GSTApplicable").checked && $("#hdtHsnCode").val() != "") {
            LoadBranchStateCode();
            if (CRPBranchstatecode != GeteShippingStateCode()) {
                var msg = confirm("Branch address and Customer shipping address is different. Do you want proceed with IGST calculation?");
                if (msg == false) {
                    cLoadingPanelCRP.Hide();
                    return false;
                }
                else {
                    SelectAllData();
                    grid.UpdateEdit();
                }
            }
            else {
                SelectAllData();
                grid.UpdateEdit();
            }
        }
        else {
            SelectAllData();
            grid.UpdateEdit();
        }

    }
}


function LoadBranchStateCode() {


    var OtherDetails = {}
    OtherDetails.BranchId = cddlBranch.GetValue();
    $.ajax({
        async: false,
        type: "POST",
        url: "../Activities/Services/CustomerReceiptPayment.asmx/GetBranchStateCode",
        data: JSON.stringify(OtherDetails),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var returnObject = msg.d;

            if (returnObject) {
                CRPBranchstatecode = returnObject;
            }

        }
    });


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
    // Rev 1.0
    var InstrumentType = cComboInstrumentTypee.GetValue();
    if (InstrumentType != "NA" && InstrumentType != "CH") {
        if (ctxtInstNobth.GetText() == null || ctxtInstNobth.GetText() == "") {
            cLoadingPanelCRP.Hide();
            $("#MandatoryInstNo").show();
            return false;
        }
        else {
            $("#MandatoryInstNo").hide();
        }
    }
    // End of Rev 1.0
    if (VoucherAmount == "0.00") {

        jAlert("Voucher amount must be greater then ZERO.");
        cLoadingPanelCRP.Hide();
        return false;
    }


    if (document.getElementById("CB_GSTApplicable").checked && ($("#hdtHsnCode").val() == "" || $("#hdtHsnCode").val() == null)) {
        jAlert("Please select a valid product to proceed.");
        cLoadingPanelCRP.Hide();
        return false;
    }

    if (document.getElementById("CB_GSTApplicable").checked && $("#hdtHsnCode").val() != "" && (ctxtbillingPin.GetText() == "" || ctxtShippingPin.GetText() == "" || ctxtAddress1.GetText() == "" || ctxtsAddress1.GetText() == "")) {
        jAlert("Please select a valid address to calculate GST and proceed.", function () {
            cLoadingPanelCRP.Hide();
            page.SetActiveTabIndex(1);

        });

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

    for (var i = 0; i < 1000; i++) {
        if (grid.GetRow(i)) {
            if (grid.GetRow(i).style.display != "none") {
                grid.batchEditApi.StartEdit(i, 2);
                if (grid.GetEditor("TypeId").GetText() == "" && grid.GetEditor("Receipt").GetText() != "0.00" && grid.GetEditor("Receipt").GetText() != "") {
                    cLoadingPanelCRP.Hide();
                    jAlert("Please select a valid type to proceed.");
                    return false;
                }

                if (grid.GetEditor("TypeId").GetText() != "" && (grid.GetEditor("Receipt").GetText() == "0.00" || grid.GetEditor("Receipt").GetText() == "")) {
                    cLoadingPanelCRP.Hide();
                    jAlert("Please enter a valid amount to proceed.");
                    return false;
                }

                if (grid.GetEditor("TypeId").GetText().toUpperCase() != "ADVANCE" && grid.GetEditor("TypeId").GetText().toUpperCase() != "ONACCOUNT" && grid.GetEditor("TypeId").GetText().toUpperCase() != "") {

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
                if (grid.GetEditor("TypeId").GetText() == "" && grid.GetEditor("Receipt").GetText() != "0.00" && grid.GetEditor("Receipt").GetText() != "") {
                    cLoadingPanelCRP.Hide();
                    jAlert("Please select a valid type to proceed.");
                    return false;
                }

                if (grid.GetEditor("TypeId").GetText() != "" && (grid.GetEditor("Receipt").GetText() == "0.00" || grid.GetEditor("Receipt").GetText() == "")) {
                    cLoadingPanelCRP.Hide();
                    jAlert("Please enter a valid amount to proceed.");
                    return false;
                }

                if (grid.GetEditor("TypeId").GetText().toUpperCase() != "ADVANCE" && grid.GetEditor("TypeId").GetText().toUpperCase() != "ONACCOUNT" && grid.GetEditor("TypeId").GetText().toUpperCase() != "") {

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
                totalAmount = totalAmount + DecimalRoundoff(grid.GetEditor("Receipt").GetText(), 2);
            }
        }
    }

    for (i = -1; i > -1000; i--) {
        if (grid.GetRow(i)) {
            if (grid.GetRow(i).style.display != "none") {
                grid.batchEditApi.StartEdit(i, 2);
                totalAmount = totalAmount + DecimalRoundoff(grid.GetEditor("Receipt").GetText(), 2);
            }
        }
    }

    return totalAmount;
}


function Receipt_LostFocus(s, e) {

    grid.batchEditApi.StartEdit(globalRowIndex, 4);
    var TypeID = grid.GetEditor("TypeId").GetText();
    var DocID = grid.GetEditor("DocId").GetText();
    var ActualAmount = grid.GetEditor("ActualAmount").GetText();
    var Receipt = grid.GetEditor("Receipt").GetText();

    if (TypeID.toUpperCase() != "ADVANCE" && TypeID.toUpperCase() != "ONACCOUNT") {

        if (DocID != "") {
            if (parseFloat(Receipt) > parseFloat(ActualAmount)) {
                var NewAmt = grid.GetEditor("ActualAmount");
                s.SetValue(NewAmt.GetValue());
                jAlert("Receipt amount can not be greater than unpaid amount.");
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
    if ($("#hdnProjectSelectInEntryModule").val() == "1") {
        setTimeout(function () {
            grid.batchEditApi.EndEdit();
            grid.batchEditApi.StartEdit(globalRowIndex, 5);
        }, 300);
    }
    else
    {
        setTimeout(function () {
            grid.batchEditApi.EndEdit();
            grid.batchEditApi.StartEdit(globalRowIndex, 6);
        }, 300);
    }
}

function ShowRunningTotal() {
    var VoucherAmount = ctxtVoucherAmount.GetValue();

    var TotAmt = DecimalRoundoff(GetTotalAmount(), 2);

    c_txt_Debit.SetText(TotAmt.toString());
    ctxtVoucherAmount.SetValue(TotAmt.toString());
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
    debugger;
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
    debugger;
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

                                cbtnProduct.SetText("");
                                $("#hdnProductId").val("");
                                $("#hdtHsnCode").val("");

                                var Multiselect = new Object()
                                Multiselect.chkId = "";
                                Multiselect.Id = "";
                                Multiselect.Name = "";
                                Multiselect.hsn = "";



                                // arrMultiPopup.push(Multiselect);
                                arrMultiPopup[0].ArraySource = []

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


function Save_TaxesClick() {

    if (!document.getElementById("CB_GSTApplicable").checked)
    { return; }
    else {
        if ($("#hdtHsnCode").val() == "") {
            return
        }
        else {

            if ($('#hdStateCodeShipping').val() != "") {

                $('#TaxModal').modal('show');
                //string hsnCodeTax,decimal Amount,string branchid,string customerstate

                var OtherDetails = {}
                var HeaderCaption = [];

                OtherDetails.hsnCodeTax = $("#hdtHsnCode").val();
                OtherDetails.Amount = ctxtVoucherAmount.GetText();
                OtherDetails.branchid = cddlBranch.GetValue();
                OtherDetails.customerstate = $('#hdStateCodeShipping').val();

                HeaderCaption.push("Sl#");
                HeaderCaption.push("Taxe Name");
                HeaderCaption.push("Tax Type");
                HeaderCaption.push("Tax Rate");
                HeaderCaption.push("Tax Code");
                HeaderCaption.push("Taxable Amount");
                HeaderCaption.push("Tax Amount");


                callonServer("../Activities/Services/CustomerReceiptPayment.asmx/GetTaxTableSet", OtherDetails, "TaxTable", HeaderCaption, "TaxIndex", "SetSelectedValues");

            }
        }
    }




}

function CloseTaxModal() {
    $('#TaxModal').modal('hide');
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




//<%-- For multiselection--%>



function ShowTCS() {

    var CustomerId = $("#hdnCustomerId").val();
    var doc_id = $("#ReceiptPaymentId").text();
    var date = cdtTDate.GetText();
    var totalAmount = ctxtVoucherAmount.GetText();

    var obj = {};
    obj.CustomerId = CustomerId;
    obj.doc_d = doc_id;
    obj.date = date;
    obj.totalAmount = totalAmount;
    obj.branch_id = cddlBranch.GetValue();

    if (doc_id == "" || doc_id == null) {
        $.ajax({
            type: "POST",
            url: 'CustomerReceipt.aspx/getTCSDetails',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(obj),
            success: function (msg) {

                if (msg) {
                    var response = msg.d;
                    ctxtTCSSection.SetText(response.Code);
                    ctxtTCSapplAmount.SetText(response.tds_amount);
                    ctxtTCSpercentage.SetText(response.Rate);
                    ctxtTCSAmount.SetText(response.Amount);
                }


            }
        });
    }


    $("#tcsModal").modal('show');
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
   // SetDefaultSegmentBillingShippingAddress($("#hdnCustomerId").val(), Id);

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

   // SetDefaultSegmentBillingShippingAddress($("#hdnCustomerId").val(), Id);
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

    //SetDefaultSegmentBillingShippingAddress($("#hdnCustomerId").val(), Id);
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
  //  SetDefaultSegmentBillingShippingAddress($("#hdnCustomerId").val(), Id);
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

   // SetDefaultSegmentBillingShippingAddress($("#hdnCustomerId").val(), Id);
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