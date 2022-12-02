function WH_ValueChange(s, e)
{
    var WarehouseID = ccmbWarehouse.GetValue();
    var BranchId = $('#ddlBranch').val();
    var ProductId = GetObjectID('hdnProductId').value;

    if ((WarehouseID != "" || WarehouseID != null) && BranchId != "" && ProductId != "") {
        $.ajax({
            type: "POST",
            url: "StockAdjustmentAdd.aspx/GetStockInHand",
            data: JSON.stringify({ ProductId: ProductId, WarehouseID: WarehouseID, BranchId: BranchId }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var data = msg.d;
                var strStockID = data.split("~")[0].toString();
                var strStockUOM = data.split("~")[1].toString();
                var Rate = data.split("~")[1].toString();

                //$("#txtStockInHand").prop('disabled', true);
                //$("#txtStockInHand").val(strStockID);
                $("#hdnstockInHand").val(strStockID);
                ctxtStockInHand.SetText(strStockID);
                clblStockHand.SetText(strStockUOM);
                clblEnterAdjustQty.SetText(strStockUOM);
                clblTotalStockInHand.SetText(strStockUOM);
                var avlStk = strStockID + " " + strStockUOM
                document.getElementById("lblAvailableStk").innerHTML = avlStk;
                $("txtReason").val(Rate);

                ctxtEnterAdjustQty.SetText("0.00");
                ctxtTotalStockInHand.SetText("0.00");
            }
        });
    }
}



function CmbScheme_ValueChange(s, e) {
    var numbSchm = s.GetValue();
    var splitData = numbSchm.split('~');
    var startNo = splitData[1];
    if (numbSchm != "") {
        $('#ddlBranch').val(splitData[2]);
       /// document.getElementById('<%= ddlBranch.ClientID %>').disabled = true;
        $("#ddlBranch").prop("disabled", true);
        $("#hdnBranch").val(splitData[2]);
    }
    if (startNo == "1") {
        ctxtVoucherNo.SetText("Auto");
        ctxtVoucherNo.SetEnabled(false);
    } else {
        ctxtVoucherNo.SetText("");
        ctxtVoucherNo.SetEnabled(true);
    }

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

    var OtherDetails = {}
    OtherDetails.BranchID = $('#ddlBranch').val();

    $.ajax({
        type: "POST",
        url: "../Activities/Services/Master.asmx/GetWarehouseByBranch",
        data: JSON.stringify(OtherDetails),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var returnObject = msg.d;

            if (returnObject.ForWareHouse) {
                SetDataSourceOnComboBox(ccmbWarehouse, returnObject.ForWareHouse);
            } 
        
        }
    });

    var WarehouseID = ccmbWarehouse.GetValue();
    var BranchId = $('#ddlBranch').val();
    var ProductId = GetObjectID('hdnProductId').value;

    if((WarehouseID!=""||WarehouseID!=null) && BranchId!="" && ProductId!="")
    {
        $.ajax({
            type: "POST",
            url: "StockAdjustmentAdd.aspx/GetStockInHand",
            data: JSON.stringify({ ProductId: ProductId, WarehouseID: WarehouseID, BranchId: BranchId }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var data = msg.d;
                var strStockID = data.split("~")[0].toString();
                var strStockUOM = data.split("~")[1].toString();
                var Rate = data.split("~")[1].toString();

                //$("#txtStockInHand").prop('disabled', true);
                //$("#txtStockInHand").val(strStockID);
                $("#hdnstockInHand").val(strStockID);
                ctxtStockInHand.SetText(strStockID);
                clblStockHand.SetText(strStockUOM);
                clblEnterAdjustQty.SetText(strStockUOM);
                clblTotalStockInHand.SetText(strStockUOM);
                var avlStk = strStockID + " " + strStockUOM
                document.getElementById("lblAvailableStk").innerHTML = avlStk;
                $("txtReason").val(Rate);

                ctxtEnterAdjustQty.SetText("0.00");
                ctxtTotalStockInHand.SetText("0.00");
            }
        });
    }

}

