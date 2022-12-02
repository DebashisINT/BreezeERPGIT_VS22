<%@ Page Title="Vendor Debit/Credit Note" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" 
    CodeBehind="VendorDebitCreditNote.aspx.cs" Inherits="OpeningEntry.ERP.VendorDebitCreditNote" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function onPrintJv(id) {
            window.location.href = "../../reports/XtraReports/Viewer/VendorCRDRNoteReportViewer.aspx?id=" + id;
        }

        function OnAddButtonClick() {
            document.getElementById('divAddNew').style.display = 'block';
            TblSearch.style.display = "none";
            btncross.style.display = "block";
            $('#<%=hdnMode.ClientID %>').val('0'); //Entry
            $('#<%= lblHeading.ClientID %>').text("Add Vendor Credit/Debit Note");

            var CmbScheme = document.getElementById("<%=CmbScheme.ClientID%>");
            CmbScheme.options[0].selected = true;

            CountryID.PerformCallback(document.getElementById('ddlBranch').value);

            grid.AddNewRow();
            grid.batchEditApi.EndEdit();
            document.getElementById("<%=ddlNoteType.ClientID%>").focus();

            document.getElementById('<%= txtBillNo.ClientID %>').disabled = true;
            document.getElementById('Keyval_internalId').value = "Add";

            //cbtnSaveRecords.SetVisible(false);
            //cbtn_SaveRecords.SetVisible(false);
            //loadCurrencyMassage.style.display = "block";
        }

        ////This Method is Used For Checking Lock Date and Financial Year and Alert User For That if Date OutSide
        function DateChange() {

            var Ctype = $('#<%=hdnMode.ClientID %>').val();
            if (Ctype != 1) {
                var SelectedDate = new Date(tDate.GetDate());
                var monthnumber = SelectedDate.getMonth();
                var monthday = SelectedDate.getDate();
                var year = SelectedDate.getYear();

                var SelectedDateValue = new Date(year, monthnumber, monthday);
                ///Checking of Transaction Date For MaxLockDate
                var MaxLockDate = new Date('<%=Session["LCKJV"]%>');
                monthnumber = MaxLockDate.getMonth();
                monthday = MaxLockDate.getDate();
                year = MaxLockDate.getYear();
                var MaxLockDateNumeric = new Date(year, monthnumber, monthday).getTime();

                if (SelectedDateValue <= MaxLockDateNumeric) {
                    jAlert('This Entry Date has been Locked.');
                    MaxLockDate.setDate(MaxLockDate.getDate() + 1);
                    tDate.SetDate(MaxLockDate);
                    return;
                }
            }

            var FYS = "<%=Session["FinYearStart"]%>";
            var FYE = "<%=Session["FinYearEnd"]%>";
            var LFY = "<%=Session["LastFinYear"]%>";
            var SelectedDate = new Date(tDate.GetDate());
            var FinYearStartDate = new Date(FYS);
            var FinYearEndDate = new Date(FYE);
            var LastFinYearDate = new Date(LFY);

            var monthnumber = SelectedDate.getMonth();
            var monthday = SelectedDate.getDate();
            var year = SelectedDate.getYear();

            var SelectedDateValue = new Date(year, monthnumber, monthday);

            monthnumber = FinYearStartDate.getMonth();
            monthday = FinYearStartDate.getDate();
            year = FinYearStartDate.getYear();
            var FinYearStartDateValue = new Date(year, monthnumber, monthday);


            monthnumber = FinYearEndDate.getMonth();
            monthday = FinYearEndDate.getDate();
            year = FinYearEndDate.getYear();
            var FinYearEndDateValue = new Date(year, monthnumber, monthday);

            var SelectedDateNumericValue = SelectedDateValue.getTime();
            var FinYearStartDateNumericValue = FinYearStartDateValue.getTime();
            var FinYearEndDatNumbericValue = FinYearEndDateValue.getTime();
            if (SelectedDateNumericValue >= FinYearStartDateNumericValue && SelectedDateNumericValue <= FinYearEndDatNumbericValue) {
                if (grid.GetVisibleRowsOnPage() == 1) {
                    //grid.batchEditApi.StartEdit(-1, 1);
                }
            }
            else {
                jAlert('Enter Date Is Outside Of Financial Year !!');
                if (SelectedDateNumericValue < FinYearStartDateNumericValue) {
                    tDate.SetDate(new Date(FinYearStartDate));
                }
                if (SelectedDateNumericValue > FinYearEndDatNumbericValue) {
                    tDate.SetDate(new Date(FinYearEndDate));
                }
            }
        }

        function CustomButtonClick(s, e) {
            var TransactionDate = new Date(tDate.GetDate());
            monthnumber = TransactionDate.getMonth();
            monthday = TransactionDate.getDate();
            year = TransactionDate.getYear();
            var TransactionDateNumeric = new Date(year, monthnumber, monthday).getTime();

            var MaxLockDate = new Date('<%=Session["LCKJV"]%>');
            monthnumber = MaxLockDate.getMonth();
            monthday = MaxLockDate.getDate();
            year = MaxLockDate.getYear();
            var MaxLockDateNumeric = new Date(year, monthnumber, monthday).getTime();

            if (TransactionDateNumeric <= MaxLockDateNumeric) {
                jAlert('This Entry has been Locked.You Can Only View The Detail');
                return;
                //VisibleIndexE = e.visibleIndex;
                //cGvJvSearch.PerformCallback('PCB_BtnOkE~' + e.visibleIndex);
                //return;
            }

            if (e.buttonID == 'CustomBtnEdit') {
                VisibleIndexE = e.visibleIndex;
                $('#<%= lblHeading.ClientID %>').text("Modify Vendor Credit/Debit Note");
                $('#<%=hdnMode.ClientID %>').val('1');
                document.getElementById('div_Edit').style.display = 'none';
                document.getElementById('<%= txtBillNo.ClientID %>').disabled = true;

                document.getElementById('divAddNew').style.display = 'block';
                btncross.style.display = "block";
                TblSearch.style.display = "none";
                //cGvJvSearch.GetRowValues(VisibleIndexE, "BranchID", MainAccountCallBack);
                grid.PerformCallback('Edit~' + VisibleIndexE);
                LoadingPanel.Show();

                cbtnSaveRecords.SetVisible(false)
            }
            else if (e.buttonID == 'CustomBtnView') {
                VisibleIndexE = e.visibleIndex;
                $('#<%= lblHeading.ClientID %>').text("View Vendor Credit/Debit Note");
                $('#<%=hdnMode.ClientID %>').val('1');
                document.getElementById('div_Edit').style.display = 'none';
                document.getElementById('<%= txtBillNo.ClientID %>').disabled = true;

                document.getElementById('divAddNew').style.display = 'block';
                btncross.style.display = "block";
                TblSearch.style.display = "none";
                //cGvJvSearch.GetRowValues(VisibleIndexE, "BranchID", MainAccountCallBack);
                grid.PerformCallback('View~' + VisibleIndexE);
                LoadingPanel.Show();

                cbtnSaveRecords.SetVisible(false)
            }
            else if (e.buttonID == 'CustomBtnDelete') {
                VisibleIndexE = e.visibleIndex;

                jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                    if (r == true) {
                        cGvJvSearch.PerformCallback('PCB_DeleteBtnOkE~' + VisibleIndexE);
                    }
                });
            }
            else if (e.buttonID == 'CustomBtnPrint') {
                //var keyValueindex = e.visibleIndex;
               // jConfirm('Do you want to Print?', 'Confirmation Dialog', function (r) {
                    //if (r == true) {
                       // cGvJvSearch.GetRowValues(keyValueindex, "JvID", onPrintJv)
                   // }
                // });
                var keyValueindex = s.GetRowKey(e.visibleIndex);
                onPrintJv(keyValueindex);
            }
    }

  
    function MainAccountCallBack(branch) {
        CountryID.PerformCallback(branch);
    }

    function GvJvSearch_EndCallBack() {
        if (cGvJvSearch.cpJVDelete != undefined) {
            jAlert(cGvJvSearch.cpJVDelete);
            cGvJvSearch.cpJVDelete = null;
            cGvJvSearch.PerformCallback('PCB_BindAfterDelete');
        }
    }
    </script>
    <script type="text/javascript">
        var currentEditableVisibleIndex;
        var preventEndEditOnLostFocus = false;
        var lastCountryID;
        var setValueFlag;
        var debitOldValue;
        var debitNewValue;
        var CreditOldValue;
        var CreditNewValue;

        function CountriesCombo_SelectedIndexChanged(s, e) {
            var currentValue = grid.GetEditor('CountryID').GetValue();
            //var currentValue = s.GetValue();
            if (lastCountryID == currentValue) {
                if (CityID.GetSelectedIndex() < 0)
                    CityID.SetSelectedIndex(0);
                return;
            }
            lastCountryID = currentValue;
            CityID.PerformCallback(currentValue);
        }
        function IntializeGlobalVariables(grid) {
            lastCountryID = grid.cplastCountryID;
            currentEditableVisibleIndex = -1;
            setValueFlag = -1;
        }
        function OnInit(s, e) {
            IntializeGlobalVariables(s);
        }

        function OnEndCallback(s, e) {
            debugger;
            IntializeGlobalVariables(s);
            LoadingPanel.Hide();

            if (grid.cpEdit != null) {
                //grid.ShowLoadingPanel();
                //LoadingPanel.Show();
                var VoucherNo = grid.cpEdit.split('~')[0];
                var Narration = grid.cpEdit.split('~')[1];
                var BranchID = grid.cpEdit.split('~')[2];
                var Credit = grid.cpEdit.split('~')[3];
                var Debit = grid.cpEdit.split('~')[4];
                var trDate = grid.cpEdit.split('~')[5];
                var NoteType = grid.cpEdit.split('~')[6];
                var CustomerID = grid.cpEdit.split('~')[7];
                var CurrencyId = grid.cpEdit.split('~')[8];
                var Rate = grid.cpEdit.split('~')[9];
                var NoteID = grid.cpEdit.split('~')[10];

                document.getElementById('txtBillNo').value = VoucherNo;
                document.getElementById('txtNarration').value = Narration;

                ctxt_Rate.SetValue(Rate);
                document.getElementById('ddl_Currency').value = CurrencyId;
                document.getElementById('ddlNoteType').value = NoteType;
                document.getElementById('hdnNotelNo').value = NoteID;
                gridLookup.SetValue(CustomerID);
                document.getElementById('Keyval_internalId').value = "VendorNote" + NoteID;

                //gridLookup.disabled = false;
                document.getElementById('ddlNoteType').disabled = true;

                <%--var ddlBranch = document.getElementById("<%=ddlBranch.ClientID%>");
                ddlBranch.options[BranchID].selected = true;--%>
                <%--var ddlBranch = document.getElementById("<%=ddlBranch.ClientID%>");
                ddlBranch.Items.FindByValue(BranchID).Selected = true;--%>

                //var dropdownlistbox = document.getElementById("ddlBranch")

                //for (var x = 0; x < dropdownlistbox.length - 1 ; x++) {
                //    if (BranchID == dropdownlistbox.options[x].value) {
                //        dropdownlistbox.selectedIndex = x;
                //        break;
                //    }
                //}

                document.getElementById('ddlBranch').value = BranchID;

                var Transdt = new Date(trDate);
                tDate.SetDate(Transdt);

                //Bind again the main account with respect to branch
                CountryID.PerformCallback(BranchID);
                var strSchemaType = document.getElementById('hdnSchemaType').value;
                var RefreshType = document.getElementById('hdnRefreshType').value;

                c_txt_Credit.SetValue(Credit);
                c_txt_Debit.SetValue(Debit);

                //if (Debit == Credit) {
                //    cbtnSaveRecords.SetVisible(true);
                //    cbtn_SaveRecords.SetVisible(true);
                //    loadCurrencyMassage.style.display = "none";
                //}
                //else {
                //    cbtnSaveRecords.SetVisible(false);
                //    cbtn_SaveRecords.SetVisible(false);
                //    loadCurrencyMassage.style.display = "block";
                //}
            }

            var value = document.getElementById('hdnRefreshType').value;

            if (grid.cpSaveSuccessOrFail == "outrange") {
                grid.cpSaveSuccessOrFail = null;
                jAlert('Can Not Add More Vendor Debit/Credit Note as Scheme Exausted.<br />Update The Scheme and Try Again');
            }
            else if (grid.cpSaveSuccessOrFail == "duplicate") {
                grid.cpSaveSuccessOrFail = null;
                jAlert('Can Not Save as Duplicate Vendor Debit/Credit Note No. Found');
            }
            else if (grid.cpSaveSuccessOrFail == "errorInsert") {
                grid.cpSaveSuccessOrFail = null;
                jAlert('Try again later.');
            }
            else if (grid.cpSaveSuccessOrFail == "successInsert") {
                grid.cpSaveSuccessOrFail = null;
                var JV_Number = grid.cpVouvherNo;
                var JV_Msg = "Vendor Debit/Credit Note No. " + JV_Number + " generated.";
                var strSchemaType = document.getElementById('hdnSchemaType').value;

                if (value == "E") {
                    var IsComplete = "0";

                    if (JV_Number != "") {
                        var strconfirm = confirm(JV_Msg);
                        if (strconfirm == true) {
                            window.location.reload();
                        }
                        else {
                            window.location.reload();
                        }
                    } else {
                        window.location.reload();
                    }
                }
                else if (value == "S") {
                    var IsComp = "0";

                    if (JV_Number != "") {
                        var strconfirm = confirm(JV_Msg);
                        if (strconfirm == true) {
                            IsComp = "1";
                        } else {
                            IsComp = "1";
                        }
                    }
                    else {
                        IsComp = "1";
                    }

                    if (IsComp == "1") {
                <%--$('#<%=hdnMode.ClientID %>').val('0');
                    document.getElementById('div_Edit').style.display = 'block';
                    cbtnSaveRecords.SetVisible(false);
                    cbtn_SaveRecords.SetVisible(false);
                    grid.PerformCallback('BlanckEdit');--%>

                        grid.AddNewRow();
                        $('#<%=hdnMode.ClientID %>').val('0');

                        $('#<%= lblHeading.ClientID %>').text("Add Vendor Debit/Credit Note");

                        //cbtnSaveRecords.SetVisible(false);
                        //cbtn_SaveRecords.SetVisible(false);
                        //loadCurrencyMassage.style.display = "block";

                        document.getElementById('div_Edit').style.display = 'block';
                        document.getElementById('<%= txtBillNo.ClientID %>').disabled = true;
                        //cCmbScheme.SetValue("0");
                        document.getElementById('<%= txtBillNo.ClientID %>').value = "";
                        document.getElementById('<%= txtBillNo.ClientID %>').disabled = true;
                        c_txt_Debit.SetValue("0");
                        c_txt_Credit.SetValue("0");
                        document.getElementById('<%= txtNarration.ClientID %>').value = "";
                        //cCmbScheme.Focus();

                        var ActiveCurrency = '<%=Session["LocalCurrency"]%>'
                        var Currency = ActiveCurrency.toString().split('~')[0];

                        //document.getElementById('ddl_Currency').value = Currency;
                        //document.getElementById('ddlNoteType').value = "Dr";
                        document.getElementById('hdnNotelNo').value = "";
                        document.getElementById('Keyval_internalId').value = "Add";
                        //gridLookup.SetValue("0");
                        //ctxt_Rate.SetValue("0");

                        if (strSchemaType == "0") {
                            document.getElementById('<%= txtBillNo.ClientID %>').disabled = false;
                            document.getElementById('<%= txtBillNo.ClientID %>').value = "";
                            //document.getElementById("txtBillNo").focus();
                            //cCmbScheme.Focus();

                            document.getElementById("CmbScheme").focus();
                        }
                        else if (strSchemaType == "1") {
                            document.getElementById('<%= txtBillNo.ClientID %>').disabled = true;
                            document.getElementById('<%= txtBillNo.ClientID %>').value = "Auto";
                            grid.batchEditApi.StartEdit(-1, 1);
                        }
                        else if (strSchemaType == "2") {
                            document.getElementById('<%= txtBillNo.ClientID %>').disabled = true;
                            document.getElementById('<%= txtBillNo.ClientID %>').value = "Datewise";
                            grid.batchEditApi.StartEdit(-1, 1);
                        }
                        else {
                            //cCmbScheme.SetValue("0");
                            //cCmbScheme.Focus();

                            document.getElementById('<%= txtBillNo.ClientID %>').disabled = true;
                            document.getElementById('<%= txtBillNo.ClientID %>').value = "";

                            var CmbScheme = document.getElementById("<%=CmbScheme.ClientID%>");
                            CmbScheme.options[0].selected = true;
                            document.getElementById("CmbScheme").focus();
                        }
            }
        }

}

else {
    grid.AddNewRow();
}
            ////##### coded by Samrat Roy - 14/04/2017 - ref IssueLog(Voucher - 110) 
            ////This method is called when request is for View Only .
    if (grid.cpView == "1") {
        viewOnly();
    }
}

