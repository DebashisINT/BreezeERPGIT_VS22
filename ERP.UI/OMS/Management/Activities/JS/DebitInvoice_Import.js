
        $(document).ready(function () {
            $('#ddl_numberingScheme').change(function () {
                clookup_MainAccount.gridView.SetFocusedRowIndex(-1);
                clookup_SubAccount.gridView.SetFocusedRowIndex(-1);

                var NoSchemeTypedtl = $(this).val();
                var NoSchemeType = NoSchemeTypedtl.toString().split('~')[1];
                var quotelength = NoSchemeTypedtl.toString().split('~')[2];

                var branchID = (NoSchemeTypedtl.toString().split('~')[3] != null) ? NoSchemeTypedtl.toString().split('~')[3] : "";

                var fromdate = NoSchemeTypedtl.toString().split('~')[4];
                var todate = NoSchemeTypedtl.toString().split('~')[5];

                var dt = new Date();

                cdtPostingDate.SetDate(dt);

                if (dt < new Date(fromdate)) {
                    cdtPostingDate.SetDate(new Date(fromdate));
                }

                if (dt > new Date(todate)) {
                    cdtPostingDate.SetDate(new Date(todate));
                }

                cdtPostingDate.SetMinDate(new Date(fromdate));
                cdtPostingDate.SetMaxDate(new Date(todate));

                if (branchID != "") document.getElementById('ddl_Branch').value = branchID;
                GetObjectID('hdnBranchID').value = branchID;

                cMainAccountPanel.PerformCallback();
            });
        });

function btnImportClick() {
    cgridInvoice.PerformCallback("Save");
}

function CloseGridLookup() {
    clookup_MainAccount.ConfirmCurrentSelection();
    clookup_MainAccount.HideDropDown();
    clookup_MainAccount.Focus();
}

function GetMainAccount(e) {
    clookup_SubAccount.gridView.SetFocusedRowIndex(-1);
    var MainAccount = clookup_MainAccount.GetGridView().GetRowKey(clookup_MainAccount.GetGridView().GetFocusedRowIndex());
    cSubAccountPanel.PerformCallback(MainAccount);
}

function checkValidate() {
    if ($('#ddl_numberingScheme').val() == "") {
        $("#div_numberingScheme").show();
        return false;
    }
    else {
        $("#div_numberingScheme").hide();
    }

    if (cdtPostingDate.GetDate() == null) {
        $("#div_Date").show();
        return false;
    }
    else {
        $("#div_Date").hide();
    }

    var NoSchemeTypedtl = $("#ddl_numberingScheme").val();
    var branchID = (NoSchemeTypedtl.toString().split('~')[3] != null) ? NoSchemeTypedtl.toString().split('~')[3] : "";
    if (branchID != "") document.getElementById('ddl_Branch').value = branchID;

    var key = clookup_MainAccount.GetGridView().GetRowKey(clookup_MainAccount.GetGridView().GetFocusedRowIndex());
    if (key == null || key == '') {
        $("#div_mainAccount").show();
        return false;
    }
    else {
        $("#div_mainAccount").hide();
    }
}
