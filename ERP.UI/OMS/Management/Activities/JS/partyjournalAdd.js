//Customer
var globalRowIndex;
var globalRowIndexVendor;


function UnitKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter" || e.htmlEvent.key == "NumpadEnter") {
        s.OnButtonClick(0);
    }
}

function UnitButnClick(s, e) {
    $("#gridContainer").dxDataGrid({
        dataSource: new DevExpress.data.CustomStore({
            load: function () {
                var deferred = $.Deferred();

                var xhr = $.ajax({
                    method: "POST",
                    url: "partyjournalAdd.aspx/GetBranch",
                    //data: JSON.stringify(Filters), 
                    contentType: "application/json",
                    dataType: "JSON",
                    //async: false,  
                    success: function (result) {
                        console.log('before resolve');
                        deferred.resolve(result.d);
                        console.log('after resolve');
                    }
                });
                return deferred.promise();
            }
        }),


        keyExpr: "branch_id",
        selection: {
            mode: "single"
        },
        filterRow: {
            visible: true,
            applyFilter: "auto"
        },
        hoverStateEnabled: true,
        showBorders: true,
        paging: {
            pageSize: 10
        },
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [10, 25, 50, 100]
        },
        columnAutoWidth: true,
        remoteOperations: false,
        searchPanel: {
            visible: true,
            highlightCaseSensitive: true
        },

        allowColumnReordering: true,
        rowAlternationEnabled: true,
        showBorders: true,
        columns: [
            {
                dataField: "branch_description",
                caption: "Branch Name",
                dataType: "string"
            }
        ],
        onSelectionChanged: function (selectedItems) {
            var data = selectedItems.selectedRowsData[0];
            if (data) {
                $("#UnitNumberingnidal").modal('hide')
                grid.batchEditApi.StartEdit(globalRowIndex, 2);
                grid.GetEditor("UnitID").SetText(data.branch_id);
                grid.GetEditor("Unit").SetText(data.branch_description);
                this.clearSelection();
            }
        }
    });

    $("#UnitNumberingnidal").modal('show')
}

function DocumentNumberingKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter" || e.htmlEvent.key == "NumpadEnter") {
        s.OnButtonClick(0);
    }
}

function DocumentNumberingButnClick(s, e) {

    var branch_id = grid.GetEditor("UnitID").GetText();

    var obj = {};
    obj.branch_id = branch_id;

    $("#gridContainerNumbering").dxDataGrid({
        dataSource: new DevExpress.data.CustomStore({
            load: function () {
                var deferred = $.Deferred();

                var xhr = $.ajax({
                    method: "POST",
                    url: "partyjournalAdd.aspx/GetNumbering",
                    data: JSON.stringify(obj),
                    contentType: "application/json",
                    dataType: "JSON",
                    //async: false,  
                    success: function (result) {
                        console.log('before resolve');
                        deferred.resolve(result.d);
                        console.log('after resolve');
                    }
                });
                return deferred.promise();
            }
        }),

        filterRow: {
            visible: true,
            applyFilter: "auto"
        },
        keyExpr: "ID",
        selection: {
            mode: "single"
        },
        hoverStateEnabled: true,
        showBorders: true,
        paging: {
            pageSize: 10
        },
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [10, 25, 50, 100]
        },
        columnAutoWidth: true,
        remoteOperations: false,
        searchPanel: {
            visible: true,
            highlightCaseSensitive: true
        },

        allowColumnReordering: true,
        rowAlternationEnabled: true,
        showBorders: true,
        columns: [
            {
                dataField: "S_NAME",
                caption: "Schema Name",
                dataType: "string"
            },
            {
                dataField: "branch_description",
                caption: "Branch Name",
                dataType: "string"
            }
        ],
        onSelectionChanged: function (selectedItems) {
            var data = selectedItems.selectedRowsData[0];
            if (data) {
                $("#Numberingnidal").modal('hide')
                grid.batchEditApi.StartEdit(globalRowIndex, 3);
                grid.GetEditor("SchemaID").SetText(data.ID);
                grid.GetEditor("DocumentNumbering").SetText(data.S_NAME);
                this.clearSelection();
            }
        }
    });

    $("#Numberingnidal").modal('show')
}

function CustomerKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter" || e.htmlEvent.key == "NumpadEnter") {
        s.OnButtonClick(0);
    }
}

function CustomerButnClick(s, e) {
    $("#gridContainerCustomer").dxDataGrid({
        dataSource: new DevExpress.data.CustomStore({
            load: function () {
                var deferred = $.Deferred();

                var xhr = $.ajax({
                    method: "POST",
                    url: "partyjournalAdd.aspx/GetCustomer",
                    //data: JSON.stringify(Filters), 
                    contentType: "application/json",
                    dataType: "JSON",
                    //async: false,  
                    success: function (result) {
                        console.log('before resolve');
                        deferred.resolve(result.d);
                        console.log('after resolve');
                    }
                });
                return deferred.promise();
            }
        }),
        filterRow: {
            visible: true,
            applyFilter: "auto"
        },

        keyExpr: "cnt_internalid",
        selection: {
            mode: "single"
        },
        hoverStateEnabled: true,
        showBorders: true,
        paging: {
            pageSize: 10
        },
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [10, 25, 50, 100]
        },
        columnAutoWidth: true,
        remoteOperations: false,
        searchPanel: {
            visible: true,
            highlightCaseSensitive: true
        },

        allowColumnReordering: true,
        rowAlternationEnabled: true,
        showBorders: true,
        columns: [
            {
                dataField: "shortname",
                caption: "Short Name",
                dataType: "string"
            },
            {
                dataField: "Name",
                caption: "Name",
                dataType: "string",
                width: 300
            },
            {
                dataField: "Billing",
                caption: "Billing",
                dataType: "string"
            },
            {
                dataField: "Type",
                caption: "Type",
                dataType: "string"
            }
        ],
        onSelectionChanged: function (selectedItems) {
            var data = selectedItems.selectedRowsData[0];
            if (data) {
                $("#Customermodal").modal('hide')
                grid.batchEditApi.StartEdit(globalRowIndex, 4);
                grid.GetEditor("Customer").SetText(data.Name);
                grid.GetEditor("CustomerID").SetText(data.cnt_internalid);
                this.clearSelection();
            }
        }
    });

    $("#Customermodal").modal('show')
}

function ProjectKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter" || e.htmlEvent.key == "NumpadEnter") {
        s.OnButtonClick(0);
    }
}

function ProjectClick(s, e) {
    var customer_id = grid.GetEditor("CustomerID").GetText();
    var branch_id = grid.GetEditor("UnitID").GetText();
    var obj = {};
    obj.customer_id = customer_id;
    obj.branch_id = branch_id;

    $("#gridContainerProject").dxDataGrid({
        dataSource: new DevExpress.data.CustomStore({
            load: function () {
                var deferred = $.Deferred();

                var xhr = $.ajax({
                    method: "POST",
                    url: "partyjournalAdd.aspx/GetProject",
                    data: JSON.stringify(obj),
                    contentType: "application/json",
                    dataType: "JSON",
                    //async: false,  
                    success: function (result) {
                        console.log('before resolve');
                        deferred.resolve(result.d);
                        console.log('after resolve');
                    }
                });
                return deferred.promise();
            }
        }),

        filterRow: {
            visible: true,
            applyFilter: "auto"
        },
        keyExpr: "Proj_Id",
        selection: {
            mode: "single"
        },
        hoverStateEnabled: true,
        showBorders: true,
        paging: {
            pageSize: 10
        },
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [10, 25, 50, 100]
        },
        columnAutoWidth: true,
        remoteOperations: false,
        searchPanel: {
            visible: true,
            highlightCaseSensitive: true
        },

        allowColumnReordering: true,
        rowAlternationEnabled: true,
        showBorders: true,
        columns: [
            {
                dataField: "Proj_Name",
                caption: "Project Name",
                dataType: "string"
            },
            {
                dataField: "Proj_Code",
                caption: "Project Code",
                dataType: "string"
            },
            {
                dataField: "Customer",
                caption: "Customer",
                dataType: "string"
            }
        ],
        onSelectionChanged: function (selectedItems) {
            var data = selectedItems.selectedRowsData[0];
            if (data) {
                $("#Projectmodal").modal('hide')
                grid.batchEditApi.StartEdit(globalRowIndex, 5);
                grid.GetEditor("ProjectId").SetText(data.Proj_Id);
                grid.GetEditor("Project").SetText(data.Proj_Name);
                this.clearSelection();
            }
        }
    });

    $("#Projectmodal").modal('show')
}

function RefDocKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter" || e.htmlEvent.key == "NumpadEnter") {
        s.OnButtonClick(0);
    }
}

function RefDocClick(s, e) {
    var customer_id = grid.GetEditor("CustomerID").GetText();
    var branch_id = grid.GetEditor("UnitID").GetText();
    var project_id = grid.GetEditor("ProjectId").GetText();
    var date = cdtTDate.GetText();

    var obj = {};
    obj.customer_id = customer_id;
    obj.branch_id = branch_id;
    obj.project_id = project_id;
    obj.date = date;


    $("#RefDocProject").dxDataGrid({
        dataSource: new DevExpress.data.CustomStore({
            load: function () {
                var deferred = $.Deferred();

                var xhr = $.ajax({
                    method: "POST",
                    url: "partyjournalAdd.aspx/GetRefDoc",
                    data: JSON.stringify(obj),
                    contentType: "application/json",
                    dataType: "JSON",
                    //async: false,  
                    success: function (result) {
                        console.log('before resolve');
                        deferred.resolve(result.d);
                        console.log('after resolve');
                    }
                });
                return deferred.promise();
            }
        }),
        filterRow: {
            visible: true,
            applyFilter: "auto"
        },

        keyExpr: "Invoice_Id",
        selection: {
            mode: "single"
        },
        hoverStateEnabled: true,
        showBorders: true,
        paging: {
            pageSize: 10
        },
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [10, 25, 50, 100]
        },
        columnAutoWidth: true,
        remoteOperations: false,
        searchPanel: {
            visible: true,
            highlightCaseSensitive: true
        },

        allowColumnReordering: true,
        rowAlternationEnabled: true,
        showBorders: true,
        columns: [
            {
                dataField: "Invoice_Number",
                caption: "Invoice Number",
                dataType: "string"
            },
            {
                dataField: "invoice_date",
                caption: "Invoice Date",
                dataType: "string"
            },
            {
                dataField: "Cust_Name",
                caption: "Customer",
                dataType: "string"
            },
            {
                dataField: "branch_description",
                caption: "Unit",
                dataType: "string"
            },
            {
                dataField: "Proj_Name",
                caption: "Project",
                dataType: "string"
            },
            {
                dataField: "Invoice_TotalAmount",
                caption: "Total Amount",
                dataType: "string"
            },
            {
                dataField: "UnPaidAmount",
                caption: "Bal. Amount",
                dataType: "string"
            }
        ],
        onSelectionChanged: function (selectedItems) {
            var data = selectedItems.selectedRowsData[0];
            if (data) {
                $("#RefDocmodal").modal('hide')
                grid.batchEditApi.StartEdit(globalRowIndex, 6);
                grid.GetEditor("RefDoc").SetText(data.Invoice_Number);
                grid.GetEditor("Amount").SetText(data.Invoice_TotalAmount);
                grid.GetEditor("BalAmount").SetText(data.UnPaidAmount);
                grid.GetEditor("AdjAmount").SetText(0);
                grid.GetEditor("DocId").SetText(data.Invoice_Id);

                this.clearSelection();
            }

        }
    });

    $("#RefDocmodal").modal('show')
}


