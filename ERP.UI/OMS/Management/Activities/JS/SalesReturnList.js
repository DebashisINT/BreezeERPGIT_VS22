
var ReturnId = 0;
var JVid = "";
var Action = "Add";
var AutoJVNumber = "";
function onPrintJv(id) {
   
    var ActiveEInvoice = $('#hdnActiveEInvoice').val();
    if (ActiveEInvoice == "1") {
        $.ajax({
            type: "POST",
            url: "SalesReturnList.aspx/Prc_EInvoiceChecking_details",
            data: "{'returnid':'" + id + "','mode':'Print'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (msg) {
                var status = msg.d;
                if (status == "Yes") {
                    ReturnId = id;
                    cSelectPanel.cpSuccess = "";
                    cDocumentsPopup.Show();
                    cCmbDesignName.SetSelectedIndex(0);
                    cSelectPanel.PerformCallback('Bindalldesignes');
                    $('#btnOK').focus();
                }
                else {
                    jAlert("IRN generation is still pending. Cannot take print.");
                }
            }
        });
    }
    else {
        ReturnId = id;
        cSelectPanel.cpSuccess = "";
        cDocumentsPopup.Show();
        cCmbDesignName.SetSelectedIndex(0);
        cSelectPanel.PerformCallback('Bindalldesignes');
        $('#btnOK').focus();
    }    
}

