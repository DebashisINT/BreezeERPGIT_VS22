var CopyInvoiceId;
var cpSelectedKeys = [];

var rets;
function SetInfluencer(Id, Name) {
    if (Id) {

        var inf_id = Id.split('~')[0];
        var Ma_id = Id.split('~')[1];
        var Ma_name = Id.split('~')[2];

        if (Ma_id == "") {
            jAlert('Please add a main account to influencer to proceed');
            return;
        }

        if (Ma_id == "0") {
            jAlert('Please add a main account to influencer to proceed');
            return;
        }

        if (Ma_id == null) {
            jAlert('Please add a main account to influencer to proceed');
            return;
        }



        $("#infid").val(inf_id);
        ctxtInfluencerAdjustment.SetText(Name);
        $('#InfluencerModel').modal('toggle');
        onInfluencerReturn();
        // $('#VehicleModel').modal('toggle');
    }
}
function onInfluencerReturn() {
    $("#CmbScheme").val('0');
    $("#txtBillNo").val("")
    $("#txtBillNo").prop('disabled', true)
    $.ajax({
        type: "POST",
        url: "PosSalesInvoiceList.aspx/GetInfluencerReturnDetails",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ invid: $("#infid").val() }),
        dataType: "json",
        success: function (msg) {
            var status = msg.d;

            var inv = status.Invoice_Data;
            rets = status.Return_Data;
            var ret = rets;
            str = "<thead>";
            str = str + "<tr>";
                    
            str = str + "<th  class='hide'>DOCID</th>";
            str = str + "<th >Select</th>";
            str = str + "<th  class='hide'>INFLUENCERID</th>";
            str = str + "<th >Influencer</th>";
            str = str + "<th>Return Number</th>";
            str = str + "<th >Return Date</th>";
            str = str + "<th style='width:200px;'>Total Comm. Amount</th>";
            str = str + "<th>Unpaid Amount</th>";
            str = str + "<th class='hide'>Actual Amount</th>";
            str = str + "</tr>";
            str = str + "</thead>";
            str = str + "<tbody id='infadjretbodyRet'>";
            for (var i = 0; i < ret.length; i++) {
                str = str + "<tr id='trRet" + ret[i].DOC_ID + "'>";
                str = str + "<td  class='hide' id='DocRet_" + ret[i].DOC_ID + "'>" + ret[i].DOC_ID + "</td>";
                str = str + "<td  id='tdRetInfluencerIDSelect" + ret[i].DOC_ID + "'><input type='checkbox' id='chkInfIdRet" + ret[i].DOC_ID + "'></td>";
                str = str + "<td  class='hide' id='tdRetInfluencerID" + ret[i].DOC_ID + "'>" + ret[i].CON_ID + "</td>";
                str = str + "<td id='tdRetInfluencer" + ret[i].DOC_ID + "' >" + ret[i].NAME + "</td>";
                str = str + "<td id='tdRetNumber" + ret[i].DOC_ID + "'>" + ret[i].DOC_NUMBER + "</td>";
                str = str + "<td id='tdRetDate" + ret[i].DOC_ID + "' >" + ret[i].DOC_DATE + "</td>";
                str = str + "<td id='tdRetComm" + ret[i].DOC_ID + "'  >" + ret[i].Total_Comm + "</td>";
                str = str + "<td id='tdRetUnpaid" + ret[i].DOC_ID + "' ><input type='text' id='unpaidRet" + ret[i].DOC_ID + "' value= " + ret[i].Unpaid + " ></td>";
                str = str + "<td class='hide' id='tdRetActualUnpaid" + ret[i].DOC_ID + "' >" + ret[i].Unpaid + " </td>";

                str = str + "</tr>";
            }
            str = str + "</tbody>";
            $("#tableProductAdjustmentReturn").html('');
            $("#tableProductAdjustmentReturn").html(str);
            $("#InfluencerPopupModel").modal('show');
            str = "<thead>";
            str = str + "<tr>";
            str = str + "<th  class='hide'>DOC_ID</th>";
            str = str + "<th >Select</th>";
            str = str + "<th  class='hide'>INFLUENCERID</th>";
            str = str + "<th >Influencer</th>";
            str = str + "<th>Invoice Number</th>";
            str = str + "<th >Return Date</th>";
            str = str + "<th style='width:200px;'>Total Comm. Amount</th>";
            str = str + "<th>Unpaid Amount</th>";
            str = str + "</tr>";
            str = str + "</thead>";
            str = str + "<tbody id='infadjretbodyInv'>";
            for (var i = 0; i < inv.length; i++) {
                str = str + "<tr id='trInv" + inv[i].DOC_ID + "'>";
                str = str + "<td class='hide' id='DocInv_" + inv[i].DOC_ID + "'>" + inv[i].DOC_ID + "</td>";
                //str = str + "<td class='hide' id='tdInvInfluencerIDSelect" + inv[i].DOC_ID + "'><input type='radio' id='radInfId" + inv[i].CON_ID + "' name='InfSelection' onclick='handleClick(" + JSON.stringify(inv[i].CON_ID) + ");'></td>";
                str = str + "<td  id='tdInvInfluencerIDSelect" + inv[i].DOC_ID + "'><input type='checkbox' id='chkInfIdInv" + inv[i].DOC_ID + "'></td>";
                str = str + "<td  class='hide' id='tdInvInfluencerID" + inv[i].DOC_ID + "'>" + inv[i].CON_ID + "</td>";
                str = str + "<td id='tdInvInfluencer" + inv[i].DOC_ID + "' >" + inv[i].NAME + "</td>";
                str = str + "<td id='tdInvNumber" + inv[i].DOC_ID + "'>" + inv[i].DOC_NUMBER + "</td>";
                str = str + "<td id='tdInvDate" + inv[i].DOC_ID + "' >" + inv[i].DOC_DATE + "</td>";
                str = str + "<td id='tdInvComm" + inv[i].DOC_ID + "'  >" + inv[i].Total_Comm + "</td>";
                str = str + "<td id='tdInvUnpaid" + inv[i].DOC_ID + "' ><input type='text' id='unpaidInv" + inv[i].DOC_ID + "' value= " + inv[i].Unpaid + " ></td>";
                str = str + "</tr>";
            }
            str = str + "</tbody>";
            $("#tableProductAdjustmentInvoice").html('');
            $("#tableProductAdjustmentInvoice").html(str);
            $("#InfluencerPopupModel").modal('show');
        }
    });

}
function SaveInfluencerdetailsAdjustment() {
    var InfobjInv = [];
    var InfobjRet = [];
    $("#tableProductAdjustmentInvoice tr").each(function () {
        var arrayOfThisRow = [];
        var tableData = $(this).find('td');
        if (tableData.length > 0) {

            var id = $(tableData[0]).html().trim();
            if ($('#chkInfIdInv' + id).prop("checked")) {
                var Myobj = {
                    INF_ID: $("#tdInvInfluencerID" + id).html(),
                    DOC_ID: id,
                    AMOUNT: $("#unpaidInv" + id).val()
                };
                InfobjInv.push(Myobj);

            }          
        }
    });
    var myInfobjInv = JSON.stringify(InfobjInv);
    $("#tableProductAdjustmentReturn tr").each(function () {
        var arrayOfThisRow = [];
        var tableData = $(this).find('td');
        if (tableData.length > 0) {

            var id = $(tableData[0]).html().trim();
            if ($('#chkInfIdRet' + id).prop("checked")) {
                var Myobj = {
                    INF_ID: $("#tdRetInfluencerID" + id).html(),
                    DOC_ID: id,
                    AMOUNT: $("#unpaidRet" + id).val()
                };
                InfobjRet.push(Myobj);
            }


                    
        }
    });
    var myInfobjRet = JSON.stringify(InfobjRet);







    $.ajax({
        type: "POST",
        url: "PosSalesInvoiceList.aspx/SaveInfluencerAdj",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ invoice: InfobjInv, returns: InfobjRet }),
        dataType: "json",
        async: false,
        success: function (response) {

            jAlert(response.d);
            $("#InfluencerPopupModel").modal('toggle');
        },
        error: function (response) {
            jAlert("Please try again later");
            //LoadingPanel.Hide();
        }
    });
}


