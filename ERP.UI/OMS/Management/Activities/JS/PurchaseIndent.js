//=======================================================Revision History =========================================================================
//1.0     Priti    V2.0.36   17 - 02 - 2023     After Listing view upgradation delete data show in listing issue solved.
//2.0     Pallab   V2.0.38   18 - 05 - 2023     26166: The Product Name and Description is too small in the Grid of Purchase Indent Module when the Screen Resolution is 1366X768
//=========================================================End Revision History========================================================================
    $(function () {
            $('#UOMModal').on('hide.bs.modal', function () {
                InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 6);
            });
        });

    function closeMultiUOM(s, e) {
        e.cancel = false;
    // cPopup_MultiUOM.Hide();
}

    $(function () {
        $(".allownumericwithdecimal").on("keypress keyup blur", function (event) {
    //this.value = this.value.replace(/[^0-9\.]/g,'');
            $(this).val($(this).val().replace(/[^0-9\.]/g, ''));
            if ((event.which != 46 || $(this).val().indexOf('.') != -1) && (event.which < 48 || event.which > 57)) {
                event.preventDefault();
}
});
});

var  _ComponentDetails;
var Uomlength = 0;
function UomLenthCalculation() {
    InsgridBatch.batchEditApi.StartEdit(globalRowIndex);
    var SLNo = "";
            
    SLNo = InsgridBatch.GetEditor('SrlNo').GetValue();
           
    $.ajax({
        type: "POST",
        url: "PurchaseIndent.aspx/GetQuantityfromSL",
        data: JSON.stringify({ SLNo: SLNo}),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {

            Uomlength = msg.d;

        }
    });
}


function FinalMultiUOM() {
    //debugger;
    UomLenthCalculation();

    if (Uomlength == 0 || Uomlength < 0) {
        // Mantis Issue 24428 
        //jAlert("Please add Alt. Quantity.");
        jAlert("Please add atleast one Alt. Quantity with Update Row as checked.");
        // End of Mantis Issue 24428 
        return;
    }
    else {
        cPopup_MultiUOM.Hide();
         // Mantis Issue 24428 
        var SLNo = InsgridBatch.GetEditor('SrlNo').GetValue();
        cgrid_MultiUOM.PerformCallback('SetBaseQtyRateInGrid~' + SLNo);

        // End of Mantis Issue 24428 
        setTimeout(function () {
            InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 8);
        }, 200)
    }
    setTimeout(function () {
        InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 8);
    }, 600)
}

function OnMultiUOMEndCallback(s, e) {
    if (cgrid_MultiUOM.cpDuplicateAltUOM == "DuplicateAltUOM") {
        jAlert("Please Enter Different Alt. Quantity.");
        return;
    }
    
    // Mantis Issue 24428
    if (cgrid_MultiUOM.cpSetBaseQtyRateInGrid != null && cgrid_MultiUOM.cpSetBaseQtyRateInGrid == "1") {
        InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 6);
        var BaseQty = cgrid_MultiUOM.cpBaseQty;
        var BaseRate = cgrid_MultiUOM.cpBaseRate;
       
        if (ctaggingList.GetValue() != "") {
            InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 6);
            var Quantity = (InsgridBatch.GetEditor('gvColQuantity').GetValue() != null) ? InsgridBatch.GetEditor('gvColQuantity').GetValue() : "0";
            var BalanceQty = (InsgridBatch.GetEditor('BalanceQty').GetValue() != null) ? InsgridBatch.GetEditor('BalanceQty').GetValue() : "0";

            //if (parseFloat(BaseQty) > parseFloat(BalanceQty)) {
            //    jAlert('Quantity can not be greater than tagged quantity.', 'Alert', function () {
            //        InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 6);
            //        InsgridBatch.GetEditor('gvColQuantity').SetValue(0);
            //    });
            //}
            //else
            //{
            //    InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 6);
            //    InsgridBatch.GetEditor('gvColQuantity').SetValue(BaseQty);
            //}
                InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 6);
                InsgridBatch.GetEditor('gvColQuantity').SetValue(BaseQty);
        }
        else
        {
            InsgridBatch.GetEditor("gvColQuantity").SetValue(BaseQty);
        }
        //InsgridBatch.GetEditor("SalePrice").SetValue(BaseRate);
        //InsgridBatch.GetEditor("Amount").SetValue(BaseQty * BaseRate)
        InsgridBatch.GetEditor("Order_AltQuantity").SetValue(cgrid_MultiUOM.cpAltQty);
        InsgridBatch.GetEditor("Order_AltUOM").SetValue(cgrid_MultiUOM.cpAltUom);
    }

    if (cgrid_MultiUOM.cpAllDetails == "EditData") {
        var Quan = (cgrid_MultiUOM.cpBaseQty).toFixed(4);
        $('#UOMQuantity').val(Quan);
      //  $('#UOMQuantity').val(cgrid_MultiUOM.cpBaseQty);
        ccmbBaseRate.SetValue(cgrid_MultiUOM.cpBaseRate)
        ccmbSecondUOM.SetValue(cgrid_MultiUOM.cpAltUom);
        cAltUOMQuantity.SetValue(cgrid_MultiUOM.cpAltQty);
        ccmbAltRate.SetValue(cgrid_MultiUOM.cpAltRate);
        hdMultiUOMID = cgrid_MultiUOM.cpuomid;
        if (cgrid_MultiUOM.cpUpdatedrow == true) {
            $("#chkUpdateRow").prop('checked', true);
            $("#chkUpdateRow").attr('checked', 'checked');
        }
        else {
            $("#chkUpdateRow").prop('checked', false);
            $("#chkUpdateRow").removeAttr("checked");
        }
    }

    // End of Mantis Issue 24428
    if (cgrid_MultiUOM.cpOpenFocus == "OpenFocus") {
        setTimeout(function () {
            ccmbSecondUOM.SetFocus();
        }, 400)
        //ccmbSecondUOM.SetFocus();
    }            
}


function Delete_MultiUom(keyValue, SrlNo, DetailsId) {


    cgrid_MultiUOM.PerformCallback('MultiUomDelete~' + keyValue + '~' + SrlNo + '~' + DetailsId);
    cgrid_MultiUOM.cpDuplicateAltUOM = "";

}

function PopulateMultiUomAltQuantity() {
    //debugger;
    var otherdet = {};
    var Quantity = $("#UOMQuantity").val();
    otherdet.Quantity = Quantity;
    var UomId = ccmbUOM.GetValue();
    otherdet.UomId = UomId;
    var Productdetails = (InsgridBatch.GetEditor('gvColProduct').GetText() != null) ? InsgridBatch.GetEditor('gvColProduct').GetText() : "0";
    var ProductID = Productdetails.split("||@||")[0];
    otherdet.ProductID = ProductID;
    var AltUomId = ccmbSecondUOM.GetValue();
    otherdet.AltUomId = AltUomId;

    $.ajax({
        type: "POST",
        url: "PurchaseIndent.aspx/GetPackingQuantity",
        data: JSON.stringify(otherdet),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            //debugger;
            if (msg.d.length != 0) {
                var packingQuantity = msg.d[0].packing_quantity;
                var sProduct_quantity = msg.d[0].sProduct_quantity;
            }
            else {
                var packingQuantity = 0;
                var sProduct_quantity = 0;
            }
            var uomfactor = 0
            if (sProduct_quantity != 0 && packingQuantity != 0) {
                uomfactor = parseFloat(packingQuantity / sProduct_quantity).toFixed(4);
                $('#hddnuomFactor').val(parseFloat(packingQuantity / sProduct_quantity));
            }
            else {
                $('#hddnuomFactor').val(0);
            }

            var uomfac_Qty_to_stock = $('#hddnuomFactor').val();
            var Qty = $("#UOMQuantity").val();
            var calcQuantity = parseFloat(Qty * uomfac_Qty_to_stock).toFixed(4);

            //$("#AltUOMQuantity").val(calcQuantity);
            cAltUOMQuantity.SetValue(calcQuantity);

        }
    });
}


function SaveMultiUOM() {
    //debugger;

    //grid.GetEditor('gvColProduct').GetText().split("||@||")[3];

    var qnty = $("#UOMQuantity").val();


    var UomId = ccmbUOM.GetValue();
    //var UomId = ccmbUOM.SetSelectedIndex(grid.GetEditor('gvColProduct').GetText().split("||@||")[3] - 1);
    var UomName = ccmbUOM.GetText();
    //var AltQnty = parseFloat($("#AltUOMQuantity").val()).toFixed(4);
    var AltQnty = cAltUOMQuantity.GetValue();
    var AltUomId = ccmbSecondUOM.GetValue();
    var AltUomName = ccmbSecondUOM.GetText();
    // Rev Mantis Issue 24428/24429
    InsgridBatch.batchEditApi.StartEdit(globalRowIndex);
    // End of Rev Mantis Issue 24428/24429
    var srlNo = (InsgridBatch.GetEditor('SrlNo').GetValue() != null) ? InsgridBatch.GetEditor('SrlNo').GetValue() : "";
    var Productdetails = (InsgridBatch.GetEditor('gvColProduct').GetText() != null) ? InsgridBatch.GetEditor('gvColProduct').GetText() : "0";
    var ProductID = Productdetails.split("||@||")[0];

    // Mantis Issue 24428
    if (ProductID == "") {
        ProductID = hdProductID.value;
    }
    // Mantis Issue 24428
    var BaseRate = ccmbBaseRate.GetValue();
    var AltRate = ccmbAltRate.GetValue();

    var UpdateRow = 'False';
    if ($("#chkUpdateRow").prop("checked")) {
        UpdateRow = 'True';
    }
    // End of Mantis Issue 24428
    // Mantis Issue 24428
    //if (srlNo != "" && qnty != "0.0000" && UomId != "" && UomName != "" && AltUomId != "" && ProductID != "" && AltUomId != null && AltUomName !="") {
    if (srlNo != "" && UomId != "" && UomName != "" && AltUomId != "" && ProductID != "" && AltUomId != null && AltUomName != "" && AltQnty != "0.0000") {
        if ((qnty != "0" && UpdateRow == 'True') || (qnty == "0" && UpdateRow == 'False') || (qnty != "0" && UpdateRow == 'False')) {
            // End of Mantis Issue 24428

            // Mantis Issue 24428
            if (cbtnMUltiUOM.GetText() == "Update") {
                cgrid_MultiUOM.PerformCallback('UpdateRow~' + srlNo + '~' + qnty + '~' + UomName + '~' + AltUomName + '~' + AltQnty + '~' + UomId + '~' + AltUomId + '~' + ProductID + '~' + BaseRate + '~' + AltRate + '~' + UpdateRow + '~' + hdMultiUOMID);
                //$("#AltUOMQuantity").val(parseFloat(0).toFixed(4));
                cAltUOMQuantity.SetValue("0.0000");
                // Mantis Issue 24428
                $("#UOMQuantity").val(0);
                ccmbBaseRate.SetValue(0);
                cAltUOMQuantity.SetValue(0);
                ccmbAltRate.SetValue(0);
                ccmbSecondUOM.SetValue("");
                cgrid_MultiUOM.cpAllDetails = "";
                cbtnMUltiUOM.SetText("Add");
              
                $("#chkUpdateRow").prop('checked', false);
                $("#chkUpdateRow").removeAttr("checked");
            


            }
             // End of Mantis Issue 24428

            else {

                cgrid_MultiUOM.PerformCallback('SaveDisplay~' + srlNo + '~' + qnty + '~' + UomName + '~' + AltUomName + '~' + AltQnty + '~' + UomId + '~' + AltUomId + '~' + ProductID + '~' + BaseRate + '~' + AltRate + '~' + UpdateRow);

                //$("#AltUOMQuantity").val(parseFloat(0).toFixed(4));
                cAltUOMQuantity.SetValue("0.0000");
                // Mantis Issue 24428
                $("#UOMQuantity").val(0);
                ccmbBaseRate.SetValue(0)
                cAltUOMQuantity.SetValue(0)
                ccmbAltRate.SetValue(0)
                ccmbSecondUOM.SetValue("");
               
                $("#chkUpdateRow").prop('checked', false);
                $("#chkUpdateRow").removeAttr("checked");
               
                // End of Mantis Issue 24428
            }
            // Mantis Issue 24428
        }
        else {
            return;
        }
        // End of Mantis Issue 24428
    }
    else {
        return;
    }
    // End of Mantis Issue 24428
}

// Mantis Issue 24428
function Edit_MultiUom(keyValue, SrlNo) {
    cbtnMUltiUOM.SetText("Update");
    cgrid_MultiUOM.PerformCallback('EditData~' + keyValue + '~' + SrlNo);
}
// End of Mantis Issue 24428

