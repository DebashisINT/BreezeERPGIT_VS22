
var ISdatatable = 0;
var tdspay = 0;


function AfterSaveBillingShipiing(validate) {
    // GetPurchaseForGstValue();
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


function selectTDSChange(s, e) {
    document.getElementById("chkall").checked = false;
    document.getElementById("lblcount").innerHTML = 0;
    cdtTDate.SetEnabled(false);
    var ID = ctdsSection.GetValue();
    var desc = ID.split('~')[1].trim();
    var code = ID.split('~')[0].trim();
    ctxtDeductionON.SetText(desc);
    var str = "";
    if (ISdatatable == 0) {
        str = "<thead><tr><th>Select</th><th class='hide'></th><th class='hide'></th><th class='hide'></th><th>Document No.</th><th>Party ID</th><th>Section</th><th>Payment/Credit Date</th><th>Total Tax</th><th>Amount of Tax</th><th>Surcharge</th><th>Edu. Cess</th><th class=''>IsOpening</th></tr></thead>";
    }
    //
    str += "<tbody>";

    // Mantis Issue 24153 [ new parameter "chkBranchWise" and "ddlBranch" added ]
    $.ajax({
        type: "POST",
        url: "CashBankEntry.aspx/GETTDSDOCDETAILS",
        data: JSON.stringify({ TDSPaymentDate: cdtTDate.GetDate(), TDSCode: code, TDSQuater: $("#ddlQuater").val(), TDSYear: $("#ddlFinYear").val(), Type: $("#ddlEntityType").val(), BranchWise: document.getElementById('chkBranchWise').checked, Branch: $('#ddlBranch').val() }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {
            var data = msg.d;
            tdspay = data;
            // debugger;
            for (var i = 0; i < data.length; i++) {
                document.getElementById("chkall").disabled = false;

                var obj = {};
                obj.newId = data[i].DETID;


                str += "<tr>";
                str += "<td><input onclick='iCheckClick(this,\"" + data[i].DETID + "\");'  type='checkbox' id='chk" + data[i].DETID + "'/></td>";
                str += "<td class='hide'>" + data[i].DETID + "</td>";
                str += "<td class='hide'>" + data[i].VendorId + "</td>";
                str += "<td class='hide'>" + data[i].MainAccountID + "</td>";
                str += "<td>" + data[i].DocumentNo + "</td>";
                str += "<td>" + data[i].PartyID + "</td>";
                str += "<td>" + data[i].TDSTCS_Code + "</td>";
                str += "<td>" + data[i].PaymentDate + "</td>";
                str += "<td>" + data[i].Total_Tax + "</td>";
                str += "<td>" + data[i].Tax_Amount + "</td>";
                str += "<td>" + data[i].Surcharge + "</td>";
                str += "<td>" + data[i].EduCess + "</td>";
                str += "<td>" + data[i].IsOpening + "</td>";

                str += "</tr>";

            }


        }
    });

    str += "</tbody>";
    $("#tbltdsDetails").html('');
    $("#tbltdsDetails").html(str);

    //if (ISdatatable == 0) {
    $("#tbltdsDetails").dataTable().fnDestroy()
    var table = "";
    table = $('#tbltdsDetails').DataTable({
        scrollY: '300px',
        scrollCollapse: true,
        paging: false

    }).draw();
    table.order.listener('#sorter', 1);
    ISdatatable = ISdatatable + 1;
    //}



    ctxtSurcharge.SetText(0);
    ctxteduCess.SetText(0);
    ctxtTotal.SetText(0);
    ctxtTax.SetText(0);

}
var totTDSamount = 0;
var totTax = 0;
var totEdu = 0;
var totSurcharge = 0;
var totalcount = 0;

function ProjectListKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter") {
        s.OnButtonClick(0);
    }
}


$(document).ready(function () {
    if ($("#hdn_Mode").val() == "Edit") {
        clookup_Project.SetEnabled(true);
        if ($("#ddlBranch").val() != null && $("#ddlBranch").val() != "") {
            LoadBranchAddressInEditMode($('#ddlBranch').val());
        }
    }
});


function ProjectListButnClick(s, e) {
    //ctaggingGrid.PerformCallback('BindComponentGrid');
    clookup_Project.ShowDropDown();
}




function BindGridViaTDSData() {
    $("#hdnTDSSection").val(ctdsSection.GetValue());


    InsgridBatch.PerformCallback('TDSPayment~' + ctxtTotal.GetText());
    $("#TDSmodal").modal('hide');
}
var TDSSection = "";
function iCheckClick(cb, id) {
    //debugger;
    totTDSamount = 0;
    totTax = 0;
    totEdu = 0;
    totSurcharge = 0;
    totalcount = 0;
    var table = document.getElementById('tbltdsDetails');

    var rowLength = table.rows.length;

    var chkedArr = [];
    var t = 0;
    for (var i = 0; i < rowLength; i += 1) {
        var row = table.rows[i];
        if (row.children[1].innerText != "") {
            console.log($("#chk" + row.children[1].innerText).prop('checked'));

            if ($("#chk" + row.children[1].innerText).prop('checked')) {
                totalcount = totalcount + 1;
                var obj = {};

                obj.CheckedID = row.children[1].innerText;
                totTDSamount = totTDSamount + parseFloat(row.children[8].innerText);
                totTax = totTax + parseFloat(row.children[9].innerText);
                totEdu = totEdu + parseFloat(row.children[11].innerText);
                totSurcharge = totSurcharge + parseFloat(row.children[10].innerText);
                if (t == 0) {
                    TDSSection = row.children[1].innerText.replace(/["']/g, "");
                    t = t + 1;
                }
                else {
                    TDSSection = TDSSection + ',' + row.children[1].innerText.replace(/["']/g, "")
                }
                chkedArr.push(obj);
            }
            //else {
            //    alert("a");

            //}
        }


    }

    //  alert(totalcount);

    totalcount = chkedArr.length;

    //alert(totalcount);


    document.getElementById("lblcount").innerHTML = totalcount;
    //if()

    ctxtSurcharge.SetValue(parseFloat(totSurcharge));
    ctxteduCess.SetValue(parseFloat(totEdu));
    ctxtTotal.SetValue(parseFloat(totTDSamount));
    ctxtTax.SetValue(parseFloat(totTax));

}

function RecalculateTotal() {

    var total = DecimalRoundoff(totTDSamount, 2);
    //total = total + DecimalRoundoff(ctxtInterest.GetValue(), 2) + DecimalRoundoff(ctxtLateFees.GetValue(), 2) + DecimalRoundoff(ctxtOthers.GetValue(), 2);
    ctxtTotal.SetValue(parseFloat(total));
}



function ShowTDSPopup() {
    // debugger;

    $("#TDSmodal").modal('show');



    if ($("#hdnEditRfid").val() != "" && $("#hdnEditRfid").val() != "" && $("#hdnIsTDS").val() == "1") {

        $.ajax({
            type: "POST",
            url: "CashBankEntry.aspx/ShowTDSEditDetails",
            data: JSON.stringify({ doc_id: $("#hdnEditRfid").val() }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {


                var data = msg.d.tdsPayDet;

                // var str = "<thead><tr><th><input onclick='iCheckClick(this," + JSON.parse(data[i].DETID) + ");' type='checkbox' checked disabled id='chk" + data[i].DETID + "'/></th></tr></thead>";
                var str = "<thead><tr><th>Select</th><th class='hide'></th><th >Document Number<th class='hide'></th><th class='hide'></th><th>Party ID</th><th>Section</th><th>Payment/Credit Date</th><th>Total Tax</th><th>Amount of Tax</th><th>Surcharge</th><th>Edu. Cess</th></tr></thead>";
                //var str = "";
                str += "<tbody>";
                for (var i = 0; i < data.length; i++) {
                    str += "<tr>";

                    str += "<td><input onclick='iCheckClick(this," + JSON.parse(data[i].DETID) + ");' type='checkbox' checked disabled id='chk" + data[i].DETID + "'/></td>";
                    str += "<td class='hide'>" + data[i].DETID + "</td>";
                    str += "<td>" + data[i].DocumentNo + "</td>";
                    str += "<td class='hide'>" + data[i].VendorId + "</td>";
                    str += "<td class='hide'>" + data[i].MainAccountID + "</td>";
                    str += "<td>" + data[i].PartyID + "</td>";
                    str += "<td>" + data[i].TDSTCS_Code + "</td>";
                    str += "<td>" + data[i].PaymentDate + "</td>";
                    str += "<td>" + data[i].Total_Tax + "</td>";
                    str += "<td>" + data[i].Tax_Amount + "</td>";
                    str += "<td>" + data[i].Surcharge + "</td>";
                    str += "<td>" + data[i].EduCess + "</td>";
                    str += "</tr>";
                    //Rev Rajdip
                    document.getElementById("lblcount").innerHTML = i + 1;
                    //End Rev Rajdip

                }

                str += "</tbody>";
                $("#tbltdsDetails").html('');
                $("#tbltdsDetails").html(str);
                ;
                ctdsSection.SetValue(msg.d.SectionID);
                ctxtDeductionON.SetText(msg.d.DeductionON);
                $("#ddlQuater").val(msg.d.Quater);
                ctxtSurcharge.SetValue(msg.d.Surcharge);
                ctxteduCess.SetValue(msg.d.EduCess);
                ctxtInterest.SetValue(msg.d.Interest)
                ctxtTotal.SetValue(msg.d.Total);
                ctxtTax.SetValue(msg.d.Tax);
                ctxtOthers.SetValue(msg.d.Others);
                ctxtBankName.SetText(msg.d.BankName);
                ctxtBankBranch.SetText(msg.d.BankBrach);
                ctxtBRS.SetText(msg.d.BRS);
                ctxtChallanNo.SetText(msg.d.ChallanNo);
                //ctxtLateFees.SetValue(msg.d.LateFees)
                // Rev Mantis Issue 24161
                ctxtAdvance.SetValue(msg.d.Advance);
                // End of Rev Mantis Issue 24161

                document.getElementById("chkall").disabled = true;
                document.getElementById("btnsave").style.visibility = "hidden";
                document.getElementById("tdsSection").disabled = true;
                ctdsSection.SetEnabled(false);
                // ctdsSection.visibility(false); tdsSection


                //Rev Rajdip
                //  var Dtpaydate = new Date(parseInt(msg.d.Payment_Date.EndDate.substr(6)));
                var dt = msg.d.Payment_Date;
                var Dtpaydate = new Date(parseInt(dt.substr(6)));
                ctdsDate.SetDate(cdtTDate.GetDate());
                //ctdsDate.SetText(msg.d.Dtpaydate);SetMinDate
                //ctdsDate.SetText(msg.d.Payment_Date);
                //End Rev Rajdip




                $("#txtLateFees").val(msg.d.LateFees)

                $("#tbltdsDetails").dataTable().fnDestroy();

                $('#tbltdsDetails').DataTable({
                    scrollY: '200px',
                    scrollCollapse: true,
                    paging: false
                });



            }

        });
    }
    else {
        var ID = ctdsSection.GetValue();
        var desc = ID.split('~')[1].trim();
        ctdsDate.SetDate(cdtTDate.GetDate());
        document.getElementById("chkall").disabled = true;
    }

    //$.ajax({
    //    type: "POST",
    //    url: "CashBankEntry.aspx/GETTDSDOCDETAILS",
    //    data: JSON.stringify({ TDSPaymentDate: cdtTDate.GetDate(), TDSCode: ID }),
    //    contentType: "application/json; charset=utf-8",
    //    dataType: "json",
    //    success: function (msg) {
    //        var data = msg.d;
    //        ctxtRate.SetValue(data);


    //    }
    //});
}


function chechall() {
    if (document.getElementById('chkall').checked) {

        document.getElementById("hddnflag").value = "1";
        if (document.getElementById("hddnflag").value == "1") {
            for (var i = 0; i < tdspay.length; i++) {
                // document.getElementById("lblcount").innerHTML = tdspay.length
                //,'chk' + data[i].DETID
                $("#chk" + tdspay[i].DETID).prop("checked", true);
                iCheckClick(this, tdspay[i].DETID);

            }
        }
    }
    else {

        for (var i = 0; i < tdspay.length; i++) {
            //  document.getElementById("lblcount").innerHTML = 0;
            //,'chk' + data[i].DETID
            $("#chk" + tdspay[i].DETID).prop("checked", false);
            iCheckClick(this, tdspay[i].DETID);

            ctxtTotal.SetValue(0.00);

            ctxtTax.SetValue(0.00);
        }
    }
}


//  Model Function ////
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

        //callonServer("/OMS/Management/Activities/Services/Master.asmx/GetMainAccountCashBank", OtherDetails, "MainAccountTable", HeaderCaption, "MainAccountIndex", "SetMainAccount");
        callonServer("/OMS/Management/Activities/Services/Master.asmx/GetMainAccountCashBankByProcedure", OtherDetails, "MainAccountTable", HeaderCaption, "MainAccountIndex", "SetMainAccount");


    }
    else if (e.code == "ArrowDown") {
        if ($("input[MainAccountIndex=0]"))
            $("input[MainAccountIndex=0]").focus();
    }
    else if (e.code == "Escape") {
        //  
        $('#MainAccountModel').modal('hide');
        InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 1);

    }
}


function MainAccountNewkeydownRO(e) {
    var OtherDetails = {}
    OtherDetails.SearchKey = $("#txtMainAccountSearchRO").val();
    OtherDetails.branchId = $("#ddlBranch").val();
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        if ($("#txtMainAccountSearchRO").val() == "")
            return;
        var HeaderCaption = [];
        HeaderCaption.push("Main Account Name");
        HeaderCaption.push("Subledger Type");
        HeaderCaption.push("Reverse Applicable");
        HeaderCaption.push("HSN/SAC");

        //callonServer("/OMS/Management/Activities/Services/Master.asmx/GetMainAccountCashBank", OtherDetails, "MainAccountTable", HeaderCaption, "MainAccountIndex", "SetMainAccount");
        callonServer("/OMS/Management/Activities/Services/Master.asmx/GetMainAccountCashBankByProcedure", OtherDetails, "MainAccountTableRO", HeaderCaption, "MainAccountIndexRO", "SetMainAccountRO");


    }
    else if (e.code == "ArrowDown") {
        if ($("input[MainAccountIndexRO=0]"))
            $("input[MainAccountIndexRO=0]").focus();
    }
    else if (e.code == "Escape") {
        //  
        $('#MainAccountModelRO').modal('hide');

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
    InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 2);

}


function SetMainAccountRO(Id, name, e) {

    $('#MainAccountModelRO').modal('hide');
    cbtnMARoundOff.SetText(name);
    $('#hdnROMainAc').val(Id);
}




function ValueSelected(e, indexName) {
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var Id = e.target.parentElement.parentElement.cells[0].innerText;
        var name = e.target.parentElement.parentElement.cells[1].children[0].value;
        if (Id) {
            if (indexName == "MainAccountIndex") {
                $('#MainAccountModel').modal('hide');
                //var Code = e.target.parentElement.parentElement.children[2].innerText;
                var IsSub = e.target.parentElement.parentElement.children[2].innerText;
                var RevApp = e.target.parentElement.parentElement.children[3].innerText;
                if (RevApp == 'Yes') {
                    RevApp = '1';
                }
                else {
                    RevApp = '0';
                }
                var TaxAble = e.target.parentElement.parentElement.children[4].innerText;
                GetMainAcountComboBox(Id, name, IsSub, RevApp, TaxAble);
                InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 3);
            }
            else if (indexName == "MainAccountIndexRO") {
                $('#MainAccountModelRO').modal('hide');
                var IsSub = e.target.parentElement.parentElement.children[2].innerText;
                var RevApp = e.target.parentElement.parentElement.children[3].innerText;
                if (RevApp == 'Yes') {
                    RevApp = '1';
                }
                else {
                    RevApp = '0';
                }
                var TaxAble = e.target.parentElement.parentElement.children[4].innerText;
                SetMainAccountRO(Id, name, IsSub, RevApp, TaxAble);
            }
            else if (indexName == "SubAccountIndex") {
                $('#SubAccountModel').modal('hide');
                GetSubAcountComboBox(Id, name);
                InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 4);
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
            if (indexName == "MainAccountIndex")
                $('#txtMainAccountSearch').focus();
            else if (indexName == "SubAccountIndex")
                $('#txtSubAccountSearch').focus();
            else if (indexName == "MainAccountIndexRO")
                $('#txtMainAccountSearch').focus();
        }
    }
    else if (e.code == "Escape") {
        if (indexName == "MainAccountIndex") {
            $('#MainAccountModel').modal('hide');
            InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 2);


        }
        else if (indexName == "SubAccountIndex") {
            $('#SubAccountModel').modal('hide');
            InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 3);
        }
        else if (indexName == "MainAccountIndexRO") {
            $('#MainAccountModelRO').modal('hide');
        }
    }


}
function MainAccountButnClick(s, e) {
    if (e.buttonIndex == 0) {
        var txt = "<table border='1' width=\"100%\"  class=\"dynamicPopupTbl\"><tr class=\"HeaderStyle\" ><th>Main Account Name</th><th>Subledger Type</th><th>Reverse Applicable</th><th>HSN/SAC</th></tr><table>";
        document.getElementById("MainAccountTable").innerHTML = txt;
        $('#MainAccountModel').modal('show');
        cMainAccountComboBox.Focus();

    }
}

