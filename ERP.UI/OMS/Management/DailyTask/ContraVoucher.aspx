<%--=======================================================Revision History=========================================================================    
    1.0 Priti   V2.0.36  02-02-2023     0025264: listing view upgradation required of Contra Voucher of Accounts & Finance
=========================================================End Revision History========================================================================--%>


<%@ Page Title="Contra Voucher" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="ContraVoucher.aspx.cs" Inherits="ERP.OMS.Management.DailyTask.ContraVoucher" %>


<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Activities/JS/SearchPopup.js"></script>
    <link href="../Activities/CSS/SearchPopup.css" rel="stylesheet" />
    <script type="text/javascript">
        ActiveCurrencySymbol = "";

        function SetLostFocusonDemand(e) {
            if ((new Date($("#hdnLockFromDate").val()) <= tDate.GetDate()) && (tDate.GetDate() <= new Date($("#hdnLockToDate").val()))) {
                jAlert("DATA is Freezed between   " + $("#hdnLockFromDateCon").val() + " to " + $("#hdnLockToDateCon").val() + " for Add.");
            }
        }

        function AllControlInitilize() {
            if (localStorage.getItem('ContraVoucherFromDate')) {
                var fromdatearray = localStorage.getItem('ContraVoucherFromDate').split('-');
                var fromdate = new Date(fromdatearray[0], parseFloat(fromdatearray[1]) - 1, fromdatearray[2], 0, 0, 0, 0);
                cFormDate.SetDate(fromdate);
            }

            if (localStorage.getItem('ContraVoucherToDate')) {
                var todatearray = localStorage.getItem('ContraVoucherToDate').split('-');
                var todate = new Date(todatearray[0], parseFloat(todatearray[1]) - 1, todatearray[2], 0, 0, 0, 0);
                ctoDate.SetDate(todate);
            }
            if (localStorage.getItem('ContraVoucherBranch')) {
                if (ccmbBranchfilter.FindItemByValue(localStorage.getItem('ContraVoucherBranch'))) {
                    ccmbBranchfilter.SetValue(localStorage.getItem('ContraVoucherBranch'));
                }

            }
            // updateGridByDate();
        }

        ////****************************************New Block for Print --04.04.2018 *************************************************************

        $(document).ready(function () {
            if ($("#hdn_Mode").val() == "EDIT") {
                clookup_Project.SetEnabled(true);
            }
        });


        function onPrintJv(id) {
           
            VoucherId = id;
            cDocumentsPopup.Show();
            cCmbDesignName.SetSelectedIndex(0);
            cSelectPanel.PerformCallback('Bindalldesignes');
            $('#btnOK').focus();
        }
        function cSelectPanelEndCall(s, e) {
         
            if (cSelectPanel.cpSuccess != null) {
                var reportName = cCmbDesignName.GetValue();
                var module = 'CONTRAVOUCHER';
                window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + VoucherId, '_blank')
            }
            if (cSelectPanel.cpSuccess == "") {
                cCmbDesignName.SetSelectedIndex(0);
            }
        }
        function PerformCallToGridBind() {
            cSelectPanel.PerformCallback('Bindsingledesign');
            cDocumentsPopup.Hide();
            return false;
        }
        //****************************************************************************************************************************************
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
                localStorage.setItem("ContraVoucherFromDate", cFormDate.GetDate().format('yyyy-MM-dd'));
                localStorage.setItem("ContraVoucherToDate", ctoDate.GetDate().format('yyyy-MM-dd'));
                localStorage.setItem("ContraVoucherBranch", ccmbBranchfilter.GetValue());

                $("#hfFromDate").val(cFormDate.GetDate().format('yyyy-MM-dd'));
                $("#hfToDate").val(ctoDate.GetDate().format('yyyy-MM-dd'));
                $("#hfBranchID").val(ccmbBranchfilter.GetValue());
                $("#hfIsFilter").val("Y");

                
                //rev 1.0
                // gvContraVoucherInstance.Refresh();
                $("#hFilterType").val("All");
                cCallbackPanel.PerformCallback("");
                 //end rev 1.0


                //gvContraVoucherInstance.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue())
            }
        }
        //rev 1.0
        function CallbackPanelEndCall(s, e) {
            gvContraVoucherInstance.Refresh();
        }
        //end rev 1.0
        $(function () {

            var IsEdit = false;
        });
        function PageLoad() {
            var ActiveCurrency = '<%=Session["ActiveCurrency"]%>'

            ActiveCurrencySymbol = ActiveCurrency.split('~')[2];
        }
        function showExitMsg(obj) {

            var JV_Msg = "Cash/Bank Voucher No. " + obj + " generated.";
            var strconfirm = confirm(JV_Msg);
            if (strconfirm == true) {
                window.location.href = "ContraVoucher.aspx";
            }
            else {
                window.location.href = "ContraVoucher.aspx";
            }

        }


        function NextFocus(s, e) {
            var str = $('#<%=hdn_Mode.ClientID %>').val();
            var Campany_ID = '<%=Session["LastCompany"]%>';
            var LocalCurrency = '<%=Session["LocalCurrency"]%>';
            var basedCurrency = LocalCurrency.split("~");
            var setCurrency = grid.GetEditor('Currency_ID').GetValue();
            if (basedCurrency[0] == setCurrency) {
                if (str == "EDIT") {
                    grid.batchEditApi.StartEdit(0, 5);
                }
                else {
                    grid.batchEditApi.StartEdit(-1, 5);
                }


            }
            else {
                var str = $('#<%=hdn_Mode.ClientID %>').val();

                if (str == "EDIT") {
                    grid.batchEditApi.StartEdit(0, 3);
                }
                else {
                    grid.batchEditApi.StartEdit(-1, 3);
                }

            }
        }
        function txtBillNo_TextChanged() {
            var VoucherNo = document.getElementById("txtVoucherNo").value;

            $.ajax({
                type: "POST",
                url: "ContraVoucher.aspx/CheckUniqueName",
                data: JSON.stringify({ VoucherNo: VoucherNo }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var data = msg.d;

                    if (data == true) {
                        $("#MandatoryBillNoDUPLICALE").show();                      
                        document.getElementById("txtVoucherNo").value = '';
                        document.getElementById("txtVoucherNo").focus();
                    }
                    else {
                        $("#MandatoryBillNoDUPLICALE").hide();
                    }
                }
            });
        }
        function RateOnKeyDown(s, e) {
            if (e.htmlEvent.keyCode == 40 || e.htmlEvent.keyCode == 38)
                return ASPxClientUtils.PreventEvent(e.htmlEvent);
        }
        function HomeCurrencyOnKeyDown(s, e) {
            if (e.htmlEvent.keyCode == 40 || e.htmlEvent.keyCode == 38)
                return ASPxClientUtils.PreventEvent(e.htmlEvent);
        }
        function InstrumentDateChange() {
            var SelectedDate = new Date(cInstDate.GetDate());
            var monthnumber = SelectedDate.getMonth();
            var monthday = SelectedDate.getDate();
            var year = SelectedDate.getYear();
            var SelectedDateValue = new Date(year, monthnumber, monthday);            
            var MaxLockDate = new Date('<%=Session["LCKBNK"]%>');
            monthnumber = MaxLockDate.getMonth();
            monthday = MaxLockDate.getDate();
            year = MaxLockDate.getYear();
            var MaxLockDateNumeric = new Date(year, monthnumber, monthday).getTime();

            if (SelectedDateValue <= MaxLockDateNumeric) {
                jAlert('This Entry Date has been Locked.');
                MaxLockDate.setDate(MaxLockDate.getDate() + 1);
                cInstDate.SetDate(MaxLockDate);
                return;
            }           
            var FYS = "<%=Session["FinYearStart"]%>";
            var FYE = "<%=Session["FinYearEnd"]%>";
            var LFY = "<%=Session["LastFinYear"]%>";
            var FinYearStartDate = new Date(FYS);
            var FinYearEndDate = new Date(FYE);
            var LastFinYearDate = new Date(LFY);
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
               
            }
            else {
                jAlert('Enter Date Is Outside Of Financial Year !!');
                if (SelectedDateNumericValue < FinYearStartDateNumericValue) {
                    cInstDate.SetDate(new Date(FinYearStartDate));
                }
                if (SelectedDateNumericValue > FinYearEndDatNumbericValue) {
                    cInstDate.SetDate(new Date(FinYearEndDate));
                }
            }          
        }
        function TDateChange() {
            var SelectedDate = new Date(tDate.GetDate());
            var monthnumber = SelectedDate.getMonth();
            var monthday = SelectedDate.getDate();
            var year = SelectedDate.getYear();

            var SelectedDateValue = new Date(year, monthnumber, monthday);
            ///Checking of Transaction Date For MaxLockDate
            var MaxLockDate = new Date('<%=Session["LCKBNK"]%>');
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
            var FYS = "<%=Session["FinYearStart"]%>";
            var FYE = "<%=Session["FinYearEnd"]%>";
            var LFY = "<%=Session["LastFinYear"]%>";
            var FinYearStartDate = new Date(FYS);
            var FinYearEndDate = new Date(FYE);
            var LastFinYearDate = new Date(LFY);

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
            ///End OF Date Should Between Current Fin Year StartDate and EndDate
        }       
        function SetVisibility() {
            var Currency_ID = (grid.GetEditor('Currency_ID').GetValue() != null) ? parseFloat(grid.GetEditor('Currency_ID').GetValue()) : "0";
            //  var Currency_ID = (grid.GetEditor('Currency_ID').GetValue() != null) ? parseFloat(grid.GetEditor('Currency_ID').GetValue()) : "0";
            var Campany_ID = '<%=Session["LastCompany"]%>';
            var LocalCurrency = '<%=Session["LocalCurrency"]%>';
            var basedCurrency = LocalCurrency.split("~");
            var tdRate = grid.GetEditor('Rate');
            var tdHomeCurrency = grid.GetEditor('CashBankDetail_PaymentAmount');
            var tdAmount = grid.GetEditor('Amount');
            $.ajax({
                type: "POST",
                url: "ContraVoucher.aspx/GetRate",
                data: JSON.stringify({ Currency_ID: Currency_ID, Campany_ID: Campany_ID, basedCurrency: basedCurrency[0] }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var data = msg.d;
                    tdRate.SetValue(data);
                    tdHomeCurrency.SetValue(tdRate.GetValue() * tdAmount.GetValue());

                }
            });
            if (Currency_ID == "0") {
                grid.GetEditor('Amount').SetEnabled(false);
                grid.GetEditor('Rate').SetEnabled(false);
            }
            else {
                grid.GetEditor('Amount').SetEnabled(true);
                grid.GetEditor('Rate').SetEnabled(true);
            }
        }
        function OnGetRowValuesOnDelete(values, TransactionDate, BankValueDate, ref) {

            var ValueDate = new Date(BankValueDate);
            var monthnumber = ValueDate.getMonth();
            var monthday = ValueDate.getDate();
            var year = ValueDate.getYear();
            var ValueDateNumeric = new Date(year, monthnumber, monthday).getTime();

            var TransactionDate = new Date(TransactionDate);
            monthnumber = TransactionDate.getMonth();
            monthday = TransactionDate.getDate();
            year = TransactionDate.getYear();
            var TransactionDateNumeric = new Date(year, monthnumber, monthday).getTime();

            var MaxLockDate = new Date('<%=Session["LCKBNK"] %>');
            monthnumber = MaxLockDate.getMonth();
            monthday = MaxLockDate.getDate();
            year = MaxLockDate.getYear();
            var MaxLockDateNumeric = new Date(year, monthnumber, monthday).getTime();
            if (TransactionDateNumeric <= MaxLockDateNumeric) {
                jAlert('This Entry has been Locked.Contact Your System Administrator!!!.');
                return;
            }
            if (BankValueDate == "") {
                jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                    if (r == true) {
                        gvContraVoucherInstance.PerformCallback('Delete~' + values + "~" + ref);
                    }
                });
            }
            else {
                jAlert('Voucher is Reconciled.Cannot Delete');
            }
        }
        function BranchFrom_SelectedIndexChanged() {
            var branchfrom = $("#ddlBranch").val();
            $("#hddn_BranchID").val(branchfrom);        
            var strWithDrawFrom = grid.GetEditor('WithDrawFrom').GetValue();
            if (strWithDrawFrom != null) {
                jConfirm('You have changed Branch. All the entries of ledger in this voucher to be reset to blank. \n You have to select and re-enter.Want to continue?', 'Confirmation Dialog', function (r) {

                    if (r == true) {
                        grid.DeleteRow(0);
                        grid.DeleteRow(-1);
                        grid.AddNewRow();
                        // $("#hddn_BranchID").val(document.getElementById('ddlBranch').value);
                    }
                    else {
                        // document.getElementById('ddlBranch').value = $("#hddn_BranchID").val();
                    }
                });
            }
            if ($("#hdnProjectSelectInEntryModule").val() == "1")
                clookup_Project.gridView.Refresh();
            WithDrawFrom.PerformCallback(branchfrom);          
            grid.batchEditApi.StartEdit(-1, 0);
        }

        function batchgridFocus() {         
            var branchTo = $("#ddlBranchTo").val();
            $("#hdnBranchIdTo").val(branchTo);
            var strDepositInto = grid.GetEditor('DepositInto').GetValue();
            if (strDepositInto != null) {
                jConfirm('You have changed Branch. All the entries of ledger in this voucher to be reset to blank. \n You have to select and re-enter.Want to continue?', 'Confirmation Dialog', function (r) {

                    if (r == true) {
                        grid.DeleteRow(0);
                        grid.DeleteRow(-1);
                        grid.AddNewRow();                        
                    }
                    else {                       
                    }
                });
            }           
            DepositInto.PerformCallback(document.getElementById('ddlBranchTo').value);           
        }


        function ddlBranchTo_LostFocus() {
            if ($("#hdnProjectSelectInEntryModule").val() == "0") {
                grid.batchEditApi.StartEdit(-1, 0);
            }
            //else if ($("#hdnProjectSelectInEntryModule").val() == "1") {
            //    clookup_Project.SetFocus();
            //}

            // clookup_Project.SetFocus();
            //grid.batchEditApi.StartEdit(-1, 0);
        }

        function clookup_Project_LostFocus() {

            grid.batchEditApi.StartEdit(-1, 0);
        //Hierarchy Start Tanmoy
        var projID = clookup_Project.GetValue();
        if (projID == null || projID == "") {
            $("#ddlHierarchy").val(0);
        }
        //Hierarchy End Tanmoy
        }

        //Hierarchy Start Tanmoy
        function ProjectValueChange(s, e) {
            //debugger;
            var projID = clookup_Project.GetValue();

            $.ajax({
                type: "POST",
                url: 'ContraVoucher.aspx/getHierarchyID',
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

        $(document).ready(function () {
           // grid_col8.style.display = 'none';
            IsEdit = false;
            $('#ddlBranch').blur(function () {
               // grid.batchEditApi.StartEdit(-1, 0);
            })
            /*Mantis Issue:- 0024782 Swati Rev work Copy feature Add 25.03.2022*/
            var id = 0;
            var Type = 'Add';
            SetNumberingSchemeDataSource(id, Type);
            /*Close of Mantis Issue:- 0024782 Swati Rev work Copy feature Add 25.03.2022*/
        });

        function OnAddButtonClick() {
            IsEdit = false;
            grid.GetEditor('Amount').SetEnabled(false);
            grid.GetEditor('Rate').SetEnabled(false);
            document.getElementById('divAddNew').style.display = 'block';
            TblSearch.style.display = "none";
            btncross.style.display = "block";

            cCmbScheme.SetValue("0");
            document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;
            document.getElementById('<%= txtVoucherNo.ClientID %>').value = "";
            document.getElementById('gridFilter').style.display = 'none';
            $('#<%= lblHeading.ClientID %>').text("Add Contra Voucher");


            grid.AddNewRow();
            cCmbScheme.Focus();

        }
        /*Mantis Issue:- 0024782 Swati Rev work Copy feature Add 25.03.2022*/
        function OnCopy(obj, BankValueDate) {            
            IsEdit = false;
            grid.GetEditor('Amount').SetEnabled(false);
            grid.GetEditor('Rate').SetEnabled(false);
            document.getElementById('divAddNew').style.display = 'block';
            TblSearch.style.display = "none";
            btncross.style.display = "block";            
            cCmbScheme.SetValue("0");
            
            document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;
            document.getElementById('<%= txtVoucherNo.ClientID %>').value = "";
            document.getElementById('gridFilter').style.display = 'none';
            $('#<%= lblHeading.ClientID %>').text("Add Contra Voucher");

            document.getElementById('hdnType').value = "Copy";
            var Type = "Copy";
            grid.AddNewRow();
            //cCmbScheme.Focus();
            var mode = obj.split("~");
            var id = mode[1];
            document.getElementById('htID').value = id;
            SetNumberingSchemeDataSource(id, Type);

            grid.PerformCallback('BEFORE_' + obj);
            
            document.getElementById('hdn_Mode').value = mode[0];
            document.getElementById('hdn_CashBankID').value = mode[1];
            var keyValue = mode[1];
           
            var txtVoucherNo = document.getElementById('<%= txtVoucherNo.ClientID %>');
                $(txtVoucherNo).attr('disabled', true);
                cCmbScheme.SetEnabled(true);

                document.getElementById('gridFilter').style.display = 'none';
                document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;
                document.getElementById('<%= txtVoucherNo.ClientID %>').value = "";
        }
        function SetNumberingSchemeDataSource(id, Type) {
           $.ajax({
                type: "POST",
                url: 'ContraVoucher.aspx/GetNumberingSchemeByType',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ id: id, Type: Type }),
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
        /*Close of Mantis Issue:- 0024782 Swati Rev work Copy feature Add 25.03.2022*/
        function AddExitMsgShow() {
            if (hdnBtnValue.value == "Exit") {
                if (grid.cprtnVoucherNo != null) {
                    var strconfirm = confirm(grid.cprtnVoucherNo);

                    jAlert(strconfirm, 'Alert Dialog: [ContraVoucher]', function (r) {
                        if (r == true) {
                            grid.cprtnVoucherNo = null;
                            hdnBtnValue.value == "";
                            window.location.reload();
                        }
                    });
                }
            }
        }
        function AddNewRowGrid() {

            if (grid.cprtnVoucherNo != null) {
                jAlert(grid.cprtnVoucherNo);
                grid.cprtnVoucherNo = null;
            }
            document.getElementById('divAddNew').style.display = 'block';
            TblSearch.style.display = "none";
            btncross.style.display = "block";
            $('#<%= lblHeading.ClientID %>').text("Add Contra Voucher");
            grid.AddNewRow();
            $("#hddn_BranchID").val(document.getElementById('ddlBranch').value);
            $("#hdnBranchIdTo").val(document.getElementById('ddlBranchTo').value);

        }
        function AddNewRowGridModify() {
            document.getElementById('divAddNew').style.display = 'block';
            TblSearch.style.display = "none";
            btncross.style.display = "block";
            var numbering = cCmbScheme.GetText();
            cCmbScheme.SetValue("0");
            document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;
            document.getElementById('<%= txtVoucherNo.ClientID %>').value = "";

            $('#<%= lblHeading.ClientID %>').text("Add Contra Voucher");
            grid.AddNewRow();
            cCmbScheme.Focus();


        }

        //Rev Debashis
        function zoomcontravoucher(keyid, docno) {
            IsView = true;
            document.getElementById('hdnType').value = "E";
            document.getElementById('divAddNew').style.display = 'block';
            TblSearch.style.display = "none";
            document.getElementById('btnAddExport').style.display = 'none';
            document.getElementById('divNumberingScheme').style.display = 'none';
            $('#lblHeading').text("View Contra Voucher");
            grid.AddNewRow();
            document.getElementById('gridFilter').style.display = 'none';
            document.getElementById('hdn_Mode').value = "VIEW";
            grid.PerformCallback('BEFORE_EDIT'+ '~'+ keyid);
            document.getElementById('btnSaveExit').style.display = 'none';
        }
        //End of Rev Debashis

        ////##### coded by Samrat Roy - 14/04/2017 - ref IssueLog(Contra - 77) 
        ////This method calls on View Request Only.
        function OnView(obj, BankValueDate) {

            if (BankValueDate != "") {
                jAlert("Voucher is Reconciled.Cannot View");
            }
            else {
                IsView = true;
                document.getElementById('hdnType').value = "E";
                document.getElementById('divAddNew').style.display = 'block';
                TblSearch.style.display = "none";
                btncross.style.display = "block";

                document.getElementById('divNumberingScheme').style.display = 'none';
                $('#<%= lblHeading.ClientID %>').text("View Contra Voucher");
                grid.AddNewRow();
                document.getElementById('gridFilter').style.display = 'none';
                var mode = obj.split("~");
                document.getElementById('hdn_Mode').value = "VIEW";
                document.getElementById('hdn_CashBankID').value = mode[1];
                grid.PerformCallback('BEFORE_' + obj);

                document.getElementById('btnSaveExit').style.display = 'none';
            }
        }
        // Coded By Samrat Roy -- 14/04/2017 -- refered by Issue Log Excel 

        function OnEdit(obj, BankValueDate) {            
            if (BankValueDate != "") {
                jAlert("Voucher is Reconciled.Cannot Edit");
            }
            else {
                IsEdit = true;
                document.getElementById('hdnType').value = "E";
                document.getElementById('divAddNew').style.display = 'block';
                TblSearch.style.display = "none";
                btncross.style.display = "block";

                document.getElementById('divNumberingScheme').style.display = 'none';
                $('#<%= lblHeading.ClientID %>').text("Modify Contra Voucher");
                grid.AddNewRow();

                grid.PerformCallback('BEFORE_' + obj);
                var mode = obj.split("~");
                /*Mantis Issue:- 0024782 Swati  Rev work Copy feature Add 25.03.2022*/
                var Type = "Edit";
                var id = mode[1];
                document.getElementById('htID').value = id;
                SetNumberingSchemeDataSource(id, Type);
                /*Mantis Issue:- 0024782 Swati  Close of Rev work Copy feature Add 25.03.2022 */
                document.getElementById('hdn_Mode').value = mode[0];
                document.getElementById('hdn_CashBankID').value = mode[1];
                var txtVoucherNo = document.getElementById('<%= txtVoucherNo.ClientID %>');
                $(txtVoucherNo).attr('disabled', true);
                cCmbScheme.SetEnabled(false);
                document.getElementById('gridFilter').style.display = 'none';
                cCmbScheme.SetValue("0");
                document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;
                document.getElementById('<%= txtVoucherNo.ClientID %>').value = "";
                //clookup_Project.SetEnabled(false);
                //if ($("#hdnProjectSelectInEntryModule").val() == "1")
                //    clookup_Project.gridView.Refresh();

            }
        }

        function OnAddNewClick() {
            grid.AddNewRow();
            document.getElementById("btnAddNew").style.display = 'none';
        }
        function OnComboInstTypeSelectedIndexChanged() {
            $('#MandatoryInstrumentType').hide();
            var InstType = cComboInstType.GetValue();
            if (InstType == "0") {
                document.getElementById("tdINoDiv").style.display = 'none';
                document.getElementById("tdIDateDiv").style.display = 'none';
            }
            else {

                document.getElementById("tdINoDiv").style.display = 'block';
                document.getElementById("tdIDateDiv").style.display = 'block';
            }
        }
        function AutoCalValue() {

            var Rate = (grid.GetEditor('Rate').GetValue() != null) ? grid.GetEditor('Rate').GetValue() : "0";
            var Amount = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
            if (Rate != "0" && Amount != "0") {
                var stramount = Rate * Amount;
                var PaymentAmount = grid.GetEditor("CashBankDetail_PaymentAmount");
                PaymentAmount.SetValue(stramount);
            }
            if (Rate == 0 && Amount != 0) {
                grid.GetEditor('CashBankDetail_PaymentAmount').SetValue(Amount)
            }

        }
        function ReceiptTextChange() {

            var Rate = (grid.GetEditor('Rate').GetValue() != null) ? grid.GetEditor('Rate').GetValue() : "0";
            var Amount = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
            if (Rate != 0 && Amount != 0) {
                grid.SetValue('CashBankDetail_PaymentAmount').SetValue(Rate * Amount)
            }
            if (Rate == 0 && Amount != 0) {
                grid.SetValue('CashBankDetail_PaymentAmount').SetValue(Amount)
            }
        }

        function AddContraLockStatus()
        {
            var LockDate = tDate.GetDate();
            $.ajax({
                type: "POST",
                url: "ContraVoucher.aspx/GetAddLock",
                data: JSON.stringify({ LockDate: LockDate }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async:false,
                success: function (msg) {
                    var currentRate = msg.d;
                    if (currentRate != null && currentRate=="-9") {
                        $("#hdnValAfterLock").val("-9");
                    }
                    else {
                        $("#hdnValAfterLock").val("1");
                    }
                    
                }
            });
        }

        function SaveNewButtonClick(s, e) {            
            LoadingPanel.Show();
            var WithDrawFrom = (grid.GetEditor('WithDrawFrom').GetValue() != null) ? grid.GetEditor('WithDrawFrom').GetValue() : "";
            var DepositInto = (grid.GetEditor('DepositInto').GetValue() != null) ? grid.GetEditor('DepositInto').GetValue() : "";
            var Currency_ID = (grid.GetEditor('Currency_ID').GetValue() != null) ? grid.GetEditor('Currency_ID').GetValue() : "";
            var AmountInHomeCurrency = (grid.GetEditor('CashBankDetail_PaymentAmount').GetValue() != null) ? grid.GetEditor('CashBankDetail_PaymentAmount').GetValue() : "0.00";

            var Remarks = (grid.GetEditor('Remarks').GetValue() != null) ? grid.GetEditor('Remarks').GetValue() : "";
            var Rate = (grid.GetEditor('Rate').GetValue() != null) ? grid.GetEditor('Rate').GetValue() : "";
            var Amount = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "";
            var ProjectId = (grid.GetEditor('ProjectId').GetValue() != null) ? grid.GetEditor('ProjectId').GetValue() : "";
            AddContraLockStatus();
            document.getElementById('hdnWithDrawFrom').value = "";
            document.getElementById('hdnDepositInto').value = "";
            document.getElementById('hdnCurrency_ID').value = "";
            document.getElementById('hdnAmountInHomeCurrency').value = "";
            document.getElementById('hdnRemarks').value = "";
            document.getElementById('hdnRate').value = "";
            document.getElementById('hdnAmount').value = "";
            document.getElementById('hdnInlineProjId').value = "0";
            document.getElementById('hdnWithDrawlLedgerName').value = "";
            document.getElementById('hdnDedpositLedgerName').value = "";
            

            document.getElementById('hdnWithDrawFrom').value = WithDrawFrom;
            document.getElementById('hdnDepositInto').value = DepositInto;
            document.getElementById('hdnCurrency_ID').value = Currency_ID;
            document.getElementById('hdnAmountInHomeCurrency').value = AmountInHomeCurrency;
            document.getElementById('hdnRemarks').value = Remarks;
            document.getElementById('hdnRate').value = Rate;
            document.getElementById('hdnAmount').value = Amount;//AmountInHomeCurrency;//Amount;
            document.getElementById('hdnInlineProjId').value = ProjectId;
            document.getElementById('hdnWithDrawlLedgerName').value = (grid.GetEditor('WithdrawlSubledgerId').GetValue() != null) ? grid.GetEditor('WithdrawlSubledgerId').GetValue() : "";
            document.getElementById('hdnDedpositLedgerName').value = (grid.GetEditor('DepositSubledgerId').GetValue() != null) ? grid.GetEditor('DepositSubledgerId').GetValue() : "";



            var type = document.getElementById('hdnType').value;

            var val = cCmbScheme.GetValue();
            var Branchval = $("#ddlBranch").val();
            var BranchvalTo = $("#ddlBranchTo").val();
            /*Mantis Issue:- 0024782 Swati  Rev work Copy feature Add 25.03.2022*/
            document.getElementById('hdnScemeID').vaue = val;
            /* Close of Mantis Issue:- 0024782 Swati  Rev work Copy feature Add 25.03.2022 */

            var voucherNo = document.getElementById('<%= txtVoucherNo.ClientID %>').value;
            document.getElementById('hdnVoucherNo').value = voucherNo;
            var InstType = cComboInstType.GetValue();


            var ProjectCode = clookup_Project.GetText();
            if ($("#hdnProjectSelectInEntryModule").val() == "1" && $("#hdnProjectMandatory").val() == "1" && ProjectCode == "") {
                jAlert("Please Select Project.");
                e.processOnServer = false;
               // cbtnexit.SetVisible(true);
                document.getElementById('btnSaveExit').style.display = 'block';
                LoadingPanel.Hide();
                return false;
            }
            if (val == "0") {               
                $("#MandatoryNumberingScheme").show();
                e.processOnServer = false;
                //cbtnexit.SetVisible(true);
                document.getElementById('btnSaveExit').style.display = 'block';
                LoadingPanel.Hide();
                return false;
            }
            else if (document.getElementById('<%= txtVoucherNo.ClientID %>').value.trim() == "") {
                $("#MandatoryBillNo").show();              
                e.processOnServer = false;
               // cbtnexit.SetVisible(true);
                document.getElementById('btnSaveExit').style.display = 'block';
                LoadingPanel.Hide();
                return false;
            }
            else if (Branchval == "0") {
                document.getElementById('<%= ddlBranch.ClientID %>').focus();               
                $("#MandatoryFromBranch").show();
                e.processOnServer = false;
                //cbtnexit.SetVisible(true);
                document.getElementById('btnSaveExit').style.display = 'block';
                LoadingPanel.Hide();
                return false;
            }
            else if (BranchvalTo == "0") {
                document.getElementById('<%= ddlBranch.ClientID %>').focus();              
                    $("#MandatoryBranchTo").show();
                    e.processOnServer = false;
                   // cbtnexit.SetVisible(true);
                    document.getElementById('btnSaveExit').style.display = 'block';
                    LoadingPanel.Hide();
                    return false;
                }
                else if (WithDrawFrom == "") {
                    jAlert("Please Fill Withdrawal From");
                    e.processOnServer = false;
                   // cbtnexit.SetVisible(true);
                    document.getElementById('btnSaveExit').style.display = 'block';
                    LoadingPanel.Hide();
                    return false;

                }
                else if (DepositInto == "") {
                    jAlert("Please Fill Deposit Into");
                    e.processOnServer = false;
                    //cbtnexit.SetVisible(true);
                    document.getElementById('btnSaveExit').style.display = 'block';
                    LoadingPanel.Hide();
                    return false;
                }
                else if (AmountInHomeCurrency == "0.00") {
                    jAlert("Please Enter Amount in Home Currency");
                    e.processOnServer = false;
                   // cbtnexit.SetVisible(true);
                    document.getElementById('btnSaveExit').style.display = 'block';
                    LoadingPanel.Hide();
                    return false;
                }
                else if (InstType == "NA") {
                    $("#MandatoryInstrumentType").show();                  
                    e.processOnServer = false;
                    //cbtnexit.SetVisible(true);
                    document.getElementById('btnSaveExit').style.display = 'block';
                    LoadingPanel.Hide();
                    return false;
                }
                else if ($("#hdnValAfterLock").val()=="-9")
                {
                    e.processOnServer = false;
                    //cbtnexit.SetVisible(true);
                    document.getElementById('btnSaveExit').style.display = 'block';
                    LoadingPanel.Hide();
                    jAlert("DATA is Freezed between   " + $("#hdnLockFromDateCon").val() + " to " + $("#hdnLockToDateCon").val() + " for Add.");
                    return false;
                }
            //cbtnexit.SetVisible(false);           
            document.getElementById('btnSaveExit').style.display = 'none';

}

function OnInit(s, e) {
    //IntializeGlobalVariables(s);
}
function OnEndCallback(s, e) {
    //IntializeGlobalVariables(s);
}


function CmbScheme_LostFocus() {    
        if ($("#hdnProjectSelectInEntryModule").val() == "1")
            clookup_Project.gridView.Refresh();  
}



function CmbScheme_ValueChange() {
    $("#MandatoryNumberingScheme").hide();
    var val = cCmbScheme.GetValue();

    if (val != "0") {
        $.ajax({
            type: "POST",
            url: 'ContraVoucher.aspx/getSchemeType',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{sel_scheme_id:\"" + val + "\"}",
            success: function (type) {
                var schemetypeValue = type.d;
                var schemetype = schemetypeValue.toString().split('~')[0];
                var schemelength = schemetypeValue.toString().split('~')[1];
                $('#txtVoucherNo').attr('maxLength', schemelength); 
                var branchID = schemetypeValue.toString().split('~')[2];

                //chinmoy add below code 14-03-2019

                var fromdate = schemetypeValue.toString().split('~')[3];
                var todate = schemetypeValue.toString().split('~')[4];

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




                //End
                $("#hddn_BranchID").val(branchID);
                //Rev work start Mantis Issue:- 0024782
                if ($('#hdnType').val() != "Copy") {
                    WithDrawFrom.PerformCallback(branchID);
                }
                //Rev work close Mantis Issue:- 0024782
                //DepositInto.PerformCallback(branchID);
                if (schemetypeValue != "") {

                    document.getElementById('ddlBranch').value = branchID;
                    document.getElementById('ddlBranchTo').value = branchID;
                    //document.getElementById('<%= ddlBranch.ClientID %>').disabled = true;

                    calWithdrawalBal();
                }
                if (schemetype == '0') {
                    document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = false;
                    document.getElementById('<%= txtVoucherNo.ClientID %>').value = "";
                }
                else if (schemetype == '1') {

                    document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;
                    document.getElementById('<%= txtVoucherNo.ClientID %>').value = "Auto";
                    document.getElementById('hdnScemeID').value = val;
                    $("#MandatoryBillNo").hide();
                }
                else {
                    document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;
                    document.getElementById('<%= txtVoucherNo.ClientID %>').value = "Datewise";
                }
            }
        });
}
else {
    document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;
        document.getElementById('<%= txtVoucherNo.ClientID %>').value = "";
    }


}

$(document).ready(function () {
    $('#CmbScheme_I').blur(function () {
        $("#MandatoryNumberingScheme").hide();
        var val = cCmbScheme.GetValue();

        if (val != "0") {
            $("#MandatoryNumberingScheme").hide();
        }
        else {
            cCmbScheme.Focus();
            $("#MandatoryNumberingScheme").show();
        }
    })

});


function ShowMsgLastCall() {

    if (gvContraVoucherInstance.cpCBDelete != null) {
        jAlert(gvContraVoucherInstance.cpCBDelete);
        gvContraVoucherInstance.cpCBDelete = null;
        updateGridByDate();

        //gvContraVoucherInstance.PerformCallback();
    }

}
function LastCall(obj) {
    if (grid.cpEdit != null) {
        var date = grid.cpEdit.split('~')[0];
        var VoucherNo = grid.cpEdit.split('~')[1];
        var Narration = grid.cpEdit.split('~')[2];
        var CashBank_BranchID = grid.cpEdit.split('~')[4];
        $("#hddn_BranchID").val(CashBank_BranchID);
        var CashBank_Currency = grid.cpEdit.split('~')[5];
        var CashBankDetail_InstrumentType = grid.cpEdit.split('~')[6];

        var CashBankDetail_InstrumentNumber = grid.cpEdit.split('~')[7];
        var Remarks = grid.cpEdit.split('~')[8];
        var CashBankDetail_InstrumentDate = grid.cpEdit.split('~')[9];
        var CashBank_IBRef = grid.cpEdit.split('~')[10];
        var CashBank_FinYear = grid.cpEdit.split('~')[11];
        var CashBank_CompanyID = grid.cpEdit.split('~')[12];
        var CashBank_ExchangeSegmentID = grid.cpEdit.split('~')[13];
        var CaskBank_NumberingScheme = grid.cpEdit.split('~')[14];

        var BranchIDTo = grid.cpEdit.split('~')[17];
        var projectIdForMo = grid.cpEdit.split('~')[18];
        var Type = grid.cpEdit.split('~')[19];



        var Transdt = new Date(date);
        tDate.SetDate(Transdt);
        document.getElementById('ddlBranch').value = CashBank_BranchID;
        document.getElementById('txtVoucherNo').value = VoucherNo;
        document.getElementById('hdnCashBank_IBRef').value = CashBank_IBRef;
        document.getElementById('hdnCashBank_FinYear').value = CashBank_FinYear;
        document.getElementById('hdnCashBank_CompanyID').value = CashBank_CompanyID;
        document.getElementById('hdnCashBank_ExchangeSegmentID').value = CashBank_ExchangeSegmentID;
        document.getElementById('hdnDbSaveCurrenct').value = CashBank_Currency;
        WithDrawFrom.PerformCallback(document.getElementById('ddlBranch').value);
        document.getElementById('ddlBranchTo').value = BranchIDTo;

        $("#hdnBranchIdTo").val(BranchIDTo);
        //DepositInto.PerformCallback(document.getElementById('ddlBranchTo').value);
        DepositInto.PerformCallback(document.getElementById('hdnBranchIdTo').value);
        var instDate = new Date(CashBankDetail_InstrumentDate);
        cInstDate.SetDate(instDate);
        if (CashBankDetail_InstrumentType == "0") {
            document.getElementById("tdINoDiv").style.display = 'none';
            document.getElementById("tdIDateDiv").style.display = 'none';
        }
        var setCurr = '<%=Session["LocalCurrency"]%>';
        var localCurrency = setCurr.split('~')[0];
        if (CashBank_Currency == localCurrency) {
            grid.GetEditor('Amount').SetEnabled(false);
            grid.GetEditor('Rate').SetEnabled(false);
        }
        var WithdrawalType = "";
        if (CashBankDetail_InstrumentType == "0") {
            WithdrawalType = "Cash";
        }
        WithdrawalChanged(WithdrawalType);
        cComboInstType.SetValue(CashBankDetail_InstrumentType);
        OnComboInstTypeSelectedIndexChanged();
        ctxtInstNo.SetText(CashBankDetail_InstrumentNumber);
        ctxtNarration.SetText(Narration);
        <%--Mantis Issue:- 0024782 Swati Rev work Copy feature Add 25.03.2022--%>
        //cCmbScheme.SetEnabled(false);
        if (Type == 'Copy') {
            cCmbScheme.SetEnabled(true);
            $('#hdn_Mode').val("Copy");
            $('#txtVoucherNo').val('Auto');
            cCmbScheme.Clear();
            cCmbScheme.SetValue(0);
            //cCmbScheme.SetValue(CaskBank_NumberingScheme);
        }
        else
        {
            cCmbScheme.SetEnabled(false);
            cCmbScheme.SetValue(CaskBank_NumberingScheme);
        }
        
        //cCmbScheme.SetValue(CaskBank_NumberingScheme);
        <%--Close of Mantis Issue:- 0024782 Swati Rev work Copy feature Add 25.03.2022--%>
        clookup_Project.gridView.SelectItemsByKey(projectIdForMo);

        document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;
        grid.StartEditRow(0);


        $('#<%= ddlBranch.ClientID %>').attr('disabled', true);
        tDate.SetEnabled(false);


        var projID = clookup_Project.GetValue();

        $.ajax({
            type: "POST",
            url: 'ContraVoucher.aspx/getHierarchyID',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ ProjID: projID }),
            success: function (msg) {
                var data = msg.d;
                $("#ddlHierarchy").val(data);
            }
        });

        ////##### coded by Samrat Roy - 14/04/2017 - ref IssueLog(Contra - 77) 
        ////This condition is working on View Request Only.
        if ($('#hdn_Mode').val().toUpperCase() == 'VIEW') {
            viewOnly();
        }
    }
    // alert($("#hdnBtnValue").val());

    Discard_dipositinto();
}
////##### coded by Samrat Roy - 14/04/2017 - ref IssueLog(Contra - 77) 
////This method is for disable all the attributes.
function viewOnly() {
    $('#<%= txtVoucherNo.ClientID %>').attr('disabled', true);
    $('#<%= ddlBranch.ClientID %>').attr('disabled', true);
    $('#<%= txtNarration.ClientID %>').attr('disabled', true);

    grid.SetEnabled(false);
    tDate.SetEnabled(false);

    cComboInstType.SetEnabled(false);
    cInstDate.SetEnabled(false);
    ctxtNarration.SetEnabled(false);
    ctxtInstNo.SetEnabled(false);

    cbtnnew.SetVisible(false);
    cbtnexit.SetVisible(false);
    calWithdrawalBal();
}

function chkValidConta(contano_status) {
    if (contano_status == "outrange") {
        jAlert('Can Not Add More Contra Voucher as Contra Scheme Exausted.<br />Update The Scheme and Try Again');
    } else if (contano_status == "duplicate") {
        jAlert('Can Not Save as Duplicate Contra Voucher No. Found');
    }
    return false;
}


function gridRowclick(s, e) {
    //alert('hi');
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


    </script>
    <style>
        #grid_DXStatus.statusBar a {
            visibility: hidden;
        }

        .gridcellleft.dx-nowrap.dxgv, .gridcellleft.dxgv, .dxgv.dx-al, .dxgv.dx-ar, .dxgv.dx-ac {
            display: table-cell !important;
        }

        .dxgv.dx-al, .dxgv.dx-ar, .dx-nowrap.dxgv, .gridcellleft.dxgv, .dxgv.dx-ac, .dxgvCommandColumn_PlasticBlue.dxgv.dx-ac {
            display: table-cell !important;
        }

        .dxgvControl_PlasticBlue td.dxgvBatchEditModifiedCell_PlasticBlue {
            background: #fff !important;
        }

        .voucherno {
            position: absolute;
            right: 22px;
            top: 37px;
        }

        .FromBranch {
            position: absolute;
            right: 22px;
            top: 37px;
        }

        .BranchTo {
            position: absolute;
            right: 1px;
            top: 25px;
        }

        .iconInstrumentType {
            position: absolute;
            right: -2px;
            top: 35px;
        }

        .iconNumberScheme {
            position: absolute;
            right: -2px;
            top: 34px;
        }

        #Grid_ContraVoucher_DXFilterRow .dxgv {
            display: table-cell !important;
        }

        #Grid_ContraVoucher_DXGroupRow0.dxgvGroupRow_PlasticBlue td.dxgv {
            display: table-cell !important;
        }

        .padTabtype2 > tbody > tr > td {
            padding: 0px 5px;
        }

            .padTabtype2 > tbody > tr > td > label {
                margin-bottom: 0 !important;
                margin-right: 15px;
            }
    </style>
    <script type="text/javascript">
        var isCtrl = false;
        var currentval = '';
        function NumberingScheme_GotFocus() {
            cCmbScheme.ShowDropDown();
        }
        document.onkeyup = function (e) {
            if (event.keyCode == 17) {
                isCtrl = false;
            }
            else if (event.keyCode == 27) {
                btnCancel_Click();
            }
        }

        document.onkeydown = function (e) {
            // if (event.keyCode == 17) altKey = true;

            if (event.keyCode == 83 && event.altKey == true) {
                document.getElementById('btnnew').click();
                return false;
            }
            else if ((event.keyCode == 120 || event.keyCode == 88) && event.altKey == true) {
             
                if (document.getElementById('btnSaveExit').style.display != 'none') {
                    document.getElementById('btnSaveExit').click();
                    return false;
                }
                   
               
            }
            else if ((event.keyCode == 120 || event.keyCode == 65) && event.altKey == true) {
                //run code for Ctrl+A -- ie, Add New
                if (document.getElementById('divAddNew').style.display != 'block') {
                    OnAddButtonClick();
                }
            }
        }

        function btnCancel_Click() {
            jConfirm('Do you Want To Close This Window?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    //parent.editwin.close();
                    window.location.reload();
                }
            })
        }
        function Discard_dipositinto() {
            var BranchFor = grid.GetEditor('WithDrawFrom').GetValue();
            var DepositInto = grid.GetEditor('DepositInto').GetValue();

            var i = 0
            for (i = 0; i < grid.GetEditor('DepositInto').GetItemCount() ; i++) {
                var DepositInto = grid.GetEditor('DepositInto').GetItem(i).value;
                if (BranchFor == DepositInto) {
                    grid.GetEditor('DepositInto').RemoveItem(i);
                    break
                }
            }


            if (currentval != '' && currentval != 0) {
                for (i = 0; i < grid.GetEditor('WithDrawFrom').GetItemCount() ; i++) {
                    var BranchForVal = grid.GetEditor('WithDrawFrom').GetItem(i).value;
                    var BranchForText = grid.GetEditor('WithDrawFrom').GetItem(i).text;
                    if (currentval == BranchForVal) {
                        //var option = document.createElement("option");
                        //option.text = BranchForText;
                        //option.value = BranchForVal;
                        grid.GetEditor('DepositInto').AddItem(BranchForText, BranchForVal, null);
                        break
                    }
                }
            }

            currentval = BranchFor;
        }
        function Discard_WithDrawFrom() {
            var BranchFor = grid.GetEditor('WithDrawFrom').GetValue();
            var DepositInto = grid.GetEditor('DepositInto').GetValue();

            var i = 0
            for (i = 0; i < grid.GetEditor('WithDrawFrom').GetItemCount() ; i++) {
                var BranchFor = grid.GetEditor('WithDrawFrom').GetItem(i).value;
                if (BranchFor == DepositInto) {
                    grid.GetEditor('WithDrawFrom').RemoveItem(i);
                    break
                }
            }


            if (currentval != '' && currentval != 0) {
                for (i = 0; i < grid.GetEditor('DepositInto').GetItemCount() ; i++) {
                    var BranchForVal = grid.GetEditor('DepositInto').GetItem(i).value;
                    var BranchForText = grid.GetEditor('DepositInto').GetItem(i).text;
                    if (currentval == BranchForVal) {

                        grid.GetEditor('WithDrawFrom').AddItem(BranchForText, BranchForVal, null);
                        break
                    }
                }
            }

            currentval = DepositInto;
        }
        function PopulateCurrentBankBalance(MainAccountID, BranchId) {
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
                            document.getElementById('<%=B_ImgSymbolBankBal.ClientID %>').innerHTML = ActiveCurrencySymbol;
                            document.getElementById('<%=B_BankBalance.ClientID %>').innerHTML = msg.d.split('~')[0];
                            //  document.getElementById('<%=B2.ClientID %>').innerHTML = '0.0';
                            <%--document.getElementById('<%=B_BankBalance.ClientID %>').style.color = msg.d.split('~')[1];--%>
                            document.getElementById('<%=B_BankBalance.ClientID %>').style.color = "Black";
                        }
                        else {
                            document.getElementById('<%=B_ImgSymbolBankBal.ClientID %>').innerHTML = '';
                            document.getElementById('<%=B_BankBalance.ClientID %>').innerHTML = '0.0';
                            //  document.getElementById('<%=B2.ClientID %>').innerHTML = '0.0';
                            document.getElementById('<%=B_BankBalance.ClientID %>').style.color = "Black";

                        }
                    }

                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    jAlert(textStatus);
                }
            });

        }
        function PopulateCurrentBankBalanceForDeposit(MainAccountID, strBranch) {
            $.ajax({
                type: "POST",
                url: 'ContraVoucher.aspx/GetCurrentBankBalance',
                data: "{MainAccountID:\"" + MainAccountID + "\",Branch:\"" + strBranch + "\"}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var list = msg.d;

                    if (msg.d.length > 0) {
                        document.getElementById("pageheaderContentdeposit").style.display = 'block';
                        if (msg.d.split('~')[0] != '') {
                            document.getElementById('<%=B1.ClientID %>').innerHTML = ActiveCurrencySymbol;
                            document.getElementById('<%=B2.ClientID %>').innerHTML = msg.d.split('~')[0];
                            <%--document.getElementById('<%=B_BankBalance.ClientID %>').style.color = msg.d.split('~')[1];--%>
                            document.getElementById('<%=B2.ClientID %>').style.color = "Black";
                        }
                        else {
                            document.getElementById('<%=B1.ClientID %>').innerHTML = '';
                            document.getElementById('<%=B2.ClientID %>').innerHTML = '0.0';
                            document.getElementById('<%=B2.ClientID %>').style.color = "Black";

                        }
                    }

                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    jAlert(textStatus);
                }
            });
        }
        function Deposit_SelectedIndexChanged() {
            // var strBranch = document.getElementById('ddlBranch').value;
            // DepositInto.PerformCallback(strBranch);
            Discard_WithDrawFrom();
            var DepositInto = grid.GetEditor('DepositInto').GetValue();
            if (DepositInto != null) {
                var arr = DepositInto.split('-');
                var strBranch = document.getElementById('ddlBranch').value;
                PopulateCurrentBankBalanceForDeposit(arr[0], strBranch);
            }
          

        }
        function calWithdrawalBal() {
            var WithDrawValue = grid.GetEditor('WithDrawFrom').GetText();
            if (WithDrawValue != "") {
                var strBranch = document.getElementById('ddlBranch').value;
                var arr = WithDrawValue.split('-');
                PopulateCurrentBankBalance(arr[0], strBranch);
                Deposit_SelectedIndexChanged();
            }

        }

        function Withdrawal_LostFocus()
        {
           
            var WithDrawllll = grid.GetEditor('WithDrawFrom').GetValue();
            if ($("#hdnSubledgerCashBankType").val() == "1") {
                $.ajax({
                    type: "POST",
                    url: "ContraVoucher.aspx/getWithdrawlValue",
                    data: JSON.stringify({ WithDrawValue: WithDrawllll }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var data = msg.d;

                        if ((data == "None") || (data == "") || (data == null)) {
                            grid.GetEditor('WithdrawlSubLedger').SetEnabled(false);
                        }
                        else {
                            grid.GetEditor('WithdrawlSubLedger').SetEnabled(true);
                        }
                    }
                });

            }
        }



        function DepositInto_LostFocus()
        {
            var DepositIntoVal = grid.GetEditor('DepositInto').GetValue();

            if ($("#hdnSubledgerCashBankType").val() == "1") {
                $.ajax({
                    type: "POST",
                    url: "ContraVoucher.aspx/getWithdrawlValue",
                    data: JSON.stringify({ WithDrawValue: DepositIntoVal }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var data = msg.d;

                        if ((data == "None") || (data == "") || (data == null)) {
                            grid.GetEditor('DepositSubLedger').SetEnabled(false);
                        }
                        else {
                            grid.GetEditor('DepositSubLedger').SetEnabled(true);
                        }
                    }
                });

            }
        }


        //function WithDrawl_LostFocus()
        //{
        //    if($("#hdnSubledgerCashBankType")=="1")
        //    {
        //        grid.batchEditApi.StartEdit(globalRowIndex);
        //        grid.batchEditApi.StartEdit(globalRowIndex,1);
        //    }
        //}


        function Withdrawal_SelectedIndexChanged() {
            // var strBranch = document.getElementById('ddlBranch').value;
            // WithDrawFrom.PerformCallback(strBranch);
            Discard_dipositinto();
            var strBranch = document.getElementById('ddlBranch').value;
            var WithDrawValue = grid.GetEditor('WithDrawFrom').GetText();
            var arr = WithDrawValue.split('-');
            PopulateCurrentBankBalance(arr[0], strBranch);
           

            var SpliteDetails = WithDrawValue.split("]");
            var WithDrawType = String(SpliteDetails[1]).trim();

            var WithDrawllll = grid.GetEditor('WithDrawFrom').GetValue();
            if ($("#hdnSubledgerCashBankType").val() == "1") {
                $.ajax({
                    type: "POST",
                    url: "ContraVoucher.aspx/getWithdrawlValue",
                    data: JSON.stringify({ WithDrawValue: WithDrawllll }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var data = msg.d;

                        if ((data == "None") || (data == "") || (data == null)) {
                            grid.GetEditor('WithdrawlSubLedger').SetEnabled(false);
                        }
                        else {
                            grid.GetEditor('WithdrawlSubLedger').SetEnabled(true);
                        }
                    }
                });

            }

            if ($("#hdnProjectSelectInEntryModule").val() == "1") {
                if ((clookup_Project.GetGridView().GetRowKey(clookup_Project.GetGridView().GetFocusedRowIndex()) != null)) {

                    var ProjectLookUpData = clookup_Project.GetGridView().GetRowKey(clookup_Project.GetGridView().GetFocusedRowIndex());
                    grid.GetEditor("Project_Code").SetText(clookup_Project.GetValue());
                    grid.GetEditor("ProjectId").SetText(ProjectLookUpData);
                }
            }



            if (WithDrawType == "Cash") {
                var comboitem = cComboInstType.FindItemByValue('C');
                if (comboitem != undefined && comboitem != null) {
                    cComboInstType.RemoveItem(comboitem.index);

                    //cComboInstType.SetValue("NA");
                    //OnComboInstTypeSelectedIndexChanged();
                }
                var comboitem = cComboInstType.FindItemByValue('D');
                if (comboitem != undefined && comboitem != null) {
                    cComboInstType.RemoveItem(comboitem.index);

                    // cComboInstType.SetValue("NA");
                    // OnComboInstTypeSelectedIndexChanged();
                }
                var comboitem = cComboInstType.FindItemByValue('E');
                if (comboitem != undefined && comboitem != null) {
                    cComboInstType.RemoveItem(comboitem.index);

                    //cComboInstType.SetValue("NA");
                    // OnComboInstTypeSelectedIndexChanged();
                }
                var comboitem = cComboInstType.FindItemByValue('0');
                if (comboitem == undefined || comboitem == null) {
                    cComboInstType.AddItem("Cash", "0");
                }
                cComboInstType.SetValue("0");
                OnComboInstTypeSelectedIndexChanged();
            }
            else {
                var comboitem = cComboInstType.FindItemByValue('C');
                if (comboitem == undefined || comboitem == null) {
                    cComboInstType.AddItem("Cheque", "C");
                }
                var comboitem = cComboInstType.FindItemByValue('D');
                if (comboitem == undefined || comboitem == null) {
                    cComboInstType.AddItem("Draft", "D");
                }
                var comboitem = cComboInstType.FindItemByValue('E');
                if (comboitem == undefined || comboitem == null) {
                    cComboInstType.AddItem("E.Transfer", "E");
                }
                var comboitem = cComboInstType.FindItemByValue('0');
                if (comboitem != undefined && comboitem != null) {
                    cComboInstType.RemoveItem(comboitem.index);

                    cComboInstType.SetValue("C");
                    OnComboInstTypeSelectedIndexChanged();
                }
            }
           // clookup_Project.SetEnabled(false);
        }
        function WithdrawalChanged(WithDrawType) {

            if (WithDrawType == "Cash") {
                var comboitem = cComboInstType.FindItemByValue('C');
                if (comboitem != undefined && comboitem != null) {
                    cComboInstType.RemoveItem(comboitem.index);

                    // cComboInstType.SetValue("NA");
                    // OnComboInstTypeSelectedIndexChanged();
                }
                var comboitem = cComboInstType.FindItemByValue('D');
                if (comboitem != undefined && comboitem != null) {
                    cComboInstType.RemoveItem(comboitem.index);

                    // cComboInstType.SetValue("NA");
                    // OnComboInstTypeSelectedIndexChanged();
                }
                var comboitem = cComboInstType.FindItemByValue('E');
                if (comboitem != undefined && comboitem != null) {
                    cComboInstType.RemoveItem(comboitem.index);

                    //cComboInstType.SetValue("NA");
                    // OnComboInstTypeSelectedIndexChanged();
                }
                var comboitem = cComboInstType.FindItemByValue('0');
                if (comboitem == undefined || comboitem == null) {
                    cComboInstType.AddItem("Cash", "0");
                }
                cComboInstType.SetValue("0");
                OnComboInstTypeSelectedIndexChanged();
            }
            else {
                var comboitem = cComboInstType.FindItemByValue('C');
                if (comboitem == undefined || comboitem == null) {
                    cComboInstType.AddItem("Cheque", "C");
                }
                var comboitem = cComboInstType.FindItemByValue('D');
                if (comboitem == undefined || comboitem == null) {
                    cComboInstType.AddItem("Draft", "D");
                }
                var comboitem = cComboInstType.FindItemByValue('E');
                if (comboitem == undefined || comboitem == null) {
                    cComboInstType.AddItem("E.Transfer", "E");
                }
                var comboitem = cComboInstType.FindItemByValue('0');
                if (comboitem != undefined && comboitem != null) {
                    cComboInstType.RemoveItem(comboitem.index);

                    cComboInstType.SetValue("C");
                    OnComboInstTypeSelectedIndexChanged();
                }
            }
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                grid.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                grid.SetWidth(cntWidth);
            }
            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    grid.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    grid.SetWidth(cntWidth);
                }

            });
        });
    </script>
    <script>
        function ProjectCodeSelected(s, e) {
          
            if (clookupPopup_ProjectCode.GetGridView().GetFocusedRowIndex() == -1) {
                cProjectCodePopup.Hide();

                return;
            }
            var ProjectInlineLookUpData = clookupPopup_ProjectCode.GetGridView().GetRowKey(clookupPopup_ProjectCode.GetGridView().GetFocusedRowIndex());
            var ProjectInlinedata = ProjectInlineLookUpData.split('~')[0];
            grid.batchEditApi.StartEdit(globalRowIndex);
            var ProjectCode = clookupPopup_ProjectCode.GetValue();
            cProjectCodePopup.Hide();

            grid.GetEditor("Project_Code").SetText(ProjectCode);
            grid.GetEditor("ProjectId").SetText(ProjectInlinedata);

        }

        function lookup_ProjectCodeKeyDown(s, e) {
            if (e.htmlEvent.key == "Escape") {
                cProjectCodePopup.Hide();

            }
        }


        function ProjectCodeCallback_endcallback() {

            clookupPopup_ProjectCode.ShowDropDown();;
            clookupPopup_ProjectCode.Focus();
            clookupPopup_ProjectCode.Clear()

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

        
        function GetVisibleIndexContra(s, e) {
            globalRowIndex = e.visibleIndex;
            // EnableOrDisableTax();
        }
        $('#SubledgerModel').on('shown.bs.modal', function () {
            setTimeout(function () {
                $('#txtSubledgerSearch').focus();
            }, 200);
            
        });

        function SubledgerButnClick(s, e) {
            $('#SubledgerModel').modal('show');
            setTimeout(function () {
                $('#txtSubledgerSearch').focus();
            }, 1000);
        }

        
        function DepositSubledgerButnClick(s, e) {
            $('#DepositSubledgerModel').modal('show');
            setTimeout(function () {
                $('#txtDepositSubledgerSearch').focus();
            }, 1000);
           
        }

        function SubledgerDown(s, e) {
            if (e.htmlEvent.key == "Enter") {
                $('#SubledgerModel').modal('show');
               
            }
            setTimeout(function () {
                $('#txtSubledgerSearch').focus();
            }, 1000);
        }

        function DepositSubledgerDown(s, e) {
            if (e.htmlEvent.key == "Enter") {
                $('#DepositSubledgerModel').modal('show');
                
            }
            setTimeout(function () {
                $('#txtDepositSubledgerSearch').focus();
            }, 1000);
        }


        function Subledgermodkeydown(e) {
          
            var WithDrawFrom = (grid.GetEditor('WithDrawFrom').GetValue() != null) ? grid.GetEditor('WithDrawFrom').GetValue() : "";
            var OtherDetails = {}
            OtherDetails.SearchKey = $("#txtSubledgerSearch").val();
            OtherDetails.AccountType = WithDrawFrom;
            if (e.code == "Enter" || e.code == "NumpadEnter") {
               
                var HeaderCaption = [];
                HeaderCaption.push("Name");
                HeaderCaption.push("Unique Id");
                HeaderCaption.push("Address");
                if ($("#txtSubledgerSearch").val() != null && $("#txtSubledgerSearch").val() != "") {
                    //gridquotationLookup.SetEnabled(false);
                   // $('input[type=radio]').prop('checked', false);
                    callonServer("ContraVoucher.aspx/GetCustomer", OtherDetails, "LedgerTable", HeaderCaption, "customerIndex", "SetLedger");
                }
            }
            else if (e.code == "ArrowDown") {
                if ($("input[customerindex=0]"))
                    $("input[customerindex=0]").focus();
            }
            else if (e.code == "Escape") {
                //ctxtCustName.Focus();
            }
        }

        function DepositSubledgermodkeydown(e) {

            var WithDrawFrom = (grid.GetEditor('DepositInto').GetValue() != null) ? grid.GetEditor('DepositInto').GetValue() : "";
            var OtherDetails = {}
            OtherDetails.SearchKey = $("#txtDepositSubledgerSearch").val();
            OtherDetails.AccountType = WithDrawFrom;
            if (e.code == "Enter" || e.code == "NumpadEnter") {
                
                var HeaderCaption = [];
                HeaderCaption.push("Name");
                HeaderCaption.push("Unique Id");
                HeaderCaption.push("Address");
                if ($("#txtDepositSubledgerSearch").val() != null && $("#txtDepositSubledgerSearch").val() != "") {
                    //gridquotationLookup.SetEnabled(false);
                    // $('input[type=radio]').prop('checked', false);
                    callonServer("ContraVoucher.aspx/GetCustomer", OtherDetails, "DepositLedgerTable", HeaderCaption, "customerdepositIndex", "SetDepositLedger");
                }
            }
            else if (e.code == "ArrowDown") {
                if ($("input[customerdepositIndex=0]"))
                    $("input[customerdepositIndex=0]").focus();
            }
            else if (e.code == "Escape") {
                //ctxtCustName.Focus();
            }
        }


        function SetLedger(Id, Name) {

            var key = Id;
            if (key != null && key != '') {
                $('#SubledgerModel').modal('hide');
                grid.batchEditApi.StartEdit(globalRowIndex);
                grid.GetEditor("WithdrawlSubLedger").SetText(Name);
                grid.GetEditor("WithdrawlSubledgerId").SetText(Id);
                $("#hdnWithDrawlLedgerName").val(Id);


            }
            //setTimeout(function () {
            //    grid.batchEditApi.StartEdit(globalRowIndex, 2);
            //}, 200);
          
        }


        function SetDepositLedger(Id, Name) {

            var key = Id;
            if (key != null && key != '') {
                $('#DepositSubledgerModel').modal('hide');
                grid.batchEditApi.StartEdit(globalRowIndex);
                grid.GetEditor("DepositSubLedger").SetText(Name);
                grid.GetEditor("DepositSubledgerId").SetText(Id);
                $("#hdnDedpositLedgerName").val(Id);
            }
        }

        function ValueSelected(e, indexName) {
            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var Id = e.target.parentElement.parentElement.cells[0].innerText;
                var name = e.target.parentElement.parentElement.cells[1].children[0].value;
                if (Id) {
                    if (indexName == "customerIndex") {
                        SetLedger(Id, name);
                    }
                    else if (indexName == "customerdepositIndex") {
                        SetDepositLedger(Id, name);
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
                        $('#txtSubledgerSearch').focus();
                    else if(indexName == "customerdepositIndex")
                        $('#txtDepositSubledgerSearch').focus();
                }
            }
        }


    </script>

    <style>
        .dxeErrorFrameSys.dxeErrorCellSys {
            position: absolute;
        }
        /*#grid_DXMainTable>tbody>tr>td:last-child {
            display:none !important;
        }*/

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title clearfix">
            <h3 class="pull-left"><span class="pull-left">
                <asp:Label ID="lblHeading" runat="server" Text="Contra Voucher"></asp:Label></span>

            </h3>

            <div id="pageheaderContent" class="pull-right wrapHolder reverse content horizontal-images" style="display: none;">
                <ul>
                    <li>
                        <div class="lblHolder">
                            <table>
                                <tbody>
                                    <tr>
                                        <td>Current Balance For Withdrawal </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div style="width: 100%;">
                                                <b style="text-align: left" id="B_ImgSymbolBankBal" runat="server"></b>
                                                <b style="text-align: center" id="B_BankBalance" runat="server">0.0</b>
                                            </div>

                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </li>
                    <li>
                        <div class="lblHolder" id="pageheaderContentdeposit">
                            <table>
                                <tbody>
                                    <tr>
                                        <td>Current Balance For Deposit </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div style="width: 100%;">
                                                <b style="text-align: left" id="B1" runat="server"></b>
                                                <b style="text-align: center" id="B2" runat="server">0.0</b>
                                            </div>

                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </li>

                </ul>
            </div>

            <div id="btncross" runat="server" class="crossBtn" style="display: none; margin-left: 50px;"><a href="ContraVoucher.aspx"><i class="fa fa-times"></i></a></div>
        </div>
    </div>
    <div class="form_main">
        <div class="clearfix">
            <div style="float: left; padding-right: 5px;" id="btnAddExport">
                <% if (rights.CanAdd)
                   { %>

                <a href="javascript:void(0);" onclick="OnAddButtonClick()" class="btn btn-success btn-radius"><span class="btn-icon"><i class="fa fa-plus"></i></span><span><u>A</u>dd New</span> </a>
                <% } %>
                <% if (rights.CanExport)
                   { %>
                <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-radius" OnSelectedIndexChanged="drdExport_SelectedIndexChanged" AutoPostBack="true">
                    <asp:ListItem Value="0">Export to</asp:ListItem>
                    <asp:ListItem Value="1">PDF</asp:ListItem>
                    <asp:ListItem Value="2">XLS</asp:ListItem>
                    <asp:ListItem Value="3">RTF</asp:ListItem>
                    <asp:ListItem Value="4">CSV</asp:ListItem>
                </asp:DropDownList>
                <% } %>
            </div>
            <table class="padTabtype2 pull-right" id="gridFilter">
                <tr>
                    <td>
                        <label>From Date</label></td>
                    <td>&nbsp;</td>
                    <td>
                        <dxe:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDate" Width="100%">
                            <ButtonStyle Width="13px">
                            </ButtonStyle>
                        </dxe:ASPxDateEdit>
                    </td>
                    <td>&nbsp;</td>
                    <td>
                        <label>To Date</label>
                    </td>
                    <td>&nbsp;</td>
                    <td>
                        <dxe:ASPxDateEdit ID="toDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDate" Width="100%">
                            <ButtonStyle Width="13px">
                            </ButtonStyle>
                        </dxe:ASPxDateEdit>
                    </td>
                    <td>&nbsp;</td>
                    <td>Unit</td>
                    <td>
                        <dxe:ASPxComboBox ID="cmbBranchfilter" runat="server" ClientInstanceName="ccmbBranchfilter" Width="100%">
                        </dxe:ASPxComboBox>
                    </td>
                    <td>&nbsp;</td>
                    <td>
                        <input type="button" value="Show" class="btn btn-primary" onclick="updateGridByDate()" />
                    </td>

                </tr>

            </table>
        </div>
        <div id="divAddNew" style="display: none" runat="server">
            <div style="background: #f5f4f3; padding: 8px 0; margin-bottom: 15px; border-radius: 4px; border: 1px solid #ccc;" class="clearfix">
                <div class="col-md-2" id="divNumberingScheme">
                    <label style="">Select Numbering Scheme <span style="color: red">*</span></label>
                    <div>
                        <dxe:ASPxComboBox ID="CmbScheme" EnableIncrementalFiltering="true" ClientInstanceName="cCmbScheme"
                            TextField="SchemaName" ValueField="Id"
                            runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True">
                            <ClientSideEvents ValueChanged="function(s,e){CmbScheme_ValueChange()}" GotFocus="NumberingScheme_GotFocus" LostFocus="CmbScheme_LostFocus"></ClientSideEvents>
                        </dxe:ASPxComboBox>
                        <span id="MandatoryNumberingScheme" class="iconNumberScheme pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>

                    </div>
                </div>
                <div class="col-md-2">
                    <label>Document No. <span style="color: red">*</span></label>
                    <div>
                        <asp:TextBox ID="txtVoucherNo" runat="server" Width="100%" MaxLength="30" onchange="txtBillNo_TextChanged()">                             
                        </asp:TextBox>
                        <span id="MandatoryBillNo" class="voucherno  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                        <span id="MandatoryBillNoDUPLICALE" class="voucherno  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Duplicate Voucher No."></span>
                        <%-- <asp:RequiredFieldValidator Display="Dynamic" runat="server" ID="RequiredFieldValidator2" ControlToValidate="txtVoucherNo"
                       SetFocusOnError="true" ErrorMessage="" class="pullrightClass fa fa-exclamation-circle abs iconRed" ToolTip="Mandatory" ValidationGroup="branchgrp" >                                                        
                        </asp:RequiredFieldValidator>--%>
                    </div>
                </div>
                <div class="col-md-2">
                    <label>Posting Date</label>
                    <div>
                        <dxe:ASPxDateEdit ID="tDate" runat="server" EditFormat="Custom" ClientInstanceName="tDate" DisplayFormatString="dd-MM-yyyy"
                            UseMaskBehavior="True" Width="100%" EditFormatString="dd-MM-yyyy">
                            <%-- <ClientSideEvents DateChanged="function(s,e){DateChange()}" />--%>
                            <%-- <ClientSideEvents Init="function(s,e){ s.SetDate(new Date());}" />--%>
                            <ClientSideEvents DateChanged="function(s,e){TDateChange();}" GotFocus="function(s,e){tDate.ShowDropDown();}" LostFocus="function(s, e) { SetLostFocusonDemand(e)}" ></ClientSideEvents>
                            <ValidationSettings RequiredField-IsRequired="true"></ValidationSettings>
                        </dxe:ASPxDateEdit>
                    </div>
                </div>
                <div class="col-md-3">

                    <label style="margin-top: 0">From Unit <span style="color: red">*</span></label>
                    <div>
                        <%-- <asp:DropDownList ID="ddlBranch" runat="server" DataSourceID="dsBranch" onchange="BranchFrom_SelectedIndexChanged()"
                            DataTextField="BANKBRANCH_NAME" DataValueField="BANKBRANCH_ID" Width="100%">
                        </asp:DropDownList>--%>
                        <asp:DropDownList ID="ddlBranch" runat="server" onchange="BranchFrom_SelectedIndexChanged()"
                            DataTextField="BANKBRANCH_NAME" DataValueField="BANKBRANCH_ID" Width="100%">
                        </asp:DropDownList>
                        <span id="MandatoryFromBranch" class="FromBranch  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                    </div>
                </div>
                <div class="col-md-3">

                    <label style="margin-top: 0">To Unit <span style="color: red">*</span></label>
                    <div>
                        <%-- <asp:DropDownList ID="ddlBranchTo" runat="server" DataSourceID="dsBranch"
                            DataTextField="BANKBRANCH_NAME" DataValueField="BANKBRANCH_ID" Width="100%" onchange="batchgridFocus()">
                        </asp:DropDownList>--%>
                        <asp:DropDownList ID="ddlBranchTo" runat="server"
                            DataTextField="BANKBRANCH_NAME" DataValueField="BANKBRANCH_ID" Width="100%" onchange="batchgridFocus()" onblur="ddlBranchTo_LostFocus()">
                        </asp:DropDownList>
                        <span id="MandatoryBranchTo" class="BranchTo  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                    </div>
                </div>
                <div class="col-md-3">
                    <label id="lblProject" runat="server" style="margin-top: 0">Project </label>
                    <div>
                        <dxe:ASPxGridLookup ID="lookup_Project" runat="server" ClientInstanceName="clookup_Project" DataSourceID="EntityServerModeDataSourceProject"
                            KeyFieldName="Proj_Id" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" >
                            <Columns>
                                <dxe:GridViewDataColumn FieldName="Proj_Code" Visible="true" VisibleIndex="0" Caption="Project Code" Settings-AutoFilterCondition="Contains" Width="200px">
                                </dxe:GridViewDataColumn>
                                <dxe:GridViewDataColumn FieldName="Proj_Name" Visible="true" VisibleIndex="1" Caption="Project Name" Settings-AutoFilterCondition="Contains" Width="200px">
                                </dxe:GridViewDataColumn>
                                <dxe:GridViewDataColumn FieldName="Customer" Visible="true" VisibleIndex="2" Caption="Customer" Settings-AutoFilterCondition="Contains" Width="200px">
                                </dxe:GridViewDataColumn>
                                <dxe:GridViewDataColumn FieldName="HIERARCHY_NAME" Visible="true" VisibleIndex="3" Caption="Hierarchy" Settings-AutoFilterCondition="Contains" Width="200px">
                                </dxe:GridViewDataColumn>
                            </Columns>
                            <GridViewProperties Settings-VerticalScrollBarMode="Auto">
                                <Templates>
                                    <StatusBar>
                                        <table class="OptionsTable" style="float: right">
                                            <tr>
                                                <td></td>
                                            </tr>
                                        </table>
                                    </StatusBar>
                                </Templates>

                                <SettingsBehavior AllowFocusedRow="True" AllowSelectSingleRowOnly="True"></SettingsBehavior>
                                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                            </GridViewProperties>
                           <%-- <ClientSideEvents GotFocus="function(s,e){clookup_Project.ShowDropDown();}" LostFocus="clookup_Project_LostFocus" ValueChanged="ProjectValueChange" />--%>

                           <%-- <ClearButton DisplayMode="Always">
                            </ClearButton>--%>
                        </dxe:ASPxGridLookup>
                        <dx:LinqServerModeDataSource ID="EntityServerModeDataSourceProject" runat="server" OnSelecting="EntityServerModeDataSourceProject_Selecting"
                            ContextTypeName="ERPDataClassesDataContext" TableName="V_ProjectListForContra" />
                    </div>
                </div>

                <div class="col-md-4">
                    <label>
                        <dxe:ASPxLabel ID="lblHierarchy" runat="server" Text="Hierarchy">
                        </dxe:ASPxLabel>
                    </label>
                    <asp:DropDownList ID="ddlHierarchy" runat="server" Width="100%" Enabled="false">
                    </asp:DropDownList>
                </div>
            </div>
            <div>
                <div>
                    <br />
                </div>


                <dxe:ASPxGridView runat="server" ClientInstanceName="grid" ID="grid" KeyFieldName="WithDrawFrom"
                    Width="100%" EnableRowsCache="False" OnCellEditorInitialize="grid_CellEditorInitialize" OnCustomCallback="grid_CustomCallback"
                    OnCustomJSProperties="grid_CustomJSProperties" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" OnInitNewRow="grid_InitNewRow">
                    <SettingsPager Visible="false"></SettingsPager>
                    <Columns>
                        <dxe:GridViewDataComboBoxColumn Caption="Withdrawal From" FieldName="WithDrawFrom" VisibleIndex="0" Width="280px">
                            <PropertiesComboBox ValueField="AccountCode" ClientInstanceName="WithDrawFrom" TextField="IntegrateMainAccount" ClearButton-DisplayMode="Always">
                                <%-- <ValidationSettings RequiredField-IsRequired="true" Display="None"></ValidationSettings>--%>
                                <ClientSideEvents SelectedIndexChanged="Withdrawal_SelectedIndexChanged" GotFocus="calWithdrawalBal"  />
                            </PropertiesComboBox>
                        </dxe:GridViewDataComboBoxColumn>

                         <dxe:GridViewDataButtonEditColumn Caption="Sub Ledger" FieldName="WithdrawlSubLedger" VisibleIndex="1" Width="150" ReadOnly="true">
                                                        <PropertiesButtonEdit>
                                                            <ClientSideEvents ButtonClick="SubledgerButnClick"  KeyDown="SubledgerDown"/>
                                                            <Buttons>
                                                                <dxe:EditButton Text="..." Width="20px">
                                                                </dxe:EditButton>
                                                            </Buttons>
                                                        </PropertiesButtonEdit>
                        </dxe:GridViewDataButtonEditColumn>


                        <dxe:GridViewDataComboBoxColumn FieldName="DepositInto" Caption="Deposit Into" VisibleIndex="2" Width="280px">
                            <PropertiesComboBox ValueField="AccountCode" ClientInstanceName="DepositInto" TextField="IntegrateMainAccount" ClearButton-DisplayMode="Always">
                                <%-- <ValidationSettings RequiredField-IsRequired="true" Display="None"></ValidationSettings>--%>
                                <ClientSideEvents SelectedIndexChanged="Deposit_SelectedIndexChanged" LostFocus="DepositInto_LostFocus"  />
                            </PropertiesComboBox>
                        </dxe:GridViewDataComboBoxColumn>

                        <dxe:GridViewDataButtonEditColumn Caption="Sub Ledger" FieldName="DepositSubLedger" VisibleIndex="3" Width="150" ReadOnly="true">
                                                        <PropertiesButtonEdit>
                                                            <ClientSideEvents ButtonClick="DepositSubledgerButnClick"  KeyDown="DepositSubledgerDown" />
                                                            <Buttons>
                                                                <dxe:EditButton Text="..." Width="20px">
                                                                </dxe:EditButton>
                                                            </Buttons>
                                                        </PropertiesButtonEdit>
                        </dxe:GridViewDataButtonEditColumn>
                        



                        <dxe:GridViewDataComboBoxColumn VisibleIndex="4" FieldName="Currency_ID" Caption="Currency" Width="100px">
                            <PropertiesComboBox ValueField="Currency_ID" ClientInstanceName="CurrencyID" TextField="Currency_AlphaCode">


                                <ClientSideEvents SelectedIndexChanged="SetVisibility" />
                                <ClientSideEvents
                                    LostFocus="function(s,e){
                                    NextFocus(s,e);
                                    }" />
                                <ClientSideEvents />
                            </PropertiesComboBox>
                        </dxe:GridViewDataComboBoxColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="5" Caption="Rate" FieldName="Rate" Width="100px">
                            <PropertiesTextEdit>
                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                                <ClientSideEvents TextChanged="ReceiptTextChange" KeyDown="RateOnKeyDown" />
                                <ClientSideEvents />
                                <ValidationSettings RequiredField-IsRequired="true" Display="None"></ValidationSettings>
                            </PropertiesTextEdit>
                            <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="6" Caption="Amount" FieldName="Amount" Width="100px">
                            <PropertiesTextEdit>
                                <MaskSettings Mask="&lt;0..999999999999999999&gt;.&lt;00..99&gt;" />
                                <ClientSideEvents TextChanged="AutoCalValue" />
                                <ValidationSettings RequiredField-IsRequired="true" Display="None"></ValidationSettings>
                            </PropertiesTextEdit>
                            <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="CashBankDetail_PaymentAmount" Caption="Amount in Home Currency" Width="100px">
                            <PropertiesTextEdit>
                                <%--  <MaskSettings Mask="<0..999999999999>.<0..9999>" />--%>
                                <MaskSettings Mask="&lt;0..999999999999999999&gt;.&lt;00..99&gt;" />
                                <ClientSideEvents KeyDown="HomeCurrencyOnKeyDown" />
                                <ValidationSettings RequiredField-IsRequired="true" Display="None"></ValidationSettings>
                            </PropertiesTextEdit>
                            <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                        </dxe:GridViewDataTextColumn>


                        <dxe:GridViewDataButtonEditColumn FieldName="Project_Code" Caption="Project Code" VisibleIndex="8" Width="200px">
                                                                         <PropertiesButtonEdit>
                                                                            <ClientSideEvents ButtonClick="ProjectCodeButnClick" KeyDown="ProjectCodeKeyDown"/>
                                                                                  <Buttons>
                                                                            <dxe:EditButton Text="..." Width="20px">
                                                                            </dxe:EditButton>
                                                                                  </Buttons>
                                                                        </PropertiesButtonEdit>
                       </dxe:GridViewDataButtonEditColumn>




                        <dxe:GridViewDataTextColumn VisibleIndex="9" Caption="Remarks" FieldName="Remarks">
                            <PropertiesTextEdit>
                            </PropertiesTextEdit>
                            <CellStyle Wrap="False" HorizontalAlign="left" CssClass="gridcellleft"></CellStyle>
                        </dxe:GridViewDataTextColumn>

                       <%--  <dxe:GridViewDataTextColumn  Caption="Project Id" FieldName="ProjectId" CellStyle-CssClass="hide" Width="0px" >
                          
                        </dxe:GridViewDataTextColumn>--%>

                        <dxe:GridViewDataTextColumn Caption="Account Group" Visible="false" CellStyle-CssClass="hide" Width="0px">
                        </dxe:GridViewDataTextColumn>

                       

                        <dxe:GridViewDataTextColumn FieldName="ProjectId" Caption="Project" ReadOnly="True" Width="0px"
                                EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide"
                                PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                                <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn FieldName="WithdrawlSubledgerId" Caption="WithdrawlSubledgerId" ReadOnly="True" Width="0px"
                                EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide"
                                PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                                <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn FieldName="DepositSubledgerId" Caption="DepositSubledgerId" ReadOnly="True" Width="0px"
                                EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide"
                                PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                                <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                        </dxe:GridViewDataTextColumn>

                    </Columns>
                    <SettingsBehavior AllowFocusedRow="True" ColumnResizeMode="NextColumn" />
                    <%--  <ClientSideEvents Init="OnInit" EndCallback="OnEndCallback" />--%>
                    <SettingsDataSecurity AllowEdit="true" />
                    <SettingsEditing Mode="Batch">
                        <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="Row" />
                    </SettingsEditing>
                    <ClientSideEvents EndCallback="function(s, e) {
	                                        LastCall(s.cpHeight);
                                        }"    RowClick="GetVisibleIndexContra"  />
                    <Styles>
                        <StatusBar CssClass="statusBar">
                        </StatusBar>
                    </Styles>
                </dxe:ASPxGridView>
                <asp:SqlDataSource ID="batchgrid" runat="server" SelectCommand="Select (select Currency_AlphaCode from Master_Currency where Currency_ID=tc.CashBank_Currency)as Currency_ID,
                (Select top 1 CashBankDetail_PaymentAmount from Trans_CashBankDetail where CashBankDetail_VoucherID=tc.CashBank_ID)as CashBankDetail_PaymentAmount
                from Trans_CashBankVouchers tc  ">
                    <SelectParameters>
                        <asp:Parameter Name="CashBank_ID" Type="string" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <div>
                    <br />
                </div>
                <div style="background: #f5f4f3; padding: 8px 0; margin-bottom: 15px; border-radius: 4px; border: 1px solid #ccc;" class="clearfix">
                    <div class="col-md-3">
                        <label id="tdITypeLable" style="">Instrument Type</label>
                        <div style="" id="tdITypeValue">
                            <dxe:ASPxComboBox ID="ComboInstType" runat="server" ClientInstanceName="cComboInstType" Font-Size="12px"
                                ValueType="System.String" Width="100%" EnableIncrementalFiltering="True">
                                <Items>
                                    <%-- <dxe:ListEditItem Text="Select" Value="NA" Selected />--%>
                                    <dxe:ListEditItem Text="Cheque" Value="C" Selected />
                                    <dxe:ListEditItem Text="Draft" Value="D" />
                                    <dxe:ListEditItem Text="E.Transfer" Value="E" />
                                    <%-- <dxe:ListEditItem Text="Cash" Value="CH" />--%>
                                    <dxe:ListEditItem Text="Cash" Value="0" />
                                </Items>
                                <ClientSideEvents SelectedIndexChanged="OnComboInstTypeSelectedIndexChanged" GotFocus="function(s,e){cComboInstType.ShowDropDown();}" />

                            </dxe:ASPxComboBox>
                            <span id="MandatoryInstrumentType" class="iconInstrumentType pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                        </div>
                    </div>
                    <div class="col-md-3" id="tdINoDiv" style="">
                        <label id="tdINoLable" style="">Instrument No</label>
                        <div id="tdINoValue">
                            <dxe:ASPxTextBox runat="server" ID="txtInstNo" ClientInstanceName="ctxtInstNo" Width="100%" MaxLength="20">
                                <%--<ClientSideEvents LostFocus="SetInstDate" GotFocus="SetInstDate_OnGotFocus" />--%>
                            </dxe:ASPxTextBox>
                        </div>
                    </div>
                    <div class="col-md-3" id="tdIDateDiv" style="">
                        <label id="tdIDateLable" style="">Instrument Date</label>
                        <div id="tdIDateValue" style="">
                            <dxe:ASPxDateEdit ID="InstDate" runat="server" EditFormat="Custom" ClientInstanceName="cInstDate"
                                UseMaskBehavior="True" Font-Size="12px" Width="100%" EditFormatString="dd-MM-yyyy">
                                <%-- <ClientSideEvents DateChanged="function(s,e){ }" />--%>
                                <%-- <ClientSideEvents Init="function(s,e){ s.SetDate(new Date());}" />--%>
                                <ClientSideEvents DateChanged="function(s,e){InstrumentDateChange();}" GotFocus="function(s,e){cInstDate.ShowDropDown();}"></ClientSideEvents>
                                <ValidationSettings RequiredField-IsRequired="true"></ValidationSettings>
                                <ButtonStyle Width="13px">
                                </ButtonStyle>
                            </dxe:ASPxDateEdit>
                        </div>
                    </div>
                    <div style="clear: both"></div>
                    <div class="col-md-8">
                        <label style="margin-bottom: 5px; display: inline-block">Narration</label>
                        <div>
                            <dxe:ASPxMemo ID="txtNarration" ClientInstanceName="ctxtNarration" runat="server" Height="50px" Width="100%" Font-Names="Arial"></dxe:ASPxMemo>
                        </div>
                    </div>
                    <div class="clear"></div>
                </div>
                <div>
                    <br />
                </div>
                <table style="float: left;">
                    <tr>

                        <td></td>
                        <td>
                            <dxe:ASPxButton ID="btnSaveExit" ClientInstanceName="cbtnexit" runat="server" AutoPostBack="False" Text="Save & Ex&#818;it" OnClick="btnSaveExit_Click"
                                CssClass="btn btn-primary" UseSubmitBehavior="False">
                                <ClientSideEvents Click="function(s, e) {SaveNewButtonClick(s,e);}" />
                            </dxe:ASPxButton>

                        </td>

                    </tr>
                </table>
            </div>

        </div>

       

        <table class="TableMain100" style="width: 100%">
            <tr>
                <td>
                    <table style="width: 100%;" id="TblSearch" runat="server">
                        <tr>
                            <td class="relative">
                                <div class="">
                                </div>

                                 <div id="spnEditLock" runat="server" style="display:none; color:red;text-align:center"></div>
                                  <div id="spnDeleteLock" runat="server" style="display:none; color:red;text-align:center"></div>
                                <dxe:ASPxGridView ID="Grid_ContraVoucher" runat="server" KeyFieldName="CashBank_ID" AutoGenerateColumns="False"
                                    Width="100%" ClientInstanceName="gvContraVoucherInstance" SettingsBehavior-AllowFocusedRow="true" Settings-VerticalScrollableHeight="280" Settings-VerticalScrollBarMode="Auto"
                                    SettingsBehavior-AllowSelectSingleRowOnly="false" SettingsBehavior-AllowSelectByRowClick="true" SettingsBehavior-ColumnResizeMode="Control"
                                    DataSourceID="EntityServerModeDataSource" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" OnCustomJSProperties="Grid_ContraVoucher_CustomJSProperties"
                                    OnCustomCallback="Grid_ContraVoucher_CustomCallback">
                                    <SettingsSearchPanel Visible="True" Delay="5000" />

                                    <Columns>
                                        <dxe:GridViewDataTextColumn FieldName="CashBank_ID" SortOrder="Descending" VisibleIndex="0" Visible="false">
                                            <Settings AllowAutoFilterTextInputTimer="False" />
                                        </dxe:GridViewDataTextColumn>
                                       <%-- 0024170 add  <PropertiesTextEdit DisplayFormatString="dd-MM-yyyy"></PropertiesTextEdit>--%>
                                        <dxe:GridViewDataTextColumn VisibleIndex="1" Caption="Posting Date" FieldName="CashBank_TransactionDateText" FixedStyle="Left" Width="10%">
                                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                            <Settings AllowAutoFilterTextInputTimer="False" />
                                            <%--<PropertiesTextEdit DisplayFormatString="dd/MM/yyyy"></PropertiesTextEdit>--%>
                                            <PropertiesTextEdit DisplayFormatString="dd-MM-yyyy"></PropertiesTextEdit>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn VisibleIndex="2" Caption="Document No." FixedStyle="Left" FieldName="CashBank_VoucherNumber" Width="10%">
                                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                            <Settings AllowAutoFilterTextInputTimer="False" />
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn VisibleIndex="3" Caption="Amount" FieldName="CashBankDetail_PaymentAmount" Width="10%">
                                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                            <Settings AllowAutoFilterTextInputTimer="False" />
                                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn VisibleIndex="4" Caption="Narration" FieldName="CashBank_Narration" Settings-AllowAutoFilter="False" Width="20%">
                                            <CellStyle CssClass="gridcellleft"></CellStyle>
                                            <Settings AllowAutoFilterTextInputTimer="False" />
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn VisibleIndex="5" Caption="Project Name" FieldName="Proj_Name" Settings-AllowAutoFilter="true" Width="20%">
                                            <CellStyle CssClass="gridcellleft"></CellStyle>
                                            <Settings AllowAutoFilterTextInputTimer="true" />
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn VisibleIndex="6" Caption="Entered By" FieldName="CashBank_CreateUser" Settings-AllowAutoFilter="False" Width="10%">
                                            <CellStyle CssClass="gridcellleft"></CellStyle>
                                            <Settings AllowAutoFilterTextInputTimer="False" />
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn VisibleIndex="7" Caption="Last Update On" FieldName="CashBank_ModifyDateTime" Settings-AllowAutoFilter="False" Width="15%">
                                            <CellStyle CssClass="gridcellleft"></CellStyle>
                                            <Settings AllowAutoFilterTextInputTimer="False" />
                                            <PropertiesTextEdit DisplayFormatString="dd-MM-yyyy hh:mm:ss"></PropertiesTextEdit>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn VisibleIndex="8" Caption="Updated By" FieldName="CashBank_ModifyUser" Settings-AllowAutoFilter="False" Width="10%">
                                            <CellStyle CssClass="gridcellleft"></CellStyle>
                                            <Settings AllowAutoFilterTextInputTimer="False" />
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="" VisibleIndex="8" Width="0">
                                            <CellStyle HorizontalAlign="Center">
                                            </CellStyle>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <HeaderTemplate>
                                            </HeaderTemplate>
                                            <DataItemTemplate>
                                                <div class='floatedBtnArea'>
                                                    <% if (rights.CanView)
                                                       { %>
                                                    <a href="javascript:void(0);" onclick="OnView('EDIT~'+'<%# Container.KeyValue %>','<%#Eval("CashBankDetail_BankValueDate") %>')" class="">
                                                        <span class='ico ColorThree'><i class='fa fa-eye'></i></span><span class='hidden-xs'>View</span></a>
                                                    <%} %>
                                                    <% if (rights.CanEdit)
                                                       { %>
                                                    <a href="javascript:void(0);" onclick="OnEdit('EDIT~'+'<%# Container.KeyValue %>','<%#Eval("CashBankDetail_BankValueDate") %>')" class="" style='<%#Eval("Editlock")%>'>
                                                        <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Edit</span></a>
                                                    <%} %>
                                                    <%--Mantis Issue:- 0024782 Swati Rev work Copy feature Add 25.03.2022--%>
                                                     <% if (rights.CanAdd)
                                                       { %>
                                                        <a href="javascript:void(0);" onclick="OnCopy('EDIT~'+'<%# Container.KeyValue %>','<%#Eval("CashBankDetail_BankValueDate") %>')" class="" style='<%#Eval("Editlock")%>'>
                                                        <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Copy</span></a>
                                                    <%} %>
                                                    <%--Close of Mantis Issue:- 0024782 Swati Rev work Copy feature Add 25.03.2022--%>
                                                    <% if (rights.CanDelete)
                                                       { %>
                                                    <a href="javascript:void(0);" onclick="OnGetRowValuesOnDelete('<%# Container.KeyValue %>','<%#Eval("CashBank_TransactionDate") %>','<%#Eval("CashBankDetail_BankValueDate") %>','<%#Eval("CashBank_IBRef") %>')"
                                                        class="" style='<%#Eval("Deletelock")%>'>
                                                        <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>Delete</span></a>
                                                    <%} %>
                                                    <% if (rights.CanPrint)
                                                       { %>
                                                    <a href="javascript:void(0);" onclick="onPrintJv('<%# Container.KeyValue %>')" class="pad" title="">
                                                        <span class='ico ColorFour'><i class='fa fa-print'></i></span><span class='hidden-xs'>Print</span>
                                                    </a><%} %>
                                                </div>
                                            </DataItemTemplate>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn VisibleIndex="9" Caption="" FieldName="CashBankDetail_BankValueDate" Visible="false">
                                            <CellStyle CssClass="gridcellleft"></CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn VisibleIndex="9" Caption="" FieldName="CashBank_IBRef" Visible="false">
                                            <CellStyle CssClass="gridcellleft"></CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                    </Columns>
                                    <ClientSideEvents RowClick="gridRowclick" />
                                    <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                                    <SettingsPager PageSize="10">
                                        <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,20,50,100,150,200" />
                                    </SettingsPager>
                                    <Settings ShowGroupPanel="True" ShowFilterRow="true" ShowFilterRowMenu="true" />
                                    <SettingsLoadingPanel Text="Please Wait..." />
                                    <ClientSideEvents EndCallback="function(s,e){ ShowMsgLastCall(s,e);}" />
                                </dxe:ASPxGridView>
                                <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
                                    ContextTypeName="ERPDataClassesDataContext" TableName="v_ContraVoucherList" />
                                <dxe:ASPxGridViewExporter ID="exporter" runat="server">
                                </dxe:ASPxGridViewExporter>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>

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
                            </dxe:PanelContent>
                        </PanelCollection>
                    </dxe:ASPxCallbackPanel>
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>
    </div>
    <%--DEBASHIS--%>


       <%--Chinmoy added inline Project code start 13-12-2019--%>

         <dxe:ASPxPopupControl ID="ProjectCodePopup" runat="server" ClientInstanceName="cProjectCodePopup"
            CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Height="400"
            Width="700" HeaderText="Select Document Number" AllowResize="true" ResizingMode="Postponed" Modal="true">
            <%--  <headertemplate>
                <span>Select Document Number</span>
            </headertemplate>--%>
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <label><strong>Search By Document Number</strong></label>
                 <%--   <span style="color: red;">[Press ESC key to Cancel]</span>--%>
                    <dxe:ASPxCallbackPanel runat="server" ID="ProjectCodeCallback" ClientInstanceName="cProjectCodeCallback"
                        OnCallback="ProjectCodeCallback_Callback">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <dxe:ASPxGridLookup ID="lookupPopup_ProjectCode" runat="server" ClientInstanceName="clookupPopup_ProjectCode" Width="800"
                                    KeyFieldName="ProjectId" TextFormatString="{0}" MultiTextSeparator=", " ClientSideEvents-TextChanged="ProjectCodeSelected"
                                    ClientSideEvents-KeyDown="lookup_ProjectCodeKeyDown" OnDataBinding="lookup_ProjectCode_DataBinding">
                                    <Columns>

                                                 <%--   <dxe:GridViewDataColumn FieldName="Proj_Id" Visible="true" VisibleIndex="0" Caption="Project id" Settings-AutoFilterCondition="Contains" Width="200px">
                                                    </dxe:GridViewDataColumn>--%>
                                                    <dxe:GridViewDataColumn FieldName="ProjectCode" Visible="true" VisibleIndex="1" Caption="Project Code" Settings-AutoFilterCondition="Contains" Width="200px">
                                                    </dxe:GridViewDataColumn>
                                                    <dxe:GridViewDataColumn FieldName="Proj_Name" Visible="true" VisibleIndex="2" Caption="Project Name" Settings-AutoFilterCondition="Contains" Width="200px">
                                                    </dxe:GridViewDataColumn>
                                                    <dxe:GridViewDataColumn FieldName="Customer" Visible="true" VisibleIndex="3" Caption="Customer" Settings-AutoFilterCondition="Contains" Width="200px">
                                                    </dxe:GridViewDataColumn>
                                                    <dxe:GridViewDataColumn FieldName="HIERARCHY_NAME" Visible="true" VisibleIndex="4" Caption="Hierarchy" Settings-AutoFilterCondition="Contains" Width="200px">
                                                        </dxe:GridViewDataColumn>
                                                    <dxe:GridViewDataColumn FieldName="Hierarchy_ID" Visible="true" VisibleIndex="5" Caption="Hierarchy_ID" Settings-AutoFilterCondition="Contains" Width="0">
                                                    </dxe:GridViewDataColumn>


                                    </Columns>
                                    <GridViewProperties Settings-VerticalScrollBarMode="Auto">

                                        <Templates>
                                            <StatusBar>
                                                <table class="OptionsTable" style="float: right">
                                                    <tr>
                                                        <td>
                                                            <%--  <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridLookup" />--%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </StatusBar>
                                        </Templates>
                                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                    </GridViewProperties>
                                </dxe:ASPxGridLookup>
                             <%--   <dx:LinqServerModeDataSource ID="EntityServerModeDataProjectQuotation" runat="server" OnSelecting="EntityServerModeDataProjectQuotation_Selecting"
                                                ContextTypeName="ERPDataClassesDataContext" TableName="ProjectCodeBind" />--%>
                            </dxe:PanelContent>
                        </PanelCollection>
                        <ClientSideEvents EndCallback="ProjectCodeCallback_endcallback" />
                    </dxe:ASPxCallbackPanel>
                </dxe:PopupControlContentControl>
            </ContentCollection>
            <HeaderStyle BackColor="Blue" Font-Bold="True" ForeColor="White" />
        </dxe:ASPxPopupControl>



        <%--//End--%>





    <asp:HiddenField ID="hddn_BranchID" runat="server" />
    <asp:HiddenField ID="hdnBranchIdTo" runat="server" />
    <%--Added BY:Subhabrata--%>
    <asp:HiddenField ID="hdn_CashBankID" runat="server" />
    <asp:HiddenField ID="hdnWithDrawFrom" runat="server" />
     <asp:HiddenField ID="hdnInlineProjId" runat="server" />
    <asp:HiddenField ID="hdnDepositInto" runat="server" />
    <asp:HiddenField ID="hdnCurrency_ID" runat="server" />
    <asp:HiddenField ID="hdnAmountInHomeCurrency" runat="server" />
    <asp:HiddenField ID="hdnRemarks" runat="server" />
    <asp:HiddenField ID="hdnCashBankId" runat="server" />
    <asp:HiddenField ID="hdnCashBankText" runat="server" />
    <asp:HiddenField ID="hdn_CurrentSegment" runat="server" />
    <asp:HiddenField ID="hdn_Mode" runat="server" />
    <asp:HiddenField ID="hdnCashBank_IBRef" runat="server" />
    <asp:HiddenField ID="hdnCashBank_FinYear" runat="server" />
    <asp:HiddenField ID="hdnCashBank_CompanyID" runat="server" />
    <asp:HiddenField ID="hdnCashBank_ExchangeSegmentID" runat="server" />
    <asp:HiddenField ID="hdnVoucherNo" runat="server" />
    <asp:HiddenField ID="hdnType" runat="server" />
    <asp:HiddenField ID="hdnRate" runat="server" />
    <asp:HiddenField ID="hdnAmount" runat="server" />
    <asp:HiddenField ID="hdnBtnValue" runat="server" />
    <asp:HiddenField ID="hdnDbSaveCurrenct" runat="server" />
    <asp:HiddenField ID="hdnAllowProjectInDetailsLevel" runat="server" />
      <asp:HiddenField ID="hdnEditProjId" runat="server" />
     <%--Rev work Copy feature Add 25.03.2022--%>
    <asp:HiddenField ID="htID" runat="server" />
    <asp:HiddenField ID="hdnScemeID" runat="server" />
    <%--Close of Rev work Copy feature Add 25.03.2022--%>
    <%-- <asp:SqlDataSource ID="dsBranch" runat="server"
        ConflictDetection="CompareAllValues"
        SelectCommand="SELECT BRANCH_id AS BANKBRANCH_ID , RTRIM(BRANCH_DESCRIPTION)+' ['+ISNULL(RTRIM(BRANCH_CODE),'')+']' AS BANKBRANCH_NAME  FROM TBL_MASTER_BRANCH where Cast(branch_id as nvarchar(max)) in(@userbranchHierarchy)">
         <SelectParameters>
            <asp:SessionParameter Name="userbranchHierarchy" SessionField="userbranchHierarchy" />
         </SelectParameters>
        </asp:SqlDataSource>--%>
    <asp:SqlDataSource ID="dsBranch" runat="server"
        SelectCommand="Prc_Search_ContraVoucher" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:Parameter Type="String" Name="Action" DefaultValue="GetbranchListTo" />
            <asp:SessionParameter Name="BranchList" SessionField="userbranchHierarchy" Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlSchematype" runat="server"
        SelectCommand="Select * From ((Select '0' as ID,'Select' as SchemaName) Union (Select ID,SchemaName From tbl_master_Idschema  Where TYPE_ID='6' AND Isnull(Branch,'')=@userbranchID AND Isnull(comapanyInt,'')=@LastCompany)) as X Order By ID ASC">
        <SelectParameters>
            <asp:SessionParameter Name="LastCompany" SessionField="LastCompany" />
            <asp:SessionParameter Name="userbranchID" SessionField="userbranchID" />
        </SelectParameters>
    </asp:SqlDataSource>
    <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
        <ClientSideEvents ControlsInitialized="AllControlInitilize" />
    </dxe:ASPxGlobalEvents>
    <div>
        <asp:HiddenField ID="hfIsFilter" runat="server" />
        <asp:HiddenField ID="hfFromDate" runat="server" />
        <asp:HiddenField ID="hfToDate" runat="server" />
        <asp:HiddenField ID="hfBranchID" runat="server" />
        <asp:HiddenField ID="hdnProjectSelectInEntryModule" runat="server" />
        <asp:HiddenField ID="hdnWithDrawlLedgerName" runat="server" />
        <asp:HiddenField ID="hdnDedpositLedgerName" runat="server" />
         <asp:HiddenField ID="hdnSubledgerCashBankType" runat="server" />
        
    </div>

   <%-- subledger model--%>
     <div class="modal fade" id="SubledgerModel" role="dialog">
                <div class="modal-dialog">

                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Sub Ledger Search</h4>
                        </div>
                        <div class="modal-body">
                            <input type="text" onkeydown="Subledgermodkeydown(event)" id="txtSubledgerSearch" autofocus width="100%"  placeholder="Search By  Name" />

                            <div id="LedgerTable">
                                <table border='1' width="100%" class="dynamicPopupTbl">
                                    <tr class="HeaderStyle">
                                       <th class="hide">id</th>
                                        <th>Name</th>
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


         <div class="modal fade" id="DepositSubledgerModel" role="dialog">
                <div class="modal-dialog">

                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Sub Ledger Search</h4>
                        </div>
                        <div class="modal-body">
                            <input type="text" onkeydown="DepositSubledgermodkeydown(event)" id="txtDepositSubledgerSearch" autofocus width="100%" placeholder="Search By  Name" />

                            <div id="DepositLedgerTable">
                                <table border='1' width="100%" class="dynamicPopupTbl">
                                    <tr class="HeaderStyle">
                                       <th class="hide">id</th>
                                        <th>Name</th>
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

    <asp:HiddenField ID="hdnProjectMandatory" runat="server" />
     <dxe:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel"
        Modal="True">
    </dxe:ASPxLoadingPanel>

    <asp:HiddenField ID="hdnLockFromDate" runat="server" />
    <asp:HiddenField ID="hdnLockToDate" runat="server" />
    <asp:HiddenField ID="hdnLockFromDateCon" runat="server" />
    <asp:HiddenField ID="hdnLockToDateCon" runat="server" />
    <asp:HiddenField ID="hdnValAfterLock" runat="server" />
    <asp:HiddenField ID="hdnValAfterLockMSG" runat="server" />
    <asp:HiddenField ID="hdnLockFromDateedit" runat="server" />
    <asp:HiddenField ID="hdnLockToDateedit" runat="server" /> 
    <asp:HiddenField ID="hdnLockFromDatedelete" runat="server" />
    <asp:HiddenField ID="hdnLockToDatedelete" runat="server" />

      <%--  REV 1.0--%>
    <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="CallbackPanelEndCall" />
    </dxe:ASPxCallbackPanel>
    <asp:HiddenField ID="hFilterType" runat="server" />
    <%--END REV 1.0--%>

</asp:Content>