var isFirstTime = true;
function AllControlInitilize() {
    ///  document.getElementById('AddButton').style.display = 'inline-block';
    if (isFirstTime) {

        if (localStorage.getItem('PurIndentFromDate')) {
            var fromdatearray = localStorage.getItem('PurIndentFromDate').split('-');
            var fromdate = new Date(fromdatearray[0], parseFloat(fromdatearray[1]) - 1, fromdatearray[2], 0, 0, 0, 0);
            cFormDate.SetDate(fromdate);
        }

        if (localStorage.getItem('PurIndentToDate')) {
            var todatearray = localStorage.getItem('PurIndentToDate').split('-');
            var todate = new Date(todatearray[0], parseFloat(todatearray[1]) - 1, todatearray[2], 0, 0, 0, 0);
            ctoDate.SetDate(todate);
        }
        if (localStorage.getItem('PurIndentBranch')) {
            if (ccmbBranchfilter.FindItemByValue(localStorage.getItem('PurIndentBranch'))) {
                ccmbBranchfilter.SetValue(localStorage.getItem('PurIndentBranch'));
            }

        }
        //updateGridByDate();

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

        localStorage.setItem("PurIndentFromDate", cFormDate.GetDate().format('yyyy-MM-dd'));
        localStorage.setItem("PurIndentToDate", ctoDate.GetDate().format('yyyy-MM-dd'));
        localStorage.setItem("PurIndentBranch", ccmbBranchfilter.GetValue());

        $("#hfFromDate").val(cFormDate.GetDate().format('yyyy-MM-dd'));
        $("#hfToDate").val(ctoDate.GetDate().format('yyyy-MM-dd'));
        $("#hfBranchID").val(ccmbBranchfilter.GetValue());
        $("#hfIsFilter").val("Y");

        // Mantis Issue 25394
        //CgvPurchaseIndent.Refresh();
        cCallbackPanel.PerformCallback("");
        // End of Mantis Issue 25394

        $("#drdExport").val(0);
    }
}

//Rev Subhra 0019337 23-01-2019
//function onPrintJv(id) {
//    //window.location.href = "../../reports/XtraReports/Viewer/PurchaseIndentReportViewer.aspx?id=" + id;
//    window.open("../../reports/XtraReports/Viewer/PurchaseIndentReportViewer.aspx?id=" + id, '_blank')
//}

var PIndentId = 0;
function onPrintJv(id) {
    PIndentId = id;
    cDocumentsPopup.Show();
    cCmbDesignName.SetSelectedIndex(0);
    cSelectPanel.PerformCallback('Bindalldesignes');
    $('#btnOK').focus();
}
function cSelectPanelEndCall(s, e) {
    if (cSelectPanel.cpSuccess != null) {
        var TotDocument = cSelectPanel.cpSuccess.split(',');
        var reportName = cCmbDesignName.GetValue();
        var module = 'PINDENT';
        window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + PIndentId, '_blank')
    }
    cSelectPanel.cpSuccess = null
    if (cSelectPanel.cpSuccess == null) {
        cCmbDesignName.SetSelectedIndex(0);
    }
}

function PerformCallToGridBind() {
    cSelectPanel.PerformCallback('Bindsingledesign');
    cDocumentsPopup.Hide();
    return false;
}

//End of Rev Subhra 0019337 23-01-2019

//This function is called to show the Status of All Sales Order Created By Login User Start
function OpenPopUPUserWiseQuotaion() {
    cgridUserWiseQuotation.PerformCallback();
    cPopupUserWiseQuotation.Show();
}
// function above  End

//This function is called to show all Pending Approval of Sales Order whose Userid has been set LevelWise using Approval Configuration Module 
function OpenPopUPApprovalStatus() {
    cgridPendingApproval.PerformCallback();
    cpopupApproval.Show();
}
// function above  End


// Status 2 is passed If Approved Check box is checked by User Both Below function is called and used to show in POPUP,  the Add Page of Respective Segment(like Page for Adding Quotation ,Sale Order ,Challan)
function GetApprovedQuoteId(s, e, itemIndex) {
    var rowvalue = cgridPendingApproval.GetRowValues(itemIndex, 'ID', OnGetApprovedRowValues);
    //cgridPendingApproval.PerformCallback('Status~' + rowvalue);
    //cgridPendingApproval.GetRowValues(itemIndex, 'ID', OnGetApprovedRowValues);

}
function OnGetApprovedRowValues(obj) {
    uri = "PurchaseIndent.aspx?key=" + obj + "&status=2";
    popup.SetContentUrl(uri);
    popup.Show();
}
// function above  End For Approved

// Status 3 is passed If Approved Check box is checked by User Both Below function is called and used to show in POPUP,  the Add Page of Respective Segment(like Page for Adding Quotation ,Sale Order ,Challan)
function GetRejectedQuoteId(s, e, itemIndex) {

    cgridPendingApproval.GetRowValues(itemIndex, 'ID', OnGetRejectedRowValues);

}
function OnGetRejectedRowValues(obj) {
    uri = "PurchaseIndent.aspx?key=" + obj + "&status=3";
    popup.SetContentUrl(uri);
    popup.Show();
}
// function above  End For Rejected

// To Reflect the Data in Pending Waiting Grid and Pending Waiting Counting if the user approve or Rejecte the Order and Saved 

function OnApprovalEndCall(s, e) {
    $.ajax({
        type: "POST",
        url: "PurchaseIndent.aspx/GetPendingCase",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $('#lblWaiting').text(data.d);
        }
    });
}

// function above  End 

// To Hide the Popup and Refresh the Data in Pending Waiting Grid 


function ApprovalSettingstatus(key) {
    if ($("#hdnPageStatForApprove").val() == "PO" && $("#hdnApprovalReqInq").val() == "1") //&& $("#hdnApprovalReqInq").val() == "1"	
    {
        document.getElementById("dvReject").style.display = "inline-block";
        document.getElementById("dvApprove").style.display = "inline-block";
        document.getElementById("dvAppRejRemarks").style.display = "block";
    }
    if ($("#hdnPageStatForApprove").val() == "PO" && $("#hdnApprovalReqInq").val() == "1") // && $("#hdnApprovalReqInq").val() == "1"	
    {
        var det = {};
        det.Id = key;
        if ($("#hdnEditOrderId").val() != "") {
            $.ajax({
                type: "POST",
                url: "PurchaseIndent.aspx/GetApproveRejectStatus",
                data: JSON.stringify(det),
                contentType: "application/json; charset=utf-8",
                async: false,
                dataType: "json",
                //async:false,	
                success: function (msg) {
                    var statusValueforApproval = msg.d;
                    if (statusValueforApproval == 1) {
                        document.getElementById("dvRevisionDate").style.display = "block";
                        document.getElementById("dvRevision").style.display = "block";
                        document.getElementById("dvAppRejRemarks").style.display = "block";
                        document.getElementById("dvReject").style.display = "none";
                        document.getElementById("dvApprove").style.display = "none";
                        document.getElementById("btnnew").style.display = "none";
                        document.getElementById("btnSaveExit").style.display = "none"; 
                        document.getElementById("taggedApproval").style.display = "block";
                        //$("#taggedApproval").val("***Already Approved.")
                    }
                    else if (statusValueforApproval == 2) {
                        document.getElementById("dvAppRejRemarks").style.display = "block";
                        document.getElementById("dvReject").style.display = "none";
                        document.getElementById("dvApprove").style.display = "inline-block";
                        document.getElementById("btnnew").style.display = "none";
                        document.getElementById("btnSaveExit").style.display = "none";

                    }
                    else if (statusValueforApproval == 0) {
                        document.getElementById("dvAppRejRemarks").style.display = "block";
                        document.getElementById("dvReject").style.display = "inline-block";
                        document.getElementById("dvApprove").style.display = "inline-block";
                        document.getElementById("btnnew").style.display = "none";
                        document.getElementById("btnSaveExit").style.display = "none";
                    }
                }
            });
        }
    }
    if ($("#hdnPageStatus").val() == "update" && $("#hdnApprovalReqInq").val() == "1") // && $("#hdnApprovalReqInq").val() == "1"	
    {
        var detApp = {};
        detApp.Id = key;
        $.ajax({
            type: "POST",
            url: "PurchaseIndent.aspx/GetApproveRejectStatus",
            data: JSON.stringify(detApp),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (msg) {
                statusValueforRejectApproval = msg.d;
                if (statusValueforRejectApproval == 1) {
                    document.getElementById("dvRevisionDate").style.display = "block";
                    document.getElementById("dvRevision").style.display = "block";
                    //ctxtRevisionDate.SetMinDate(cPLSalesOrderDate.GetDate());	
                    //ctxtRevisionDate.SetMinDate(new Date(cPLQuoteDate.GetDate().toDateString()))	
                }
            }
        });
    }
}


$(document).ready(function () {
    $('#ApprovalCross').click(function () {

        //window.popup.Hide();
        //cgridPendingApproval.Refresh();
        window.parent.popup.Hide();
        window.parent.cgridPendingApproval.PerformCallback();
        window.location.href = 'PurchaseIndent.aspx'
    })
});

// Basic Setting for Approval in Edit Mode this page has the List and Detial Part so to call it
function StartingSetupForApproval(indentid) {
    $('#hdnEditClick').val('T'); //Edit
    $('#hdn_Mode').val('Edit'); //Edit
    //VisibleIndexE = e.visibleIndex;
    $('#lblHeading').text("Modify Purchase Indent/Requisition");
    document.getElementById("divfromTo").style.display = 'none';
    document.getElementById("divfromTo").style.display = 'none';
    document.getElementById('DivEntry').style.display = 'block';
    document.getElementById('DivEdit').style.display = 'none';
    document.getElementById('btnAddNew').style.display = 'none';
    //btncross.style.display = "block";
    InsgridBatch.PerformCallback("ApprovalEdit~" + indentid);
    chkAccount = 1;
    document.getElementById('divNumberingScheme').style.display = 'none';
}

// function above  End 



var UomlengthForQty = 0;
function UomLenthCalculationForQty() {
    InsgridBatch.batchEditApi.StartEdit(globalRowIndex);
    var SLNo = "";
           
    SLNo = InsgridBatch.GetEditor('SrlNo').GetValue();
           
    $.ajax({
        type: "POST",
        url: "PurchaseIndent.aspx/GetQuantityfromSL",
        data: JSON.stringify({ SLNo: SLNo}),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {

            UomlengthForQty = msg.d;

        }
    });
}


//Added By Subhra For UOM 28/02/2019
function QuantityProductsGotFocus(s, e) {



    var ProductID = InsgridBatch.GetEditor('gvColProduct').GetValue();

    var ProductID = (InsgridBatch.GetEditor('gvColProduct').GetText() != null) ? InsgridBatch.GetEditor('gvColProduct').GetText() : "0";
    var strProductName = (InsgridBatch.GetEditor('gvColProduct').GetText() != null) ? InsgridBatch.GetEditor('gvColProduct').GetText() : "0";
    var QuantityValue = (InsgridBatch.GetEditor('gvColQuantity').GetValue() != null) ? InsgridBatch.GetEditor('gvColQuantity').GetValue() : "0";

    var SpliteDetails = ProductID.split("||@||");
    var strProductID = SpliteDetails[0];
    var strDescription = SpliteDetails[1];
    var strUOM = SpliteDetails[2];
    var strUOMstk = SpliteDetails[4];


    strProductName = strDescription;

    var isOverideConvertion = SpliteDetails[12];
    var packing_saleUOM = SpliteDetails[11];
    var sProduct_SaleUom = SpliteDetails[10];
    var sProduct_quantity = SpliteDetails[9];
    var packing_quantity = SpliteDetails[8];

    var slno = (InsgridBatch.GetEditor('SrlNo').GetText() != null) ? InsgridBatch.GetEditor('SrlNo').GetText() : "0";

    //var Indent_Num = (grid.GetEditor('Indent_Num').GetText() != null) ? grid.GetEditor('Indent_Num').GetText() : "0";

    var ConvertionOverideVisible = $('#hdnConvertionOverideVisible').val();
    var ShowUOMConversionInEntry = $('#hdnShowUOMConversionInEntry').val();
    var type = 'add';
    var gridprodqty = parseFloat(InsgridBatch.GetEditor('gvColQuantity').GetText()).toFixed(5);
    var gridPackingQty = '';
    var IsInventory = '';


    //var gridPackingQty = grid.GetEditor('QuoteDetails_PackingQty').GetText();
    //if (SpliteDetails.length == 14) {
    //    if (SpliteDetails[13] == "1") {
    //        IsInventory = 'Yes';
    //    }
    //}



    if (SpliteDetails.length > 13) {
        if (SpliteDetails[13] == "1") {
            IsInventory = 'Yes';
            type = 'add';
            var actionQry = '';
            var IndentDetailsid = InsgridBatch.GetEditor('gvColIndentDetailsId').GetValue();
            actionQry = 'GetPurchaseIndentQty';
            $.ajax({
                type: "POST",
                url: "Services/Master.asmx/GetMultiUOMDetails",
                data: JSON.stringify({ orderid: IndentDetailsid, action: actionQry, module: 'PurchaseIndent', strKey: '' }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {

                    gridPackingQty = msg.d;
                    if (gridPackingQty != "" && gridPackingQty != null) {
                        type = 'edit';
                    }
                    if ($("#hddnMultiUOMSelection").val() == "0") {
                        if (ShowUOMConversionInEntry == "1" && SpliteDetails.length > 1) {
                            ShowUOM(type, "Purchase", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);
                        }
                    }
                }
            });
        }
    }
    else {
        if ($("#hddnMultiUOMSelection").val() == "0") {
            if (ShowUOMConversionInEntry == "1" && SpliteDetails.length > 1) {
                ShowUOM(type, "PurchaseIndent", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);
            }
        }
    }

    if ($("#hddnMultiUOMSelection").val() == "1") {
        InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 5);
        // if ((gridquotationLookup.GetValue() != "") && (gridquotationLookup.GetValue() !=null)) {
        if (InsgridBatch.GetEditor('gvColQuantity').GetValue() != "0.0000") {
            InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 5);
            $("#UOMQuantity").val(InsgridBatch.GetEditor('gvColQuantity').GetValue());
        }
        // }
    }

}
var issavePacking = 0;
function SetDataToGrid(Quantity, packing, PackingUom, PackingSelectUom, productid, slno) {
    issavePacking = 1;
    InsgridBatch.batchEditApi.StartEdit(globalRowIndex);
    InsgridBatch.GetEditor('gvColQuantity').SetValue(Quantity);
    
      setTimeout(function () {
          InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 6);
    }, 400)
       
        }