function MainAccountButnClickRO(s, e) {
    if (e.buttonIndex == 0) {
        var txt = "<table border='1' width=\"100%\"  class=\"dynamicPopupTbl\"><tr class=\"HeaderStyle\" ><th>Main Account Name</th><th>Subledger Type</th><th>Reverse Applicable</th><th>HSN/SAC</th></tr><table>";
        document.getElementById("MainAccountTableRO").innerHTML = txt;
        $('#MainAccountModelRO').modal('show');
        cMainAccountComboBox.Focus();

    }
}


function closeModal() {
    $('#MainAccountModel').modal('hide');
    InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 2);
}

function SubAccountButnClick(s, e) {


    txt = " <table border='1' width=\"100%\"  class=\"dynamicPopupTbl\"><tr class=\"HeaderStyle\"><th>Sub Account Name [Unique Id]</th><th>Sub Account Type</th></tr></table>";
    document.getElementById("SubAccountTable").innerHTML = txt;

    $("#mainActMsgSub").hide();
    if (IsSubAccount != 'None') {
        InsgridBatch.batchEditApi.StartEdit(e.visibleIndex);
        var strMainAccountID = (InsgridBatch.GetEditor('MainAccount').GetText() != null) ? InsgridBatch.GetEditor('MainAccount').GetText() : "0";
        var MainAccountID = (InsgridBatch.GetEditor('gvColMainAccount').GetValue() != null) ? InsgridBatch.GetEditor('gvColMainAccount').GetValue() : "0";
        //Add for found Main Accoun type Tanmoy
        var MainAccountType = "";
        $.ajax({
            type: "POST",
            url: 'CashBankEntry.aspx/getMainAccountType',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            data: JSON.stringify({ MainAccountCode: MainAccountID }),
            success: function (msg) {
                var data = msg.d;
                MainAccountType = msg.d;
            }
        });
        //Add for found Main Accoun type Tanmoy
        if (MainAccountType != "Lead" || $("#hdnIsLeadAvailableinTransactions").val() == "Yes") {
            if (e.buttonIndex == 0) {
                if (strMainAccountID.trim() != "") {
                    document.getElementById('hdnMainAccountId').value = MainAccountID;
                    var FullName = new Array("", "");
                    cSubAcountComboBox.AddItem(FullName, "");
                    cSubAcountComboBox.SetValue("");
                    $('#SubAccountModel').modal('show');

                }
            }
        }
    }
}
function SubAccountNewkeydown(e) {
    InsgridBatch.batchEditApi.StartEdit(e.visibleIndex);
    var strMainAccountID = (InsgridBatch.GetEditor('MainAccount').GetText() != null) ? InsgridBatch.GetEditor('MainAccount').GetText() : "0";
    var MainAccountID = (InsgridBatch.GetEditor('gvColMainAccount').GetValue() != null) ? InsgridBatch.GetEditor('gvColMainAccount').GetValue() : "0";

    var OtherDetails = {}
    OtherDetails.SearchKey = $("#txtSubAccountSearch").val();
    OtherDetails.MainAccountCode = MainAccountID;
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        if ($("#txtSubAccountSearch").val() == "")
            return;
        var HeaderCaption = [];
        HeaderCaption.push("Sub Account Name [Unique Id]");
        HeaderCaption.push("Sub Account Type");

        callonServer("/OMS/Management/Activities/Services/Master.asmx/GetSubAccountJournal", OtherDetails, "SubAccountTable", HeaderCaption, "SubAccountIndex", "SetSubAccount");
    }
    else if (e.code == "ArrowDown") {
        if ($("input[SubAccountIndex=0]"))
            $("input[SubAccountIndex=0]").focus();
    }
    else if (e.code == "Escape") {
        $('#SubAccountModel').modal('hide');
        InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 3);
    }
}
function SetSubAccount(Id, name) {
    $('#SubAccountModel').modal('hide');
    GetSubAcountComboBox(Id, name);
    InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 4);
}



// Model Function ///





var globalRowIndex;
var shouldCheck = 0;
function MainAccountClose(s, e) {
    cMainAccountpopUp.Hide();
    InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 2);

}
function SubAccountClose(s, e) {
    cSubAccountpopUp.Hide();
    InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 3);

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

        InsgridBatch.batchEditApi.StartEdit(globalRowIndex);


        var VoucherType = document.getElementById('rbtnType').value;
        if (VoucherType == "P") {
            setTimeout(function () { InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 5); }, 500);

        }
        else {
            setTimeout(function () { InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 4); }, 500);
        }
        InsgridBatch.GetEditor("bthSubAccount").SetText(subAccountText);
        InsgridBatch.GetEditor("gvColSubAccount").SetText(subAccountID);



    }
}

$(document).ready(function () {



    $('#MainAccountModel').on('shown.bs.modal', function () {
        $('#txtMainAccountSearch').val("");
        $('#txtMainAccountSearch').focus();
    })
    $('#MainAccountModelRO').on('shown.bs.modal', function () {
        $('#txtMainAccountSearchRO').val("");
        $('#txtMainAccountSearchRO').focus();
    })
    $('#SubAccountModel').on('shown.bs.modal', function () {
        $('#txtSubAccountSearch').val("");
        $('#txtSubAccountSearch').focus();
    })
    $('#SubAccountModel').on('hide.bs.modal', function () {

        //grid.batchEditApi.StartEdit(globalRowIndex, 2);
    })
});

function MainAccountComboBoxKeyDown(s, e) {
    if (e.htmlEvent.key == "Escape") {
        cMainAccountpopUp.Hide();
        InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 3);
    }
    //if (e.htmlEvent.key == "Enter") {
    //    var MainAccountText = cMainAccountComboBox.GetText();                
    //    if (MainAccountText != "") {
    //        if (!cMainAccountComboBox.FindItemByText(MainAccountText)) {
    //            jAlert("Main Account does not Exist.");
    //            cMainAccountComboBox.SetText("");
    //            shouldCheck = 0;
    //            return;
    //        }
    //    }               

    //}
}
function SubAccountComboBoxKeyDown(s, e) {
    if (e.htmlEvent.key == "Escape") {
        cSubAccountpopUp.Hide();
        InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 4);
    }
    if (e.htmlEvent.key == "Enter") {
        GetSubAcountComboBox(e);
    }
}
function GetMainAcountComboBox(Id, name, IsSub, RevApp, TaxAble) {
    //debugger;
    var MainAccountText = name;

    IsSubAccount = IsSub;
    cMainAccountpopUp.Hide();
    var MainAccountID = Id;//cMainAccountComboBox.GetValue();
    var ReverseApplicable = RevApp; //cMainAccountComboBox.GetSelectedItem().texts[2];
    var TaxApplicable = TaxAble;// cMainAccountComboBox.GetSelectedItem().texts[3];
    // InsgridBatch.batchEditApi.StartEdit(globalRowIndex);
    ReciptOldValue = InsgridBatch.GetEditor("btnRecieve").GetValue();
    PaymentOldValue = InsgridBatch.GetEditor("btnPayment").GetValue();
    ReciptNewValue = 0;

    if (ReciptOldValue != ReciptNewValue) {
        changeReciptTotalSummary();
    }


    // Rev Sayantani
    PaymentNewValue = 0;

    if (PaymentOldValue != PaymentNewValue) {
        changePaymentTotalSummary();
    }
    // End of Rev Sayantani

    InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 3);
    InsgridBatch.GetEditor("MainAccount").SetText(MainAccountText);
    InsgridBatch.GetEditor("gvColMainAccount").SetText(MainAccountID);
    InsgridBatch.GetEditor("ReverseApplicable").SetValue(ReverseApplicable);
    shouldCheck = 0;
    InsgridBatch.GetEditor("bthSubAccount").SetValue("");
    InsgridBatch.GetEditor("btnRecieve").SetValue("");
    InsgridBatch.GetEditor("btnPayment").SetValue("");
    // debugger;
    if ($("#hdnProjectSelectInEntryModule").val() == "1") {
        //clookup_Project.SetEnabled(false);
        if ((clookup_Project.GetGridView().GetRowKey(clookup_Project.GetGridView().GetFocusedRowIndex()) != null)) {

            var ProjectLookUpData = clookup_Project.GetGridView().GetRowKey(clookup_Project.GetGridView().GetFocusedRowIndex());
            InsgridBatch.GetEditor("Project_Code").SetText(clookup_Project.GetValue());
            InsgridBatch.GetEditor("ProjectId").SetText(ProjectLookUpData);
        }
    }


    var prevAmount = InsgridBatch.GetEditor("NetAmount").GetText();
    var totamoutcng = ctxtTotalPayment.GetText();

    var diffcng = DecimalRoundoff(totamoutcng, 2) - DecimalRoundoff(prevAmount, 2);


    //ctxtTotalPayment.SetText(diffcng.toFixed(2));

    InsgridBatch.GetEditor("TaxAmount").SetValue("0.00");
    InsgridBatch.GetEditor("NetAmount").SetValue("0.00");
    InsgridBatch.GetEditor("gvColSubAccount").SetValue("");
    InsgridBatch.GetEditor("IsSubledger").SetValue(IsSubAccount);
    cddl_AmountAre.SetEnabled(false);
    $("#rbtnType").attr("disabled", "disabled");
    $("#IsTaxApplicable").val(TaxApplicable);
    var VoucherType = document.getElementById('rbtnType').value;
    if (ReverseApplicable == "1" && VoucherType == "P") {
        $("#chk_reversemechenism").prop("disabled", false);
        $("#chk_reversemechenism").prop("checked", true);
    }
    else {
        if ($("#chk_reversemechenism").prop('checked') == false) {
            $("#chk_reversemechenism").prop("checked", false);
        }
    }

    //ReceiptLostFocus();
    //Payment_Lost_Focus();


}
function AmtTextChange(s, e) {
    InsgridBatch.batchEditApi.StartEdit(globalRowIndex);
    var Amount = (InsgridBatch.GetEditor('btnPayment').GetValue() != null) ? InsgridBatch.GetEditor('btnPayment').GetValue() : "0";
    var TaxAmount = (InsgridBatch.GetEditor('TaxAmount').GetValue() != null) ? InsgridBatch.GetEditor('TaxAmount').GetValue() : "0";
    //chinmoy commeented for lightweight billingshiping
    //var ShippingStateCode = $("#bsSCmbStateHF").val();
    var ShippingStateCode = $("#hdStateIdShipping").val();
    var TaxType = "";
    if (cddl_AmountAre.GetValue() == "1") {
        TaxType = "E";
    }
    else if (cddl_AmountAre.GetValue() == "2") {
        TaxType = "I";
    }
    //if (GetObjectID('hdnCustomerStateCodeId').value != null && GetObjectID('hdnCustomerStateCodeId').value != '' && GetObjectID('hdnCustomerStateCodeId').value != '0') {
    caluculateAndSetGST(InsgridBatch.GetEditor("btnPayment"), InsgridBatch.GetEditor("TaxAmount"), InsgridBatch.GetEditor("NetAmount"), $("#IsTaxApplicable").val(), Amount, Amount, TaxType, ShippingStateCode, $('#ddlBranch').val());

}
function GetSubAcountComboBox(Id, name) {
    var SubAcountText = cSubAcountComboBox.GetText();
    //if (cSubAcountComboBox.GetText() != "") {
    //if (!cSubAcountComboBox.FindItemByValue(cSubAcountComboBox.GetValue())) {
    //if (!cSubAcountComboBox.FindItemByText(SubAcountText)) {
    //    //jAlert("Sub Account does not Exist.", "Alert", function () { cSubAcountComboBox.SetValue(); cSubAcountComboBox.Focus(); });
    //    $('#subActMsg').show();
    //    return;
    //}
    //else {
    //    if (e.keyCode == 27)//escape 
    //    {
    //        InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 3);
    //        return;
    //    }
    var subAccountText = name;//cSubAcountComboBox.GetText();
    var subAccountID = Id;//cSubAcountComboBox.GetValue();
    InsgridBatch.batchEditApi.StartEdit(globalRowIndex);


    var VoucherType = document.getElementById('rbtnType').value;
    if (VoucherType == "P") {
        setTimeout(function () { InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 5); }, 500);

    }
    else {
        setTimeout(function () { InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 4); }, 500);
    }
    InsgridBatch.GetEditor("bthSubAccount").SetText(subAccountText);
    InsgridBatch.GetEditor("gvColSubAccount").SetText(subAccountID);
    cSubAccountpopUp.Hide();

    //}

    // }
}
function CloseSubModal() {
    $('#SubAccountModel').modal('hide');
    InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 2);

}

var chkAccount = 0;
var ReciptOldValue;
var ReciptNewValue;
var PaymentOldValue;
var PaymentNewValue;

var GlobargotRecpt = null;
var GlobargotPayMent = null;
var oldBranchdata;
var globalNetAmount = 0;
var isCtrl = false;
var SrlNo = 0;

var NetAmountOldValue;
var NetAmountNewValue;

//------------------------------------------------Tax-------------------------------------
var taxJson;
var ChargegstcstvatGlobalName;
var taxAmountGlobal;
var globalTaxRowIndex;
var gstcstvatGlobalName;
var GlobalCurTaxAmt = 0;