function SetDataSourceOnComboBox(ControlObject, Source) {
    ControlObject.ClearItems();
    for (var count = 0; count < Source.length; count++) {
        ControlObject.AddItem(Source[count].Name, Source[count].Id);
    }
    ControlObject.SetSelectedIndex(0);
}


function ProductButnClick(s, e) {
   
        $('#txtProdSearch').val('');
        $('#ProductModel').modal('show');
        setTimeout(function () { $("#txtProdSearch").focus(); }, 500);
    
}

function Product_KeyDown(s, e) {
    if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
        $('#ProductModel').modal('show');
    }
}

function prodkeydown(e) {  

    var OtherDetails = {}
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
            callonServer("Services/Master.asmx/GetProductForStockAdjustment", OtherDetails, "ProductTable", HeaderCaption, "ProdIndex", "SetProduct");
        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[ProdIndex=0]"))
            $("input[ProdIndex=0]").focus();
    }
    else if (e.code == "Escape") {       
        grid.batchEditApi.StartEdit(globalRowIndex, 3);
    }
}

function SetProduct(Id, Name) {
   
    if (Id) {
        $('#ProductModel').modal('hide');
        var LookUpData = Id;
        var ProductCode = Name;
        ctxtProdName.SetText(Name);
        GetObjectID('hdnProductId').value = Id;
        ctxtEnterAdjustQty.Focus();
        var WarehouseID = ccmbWarehouse.GetValue();
        var BranchId = $('#ddlBranch').val();
        $.ajax({
            type: "POST",
            url: "StockAdjustmentAdd.aspx/GetStockInHand",
            data: JSON.stringify({ ProductId: Id, WarehouseID: WarehouseID, BranchId: BranchId }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var data = msg.d;
                var strStockID = data.split("~")[0].toString();
                var strStockUOM = data.split("~")[1].toString();
                var Rate = data.split("~")[1].toString();

                //$("#txtStockInHand").prop('disabled', true);
                //$("#txtStockInHand").val(strStockID);
                $("#hdnstockInHand").val(strStockID);
                ctxtStockInHand.SetText(strStockID);
                clblStockHand.SetText(strStockUOM);
                clblEnterAdjustQty.SetText(strStockUOM);
                clblTotalStockInHand.SetText(strStockUOM);
                var avlStk = strStockID + " " + strStockUOM
                document.getElementById("lblAvailableStk").innerHTML = avlStk;
                $("txtReason").val(Rate);

                ctxtEnterAdjustQty.SetText("0.00");
                ctxtTotalStockInHand.SetText("0.00");
            }
        });

        $.ajax({
            type: "POST",
            url: "StockAdjustmentAdd.aspx/GetProductUOM",
            data: JSON.stringify({ ProductId: Id }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var data = msg.d;
               // var strProductUOM = data.split("~")[0].toString();              
               
               
                clblStockHand.SetText(data);
                clblEnterAdjustQty.SetText(data);
                clblTotalStockInHand.SetText(data);

               
            }
        });
    }
    
}
function ValueSelected(e, indexName) {
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var Id = e.target.parentElement.parentElement.cells[0].innerText;
        var name = e.target.parentElement.parentElement.cells[1].children[0].value;
        if (Id) {
            if (indexName == "ProdIndex")
                SetProduct(Id, name);
                
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
            if (indexName == "ProdIndex")
                $('#txtProdSearch').focus();
               
        }
    }

}

function Project_LostFocus() {
    var projID = clookup_Project.GetValue();
    if (projID == null || projID == "") {
        $("#ddlHierarchy").val(0);
    }
}
function Project_gotFocus() {
    if ($("#hdnProjectSelectInEntryModule").val() == "1")
        clookup_Project.gridView.Refresh();
    clookup_Project.ShowDropDown();
}
function ProjectValueChange(s, e) {
    var projID = clookup_Project.GetValue();
    $.ajax({
        type: "POST",
        url: 'StockAdjustmentAdd.aspx/getHierarchyID',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ ProjID: projID }),
        success: function (msg) {
            var data = msg.d;
            $("#ddlHierarchy").val(data);
        }
    });
}