function handleClick(infID) {

    var RetFilter = $.grep(rets, function (e) { return e.CON_ID == infID; });
    str = "<thead>";
    str = str + "<tr>";
    str = str + "<th  class='hide'>DOC_ID</th>";
    str = str + "<th  class='hide'>INFLUENCERID</th>";

    str = str + "<th >Influencer</th>";
    str = str + "<th>Return Number</th>";
    str = str + "<th >Return Date</th>";
    str = str + "<th style='width:200px;'>Total Comm. Amount</th>";
    str = str + "<th>Unpaid Amount</th>";
    str = str + "<th class='hide'>Actual Amount</th>";
    str = str + "</tr>";
    str = str + "</thead>";
    str = str + "<tbody id='infadjretbodyRet'>";
    for (var i = 0; i < RetFilter.length; i++) {
        str = str + "<tr id='trRet" + RetFilter[i].DOC_ID + "'>";
        str = str + "<td class='hide' id='DocRet_" + RetFilter[i].DOC_ID + "'>" + RetFilter[i].DOC_ID + "</td>";
        str = str + "<td  class='hide' id='tdRetInfluencerID" + RetFilter[i].DOC_ID + "'>" + RetFilter[i].CON_ID + "</td>";
        str = str + "<td id='tdRetInfluencer" + RetFilter[i].DOC_ID + "' >" + RetFilter[i].NAME + "</td>";

        str = str + "<td id='tdRetNumber" + RetFilter[i].DOC_ID + "'>" + RetFilter[i].DOC_NUMBER + "</td>";
        str = str + "<td id='tdRetDate" + RetFilter[i].DOC_ID + "' >" + RetFilter[i].DOC_DATE + "</td>";
        str = str + "<td id='tdRetComm" + RetFilter[i].DOC_ID + "'  >" + RetFilter[i].Total_Comm + "</td>";
        str = str + "<td id='tdRetUnpaid" + RetFilter[i].DOC_ID + "' ><input type='text' id='unpaidRet" + RetFilter[i].DOC_ID + "' value= " + RetFilter[i].Unpaid + " ></td>";
        str = str + "<td class='hide' id='tdRetActualUnpaid" + RetFilter[i].DOC_ID + "' >" + RetFilter[i].Unpaid + " </td>";

        str = str + "</tr>";
    }
    str = str + "</tbody>";
    $("#tableProductAdjustmentReturn").html('');
    $("#tableProductAdjustmentReturn").html(str);

    $("#InfluencerPopupModel").modal('show');

}
function VehicleSelectionChanged(s, e) {
    if (e.isChangedOnServer) return;
    globalindexcheck = e.visibleIndex;
    var key = s.GetRowKey(e.visibleIndex);
    if (e.isSelected) {
        cpSelectedKeys.push(key);
    }
    else {
        cpSelectedKeys = RemoveElementFromArray(cpSelectedKeys, key);

    }
    appcode = cpSelectedKeys;

}
function OnclickViewAttachment(obj) {
    //var URL = '/OMS/Management/Activities/SalesInvoice_Document.aspx?idbldng=' + obj + '&type=SalesInvoice';
    var URL = '/OMS/Management/Activities/EntriesDocuments.aspx?idbldng=' + obj + '&type=SalesInvoice';
    window.location.href = URL;
}

function RemoveElementFromArray(array, element) {
    var index = array.indexOf(element);
    if (index < 0) return array;
    array[index] = null;
    var result = [];
    for (var i = 0; i < array.length; i++) {
        if (array[i] === null)
            continue;
        result.push(array[i]);
    }
    return result;
}

function CustomerClick(s, e) {


    $("#hdnCustomerId").val(e);


    $("#drdExport").val('0');
    cOutstandingPopup.Show();
    var CustomerId = $("#hdnCustomerId").val();
    var BranchId = ccmbBranchfilter.GetValue();
    $("#hddnBranchId").val(BranchId);
    var AsOnDate = new Date().format('yyyy-MM-dd');
    $("#hddnAsOnDate").val(AsOnDate);
    $("#hddnOutStandingBlock").val('1');
    //Clear Row
    var rw = $("[id$='CustomerOutstanding_DXMainTable']").find("tr")
    for (var RowClount = 0; RowClount < rw.length; RowClount++) {
        rw[RowClount].remove();
    }



    //cCustomerOutstanding.Refresh();

    //cCustomerOutstanding.PerformCallback('BindOutStanding~' + CustomerId + '~' + BranchId + '~' + AsOnDate);
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


    //cCustomerOutstanding.Refresh();
    //cOutstandingPopup.Show();


}



var SelectedInvoiceId = 0;

var GlobalRowIndex = 0;
var BranchMassListByKeyValue = [];
var isFirstTime = true;
function AllControlInitilize() {
    if (isFirstTime) {
        // PopulateCurrentBankBalance(ccmbBranchfilter.GetValue());

        if (localStorage.getItem('PosListFromDate')) {
            var fromdatearray = localStorage.getItem('PosListFromDate').split('-');
            var fromdate = new Date(fromdatearray[0], parseFloat(fromdatearray[1]) - 1, fromdatearray[2], 0, 0, 0, 0);
            cFormDate.SetDate(fromdate);
        }

        if (localStorage.getItem('PosListToDate')) {
            var todatearray = localStorage.getItem('PosListToDate').split('-');
            var todate = new Date(todatearray[0], parseFloat(todatearray[1]) - 1, todatearray[2], 0, 0, 0, 0);
            ctoDate.SetDate(todate);
        }
        if (localStorage.getItem('PosListBranch')) {
            if (ccmbBranchfilter.FindItemByValue(localStorage.getItem('PosListBranch'))) {
                ccmbBranchfilter.SetValue(localStorage.getItem('PosListBranch'));
            }

        }


        if ($("#LoadGridData").val() == "ok")
            updateGridByDate();

        isFirstTime = false;
    }
}

$(function () {
    BindERPSettings();
});

function BindERPSettings() {
    var OtherDetails = {}
    OtherDetails.Key = "ShowCreditInvoice";
    $.ajax({
        type: "POST",
        url: "Services/Master.asmx/GetMasterSettings",
        data: JSON.stringify(OtherDetails),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {
            var returnObject = msg.d;

            if (returnObject == "0") {
                $("#btnCredit").addClass(" hide");
            }
            else {
                $("#btnCredit").removeClass("hide");
            }

        }
    });
    OtherDetails.Key = "ShowCashInvoice";
    $.ajax({
        type: "POST",
        url: "Services/Master.asmx/GetMasterSettings",
        data: JSON.stringify(OtherDetails),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {
            var returnObject = msg.d;

            if (returnObject == "0") {
                $("#btnCash").addClass(" hide");
            }
            else {
                $("#btnCash").removeClass("hide");
            }

        }
    });


    OtherDetails.Key = "ShowFinInvoice";
    $.ajax({
        type: "POST",
        url: "Services/Master.asmx/GetMasterSettings",
        data: JSON.stringify(OtherDetails),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {
            var returnObject = msg.d;

            if (returnObject == "0") {
                $("#btnFin").addClass(" hide");
            }
            else {
                $("#btnFin").removeClass("hide");
            }

        }
    });


    OtherDetails.Key = "ShowIST";
    $.ajax({
        type: "POST",
        url: "Services/Master.asmx/GetMasterSettings",
        data: JSON.stringify(OtherDetails),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {
            var returnObject = msg.d;

            if (returnObject == "0") {
                $("#btnIST").addClass(" hide");
            }
            else {
                $("#btnIST").removeClass("hide");
            }

        }
    });


    OtherDetails.Key = "ShowCRP";
    $.ajax({
        type: "POST",
        url: "Services/Master.asmx/GetMasterSettings",
        data: JSON.stringify(OtherDetails),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {
            var returnObject = msg.d;

            if (returnObject == "0") {
                $("#btnCRP").addClass(" hide");
            }
            else {
                $("#btnCRP").removeClass("hide");
            }

        }
    });

}

function updateMassBranchAssign() {

}
function PerformCallToRacpayGridBind() {
    CustomerRecpayPanel.PerformCallback('Bindsingledesign');
    cCustDocumentsPopup.Hide();
    return false;
}

function CustRacPayPanelEndCall(s, e) {
    debugger;
    if (CustomerRecpayPanel.cpSuccess != null) {
        var TotDocument = CustomerRecpayPanel.cpSuccess.split(',');
        var reportName = cCustCmbDesignName.GetValue();
        var module = 'CUSTRECPAY';
        window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + RecPayId + '&PrintOption=' + 1, '_blank')
    }
    CustomerRecpayPanel.cpSuccess = null
    if (CustomerRecpayPanel.cpSuccess == null) {
        CustomerRecpayPanel.SetSelectedIndex(0);
    }
}

function onCustomerReceiptPrint(id) {
    RecPayId = id;
    cCustDocumentsPopup.Show();
    cCustCmbDesignName.SetSelectedIndex(0);
    CustomerRecpayPanel.PerformCallback('Bindalldesignes');
    $('#btnOK').focus();
}