function GlobalBillingShippingEndCallBack() {
    var NoSchemeTypedtl = cCmbScheme.GetValue();
    if (NoSchemeTypedtl != null && NoSchemeTypedtl != '') {
        var branchID = (NoSchemeTypedtl.toString().split('~')[3] != null) ? NoSchemeTypedtl.toString().split('~')[3] : "";
    }
}  /// this emplty function required for billing/Shipping

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
    totalInlineTaxAmount = totalInlineTaxAmount + parseFloat(ctxtGstCstVat.GetValue());
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
    SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''));
    //Set Running Total
    SetRunningTotal();
    RecalCulateTaxTotalAmountInline();
}
function txtPercentageLostFocus(s, e) {

    var ProdAmt = parseFloat(cgridTax.GetEditor("calCulatedOn").GetValue());
    if (s.GetText().trim() != '') {
        if (!isNaN(parseFloat(ProdAmt * s.GetText()) / 100)) {
            //Checking Add or less
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
            SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''));

            //Call for Running Total
            SetRunningTotal();
        } else {
            s.SetText("");
        }
    }
    RecalCulateTaxTotalAmountInline();
}
function CmbtaxClick(s, e) {
    GlobalCurTaxAmt = parseFloat(ctxtGstCstVat.GetText());
    gstcstvatGlobalName = s.GetText();
}
function GetTaxVisibleIndex(s, e) {
    globalTaxRowIndex = e.visibleIndex;
}
function taxAmountGotFocus(s, e) {
    taxAmountGlobal = parseFloat(s.GetValue());
}
function chargeCmbtaxClick(s, e) {
    GlobalCurChargeTaxAmt = parseFloat(ctxtGstCstVatCharge.GetText());
    ChargegstcstvatGlobalName = s.GetText();
}
function ShowTaxPopUp(type) {
    if (type == "IY") {
        $('#ContentErrorMsg').hide();
        $('#content-6').show();
        document.getElementById('calculateTotalAmountOK').style.display = 'block';
        if (ccmbGstCstVat.GetItemCount() <= 1) {
            $('.InlineTaxClass').hide();
        } else {
            $('.InlineTaxClass').show();
        }
        if (cgridTax.GetVisibleRowsOnPage() < 1) {
            $('.cgridTaxClass').hide();

        } else {
            $('.cgridTaxClass').show();
        }

        if (ccmbGstCstVat.GetItemCount() <= 1 && cgridTax.GetVisibleRowsOnPage() < 1) {
            $('#ContentErrorMsg').show();
            $('#content-6').hide();
            document.getElementById('calculateTotalAmountOK').style.display = 'none';

        }
    }
    if (type == "IN") {
        $('#ErrorMsgCharges').hide();
        $('#content-5').show();

        if (ccmbGstCstVatcharge.GetItemCount() <= 1) {
            $('.chargesDDownTaxClass').hide();
        } else {
            $('.chargesDDownTaxClass').show();
        }
        if (gridTax.GetVisibleRowsOnPage() < 1) {
            $('.gridTaxClass').hide();

        } else {
            $('.gridTaxClass').show();
        }

        if (ccmbGstCstVatcharge.GetItemCount() <= 1 && gridTax.GetVisibleRowsOnPage() < 1) {
            $('#ErrorMsgCharges').show();
            $('#content-5').hide();
        }
    }
}
function cgridTax_EndCallBack(s, e) {
    $("#TaxAmountOngrid").val("");
    $("#VisibleIndexForTax").val("");


    $('.cgridTaxClass').show();
    cgridTax.StartEditRow(0);
    //check Json data
    if (cgridTax.cpJsonData) {
        if (cgridTax.cpJsonData != "") {
            taxJson = JSON.parse(cgridTax.cpJsonData);
            cgridTax.cpJsonData = null;
        }
    }
    //End Here
    if (cgridTax.cpComboCode) {
        if (cgridTax.cpComboCode != "") {
            if (cddl_AmountAre.GetValue() == "1") {
                var selectedIndex;
                for (var i = 0; i < ccmbGstCstVat.GetItemCount() ; i++) {
                    if (ccmbGstCstVat.GetItem(i).value.split('~')[0] == cgridTax.cpComboCode) {
                        selectedIndex = i;
                    }
                }
                if (selectedIndex && ccmbGstCstVat.GetItem(selectedIndex) != null) {
                    ccmbGstCstVat.SetValue(ccmbGstCstVat.GetItem(selectedIndex).value);
                }
                cmbGstCstVatChange(ccmbGstCstVat);
                cgridTax.cpComboCode = null;
            }
        }
    }

    if (cgridTax.cpUpdated.split('~')[0] == 'ok') {
        ctxtTaxTotAmt.SetValue(cgridTax.cpUpdated.split('~')[1]);
        var gridValue = parseFloat(cgridTax.cpUpdated.split('~')[1]);
        var ddValue = parseFloat(ctxtGstCstVat.GetValue());
        ctxtTaxTotAmt.SetValue(gridValue + ddValue);
        cgridTax.cpUpdated = "";
        RecalCulateTaxTotalAmountInline();
    }
    else {
        var totAmt = ctxtTaxTotAmt.GetValue();
        caspxTaxpopUp.Hide();
        cgridTax.CancelEdit();
    }
    if (cgridTax.GetVisibleRowsOnPage() == 0) {
        $('.cgridTaxClass').hide();
        ccmbGstCstVat.Focus();
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
            SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''));
        }
        runningTot = runningTot + parseFloat(cgridTax.GetEditor("Amount").GetValue());
        cgridTax.batchEditApi.EndEdit();
    }
}
function recalculateTax() {
    cmbGstCstVatChange(ccmbGstCstVat);
}
function recalculateTaxCharge() {
    ChargecmbGstCstVatChange(ccmbGstCstVatcharge);
}
function chargeCmbtaxClick(s, e) {
    GlobalCurChargeTaxAmt = parseFloat(ctxtGstCstVatCharge.GetText());
    ChargegstcstvatGlobalName = s.GetText();
}
function showTax() {
    //var strMainAccountID = (InsgridBatch.GetEditor('MainAccount').GetText() != null) ? InsgridBatch.GetEditor('MainAccount').GetText() : "0";
    //var MainAccountID = (InsgridBatch.GetEditor('MainAccount').GetValue() != null) ? InsgridBatch.GetEditor('MainAccount').GetValue() : "0";
    var strMainAccountID = (InsgridBatch.GetEditor('MainAccount').GetText() != null) ? InsgridBatch.GetEditor('MainAccount').GetText() : "0";
    var MainAccountID = (InsgridBatch.GetEditor('gvColMainAccount').GetValue() != null) ? InsgridBatch.GetEditor('gvColMainAccount').GetValue() : "0";
    var StrAmount = "0";

    var VoucherType = document.getElementById('rbtnType').value;

    //if (cComboType.GetValue() == "R") {
    if (VoucherType == "R") {
        var ReceiptValue = (InsgridBatch.GetEditor('btnRecieve').GetValue() != null) ? parseFloat(InsgridBatch.GetEditor('btnRecieve').GetValue()) : "0";
        var NetAmount = (InsgridBatch.GetEditor('NetAmount').GetValue() != null) ? parseFloat(InsgridBatch.GetEditor('NetAmount').GetValue()) : "0";
        var TaxAmount = (InsgridBatch.GetEditor('TaxAmount').GetValue() != null) ? parseFloat(InsgridBatch.GetEditor('TaxAmount').GetValue()) : "0";
        //StrAmount = ReceiptValue;
        if (cddl_AmountAre.GetValue() == "2") {
            StrAmount = NetAmount;
        }
        else if (cddl_AmountAre.GetValue() == "1") {
            StrAmount = parseFloat(NetAmount) - parseFloat(TaxAmount);
        }
    }
    else {
        var PaymentValue = (InsgridBatch.GetEditor('btnPayment').GetValue() != null) ? InsgridBatch.GetEditor('btnPayment').GetValue() : "0";
        // StrAmount = PaymentValue;
        var NetAmount = (InsgridBatch.GetEditor('NetAmount').GetValue() != null) ? parseFloat(InsgridBatch.GetEditor('NetAmount').GetValue()) : "0";
        var TaxAmount = (InsgridBatch.GetEditor('TaxAmount').GetValue() != null) ? parseFloat(InsgridBatch.GetEditor('TaxAmount').GetValue()) : "0";
        if (cddl_AmountAre.GetValue() == "2") {
            StrAmount = NetAmount;
        }
        else if (cddl_AmountAre.GetValue() == "1") {
            StrAmount = parseFloat(NetAmount) - parseFloat(TaxAmount);
        }
    }
    if (strMainAccountID.trim() != "") {
        globalNetAmount = parseFloat(StrAmount);
        document.getElementById('setCurrentProdCode').value = MainAccountID;
        document.getElementById('HdSerialNo').value = SrlNo + 1;
        var strSrlNo = SrlNo + 1;
        SrlNo = strSrlNo;
        ctxtTaxTotAmt.SetValue(0);
        ccmbGstCstVat.SetSelectedIndex(0);
        $('.RecalculateInline').hide();
        caspxTaxpopUp.Show();
        //  var Amount = Math.round(QuantityValue * strFactor * (strSalePrice / strRate)).toFixed(2);
        var Amount = (Math.round(StrAmount * 100) / 100).toFixed(2);
        clblTaxProdGrossAmt.SetText(Amount);
        clblProdNetAmt.SetText(Amount);
        document.getElementById('HdProdGrossAmt').value = Amount;
        document.getElementById('HdProdNetAmt').value = Amount;
        clblTaxDiscount.SetText('0.00');
        //Checking is gstcstvat will be hidden or not
        if (cddl_AmountAre.GetValue() == "2") {
            $('.GstCstvatClass').hide();
            $('.gstGrossAmount').show();
            clblTaxableGross.SetText("(Taxable)");
            clblTaxableNet.SetText("(Taxable)");
            $('.gstNetAmount').show();

            $('.gstGrossAmount').hide();
            $('.gstNetAmount').hide();
            clblTaxableGross.SetText("");
            clblTaxableNet.SetText("");
            //}
        }
        else if (cddl_AmountAre.GetValue() == "1") {
            $('.GstCstvatClass').show();
            $('.gstGrossAmount').hide();
            $('.gstNetAmount').hide();
            clblTaxableGross.SetText("");
            clblTaxableNet.SetText("");
            //Get Customer Shipping StateCode
            var shippingStCode = '';
            //chinmoy commeented for lightweight billingshiping
            //shippingStCode = cbsSCmbState.GetText();
            shippingStCode = GeteShippingStateCode();
            //shippingStCode = shippingStCode.substring(shippingStCode.lastIndexOf('(')).replace('(State Code:', '').replace(')', '').trim();
            shippingStCode = shippingStCode;
            //Debjyoti 09032017
            if (shippingStCode.trim() != '') {
                for (var cmbCount = 1; cmbCount < ccmbGstCstVat.GetItemCount() ; cmbCount++) {
                    //Check if gstin is blank then delete all tax
                    if (ccmbGstCstVat.GetItem(cmbCount).value.split('~')[5] != "") {
                        if (ccmbGstCstVat.GetItem(cmbCount).value.split('~')[5] == shippingStCode) {
                            //if its state is union territories then only UTGST will apply
                            if (shippingStCode == "4" || shippingStCode == "35" || shippingStCode == "26" || shippingStCode == "25" || shippingStCode == "7" || shippingStCode == "31" || shippingStCode == "34") {
                                if (ccmbGstCstVat.GetItem(cmbCount).value.split('~')[4] == 'I' || ccmbGstCstVat.GetItem(cmbCount).value.split('~')[4] == 'C' || ccmbGstCstVat.GetItem(cmbCount).value.split('~')[4] == 'S') {
                                    ccmbGstCstVat.RemoveItem(cmbCount);
                                    cmbCount--;
                                }
                            }
                            else {
                                if (ccmbGstCstVat.GetItem(cmbCount).value.split('~')[4] == 'I' || ccmbGstCstVat.GetItem(cmbCount).value.split('~')[4] == 'U') {
                                    ccmbGstCstVat.RemoveItem(cmbCount);
                                    cmbCount--;
                                }
                            }
                        } else {
                            if (ccmbGstCstVat.GetItem(cmbCount).value.split('~')[4] == 'S' || ccmbGstCstVat.GetItem(cmbCount).value.split('~')[4] == 'C' || ccmbGstCstVat.GetItem(cmbCount).value.split('~')[4] == 'U') {
                                ccmbGstCstVat.RemoveItem(cmbCount);
                                cmbCount--;
                            }
                        }
                    } else {
                        //remove tax because GSTIN is not define
                        ccmbGstCstVat.RemoveItem(cmbCount);
                        cmbCount--;
                    }
                }
            }

        }
        //End here

        if (globalRowIndex > -1) {
            cgridTax.PerformCallback('New~' + cddl_AmountAre.GetValue());
        }
        else {
            cgridTax.PerformCallback('New~' + cddl_AmountAre.GetValue());
            //Set default combo
            // cgridTax.cpComboCode = grid.GetEditor('ProductID').GetValue().split('||@||')[9];
        }
        ctxtprodBasicAmt.SetValue(Amount);
    } else {

    }
}
function calculateTotalAmount() {

    var TaxAmount = ctxtTaxTotAmt.GetValue();
    var Receipt = InsgridBatch.GetEditor("btnRecieve").GetValue();
    var payment = InsgridBatch.GetEditor("btnPayment").GetValue();

    if (cddl_AmountAre.GetValue() == "1") { /// Exclusive Tax Calculation
        if (Receipt != "0.0" && Receipt != "0.00") {
            var TotalReceipt = parseFloat(TaxAmount) + parseFloat(Receipt);
            InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 7);
            var cashBankGridTaxAmount = InsgridBatch.GetEditor("TaxAmount");
            cashBankGridTaxAmount.SetValue(TaxAmount);
            var cashBankGrid = InsgridBatch.GetEditor("NetAmount");
            cashBankGrid.SetValue(TotalReceipt.toFixed(2));
            var CuurentNetAmount = InsgridBatch.GetEditor("NetAmount").GetValue();
            c_txtTotalNetAmount.SetValue(CuurentNetAmount + TotalReceipt.toFixed(2));
        }
        if (payment != "0.0" && payment != "0.00") {
            var TotalPayment = parseFloat(TaxAmount) + parseFloat(payment);
            InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 7);
            var cashBankGridTaxAmount = InsgridBatch.GetEditor("TaxAmount");
            cashBankGridTaxAmount.SetValue(TaxAmount);
            var cashBankGridNetAmount = InsgridBatch.GetEditor("NetAmount");
            cashBankGridNetAmount.SetValue(TotalPayment.toFixed(2));
            var CuurentNetAmount = InsgridBatch.GetEditor("NetAmount").GetValue();
            c_txtTotalNetAmount.SetValue(CuurentNetAmount + TotalPayment.toFixed(2));
        }
    }
    else if (cddl_AmountAre.GetValue() == "2") {  /// Inclusive Calculation
        InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 7);
        var cashBankGridTaxAmount = InsgridBatch.GetEditor("TaxAmount");
        cashBankGridTaxAmount.SetValue(TaxAmount);
    }
    document.getElementById('HdSerialNo').val = InsgridBatch;
    if (cgridTax.GetVisibleRowsOnPage() > 0) {
        document.getElementById('HdSerialNo1').value = InsgridBatch.GetEditor('SrlNo').GetText();
        cgridTax.UpdateEdit();
    }
    else {
        cgridTax.PerformCallback('SaveGst');
    }
    LoadingPanel.Hide();
    return false;
}
//....................................................End Tax........................................

function CashBank_GotFocus() {
    cddlCashBank.ShowDropDown();
}
function CashBank_SelectedIndexChanged() {
    //var VoucherType = cComboType.GetValue();
    var VoucherType = document.getElementById('rbtnType').value;
    // LoadCustomerAddress('', $('#ddlBranch').val(), 'PO');
    var CashBankId = cddlCashBank.GetValue();

    var CashBankText = cddlCashBank.GetText();
    var arr = CashBankText.split('|');
    var strbranch = $('#hdnBranchId').val();
    PopulateCurrentBankBalance(arr[0], strbranch);
    $('#MandatoryCashBank').hide();
    //var CashBankText = cddlCashBank.GetText();
   // var SpliteDetails = CashBankText.split(']');
    var WithDrawType = "";
    var CashBankText = cddlCashBank.GetValue();
    if (CashBankText != "" && CashBankText != null)
    {
        var SpliteDetails = CashBankText.split('~');
        WithDrawType = SpliteDetails[1];
    }
  
    //var WithDrawType = SpliteDetails[1].trim();
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
        var comboitem = cComboInstrumentTypee.FindItemByValue('CH');
        if (comboitem != undefined && comboitem != null) {
            cComboInstrumentTypee.RemoveItem(comboitem.index);
            cComboInstrumentTypee.SetValue("C");
            InstrumentTypeSelectedIndexChanged();
        }
    }
    if ($("#hdnCheckSaveNew").val() == "1") {
        ClearBillingShipping();
        if ($("#ddlBranch").val() != null && $("#ddlBranch").val() != "") {
            SetPurchaseBillingShippingAddress($('#ddlBranch').val());
        }
    }


}
function CashBank_EndCallback() {

    var CashBankId = $('#hdnCashBankId').val();
    cddlCashBank.SetValue(CashBankId);
    var CashBankText = cddlCashBank.GetText();
    var arr = CashBankText.split('|');
    var strbranch = $('#hdnBranchId').val();
    PopulateCurrentBankBalance(arr[0], strbranch);
}
function VoucherType_GotFocus() {
    cComboType.ShowDropDown();
}
function NumberingScheme_GotFocus() {
    cCmbScheme.ShowDropDown();
}
function ReloadPage() {
    //sessionStorage.removeItem('CashBankDetails');
    $('#hdnEditRfid').val('');
    window.location.assign("CashBankEntryList.aspx");
    // cacpCrossBtn.PerformCallback();
}
function acpCrossBtnEndCall() {
    window.location.reload();
}
var isFirstTime = true;
function AllControlInitilize() {
    // document.getElementById('AddButton').style.display = 'inline-block';
    if (isFirstTime) {
        $("#TaxAmountOngrid").val("");
        $("#VisibleIndexForTax").val("");
        if ($('#hdn_Mode').val() != "Edit") {
            //document.getElementById('rbtnType').value = 'P';
            AddButtonClick();
        }

        //OnAddNewClick();
        //if (localStorage.getItem('FromDateCashBank')) {
        //    var fromdatearray = localStorage.getItem('FromDateCashBank').split('-');
        //    var fromdate = new Date(fromdatearray[0], parseFloat(fromdatearray[1]) - 1, fromdatearray[2], 0, 0, 0, 0);
        //    cFormDate.SetDate(fromdate);
        //}

        //if (localStorage.getItem('ToDateCashBank')) {
        //    var todatearray = localStorage.getItem('ToDateCashBank').split('-');
        //    var todate = new Date(todatearray[0], parseFloat(todatearray[1]) - 1, todatearray[2], 0, 0, 0, 0);
        //    ctoDate.SetDate(todate);
        //}
        //if (localStorage.getItem('BranchCashBank')) {
        //    if (ccmbBranchfilter.FindItemByValue(localStorage.getItem('BranchCashBank'))) {
        //        ccmbBranchfilter.SetValue(localStorage.getItem('BranchCashBank'));
        //    }

        //}


        isFirstTime = false;
    }
}