function OnEndCallback(s, e) {

    if (s.cpSuccess == "1") {
        vendorgrid.UpdateEdit();
        s.cpSuccess = null;
        s.cpMsg = null;
    }
    else {
        jAlert(s.cpMsg, 'Alert');
        s.cpMsg = null;
        s.cpSuccess = null;

    }
}
function OnCustomButtonClick(s, e) {
    if (e.buttonID == 'CustomDelete') {
        if (grid.GetVisibleRowsOnPage() > 1) {
            grid.batchEditApi.StartEdit(e.visibleIndex);
            //PopOnPicked(grid.GetEditor("TypeId").GetText() + grid.GetEditor("IsOpening").GetText() + grid.GetEditor("DocId").GetText());
            grid.DeleteRow(e.visibleIndex);
            var IndexNo = globalRowIndex;
            SuffuleSerialNumber();
            // EnableDisableGST();
            //ShowRunningTotal();
            setTimeout(function () {
                grid.batchEditApi.StartEdit(globalRowIndex, 2);
            }, 200);

        }
    }
    if (e.buttonID == 'AddNew') {
        grid.batchEditApi.StartEdit(e.visibleIndex);

        //if (grid.GetEditor("TypeId").GetText() == "") return;
        //if (grid.GetEditor("Receipt").GetText() == "0.00" || grid.GetEditor("Receipt").GetText() == "") return;

        //if (grid.GetEditor("TypeId").GetText().toUpperCase() != "ADVANCE" && grid.GetEditor("TypeId").GetText().toUpperCase() != "ONACCOUNT") {
        //    if (grid.GetEditor("DocId").GetText() == "") return;
        //}

        AddNewRow();
    }
}

function GetVisibleIndex(s, e) {
    globalRowIndex = e.visibleIndex;
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

function AdjLostfocus(s, e) {
    var amount = grid.GetEditor("BalAmount").GetText();
    var adj_amount = grid.GetEditor("AdjAmount").GetText();
    var doc_no = grid.GetEditor("RefDoc").GetText();


    if (parseFloat(adj_amount) > parseFloat(amount) && doc_no != "") {
        jAlert('Adjustment amount can not be greater than balance amount', 'Alert', function () {
            grid.GetEditor("AdjAmount").SetText(amount);
            grid.batchEditApi.StartEdit(globalRowIndex, 9);
        })
    }
    else
    {
        SetTotalTaxableAmount(globalRowIndex);
    }
}

function SetTotalTaxableAmount(RowIndex) {
    var count = grid.GetVisibleRowsOnPage();
    var totalAmount = 0;
    for (var i = 0; i < count + 10; i++) {
        if (grid.GetRow(i)) {
            if (grid.GetRow(i).style.display != "none") {
                grid.batchEditApi.StartEdit(i, 2);
                totalAmount = totalAmount + DecimalRoundoff(grid.GetEditor("AdjAmount").GetValue(), 2);
            }
        }
    }

    for (i = -1; i > -count - 10; i--) {
        if (grid.GetRow(i)) {
            if (grid.GetRow(i).style.display != "none") {
                grid.batchEditApi.StartEdit(i, 2);
                totalAmount = totalAmount + DecimalRoundoff(grid.GetEditor("AdjAmount").GetValue(), 2);

            }
        }
    }
    grid.batchEditApi.EndEdit()
    grid.batchEditApi.StartEdit(RowIndex, 10);
    $("#totCr").text(totalAmount);
    var Running = DecimalRoundoff(parseFloat($("#totCr").text()) - parseFloat($("#totDr").text()), 2);
    $("#RunningTot").text(Running);


}

//End Customer


//Vendor
function VendorUnitKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter" || e.htmlEvent.key == "NumpadEnter") {
        s.OnButtonClick(0);
    }
}

function VendorUnitButnClick(s, e) {
    $("#gridContainerVendor").dxDataGrid({
        dataSource: new DevExpress.data.CustomStore({
            load: function () {
                var deferred = $.Deferred();

                var xhr = $.ajax({
                    method: "POST",
                    url: "partyjournalAdd.aspx/GetBranch",
                    //data: JSON.stringify(Filters), 
                    contentType: "application/json",
                    dataType: "JSON",
                    //async: false,  
                    success: function (result) {
                        console.log('before resolve');
                        deferred.resolve(result.d);
                        console.log('after resolve');
                    }
                });
                return deferred.promise();
            }
        }),


        keyExpr: "branch_id",
        selection: {
            mode: "single"
        },
        filterRow: {
            visible: true,
            applyFilter: "auto"
        },
        hoverStateEnabled: true,
        showBorders: true,
        paging: {
            pageSize: 10
        },
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [10, 25, 50, 100]
        },
        columnAutoWidth: true,
        remoteOperations: false,
        searchPanel: {
            visible: true,
            highlightCaseSensitive: true
        },

        allowColumnReordering: true,
        rowAlternationEnabled: true,
        showBorders: true,
        columns: [
            {
                dataField: "branch_description",
                caption: "Branch Name",
                dataType: "string"
            }
        ],
        onSelectionChanged: function (selectedItems) {
            var data = selectedItems.selectedRowsData[0];
            if (data) {
                $("#UnitNumberingnidalVendor").modal('hide')
                vendorgrid.batchEditApi.StartEdit(globalRowIndexVendor, 2);
                vendorgrid.GetEditor("UnitID").SetText(data.branch_id);
                vendorgrid.GetEditor("Unit").SetText(data.branch_description);
                this.clearSelection();
            }
        }
    });

    $("#UnitNumberingnidalVendor").modal('show')
}

function VendorDocumentNumberingKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter" || e.htmlEvent.key == "NumpadEnter") {
        s.OnButtonClick(0);
    }
}


function VendorDocumentNumberingButnClick(s, e) {
    var branch_id = vendorgrid.GetEditor("UnitID").GetText();

    var obj = {};
    obj.branch_id = branch_id;

    $("#gridContainerNumberingVendor").dxDataGrid({
        dataSource: new DevExpress.data.CustomStore({
            load: function () {
                var deferred = $.Deferred();

                var xhr = $.ajax({
                    method: "POST",
                    url: "partyjournalAdd.aspx/GetNumbering",
                    data: JSON.stringify(obj),
                    contentType: "application/json",
                    dataType: "JSON",
                    //async: false,  
                    success: function (result) {
                        console.log('before resolve');
                        deferred.resolve(result.d);
                        console.log('after resolve');
                    }
                });
                return deferred.promise();
            }
        }),

        filterRow: {
            visible: true,
            applyFilter: "auto"
        },
        keyExpr: "ID",
        selection: {
            mode: "single"
        },
        hoverStateEnabled: true,
        showBorders: true,
        paging: {
            pageSize: 10
        },
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [10, 25, 50, 100]
        },
        columnAutoWidth: true,
        remoteOperations: false,
        searchPanel: {
            visible: true,
            highlightCaseSensitive: true
        },

        allowColumnReordering: true,
        rowAlternationEnabled: true,
        showBorders: true,
        columns: [
            {
                dataField: "S_NAME",
                caption: "Schema Name",
                dataType: "string"
            },
            {
                dataField: "branch_description",
                caption: "Branch Name",
                dataType: "string"
            }
        ],
        onSelectionChanged: function (selectedItems) {
            var data = selectedItems.selectedRowsData[0];
            if (data) {
                $("#NumberingnidalVendor").modal('hide')
                vendorgrid.batchEditApi.StartEdit(globalRowIndexVendor, 3);
                vendorgrid.GetEditor("SchemaID").SetText(data.ID);
                vendorgrid.GetEditor("DocumentNumbering").SetText(data.S_NAME);
                this.clearSelection();
            }
        }
    });

    $("#NumberingnidalVendor").modal('show')
}

function VendorKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter" || e.htmlEvent.key == "NumpadEnter") {
        s.OnButtonClick(0);
    }
}

function VendorButnClick(s, e) {
    $("#gridContainerCustomerVendor").dxDataGrid({
        dataSource: new DevExpress.data.CustomStore({
            load: function () {
                var deferred = $.Deferred();

                var xhr = $.ajax({
                    method: "POST",
                    url: "partyjournalAdd.aspx/GetVendor",
                    //data: JSON.stringify(Filters), 
                    contentType: "application/json",
                    dataType: "JSON",
                    //async: false,  
                    success: function (result) {
                        console.log('before resolve');
                        deferred.resolve(result.d);
                        console.log('after resolve');
                    }
                });
                return deferred.promise();
            }
        }),
        filterRow: {
            visible: true,
            applyFilter: "auto"
        },

        keyExpr: "cnt_internalid",
        selection: {
            mode: "single"
        },
        hoverStateEnabled: true,
        showBorders: true,
        paging: {
            pageSize: 10
        },
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [10, 25, 50, 100]
        },
        columnAutoWidth: true,
        remoteOperations: false,
        searchPanel: {
            visible: true,
            highlightCaseSensitive: true
        },

        allowColumnReordering: true,
        rowAlternationEnabled: true,
        showBorders: true,
        columns: [
            {
                dataField: "shortname",
                caption: "Short Name",
                dataType: "string"
            },
            {
                dataField: "Name",
                caption: "Name",
                dataType: "string",
                width: 300
            },
            {
                dataField: "Type",
                caption: "Type",
                dataType: "string"
            }
        ],
        onSelectionChanged: function (selectedItems) {
            var data = selectedItems.selectedRowsData[0];
            if (data) {
                $("#CustomermodalVendor").modal('hide')
                vendorgrid.batchEditApi.StartEdit(globalRowIndexVendor, 4);
                vendorgrid.GetEditor("Vendor").SetText(data.Name);
                vendorgrid.GetEditor("VendorID").SetText(data.cnt_internalid);
                this.clearSelection();
            }
        }
    });

    $("#CustomermodalVendor").modal('show')
}

function VendorProjectKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter" || e.htmlEvent.key == "NumpadEnter") {
        s.OnButtonClick(0);
    }
}

function VendorProjectClick(s, e) {
    var branch_id = vendorgrid.GetEditor("UnitID").GetText();
    var customer_id = vendorgrid.GetEditor("VendorID").GetText();

    var obj = {};
    obj.branch_id = branch_id;
    obj.customer_id = customer_id;

    $("#gridContainerProjectVendor").dxDataGrid({
        dataSource: new DevExpress.data.CustomStore({
            load: function () {
                var deferred = $.Deferred();

                var xhr = $.ajax({
                    method: "POST",
                    url: "partyjournalAdd.aspx/GetProjectVendor",
                    data: JSON.stringify(obj),
                    contentType: "application/json",
                    dataType: "JSON",
                    //async: false,  
                    success: function (result) {
                        console.log('before resolve');
                        deferred.resolve(result.d);
                        console.log('after resolve');
                    }
                });
                return deferred.promise();
            }
        }),

        filterRow: {
            visible: true,
            applyFilter: "auto"
        },
        keyExpr: "Proj_Id",
        selection: {
            mode: "single"
        },
        hoverStateEnabled: true,
        showBorders: true,
        paging: {
            pageSize: 10
        },
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [10, 25, 50, 100]
        },
        columnAutoWidth: true,
        remoteOperations: false,
        searchPanel: {
            visible: true,
            highlightCaseSensitive: true
        },

        allowColumnReordering: true,
        rowAlternationEnabled: true,
        showBorders: true,
        columns: [
            {
                dataField: "Proj_Name",
                caption: "Project Name",
                dataType: "string"
            },
            {
                dataField: "Proj_Code",
                caption: "Project Code",
                dataType: "string"
            },
            {
                dataField: "Customer",
                caption: "Customer",
                dataType: "string"
            }
        ],
        onSelectionChanged: function (selectedItems) {
            var data = selectedItems.selectedRowsData[0];
            if (data) {
                $("#ProjectmodalVendor").modal('hide')
                vendorgrid.batchEditApi.StartEdit(globalRowIndexVendor, 5);
                vendorgrid.GetEditor("ProjectId").SetText(data.Proj_Id);
                vendorgrid.GetEditor("Project").SetText(data.Proj_Name);
                this.clearSelection();
            }
        }
    });

    $("#ProjectmodalVendor").modal('show')
}

function VendorRefDocKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter" || e.htmlEvent.key == "NumpadEnter") {
        s.OnButtonClick(0);
    }
}