function OnCustReceiptViewClick(id) {
    uri = "CustomerReceiptPayment.aspx?key=" + id + "&req=V&IsTagged=Y&type=CRP";
    capcReciptPopup.SetContentUrl(uri);
    capcReciptPopup.SetHeaderText("View Money Receipt");
    capcReciptPopup.Show();
}

function ListingGridEndCallback(s, e) {
    IconChange();
    if (cGrdQuotation.cpCancelAssignMent) {
        if (cGrdQuotation.cpCancelAssignMent == "yes") {
            jAlert("Branch Assignment Cancel Successfully.");
            cGrdQuotation.cpCancelAssignMent = null;
            cGrdQuotation.Refresh();
        }
    }


    if (cGrdQuotation.cpDelete) {
        jAlert(cGrdQuotation.cpDelete);
        cGrdQuotation.cpDelete = null;
        cGrdQuotation.Refresh();

    }
}




function CancelBranchToThisInvoice() {
    cAssignedBranch.SetValue('0');
    AssignedBranchSelectedIndexChanged(cAssignedBranch);
}

function onCancelBranchAssignment(invId) {
    cGrdQuotation.PerformCallback('CancelAssignment~' + invId);
}

var JVid = "";
var Action = "Add";
var AutoJVNumber = "";

function onAddInfluencer(invId) {
    $("#CmbScheme").val('0');
    $("#txtBillNo").val("")
    $("#txtBillNo").prop('disabled', true)
    $.ajax({
        type: "POST",
        url: "PosSalesInvoiceList.aspx/GetInfluencerDetails",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ invid: invId }),
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
                if (status.Influencer.AUTOJV_NUMBER != "" && status.Influencer.AUTOJV_NUMBER != null) {
                    $("#div_Edit").addClass('hide');
                    AutoJVNumber = status.Influencer.AUTOJV_NUMBER
                    $("#txtBillNo").val(status.Influencer.AUTOJV_NUMBER);
                    $("#txtBillNo").prop('disabled', true);
                    cbtnMAdr.SetText(status.Influencer.MainAccount_AccountCode);
                    $("#txtCommAmt").html(status.Influencer.COMM_AMOUNT);
                    $("#mainacdrid").val(status.Influencer.MAINACCOUNT_DR);

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
                str = str + "<th>Action</th>";
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


                    str = str + "<td><img onclick='infdeleteClick(" + JSON.stringify(status.Influencer_Details[i].DET_INFLUENCER_ID) + ")' src='../../../assests/images/crs.png' /></td>";
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
                str = str + "<th>Basis</th>";
                str = str + "<th>Commission Rate/Qty or Commission %</th>";
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

                    if (parseFloat(status.INF_Inv_Products[i].prod_Qty) > 0)
                        str = str + "<td><select id='prodappl" + status.INF_Inv_Products[i].Prod_id + "' onchange='ChangeType(" + status.INF_Inv_Products[i].Prod_id + ")'>";
                    else
                        str = str + "<td><select disabled id='prodappl" + status.INF_Inv_Products[i].Prod_id + "' onchange='ChangeType(" + status.INF_Inv_Products[i].Prod_id + ")'>";


                    if (status.INF_Inv_Products[i].Applicable_On == "1") {
                        str = str + "<option value='1' selected>On Qty.</option>";
                    }
                    else {
                        str = str + "<option value='1'>On Qty.</option>";
                    }



                    if (status.INF_Inv_Products[i].Applicable_On == "2") {
                        str = str + "<option value='2' selected>Amount before GST</option>";
                    }
                    else {
                        str = str + "<option value='2'>Amount before GST</option>";

                    }


                    if (status.INF_Inv_Products[i].Applicable_On == "3") {
                        str = str + "<option value='3' selected>Amount with GST</option>";
                    }
                    else {
                        str = str + "<option value='3'>Amount with GST</option>";
                    }


                    if (status.INF_Inv_Products[i].Applicable_On == "4") {
                        str = str + "<option value='4' selected>Flat Value</option>";
                    }
                    else {
                        str = str + "<option value='4'>Flat Value</option>";

                    }
                    str = str + "</select></td>";


                    if (parseFloat(status.INF_Inv_Products[i].prod_Qty) > 0) {
                        if (status.INF_Inv_Products[i].Applicable_On == "4") {
                            str = str + "<td><input type='textbox' class='numeric'  onfocusout='percenLostfocus(" + status.INF_Inv_Products[i].Prod_id + ")' onkeypress='return isNumberKey(event)' id='prodpercen" + status.INF_Inv_Products[i].Prod_id + "' value='" + status.INF_Inv_Products[i].Prod_Percentage + "'disabled></td>";
                            str = str + "<td><input type='textbox' class='numeric'  onfocusout='AmountLostfocus(" + status.INF_Inv_Products[i].Prod_id + ")' onkeypress='return isNumberKey(event)' id='prodcommamt" + status.INF_Inv_Products[i].Prod_id + "' value='" + status.INF_Inv_Products[i].PROD_COMM_AMOUNT + "' ></td>";
                        }
                        else {
                            str = str + "<td><input type='textbox' class='numeric'  onfocusout='percenLostfocus(" + status.INF_Inv_Products[i].Prod_id + ")' onkeypress='return isNumberKey(event)' id='prodpercen" + status.INF_Inv_Products[i].Prod_id + "' value='" + status.INF_Inv_Products[i].Prod_Percentage + "' ></td>";
                            str = str + "<td><input type='textbox' class='numeric'  onfocusout='AmountLostfocus(" + status.INF_Inv_Products[i].Prod_id + ")' onkeypress='return isNumberKey(event)' id='prodcommamt" + status.INF_Inv_Products[i].Prod_id + "' value='" + status.INF_Inv_Products[i].PROD_COMM_AMOUNT + "' disabled></td>";
                        }
                    }
                    else {
                        str = str + "<td><input type='textbox' class='numeric'  onfocusout='percenLostfocus(" + status.INF_Inv_Products[i].Prod_id + ")' onkeypress='return isNumberKey(event)' id='prodpercen" + status.INF_Inv_Products[i].Prod_id + "' value='" + status.INF_Inv_Products[i].Prod_Percentage + "' disabled></td>";
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
                jAlert('Return fully done . Can not add Influencer.', 'Alert');
            }
        }

    });


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
function OnInfAddClick() {

    var str = "";
    var DET_INFLUENCER_ID = $("#infid").val();
    var trcount = $('#influencerGrid tr').length;
    var DET_AMOUNT_CR = DecimalRoundoff($("#txtCommAmt").html().trim(), 2) / parseInt(trcount);
    var DET_MAINACCOUNT_CR = $("#mainaccrid").val();
    var INF_Name = ctxtInfluencer.GetText();
    var DET_MAINACCOUNT_NAME = cbtnMainAccount.GetText();

    var valid = true;

    $("#infbody tr td:contains(" + DET_INFLUENCER_ID + ")")
    .next().text(function () {
        valid = false;
    });

    if (!valid) {
        jAlert('Influencer already added.', 'Alert');
        return;
    }

    str = str + "<tr id='tr" + DET_INFLUENCER_ID + "'>";
    str = str + "<td id='infid" + DET_INFLUENCER_ID + "' class='hide'>" + DET_INFLUENCER_ID + "</td>";
    str = str + "<td id='infmaid" + DET_INFLUENCER_ID + "' class='hide' >" + DET_MAINACCOUNT_CR + "</td>";
    str = str + "<td id='infmaname" + DET_INFLUENCER_ID + "' >" + DET_MAINACCOUNT_NAME + "</td>";
    str = str + "<td id='infname" + DET_INFLUENCER_ID + "' >" + INF_Name + "</td>";
    if (trcount > 2) {
        str = str + "<td id='infamt" + DET_INFLUENCER_ID + "'  class='asas' ><input type='text' id='txtinfamt" + DET_INFLUENCER_ID + "' value='" + DET_AMOUNT_CR + "' /></td>";
    }
    else {
        str = str + "<td id='infamt" + DET_INFLUENCER_ID + "'  class='asas' ><input  type='text' id='txtinfamt" + DET_INFLUENCER_ID + "' value='" + DET_AMOUNT_CR + "' disabled /></td>";

    }
    //str = str + "<td><img onclick='infdeleteClick(" + DET_INFLUENCER_ID + ")' src='../../../assests/images/crs.png' /></td>";
    str = str + '<td><img  onclick="infdeleteClick(' + JSON.stringify(DET_INFLUENCER_ID).replace(/"/g, '&quot;') + ');" src="../../../assests/images/crs.png" /></td>';

    str = str + "</tr>";
    $("#infbody").append(str);


    ReclaculateTotal();





}
function ValueSelected(e, indexName) {
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var Id = e.target.parentElement.parentElement.cells[0].innerText;
        var name = e.target.parentElement.parentElement.cells[1].children[0].value;
        if (Id) {
            if (indexName == "MainAccountIndex") {
                SetMainAccount(Id, name, e);
            }
            else if (indexName == "MainAccountIndexdr") {
                SetMainAccountdr(Id, name, e);
            }
            else if (indexName == "InvoiceCustomerIndex") {
                InvoiceSetCustomer(Id, name);
            }
            else if (indexName == "InfluencerIndex") {
                SetInfluencer(Id, name);
            }
            else {
                SetCustomer(Id, name);
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
            else if (indexName == "MainAccountIndexdr")
                $('#txtMainAccountSearchdr').focus();
            else if (indexName == "InvoiceCustomerIndex")
                $('#InvoicetxtCustsSearch').focus();
            else
                $('#txtCustSearch').focus();
        }
    }


}
function InvoiceSetCustomer(Id, Name) {
    $("#hdnInvoiceCustID").val(Id);
    ctxtCustName.SetValue(Name);
    $('#InvoiceCustsModel').modal('toggle');
    ctxtCustName.SetFocus()
}

function SetCustomer(Id, Name) {
    if (Id) {

        var inf_id = Id.split('~')[0];
        var Ma_id = Id.split('~')[1];
        var Ma_name = Id.split('~')[2];

        if (Ma_id == "") {
            jAlert('Please add a main account to influencer to proceed');
            return;
        }

        if (Ma_id == "0") {
            jAlert('Please add a main account to influencer to proceed');
            return;
        }

        if (Ma_id == null) {
            jAlert('Please add a main account to influencer to proceed');
            return;
        }


        cbtnMainAccount.SetText(Ma_name);
        $("#mainaccrid").val(Ma_id);

        $("#infid").val(inf_id);
        ctxtInfluencer.SetText(Name);
        $('#CustModel').modal('toggle');
        // $('#VehicleModel').modal('toggle');
    }
}

        


function SetMainAccount(Id, name, e) {

    $("#mainaccrid").val(Id);
    cbtnMainAccount.SetText(name);
    $('#MainAccountModel').modal('toggle');
}
function SetMainAccountdr(Id, name, e) {

    $("#mainacdrid").val(Id);
    cbtnMAdr.SetText(name);
    $('#MainAccountModeldr').modal('toggle');
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
    $('#CustModel').on('shown.bs.modal', function () {
        $('#txtCustSearch').focus();
    })

    $('#CustModel').on('hide.bs.modal', function () {
        cbtnSaveInfluencer.SetFocus();
    })
    $('#VehicleModel').on('shown.bs.modal', function () {
        $('#txtVechileSearch').focus();
    })

    $('#VehicleModel').on('hide.bs.modal', function () {
        cbtnSaveInfluencer.SetFocus();
    })

    //cQuotationComponentPanel.PerformCallback('DateCheckOnChanged' + '~' + key + '~' + startDate + '~' + '@');

    //if (key != null && key != '' && type != "") {
    //    cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + key + '~' + startDate + '~' + 'DateCheck' + '~' + type + '~' + inventory);
    //}

    //Chinmoy added below line
    // cddl_PosGst.SetEnabled(false);

    var componentType = gridquotationLookup.GetValue();//gridquotationLookup.GetGridView().GetRowKey(gridquotationLookup.GetGridView().GetFocusedRowIndex());
    if (componentType != null && componentType != '') {
        grid.PerformCallback('GridBlank');
        cddl_PosGst.SetEnabled(true);
    }
});
function percenLostfocus(id) {
    if ($("#prodpercen" + id).val() == "") {
        $("#prodpercen" + id).val('0.00')
    }

    if ($("#prodappl" + id).val() == "1") {

        var qty = $("#prodqty" + id).html();
        var salesprice = $("#prodsp" + id).html();
        var percen = $("#prodpercen" + id).val();

        if (DecimalRoundoff(percen, 2) > DecimalRoundoff(salesprice, 2)) {

            jAlert('Amount per quantity can not be greater than sales price', 'Alert');
            $("#prodpercen" + id).val("0.00");
        }
        else {
            var amt = (DecimalRoundoff(qty, 2) * DecimalRoundoff(percen, 2)).toFixed(2);
            $("#prodcommamt" + id).val(amt);
        }


    }
    else if ($("#prodappl" + id).val() == "2") {

        var qty = $("#prodqty" + id).html();
        var salesprice = $("#prodsp" + id).html();
        var percen = $("#prodpercen" + id).val();
        var invamt = $("#proddetamt" + id).html();



        if (DecimalRoundoff(percen, 2) > 100) {

            jAlert('Amount per quantity can not be greater than sales price', 'Alert');
            $("#prodpercen" + id).val("0.00");
        }
        else {
            var amt = (DecimalRoundoff(invamt, 2) * DecimalRoundoff(percen, 2) * 0.01).toFixed(2);
            $("#prodcommamt" + id).val(amt);
        }

    }
    else if ($("#prodappl" + id).val() == "3") {

        var qty = $("#prodqty" + id).html();
        var salesprice = $("#prodsp" + id).html();
        var percen = $("#prodpercen" + id).val();
        var invamt = DecimalRoundoff(qty, 2) * DecimalRoundoff(salesprice, 2);



        if (DecimalRoundoff(percen, 2) > 100) {

            jAlert('Amount per quantity can not be greater than sales price', 'Alert');
            $("#prodpercen" + id).val("0.00");
        }
        else {
            var amt = (DecimalRoundoff(invamt, 2) * DecimalRoundoff(percen, 2) * 0.01).toFixed(2);
            $("#prodcommamt" + id).val(amt);
        }

    }

    var sum = 0;
    $("#tableProduct tbody tr").each(function () {
        var obj = $(this).find("td")[0].innerHTML.trim();
        sum = sum + DecimalRoundoff($("#prodcommamt" + obj).val(), 2);
    })

    $("#txtCommAmt").html(sum);
    ReclaculateTotal();


}