function EnableOrDisableTax() {
    if (cddl_AmountAre.GetValue() == '3') {
        var TaxAmt = InsgridBatch.GetEditor('TaxAmount');
        TaxAmt.SetEnabled(false);
    }
    else {
        var TaxAmt = InsgridBatch.GetEditor('TaxAmount');
        TaxAmt.SetEnabled(true);
    }
}
function GetVisibleIndex(s, e) {
    globalRowIndex = e.visibleIndex;
    EnableOrDisableTax();
}
function ddlBranch_SelectedIndexChanged() {
    if ($("#hdnProjectSelectInEntryModule").val() == "1")
        clookup_Project.gridView.Refresh();
    var branch = $("#ddlBranch").val();
    InsgridBatch.batchEditApi.StartEdit(-1, 1);
    var accountingDataMin = InsgridBatch.GetEditor('MainAccount').GetValue();
    InsgridBatch.batchEditApi.EndEdit();

    InsgridBatch.batchEditApi.StartEdit(0, 1);
    var accountingDataplus = InsgridBatch.GetEditor('MainAccount').GetValue();
    InsgridBatch.batchEditApi.EndEdit();

    if (accountingDataMin != null || accountingDataplus != null) {
        jConfirm('You have changed Branch. All the entries of ledger in this voucher to be reset to blank. \n You have to select and re-enter. Continue?', 'Confirmation Dialog', function (r) {

            if (r == true) {
                deleteAllRows();
                InsgridBatch.AddNewRow();
                InsgridBatch.GetEditor('SrlNo').SetValue('1');
                MainAccount.PerformCallback(branch)
            }
        });
    }
    else {
        MainAccount.PerformCallback(branch)
    }

}
//function chkValidConta(contano_status) {
//    if (contano_status == "outrange") {
//        jAlert('Can Not Add More Cash/Bank Voucher as Contra Scheme Exausted.<br />Update The Scheme and Try Again');
//    } else if (contano_status == "duplicate") {
//        jAlert('Can Not Save as Duplicate Contra Voucher No. Found');
//    }
//    return false;
//}
function OnKeyDown(s, e) {
    if (e.htmlEvent.keyCode == 40 || e.htmlEvent.keyCode == 38)
        return ASPxClientUtils.PreventEvent(e.htmlEvent);
}
function cddl_AmountAre_LostFocus() {
    //if (InsgridBatch.GetVisibleRowsOnPage() == 1) {
    //    InsgridBatch.batchEditApi.StartEdit(-1, 2);
    //}
    if ($("#hdnProjectSelectInEntryModule").val() == "1") {
        clookup_Project.SetFocus();
    }
    else if ($("#hdnProjectSelectInEntryModule").val() == "0") {
        if (InsgridBatch.GetVisibleRowsOnPage() == 1) {
            //alert("5");
            //InsgridBatch.batchEditApi.StartEdit(-1, 2);
        }
        if ($("#hdnhiarchy").val() == "1") {
            $("ddlHierarchy").val();
        }
        else {
            //clookup_CashFund.focus();
        }
    }
    // clookup_CashFund.SetFocus();
}
function Project_gotFocus() {
    if ($("#hdnProjectSelectInEntryModule").val() == "1")
        clookup_Project.gridView.Refresh();
    clookup_Project.SetFocus()
    clookup_Project.ShowDropDown();
}


function Project_LostFocus() {


    if (clookup_Project.GetText() == "" && InsgridBatch.GetVisibleRowsOnPage() == 0) {
        InsgridBatch.PerformCallback("RefcashbankClear");
        clookup_CashFund.GetGridView().SetFocusedRowIndex(-1);
        clookup_CashFund.gridView.Refresh();
    }

    //if (InsgridBatch.GetVisibleRowsOnPage() == 1) {
    //    InsgridBatch.batchEditApi.StartEdit(-1, 2);
    //}
    //Hierarchy Start Tanmoy
    var projID = clookup_Project.GetValue();
    if (projID == null || projID == "") {
        $("#ddlHierarchy").val(0);
    }
    //Hierarchy End Tanmoy
    if ($("#hdnhiarchy").val() == "1") {
        $("ddlHierarchy").focus();
    }
    else if ($("#hdnhiarchy").val() != "0") {
        clookup_CashFund.focus();
    }
}

//Hierarchy Start Tanmoy
function ProjectValueChange(s, e) {
    //debugger;

    var projID = clookup_Project.GetValue();
    //clookup_CashFund.Clear();
    //InsgridBatch.PerformCallback("RefcashbankClear");
    clookup_CashFund.GetGridView().SetFocusedRowIndex(-1);
    clookup_CashFund.gridView.Refresh();

    $.ajax({
        type: "POST",
        url: 'CashBankEntry.aspx/getHierarchyID',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ ProjID: projID }),
        success: function (msg) {
            var data = msg.d;
            $("#ddlHierarchy").val(data);
        }
    });





}
//Hierarchy End Tanmoy

function CreditGotFocus(s, e) {
    PaymentOldValue = s.GetText();
    NetAmountOldValue = InsgridBatch.GetEditor("NetAmount").GetValue();
    var indx = PaymentOldValue.indexOf(',');
    if (indx != -1) {
        PaymentOldValue = PaymentOldValue.replace(/,/g, '');
    }
}
function Payment_Lost_Focus(s, e) {
    PaymentNewValue = s.GetText();
    var indx = PaymentNewValue.indexOf(',');
    if (indx != -1) {
        PaymentNewValue = PaymentNewValue.replace(/,/g, '');
    }


    if (PaymentOldValue != PaymentNewValue) {
        changePaymentTotalSummary();
    }
}
function recalculateReceipt(oldVal) {
    // debugger;
    if (oldVal != 0) {
        ReciptNewValue = 0;
        ReciptOldValue = oldVal;
        changeReciptTotalSummary();
    }
}









function PaymentTextChange(s, e) {
    Payment_Lost_Focus(s, e);
    var PaymentValue = (InsgridBatch.GetEditor('btnPayment').GetValue() != null) ? InsgridBatch.GetEditor('btnPayment').GetValue() : "0";
    var ReceiptValue = (InsgridBatch.GetEditor('btnRecieve').GetValue() != null) ? parseFloat(InsgridBatch.GetEditor('btnRecieve').GetValue()) : "0";

    if (PaymentValue > 0) {
        recalculateReceipt(InsgridBatch.GetEditor('btnRecieve').GetValue());
        InsgridBatch.GetEditor('btnRecieve').SetValue("0");
        if (PaymentValue != PaymentOldValue) {
            InsgridBatch.GetEditor('TaxAmount').SetValue("0");
        }
        InsgridBatch.GetEditor('NetAmount').SetValue(PaymentValue);
    }
    $("#HdProdGrossAmt").val(PaymentValue);

    var MainAccountID = (InsgridBatch.GetEditor('gvColMainAccount').GetValue() != null) ? InsgridBatch.GetEditor('gvColMainAccount').GetValue() : "0";


    var VoucherType = document.getElementById('rbtnType').value;

    if (VoucherType == "P") {
        $.ajax({
            type: "POST",
            url: "CashBankEntry.aspx/GetTotalBalanceByToDay",
            data: JSON.stringify({ MainAccountID: MainAccountID }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (msg) {
                var data = msg.d;
                var VoucherAmount = data.toString().split('~')[0];
                var BalanceLimit = data.toString().split('~')[1];
                var BalanceExceed = data.toString().split('~')[2];
                if (BalanceLimit != '0.00') {
                    var TotalVoucherAmount = parseFloat(PaymentValue) + parseFloat(VoucherAmount);
                    console.log(TotalVoucherAmount);

                    if (parseFloat(TotalVoucherAmount) > parseFloat(BalanceLimit)) {

                        if (BalanceExceed.trim() == 'W') {

                            jConfirm('Daily Balance - Limit is exceed do you want to proceed?', 'Confirmation Dialog', function (r) {
                                if (r == true) {

                                }
                                else {
                                    InsgridBatch.GetEditor('btnPayment').SetValue("0");
                                    InsgridBatch.GetEditor('NetAmount').SetValue("0");
                                }
                            });
                        }
                        else if (BalanceExceed.trim() == 'B') {
                            jAlert('Daily Balance - Limit is exceed can not proceed');
                            InsgridBatch.GetEditor('btnPayment').SetValue("0");
                            InsgridBatch.GetEditor('NetAmount').SetValue("0");

                        }
                        else if (BalanceExceed.trim() == 'S') {
                            jAlert('Please select Daily Balance - Limit exceed option.');
                            InsgridBatch.GetEditor('btnPayment').SetValue("0");
                            InsgridBatch.GetEditor('NetAmount').SetValue("0");

                        }
                        else if (BalanceExceed.trim() == '') {
                            jAlert('Please select Daily Balance - Limit exceed option.');
                            InsgridBatch.GetEditor('btnPayment').SetValue("0");
                            InsgridBatch.GetEditor('NetAmount').SetValue("0");

                        }
                        else if (BalanceExceed.trim() == 'I') {

                        }
                    }
                    else {
                        //OnAddNewClick();
                        //cbtnSaveNew.SetVisible(false);
                        //cbtnSaveRecords.SetVisible(false);

                        //InsgridBatch.UpdateEdit();
                        //chkAccount = 0;
                    }
                }
                else {
                    //OnAddNewClick();
                    //cbtnSaveNew.SetVisible(false);
                    //cbtnSaveRecords.SetVisible(false);

                    //InsgridBatch.UpdateEdit();
                    //chkAccount = 0;
                }

            }
        });
    }
    // caspxTaxpopUp.Show();
}
function DebitGotFocus(s, e) {
    ReciptOldValue = s.GetText();
    var indx = ReciptOldValue.indexOf(',');
    if (indx != -1) {
        ReciptOldValue = ReciptOldValue.replace(/,/g, '');
    }
}
function changePaymentTotalSummary() {
    var newDif = PaymentOldValue - PaymentNewValue;
    var CurrentSum = ctxtTotalPayment.GetText();
    var indx = CurrentSum.indexOf(',');
    if (indx != -1) {
        CurrentSum = CurrentSum.replace(/,/g, '');
    }
    ctxtTotalPayment.SetValue(parseFloat(CurrentSum - newDif));
    var newNetAmountDiff = NetAmountOldValue - PaymentNewValue;
    var CurrentNetSum = c_txtTotalNetAmount.GetText();
    c_txtTotalNetAmount.SetValue(parseFloat(CurrentNetSum - newNetAmountDiff));

}

//New By Indranil
function txtPercentageLostFocus(s, e) {
    var ProdAmt = parseFloat(cgridTax.GetEditor("calCulatedOn").GetValue());
    if (s.GetText().trim() != '') {
        if (!isNaN(parseFloat(ProdAmt * s.GetText()) / 100)) {
            //Checking Add or less
            var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
            var taxNameWithSign = cgridTax.GetEditor("TaxField").GetText();
            var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
            if (sign == '(+)') {
                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * s.GetText()) / 100);
                ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt);
                GlobalCurTaxAmt = 0;
            }
            else {

                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                cgridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * s.GetText()) / 100) * -1);
                ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) * -1) - (GlobalCurTaxAmt * -1));
                GlobalCurTaxAmt = 0;
            }
            SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''));

            //Call for Running Total
            SetRunningTotal();

        } else {
            s.SetText("");
        }
    }
    RecalCulateTaxTotalAmountInline();
}
function CmbtaxClick(s, e) {
    GlobalCurTaxAmt = parseFloat(ctxtGstCstVat.GetText());
    gstcstvatGlobalName = s.GetText();
}
function taxAmountLostFocus(s, e) {
    var finalTaxAmt = parseFloat(s.GetValue());
    var totAmt = parseFloat(ctxtTaxTotAmt.GetText());
    var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
    var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
    if (sign == '(+)') {
        ctxtTaxTotAmt.SetValue(totAmt + finalTaxAmt - taxAmountGlobal);
    } else {
        ctxtTaxTotAmt.SetValue(totAmt + (finalTaxAmt * -1) - (taxAmountGlobal * -1));
    }
    SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''));
    //Set Running Total
    SetRunningTotal();
    RecalCulateTaxTotalAmountInline();
}
function QuotationTaxAmountGotFocus(s, e) {
    taxAmountGlobalCharges = parseFloat(s.GetValue());
}
function SetOtherTaxValueOnRespectiveRow(idx, amt, name) {
    for (var i = 0; i < taxJson.length; i++) {
        if (taxJson[i].applicableBy == name) {
            cgridTax.batchEditApi.StartEdit(i, 3);
            cgridTax.GetEditor('calCulatedOn').SetValue(amt);

            var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
            var taxNameWithSign = cgridTax.GetEditor("TaxField").GetText();
            var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
            var ProdAmt = parseFloat(cgridTax.GetEditor("calCulatedOn").GetValue());
            var s = cgridTax.GetEditor("TaxField");
            if (sign == '(+)') {
                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * s.GetText()) / 100);

                ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt);
                GlobalCurTaxAmt = 0;
            }
            else {

                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                cgridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * s.GetText()) / 100) * -1);

                ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) * -1) - (GlobalCurTaxAmt * -1));
                GlobalCurTaxAmt = 0;
            }
        }
    }
    //return;
    cgridTax.batchEditApi.EndEdit();

}

//-----------------------------------------------------------------------------------------------------