function onInfluencerCommissionReturn(id) {
    $("#CmbScheme").val('0');
    $("#txtBillNo").val("")
    $("#txtBillNo").prop('disabled', true)
    $.ajax({
        type: "POST",
        url: "SalesReturnList.aspx/GetInfluencerDetails",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ invid: id }),
        dataType: "json",
        success: function (msg) {
            var status = msg.d;
            var str = "";
            if (parseInt(status.RemainingBalance) > 0) {

                if (status.INF_Inv_Details) {
                    $("#ddlBranch").val(status.INF_Inv_Details.Inv_BranchId);
                    $("#invid").val(status.INF_Inv_Details.Inv_Id);
                    $("#txtInvoiceNumber").html(status.INF_Inv_Details.Inv_No);
                    $("#txtInvoiceAmount").html(status.INF_Inv_Details.Amount);



                }

                cbtnMAdr.SetText(status.Influencer.MainAccount_AccountCode);
                $("#txtCommAmt").html(status.Influencer.COMM_AMOUNT);
                $("#mainacdrid").val(status.Influencer.MAINACCOUNT_DR);

                if (status.Influencer.AUTOJV_NUMBER != "" && status.Influencer.AUTOJV_NUMBER != null) {
                    $("#div_Edit").addClass('hide');
                    AutoJVNumber = status.Influencer.AUTOJV_NUMBER
                    $("#txtBillNo").val(status.Influencer.AUTOJV_NUMBER);
                    $("#txtBillNo").prop('disabled', true);


                    if (parseInt(status.Influencer.IsTagged) > 0) {
                        $("#divSaveinf").addClass('hide')
                        $("#divDeleteinf").addClass('hide')
                        $("#divmsg").removeClass('hide')
                    }
                    else {
                        $("#divmsg").addClass('hide')
                        $("#divSaveinf").removeClass('hide')
                        $("#divDeleteinf").removeClass('hide')
                    }

                    tDate.SetDate(new Date(parseInt(status.Influencer.POSTING_DATE.substr(6))));
                    tDate.SetEnabled(false);
                    Action = "Edit";
                }
                else {
                    $("#div_Edit").removeClass('hide');

                    $("#txtBillNo").prop('disabled', false);
                    tDate.SetEnabled(true);
                    $("#divDeleteinf").addClass('hide')
                    Action = "Add";
                }
                str = "<thead>";
                str = str + "<tr>";
                str = str + "<th class='hide'>INFID</th>";
                str = str + "<th class='hide'>INFMAID</th>";
                str = str + "<th style='width:200px;'>Liability Ledger</th>";
                str = str + "<th>Influencer Name</th>";
                str = str + "<th style='width:150px;'>Amount</th>";
                //str = str + "<th>Action</th>";
                str = str + "</tr>";
                str = str + "</thead>";
                str = str + "<tbody id='infbody'>";
                for (var i = 0; i < status.Influencer_Details.length; i++) {
                    str = str + "<tr id='tr" + status.Influencer_Details[i].DET_INFLUENCER_ID + "'>";
                    str = str + "<td id='infid" + status.Influencer_Details[i].DET_INFLUENCER_ID + "' class='hide'>" + status.Influencer_Details[i].DET_INFLUENCER_ID + "</td>";
                    str = str + "<td id='infmaid" + status.Influencer_Details[i].DET_INFLUENCER_ID + "' class='hide' >" + status.Influencer_Details[i].DET_MAINACCOUNT_CR + "</td>";
                    str = str + "<td id='infmaname" + status.Influencer_Details[i].DET_INFLUENCER_ID + "' >" + status.Influencer_Details[i].DET_MAINACCOUNT_NAME + "</td>";
                    str = str + "<td id='infname" + status.Influencer_Details[i].DET_INFLUENCER_ID + "' >" + status.Influencer_Details[i].INF_Name + "</td>";

                    if (status.Influencer_Details.length > 1)
                        str = str + "<td id='infamt" + status.Influencer_Details[i].DET_INFLUENCER_ID + "' ><input  type='text' id='txtinfamt" + status.Influencer_Details[i].DET_INFLUENCER_ID + "' value='" + status.Influencer_Details[i].DET_AMOUNT_CR + "'  /></td>";
                    else
                        str = str + "<td id='infamt" + status.Influencer_Details[i].DET_INFLUENCER_ID + "' ><input  type='text' id='txtinfamt" + status.Influencer_Details[i].DET_INFLUENCER_ID + "' value='" + status.Influencer_Details[i].DET_AMOUNT_CR + "' disabled /></td>";


                    //str = str + "<td><img onclick='infdeleteClick(" + JSON.stringify(status.Influencer_Details[i].DET_INFLUENCER_ID) + ")' src='../../../assests/images/crs.png' /></td>";
                    str = str + "</tr>";
                }
                str = str + "</tbody>";
                $("#influencerGrid").html('');
                $("#influencerGrid").html(str);

                str = "<thead><tr>";
                str = str + "<th class='hide'>Product Id</th>";
                str = str + "<th style='width:250px !important;'>Product Names</th>";
                str = str + "<th>Product Qty.</th>";
                str = str + "<th>Sales Price</th>";
                str = str + "<th>Amount(Before GST)</th>";
                str = str + "<th>Amount(With GST)";
                str = str + "<th class='hide'>prod det id</th>";
                //str = str + "<th>Basis</th>";
                //str = str + "<th>Commission Rate/Qty or Commission %</th>";
                str = str + "<th>Comm. Amount</th>";
                str = str + "</tr>";
                str = str + "</thead>";
                str = str + "<tbody id='prodbody'>";
                for (var i = 0; i < status.INF_Inv_Products.length; i++) {
                    str = str + "<tr class='detINF'>";
                    str = str + "<td id='prodid" + status.INF_Inv_Products[i].Prod_id + "' class='hide'>" + status.INF_Inv_Products[i].Prod_id + "</td>";
                    str = str + "<td id='proddesc" + status.INF_Inv_Products[i].Prod_id + "' style='width:200px !important;'>" + status.INF_Inv_Products[i].Prod_description + "</td>";
                    str = str + "<td id='prodqty" + status.INF_Inv_Products[i].Prod_id + "' >" + status.INF_Inv_Products[i].prod_Qty + "</td>";
                    str = str + "<td id='prodsp" + status.INF_Inv_Products[i].Prod_id + "' >" + status.INF_Inv_Products[i].prod_Salesprice + "</td>";
                    str = str + "<td id='proddetamt" + status.INF_Inv_Products[i].Prod_id + "' >" + status.INF_Inv_Products[i].prod_amt + "</td>";
                    str = str + "<td id='proddetamtgst" + status.INF_Inv_Products[i].Prod_id + "' >" + status.INF_Inv_Products[i].prod_SalespriceWithGST + "</td>";

                    str = str + "<td id='proddetid" + status.INF_Inv_Products[i].Prod_id + "' class='hide'>" + status.INF_Inv_Products[i].prod_details_id + "</td>";



                    if (parseFloat(status.INF_Inv_Products[i].prod_Qty) > 0) {
                        if (status.INF_Inv_Products[i].Applicable_On == "4") {
                            //str = str + "<td><input type='textbox' class='numeric'  onfocusout='percenLostfocus(" + status.INF_Inv_Products[i].Prod_id + ")' onkeypress='return isNumberKey(event)' id='prodpercen" + status.INF_Inv_Products[i].Prod_id + "' value='" + status.INF_Inv_Products[i].Prod_Percentage + "'disabled></td>";
                            str = str + "<td><input type='textbox' class='numeric'  onfocusout='AmountLostfocus(" + status.INF_Inv_Products[i].Prod_id + ")' onkeypress='return isNumberKey(event)' id='prodcommamt" + status.INF_Inv_Products[i].Prod_id + "' value='" + status.INF_Inv_Products[i].PROD_COMM_AMOUNT + "' ></td>";
                        }
                        else {
                            //str = str + "<td><input type='textbox' class='numeric'  onfocusout='percenLostfocus(" + status.INF_Inv_Products[i].Prod_id + ")' onkeypress='return isNumberKey(event)' id='prodpercen" + status.INF_Inv_Products[i].Prod_id + "' value='" + status.INF_Inv_Products[i].Prod_Percentage + "' ></td>";
                            str = str + "<td><input type='textbox' class='numeric'  onfocusout='AmountLostfocus(" + status.INF_Inv_Products[i].Prod_id + ")' onkeypress='return isNumberKey(event)' id='prodcommamt" + status.INF_Inv_Products[i].Prod_id + "' value='" + status.INF_Inv_Products[i].PROD_COMM_AMOUNT + "' disabled></td>";
                        }
                    }
                    else {
                        // str = str + "<td><input type='textbox' class='numeric'  onfocusout='percenLostfocus(" + status.INF_Inv_Products[i].Prod_id + ")' onkeypress='return isNumberKey(event)' id='prodpercen" + status.INF_Inv_Products[i].Prod_id + "' value='" + status.INF_Inv_Products[i].Prod_Percentage + "' disabled></td>";
                        str = str + "<td><input type='textbox' class='numeric'  onfocusout='AmountLostfocus(" + status.INF_Inv_Products[i].Prod_id + ")' onkeypress='return isNumberKey(event)' id='prodcommamt" + status.INF_Inv_Products[i].Prod_id + "' value='" + status.INF_Inv_Products[i].PROD_COMM_AMOUNT + "' disabled></td>";

                    }


                    str = str + "</tr>";


                    //if( parseFloat($('#prodqty' + status.INF_Inv_Products[i].Prod_id).val())==0){
                    //    $('#prodqty' + status.INF_Inv_Products[i].Prod_id).closest(".detINF").find('input, select').attr('disabled', true)
                    //}
                }
                str = str + "</tbody>";
                // $(".numeric").numeric({ decimal: ".", negative: false, scale: 3 });
                $("#tableProduct").html('');
                $("#tableProduct").html(str);



                //$('#tableProduct').DataTable({
                //    "iDisplayLength": 5,
                //    "searching": false,
                //    "lengthChange": false
                //});

                $("#myModal").modal('toggle');
            }
            else {
                jAlert('Influencer booking not found . Can not add Influencer.', 'Alert');
            }
        }

    });
}