function ReclaculateTotal() {
    var trcount = $('#influencerGrid tr').length - 1;
    var DET_AMOUNT_CR = (DecimalRoundoff($("#txtCommAmt").html().trim(), 2) / parseInt(trcount)).toFixed(2);
    var sum = 0;
    $("#infbody tr").each(function () {
        var obj = $(this).find("td")[0].innerHTML.trim();
        $("#txtinfamt" + obj).val(DET_AMOUNT_CR);
        if (trcount > 1) {
            $("#txtinfamt" + obj).prop("disabled", false);
        }
        else {
            $("#txtinfamt" + obj).prop("disabled", true);

        }
    })
}


function AmountLostfocus(id) {

    var sum = 0;
    $("#tableProduct tbody tr").each(function () {
        var obj = $(this).find("td")[0].innerHTML.trim();
        sum = sum + DecimalRoundoff($("#prodcommamt" + obj).val(), 2);
    })

    $("#txtCommAmt").html(sum);
    ReclaculateTotal();
    //if ($("#prodappl" + id).val() == "1") {

    //    var qty = $("#prodqty" + id).html();
    //    var salesprice = $("#prodsp" + id).html();
    //    var amt = $("#prodcommamt" + id).val();
    //    var lastamt = (DecimalRoundoff(amt, 2) / DecimalRoundoff(qty, 2)).toFixed(2);



    //    if (DecimalRoundoff(lastamt, 2) > DecimalRoundoff(salesprice, 2)) {

    //        jAlert('Amount per quantity can not be greater than sales price', 'Alert');
    //        $("#prodcommamt" + id).val("0.00");
    //        $("#prodpercen" + id).val("0.00");
    //    }
    //    else {

    //        $("#prodpercen" + id).val(lastamt);
    //    }


    //}
    //else if ($("#prodappl" + id).val() == "2") {

    //    var qty = $("#prodqty" + id).html();
    //    var salesprice = $("#prodsp" + id).html();
    //    var prodamt = $("#prodcommamt" + id).val();
    //    var invamt = $("#proddetamt" + id).html();

    //    var percent = ((DecimalRoundoff(invamt, 2) / DecimalRoundoff(prodamt, 2)) * 100).toFixed(2);


    //    if (DecimalRoundoff(prodamt, 2) > DecimalRoundoff(invamt, 2)) {

    //        jAlert('Amount per quantity can not be greater than sales price', 'Alert');
    //        $("#prodcommamt" + id).val("0.00");
    //        $("#prodpercen" + id).val("0.00");
    //    }
    //    else {

    //        $("#prodcommamt" + id).val(percent);
    //    }

    //}
    //else if ($("#prodappl" + id).val() == "3") {

    //    var qty = $("#prodqty" + id).html();
    //    //var salesprice = $("#prodsp" + id).html();
    //    var prodamt = $("#prodcommamt" + id).val();
    //    var invamt = $("#prodsp" + id).html();

    //    var percent = ((DecimalRoundoff(invamt, 2) / DecimalRoundoff(prodamt, 2)) * 100).toFixed(2);


    //    if (DecimalRoundoff(prodamt, 2) > DecimalRoundoff(invamt, 2)) {

    //        jAlert('Amount per quantity can not be greater than sales price', 'Alert');
    //        $("#prodcommamt" + id).val("0.00");
    //        $("#prodpercen" + id).val("0.00");
    //    }
    //    else {

    //        $("#prodcommamt" + id).val(percent);
    //    }

    //}






    //var sum = 0;
    //$("#tableProduct tbody tr").each(function () {
    //    var obj = $(this).find("td")[0].innerHTML.trim();
    //    sum = sum + DecimalRoundoff($("#prodcommamt" + obj).val(), 2);
    //})

    //$("#txtCommAmt").html(sum);


    //ReclaculateTotal();
}