////##### coded by Samrat Roy - 17/04/2017 - ref IssueLog(Ven Db Nt - 7) 
////This method is for disable all the attributes.
function viewOnly() {
    debugger;
    $('#divAddNew').find('input, textarea, button, select').attr('disabled', 'disabled');

    grid.SetEnabled(false);

    tDate.SetEnabled(false);
    gridLookup.SetEnabled(false);

    cbtnSaveRecords.SetVisible(false);
    cbtn_SaveRecords.SetVisible(false);
    cbtnUDF.SetVisible(false);
    LoadingPanel.Hide();
}

function CitiesCombo_EndCallback(s, e) {
    //if (setValueFlag == -1)
    //    s.SetSelectedIndex(0);
    //else if (setValueFlag > -1) {
    //    CityID.SetSelectedItem(CityID.FindItemByValue(setValueFlag));
    //    setValueFlag = -1;
    //}

    if (setValueFlag == null || setValueFlag == "0" || setValueFlag == "") {
        s.SetSelectedIndex(-1);
    }
    else {
        if (CityID.FindItemByValue(setValueFlag) != null) {
            CityID.SetValue(setValueFlag);
            setValueFlag = null;
        }
    }

    LoadingPanel.Hide();
    //LoadingPanel.Hide();
    //grid.HideLoadingPanel();
}
function OnBatchEditStartEditing(s, e) {
    currentEditableVisibleIndex = e.visibleIndex;
    var currentCountryID = grid.batchEditApi.GetCellValue(currentEditableVisibleIndex, "CountryID");
    var cityIDColumn = s.GetColumnByField("CityID");
    if (!e.rowValues.hasOwnProperty(cityIDColumn.index))
        return;
    var cellInfo = e.rowValues[cityIDColumn.index];

    if (lastCountryID == currentCountryID) {
        if (CityID.FindItemByValue(cellInfo.value) != null) {
            CityID.SetValue(cellInfo.value);
        }
        else {
            RefreshData(cellInfo, lastCountryID);
            LoadingPanel.Show();
        }
    }
    else {
        if (currentCountryID == null) {
            CityID.SetSelectedIndex(-1);
            return;
        }
        lastCountryID = currentCountryID;
        RefreshData(cellInfo, lastCountryID);
        LoadingPanel.Show();
    }

    //setValueFlag = cellInfo.value;
    //CityID.PerformCallback(currentCountryID);
    //LoadingPanel.Show();
}
function RefreshData(cellInfo, countryID) {
    setValueFlag = cellInfo.value;
    CityID.PerformCallback(countryID);

    //setTimeout(function () {
    //    CityID.PerformCallback(countryID);
    //}, 0);
}
//Debjyoti 
function OnCustomButtonClick(s, e) {
    if (e.buttonID == 'CustomDelete') {
        grid.batchEditApi.EndEdit();
        var noofvisiblerows = grid.GetVisibleRowsOnPage();

        if (noofvisiblerows != "1") {
            var debit = grid.batchEditApi.GetCellValue(e.visibleIndex, "WithDrawl");
            var credit = grid.batchEditApi.GetCellValue(e.visibleIndex, "Receipt");
            if (debit != 0)
                c_txt_Debit.SetValue(c_txt_Debit.GetValue() - debit);
            if (credit != 0)
                c_txt_Credit.SetValue(c_txt_Credit.GetValue() - credit);

            var Debit = parseFloat(c_txt_Debit.GetValue());
            var Credit = parseFloat(c_txt_Credit.GetValue());

            //if (Debit == 0 && Credit == 0) {
            //    cbtnSaveRecords.SetVisible(false);
            //    cbtn_SaveRecords.SetVisible(false);
            //    loadCurrencyMassage.style.display = "block";
            //}
            //else if (Debit == Credit) {
            //    cbtnSaveRecords.SetVisible(true);
            //    cbtn_SaveRecords.SetVisible(true);
            //    loadCurrencyMassage.style.display = "none";
            //}
            //else {
            //    cbtnSaveRecords.SetVisible(false);
            //    cbtn_SaveRecords.SetVisible(false);
            //    loadCurrencyMassage.style.display = "block";
            //}

            grid.DeleteRow(e.visibleIndex);

            var type = $('#<%=hdnMode.ClientID %>').val();
            if (type == '1') {
                var IsJournal = "";
                for (var i = 0; i < grid.GetVisibleRowsOnPage() ; i++) {
                    var frontProduct = (grid.batchEditApi.GetCellValue(i, 'CountryID') != null) ? (grid.batchEditApi.GetCellValue(i, 'CountryID')) : "";

                    if (frontProduct == "") {
                        IsJournal = "N";
                        break;
                    }
                }

                if (IsJournal == "") {
                    grid.StartEditRow(0);
                }
            }
        }
    }
}