function SaveInfluencerdetails() {
    var obj = {};
    obj.PostingDate = tDate.GetDate();
    obj.Schema = $("#CmbScheme").val();
    obj.Billno = $("#txtBillNo").val();
    obj.Branch = $("#ddlBranch").val();
    obj.Invoice_Id = $("#invid").val();


    obj.Mainaccr = $("#mainacdrid").val();



    var proobj = [];

    $("#tableProduct tr").each(function () {
        var arrayOfThisRow = [];
        var tableData = $(this).find('td');
        if (tableData.length > 0) {

            var id = $(tableData[0]).html().trim();
            var Myobj = {
                PRODID: $("#prodid" + id).html(),
                QTY: $("#prodqty" + id).html(),
                SALESPRICE: $("#prodsp" + id).html(),
                TOTALPRICE: $("#proddetamt" + id).html(),
                DETID: $("#proddetid" + id).html(),
                BASIS: $("#prodappl" + id).val(),
                PERSENTAGE: $("#prodpercen" + id).val(),
                AMOUNT: $("#prodcommamt" + id).val(),
            };


            proobj.push(Myobj);
        }
    });
    var myproobj = JSON.stringify(proobj);

    obj.product = proobj;


    myTableArray = [];

    $("#influencerGrid tr").each(function () {
        var arrayOfThisRow = [];
        var tableData = $(this).find('td');
        if (tableData.length > 0) {
            tableData.each(function () {
                arrayOfThisRow.push($(this).html());
            });
            myTableArray.push(arrayOfThisRow);
        }
    });
    //obj.Influencer = myTableArray;
    var totamount = 0;
    var Infobj = [];
    for (var i = 0; i <= myTableArray.length - 1; i++) {

        var Myobj = {
            INFID: myTableArray[i][0],
            MACRID: myTableArray[i][1],
            AMT: $("#txtinfamt" + myTableArray[i][0]).val()
        };
        totamount = totamount + DecimalRoundoff($("#txtinfamt" + myTableArray[i][0]).val(), 2)
        Infobj.push(Myobj);
    }

    if (totamount > DecimalRoundoff($("#txtCommAmt").html().trim(), 2)) {
        jAlert('The total of the Individual Commission must be less than Total Calculated Commission', 'Alert');
        return;
    }


    var myInfobj = JSON.stringify(Infobj);

    obj.Influencer = Infobj;

    obj.Action = Action;

    if (Infobj.length == 0) {

        jAlert('Please add atleast one Influencer to proceed.', 'Alert');
        return;
    }

    if (tDate.GetText() == "") {
        jAlert('Please select a valid date to proceed.', 'Alert');
        return;
    }

    if (AutoJVNumber != "" && AutoJVNumber != null) {

    }
    else {
        if ($("#CmbScheme").val() == "") {
            jAlert('Please select a valid schema to proceed.', 'Alert');
            return;
        }
        if ($("#CmbScheme").val() == "0") {
            jAlert('Please select a valid schema to proceed.', 'Alert');
            return;
        }

    }
    if (parseFloat($("#txtCommAmt").html().trim()) == 0) {
        jAlert('Please add amount to proceed.', 'Alert');
        return;
    }

    if (cbtnMAdr.GetText() == "") {
        jAlert('Please select a valid main account to proceed.', 'Alert');
        return;
    }


    $.ajax({
        type: "POST",
        url: "SalesReturnList.aspx/SaveInfluencer",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ infsave: obj }),
        dataType: "json",
        async: false,
        success: function (response) {

            jAlert(response.d);
            $("#myModal").modal('toggle');
        },
        error: function (response) {
            jAlert("Please try again later");
            //LoadingPanel.Hide();
        }
    });




    //$.ajax({
    //    type: "POST",
    //    url: "PosSalesInvoiceList.aspx/SaveInfluencer",
    //    contentType: "application/json; charset=utf-8",
    //    data: JSON.stringify({ infsave: obj }),
    //    dataType: "json",
    //    success: function (msg) {
    //    }
    //});




}