function VendorRefDocClick(s, e) {
    var vendor_id = vendorgrid.GetEditor("VendorID").GetText();
    var branch_id = vendorgrid.GetEditor("UnitID").GetText();
    var project_id = vendorgrid.GetEditor("ProjectId").GetText();
    var date = cdtTDate.GetText();

    var obj = {};
    obj.vendor_id = vendor_id;
    obj.branch_id = branch_id;
    obj.project_id = project_id;
    obj.date = date;


    $("#RefDocProjectVendor").dxDataGrid({
        dataSource: new DevExpress.data.CustomStore({
            load: function () {
                var deferred = $.Deferred();

                var xhr = $.ajax({
                    method: "POST",
                    url: "partyjournalAdd.aspx/GetRefDocVendor",
                    data: JSON.stringify(obj),
                    contentType: "application/json",
                    dataType: "JSON",
                    //async: false,  
                    success: function (result) {
                        console.log('before resolve');
                        deferred.resolve(result.d);
                        console.log('after resolve');
                    }
                });
                return deferred.promise();
            }
        }),
        filterRow: {
            visible: true,
            applyFilter: "auto"
        },

        keyExpr: "Invoice_Id",
        selection: {
            mode: "single"
        },
        hoverStateEnabled: true,
        showBorders: true,
        paging: {
            pageSize: 10
        },
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [10, 25, 50, 100]
        },
        columnAutoWidth: true,
        remoteOperations: false,
        searchPanel: {
            visible: true,
            highlightCaseSensitive: true
        },

        allowColumnReordering: true,
        rowAlternationEnabled: true,
        showBorders: true,
        columns: [
            {
                dataField: "Invoice_Number",
                caption: "Invoice Number",
                dataType: "string"
            },
            {
                dataField: "invoice_date",
                caption: "Invoice Date",
                dataType: "string"
            },
            {
                dataField: "Cust_Name",
                caption: "Customer",
                dataType: "string"
            },
            {
                dataField: "branch_description",
                caption: "Unit",
                dataType: "string"
            },
            {
                dataField: "Proj_Name",
                caption: "Project",
                dataType: "string"
            },
            {
                dataField: "Invoice_TotalAmount",
                caption: "Total Amount",
                dataType: "string"
            },
            {
                dataField: "UnPaidAmount",
                caption: "Bal. Amount",
                dataType: "string"
            }
        ],
        onSelectionChanged: function (selectedItems) {
            var data = selectedItems.selectedRowsData[0];
            if (data) {
                $("#RefDocmodalVendor").modal('hide')
                vendorgrid.batchEditApi.StartEdit(globalRowIndexVendor, 6);
                vendorgrid.GetEditor("RefDoc").SetText(data.Invoice_Number);
                vendorgrid.GetEditor("Amount").SetText(data.Invoice_TotalAmount);
                vendorgrid.GetEditor("BalAmount").SetText(data.UnPaidAmount);
                vendorgrid.GetEditor("AdjAmount").SetText(0);
                grid.GetEditor("DocId").SetText(data.Invoice_Id);
                this.clearSelection();
                //grid.GetEditor("Project").SetText(data.S_NAME);
            }
        }
    });

    $("#RefDocmodalVendor").modal('show')
}

function OnEndCallbackVendor(s, e) {
    if (s.cpSuccess == "1") {
        jAlert(s.cpMsg, 'Alert', function () {
            window.location.href = '/OMS/management/Activities/partyjournallist.aspx';
        });
        s.cpSuccess = null;
        s.cpMsg = null;
    }
    else {
        jAlert(s.cpMsg, 'Alert');
        s.cpMsg = null;
        s.cpSuccess = null;

    }
}

function OnCustomButtonClickVendor(s, e) {
    if (e.buttonID == 'VendorCustomDelete') {
        if (vendorgrid.GetVisibleRowsOnPage() > 1) {
            vendorgrid.batchEditApi.StartEdit(e.visibleIndex);
            //PopOnPicked(vendorgrid.GetEditor("TypeId").GetText() + vendorgrid.GetEditor("IsOpening").GetText() + vendorgrid.GetEditor("DocId").GetText());
            vendorgrid.DeleteRow(e.visibleIndex);
            var IndexNo = globalRowIndexVendor;
            SuffuleSerialNumbervendor();
            //EnableDisableGST();
            //ShowRunningTotal();
            setTimeout(function () {
                vendorgrid.batchEditApi.StartEdit(globalRowIndexVendor, 2);
            }, 200);

        }
    }
    if (e.buttonID == 'VendorAddNew') {
        vendorgrid.batchEditApi.StartEdit(e.visibleIndex);

        //if (vendorgrid.GetEditor("TypeId").GetText() == "") return;
        //if (vendorgrid.GetEditor("Receipt").GetText() == "0.00" || vendorgrid.GetEditor("Receipt").GetText() == "") return;

        //if (vendorgrid.GetEditor("TypeId").GetText().toUpperCase() != "ADVANCE" && vendorgrid.GetEditor("TypeId").GetText().toUpperCase() != "ONACCOUNT") {
        //    if (vendorgrid.GetEditor("DocId").GetText() == "") return;
        //}

        AddNewRowVendor();
    }
}