function infdeleteClick(infid) {
    var row = document.getElementById("tr" + infid);
    row.parentNode.removeChild(row);


    var trcount = $('#influencerGrid tr').length - 1;
    var DET_AMOUNT_CR = DecimalRoundoff($("#txtCommAmt").html().trim(), 2) / parseInt(trcount);


    $("#infbody tr").each(function () {
        var obj = $(this).find("td")[0].innerHTML.trim();
        $("#infamt" + obj).html(DET_AMOUNT_CR);
    })
}


function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    console.log(charCode);
    if (charCode == 46)
        return true;
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;
    return true;
}
function ChangeType(id) {
    // alert(id);
    if ($("#prodappl" + id).val() == "4") {

        $("#prodpercen" + id).prop('disabled', true);
        $("#prodpercen" + id).val("0.00");
        $("#prodcommamt" + id).val("0.00");
        $("#prodcommamt" + id).prop('disabled', false);
    }
    else {

        $("#prodpercen" + id).prop('disabled', false);
        $("#prodpercen" + id).val("0.00");
        $("#prodcommamt" + id).val("0.00");
        $("#prodcommamt" + id).prop('disabled', true);
    }

    var sum = 0;
    $("#tableProduct tbody tr").each(function () {
        var obj = $(this).find("td")[0].innerHTML.trim();
        sum = sum + DecimalRoundoff($("#prodcommamt" + obj).val(), 2);
    })

    $("#txtCommAmt").html(sum);

    ReclaculateTotal();
}

function MassBranchCustomButtonClick(s, e) {
    if (e.buttonID == 'CancelAssignment') {
        GlobalRowIndex = e.visibleIndex;
        cmassBranch.batchEditApi.StartEdit(GlobalRowIndex);
        cmassBranch.GetEditor('pos_assignBranch').SetValue(0);
        BranchChangeOnMassChange(cmassBranch.GetEditor('pos_assignBranch'));
    }
    else if (e.buttonID == 'ShowStock') {
        cAssignmentGrid.PerformCallback('0~0');
        GlobalRowIndex = e.visibleIndex;
        SelectedInvoiceId = cmassBranch.GetRowKey(GlobalRowIndex);
        $('#BranchAssignmentHeader').hide();
        cAssignmentPopUp.SetHeaderText('Show Stock');
        cAssignmentPopUp.Show();
    }


}

function gridFocusedRowChanged(s, e) {
    GlobalRowIndex = e.visibleIndex;
}





function BranchChangeOnMassChange(s, e) {

    var AssignedBranch = {
        InvoiceId: '',
        BranchId: ''
    }
    AssignedBranch.InvoiceId = cmassBranch.GetRowKey(GlobalRowIndex);
    AssignedBranch.BranchId = s.GetValue();
    for (var ind = 0; ind < BranchMassListByKeyValue.length; ind++) {
        if (BranchMassListByKeyValue[ind].InvoiceId == AssignedBranch.InvoiceId) {
            BranchMassListByKeyValue.pop(ind);
        }
    }


    BranchMassListByKeyValue.push(AssignedBranch);
}


function MassBranchAssign() {
    //cmassBranch.Refresh();
    //cmassBranchPopup.Show();
    var url = '/OMS/Management/Activities/PosMassBranch.aspx';
    cmassBranchPopup.SetContentUrl(url);
    cmassBranchPopup.Show();

    //}
    return true;
}

function BranchAssignmentBranchSelectedIndexChanged() {
    cAssignedBranch.SetValue(cBranchAssignmentBranch.GetValue());
    //    AssignedBranchSelectedIndexChanged(cBranchAssignmentBranch);
    cAssignedWareHouse.PerformCallback(cAssignedBranch.GetValue());
}

function AssignmentGridEndCallback() {
    if (cAssignmentGrid.cpMsg) {
        if (cAssignmentGrid.cpMsg != '') {
            jAlert(cAssignmentGrid.cpMsg, 'Alert', function () {
                cAssignmentPopUp.Hide();
                if (page.activeTabIndex == 0) {
                    //cGrdQuotation.PerformCallback('RefreshGrid');
                    cGrdQuotation.Refresh();
                }

            });
            cAssignmentGrid.cpMsg = null;
        }
    }
}
function AssignBranchToThisInvoice() {
    //$('#MandatoryBranchAssign').attr('style', 'display:none');
    //$('#mandetoryAssignedWareHouse').attr('style', 'display:none');


    //if (cAssignedBranch.GetValue() == null || cAssignedBranch.GetValue() == '0') {
    //    $('#MandatoryBranchAssign').attr('style', 'display:block');
    //}
    //else if (cAssignedWareHouse.GetValue() == null) {
    //    $('#mandetoryAssignedWareHouse').attr('style', 'display:block');
    //} else {
    //    cAssignmentGrid.PerformCallback('AssignBranch~' + SelectedInvoiceId + '~' + cAssignedBranch.GetValue() + '~' + cAssignedWareHouse.GetValue());
    //}

    cAssignmentGrid.PerformCallback('AssignBranch~' + SelectedInvoiceId + '~' + cAssignedBranch.GetValue() + '~' + cAssignedWareHouse.GetValue());
}

function watingInvoicegridEndCallback() {
    if (cwatingInvoicegrid.cpReturnMsg) {
        if (cwatingInvoicegrid.cpReturnMsg != "") {
            jAlert(cwatingInvoicegrid.cpReturnMsg);
            document.getElementById('waitingInvoiceCount').value = parseFloat(document.getElementById('waitingInvoiceCount').value) - 1;
            cwatingInvoicegrid.cpReturnMsg = null;
        }
    }
}


function onViewBranchAssignment(obj) {
    SelectedInvoiceId = obj;
    cAssignmentGrid.PerformCallback('0~0');
    cBranchRequUpdatePanel.PerformCallback(SelectedInvoiceId);
    $('#BranchAssignmentHeader').show();
    cAssignmentPopUp.SetHeaderText('Branch Assignment');
    cAssignmentPopUp.Show();

}

function onCopyInvoice(obj, Inv) {
    debugger;
    $('#InvoiceCopyModel').modal('show');
    ctxt_InvoiceNo.SetEnabled(false);
    ctxt_InvoiceNo.SetValue(Inv);

    CopyInvoiceId = Inv;

    // SalesManLoad();
    cddl_SalesAgent.PerformCallback(CopyInvoiceId);

}

function SalesManLoad() {
    cddl_SalesAgent.ClearItems();
    var otherdetails = {};
    otherdetails.Inv = CopyInvoiceId;
    $.ajax({
        type: "POST",
        url: "PosSalesInvoiceList.aspx/bindSalesmanByBranch",
        data: JSON.stringify(otherdetails),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var returnObject = msg.d;
            for (var i = 0; i < returnObject.SalesManDetails.length; i++) {
                cddl_SalesAgent.AddItem(returnObject.SalesManDetails[i].Name, returnObject.SalesManDetails[i].CNTId);
            }
        },
        error: function (msg) {
            var ss = msg.d;
            alert(msg);
        }
    });

}