function recalculatePayment(oldVal) {
    if (oldVal != 0) {
        PaymentNewValue = 0;
        PaymentOldValue = oldVal;
        changePaymentTotalSummary();
    }
}
function changeReciptTotalSummary() {
    // debugger;
    var newDif = ReciptOldValue - ReciptNewValue;
    var CurrentSum = c_txt_Debit.GetText();
    var indx = CurrentSum.indexOf(',');
    if (indx != -1) {
        CurrentSum = CurrentSum.replace(/,/g, '');
    }

    c_txt_Debit.SetValue(parseFloat(CurrentSum - newDif));
}
function ReceiptLostFocus(s, e) {
    //  debugger;
    ReciptNewValue = s.GetText();
    var indx = ReciptNewValue.indexOf(',');

    if (indx != -1) {
        ReciptNewValue = ReciptNewValue.replace(/,/g, '');
    }
    if (ReciptOldValue != ReciptNewValue) {
        changeReciptTotalSummary();
    }
}
function ReceiptTextChange(s, e) {
    // debugger;
    ReceiptLostFocus(s, e);
    var RecieveValue = (InsgridBatch.GetEditor('btnRecieve').GetValue() != null) ? parseFloat(InsgridBatch.GetEditor('btnRecieve').GetValue()) : "0";
    var receiptValue = (InsgridBatch.GetEditor('btnPayment').GetValue() != null) ? InsgridBatch.GetEditor('btnPayment').GetValue() : "0";

    if (RecieveValue > 0) {
        recalculatePayment(InsgridBatch.GetEditor('btnPayment').GetValue());
        InsgridBatch.GetEditor('btnPayment').SetValue("0");
        if (RecieveValue != ReciptOldValue) {
            InsgridBatch.GetEditor('TaxAmount').SetValue("0");
        }
        InsgridBatch.GetEditor('NetAmount').SetValue(RecieveValue);
    }


}
function Receipt_TextChange(s, e) {
    //  debugger;
    var noofvisiblerows = InsgridBatch.GetVisibleRowsOnPage();
    var RecieveValue = (InsgridBatch.GetEditor('btnRecieve').GetValue() != null) ? parseFloat(InsgridBatch.GetEditor('btnRecieve').GetValue()) : "0";
    var PaymentValue = (InsgridBatch.GetEditor('btnPayment').GetValue() != null) ? InsgridBatch.GetEditor('btnPayment').GetValue() : "0";

    if (parseFloat(RecieveValue) > 0) {
        var tbPayment = InsgridBatch.GetEditor("btnPayment");
        tbPayment.SetValue("0");
    }

    var noofvisiblerows = InsgridBatch.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
    var i, cnt = 1;
    var sumPayment = 0, sumRecieve = 0;
    for (i = -1 ; cnt <= noofvisiblerows ; i--) {
        var Payment = (InsgridBatch.batchEditApi.GetCellValue(i, 'btnPayment') != null) ? (InsgridBatch.batchEditApi.GetCellValue(i, 'btnPayment')) : "0";
        var Recieve = (InsgridBatch.batchEditApi.GetCellValue(i, 'btnRecieve') != null) ? (InsgridBatch.batchEditApi.GetCellValue(i, 'btnRecieve')) : "0";

        sumPayment = sumPayment + parseFloat(Payment);
        sumRecieve = sumRecieve + parseFloat(Recieve);

        cnt++;
    }

    if (parseFloat(RecieveValue) > 0) {
        c_txt_Debit.SetValue(sumRecieve + parseFloat(RecieveValue));
        ctxtTotalPayment.SetValue(sumPayment - parseFloat(PaymentValue));
    }
    else {
        c_txt_Debit.SetValue(sumRecieve + parseFloat(RecieveValue));
        ctxtTotalPayment.SetValue(sumPayment + parseFloat(PaymentValue));
    }
}
function Payment_TextChange(s, e) {
    //  debugger;
    var noofvisiblerows = InsgridBatch.GetVisibleRowsOnPage();
    var RecieveValue = (InsgridBatch.GetEditor('btnRecieve').GetValue() != null) ? parseFloat(InsgridBatch.GetEditor('btnRecieve').GetValue()) : "0";
    var PaymentValue = (InsgridBatch.GetEditor('btnPayment').GetValue() != null) ? InsgridBatch.GetEditor('btnPayment').GetValue() : "0";

    if (parseFloat(PaymentValue) > 0) {
        var tbRecieve = InsgridBatch.GetEditor("btnRecieve");
        tbRecieve.SetValue("0");
    }

    var noofvisiblerows = InsgridBatch.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
    var i, cnt = 1;
    var sumPayment = 0, sumRecieve = 0;
    for (i = -1 ; cnt <= noofvisiblerows ; i--) {
        var Payment = (InsgridBatch.batchEditApi.GetCellValue(i, 'btnPayment') != null) ? (InsgridBatch.batchEditApi.GetCellValue(i, 'btnPayment')) : "0";
        var Recieve = (InsgridBatch.batchEditApi.GetCellValue(i, 'btnRecieve') != null) ? (InsgridBatch.batchEditApi.GetCellValue(i, 'btnRecieve')) : "0";

        sumPayment = sumPayment + parseFloat(Payment);
        sumRecieve = sumRecieve + parseFloat(Recieve);

        cnt++;
    }

    if (parseFloat(PaymentValue) > 0) {
        c_txt_Debit.SetValue(sumRecieve - parseFloat(RecieveValue));
        ctxtTotalPayment.SetValue(sumPayment + parseFloat(PaymentValue));
    }
    else {
        c_txt_Debit.SetValue(sumRecieve + parseFloat(RecieveValue));
        ctxtTotalPayment.SetValue(sumPayment + parseFloat(PaymentValue));
    }
}
function Calculate() {
    var noofvisiblerows = InsgridBatch.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
    var i, cnt = 1;
    var sumPayment = 0, sumRecieve = 0;
    for (i = -1 ; cnt <= noofvisiblerows ; i--) {
        var Payment = (InsgridBatch.batchEditApi.GetCellValue(i, 'btnPayment') != null) ? (InsgridBatch.batchEditApi.GetCellValue(i, 'btnPayment')) : "0";
        var Recieve = (InsgridBatch.batchEditApi.GetCellValue(i, 'btnRecieve') != null) ? (InsgridBatch.batchEditApi.GetCellValue(i, 'btnRecieve')) : "0";

        sumPayment = sumPayment + parseFloat(Payment);
        sumRecieve = sumRecieve + parseFloat(Recieve);

        cnt++;
    }

    c_txt_Debit.SetValue(sumRecieve);
    ctxtTotalPayment.SetValue(sumPayment);
}
var lastCRP = null;
function rbtnType_SelectedIndexChanged() {
    document.getElementById('txtVoucherNo').value = "";
    var VoucherType = document.getElementById('rbtnType').value;

    //if (cCmbScheme.InCallback()) {
    //    lastCRP = VoucherType;
    //}
    //else {
    //cCmbScheme.PerformCallback(VoucherType);
    var OtherDetails = {}
    OtherDetails.VoucherType = VoucherType;
    $.ajax({
        type: "POST",
        url: "CashBankEntry.aspx/GetNumberingSchema",
        data: JSON.stringify(OtherDetails),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var returnObject = msg.d;
            if (returnObject) {
                SetDataSourceOnComboBox(cCmbScheme, returnObject);
            }
        }
    });


    //}
    if (VoucherType == "P") {
        $("#dvrefcashbankreq").show();
        document.getElementById('divPaidTo').style.display = 'block';
        document.getElementById('divReceivedfrom').style.display = 'none';
        //chinmoy edited below code for new billing shipping
        // LoadCustomerAddress('', $('#ddlBranch').val(), 'PO');

        if ($("#ddlBranch").val() != null && $("#ddlBranch").val() != "") {
            SetPurchaseBillingShippingAddress($('#ddlBranch').val());
        }
    }
    else {
        $("#dvrefcashbankreq").hide();
        document.getElementById('divReceivedfrom').style.display = 'block';
        document.getElementById('divPaidTo').style.display = 'none';
        ClearBillingShipping();
    }
}
function SetDataSourceOnComboBox(ControlObject, Source) {
    ControlObject.ClearItems();
    for (var count = 0; count < Source.length; count++) {
        ControlObject.AddItem(Source[count].Name, Source[count].Id);
    }
    ControlObject.SetSelectedIndex(0);
}
function CmbSchemeEndCallback() {
    if (lastCRP) {
        cCmbScheme.PerformCallback(lastCRP);
        lastCRP = null;
    }

}
function OnAddNewClick() {

    InsgridBatch.AddNewRow();
    var noofvisiblerows = InsgridBatch.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
    var i;
    var cnt = 1;
    for (i = -1 ; cnt <= noofvisiblerows ; i--) {
        var tbQuotation = InsgridBatch.GetEditor("SrlNo");
        tbQuotation.SetValue(cnt);
        cnt++;
    }
}
var CustomDeleteID = "";
function OnCustomButtonClick(s, e) {

    if (e.buttonID == 'AddNew') {
        InsgridBatch.batchEditApi.StartEdit(e.visibleIndex);
        var mainAccountValue = (InsgridBatch.GetEditor('MainAccount').GetValue() != null) ? InsgridBatch.GetEditor('MainAccount').GetValue() : "";
        var btnRecieve = (InsgridBatch.GetEditor('btnRecieve').GetValue() != null) ? InsgridBatch.GetEditor('btnRecieve').GetValue() : "";
        var btnPayment = (InsgridBatch.GetEditor('btnPayment').GetValue() != null) ? InsgridBatch.GetEditor('btnPayment').GetValue() : "";
        if (mainAccountValue != "" && (btnRecieve != "0.0" || btnPayment != "0.0")) {
            document.getElementById('hdnTaxGridBind').value = 'YES';
            InsgridBatch.SetFocusedRowIndex();
            //document.getElementById('hdnCheckAdd').value = 'YES';
            OnAddNewClick();
        }
    }
    if (e.buttonID == 'CustomDelete') {

        var SrlNo = InsgridBatch.batchEditApi.GetCellValue(e.visibleIndex, 'SrlNo');

        $('#hdnRefreshType').val('');
        $('#hdnDeleteSrlNo').val(SrlNo);
        CustomDeleteID = "1";
        var noofvisiblerows = InsgridBatch.GetVisibleRowsOnPage();
        if (noofvisiblerows != "1") {
            InsgridBatch.batchEditApi.StartEdit(e.visibleIndex, 1);
            InsgridBatch.DeleteRow(e.visibleIndex);
            SuffuleSerialNumber();
          //  $('#hdfIsDelete').val('D');
           // InsgridBatch.UpdateEdit();

        //    InsgridBatch.PerformCallback('Display');
          //  $('#hdnPageStatus').val('delete');


            var PaymentValue = (InsgridBatch.GetEditor('btnPayment').GetValue() != null) ? InsgridBatch.GetEditor('btnPayment').GetValue() : "0";
            var ReceiptValue = (InsgridBatch.GetEditor('btnRecieve').GetValue() != null) ? parseFloat(InsgridBatch.GetEditor('btnRecieve').GetValue()) : "0";

            var receiptTotValue = c_txt_Debit.GetValue();
            var paymentTotValue = ctxtTotalPayment.GetValue();

            c_txt_Debit.SetValue(parseFloat(receiptTotValue) - parseFloat(ReceiptValue));
            ctxtTotalPayment.SetValue(parseFloat(paymentTotValue) - parseFloat(PaymentValue));


            //document.getElementById('btnSaveNew').style.display = 'block';
            //document.getElementById('btnSaveRecords').style.display = 'block';
          //  cbtnSaveNew.SetVisible(false);
          //  cbtnSaveRecords.SetVisible(false);


        }
    }
}
function SuffuleSerialNumber() {
    var TotRowNumber = InsgridBatch.GetVisibleItemsOnPage();
    var SlnoCount = 1;
    for (var i = 0; i < 1000; i++) {
        if (InsgridBatch.GetRow(i)) {
            if (InsgridBatch.GetRow(i).style.display != "none") {
                InsgridBatch.batchEditApi.StartEdit(i, 2);
                InsgridBatch.GetEditor("SrlNo").SetText(SlnoCount);
                SlnoCount++;
            }
        }
    }

    for (i = -1; i > -1000; i--) {
        if (InsgridBatch.GetRow(i)) {
            if (InsgridBatch.GetRow(i).style.display != "none") {
                InsgridBatch.batchEditApi.StartEdit(i, 2);
                InsgridBatch.GetEditor("SrlNo").SetText(SlnoCount);
                SlnoCount++;
            }
        }
    }
}
function PaymentgotFocus(s, e) {
    //  debugger;
    GlobargotPayMent = s.GetValue();

}
function PaymentLostFocus(s, e) {

    var PayVal = parseFloat((s.GetValue() != null) ? s.GetValue() : "0");
    if (GlobargotPayMent != null) {
        if (parseFloat(GlobargotPayMent) != PayVal) {
            var Curval = parseFloat(ctxtTotalPayment.GetText());
            var Totalval = PayVal + Curval - GlobargotPayMent;
            ctxtTotalPayment.SetValue(Totalval);
            GlobargotPayMent = null;
        }
    }

}
function RecptgotFocus(s, e) {
    // debugger;
    GlobargotRecpt = s.GetValue();

}
function SumReceipt(s, e) {

    var recptVal = parseFloat((s.GetValue() != null) ? s.GetValue() : "0");
    if (GlobargotRecpt != null) {
        if (parseFloat(GlobargotRecpt) != recptVal) {
            var Curval = parseFloat(c_txt_Debit.GetText());
            var Totalval = recptVal + Curval - GlobargotRecpt;
            c_txt_Debit.SetValue(Totalval);
            GlobargotRecpt = null;
        }
    }

}
function ChangeVoucherType() {
    var colName;
    var val = "0";
    var AspRadio = document.getElementById("rbtnType");
    var AspRadio_ListItem = AspRadio.getElementsByTagName('input');
    for (var i = 0; i < AspRadio_ListItem.length; i++) {
        if (AspRadio_ListItem[i].checked) {
            val = AspRadio_ListItem[i].value;
        }
    }

    if (val == "R") {

        InsgridBatch.GetEditor('btnPayment').SetEnabled(false);

    }
    else {
        InsgridBatch.GetEditor('btnPayment').SetEnabled(true);
    }
    if (val == "P") {

        InsgridBatch.GetEditor('btnRecieve').SetEnabled(false);
    }
    else {
        InsgridBatch.GetEditor('btnRecieve').SetEnabled(true);
    }
}

function deleteAllRows() {
    var frontRow = 0;
    var backRow = -1;
    for (var i = 0; i <= InsgridBatch.GetVisibleRowsOnPage() + 100 ; i++) {
        InsgridBatch.DeleteRow(frontRow);
        InsgridBatch.DeleteRow(backRow);
        backRow--;
        frontRow++;
    }
    InsgridBatch.AddNewRow();

    ctxtTotalPayment.SetValue(0);
    c_txt_Debit.SetValue(0);

}
//function onBranchItems(e) {

//    //get the first row accounting value debjyoti 
//    InsgridBatch.batchEditApi.StartEdit(-1, 1);
//    var accountingDataMin = InsgridBatch.GetEditor('MainAccount').GetValue();
//    InsgridBatch.batchEditApi.EndEdit();

//    InsgridBatch.batchEditApi.StartEdit(0, 1);
//    var accountingDataplus = InsgridBatch.GetEditor('MainAccount').GetValue();
//    InsgridBatch.batchEditApi.EndEdit();


//    if (accountingDataMin != null || accountingDataplus != null) {
//        jConfirm('You have changed Branch. All the entries of ledger in this voucher to be reset to blank. \n You have to select and re-enter. Continue?', 'Confirmation Dialog', function (r) {

//            if (r == true) {
//                deleteAllRows();

//                MainAccount.PerformCallback(document.getElementById('ddlBranch').value);
//                oldBranchdata = document.getElementById('lstBranchItems').value;
//                $('#MandatoryBranch').hide();
//                BindCashBankAccountListByBranch(document.getElementById('lstBranchItems').value);
//            } else {
//                Bind_Branch_Edit(oldBranchdata);
//            }
//        });

//    }
//    else {
//        BindCashBankAccountListByBranch(document.getElementById('lstBranchItems').value);
//        //MainAccount.PerformCallback(document.getElementById('lstBranchItems').value);
//        MainAccount.PerformCallback(document.getElementById('ddlBranch').value);
//    }

//}
function AddBatchNew(s, e) {
    InsgridBatch.batchEditApi.StartEdit(e.visibleIndex);
    var keyCode = ASPxClientUtils.GetKeyCode(e.htmlEvent);
    if (keyCode === 13) {
        var mainAccountValue = (InsgridBatch.GetEditor('MainAccount').GetValue() != null) ? InsgridBatch.GetEditor('MainAccount').GetValue() : "";
        var btnRecieve = (InsgridBatch.GetEditor('btnRecieve').GetValue() != null) ? InsgridBatch.GetEditor('btnRecieve').GetValue() : "";
        var btnPayment = (InsgridBatch.GetEditor('btnPayment').GetValue() != null) ? InsgridBatch.GetEditor('btnPayment').GetValue() : "";
        if (mainAccountValue != "" && (btnRecieve != "0.0" || btnPayment != "0.0")) {
            InsgridBatch.AddNewRow();
            InsgridBatch.SetFocusedRowIndex();
        }
    }
    else if (keyCode === 9) {
        cbtnSaveNew.Focus();
    }
    else {
        return false;
    }

}
//...................Shortcut keys.................

document.onkeydown = function (e) {
    if (event.keyCode == 83 && event.altKey == true) {
        //run code for Alt+S -- ie, save!   
        StopDefaultAction(e);

        if (CustomDeleteID == "1") {

        }
        else {
            document.getElementById('btnSaveNew').click();
        }

    }
    else if ((event.keyCode == 120 || event.keyCode == 88) && event.altKey == true) {
        //run code for Alt+X -- ie, Save & Exit! 
        StopDefaultAction(e);
        if (CustomDeleteID == "1") {

        }
        else {
            document.getElementById('btnSaveRecords').click();
        }


        //return false;
    }
        //else if ((event.keyCode == 120 || event.keyCode == 65) && event.altKey == true) {

        //    if (document.getElementById('DivEntry').style.display != 'block') {
        //        if (document.getElementById('AddButton').style.display != 'none') {
        //            AddButtonClick();
        //        }
        //    }
        //}
    else if (event.keyCode == 79 && event.altKey == true) {
        //run code for Alt+X -- ie, Billing/Shipping Ok button! 
        StopDefaultAction(e);
        if (page.GetActiveTabIndex() == 1) {
            //chinmoy commeneted for new billing shipping
            // fnSaveBillingShipping();
        }
        return false;
    }
}
function StopDefaultAction(e) {
    if (e.preventDefault) { e.preventDefault() }
    else { e.stop() };

    e.returnValue = false;
    e.stopPropagation();
}
//...................end............................

function InstrumentTypeSelectedIndexChanged() {
    $("#MandatoryInstrumentType").hide();
    $("#MandatoryInstNo").hide();

    var InstType = cComboInstrumentTypee.GetValue();

    if (InstType == "CH") {
        $('#hdnInstrumentType').val(0);
        document.getElementById("divInstrumentNo").style.display = 'none';
        document.getElementById("tdIDateDiv").style.display = 'none';
        document.getElementById("divDraweeBank").style.display = 'none';
    }
    else {
        $('#hdnInstrumentType').val(InstType);
        document.getElementById("divInstrumentNo").style.display = 'block';
        document.getElementById("tdIDateDiv").style.display = 'block';
        document.getElementById("divDraweeBank").style.display = 'block';
    }
}