function SetFoucs() {

}
//End

    var globalRowIndex;
var chkAccount = 0;
function OnMoreInfoClick(keyValue) {
    var URL = "PurchaseOrder.aspx?key=" + keyValue + "&req=V&status=PO&type=PO";
    capcPoDetails.SetContentUrl(URL);
    capcPoDetails.Show();

    document.getElementById('PODetailsCross').style.display = 'Block';
    document.getElementById('ApprovalCross').style.display = 'none';

}
//Code for UDF Control 
function OpenUdf() {
    if (document.getElementById('IsUdfpresent').value == '0') {
        jAlert("UDF not define.");
    }
    else {
        var keyVal = document.getElementById('Keyval_internalId').value;
        var url = '/OMS/management/Master/frm_BranchUdfPopUp.aspx?Type=PI&&KeyVal_InternalID=' + keyVal;
        cUDFpopup.SetContentUrl(url);
        cUDFpopup.Show();
    }
    return true;
}
function acbpCrpUdfEndCall(s, e) {
    if (cacbpCrpUdf.cpUDFPI) {
        if (cacbpCrpUdf.cpUDFPI == "true") {
            InsgridBatch.UpdateEdit();
            cacbpCrpUdf.cpUDFPI = null;
        }
        else {
            jAlert('UDF is set as Mandatory. Please enter values.', 'Alert', function () {
                OpenUdf();
            });
            cacbpCrpUdf.cpUDFPI = null;
        }
    }
}
// End Udf Code

function BtnVisible() {
    document.getElementById('btnSaveExit').style.display = 'none'
    document.getElementById('btnnew').style.display = 'none'
    document.getElementById('tagged').style.display = 'block'

}
function GetVisibleIndex(s, e) {
    globalRowIndex = e.visibleIndex;
}
function InstrumentDateChange() {

    var ExpectedDeliveryDate = new Date(InsgridBatch.GetEditor('ExpectedDeliveryDate').GetValue());
    var requisitionDate = new Date(ctDate.GetValue());
    if (ExpectedDeliveryDate.format('yyyy-MM-dd') != requisitionDate.format('yyyy-MM-dd'))
        if (ExpectedDeliveryDate < requisitionDate) {
            jAlert('Expected Delivery date must be same or later to Requisition Date. Cannot Proceed.', 'Alert', function () {

                InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 9);
                InsgridBatch.GetEditor('ExpectedDeliveryDate').SetValue(null);
            });

        }
}
// Mantis Issue 25235
function ParentCustomerOnClose(newCustId, customerName, Unique) {


    GetObjectID('hdnCustomerId').value = newCustId;

    AspxDirectAddCustPopup.Hide();
    ctxtShipToPartyShippingAdd.SetText('');
    if (newCustId != "") {
        ctxtVendorName.SetText(customerName);
        SetCustomer(newCustId, customerName);
    }

}
function AddVendorClick() {
    var url = '/OMS/management/Master/vendorPopup.html?var=1.1.5.7';
    AspxDirectAddCustPopup.SetContentUrl(url);

    AspxDirectAddCustPopup.RefreshContentUrl();
    AspxDirectAddCustPopup.Show();
}
function VendorButnClick(s, e) {
    var txt = "<table border='1' width=\"100%\" class=\"dynamicPopupTbl\"><tr class=\"HeaderStyle\"><th>Vendor Name</th><th>Unique Id</th></tr><table>";
    document.getElementById("CustomerTable").innerHTML = txt;
    setTimeout(function () { $("#txtCustSearch").focus(); }, 500);
    $('#txtCustSearch').val('');
    $('#CustModel').modal('show');
}
function VendorKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
        s.OnButtonClick(0);
    }
}
function VendorModekkeydown(e) {
    var OtherDetails = {}
    OtherDetails.SearchKey = $("#txtCustSearch").val();
    OtherDetails.BranchID = $('#ddlBranch').val();


    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var HeaderCaption = [];
        HeaderCaption.push("Vendor Name");
        HeaderCaption.push("Unique Id");
        if ($("#txtCustSearch").val() != "") {
            //callonServer("Services/Master.asmx/GetVendorWithOutBranch", OtherDetails, "CustomerTable", HeaderCaption, "customerIndex", "SetCustomer");
            callonServer("Services/Master.asmx/GetVendorWithBranchPO", OtherDetails, "CustomerTable", HeaderCaption, "customerIndex", "SetCustomer");
        }

    }
    else if (e.code == "ArrowDown") {
        if ($("input[customerindex=0]"))
            $("input[customerindex=0]").focus();
    }
}
function SetCustomer(Id, Name) {
    if (Id) {
        $('#CustModel').modal('hide');
        ctxtVendorName.SetText(Name);
        GetObjectID('hdnCustomerId').value = Id;
        $('#MandatorysVendor').attr('style', 'display:none');
        var VendorId = Id;
        GetVendorGSTInFromBillShip(VendorId);

        //  GetContactPerson();
        $('#CustModel').modal('hide');
        cContactPerson.Focus();
        $.ajax({
            type: "POST",
            url: "PurchaseOrder.aspx/GetVendorReletedData",
            data: JSON.stringify({ VendorId: VendorId }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var data = msg.d;
                var strDueDate = data.toString().split('~')[0];
                var strcountryId = data.toString().split('~')[1];
                var strOutstanding = data.toString().split('~')[2];
                var strGSTN = data.toString().split('~')[3];

                if (strDueDate != null) {
                    var DeuDate = strDueDate;
                    var myDate = new Date(DeuDate);
                    var invoiceDate = new Date();
                    var datediff = Math.round((myDate - invoiceDate) / (1000 * 60 * 60 * 24));
                    ctxtCreditDays.SetValue(datediff);
                    cdt_PODue.SetDate(myDate);
                }
                if (strcountryId != null && strcountryId != "") {
                    var CountryID = strcountryId;
                    if (CountryID != "1") {
                        cddl_AmountAre.SetValue("4");
                        cddl_AmountAre.SetEnabled(false);
                        grid.GetEditor('gvColTaxAmount').SetEnabled(true);
                    }
                    else {
                        cddl_AmountAre.SetValue("1");
                        cddl_AmountAre.SetEnabled(true);
                    }
                    if (CountryID == "0") {

                        jAlert('You must enter the default Billing/Shipping Address for selected Vendor to proceed further.');
                        ctxtVendorName.SetText("");
                        GetObjectID('hdnCustomerId').value = "";
                        cddl_AmountAre.SetValue("1");
                        cddl_AmountAre.SetEnabled(true);
                        ctxtVendorName.Focus();
                    }
                    else {
                        GetPurchaseForGstValue();
                    }
                    SetEntityType(VendorId)
                }
                else {
                    cddl_AmountAre.SetValue("1");
                    cddl_AmountAre.SetEnabled(true);
                }

                if (strOutstanding != null) {
                    pageheaderContent.style.display = "block";
                    $("#divOutstanding").attr('style', 'display:block');
                    document.getElementById('lblTotalPayable').innerHTML = strOutstanding;
                }
                else {
                    pageheaderContent.style.display = "none";
                    $("#divOutstanding").attr('style', 'display:none');
                    document.getElementById('lblTotalPayable').innerHTML = '';
                }
                if (strGSTN != null) {
                    $("#divGSTIN").attr('style', 'display:block');
                    document.getElementById('lblGSTIN').innerHTML = strGSTN;
                }
                GetContactPerson();
            }
        });
        var VendorId = $('#hdnCustomerId').val();

        if (VendorId != null && VendorId != "") {

            //cContactPerson.PerformCallback('BindContactPerson~' + VendorId);
            var OtherDetails = {}
            OtherDetails.VendorId = VendorId;
            $.ajax({
                type: "POST",
                url: "PurchaseOrder.aspx/GetContactPerson",
                data: JSON.stringify(OtherDetails),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var returnObject = msg.d;
                    if (returnObject) {
                        SetDataSourceOnComboBox(cContactPerson, returnObject);
                    }
                }
            });

            if ($("#btn_TermsCondition").is(":visible")) {
                callTCspecefiFields_PO(VendorId);
            }
        }
    }
}
// End of Mantis Issue 25235

//...................Shortcut keys.................
var isCtrl = false;
document.onkeydown = function (e) {
    if (event.keyCode == 78 && event.altKey == true) {
        //run code for Ctrl+S -- ie, save!       

    }
    else if (event.keyCode == 88 && event.altKey == true) {
        //run code for Ctrl+X -- ie, Save & Exit! 
        document.getElementById('btnSaveExit').click();
        return false;
    }
    else if (event.keyCode == 73 && event.altKey == true) {
        //(event.keyCode == 120 || )
        //run code for Ctrl+A -- ie, Add New
        if (document.getElementById('DivEntry').style.display != 'block') {
            AddButtonClick();
        }
    }
    else if (event.keyCode == 85 && event.altKey == true) {
        OpenUdf();
    }

}
//...................end............................
function ShowMsgLastCall() {
    if (CgvPurchaseIndent.cpDelete != null) {
        jAlert(CgvPurchaseIndent.cpDelete)
        //CgvPurchaseIndent.PerformCallback();
        //Rev 1.0
        //CgvPurchaseIndent.Refresh();
        updateGridByDate();
        //Rev 1.0 End
        CgvPurchaseIndent.cpDelete = null
    }
}
function viewOnly() {

    if ($('#hdn_Mode').val().toUpperCase() == 'VIEW') {
        $('#DivEntry').find('input, textarea, button, select').attr('disabled', 'disabled');

        InsgridBatch.SetEnabled(false);
        ctDate.SetEnabled(false);
        cbtnnew.SetVisible(false);
        cbtnSaveExit.SetVisible(false);
    }
}


function OnPODetailsClick(key) {
    //VisibleIndexE = e.visibleIndex;	
    //CgvPurchaseIndent.GetRowKey(VisibleIndexE);	
    capcPurchaseOrderList.Show();
    //cgridPOLIst.PerformCallback("BindPOLIst~" + CgvPurchaseIndent.GetRowKey(VisibleIndexE));	
    cgridPOLIst.PerformCallback("BindPOLIst~" + key);
}

function OnClosedClick(keyValue, visibleIndex,ClosedVal) {

     $("#hdnEditOrderId").val(keyValue);
    CgvPurchaseIndent.SetFocusedRowIndex(visibleIndex);
    //var PurOrderNumber = CgvPurchaseIndent.GetRow(CgvPurchaseIndent.GetFocusedRowIndex()).children[15].innerText;
    //CgvPurchaseOrder.SetFocusedRowIndex(visibleIndex);

    if (ClosedVal == "False") {
        jConfirm('Do you want to close the Purchase Indent ?', 'Confirm Dialog', function (r) {
            if (r == true) {
                $('#MandatoryRemarksFeedback1').attr('style', 'display:none;position: absolute; right: -20px; top: 8px;');
                cPopup_Closed.Show();
            }
            else {
                return false;
            }
        });
    }
    else
    {
        jAlert("Purchase Indent is aleady closed.");
    }
}



function CallClosed_save() {
    //debugger;
    var KeyVal = $("#hdnEditOrderId").val();
    var flag = true;

    var Remarks = txtClosed.GetValue();
    if (Remarks == "" || Remarks == null) {
        $('#MandatoryRemarksFeedback1').attr('style', 'display:block;position: absolute; right: -20px; top: 8px;');
        flag = false;
    }
    else {
        $('#MandatoryRemarksFeedback1').attr('style', 'display:none;position: absolute; right: -20px; top: 8px;');
        cPopup_Closed.Hide();
        ClosedSalesOrder(KeyVal, Remarks);
        CgvPurchaseIndent.Refresh();
        


    }
    return flag;
}
function CancelClosed_save() {
    txtClosed.SetValue();
    cPopup_Closed.Hide();
    
}