function onAddVehicle(obj) {
    //SelectedInvoiceId = obj;
    //cAssignmentGrid.PerformCallback('0~0');
    //cBranchRequUpdatePanel.PerformCallback(SelectedInvoiceId);
    //$('#BranchAssignmentHeader').show();
    //cAssignmentPopUp.SetHeaderText('Branch Assignment');
    //cAssignmentPopUp.Show();
    $("#invid").val(obj);
    $.ajax({
        type: "POST",
        url: "PosSalesInvoiceList.aspx/viewDelavery",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ invoice: obj }),
        dataType: "json",
        success: function (msg) {
            var status = msg.d;
            var str = "";

            if (status != null) {

                $("#txtRemarks").val(status.Remarks);
                ctxt_EwayValue.SetValue(status.EwayValu);
                ctxt_EbillNo.SetValue(status.ENo);
                ccmbOtehrChrgs.SetValue(status.CmbOtehrChrgs);
                ccmbPaymentTrms.SetValue(status.cmbPaymentTrms);
                //gridquotationLookup.grid.Selection.SelectRowByKey(1);
                // cEwayDate.SetDate(status.PostingDate);
                if (status.POSTING_DATE != null) {
                    cEwayDate.SetDate(new Date(parseInt(status.POSTING_DATE.substr(6))));
                }
            }
        }
    });

    $('#VehicleModel').modal('show');
    cQuotationComponentPanel.PerformCallback();
}



function AssignedBranchSelectedIndexChanged() {
    cAssignedWareHouse.PerformCallback(cAssignedBranch.GetValue());

    cBranchAssignmentBranch.SetValue(cAssignedBranch.GetValue());
    //updateAssignmentGrid();
}

function updateAssignmentGrid() {
    cAssignmentGrid.PerformCallback(SelectedInvoiceId + '~' + cBranchAssignmentBranch.GetValue());
}

function IconChange() {
    $(function () {
        var $tr = $("#GrdQuotation_DXMainTable > tbody > tr");
        $tr.each(function (index, value) {
            //var $DelvStatus = $(this).find("td").eq(8).text();
            //var $payType = $(this).find("td").eq(5).text();
            //var $DelvType = $(this).find("td").eq(6).text();
            //var $AssignedBranch = $(this).find("td").eq(15).text();
            //var $ReturnNumber = $(this).find("td").eq(12).text();

            var $DelvStatus = $(this).find("td").eq(7).text();
            var $payType = $(this).find("td").eq(4).text();
            var $DelvType = $(this).find("td").eq(5).text();
            var $AssignedBranch = $(this).find("td").eq(14).text();
            var $ReturnNumber = $(this).find("td").eq(11).text();
            var $ChallanNumber = $(this).find("td").eq(8).text();



            //var $a_Assignment = $(this).find("td").eq(17).find("#a_Assignment");
            //var $Cancel_Assignment = $(this).find("td").eq(17).find("#Cancel_Assignment");
            //var $a_editInvoice = $(this).find("td").eq(17).find("#a_editInvoice");
            //var $a_delete = $(this).find("td").eq(17).find("#a_delete");]

            var $a_Assignment = $(this).find("td").eq(17).find("#a_Assignment");
            var $Cancel_Assignment = $(this).find("td").eq(17).find("#Cancel_Assignment");
            var $a_editInvoice = $(this).find("td").eq(17).find("#a_editInvoice");
            var $a_delete = $(this).find("td").eq(17).find("#a_delete");




            //if ($DelvType.trim() == "Already Delivered") {
            //    $a_delete.hide();
            //}
            //if ($ChallanNumber != "") {
            //    $a_Assignment.hide();
            //    $a_editInvoice.hide();
            //    $a_delete.hide();
            //}
            //if ($ReturnNumber.trim() != "") {
            //    $a_Assignment.hide();
            //    $a_editInvoice.hide();
            //    $a_delete.hide();
            //}
            //if ($DelvStatus == 'Pending') {
            //    $a_Assignment.show();
            //    $a_editInvoice.show();
            //}
            //else {
            //    if ($payType != 'Finance') {
            //        $a_Assignment.hide();
            //        $a_editInvoice.hide();
            //    }
            //}

            //if ($DelvStatus == 'Transfered') {
            //    $Cancel_Assignment.show();
            //} else {
            //    $Cancel_Assignment.hide();
            //}


            switch ($DelvType.trim()) {
                case "Our":
                    if ($ChallanNumber.trim() != "") {
                        $a_Assignment.hide();
                        $a_editInvoice.hide();
                        $a_delete.hide();
                        $Cancel_Assignment.hide();
                    }
                    else {
                        //$a_Assignment.show();
                        //$Cancel_Assignment.hide();
                        $a_editInvoice.show();
                        $a_delete.show();
                        if ($DelvStatus == 'Pending') {
                            $a_Assignment.show();
                            $a_editInvoice.show();
                        }
                        else {
                            if ($payType != 'Finance') {
                                $a_Assignment.hide();
                                $a_editInvoice.hide();
                            }
                        }

                        if ($DelvStatus == 'Transfered') {
                            $Cancel_Assignment.show();
                        } else {
                            $Cancel_Assignment.hide();
                        }
                    }
                    if ($ReturnNumber.trim() != "") {
                        $a_Assignment.hide();
                        $a_editInvoice.hide();
                        $a_delete.hide();
                        $Cancel_Assignment.hide();
                    }
                    break;
                case "Self":
                    {
                        if ($ChallanNumber.trim() != "") {
                            $a_Assignment.hide();
                            $a_editInvoice.hide();
                            $a_delete.hide();
                            $Cancel_Assignment.hide();
                        }
                        else {
                            //$a_Assignment.show();
                            //$Cancel_Assignment.hide();
                            $a_editInvoice.show();
                            $a_delete.show();
                            if ($DelvStatus == 'Pending') {
                                $a_Assignment.show();
                                $a_editInvoice.show();
                            }
                            else {
                                if ($payType != 'Finance') {
                                    $a_Assignment.hide();
                                    $a_editInvoice.hide();
                                }
                            }

                            if ($DelvStatus == 'Transfered') {
                                $Cancel_Assignment.show();
                            } else {
                                $Cancel_Assignment.hide();
                            }

                        }
                        if ($ReturnNumber.trim() != "") {
                            $a_Assignment.hide();
                            $a_editInvoice.hide();
                            $a_delete.hide();
                            $Cancel_Assignment.hide();
                        }
                        break
                    }
                case "Already Delivered":
                    if ($ChallanNumber.trim() != "") {
                        $a_Assignment.hide();
                        //$a_editInvoice.hide();
                        $a_delete.hide();
                        $Cancel_Assignment.hide();
                    }
                    if ($ReturnNumber.trim() != "") {
                        $Cancel_Assignment.hide();
                        $a_Assignment.hide();
                        //$a_editInvoice.hide();
                        $a_delete.hide();
                    }
                    break;
            }
            if ($AssignedBranch.trim() != "") {
                $a_delete.hide();
                //$a_editInvoice.hide();
            }

        });
    });
}