function ClearBillingShipping() {
    //shipping
    ctxtsAddress1.SetText('');
    ctxtsAddress2.SetText('');
    ctxtsAddress3.SetText('');
    ctxtslandmark.SetText('');
    ctxtShippingPin.SetText('');
    $('#hdShippingPin').val('');
    ctxtshippingCountry.SetText('');
    $('#hdCountryIdShipping').val('');
    ctxtshippingState.SetText('');
    $('#hdStateCodeShipping').val('');
    $('#hdStateIdShipping').val('');
    ctxtshippingCity.SetText('');
    $('#hdCityIdShipping').val('');
    ctxtSelectShippingArea.SetText('');
    $('#hdAreaIdShipping').val('');
    // ctxtDistanceShipping.SetText('');
    ctxtShippingGSTIN1.SetText('');
    ctxtShippingGSTIN2.SetText('');
    ctxtShippingGSTIN3.SetText('');

    //billing
    ctxtAddress1.SetText('');
    ctxtAddress2.SetText('');
    ctxtAddress3.SetText('');
    ctxtbillingPin.SetText('');
    $('#hdBillingPin').val('');
    ctxtbillingCountry.SetText('');
    $('#hdCountryIdBilling').val('');
    ctxtbillingState.SetText('');
    $('#hdStateIdBilling').val('');
    $('#hdStateCodeBilling').val('');
    ctxtbillingCity.SetText('');
    $('#hdCityIdBilling').val('');
    //var GSTIN = BillShipDet.GSTIN;
    //GSTIN1 = GSTIN.substring(0, 2);
    //GSTIN2 = GSTIN.substring(2, 12);
    //GSTIN3 = GSTIN.substring(12, 15);
    ctxtBillingGSTIN1.SetText('');
    ctxtBillingGSTIN2.SetText('');
    ctxtBillingGSTIN3.SetText('');

}

function CmbScheme_ValueChange() {
    var NoSchemeTypedtl = (cCmbScheme.GetValue() == null ? "" : cCmbScheme.GetValue());
    var schemetype = NoSchemeTypedtl.toString().split('~')[1];
    var schemelength = NoSchemeTypedtl.toString().split('~')[2];
    var branchID = (NoSchemeTypedtl.toString().split('~')[3] != null) ? NoSchemeTypedtl.toString().split('~')[3] : "";
    var fromdate = (NoSchemeTypedtl.toString().split('~')[4] != null) ? NoSchemeTypedtl.toString().split('~')[4] : "";
    var todate = (NoSchemeTypedtl.toString().split('~')[5] != null) ? NoSchemeTypedtl.toString().split('~')[5] : "";

    /* Mantis Issue 24725 03.03.2022*/
    var CopyType = $('#hrCopy').val();/*Get the request string value from hidden field of copy*/
    /* End of Mantis Issue 24725 03.03.2022 */
  
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

    //MainAccount.PerformCallback(branchID);
    $('#txtVoucherNo').attr('maxLength', schemelength);
    $('#hdnBranchId').val(branchID);
    $('#hfIsFilter').val(branchID);

    document.getElementById('ddlBranch').value = branchID;
    //cddlCashBank.PerformCallback(branchID);
    var OtherDetails = {}
    OtherDetails.branchID = branchID;
    $.ajax({
        type: "POST",
        url: "CashBankEntry.aspx/GetCashBank",
        data: JSON.stringify(OtherDetails),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {
            var returnObject = msg.d;
            if (returnObject) {
                SetDataSourceOnComboBox(cddlCashBank, returnObject);
            }
        }
    });
    CashBank_SelectedIndexChanged();
    document.getElementById('ddlEnterBranch').value = branchID;
    var VoucherType = document.getElementById('rbtnType').value;
    ClearBillingShipping();

    var CashBankText = cddlCashBank.GetText();
    if (CashBankText != "") {
        var arr = CashBankText.split('|');
        var strbranch = $('#hdnBranchId').val();
    }
    else {
        document.getElementById("pageheaderContent").style.display = 'none';
    }

   /* deleteAllRows();
    InsgridBatch.AddNewRow();
    InsgridBatch.GetEditor('SrlNo').SetValue('1');
    InsgridBatch.batchEditApi.EndEdit();*/

    /* Mantis Issue 24725 03.03.2022*/
    if (CopyType != 'Copy') {
        deleteAllRows();
        InsgridBatch.AddNewRow();
        InsgridBatch.GetEditor('SrlNo').SetValue('1');
        InsgridBatch.batchEditApi.EndEdit();
    }
    /* End of Mantis Issue 24725 03.03.2022 */
   
    c_txt_Debit.SetValue("0.00");
    ctxtTotalPayment.SetValue("0.00");

    if (schemetype == '0') {
        $('#hdnSchemaType').val('0');
        document.getElementById('txtVoucherNo').disabled = false;
        document.getElementById('txtVoucherNo').value = "";

    }
    else if (schemetype == '1') {
        $('#hdnSchemaType').val('1');
        document.getElementById('txtVoucherNo').disabled = true;
        document.getElementById('txtVoucherNo').value = "Auto";
        $("#MandatoryBillNo").hide();
        cdtTDate.Focus();
    }
    else if (schemetype == '2') {
        $('#hdnSchemaType').val('2');
        document.getElementById('txtVoucherNo').disabled = true;
        document.getElementById('txtVoucherNo').value = "Datewise";
    }
    else if (schemetype == 'n') {
        document.getElementById('txtVoucherNo').disabled = true;
        document.getElementById('txtVoucherNo').value = "";
    }


    if (schemetype == '0') {
        document.getElementById("txtVoucherNo").focus();
    } else {
        cCmbScheme.Focus();
    }
    //chinmoy edited below code for new billing shipping
    // LoadCustomerAddress('', $('#ddlBranch').val(), 'PO');
    if ($("#ddlBranch").val() != null && $("#ddlBranch").val() != "") {
        SetPurchaseBillingShippingAddress($('#ddlBranch').val());
    }
    
}


function IntializeGlobalVariables(InsgridBatch) {
    lastMainAccountID = InsgridBatch.cplastMainAccountID;
    currentEditableVisibleIndex = -1;
    setValueFlag = -1;

}


function WithdrawalChangedNew(WithDrawType) {

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
        var comboitem = cComboInstrumentTypee.FindItemByValue('CH');
        if (comboitem != undefined && comboitem != null) {
            cComboInstrumentTypee.RemoveItem(comboitem.index);
            cComboInstrumentTypee.SetValue("C");
            InstrumentTypeSelectedIndexChanged();
        }
    }
}


function GridADD() {
    // AddButtonClick();
    // InsgridBatch.PerformCallback('Display');

}



function OnBatchEditStartEditing(s, e) {

    currentEditableVisibleIndex = e.visibleIndex;
    globalRowIndex = e.visibleIndex;

    if ($("#hdnIsTDS").val() == "1") {
        e.cancel = true;
    }

    //var currentMainAccount = InsgridBatch.batchEditApi.GetCellValue(currentEditableVisibleIndex, "MainAccount");
    //var SubAccountIDColumn = s.GetColumnByField("bthSubAccount");
    //if (!e.rowValues.hasOwnProperty(SubAccountIDColumn.index))
    //    return;
    //var cellInfo = e.rowValues[SubAccountIDColumn.index];


    //if (lastMainAccountID == currentMainAccount)
    //    if (SubAccount_ReferenceID.FindItemByValue(cellInfo.value) != null)
    //        SubAccount_ReferenceID.SetValue(cellInfo.value);
    //    else {

    //        if (e.focusedColumn.fieldName != "TaxAmount") {
    //            LoadingPanel.Show();
    //        }
    //        RefreshData(cellInfo, lastMainAccountID);

    //    }

    //else {
    //    if (currentMainAccount == null) {
    //        SubAccount_ReferenceID.SetSelectedIndex(-1);
    //        return;
    //    }
    //    lastMainAccountID = currentMainAccount;


    //    if (e.focusedColumn.fieldName != "TaxAmount") {
    //        LoadingPanel.Show();
    //    }
    //    RefreshData(cellInfo, lastMainAccountID);

    //}
}
//function RefreshData(cellInfo, MainAccountID) {
//    setValueFlag = cellInfo.value;

//    if (setValueFlag != null) {
//        SubAccount_ReferenceID.PerformCallback(MainAccountID + '~' + setValueFlag);
//    }
//        // SubAccount_ReferenceID.PerformCallback(MainAccountID + '~' + setValueFlag);            
//        //else if(document.getElementById('hdnCheckAdd').value == 'YES')
//        //{
//        //    document.getElementById('hdnCheckAdd').value = "";
//        //    SubAccount_ReferenceID.PerformCallback(MainAccountID + '~' + setValueFlag);
//        //}
//    else {
//        LoadingPanel.Hide();
//    }
//}
//function OnBatchEditEndEditing(s, e) {

//    currentEditableVisibleIndex = -1;
//    var SubAccountIDColumn = s.GetColumnByField("bthSubAccount");
//    if (!e.rowValues.hasOwnProperty(SubAccountIDColumn.index))
//        return;
//    var cellInfo = e.rowValues[SubAccountIDColumn.index];
//    if (SubAccount_ReferenceID.GetSelectedIndex() > -1 || cellInfo.text != SubAccount_ReferenceID.GetText()) {
//        cellInfo.value = SubAccount_ReferenceID.GetValue();
//        cellInfo.text = SubAccount_ReferenceID.GetText();
//        SubAccount_ReferenceID.SetValue(null);
//    }
//}
//function SubAccountCombo_EndCallback(s, e) {
//    if (setValueFlag == null || setValueFlag == "0" || setValueFlag == "") {
//        s.SetSelectedIndex(-1);
//    }
//    else {
//        if (SubAccount_ReferenceID.FindItemByValue(setValueFlag) != null) {
//            SubAccount_ReferenceID.SetValue(setValueFlag);
//            setValueFlag = null;
//        }
//    }
//    var reverseApplicable = InsgridBatch.GetEditor("ReverseApplicable");
//    reverseApplicable.SetValue(SubAccount_ReferenceID.cpReverseApplicable);                
//    $("#IsTaxApplicable").val(SubAccount_ReferenceID.cpIsTaxable);
//    var VoucherType = document.getElementById('rbtnType').value;
//    if (SubAccount_ReferenceID.cpReverseApplicable == "1" && VoucherType == "P") {
//        $("#chk_reversemechenism").prop("disabled", false);
//        $("#chk_reversemechenism").prop("checked", true);
//    }
//    else {
//        if ($("#chk_reversemechenism").prop('checked') == false) {
//            $("#chk_reversemechenism").prop("checked", false);
//        }
//    }
//    SubAccount_ReferenceID.cpReverseApplicable = null;
//    LoadingPanel.Hide();
//}
function OnInit(s, e) {
    IntializeGlobalVariables(s);
}
//....end....

function deleteAllRows() {
    var frontRow = 0;
    var backRow = -1;
    for (var i = 0; i <= InsgridBatch.GetVisibleRowsOnPage() + 100 ; i++) {
        InsgridBatch.DeleteRow(frontRow);
        InsgridBatch.DeleteRow(backRow);
        backRow--;
        frontRow++;
    }

}
//function focusval(obj) {

//}

$(function () {

    // BindCashBankAccountList();
    //BindBranchList();
    // ListBranchBind();
    // ListAccountBind();

    // ListMainAccountBind();
    //  ListSubAccountBind();


    //BindWithFromList();

    //ListWithFromBind();
    // BindDepositIntoList();
    //ListDepositIntoBind();

});
//ProtoType
String.prototype.trim = function () {
    return this.replace(/^\s+|\s+$/g, "");
}
String.prototype.ltrim = function () {
    return this.replace(/^\s+/, "");
}
String.prototype.rtrim = function () {
    return this.replace(/\s+$/, "");
}
//Global Variable
FieldName = 'txtVoucherNo';
IsSubAccountChange = "False";
Param_SubAccountID = '';
SubLedgerType = "";
ActiveCurrencyID = "";
ActiveCurrencyName = "";
ActiveCurrencySymbol = "";
function checkTextAreaMaxLength(textBox, e, length) {

    var mLen = textBox["MaxLength"];
    if (null == mLen)
        mLen = length;
    var maxLength = parseInt(mLen);
    if (!checkSpecialKeys(e)) {
        if (textBox.value.length > maxLength - 1) {
            if (window.event)//IE
                e.returnValue = false;
            else//Firefox
                e.preventDefault();
        }
    }
}

function checkSpecialKeys(e) {
    if (e.keyCode != 8 && e.keyCode != 46 && e.keyCode != 37 && e.keyCode != 38 && e.keyCode != 39 && e.keyCode != 40)
        return false;
    else
        return true;
}


function SetAllDisplayNone() {
    document.getElementById("tdIBankLable").style.display = 'none';
    document.getElementById("tdIBankValue").style.display = 'none';
    document.getElementById("tdCBankLable").style.display = 'none';
    document.getElementById("tdCBankValue").style.visibility = 'hidden'
    document.getElementById("tdAuthLable").style.display = 'none';
    document.getElementById("tdAuthValue").style.display = 'none';
    document.getElementById("tdPayeeLable").style.visibility = 'hidden';
    document.getElementById("tdPayeeValue").style.visibility = 'hidden';
    document.getElementById("tdContraEntry").style.display = 'none';
    document.getElementById("tdpayment").style.display = 'none';
    document.getElementById("tdpaymentValue").style.display = 'none';
    document.getElementById("tdpaymentDiv").style.display = 'none'
    document.getElementById("tdRecieve").style.display = 'none';
    document.getElementById("tdRecieveValue").style.display = 'none';
    document.getElementById("tdRecieveDiv").style.display = 'none'
    document.getElementById("tdINoLable").style.visibility = 'hidden'
    document.getElementById("tdINoDiv").style.display = 'none'
    document.getElementById("tdINoValue").style.visibility = 'hidden'
    document.getElementById("tdIDateLable").style.visibility = 'hidden'
    document.getElementById("tdIDateDiv").style.display = 'none'
    document.getElementById("tdIDateValue").style.visibility = 'hidden'

}

function PopulateCurrentBankBalance(MainAccountID, BranchId) {
    if (MainAccountID.trim() == "" || BranchId.trim() == "") {
        return;
    }
    else {
        $.ajax({
            type: "POST",
            url: 'CashBankEntry.aspx/GetCurrentBankBalance',
            data: "{MainAccountID:\"" + MainAccountID + "\",BranchID:\"" + BranchId + "\"}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var list = msg.d;

                if (msg.d.length > 0) {
                    document.getElementById("pageheaderContent").style.display = 'block';
                    if (msg.d.split('~')[0] != '') {
                        document.getElementById('B_ImgSymbolBankBal').innerHTML = ActiveCurrencySymbol;
                        document.getElementById('B_BankBalance').innerHTML = msg.d.split('~')[0];
                        document.getElementById('B_BankBalance').style.color = "Black";
                    }
                    else {
                        document.getElementById('B_ImgSymbolBankBal').innerHTML = '';
                        document.getElementById('B_BankBalance').innerHTML = '0.0';
                        document.getElementById('B_BankBalance').style.color = "Black";
                    }
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                jAlert(textStatus);
            }
        });
    }
}
//function setDepositIntoBind(obj) {
//    if (obj) {
//        var lstDepositInto = document.getElementById("lstDepositIntoItems");
//        for (var i = 0; i < lstDepositInto.options.length; i++) {