function ClosedSalesOrder(keyValue, Reason) {
   // debugger;
    $.ajax({
        type: "POST",
        url: "PurchaseIndent.aspx/ClosedPurchaseIndentOnRequest",
        data: JSON.stringify({ keyValue: keyValue, Reason: Reason }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,//Added By:Subhabrata
        success: function (msg) {
            //debugger;
            var status = msg.d;
            if (status == "1") {
                jAlert("Purchase Indent is closed successfully.");
                //cGrdOrder.PerformCallback('BindGrid');

            }
            else if (status == "-1") {
                jAlert("Purchase Indent is not closed. Try again later");
            }
            else if (status == "-2") {
                jAlert("Selected Purchase Indent is tagged in other module. Cannot proceed.");
            }
            else if (status == "-3") {
                jAlert("Purchase Indent is  already closed.");
            }
            else if (status == "-4") {
                jAlert("Purchase Indent is already cancelled. Cannot proceed.");
            }
            else if (status == "-5") {
                //jAlert("No balance quantity available for this order. Cannot proceed.");
                
                jAlert("No balance quantity available for this Indent. Cannot proceed.");
            }
        }
    });
}

function OnEditClick(key, vis, ClosedVal) {
    if (ClosedVal == "False") {
        $('#hdnEditClick').val('T'); //Edit	
        $('#hdn_Mode').val('Edit'); //Edit	
        //VisibleIndexE = e.visibleIndex;	
        //Sandip Section For Approval Section Detail Start	
        // var key = s.GetRowKey(e.visibleIndex);	
        $('#hdngridkeyval').val(key);
        $("#hdnPageStatForApprove").val("");
        $("#hdnPageStatus").val("update");
        $("#hdnEditOrderId").val(key);
        //Sandip Section For Approval Section Detail End	
        $('#lblHeading').text("Modify Purchase Indent/Requisition");
        document.getElementById("divfromTo").style.display = 'none';
        document.getElementById('DivEntry').style.display = 'block';
        document.getElementById('DivEdit').style.display = 'none';
        document.getElementById('btnAddNew').style.display = 'none';
        btncross.style.display = "block";
        ApprovalSettingstatus(key);
        //InsgridBatch.PerformCallback("Edit~" + VisibleIndexE);	
        InsgridBatch.PerformCallback("Edit~" + key);
        chkAccount = 1;
        document.getElementById('divNumberingScheme').style.display = 'none';
        // clookup_Project.gridView.Refresh();	
        //    clookup_Project.gridView.Refresh();
    }
    else
    {
        jAlert("Purchase Indent is already closed.Edit is not allowed");
    }
    //Mantis Issue 25053
    CheckAddOrEdit();
    //End of Mantis Issue 25053
}
//Mantis Issue 24912
function OnCopyClick(key, vis, ClosedVal) {
    $('#hdnEditClick').val('T'); //Edit	
    $('#hdn_Mode').val('Entry'); //Edit	
    $('#Keyval_internalId').val('Add');
    //VisibleIndexE = e.visibleIndex;	
    //Sandip Section For Approval Section Detail Start	
    // var key = s.GetRowKey(e.visibleIndex);	
    $('#hdngridkeyval').val(key);
    $("#hdnPageStatForApprove").val("");
    //$("#hdnPageStatus").val("update");
    $("#hdnPageStatus").val("Copy");
    $("#hdnEditOrderId").val(key);
    $('#lblHeading').text("");
    $('#lblHeading').text("Add Purchase Indent/Requisition");
    document.getElementById("divfromTo").style.display = 'none';
    document.getElementById('DivEntry').style.display = 'block';
    document.getElementById('DivEdit').style.display = 'none';
    document.getElementById('btnAddNew').style.display = 'none';
    btncross.style.display = "block";
    //ApprovalSettingstatus(key);
    //InsgridBatch.PerformCallback("Edit~" + VisibleIndexE);	
    InsgridBatch.PerformCallback("Edit~" + key);
    $("#hdnApproveStatus").val(0);
    chkAccount = 1;
    //document.getElementById('divNumberingScheme').style.display = 'none';
    // clookup_Project.gridView.Refresh();	
    //    clookup_Project.gridView.Refresh();
    
    SetNumberingSchemeDataSource(key);
    $("#txtVoucherNo").val("");
    $("#txtVoucherNo").text("");
    document.getElementById("txtVoucherNo").value = '';
    document.getElementById("txtVoucherNo").text = '';
    //cgridproducts.cpComponentDetails = null;
    //var ComponentNumber = cgridproducts.cpComponentDetails;
    //alert(ComponentNumber)
    GetProductDetails(key)
    //Mantis Issue 25053
    CheckAddOrEdit();
    //End of Mantis Issue 25053
}
function SetNumberingSchemeDataSource(id) {
    $.ajax({
        type: "POST",
        url: 'PurchaseIndent.aspx/GetNumberingSchemeByType',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ id: id }),
        success: function (msg) {
            var returnObject = msg.d;
            if (returnObject.NumberingSchema) {
                SetDataSourceOnComboBox(cCmbScheme, returnObject.NumberingSchema);
            }
        }
    });
}
function SetDataSourceOnComboBox(ControlObject, Source) {
    ControlObject.ClearItems();
    for (var count = 0; count < Source.length; count++) {
        ControlObject.AddItem(Source[count].Name, Source[count].Id);
    }
    ControlObject.SetSelectedIndex(0);
}
function GetProductDetails(id) {
    $.ajax({
        type: "POST",
        url: 'PurchaseIndent.aspx/GetProductDetails',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ id: id }),
        success: function (msg) {
            var returnObject = msg.d;
                for (var count = 0; count < msg.d.length; count++) {
                    SetProduct(msg.d[count].Id, msg.d[count].Name);
                }
        }
    });
}
//End of Mantis Issue 24912
function OnClickDelete(key, vis, ClosedVal) {
    if (ClosedVal == "False") {
        jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
            if (r == true) {
                //VisibleIndexE = e.visibleIndex;	
                //CgvPurchaseIndent.PerformCallback("Delete~" + VisibleIndexE);	
                CgvPurchaseIndent.PerformCallback("Delete~" + key);
            }
            else {
                return false;
            }
        });
    }
    else
    {
        jAlert("Purchase Indent is already closed.Delete is not allowed");
    }
}
function OnclickApprove(key,vis,ClosedVal) {
    if (ClosedVal == "False") {
        $('#hdnEditClick').val('T'); //Edit	
        $('#hdn_Mode').val('Edit'); //Edit	
        // VisibleIndexE = e.visibleIndex;	
        //Sandip Section For Approval Section Detail Start	
        // var key = s.GetRowKey(e.visibleIndex);	
        $("#hdnEditOrderId").val(key);
        $('#hdngridkeyval').val(key);
        $("#hdnPageStatForApprove").val("PO");
        ApprovalSettingstatus(key);
        //Sandip Section For Approval Section Detail End	
        $('#lblHeading').text("Approve Purchase Indent/Requisition");
        document.getElementById("divfromTo").style.display = 'none';
        document.getElementById('DivEntry').style.display = 'block';
        document.getElementById('DivEdit').style.display = 'none';
        document.getElementById('btnAddNew').style.display = 'none';
        btncross.style.display = "block";
        //InsgridBatch.PerformCallback("Edit~" + VisibleIndexE);	
        InsgridBatch.PerformCallback("Edit~" + key);
        chkAccount = 1;
        document.getElementById('divNumberingScheme').style.display = 'none';
        // clookup_Project.gridView.Refresh();	
        //    clookup_Project.gridView.Refresh();	
    }
    else
    {
        jAlert("Purchase Indent is already closed.Approval is not allowed");
    }
}
function OnclickView(key) {
    $('#hdnEditClick').val('T'); //Edit	
    // VisibleIndexE = e.visibleIndex;	
    //var key = s.GetRowKey(e.visibleIndex);	
    $("#hdnEditOrderId").val(key);
    document.getElementById("divfromTo").style.display = 'none';
    $('#lblHeading').text("View Purchase Indent/Requisition");
    document.getElementById('DivEntry').style.display = 'block';
    //document.getElementById('btnAddNew').style.display = 'none';	
    document.getElementById('DivEdit').style.display = 'none';
    document.getElementById('btnAddNew').style.display = 'none';
    btncross.style.display = "block";
    $('#hdn_Mode').val('View');
    //InsgridBatch.PerformCallback("View~" + VisibleIndexE);	
    InsgridBatch.PerformCallback("View~" + key);
    //LoadingPanel.Show();	
    chkAccount = 1;
    document.getElementById('divNumberingScheme').style.display = 'none';
    // clookup_Project.gridView.Refresh();	
    //   clookup_Project.gridView.Refresh();	
    //Mantis Issue 25053
    CheckAddOrEdit();
    //End of Mantis Issue 25053
}
function onPrintClickJv(key) {
    // var keyValueindex = s.GetRowKey(e.visibleIndex);	
    //onPrintJv(keyValueindex);	
    //onPrintJv(key);	
    PIndentId = key;
    cDocumentsPopup.Show();
    cCmbDesignName.SetSelectedIndex(0);
    cSelectPanel.PerformCallback('Bindalldesignes');
    $('#btnOK').focus();
}


function CustomButtonClick(s, e) {
    if (e.buttonID == 'CustomBtnPO') {
        VisibleIndexE = e.visibleIndex;
        CgvPurchaseIndent.GetRowKey(VisibleIndexE);
        capcPurchaseOrderList.Show();
        cgridPOLIst.PerformCallback("BindPOLIst~" + CgvPurchaseIndent.GetRowKey(VisibleIndexE));
    }
    if (e.buttonID == 'CustomBtnEdit') {
        $('#hdnEditClick').val('T'); //Edit
        $('#hdn_Mode').val('Edit'); //Edit
        VisibleIndexE = e.visibleIndex;
        //Sandip Section For Approval Section Detail Start
        var key = s.GetRowKey(e.visibleIndex);
        $('#hdngridkeyval').val(key);
        $("#hdnPageStatForApprove").val("");
        $("#hdnPageStatus").val("update");
        $("#hdnEditOrderId").val(key);

        //Sandip Section For Approval Section Detail End
        $('#lblHeading').text("Modify Purchase Indent/Requisition");
        document.getElementById("divfromTo").style.display = 'none';
        document.getElementById('DivEntry').style.display = 'block';
        document.getElementById('DivEdit').style.display = 'none';
        document.getElementById('btnAddNew').style.display = 'none';
        btncross.style.display = "block";
        ApprovalSettingstatus(key);
        InsgridBatch.PerformCallback("Edit~" + VisibleIndexE);
        chkAccount = 1;
        document.getElementById('divNumberingScheme').style.display = 'none';
        // clookup_Project.gridView.Refresh();
        //    clookup_Project.gridView.Refresh();
    }
    if (e.buttonID == 'CustomBtnApprove') {
        $('#hdnEditClick').val('T'); //Edit	
        $('#hdn_Mode').val('Edit'); //Edit	
        VisibleIndexE = e.visibleIndex;
        //Sandip Section For Approval Section Detail Start	
        var key = s.GetRowKey(e.visibleIndex);
        $("#hdnEditOrderId").val(key);
        $('#hdngridkeyval').val(key);
        $("#hdnPageStatForApprove").val("PO");
        ApprovalSettingstatus(key);


        //Sandip Section For Approval Section Detail End	
        $('#lblHeading').text("Approve Purchase Indent/Requisition");
        document.getElementById("divfromTo").style.display = 'none';
        document.getElementById('DivEntry').style.display = 'block';
        document.getElementById('DivEdit').style.display = 'none';
        document.getElementById('btnAddNew').style.display = 'none';
        btncross.style.display = "block";
        InsgridBatch.PerformCallback("Edit~" + VisibleIndexE);
        chkAccount = 1;
        document.getElementById('divNumberingScheme').style.display = 'none';
        // clookup_Project.gridView.Refresh();	
        //    clookup_Project.gridView.Refresh();	
    }
    if (e.buttonID == 'CustomBtnView') {
        $('#hdnEditClick').val('T'); //Edit

        VisibleIndexE = e.visibleIndex;
        var key = s.GetRowKey(e.visibleIndex);
        $("#hdnEditOrderId").val(key);
        document.getElementById("divfromTo").style.display = 'none';
        $('#lblHeading').text("View Purchase Indent/Requisition");
        document.getElementById('DivEntry').style.display = 'block';
        //document.getElementById('btnAddNew').style.display = 'none';
        document.getElementById('DivEdit').style.display = 'none';
        document.getElementById('btnAddNew').style.display = 'none';

        btncross.style.display = "block";
        $('#hdn_Mode').val('View');
        InsgridBatch.PerformCallback("View~" + VisibleIndexE);
        //LoadingPanel.Show();
        chkAccount = 1;
        document.getElementById('divNumberingScheme').style.display = 'none';
        // clookup_Project.gridView.Refresh();
        //   clookup_Project.gridView.Refresh();
    }

   

    if (e.buttonID == 'CustomBtnDelete') {

        jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
            if (r == true) {
                VisibleIndexE = e.visibleIndex;
                CgvPurchaseIndent.PerformCallback("Delete~" + VisibleIndexE);

            }
            else {
                return false;
            }
        });
    }
    else if (e.buttonID == 'CustomBtnPrint') {
        //var keyValueindex = s.GetRowKey(e.visibleIndex);
        //onPrintJv(keyValueindex); 
        //Rev Subhra 0019337 23-01-2019
        //jAlert('The Document Design is under Development...');
        var keyValueindex = s.GetRowKey(e.visibleIndex);
        onPrintJv(keyValueindex);
        //End of Rev Subhra 0019337 23-01-2019
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

}
function UomLostFocus(s,e)
{
    if (($("#hddnMultiUOMSelection").val() == "0")) {
        setTimeout(function () {
            InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 8);
        }, 600)

    }
}