function CreditGotFocus(s, e) {
    CreditOldValue = s.GetText();
    var indx = CreditOldValue.indexOf(',');
    if (indx != -1) {
        CreditOldValue = CreditOldValue.replace(/,/g, '');
    }
}

function CreditLostFocus(s, e) {
    CreditNewValue = s.GetText();
    var indx = CreditNewValue.indexOf(',');
    if (indx != -1) {
        CreditNewValue = CreditNewValue.replace(/,/g, '');
    }

    if (CreditOldValue != CreditNewValue) {
        changeCreditTotalSummary();
    }
}
function changeCreditTotalSummary() {
    var newDif = CreditOldValue - CreditNewValue;
    var CurrentSum = c_txt_Credit.GetText();
    var indx = CurrentSum.indexOf(',');
    if (indx != -1) {
        CurrentSum = CurrentSum.replace(/,/g, '');
    }

    c_txt_Credit.SetValue(parseFloat(CurrentSum - newDif));
}
function recalculateCredit(oldVal) {
    if (oldVal != 0) {
        CreditNewValue = 0;
        CreditOldValue = oldVal;
        changeCreditTotalSummary();
    }
}

function DebitGotFocus(s, e) {
    debitOldValue = s.GetText();
    var indx = debitOldValue.indexOf(',');
    if (indx != -1) {
        debitOldValue = debitOldValue.replace(/,/g, '');
    }
}

function DebitLostFocus(s, e) {
    debitNewValue = s.GetText();
    var indx = debitNewValue.indexOf(',');

    if (indx != -1) {
        debitNewValue = debitNewValue.replace(/,/g, '');
    }
    if (debitOldValue != debitNewValue) {
        changeDebitTotalSummary();
    }
}
function recalculateDebit(oldVal) {
    if (oldVal != 0) {
        debitNewValue = 0;
        debitOldValue = oldVal;
        changeDebitTotalSummary();
    }
}

function changeDebitTotalSummary() {
    var newDif = debitOldValue - debitNewValue;
    var CurrentSum = c_txt_Debit.GetText();
    var indx = CurrentSum.indexOf(',');
    if (indx != -1) {
        CurrentSum = CurrentSum.replace(/,/g, '');
    }

    c_txt_Debit.SetValue(parseFloat(CurrentSum - newDif));
}
function CalculateSummary(grid, rowValues, visibleIndex, isDeleting) {
    //ctxtTDebit
    var originalValue = grid.batchEditApi.GetCellValue(visibleIndex, "WithDrawl");
    var newValue = rowValues[(grid.GetColumnByField("WithDrawl").index)].value;
    var dif = isDeleting ? -newValue : newValue - originalValue;
    c_txt_Debit.SetValue((parseFloat(c_txt_Debit.GetValue()) + dif).toFixed(1));
    //ctxtTCredit
    var CoriginalValue = grid.batchEditApi.GetCellValue(visibleIndex, "Receipt");
    var CnewValue = rowValues[(grid.GetColumnByField("Receipt").index)].value;
    var Cdif = isDeleting ? -CnewValue : CnewValue - CoriginalValue;
    c_txt_Credit.SetValue((parseFloat(c_txt_Credit.GetValue()) + Cdif).toFixed(1));

    var Debit = parseFloat(c_txt_Debit.GetValue());
    var Credit = parseFloat(c_txt_Credit.GetValue());

    //if (Debit == 0 && Credit == 0) {
    //    cbtnSaveRecords.SetVisible(false);
    //    cbtn_SaveRecords.SetVisible(false);
    //    loadCurrencyMassage.style.display = "block";
    //}
    //else if (Debit == Credit) {
    //    cbtnSaveRecords.SetVisible(true);
    //    cbtn_SaveRecords.SetVisible(true);
    //    loadCurrencyMassage.style.display = "none";
    //}
    //else {
    //    cbtnSaveRecords.SetVisible(false);
    //    cbtn_SaveRecords.SetVisible(false);
    //    loadCurrencyMassage.style.display = "block";
    //}
}
//End here