//            var DepositIntoval = lstDepositInto.options[i].value;
//            var n = DepositIntoval.indexOf("~");
//            var res = DepositIntoval.substr(0, n);
//            if (res == obj) {
//                lstDepositInto.options[i].selected = true;
//            }
//        }
//    }
//}
//........Bind Branch......
//function BindBranchList() {
//    var lBox = $('select[id$=lstBranchItems]');
//    var lstBranchItems = [];
//    //Customer or Lead radio button is clicked kaushik 21-11-2016
//    lBox.empty();
//    $.ajax({
//        type: "POST",
//        url: 'CashBankEntry.aspx/GetBranchList',
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        success: function (msg) {
//            var list = msg.d;
//            if (list.length > 0) {

//                for (var i = 0; i < list.length; i++) {

//                    var id = '';
//                    var name = '';
//                    id = list[i].split('|')[1];
//                    name = list[i].split('|')[0];

//                    lstBranchItems.push('<option value="' +
//                    id + '">' + name
//                    + '</option>');
//                }

//                $(lBox).append(lstBranchItems.join(''));
//                ListBranchBind();
//                $('#lstBranchItems').trigger("chosen:updated");
//                $('#lstBranchItems').prop('disabled', false).trigger("chosen:updated");

//            }
//            else {
//                lBox.empty();
//                ListBranchBind();
//                $('#lstBranchItems').trigger("chosen:updated");
//                $('#lstBranchItems').prop('disabled', true).trigger("chosen:updated");

//            }
//        },
//        error: function (XMLHttpRequest, textStatus, errorThrown) {
//            jAlert(textStatus);
//        }
//    });
//}
//......end....



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
    debugger;
    $('#hdnBtnClick').val("Save_Exit");
    $('#hdnJNMode').val('0'); //Entry     
    $('#hdnRefreshType').val('E');
    $('#hdfIsDelete').val('I');
    $('#hdnPayment').val('NO');
    $('#hdnTaxGridBind').val('NO');
    $("#hdnTDSSection").val(TDSSection);
    var PaidToYesNO = document.getElementById('hdnPaidToYesNO').value;
    var VoucherType = document.getElementById('rbtnType').value;
    var Branch = $('#ddlBranch').val();
    var CashBank = cddlCashBank.GetValue();
    var InstrumentType = cComboInstrumentTypee.GetValue();
    var InstrumentNo = ctxtInstNobth.GetValue();
    var InstType = document.getElementById('hdn_CashBankType_InstType').value;

    var ProjectCode = clookup_Project.GetText();
    if ($("#hdnProjectSelectInEntryModule").val() == "1" && $("#hdnProjectMandatory").val() == "1" && ProjectCode == "" && $("#hdnIsTdsProj").val() == "0") {
        jAlert("Please Select Project.");
        return false;
    }


    if (document.getElementById('txtVoucherNo').value.trim() == "") {
        $("#MandatoryBillNo").show();
        return false;
    }
    else if (Branch == null) {
        $("#MandatoryBranch").show();
        return false;
    }
    else if (CashBank == null) {
        $("#MandatoryCashBank").show();
        return false;
    }
    else if (InstrumentType == "NA") {
        $("#MandatoryInstrumentType").show();
        return false;
    }
    else if (InstType == "Yes" && InstrumentType == "C") {
        if (InstrumentNo == null) {
            $("#MandatoryInstNo").show();
            return false;
        }
    }
    if (PaidToYesNO == "Yes" && VoucherType == "P") {
        var strPaidTo = ctxtPaidTo.GetText().trim();
        if (strPaidTo == "") {
            $("#MandatoryPaidTo").show();
            return false;
        }
    }
    if ($('#hdnInstrumentNo').val() == "") {
        $('#hdnInstrumentNo').val(InstrumentNo);
    }
    //InsgridBatch.UpdateEdit();
    InsgridBatch.batchEditApi.EndEdit();
    var gridCount = InsgridBatch.GetVisibleRowsOnPage();
    //var VoucherType = cComboType.GetValue();

    var txtTotalAmount = c_txt_Debit.GetValue() != null ? c_txt_Debit.GetValue() : 0;
    var txtTotalPayment = ctxtTotalPayment.GetValue() != null ? ctxtTotalPayment.GetValue() : 0;
    var IsType = "";
    var frontRow = 0;
    var backRow = -1;

    for (var i = 0; i <= InsgridBatch.GetVisibleRowsOnPage() ; i++) {
        var frontProduct = (InsgridBatch.batchEditApi.GetCellValue(backRow, 'MainAccount') != null) ? (InsgridBatch.batchEditApi.GetCellValue(backRow, 'MainAccount')) : "";
        var backProduct = (InsgridBatch.batchEditApi.GetCellValue(frontRow, 'MainAccount') != null) ? (InsgridBatch.batchEditApi.GetCellValue(frontRow, 'MainAccount')) : "";
        if (frontProduct != "" || backProduct != "") {
            
            IsType = "Y";
            break;
        }
        backRow--;
        frontRow++;
    }   
  
    if (gridCount > 0) {
        // if (chkAccount == 1) {
        if (IsType == "Y") {
            if (VoucherType == "P") {
                if (parseFloat(txtTotalAmount) <= parseFloat(txtTotalPayment)) {
                    //OnAddNewClick();
                    //cbtnSaveNew.SetVisible(false);
                    //cbtnSaveRecords.SetVisible(false);


                    //document.getElementById('btnSaveNew').style.display = 'block';
                    // document.getElementById('btnSaveRecords').style.display = 'block';
                    //SaveTDS();

                    var urlKeys = getUrlVars();

                    var CashBankVoucherID;
                    if (urlKeys.key != 'ADD') {
                        CashBankVoucherID = urlKeys.key;
                    }
                    else {
                        CashBankVoucherID = 0;
                    }
                    var PostingDate = GetServerDateFormat(cdtTDate.GetValue());

                    $.ajax({
                        type: "POST",
                        url: "CashBankEntry.aspx/GetTotalBalanceByCashBankID",
                        data: JSON.stringify({ CashBankID: CashBank, CashBankVoucherID: CashBankVoucherID, PostingDate: PostingDate }),
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
                                //   debugger;
                                var TotalVoucherAmount = parseFloat(txtTotalPayment) - parseFloat(txtTotalAmount);
                                var closingAmount = parseFloat(closingBalnc) - parseFloat(TotalVoucherAmount);
                                if (parseFloat(closingAmount) < parseFloat(BalanceLimit)) {

                                    if (BalanceExceed.trim() == 'W') {

                                        jConfirm('Cash/Bank Balance - Limit is exceed do you want to proceed?', 'Confirmation Dialog', function (r) {
                                            if (r == true) {
                                                OnAddNewClick();
                                                cbtnSaveNew.SetVisible(false);
                                                cbtnSaveRecords.SetVisible(false);


                                                InsgridBatch.UpdateEdit();
                                                chkAccount = 0;
                                            }
                                            else {

                                            }
                                        });
                                    }
                                    else if (BalanceExceed.trim() == 'B') {
                                        jAlert('Cash/Bank Balance - Limit is exceed can not proceed');

                                    }
                                    else if (BalanceExceed.trim() == 'S') {
                                        //OnAddNewClick();
                                        //cbtnSaveNew.SetVisible(false);
                                        //cbtnSaveRecords.SetVisible(false);

                                        //InsgridBatch.UpdateEdit();
                                        //chkAccount = 0;
                                        jAlert('Please select Cash/Bank Balance - Limit exceed option.');
                                    }
                                    else if (BalanceExceed.trim() == 'I') {
                                        OnAddNewClick();
                                        cbtnSaveNew.SetVisible(false);
                                        cbtnSaveRecords.SetVisible(false);

                                        InsgridBatch.UpdateEdit();
                                        chkAccount = 0;
                                    }
                                }
                                else {
                                    OnAddNewClick();
                                    cbtnSaveNew.SetVisible(false);
                                    cbtnSaveRecords.SetVisible(false);

                                    InsgridBatch.UpdateEdit();
                                    chkAccount = 0;
                                }
                            }
                            else {
                                OnAddNewClick();
                                cbtnSaveNew.SetVisible(false);
                                cbtnSaveRecords.SetVisible(false);

                                InsgridBatch.UpdateEdit();
                                chkAccount = 0;
                            }

                        }
                    });



                    //InsgridBatch.UpdateEdit();
                    //chkAccount = 0;
                }
                else {
                    chkAccount = 1;
                    jAlert('As per the selcted Voucher type, Payment column amount should be greater than Receipt column amount.');
                }
            }
            if (VoucherType == "R") {
                if (parseFloat(txtTotalAmount) >= parseFloat(txtTotalPayment)) {
                    OnAddNewClick();
                    cbtnSaveNew.SetVisible(false);
                    cbtnSaveRecords.SetVisible(false);
                    //document.getElementById('btnSaveNew').style.display = 'block';
                    // document.getElementById('btnSaveRecords').style.display = 'block';
                    //SaveTDS();
                    InsgridBatch.UpdateEdit();
                    chkAccount = 0;
                }
                else {
                    chkAccount = 1;
                    jAlert('As per the selcted Voucher type, Receipt column amount should be greater than Payment column amount.');
                }
            }
        }
        else {
            //chkAccount = 0;
            jAlert('Cannot Save. You must enter atleast one Record to save this entry.');
        }
    }
    else {
        //chkAccount = 0;
        jAlert('Cannot Save. You must enter atleast one Record to save this entry.');
    }
}
function txtBillNo_TextChanged() {
    var VoucherNo = document.getElementById("txtVoucherNo").value;
    $.ajax({
        type: "POST",
        url: "CashBankEntry.aspx/CheckUniqueName",
        data: JSON.stringify({ VoucherNo: VoucherNo }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var data = msg.d;
            if (data == true) {
                $("#MandatoryBillNo").show();
                document.getElementById("txtVoucherNo").value = '';
                document.getElementById("txtVoucherNo").focus();
            }
            else {
                $("#MandatoryBillNo").hide();
            }
        }
    });
}
function SaveButtonClickNew() {

    $('#hdnBtnClick').val("Save_New");
    $('#hdnJNMode').val('0'); //Entry  
    $('#hdnRefreshType').val('S');
    $('#hdfIsDelete').val('I');
    $('#hdnPayment').val('NO');
    $('#hdnTaxGridBind').val('NO');
    var VoucherType = document.getElementById('rbtnType').value;
    $("#hdnTDSSection").val(TDSSection);
    var Branch = $('#ddlBranch').val();
    var CashBank = cddlCashBank.GetValue();
    var InstrumentType = cComboInstrumentTypee.GetValue();
    var InstrumentNo = ctxtInstNobth.GetValue();
    var CashBankId = cddlCashBank.GetValue();
    var InstType = document.getElementById('hdn_CashBankType_InstType').value;
    var PaidToYesNO = document.getElementById('hdnPaidToYesNO').value;

    var ProjectCode = clookup_Project.GetText();
    if ($("#hdnProjectSelectInEntryModule").val() == "1" && $("#hdnProjectMandatory").val() == "1" && ProjectCode == "" && $("#hdnIsTdsProj").val() == "0") {
        jAlert("Please Select Project.");
        return false;
    }


    if (document.getElementById('txtVoucherNo').value.trim() == "") {
        $("#MandatoryBillNo").show();

        return false;
    }
    else if (Branch == null) {
        $("#MandatoryBranch").show();
        return false;
    }
    else if (CashBank == null) {
        $("#MandatoryCashBank").show();
        return false;
    }
    else if (InstrumentType == "NA") {
        $("#MandatoryInstrumentType").show();
        return false;
    }
    else if (InstType == "Yes" && InstrumentType == "C") {
        if (InstrumentNo == null) {
            $("#MandatoryInstNo").show();
            return false;
        }
    }
    if (PaidToYesNO == "Yes" && VoucherType == "P") {
        var strPaidTo = ctxtPaidTo.GetText().trim();
        if (strPaidTo == "") {
            $("#MandatoryPaidTo").show();
            return false;
        }

    }
    if ($('#hdnInstrumentNo').val() == "") {
        $('#hdnInstrumentNo').val(InstrumentNo);
    }
    var CashBankText = cddlCashBank.GetText();
    // var SpliteDetails = CashBankText.split(']');
    // var WithDrawType = SpliteDetails[1].trim();
    var WithDrawType = "";
    if (InstrumentType == "C") {
        WithDrawType = "Cheque";
    }
    else if (InstrumentType == "E") {
        WithDrawType = "E.Transfer";
    }
    else if (InstrumentType == "D") {
        WithDrawType = "Draft";
    }
    else if (InstrumentType == "CH") {
        WithDrawType = "Cash";
    }
    WithdrawalChangedNew(WithDrawType);
    cComboInstrumentTypee.SetValue(InstrumentType);
    InstrumentTypeSelectedIndexChanged();
    //Code added by Sudip
    InsgridBatch.batchEditApi.EndEdit();
    var gridCount = InsgridBatch.GetVisibleRowsOnPage();

    var txtTotalAmount = c_txt_Debit.GetValue() != null ? c_txt_Debit.GetValue() : 0;
    var txtTotalPayment = ctxtTotalPayment.GetValue() != null ? ctxtTotalPayment.GetValue() : 0;
    // var VoucherType = cComboType.GetValue();

    var IsType = "";
    var frontRow = 0;
    var backRow = -1;

    for (var i = 0; i <= InsgridBatch.GetVisibleRowsOnPage() ; i++) {
        var frontProduct = (InsgridBatch.batchEditApi.GetCellValue(backRow, 'MainAccount') != null) ? (InsgridBatch.batchEditApi.GetCellValue(backRow, 'MainAccount')) : "";
        var backProduct = (InsgridBatch.batchEditApi.GetCellValue(frontRow, 'MainAccount') != null) ? (InsgridBatch.batchEditApi.GetCellValue(frontRow, 'MainAccount')) : "";
        if (frontProduct != "" || backProduct != "") {
            IsType = "Y";
            break;
        }
        backRow--;
        frontRow++;
    }
    if (gridCount > 0) {
        // if (chkAccount == 1) {
        if (IsType == "Y") {
            if (VoucherType == "P") {
                if (parseFloat(txtTotalAmount) <= parseFloat(txtTotalPayment)) {
                    //OnAddNewClick();
                    //cbtnSaveNew.SetVisible(false);
                    //cbtnSaveRecords.SetVisible(false);
                    //document.getElementById('btnSaveNew').style.display = 'block';
                    //document.getElementById('btnSaveRecords').style.display = 'block';
                    //SaveTDS();

                    var urlKeys = getUrlVars();
                    var CashBankVoucherID;
                    if (urlKeys.key != 'ADD') {
                        CashBankVoucherID = urlKeys.key;
                    }
                    else {
                        CashBankVoucherID = 0;
                    }
                    var PostingDate = GetServerDateFormat(cdtTDate.GetValue());

                    $.ajax({
                        type: "POST",
                        url: "CashBankEntry.aspx/GetTotalBalanceByCashBankID",
                        data: JSON.stringify({ CashBankID: CashBank, CashBankVoucherID: CashBankVoucherID, PostingDate: PostingDate }),
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
                                var TotalVoucherAmount = parseFloat(txtTotalPayment) - parseFloat(txtTotalAmount);
                                var closingAmount = parseFloat(closingBalnc) - parseFloat(TotalVoucherAmount);
                                if (parseFloat(closingAmount) < parseFloat(BalanceLimit)) {
                                    // if (parseFloat(TotalVoucherAmount) > parseFloat(BalanceLimit)) {

                                    if (BalanceExceed.trim() == 'W') {

                                        jConfirm('Cash/Bank Balance - Limit is exceed do you want to proceed?', 'Confirmation Dialog', function (r) {
                                            if (r == true) {
                                                OnAddNewClick();
                                                cbtnSaveNew.SetVisible(false);
                                                cbtnSaveRecords.SetVisible(false);


                                                InsgridBatch.UpdateEdit();
                                                chkAccount = 0;
                                            }
                                            else {

                                            }
                                        });
                                    }
                                    else if (BalanceExceed.trim() == 'B') {
                                        jAlert('Cash/Bank Balance - Limit is exceed can not proceed');

                                    }
                                    else if (BalanceExceed.trim() == 'S') {
                                        //OnAddNewClick();
                                        //cbtnSaveNew.SetVisible(false);
                                        //cbtnSaveRecords.SetVisible(false);

                                        //InsgridBatch.UpdateEdit();
                                        //chkAccount = 0;
                                        jAlert('Please select Cash/Bank Balance - Limit exceed option.');

                                    }
                                    else if (BalanceExceed.trim() == 'I') {
                                        OnAddNewClick();
                                        cbtnSaveNew.SetVisible(false);
                                        cbtnSaveRecords.SetVisible(false);

                                        InsgridBatch.UpdateEdit();
                                        chkAccount = 0;

                                    }
                                }
                                else {
                                    OnAddNewClick();
                                    cbtnSaveNew.SetVisible(false);
                                    cbtnSaveRecords.SetVisible(false);

                                    InsgridBatch.UpdateEdit();
                                    chkAccount = 0;
                                }
                            }
                            else {
                                OnAddNewClick();
                                cbtnSaveNew.SetVisible(false);
                                cbtnSaveRecords.SetVisible(false);

                                InsgridBatch.UpdateEdit();
                                chkAccount = 0;
                            }

                        }
                    });








                    //InsgridBatch.UpdateEdit();
                    //chkAccount = 0;
                }
                else {
                    chkAccount = 1;
                    jAlert('Payment amount can not be less than receipt amount ');

                }
            }


            if (VoucherType == "R") {
                if (parseFloat(txtTotalAmount) >= parseFloat(txtTotalPayment)) {
                    OnAddNewClick();
                    cbtnSaveNew.SetVisible(false);
                    cbtnSaveRecords.SetVisible(false);
                    //document.getElementById('btnSaveNew').style.display = 'block';
                    //document.getElementById('btnSaveRecords').style.display = 'block';

                    //SaveTDS();

                    InsgridBatch.UpdateEdit();
                    chkAccount = 0;
                }
                else {
                    chkAccount = 1;
                    jAlert('Receipt amount can not be less than payment amount');

                }
            }
        }
        else {
            // chkAccount = 0;
            jAlert('Cannot Save. You must enter atleast one Record to save this entry.');
        }
    }
    else {
        //chkAccount = 0;
        jAlert('Cannot Save. You must enter atleast one Record to save this entry.');
    }
}