function AutoCalValue(s, e) {



    if (($("#hddnMultiUOMSelection").val() == "1")) {

        //setTimeout(function () {
        UomLenthCalculationForQty();
        //  }, 200)

        InsgridBatch.batchEditApi.StartEdit(globalRowIndex);
        var SLNo = InsgridBatch.GetEditor('SrlNo').GetValue();

        if (UomlengthForQty > 0) {
            var qnty = $("#UOMQuantity").val();
            var QValue = (InsgridBatch.GetEditor('gvColQuantity').GetValue() != null) ? InsgridBatch.GetEditor('gvColQuantity').GetValue() : "0.0000";
            if (QValue != "0.0000" && QValue != qnty) {
                jConfirm('Qunatity Change Will Clear Multiple UOM Details , Confirm ?', 'Confirmation Dialog', function (r) {
                    if (r == true) {
                        InsgridBatch.batchEditApi.StartEdit(globalRowIndex);
                        var tbqty = InsgridBatch.GetEditor('gvColQuantity');
                        //tbqty.SetValue(Quantity);


                        cgrid_MultiUOM.PerformCallback('CheckMultiUOmDetailsQuantity~' + SLNo);

                        
                        setTimeout(function () {
                            InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 6);
                        }, 600)
                        }
                    else {
                        InsgridBatch.batchEditApi.StartEdit(globalRowIndex);
                        InsgridBatch.GetEditor('gvColQuantity').SetValue(qnty);
                        setTimeout(function () {
                            InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 6);
                        }, 400);
                    }


                });
            }
            else {
                InsgridBatch.batchEditApi.StartEdit(globalRowIndex);
                InsgridBatch.GetEditor('gvColQuantity').SetValue(qnty);
               
                setTimeout(function () {
                    InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 6);
                }, 600)
                   
           }
        }

    }



    var Quantity = (InsgridBatch.GetEditor('gvColQuantity').GetValue() != null) ? parseFloat(InsgridBatch.GetEditor('gvColQuantity').GetValue()) : "0";
    var Rate = (InsgridBatch.GetEditor('gvColRate').GetValue() != null) ? parseFloat(InsgridBatch.GetEditor('gvColRate').GetValue()) : "0";
    InsgridBatch.GetEditor('gvColValue').SetValue(Quantity * Rate);

    if (($("#hddnMultiUOMSelection").val() == "1")) {
        setTimeout(function () {
            InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 6);
        }, 600)
    }
}
function AutoCalValueBtRate(s, e) {
    var Quantity = (InsgridBatch.GetEditor('gvColQuantity').GetValue() != null) ? parseFloat(InsgridBatch.GetEditor('gvColQuantity').GetValue()) : "0";
    var Rate = (InsgridBatch.GetEditor('gvColRate').GetValue() != null) ? parseFloat(InsgridBatch.GetEditor('gvColRate').GetValue()) : "0";
    InsgridBatch.GetEditor('gvColValue').SetValue(Quantity * Rate);
}
function txtBillNo_TextChanged() {
    var VoucherNo = document.getElementById("txtVoucherNo").value;

    $.ajax({
        type: "POST",
        url: "PurchaseIndent.aspx/CheckUniqueName",
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
function AddNewRow() {
    InsgridBatch.AddNewRow();
    var noofvisiblerows = InsgridBatch.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
    var tbQuotation = InsgridBatch.GetEditor("SrlNo");
    tbQuotation.SetValue(noofvisiblerows);
}




function AutoPopulateMultiUOM() {
    //debugger;
    var Productdetails = (InsgridBatch.GetEditor('gvColProduct').GetText() != null) ? InsgridBatch.GetEditor('gvColProduct').GetText() : "0";
    var ProductID = Productdetails.split("||@||")[0];
    var QuantityValue = (InsgridBatch.GetEditor('gvColQuantity').GetValue() != null) ? InsgridBatch.GetEditor('gvColQuantity').GetValue() : "0";

    $.ajax({
        type: "POST",
        url: "PurchaseIndent.aspx/AutoPopulateAltQuantity",
        data: JSON.stringify({ ProductID: ProductID }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            //debugger;
            if (msg.d.length != 0) {
                var packingQuantity = msg.d[0].packing_quantity;
                var sProduct_quantity = msg.d[0].sProduct_quantity;
                var AltUOMId = msg.d[0].AltUOMId;
            }
            else {
                var packingQuantity = 0;
                var sProduct_quantity = 0;
                var AltUOMId = 0;
            }
            var uomfactor = 0
            if (sProduct_quantity != 0 && packingQuantity != 0) {
                uomfactor = parseFloat(packingQuantity / sProduct_quantity).toFixed(4);
                $('#hddnuomFactor').val(parseFloat(packingQuantity / sProduct_quantity));
            }
            else {
                $('#hddnuomFactor').val(0);
            }

            var uomfac_Qty_to_stock = $('#hddnuomFactor').val();
            var Qty = QuantityValue;
            var calcQuantity = parseFloat(Qty * uomfac_Qty_to_stock).toFixed(4);
            if ($("#Keyval_internalId").val() != "Add") {
                ccmbSecondUOM.SetValue('');
                //$("#AltUOMQuantity").val(calcQuantity);

                //cAltUOMQuantity.SetValue(calcQuantity);
                cAltUOMQuantity.SetValue("0.0000");
            }
            else {
                //ccmbSecondUOM.SetValue(AltUOMId);
                if (AltUOMId == 0) {
                    ccmbSecondUOM.SetValue('');
                }
                else {
                    ccmbSecondUOM.SetValue(AltUOMId);
                }
                //cAltUOMQuantity.SetValue(calcQuantity);
            }

        }
    });
}

function OnCustomButtonClick(s, e) {
    if (e.buttonID == 'CustomDelete') {
        if (ctaggingList.GetValue() != null) {
            jAlert('Cannot Delete using this button as the MRP is linked with this Purchase Indent.<br/>Click on Plus(+) sign to Add or Delete Product from last column.!', 'Alert Dialog', function (r) {

            });
        }
        if (InsgridBatch.GetVisibleRowsOnPage() > 1 && ctaggingList.GetValue() == null) {
            InsgridBatch.batchEditApi.EndEdit();
            InsgridBatch.DeleteRow(e.visibleIndex);
            $('#hdfIsDelete').val('D');
            InsgridBatch.UpdateEdit();
            InsgridBatch.PerformCallback('Display');
            InsgridBatch.batchEditApi.StartEdit(-1, 2);
            InsgridBatch.batchEditApi.StartEdit(0, 2);
        }
    }



    if (e.buttonID == 'CustomAddNewRow') {
        if (ctaggingList.GetValue() == null) {
            InsgridBatch.batchEditApi.StartEdit(e.visibleIndex, 2);
            var Product = (InsgridBatch.GetEditor('gvColProduct').GetText() != null) ? InsgridBatch.GetEditor('gvColProduct').GetText() : "";
            var Quantity = (InsgridBatch.GetEditor('gvColQuantity').GetValue() != null) ? InsgridBatch.GetEditor('gvColQuantity').GetValue() : "0.0";
            if (Product != "" && Quantity != "0.0") {
                InsgridBatch.AddNewRow();
                var noofvisiblerows = InsgridBatch.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
                var tbQuotation = InsgridBatch.GetEditor("SrlNo");
                tbQuotation.SetValue(noofvisiblerows);
                setTimeout(function () {
                    InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 2);
                }, 500);
                return false;
            }
        }
        else {
            QuotationNumberChanged();
        }
    }

    if (e.buttonID == 'CustomMultiUOM') {
        //debugger;
        var index = e.visibleIndex;
        InsgridBatch.batchEditApi.StartEdit(e.visibleIndex);
        var Productdetails = (InsgridBatch.GetEditor('gvColProduct').GetText() != null) ? InsgridBatch.GetEditor('gvColProduct').GetText() : "0";
        var ProductID = Productdetails.split("||@||")[0];
        var UOMName = InsgridBatch.GetEditor("gvColUOM").GetValue();
        var quantity = InsgridBatch.GetEditor("gvColQuantity").GetValue();
        var StockUOM = Productdetails.split("||@||")[5];
        // Mantis Issue 24428
        hdProductID.value = ProductID;
        // Mantis Issue 24428 END
        if (StockUOM == "") {
            StockUOM = "0";
        }
        //$("#AltUOMQuantity").val(parseFloat(0).toFixed(4));
        cAltUOMQuantity.SetValue("0.0000");
        // Mantis Issue 24428
        // if ((ProductID != "") && (UOMName != "") && (quantity != "0.0000")) {
        if ((ProductID != "") && (UOMName != "")) {
            // Mantis Issue END 24428
            if (StockUOM == "0") {
                jAlert("Main Unit Not Defined.");
            }
            else
            {
                ccmbUOM.SetEnabled(false);
                var index = e.visibleIndex;
                InsgridBatch.batchEditApi.StartEdit(e.visibleIndex);
                //grid.batchEditApi.StartEdit(globalRowIndex);
                var Qnty = InsgridBatch.GetEditor("gvColQuantity").GetValue();
                var SrlNo = (InsgridBatch.GetEditor('SrlNo').GetValue() != null) ? InsgridBatch.GetEditor('SrlNo').GetValue() : "";
                var UomId = InsgridBatch.GetEditor('gvColProduct').GetText().split("||@||")[3];
                ccmbUOM.SetValue(UomId);
                // Mantis Issue 24428
                //$("#UOMQuantity").val(Qnty);
                $("#UOMQuantity").val("0.0000");
                ccmbBaseRate.SetValue(0)
                cAltUOMQuantity.SetValue(0)
                ccmbAltRate.SetValue(0)
                ccmbSecondUOM.SetValue("")
                // End of Mantis Issue 24428
                if ($("#hddnMultiUOMSelection").val() == "1") {
                    cPopup_MultiUOM.Show();
                    cgrid_MultiUOM.cpDuplicateAltUOM = "";
                    AutoPopulateMultiUOM();
                }
                cgrid_MultiUOM.PerformCallback('MultiUOMDisPlay~' + SrlNo);
            }
        }
        else {
            return;
        }
    }

    if (e.buttonID == "addlDesc") {
        var index = e.visibleIndex;
        InsgridBatch.batchEditApi.StartEdit(e.visibleIndex, 4);
        cPopup_InlineRemarks.Show();
        $("#txtInlineRemarks").val('');
        var SrlNo = (InsgridBatch.GetEditor('SrlNo').GetValue() != null) ? InsgridBatch.GetEditor('SrlNo').GetValue() : "";
        var ProductID = (InsgridBatch.GetEditor('gvColProduct').GetText() != null) ? InsgridBatch.GetEditor('gvColProduct').GetText() : "";
        if (ProductID != "") {
            // ccallback_InlineRemarks.PerformCallback('BindRemarks'+'~' + '0'+'~'+'0');
            ccallback_InlineRemarks.PerformCallback('DisplayRemarks' + '~' + SrlNo + '~' + '0');
        }
        else {
            $("#txtInlineRemarks").val('');
        }
        //$("#txtInlineRemarks").focus();
        document.getElementById("txtInlineRemarks").focus();
    }

}


function callback_InlineRemarks_EndCall(s, e) {
    if (ccallback_InlineRemarks.cpDisplayFocus == "DisplayRemarksFocus") {
        setTimeout(function () {
            $("#txtInlineRemarks").focus();
        }, 500);
    }
    else {
        cPopup_InlineRemarks.Hide();
        //setTimeout(function () {
        //    InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 5);
        //}, 700);
    }
}

function FinalRemarks() {
    ccallback_InlineRemarks.PerformCallback('RemarksFinal' + '~' + InsgridBatch.GetEditor('SrlNo').GetValue() + '~' + $('#txtInlineRemarks').val());
    $("#txtInlineRemarks").val('');
    //setTimeout(function () {
    //    InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 5);
    //}, 700);
}

function closeRemarks(s, e) {
    cPopup_InlineRemarks.Hide();
    setTimeout(function () {
        InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 5);
    }, 700);
    //e.cancel = false;
    //ccallback_InlineRemarks.PerformCallback('RemarksDelete'+'~'+InsgridBatch.GetEditor('SrlNo').GetValue()+'~'+$('#txtInlineRemarks').val());
    //cPopup_InlineRemarks.Hide();
    //e.cancel = false;
    // cPopup_InlineRemarks.Hide();
}

//....Tab Index Change From Rate to Grid First Column......



// Mantis Issue 24428 
function CalcBaseQty() {
    //var PackingQtyAlt = Productdetails.split("||@||")[8];  // Alternate UOM selected from Product Master (tbl_master_product_packingDetails.packing_quantity)
    //var PackingQty = Productdetails.split("||@||")[9];  // Alternate UOM selected from Product Master (tbl_master_product_packingDetails.sProduct_quantity)
    //var PackingSaleUOM = Productdetails.split("||@||")[11];  // Alternate UOM selected from Product Master (tbl_master_product_packingDetails.packing_saleUOM)

    var Productdetails = (InsgridBatch.GetEditor('gvColProduct').GetText() != null) ? InsgridBatch.GetEditor('gvColProduct').GetText() : "0";
    var ProductID = Productdetails.split("||@||")[0];
    var PackingQtyAlt = 0;
    var PackingQty = 0;
    var PackingSaleUOM = 0;
    
    
    $.ajax({
        type: "POST",
        url: "PurchaseIndent.aspx/AutoPopulateAltQuantity",
        data: JSON.stringify({ ProductID: ProductID }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
           
            if (msg.d.length != 0) {
                 PackingQtyAlt = msg.d[0].packing_quantity;  // Alternate UOM selected from Product Master (tbl_master_product_packingDetails.packing_quantity)
                 PackingQty = msg.d[0].sProduct_quantity;  // Alternate UOM selected from Product Master (tbl_master_product_packingDetails.sProduct_quantity)
                 PackingSaleUOM = msg.d[0].AltUOMId;  // Alternate UOM selected from Product Master (tbl_master_product_packingDetails.packing_saleUOM)
            }
            else {
                PackingQtyAlt = 0;  // Alternate UOM selected from Product Master (tbl_master_product_packingDetails.packing_quantity)
                PackingQty = 0;  // Alternate UOM selected from Product Master (tbl_master_product_packingDetails.sProduct_quantity)
                PackingSaleUOM = 0;  // Alternate UOM selected from Product Master (tbl_master_product_packingDetails.packing_saleUOM)
            }

            if (PackingQtyAlt == "") {
                PackingQtyAlt = 0
            }
            if (PackingQty == "") {
                PackingQty = 0
            }

            // if Base UOM of product is not same as the Alternate UOM selected from Product Master, then Calculation of Base Quantity will not happen
            if (ccmbSecondUOM.GetValue() != PackingSaleUOM) {
                PackingQtyAlt = 0;
                PackingQty = 0;
            }
            
            var BaseQty = 0
            if (PackingQtyAlt > 0) {
                var ConvFact = PackingQty / PackingQtyAlt;
                var altQty = cAltUOMQuantity.GetValue();

                if (ConvFact > 0) {
                    var BaseQty = (altQty * ConvFact).toFixed(4);
                    $("#UOMQuantity").val(BaseQty);
                }
            }
            else {
                $("#UOMQuantity").val("0.0000");
            }
        }
    });
    
}

function CalcBaseRate() {
    var altQty = cAltUOMQuantity.GetValue();
    var altRate = ccmbAltRate.GetValue();
    var baseQty = $("#UOMQuantity").val();


    if (baseQty > 0) {
        var BaseRate = (altQty * altRate) / baseQty;
        ccmbBaseRate.SetValue(BaseRate);
    }
}
// End of Mantis Issue 24428 
$(document).ready(function () {
    $('#txtMemoPurpose_I').blur(function () {
        if (InsgridBatch.GetVisibleRowsOnPage() == 1) {
            InsgridBatch.batchEditApi.StartEdit(-1, 2);
        }
    })
});
//.....end..........

function SetArrForUOM() {
    if (aarr.length == 0) {
        for (var i = -500; i < 500; i++) {
            if (InsgridBatch.GetRow(i) != null) {

                var ProductID = (InsgridBatch.batchEditApi.GetCellValue(i, 'gvColProduct') != null) ? InsgridBatch.batchEditApi.GetCellValue(i, 'gvColProduct') : "0";
                if (ProductID != "0") {
                    //var Indent_Num = (InsgridBatch.GetEditor('Indent_Num').GetText() != null) ? InsgridBatch.GetEditor('Indent_Num').GetText() : "";
                    var actionQry = 'GetPurchaseIndentQty';
                    //if ($("#hdAddOrEdit").val() == "Edit") {

                    var SpliteDetails = ProductID.split("||@||");
                    var strProductID = SpliteDetails[0];
                    var orderid = InsgridBatch.batchEditApi.GetCellValue(i, 'gvColIndentDetailsId');//InsgridBatch.GetRowKey(i);
                    var slnoget = InsgridBatch.batchEditApi.GetCellValue(i, 'SrlNo');
                    var Quantity = InsgridBatch.batchEditApi.GetCellValue(i, 'gvColQuantity');
                    $.ajax({
                        type: "POST",
                        url: "Services/Master.asmx/GetMultiUOMDetails",
                        data: JSON.stringify({ orderid: orderid, action: actionQry, module: 'PurchaseIndent', strKey: '' }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,
                        success: function (msg) {

                            gridPackingQty = msg.d;

                            if (msg.d != "") {
                                var packing = SpliteDetails[8];
                                var PackingUom = SpliteDetails[11];
                                var PackingSelectUom = SpliteDetails[10];
                                var arrobj = {};
                                arrobj.productid = strProductID;
                                arrobj.slno = slnoget;
                                arrobj.Quantity = Quantity;
                                arrobj.packing = gridPackingQty;
                                arrobj.PackingUom = PackingUom;
                                arrobj.PackingSelectUom = PackingSelectUom;

                                aarr.push(arrobj);
                                //alert();
                            }
                        }
                    });
                    //}
                }
            }
        }

    }
}


function Save_ButtonClick() {
    cLoadingPanelCRP.Show();

    if ($("#hdnEditOrderId").val() != "") {
        var det = {};
        det.Id = $("#hdnEditOrderId").val();
        $.ajax({
            type: "POST",
            url: "PurchaseIndent.aspx/GetApproveRejectStatus",
            data: JSON.stringify(det),
            contentType: "application/json; charset=utf-8",
            async: false,
            dataType: "json",
            //async:false,	
            success: function (msg) {
                var statusValueforApproval = msg.d;
                $("#hdnApproveStatus").val(statusValueforApproval);
            }
        });
    }
    var revdate = ctxtRevisionDate.GetText();
    var RevisionDate = new Date(revdate);
    if ($("#hdnApproveStatus").val() == 1 && $("#hdnPageStatus").val() == "update" && $("#hdnApprovalReqInq").val() == "1")  // && $("#hdnApprovalReqInq").val() == "1"	
    {
        if (revdate == "01-01-0100" || revdate == null || revdate == "") {

            cLoadingPanelCRP.Hide();
            jAlert("Please Enter Revision Details.");
            ctxtRevisionDate.SetFocus();
            return false;
        }
    }
    if ($("#hdnApproveStatus").val() == 1 && $("#hdnPageStatus").val() == "update" && $("#hdnApprovalReqInq").val() == "1") //&& $("#hdnApprovalReqInq").val() == "1"	
    {
        if (ctxtRevisionNo.GetText() == "") {

            cLoadingPanelCRP.Hide();
            jAlert("Please Enter Revision Details.");
            ctxtRevisionNo.SetFocus();
            return false;
        }
    }
    if ($("#hdnApproveStatus").val() == 1 && $("#hdnPageStatus").val() == "update" && $("#hdnApprovalReqInq").val() == "1")  //&& $("#hdnApprovalReqInq").val() == "1"	
    {
        //if(ctxtRevisionNo.GetText()=="")	
        //{	
        var detRev = {};
        detRev.RevNo = ctxtRevisionNo.GetText();
        detRev.Order = $("#hdnEditOrderId").val();
        $.ajax({
            type: "POST",
            url: "PurchaseIndent.aspx/Duplicaterevnumbercheck",
            data: JSON.stringify(detRev),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (msg) {
                var duplicateRevCheck = msg.d;
                if (duplicateRevCheck == 1) {
                    //flag = false;	
                    cLoadingPanelCRP.Hide();
                    jAlert("Please Enter a valid Revision No");

                    ctxtRevisionNo.SetFocus();
                    return false;
                }
            }
        });

    }

    if ($("#hdnProjectSelectInEntryModule").val() == "1" && $("#hdnProjectMandatory").val() == "1" && ProjectCode == "") {
        jAlert("Please Select Project.");
        cLoadingPanelCRP.Hide();
        return false;
    }

    $('#hdnSaveNew').val("Save_New");
    $('#hdfIsDelete').val('I');
    $('#hdnRefreshType').val('S');
    if (document.getElementById('txtVoucherNo').value == "") {
        $("#MandatoryBillNo").show();
        cLoadingPanelCRP.Hide();
        return false;
    }
    var IsType = "";
    var frontRow = 0;
    var backRow = -1;
    for (var i = 0; i <= InsgridBatch.GetVisibleRowsOnPage() ; i++) {
        var frontProduct = "";
        var backProduct = "";
        frontProduct = (InsgridBatch.batchEditApi.GetCellValue(backRow, 'ProductName') != null) ? (InsgridBatch.batchEditApi.GetCellValue(backRow, 'ProductName')) : "";
        backProduct = (InsgridBatch.batchEditApi.GetCellValue(frontRow, 'ProductName') != null) ? (InsgridBatch.batchEditApi.GetCellValue(frontRow, 'ProductName')) : "";

        if (frontProduct != "" || backProduct != "") {
            IsType = "Y";
            break;
        }
        backRow--;
        frontRow++;
    }
    SetArrForUOM(); //Surojit For UOM EDIT

    //Rev Subhra 01-03-2019
    if (issavePacking == 1) {
        if (aarr.length > 0) {
            $.ajax({
                type: "POST",
                url: "PurchaseIndent.aspx/SetSessionPacking",
                data: "{'list':'" + JSON.stringify(aarr) + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {

                }
            });
        }
    }
    else {
        if (aarr.length > 0) {
            $.ajax({
                type: "POST",
                url: "PurchaseIndent.aspx/SetSessionPacking",
                data: "{'list':'" + JSON.stringify(aarr) + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {

                }
            });
        }
    }
    //End of Rev
    if (InsgridBatch.GetVisibleRowsOnPage() > 0) {
        if (IsType == "Y") {
            cacbpCrpUdf.PerformCallback();
        }
        else {
            jAlert('Cannot Save. You must enter atleast one Product to save this entry.');
            cLoadingPanelCRP.Hide();
        }
    }
    else {
        jAlert('Cannot Save. You must enter atleast one Product to save this entry.');
        cLoadingPanelCRP.Hide();
    }
    chkAccount = 0;
    return false;
}


function Approve_ButtonClick() {
    cLoadingPanelCRP.Show();
    InsgridBatch.AddNewRow();

    $("#hdnApprovalRemarksValue").val(ctxtAppRejRemarks.GetText());
    if ($("#hdnPageStatForApprove").val() == "PO" && $("#hdnApprovalReqInq").val() == "1")  //&& $("#hdnApprovalReqInq").val() == "1"	
    {
        if (ctxtAppRejRemarks.GetText() == "") {
            cLoadingPanelCRP.Hide();
            jAlert("Please Enter Approval Remarks.")
            $("#txtAppRejRemarks").focus();
            return false;
        }
    }
    $("#hdnApproveStatus").val(1);
    var ProjectCode = clookup_Project.GetText();
    if ($("#hdnProjectSelectInEntryModule").val() == "1" && $("#hdnProjectMandatory").val() == "1" && ProjectCode == "") {
        jAlert("Please Select Project.");
        cLoadingPanelCRP.Hide();
        return false;
    }
    $('#hdnSaveNew').val("Save_Exit");
    var a = $('#hdnEditIndentID').val();
    $('#hdnRefreshType').val('E');
    $('#hdfIsDelete').val('I');
    if (document.getElementById('txtVoucherNo').value == "") {
        $("#MandatoryBillNo").show();
        cLoadingPanelCRP.Hide();
        return false;
    }
    var IsType = "";
    var frontRow = 0;
    var backRow = -1;
    for (var i = 0; i <= InsgridBatch.GetVisibleRowsOnPage() ; i++) {
        var frontProduct = "";
        var backProduct = "";
        frontProduct = (InsgridBatch.batchEditApi.GetCellValue(backRow, 'ProductName') != null) ? (InsgridBatch.batchEditApi.GetCellValue(backRow, 'ProductName')) : "";
        backProduct = (InsgridBatch.batchEditApi.GetCellValue(frontRow, 'ProductName') != null) ? (InsgridBatch.batchEditApi.GetCellValue(frontRow, 'ProductName')) : "";
        if (frontProduct != "" || backProduct != "") {
            IsType = "Y";
            break;
        }
        backRow--;
        frontRow++;
    }
    SetArrForUOM(); //Surojit For UOM EDIT	
    //Rev Subhra 01-03-2019	
    if (issavePacking == 1) {
        if (aarr.length > 0) {
            $.ajax({
                type: "POST",
                url: "PurchaseIndent.aspx/SetSessionPacking",
                data: "{'list':'" + JSON.stringify(aarr) + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                }
            });
        }
    }
    //End of Rev	
    if (InsgridBatch.GetVisibleRowsOnPage() > 0) {
        if (IsType == "Y") {
            cacbpCrpUdf.PerformCallback();
        }
        else {
            jAlert('Cannot Save. You must enter atleast one Product to save this entry.');
            cLoadingPanelCRP.Hide();
        }
    }
    else {
        jAlert('Cannot Save. You must enter atleast one Product to save this entry.');
        cLoadingPanelCRP.Hide();
    }
    chkAccount = 0;
    cLoadingPanelCRP.Hide();
    return false;
}
function Reject_ButtonClick() {
    if ($("#hdnPageStatForApprove").val() == "PO" && $("#hdnApprovalReqInq").val() == "1")  // && $("#hdnApprovalReqInq").val() == "1"	
    {
        if (ctxtAppRejRemarks.GetText() == "") {
            cLoadingPanelCRP.Hide();
            jAlert("Please Enter Reject Remarks.")
            $("#txtAppRejRemarks").focus();
            return false;
        }
    }
    var otherdet = {};
    otherdet.ApproveRemarks = ctxtAppRejRemarks.GetText();//$("#txtAppRejRemarks").val();
    otherdet.ApproveRejStatus = 2;
    otherdet.OrderId = $("#hdnEditOrderId").val();
    $.ajax({
        type: "POST",
        url: "PurchaseIndent.aspx/SetApproveReject",
        data: JSON.stringify(otherdet),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var value = msg.d;
            if (value == "1") {
                jAlert("Indent Rejected.");
                // window.location.href = "PurchaseIndent.aspx";	
                window.location.assign("PurchaseIndent.aspx");
            }
        }
    });
}