function CmbScheme_ValueChange() {

    var val = document.getElementById("CmbScheme").value;
    $("#MandatoryBillNo").hide();
    if (val != "0") {
        $.ajax({
            type: "POST",
            url: '../DailyTask/JournalEntry.aspx/getSchemeType',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{sel_scheme_id:\"" + val + "\"}",
            success: function (type) {
                console.log(type);

                var schemetypeValue = type.d;
                var schemetype = schemetypeValue.toString().split('~')[0];
                var schemelength = schemetypeValue.toString().split('~')[1];
                $('#txtBillNo').attr('maxLength', schemelength);
                var branchID = schemetypeValue.toString().split('~')[2];
                var branchStateID = schemetypeValue.toString().split('~')[3];

                var fromdate = schemetypeValue.toString().split('~')[4];
                var todate = schemetypeValue.toString().split('~')[5];

                var dt = new Date();

                tDate.SetDate(dt);

                if (dt < new Date(fromdate)) {
                    tDate.SetDate(new Date(fromdate));
                }

                if (dt > new Date(todate)) {
                    tDate.SetDate(new Date(todate));
                }




                tDate.SetMinDate(new Date(fromdate));
                tDate.SetMaxDate(new Date(todate));

                if (schemetypeValue != "") {

                }
                if (schemetype == '0') {

                    document.getElementById('txtBillNo').disabled = false;
                    document.getElementById('txtBillNo').value = "";
                    //document.getElementById("txtBillNo").focus();
                    setTimeout(function () { $("#txtBillNo").focus(); }, 200);

                }
                else if (schemetype == '1') {

                    document.getElementById('txtBillNo').disabled = true;
                    document.getElementById('txtBillNo').value = "Auto";
                    tDate.Focus();
                }
                else if (schemetype == '2') {

                    document.getElementById('txtBillNo').disabled = true;
                    document.getElementById('txtBillNo').value = "Datewise";
                }

            }

        });
    }
    else {
        document.getElementById('txtBillNo').disabled = true;
        document.getElementById('txtBillNo').value = "";
    }
}

function PerformCallToGridBind() {
    cSelectPanel.PerformCallback('Bindsingledesign');
    cDocumentsPopup.Hide();
    return false;
}
function cSelectPanelEndCall(s, e) {
    debugger;
    if (cSelectPanel.cpSuccess != "") {
        var TotDocument = cSelectPanel.cpSuccess.split(',');
        var reportName = cCmbDesignName.GetValue();
        var module = 'Sales_Return';
        if (TotDocument.length > 0) {
            for (var i = 0; i < TotDocument.length; i++) {
                if (TotDocument[i] != "") {
                    window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + ReturnId + '&PrintOption=' + TotDocument[i], '_blank')
                }
            }
        }
    }
    if (cSelectPanel.cpSuccess == "") {
        if (cSelectPanel.cpChecked != "") {
            jAlert('Please check Original For Recipient and proceed.');
        }
        CselectOriginal.SetCheckState('UnChecked');
        CselectDuplicate.SetCheckState('UnChecked');
        CselectTriplicate.SetCheckState('UnChecked');
        cCmbDesignName.SetSelectedIndex(0);
    }
}
var isFirstTime = true;