function GetVisibleIndexVendor(s, e) {
    globalRowIndexVendor = e.visibleIndex;
}

function SuffuleSerialNumbervendor() {
    var TotRowNumber = vendorgrid.GetVisibleItemsOnPage();
    var SlnoCount = 1;
    for (var i = 0; i < 1000; i++) {
        if (vendorgrid.GetRow(i)) {
            if (vendorgrid.GetRow(i).style.display != "none") {
                vendorgrid.batchEditApi.StartEdit(i, 2);
                vendorgrid.GetEditor("SrlNo").SetText(SlnoCount);
                SlnoCount++;
            }
        }
    }

    for (i = -1; i > -1000; i--) {
        if (vendorgrid.GetRow(i)) {
            if (vendorgrid.GetRow(i).style.display != "none") {
                vendorgrid.batchEditApi.StartEdit(i, 2);
                vendorgrid.GetEditor("SrlNo").SetText(SlnoCount);
                SlnoCount++;
            }
        }
    }
}

function AdjLostfocusVendor(s, e) {
    var amount = vendorgrid.GetEditor("BalAmount").GetText();
    var adj_amount = vendorgrid.GetEditor("AdjAmount").GetText();
    var doc_no = vendorgrid.GetEditor("RefDoc").GetText();


    if (parseFloat(adj_amount) > parseFloat(amount) && doc_no != "") {
        jAlert('Adjustment amount can not be greater than balance amount', 'Alert', function () {
            vendorgrid.GetEditor("AdjAmount").SetText(amount);
            vendorgrid.batchEditApi.StartEdit(globalRowIndexVendor, 9);
        })
    }
    else
    {
        SetTotalTaxableAmountVendor(globalRowIndexVendor);
    }
}



function SetTotalTaxableAmountVendor(RowIndexVendor) {
    var count = grid.GetVisibleRowsOnPage();
    var totalAmount = 0;
    for (var i = 0; i < count + 10; i++) {
        if (vendorgrid.GetRow(i)) {
            if (vendorgrid.GetRow(i).style.display != "none") {
                vendorgrid.batchEditApi.StartEdit(i, 2);
                totalAmount = totalAmount + DecimalRoundoff(vendorgrid.GetEditor("AdjAmount").GetValue(), 2);
            }
        }
    }

    for (i = -1; i > -count - 10; i--) {
        if (vendorgrid.GetRow(i)) {
            if (vendorgrid.GetRow(i).style.display != "none") {
                vendorgrid.batchEditApi.StartEdit(i, 2);
                totalAmount = totalAmount + DecimalRoundoff(vendorgrid.GetEditor("AdjAmount").GetValue(), 2);
               
            }
        }
    }
    vendorgrid.batchEditApi.EndEdit()
    vendorgrid.batchEditApi.StartEdit(RowIndexVendor, 10);
    $("#totDr").text(totalAmount);    
    var Running = DecimalRoundoff( parseFloat($("#totCr").text()) - parseFloat($("#totDr").text()) ,2);
    $("#RunningTot").text(Running);


}

//end vendor


// General

function AllControlInitilize(s, e) {

    AddNewRow();
    AddNewRowVendor();

    SetTotalTaxableAmountVendor(-1);
    SetTotalTaxableAmount(-1);

}

function AddNewRow() {
    grid.AddNewRow();
    grid.GetEditor("SrlNo").SetText(grid.GetVisibleItemsOnPage());
}

function AddNewRowVendor() {
    vendorgrid.AddNewRow()
    vendorgrid.GetEditor("SrlNo").SetText(vendorgrid.GetVisibleItemsOnPage());
}

function SaveExit()
{
    if (ccmbBranchfilter.GetValue() == "0")
    {
        jAlert('Select numbering Scheme.')
        return;
    }
    if (parseFloat($("#RunningTot").text()) != 0)
    {
        jAlert('Total debit and total credit does not match. Can not proceed.')
        return;
    }



    grid.UpdateEdit();
}

function CmbScheme_ValueChange(s,e) {

    var schemetypeValue = s.GetValue();
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
        
    }

}

// End General