function SaveExitButtonClick()  {
    cLoadingPanelCRP.Show();
    InsgridBatch.AddNewRow();
    //Mantis Issue 25053
    if ($("#hdnSettings").val() == "Yes" && $('#chkDirectorApprovalRequired').is(':checked') == true) {
        if (cdddlApprovalEmployee.GetValue() == "0") {
            jAlert("Please Select Employee.");
            cLoadingPanelCRP.Hide();
            return false;
        }
        else {
            var val = cdddlApprovalEmployee.GetValue();
            $("#hdnEmployee").val(val);
        }
    }
    //End of Mantis Issue 25053
    $("#hdnApprovalRemarksValue").val(ctxtAppRejRemarks.GetText());
    if ($("#hdnEditOrderId").val() != "") {
        var det = {};
        det.Id = $("#hdnEditOrderId").val();
        $.ajax({
            type: "POST",
            url: "PurchaseIndent.aspx/GetApproveRejectStatus",
            data: JSON.stringify(det),
            contentType: "application/json; charset=utf-8",
            async: false,
            dataType: "json",
            //async:false,	
            success: function (msg) {
                var statusValueforApproval = msg.d;
                $("#hdnApproveStatus").val(statusValueforApproval);
            }
        });
    }
    var revdate = ctxtRevisionDate.GetText();
    var RevisionDate = new Date(revdate);
    if ($("#hdnApproveStatus").val() == 1 && $("#hdnPageStatus").val() == "update" && $("#hdnApprovalReqInq").val() == "1")  // && $("#hdnApprovalReqInq").val() == "1"	
    {
        if (revdate == "01-01-0100" || revdate == null || revdate == "") {

            cLoadingPanelCRP.Hide();
            jAlert("Please Enter Revision Details.");
            ctxtRevisionDate.SetFocus();
            return false;
        }
    }
    if ($("#hdnApproveStatus").val() == 1 && $("#hdnPageStatus").val() == "update" && $("#hdnApprovalReqInq").val() == "1") //&& $("#hdnApprovalReqInq").val() == "1"	
    {
        if (ctxtRevisionNo.GetText() == "") {

            cLoadingPanelCRP.Hide();
            jAlert("Please Enter Revision Details.");
            ctxtRevisionNo.SetFocus();
            return false;
        }
    }
    var RevDuplicate="0"
    if ($("#hdnApproveStatus").val() == 1 && $("#hdnPageStatus").val() == "update" && $("#hdnApprovalReqInq").val() == "1")  //&& $("#hdnApprovalReqInq").val() == "1"	
    {
        //if(ctxtRevisionNo.GetText()=="")	
        //{	
        var detRev = {};
        detRev.RevNo = ctxtRevisionNo.GetText();
        detRev.Order = $("#hdnEditOrderId").val();
        $.ajax({
            type: "POST",
            url: "PurchaseIndent.aspx/Duplicaterevnumbercheck",
            data: JSON.stringify(detRev),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (msg) {
                var duplicateRevCheck = msg.d;
                if (duplicateRevCheck == 1) {
                    //flag = false;
                    RevDuplicate="1"
                    //cLoadingPanelCRP.Hide();
                    //jAlert("Please Enter a valid Revision No");
                    ////ctxtRevisionNo.SetFocus();
                    //return false;
                }
            }
        });
    }


    if ($("#hdnApproveStatus").val() == 1 && $("#hdnPageStatus").val() == "update" && $("#hdnApprovalReqInq").val() == "1" && RevDuplicate=="1")  //&& $("#hdnApprovalReqInq").val() == "1"	
    {
        jAlert("Please Enter a valid Revision No");
        cLoadingPanelCRP.Hide();
        return false;
    }

    var ProjectCode = clookup_Project.GetText();
    if ($("#hdnProjectSelectInEntryModule").val() == "1" && $("#hdnProjectMandatory").val() == "1" && ProjectCode == "") {
        jAlert("Please Select Project.");
        cLoadingPanelCRP.Hide();
        return false;
    }


    $('#hdnSaveNew').val("Save_Exit");
    var a = $('#hdnEditIndentID').val();
    $('#hdnRefreshType').val('E');
    $('#hdfIsDelete').val('I');
    if (document.getElementById('txtVoucherNo').value == "") {
        $("#MandatoryBillNo").show();
        cLoadingPanelCRP.Hide();
        return false;
    }
    var IsType = "";
    var frontRow = 0;
    var backRow = -1;

    for (var i = 0; i <= InsgridBatch.GetVisibleRowsOnPage() ; i++) {
        var frontProduct = "";
        var backProduct = "";
        frontProduct = (InsgridBatch.batchEditApi.GetCellValue(backRow, 'ProductName') != null) ? (InsgridBatch.batchEditApi.GetCellValue(backRow, 'ProductName')) : "";
        backProduct = (InsgridBatch.batchEditApi.GetCellValue(frontRow, 'ProductName') != null) ? (InsgridBatch.batchEditApi.GetCellValue(frontRow, 'ProductName')) : "";

        if (frontProduct != "" || backProduct != "") {
            IsType = "Y";
            break;
        }
        backRow--;
        frontRow++;
    }
    SetArrForUOM(); //Surojit For UOM EDIT

    //Rev Subhra 01-03-2019
    if (issavePacking == 1) {
        if (aarr.length > 0) {
            $.ajax({
                type: "POST",
                url: "PurchaseIndent.aspx/SetSessionPacking",
                data: "{'list':'" + JSON.stringify(aarr) + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {

                }
            });
        }
    }
    //End of Rev
    if (InsgridBatch.GetVisibleRowsOnPage() > 0) {
        if (IsType == "Y") {
            cacbpCrpUdf.PerformCallback();
        }
        else {
            jAlert('Cannot Save. You must enter atleast one Product to save this entry.');
            cLoadingPanelCRP.Hide();
        }
    }
    else {
        jAlert('Cannot Save. You must enter atleast one Product to save this entry.');
        cLoadingPanelCRP.Hide();
    }
    chkAccount = 0;
    cLoadingPanelCRP.Hide();
    return false;
}

