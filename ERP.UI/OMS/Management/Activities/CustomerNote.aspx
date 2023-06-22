<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                20-06-2023        2.0.38           Pallab              26395: Customer Debit/Credit Note module all bootstrap modal outside click event disable
====================================================== Revision History =============================================--%>

<%@ Page Title="Customer Debit/Credit Note" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="CustomerNote.aspx.cs" Inherits="ERP.OMS.Management.Activities.CustomerNote" EnableEventValidation="false" %>

<%@ Register Src="~/OMS/Management/Activities/UserControls/BillingShippingControl.ascx" TagPrefix="ucBS" TagName="BillingShippingControl" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="JS/SearchPopup.js"></script>
    <link href="../Activities/CSS/SearchPopup.css" rel="stylesheet" />
    <script type="text/javascript">
        function OnBatchEditStartEditing(s, e) {
            currentEditableVisibleIndex = e.visibleIndex;
            globalRowIndex = e.visibleIndex;
        }
        function CustomerButnClick(s, e) {
            $('#CustModel').modal('show');
        }

        function Customer_KeyDown(s, e) {
            if (e.htmlEvent.key == "Enter") {
                $('#CustModel').modal('show');
            }
        }
        function Customerkeydown(e) {
            var OtherDetails = {}
            OtherDetails.SearchKey = $("#txtCustSearch").val();
            // OtherDetails.BranchID = $('#ddl_Branch').val();

            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var HeaderCaption = [];
                HeaderCaption.push("Customer Name");
                HeaderCaption.push("Unique Id");
                HeaderCaption.push("Address");
                if ($("#txtCustSearch").val() != "") {
                    callonServer("Services/Master.asmx/GetCustomer", OtherDetails, "CustomerTable", HeaderCaption, "customerIndex", "SetCustomer");
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
                ctxtCustName.SetText(Name);
                GetObjectID('hdnCustomerId').value = key;
                $('#CustModel').modal('hide');
                GetInvoiceDetails();
                if (document.getElementById('ddlNoteType').value == "Cr") {
                    $('#txtPartyInvoice').focus();
                }
                else {
                    cddlInvoice.Focus();
                }
            }
        }
        //function ValueSelected(e, indexName) {
        //    if (e.code == "Enter") {
        //        var Id = e.target.parentElement.parentElement.cells[0].innerText;
        //        var name = e.target.parentElement.parentElement.cells[1].children[0].value;
        //        if (Id) {
        //            SetCustomer(Id, name);
        //        }
        //    }
        //    else if (e.code == "ArrowDown") {
        //        thisindex = parseFloat(e.target.getAttribute(indexName));
        //        thisindex++;
        //        if (thisindex < 10)
        //            $("input[" + indexName + "=" + thisindex + "]").focus();
        //    }
        //    else if (e.code == "ArrowUp") {
        //        thisindex = parseFloat(e.target.getAttribute(indexName));
        //        thisindex--;
        //        if (thisindex > -1) {
        //            $("input[" + indexName + "=" + thisindex + "]").focus();
        //        }
        //        else {
        //            $('#txtCustSearch').focus();
        //        }
        //    }
        //}
    </script>
    <script language="javascript" type="text/javascript">
        // var shouldCheck = 0;
        var IsSubAccount = '';
        var globalRowIndex;
        function MainAccountNewkeydown(e) {
            var OtherDetails = {}
            OtherDetails.SearchKey = $("#txtMainAccountSearch").val();
            OtherDetails.branchId = $("#ddlBranch").val();
            if (e.code == "Enter" || e.code == "NumpadEnter") {
                if ($("#txtMainAccountSearch").val() == "")
                    return;
                var HeaderCaption = [];
                HeaderCaption.push("Main Account Name");
                HeaderCaption.push("Short Name");
                HeaderCaption.push("Subledger Type");

                callonServer("/OMS/Management/Activities/Services/Master.asmx/GetMainAccountCustomerNote", OtherDetails, "MainAccountTable", HeaderCaption, "MainAccountIndex", "SetMainAccount");
            }
            else if (e.code == "ArrowDown") {
                if ($("input[MainAccountIndex=0]"))
                    $("input[MainAccountIndex=0]").focus();
            }
            else if (e.code == "Escape") {
                $('#MainAccountModel').modal('hide');
                grid.batchEditApi.StartEdit(globalRowIndex, 1);
            }
        }
        function SetMainAccount(Id, name, e) {
            $('#MainAccountModel').modal('hide');
            //var IsSub = e.target.parentElement.parentElement.children[2].innerText;
            var IsSub = e.parentElement.cells[3].innerText;
            GetMainAcountComboBox(Id, name, IsSub);
            setTimeout(function () {
                grid.batchEditApi.StartEdit(globalRowIndex, 2);
                return;
            }, 300);

            ctxtCustName.SetEnabled(false);
        }
        function ValueSelected(e, indexName) {
            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var Id = e.target.parentElement.parentElement.cells[0].innerText;
                var name = e.target.parentElement.parentElement.cells[1].children[0].value;
                if (Id) {
                    if (indexName == "MainAccountIndex") {
                        $('#MainAccountModel').modal('hide');
                        var IsSub = e.target.parentElement.parentElement.children[3].innerText;
                        GetMainAcountComboBox(Id, name, IsSub);
                        setTimeout(function () {
                            grid.batchEditApi.StartEdit(globalRowIndex, 2);
                            return;
                        }, 300);
                    }
                    else if (indexName == "SubAccountIndex") {
                        $('#SubAccountModel').modal('hide');
                        GetSubAcountComboBox(Id, name);
                        setTimeout(function () {
                            grid.batchEditApi.StartEdit(globalRowIndex, 3);
                            return;
                        }, 300);
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
                    if (indexName == "MainAccountIndex") {
                        $('#txtMainAccountSearch').focus();
                    }
                    else if (indexName == "SubAccountIndex") {
                        $('#txtSubAccountSearch').focus();
                    }
                    else {
                        if (document.getElementById('ddlNoteType').value == "Cr") {
                            $('#txtPartyInvoice').focus();
                        }
                        else {
                            cddlInvoice.focus();
                        }
                    }
                }
            }
            else if (e.code == "Escape") {
                if (indexName == "MainAccountIndex") {
                    $('#MainAccountModel').modal('hide');
                    grid.batchEditApi.StartEdit(globalRowIndex, 2);
                }
                else if (indexName == "SubAccountIndex") {
                    $('#SubAccountModel').modal('hide');
                    grid.batchEditApi.StartEdit(globalRowIndex, 3);
                }
            }
        }
        function MainAccountButnClick(s, e) {
            if (e.buttonIndex == 0) {
                var txt = "<table border='1' width=\"100%\"  class=\"dynamicPopupTbl\"><tr class=\"HeaderStyle\" ><th>Main Account Name</th><th>Short Name</th><th>Subledger Type</th></tr><table>";
                document.getElementById("MainAccountTable").innerHTML = txt;
                $('#MainAccountModel').modal('show');
            }
        }
        function GetMainAcountComboBox(id, name, IsSub) {

            IsSubAccount = IsSub;
            grid.batchEditApi.StartEdit(globalRowIndex, 2);
            grid.GetEditor("CountryID").SetText(name);
            grid.GetEditor("gvColMainAccount").SetText(id);
            grid.GetEditor("CityID").SetValue("");
            grid.GetEditor("WithDrawl").SetValue("");
            grid.GetEditor("Narration").SetValue("");
            grid.GetEditor("gvColSubAccount").SetValue("");
            grid.GetEditor("IsSubledger").SetValue(IsSubAccount);

            var CashReportID = grid.GetEditor('CashReportID').GetValue();
            var strMode = document.getElementById('hdnMode').value;
            if (strMode == "1") {
                if (typeof (CashReportID) != "undefined" && CashReportID != null) {
                    grid.PerformCallback('DeleteTaxCalculate~' + CashReportID + "~" + id + "~" + name);
                }

            }

            if (grid.GetEditor('CashReportID').GetValue() != "" || grid.GetEditor('CashReportID').GetValue() != null) {
                grid.GetEditor('CashReportID').SetValue("");
            }

        }
        function closeModal() {
            $('#MainAccountModel').modal('hide');
            grid.batchEditApi.StartEdit(globalRowIndex, 2);
        }
        function SubAccountButnClick(s, e) {
            txt = " <table border='1' width=\"100%\"  class=\"dynamicPopupTbl\"><tr class=\"HeaderStyle\"><th>Sub Account Name [Unique Id]</th><th>Sub Account Code</th></tr></table>";
            document.getElementById("SubAccountTable").innerHTML = txt;
            $("#mainActMsgSub").hide();
            if (IsSubAccount != 'None') {
                grid.batchEditApi.StartEdit(e.visibleIndex);
                var strMainAccountID = (grid.GetEditor('CountryID').GetText() != null) ? grid.GetEditor('CountryID').GetText() : "0";
                var MainAccountID = (grid.GetEditor('gvColMainAccount').GetValue() != null) ? grid.GetEditor('gvColMainAccount').GetValue() : "0";
                if (e.buttonIndex == 0) {
                    if (strMainAccountID.trim() != "") {
                        document.getElementById('hdnMainAccountId').value = MainAccountID;
                        var FullName = new Array("", "");
                        $('#SubAccountModel').modal('show');
                    }
                }
            }
        }
        function SubAccountNewkeydown(e) {
            grid.batchEditApi.StartEdit(e.visibleIndex);
            var strMainAccountID = (grid.GetEditor('CountryID').GetText() != null) ? grid.GetEditor('CountryID').GetText() : "0";
            var MainAccountID = (grid.GetEditor('gvColMainAccount').GetValue() != null) ? grid.GetEditor('gvColMainAccount').GetValue() : "0";
            var OtherDetails = {}
            OtherDetails.SearchKey = $("#txtSubAccountSearch").val();
            OtherDetails.MainAccountCode = MainAccountID;
            if (e.code == "Enter" || e.code == "NumpadEnter") {
                if ($("#txtSubAccountSearch").val() == "")
                    return;
                var HeaderCaption = [];
                HeaderCaption.push("Sub Account Name [Unique Id]");
                HeaderCaption.push("Subledger Type");

                callonServer("/OMS/Management/Activities/Services/Master.asmx/GetSubAccountJournal", OtherDetails, "SubAccountTable", HeaderCaption, "SubAccountIndex", "SetSubAccount");
            }
            else if (e.code == "ArrowDown") {
                if ($("input[SubAccountIndex=0]"))
                    $("input[SubAccountIndex=0]").focus();
            }
            else if (e.code == "Escape") {
                $('#SubAccountModel').modal('hide');
                grid.batchEditApi.StartEdit(globalRowIndex, 3);
            }
        }
        function SetSubAccount(Id, name) {
            $('#SubAccountModel').modal('hide');
            //GetSubAcountComboBox(Id, name);
            var subAccountText = name;
            var subAccountID = Id;
            grid.batchEditApi.StartEdit(globalRowIndex);
            grid.GetEditor("CityID").SetText(subAccountText);
            grid.GetEditor("gvColSubAccount").SetText(subAccountID);
            setTimeout(function () { grid.batchEditApi.StartEdit(globalRowIndex, 3); }, 200);

        }
        function SubAccountClose(s, e) {
            cSubAccountpopUp.Hide();
            grid.batchEditApi.StartEdit(globalRowIndex, 3);
        }
        function MainAccountKeyDown(s, e) {
            if (e.htmlEvent.key == "Enter") {
                s.OnButtonClick(0);
            }
        }
        function SubAccountKeyDown(s, e) {
            if (e.htmlEvent.key == "Enter") {
                s.OnButtonClick(0);
            }
            //if (e.htmlEvent.key == "Delete") {
            //    var subAccountText = "";
            //    var subAccountID = "";
            //    grid.batchEditApi.StartEdit(globalRowIndex);
            //    grid.GetEditor("CityID").SetText(subAccountText);
            //    grid.GetEditor("gvColSubAccount").SetText(subAccountID);
            //}
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
            //$('#SubAccountModel').on('hide.bs.modal', function () {
            //})
        });

        function GetSubAcountComboBox(Id, name) {
            var subAccountText = name;
            var subAccountID = Id;
            grid.batchEditApi.StartEdit(globalRowIndex);
            grid.GetEditor("CityID").SetText(subAccountText);
            grid.GetEditor("gvColSubAccount").SetText(subAccountID);
            setTimeout(function () { grid.batchEditApi.StartEdit(globalRowIndex, 3); }, 500);
            //cSubAccountpopUp.Hide();             
        }
        function CloseSubModal() {
            $('#SubAccountModel').modal('hide');
            grid.batchEditApi.StartEdit(globalRowIndex, 3);
        }

    </script>
    <script>

        function GlobalBillingShippingEndCallBack() { };

        var DCNoteID = 0;
        function onPrintJv(id, NoteType) {
            debugger;
            DCNoteID = id;            
            cDocumentsPopup.Show();
            $('#HdCrDrNoteType').val(NoteType);
            cCmbDesignName.SetSelectedIndex(0);
            cSelectPanel.PerformCallback('Bindalldesignes');
            $('#btnOK').focus();
        }

        function PerformCallToGridBind() {
            cSelectPanel.PerformCallback('Bindsingledesign');
            cDocumentsPopup.Hide();
            return false;
        }

        function cSelectPanelEndCall(s, e) {

            if (cSelectPanel.cpSuccess != null) {
                var reportName = cCmbDesignName.GetValue();
                var module = 'CUSTDRCRNOTE';
                window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + DCNoteID, '_blank')
            }
            cSelectPanel.cpSuccess = null
            if (cSelectPanel.cpSuccess == null) {
                cCmbDesignName.SetSelectedIndex(0);
            }
        }

        function OnAddButtonClick() {
            $("#hfFirstTimeLoadFlag").val("1");
            InvoiceID = "";
            document.getElementById('divAddNew').style.display = 'block';
            TblSearch.style.display = "none";
            divcross.style.display = "block";
            $('#<%=hdnMode.ClientID %>').val('0'); //Entry
            $('#<%= lblHeading.ClientID %>').text("Add Customer Debit/Credit Note");
            var CmbScheme = document.getElementById("<%=CmbScheme.ClientID%>");
            CmbScheme.options[0].selected = true;

            grid.batchEditApi.EndEdit();
            document.getElementById('<%= txtBillNo.ClientID %>').disabled = true;
            document.getElementById('Keyval_internalId').value = "Add";
            //cexUpdatePanel.PerformCallback("SessClear");           
            grid.AddNewRow();
            grid.batchEditApi.EndEdit();
            document.getElementById("<%=ddlNoteType.ClientID%>").focus();
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

        function Customernoteledger(keyid) {

            $('#<%= lblHeading.ClientID %>').text("Modify Customer Debit/Credit Note");
            $('#<%=hdnMode.ClientID %>').val('1');
            document.getElementById('div_Edit').style.display = 'none';
            document.getElementById('<%= txtBillNo.ClientID %>').disabled = true;
            document.getElementById('divAddNew').style.display = 'block';
            //divcross.style.display = "block";
            TblSearch.style.display = "none";
            //cGvJvSearch.GetRowValues(VisibleIndexE, "BranchID", MainAccountCallBack);
            $('#btn_SaveRecords').attr('style', 'display:none;');
            $('#btnUDF').attr('style', 'display:none;');
            grid.PerformCallback('Editledger~' + keyid);
            LoadingPanel.Show();
            cbtnSaveRecords.SetVisible(false)
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
                //debugger;
                VisibleIndexE = e.visibleIndex;
                $('#<%= lblHeading.ClientID %>').text("Modify Customer Debit/Credit Note");
                $('#<%=hdnMode.ClientID %>').val('1');
                document.getElementById('div_Edit').style.display = 'none';
                document.getElementById('<%= txtBillNo.ClientID %>').disabled = true;

                document.getElementById('divAddNew').style.display = 'block';
                divcross.style.display = "block";
                TblSearch.style.display = "none";
                //cGvJvSearch.GetRowValues(VisibleIndexE, "BranchID", MainAccountCallBack);
                grid.PerformCallback('Edit~' + VisibleIndexE);

                LoadingPanel.Show();

                cbtnSaveRecords.SetVisible(false)
            }


            if (e.buttonID == 'CustomBtnView') {
                VisibleIndexE = e.visibleIndex;
                $('#<%= lblHeading.ClientID %>').text("View Customer Debit/Credit Note");
                $('#<%=hdnMode.ClientID %>').val('1');
                document.getElementById('div_Edit').style.display = 'none';
                document.getElementById('<%= txtBillNo.ClientID %>').disabled = true;

                document.getElementById('divAddNew').style.display = 'block';
                divcross.style.display = "block";
                TblSearch.style.display = "none";
                //cGvJvSearch.GetRowValues(VisibleIndexE, "BranchID", MainAccountCallBack);
                grid.PerformCallback('Edit~' + VisibleIndexE);
                LoadingPanel.Show();

                cbtnSaveRecords.SetVisible(false);
                cbtn_SaveRecords.SetVisible(false);
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
                //debugger;
                var keyValueindex = s.GetRowKey(e.visibleIndex);
                var index = s.GetRow(e.visibleIndex);
                var NoteType = index.children[0].innerHTML;
                onPrintJv(keyValueindex, NoteType);
                //jConfirm('Do you want to Print?', 'Confirmation Dialog', function (r) {
                //    if (r == true) {
                //        //cGvJvSearch.GetRowValues(keyValueindex, "JvID", onPrintJv)
                //        onPrintJv(keyValueindex);
                //    }
                //});
            }
        }

        //function onPrintJv(id) {
        // window.location.href = "../../reports/XtraReports/JournalVoucherReportViewer.aspx?id=" + id;
        // }

        function MainAccountCallBack(branch) {
            CountryID.PerformCallback(branch);
        }

        function GvJvSearch_EndCallBack() {
            if (cGvJvSearch.cpJVDelete != undefined && cGvJvSearch.cpJVDelete != null) {
                jAlert(cGvJvSearch.cpJVDelete);
                cGvJvSearch.cpJVDelete = null;
                updateGridByDate();
                //cGvJvSearch.PerformCallback('PCB_BindAfterDelete');
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

        var OldMainAccount;
        var CurrentMainAccount;

        function CountriesCombo_GotFocus(s, e) {
            OldMainAccount = grid.GetEditor('CountryID').GetValue();
        }

        function CountriesCombo_SelectedIndexChanged(s, e) {
            document.getElementById('CmbScheme').disabled = true;
            CurrentMainAccount = grid.GetEditor('CountryID').GetValue();

            if (grid.GetEditor('Parent_LedgerID').GetValue() != "0" && grid.GetEditor('Parent_LedgerID').GetValue() != "" && grid.GetEditor('Parent_LedgerID').GetValue() != null) {
                jAlert("You can not chnage this ledger!!", "Alert!!", function () {
                    //   alert(currentEditableVisibleIndex + '-' + OldMainAccount);
                    grid.batchEditApi.StartEdit(currentEditableVisibleIndex);
                    var tbMainAccountCell = grid.GetEditor("CountryID");
                    tbMainAccountCell.SetValue(OldMainAccount);
                });
            }
            else {
                var currentValue = grid.GetEditor('CountryID').GetValue();
                var oldSeletectedMainAccount = OldMainAccount;
                if (grid.GetEditor('CashReportID').GetValue() != null && grid.GetEditor('CashReportID').GetValue() != "") {
                    jConfirm('Taxes related to this ledger will be re-calculated. Confirm?', 'Confirmation Dialog', function (r) {
                        OldMainAccount = grid.GetEditor('CountryID').GetValue();
                        if (r == true) {
                            grid.PerformCallback('TaxReCalculate~' + grid.GetEditor('CashReportID').GetValue() + '~' + '0' + '~' + 'D~' + CurrentMainAccount); /// D reflects LedgerSelection
                        } else {
                            grid.batchEditApi.StartEdit(currentEditableVisibleIndex);
                            var tbMainAccountCell = grid.GetEditor("CountryID");
                            tbMainAccountCell.SetValue(oldSeletectedMainAccount);

                            return false;
                        }
                    });
                }

                //var currentValue = s.GetValue();
                if (lastCountryID == currentValue) {
                    if (CityID.GetSelectedIndex() < 0)
                        CityID.SetSelectedIndex(0);
                    return;
                }
                lastCountryID = currentValue;
                CityID.PerformCallback(currentValue);
            }
        }
        function IntializeGlobalVariables(grid) {
            lastCountryID = grid.cplastCountryID;
            currentEditableVisibleIndex = -1;
            setValueFlag = -1;
        }
        function OnInit(s, e) {
            IntializeGlobalVariables(s);
        }

        var InvoiceID = "";
        function OnEndCallback(s, e) {
            IntializeGlobalVariables(s);
            LoadingPanel.Hide();
            if (grid.cpEdit != null) {
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
                InvoiceID = grid.cpEdit.split('~')[11];
                var PartyInvoiceNo = grid.cpEdit.split('~')[12];
                var PartyInvoiceDate = grid.cpEdit.split('~')[13];
                var TaggedDoc = grid.cpEdit.split('~')[14];
                var CustomerName = grid.cpEdit.split('~')[15];
                var CustomerUnique = grid.cpEdit.split('~')[16];
                var ReasonID = grid.cpEdit.split('~')[17];
                document.getElementById('txtBillNo').value = VoucherNo;
                document.getElementById('txtNarration').value = Narration;
                ctxt_Rate.SetValue(Rate);
                document.getElementById('ddl_Currency').value = CurrencyId;
                document.getElementById('ddlNoteType').value = NoteType;
                document.getElementById('hdnNotelNo').value = NoteID;
                document.getElementById('ddl_Reason').value = ReasonID;

                ctxtCustName.SetValue(CustomerName);
                document.getElementById('hdnCustomerId').value = CustomerID;
                document.getElementById('Keyval_internalId').value = "CustomerNote" + NoteID;
                EditAddressSinglePage(NoteID, 'CN');
                if (NoteType == "Cr") {
                    document.getElementById('div_InvoiceNo').style.display = 'block';
                    document.getElementById('div_InvoiceDate').style.display = 'block';
                    txtPartyInvoice.SetValue(PartyInvoiceNo);
                    if (PartyInvoiceDate != "") {
                        var InvoiceDate = new Date(PartyInvoiceDate);
                        cPLPartyDate.SetDate(InvoiceDate);
                    }
                }
                else {
                    document.getElementById('div_InvoiceNo').style.display = 'none';
                    document.getElementById('div_InvoiceDate').style.display = 'none';
                }
                document.getElementById('ddlNoteType').disabled = true;
                document.getElementById('ddlBranch').value = BranchID;
                var Transdt = new Date(trDate);
                tDate.SetDate(Transdt);
                tDate.SetEnabled(false);
                var strSchemaType = document.getElementById('hdnSchemaType').value;
                var RefreshType = document.getElementById('hdnRefreshType').value;
                c_txt_Credit.SetValue(Credit);
                c_txt_Debit.SetValue(Debit);
                if (NoteType == "Dr") clblInvoiceNo.SetText('Ref. Sale Invoice No.');
                else if (NoteType == "Cr") clblInvoiceNo.SetText('Ref. Sale Return No.');
                if (NoteType != "" && CustomerID != "") {
                    cddlInvoice.PerformCallback(NoteType + '~' + CustomerID)
                }

                BtnVisible(TaggedDoc);
                grid.cpEdit = null;
            }
            var value = document.getElementById('hdnRefreshType').value;
            if (grid.cpSaveSuccessOrFail == "outrange") {
                grid.cpSaveSuccessOrFail = null;
                jAlert('Can Not Add More Customer Debit/Credit Note as Scheme Exausted.<br />Update The Scheme and Try Again');
                cbtnSaveRecords.SetVisible(true);
                cbtn_SaveRecords.SetVisible(true);
            }
            else if (grid.cpSaveSuccessOrFail == "duplicate") {
                grid.cpSaveSuccessOrFail = null;
                jAlert('Can Not Save as Duplicate Customer Debit/Credit Note No. Found');
                cbtnSaveRecords.SetVisible(true);
                cbtn_SaveRecords.SetVisible(true);
            }
            else if (grid.cpSaveSuccessOrFail == "errorInsert") {
                grid.cpSaveSuccessOrFail = null;
                jAlert('Try again later.');
                cbtnSaveRecords.SetVisible(true);
                cbtn_SaveRecords.SetVisible(true);
            }
            else if (grid.cpSaveSuccessOrFail == "TaxRequired") {
                grid.cpSaveSuccessOrFail = null;
                $('#<%=hdfLookupCustomer.ClientID %>').val('');
                $('#<%=hdnRefreshType.ClientID %>').val('D');
                $("#HDstatus").val('D');
                jAlert('Click on Plus(+) sign to calculate GST.');
                OnAddNewClick();
                cbtnSaveRecords.SetVisible(true);
                cbtn_SaveRecords.SetVisible(true);
            }
            else if (grid.cpSaveSuccessOrFail == "successInsert") {
                grid.cpSaveSuccessOrFail = null;
                var JV_Number = grid.cpVouvherNo;
                var JV_Msg = "Customer Debit/Credit Note No. " + JV_Number + " generated.";
                var strSchemaType = document.getElementById('hdnSchemaType').value;

                if (value == "E") {
                    var IsComplete = "0";
                    if (JV_Number != "") {
                        jAlert(JV_Msg, 'Alert Dialog: [CustomerNote]', function (r) {
                            if (r == true) {
                                window.location.reload();
                            }
                        });
                    } else {
                        window.location.reload();
                    }
                }
                else if (value == "S") {
                    if (JV_Number != "") {
                        jAlert(JV_Msg, 'Alert Dialog: [CustomerNote]', function (r) {
                            if (r == true) {
                                setTimeout(function () { $("#txtCustName").focus(); }, 500);
                            }
                        });
                    }
                    grid.AddNewRow();
                    $('#<%=hdnMode.ClientID %>').val('0');
                    $('#<%= lblHeading.ClientID %>').text("Add Customer Debit/Credit Note");
                    cbtnSaveRecords.SetVisible(true);
                    cbtn_SaveRecords.SetVisible(true);
                    document.getElementById('div_Edit').style.display = 'block';
                    document.getElementById('<%= txtBillNo.ClientID %>').disabled = true;
                    document.getElementById('<%= txtBillNo.ClientID %>').value = "";
                    document.getElementById('<%= txtBillNo.ClientID %>').disabled = true;
                    c_txt_Debit.SetValue("0");
                    c_txt_Credit.SetValue("0");
                    document.getElementById('<%= txtNarration.ClientID %>').value = "";

                    var ActiveCurrency = '<%=Session["LocalCurrency"]%>'
                    var Currency = ActiveCurrency.toString().split('~')[0];
                    document.getElementById('hdnNotelNo').value = "";
                    document.getElementById('Keyval_internalId').value = "Add";

                    if (strSchemaType == "0") {
                        document.getElementById('<%= txtBillNo.ClientID %>').disabled = false;
                        document.getElementById('<%= txtBillNo.ClientID %>').value = "";
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
                        document.getElementById('<%= txtBillNo.ClientID %>').disabled = true;
                        document.getElementById('<%= txtBillNo.ClientID %>').value = "";
                        var CmbScheme = document.getElementById("<%=CmbScheme.ClientID%>");
                        CmbScheme.options[0].selected = true;
                        document.getElementById("CmbScheme").focus();
                    }
        }
}
else if (grid.cpReCalTax != null) {
    grid.cpReCalTax = null;
    $("#HDParentSlNo").val(grid.GetRowKey(currentEditableVisibleIndex));
    OpenTaxPopUp();
}
else if (grid.cpReCalTaxLedger != null) {
    grid.cpReCalTaxLedger = null;
}
else {
    if (grid.cpDeleteTax != null) {
        grid.cpDeleteTax = null;

        setTimeout(function () {
            grid.batchEditApi.StartEdit(globalRowIndex, 2);
            return;
        }, 300);

    }
    else {
        if ($("#divAddNew").is(':visible')) {
            if (caspxTaxpopUp.GetVisible() == false) {

            }
        }
    }

}
    if (grid.cpView == "1") {
        grid.cpView = null;
        viewOnly();
    }
    if (value != "S") {
        c_txtTaxableAmount.SetValue(grid.cpTotalTaxableAmount);
        c_txtTaxAmount.SetValue(grid.cpTotalTaxAmount);
        c_txt_Debit.SetValue(grid.cpTotalAmount);
    }
   // grid.AddNewRow();
    if ($("#hfFocusSet").val() == "NT") {
        $("#hfFocusSet").val('');
        $('select[id^="ddlNoteType"]').eq(1).focus();
    }
    if ($("#hfFirstTimeLoadFlag").val() == "1") {
        $("#hfFirstTimeLoadFlag").val('');
        $('select[id^="ddlNoteType"]').eq(1).focus();
    }

}

function CitiesCombo_EndCallback(s, e) {

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
}

function RefreshData(cellInfo, countryID) {
    setValueFlag = cellInfo.value;
    CityID.PerformCallback(countryID);
}
//Debjyoti 
function OnCustomButtonClick(s, e) {
    if (e.buttonID == 'CustomDelete') {

        var arr = [];
        $('#grid').find('img,[type=image]').each(function () {
            arr.push($(this).attr('src')); // stores the original HTML attribute
        });

        var deletecount = 4;
        $.map(arr, function (i, k) {
            if (i == "/assests/images/crs.png") {
                deletecount++;
            }
        });
        if ($('#<%=hdnMode.ClientID %>').val() == '0') {
            deletecount = 4; ///force to call delete functionallity at Add mode
        }
        if (deletecount > 3) {
            grid.batchEditApi.EndEdit();
            var noofvisiblerows = grid.GetVisibleRowsOnPage();
            
            if (noofvisiblerows != "1") {
                grid.PerformCallback('Delete~' + grid.GetRowKey(e.visibleIndex));
                var debit = grid.batchEditApi.GetCellValue(e.visibleIndex, "WithDrawl");
                var credit = grid.batchEditApi.GetCellValue(e.visibleIndex, "Receipt");
                if (debit != 0)
                    c_txt_Debit.SetValue(c_txt_Debit.GetValue() - debit);
                if (credit != 0)
                    c_txt_Credit.SetValue(c_txt_Credit.GetValue() - credit);

                var Debit = parseFloat(c_txt_Debit.GetValue());
                var Credit = parseFloat(c_txt_Credit.GetValue());

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
        else {
            jAlert("You can not Delete. Atleast One ledger should be present in the list.");
        }
    }
    else if (e.buttonID == 'AddNew') {
        grid.batchEditApi.StartEdit(e.visibleIndex);
        if (grid.GetEditor('CountryID').GetValue() != null && grid.GetEditor('CountryID').GetValue() != '' && grid.GetEditor('WithDrawl').GetValue() != "0.00") {
            $("#hfCashReportID").val('');
            $("#hfLedgerName").val('');
            $("#hfLedgerID").val('');
            $("#hfWithDrawlAmount").val('');
            $("#HDParentSlNo").val(grid.GetRowKey(currentEditableVisibleIndex));
            var LedgerID = (grid.GetEditor('gvColMainAccount').GetValue() != null) ? grid.GetEditor('gvColMainAccount').GetValue() : "0";
            $.ajax({
                type: "POST",
                url: "CustomerNote.aspx/HSNSACMappingFLag",
                data: "{LedgerID:\"" + LedgerID + "\",TDate:\"" + tDate.GetText() + "\"}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (r) {

                    if (r.d == "1") {
                        if (cbsSCmbState.GetText() == null || cbsSCmbState.GetText() == "" || cbsSCmbState.GetText() == "0" || cbsBTxtAddress1 == "" || bsSTxtAddress1 == "") {
                            jAlert("Select ledger is mapped with HSN/SAC & GST not calculated due to Shipping State not entered!! Please enter shipping state and proceed further.", "Alert !!", function () {
                                OpenTaxPopUpFlag = 0;
                                page.SetActiveTabIndex(1);
                                cbsSave_BillingShipping.Focus();
                                page.tabs[0].SetEnabled(false);
                                $("#divcross").hide();
                            });
                        }
                        else {
                            OpenTaxPopUp();
                            var mainAccountValue = (grid.GetEditor('CountryID').GetValue() != null) ? grid.GetEditor('CountryID').GetValue() : "";
                            if (mainAccountValue != "") {
                            }
                        }
                    }
                    else {
                        //OpenTaxPopUp();
                        var mainAccountValue = (grid.GetEditor('CountryID').GetValue() != null) ? grid.GetEditor('CountryID').GetValue() : "";
                        if (mainAccountValue != "") {
                            grid.AddNewRow();
                            // grid.SetFocusedRowIndex();
                        }
                    }
                }
            });
        }
        else {
            if (grid.GetEditor('CountryID').GetValue() == null) {
                jAlert("Please select a ledger", "Alert!!", function () {
                    grid.batchEditApi.StartEdit(e.visibleIndex);
                });
            }
            else if (grid.GetEditor('WithDrawl').GetValue() == "0.00") {
                jAlert("Amount can not be Zero", "Alert!!", function () {
                    grid.batchEditApi.StartEdit(e.visibleIndex, 3);
                });
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
//var $MainGridAmount = '';
function DebitGotFocus(s, e) {
    debitOldValue = s.GetText();
    var indx = debitOldValue.indexOf(',');
    if (indx != -1) {
        debitOldValue = debitOldValue.replace(/,/g, '');
    }
}

function DebitLostFocus(s, e) {
    debitNewValue = s.GetText();
    ////samrat....
    //if (debitNewValue != debitOldValue) {
    //    jAlert("Due to amount chnages, all the previous tax will be deleted", "Alert", function () {
    //        OpenTaxPopUp();
    //        //grid.GetEditor('WithDrawl').SetText($MainGridAmount);
    //        //grid.GetEditor('WithDrawl').Focus();
    //    });
    //}

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
    if (e.htmlEvent.key == "Enter") {

        //console.log(grid.GetRowKey(currentEditableVisibleIndex));

        $("#HDParentSlNo").val(grid.GetRowKey(currentEditableVisibleIndex));
        OpenTaxPopUp();
    }
    if (keyCode === 13) {
        var mainAccountValue = (grid.GetEditor('CountryID').GetValue() != null) ? grid.GetEditor('CountryID').GetValue() : "";
        if (mainAccountValue != "") {
            //alert('2');
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
        // alert('3');
        grid.AddNewRow();
    }
    else if (gridcount > 0 && mainAccountValue != "") {
        //  alert('4');
        grid.AddNewRow();
    }
}
function WithDrawlTextChange(s, e) {
    DebitLostFocus(s, e);
    if (grid.GetEditor('CashReportID').GetValue() != '' && grid.GetEditor('CashReportID').GetValue() != null) {
        var Debit = parseFloat(s.GetValue());
        if (Debit != debitOldValue) {
            if (grid.GetEditor('Parent_LedgerID').GetText() != '0' && grid.GetEditor('Parent_LedgerID').GetText() != '') {
                jAlert('Amount can not be changed', 'Alert!!', function () {

                    grid.batchEditApi.SetCellValue(currentEditableVisibleIndex, "WithDrawl", debitOldValue)
                    //grid.GetEditor('WithDrawl').SetValue(debitOldValue);
                    var gridNarrationCell = grid.GetEditor('Narration');
                    gridNarrationCell.Focus();
                });
            }
            else {
                $("#hfCashReportID").val(grid.GetEditor('CashReportID').GetValue());
                $("#hfLedgerName").val(grid.GetEditor('CountryID').GetText());
                $("#hfLedgerID").val(grid.GetEditor('CountryID').GetValue());
                $("#hfWithDrawlAmount").val(grid.GetEditor('WithDrawl').GetValue());
                //grid.PerformCallback('TaxReCalculate~' + grid.GetEditor('CashReportID').GetValue() + '~' + Debit + '~' + 'A'); /// A reflects Amount Change
            }
        }
    }
    else {
        var mainAccountValue = (grid.GetEditor('CountryID').GetValue() != null) ? grid.GetEditor('CountryID').GetValue() : "";

        if (mainAccountValue != "") {
            // DebitLostFocus(s, e);
            var withDrawlValue = (grid.GetEditor('WithDrawl').GetValue() != null) ? parseFloat(grid.GetEditor('WithDrawl').GetValue()) : "0";
            var receiptValue = "0";
            if (withDrawlValue > 0) {
                recalculateCredit("0");
            }
            var Debit = parseFloat(c_txt_Debit.GetValue());
            var Credit = parseFloat(c_txt_Credit.GetValue());
        }
        else {
            grid.GetEditor('WithDrawl').SetValue("0");
        }
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

function ddlNoteType_ValueChange() {
    var val = document.getElementById("ddlNoteType").value;
    document.getElementById('<%= txtBillNo.ClientID %>').disabled = true;
    document.getElementById('<%= txtBillNo.ClientID %>').value = "";
    GetInvoiceDetails();

    if (val == "Cr") {
        document.getElementById('div_InvoiceNo').style.display = 'block';
        document.getElementById('div_InvoiceDate').style.display = 'block';
    }
    else {
        document.getElementById('div_InvoiceNo').style.display = 'none';
        document.getElementById('div_InvoiceDate').style.display = 'none';
    }

    $.ajax({
        type: "POST",
        url: "CustomerNote.aspx/GetScheme",
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

    document.getElementById('ddlNoteType').disabled = true;
}

function CmbScheme_ValueChange() {
    //var val = cCmbScheme.GetValue();

    var val = document.getElementById("CmbScheme").value;
    $('#<%=hdnSchemaID.ClientID %>').val(val);
    $("#MandatoryBillNo").hide();

    if (val != "0") {
        $.ajax({
            type: "POST",
            url: 'CustomerNote.aspx/getSchemeType',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{sel_scheme_id:\"" + val + "\"}",
            success: function (type) {
                //console.log(type);

                var schemetypeValue = type.d;
                var schemetype = schemetypeValue.toString().split('~')[0];
                var schemelength = schemetypeValue.toString().split('~')[1];
                $('#txtBillNo').attr('maxLength', schemelength);

                var branchID = (schemetypeValue.toString().split('~')[2] != null) ? schemetypeValue.toString().split('~')[2] : "";
                if (branchID != "") document.getElementById('ddlBranch').value = branchID;

                $('#<%=hdnBranchId.ClientID %>').val(branchID);
                GetInvoiceDetails();

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
    // alert('5');
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
    GetInvoiceDetails();

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
    //var customerval = ctxtCustName.GetValue();//(gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
    var customerval = GetObjectID('hdnCustomerId').value;
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
        else if (customerval == "" || customerval == null) {
            $("#MandatorysCustomer").show();
        }
        else if ($("#CmbScheme").val() == "0") {
            var Mode = document.getElementById('hdnMode').value;
            if (Mode == "0") {
                jAlert('Select Numbering Scheme');
            }

        }
        else {
            grid.batchEditApi.EndEdit();

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

            if (IsJournal == "Y") {
                $('#<%=hdfLookupCustomer.ClientID %>').val(customerval);
                $('#<%=hdnRefreshType.ClientID %>').val('S');
                $("#HDstatus").val("S");
                grid.AddNewRow();
                cbtn_SaveRecords.SetVisible(false);
                cbtnSaveRecords.SetVisible(false);
                grid.UpdateEdit();
            }
            else {
                jAlert('Cannot Save. You must enter atleast one Ledger to save this entry.');
            }
        }
}
}
function SaveExitButtonClick() {
    //var customerval = cCustomerComboBox.GetValue();//(gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
    var customerval = GetObjectID('hdnCustomerId').value;
    if (cbtn_SaveRecords.IsVisible() == true) {
        var val = document.getElementById("CmbScheme").value;
        var Branchval = $("#ddlBranch").val();
        $("#MandatoryBillNo").hide();
        $("#MandatorysCustomer").hide();
        var Mode = document.getElementById('hdnMode').value;
        if (document.getElementById('<%= txtBillNo.ClientID %>').value == "") {
            $("#MandatoryBillNo").show();
            document.getElementById('<%= txtBillNo.ClientID %>').focus();
        }
        else if (Branchval == "0") {
            document.getElementById('<%= ddlBranch.ClientID %>').focus();
            jAlert('Enter Branch');
        }
        else if (customerval == "" || customerval == null) {
            $("#MandatorysCustomer").show();
        }
        else if ($("#CmbScheme").val() == "0" && Mode == "0") {
            var Mode = document.getElementById('hdnMode').value;
            if (Mode == "0") {
                jAlert('Select Numbering Scheme');
            }

        }
        else {
            grid.batchEditApi.EndEdit();
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
            if (IsJournal == "Y") {
                $('#<%=hdfLookupCustomer.ClientID %>').val(customerval);
                $('#<%=hdnRefreshType.ClientID %>').val('E');
                $("#HDstatus").val("S");
                grid.AddNewRow();
                cbtn_SaveRecords.SetVisible(false);
                cbtnSaveRecords.SetVisible(false);
                grid.UpdateEdit();
            }
            else {
                jAlert('Cannot Save. You must enter atleast one Ledger to save this entry.');
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

        function OpenTaxPopUp() {
            if (GetObjectID('hdnCustomerId').value != null && GetObjectID('hdnCustomerId').value != "" && $('#txtBillNo').val().trim() != "") {

                if (grid.GetEditor('Parent_LedgerID').GetText() != '0' && grid.GetEditor('Parent_LedgerID').GetText() != '') {
                    jAlert('You can not change', 'Alert!!', function () {
                    });
                }
                else {
                    grid.batchEditApi.StartEdit(globalRowIndex, 1);
                    grid.UpdateEdit();
                    taxAmtButnClick();
                }
            }
            else {
                if ($('#txtBillNo').val().trim() == "") {
                    jAlert("Please Select Numbering Scheme", "Alert", function () {
                        $('#CmbScheme').focus();
                    });
                }
                    //else if (cCustomerComboBox.GetValue() == null || cCustomerComboBox.GetValue() == "")
                else if (GetObjectID('hdnCustomerId').value == null || GetObjectID('hdnCustomerId').value == "") {
                    jAlert("Please Select Customer", "Alert", function () {
                        //cCustomerComboBox.Focus();
                        ctxtCustName.Focus();
                    });
                }

            }
        }
        function CalculateTotalAmount() {
            $(function () {

                var $Parent_LedgerID = grid.GetEditor('Parent_LedgerID').GetText();
                var $amount = grid.GetEditor('WithDrawl').GetText();

                //    var $tr = $("#grid_DXMainTable > tbody > tr:gt(1)");
                //    var $amount = 0;

                //    $tr.each(function (index, value) {
                //       
                //        $amount = $(this).find("td").eq(3).text();
                //        alert($amount);
                //    });
                //    c_txt_Debit.SetValue(parseFloat($amount));
            });
        }

        function txtBillNo_TextChanged() {
            var VoucherNo = document.getElementById("txtBillNo").value;
            var type = $('#<%=hdnMode.ClientID %>').val();

            if (VoucherNo != "") {
                $("#MandatoryBillNo").hide();
            }

            $.ajax({
                type: "POST",
                url: "CustomerNote.aspx/CheckUniqueName",
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

            $("#ddl_Currency").blur(function () {
                //if (grid.GetVisibleRowsOnPage() == 1) {
                //    grid.batchEditApi.StartEdit(-1, 1);
                //}
                // here goes your code
            });

            $("#ddl_Reason").blur(function () {
                if (grid.GetVisibleRowsOnPage() == 1) {
                    grid.batchEditApi.StartEdit(-1, 1);
                }
                // here goes your code
            });
            $('#txt_Rate_I').blur(function () {
                //if (grid.GetVisibleRowsOnPage() == 1) {
                //    grid.batchEditApi.StartEdit(-1, 1);
                //}
            })

            $('#ddl_Currency').change(function () {
                var CurrencyId = $(this).val();
                var ActiveCurrency = '<%=Session["LocalCurrency"]%>'
                var Currency = ActiveCurrency.toString().split('~')[0];
                if (Currency != CurrencyId) {
                    ctxt_Rate.SetEnabled(true);
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
                    ctxt_Rate.SetEnabled(false);
                }
            });

        });

        function GetInvoiceDetails() {
            InvoiceID = "";
            var NoteType = (document.getElementById("ddlNoteType").value != null) ? document.getElementById("ddlNoteType").value : "";
            //var CustomerID = cCustomerComboBox.GetValue();//(gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex())) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";

            var CustomerID = GetObjectID('hdnCustomerId').value;
            if (CustomerID != "") {
                LoadCustomerAddress(CustomerID, $('#ddlBranch').val(), 'CN');
                if ($('#hfBSAlertFlag').val() == "1") {
                    jConfirm('Wish to View/Select Billing and Shipping details?', 'Confirmation Dialog', function (r) {
                        if (r == true) {
                            page.tabs[0].SetEnabled(true);
                        }
                    });
                }
                else {
                    page.tabs[0].SetEnabled(true);
                }
            }

            clblInvoiceNo.SetText('Ref. Sale Invoice No.');
            if (NoteType != "" && CustomerID != "") {
                cddlInvoice.PerformCallback("Dr" + '~' + CustomerID)
            }
        }


        function ddlInvoice_EndCallback(s, e) {
            if (InvoiceID != "" && InvoiceID != "0") {
                cddlInvoice.SetValue(InvoiceID);
            }

            if (cddlInvoice.cpGSTN != null && cddlInvoice.cpGSTN != undefined) {
                $("#<%=divGSTIN.ClientID%>").attr('style', 'display:block');
                document.getElementById('<%=lblGSTIN.ClientID %>').innerHTML = cddlInvoice.cpGSTN;
                cddlInvoice.cpGSTN = null;
            }
        }

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
            else if (event.keyCode == 79 && event.altKey == true) {
                //run code for Alt+X -- ie, Billing/Shipping Ok button! 
                StopDefaultAction(e);
                if (page.GetActiveTabIndex() == 1) {
                    fnSaveBillingShipping();
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

        function ReloadPage() {
            InvoiceID = "";
            $('#<%=hdnNotelNo.ClientID %>').val('');
            $('#<%=hfPageLoadFlag.ClientID %>').val('');
            window.location.reload();
        }

        function CloseGridLookup() {

            //cCustomerComboBox.Focus();
            ctxtCustName.Focus();
        }
    </script>
    <style>
        /*.dxgv {
            display: none;
        }*/
        .invisible {
            disabled: disabled;
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
            right: 8px;
            top: 35px;
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
                var url = '/OMS/management/Master/frm_BranchUdfPopUp.aspx?Type=CNOTE&&KeyVal_InternalID=' + keyVal;
                popup.SetContentUrl(url);
                popup.Show();
            }
            return true;
        }
        // End Udf Code
    </script>
    <%--Tax script Start--%>
    <script type="text/javascript">
        function ctaxUpdatePanelEndCall(s, e) {
            if (ctaxUpdatePanel.cpstock != null) {
                divAvailableStk.style.display = "block";
                divpopupAvailableStock.style.display = "block";
                ctaxUpdatePanel.cpstock = null;
                grid.batchEditApi.StartEdit(globalRowIndex, 5);
                return false;
            }
        }
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

            //ctxtTaxTotAmt.SetValue(Math.round(totalInlineTaxAmount));
            ctxtTaxTotAmt.SetValue(totalInlineTaxAmount);
        }

        function PopulateTaxWiseLedgerAmount() {
            $(function () {

                var $tr = $("#aspxGridTax_DXMainTable > tbody > tr:gt(1)");

                $tr.each(function (index, value) {


                    var $amount = $(this).find("td").eq(4).text();
                    alert($amount);
                    var mainAccountValue = grid.batchEditApi.GetCellValue(0, "CountryID");
                    var lastRow = grid.GetVisibleRowsOnPage() - 1;
                    //  alert('6');
                    grid.AddNewRow();
                    grid.batchEditApi.SetCellValue(lastRow, 'CountryID', mainAccountValue);
                    grid.batchEditApi.SetCellValue(lastRow, 'WithDrawl', $amount);


                });


            });
        }

        function ShowTaxPopUp(type) {
            if (type == "IY") {
                $('#ContentErrorMsg').hide();
                $('#content-6').show();
                $('#btnTaxpopup').show();

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
                    $('#btnTaxpopup').hide();
                    $('#content-6').hide();
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



        function OnBatchEditEndEditing(s, e) {
            //var ProductIDColumn = s.GetColumnByField("ProductID");
            //if (!e.rowValues.hasOwnProperty(ProductIDColumn.index))
            //    return;
            //var cellInfo = e.rowValues[ProductIDColumn.index];
            //if (cCmbProduct.GetSelectedIndex() > -1 || cellInfo.text != cCmbProduct.GetText()) {
            //    cellInfo.value = cCmbProduct.GetValue();
            //    cellInfo.text = cCmbProduct.GetText();
            //    cCmbProduct.SetValue(null);
            //}
        }

        function TaxAmountKeyDown(s, e) {

            if (e.htmlEvent.key == "Enter") {
                s.OnButtonClick(0);
            }
        }

        var taxAmountGlobal;
        function taxAmountGotFocus(s, e) {
            taxAmountGlobal = parseFloat(s.GetValue());
        }
        function taxAmountLostFocus(s, e) {
            var finalTaxAmt = parseFloat(s.GetValue());
            var totAmt = parseFloat(ctxtTaxTotAmt.GetText());
            var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
            var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
            if (sign == '(+)') {
                //ctxtTaxTotAmt.SetValue(Math.round(totAmt + finalTaxAmt - taxAmountGlobal));
                ctxtTaxTotAmt.SetValue(totAmt + finalTaxAmt - taxAmountGlobal);
            } else {
                //ctxtTaxTotAmt.SetValue(Math.round(totAmt + (finalTaxAmt * -1) - (taxAmountGlobal * -1)));
                ctxtTaxTotAmt.SetValue(totAmt + (finalTaxAmt * -1) - (taxAmountGlobal * -1));
            }


            SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''));
            //Set Running Total
            SetRunningTotal();

            RecalCulateTaxTotalAmountInline();
        }

        function cmbGstCstVatChange(s, e) {

            SetOtherTaxValueOnRespectiveRow(0, 0, gstcstvatGlobalName);
            $('.RecalculateInline').hide();
            var ProdAmt = parseFloat(ctxtprodBasicAmt.GetValue());
            if (s.GetValue().split('~')[2] == 'G') {
                ProdAmt = parseFloat(clblTaxProdGrossAmt.GetValue());
            }
            else if (s.GetValue().split('~')[2] == 'N') {
                ProdAmt = parseFloat(clblProdNetAmt.GetValue());
            }
            else if (s.GetValue().split('~')[2] == 'O') {
                //Check for Other Dependecy
                $('.RecalculateInline').show();
                ProdAmt = 0;
                var taxdependentName = s.GetValue().split('~')[3];
                for (var i = 0; i < taxJson.length; i++) {
                    cgridTax.batchEditApi.StartEdit(i, 3);
                    var gridTaxName = cgridTax.GetEditor("Taxes_Name").GetText();
                    gridTaxName = gridTaxName.substring(0, gridTaxName.length - 3).trim();
                    if (gridTaxName == taxdependentName) {
                        ProdAmt = cgridTax.GetEditor("Amount").GetValue();
                    }
                }
            }
            else if (s.GetValue().split('~')[2] == 'R') {
                ProdAmt = GetTotalRunningAmount();
                $('.RecalculateInline').show();
            }

            GlobalCurTaxAmt = parseFloat(ctxtGstCstVat.GetText());

            var calculatedValue = parseFloat(ProdAmt * ccmbGstCstVat.GetValue().split('~')[1]) / 100;
            ctxtGstCstVat.SetValue(calculatedValue);

            var totAmt = parseFloat(ctxtTaxTotAmt.GetText());
            //ctxtTaxTotAmt.SetValue(Math.round(totAmt + calculatedValue - GlobalCurTaxAmt));
            ctxtTaxTotAmt.SetValue(totAmt + calculatedValue - GlobalCurTaxAmt);

            //tax others
            SetOtherTaxValueOnRespectiveRow(0, calculatedValue, ccmbGstCstVat.GetText());
            gstcstvatGlobalName = ccmbGstCstVat.GetText();
        }


        //for tax and charges
        function Onddl_VatGstCstEndCallback(s, e) {
            if (s.GetItemCount() == 1) {
                cddlVatGstCst.SetEnabled(false);
            }
        }
        var GlobalCurChargeTaxAmt;
        var ChargegstcstvatGlobalName;
        function ChargecmbGstCstVatChange(s, e) {

            SetOtherChargeTaxValueOnRespectiveRow(0, 0, ChargegstcstvatGlobalName);
            $('.RecalculateCharge').hide();
            var ProdAmt = parseFloat(ctxtProductAmount.GetValue());

            //Set ProductAmount
            if (s.GetValue().split('~')[2] == 'G') {
                ProdAmt = parseFloat(ctxtProductAmount.GetValue());
            }
            else if (s.GetValue().split('~')[2] == 'N') {
                ProdAmt = parseFloat(clblProdNetAmt.GetValue());
            }
            else if (s.GetValue().split('~')[2] == 'O') {
                //Check for Other Dependecy
                $('.RecalculateCharge').show();
                ProdAmt = 0;
                var taxdependentName = s.GetValue().split('~')[3];
                for (var i = 0; i < taxJson.length; i++) {
                    gridTax.batchEditApi.StartEdit(i, 3);
                    var gridTaxName = gridTax.GetEditor("TaxName").GetText();
                    gridTaxName = gridTaxName.substring(0, gridTaxName.length - 3).trim();
                    if (gridTaxName == taxdependentName) {
                        ProdAmt = gridTax.GetEditor("Amount").GetValue();
                    }
                }
            }
            else if (s.GetValue().split('~')[2] == 'R') {
                $('.RecalculateCharge').show();
                ProdAmt = GetChargesTotalRunningAmount();
            }

            GlobalCurChargeTaxAmt = parseFloat(ctxtGstCstVatCharge.GetText());

            var calculatedValue = parseFloat(ProdAmt * ccmbGstCstVatcharge.GetValue().split('~')[1]) / 100;
            ctxtGstCstVatCharge.SetValue(calculatedValue);

            var totAmt = parseFloat(ctxtQuoteTaxTotalAmt.GetText());
            ctxtQuoteTaxTotalAmt.SetValue(totAmt + calculatedValue - GlobalCurChargeTaxAmt);

            //tax others
            SetOtherChargeTaxValueOnRespectiveRow(0, calculatedValue, ctxtGstCstVatCharge.GetText());
            ChargegstcstvatGlobalName = ctxtGstCstVatCharge.GetText();

            //set Total Amount
            ctxtTotalAmount.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + parseFloat(ctxtProductNetAmount.GetValue()));
        }
        function GetChargesTotalRunningAmount() {
            var runningTot = parseFloat(ctxtProductNetAmount.GetValue());
            for (var i = 0; i < chargejsonTax.length; i++) {
                gridTax.batchEditApi.StartEdit(i, 3);
                runningTot = runningTot + parseFloat(gridTax.GetEditor("Amount").GetValue());
                gridTax.batchEditApi.EndEdit();
            }

            return runningTot;
        }

        function chargeCmbtaxClick(s, e) {
            GlobalCurChargeTaxAmt = parseFloat(ctxtGstCstVatCharge.GetText());
            ChargegstcstvatGlobalName = s.GetText();
        }

        var GlobalCurTaxAmt = 0;
        var rowEditCtrl;
        var globalRowIndex;
        var globalTaxRowIndex;
        function GetVisibleIndex(s, e) {
            globalRowIndex = e.visibleIndex;
        }
        function GetTaxVisibleIndex(s, e) {

            globalTaxRowIndex = e.visibleIndex;
        }
        function cmbtaxCodeindexChange(s, e) {
            if (cgridTax.GetEditor("Taxes_Name").GetText() == "GST/CST/VAT") {

                var taxValue = s.GetValue();

                if (taxValue == null) {
                    taxValue = 0;
                    GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                    cgridTax.GetEditor("Amount").SetValue(0);
                    //ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) - GlobalCurTaxAmt));
                    ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) - GlobalCurTaxAmt);
                }


                var isValid = taxValue.indexOf('~');
                if (isValid != -1) {
                    var rate = parseFloat(taxValue.split('~')[1]);
                    var ProdAmt = parseFloat(ctxtprodBasicAmt.GetValue());

                    GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());


                    cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * rate) / 100);
                    //ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * rate) / 100) - GlobalCurTaxAmt));
                    ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * rate) / 100) - GlobalCurTaxAmt);
                    GlobalCurTaxAmt = 0;
                }
                else {
                    s.SetText("");
                }

            } else {
                var ProdAmt = parseFloat(ctxtprodBasicAmt.GetValue());

                if (s.GetValue() == null) {
                    s.SetValue(0);
                }

                if (!isNaN(parseFloat(ProdAmt * s.GetText()) / 100)) {

                    GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                    cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * s.GetText()) / 100);

                    //ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt));
                    ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt);
                    GlobalCurTaxAmt = 0;
                } else {
                    s.SetText("");
                }
            }

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

                        //ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt));
                        ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt);
                        GlobalCurTaxAmt = 0;
                    }
                    else {

                        GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                        cgridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * s.GetText()) / 100) * -1);

                        //ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) * -1) - (GlobalCurTaxAmt * -1)));
                        ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) * -1) - (GlobalCurTaxAmt * -1));
                        GlobalCurTaxAmt = 0;
                    }




                }
            }
            //return;
            cgridTax.batchEditApi.EndEdit();

        }
        function SetOtherChargeTaxValueOnRespectiveRow(idx, amt, name) {
            name = name.substring(0, name.length - 3).trim();
            for (var i = 0; i < chargejsonTax.length; i++) {
                if (chargejsonTax[i].applicableBy == name) {
                    gridTax.batchEditApi.StartEdit(i, 3);
                    gridTax.GetEditor('calCulatedOn').SetValue(amt);

                    var totLength = gridTax.GetEditor("TaxName").GetText().length;
                    var taxNameWithSign = gridTax.GetEditor("Percentage").GetText();
                    var sign = gridTax.GetEditor("TaxName").GetText().substring(totLength - 3);
                    var ProdAmt = parseFloat(gridTax.GetEditor("calCulatedOn").GetValue());
                    var s = gridTax.GetEditor("Percentage");
                    if (sign == '(+)') {
                        GlobalCurTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
                        gridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * s.GetText()) / 100);

                        //ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt));
                        ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt);
                        GlobalCurTaxAmt = 0;
                    }
                    else {

                        GlobalCurTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
                        gridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * s.GetText()) / 100) * -1);

                        //ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) * -1) - (GlobalCurTaxAmt * -1)));
                        ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) * -1) - (GlobalCurTaxAmt * -1));
                        GlobalCurTaxAmt = 0;
                    }




                }
            }
            //return;
            gridTax.batchEditApi.EndEdit();
        }
        function txtPercentageLostFocus(s, e) {

            //var ProdAmt = parseFloat(ctxtprodBasicAmt.GetValue());
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

                        //ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt));
                        ctxtTaxTotAmt.SetValue((parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt));
                        GlobalCurTaxAmt = 0;
                    }
                    else {

                        GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                        cgridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * s.GetText()) / 100) * -1);

                        //ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) * -1) - (GlobalCurTaxAmt * -1)));
                        ctxtTaxTotAmt.SetValue((parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) * -1) - (GlobalCurTaxAmt * -1)));
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

                        //ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(cgridTax.GetEditor("TaxField").GetValue())) / 100) - GlobalCurTaxAmt));
                        ctxtTaxTotAmt.SetValue((parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(cgridTax.GetEditor("TaxField").GetValue())) / 100) - GlobalCurTaxAmt));
                        GlobalCurTaxAmt = 0;
                    }
                    else {

                        GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                        cgridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * cgridTax.GetEditor("TaxField").GetValue()) / 100) * -1);

                        //ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(cgridTax.GetEditor("TaxField").GetValue())) / 100) * -1) - (GlobalCurTaxAmt * -1)));
                        ctxtTaxTotAmt.SetValue((parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(cgridTax.GetEditor("TaxField").GetValue())) / 100) * -1) - (GlobalCurTaxAmt * -1)));
                        GlobalCurTaxAmt = 0;
                    }
                    SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''));
                }
                runningTot = runningTot + parseFloat(cgridTax.GetEditor("Amount").GetValue());
                cgridTax.batchEditApi.EndEdit();
            }
        }

        function GetTotalRunningAmount() {
            var runningTot = parseFloat(clblProdNetAmt.GetValue());
            for (var i = 0; i < taxJson.length; i++) {
                cgridTax.batchEditApi.StartEdit(i, 3);
                runningTot = runningTot + parseFloat(cgridTax.GetEditor("Amount").GetValue());
                cgridTax.batchEditApi.EndEdit();
            }

            return runningTot;
        }



        var gstcstvatGlobalName;
        function CmbtaxClick(s, e) {
            GlobalCurTaxAmt = parseFloat(ctxtGstCstVat.GetText());
            gstcstvatGlobalName = s.GetText();
        }


        function txtTax_TextChanged(s, i, e) {
            cgridTax.batchEditApi.StartEdit(i, 2);
            var ProdAmt = parseFloat(ctxtprodBasicAmt.GetValue());
            cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * s.GetText()) / 100);
        }

        function taxAmtButnClick(s, e) {

            //var CountryID = (grid.GetEditor('CountryID').GetText() != null) ? grid.GetEditor('CountryID').GetText() : "0";
            var CountryID = (grid.GetEditor('gvColMainAccount').GetText() != null) ? grid.GetEditor('gvColMainAccount').GetText() : "0";
            CountryID = (CountryID == "" || CountryID == null) ? ($("#hfLedgerName").val()) : CountryID;

            if (CountryID.trim() != "") {

                //var ledgerId = (grid.GetEditor('CountryID').GetValue() != null) ? grid.GetEditor('CountryID').GetValue() : "0";
                var ledgerId = (grid.GetEditor('gvColMainAccount').GetValue() != null) ? grid.GetEditor('gvColMainAccount').GetValue() : "0";
                ledgerId = (ledgerId == "0" || ledgerId == null) ? ($("#hfLedgerID").val()) : ledgerId;

                document.getElementById('HDmainAccount_id').value = ledgerId;

                var cashReportID = (grid.GetEditor('CashReportID').GetValue() != null) ? grid.GetEditor('CashReportID').GetValue() : "0";
                cashReportID = (cashReportID == "0" || cashReportID == null) ? ($("#hfCashReportID").val()) : cashReportID;

                document.getElementById('HDParentSlNo').value = cashReportID;

                ctxtTaxTotAmt.SetValue(0);
                ccmbGstCstVat.SetSelectedIndex(0);
                $('.RecalculateInline').hide();
                caspxTaxpopUp.Show();
                var strRate = (ctxt_Rate.GetValue() != null && ctxt_Rate.GetValue() != "0") ? ctxt_Rate.GetValue() : "1";
                if (strRate == 0) {
                    strRate = 1;
                }

                var Amount = (grid.GetEditor('WithDrawl').GetText() != null) ? grid.GetEditor('WithDrawl').GetText() : "0";
                Amount = (Amount == "" || Amount == null || Amount == "0.00") ? ($("#hfWithDrawlAmount").val()) : Amount;
                clblTaxProdGrossAmt.SetText(Amount);
                clblProdNetAmt.SetText(Amount);
                document.getElementById('HdProdGrossAmt').value = Amount;
                document.getElementById('HdProdNetAmt').value = Amount;
                clblTaxDiscount.SetText('0.00');
                $('.GstCstvatClass').show();
                $('.gstGrossAmount').hide();
                $('.gstNetAmount').hide();
                clblTaxableGross.SetText("");
                clblTaxableNet.SetText("");

                //Get Customer Shipping StateCode
                var shippingStCode = '';
                shippingStCode = cbsSCmbState.GetText();
                shippingStCode = shippingStCode.substring(shippingStCode.lastIndexOf('(')).replace('(State Code:', '').replace(')', '').trim();

                //Debjyoti 09032017
                if (shippingStCode.trim() != '') {
                    for (var cmbCount = 1; cmbCount < ccmbGstCstVat.GetItemCount() ; cmbCount++) {
                        //Check if gstin is blank then delete all tax
                        if (ccmbGstCstVat.GetItem(cmbCount).value.split('~')[5] != "") {

                            if (ccmbGstCstVat.GetItem(cmbCount).value.split('~')[5] == shippingStCode) {

                                //if its state is union territories then only UTGST will apply
                                if (shippingStCode == "4" || shippingStCode == "26" || shippingStCode == "25" || shippingStCode == "35" || shippingStCode == "31" || shippingStCode == "34") {
                                    if (ccmbGstCstVat.GetItem(cmbCount).value.split('~')[4] == 'I' || ccmbGstCstVat.GetItem(cmbCount).value.split('~')[4] == 'S') {
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

                if (globalRowIndex > -1) {
                    cgridTax.PerformCallback(grid.keys[globalRowIndex] + '~1');
                } else {
                    cgridTax.PerformCallback('New~1');
                }
                ctxtprodBasicAmt.SetValue(grid.GetEditor('WithDrawl').GetValue());
            } else {
                grid.batchEditApi.StartEdit(globalRowIndex, 1);
            }

        }
        function taxAmtButnClick1(s, e) {
            //console.log(grid.GetFocusedRowIndex());
            rowEditCtrl = s;
        }

        function BatchUpdate() {

            //cgridTax.batchEditApi.StartEdit(0, 1);

            //if (cgridTax.GetEditor("TaxField").GetText().indexOf('.') == -1) {
            //    cgridTax.GetEditor("TaxField").SetText(cgridTax.GetEditor("TaxField").GetText().trim() + '.00');
            //} else {
            //    cgridTax.GetEditor("TaxField").SetText(cgridTax.GetEditor("TaxField").GetText().trim() + '0');
            //}
            if (cgridTax.GetVisibleRowsOnPage() > 0) {
                cgridTax.UpdateEdit();
            }
            else {
                cgridTax.PerformCallback('SaveGST');
            }
            return false;
        }

        var taxJson;
        function cgridTax_EndCallBack(s, e) {

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
                ctxtTaxTotAmt.SetValue((cgridTax.cpUpdated.split('~')[1]));
                var gridValue = parseFloat(cgridTax.cpUpdated.split('~')[1]);
                ctxtTaxTotAmt.SetValue(gridValue);
                cgridTax.cpUpdated = "";
            }
            else {
                var totAmt = ctxtTaxTotAmt.GetValue();
                cgridTax.CancelEdit();
                caspxTaxpopUp.Hide();
            }
            if (cgridTax.GetVisibleRowsOnPage() == 0) {
                $('.cgridTaxClass').hide();
                ccmbGstCstVat.Focus();
            }
            //Debjyoti Check where any Gst Present or not
            // If Not then hide the hole section
            SetRunningTotal();
            ShowTaxPopUp("IY");
            grid.PerformCallback('AgainDisplay');
        }

        function recalculateTax() {
            cmbGstCstVatChange(ccmbGstCstVat);
        }
        function recalculateTaxCharge() {
            ChargecmbGstCstVatChange(ccmbGstCstVatcharge);
        }
    </script>
    <%--Tax script End--%>
    <%--Added By : Samrat Roy -- New Billing/Shipping Section--%>
    <script>
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
            $("#shippingcustomer").css("display", "none");
            if (name == "tab0") {
                // gridLookup.Focus();
                //cCustomerComboBox.Focus();
                ctxtCustName.Focus();
            }
            if (name == "tab1") {
            }
        }

        function cexUpdatePanel_EndCallBack(s, e) {
            $("#hfFocusSet").val("NT"); /// NT refers NoteType          
            grid.Refresh();
        }

        function BtnVisible(tagDocNo) {
            if (tagDocNo != '') {
                document.getElementById('btnSaveRecords').style.display = 'none';
                document.getElementById('btn_SaveRecords').style.display = 'none';
                $("#spanTaggedDocNo").html(tagDocNo);
                document.getElementById('tagged').style.display = 'block';

            }
        }
    </script>
    <%--Added By : Samrat Roy -- New Billing/Shipping Section End--%>
    <style>
        #DivBilling [class^="col-md"], #DivShipping [class^="col-md"] {
            padding-top: 5px;
            padding-bottom: 5px;
        }

        .dxtcSys.dxtc-init > .dxtc-stripContainer {
            visibility: visible;
        }

        #aspxGridTax_DXStatus, #grid_DXStatus {
            display: none;
        }

        .padTabtype2 > tbody > tr > td {
            padding: 0px 5px;
        }

            .padTabtype2 > tbody > tr > td > label {
                margin-bottom: 0 !important;
                margin-right: 15px;
            }
    </style>
    <script>
        var isFirstTime = true;
        function AllControlInitilize() {
            ///  document.getElementById('AddButton').style.display = 'inline-block';
            if (isFirstTime) {

                if (localStorage.getItem('FromDateCustomerCrDrNote')) {
                    var fromdatearray = localStorage.getItem('FromDateCustomerCrDrNote').split('-');
                    var fromdate = new Date(fromdatearray[0], parseFloat(fromdatearray[1]) - 1, fromdatearray[2], 0, 0, 0, 0);
                    //cFormDate.SetDate(fromdate);
                }

                if (localStorage.getItem('ToDateCustomerCrDrNote')) {
                    var todatearray = localStorage.getItem('ToDateCustomerCrDrNote').split('-');
                    var todate = new Date(todatearray[0], parseFloat(todatearray[1]) - 1, todatearray[2], 0, 0, 0, 0);
                    //ctoDate.SetDate(todate);
                }
                if (localStorage.getItem('BrancheCustomerCrDrNote')) {
                    if (ccmbBranchfilter.FindItemByValue(localStorage.getItem('BrancheCustomerCrDrNote'))) {
                        ccmbBranchfilter.SetValue(localStorage.getItem('BrancheCustomerCrDrNote'));
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
                localStorage.setItem("FromDateCustomerCrDrNote", cFormDate.GetDate().format('yyyy-MM-dd'));
                localStorage.setItem("ToDateCustomerCrDrNote", ctoDate.GetDate().format('yyyy-MM-dd'));
                localStorage.setItem("BrancheCustomerCrDrNote", ccmbBranchfilter.GetValue());

                $("#hfFromDate").val(cFormDate.GetDate().format('yyyy-MM-dd'));
                $("#hfToDate").val(ctoDate.GetDate().format('yyyy-MM-dd'));
                $("#hfBranchID").val(ccmbBranchfilter.GetValue());
                $("#hfIsFilter").val("Y");

                cGvJvSearch.Refresh();

                //cGvJvSearch.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue());
            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title clearfix">
            <h3 class="clearfix pull-left">
                <asp:Label ID="lblHeading" runat="server" Text="Customer Debit/Credit Note"></asp:Label>
            </h3>
            <div id="pageheaderContent" class="pull-right reverse wrapHolder content horizontal-images">
                <div class="Top clearfix">
                    <ul>
                        <li>
                            <div class="lblHolder" id="divGSTIN" style="display: none;" runat="server">
                                <table>
                                    <tr>
                                        <td>GST Registed?</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblGSTIN" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
            <div id="divcross" class="crossBtn" runat="server" style="display: none; margin-left: 50px;"><a href="javascript:ReloadPage();cexUpdatePanel.PerformCallback('SessClear');"><i class="fa fa-times"></i></a></div>

        </div>
    </div>
    <div class="form_main">
        <div id="TblSearch" class="clearfix">
            <div class="clearfix">
                <div style="padding-right: 5px;">
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

                    <table class="padTabtype2 pull-right" id="gridFilter">
                        <tr>
                            <td>
                                <label>From Date</label></td>
                            <td>
                                <dxe:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDate" Width="100%">
                                    <ButtonStyle Width="13px">
                                    </ButtonStyle>
                                </dxe:ASPxDateEdit>
                            </td>
                            <td>
                                <label>To Date</label>
                            </td>
                            <td>
                                <dxe:ASPxDateEdit ID="toDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDate" Width="100%">
                                    <ButtonStyle Width="13px">
                                    </ButtonStyle>
                                </dxe:ASPxDateEdit>
                            </td>
                            <td>Unit</td>
                            <td>
                                <dxe:ASPxComboBox ID="cmbBranchfilter" runat="server" ClientInstanceName="ccmbBranchfilter" Width="100%">
                                </dxe:ASPxComboBox>
                            </td>
                            <td>
                                <input type="button" value="Show" class="btn btn-primary" onclick="updateGridByDate()" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div class="clearfix">
                <dxe:ASPxGridView ID="GvJvSearch" runat="server" AutoGenerateColumns="False" SettingsBehavior-AllowSort="true"
                    ClientInstanceName="cGvJvSearch" KeyFieldName="DCNote_ID" Width="100%" Settings-HorizontalScrollBarMode="Auto" DataSourceID="EntityServerModeDataSource" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false"
                    OnCustomCallback="GvJvSearch_CustomCallback" OnCustomButtonInitialize="GvJvSearch_CustomButtonInitialize"
                    OnSummaryDisplayText="ShowGrid_SummaryDisplayText">
                    <SettingsSearchPanel Visible="true" Delay="5000" />
                    <ClientSideEvents CustomButtonClick="CustomButtonClick" EndCallback="function(s, e) {GvJvSearch_EndCallBack();}" />
                    <SettingsBehavior ConfirmDelete="True" />
                    <Styles>
                        <Header SortingImageSpacing="5px" ImageSpacing="5px"></Header>
                        <FocusedRow HorizontalAlign="Left" VerticalAlign="Top" CssClass="gridselectrow"></FocusedRow>
                        <LoadingPanel ImageSpacing="10px"></LoadingPanel>
                        <FocusedGroupRow CssClass="gridselectrow"></FocusedGroupRow>
                        <Footer CssClass="gridfooter"></Footer>
                    </Styles>
                    <SettingsPager NumericButtonCount="10" PageSize="10" ShowSeparators="True" AlwaysShowPager="True">
                        <FirstPageButton Visible="True">
                        </FirstPageButton>
                        <LastPageButton Visible="True">
                        </LastPageButton>
                    </SettingsPager>

                    <Columns>


                        <%-- <dxe:GridViewDataTextColumn VisibleIndex="0" FieldName="NoteType" Caption="Type">
                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        </dxe:GridViewDataTextColumn>--%>
                        <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="0" FieldName="DCNote_ID" Caption="DCNote_ID" SortOrder="Descending">
                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataComboBoxColumn Caption="Type" FieldName="NoteType" VisibleIndex="0">
                            <PropertiesComboBox EnableSynchronization="False" EnableIncrementalFiltering="True"
                                ValueType="System.String" DataSourceID="SqlDataSourceapplicable" TextField="TypeName" ValueField="TypeID">
                            </PropertiesComboBox>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataComboBoxColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="TransDate" Caption="Posting Date">
                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="NoteNumber" Caption="Document Number" Width="150px">
                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="BranchName" Caption="Unit">
                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="Currency" Caption="Currency" Settings-AllowAutoFilter="False">
                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="ImportType" Caption="Type">
                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="CustomerName" Caption="Customer Name" Width="180px">
                            <CellStyle CssClass="gridcellleft"></CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="Amount" Caption="Amount">
                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="Total_CGST" Caption="CGST">
                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="8" FieldName="Total_SGST" Caption="SGST">
                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="9" FieldName="Total_UTGST" Caption="UTGST">
                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="10" FieldName="Total_IGST" Caption="IGST">
                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="11" FieldName="Total_taxable_amount" Caption="Taxable Amount">
                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="12" FieldName="EnteredBy" Caption="Entered On" Settings-AllowAutoFilter="False">
                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="13" FieldName="UpdateOn" Caption="Last Update On" Width="130px" Settings-AllowAutoFilter="False">
                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                            <PropertiesTextEdit DisplayFormatString="dd/MM/yyyy hh:mm:ss"></PropertiesTextEdit>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="14" FieldName="UpdatedBy" Caption="Updated By" Settings-AllowAutoFilter="False">
                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Visible="False" FieldName="DCNote_ID"></dxe:GridViewDataTextColumn>
                        <dxe:GridViewCommandColumn VisibleIndex="15" Width="130px" ButtonType="Image" Caption="Actions" HeaderStyle-HorizontalAlign="Center">
                            <CustomButtons>
                                <dxe:GridViewCommandColumnCustomButton ID="CustomBtnView" meta:resourcekey="GridViewCommandColumnCustomButtonResource1" Image-ToolTip="View" Styles-Style-CssClass="pad">
                                    <Image Url="/assests/images/doc.png"></Image>
                                </dxe:GridViewCommandColumnCustomButton>
                                <dxe:GridViewCommandColumnCustomButton ID="CustomBtnEdit" meta:resourcekey="GridViewCommandColumnCustomButtonResource1" Image-ToolTip="Edit" Styles-Style-CssClass="pad">
                                    <Image Url="/assests/images/Edit.png"></Image>
                                </dxe:GridViewCommandColumnCustomButton>
                                <dxe:GridViewCommandColumnCustomButton ID="CustomBtnDelete" meta:resourcekey="GridViewCommandColumnCustomButtonResource2" Image-ToolTip="Delete" Styles-Style-CssClass="pad">
                                    <Image Url="/assests/images/Delete.png"></Image>
                                </dxe:GridViewCommandColumnCustomButton>

                                <dxe:GridViewCommandColumnCustomButton ID="CustomBtnPrint" meta:resourcekey="GridViewCommandColumnCustomButtonResource3" Image-ToolTip="Print" Styles-Style-CssClass="pad">
                                    <Image Url="/assests/images/Print.png"></Image>

                                </dxe:GridViewCommandColumnCustomButton>
                                <%--Print Customer Debit/Credit Note--%>
                                <%--<dxe:GridViewCommandColumnCustomButton ID="CustomBtnPrint" meta:resourcekey="GridViewCommandColumnCustomButtonResource3" Image-ToolTip="Print" Styles-Style-CssClass="pad">
                                    <Image Url="/assests/images/Print.png"></Image>
                                </dxe:GridViewCommandColumnCustomButton>--%>
                                <%--End Print Customer Debit/Credit Note--%>
                            </CustomButtons>

                        </dxe:GridViewCommandColumn>
                    </Columns>
                    <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                    <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" HorizontalScrollBarMode="Visible" ShowFooter="true" />
                    <SettingsPager NumericButtonCount="10" PageSize="10" ShowSeparators="True" Mode="ShowPager">
                        <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                        <FirstPageButton Visible="True">
                        </FirstPageButton>
                        <LastPageButton Visible="True">
                        </LastPageButton>
                    </SettingsPager>
                    <TotalSummary>
                        <dxe:ASPxSummaryItem FieldName="Amount" SummaryType="Sum" />
                        <dxe:ASPxSummaryItem FieldName="Total_CGST" SummaryType="Sum" />
                        <dxe:ASPxSummaryItem FieldName="Total_SGST" SummaryType="Sum" />
                        <dxe:ASPxSummaryItem FieldName="Total_UTGST" SummaryType="Sum" />
                        <dxe:ASPxSummaryItem FieldName="Total_IGST" SummaryType="Sum" />
                        <dxe:ASPxSummaryItem FieldName="Total_taxable_amount" SummaryType="Sum" />
                    </TotalSummary>
                </dxe:ASPxGridView>
                <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
                    ContextTypeName="ERPDataClassesDataContext" TableName="v_CustomerNoteList" />
            </div>
        </div>
        <div id="divAddNew" class="clearfix" style="display: none;">
            <%--style="display: none;"--%>

            <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ClientInstanceName="page" Width="100%">
                <TabPages>
                    <dxe:TabPage Name="General" Text="General">
                        <ContentCollection>
                            <dxe:ContentControl runat="server">
                                <div style="background: #f5f4f3; padding: 8px 0; margin-bottom: 0px; border-radius: 4px; border: 1px solid #ccc;" class="clearfix">
                                    <div class="col-md-3">
                                        <label>Note Type</label>
                                        <div>
                                            <asp:DropDownList ID="ddlNoteType" runat="server" Width="100%" onchange="ddlNoteType_ValueChange()">
                                                <asp:ListItem Text="Debit Note" Value="Dr" />
                                                <asp:ListItem Text="Credit Note" Value="Cr" />
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-9">
                                        <div class="row">
                                            <div class="col-md-3" id="div_Edit">
                                                <label>Select Numbering Scheme</label>
                                                <div>
                                                    <asp:DropDownList ID="CmbScheme" runat="server" DataSourceID="SqlSchematype"
                                                        DataTextField="SchemaName" DataValueField="ID" Width="100%"
                                                        onchange="CmbScheme_ValueChange()">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>Document No.</label>
                                                <div>
                                                    <asp:TextBox ID="txtBillNo" runat="server" Width="95%" meta:resourcekey="txtBillNoResource1" MaxLength="30" onchange="txtBillNo_TextChanged()"></asp:TextBox>
                                                    <span id="MandatoryBillNo" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                                    <span id="duplicateMandatoryBillNo" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Duplicate Journal No."></span>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label style="">Posting Date</label>
                                                <div>
                                                    <dxe:ASPxDateEdit ID="tDate" runat="server" EditFormat="Custom" ClientInstanceName="tDate"
                                                        UseMaskBehavior="True" Width="100%" meta:resourcekey="tDateResource1">
                                                        <ClientSideEvents DateChanged="function(s,e){DateChange()}" />
                                                    </dxe:ASPxDateEdit>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>Unit</label>
                                                <div>
                                                    <asp:DropDownList ID="ddlBranch" runat="server" DataSourceID="dsBranch" Enabled="false"
                                                        DataTextField="BANKBRANCH_NAME" DataValueField="BANKBRANCH_ID" Width="100%"
                                                        meta:resourcekey="ddlBranchResource1" onfocus="BranchGotFocus()" onchange="ddlBranch_ChangeIndex()">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div style="clear: both;"></div>
                                    <div class="col-md-3">
                                        <dxe:ASPxLabel ID="lbl_Customer" runat="server" Text="Customer">
                                        </dxe:ASPxLabel>
                                        <%-- <i id="openlink" class="fa fa-plus-circle" aria-hidden="true"></i>--%>
                                        <%--<dxe:ASPxGridLookup ID="lookup_Customer" runat="server" ClientInstanceName="gridLookup" DataSourceID="dsCustomer"
                                            KeyFieldName="cnt_internalid" Width="100%" TextFormatString="{1}" AutoGenerateColumns="False">
                                            <Columns>
                                                <dxe:GridViewDataColumn FieldName="shortname" Visible="true" VisibleIndex="0" Caption="Unique ID" Width="200px" Settings-AutoFilterCondition="Contains" />
                                                <dxe:GridViewDataColumn FieldName="Name" Visible="true" VisibleIndex="1" Caption="Name" Settings-AutoFilterCondition="Contains" Width="200px">
                                                </dxe:GridViewDataColumn>
                                                <dxe:GridViewDataColumn FieldName="Type" Visible="true" VisibleIndex="2" Caption="Type" Settings-AutoFilterCondition="Contains" Width="200px">
                                                </dxe:GridViewDataColumn>
                                                <dxe:GridViewDataColumn FieldName="cnt_internalid" Visible="false" VisibleIndex="3" Settings-AllowAutoFilter="False" Width="200px">
                                                    <Settings AllowAutoFilter="False"></Settings>
                                                </dxe:GridViewDataColumn>
                                            </Columns>
                                            <GridViewProperties Settings-VerticalScrollBarMode="Auto">
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
                                            </GridViewProperties>
                                            <ClientSideEvents TextChanged="function(s, e) { GetInvoiceDetails()}" GotFocus="function(s,e){gridLookup.ShowDropDown();}" />
                                            <ClearButton DisplayMode="Auto">
                                            </ClearButton>
                                        </dxe:ASPxGridLookup>--%>
                                        <%--<dxe:ASPxComboBox ID="CustomerComboBox" runat="server" EnableCallbackMode="true" CallbackPageSize="15"
                                            ValueType="System.String" ValueField="cnt_internalid" ClientInstanceName="cCustomerComboBox" Width="92%"
                                            OnItemsRequestedByFilterCondition="ASPxComboBox_OnItemsRequestedByFilterCondition_SQL"
                                            OnItemRequestedByValue="ASPxComboBox_OnItemRequestedByValue_SQL" TextFormatString="{1} [{0}]"
                                            DropDownStyle="DropDown">
                                            <Columns>
                                                <dxe:ListBoxColumn FieldName="uniquename" Caption="Unique ID" Width="200px" />
                                                <dxe:ListBoxColumn FieldName="Name" Caption="Name" Width="200px" />                                               
                                            </Columns>
                                            <ClientSideEvents ValueChanged="function(s, e) {GetInvoiceDetails()}" GotFocus="function(s,e){cCustomerComboBox.ShowDropDown();}" />
                                        </dxe:ASPxComboBox>--%>
                                        <dxe:ASPxButtonEdit ID="txtCustName" runat="server" ReadOnly="true" ClientInstanceName="ctxtCustName" Width="100%">
                                            <Buttons>
                                                <dxe:EditButton>
                                                </dxe:EditButton>
                                            </Buttons>
                                            <ClientSideEvents ButtonClick="function(s,e){CustomerButnClick();}" KeyDown="function(s,e){Customer_KeyDown(s,e);}" />
                                        </dxe:ASPxButtonEdit>
                                        <span id="MandatorysCustomer" class="fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none; right: -3px; top: 21px;" title="Mandatory"></span>
                                    </div>
                                    <div class="col-md-9">
                                        <div class="row">
                                            <div class="col-md-3" id="div_InvoiceNo" style="display: none">
                                                <dxe:ASPxLabel ID="ASPxLabel9" runat="server" Text="Party Debit Note Number">
                                                </dxe:ASPxLabel>
                                                <dxe:ASPxTextBox ID="txtPartyInvoice" runat="server" Width="100%">
                                                </dxe:ASPxTextBox>
                                            </div>
                                            <div class="col-md-3" id="div_InvoiceDate" style="display: none">
                                                <dxe:ASPxLabel ID="ASPxLabel10" runat="server" Text="Party Debit Note Date">
                                                </dxe:ASPxLabel>
                                                <dxe:ASPxDateEdit ID="dt_PartyDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cPLPartyDate" Width="100%">
                                                    <ButtonStyle Width="13px">
                                                    </ButtonStyle>
                                                </dxe:ASPxDateEdit>
                                            </div>
                                            <div class="col-md-3">
                                                <dxe:ASPxLabel ID="lblInvoiceNo" ClientInstanceName="clblInvoiceNo" runat="server" Text="Ref. Sale Invoice No.">
                                                </dxe:ASPxLabel>
                                                <dxe:ASPxComboBox ID="ddlInvoice" runat="server" ClientInstanceName="cddlInvoice" OnCallback="ddlInvoice_Callback" Width="100%">
                                                    <ClientSideEvents EndCallback="ddlInvoice_EndCallback" />
                                                </dxe:ASPxComboBox>
                                            </div>
                                            <div class="col-md-3">
                                                <div class="row">
                                                    <div class="col-md-6 lblmTop8">
                                                        <dxe:ASPxLabel ID="lbl_Currency" runat="server" Text="Currency">
                                                        </dxe:ASPxLabel>
                                                        <asp:DropDownList ID="ddl_Currency" runat="server" Width="95%">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-6 lblmTop8">
                                                        <dxe:ASPxLabel ID="lbl_Rate" runat="server" Text="Exch. Rate">
                                                        </dxe:ASPxLabel>
                                                        <dxe:ASPxTextBox ID="txt_Rate" ClientInstanceName="ctxt_Rate" runat="server" Width="100%">
                                                            <ValidationSettings RequiredField-IsRequired="false" Display="None"></ValidationSettings>
                                                            <MaskSettings Mask="<0..999999999>.<0..9999>" AllowMouseWheel="false" />
                                                        </dxe:ASPxTextBox>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <label>Reason For Issuing document</label>
                                        <div>
                                            <asp:DropDownList ID="ddl_Reason" runat="server" DataSourceID="SqlReasonIssuedocument"
                                                DataTextField="Reason" DataValueField="ReasonID" Width="100%">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                    </div>
                                </div>
                                <div class="clearfix">
                                    <br />
                                    <dxe:ASPxGridView runat="server" OnBatchUpdate="grid_BatchUpdate" KeyFieldName="CashReportID" ClientInstanceName="grid" ID="grid"
                                        Width="100%" OnCellEditorInitialize="grid_CellEditorInitialize" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false"
                                        Settings-ShowFooter="false" OnCustomCallback="grid_CustomCallback" OnDataBinding="grid_DataBinding"
                                        OnRowInserting="Grid_RowInserting" OnRowUpdating="Grid_RowUpdating" OnRowDeleting="Grid_RowDeleting" OnHtmlRowPrepared="grid_HtmlRowPrepared"
                                        SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="150">
                                        <SettingsPager Visible="false"></SettingsPager>
                                        <Columns>
                                            <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="50" VisibleIndex="0" Caption="Action">
                                                <CustomButtons>
                                                    <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomDelete" Image-Url="/assests/images/crs.png">
                                                    </dxe:GridViewCommandColumnCustomButton>
                                                </CustomButtons>
                                            </dxe:GridViewCommandColumn>
                                            <%--  <dxe:GridViewDataComboBoxColumn Caption="Main Account" FieldName="CountryID" VisibleIndex="1" Width="300">
                                                <PropertiesComboBox ValueField="CountryID" ClientInstanceName="CountryID" TextField="CountryName" AllowMouseWheel="false">                                                   
                                                    <ClientSideEvents GotFocus="CountriesCombo_GotFocus" SelectedIndexChanged="CountriesCombo_SelectedIndexChanged" />
                                                </PropertiesComboBox>
                                            </dxe:GridViewDataComboBoxColumn>--%>
                                            <dxe:GridViewDataButtonEditColumn FieldName="CountryID" Caption="Main Account" VisibleIndex="1" Width="300">
                                                <PropertiesButtonEdit>
                                                    <ClientSideEvents ButtonClick="MainAccountButnClick" KeyDown="MainAccountKeyDown" />
                                                    <Buttons>
                                                        <dxe:EditButton Text="..." Width="20px">
                                                        </dxe:EditButton>
                                                    </Buttons>
                                                </PropertiesButtonEdit>
                                            </dxe:GridViewDataButtonEditColumn>
                                            <%--<dxe:GridViewDataComboBoxColumn FieldName="CityID" Caption="Sub Account" VisibleIndex="2" Width="300">
                                                <PropertiesComboBox TextField="CityName" ValueField="CityID">
                                                </PropertiesComboBox>
                                                <EditItemTemplate>
                                                    <dxe:ASPxComboBox runat="server" OnInit="CityCmb_Init" Width="100%" EnableIncrementalFiltering="true" TextField="CityName"
                                                        OnCallback="CityCmb_Callback" ValueField="CityID" ID="CityCmb" ClientInstanceName="CityID" EnableCallbackMode="true" AllowMouseWheel="false">
                                                        <ClientSideEvents EndCallback="CitiesCombo_EndCallback" />
                                                        <ValidationSettings RequiredField-IsRequired="true" Display="None"></ValidationSettings>
                                                    </dxe:ASPxComboBox>
                                                </EditItemTemplate>
                                            </dxe:GridViewDataComboBoxColumn>--%>
                                            <dxe:GridViewDataButtonEditColumn FieldName="CityID" Caption="Sub Account" VisibleIndex="2" Width="300">
                                                <PropertiesButtonEdit>
                                                    <ClientSideEvents ButtonClick="SubAccountButnClick" KeyDown="SubAccountKeyDown" />
                                                    <Buttons>
                                                        <dxe:EditButton Text="..." Width="20px">
                                                        </dxe:EditButton>
                                                    </Buttons>
                                                </PropertiesButtonEdit>
                                            </dxe:GridViewDataButtonEditColumn>
                                            <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="WithDrawl" Caption="Amount" Width="180" EditCellStyle-HorizontalAlign="Right">
                                                <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
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
                                                    <%-- <ClientSideEvents KeyDown="AddBatchNew"></ClientSideEvents>--%>
                                                </PropertiesTextEdit>
                                                <CellStyle Wrap="False" HorizontalAlign="Left" CssClass="gridcellleft"></CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="4.5%" VisibleIndex="5" Caption="Taxes">
                                                <CustomButtons>
                                                    <dxe:GridViewCommandColumnCustomButton Text=" " ID="AddNew" Image-Url="/assests/images/add.png">
                                                        <Image Url="/assests/images/add.png">
                                                        </Image>
                                                    </dxe:GridViewCommandColumnCustomButton>
                                                </CustomButtons>
                                            </dxe:GridViewCommandColumn>

                                            <dxe:GridViewDataTextColumn FieldName="Parent_LedgerID" Caption="Parent_LedgerID" HeaderStyle-CssClass="hide" Width="0">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="CashReportID" Caption="Srl No" ReadOnly="true" HeaderStyle-CssClass="hide" Width="0">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="gvColMainAccount" Caption="hidden Field Id" Width="0" HeaderStyle-CssClass="hide">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="gvColSubAccount" Caption="hidden Field Id" Width="0" HeaderStyle-CssClass="hide">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="IsSubledger" Caption="hidden Field Id" Width="0" HeaderStyle-CssClass="hide">
                                            </dxe:GridViewDataTextColumn>
                                        </Columns>
                                        <TotalSummary>
                                            <dxe:ASPxSummaryItem SummaryType="Sum" FieldName="C2" Tag="C2_Sum" />
                                        </TotalSummary>
                                        <ClientSideEvents Init="OnInit" EndCallback="OnEndCallback" BatchEditStartEditing="OnBatchEditStartEditing"
                                            CustomButtonClick="OnCustomButtonClick" />
                                        <%--  <ClientSideEvents Init="OnInit" EndCallback="OnEndCallback" BatchEditStartEditing="OnBatchEditStartEditing" 
                                            BatchEditEndEditing="OnBatchEditEndEditing"
                                            CustomButtonClick="OnCustomButtonClick" />--%>
                                        <SettingsDataSecurity AllowEdit="true" />
                                        <SettingsEditing Mode="Batch" NewItemRowPosition="Bottom">
                                            <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="row" />
                                        </SettingsEditing>
                                    </dxe:ASPxGridView>
                                </div>
                                <div class="text-center">
                                    <table style="margin-left: 193px; margin-top: 5px; margin-bottom: 5px">

                                        <tr>
                                            <td style="padding-right: 6px; width: 150px;">Taxable Amount</td>
                                            <td>
                                                <dxe:ASPxTextBox ID="txtTaxableAmount" runat="server" Width="105px" ClientInstanceName="c_txtTaxableAmount" HorizontalAlign="Right" Font-Size="12px" ReadOnly="true">
                                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                                </dxe:ASPxTextBox>
                                            </td>
                                            <td style="padding-right: 16px; width: 150px;">Tax Amount</td>
                                            <td>
                                                <dxe:ASPxTextBox ID="txtTaxAmount" runat="server" Width="105px" ClientInstanceName="c_txtTaxAmount" HorizontalAlign="Right" Font-Size="12px" ReadOnly="true">
                                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                                </dxe:ASPxTextBox>
                                            </td>
                                            <td style="padding-right: 16px; width: 150px;">Total Amount</td>
                                            <td>
                                                <dxe:ASPxTextBox ID="txt_Debit" runat="server" Width="105px" ClientInstanceName="c_txt_Debit" HorizontalAlign="Right" Font-Size="12px" ReadOnly="true">
                                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                                </dxe:ASPxTextBox>
                                            </td>
                                            <td style="display: none;">
                                                <dxe:ASPxTextBox ID="txt_Credit" runat="server" Width="105px" ClientInstanceName="c_txt_Credit" HorizontalAlign="Right" Font-Size="12px" ReadOnly="true">
                                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
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
                                    <b><span id="tagged" style="display: none; color: red">This Customer Debit/Credit Note is tagged with Document : <span id="spanTaggedDocNo"></span>. Cannot Modify data!!</span></b>
                                    <dxe:ASPxButton ID="btnSaveRecords" ClientInstanceName="cbtnSaveRecords" runat="server" AutoPostBack="False" Text="S&#818;ave & New" CssClass="btn btn-primary" UseSubmitBehavior="False">
                                        <ClientSideEvents Click="function(s, e) {SaveButtonClick();}" />
                                    </dxe:ASPxButton>
                                    <dxe:ASPxButton ID="btn_SaveRecords" ClientInstanceName="cbtn_SaveRecords" runat="server" AutoPostBack="False" Text="Save & Ex&#818;it" CssClass="btn btn-primary" UseSubmitBehavior="False">
                                        <ClientSideEvents Click="function(s, e) {SaveExitButtonClick();}" />
                                    </dxe:ASPxButton>
                                    <dxe:ASPxButton ID="btnUDF" ClientInstanceName="cbtnUDF" runat="server" AutoPostBack="False" Text="U&#818;DF" CssClass="btn btn-primary" UseSubmitBehavior="False">
                                        <ClientSideEvents Click="function(s, e) {if(OpenUdf()){ return false}}" />
                                    </dxe:ASPxButton>
                                </div>

                                <div id="loadCurrencyMassage" style="display: none;">
                                    <label><span style="color: red; font-weight: bold; font-size: medium;">**  Mismatch detected in Total of Debit & Credit Amount.</span></label>
                                </div>
                                <div>
                                    <asp:SqlDataSource ID="dsBranch" runat="server"  ConflictDetection="CompareAllValues"
                                        SelectCommand="SELECT BRANCH_id AS BANKBRANCH_ID , RTRIM(BRANCH_DESCRIPTION)+' ['+ISNULL(RTRIM(BRANCH_CODE),'')+']' AS BANKBRANCH_NAME  FROM TBL_MASTER_BRANCH"></asp:SqlDataSource>

                                    <asp:SqlDataSource ID="SqlSchematype" runat="server" 
                                        SelectCommand="Select * From ((Select '0' as ID,'Select' as SchemaName) Union (Select ID,SchemaName +(Case When (SELECT Isnull(branch_description,'') FROM tbl_master_branch where branch_id=Branch) Is Null Then '' Else ' ('+(SELECT Isnull(branch_description,'') FROM tbl_master_branch where branch_id=Branch)+')' End) From tbl_master_Idschema  Where TYPE_ID=(Case When @NoteType='Dr' Then '25' Else '26' End) AND IsActive=1 AND Isnull(Branch,'') in (select s FROM dbo.GetSplit(',',@userbranchHierarchy)) AND Isnull(comapanyInt,'')=@LastCompany AND financial_year_id=(Select Top 1 FinYear_ID FROM Master_FinYear WHERE FinYear_Code=@LastFinYear))) as x Order By ID asc">
                                        <SelectParameters>
                                            <asp:SessionParameter Name="LastCompany" SessionField="LastCompany" />
                                            <asp:SessionParameter Name="userbranchHierarchy" SessionField="userbranchHierarchy" />
                                            <asp:SessionParameter Name="LastFinYear" SessionField="LastFinYear" />
                                            <asp:ControlParameter Name="NoteType" ControlID="ddlNoteType" PropertyName="SelectedValue" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                    <asp:SqlDataSource runat="server" ID="dsCustomer" 
                                        SelectCommand="proc_DebitCreditNoteDetails" SelectCommandType="StoredProcedure">
                                        <SelectParameters>
                                            <asp:Parameter Type="String" Name="Action" DefaultValue="PopulateCustomerDetail" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                    <asp:SqlDataSource ID="SqlReasonIssuedocument" runat="server" 
                                        SelectCommand="Select * From ((Select '0' as ReasonID,'Select' as Reason) Union (select * from Master_Reason_for_doc))tbl"></asp:SqlDataSource>
                                </div>
                            </dxe:ContentControl>
                        </ContentCollection>
                    </dxe:TabPage>
                    <dxe:TabPage Name="Billing/Shipping" Text="Billing/Shipping">
                        <ContentCollection>
                            <dxe:ContentControl runat="server">
                                <ucBS:BillingShippingControl runat="server" ID="BillingShippingControl" />
                                <asp:HiddenField runat="server" ID="hfTermsConditionDocType" Value="CN" />
                            </dxe:ContentControl>
                        </ContentCollection>
                    </dxe:TabPage>
                </TabPages>
                <ClientSideEvents ActiveTabChanged="function(s, e) {
	                                            var activeTab = page.GetActiveTab();
	                                            var Tab0 = page.GetTab(0);
	                                            var Tab1 = page.GetTab(1);
	                                            var Tab2 = page.GetTab(2);
                                                 
                                               if(activeTab == Tab0)
	                                            {
	                                                disp_prompt('tab0');
	                                            }
	                                            if(activeTab == Tab1)
	                                            {
	                                                disp_prompt('tab1');
	                                            }
	                                           else if(activeTab == Tab2)
	                                            {
	                                                disp_prompt('tab2');
	                                            }


	                                            }"></ClientSideEvents>

            </dxe:ASPxPageControl>
        </div>
    </div>
    <asp:SqlDataSource ID="SqlDataSourceapplicable" runat="server" 
        SelectCommand="(SELECT 'Debit Note' as TypeID,'Debit Note' as TypeName) Union (SELECT 'Credit Note' as TypeID,'Credit Note' as TypeName)"></asp:SqlDataSource>
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
        <asp:HiddenField runat="server" ID="hfLedgerName" />
        <asp:HiddenField runat="server" ID="hfLedgerID" />
        <asp:HiddenField runat="server" ID="hfWithDrawlAmount" />
        <asp:HiddenField runat="server" ID="hfCashReportID" />
        <asp:HiddenField runat="server" ID="hfPageLoadFlag" />
        <asp:HiddenField runat="server" ID="hfFocusSet" Value="" />
        <asp:HiddenField runat="server" ID="hfFirstTimeLoadFlag" Value="" />
    </div>
    <div style="display: none">
        <%-- <asp:Button ID="btnPrint" runat="server" Text="Print" OnClick="btnPrint_Click" meta:resourcekey="btnPrintResource1" />--%>
        <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A3" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
    </div>
    <dxe:ASPxPopupControl ID="ASPXPopupControl2" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popup" Height="630px"
        Width="600px" HeaderText="Add/Modify UDF" Modal="true" AllowResize="true" ResizingMode="Postponed">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>
    <dxe:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" ContainerElementID="grid"
        Modal="True">
    </dxe:ASPxLoadingPanel>

    <%--InlineTax--%>
    <dxe:ASPxPopupControl ID="aspxTaxpopUp" runat="server" ClientInstanceName="caspxTaxpopUp"
        Width="850px" HeaderText="Select Tax" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
        <HeaderTemplate>
            <span style="color: #fff"><strong>Select Tax</strong></span>
            <dxe:ASPxImage ID="ASPxImage31" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader pull-right">
                <ClientSideEvents Click="function(s, e){ 
                                         cgridTax.CancelEdit();
                                         caspxTaxpopUp.Hide();
                                         grid.AddNewRow();
                                         }" />
            </dxe:ASPxImage>
        </HeaderTemplate>
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <asp:HiddenField runat="server" ID="setCurrentProdCode" />
                <asp:HiddenField runat="server" ID="HDParentSlNo" />
                <asp:HiddenField runat="server" ID="HDmainAccount_id" />
                <asp:HiddenField runat="server" ID="HdProdGrossAmt" />
                <asp:HiddenField runat="server" ID="HdProdNetAmt" />
                <asp:HiddenField runat="server" ID="HDstatus" Value="D" />
                <div id="content-6">
                    <div class="col-sm-3">
                        <div class="lblHolder">
                            <table>
                                <tbody>
                                    <tr>
                                        <td>Gross Amount
                                                    <dxe:ASPxLabel ID="ASPxLabel1" runat="server" Text="(Taxable)" ClientInstanceName="clblTaxableGross"></dxe:ASPxLabel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dxe:ASPxLabel ID="lblTaxProdGrossAmt" runat="server" Text="" ClientInstanceName="clblTaxProdGrossAmt"></dxe:ASPxLabel>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>

                    <div class="col-sm-3 gstGrossAmount">
                        <div class="lblHolder">
                            <table>
                                <tbody>
                                    <tr>
                                        <td>GST</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dxe:ASPxLabel ID="lblGstForGross" runat="server" Text="" ClientInstanceName="clblGstForGross"></dxe:ASPxLabel>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>

                    <div class="col-sm-3">
                        <div class="lblHolder">
                            <table>
                                <tbody>
                                    <tr>
                                        <td>Discount</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dxe:ASPxLabel ID="lblTaxDiscount" runat="server" Text="" ClientInstanceName="clblTaxDiscount"></dxe:ASPxLabel>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>


                    <div class="col-sm-3">
                        <div class="lblHolder">
                            <table>
                                <tbody>
                                    <tr>
                                        <td>Net Amount
                                                    <dxe:ASPxLabel ID="ASPxLabel5" runat="server" Text="(Taxable)" ClientInstanceName="clblTaxableNet"></dxe:ASPxLabel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dxe:ASPxLabel ID="lblProdNetAmt" runat="server" Text="" ClientInstanceName="clblProdNetAmt"></dxe:ASPxLabel>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>

                    <div class="col-sm-2 gstNetAmount">
                        <div class="lblHolder">
                            <table>
                                <tbody>
                                    <tr>
                                        <td>GST</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dxe:ASPxLabel ID="lblGstForNet" runat="server" Text="" ClientInstanceName="clblGstForNet"></dxe:ASPxLabel>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>

                </div>

                <%--Error Message--%>
                <div id="ContentErrorMsg">
                    <div class="col-sm-8">
                        <div class="lblHolder">
                            <table>
                                <tbody>
                                    <tr>
                                        <td>Status
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Tax Code/Charges Not defined.
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
                <table style="width: 100%;">
                    <tr>
                        <td colspan="2"></td>
                    </tr>

                    <tr>
                        <td colspan="2"></td>
                    </tr>


                    <tr style="display: none">
                        <td><span><strong>Product Basic Amount</strong></span></td>
                        <td>
                            <dxe:ASPxTextBox ID="txtprodBasicAmt" MaxLength="80" ClientInstanceName="ctxtprodBasicAmt" ReadOnly="true"
                                runat="server" Width="50%">
                                <MaskSettings Mask="&lt;0..999999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                            </dxe:ASPxTextBox>
                        </td>
                    </tr>
                    <tr class="cgridTaxClass">
                        <td colspan="3">
                            <dxe:ASPxGridView runat="server" OnBatchUpdate="taxgrid_BatchUpdate" KeyFieldName="Taxes_ID" ClientInstanceName="cgridTax" ID="aspxGridTax"
                                Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" SettingsPager-Mode="ShowAllRecords" OnCustomCallback="cgridTax_CustomCallback"
                                Settings-ShowFooter="false" AutoGenerateColumns="False" OnCellEditorInitialize="aspxGridTax_CellEditorInitialize" OnHtmlRowCreated="aspxGridTax_HtmlRowCreated"
                                OnRowInserting="taxgrid_RowInserting" OnRowUpdating="taxgrid_RowUpdating" OnRowDeleting="taxgrid_RowDeleting">
                                <Settings VerticalScrollableHeight="150" VerticalScrollBarMode="Auto"></Settings>
                                <SettingsBehavior AllowDragDrop="False" AllowSort="False"></SettingsBehavior>
                                <SettingsPager Visible="false"></SettingsPager>
                                <Columns>
                                    <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="Taxes_Name" ReadOnly="true" Caption="Tax Component ID">
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="taxCodeName" ReadOnly="true" Caption="Tax Component Name">
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="calCulatedOn" ReadOnly="true" Caption="Calculated On">
                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                            <MaskSettings Mask="&lt;0..999999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                        </PropertiesTextEdit>
                                        <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                    <%--<dxe:GridViewDataComboBoxColumn Caption="percentage" FieldName="TaxField" VisibleIndex="2">
                                                <PropertiesComboBox ClientInstanceName="cTaxes_Name" ValueField="Taxes_ID" TextField="Taxes_Name" DropDownStyle="DropDown">
                                                    <ClientSideEvents SelectedIndexChanged="cmbtaxCodeindexChange"
                                                        GotFocus="CmbtaxClick" />
                                                </PropertiesComboBox>
                                            </dxe:GridViewDataComboBoxColumn>--%>
                                    <dxe:GridViewDataTextColumn Caption="Percentage" FieldName="TaxField" VisibleIndex="4">
                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                            <ClientSideEvents LostFocus="txtPercentageLostFocus" GotFocus="CmbtaxClick" />
                                            <MaskSettings Mask="&lt;0..999999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                        </PropertiesTextEdit>
                                        <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="Amount" Caption="Amount" ReadOnly="true">
                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                            <ClientSideEvents LostFocus="taxAmountLostFocus" GotFocus="taxAmountGotFocus" />
                                            <MaskSettings Mask="&lt;0..999999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                        </PropertiesTextEdit>
                                        <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                </Columns>
                                <%--  <SettingsPager Mode="ShowAllRecords"></SettingsPager>--%>
                                <SettingsDataSecurity AllowEdit="true" />
                                <SettingsEditing Mode="Batch">
                                    <BatchEditSettings EditMode="row" />
                                </SettingsEditing>
                                <ClientSideEvents EndCallback="cgridTax_EndCallBack " RowClick="GetTaxVisibleIndex" />
                            </dxe:ASPxGridView>

                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <table class="InlineTaxClass">
                                <tr class="GstCstvatClass" style="">
                                    <td style="padding-top: 10px; padding-bottom: 15px; padding-right: 25px"><span><strong>GST</strong></span></td>
                                    <td style="padding-top: 10px; padding-bottom: 15px;">
                                        <dxe:ASPxComboBox ID="cmbGstCstVat" ClientInstanceName="ccmbGstCstVat" runat="server" SelectedIndex="-1"
                                            ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" TextFormatString="{0}"
                                            ClearButton-DisplayMode="Always" OnCallback="cmbGstCstVat_Callback">
                                            <Columns>
                                                <dxe:ListBoxColumn FieldName="Taxes_Name" Caption="Tax Component ID" Width="250" />
                                                <dxe:ListBoxColumn FieldName="TaxCodeName" Caption="Tax Component Name" Width="250" />

                                            </Columns>
                                            <ClientSideEvents SelectedIndexChanged="cmbGstCstVatChange"
                                                GotFocus="CmbtaxClick" />
                                        </dxe:ASPxComboBox>
                                    </td>
                                    <td style="padding-left: 15px; padding-top: 10px; padding-bottom: 15px; padding-right: 25px">
                                        <dxe:ASPxTextBox ID="txtGstCstVat" MaxLength="80" ClientInstanceName="ctxtGstCstVat" ReadOnly="true" Text="0.00"
                                            runat="server" Width="100%">
                                            <MaskSettings Mask="&lt;-999999999999..999999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                        </dxe:ASPxTextBox>


                                    </td>
                                    <td>
                                        <input type="button" onclick="recalculateTax()" class="btn btn-info btn-small RecalculateInline" value="Recalculate GST" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" style="padding-top: 10px;">
                            <div class="pull-left">
                                <asp:Button ID="btnTaxpopup" runat="server" Text="Ok" CssClass="btn btn-primary mTop" OnClientClick="return BatchUpdate();" Width="85px" />
                                <asp:Button ID="Button2" runat="server" Text="Cancel" CssClass="btn btn-danger mTop" Width="85px" OnClientClick="cgridTax.CancelEdit(); caspxTaxpopUp.Hide(); grid.AddNewRow();  return false;" />
                            </div>
                            <table class="pull-right">
                                <tr>
                                    <td style="padding-right: 5px"><strong>Total Charges</strong></td>
                                    <td>
                                        <dxe:ASPxTextBox ID="txtTaxTotAmt" MaxLength="80" ClientInstanceName="ctxtTaxTotAmt" Text="0.00" ReadOnly="true"
                                            runat="server" Width="100%" CssClass="pull-left mTop">
                                            <MaskSettings Mask="&lt;-999999999999..999999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                            <%--<MaskSettings Mask="<0..999999999>.<0..99>" AllowMouseWheel="false" /> --%>
                                        </dxe:ASPxTextBox>

                                    </td>
                                </tr>
                            </table>


                            <div class="clear"></div>
                        </td>
                    </tr>

                </table>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <ContentStyle VerticalAlign="Top" CssClass="pad"></ContentStyle>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
    </dxe:ASPxPopupControl>
    <%--End Sayan 21-06-2016--%>
    <%--DEBASHIS--%>
    <div class="PopUpArea">
        <dxe:ASPxPopupControl ID="ASPxDocumentsPopup" runat="server" ClientInstanceName="cDocumentsPopup"
            Width="350px" HeaderText="Select Design(s)" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <dxe:ASPxCallbackPanel runat="server" ID="SelectPanel" ClientInstanceName="cSelectPanel" OnCallback="SelectPanel_Callback" ClientSideEvents-EndCallback="cSelectPanelEndCall">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <dxe:ASPxComboBox ID="CmbDesignName" ClientInstanceName="cCmbDesignName" runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True">
                                </dxe:ASPxComboBox>
                                <div class="text-center pTop10">
                                    <dxe:ASPxButton ID="btnOK" ClientInstanceName="cbtnOK" runat="server" AutoPostBack="False" Text="OK" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                        <ClientSideEvents Click="function(s, e) {return PerformCallToGridBind();}" />
                                    </dxe:ASPxButton>
                                </div>
                                <asp:HiddenField ID="HdCrDrNoteType" runat="server" />
                            </dxe:PanelContent>
                        </PanelCollection>
                    </dxe:ASPxCallbackPanel>
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>
    </div>
    <%--DEBASHIS--%>
    <dxe:ASPxCallbackPanel runat="server" ID="taxUpdatePanel" ClientInstanceName="ctaxUpdatePanel" OnCallback="taxUpdatePanel_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="ctaxUpdatePanelEndCall" />
    </dxe:ASPxCallbackPanel>
    <%--Sayan Section End--%>
    <dxe:ASPxCallbackPanel runat="server" ID="exUpdatePanel" ClientInstanceName="cexUpdatePanel" OnCallback="exUpdatePanel_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="cexUpdatePanel_EndCallBack" />
    </dxe:ASPxCallbackPanel>
    <asp:SqlDataSource ID="CustomerDataSource" runat="server"  />
    <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
        <ClientSideEvents ControlsInitialized="AllControlInitilize" />
    </dxe:ASPxGlobalEvents>

    <div>
        <asp:HiddenField ID="hfIsFilter" runat="server" />
        <asp:HiddenField ID="hfFromDate" runat="server" />
        <asp:HiddenField ID="hfToDate" runat="server" />
        <asp:HiddenField ID="hfBranchID" runat="server" />
        <asp:HiddenField ID="hdnBranchId" runat="server" />
        <asp:HiddenField ID="hdnMainAccountId" runat="server" />
        <asp:HiddenField ID="hdnCustomerId" runat="server" />
    </div>
    <%-- -------------------POPUPControl   FOR Main & Sub Account-------------------------------------%>
    <%-- <dxe:ASPxPopupControl ID="MainAccountpopUp" runat="server" ClientInstanceName="cMainAccountpopUp"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Height="400"
        Width="700" HeaderText="Select Main Account" AllowResize="true" ResizingMode="Postponed" Modal="true">
        <HeaderTemplate>
            <span style="color: #fff"><strong>Search By Main Account (4 Char)</strong></span>
            <dxe:ASPxImage ID="ASPxImage3" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader pull-right">
                <ClientSideEvents Click="function(s, e){ 
                                                                       MainAccountClose();
                                                                    }" />
            </dxe:ASPxImage>
        </HeaderTemplate>
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <label><strong>Search By Main Account(4 Char)</strong> <span style="color: red">[Press Esc to Cancel]</span></label>
                <div id="mainActMsg">
                    <span style="color: red; right: 46px;"><strong>* Invalid Main Account</strong> </span>
                </div>
                <dxe:ASPxComboBox ID="MainAccountComboBox" runat="server" EnableCallbackMode="true" CallbackPageSize="10" Width="100%"
                    ValueType="System.String" ValueField="MainAccount_ReferenceID" ClientInstanceName="cMainAccountComboBox"
                    OnItemsRequestedByFilterCondition="ASPxMainAccountComboBox_OnItemsRequestedByFilterCondition_SQL"
                    OnItemRequestedByValue="ASPxMainComboBox_OnItemRequestedByValue_SQL"
                    FilterMinLength="4"
                    TextFormatString="{0}"
                    DropDownStyle="DropDown">

                    <Columns>
                        <dxe:ListBoxColumn FieldName="MainAccount_Name" Caption="Main Account Name" Width="320px" />
                        <dxe:ListBoxColumn FieldName="MainAccount_SubLedgerType" Caption="Sub Account Type" Width="150px" />
                        <dxe:ListBoxColumn FieldName="MainAccount_ReverseApplicable" Caption="ReverseApplicable" Width="0" />
                        <dxe:ListBoxColumn FieldName="TAXable" Caption="TAXable" Width="0" />
                    </Columns>
                    <ClientSideEvents ValueChanged="function(s, e) {GetMainAcountComboBox(e)}" KeyDown="MainAccountComboBoxKeyDown" />
                </dxe:ASPxComboBox>

            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="Blue" Font-Bold="True" ForeColor="White" />
    </dxe:ASPxPopupControl>

    <dxe:ASPxPopupControl ID="SubAccountpopUp" runat="server" ClientInstanceName="cSubAccountpopUp"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Height="200"
        Width="700" HeaderText="Select Sub Account" AllowResize="true" ResizingMode="Postponed" Modal="true">
        <HeaderTemplate>
            <span style="color: #fff"><strong>Search By Sub Account (4 Char)</strong></span>
            <dxe:ASPxImage ID="ASPxImage3" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader pull-right">
                <ClientSideEvents Click="function(s, e){ 
                                                                       SubAccountClose();
                                                                    }" />
            </dxe:ASPxImage>
        </HeaderTemplate>
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <label><strong>Search By Sub Account(4 Char)</strong><span style="color: red"> [Press Esc to Cancel]</span></label>
                <div id="subActMsg">
                    <span style="color: red; right: 46px;"><strong>* Invalid Sub Account</strong> </span>
                </div>
                <dxe:ASPxComboBox ID="SubAcountComboBox" runat="server" EnableCallbackMode="true" CallbackPageSize="10" Width="95%"
                    ValueType="System.String" ValueField="SubAccount_ReferenceID" ClientInstanceName="cSubAcountComboBox"
                    OnItemsRequestedByFilterCondition="SubAcountComboBox_OnItemsRequestedByFilterCondition_SQL" FilterMinLength="4"
                    OnItemRequestedByValue="SubAcountComboBox_OnItemRequestedByValue_SQL" TextFormatString="{0}"
                    DropDownStyle="DropDown" DropDownRows="7">
                    <Columns>
                        <dxe:ListBoxColumn FieldName="Contact_Name" Caption="Sub Account Name" Width="320px" />
                    </Columns>
                    <ClientSideEvents ValueChanged="function(s, e) {GetSubAcountComboBox(e)}" KeyDown="SubAccountComboBoxKeyDown" />
                </dxe:ASPxComboBox>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="Blue" Font-Bold="True" ForeColor="White" />
    </dxe:ASPxPopupControl>
    <asp:SqlDataSource ID="SqlDataSourceMainAccount" runat="server" ConnectionString="<%$ ConnectionStrings:crmConnectionString %>"
        SelectCommand=""></asp:SqlDataSource>--%>
    <%--<asp:SqlDataSource ID="SqlDataSourceSubAccount" runat="server" ConnectionString="<%$ ConnectionStrings:crmConnectionString %>"
        SelectCommand=""></asp:SqlDataSource>--%>

    <%--Main/Sub Account Model--%>
    <%--Rev 1.0--%>
    <%--<div class="modal fade" id="MainAccountModel" role="dialog">--%>
    <div class="modal fade" id="MainAccountModel" role="dialog" data-backdrop="static" data-keyboard="false">
        <%--Rev end 1.0--%>
        <div class="modal-dialog">

            <!-- Modal content-->
            <!-- Modal MainAccount-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" onclick="closeModal();">&times;</button>
                    <h4 class="modal-title">Main Account Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="MainAccountNewkeydown(event)" id="txtMainAccountSearch" autofocus width="100%" placeholder="Search by Main Account Name or Short Name" />

                    <div id="MainAccountTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th>Main Account Name</th>
                                <th>Short Name</th>
                                <th>Subledger Type</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" onclick="closeModal();">Close</button>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="SubAccountModel" role="dialog" data-backdrop="static"
        data-keyboard="false">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" onclick="CloseSubModal();">&times;</button>
                    <h4 class="modal-title">Sub Account Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="SubAccountNewkeydown(event)" id="txtSubAccountSearch" autofocus width="100%" placeholder="Search By Sub Account Name or Code" />

                    <div id="SubAccountTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th>Sub Account Name [Unique ID]</th>
                                <th>Sub Account Code</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" onclick="CloseSubModal();">Close</button>
                </div>
            </div>

        </div>
    </div>
    <%-- End --%>
    <!--Customer Modal -->
    <%--Rev 1.0--%>
    <%--<div class="modal fade" id="CustModel" role="dialog">--%>
    <div class="modal fade" id="CustModel" role="dialog" data-backdrop="static" data-keyboard="false">
    <%--Rev end 1.0--%>
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Customer Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Customerkeydown(event)" id="txtCustSearch" autofocus width="100%" placeholder="Search by Customer Name or Unique Id" />
                    <div id="CustomerTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Customer Name</th>
                                <th>Unique Id</th>
                                <th>Address</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>
    <!--Customer Modal -->
</asp:Content>