function AllControlInitilize() {
    if (isFirstTime) {
        if (localStorage.getItem('ReturnList_FromDate')) {
            var fromdatearray = localStorage.getItem('ReturnList_FromDate').split('-');
            var fromdate = new Date(fromdatearray[0], parseFloat(fromdatearray[1]) - 1, fromdatearray[2], 0, 0, 0, 0);
            cFormDate.SetDate(fromdate);
        }

        if (localStorage.getItem('ReturnList_ToDate')) {
            var todatearray = localStorage.getItem('ReturnList_ToDate').split('-');
            var todate = new Date(todatearray[0], parseFloat(todatearray[1]) - 1, todatearray[2], 0, 0, 0, 0);
            ctoDate.SetDate(todate);
        }

        if (localStorage.getItem('ReturnList_Branch')) {
            if (ccmbBranchfilter.FindItemByValue(localStorage.getItem('ReturnList_Branch'))) {
                ccmbBranchfilter.SetValue(localStorage.getItem('ReturnList_Branch'));
            }

        }

        //if ($("#LoadGridData").val() == "ok")
        //    updateGridByDate();
        isFirstTime = false;
    }
}

function updateGridByDate() {
    if (cFormDate.GetDate() == null) {
        jAlert('Please select from date.', 'Alert', function () { cFormDate.Focus(); });
    }
    else if (ctoDate.GetDate() == null) {
        jAlert('Please select to date.', 'Alert', function () { ctoDate.Focus(); });
    }
    else if (ccmbBranchfilter.GetValue() == null) {
        jAlert('Please select Branch.', 'Alert', function () { ccmbBranchfilter.Focus(); });
    }
    else {
        localStorage.setItem("ReturnList_FromDate", cFormDate.GetDate().format('yyyy-MM-dd'));
        localStorage.setItem("ReturnList_ToDate", ctoDate.GetDate().format('yyyy-MM-dd'));
        localStorage.setItem("ReturnList_Branch", ccmbBranchfilter.GetValue());



        $("#hfFromDate").val(cFormDate.GetDate().format('yyyy-MM-dd'));
        $("#hfToDate").val(ctoDate.GetDate().format('yyyy-MM-dd'));
        $("#hfBranchID").val(ccmbBranchfilter.GetValue());
        $("#hfIsFilter").val("Y");


        //REV 1.0
        // cGrdSalesReturn.Refresh();
        $("#hFilterType").val("All");
        cCallbackPanel.PerformCallback("");
        //END REV 1.0       
        // cGrdSalesReturn.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue())
    }
}
//REV 1.0
function CallbackPanelEndCall(s, e) {
    cGrdSalesReturn.Refresh();
}
        //END REV 1.0