function AddBatchNew(s, e) {
    InsgridBatch.batchEditApi.EndEdit();
    var keyCode = ASPxClientUtils.GetKeyCode(e.htmlEvent);
    if (keyCode === 13) {
        var noofvisiblerows = InsgridBatch.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
        var i;
        var cnt = 2;

        InsgridBatch.AddNewRow();
        if (noofvisiblerows == "0") {
            InsgridBatch.AddNewRow();
        }
        InsgridBatch.SetFocusedRowIndex();

        for (i = -1 ; cnt <= noofvisiblerows ; i--) {
            cnt++;
        }

        var tbQuotation = grid.GetEditor("SrlNo");
        tbQuotation.SetValue(cnt);
    }
}


function AddButtonClick() {
    $('#hdn_Mode').val('Entry'); //Entry
    $('#Keyval_internalId').val('Add');
    cCmbScheme.SetValue("0");
    ctxtRate.SetEnabled(false);
    document.getElementById('DivEntry').style.display = 'block';
    document.getElementById('DivEdit').style.display = 'none';
    document.getElementById('btnAddNew').style.display = 'none';
    document.getElementById("divfromTo").style.display = 'none';
    btncross.style.display = "block";
    //btnheadlist.style.display = "none";
    //document.getElementById('drdCashBank').style.display = 'none';
    document.getElementById('txtVoucherNo').disabled = true;
    document.getElementById('txtRate').disabled = true;
    document.getElementById('ddlBranch').disabled = true;
    $('#lblHeading').text("");
    $('#lblHeading').text("Add Purchase Indent/Requisition");
    deleteAllRows();
    InsgridBatch.AddNewRow();
    var noofvisiblerows = InsgridBatch.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
    var tbQuotation = InsgridBatch.GetEditor("SrlNo");
    tbQuotation.SetValue(noofvisiblerows);
    cCmbScheme.Focus();
    //Mantis Issue 25053
    CheckAddOrEdit();
    //End of Mantis Issue 25053
}
//Mantis Issue 25053
function onSmsClickJv(key) {
    $("#assignEmployee").modal('show');
    BindModalEmployee();
    //PIndentId = key;
    $("#hdnIndentId").val(key);
}
function PhoneNoSend() {
    
    $("#assignEmployee").modal('hide');
    var EmployeeId = $("#ddl_DirEmployee").val();
    if (EmployeeId == "0") {
        jAlert("please select an Employee.");
        return false;
    }
    //var PIndentId = $("#hdnIndentId").val();
    jConfirm('Send SMS?', 'Alert', function (r) {
        if (r) {
            $.ajax({
                type: "POST",
                url: "PurchaseIndent.aspx/SendSMSManualNo",
                data: JSON.stringify({ PIndentId: $("#hdnIndentId").val(), EmployeeId: $("#ddl_DirEmployee").val() }),
                async: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    //console.log(response);
                    if (response.d) {
                        if (response.d == "true") {
                            jAlert("SMS sent Successfully.", "Alert", function () {
                            });
                        }
                        else {
                            jAlert(response.d);
                            return
                        }
                    }
                },
                error: function (response) {
                    console.log(response);
                }
            });
        }
    });
}
function CheckAddOrEdit() {
        if ($('#hdnSettings').val() == "Yes" && $('#Keyval_internalId').val() == "Add") {
            $("#divIsDirector").removeClass('hide');
            //$("#onSmsClickJv").removeClass('hide');
        }
        else {
            $("#divIsDirector").addClass('hide');
            //$("#onSmsClickJv").addClass('hide');
        }
}
//End of Mantis Issue 25053
function CmbScheme_ValueChange() {
    var val = cCmbScheme.GetValue();
    $.ajax({
        type: "POST",
        url: 'PurchaseIndent.aspx/getSchemeType',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: "{sel_scheme_id:\"" + val + "\"}",
        success: function (type) {
            var schemetypeValue = type.d;
            var schemetype = schemetypeValue.toString().split('~')[0];
            var schemelength = schemetypeValue.toString().split('~')[1];
            $('#txtVoucherNo').attr('maxLength', schemelength);
            var branchID = schemetypeValue.toString().split('~')[2];

            var fromdate = schemetypeValue.toString().split('~')[3];
            var todate = schemetypeValue.toString().split('~')[4];

            var dt = new Date();

            ctDate.SetDate(dt);

            if (dt < new Date(fromdate)) {
                ctDate.SetDate(new Date(fromdate));
            }

            if (dt > new Date(todate)) {
                ctDate.SetDate(new Date(todate));
            }




            ctDate.SetMinDate(new Date(fromdate));
            ctDate.SetMaxDate(new Date(todate));


            document.getElementById('ddlBranch').value = branchID;
            document.getElementById('ddlBranch').disabled = true;
            if (schemetype == '0') {
                $('#hdnSchemaType').val('0');
                document.getElementById('txtVoucherNo').disabled = false;
                document.getElementById('txtVoucherNo').value = "";
                $('#txtVoucherNo').focus();
            }
            else if (schemetype == '1') {
                $('#hdnSchemaType').val('1');
                document.getElementById('txtVoucherNo').disabled = true;
                document.getElementById('txtVoucherNo').value = "Auto";
                $("#MandatoryBillNo").hide();
                ctDate.Focus();
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
        }
    });

    //clookup_Project.gridView.Refresh();
    clookup_Project.gridView.Refresh();
}


    var preColumn = '';
function gridFocusedRowChanged(s, e) {
    globalRowIndex = e.visibleIndex;
}

function OnGridFocusedRowChanged(s, e) {
    s.GetRowValues(s.GetFocusedRowIndex(), 'Indent_Id;Closed', OnGetRowValues);
}
function OnGetRowValues(values)
{
    var s=values;
}
function ProductsGotFocus(s, e) {
    document.getElementById("pageheaderContent").style.display = 'block';
    //document.getElementById("liToBranch").style.display = 'block';
    var tbDescription = InsgridBatch.GetEditor("gvColDiscription");
    var tbUOM = InsgridBatch.GetEditor("gvColUOM");
    var tdRate = InsgridBatch.GetEditor("gvColRate");
    var AvailableStock = InsgridBatch.GetEditor("gvColAvailableStock");
    var ProductID = (InsgridBatch.GetEditor('gvColProduct').GetValue() != null) ? InsgridBatch.GetEditor('gvColProduct').GetValue() : "0";
    var SpliteDetails = ProductID.split("||@||");
    var strProductID = SpliteDetails[0];
    var strDescription = SpliteDetails[1];
    var strUOM = SpliteDetails[2];
    var strUOMstk = SpliteDetails[4];
    chkAccount = 1;
    tbDescription.SetValue(strDescription);
    tbUOM.SetValue(strUOM);
    $('#lblStkQty').text("0.00");
    $('#lblStkUOM').text(strUOM);
    if (ProductID != "0") {
        cacpAvailableStock.PerformCallback(strProductID);
    }
}
function ProductsGotFocusFromID(s, e) {
    pageheaderContent.style.display = "block";

    var ProductID = (InsgridBatch.GetEditor('gvColProduct').GetValue() != null) ? InsgridBatch.GetEditor('gvColProduct').GetValue() : "0";
    var strProductName = (InsgridBatch.GetEditor('gvColProduct').GetText() != null) ? InsgridBatch.GetEditor('gvColProduct').GetText() : "0";
    var QuantityValue = (InsgridBatch.GetEditor('gvColQuantity').GetValue() != null) ? InsgridBatch.GetEditor('gvColQuantity').GetValue() : "0";
    var tbDescription = InsgridBatch.GetEditor("gvColDiscription");
    var tbUOM = InsgridBatch.GetEditor("gvColUOM");
    var tdRate = InsgridBatch.GetEditor("gvColRate");
    var SpliteDetails = ProductID.split("||@||");
    var strProductID = SpliteDetails[0];
    var strDescription = SpliteDetails[1];
    var strUOM = SpliteDetails[2];
    var strUOMstk = SpliteDetails[4];

    var strRate = SpliteDetails[6];
    chkAccount = 1;
    tbDescription.SetValue(strDescription);
    tbUOM.SetValue(strUOM);
    $('#lblStkQty').text("0.00");
    $('#lblStkUOM').text(strUOM);

    if (ProductID != "0") {
        cacpAvailableStock.PerformCallback(strProductID);
    }
}
function acpAvailableStockEndCall(s, e) {
    if (cacpAvailableStock.cpstock != null) {
        var AvailableStock = cacpAvailableStock.cpstock + " " + document.getElementById('lblStkUOM').innerHTML;
        $('#B_AvailableStock').text(AvailableStock);
        cacpAvailableStock.cpstock = null;
    }
    if (preColumn == "Product") {
        InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 5);
        preColumn = '';
        return;
    }
}
function ProductKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter") {
        s.OnButtonClick(0);
    }
    //if (e.htmlEvent.key == "Tab") {
    //    s.OnButtonClick(0);
    //}
}
function prodkeydown(e) {

    var OtherDetails = {}
    if ($.trim($("#txtProdSearch").val()) == "" || $.trim($("#txtProdSearch").val()) == null) {
        return false;
    }
    OtherDetails.SearchKey = $("#txtProdSearch").val();

    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var HeaderCaption = [];
        HeaderCaption.push("Product Code");
        HeaderCaption.push("Product Name");
        HeaderCaption.push("Inventory");
        HeaderCaption.push("HSN/SAC");
        HeaderCaption.push("Class");
        HeaderCaption.push("Brand");
        if ($("#txtProdSearch").val() != '') {
            callonServer("Services/Master.asmx/GetProductDetailsIndentRequisition", OtherDetails, "ProductTable", HeaderCaption, "ProdIndex", "SetProduct");
        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[ProdIndex=0]"))
            $("input[ProdIndex=0]").focus();
    }
}
function ProductButnClick(s, e) {
    if (e.buttonIndex == 0) {
        setTimeout(function () { $("#txtProdSearch").focus(); }, 500);
        $('#txtProdSearch').val('');
        $('#ProductModel').modal('show');
    }
    //if (e.buttonIndex == 0) {

    //    if (cproductLookUp.Clear()) {
    //        cProductpopUp.Show();
    //        cproductLookUp.Focus();
    //        cproductLookUp.ShowDropDown();
    //    }
    //}
}
function ValueSelected(e, indexName) {
    if (e.code == "Enter") {
        var Id = e.target.parentElement.parentElement.cells[0].innerText;
        var name = e.target.parentElement.parentElement.cells[1].children[0].value;
        if (Id) {
            if (indexName == "ProdIndex") {
                SetProduct(Id, name);
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
            if (indexName == "ProdIndex") {
                $('#txtProdSearch').focus();
            }

        }
    }
}
function ProductlookUpKeyDown(s, e) {
    if (e.htmlEvent.key == "Escape") {
        cProductpopUp.Hide();
        InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 5);
    }
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

function ddlBranch_SelectedIndexChanged() {
    //clookup_Project.gridView.Refresh();
    clookup_Project.gridView.Refresh();
}

function CmbScheme_LostFocus() {
    if ($("#hdnProjectSelectInEntryModule").val() == "1")
        clookup_Project.gridView.Refresh();
}

    $(document).ready(function () {
        //Toggle fullscreen expandEntryGrid
        $("#expandEntryGrid").click(function (e) {
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

                InsgridBatch.SetHeight(300);

                var cntWidth = $this.parent('.makeFullscreen').width();
                InsgridBatch.SetWidth(cntWidth);
            }

        });
        $("#expandCgvPurchaseIndent").click(function (e) {
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


                CgvPurchaseIndent.SetHeight(browserHeight - 150);
                CgvPurchaseIndent.SetWidth(cntWidth);
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


                CgvPurchaseIndent.SetHeight(450);

                var cntWidth = $this.parent('.makeFullscreen').width();
                CgvPurchaseIndent.SetWidth(cntWidth);

            }

        });
        /*Rev 2.0*/
        $("#expandEntryft67").click(function (e) {
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


                CgvPurchaseIndent.SetHeight(browserHeight - 150);
                CgvPurchaseIndent.SetWidth(cntWidth);
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


                CgvPurchaseIndent.SetHeight(450);

                var cntWidth = $this.parent('.makeFullscreen').width();
                CgvPurchaseIndent.SetWidth(cntWidth);

            }

        });
        /*Rev end 2.0*/
    });

    $(document).ready(function () {
        setTimeout(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                InsgridBatch.SetWidth(cntWidth);
                CgvPurchaseIndent.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                InsgridBatch.SetWidth(cntWidth);
                CgvPurchaseIndent.SetWidth(cntWidth);
            }
        },200)

        $('.navbar-minimalize').click(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                InsgridBatch.SetWidth(cntWidth);
                CgvPurchaseIndent.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                InsgridBatch.SetWidth(cntWidth);
                CgvPurchaseIndent.SetWidth(cntWidth);
            }

        });
    });

    function clookupProjectLostFocus(s, e) {
        if (InsgridBatch.GetVisibleRowsOnPage() == 0) {
            OnAddNewClick();
        }

        var projID = clookup_Project.GetValue();
        if (projID == null && projID == "") {
            $("#ddlHierarchy").val(0);
        }
    }