function SaveTDS() {
    // debugger;
    myTableArray = [];

    $("#tbltdsDetails tr").each(function () {
        var arrayOfThisRow = [];
        var tableData = $(this).find('td');
        if (tableData.length > 0) {
            //  debugger;
            var id = $(tableData[0]).html().trim();
            var Myobj = {

                DETID: $(tableData[0]).html().trim(),
                VendorId: $(tableData[0]).html().trim(),
                MainAccountID: $(tableData[0]).html().trim(),
                PartyID: $(tableData[0]).html().trim(),
                TDSTCS_Code: $(tableData[0]).html().trim(),
                PaymentDate: $(tableData[0]).html().trim(),
                Total_Tax: $(tableData[0]).html().trim(),
                Tax_Amount: $(tableData[0]).html().trim(),
                Surcharge: $(tableData[0]).html().trim(),
                EduCess: $(tableData[0]).html().trim(),
                IsOpening: $(tableData[0]).html().trim(),

            };


            myTableArray.push(Myobj);
        }
    });

    var NewJSONstr = JSON.stringify(myTableArray);

    $("#hdnTDSData").val(NewJSONstr);

    console.log(myTableArray);
}




function GvCBSearch_EndCallBack() {
    if (cGvCBSearch.cpDelete != null) {
        jAlert(cGvCBSearch.cpDelete);
        cGvCBSearch.cpDelete = null;
    }
}
function CustomButtonClick(s, e) {
    if (e.buttonID == 'CustomBtnEdit') {
        s.GetRowValues(e.visibleIndex, 'ValueDate', function (value) {
            if (value != null)
            { jAlert("Voucher is Reconciled.Cannot Edit"); }
            else {
                $('#hdnEditClick').val('T'); //Edit
                $('#hdnJNMode').val('1');//Edit
                VisibleIndexE = e.visibleIndex;
                $('#lblHeading').text("Modify Cash/Bank Voucher");
                document.getElementById('DivEntry').style.display = 'block';
                document.getElementById('divExportto').style.display = 'none';
                document.getElementById('divAddBBindGridViaTDSDatautton').style.display = 'none';
                document.getElementById('gridFilter').style.display = 'none';
                document.getElementById('DivEdit').style.display = 'none';
                btncross.style.display = "block";
                $('#hdn_Mode').val('Edit');
                //InsgridBatch.PerformCallback("Edit~" + VisibleIndexE);
                cASPxCallbackGeneral.PerformCallback("Edit~" + VisibleIndexE);
                LoadingPanel.Show();
                chkAccount = 1;
                document.getElementById('divNumberingScheme').style.display = 'none';
                document.getElementById('divEnterBranch').style.display = 'Block';
            }
        });
    }
    else if (e.buttonID == 'CustomBtnView') {
        $('#hdnEditClick').val('T'); //Edit
        $('#hdnJNMode').val('1');//Edit
        VisibleIndexE = e.visibleIndex;
        $('#lblHeading').text("View Cash/Bank Voucher");
        document.getElementById('DivEntry').style.display = 'block';
        document.getElementById('divExportto').style.display = 'none';
        document.getElementById('divAddButton').style.display = 'none';
        document.getElementById('gridFilter').style.display = 'none';
        document.getElementById('DivEdit').style.display = 'none';
        btncross.style.display = "block";
        $('#hdn_Mode').val('View');
        //InsgridBatch.PerformCallback("View~" + VisibleIndexE);
        cASPxCallbackGeneral.PerformCallback("View~" + VisibleIndexE);
        LoadingPanel.Show();
        chkAccount = 1;
        document.getElementById('divNumberingScheme').style.display = 'none';
    }
    else if (e.buttonID == 'CustomBtnDelete') {
        jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
            if (r == true) {
                VisibleIndexE = e.visibleIndex;
                cGvCBSearch.PerformCallback("Delete~" + VisibleIndexE);
            }
            else {
                return false;
            }
        });
    }
    else if (e.buttonID == 'CustomBtnPrint') {
        var keyValueindex = s.GetRowKey(e.visibleIndex);
        onPrintJv(keyValueindex);
    }
}

//function onPrintJv(id) {

//    RecPayId = id;
//    cDocumentsPopup.Show();
//    cCmbDesignName.SetSelectedIndex(0);
//    cSelectPanel.PerformCallback('Bindalldesignes');
//    $('#btnOK').focus();
//}

//function PerformCallToGridBind() {
//    cSelectPanel.PerformCallback('Bindsingledesign');
//    cDocumentsPopup.Hide();
//    return false;
//}

//function cSelectPanelEndCall(s, e) {

//    if (cSelectPanel.cpSuccess != "") {
//        var TotDocument = cSelectPanel.cpSuccess.split(',');
//        var reportName = cCmbDesignName.GetValue();
//        var module = 'CBVUCHR';
//        window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + RecPayId, '_blank')
//    }
//    if (cSelectPanel.cpSuccess == "") {
//        cCmbDesignName.SetSelectedIndex(0);
//    }
//}

////##### coded by Samrat Roy - 14/04/2017 - ref IssueLog(Voucher - 110) 
////This method is for disable all the attributes.
function viewOnly() {
    if ($('#hdn_Mode').val().toUpperCase() == 'VIEW') {
        $('#DivEntry').find('input, textarea, button, select').attr('disabled', 'disabled');
        InsgridBatch.SetEnabled(false);
        cddlCashBank.SetEnabled(false);
        // cComboType.SetEnabled(false);
        $("#rbtnType").attr("disabled", "disabled");
        cdtTDate.SetEnabled(false);
        cCmbCurrency.SetEnabled(false);
        ctxtRate.SetEnabled(false);
        cComboInstrumentTypee.SetEnabled(false);
        ctxtInstNobth.SetEnabled(false);
        cInstDate.SetEnabled(false);
        ctxtReceivedFrom.SetEnabled(false);
        ctxtPaidTo.SetEnabled(false);
        cddl_AmountAre.SetEnabled(false);
        cbtnSaveNew.SetVisible(false);
        cbtnSaveRecords.SetVisible(false);
        //document.getElementById('btnSaveNew').style.display = 'block';
        //document.getElementById('btnSaveRecords').style.display = 'block';
    }
    LoadingPanel.Hide();
}

function RecieveTextChange(s, e) {
    var RecieveValue = (InsgridBatch.GetEditor('btnRecieve').GetValue() != null) ? parseFloat(InsgridBatch.GetEditor('btnRecieve').GetValue()) : "0";
    var PaymentValue = (InsgridBatch.GetEditor('btnPayment').GetValue() != null) ? InsgridBatch.GetEditor('btnPayment').GetValue() : "0";
    if (RecieveValue > 0) {
        recalculateCredit(InsgridBatch.GetEditor('btnPayment').GetValue());
        InsgridBatch.GetEditor('btnPayment').SetValue("0");
    }
}







//<%--Added By : Samrat Roy -- New Billing/Shipping Section--%>

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
function disp_prompt(name) {


    if (name == "tab0") {
        // gridLookup.Focus();
    }
    if (name == "tab1") {

    }
}
function getParameterByName(name, url) {
    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}


function lookup_ProjectCodeKeyDown(s, e) {
    if (e.htmlEvent.key == "Escape") {
        cProjectCodePopup.Hide();

    }
}


function ProjectCodeinlineSelectedPayment(s, e) {
    if ($("#hdnProjectSelectInEntryModule").val() == "1") {

        if ((clookup_Project.GetGridView().GetRowKey(clookup_Project.GetGridView().GetFocusedRowIndex()) != null)) {
            //cProjectCodeCallback.PerformCallback('ProjectId~'+(clookup_Project.GetGridView().GetRowKey(clookup_Project.GetGridView().GetFocusedRowIndex())));
        }
    }
}


function ProjectCodeButnClick(s, e) {
    if (e.buttonIndex == 0) {
        if ($("#hdnAllowProjectInDetailsLevel").val() != "0") {
            clookupPopup_ProjectCode.Clear();


            if (clookupPopup_ProjectCode.Clear()) {
                cProjectCodePopup.Show();
                clookupPopup_ProjectCode.Focus();
            }
            //cProjectCodeCallback.PerformCallback('Type~' + Type + "~" + InvoiceNo);

            if ($("#hdnProjectSelectInEntryModule").val() == "1") {
                if ((clookup_Project.GetGridView().GetRowKey(clookup_Project.GetGridView().GetFocusedRowIndex()) != null)) {
                    cProjectCodeCallback.PerformCallback('ProjectId~' + (clookup_Project.GetGridView().GetRowKey(clookup_Project.GetGridView().GetFocusedRowIndex())));
                }
                else {
                    cProjectCodeCallback.PerformCallback('ProjectId~' + "0");
                }
            }
            else {
                cProjectCodeCallback.PerformCallback('ProjectId~' + "0");
            }
        }
    }
}


function ProjectCodeKeyDown(s, e) {


    if (e.htmlEvent.key == "Enter" || e.htmlEvent.key == "NumpadEnter") {


        s.OnButtonClick(0);
    }
}



function ProjectCodeSelected(s, e) {
    //  debugger;
    if (clookupPopup_ProjectCode.GetGridView().GetFocusedRowIndex() == -1) {
        cProjectCodePopup.Hide();

        return;
    }
    var ProjectInlineLookUpData = clookupPopup_ProjectCode.GetGridView().GetRowKey(clookupPopup_ProjectCode.GetGridView().GetFocusedRowIndex());
    var ProjectInlinedata = ProjectInlineLookUpData.split('~')[0];
    InsgridBatch.batchEditApi.StartEdit(globalRowIndex);
    var ProjectCode = clookupPopup_ProjectCode.GetValue();
    cProjectCodePopup.Hide();

    InsgridBatch.GetEditor("Project_Code").SetText(ProjectCode);
    InsgridBatch.GetEditor("ProjectId").SetText(ProjectInlinedata);

}

function ProjectCodeCallback_endcallback() {

    clookupPopup_ProjectCode.ShowDropDown();;
    clookupPopup_ProjectCode.Focus();
    clookupPopup_ProjectCode.Clear()

}



function TaxAmountKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter") {
        s.OnButtonClick(0);
    }
}
function taxAmtButnClick1(s, e) {
    rowEditCtrl = s;
}
function taxAmtButnClick(s, e) {
    var TaxAmountOngrid = InsgridBatch.GetEditor("TaxAmount").GetValue();
    $("#TaxAmountOngrid").val(TaxAmountOngrid);
    $("#VisibleIndexForTax").val(globalRowIndex);
    if (e.buttonIndex == 0) {
        InsgridBatch.batchEditApi.StartEdit(e.visibleIndex);
        var shippingStCode = '';
        //chinmoy commeented for lightweight billingshiping
        //shippingStCode = cbsSCmbState.GetText(); 
        shippingStCode = GeteShippingStateCode();
        var VoucherType = document.getElementById('rbtnType').value;
        document.getElementById('HdSerialNo1').value = InsgridBatch.GetEditor('SrlNo').GetText();
        if (cddl_AmountAre.GetValue() != '3') {
            if (shippingStCode != '') {
                //if (cComboType.GetValue() == "P") {
                if (VoucherType == "P") {
                    if (InsgridBatch.GetEditor("ReverseApplicable").GetValue() == "0") {
                        showTax();
                    }
                    else {
                        if (cddl_AmountAre.GetValue() == '1') {
                            jAlert("GST is applicable under Reverse Charge.", "Alert!!", function () {
                                InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 7);
                            });
                        }
                        else if (cddl_AmountAre.GetValue() == '2') {
                            showTax();
                        }
                    }
                }
                    //else if (cComboType.GetValue() == "R") {
                else if (VoucherType == "R") {
                    showTax();
                }
            }
            else {
                //if (cComboType.GetValue() == "P") {
                if (VoucherType == "P") {
                    if (InsgridBatch.GetEditor("ReverseApplicable").GetValue() == "0" && $("#IsTaxApplicable").val() != "" && $("#IsTaxApplicable").val() != null) {
                        jAlert("Please Enter Billing/Shipping Details to Calculate GST.", "Alert !!", function () {
                            page.SetActiveTabIndex(1);
                            //chinmoy edited for new billing shipping
                            //  cbsSave_BillingShipping.Focus();
                            cbtnSave_SalesBillingShiping.Focus();
                            page.tabs[0].SetEnabled(false);
                            $("#divcross").hide();
                        });
                    }
                }
                    //else if (cComboType.GetValue() == "R") {
                else if (VoucherType == "R") {
                    if ($("#IsTaxApplicable").val() != "" && $("#IsTaxApplicable").val() != null) {
                        jAlert("Please Enter Billing/Shipping Details to Calculate GST.", "Alert !!", function () {
                            page.SetActiveTabIndex(1);
                            //chinmoy edited for new billing shipping
                            // cbsSave_BillingShipping.Focus();
                            cbtnSave_SalesBillingShiping.Focus();
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
        else {
            jAlert("No Tax is applicable.", "Alert!!", function () {
                InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 7);
            });
        }
    }
}

$(function () {
    $("#chk_reversemechenism").prop("disabled", false);
    $("#chk_reversemechenism").on("change", function () {
        if ($("#chk_reversemechenism").prop("checked") == true) {

        }
        else {

        }
    })
});

//Function for Date wise filteration
//function updateGridByDate() {

//    if (cFormDate.GetDate() == null) {
//        jAlert('Please select from date.', 'Alert', function () { cFormDate.Focus(); });
//    }
//    else if (ctoDate.GetDate() == null) {
//        jAlert('Please select to date.', 'Alert', function () { ctoDate.Focus(); });
//    }
//    else if (ccmbBranchfilter.GetValue() == null) {
//        jAlert('Please select Branch.', 'Alert', function () { ccmbBranchfilter.Focus(); });
//    }
//    else {
//        localStorage.setItem("FromDateCashBank", cFormDate.GetDate().format('yyyy-MM-dd'));
//        localStorage.setItem("ToDateCashBank", ctoDate.GetDate().format('yyyy-MM-dd'));
//        localStorage.setItem("BranchCashBank", ccmbBranchfilter.GetValue());
//        cGvCBSearch.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue());
//    }
//}

$(document).ready(function () {
    //Toggle fullscreen expandEntryGrid



    $("#expandInsgridBatch").click(function (e) {
        e.preventDefault();
        var $this = $(this);
        if ($this.children('i').hasClass('fa-expand')) {
            $this.removeClass('hovered half').addClass('full');
            $this.attr('title', 'Minimize Grid');
            $this.children('i').removeClass('fa-expand');
            $this.children('i').addClass('fa-arrows-alt');
            var gridId = $(this).attr('data-instance');
            $(this).closest('.makeFullscreen').addClass('panel-fullscreen');
            var cntWidth = $(this).parent('.makeFullscreen').width();
            var browserHeight = document.documentElement.clientHeight;
            var browserWidth = document.documentElement.clientWidth;
            InsgridBatch.SetHeight(browserHeight - 150);
            InsgridBatch.SetWidth(cntWidth);
        }
        else if ($this.children('i').hasClass('fa-arrows-alt')) {
            $this.children('i').removeClass('fa-arrows-alt');
            $this.removeClass('full').addClass('hovered half');
            $this.attr('title', 'Maximize Grid');
            $this.children('i').addClass('fa-expand');
            var gridId = $(this).attr('data-instance');
            $(this).closest('.makeFullscreen').removeClass('panel-fullscreen');
            var browserHeight = document.documentElement.clientHeight;
            var browserWidth = document.documentElement.clientWidth;
            InsgridBatch.SetHeight(200);
            var cntWidth = $this.parent('.makeFullscreen').width();
            InsgridBatch.SetWidth(cntWidth);
        }
    });
});

$(document).ready(function () {

    $('.HtmlGrid > tbody > tr').click(function () {
        $('.HtmlGrid>tbody>tr').removeClass('hovered');
        $(this).addClass('hovered');
    });
});