$(document).ready(function () {
    IconChange();


});
function OnBeginAfterCallback(s, e) {
    IconChange();
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

        localStorage.setItem("PosListFromDate", cFormDate.GetDate().format('yyyy-MM-dd'));
        localStorage.setItem("PosListToDate", ctoDate.GetDate().format('yyyy-MM-dd'));
        localStorage.setItem("PosListBranch", ccmbBranchfilter.GetValue());

        $('#branchName').text(ccmbBranchfilter.GetText());
        PopulateCurrentBankBalance(ccmbBranchfilter.GetValue());
        if (page.activeTabIndex == 0) {
            //cGrdQuotation.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue())
            $("#hfFromDate").val(cFormDate.GetDate().format('yyyy-MM-dd'));
            $("#hfToDate").val(ctoDate.GetDate().format('yyyy-MM-dd'));
            $("#hfBranchID").val(ccmbBranchfilter.GetValue());
            $("#hfIsFilter").val("Y");
            cGrdQuotation.Refresh();
        }
        else if (page.activeTabIndex == 1) {

        }
        if (page.activeTabIndex == 2) {

        }
    }
}
function InvoiceWattingOkClick() {
    var index = cwatingInvoicegrid.GetFocusedRowIndex();
    var listKey = cwatingInvoicegrid.GetRowKey(index);
    if (listKey) {
        if (cwatingInvoicegrid.GetRow(index).children[6].innerText != "Advance") {
            var url = $("#hdnInvoiceType").val() + '?key=' + 'ADD&&BasketId=' + listKey;
            LoadingPanel.Show();
            window.location.href = url;
        } else {
            ShowbasketReceiptPayment(listKey);
        }
                
    }
}
function cSelectPanelEndCall(s, e) {
    debugger;
    if (cSelectPanel.cpSuccess != null) {
        var TotDocument = cSelectPanel.cpSuccess.split(',');
        var reportName = cCmbDesignName.GetValue();
        var module = 'Invoice_POS';
        if (TotDocument.length > 0) {
            for (var i = 0; i < TotDocument.length; i++) {
                if (TotDocument[i] != "") {
                    //if (cCmbDesignName.GetValue() == 1) {
                    //    window.open("../../reports/XtraReports/Viewer/InvoiceReportViewer.aspx?id=" + InvoiceId + '&PrintOption=' + TotDocument[i], '_blank')
                    //}
                    //else if (cCmbDesignName.GetValue() == 2) {
                    //    window.open("../../reports/XtraReports/Viewer/TaxInvoiceReportViewer.aspx?id=" + InvoiceId + '&PrintOption=' + TotDocument[i], '_blank')
                    //}
                    window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + InvoiceId + '&PrintOption=' + TotDocument[i], '_blank')
                }
            }
        }
    }
    //cSelectPanel.cpSuccess = null
    if (cSelectPanel.cpSuccess == "") {
        if (cSelectPanel.cpChecked != "") {
            jAlert('Please check Original For Recipient and proceed.');
        }
        //CselectDuplicate.SetEnabled(false);
        //CselectTriplicate.SetEnabled(false);
        CselectOriginal.SetCheckState('UnChecked');
        CselectDuplicate.SetCheckState('UnChecked');
        CselectFDuplicate.SetCheckState('UnChecked');
        CselectTriplicate.SetCheckState('UnChecked');
        cCmbDesignName.SetSelectedIndex(0);
    }
}
function PerformCallToGridBind() {
    //cSelectPanel.PerformCallback();

    cSelectPanel.PerformCallback('Bindsingledesign~' + InvoiceId);
    cDocumentsPopup.Hide();
    return false;
}
//function OrginalCheckChange(s, e) {
//    debugger;
//    if (s.GetCheckState() == 'Checked') {
//        CselectDuplicate.SetEnabled(true);
//    }
//    else {
//        CselectDuplicate.SetCheckState('UnChecked');
//        CselectDuplicate.SetEnabled(false);
//        CselectTriplicate.SetCheckState('UnChecked');
//        CselectTriplicate.SetEnabled(false);
//    }

//}
//function DuplicateCheckChange(s, e) {
//    if (s.GetCheckState() == 'Checked') {
//        CselectTriplicate.SetEnabled(true);
//    }
//    else {
//        CselectTriplicate.SetCheckState('UnChecked');
//        CselectTriplicate.SetEnabled(false);
//    }

//}
var InvoiceId = 0;
function onPrintJv(id, RowIndex) {
    InvoiceId = id;
    cSelectPanel.cpSuccess = "";
    cDocumentsPopup.Show();
    //$('#HdInvoiceType').val(cGrdQuotation.GetRow(RowIndex).children[5].innerText);
    $('#HdInvoiceType').val(cGrdQuotation.GetRow(RowIndex).children[4].innerText);
    //CselectDuplicate.SetEnabled(false);
    //CselectTriplicate.SetEnabled(false);
    CselectOriginal.SetCheckState('UnChecked');
    CselectDuplicate.SetCheckState('UnChecked');
    CselectFDuplicate.SetCheckState('UnChecked');
    CselectTriplicate.SetCheckState('UnChecked');
    cCmbDesignName.SetSelectedIndex(0);
    cSelectPanel.PerformCallback('Bindalldesignes~' + InvoiceId);
    $('#btnOK').focus();
}

function onPrintJvIST(id, RowIndex) {
    InvoiceId = id;
    cSelectPanel.cpSuccess = "";
    cDocumentsPopup.Show();




    $('#HdInvoiceType').val('Stock Transfer');
    //CselectDuplicate.SetEnabled(false);
    //CselectTriplicate.SetEnabled(false);
    CselectOriginal.SetCheckState('UnChecked');
    CselectDuplicate.SetCheckState('UnChecked');
    CselectFDuplicate.SetCheckState('UnChecked');
    CselectTriplicate.SetCheckState('UnChecked');
    cCmbDesignName.SetSelectedIndex(0);
    cSelectPanel.PerformCallback('Bindalldesignes~' + InvoiceId);
    $('#btnOK').focus();
}

function OnWaitingGridKeyPress(e) {

    if (e.code == "Enter") {
        var index = cwatingInvoicegrid.GetFocusedRowIndex();
        var listKey = cwatingInvoicegrid.GetRowKey(index);
        if (listKey) {
            if (cwatingInvoicegrid.GetRow(index).children[6].innerText != "Advance") {
                var url = $("#hdnInvoiceType").val() + '?key=' + 'ADD&&BasketId=' + listKey;
                LoadingPanel.Show();
                window.location.href = url;
            } else {
                ShowbasketReceiptPayment(listKey);
            }
        }
    }

}
function RemoveInvoice(obj) {
    if (obj) {
        jConfirm("Clicking on Delete will not allow to use this Billing request again. Are you sure?", "Alert", function (ret) {
            if (ret) {
                cwatingInvoicegrid.PerformCallback('Remove~' + obj);
            }
        });

    }
}


function ShowReceiptPayment() {
    uri = "CustomerReceipt.aspx?key=ADD&IsTagged=Y";
    capcReciptPopup.SetContentUrl(uri);
    capcReciptPopup.SetHeaderText("Add Money Receipt");
    capcReciptPopup.Show();
}

function ShowbasketReceiptPayment(id) {
    uri = "CustomerReceiptPayment.aspx?key=ADD&IsTagged=Y&&basketId=" + id;
    capcReciptPopup.SetContentUrl(uri);
    capcReciptPopup.SetHeaderText("Add Money Receipt");
    capcReciptPopup.Show();
}

function timerTick() {
    //   cwatingInvoicegrid.Refresh();


    $.ajax({
        type: "POST",
        url: "PosSalesInvoiceList.aspx/GetTotalWatingInvoiceCount",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var status = msg.d;
            console.log(status);
            clblweatingCount.SetText(status);
            var fetcheddata = parseFloat(document.getElementById('waitingInvoiceCount').value);
            if (status != fetcheddata) {
                cwatingInvoicegrid.Refresh();
                document.getElementById('waitingInvoiceCount').value = status;
            }
        }
    });

}
function InvoiceWatingClick() {

    waitingPopUp.Show();
    cwatingInvoicegrid.Focus();
}
function ListRowClicked(s, e) {

    var index = e.visibleIndex;
    var listKey = cwatingInvoicegrid.GetRowKey(index);
    if (e.htmlEvent.target.id != "CloseRemoveWattingBtn") {
        if (cwatingInvoicegrid.GetRow(index).children[6].innerText != "Advance") {
            var url = $("#hdnInvoiceType").val() + '?key=' + 'ADD&&BasketId=' + listKey;
            LoadingPanel.Show();
            window.location.href = url;
        } else {
            ShowbasketReceiptPayment(listKey);
        }
    }
}
document.onkeydown = function (e) {
    if (event.keyCode == 73 && event.altKey == true) {
        StopDefaultAction(e);
        InvoiceWatingClick();
    }
    else if (event.keyCode == 67 && event.altKey == true) {
        StopDefaultAction(e);
        OnAddInvoiceButtonClick('Cash');
    }
    else if (event.keyCode == 68 && event.altKey == true) {
        StopDefaultAction(e);
        OnAddInvoiceButtonClick('Crd');
    }
    else if (event.keyCode == 70 && event.altKey == true) {
        StopDefaultAction(e);
        OnAddInvoiceButtonClick('Fin');
    }
    else if (event.keyCode == 82 && event.altKey == true) {
        StopDefaultAction(e);
        ShowReceiptPayment();
    }
    else if (event.keyCode == 77 && event.altKey == true) {
        StopDefaultAction(e);
        MassBranchAssign();
    }
    else if (event.keyCode == 83 && event.altKey == true) {
        StopDefaultAction(e);
        if (cmassBranchPopup.IsVisible()) {
            MassBranchAssignSaveClick();
        }
        else {
            OnAddInvoiceButtonClick('IST');
        }
    }

    else if (event.keyCode == 79 && event.altKey == true) {
        if (waitingPopUp.IsVisible()) {
            StopDefaultAction(e);
            var index = cwatingInvoicegrid.GetFocusedRowIndex();
            var listKey = cwatingInvoicegrid.GetRowKey(index);
            if (listKey) {
                if (cwatingInvoicegrid.GetRow(index).children[6].innerText != "Advance") {
                    var url = $("#hdnInvoiceType").val() + '?key=' + 'ADD&&BasketId=' + listKey;
                    LoadingPanel.Show();
                    window.location.href = url;
                } else {
                    ShowReceiptPayment();
                }
            }
        }
    }
}
function StopDefaultAction(e) {
    if (e.preventDefault) { e.preventDefault() }
    else { e.stop() };

    e.returnValue = false;
    e.stopPropagation();
}
function OnAddButtonClick() {
    var url = 'blankpage.aspx?key=' + 'ADD';

    window.location.href = url;
}
function OnAddInvoiceButtonClick(obj) {
    eraseCookie('MenuCloseOpen');

    if (obj != "IST") {
        LoadingPanel.Show();
        var url = $("#hdnInvoiceType").val() + '?key=ADD&&type=' + obj;
    } else {
        if ($('#hdIsStockLedger').val() == "no") {
            jAlert("Ledger for Interstate Stk-Out not mapped in Account Heads. Cannot Proceed.");
            return false;
        } else {
            LoadingPanel.Show();
            var url = $("#hdnInvoiceType").val() + '?key=ADD&&type=' + obj;
        }
    }

    window.location.href = url;
}
function openAdvanceReceipt() {
    var url = 'CustomerReceiptPayment.aspx?key=ADD'
    window.location.href = url;
}