/// Mantis Issue 24428 
    function OnAddNewClick() {
        grid.AddNewRow();
        //grid.GetEditor('SrlNo').SetEnabled(false);

        var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
        var tbQuotation = grid.GetEditor("SrlNo");
        tbQuotation.SetValue(noofvisiblerows);
        /// Mantis Issue 24428 
        $("#UOMQuantity").val(0);
        Uomlength = 0;
        // End of Mantis Issue 24428 
    }
// End of Mantis Issue 24428 

function ProjectEndCallback(s, e) {
    // debugger;
    var projID = clookup_Project.GetValue();

    $.ajax({
        type: "POST",
        url: 'PmsProjectIndent.aspx/getHierarchyID',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ ProjID: projID }),
        success: function (msg) {
            var data = msg.d;
            $("#ddlHierarchy").val(data);
        }
    });
}


function selectValueForRadioBtn() {

    var checked = $("[id$='rdl_Salesquotation']").find(":checked").val();
    if (checked == "MRP") {
        ctaggingList.SetEnabled(true);
    }
    else {
        ctaggingList.SetEnabled(false);
    }

  
    //var fromday = cdtFromDate.GetDate();
    //var today = cdtToDate.GetDate();
    //if (fromday == null || fromday == "") {
    //    jAlert("From date required !", 'Alert Dialog: [Quoation]', function (r) {
    //        if (r == true) {
    //            cdtFromDate.Focus();
              
    //            $('input[type=radio]').prop('checked', false);
    //        }
    //    });

    //    return;

    //}

    //if (today == null || today == "") {
    //    jAlert("To date required !", 'Alert Dialog: [Quoation]', function (r) {
    //        if (r == true) {
    //            cdtToDate.Focus();
            
    //            $('input[type=radio]').prop('checked', false);
    //        }
    //    });

    //    return;

    //}
}
function taggingListKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter") {
        s.OnButtonClick(0);
    }
}

function taggingListButnClick(s, e) {
    ctaggingGrid.PerformCallback('BindComponentGrid');
    cpopup_taggingGrid.Show();
}
function QuotationNumberChanged() {  

    var ComponentIDs= $("#hdnComponent").val();
    var ComponentDetailsIDs = $("#hdnComponentDetailsIDs").val();


    cgridproducts.PerformCallback('BindProductsDetails' + "~" + ComponentIDs + "~" + ComponentDetailsIDs);
    cpopup_taggingGrid.Hide();
    cProductsPopup.Show();
}
function OpenTemplate()
{
    cgridTemplateproducts.PerformCallback('BindTempProductsDetails');
    cProductsPopup.Hide();
    cTemplateProductsPopup.Show();
}
function Tag_ChangeState(value)
{
    ctaggingGrid.PerformCallback('SelectAndDeSelectProducts' + '~' + value);
}
function ChangeState(value) {
    cgridproducts.PerformCallback('SelectAndDeSelectProducts' + '~' + value + "~" + "0");
}
function TemplateChangeState(value) {
    cgridTemplateproducts.PerformCallback('SelectAndDeSelectProducts' + '~' + value);
}
function InventoryItemToGridBind()
{
    var ComponentDetails = _ComponentDetails.split("~");
    cgridproducts.cpComponentDetails = null;
    var ComponentNumber = ComponentDetails[0];
    var ComponentDate = ComponentDetails[1];
    ctaggingList.SetValue(ComponentNumber);

    if ($('#hdn_Mode').val()== 'Edit')
    {
        InsgridBatch.PerformCallback('BindInventoryProductsDetails' + "~" + "BindGridMRPProductsEditMode");
    }
    else {
        InsgridBatch.PerformCallback('BindInventoryProductsDetails' + "~" + "BindGridMRPProducts");
    }



    cProductsPopup.Hide();
   // cTemplateProductsPopup.Hide();
   
}
function gridProducts_EndCallback(s, e) {
    if (cgridproducts.cpComponentDetails) {
        _ComponentDetails = cgridproducts.cpComponentDetails;
        var ComponentDetails = _ComponentDetails.split("~");
        var ComponentNumber = _ComponentDetails[0];
        var ComponentDate = _ComponentDetails[1];
        ctaggingList.SetValue(ComponentNumber);
        cgridproducts.cpComponentDetails = null;
    }
}

function taggingGrid_EndCallback(s, e) {
    if (ctaggingGrid.cpComponentDetails) {
        _ComponentDetails = ctaggingGrid.cpComponentDetails;
        var ComponentDetails = _ComponentDetails.split("~");
        var ComponentNumber = _ComponentDetails[0];
        var ComponentDate = _ComponentDetails[1];
        ctaggingList.SetValue(ComponentNumber);
      //  ctaggingGrid.cpTaggedMRP = null;
        cgridproducts.cpComponentDetails = null;
    }
}

function gridTempProducts_EndCallback(s, e) {
    if (cgridTemplateproducts.cpRtnMsg) {
        cgridTemplateproducts.cpRtnMsg = null;
        jAlert('Can not Tagged Duplicate Product in the Service Template.');
        cgridTemplateproducts.cpRtnMsg = null;
        cProductsPopup.Show();
        cTemplateProductsPopup.Hide();

    }
}
//Mantis Issue 25125
function OnclickViewAttachment(obj) {
    var URL = '/OMS/Management/Activities/EntriesDocuments.aspx?idbldng=' + obj + '&type=PurchaseIndent';
    window.location.href = URL;
}
//End of Mantis Issue 25125