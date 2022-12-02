/*****************
Global variable*/

var ReceiptList = [];
var globalRowindex = 0;
var EditglobalRowindex = 0;
var finalglobalRowindex = 0;
var DocumentList = [];
var canCallBack = true;
var PickedDocument = [];
var saveNewOrExit = '';
var alertShow = false;
var SelectWarehouse = "0";
var textSeparator = ";";
var selectedChkValue = "";
var SelectBatch = "0";
var SelectSerial = "0";
var SelectedWarehouseID = "0";
var RowCount = 0;



$(document).ready(function () {
    var div = document.getElementById('DivLbl');
    div.style.display = 'none';
    if ($("#hdAddEdit").val() == "Add") {
        GridAddnewRow();       
    }

    $('#TechModel').on('shown.bs.modal', function () {        
        $('#txtTechnicianSearch').focus();
    })

    $('#ProductModel').on('shown.bs.modal', function () {
        if ($("input[ProdIndex=0]"))
            $("input[ProdIndex=0]").focus();
    })

    $('#CustModel').on('shown.bs.modal', function () {
        $('#txtCustSearch').focus();
    })

    $('#Segment1Model').on('shown.bs.modal', function () {
        $('#txtSegment1Search').focus();
    })

    $('#Segment2Model').on('shown.bs.modal', function () {
        $('#txtSegment2Search').focus();
    })

    $('#Segment3Model').on('shown.bs.modal', function () {
        $('#txtSegment3Search').focus();
    })

    $('#Segment4Model').on('shown.bs.modal', function () {
        $('#txtSegment4Search').focus();
    })

    $('#Segment5Model').on('shown.bs.modal', function () {
        $('#txtSegment5Search').focus();
    })

    $('#SerTemModel').on('shown.bs.modal', function () {
        $('#txtSerTemSearch').focus();
    })



});
function AddBatchNew(s, e) {
    var keyCode = ASPxClientUtils.GetKeyCode(e.htmlEvent);
    if ((keyCode === 13)) {

        if ($("#hdngridAddEdit").val() != "Edit") {
            Editgrid.batchEditApi.StartEdit(EditglobalRowindex);
            var Product = (Editgrid.GetEditor('ProductName').GetValue() != null) ? Editgrid.GetEditor('ProductName').GetValue() : "";

            if (Product != "") {
                Editgrid.batchEditApi.EndEdit();

                var ComponentID = $("#lblComponentID").text();
                var DocDetailsID = $("#lblDocDetailsID").text();
                document.getElementById("hdnPagecount").value = parseInt($("#hdnPagecount").val()) + 1;
                //RowCount = RowCount + 1;
                Editgrid.AddNewRow();
                Editgrid.GetEditor("SrlNo").SetText(Editgrid.GetVisibleItemsOnPage());
                Editgrid.GetEditor("ComponentID").SetValue(ComponentID);
                Editgrid.GetEditor("DocDetailsID").SetValue(DocDetailsID);
                Editgrid.GetEditor("ActualSL").SetText(parseInt($("#hdnPagecount").val()));
                //Editgrid.GetEditor("ActualSL").SetValue(Editgrid.GetVisibleItemsOnPage());
            }
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

function ValueSelected(e, indexName) {
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var Id = e.target.parentElement.parentElement.cells[0].innerText;
        var name = e.target.parentElement.parentElement.cells[1].children[0].value;
        if (Id) {
            if (indexName == "customerIndex")
                SetCustomer(Id, name);
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
function EditgridGetVisibleIndex(s, e) {
    EditglobalRowindex = e.visibleIndex;
}
function FinalgridGetVisibleIndex(s, e) {
    finalglobalRowindex = e.visibleIndex;
}
function gridFocusedRowChanged(s, e) {
    globalRowindex = e.visibleIndex;
}
function EditgridFocusedRowChanged(s, e) {
    EditglobalRowindex = e.visibleIndex;
}
function FinalgridFocusedRowChanged(s, e) {
    finalglobalRowindex = e.visibleIndex;
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
}

function gridCustomButtonClick(s, e) {
    if (e.buttonID == 'CustomDelete') {

        if (grid.GetVisibleRowsOnPage() >= 1) {
            grid.batchEditApi.StartEdit(e.visibleIndex);
            var ComponentNumber = grid.batchEditApi.GetCellValue(e.visibleIndex, 'ComponentNumber');
            var ProductName = grid.batchEditApi.GetCellValue(e.visibleIndex, 'ProductName');
            var Description = grid.batchEditApi.GetCellValue(e.visibleIndex, 'Description');
            var Quantity = grid.batchEditApi.GetCellValue(e.visibleIndex, 'Quantity');
            var UOM = grid.batchEditApi.GetCellValue(e.visibleIndex, 'UOM');
            var SalePrice = grid.batchEditApi.GetCellValue(e.visibleIndex, 'SalePrice');
            var Amount = grid.batchEditApi.GetCellValue(e.visibleIndex, 'Amount');
            var ProductID = grid.batchEditApi.GetCellValue(e.visibleIndex, 'ProductID');
            var ComponentID = grid.batchEditApi.GetCellValue(e.visibleIndex, 'ComponentID');
            var DocDetailsID = grid.batchEditApi.GetCellValue(e.visibleIndex, 'DocDetailsID');
            var SrlNo = grid.batchEditApi.GetCellValue(e.visibleIndex, 'SrlNo');

            var div = document.getElementById('DivLbl');
            div.style.display = 'block';

            $("#lblComponentNumber").text(ComponentNumber);
            $("#lblProduct").text(ProductName);
            $("#lblDescription").text(Description);
            $("#lblQuantity").text(Quantity);
            $("#lblUOM").text(UOM);
            $("#lblPrice").text(SalePrice);
            $("#lblAmount").text(Amount);
            $("#lblProductID").text(ProductID);
            $("#lblComponentID").text(ComponentID);
            $("#lblDocDetailsID").text(DocDetailsID);
            $("#lblActualSrlID").text(SrlNo);

            $("#hdnlblComponentNumber").val(ComponentNumber);
            $("#hdnlblProduct").val(ProductName);
            $("#hdnlblDescription").val(Description);
            $("#hdnlblQuantity").val(Quantity);
            $("#hdnlblUOM").val(UOM);
            $("#hdnlblPrice").val(SalePrice);
            $("#hdnlblAmount").val(Amount);
            $("#hdnlblProductID").val(ProductID);
            $("#hdnlblComponentID").val(ComponentID);
            $("#hdnlblDocDetailsID").val(DocDetailsID);
            $("#hdnActualSrlID").val(SrlNo);






            //Editgrid.AddNewRow();
            //Editgrid.GetEditor("SrlNo").SetText(Editgrid.GetVisibleItemsOnPage());
            //Editgrid.GetEditor("ComponentID").SetValue(ComponentID);
            //Editgrid.GetEditor("DocDetailsID").SetValue(DocDetailsID);


            //PopOnPicked(grid.GetEditor("DocumentType").GetText() + grid.GetEditor("DocumentId").GetText());
            //grid.DeleteRow(e.visibleIndex);
            //SuffuleSerialNumber();

            cbtnAddSaveRecords.SetVisible(true);

            Editgrid.PerformCallback("BindGridForEdit" + "~" + ComponentID + "~" + DocDetailsID);
        }
    }
    else if (e.buttonID == 'AddNew') {
        GridAddnewRow();
    }
}

var Warehouseindex;
function EditgridCustomButtonClick(s, e) {
    if (e.buttonID == 'EditgridCustomDelete') {
        if (Editgrid.GetVisibleRowsOnPage() > 1) {
            Editgrid.batchEditApi.StartEdit(e.visibleIndex);        

            //PopOnPicked(grid.GetEditor("DocumentType").GetText() + grid.GetEditor("DocumentId").GetText());
            Editgrid.DeleteRow(e.visibleIndex);
            SuffuleSerialNumberEditgrid();
        }
    }
    else if (e.buttonID == 'CustomWarehouse') {
        var index = e.visibleIndex;
        Editgrid.batchEditApi.StartEdit(index, 2);
       // var inventoryType = (document.getElementById("ddlInventory").value != null) ? document.getElementById("ddlInventory").value : "";

      //  if (inventoryType == "C" || inventoryType == "Y") {
            Warehouseindex = index;

            var SrlNo = (Editgrid.GetEditor('SrlNo').GetValue() != null) ? Editgrid.GetEditor('SrlNo').GetValue() : "";
            var ProductID = (Editgrid.GetEditor('ProductID').GetText() != null) ? Editgrid.GetEditor('ProductID').GetText() : "0";
            var QuantityValue = (Editgrid.GetEditor('Quantity').GetValue() != null) ? Editgrid.GetEditor('Quantity').GetValue() : "0";
            var ActualSL = (Editgrid.GetEditor('ActualSL').GetValue() != null) ? Editgrid.GetEditor('ActualSL').GetValue() : "";

            $("#spnCmbWarehouse").hide();
            $("#spnCmbBatch").hide();
            $("#spncheckComboBox").hide();
            $("#spntxtQuantity").hide();

            if (ProductID != "" && parseFloat(QuantityValue) != 0) {
                var SpliteDetails = ProductID.split("||@||");
                var strProductID = SpliteDetails[0];
                var strDescription = SpliteDetails[1];
                var strUOM = SpliteDetails[2];
                var strStkUOM = SpliteDetails[4];
                var strMultiplier = SpliteDetails[7];
                var strProductName = strDescription;
              
                var StkQuantityValue = QuantityValue * strMultiplier;
               
                var Ptype = SpliteDetails[14];
                $('#hdfProductType').val(Ptype);

                document.getElementById('lblProductName').innerHTML = strProductName;
                document.getElementById('txt_SalesAmount').innerHTML = QuantityValue;
                document.getElementById('txt_SalesUOM').innerHTML = strUOM;
                document.getElementById('txt_StockAmount').innerHTML = StkQuantityValue;
                document.getElementById('txt_StockUOM').innerHTML = strStkUOM;

                $('#hdfProductID').val(strProductID);
                $('#hdfProductSerialID').val(ActualSL);
                $('#hdfProductSerialID').val(ActualSL);
                $('#hdnProductQuantity').val(QuantityValue);
                //cacpAvailableStock.PerformCallback(strProductID);

                if (Ptype == "W") {
                    div_Warehouse.style.display = 'block';
                    div_Batch.style.display = 'none';
                    div_Serial.style.display = 'none';
                    div_Quantity.style.display = 'block';
                    cCmbWarehouse.PerformCallback('BindWarehouse');
                    cGrdWarehouse.PerformCallback('Display~' + SrlNo);
                    $("#ADelete").css("display", "block");//Subhabrata
                    SelectedWarehouseID = "0";
                    cPopup_Warehouse.Show();
                }
                else if (Ptype == "B") {
                    div_Warehouse.style.display = 'none';
                    div_Batch.style.display = 'block';
                    div_Serial.style.display = 'none';
                    div_Quantity.style.display = 'block';
                    cCmbBatch.PerformCallback('BindBatch~' + "0");
                    cGrdWarehouse.PerformCallback('Display~' + SrlNo);
                    $("#ADelete").css("display", "block");//Subhabrata
                    SelectedWarehouseID = "0";
                    cPopup_Warehouse.Show();
                }
                else if (Ptype == "S") {
                    div_Warehouse.style.display = 'none';
                    div_Batch.style.display = 'none';
                    div_Serial.style.display = 'block';
                    div_Quantity.style.display = 'none';
                    checkListBox.PerformCallback('BindSerial~' + "0" + '~' + "0");
                    cGrdWarehouse.PerformCallback('Display~' + SrlNo);
                    $("#ADelete").css("display", "none");//Subhabrata
                    SelectedWarehouseID = "0";
                    cPopup_Warehouse.Show();
                }
                else if (Ptype == "WB") {
                    div_Warehouse.style.display = 'block';
                    div_Batch.style.display = 'block';
                    div_Serial.style.display = 'none';
                    div_Quantity.style.display = 'block';
                    cCmbWarehouse.PerformCallback('BindWarehouse');
                    cGrdWarehouse.PerformCallback('Display~' + SrlNo);
                    $("#ADelete").css("display", "block");//Subhabrata
                    SelectedWarehouseID = "0";
                    cPopup_Warehouse.Show();
                }
                else if (Ptype == "WS") {
                    div_Warehouse.style.display = 'block';
                    div_Batch.style.display = 'none';
                    div_Serial.style.display = 'block';
                    div_Quantity.style.display = 'none';
                    cCmbWarehouse.PerformCallback('BindWarehouse');
                    cGrdWarehouse.PerformCallback('Display~' + SrlNo);
                    $("#ADelete").css("display", "none");//Subhabrata
                    SelectedWarehouseID = "0";
                    cPopup_Warehouse.Show();
                }
                else if (Ptype == "WBS") {
                    div_Warehouse.style.display = 'block';
                    div_Batch.style.display = 'block';
                    div_Serial.style.display = 'block';
                    div_Quantity.style.display = 'none';
                    cCmbWarehouse.PerformCallback('BindWarehouse');
                    cGrdWarehouse.PerformCallback('Display~' + SrlNo);
                    $("#ADelete").css("display", "none");//Subhabrata
                    SelectedWarehouseID = "0";
                    cPopup_Warehouse.Show();
                }
                else if (Ptype == "BS") {
                    div_Warehouse.style.display = 'none';
                    div_Batch.style.display = 'block';
                    div_Serial.style.display = 'block';
                    div_Quantity.style.display = 'none';
                    cCmbBatch.PerformCallback('BindBatch~' + "0");
                    cGrdWarehouse.PerformCallback('Display~' + SrlNo);
                    $("#ADelete").css("display", "none");//Subhabrata
                    SelectedWarehouseID = "0";
                    cPopup_Warehouse.Show();
                }
                else {                   

                    jAlert("No Warehouse or Batch or Serial is actived !");
                }
            }
            else if (ProductID != "" && parseFloat(QuantityValue) == 0) {               

                jAlert("Please enter Quantity !");
            }
        //}
        //else {            

        //    jAlert("You have selected Non-Inventory Item, so You cannot updated Stock.");
        //}
    }

    else if (e.buttonID == 'AddNew') {
        GridAddnewRow();
    }
}
function SuffuleSerialNumberEditgrid() {
    var TotRowNumber = Editgrid.GetVisibleItemsOnPage();
    var SlnoCount = 1;
    for (var i = 0; i < 1000; i++) {
        if (Editgrid.GetRow(i)) {
            Editgrid.batchEditApi.StartEdit(i, 2);
            Editgrid.GetEditor("SrlNo").SetText(SlnoCount);
            SlnoCount++;
        }
    }

    for (i = -1; i > -1000; i--) {
        if (Editgrid.GetRow(i)) {
            Editgrid.batchEditApi.StartEdit(i, 2);
            Editgrid.GetEditor("SrlNo").SetText(SlnoCount);
            SlnoCount++;
        }
    }
}
function FinalgridCustomButtonClick(s, e) {
    if (e.buttonID == 'FinalgridgridCustomDelete') {
        if (Finalgrid.GetVisibleRowsOnPage() > 1) {
            Finalgrid.batchEditApi.StartEdit(e.visibleIndex);
            Finalgrid.DeleteRow(e.visibleIndex);
            SuffuleSerialNumberFinalgrid();
        }
    }
    else if (e.buttonID == 'FinalgridgridCustomEdit') {

        EditgridDeleteAllRows();

        //cbtnSaveRecords.SetVisible(false);
        //cbtn_SaveRecords.SetVisible(false);
        cbtnAddSaveRecords.SetVisible(false);
        Finalgrid.batchEditApi.StartEdit(e.visibleIndex);

        $("#hdngridAddEdit").val("Edit");
        $("#hdnVisibleIndex").val(e.visibleIndex);
         
        var ComponentNumber = Finalgrid.GetEditor("ComponentNumber").GetText();
        var _ProductName = Finalgrid.GetEditor("ProductName").GetText();
        var _Description = Finalgrid.GetEditor("Description").GetText();
        var _UOM = Finalgrid.GetEditor("UOM").GetText();
        var _Quantity = (Finalgrid.GetEditor('Quantity').GetValue() != null) ? parseFloat(Finalgrid.GetEditor('Quantity').GetValue()) : "0";
        var _SalePrice = (Finalgrid.GetEditor('SalePrice').GetValue() != null) ? parseFloat(Finalgrid.GetEditor('SalePrice').GetValue()) : "0";
        var _Amount = (Finalgrid.GetEditor('Amount').GetValue() != null) ? parseFloat(Finalgrid.GetEditor('Amount').GetValue()) : "0";
        var _ProductID = Finalgrid.GetEditor("ProductID").GetText();
        var _ComponentID = Finalgrid.GetEditor("ComponentID").GetText();
        var _DocDetailsID = Finalgrid.GetEditor("DocDetailsID").GetText();
        var _ServiceTemplateID = Finalgrid.GetEditor("ServiceTemplateID").GetText();
        var _ActualSL = Finalgrid.GetEditor("ActualSL").GetText();

        if(ComponentNumber=="")
        {
            Editgrid.AddNewRow();
            Editgrid.GetEditor("SrlNo").SetText(e.visibleIndex);
            Editgrid.GetEditor("ActualSL").SetText(_ActualSL);

            Editgrid.GetEditor("ProductName").SetValue(_ProductName);
            Editgrid.GetEditor("Description").SetValue(_Description);
            Editgrid.GetEditor("Quantity").SetValue(_Quantity);
            Editgrid.GetEditor("UOM").SetValue(_UOM);
            Editgrid.GetEditor("SalePrice").SetValue(_SalePrice);
            Editgrid.GetEditor("Amount").SetValue(_Amount);
            Editgrid.GetEditor("ProductID").SetValue(_ProductID);
            Editgrid.GetEditor("ComponentID").SetValue(_ComponentID);
            Editgrid.GetEditor("DocDetailsID").SetValue(_DocDetailsID);
            Editgrid.GetEditor("ServiceTemplateID").SetValue(_ServiceTemplateID);

            //Finalgrid.DeleteRow(e.visibleIndex);
            //SuffuleSerialNumberFinalgrid();


        }
    }



    else if (e.buttonID == 'AddNew') {
        GridAddnewRow();
    }
}

function SuffuleSerialNumberFinalgrid() {
    var TotRowNumber = Finalgrid.GetVisibleItemsOnPage();
    var SlnoCount = 1;
    for (var i = 0; i < 1000; i++) {
        if (Finalgrid.GetRow(i)) {
            Finalgrid.batchEditApi.StartEdit(i, 2);
            Finalgrid.GetEditor("SrlNo").SetText(SlnoCount);
            SlnoCount++;
        }
    }

    for (i = -1; i > -1000; i--) {
        if (Finalgrid.GetRow(i)) {
            Finalgrid.batchEditApi.StartEdit(i, 2);
            Finalgrid.GetEditor("SrlNo").SetText(SlnoCount);
            SlnoCount++;
        }
    }
}

function AllControlInitilize() {
    if (canCallBack) {

        if ($('#hdAddEdit').val() == "Add") {
           
            cCmbScheme.Focus();
        } else {
            $("#hdnPagecount").val(parseInt($("#HiddenRowCount").val()) + 1);
            //showDocumentList();
            //CreateDocumentList();
           // SuffleRows();
            cbtnSaveRecords.SetVisible(false);
            cbtn_SaveRecords.SetVisible(true);
           // ctxtNotes.Focus();
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

function EditgridDeleteAllRows() {
    var TotRowNumber = Editgrid.GetVisibleItemsOnPage();
    for (var i = 0; i < 1000; i++) {
        if (Editgrid.GetRow(i)) {
            Editgrid.DeleteRow(i);
        }
    }

    for (i = -1; i > -1000; i--) {
        if (Editgrid.GetRow(i)) {
            Editgrid.DeleteRow(i);
        }
    }    
}

function ValidateEntry() {
    var ReturnValue = true;

    if (cCmbScheme.GetText() == "-Select-")
    {
        $('#MandatoryNumberingScheme').show();
        return false;        
    } else {
        $('#MandatoryNumberingScheme').hide();
    }

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

    if (gridquotationLookup.GetValue() == null) {
        jAlert('Please enter Contract Number');
        return false;
    }
    


    for (var i = 0; i < 1000; i++) {
        if (Finalgrid.GetRow(i)) {
            if (Finalgrid.GetRow(i).style.display != "none") {
                Finalgrid.batchEditApi.StartEdit(i, 2);
                if (Finalgrid.GetEditor("ProductID").GetText() == "") {
                   
                    jAlert("Please select a valid Product to proceed.");
                    return false;
                }              

            }
        }
    }

    for (i = -1; i > -1000; i--) {
        if (Finalgrid.GetRow(i)) {
            if (Finalgrid.GetRow(i).style.display != "none") {
                Finalgrid.batchEditApi.StartEdit(i, 2);
                if (Finalgrid.GetEditor("ProductID").GetText() == "") {
                    //cLoadingPanelCRP.Hide();
                    jAlert("Please select a valid Product to proceed.");
                    return false;
                }

                if ((Finalgrid.GetEditor("Quantity").GetText() == "0.0000" || Finalgrid.GetEditor("Quantity").GetText() == "")) {
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
        if (!Finalgrid.InCallback()) {
            Finalgrid.AddNewRow();
            Finalgrid.GetEditor("SrlNo").SetText(Finalgrid.GetVisibleItemsOnPage());
            Finalgrid.UpdateEdit();
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
        window.location.href = 'ServiceMaterialIssueAdd.aspx?Key=Add';
        $('#hdAddEdit').val("Add");

    }
    else {
        window.location.href = 'ServiceMaterialIssueList.aspx';
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
    if (Finalgrid.cpBindFinal == "Y") {

        EditgridDeleteAllRows();

         $("#lblComponentNumber").text("");
         $("#lblProduct").text("");
         $("#lblDescription").text("");
         $("#lblQuantity").text("");
         $("#lblUOM").text("");
         $("#lblPrice").text("");
         $("#lblAmount").text("");
         $("#lblProductID").text("");
        $("#lblComponentID").text("");
        $("#lblDocDetailsID").text("");

        Finalgrid.cpBindFinal=null
    }
    else if (Finalgrid.cpErrorCode == "0") {
        jAlert(Finalgrid.cpadjustmentNumber, "Alert", function () { afterSave(); alertShow = false; });
    }
    else {
        jAlert(Finalgrid.cpadjustmentNumber, "Alert", function () { Finalgrid.batchEditApi.StartEdit(-1, 2); Finalgrid.batchEditApi.StartEdit(0, 2); alertShow = false; });
    }
}

function EditGridEndCallBack(s, e) {

    if (Editgrid.cpBindFinalGrid == "Y") {

        Editgrid.cpBindFinalGrid = null;
        Finalgrid.PerformCallback();
    }
    else
    {
        var ComponentID = $("#lblComponentID").text();
        var DocDetailsID = $("#lblDocDetailsID").text();

        //  RowCount = parseInt($("#HiddenRowCount").val());

        if ($("#hdnPagecount").val() != "") {
            document.getElementById("hdnPagecount").value = parseInt($("#hdnPagecount").val()) + 1;
        }
        else {
            $("#hdnPagecount").val(parseInt($("#HiddenRowCount").val()) + 1);
        }

        // RowCount = RowCount + 1;


        Editgrid.AddNewRow();
        Editgrid.GetEditor("SrlNo").SetText(Editgrid.GetVisibleItemsOnPage());
        Editgrid.GetEditor("ComponentID").SetValue(ComponentID);
        Editgrid.GetEditor("DocDetailsID").SetValue(DocDetailsID);
        Editgrid.GetEditor("ActualSL").SetText(parseInt($("#hdnPagecount").val()));
        //Editgrid.GetEditor("ActualSL").SetValue(Editgrid.GetVisibleItemsOnPage());
    }
    



    


}

function gridGridEndCallBack(s, e) {

    if (grid.cpHiddenRowCount) {
        var Row_Count = grid.cpHiddenRowCount;
        $("#HiddenRowCount").val(Row_Count);
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
        if (!Finalgrid.InCallback()) {
            Finalgrid.AddNewRow();
            Finalgrid.GetEditor("SrlNo").SetText(Finalgrid.GetVisibleItemsOnPage());
            Finalgrid.UpdateEdit();
        }
    }
}

function AddButtonClick() {

    if ($("#hdngridAddEdit").val() == "Edit") {
        var VisibleIndex = $("#hdnVisibleIndex").val();
        for (var i = 0; i < 1000; i++) {
            if (Editgrid.GetRow(i)) {
                if (Editgrid.GetRow(i).style.display != "none") {
                    Editgrid.batchEditApi.StartEdit(i, 10);

                    if (Editgrid.GetEditor("ProductID").GetText() != "") {
                       
                        var _ProductName = Editgrid.GetEditor("ProductName").GetText();
                        var _Description = Editgrid.GetEditor("Description").GetText();
                        var _UOM = Editgrid.GetEditor("UOM").GetText();
                        var _Quantity = (Editgrid.GetEditor('Quantity').GetValue() != null) ? parseFloat(Editgrid.GetEditor('Quantity').GetValue()) : "0";
                        var _SalePrice = (Editgrid.GetEditor('SalePrice').GetValue() != null) ? parseFloat(Editgrid.GetEditor('SalePrice').GetValue()) : "0";
                        var _Amount = (Editgrid.GetEditor('Amount').GetValue() != null) ? parseFloat(Editgrid.GetEditor('Amount').GetValue()) : "0";
                        var _ProductID = Editgrid.GetEditor("ProductID").GetText();
                        var _ComponentID = Editgrid.GetEditor("ComponentID").GetText();
                        var _DocDetailsID = Editgrid.GetEditor("DocDetailsID").GetText();
                        var _ServiceTemplateID = Editgrid.GetEditor("ServiceTemplateID").GetText();

                        Finalgrid.batchEditApi.StartEdit(VisibleIndex, 10);
                      
                        Finalgrid.GetEditor("ProductName").SetValue(_ProductName);
                        Finalgrid.GetEditor("Description").SetValue(_Description);
                        Finalgrid.GetEditor("Quantity").SetValue(_Quantity);
                        Finalgrid.GetEditor("UOM").SetValue(_UOM);
                        Finalgrid.GetEditor("SalePrice").SetValue(_SalePrice);
                        Finalgrid.GetEditor("Amount").SetValue(_Amount);
                        Finalgrid.GetEditor("ProductID").SetValue(_ProductID);
                        Finalgrid.GetEditor("ComponentID").SetValue(_ComponentID);
                        Finalgrid.GetEditor("DocDetailsID").SetValue(_DocDetailsID);
                        Finalgrid.GetEditor("ServiceTemplateID").SetValue(_ServiceTemplateID);
                    }
                }
            }
        }

        for (i = -1; i > -1000; i--) {
            if (Editgrid.GetRow(i)) {
                if (Editgrid.GetRow(i).style.display != "none") {
                    Editgrid.batchEditApi.StartEdit(i, 10);
                    if (Editgrid.GetEditor("ProductID").GetText() != "") {
                       
                        var _ProductName = Editgrid.GetEditor("ProductName").GetText();
                        var _Description = Editgrid.GetEditor("Description").GetText();
                        var _UOM = Editgrid.GetEditor("UOM").GetText();
                        var _Quantity = (Editgrid.GetEditor('Quantity').GetValue() != null) ? parseFloat(Editgrid.GetEditor('Quantity').GetValue()) : "0";
                        var _SalePrice = (Editgrid.GetEditor('SalePrice').GetValue() != null) ? parseFloat(Editgrid.GetEditor('SalePrice').GetValue()) : "0";
                        var _Amount = (Editgrid.GetEditor('Amount').GetValue() != null) ? parseFloat(Editgrid.GetEditor('Amount').GetValue()) : "0";
                        var _ProductID = Editgrid.GetEditor("ProductID").GetText();
                        var _ComponentID = Editgrid.GetEditor("ComponentID").GetText();
                        var _DocDetailsID = Editgrid.GetEditor("DocDetailsID").GetText();
                        var _ServiceTemplateID = Editgrid.GetEditor("ServiceTemplateID").GetText();

                        Finalgrid.batchEditApi.StartEdit(VisibleIndex, 10);
                       
                        Finalgrid.GetEditor("ProductName").SetValue(_ProductName);
                        Finalgrid.GetEditor("Description").SetValue(_Description);
                        Finalgrid.GetEditor("Quantity").SetValue(_Quantity);
                        Finalgrid.GetEditor("UOM").SetValue(_UOM);
                        Finalgrid.GetEditor("SalePrice").SetValue(_SalePrice);
                        Finalgrid.GetEditor("Amount").SetValue(_Amount);
                        Finalgrid.GetEditor("ProductID").SetValue(_ProductID);
                        Finalgrid.GetEditor("ComponentID").SetValue(_ComponentID);
                        Finalgrid.GetEditor("DocDetailsID").SetValue(_DocDetailsID);
                        Finalgrid.GetEditor("ServiceTemplateID").SetValue(_ServiceTemplateID);


                        $("#Finalgrid_DXDataRow" + VisibleIndex).addClass(" rowRed");

                    }
                }
            }
        }

        $("#hdngridAddEdit").val("");
        $("#hdnVisibleIndex").val("");
    }
    else {

        //var ComponentNumber = $("#lblComponentNumber").text();
        //var ProductName = $("#lblProduct").text();
        //var Description = $("#lblDescription").text();
        //var Quantity = $("#lblQuantity").text();
        //var UOM = $("#lblUOM").text();
        //var SalePrice = $("#lblPrice").text();
        //var Amount = $("#lblAmount").text();
        //var ProductID = $("#lblProductID").text();
        //var ComponentID = $("#lblComponentID").text();
        //var DocDetailsID = $("#lblDocDetailsID").text();

        //if (ComponentID != "") {
        //    Finalgrid.AddNewRow();
        //    Finalgrid.GetEditor("SrlNo").SetText(Finalgrid.GetVisibleItemsOnPage());

        //    Finalgrid.GetEditor("ComponentNumber").SetValue(ComponentNumber);
        //    Finalgrid.GetEditor("ProductName").SetValue(ProductName);
        //    Finalgrid.GetEditor("Description").SetValue(Description);
        //    Finalgrid.GetEditor("Quantity").SetValue(Quantity);
        //    Finalgrid.GetEditor("UOM").SetValue(UOM);
        //    Finalgrid.GetEditor("SalePrice").SetValue(SalePrice);
        //    Finalgrid.GetEditor("Amount").SetValue(Amount);
        //    Finalgrid.GetEditor("ProductID").SetValue(ProductID);
        //    Finalgrid.GetEditor("ComponentID").SetValue(ComponentID);
        //    Finalgrid.GetEditor("DocDetailsID").SetValue(DocDetailsID);


           // $("#lblComponentNumber").text("");
           // $("#lblProduct").text("");
           // $("#lblDescription").text("");
           // $("#lblQuantity").text("");
           // $("#lblUOM").text("");
           // $("#lblPrice").text("");
           // $("#lblAmount").text("");
           // $("#lblProductID").text("");
            //$("#lblComponentID").text("");
            //$("#lblDocDetailsID").text("");
        //}
        //for (var i = 0; i < 1000; i++) {
        //    if (Editgrid.GetRow(i)) {
        //        if (Editgrid.GetRow(i).style.display != "none") {
        //            Editgrid.batchEditApi.StartEdit(i, 10);

        //            if (Editgrid.GetEditor("ProductID").GetText() != "") {

        //                Finalgrid.AddNewRow();
        //                Finalgrid.GetEditor("SrlNo").SetText(Finalgrid.GetVisibleItemsOnPage());

                       
        //                var _ProductName = Editgrid.GetEditor("ProductName").GetText();
        //                var _Description = Editgrid.GetEditor("Description").GetText();
        //                var _UOM = Editgrid.GetEditor("UOM").GetText();
        //                var _Quantity = (Editgrid.GetEditor('Quantity').GetValue() != null) ? parseFloat(Editgrid.GetEditor('Quantity').GetValue()) : "0";
        //                var _SalePrice = (Editgrid.GetEditor('SalePrice').GetValue() != null) ? parseFloat(Editgrid.GetEditor('SalePrice').GetValue()) : "0";
        //                var _Amount = (Editgrid.GetEditor('Amount').GetValue() != null) ? parseFloat(Editgrid.GetEditor('Amount').GetValue()) : "0";
        //                var _ProductID = Editgrid.GetEditor("ProductID").GetText();
        //                var _ComponentID = Editgrid.GetEditor("ComponentID").GetText();
        //                var _DocDetailsID = Editgrid.GetEditor("DocDetailsID").GetText();
        //                var _ServiceTemplateID = Editgrid.GetEditor("ServiceTemplateID").GetText();

                     
        //                Finalgrid.GetEditor("ProductName").SetValue(_ProductName);
        //                Finalgrid.GetEditor("Description").SetValue(_Description);
        //                Finalgrid.GetEditor("Quantity").SetValue(_Quantity);
        //                Finalgrid.GetEditor("UOM").SetValue(_UOM);
        //                Finalgrid.GetEditor("SalePrice").SetValue(_SalePrice);
        //                Finalgrid.GetEditor("Amount").SetValue(_Amount);
        //                Finalgrid.GetEditor("ProductID").SetValue(_ProductID);
        //                Finalgrid.GetEditor("ComponentID").SetValue(_ComponentID);
        //                Finalgrid.GetEditor("DocDetailsID").SetValue(_DocDetailsID);
        //                Finalgrid.GetEditor("ServiceTemplateID").SetValue(_ServiceTemplateID);
        //            }
        //        }
        //    }
        //}

        //for (i = -1; i > -1000; i--) {
        //    if (Editgrid.GetRow(i)) {
        //        if (Editgrid.GetRow(i).style.display != "none") {
        //            Editgrid.batchEditApi.StartEdit(i, 10);
        //            if (Editgrid.GetEditor("ProductID").GetText() != "") {

        //                Finalgrid.AddNewRow();
        //                Finalgrid.GetEditor("SrlNo").SetText(Finalgrid.GetVisibleItemsOnPage());
                      
        //                var _ProductName = Editgrid.GetEditor("ProductName").GetText();
        //                var _Description = Editgrid.GetEditor("Description").GetText();
        //                var _UOM = Editgrid.GetEditor("UOM").GetText();
        //                var _Quantity = (Editgrid.GetEditor('Quantity').GetValue() != null) ? parseFloat(Editgrid.GetEditor('Quantity').GetValue()) : "0";
        //                var _SalePrice = (Editgrid.GetEditor('SalePrice').GetValue() != null) ? parseFloat(Editgrid.GetEditor('SalePrice').GetValue()) : "0";
        //                var _Amount = (Editgrid.GetEditor('Amount').GetValue() != null) ? parseFloat(Editgrid.GetEditor('Amount').GetValue()) : "0";
        //                var _ProductID = Editgrid.GetEditor("ProductID").GetText();
        //                var _ComponentID = Editgrid.GetEditor("ComponentID").GetText();
        //                var _DocDetailsID = Editgrid.GetEditor("DocDetailsID").GetText();
        //                var _ServiceTemplateID = Editgrid.GetEditor("ServiceTemplateID").GetText();


                   
        //                Finalgrid.GetEditor("ProductName").SetValue(_ProductName);
        //                Finalgrid.GetEditor("Description").SetValue(_Description);
        //                Finalgrid.GetEditor("Quantity").SetValue(_Quantity);
        //                Finalgrid.GetEditor("UOM").SetValue(_UOM);
        //                Finalgrid.GetEditor("SalePrice").SetValue(_SalePrice);
        //                Finalgrid.GetEditor("Amount").SetValue(_Amount);
        //                Finalgrid.GetEditor("ProductID").SetValue(_ProductID);
        //                Finalgrid.GetEditor("ComponentID").SetValue(_ComponentID);
        //                Finalgrid.GetEditor("DocDetailsID").SetValue(_DocDetailsID);
        //                Finalgrid.GetEditor("ServiceTemplateID").SetValue(_ServiceTemplateID);


        //               // $("#Finalgrid_DXDataRow" + finalglobalRowindex).addClass(" rowRed");

        //            }
        //        }
        //    }
        //}


        Editgrid.UpdateEdit();

        //Finalgrid.PerformCallback();
        
        

      
    }
      
   // EditgridDeleteAllRows();
    cbtnAddSaveRecords.SetVisible(false);
    cbtnSaveRecords.SetVisible(true);
    cbtn_SaveRecords.SetVisible(true);
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
    OtherDetails.InventoryType = "Y";

    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var HeaderCaption = [];
        HeaderCaption.push("Product Code");
        HeaderCaption.push("Product Description");
        HeaderCaption.push("Inventory");
        HeaderCaption.push("HSN/SAC");
        HeaderCaption.push("Class");
        HeaderCaption.push("Brand");
        if ($("#txtProdSearch").val() != '') {
            //callonServer("Services/Master.asmx/GetProductForWHStockTransfer", OtherDetails, "ProductTable", HeaderCaption, "ProdIndex", "SetProduct");
            callonServer("Services/Master.asmx/GetProductDetailsForSI", OtherDetails, "ProductTable", HeaderCaption, "ProdIndex", "SetProduct");
        }

    }
    else if (e.code == "ArrowDown") {
        if ($("input[ProdIndex=0]"))
            $("input[ProdIndex=0]").focus();
    }
    else if (e.code == "Escape") {

        Editgrid.batchEditApi.StartEdit(EditglobalRowindex, 2);
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

    Editgrid.batchEditApi.StartEdit(EditglobalRowindex);
    Editgrid.GetEditor("ProductID").SetText(LookUpData);
    Editgrid.GetEditor("ProductName").SetText(ProductCode);
    Editgrid.GetEditor("Description").SetText(ProductDescription);
    Editgrid.GetEditor("UOM").SetText(SaleUOMname);

    Editgrid.batchEditApi.EndEdit();
    Editgrid.batchEditApi.StartEdit(EditglobalRowindex, 4);
}
function RateTextChange(s, e) {
    var QuantityValue = (Editgrid.GetEditor('Quantity').GetValue() != null) ? Editgrid.GetEditor('Quantity').GetValue() : "0";
    var Rate = (Editgrid.GetEditor('SalePrice').GetValue() != null) ? Editgrid.GetEditor('SalePrice').GetValue() : "0";
    var amount = (QuantityValue * Rate);
    Editgrid.GetEditor('Amount').SetValue(amount);

    Editgrid.batchEditApi.EndEdit();
    Editgrid.batchEditApi.StartEdit(EditglobalRowindex, 8);
}

function QuantityTextChange() {

    var QuantityValue = (Editgrid.GetEditor('Quantity').GetValue() != null) ? Editgrid.GetEditor('Quantity').GetValue() : "0";
    var Rate = (Editgrid.GetEditor('SalePrice').GetValue() != null) ? Editgrid.GetEditor('SalePrice').GetValue() : "0";
    var amount = (QuantityValue * Rate);
    Editgrid.GetEditor('Amount').SetValue(amount);

    Editgrid.batchEditApi.EndEdit();
    Editgrid.batchEditApi.StartEdit(EditglobalRowindex, 6);
}


function FinalgridRateTextChange(s, e) {
    var QuantityValue = (Finalgrid.GetEditor('Quantity').GetValue() != null) ? Finalgrid.GetEditor('Quantity').GetValue() : "0";
    var Rate = (Finalgrid.GetEditor('SalePrice').GetValue() != null) ? Finalgrid.GetEditor('SalePrice').GetValue() : "0";
    var amount = (QuantityValue * Rate);
    Finalgrid.GetEditor('Amount').SetValue(amount);

    Finalgrid.batchEditApi.EndEdit();
    Finalgrid.batchEditApi.StartEdit(finalglobalRowindex, 8);
}

function FinalgridQuantityTextChange() {

    var QuantityValue = (Finalgrid.GetEditor('Quantity').GetValue() != null) ? Finalgrid.GetEditor('Quantity').GetValue() : "0";
    var Rate = (Finalgrid.GetEditor('SalePrice').GetValue() != null) ? Finalgrid.GetEditor('SalePrice').GetValue() : "0";
    var amount = (QuantityValue * Rate);
    Finalgrid.GetEditor('Amount').SetValue(amount);

    Finalgrid.batchEditApi.EndEdit();
    Finalgrid.batchEditApi.StartEdit(finalglobalRowindex, 6);
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
function SetCustomer(Id, Name) {
    if (Id) {
        $('#CustModel').modal('hide');
        ctxtCustName.SetText(Name);
        GetObjectID('hdnCustomerId').value = Id;
      
        DeleteAllRows();
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
                else {

                    document.getElementById('DivSegment1').style.display = 'none';
                    document.getElementById('DivSegment2').style.display = 'none';
                    document.getElementById('DivSegment3').style.display = 'none';
                    document.getElementById('DivSegment4').style.display = 'none';
                    document.getElementById('DivSegment5').style.display = 'none';
                }
            }

        });

        var key = GetObjectID('hdnCustomerId').value;
        if (key != null && key != '') {
            cContactPerson.PerformCallback('BindContactPerson~' + key);
        }


        cContactPerson.Focus();
    }
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
function selectValue() {
    var startDate = new Date();
   
    startDate = cdtTDate.GetValueString();
    var key = GetObjectID('hdnCustomerId').value;
    var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";

    if (type == "QO") {
        clbl_InvoiceNO.SetText('PI/Quotation Date');
    }
    else if (type == "SO") {
        clbl_InvoiceNO.SetText('Sales Order Date');
    }
    else if (type == "SC") {
        clbl_InvoiceNO.SetText('Sales Challan Date');
    }

  

    if (key != null && key != '' && type != "") {
        cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + key + '~' + startDate + '~' + 'DateCheck' + '~' + type);
    }

    var componentType = gridquotationLookup.GetValue();
    if (componentType != null && componentType != '') {
      
        deleteAllRows();
        grid.AddNewRow();
        grid.GetEditor('SrlNo').SetValue('1');
        
    }
}
function CloseGridQuotationLookup() {

    gridquotationLookup.ConfirmCurrentSelection();
    gridquotationLookup.HideDropDown();
    gridquotationLookup.Focus();

    var quotetag_Id = gridquotationLookup.gridView.GetSelectedKeysOnPage();
}
    function CloseGridQuotationLookup() {

        gridquotationLookup.ConfirmCurrentSelection();
        gridquotationLookup.HideDropDown();
        gridquotationLookup.Focus();

        
    }
    function LoadOldSelectedKeyvalue() {
        var x = gridquotationLookup.gridView.GetSelectedKeysOnPage();
        var Ids = "";
        for (var i = 0; i < x.length; i++) {
            Ids = Ids + ',' + x[i];
        }
        document.getElementById('OldSelectedKeyvalue').value = Ids;
    }
function TechnicianButnClick(s, e) {    
    $('#TechModel').modal('show');
}
function CustomerKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
        $('#TechModel').modal('show');
    }
}
function BeginComponentCallback() {

}
function Techniciankeydown(e) {

    var OtherDetails = {}
    OtherDetails.SearchKey = $("#txtTechnicianSearch").val();

    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var HeaderCaption = [];
        HeaderCaption.push("Technician Name");
        if ($("#txtTechnicianSearch").val() != '') {
            callonServer("Services/Master.asmx/GetTechnician", OtherDetails, "TechnicianTable", HeaderCaption, "TechnicianIndex", "SetTechnician");
        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[TechnicianIndex=0]"))
            $("input[TechnicianIndex=0]").focus();
    }

}
function SetTechnician(Id, Name) {
    if (Id) {
        $('#TechModel').modal('hide');
        ctxtTechnician.SetText(Name);
        GetObjectID('hdnTechnicianId').value = Id;
      
    }
}
function QuotationNumberChanged() {
    //if (SimilarProjectStatus != "-1") {
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
                    cComponentDatePanel.PerformCallback('BindComponentDate' + '~' + quote_Id + '~' + type);
                }
                else {
                    ctxt_InvoiceDate.SetText('');
                }
            }
        }
        else { ctxt_InvoiceDate.SetText(''); }

        if (quote_Id != null) {
            cgridproducts.PerformCallback('BindProductsDetails' + '~' + '@');
            cProductsPopup.Show();
        }
   // }
}
function PerformCallToGridBind() {
    var quote_Id = gridquotationLookup.gridView.GetSelectedKeysOnPage();

    grid.PerformCallback('BindGridOnQuotation' + '~' + '@');
    if (quote_Id.length == 0 || quote_Id.length < 0) {
        grid.AddNewRow();
        grid.GetEditor('SrlNo').SetValue('1');
    }  
  
    cProductsPopup.Hide();

    return false;
}

function CmbWarehouseEndCallback(s, e) {
    if (SelectWarehouse != "0") {
        cCmbWarehouse.SetValue(SelectWarehouse);
        SelectWarehouse = "0";
    }
    else {
        cCmbWarehouse.SetEnabled(true);
    }
}

function CmbBatchEndCall(s, e) {
    if (SelectBatch != "0") {
        cCmbBatch.SetValue(SelectBatch);
        SelectBatch = "0";
    }
    else {
        cCmbBatch.SetEnabled(true);
    }
}
function OnListBoxSelectionChanged(listBox, args) {
    if (args.index == 0)
        args.isSelected ? listBox.SelectAll() : listBox.UnselectAll();
    UpdateSelectAllItemState();
    UpdateText();

    //Added Subhabrata
    var selectedItems = checkListBox.GetSelectedItems();
    var val = GetSelectedItemsText(selectedItems);
    var strWarehouse = cCmbWarehouse.GetValue();
    var strBatchID = cCmbBatch.GetValue();
    var ProducttId = $("#hdfProductID").val();

    
}
function UpdateSelectAllItemState() {
    IsAllSelected() ? checkListBox.SelectIndices([0]) : checkListBox.UnselectIndices([0]);
}
function UpdateText() {
    var selectedItems = checkListBox.GetSelectedItems();
    selectedChkValue = GetSelectedItemsText(selectedItems);
    //checkComboBox.SetText(GetSelectedItemsText(selectedItems));
    //checkComboBox.SetText(selectedItems.length + " Items");

    var itemsCount = GetSelectedItemsCount(selectedItems);
    checkComboBox.SetText(itemsCount + " Items");

    var val = GetSelectedItemsText(selectedItems);
    $("#abpl").attr('data-content', val);
}
function GetSelectedItemsText(items) {
    var texts = [];
    for (var i = 0; i < items.length; i++)
        if (items[i].index != 0)
            texts.push(items[i].text);
    return texts.join(textSeparator);
}
function GetSelectedItemsCount(items) {
    var texts = [];
    for (var i = 0; i < items.length; i++)
        if (items[i].index != 0)
            texts.push(items[i].text);
    return texts.length;
}
function listBoxEndCall(s, e) {
    if (SelectSerial != "0") {
        var values = [SelectSerial];
        checkListBox.SelectValues(values);
        UpdateSelectAllItemState();
        UpdateText();
        //checkListBox.SetValue(SelectWarehouse);
        SelectSerial = "0";
        cCmbBatch.SetEnabled(false);
        cCmbWarehouse.SetEnabled(false);
    }
}
function SynchronizeListBoxValues(dropDown, args) {
    checkListBox.UnselectAll();
    // var texts = dropDown.GetText().split(textSeparator);
    var texts = selectedChkValue.split(textSeparator);

    var values = GetValuesByTexts(texts);
    checkListBox.SelectValues(values);
    UpdateSelectAllItemState();
    UpdateText(); // for remove non-existing texts
}
function GetValuesByTexts(texts) {
    var actualValues = [];
    var item;
    for (var i = 0; i < texts.length; i++) {
        item = checkListBox.FindItemByText(texts[i]);
        if (item != null)
            actualValues.push(item.value);
    }
    return actualValues;
}
function txtserialTextChanged() {
    checkListBox.UnselectAll();
    var SerialNo = (ctxtserial.GetValue() != null) ? (ctxtserial.GetValue()) : "0";

    if (SerialNo != "0") {
        ctxtserial.SetValue("");
        var texts = [SerialNo];
        var values = GetValuesByTexts(texts);

        if (values.length > 0) {
            checkListBox.SelectValues(values);
            UpdateSelectAllItemState();
            UpdateText(); // for remove non-existing texts
            SaveWarehouse();
        }
        else {
            jAlert("This Serial Number does not exists.");
        }
    }
}
function SaveWarehouse() {
    var WarehouseID = (cCmbWarehouse.GetValue() != null) ? cCmbWarehouse.GetValue() : "0";
    var WarehouseName = cCmbWarehouse.GetText();
    var BatchID = (cCmbBatch.GetValue() != null) ? cCmbBatch.GetValue() : "0";
    var BatchName = cCmbBatch.GetText();
    var SerialID = "";
    var SerialName = "";
    var Qty = ctxtQuantity.GetValue();

    var items = checkListBox.GetSelectedItems();
    var vals = [];
    var texts = [];

    for (var i = 0; i < items.length; i++) {
        if (items[i].index != 0) {
            if (i == 0) {
                SerialID = items[i].value;
                SerialName = items[i].text;
            }
            else {
                if (SerialID == "" && SerialID == "") {
                    SerialID = items[i].value;
                    SerialName = items[i].text;
                }
                else {
                    SerialID = SerialID + '||@||' + items[i].value;
                    SerialName = SerialName + '||@||' + items[i].text;
                }
            }
            //texts.push(items[i].text);
            //vals.push(items[i].value);
        }
    }

    //WarehouseID, BatchID, SerialID, Qty=0.0
    $("#spnCmbWarehouse").hide();
    $("#spnCmbBatch").hide();
    $("#spncheckComboBox").hide();
    $("#spntxtQuantity").hide();

    var Ptype = document.getElementById('hdfProductType').value;
    if ((Ptype == "W" && WarehouseID == "0") || (Ptype == "WB" && WarehouseID == "0") || (Ptype == "WS" && WarehouseID == "0") || (Ptype == "WBS" && WarehouseID == "0")) {
        $("#spnCmbWarehouse").show();
    }
    else if ((Ptype == "B" && BatchID == "0") || (Ptype == "WB" && BatchID == "0") || (Ptype == "WBS" && BatchID == "0") || (Ptype == "BS" && BatchID == "0")) {
        $("#spnCmbBatch").show();
    }
    else if ((Ptype == "W" && Qty == "0.0") || (Ptype == "B" && Qty == "0.0") || (Ptype == "WB" && Qty == "0.0")) {
        $("#spntxtQuantity").show();
    }
    else if ((Ptype == "S" && SerialID == "") || (Ptype == "WS" && SerialID == "") || (Ptype == "WBS" && SerialID == "") || (Ptype == "BS" && SerialID == "")) {
        $("#spncheckComboBox").show();
    }
    else {
        if (document.getElementById("myCheck").checked == true && SelectedWarehouseID == "0") {
            if (Ptype == "W" || Ptype == "WB" || Ptype == "B") {
                cCmbWarehouse.PerformCallback('BindWarehouse');
                cCmbBatch.PerformCallback('BindBatch~' + "");
                checkListBox.PerformCallback('BindSerial~' + "" + '~' + "");
                ctxtQuantity.SetValue("0");
            }
            else {
                IsPostBack = "N";
                PBWarehouseID = WarehouseID;
                PBBatchID = BatchID;
            }
        }
        else {
            cCmbWarehouse.PerformCallback('BindWarehouse');
            cCmbBatch.PerformCallback('BindBatch~' + "");
            checkListBox.PerformCallback('BindSerial~' + "" + '~' + "");
            ctxtQuantity.SetValue("0");
        }
        UpdateText();
        cGrdWarehouse.PerformCallback('SaveDisplay~' + WarehouseID + '~' + WarehouseName + '~' + BatchID + '~' + BatchName + '~' + SerialID + '~' + SerialName + '~' + Qty + '~' + SelectedWarehouseID);
        SelectedWarehouseID = "0";
    }
}
function OnWarehouseEndCallback(s, e) {
    var Ptype = document.getElementById('hdfProductType').value;

    if (cGrdWarehouse.cpIsSave == "Y") {
        cPopup_Warehouse.Hide();
        Editgrid.batchEditApi.StartEdit(Warehouseindex, 11);
    }
    else if (cGrdWarehouse.cpIsSave == "N") {
        jAlert('Sales Quantity must be equal to Warehouse Quantity.');
    }
    else {
        if (document.getElementById("myCheck").checked == true) {
            if (IsPostBack == "N") {
                checkListBox.PerformCallback('BindSerial~' + PBWarehouseID + '~' + PBBatchID);

                IsPostBack = "";
                PBWarehouseID = "";
                PBBatchID = "";
            }

            if (Ptype == "W" || Ptype == "WB") {
                cCmbWarehouse.Focus();
            }
            else if (Ptype == "B") {
                cCmbBatch.Focus();
            }
            else {
                ctxtserial.Focus();
            }
        }
        else {
            if (Ptype == "W" || Ptype == "WB" || Ptype == "WS" || Ptype == "WBS") {
                cCmbWarehouse.Focus();
            }
            else if (Ptype == "B" || Ptype == "BS") {
                cCmbBatch.Focus();
            }
            else if (Ptype == "S") {
                checkComboBox.Focus();
            }
        }
    }
    //Rev Subhra 15-05-2019
    //grid.batchEditApi.StartEdit(globalRowIndex, 11);
    setTimeout(function () { Editgrid.batchEditApi.StartEdit(EditglobalRowindex, 11); }, 1000);
    //End of Rev Subhra 15-05-2019
}
function CmbWarehouse_ValueChange() {
    var WarehouseID = cCmbWarehouse.GetValue();
    var type = document.getElementById('hdfProductType').value;

    if (type == "WBS" || type == "WB") {
        cCmbBatch.PerformCallback('BindBatch~' + WarehouseID);
    }
    else if (type == "WS") {
        checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + "0");
    }
}
function FinalWarehouse() {
    cGrdWarehouse.PerformCallback('WarehouseFinal');    
    setTimeout(function () { Editgrid.batchEditApi.StartEdit(EditglobalRowindex, 11); }, 600);
}
function closeWarehouse(s, e) {
    e.cancel = false;
    cGrdWarehouse.PerformCallback('WarehouseDelete');
    $('#abpl').popover('hide');//Subhabrata
}
function fn_Edit(keyValue) {
    //cGrdWarehouse.PerformCallback('EditWarehouse~' + keyValue);
    SelectedWarehouseID = keyValue;
    cCallbackPanel.PerformCallback('EditWarehouse~' + keyValue);
}
function fn_Deletecity(keyValue) {
    var WarehouseID = (cCmbWarehouse.GetValue() != null) ? cCmbWarehouse.GetValue() : "0";
    var BatchID = (cCmbBatch.GetValue() != null) ? cCmbBatch.GetValue() : "0";

    cGrdWarehouse.PerformCallback('Delete~' + keyValue);
    checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + BatchID);
}

function ServiceTemplateButnClick(s, e) {
    $('#SerTemModel').modal('show');
}
function SerTemkeydown(e) {

    var OtherDetails = {}
    OtherDetails.SearchKey = $("#txtSerTemSearch").val();
   // OtherDetails.CustomerIds = $("#hdnCustomerId").val();
    if (e.code == "Enter" || e.code == "NumpadEnter") {

        var HeaderCaption = [];
        HeaderCaption.push("Service Description");
        HeaderCaption.push("Service");
        callonServer("Services/Master.asmx/GetServiceTempleate", OtherDetails, "SerTemTable", HeaderCaption, "SerTemIndex", "SetServiceTempleate");
    }
    else if (e.code == "ArrowDown") {
        if ($("input[SerTemIndex=0]"))
            $("input[SerTemIndex=0]").focus();
    }
    else if (e.code == "Escape") {
        ctxtSegment1.Focus();
    }
}

function ServiceTemplateKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter") {
        $('#SerTemModel').modal('show');
        $("#txtSerTemSearch").focus();
    }
}

function SetServiceTempleate(Id, Name, e) {

    var LookUpData = Id;
    var ProductCode = Name;
    if (!ProductCode) {
        LookUpData = null;
    }
    $('#SerTemModel').modal('hide');
    ctxtServiceTemplate.SetText(ProductCode);

    var ComponentID = $("#hdnlblComponentID").val();
    var DocDetailsID = $("#hdnlblDocDetailsID").val();
   
    Editgrid.PerformCallback("BindGridForServiceTemplate" + "~" + Id + "~" + ComponentID + "~" + DocDetailsID);

}

function CallbackPanelEndCall(s, e) {
    if (cCallbackPanel.cpEdit != null) {
        var strWarehouse = cCallbackPanel.cpEdit.split('~')[0];
        var strBatchID = cCallbackPanel.cpEdit.split('~')[1];
        var strSrlID = cCallbackPanel.cpEdit.split('~')[2];
        var strQuantity = cCallbackPanel.cpEdit.split('~')[3];

        SelectWarehouse = strWarehouse;
        SelectBatch = strBatchID;
        SelectSerial = strSrlID;

        cCmbWarehouse.PerformCallback('BindWarehouse');
        cCmbBatch.PerformCallback('BindBatch~' + strWarehouse);
        checkListBox.PerformCallback('EditSerial~' + strWarehouse + '~' + strBatchID + '~' + strSrlID);

        cCmbWarehouse.SetValue(strWarehouse);
        ctxtQuantity.SetValue(strQuantity);
    }
}