function viewDocument(keyValue) {

    var url = '/OMS/management/Activities/View/Invoice.html?v0.000044?id=' + keyValue;

    cPosView.SetContentUrl(url);
    cPosView.RefreshContentUrl();
    cPosView.Show();

}



function OnClickDelete(keyValue) {
    jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
        if (r == true) {
            cGrdQuotation.PerformCallback('Delete~' + keyValue);
        }
    });
}




function PopulateCurrentBankBalance(BranchId) {
    var frDate = cFormDate.GetDate().format('yyyy-MM-dd');
    var toDate = ctoDate.GetDate().format('yyyy-MM-dd');

    $.ajax({
        type: "POST",
        url: 'PosSalesInvoicelist.aspx/GetCurrentBankBalance',
        data: JSON.stringify({ BranchId: BranchId, fromDate: frDate, todate: toDate }),

        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var list = msg.d;

            if (msg.d.length > 0) {
                document.getElementById("pageheaderContent").style.display = 'block';
                if (msg.d.split('~')[0] != '') {

                    document.getElementById('B_BankBalance').innerHTML = msg.d.split('~')[0];
                    document.getElementById('B_BankBalance').style.color = "Black";
                }
                else {
                    document.getElementById('B_BankBalance').innerHTML = '0.0';
                    document.getElementById('B_BankBalance').style.color = "Black";

                }
            }

        },

    });

}
function disp_prompt(name) {
    if (name == "tab0") {
        //document.location.href="crm_sales.aspx"; 
    }
    if (name == "tab1") {
        document.location.href = "PosInvoiceList.aspx?tab=tab1";
    }
    if (name == "tab2") {
        document.location.href = "PosInvoiceList.aspx?tab=tab2";
    }

}
function InfluencerBtnClick(s, e) {
    $('#CustModel').modal('show');
}

function InfluencerBtnClickAdjustment(s, e) {
    $('#InfluencerModel').modal('show');
}
function InfluencerKeyDownAdjustment(s, e) {
    if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
        $('#InfluencerModel').modal('show');
    }
}

function InfluencerKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
        $('#CustModel').modal('show');
    }
}

function VechileBtnClick(s, e) {
    $('#VechielModel').modal('show');
}

function VechileKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
        $('#VechielModel').modal('show');
    }
}

function btnMainAccountClick(s, e) {
    $('#MainAccountModel').modal('show');
}

function btnMainAccountKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
        $('#MainAccountModel').modal('show');
    }
}


function btnMAdrClick(s, e) {
    $('#MainAccountModeldr').modal('show');
}

function btnMAdrKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
        $('#MainAccountModeldr').modal('show');
    }
}


function CloseGridQuotationLookup() {

    gridquotationLookup.ConfirmCurrentSelection();
    gridquotationLookup.HideDropDown();
    // gridquotationLookup.Focus();
}

function LoadOldSelectedKeyvalue() {
    var x = gridquotationLookup.gridView.GetSelectedKeysOnPage();
    var Ids = "";
    for (var i = 0; i < x.length; i++) {
        Ids = Ids + ',' + x[i];
    }
    document.getElementById('OldSelectedKeyvalue').value = Ids;
}

function BeginComponentCallback() {
}


function componentEndCallBack(s, e) {

    gridquotationLookup.gridView.Refresh();
    //if (grid.GetVisibleRowsOnPage() == 0) {
    //    OnAddNewClick();
    //}

    if (cQuotationComponentPanel.cpRebindGridQuote && cQuotationComponentPanel.cpRebindGridQuote != "") {
        ctxt_InvoiceDate.SetText(cQuotationComponentPanel.cpRebindGridQuote);
        cQuotationComponentPanel.cpRebindGridQuote = null;
    }


    if (cQuotationComponentPanel.cpDetails != null) {
        var details = cQuotationComponentPanel.cpDetails;
        cQuotationComponentPanel.cpDetails = null;
    }
}

function QuotationNumberChanged() {
    var quote_Id = gridquotationLookup.gridView.GetSelectedKeysOnPage();//gridquotationLookup.GetValue();
    quote_Id = quote_Id.join();

    var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";

    if (quote_Id != null) {
        var arr = quote_Id.split(',');

        if (arr.length > 1) {
            if (type == "QO") {
                ctxt_InvoiceDate.SetText('Multiple Select Quotation Dates');
            }
            else if (type == "SO") {
                ctxt_InvoiceDate.SetText('Multiple Select Order Dates');
            }
            else if (type == "SC") {
                ctxt_InvoiceDate.SetText('Multiple Select Challan Dates');
            }
        }
        else {
            if (arr.length == 1) {
                // cComponentDatePanel.PerformCallback('BindComponentDate' + '~' + quote_Id + '~' + type);
            }
            else {
                ctxt_InvoiceDate.SetText('');
            }
        }
    }
    else { ctxt_InvoiceDate.SetText(''); }

    if (quote_Id != null) {
        // cgridproducts.PerformCallback('BindProductsDetails' + '~' + '@');
        // cProductsPopup.Show();
    }
}


    function gridcrmCampaignclick(s, e) {
        //alert('hi');
        //IconChange();
        $('#gridcrmCampaign').find('tr').removeClass('rowActive');
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
                    console.log(value);
                    $(value).css({ 'opacity': '1' });
                }, 100);
            });
        }, 200);
    }
    function InvoiceCustomersButnClick(s, e) {
        $('#InvoiceCustsModel').modal('show');
    }

function CopyInvoiceCustomerKeydown(s, e) {
    if (e.htmlEvent.key == "Enter") {
        $('#InvoiceCustsModel').modal('show');
    }
}



function InvoiceCustomerskeydown(e) {
    var OtherDetails = {}

    if ($.trim($("#InvoicetxtCustsSearch").val()) == "" || $.trim($("#InvoicetxtCustsSearch").val()) == null) {
        return false;
    }
    OtherDetails.SearchKey = $("#InvoicetxtCustsSearch").val();
    OtherDetails.BranchID = $('#ddl_Branch').val();

    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var HeaderCaption = [];
        HeaderCaption.push("Customer Name");
        HeaderCaption.push("Unique Id");
        HeaderCaption.push("Address");

        if ($("#InvoicetxtCustsSearch").val() != "") {
            callonServer("Services/Master.asmx/GetCustomer", OtherDetails, "InvoiceCustomersTable", HeaderCaption, "InvoiceCustomerIndex", "InvoiceSetCustomer");
        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[InvoiceCustomerIndex=0]"))
            $("input[InvoiceCustomerIndex=0]").focus();
    }
}

function OnSalesAgentEndCallback(s, e) {
    //if (lastSalesman) {
    //    cddl_SalesAgent.PerformCallback(lastSalesman);
    //    lastSalesman = null;
    //} else {
    //if (!lastSalesman)
    //    cmbUcpaymentCashLedgerChanged(ccmbUcpaymentCashLedger);
    // }
    ctxt_NewInvoiceNo.SetFocus();
}

    $(document).ready(function () {
        if ($('body').hasClass('mini-navbar')) {
            var windowWidth = $(window).width();
            var cntWidth = windowWidth - 90;
            cGrdQuotation.SetWidth(cntWidth);
        } else {
            var windowWidth = $(window).width();
            var cntWidth = windowWidth - 220;
            cGrdQuotation.SetWidth(cntWidth);
        }

        $('.navbar-minimalize').click(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                cGrdQuotation.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                cGrdQuotation.SetWidth(cntWidth);
            }

        });
    });
</script>