function OnBatchEditEndEditing(s, e) {
    //Debjyoti
    //  CalculateSummary(s, e.rowValues, e.visibleIndex, false);
    //End here
    currentEditableVisibleIndex = -1;
    var cityIDColumn = s.GetColumnByField("CityID");
    if (!e.rowValues.hasOwnProperty(cityIDColumn.index))
        return;
    var cellInfo = e.rowValues[cityIDColumn.index];
    if (CityID.GetSelectedIndex() > -1 || cellInfo.text != CityID.GetText()) {
        cellInfo.value = CityID.GetValue();
        cellInfo.text = CityID.GetText();
        CityID.SetValue(null);
    }
}
function OnBatchEditRowValidating(s, e) {
    var cityIDColumn = s.GetColumnByField("CityID");
    var cellValidationInfo = e.validationInfo[cityIDColumn.index];
    if (!cellValidationInfo) return;
    var value = cellValidationInfo.value;
    if (!ASPxClientUtils.IsExists(value) || ASPxClientUtils.Trim(value) === "") {
        cellValidationInfo.isValid = false;
        cellValidationInfo.errorText = "City is required";
    }
}
function CitiesCombo_KeyDown(s, e) {
    var keyCode = ASPxClientUtils.GetKeyCode(e.htmlEvent);
    if (keyCode !== ASPxKey.Tab && keyCode !== ASPxKey.Enter) return;
    var moveActionName = e.htmlEvent.shiftKey ? "MoveFocusBackward" : "MoveFocusForward";
    if (grid.batchEditApi[moveActionName]()) {
        ASPxClientUtils.PreventEventAndBubble(e.htmlEvent);
        preventEndEditOnLostFocus = true;
    }
}
function CitiesCombo_LostFocus(s, e) {
    if (!preventEndEditOnLostFocus)
        grid.batchEditApi.EndEdit();
    preventEndEditOnLostFocus = false;
}
function AddBatchNew(s, e) {
    var keyCode = ASPxClientUtils.GetKeyCode(e.htmlEvent);
    if (keyCode === 13) {
        var mainAccountValue = (grid.GetEditor('CountryID').GetValue() != null) ? grid.GetEditor('CountryID').GetValue() : "";
        if (mainAccountValue != "") {
            grid.AddNewRow();
            grid.SetFocusedRowIndex();
        }
    }
    else if (keyCode === 9) {
        document.getElementById("txtNarration").focus();
    }
}
function OnAddNewClick() {
    //  $('#ddlBranch').attr('Disabled', false);
    var gridcount = grid.GetVisibleRowsOnPage();
    var mainAccountValue = grid.batchEditApi.GetCellValue(0, "CountryID");
    if (gridcount == 0) {
        grid.AddNewRow();
    }
    else if (gridcount > 0 && mainAccountValue != "") {
        grid.AddNewRow();
    }
}
function WithDrawlTextChange(s, e) {
    var mainAccountValue = (grid.GetEditor('CountryID').GetValue() != null) ? grid.GetEditor('CountryID').GetValue() : "";

    if (mainAccountValue != "") {
        DebitLostFocus(s, e);
        var withDrawlValue = (grid.GetEditor('WithDrawl').GetValue() != null) ? parseFloat(grid.GetEditor('WithDrawl').GetValue()) : "0";
        var receiptValue = "0";//(grid.GetEditor('Receipt').GetValue() != null) ? grid.GetEditor('Receipt').GetValue() : "0";

        if (withDrawlValue > 0) {
            recalculateCredit("0");////recalculateCredit(grid.GetEditor('Receipt').GetValue());
            //grid.GetEditor('Receipt').SetValue("0");
        }

        var Debit = parseFloat(c_txt_Debit.GetValue());
        var Credit = parseFloat(c_txt_Credit.GetValue());

        //if (Debit == 0 && Credit == 0) {
        //    cbtnSaveRecords.SetVisible(false);
        //    cbtn_SaveRecords.SetVisible(false);
        //    loadCurrencyMassage.style.display = "block";
        //}
        //else if (Debit == Credit) {
        //    cbtnSaveRecords.SetVisible(true);
        //    cbtn_SaveRecords.SetVisible(true);
        //    loadCurrencyMassage.style.display = "none";
        //}
        //else {
        //    cbtnSaveRecords.SetVisible(false);
        //    cbtn_SaveRecords.SetVisible(false);
        //    loadCurrencyMassage.style.display = "block";
        //}
    }
    else {
        grid.GetEditor('WithDrawl').SetValue("0");
    }
}
function ReceiptTextChange(s, e) {
    var mainAccountValue = (grid.GetEditor('CountryID').GetValue() != null) ? grid.GetEditor('CountryID').GetValue() : "";

    if (mainAccountValue != "") {
        CreditLostFocus(s, e);
        var receiptValue = (grid.GetEditor('Receipt').GetValue() != null) ? grid.GetEditor('Receipt').GetValue() : "0";
        var withDrawlValue = (grid.GetEditor('WithDrawl').GetValue() != null) ? parseFloat(grid.GetEditor('WithDrawl').GetValue()) : "0";

        if (receiptValue > 0) {
            recalculateDebit(grid.GetEditor('WithDrawl').GetValue());
            grid.GetEditor('WithDrawl').SetValue("0");

            //grid.GetEditor('WithDrawl').SetEnabled(false);
        }

        var Debit = parseFloat(c_txt_Debit.GetValue());
        var Credit = parseFloat(c_txt_Credit.GetValue());

        //if (Debit == 0 && Credit == 0) {
        //    cbtnSaveRecords.SetVisible(false);
        //    cbtn_SaveRecords.SetVisible(false);
        //    loadCurrencyMassage.style.display = "block";
        //}
        //else if (Debit == Credit) {
        //    cbtnSaveRecords.SetVisible(true);
        //    cbtn_SaveRecords.SetVisible(true);
        //    loadCurrencyMassage.style.display = "none";
        //}
        //else {
        //    cbtnSaveRecords.SetVisible(false);
        //    cbtn_SaveRecords.SetVisible(false);
        //    loadCurrencyMassage.style.display = "block";
        //}
    }
    else {
        grid.GetEditor('Receipt').SetValue("0");
    }
}

////Sudip Pal
$(function () {

    ddlNoteType_ValueChange();

});

////Sudip Pal
function ddlNoteType_ValueChange() {
    var val = document.getElementById("ddlNoteType").value;
    document.getElementById('<%= txtBillNo.ClientID %>').disabled = true;
    document.getElementById('<%= txtBillNo.ClientID %>').value = "";

    $.ajax({
        type: "POST",
        url: "VendorDebitCreditNote.aspx/GetScheme",
        data: "{sel_type_id:\"" + val + "\"}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (r) {
            var CmbScheme = $("[id*=CmbScheme]");
            CmbScheme.empty().append('<option selected="selected" value="0">Select</option>');
            $.each(r.d, function () {
                CmbScheme.append($("<option></option>").val(this['Value']).html(this['Text']));
            });
        }
    });
}