function OnEWayBillClick(id, EWayBillNumber, EWayBillDate, EWayBillValue) {

    var ActiveEInvoice = $('#hdnActiveEInvoice').val();
    if (ActiveEInvoice == "1") {
        $.ajax({
            type: "POST",
            url: "SalesReturnList.aspx/Prc_EInvoiceChecking_details",
            data: "{'returnid':'" + id + "','mode':'EWayBill'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var status = msg.d;
                if (status == "Yes") {
                    $("#btnEWayBillSave").removeClass('hide');
                    $("#lblEwayBillStatus").text("");
                }
                else {
                    $("#lblEwayBillStatus").text("IRN not generated can not update.");

                    //jAlert("IRN not generated can not print.");
                    $("#btnEWayBillSave").addClass('hide');
                }
            }
        });
    }

    if (EWayBillNumber.trim() != "") {
        ctxtEWayBillNumber.SetText(EWayBillNumber);
    }
    else {
        ctxtEWayBillNumber.SetText("");
    }

    if (EWayBillDate.trim() != "" && EWayBillDate.trim() != "01-01-1970" && EWayBillDate.trim() != "01-01-1900") {
        cdt_EWayBill.SetText(EWayBillDate);
    }
    else {
        cdt_EWayBill.SetText("");
    }
    if (EWayBillValue.trim() != "0.00" && EWayBillValue.trim() != "") {
        ctxtEWayBillValue.SetText(EWayBillValue);
    }
    else {
        ctxtEWayBillValue.SetText("0.0");
    }
    $('#hddnSalesReturnID').val(id);
    cPopup_EWayBill.Show();
    ctxtEWayBillNumber.Focus();
}
function GetEWayBillDateFormat(today) {
    if (today != "") {
        var dd = today.getDate();
        var mm = today.getMonth() + 1; //January is 0!

        var yyyy = today.getFullYear();
        if (dd < 10) {
            dd = '0' + dd;
        }
        if (mm < 10) {
            mm = '0' + mm;
        }
        today = yyyy + '-' + mm + '-' + dd;
    }

    return today;
}
function CallEWayBill_save() {

    var ReturnID = $("#hddnSalesReturnID").val();
    var UpdateEWayBill = ctxtEWayBillNumber.GetValue();
    if (UpdateEWayBill == "0") {
        UpdateEWayBill = "";
    }
    if (cdt_EWayBill.GetValue() == "" && cdt_EWayBill.GetValue() == null) {
        var EWayBillDate = "1990-01-01";
    }
    else {
        var EWayBillDate = GetEWayBillDateFormat(new Date(cdt_EWayBill.GetValue()));
    }

    var EWayBillValue = ctxtEWayBillValue.GetValue();

    $.ajax({
        type: "POST",
        url: "SalesReturnList.aspx/UpdateEWayBill",
        data: JSON.stringify({
            ReturnID: ReturnID, UpdateEWayBill: UpdateEWayBill, EWayBillDate: EWayBillDate, EWayBillValue: EWayBillValue
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {
            var status = msg.d;
            if (status == "1") {
                jAlert("Saved successfully.");
                //ctxtEWayBillNumber.SetText("");
                cPopup_EWayBill.Hide();
                cGrdSalesReturn.Refresh();
            }
            else if (status == "-10") {
                jAlert("Data not saved.");
                cPopup_EWayBill.Hide();
            }
        }
    });
}
function CancelEWayBill_save() {
    cPopup_EWayBill.Hide();
}
function OnclickViewAttachment(obj) {
    //var URL = '/OMS/Management/Activities/SalesReturn_Document.aspx?idbldng=' + obj + '&type=SalesReturn';
    var URL = '/OMS/Management/Activities/EntriesDocuments.aspx?idbldng=' + obj + '&type=SaleReturn';
    window.location.href = URL;
}
document.onkeydown = function (e) {
    if (event.keyCode == 18) isCtrl = true;


    if (event.keyCode == 65 && isCtrl == true) { //run code for alt+a -- ie, Add

        StopDefaultAction(e);
        OnAddButtonClick();
    }

}

function StopDefaultAction(e) {
    if (e.preventDefault) { e.preventDefault() }
    else { e.stop() };

    e.returnValue = false;
    e.stopPropagation();
}
function OnAddButtonClick() {
    var url = 'SalesReturn.aspx?key=' + 'ADD';
    window.location.href = url;
}


////##### coded by Samrat Roy - 17/04/2017 - ref IssueLog(P Order - 70) 
////Add an another param to define request type 
function OnViewClick(keyValue) {
    var url = 'SalesReturn.aspx?key=' + keyValue + '&req=V' + '&type=SR';
    window.location.href = url;
}

function OnClickDelete(keyValue) {
    var statusmode = "";
    var ActiveEInvoice = $('#hdnActiveEInvoice').val();
    if (ActiveEInvoice == "1") {
        $.ajax({
            type: "POST",
            url: "SalesReturnList.aspx/Prc_EInvoiceChecking_details",
            data: "{'returnid':'" + keyValue + "','mode':'Delete'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (msg) {
                var status = msg.d;
                if (status == "Yes") {
                    jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                        if (r == true) {
                            cGrdSalesReturn.PerformCallback('Delete~' + keyValue);
                        }
                    });
                }
                else {
                    jAlert("IRN generated can not delete.");
                }
            }
            });
    }
    else
    {
        jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
            if (r == true) {            
                cGrdSalesReturn.PerformCallback('Delete~' + keyValue);
            }                           
        });
    }
}
function OnEndCallback(s, e) {

    if (cGrdSalesReturn.cpDelete != null) {
        jAlert(cGrdSalesReturn.cpDelete);

        cGrdSalesReturn.cpDelete = null;
        cGrdSalesReturn.Refresh();
        // window.location.href = "SalesReturnList.aspx";
    }
}
//function OnClickDelete(keyValue) {
//    jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
//        if (r == true) {
//            cGrdQuotation.PerformCallback('Delete~' + keyValue);
//        }
//    });
//}
function gridRowclick(s, e) {
    $('#GrdSalesReturn').find('tr').removeClass('rowActive');
    $('.floatedBtnArea').removeClass('insideGrid');
    //$('.floatedBtnArea a .ico').css({ 'opacity': '0' });
    $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea').addClass('insideGrid');
    $(s.GetRow(e.visibleIndex)).addClass('rowActive');
    setTimeout(function () {
        //alert('delay');
        var lists = $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a');
        //$(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a .ico').css({'opacity': '1'});
        //$(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a').each(function (e) {
        //    setTimeout(function () {
        //        $(this).fadeIn();
        //    }, 100);
        //});    
        $.each(lists, function (index, value) {
            //console.log(index);
            //console.log(value);
            setTimeout(function () {
                $(value).css({ 'opacity': '1' });
            }, 100);
        });
    }, 200);
}