function CmbScheme_ValueChange() {
    //var val = cCmbScheme.GetValue();

    var val = document.getElementById("CmbScheme").value;
    $('#<%=hdnSchemaID.ClientID %>').val(val);
    $("#MandatoryBillNo").hide();

    if (val != "0") {
        $.ajax({
            type: "POST",
            url: 'VendorDebitCreditNote.aspx/getSchemeType',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{sel_scheme_id:\"" + val + "\"}",
            success: function (type) {
                console.log(type);

                var schemetypeValue = type.d;
                var schemetype = schemetypeValue.toString().split('~')[0];
                var schemelength = schemetypeValue.toString().split('~')[1];
                $('#txtBillNo').attr('maxLength', schemelength);

                if (schemetype == '0') {
                    $('#<%=hdnSchemaType.ClientID %>').val('0');
                    document.getElementById('<%= txtBillNo.ClientID %>').disabled = false;
                    document.getElementById('<%= txtBillNo.ClientID %>').value = "";
                    document.getElementById("txtBillNo").focus();
                }
                else if (schemetype == '1') {
                    $('#<%=hdnSchemaType.ClientID %>').val('1');
                    document.getElementById('<%= txtBillNo.ClientID %>').disabled = true;
                    document.getElementById('<%= txtBillNo.ClientID %>').value = "Auto";
                    tDate.Focus();
                }
                else if (schemetype == '2') {
                    $('#<%=hdnSchemaType.ClientID %>').val('2');
                    document.getElementById('<%= txtBillNo.ClientID %>').disabled = true;
                    document.getElementById('<%= txtBillNo.ClientID %>').value = "Datewise";
                }
                else {
                    document.getElementById('<%= txtBillNo.ClientID %>').disabled = true;
                    document.getElementById('<%= txtBillNo.ClientID %>').value = "";
                }
            }
        });
}
else {
    document.getElementById('<%= txtBillNo.ClientID %>').disabled = true;
        document.getElementById('<%= txtBillNo.ClientID %>').value = "";
    }
}

function GoToNextRow() {
    var gridcount = grid.GetVisibleRowsOnPage();
    grid.batchEditApi.StartEdit(gridcount - 2, 2);
    grid.GetEditor('CountryID').Focus();
}

function deleteAllRows() {
    var frontRow = 0;
    var backRow = -1;
    for (var i = 0; i <= grid.GetVisibleRowsOnPage() + 100 ; i++) {
        grid.DeleteRow(frontRow);
        grid.DeleteRow(backRow);
        backRow--;
        frontRow++;
    }
    grid.AddNewRow();

    c_txt_Credit.SetValue(0);
    c_txt_Debit.SetValue(0);
    //cbtnSaveRecords.SetVisible(false);
    //cbtn_SaveRecords.SetVisible(false);
    //loadCurrencyMassage.style.display = "block";
}

var oldBranchdata;
function BranchGotFocus() {
    oldBranchdata = document.getElementById('ddlBranch').value;
}

function ddlBranch_ChangeIndex() {
    if (oldBranchdata != document.getElementById('ddlBranch').value) {

        //get the first row accounting value debjyoti 
        grid.batchEditApi.StartEdit(-1, 1);
        var accountingDataMin = grid.GetEditor('CountryID').GetValue();
        grid.batchEditApi.EndEdit();

        grid.batchEditApi.StartEdit(0, 1);
        var accountingDataplus = grid.GetEditor('CountryID').GetValue();
        grid.batchEditApi.EndEdit();



        if (accountingDataMin != null || accountingDataplus != null) {
            jConfirm('You have changed Branch. All the entries of ledger in this voucher to be reset to blank. \n You have to select and re-enter. Continue?', 'Confirmation Dialog', function (r) {

                if (r == true) {
                    deleteAllRows();
                    CountryID.PerformCallback(document.getElementById('ddlBranch').value);
                    if (grid.GetVisibleRowsOnPage() == 1) {
                        grid.batchEditApi.StartEdit(-1, 1);
                    }
                } else {
                    document.getElementById('ddlBranch').value = oldBranchdata;
                }
            });
        }
        else {
            CountryID.PerformCallback(document.getElementById('ddlBranch').value);
        }
    }
}

function SaveButtonClick() {
    var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";

    if (cbtnSaveRecords.IsVisible() == true) {
        var val = document.getElementById("CmbScheme").value;
        var Branchval = $("#ddlBranch").val();
        $("#MandatoryBillNo").hide();
        $("#MandatorysCustomer").hide();

        if (document.getElementById('<%= txtBillNo.ClientID %>').value == "") {
            //jAlert('Enter Journal No');
            $("#MandatoryBillNo").show();
            document.getElementById('<%= txtBillNo.ClientID %>').focus();
        }
        else if (Branchval == "0") {
            document.getElementById('<%= ddlBranch.ClientID %>').focus();
            jAlert('Enter Branch');
        }
        else if (customerval == "") {
            $("#MandatorysCustomer").show();
        }
        else {
            grid.batchEditApi.EndEdit();
            var Debit = parseFloat(c_txt_Debit.GetValue());

            var frontRow = 0;
            var backRow = -1;
            var IsJournal = "";
            for (var i = 0; i <= grid.GetVisibleRowsOnPage() ; i++) {
                var frontProduct = (grid.batchEditApi.GetCellValue(backRow, 'CountryID') != null) ? (grid.batchEditApi.GetCellValue(backRow, 'CountryID')) : "";
                var backProduct = (grid.batchEditApi.GetCellValue(frontRow, 'CountryID') != null) ? (grid.batchEditApi.GetCellValue(frontRow, 'CountryID')) : "";

                if (frontProduct != "" || backProduct != "") {
                    IsJournal = "Y";
                    break;
                }

                backRow--;
                frontRow++;
            }

            if (Debit == 0) {
                jAlert('Total Amount amount must be greater than zero(0)');
            }
            else if (IsJournal == "Y") {
                $('#<%=hdfLookupCustomer.ClientID %>').val(customerval);
                $('#<%=hdnRefreshType.ClientID %>').val('S');

                grid.UpdateEdit();
            }
            else {
                jAlert('Please add atleast single record first');
            }
    }
}
}
function SaveExitButtonClick() {
    var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";

    if (cbtn_SaveRecords.IsVisible() == true) {
        var val = document.getElementById("CmbScheme").value;
        var Branchval = $("#ddlBranch").val();
        $("#MandatoryBillNo").hide();
        $("#MandatorysCustomer").hide();

        if (document.getElementById('<%= txtBillNo.ClientID %>').value == "") {
            //jAlert('Enter Journal No');
            $("#MandatoryBillNo").show();
            document.getElementById('<%= txtBillNo.ClientID %>').focus();
        }
        else if (Branchval == "0") {
            document.getElementById('<%= ddlBranch.ClientID %>').focus();
            jAlert('Enter Branch');
        }
        else if (customerval == "") {
            $("#MandatorysCustomer").show();
        }
        else {
            grid.batchEditApi.EndEdit();
            var Debit = parseFloat(c_txt_Debit.GetValue());

            var frontRow = 0;
            var backRow = -1;
            var IsJournal = "";
            for (var i = 0; i <= grid.GetVisibleRowsOnPage() ; i++) {
                var frontProduct = (grid.batchEditApi.GetCellValue(backRow, 'CountryID') != null) ? (grid.batchEditApi.GetCellValue(backRow, 'CountryID')) : "";
                var backProduct = (grid.batchEditApi.GetCellValue(frontRow, 'CountryID') != null) ? (grid.batchEditApi.GetCellValue(frontRow, 'CountryID')) : "";

                if (frontProduct != "" || backProduct != "") {
                    IsJournal = "Y";
                    break;
                }

                backRow--;
                frontRow++;
            }

            if (Debit == 0) {
                jAlert('Total Amount amount must be greater than zero(0)');
            }
            else if (IsJournal == "Y") {
                $('#<%=hdfLookupCustomer.ClientID %>').val(customerval);
                $('#<%=hdnRefreshType.ClientID %>').val('E');

                grid.UpdateEdit();
            }
            else {
                jAlert('Please add atleast single record first');
            }
    }
}
}
    </script>
    <script type="text/javascript">
        function OnKeyDown(s, e) {
            if (e.htmlEvent.keyCode == 40 || e.htmlEvent.keyCode == 38)
                return ASPxClientUtils.PreventEvent(e.htmlEvent);
        }
        function txtBillNo_TextChanged() {
            var VoucherNo = document.getElementById("txtBillNo").value;
            var type = $('#<%=hdnMode.ClientID %>').val();

            if (VoucherNo != "") {
                $("#MandatoryBillNo").hide();
            }

            $.ajax({
                type: "POST",
                url: "VendorDebitCreditNote.aspx/CheckUniqueName",
                data: JSON.stringify({ VoucherNo: VoucherNo, Type: type }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var data = msg.d;

                    if (data == true) {
                        $("#duplicateMandatoryBillNo").show();
                        document.getElementById("txtBillNo").value = '';
                        document.getElementById("<%=txtBillNo.ClientID%>").focus();
                    }
                    else {
                        $("#duplicateMandatoryBillNo").hide();
                    }
                }
            });
        }

        $(document).ready(function () {

            $('#txt_Rate_I').blur(function () {
                if (grid.GetVisibleRowsOnPage() == 1) {
                    grid.batchEditApi.StartEdit(-1, 1);
                }
            })

            $('#ddl_Currency').change(function () {
                var CurrencyId = $(this).val();
                var ActiveCurrency = '<%=Session["LocalCurrency"]%>'
                var Currency = ActiveCurrency.toString().split('~')[0];
                if (Currency != CurrencyId) {
                    if (ActiveCurrency != null) {
                        if (CurrencyId != '0') {
                            $.ajax({
                                type: "POST",
                                url: "SalesInvoice.aspx/GetCurrentConvertedRate",
                                data: "{'CurrencyId':'" + CurrencyId + "'}",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function (msg) {
                                    var currentRate = msg.d;
                                    if (currentRate != null) {
                                        //$('#txt_Rate').text(currentRate);
                                        ctxt_Rate.SetValue(currentRate);
                                    }
                                    else {
                                        ctxt_Rate.SetValue('1');
                                    }
                                }
                            });
                        }
                        else {
                            ctxt_Rate.SetValue("1");
                        }
                    }
                }
                else {
                    ctxt_Rate.SetValue("1");
                }
            });

        });
    </script>
    <script>
        function SignOff() {
            window.parent.SignOff();
        }

        var isCtrl = false;
        document.onkeydown = function (e) {
            if (event.keyCode == 83 && event.altKey == true) { //run code for Ctrl+S -- ie, Save & New  
                StopDefaultAction(e);
                //SaveButtonClick();
                document.getElementById('btnSaveRecords').click();
                return false;
            }
            else if (event.keyCode == 88 && event.altKey == true) { //run code for Ctrl+X -- ie, Save & Exit!   
                StopDefaultAction(e);
                //SaveExitButtonClick();
                document.getElementById('btn_SaveRecords').click();
                return false;
            }
            else if (event.keyCode == 65 && event.altKey == true) {
                StopDefaultAction(e);
                if (document.getElementById('divAddNew').style.display != 'block') {
                    OnAddButtonClick();
                }
            }
        }

        function StopDefaultAction(e) {
            if (e.preventDefault) { e.preventDefault() }
            else { e.stop() };

            e.returnValue = false;
            e.stopPropagation();
        }

        function ReloadPage() {
            $('#<%=hdnNotelNo.ClientID %>').val('');
            window.location.reload();
        }

        function CloseGridLookup() {
            gridLookup.ConfirmCurrentSelection();
            gridLookup.HideDropDown();
            gridLookup.Focus();
        }
    </script>
    <style>
        .dxgv {
            display: none;
        }

            .dxgv.dx-al, .dxgv.dx-ar, .dx-nowrap.dxgv, .gridcellleft.dxgv, .dxgv.dx-ac, .dxgvCommandColumn_PlasticBlue.dxgv.dx-ac {
                display: table-cell !important;
            }

        #grid_DXMainTable tr td:first-child {
            display: table-cell !important;
        }

        .dxgvControl_PlasticBlue td.dxgvBatchEditModifiedCell_PlasticBlue {
            background: #fff !important;
        }

        #GvJvSearch_DXMainTable .dxgv {
            display: table-cell !important;
        }

        #GvJvSearch_DXFilterRow .dxgv {
            display: table-cell !important;
        }

        #lookup_Customer_DDD_gv_DXMainTable .dxgv {
            display: table-cell !important;
        }

        #lookup_Customer_DDD_gv_DXFilterRow .dxgv {
            display: table-cell !important;
        }

        .pullleftClass {
            position: absolute;
            right: 10px;
            top: 32px;
        }

        .crossBtn.CloseBtn {
            border: none;
            margin-right: 10px;
        }

            .crossBtn.CloseBtn input {
                padding: 0;
                border: none;
            }
    </style>
    <script>
        //Code for UDF Control 
        function OpenUdf() {
            if (document.getElementById('IsUdfpresent').value == '0') {
                jAlert("UDF not define.");
            }
            else {
                var keyVal = document.getElementById('Keyval_internalId').value;
                var url = '/Opening/ERP/frm_BranchUdfPopUp.aspx?Type=VNOTE&&KeyVal_InternalID=' + keyVal;
                popup.SetContentUrl(url);
                popup.Show();
            }
            return true;
        }
        // End Udf Code
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title clearfix">
            <h3 class="clearfix"><span class="pull-left">
                <asp:Label ID="lblHeading" runat="server" Text="Vendor Credit/Debit Note"></asp:Label></span>
            </h3>
            <div id="btncross" class="crossBtn" style="display: none; margin-left: 50px;"><a href="javascript:ReloadPage()"><i class="fa fa-times"></i></a></div>
            <%-- <div id="btncross" style="display: none; margin-left: 50px;" class="crossBtn CloseBtn">
                <asp:ImageButton ID="imgClose" runat="server" ImageUrl="/assests/images/CrIcon.png" OnClick="imgClose_Click" />
            </div>--%>
        </div>
    </div>
    <div class="form_main">
        <div id="TblSearch" class="clearfix">
            <div class="clearfix">
                <div style="float: left; padding-right: 5px;">
                    <% if (rights.CanAdd)
                       { %>
                    <a href="javascript:void(0);" onclick="OnAddButtonClick()" class="btn btn-primary"><span><u>A</u>dd New</span> </a>
                    <% } %>
                    <% if (rights.CanExport)
                       { %>
                    <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                        <asp:ListItem Value="0">Export to</asp:ListItem>
                        <asp:ListItem Value="1">PDF</asp:ListItem>
                        <asp:ListItem Value="2">XLS</asp:ListItem>
                        <asp:ListItem Value="3">RTF</asp:ListItem>
                        <asp:ListItem Value="4">CSV</asp:ListItem>
                    </asp:DropDownList>
                    <% } %>
                </div>
            </div>
            <div class="clearfix">
                <dxe:ASPxGridView ID="GvJvSearch" runat="server" AutoGenerateColumns="False" SettingsBehavior-AllowSort="true"
                    ClientInstanceName="cGvJvSearch" KeyFieldName="DCNote_ID" Width="100%"
                    OnCustomCallback="GvJvSearch_CustomCallback" OnCustomButtonInitialize="GvJvSearch_CustomButtonInitialize" OnDataBinding="GvJvSearch_DataBinding">
                    <clientsideevents custombuttonclick="CustomButtonClick" endcallback="function(s, e) {GvJvSearch_EndCallBack();}" />
                    <settingsbehavior confirmdelete="True" />
                    <styles>
                        <Header SortingImageSpacing="5px" ImageSpacing="5px"></Header>
                        <FocusedRow HorizontalAlign="Left" VerticalAlign="Top" CssClass="gridselectrow"></FocusedRow>
                        <LoadingPanel ImageSpacing="10px"></LoadingPanel>
                        <FocusedGroupRow CssClass="gridselectrow"></FocusedGroupRow>
                        <Footer CssClass="gridfooter"></Footer>
                    </styles>
                    <settingspager numericbuttoncount="10" pagesize="10" showseparators="True" alwaysshowpager="True">
                        <FirstPageButton Visible="True">
                        </FirstPageButton>
                        <LastPageButton Visible="True">
                        </LastPageButton>
                    </settingspager>
                    <columns>
                       <%-- <dxe:GridViewDataTextColumn VisibleIndex="0" FieldName="NoteType" Caption="Type">
                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        </dxe:GridViewDataTextColumn>--%>
                        <dxe:GridViewDataComboBoxColumn Caption="Type" FieldName="NoteType" VisibleIndex="0">
                            <PropertiesComboBox EnableSynchronization="False" EnableIncrementalFiltering="True"
                                ValueType="System.String" DataSourceID="SqlDataSourceapplicable" TextField="TypeName" ValueField="TypeID">
                            </PropertiesComboBox>
                        </dxe:GridViewDataComboBoxColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="NoteDate" Caption="Date">
                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="NoteNumber" Caption="Note Number">
                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="Currency" Caption="Currency">
                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="VendorName" Caption="Vendor Name">
                            <CellStyle CssClass="gridcellleft"></CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="Amount" Caption="Amount">
                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="EnteredBy" Caption="Entered On">
                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="UpdateOn" Caption="Last Update On">
                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="8" FieldName="UpdatedBy" Caption="Updated By">
                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Visible="False" FieldName="DCNote_ID"></dxe:GridViewDataTextColumn>
                        <dxe:GridViewCommandColumn VisibleIndex="8" Width="130px" ButtonType="Image" Caption="Actions" HeaderStyle-HorizontalAlign="Center">
                            <CustomButtons>
                            <%--     <dxe:GridViewCommandColumnCustomButton ID="CustomBtnView"  meta:resourcekey="GridViewCommandColumnCustomButtonResource3" Image-ToolTip="View" Styles-Style-CssClass="pad" >
                                    <Image Url="/assests/images/viewIcon.png" ToolTip="View"></Image>
                                </dxe:GridViewCommandColumnCustomButton>--%>
                                <dxe:GridViewCommandColumnCustomButton ID="CustomBtnEdit" meta:resourcekey="GridViewCommandColumnCustomButtonResource1" Image-ToolTip="Edit" Styles-Style-CssClass="pad">
                                    <Image Url="/assests/images/Edit.png"></Image>
                                </dxe:GridViewCommandColumnCustomButton>
                                <dxe:GridViewCommandColumnCustomButton ID="CustomBtnDelete" meta:resourcekey="GridViewCommandColumnCustomButtonResource2" Image-ToolTip="Delete" Styles-Style-CssClass="pad">
                                    <Image Url="/assests/images/Delete.png"></Image>
                                </dxe:GridViewCommandColumnCustomButton>
                                   <%--   <dxe:GridViewCommandColumnCustomButton ID="CustomBtnPrint"  meta:resourcekey="GridViewCommandColumnCustomButtonResource3" Image-ToolTip="Print" Styles-Style-CssClass="pad" Visible="false">
                                       <Image Url="/assests/images/Print.png"></Image>
                                     </dxe:GridViewCommandColumnCustomButton>--%>

                                <%--Print Customer Debit/Credit Note--%>
                                <%--<dxe:GridViewCommandColumnCustomButton ID="CustomBtnPrint" meta:resourcekey="GridViewCommandColumnCustomButtonResource3" Image-ToolTip="Print" Styles-Style-CssClass="pad">
                                    <Image Url="/assests/images/Print.png"></Image>
                                </dxe:GridViewCommandColumnCustomButton>--%>
                                <%--End Print Customer Debit/Credit Note--%>
                            </CustomButtons>
                        </dxe:GridViewCommandColumn>
                    </columns>
                    <settings showgrouppanel="True" showstatusbar="Visible" showfilterrow="true" showfilterrowmenu="true" />
                    <settingssearchpanel visible="True" />
                </dxe:ASPxGridView>
            </div>
        </div>
        <div id="divAddNew" style="display: none;" class="clearfix">
            <div style="background: #f5f4f3; padding: 8px 0; margin-bottom: 0px; border-radius: 4px; border: 1px solid #ccc;" class="clearfix">
                <div class="col-md-3">
                    <label>Note Type</label>
                    <div>
                        <asp:DropDownList ID="ddlNoteType" runat="server" TabIndex="1" Width="100%" onchange="ddlNoteType_ValueChange()">
                            <asp:ListItem Text="Credit Note" Value="Cr" />
                            <asp:ListItem Text="Debit Note" Value="Dr" />
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-3" id="div_Edit">
                    <label>Select Numbering Scheme</label>
                    <div>
                        <asp:DropDownList ID="CmbScheme" runat="server" DataSourceID="SqlSchematype" TabIndex="2"
                            DataTextField="SchemaName" DataValueField="ID" Width="100%"
                            onchange="CmbScheme_ValueChange()">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-2 lblmTop8">
                    <label>Document No.</label>
                    <div>
                        <asp:TextBox ID="txtBillNo" runat="server" Width="95%" meta:resourcekey="txtBillNoResource1" TabIndex="3" MaxLength="30" onchange="txtBillNo_TextChanged()"></asp:TextBox>
                        <span id="MandatoryBillNo" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                        <span id="duplicateMandatoryBillNo" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Duplicate Journal No."></span>
                    </div>
                </div>
                <div class="col-md-2 lblmTop8">
                    <label style="">Date</label>
                    <div>
                        <dxe:ASPxDateEdit ID="tDate" runat="server" EditFormat="Custom" ClientInstanceName="tDate" TabIndex="4"
                            UseMaskBehavior="True" Width="100%" meta:resourcekey="tDateResource1">
                            <clientsideevents datechanged="function(s,e){DateChange()}" />
                        </dxe:ASPxDateEdit>
                    </div>
                </div>
                <div class="col-md-2 lblmTop8">
                    <label>Branch</label>
                    <div>
                        <asp:DropDownList ID="ddlBranch" runat="server" DataSourceID="dsBranch" TabIndex="5"
                            DataTextField="BANKBRANCH_NAME" DataValueField="BANKBRANCH_ID" Width="100%"
                            meta:resourcekey="ddlBranchResource1" onfocus="BranchGotFocus()" onchange="ddlBranch_ChangeIndex()">
                        </asp:DropDownList>
                    </div>
                </div>
                <div style="clear: both;"></div>
                <div class="col-md-3">
                    <dxe:ASPxLabel ID="lbl_Customer" runat="server" Text="Vendor">
                    </dxe:ASPxLabel>
                    <%-- <i id="openlink" class="fa fa-plus-circle" aria-hidden="true"></i>--%>
                    <dxe:ASPxGridLookup ID="lookup_Customer" runat="server" TabIndex="6" ClientInstanceName="gridLookup" DataSourceID="dsCustomer"
                        KeyFieldName="cnt_internalid" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False">
                        <columns>
                            <dxe:GridViewDataColumn FieldName="shortname" Visible="true" VisibleIndex="0" Caption="Short Name" Width="200px" Settings-AutoFilterCondition="Contains" />
                            <dxe:GridViewDataColumn FieldName="Name" Visible="true" VisibleIndex="1" Caption="Name" Settings-AutoFilterCondition="Contains" Width="200px">
                                <%-- <Settings AllowAutoFilter="False"></Settings>--%>
                            </dxe:GridViewDataColumn>
                            <dxe:GridViewDataColumn FieldName="Type" Visible="true" VisibleIndex="2" Caption="Type" Settings-AutoFilterCondition="Contains" Width="200px">
                                <%-- <Settings AllowAutoFilter="False"></Settings>--%>
                            </dxe:GridViewDataColumn>
                            <dxe:GridViewDataColumn FieldName="cnt_internalid" Visible="false" VisibleIndex="3" Settings-AllowAutoFilter="False" Width="200px">
                                <Settings AllowAutoFilter="False"></Settings>
                            </dxe:GridViewDataColumn>
                        </columns>
                        <gridviewproperties settings-verticalscrollbarmode="Auto">
                            <Templates>
                                <StatusBar>
                                    <table class="OptionsTable" style="float: right">
                                        <tr>
                                            <td>
                                                <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridLookup" UseSubmitBehavior="False" />
                                            </td>
                                        </tr>
                                    </table>
                                </StatusBar>
                            </Templates>
                            <SettingsBehavior AllowFocusedRow="True" AllowSelectSingleRowOnly="True"></SettingsBehavior>
                            <SettingsLoadingPanel Text="Please Wait..." />
                            <Settings ShowFilterRow="True" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                        </gridviewproperties>
                        <clearbutton displaymode="Auto">
                        </clearbutton>
                    </dxe:ASPxGridLookup>
                    <span id="MandatorysCustomer" class="fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none; right: -3px; top: 24px;" title="Mandatory"></span>
                </div>
                <div class="col-md-1 lblmTop8">
                    <dxe:ASPxLabel ID="lbl_Currency" runat="server" Text="Currency">
                    </dxe:ASPxLabel>
                    <asp:DropDownList ID="ddl_Currency" runat="server" Width="95%" TabIndex="7">
                    </asp:DropDownList>
                </div>
                <div class="col-md-1 lblmTop8">
                    <dxe:ASPxLabel ID="lbl_Rate" runat="server" Text="Rate">
                    </dxe:ASPxLabel>
                    <dxe:ASPxTextBox ID="txt_Rate" ClientInstanceName="ctxt_Rate" runat="server" TabIndex="8" Width="90%" Height="28px">
                        <validationsettings requiredfield-isrequired="false" display="None"></validationsettings>
                        <masksettings mask="<0..999999999>.<0..9999>" allowmousewheel="false" />
                    </dxe:ASPxTextBox>
                </div>
                <div class="col-md-3">
                </div>
                <div class="col-md-3">
                </div>
            </div>
            <div class="clearfix">
                <br />
                <dxe:ASPxGridView runat="server" OnBatchUpdate="grid_BatchUpdate" KeyFieldName="CashReportID" ClientInstanceName="grid" ID="grid"
                    Width="100%" OnCellEditorInitialize="grid_CellEditorInitialize" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false"
                    Settings-ShowFooter="false" OnCustomCallback="grid_CustomCallback" OnDataBinding="grid_DataBinding"
                    OnRowInserting="Grid_RowInserting" OnRowUpdating="Grid_RowUpdating" OnRowDeleting="Grid_RowDeleting"
                    SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="150">
                    <settingspager visible="false"></settingspager>
                    <columns>
                        <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="true" Width="50" VisibleIndex="0" Caption="Action">
                            <CustomButtons>
                                <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomDelete" Image-Url="/assests/images/crs.png">
                                </dxe:GridViewCommandColumnCustomButton>
                            </CustomButtons>
                        </dxe:GridViewCommandColumn>
                        <dxe:GridViewDataComboBoxColumn Caption="Main Account" FieldName="CountryID" VisibleIndex="1" Width="300">
                            <PropertiesComboBox ValueField="CountryID" ClientInstanceName="CountryID" TextField="CountryName" ClearButton-DisplayMode="Always" AllowMouseWheel="false">
                                <%-- <ValidationSettings RequiredField-IsRequired="true" Display="None"></ValidationSettings>--%>
                                <ClientSideEvents SelectedIndexChanged="CountriesCombo_SelectedIndexChanged" />
                            </PropertiesComboBox>
                        </dxe:GridViewDataComboBoxColumn>
                        <dxe:GridViewDataComboBoxColumn FieldName="CityID" Caption="Sub Account" VisibleIndex="2" Width="300">
                            <PropertiesComboBox TextField="CityName" ValueField="CityID">
                            </PropertiesComboBox>
                            <EditItemTemplate>
                                <dxe:ASPxComboBox runat="server" OnInit="CityCmb_Init" Width="100%" EnableIncrementalFiltering="true" TextField="CityName" ClearButton-DisplayMode="Always"
                                    OnCallback="CityCmb_Callback" ValueField="CityID" ID="CityCmb" ClientInstanceName="CityID" EnableCallbackMode="true" AllowMouseWheel="false">
                                    <ClientSideEvents EndCallback="CitiesCombo_EndCallback" />
                                    <ValidationSettings RequiredField-IsRequired="true" Display="None"></ValidationSettings>
                                </dxe:ASPxComboBox>
                                <%--EnableCallbackMode="true"  OnInit="CityCmb_Init"  --%>
                            </EditItemTemplate>
                        </dxe:GridViewDataComboBoxColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="WithDrawl" Caption="Amount" Width="180" EditCellStyle-HorizontalAlign="Right">
                            <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                <MaskSettings Mask="<0..999999999999>.<0..99>" AllowMouseWheel="false" />
                                <ClientSideEvents KeyDown="OnKeyDown" LostFocus="WithDrawlTextChange"
                                    GotFocus="function(s,e){
                                    DebitGotFocus(s,e);
                                    }" />
                                <ClientSideEvents />
                                <ValidationSettings Display="None"></ValidationSettings>
                            </PropertiesTextEdit>
                            <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="Narration" Caption="Narration">
                            <PropertiesTextEdit>
                                <ClientSideEvents KeyDown="AddBatchNew"></ClientSideEvents>
                            </PropertiesTextEdit>
                            <CellStyle Wrap="False" HorizontalAlign="Left" CssClass="gridcellleft"></CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn FieldName="CashReportID" Caption="Srl No" ReadOnly="true" HeaderStyle-CssClass="hide" Width="0">
                        </dxe:GridViewDataTextColumn>
                    </columns>
                    <totalsummary>
                        <dxe:ASPxSummaryItem SummaryType="Sum" FieldName="C2" Tag="C2_Sum" />
                    </totalsummary>
                    <clientsideevents init="OnInit" endcallback="OnEndCallback" batcheditstartediting="OnBatchEditStartEditing" batcheditendediting="OnBatchEditEndEditing"
                        custombuttonclick="OnCustomButtonClick" />
                    <settingsdatasecurity allowedit="true" />
                    <settingsediting mode="Batch" newitemrowposition="Bottom">
                        <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="row" />
                    </settingsediting>
                </dxe:ASPxGridView>
            </div>
            <div class="text-center">
                <table style="margin-left: 590px; margin-top: 5px; margin-bottom: 5px">
                    <tr>
                        <td style="padding-right: 50px">Total Amount</td>
                        <td>
                            <dxe:ASPxTextBox ID="txt_Debit" runat="server" Width="105px" ClientInstanceName="c_txt_Debit" HorizontalAlign="Right" Font-Size="12px" ReadOnly="true">
                                <masksettings mask="<0..999999999999>.<0..99>" includeliterals="DecimalSymbol" />
                            </dxe:ASPxTextBox>
                        </td>
                        <td style="display: none;">
                            <dxe:ASPxTextBox ID="txt_Credit" runat="server" Width="105px" ClientInstanceName="c_txt_Credit" HorizontalAlign="Right" Font-Size="12px" ReadOnly="true">
                                <masksettings mask="<0..999999999999>.<0..99>" includeliterals="DecimalSymbol" />
                            </dxe:ASPxTextBox>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="clearfix" style="background: #f5f4f3; padding: 8px 0; margin-bottom: 15px; border-radius: 4px; border: 1px solid #ccc;">
                <div class="col-md-12">
                    <label>Main Narration</label>
                    <div>
                        <asp:TextBox ID="txtNarration" Font-Names="Arial" runat="server" TextMode="MultiLine"
                            Width="100%" meta:resourcekey="txtNarrationResource1" Height="40px"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div>
                <dxe:ASPxButton ID="btnSaveRecords" ClientInstanceName="cbtnSaveRecords" runat="server" AutoPostBack="False" Text="S&#818;ave & New" CssClass="btn btn-primary" UseSubmitBehavior="False">
                    <clientsideevents click="function(s, e) {SaveButtonClick();}" />
                </dxe:ASPxButton>
                <dxe:ASPxButton ID="btn_SaveRecords" ClientInstanceName="cbtn_SaveRecords" runat="server" AutoPostBack="False" Text="Save & Ex&#818;it" CssClass="btn btn-primary" UseSubmitBehavior="False">
                    <clientsideevents click="function(s, e) {SaveExitButtonClick();}" />
                </dxe:ASPxButton>
                <dxe:ASPxButton ID="btnUDF" ClientInstanceName="cbtnUDF" runat="server" AutoPostBack="False" Text="U&#818;DF" CssClass="btn btn-primary" UseSubmitBehavior="False">
                    <clientsideevents click="function(s, e) {if(OpenUdf()){ return false}}" />
                </dxe:ASPxButton>
            </div>
            <div id="loadCurrencyMassage" style="display: none;">
                <label><span style="color: red; font-weight: bold; font-size: medium;">**  Mismatch detected in Total of Debit & Credit Amount.</span></label>
            </div>
        </div>
    </div>
    <div>
        <asp:SqlDataSource ID="dsBranch" runat="server" ConflictDetection="CompareAllValues"
            SelectCommand="SELECT BRANCH_id AS BANKBRANCH_ID , RTRIM(BRANCH_DESCRIPTION)+' ['+ISNULL(RTRIM(BRANCH_CODE),'')+']' AS BANKBRANCH_NAME  FROM TBL_MASTER_BRANCH"></asp:SqlDataSource>

        <asp:SqlDataSource ID="SqlSchematype" runat="server"
            SelectCommand="Select * From ((Select '0' as ID,'Select' as SchemaName) Union (Select ID,SchemaName From tbl_master_Idschema  Where TYPE_ID=(Case When @NoteType='Dr' Then '27' Else '28' End) AND IsActive=1 AND Isnull(Branch,'')=@userbranchID AND Isnull(comapanyInt,'')=@LastCompany AND financial_year_id=(Select Top 1 FinYear_ID FROM Master_FinYear WHERE FinYear_Code=@LastFinYear))) as x Order By ID asc">
            <SelectParameters>
                <asp:SessionParameter Name="LastCompany" SessionField="LastCompany" />
                <asp:SessionParameter Name="userbranchID" SessionField="userbranchID" />
                <asp:SessionParameter Name="LastFinYear" SessionField="LastFinYear" />
                <asp:ControlParameter Name="NoteType" ControlID="ddlNoteType" PropertyName="SelectedValue" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource runat="server" ID="dsCustomer"
            SelectCommand="proc_DebitCreditNoteDetails" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:Parameter Type="String" Name="Action" DefaultValue="PopulateVendorDetail" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDataSourceapplicable" runat="server" 
            SelectCommand="(SELECT 'Debit Note' as TypeID,'Debit Note' as TypeName) Union (SELECT 'Credit Note' as TypeID,'Credit Note' as TypeName)"></asp:SqlDataSource>
    </div>
    <div>
        <asp:HiddenField ID="hdnSegmentid" runat="server" />
        <asp:HiddenField ID="hdnMode" runat="server" />
        <asp:HiddenField ID="hdnSchemaType" runat="server" />
        <asp:HiddenField ID="hdnSchemaID" runat="server" />
        <asp:HiddenField ID="hdnRefreshType" runat="server" />
        <asp:HiddenField ID="hdnNotelNo" runat="server" />
        <asp:HiddenField ID="hdfLookupCustomer" runat="server" />
        <asp:HiddenField runat="server" ID="IsUdfpresent" />
        <asp:HiddenField runat="server" ID="Keyval_internalId" />
    </div>
    <div style="display: none">
        <%-- <asp:Button ID="btnPrint" runat="server" Text="Print" OnClick="btnPrint_Click" meta:resourcekey="btnPrintResource1" />--%>
        <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A3" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
    </div>
    <dxe:ASPxPopupControl ID="ASPXPopupControl2" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popup" Height="630px"
        Width="600px" HeaderText="Add/Modify UDF" Modal="true" AllowResize="true" ResizingMode="Postponed">
        <contentcollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </contentcollection>
    </dxe:ASPxPopupControl>
    <dxe:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" ContainerElementID="grid"
        Modal="True">
    </dxe:ASPxLoadingPanel>
</asp:Content>
