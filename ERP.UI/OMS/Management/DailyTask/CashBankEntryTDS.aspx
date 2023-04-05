<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                04-04-2023        2.0.37           Pallab              Transactions pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="CashBankEntryTDS.aspx.cs" Inherits="ERP.OMS.Management.DailyTask.CashBankEntryTDS" %>

<%--<%@ Register Src="~/OMS/Management/Activities/UserControls/BillingShippingControl.ascx" TagPrefix="ucBS" TagName="BillingShippingControl" %>--%>
<%@ Register Src="~/OMS/Management/Activities/UserControls/Purchase_BillingShipping.ascx" TagPrefix="ucBS" TagName="Purchase_BillingShipping" %>
<%--<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>--%>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../Activities/JS/SearchPopup.js"></script>
    <link href="../Activities/CSS/SearchPopup.css" rel="stylesheet" />
    <script src="../../Tax%20Details/Js/TaxDetailsItemlevel.js" type="text/javascript"></script>
    <script src="/assests/pluggins/choosen/choosen.min.js"></script>

    <link rel="stylesheet" href="http://cdn.datatables.net/1.10.19/css/jquery.dataTables.min.css" />
    <script src="http://cdn.datatables.net/1.10.19/js/jquery.dataTables.min.js"></script>
    <style type="text/css">
        
    </style>

    <script>

        var debitOldValue;
        var debitNewValue;
        var CreditOldValue;
        var CreditNewValue;
        var ISdatatable = 0;


        function SetLostFocusonDemand(e) {
            if ((new Date($("#hdnLockFromDate").val()) <= cdtTDate.GetDate()) && (cdtTDate.GetDate() <= new Date($("#hdnLockToDate").val()))) {
                jAlert("DATA is Freezed between   " + $("#hdnLockFromDateCon").val() + " to " + $("#hdnLockToDateCon").val() + " for Add.");
            }
        }

        function AfterSaveBillingShipiing(validate) {
            
            if (validate) {
                page.SetActiveTabIndex(0);
                page.tabs[0].SetEnabled(true);
                $("#divcross").show();
            }
            else {
                page.SetActiveTabIndex(1);
                page.tabs[0].SetEnabled(false);
                $("#divcross").hide();
            }
        }


        function selectTDSChange(s, e) {
            var ID = ctdsSection.GetValue();
            var desc = ID.split('~')[1].trim();
            var code = ID.split('~')[0].trim();
            ctxtDeductionON.SetText(desc);
            var str = "";

            if (ISdatatable == 0) {
                str = "<thead><tr><th>Select</th><th class='hide'></th><th class='hide'></th><th class='hide'></th><th>Document No.</th><th>Party ID</th><th>Section</th><th>Payment/Credit Date</th><th>Total Tax</th><th>Amount of Tax</th><th>Surcharge</th><th>Edu. Cess</th></tr></thead>";
            }
            //
            str += "<tbody>";

            $.ajax({
                type: "POST",
                url: "CashBankEntry.aspx/GETTDSDOCDETAILS",
                data: JSON.stringify({ TDSPaymentDate: cdtTDate.GetDate(), TDSCode: code }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {
                    var data = msg.d;
                    // debugger;
                    for (var i = 0; i < data.length; i++) {
                        str += "<tr>";
                        str += "<td><input onclick='iCheckClick(this," + JSON.parse(data[i].DETID) + ");' type='checkbox' id='chk" + data[i].DETID + "'/></td>";
                        str += "<td class='hide'>" + data[i].DETID + "</td>";
                        str += "<td class='hide'>" + data[i].VendorId + "</td>";
                        str += "<td class='hide'>" + data[i].MainAccountID + "</td>";
                        str += "<td>" + data[i].DocumentNo + "</td>";
                        str += "<td>" + data[i].PartyID + "</td>";
                        str += "<td>" + data[i].TDSTCS_Code + "</td>";
                        str += "<td>" + data[i].PaymentDate + "</td>";
                        str += "<td>" + data[i].Total_Tax + "</td>";
                        str += "<td>" + data[i].Tax_Amount + "</td>";
                        str += "<td>" + data[i].Surcharge + "</td>";
                        str += "<td>" + data[i].EduCess + "</td>";
                        str += "</tr>";

                    }


                }
            });

            str += "</tbody>";
            $("#tbltdsDetails").html('');
            $("#tbltdsDetails").html(str);

            if (ISdatatable == 0) {
                var table = "";
                table = $('#tbltdsDetails').DataTable({
                    scrollY: '200px',
                    scrollCollapse: true,
                    paging: false

                }).draw();
                table.order.listener('#sorter', 1);
                ISdatatable = ISdatatable + 1;
            }



            ctxtSurcharge.SetText(0);
            ctxteduCess.SetText(0);
            ctxtTotal.SetText(0);
            ctxtTax.SetText(0);

        }

        function RcmCheckChange() {
            if (IsRcm.GetChecked()) {
                var Listddl_AmountAre = document.getElementById("ddl_AmountAre");
                Listddl_AmountAre.value = "1";
                Listddl_AmountAre.disabled = true;
                var item = Listddl_AmountAre.item(0);
                item.style.display = 'block';
            }
            else {
                var Listddl_AmountAre = document.getElementById("ddl_AmountAre");
                //Listddl_AmountAre.options.hide("Exclusive Tax");
                Listddl_AmountAre.disabled = false;
                var item = Listddl_AmountAre.item(0);
                item.style.display = 'none';
                Listddl_AmountAre.value = "3";
            }
            // else
            //ddl_AmountAre.refresh();
        }


        function SetSubAccountTDS(Id, name) {
            $('#SubAccountModelTDS').modal('hide');
            GetSubAcountComboBoxTDS(Id, name);
            gridTDS.batchEditApi.StartEdit(globalRowIndexTDS, 3);
        }

        function GetSubAcountComboBoxTDS(Id, Name) {
            setTimeout(function () { gridTDS.batchEditApi.StartEdit(globalRowIndexTDS, 3); }, 500);



            var subAccountText = Name;
            //var subAccountID = //cSubAcountComboBox.GetValue();
            //  grid.batchEditApi.StartEdit(globalRowIndex);
            var subAccountID = Id;
            gridTDS.batchEditApi.StartEdit(globalRowIndexTDS, 3);

            gridTDS.GetEditor("bthSubAccountTDS").SetText(subAccountText);
            gridTDS.GetEditor("gvColSubAccountTDS").SetText(subAccountID);
            //cSubAcountComboBox.Hide();
            gridTDS.batchEditApi.StartEdit(globalRowIndexTDS, 3);

            var updatedIndex = globalRowIndexTDS;

            var UniqueID = gridTDS.batchEditApi.GetCellValue(globalRowIndexTDS, "UniqueID");

            var TDSCode = GetTDSCodeByUniqueID(UniqueID);
            UpdateSubAccountTDS(UniqueID, subAccountText, subAccountID);
            var obj = {};
            obj.EntityId = subAccountID;
            obj.TDSCode = TDSCode;
            obj.tdsdate = cdtTDate.GetText();



            $.ajax({
                type: "POST",
                url: 'JournalEntry.aspx/GetTDSSubLedger',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                data: JSON.stringify(obj),
                success: function (response) {

                    if (response.d != "" && response.d != null) {

                        UpdateTDSRateByUniqueID(UniqueID, response.d);
                    }
                    else {
                        //setTimeout(function () { gridTDS.batchEditApi.StartEdit(uniqueindex, 2); }, 200);

                    }
                },
                error: function (response) {

                }
            });



            setTimeout(function () { gridTDS.batchEditApi.StartEdit(updatedIndex, 3); }, 500);
            //}
            //}
        }

        function UpdateTDSRateByUniqueID(uniqueID, val) {

            for (var i = 0; i < 1000; i++) {
                if (gridTDS.GetRow(i)) {
                    if (gridTDS.GetRow(i).style.display != "none") {
                        if (gridTDS.batchEditApi.GetCellValue(i, "UniqueID") == uniqueID && gridTDS.batchEditApi.GetCellValue(i, "IsTDS") != "" && gridTDS.batchEditApi.GetCellValue(i, "IsTDS") != null) {
                            gridTDS.batchEditApi.SetCellValue(i, "TDSPercentage", val, val);
                        }
                    }
                }
            }

            for (i = -1; i > -1000; i--) {
                if (gridTDS.GetRow(i)) {
                    if (gridTDS.GetRow(i).style.display != "none") {
                        if (gridTDS.batchEditApi.GetCellValue(i, "UniqueID") == uniqueID && gridTDS.batchEditApi.GetCellValue(i, "IsTDS") != "" && gridTDS.batchEditApi.GetCellValue(i, "IsTDS") != null) {
                            gridTDS.batchEditApi.SetCellValue(i, "TDSPercentage", val, val);
                        }
                    }
                }
            }
        }


        function UpdateSubAccountTDS(uniqueID, subAccountText, subAccountID) {
            for (var i = 0; i < 1000; i++) {
                if (gridTDS.GetRow(i)) {
                    if (gridTDS.GetRow(i).style.display != "none") {
                        gridTDS.batchEditApi.StartEdit(i, 2);
                        if (gridTDS.GetEditor("UniqueID").GetText() == uniqueID) {
                            gridTDS.batchEditApi.SetCellValue(i, "bthSubAccountTDS", subAccountText);
                            gridTDS.batchEditApi.SetCellValue(i, "gvColSubAccountTDS", subAccountID);
                        }
                    }
                }
            }

            for (i = -1; i > -1000; i--) {
                if (gridTDS.GetRow(i)) {
                    if (gridTDS.GetRow(i).style.display != "none") {
                        gridTDS.batchEditApi.StartEdit(i, 2);
                        if (gridTDS.GetEditor("UniqueID").GetText() == uniqueID) {
                            gridTDS.batchEditApi.SetCellValue(i, "bthSubAccountTDS", subAccountText);
                            gridTDS.batchEditApi.SetCellValue(i, "gvColSubAccountTDS", subAccountID);
                        }
                    }
                }
            }
        }

        function GetTDSCodeByUniqueID(uniqueID) {

            for (var i = 0; i < 1000; i++) {
                if (gridTDS.GetRow(i)) {
                    if (gridTDS.GetRow(i).style.display != "none") {
                        if (gridTDS.batchEditApi.GetCellValue(i, "UniqueID") == uniqueID && gridTDS.batchEditApi.GetCellValue(i, "IsTDS") != "" && gridTDS.batchEditApi.GetCellValue(i, "IsTDS") != null) {
                            return gridTDS.batchEditApi.GetCellValue(i, "IsTDS");
                        }
                    }
                }
            }

            for (i = -1; i > -1000; i--) {
                if (gridTDS.GetRow(i)) {
                    if (gridTDS.GetRow(i).style.display != "none") {
                        if (gridTDS.batchEditApi.GetCellValue(i, "UniqueID") == uniqueID && gridTDS.batchEditApi.GetCellValue(i, "IsTDS") != "" && gridTDS.batchEditApi.GetCellValue(i, "IsTDS") != null) {
                            return gridTDS.batchEditApi.GetCellValue(i, "IsTDS");
                        }
                    }
                }
            }
        }


        function changeDebitTotalSummaryTDS() {

            var newDif = debitOldValue - debitNewValue;
            var CurrentSum = c_txt_Debit.GetText();
            var indx = CurrentSum.indexOf(',');
            if (indx != -1) {
                CurrentSum = CurrentSum.replace(/,/g, '');
            }

            //c_txt_Debit.SetValue(parseFloat(CurrentSum - newDif));
            //var newNetAmountDiff = NetAmountOldValue - PaymentNewValue;
            //var CurrentNetSum = c_txtTotalNetAmount.GetText();
            //c_txt_Credit.SetValue(parseFloat(CurrentNetSum - newNetAmountDiff));


            var DebitAmount = 0;
            var CreditAmount = 0;

            for (var i = 0; i < 1000; i++) {
                if (gridTDS.GetRow(i)) {
                    if (gridTDS.GetRow(i).style.display != "none") {
                        gridTDS.batchEditApi.StartEdit(i, 2);
                        DebitAmount = DecimalRoundoff(parseFloat(DebitAmount), 2) + DecimalRoundoff(parseFloat(gridTDS.batchEditApi.GetCellValue(i, "WithDrawlTDS")), 2);
                        CreditAmount = DecimalRoundoff(parseFloat(CreditAmount), 2) + DecimalRoundoff(parseFloat(gridTDS.batchEditApi.GetCellValue(i, "ReceiptTDS")), 2);
                    }
                }
            }


            for (i = -1; i > -1000; i--) {
                if (gridTDS.GetRow(i)) {
                    if (gridTDS.GetRow(i).style.display != "none") {
                        gridTDS.batchEditApi.StartEdit(i, 2);
                        DebitAmount = DecimalRoundoff(parseFloat(DebitAmount), 2) + DecimalRoundoff(parseFloat(gridTDS.batchEditApi.GetCellValue(i, "WithDrawlTDS")), 2);
                        CreditAmount = DecimalRoundoff(parseFloat(CreditAmount), 2) + DecimalRoundoff(parseFloat(gridTDS.batchEditApi.GetCellValue(i, "ReceiptTDS")), 2);
                    }
                }
            }

            //Rev Tanmoy
            //var noofvisiblerows = gridTDS.GetVisibleRowsOnPage();
            //var i, cnt = 1;

            //for (i = -1 ; cnt <= noofvisiblerows ; i--) {
            //    var Payment = (gridTDS.batchEditApi.GetCellValue(i, 'WithDrawlTDS') != null) ? (gridTDS.batchEditApi.GetCellValue(i, 'WithDrawlTDS')) : "0";
            //    var Recieve = (gridTDS.batchEditApi.GetCellValue(i, 'ReceiptTDS') != null) ? (gridTDS.batchEditApi.GetCellValue(i, 'ReceiptTDS')) : "0";

            //    DebitAmount = DebitAmount + parseFloat(Payment);
            //    CreditAmount = CreditAmount + parseFloat(Recieve);

            //    cnt++;
            //}
            //End Rev Tanmoy
            //c_txt_Debit.SetValue(parseFloat(DecimalRoundoff((CurrentSum - newDif), 2)));

            c_txt_Debit.SetValue(parseFloat(DecimalRoundoff(DebitAmount, 2)));
            c_txt_Credit.SetValue(parseFloat(DecimalRoundoff(CreditAmount, 2)));
        }

        $(document).ready(function () {
            $('#MainAccountModelTDS').on('shown.bs.modal', function () {
                $('#txtMainAccountSearchTDS').val("");
                $('#txtMainAccountSearchTDS').focus();
            })
            $('#SubAccountModelTDS').on('shown.bs.modal', function () {
                $('#txtSubAccountSearchTDS').val("");
                $('#txtSubAccountSearchTDS').focus();
            })
            $('#SubAccountModelTDS').on('hide.bs.modal', function () {

                var updatedindex = globalRowIndexTDS;

                gridTDS.batchEditApi.StartEdit(updatedindex, 2);



                //grid.batchEditApi.StartEdit(globalRowIndex, 2);
            })
        });


        function MainAccountNewkeydownTDS(e) {
            var OtherDetails = {};
            OtherDetails.SearchKey = $("#txtMainAccountSearchTDS").val();
            OtherDetails.branchId = $("#ddlBranch").val();
            OtherDetails.considerTDS = false;
            OtherDetails.TDSCode = "";
            if (e.code == "Enter" || e.code == "NumpadEnter") {
                if ($("#txtMainAccountSearchTDS").val() == "")
                    return;
                var HeaderCaption = [];
                HeaderCaption.push("Main Account Name");
                HeaderCaption.push("Short Name");

                HeaderCaption.push("Subledger Type");
                HeaderCaption.push("TDS/TCS");

                callonServer("/OMS/Management/Activities/Services/Master.asmx/GetMainAccountJournalTDS", OtherDetails, "MainAccountTableTDS", HeaderCaption, "MainAccountIndexTDS", "SetMainAccountTDS");
            }
            else if (e.code == "ArrowDown") {
                if ($("input[MainAccountIndexTDS=0]"))
                    $("input[MainAccountIndexTDS=0]").focus();
            }
            else if (e.code == "Escape") {
                //  
                $('#MainAccountModelTDS').modal('hide');
                gridTDS.batchEditApi.StartEdit(globalRowIndexTDS, 1);
                var MainAccountID = gridTDS.GetEditor("gvMainAcCodeTDS").GetValue();

            }
        }

        function closeModalTDS() {

            var updatedindex = globalRowIndex;
            var MainAccountID = gridTDS.GetEditor("gvMainAcCodeTDS").GetValue();

            $('#MainAccountModelTDS').modal('hide');
            gridTDS.batchEditApi.StartEdit(updatedindex, 1);
        }

        function CloseSubModalTDS() {

            var updatedindex = globalRowIndex;
           // var MainAccountID = gridTDS.GetEditor("gvMainAcCodeTDS").GetValue();

            $('#SubAccountModelTDS').modal('hide');
            gridTDS.batchEditApi.StartEdit(updatedindex, 1);
        }


        function SetMainAccountTDS(Id, name, e) {

            $('#MainAccountModelTDS').modal('hide');
            var Code = e.parentElement.cells[2].innerText;
            var IsSub = e.parentElement.cells[3].innerText;
            var IsTDS = e.parentElement.cells[4].innerText;
           // clookup_Project.SetEnabled(false);
            GetMainAcountComboBoxTDS(Id, name, Code, IsSub, IsTDS);
            gridTDS.batchEditApi.StartEdit(globalRowIndexTDS, 2);

        }



        $(document).ready(function () {
            if ($("#hdn_Mode").val() == "Edit") {
                clookup_Project.SetEnabled(true);
                if ($("#ddlBranch").val() != null && $("#ddlBranch").val() != "") {
                    LoadBranchAddressInEditMode($('#ddlBranch').val());
                }
            }
        });


        function GetMainAcountComboBoxTDS(Id, Name, Code, IsSub, IsTDS) {

            gridTDS.batchEditApi.StartEdit(globalRowIndexTDS, 2);
            var uniqueindex = globalRowIndexTDS;
            var IsTDSCheck = gridTDS.GetEditor("IsTDSSource").GetValue();
            var UniqueID = gridTDS.batchEditApi.GetCellValue(globalRowIndexTDS, "UniqueID"); //gridTDS.GetEditor("gvColMainAccount").GetValue();

            if (IsTDSCheck == "1") {
                DeleteTDSRows(UniqueID);
                gridTDS.AddNewRow();
            }



            $('#mainActMsgTDS').hide();
            var MainAccountText = Name;
            console.log(MainAccountText, shouldCheck);

            if (shouldCheck != 1) {
                return;
            }

            var MainAccountID = Id;
            var MainAcCode = Code;
            IsSubAccount = IsSub;
            gridTDS.batchEditApi.StartEdit(globalRowIndexTDS, 2);
            gridTDS.GetEditor("IsSubledgerTDS").SetText(IsSubAccount);
            gridTDS.GetEditor("MainAccountTDS").SetText(MainAccountText);
            gridTDS.GetEditor("gvColMainAccountTDS").SetText(MainAccountID);
            gridTDS.GetEditor("gvMainAcCodeTDS").SetValue(IsSub);
            // gridTDS.GetEditor("ReverseApplicable").SetValue(ReverseApplicable);
            shouldCheck = 0;//
            gridTDS.GetEditor("bthSubAccountTDS").SetValue("");
            gridTDS.GetEditor("ReceiptTDS").SetValue("");
            gridTDS.GetEditor("WithDrawlTDS").SetValue("");
            gridTDS.GetEditor("gvColSubAccountTDS").SetValue("");
            var guid = uuid();
            gridTDS.GetEditor("UniqueID").SetValue(guid);
            gridTDS.GetEditor("IsTDSSource").SetValue("0");
            //gridTDS.GetEditor("IsTDS").SetText(0);
            //gridTDS.GetEditor("TDSPercentage").SetValue(0.00);
           // clookup_Project.SetEnabled(false);
            if ($("#hdnProjectSelectInEntryModule").val() == "1") {
                if ((clookup_Project.GetGridView().GetRowKey(clookup_Project.GetGridView().GetFocusedRowIndex()) != null)) {

                    var ProjectLookUpData = clookup_Project.GetGridView().GetRowKey(clookup_Project.GetGridView().GetFocusedRowIndex());
                    gridTDS.GetEditor("Project_Code").SetText(clookup_Project.GetValue());
                    gridTDS.GetEditor("ProjectId").SetText(ProjectLookUpData);
                }
            }



            if (LastDr != 0)
                c_txt_Debit.SetValue(DecimalRoundoff(parseFloat(c_txt_Debit.GetValue()) - parseFloat(LastDr), 2));
            if (LastCr != 0)
                c_txt_Credit.SetValue(DecimalRoundoff(parseFloat(c_txt_Credit.GetValue()) - parseFloat(LastCr), 2));

            var Debit = parseFloat(c_txt_Debit.GetValue());
            var Credit = parseFloat(c_txt_Credit.GetValue());

            if (Debit == 0 && Credit == 0) {
                //cbtnSaveRecordsTDS.SetVisible(false);
                //cbtn_SaveRecordsTDS.SetVisible(false);

                var Amount = parseFloat(Debit) - parseFloat(Credit);
            }
            else if (Debit == Credit) {
                //cbtnSaveRecordsTDS.SetVisible(true);
                //cbtn_SaveRecordsTDS.SetVisible(true);
            }
            else if (Debit != Credit) {
                // cbtnSaveRecordsTDS.SetVisible(false);
                //cbtn_SaveRecordsTDS.SetVisible(false);

                var Amount = parseFloat(Debit) - parseFloat(Credit);
            }


            if ($("#cpTaggedTDS").val() == "-99") {
                //cbtnSaveRecordsTDS.SetVisible(false);
                //cbtn_SaveRecordsTDS.SetVisible(false);

            }

            LastDr = 0.00;
            LastCr = 0.00;


            if (IsTDS != "" && IsTDS != null) {

                gridTDS.GetEditor("IsTDSSource").SetValue("1");



                $.ajax({
                    type: "POST",
                    url: 'JournalEntry.aspx/GetTDSLedger',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    data: "{TDSCode:\"" + IsTDS + "\",tdsdate:\"" + cdtTDate.GetText() + "\"}",
                    success: function (response) {

                        if (response != "" && response != null) {


                            var currentRow = gridTDS.GetRow(globalRowIndexTDS);
                            gridTDS.AddNewRow();


                            gridTDS.batchEditApi.StartEdit(globalRowIndexTDS, 2);
                            gridTDS.GetEditor("IsSubledgerTDS").SetText("");
                            gridTDS.GetEditor("MainAccountTDS").SetText(response.d.trim().split('~')[0]);
                            gridTDS.GetEditor("gvColMainAccountTDS").SetText(response.d.trim().split('~')[1]);
                            gridTDS.GetEditor("gvMainAcCodeTDS").SetValue("None");
                            // gridTDS.GetEditor("ReverseApplicable").SetValue(ReverseApplicable);
                            shouldCheck = 0;//
                            gridTDS.GetEditor("bthSubAccountTDS").SetValue("");
                            gridTDS.GetEditor("ReceiptTDS").SetValue("");
                            gridTDS.GetEditor("WithDrawlTDS").SetValue("");
                            gridTDS.GetEditor("gvColSubAccountTDS").SetValue("");
                            gridTDS.GetEditor("IsTDS").SetText(IsTDS);
                            gridTDS.GetEditor("UniqueID").SetValue(guid);
                            gridTDS.GetEditor("TDSPercentage").SetValue(response.d.trim().split('~')[2]);


                            $("#gridTDS_DXDataRow" + globalRowIndexTDS).addClass(" rowRed");

                            document.getElementById("gridTDS_DXDataRow" + globalRowIndexTDS).enabled = false;
                            setTimeout(function () { gridTDS.batchEditApi.StartEdit(uniqueindex, 2); }, 200);

                        }
                        else {
                            setTimeout(function () { gridTDS.batchEditApi.StartEdit(uniqueindex, 2); }, 200);

                        }
                    },
                    error: function (response) {

                    }
                });
            }


            gridTDS.batchEditApi.StartEdit(uniqueindex, 2);

        }

        function uuid() {
            function randomDigit() {
                if (crypto && crypto.getRandomValues) {
                    var rands = new Uint8Array(1);
                    crypto.getRandomValues(rands);
                    return (rands[0] % 16).toString(16);
                } else {
                    return ((Math.random() * 16) | 0).toString(16);
                }
            }
            var crypto = window.crypto || window.msCrypto;
            return 'xxxxxxxx-xxxx-4xxx-8xxx-xxxxxxxxxxxx'.replace(/x/g, randomDigit);
        }


        var totTDSamount = 0;
        var totTax = 0;
        var totEdu = 0;
        var totSurcharge = 0;


        function BindGridViaTDSData() {
            $("#hdnTDSSection").val(TDSSection);
            gridTDS.PerformCallback('TDSPayment~' + ctxtTotal.GetText());
            $("#TDSmodal").modal('hide');
        }
        var TDSSection = "";
        function iCheckClick(cb, id) {
            //debugger;
            totTDSamount = 0;
            totTax = 0;
            totEdu = 0;
            totSurcharge = 0;

            var table = document.getElementById('tbltdsDetails');

            var rowLength = table.rows.length;

            var chkedArr = [];
            var t = 0;
            for (var i = 0; i < rowLength; i += 1) {
                var row = table.rows[i];
                if (row.children[1].innerText != "") {
                    console.log($("#chk" + row.children[1].innerText).prop('checked'));

                    if ($("#chk" + row.children[1].innerText).prop('checked')) {
                        var obj = {};
                        obj.CheckedID = row.children[1].innerText;
                        totTDSamount = totTDSamount + parseFloat(row.children[8].innerText);
                        totTax = totTax + parseFloat(row.children[9].innerText);
                        totEdu = totEdu + parseFloat(row.children[11].innerText);
                        totSurcharge = totSurcharge + parseFloat(row.children[10].innerText);
                        if (t == 0) {
                            TDSSection = row.children[1].innerText;
                            t = t + 1;
                        }
                        else {
                            TDSSection = TDSSection + ',' + row.children[1].innerText
                        }
                        chkedArr.push(obj);
                    }
                }
            }
            ctxtSurcharge.SetValue(parseFloat(totSurcharge));
            ctxteduCess.SetValue(parseFloat(totEdu));
            ctxtTotal.SetValue(parseFloat(totTDSamount));
            ctxtTax.SetValue(parseFloat(totTax));

        }

        function RecalculateTotal() {

            var total = DecimalRoundoff(totTDSamount, 2);
            total = total + DecimalRoundoff(ctxtInterest.GetValue(), 2) + DecimalRoundoff(ctxtLateFees.GetValue(), 2) + DecimalRoundoff(ctxtOthers.GetValue(), 2);
            ctxtTotal.SetValue(parseFloat(total));
        }



        function ShowTDSPopup() {

            $("#TDSmodal").modal('show');




            if ($("#hdnEditRfid").val() != "" && $("#hdnEditRfid").val() != "" && $("#hdnIsTDS").val() == "1") {

                $.ajax({
                    type: "POST",
                    url: "CashBankEntry.aspx/ShowTDSEditDetails",
                    data: JSON.stringify({ doc_id: $("#hdnEditRfid").val() }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var data = msg.d.tdsPayDet;
                        //debugger;
                        //var str = "<thead><tr><th>Select</th><th class='hide'></th><th class='hide'></th><th class='hide'></th><th>Party ID</th><th>Section</th><th>Payment/Credit Date</th><th>Total Tax</th><th>Amount of Tax</th><th>Surcharge</th><th>Edu. Cess</th></tr></thead>";
                        var str = "";
                        str += "<tbody>";
                        for (var i = 0; i < data.length; i++) {
                            str += "<tr>";
                            str += "<td><input onclick='iCheckClick(this," + JSON.parse(data[i].DETID) + ");' type='checkbox' checked disabled id='chk" + data[i].DETID + "'/></td>";
                            str += "<td class='hide'>" + data[i].DETID + "</td>";
                            str += "<td class='hide'>" + data[i].VendorId + "</td>";
                            str += "<td class='hide'>" + data[i].MainAccountID + "</td>";
                            str += "<td>" + data[i].PartyID + "</td>";
                            str += "<td>" + data[i].TDSTCS_Code + "</td>";
                            str += "<td>" + data[i].PaymentDate + "</td>";
                            str += "<td>" + data[i].Total_Tax + "</td>";
                            str += "<td>" + data[i].Tax_Amount + "</td>";
                            str += "<td>" + data[i].Surcharge + "</td>";
                            str += "<td>" + data[i].EduCess + "</td>";
                            str += "</tr>";

                        }

                        str += "</tbody>";
                        $("#tbltdsDetails").html('');
                        $("#tbltdsDetails").html(str);

                        ctdsSection.SetValue(msg.d.SectionID);
                        ctxtDeductionON.SetText(msg.d.DeductionON);
                        ctdsDate.SetText(msg.d.Payment_Date)
                        $("#ddlQuater").val(msg.d.Quater);
                        ctxtSurcharge.SetValue(msg.d.Quater)
                        ctxteduCess.SetValue(msg.d.Surcharge)
                        ctxtInterest.SetValue(msg.d.Interest)
                        ctxtLateFees.SetValue(msg.d.LateFees)
                        ctxtTotal.SetValue(msg.d.Total)
                        ctxtTax.SetValue(msg.d.Tax)
                        ctxtOthers.SetValue(msg.d.Others)
                        ctxtBankName.SetText(msg.d.BankName)
                        ctxtBankBranch.SetText(msg.d.BankBrach)
                        ctxtBRS.SetText(msg.d.BRS)
                        ctxtChallanNo.SetText(msg.d.ChallanNo)



                    }
                });
            }
            else {
                var ID = ctdsSection.GetValue();
                var desc = ID.split('~')[1].trim();
                ctdsDate.SetDate(cdtTDate.GetDate());
            }
            //$.ajax({
            //    type: "POST",
            //    url: "CashBankEntry.aspx/GETTDSDOCDETAILS",
            //    data: JSON.stringify({ TDSPaymentDate: cdtTDate.GetDate(), TDSCode: ID }),
            //    contentType: "application/json; charset=utf-8",
            //    dataType: "json",
            //    success: function (msg) {
            //        var data = msg.d;
            //        ctxtRate.SetValue(data);


            //    }
            //});
        }



    </script>











    <script type="text/javascript">

        //  Model Function ////
        var IsSubAccount = '';
        function MainAccountNewkeydown(e) {
            var OtherDetails = {}
            OtherDetails.SearchKey = $("#txtMainAccountSearch").val();
            OtherDetails.branchId = $("#ddlBranch").val();
            if (e.code == "Enter" || e.code == "NumpadEnter") {
                if ($("#txtMainAccountSearch").val() == "")
                    return;
                var HeaderCaption = [];
                HeaderCaption.push("Main Account Name");
                HeaderCaption.push("Subledger Type");
                HeaderCaption.push("Reverse Applicable");
                HeaderCaption.push("HSN/SAC");

                //callonServer("/OMS/Management/Activities/Services/Master.asmx/GetMainAccountCashBank", OtherDetails, "MainAccountTable", HeaderCaption, "MainAccountIndex", "SetMainAccount");
                callonServer("/OMS/Management/Activities/Services/Master.asmx/GetMainAccountCashBankByProcedure", OtherDetails, "MainAccountTable", HeaderCaption, "MainAccountIndex", "SetMainAccount");


            }
            else if (e.code == "ArrowDown") {
                if ($("input[MainAccountIndex=0]"))
                    $("input[MainAccountIndex=0]").focus();
            }
            else if (e.code == "Escape") {
                //  
                $('#MainAccountModel').modal('hide');
                gridTDS.batchEditApi.StartEdit(globalRowIndex, 1);

            }
        }


        function SetMainAccount(Id, name, e) {

            $('#MainAccountModel').modal('hide');

            var IsSub = e.parentElement.cells[2].innerText;
            var RevApp = e.parentElement.cells[3].innerText;
            if (RevApp == 'Yes') {
                RevApp = '1';
            }
            else {
                RevApp = '0';
            }
            var TaxAble = e.parentElement.cells[4].innerText;
            GetMainAcountComboBox(Id, name, IsSub, RevApp, TaxAble);
            gridTDS.batchEditApi.StartEdit(globalRowIndex, 2);

        }



        function ValueSelected(e, indexName) {
            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var Id = e.target.parentElement.parentElement.cells[0].innerText;
                var name = e.target.parentElement.parentElement.cells[1].children[0].value;
                if (Id) {
                    if (indexName == "MainAccountIndex") {
                        $('#MainAccountModel').modal('hide');
                        var Code = e.target.parentElement.parentElement.children[2].innerText;
                        var IsSub = e.target.parentElement.parentElement.children[3].innerText;

                        GetMainAcountComboBox(Id, name, Code, IsSub);
                        grid.batchEditApi.StartEdit(globalRowIndex, 2);
                    }
                    if (indexName == "MainAccountIndexTDS") {
                        $('#MainAccountModelTDS').modal('hide');
                        var Code = e.target.parentElement.parentElement.children[2].innerText;
                        var IsSub = e.target.parentElement.parentElement.children[3].innerText;
                        var IsTDS = e.target.parentElement.parentElement.children[4].innerText;

                        GetMainAcountComboBoxTDS(Id, name, Code, IsSub, IsTDS);
                        gridTDS.batchEditApi.StartEdit(globalRowIndexTDS, 2);
                    }

                    else if (indexName == "SubAccountIndex") {
                        $('#SubAccountModel').modal('hide');
                        GetSubAcountComboBox(Id, name);
                        grid.batchEditApi.StartEdit(globalRowIndex, 3);
                    }
                    else if (indexName == "SubAccountIndexTDS") {
                        $('#SubAccountModelTDS').modal('hide');
                        GetSubAcountComboBoxTDS(Id, name);
                        gridTDS.batchEditApi.StartEdit(globalRowIndexTDS, 3);
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
                    else if (indexName == "MainAccountIndex") {
                        $('#txtMainAccountSearchTDS').focus();
                    }
                    else if (indexName == "SubAccountIndex")
                        $('#txtSubAccountSearch').focus();
                    else if (indexName == "SubAccountIndexTDS")
                        $('#txtSubAccountSearchTDS').focus();
                }
            }
            else if (e.code == "Escape") {
                if (indexName == "MainAccountIndex") {
                    $('#MainAccountModel').modal('hide');
                    grid.batchEditApi.StartEdit(globalRowIndex, 1);
                    var MainAccountID = grid.GetEditor("gvMainAcCode").GetValue();
                    if (MainAccountID == 'Customers' || MainAccountID == 'Vendors') {
                        if ($("#hdnIsPartyLedger").val() == "") {
                            $("#hdnIsPartyLedger").val('1');
                        }
                        else {
                            $("#hdnIsPartyLedger").val(parseFloat($("#hdnIsPartyLedger").val()) + 1);
                        }

                    }

                }
                else if (indexName == "MainAccountIndexTDS") {
                    $('#MainAccountModelTDS').modal('hide');
                    gridTDS.batchEditApi.StartEdit(globalRowIndexTDS, 1);
                    var MainAccountID = gridTDS.GetEditor("gvMainAcCodeTDS").GetValue();

                }
                else if (indexName == "SubAccountIndex") {
                    $('#SubAccountModel').modal('hide');
                    grid.batchEditApi.StartEdit(globalRowIndex, 2);
                }
                else if (indexName == "SubAccountIndexTDS") {
                    $('#SubAccountModelTDS').modal('hide');
                    gridTDS.batchEditApi.StartEdit(globalRowIndexTDS, 2);
                }
            }


        }


        function DebitGotFocusTDS(s, e) {
            debitOldValue = s.GetText();
            var indx = debitOldValue.indexOf(',');
            if (indx != -1) {
                debitOldValue = debitOldValue.replace(/,/g, '');
            }
        }

        function MainAccountButnClick(s, e) {
            if (e.buttonIndex == 0) {
                var txt = "<table border='1' width=\"100%\"  class=\"dynamicPopupTbl\"><tr class=\"HeaderStyle\" ><th>Main Account Name</th><th>Subledger Type</th><th>Reverse Applicable</th><th>HSN/SAC</th></tr><table>";
                document.getElementById("MainAccountTable").innerHTML = txt;
                $('#MainAccountModel').modal('show');
                cMainAccountComboBox.Focus();

            }
        }

        function closeModal() {
            $('#MainAccountModel').modal('hide');
            gridTDS.batchEditApi.StartEdit(globalRowIndex, 2);
        }

        function SubAccountButnClick(s, e) {


            txt = " <table border='1' width=\"100%\"  class=\"dynamicPopupTbl\"><tr class=\"HeaderStyle\"><th>Sub Account Name [Unique Id]</th><th>Sub Account Type</th></tr></table>";
            document.getElementById("SubAccountTable").innerHTML = txt;

            $("#mainActMsgSub").hide();
            if (IsSubAccount != 'None') {
                gridTDS.batchEditApi.StartEdit(e.visibleIndex);
                var strMainAccountID = (gridTDS.GetEditor('MainAccount').GetText() != null) ? gridTDS.GetEditor('MainAccount').GetText() : "0";
                var MainAccountID = (gridTDS.GetEditor('gvColMainAccount').GetValue() != null) ? gridTDS.GetEditor('gvColMainAccount').GetValue() : "0";
                if (e.buttonIndex == 0) {
                    if (strMainAccountID.trim() != "") {
                        document.getElementById('hdnMainAccountId').value = MainAccountID;
                        var FullName = new Array("", "");
                        cSubAcountComboBox.AddItem(FullName, "");
                        cSubAcountComboBox.SetValue("");
                        $('#SubAccountModel').modal('show');

                    }
                }
            }
        }
        function SubAccountNewkeydown(e) {
            gridTDS.batchEditApi.StartEdit(e.visibleIndex);
            var strMainAccountID = (gridTDS.GetEditor('MainAccount').GetText() != null) ? gridTDS.GetEditor('MainAccount').GetText() : "0";
            var MainAccountID = (gridTDS.GetEditor('gvColMainAccount').GetValue() != null) ? gridTDS.GetEditor('gvColMainAccount').GetValue() : "0";

            var OtherDetails = {}
            OtherDetails.SearchKey = $("#txtSubAccountSearch").val();
            OtherDetails.MainAccountCode = MainAccountID;
            if (e.code == "Enter" || e.code == "NumpadEnter") {
                if ($("#txtSubAccountSearch").val() == "")
                    return;
                var HeaderCaption = [];
                HeaderCaption.push("Sub Account Name [Unique Id]");
                HeaderCaption.push("Sub Account Type");

                callonServer("/OMS/Management/Activities/Services/Master.asmx/GetSubAccountJournal", OtherDetails, "SubAccountTable", HeaderCaption, "SubAccountIndex", "SetSubAccount");
            }
            else if (e.code == "ArrowDown") {
                if ($("input[SubAccountIndex=0]"))
                    $("input[SubAccountIndex=0]").focus();
            }
            else if (e.code == "Escape") {
                $('#SubAccountModel').modal('hide');
                gridTDS.batchEditApi.StartEdit(globalRowIndex, 3);
            }
        }
        function SetSubAccount(Id, name) {
            $('#SubAccountModel').modal('hide');
            GetSubAcountComboBox(Id, name);
            gridTDS.batchEditApi.StartEdit(globalRowIndex, 4);
        }



        // Model Function ///





        var globalRowIndex;
        var shouldCheck = 0;
        function MainAccountClose(s, e) {
            cMainAccountpopUp.Hide();
            gridTDS.batchEditApi.StartEdit(globalRowIndex, 2);

        }
        function SubAccountClose(s, e) {
            cSubAccountpopUp.Hide();
            gridTDS.batchEditApi.StartEdit(globalRowIndex, 3);

        }
        function MainAccountKeyDown(s, e) {
            if (e.htmlEvent.key == "Enter") {
                shouldCheck = 0;
                s.OnButtonClick(0);
            }
            //if (e.htmlEvent.key == "Tab") {

            //    s.OnButtonClick(0);
            //}
        }
        function SubAccountKeyDown(s, e) {
            if (e.htmlEvent.key == "Enter") {
                s.OnButtonClick(0);
            }
            if (e.htmlEvent.key == "Delete") {
                var subAccountText = "";
                var subAccountID = "";

                gridTDS.batchEditApi.StartEdit(globalRowIndex);


                var VoucherType = document.getElementById('rbtnType').value;
                if (VoucherType == "P") {
                    setTimeout(function () { gridTDS.batchEditApi.StartEdit(globalRowIndex, 5); }, 500);

                }
                else {
                    setTimeout(function () { gridTDS.batchEditApi.StartEdit(globalRowIndex, 4); }, 500);
                }
                gridTDS.GetEditor("bthSubAccount").SetText(subAccountText);
                gridTDS.GetEditor("gvColSubAccount").SetText(subAccountID);



            }
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
            $('#SubAccountModel').on('hide.bs.modal', function () {

                //grid.batchEditApi.StartEdit(globalRowIndex, 2);
            })
        });

        function MainAccountComboBoxKeyDown(s, e) {
            if (e.htmlEvent.key == "Escape") {
                cMainAccountpopUp.Hide();
                gridTDS.batchEditApi.StartEdit(globalRowIndex, 3);
            }
            //if (e.htmlEvent.key == "Enter") {
            //    var MainAccountText = cMainAccountComboBox.GetText();                
            //    if (MainAccountText != "") {
            //        if (!cMainAccountComboBox.FindItemByText(MainAccountText)) {
            //            jAlert("Main Account does not Exist.");
            //            cMainAccountComboBox.SetText("");
            //            shouldCheck = 0;
            //            return;
            //        }
            //    }               

            //}
        }
        function SubAccountComboBoxKeyDown(s, e) {
            if (e.htmlEvent.key == "Escape") {
                cSubAccountpopUp.Hide();
                gridTDS.batchEditApi.StartEdit(globalRowIndex, 4);
            }
            if (e.htmlEvent.key == "Enter") {
                GetSubAcountComboBox(e);
            }
        }
        function GetMainAcountComboBox(Id, name, IsSub, RevApp, TaxAble) {
            var MainAccountText = name;

            IsSubAccount = IsSub;
            cMainAccountpopUp.Hide();
            var MainAccountID = Id;//cMainAccountComboBox.GetValue();
            var ReverseApplicable = RevApp; //cMainAccountComboBox.GetSelectedItem().texts[2];
            var TaxApplicable = TaxAble;// cMainAccountComboBox.GetSelectedItem().texts[3];
            // gridTDS.batchEditApi.StartEdit(globalRowIndex);
            gridTDS.batchEditApi.StartEdit(globalRowIndex, 3);
            gridTDS.GetEditor("MainAccount").SetText(MainAccountText);
            gridTDS.GetEditor("gvColMainAccount").SetText(MainAccountID);
            gridTDS.GetEditor("ReverseApplicable").SetValue(ReverseApplicable);
            shouldCheck = 0;
            gridTDS.GetEditor("bthSubAccount").SetValue("");
            gridTDS.GetEditor("btnRecieve").SetValue("");
            gridTDS.GetEditor("btnPayment").SetValue("");

            //if ($("#hdnProjectSelectInEntryModule").val() == "1") {
            //    if ((clookup_Project.GetGridView().GetRowKey(clookup_Project.GetGridView().GetFocusedRowIndex()) != null)) {

            //        var ProjectLookUpData = clookup_Project.GetGridView().GetRowKey(clookup_Project.GetGridView().GetFocusedRowIndex());
            //        gridTDS.GetEditor("Project_Code").SetText(clookup_Project.GetValue());
            //        gridTDS.GetEditor("ProjectId").SetText(ProjectLookUpData);
            //    }
            //}


            var prevAmount = gridTDS.GetEditor("NetAmount").GetText();
            var totamoutcng = c_txt_Credit.GetText();

            var diffcng = DecimalRoundoff(totamoutcng, 2) - DecimalRoundoff(prevAmount, 2);


            c_txt_Credit.SetText(diffcng.toFixed(2));

            gridTDS.GetEditor("TaxAmount").SetValue("0.00");
            gridTDS.GetEditor("NetAmount").SetValue("0.00");
            gridTDS.GetEditor("gvColSubAccount").SetValue("");
            gridTDS.GetEditor("IsSubledger").SetValue(IsSubAccount);
            //cddl_AmountAre.SetEnabled(false);
            $("#rbtnType").attr("disabled", "disabled");
            $("#IsTaxApplicable").val(TaxApplicable);
            var VoucherType = document.getElementById('rbtnType').value;
            if (ReverseApplicable == "1" && VoucherType == "P") {
                $("#chk_reversemechenism").prop("disabled", false);
                $("#chk_reversemechenism").prop("checked", true);
            }
            else {
                if ($("#chk_reversemechenism").prop('checked') == false) {
                    $("#chk_reversemechenism").prop("checked", false);
                }
            }

        }

        function GetSubAcountComboBox(Id, name) {
            var SubAcountText = cSubAcountComboBox.GetText();
            //if (cSubAcountComboBox.GetText() != "") {
            //if (!cSubAcountComboBox.FindItemByValue(cSubAcountComboBox.GetValue())) {
            //if (!cSubAcountComboBox.FindItemByText(SubAcountText)) {
            //    //jAlert("Sub Account does not Exist.", "Alert", function () { cSubAcountComboBox.SetValue(); cSubAcountComboBox.Focus(); });
            //    $('#subActMsg').show();
            //    return;
            //}
            //else {
            //    if (e.keyCode == 27)//escape 
            //    {
            //        gridTDS.batchEditApi.StartEdit(globalRowIndex, 3);
            //        return;
            //    }
            var subAccountText = name;//cSubAcountComboBox.GetText();
            var subAccountID = Id;//cSubAcountComboBox.GetValue();
            gridTDS.batchEditApi.StartEdit(globalRowIndex);


            var VoucherType = document.getElementById('rbtnType').value;
            if (VoucherType == "P") {
                setTimeout(function () { gridTDS.batchEditApi.StartEdit(globalRowIndex, 5); }, 500);

            }
            else {
                setTimeout(function () { gridTDS.batchEditApi.StartEdit(globalRowIndex, 4); }, 500);
            }
            gridTDS.GetEditor("bthSubAccount").SetText(subAccountText);
            gridTDS.GetEditor("gvColSubAccount").SetText(subAccountID);
            cSubAccountpopUp.Hide();

            //}

            // }
        }
        function CloseSubModal() {
            $('#SubAccountModel').modal('hide');
            gridTDS.batchEditApi.StartEdit(globalRowIndex, 2);

        }
        //function SubAccountButnClick(s, e) {
        //    gridTDS.batchEditApi.StartEdit(e.visibleIndex);
        //    var strMainAccountID = (gridTDS.GetEditor('MainAccount').GetText() != null) ? gridTDS.GetEditor('MainAccount').GetText() : "0";
        //    var MainAccountID = (gridTDS.GetEditor('gvColMainAccount').GetValue() != null) ? gridTDS.GetEditor('gvColMainAccount').GetValue() : "0";
        //    if (e.buttonIndex == 0) {
        //        $('#subActMsg').hide();
        //        if (strMainAccountID.trim() != "") {
        //            document.getElementById('hdnMainAccountId').value = MainAccountID;
        //            var FullName = new Array("", "");
        //            cSubAcountComboBox.AddItem(FullName, "");
        //            cSubAcountComboBox.SetValue("");
        //            cSubAccountpopUp.Show();
        //            cSubAcountComboBox.Focus();
        //        }
        //    }
        //}



        <%-- Old Code --%>
        //function MainAccountButnClick(s, e) {
        //    if (e.buttonIndex == 0) {
        //        $('#mainActMsg').hide();
        //        var FullName = new Array("", "");
        //        shouldCheck = 1;
        //        cMainAccountComboBox.AddItem(FullName, "");
        //        cMainAccountComboBox.SetText("");
        //        cMainAccountpopUp.Show();
        //        cMainAccountComboBox.Focus();

        //    }
        //}
        //function MainAccountSelected(s, e) {

        //    var LookUpData = cMainAccountLookUp.GetGridView().GetRowKey(cMainAccountLookUp.GetGridView().GetFocusedRowIndex());
        //    var MainAccountCode = cMainAccountLookUp.GetValue();
        //    if (!MainAccountCode) {
        //        LookUpData = null;
        //    }
        //    cMainAccountpopUp.Hide();
        //    gridTDS.batchEditApi.StartEdit(globalRowIndex);
        //    gridTDS.batchEditApi.StartEdit(globalRowIndex, 5);
        //    gridTDS.GetEditor("MainAccount").SetText(MainAccountCode);
        //    gridTDS.GetEditor("gvColMainAccount").SetText(LookUpData);

        //}
    </script>
    <script language="javascript" type="text/javascript">



        var chkAccount = 0;
        var ReciptOldValue;
        var ReciptNewValue;
        var PaymentOldValue;
        var PaymentNewValue;

        var GlobargotRecpt = null;
        var GlobargotPayMent = null;
        var oldBranchdata;
        var globalNetAmount = 0;
        var isCtrl = false;
        var SrlNo = 0;

        var NetAmountOldValue;
        var NetAmountNewValue;

        //------------------------------------------------Tax-------------------------------------
        var taxJson;
        var ChargegstcstvatGlobalName;
        var taxAmountGlobal;
        var globalTaxRowIndex;
        var gstcstvatGlobalName;
        var GlobalCurTaxAmt = 0;


        function GlobalBillingShippingEndCallBack() {
            var NoSchemeTypedtl = cCmbScheme.GetValue();
            if (NoSchemeTypedtl != null && NoSchemeTypedtl != '') {
                var branchID = (NoSchemeTypedtl.toString().split('~')[3] != null) ? NoSchemeTypedtl.toString().split('~')[3] : "";
            }
        }  /// this emplty function required for billing/Shipping


        //....................................................End Tax........................................

        function CashBank_GotFocus() {
            //cddlCashBank.ShowDropDown();
        }
        function CashBank_SelectedIndexChanged() {
            //var VoucherType = cComboType.GetValue();
            var VoucherType = document.getElementById('rbtnType').value;
            //if (VoucherType == "P") {
            //    LoadCustomerAddress('', $('#ddlBranch').val(), 'PO');
            //}
            
            //chinmoy edited below code for new billing shipping
            //     LoadCustomerAddress('', $('#ddlBranch').val(), 'PO');
            if ($("#ddlBranch").val() != null && $("#ddlBranch").val() != "") {
                SetPurchaseBillingShippingAddress($('#ddlBranch').val());
            }
            var CashBankId = cddlCashBank.GetValue();

            var CashBankText = cddlCashBank.GetText();
            var arr = CashBankText.split('|');
            var strbranch = $('#<%=hdnBranchId.ClientID %>').val();
            PopulateCurrentBankBalance(arr[0], strbranch);
            $('#MandatoryCashBank').hide();
            var CashBankText = cddlCashBank.GetText();
            var SpliteDetails = CashBankText.split(']');
            var WithDrawType = SpliteDetails[1].trim();
            if (WithDrawType == "Cash") {
                var comboitem = cComboInstrumentTypee.FindItemByValue('C');
                if (comboitem != undefined && comboitem != null) {
                    cComboInstrumentTypee.RemoveItem(comboitem.index);
                }
                var comboitem = cComboInstrumentTypee.FindItemByValue('D');
                if (comboitem != undefined && comboitem != null) {
                    cComboInstrumentTypee.RemoveItem(comboitem.index);
                }
                var comboitem = cComboInstrumentTypee.FindItemByValue('E');
                if (comboitem != undefined && comboitem != null) {
                    cComboInstrumentTypee.RemoveItem(comboitem.index);

                }
                var comboitem = cComboInstrumentTypee.FindItemByValue('CH');
                if (comboitem == undefined || comboitem == null) {
                    cComboInstrumentTypee.AddItem("Cash", "CH");
                }
                cComboInstrumentTypee.SetValue("CH");
                InstrumentTypeSelectedIndexChanged();
            }
            else {
                var comboitem = cComboInstrumentTypee.FindItemByValue('C');
                if (comboitem == undefined || comboitem == null) {
                    cComboInstrumentTypee.AddItem("Cheque", "C");
                }
                var comboitem = cComboInstrumentTypee.FindItemByValue('D');
                if (comboitem == undefined || comboitem == null) {
                    cComboInstrumentTypee.AddItem("Draft", "D");
                }
                var comboitem = cComboInstrumentTypee.FindItemByValue('E');
                if (comboitem == undefined || comboitem == null) {
                    cComboInstrumentTypee.AddItem("E.Transfer", "E");
                }
                var comboitem = cComboInstrumentTypee.FindItemByValue('CH');
                if (comboitem != undefined && comboitem != null) {
                    cComboInstrumentTypee.RemoveItem(comboitem.index);
                    cComboInstrumentTypee.SetValue("C");
                    InstrumentTypeSelectedIndexChanged();
                }
            }
        }
        function CashBank_EndCallback() {

            var CashBankId = $('#<%=hdnCashBankId.ClientID %>').val();
            cddlCashBank.SetValue(CashBankId);
            var CashBankText = cddlCashBank.GetText();
            var arr = CashBankText.split('|');
            var strbranch = $('#<%=hdnBranchId.ClientID %>').val();
            PopulateCurrentBankBalance(arr[0], strbranch);
        }
        function VoucherType_GotFocus() {
            //cComboType.ShowDropDown();
        }
        function NumberingScheme_GotFocus() {
            //cCmbScheme.ShowDropDown();
        }
        function ReloadPage() {
            //sessionStorage.removeItem('CashBankDetails');
            $('#<%=hdnEditRfid.ClientID %>').val('');
            window.location.assign("CashBankEntryList.aspx");
            // cacpCrossBtn.PerformCallback();
        }
        function acpCrossBtnEndCall() {
            window.location.reload();
        }
        var isFirstTime = true;
        function AllControlInitilize() {
            // document.getElementById('AddButton').style.display = 'inline-block';
            if (isFirstTime) {
                $("#TaxAmountOngrid").val("");
                $("#VisibleIndexForTax").val("");
                if ($('#hdn_Mode').val() != "Edit") {
                    //document.getElementById('rbtnType').value = 'P';
                    AddButtonClick();

                }
                else {
                    UpdateTrColor();
                }

                //OnAddNewClick();
                //if (localStorage.getItem('FromDateCashBank')) {
                //    var fromdatearray = localStorage.getItem('FromDateCashBank').split('-');
                //    var fromdate = new Date(fromdatearray[0], parseFloat(fromdatearray[1]) - 1, fromdatearray[2], 0, 0, 0, 0);
                //    cFormDate.SetDate(fromdate);
                //}

                //if (localStorage.getItem('ToDateCashBank')) {
                //    var todatearray = localStorage.getItem('ToDateCashBank').split('-');
                //    var todate = new Date(todatearray[0], parseFloat(todatearray[1]) - 1, todatearray[2], 0, 0, 0, 0);
                //    ctoDate.SetDate(todate);
                //}
                //if (localStorage.getItem('BranchCashBank')) {
                //    if (ccmbBranchfilter.FindItemByValue(localStorage.getItem('BranchCashBank'))) {
                //        ccmbBranchfilter.SetValue(localStorage.getItem('BranchCashBank'));
                //    }

                //}


                isFirstTime = false;
            }
        }

        $(document).ready(function () {
            //UpdateTrColor();
        });

        function UpdateTrColor() {
            for (var i = 0; i < 1000; i++) {
                if (gridTDS.GetRow(i)) {
                    if (gridTDS.GetRow(i).style.display != "none") {
                        gridTDS.batchEditApi.StartEdit(i, 2);
                        var row = gridTDS.GetRow(i);

                        if (row.children[11].innerText.trim() != "" && row.children[11].innerText.trim() != null) {
                            $(row).addClass(" rowRed");
                        }
                    }
                }
            }

            for (i = -1; i > -1000; i--) {
                if (gridTDS.GetRow(i)) {
                    if (gridTDS.GetRow(i).style.display != "none") {
                        gridTDS.batchEditApi.StartEdit(i, 2);
                        var row = gridTDS.GetRow(i);

                        if (row.children[11].innerText.trim() != "" && row.children[11].innerText.trim() != null) {
                            $(row).addClass(" rowRed");
                        }

                    }
                }
            }
        }

        function GetVisibleIndex(s, e) {
            globalRowIndex = e.visibleIndex;
            EnableOrDisableTax();
        }
        function ddlBranch_SelectedIndexChanged() {

            if ($("#hdnProjectSelectInEntryModule").val() == "1")
                clookup_Project.gridView.Refresh();
            var branch = $("#ddlBranch").val();
            gridTDS.batchEditApi.StartEdit(-1, 1);
            var accountingDataMin = gridTDS.GetEditor('MainAccount').GetValue();
            gridTDS.batchEditApi.EndEdit();

            gridTDS.batchEditApi.StartEdit(0, 1);
            var accountingDataplus = gridTDS.GetEditor('MainAccount').GetValue();
            gridTDS.batchEditApi.EndEdit();

            if (accountingDataMin != null || accountingDataplus != null) {
                jConfirm('You have changed Branch. All the entries of ledger in this voucher to be reset to blank. \n You have to select and re-enter. Continue?', 'Confirmation Dialog', function (r) {

                    if (r == true) {
                        deleteAllRows();
                        gridTDS.AddNewRow();

                        MainAccount.PerformCallback(branch)
                    }
                });
            }
            else {
                MainAccount.PerformCallback(branch)
            }
        }
        //function chkValidConta(contano_status) {
        //    if (contano_status == "outrange") {
        //        jAlert('Can Not Add More Cash/Bank Voucher as Contra Scheme Exausted.<br />Update The Scheme and Try Again');
        //    } else if (contano_status == "duplicate") {
        //        jAlert('Can Not Save as Duplicate Contra Voucher No. Found');
        //    }
        //    return false;
        //}
        function OnKeyDown(s, e) {
            if (e.htmlEvent.keyCode == 40 || e.htmlEvent.keyCode == 38)
                return ASPxClientUtils.PreventEvent(e.htmlEvent);
        }
        function CreditGotFocus(s, e) {
            PaymentOldValue = s.GetText();
            NetAmountOldValue = gridTDS.GetEditor("NetAmount").GetValue();
            var indx = PaymentOldValue.indexOf(',');
            if (indx != -1) {
                PaymentOldValue = PaymentOldValue.replace(/,/g, '');
            }
        }
        function Payment_Lost_Focus(s, e) {
            PaymentNewValue = s.GetText();
            var indx = PaymentNewValue.indexOf(',');
            if (indx != -1) {
                PaymentNewValue = PaymentNewValue.replace(/,/g, '');
            }

            if (PaymentOldValue != PaymentNewValue) {
                changePaymentTotalSummary();
            }
        }
        function recalculateReceipt(oldVal) {
            if (oldVal != 0) {
                ReciptNewValue = 0;
                ReciptOldValue = oldVal;
                changeReciptTotalSummary();
            }
        }









        function PaymentTextChange(s, e) {
            Payment_Lost_Focus(s, e);
            var PaymentValue = (gridTDS.GetEditor('btnPayment').GetValue() != null) ? gridTDS.GetEditor('btnPayment').GetValue() : "0";
            var ReceiptValue = (gridTDS.GetEditor('btnRecieve').GetValue() != null) ? parseFloat(gridTDS.GetEditor('btnRecieve').GetValue()) : "0";

            if (PaymentValue > 0) {
                recalculateReceipt(gridTDS.GetEditor('btnRecieve').GetValue());
                gridTDS.GetEditor('btnRecieve').SetValue("0");
                if (PaymentValue != PaymentOldValue) {
                    gridTDS.GetEditor('TaxAmount').SetValue("0");
                }
                gridTDS.GetEditor('NetAmount').SetValue(PaymentValue);
            }
            $("#HdProdGrossAmt").val(PaymentValue);

            var MainAccountID = (gridTDS.GetEditor('gvColMainAccount').GetValue() != null) ? gridTDS.GetEditor('gvColMainAccount').GetValue() : "0";


            var VoucherType = document.getElementById('rbtnType').value;
            //debugger;
            if (VoucherType == "P") {
                $.ajax({
                    type: "POST",
                    url: "CashBankEntryTDS.aspx/GetTotalBalanceByToDay",
                    data: JSON.stringify({ MainAccountID: MainAccountID }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (msg) {
                        var data = msg.d;
                        var VoucherAmount = data.toString().split('~')[0];
                        var BalanceLimit = data.toString().split('~')[1];
                        var BalanceExceed = data.toString().split('~')[2];
                        if (BalanceLimit != '0.00') {
                            var TotalVoucherAmount = parseFloat(PaymentValue) + parseFloat(VoucherAmount);
                            console.log(TotalVoucherAmount);

                            if (parseFloat(TotalVoucherAmount) > parseFloat(BalanceLimit)) {

                                if (BalanceExceed.trim() == 'W') {

                                    jConfirm('Daily Balance - Limit is exceed do you want to proceed?', 'Confirmation Dialog', function (r) {
                                        if (r == true) {

                                        }
                                        else {
                                            gridTDS.GetEditor('btnPayment').SetValue("0");
                                            gridTDS.GetEditor('NetAmount').SetValue("0");
                                        }
                                    });
                                }
                                else if (BalanceExceed.trim() == 'B') {
                                    jAlert('Daily Balance - Limit is exceed can not proceed');
                                    gridTDS.GetEditor('btnPayment').SetValue("0");
                                    gridTDS.GetEditor('NetAmount').SetValue("0");

                                }
                                else if (BalanceExceed.trim() == 'S') {
                                    jAlert('Please select Daily Balance - Limit exceed option.');
                                    gridTDS.GetEditor('btnPayment').SetValue("0");
                                    gridTDS.GetEditor('NetAmount').SetValue("0");

                                }
                                else if (BalanceExceed.trim() == '') {
                                    jAlert('Please select Daily Balance - Limit exceed option.');
                                    gridTDS.GetEditor('btnPayment').SetValue("0");
                                    gridTDS.GetEditor('NetAmount').SetValue("0");

                                }
                                else if (BalanceExceed.trim() == 'I') {

                                }
                            }
                            else {
                                //OnAddNewClick();
                                //cbtnSaveNew.SetVisible(false);
                                //cbtnSaveRecords.SetVisible(false);

                                //gridTDS.UpdateEdit();
                                //chkAccount = 0;
                            }
                        }
                        else {
                            //OnAddNewClick();
                            //cbtnSaveNew.SetVisible(false);
                            //cbtnSaveRecords.SetVisible(false);

                            //gridTDS.UpdateEdit();
                            //chkAccount = 0;
                        }

                    }
                });


            }

        }
        function DebitGotFocus(s, e) {
            ReciptOldValue = s.GetText();
            var indx = ReciptOldValue.indexOf(',');
            if (indx != -1) {
                ReciptOldValue = ReciptOldValue.replace(/,/g, '');
            }
        }


        ////////////////////////// TDS Start ////////////////////////////////////////////

        function MainAccountKeyDownTDS(s, e) {
            if (e.htmlEvent.key == "Enter") {

                s.OnButtonClick(0);
                //$('#MainAccountModel').modal('show');
            }
            //if (e.htmlEvent.key == "Tab") {

            //    s.OnButtonClick(0);
            //}
        }

        function SubAccountKeyDownTDS(s, e) {
            $("#mainActMsgSub").hide();
            if (e.htmlEvent.key == "Enter") {

                s.OnButtonClick(0);
            }
            if (e.htmlEvent.key == "Delete") {

                var subAccountText = "";
                var subAccountID = "";
                gridTDS.batchEditApi.StartEdit(globalRowIndexTDS, 3);

                gridTDS.GetEditor("bthSubAccountTDS").SetText(subAccountText);
                gridTDS.GetEditor("gvColSubAccountTDS").SetText(subAccountID);
                //cSubAcountComboBox.Hide();
                setTimeout(function () { gridTDS.batchEditApi.StartEdit(globalRowIndexTDS, 2); }, 500);
            }


            // if (e.htmlEvent.key == "Tab") {

            //   s.OnButtonClick(0);
            //}
        }


        function WithDrawlTextChangeTDS(s, e) {
            var mainAccountValue = (gridTDS.GetEditor('MainAccountTDS').GetValue() != null) ? gridTDS.GetEditor('MainAccountTDS').GetValue() : "";
            var uniqueIndex = globalRowIndexTDS;
            if (mainAccountValue != "") {
                //DebitLostFocusTDS(s, e);
                var withDrawlValue = (gridTDS.GetEditor('WithDrawlTDS').GetValue() != null) ? parseFloat(gridTDS.GetEditor('WithDrawlTDS').GetValue()) : "0";
                var receiptValue = (gridTDS.GetEditor('ReceiptTDS').GetValue() != null) ? gridTDS.GetEditor('ReceiptTDS').GetValue() : "0";

                if (withDrawlValue > 0) {
                    gridTDS.GetEditor('ReceiptTDS').SetValue("0");
                }

                var Amount = (gridTDS.GetEditor('WithDrawlTDS').GetValue() != null) ? parseFloat(gridTDS.GetEditor('WithDrawlTDS').GetValue()) : "0";
                var UniqueID = gridTDS.GetEditor('UniqueID').GetText();// gridTDS.batchEditApi.GetCellValue(e.visibleIndex, "UniqueID");
                UpdateTDSValue(UniqueID, Amount);

                changeDebitTotalSummaryTDS();

                //Rev Tanmoy
                var MainAccountID = (gridTDS.GetEditor('gvColMainAccountTDS').GetValue() != null) ? gridTDS.GetEditor('gvColMainAccountTDS').GetValue() : "0";
                var VoucherType = document.getElementById('rbtnType').value;
                //debugger;
                if (VoucherType == "P") {
                    $.ajax({
                        type: "POST",
                        url: "CashBankEntryTDS.aspx/GetTotalBalanceByToDay",
                        data: JSON.stringify({ MainAccountID: MainAccountID }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,
                        success: function (msg) {
                            var data = msg.d;
                            var VoucherAmount = data.toString().split('~')[0];
                            var BalanceLimit = data.toString().split('~')[1];
                            var BalanceExceed = data.toString().split('~')[2];
                            if (BalanceLimit != '0.00') {
                                var TotalVoucherAmount = parseFloat(Amount) + parseFloat(VoucherAmount);
                                console.log(TotalVoucherAmount);

                                if (parseFloat(TotalVoucherAmount) > parseFloat(BalanceLimit)) {

                                    if (BalanceExceed.trim() == 'W') {

                                        jConfirm('Daily Balance - Limit is exceed do you want to proceed?', 'Confirmation Dialog', function (r) {
                                            if (r == true) {

                                            }
                                            else {
                                                gridTDS.GetEditor('WithDrawlTDS').SetValue("0");
                                                //gridTDS.GetEditor('NetAmount').SetValue("0");
                                            }
                                        });
                                    }
                                    else if (BalanceExceed.trim() == 'B') {
                                        jAlert('Daily Balance - Limit is exceed can not proceed');
                                        gridTDS.GetEditor('WithDrawlTDS').SetValue("0");
                                        // gridTDS.GetEditor('NetAmount').SetValue("0");
                                    }
                                    else if (BalanceExceed.trim() == 'S') {
                                        jAlert('Please select Daily Balance - Limit exceed option.');
                                        gridTDS.GetEditor('WithDrawlTDS').SetValue("0");
                                        // gridTDS.GetEditor('NetAmount').SetValue("0");

                                    }
                                    else if (BalanceExceed.trim() == '') {
                                        jAlert('Please select Daily Balance - Limit exceed option.');
                                        gridTDS.GetEditor('WithDrawlTDS').SetValue("0");
                                        // gridTDS.GetEditor('NetAmount').SetValue("0");
                                    }
                                    else if (BalanceExceed.trim() == 'I') {
                                    }
                                }
                                else {
                                }
                            }
                            else {
                            }
                        }
                    });
                }
                //End Rev Tanmoy


                var Debit = parseFloat(c_txt_Debit.GetValue());
                var Credit = parseFloat(c_txt_Credit.GetValue());

                if (Debit == 0 && Credit == 0) {
                    //cbtnSaveRecordsTDS.SetVisible(false);
                    //cbtn_SaveRecordsTDS.SetVisible(false);
                    var Amount = parseFloat(Debit) - parseFloat(Credit);
                }
                else if (Debit == Credit) {
                    // cbtnSaveRecordsTDS.SetVisible(true);
                    // cbtn_SaveRecordsTDS.SetVisible(true);
                }
                else {
                    //cbtnSaveRecordsTDS.SetVisible(false);
                    //cbtn_SaveRecordsTDS.SetVisible(false);

                    var Amount = parseFloat(Debit) - parseFloat(Credit);
                }
                if ($("#cpTaggedTDS").val() == "-99") {
                    //cbtnSaveRecordsTDS.SetVisible(false);
                    //cbtn_SaveRecordsTDS.SetVisible(false);

                }




            }
            else {
                gridTDS.GetEditor('WithDrawlTDS').SetValue("0");
            }

            setTimeout(function () { gridTDS.batchEditApi.StartEdit(uniqueIndex, 4); }, 500);

        }


        function UpdateTDSValue(UniqueID, Amount) {
            for (var i = 0; i < 1000; i++) {
                if (gridTDS.GetRow(i)) {
                    if (gridTDS.GetRow(i).style.display != "none") {
                        gridTDS.batchEditApi.StartEdit(i, 2);
                        var ISTDS = gridTDS.batchEditApi.GetCellValue(i, "IsTDS");
                        if (gridTDS.GetEditor("UniqueID").GetText() == UniqueID && ISTDS != "" && ISTDS != null) {
                            var TDSPercentage = gridTDS.batchEditApi.GetCellValue(i, "TDSPercentage");
                            var newamt = DecimalRoundoff(parseFloat(Amount), 2) * (parseFloat(TDSPercentage) / 100);
                            gridTDS.batchEditApi.SetCellValue(i, "ReceiptTDS", newamt);
                        }
                    }
                }
            }

            for (i = -1; i > -1000; i--) {
                if (gridTDS.GetRow(i)) {
                    if (gridTDS.GetRow(i).style.display != "none") {
                        gridTDS.batchEditApi.StartEdit(i, 2);
                        var ISTDS = gridTDS.batchEditApi.GetCellValue(i, "IsTDS");
                        if (gridTDS.GetEditor("UniqueID").GetText() == UniqueID && ISTDS != "" && ISTDS != null) {
                            var TDSPercentage = gridTDS.batchEditApi.GetCellValue(i, "TDSPercentage");
                            var newamt = DecimalRoundoff(parseFloat(Amount), 2) * (parseFloat(TDSPercentage) / 100);
                            gridTDS.batchEditApi.SetCellValue(i, "ReceiptTDS", newamt);
                        }
                    }
                }
            }
        }


        function CreditGotFocusTDS(s, e) {
            CreditOldValue = s.GetText();
            var indx = CreditOldValue.indexOf(',');
            if (indx != -1) {
                CreditOldValue = CreditOldValue.replace(/,/g, '');
            }
        }



        function ReceiptTextChangeTDS(s, e) {


            var IsTDSSource = gridTDS.batchEditApi.GetCellValue(globalRowIndexTDS, "IsTDSSource");
            var uniqueIndex = globalRowIndexTDS;
            if (IsTDSSource == "1") {

                gridTDS.GetEditor('ReceiptTDS').SetValue("0");
                setTimeout(function () { gridTDS.batchEditApi.StartEdit(uniqueIndex, 5); }, 200);
                return;
            }
            var receiptValue = (gridTDS.GetEditor('ReceiptTDS').GetValue() != null) ? gridTDS.GetEditor('ReceiptTDS').GetValue() : "0";
            if (receiptValue > 0) {

                gridTDS.GetEditor('WithDrawlTDS').SetValue("0");
            }

            var mainAccountValue = (gridTDS.GetEditor('MainAccountTDS').GetValue() != null) ? gridTDS.GetEditor('MainAccountTDS').GetValue() : "";


            if (mainAccountValue != "") {
                //CreditLostFocus(s, e);
                //var receiptValue = (gridTDS.GetEditor('ReceiptTDS').GetValue() != null) ? gridTDS.GetEditor('ReceiptTDS').GetValue() : "0";
                //var withDrawlValue = (gridTDS.GetEditor('WithDrawlTDS').GetValue() != null) ? parseFloat(gridTDS.GetEditor('WithDrawlTDS').GetValue()) : "0";

                //if (receiptValue > 0) {
                //    recalculateDebitTDS(gridTDS.GetEditor('WithDrawlTDS').GetValue());
                //    gridTDS.GetEditor('WithDrawlTDS').SetValue("0");

                //    //gridTDS.GetEditor('WithDrawl').SetEnabled(false);
                //}

                changeCreditTotalSummaryTDS();


                var Debit = parseFloat(c_txt_Debit.GetValue());
                var Credit = parseFloat(c_txt_Credit.GetValue());

                if (Debit == 0 && Credit == 0) {
                    // cbtnSaveRecordsTDS.SetVisible(false);
                    // cbtn_SaveRecordsTDS.SetVisible(false);

                    var Amount = parseFloat(Debit) - parseFloat(Credit);
                }
                else if (Debit == Credit) {
                    // cbtnSaveRecordsTDS.SetVisible(true);
                    //  cbtn_SaveRecordsTDS.SetVisible(true);
                }
                else {
                    //  cbtnSaveRecordsTDS.SetVisible(false);
                    //  cbtn_SaveRecordsTDS.SetVisible(false);

                    var Amount = parseFloat(Debit) - parseFloat(Credit);
                }
                if ($("#cpTagged").val() == "-99") {
                    //  cbtnSaveRecordsTDS.SetVisible(false);
                    //  cbtn_SaveRecordsTDS.SetVisible(false);

                }
            }
            else {
                gridTDS.GetEditor('ReceiptTDS').SetValue("0");
            }
            setTimeout(function () { gridTDS.batchEditApi.StartEdit(uniqueIndex, 5); }, 200);
        }


        function AddBatchNewTDS(s, e) {
            console.log(e);
            var keyCode = ASPxClientUtils.GetKeyCode(e.htmlEvent);
            var noofvisiblerows = gridTDS.GetVisibleRowsOnPage();
            // var row = grid.GetVisibleIndex();
            if ((keyCode === 13)) {
                var mainAccountValue = (gridTDS.GetEditor('MainAccountTDS').GetValue() != null) ? gridTDS.GetEditor('MainAccountTDS').GetValue() : "";
                if (mainAccountValue != "") {
                    gridTDS.AddNewRow();
                    //grid.SetFocusedRowIndex(globalRowIndex,1);
                    //grid.GetEditor("MainAccount").Focus(); // grid.SetFocusedRowIndex();
                    // return;

                    setTimeout(function () { gridTDS.batchEditApi.StartEdit(globalRowIndexTDS, 1); }, 500);

                }
            }
            else if (keyCode === 9) {
                // setTimeout(function () { grid.batchEditApi.StartEdit(globalRowIndex, 1); }, 500);
                document.getElementById("txtNarrationTDS").focus();
            }
        }

        function OnInitTDS(s, e) {
            IntializeGlobalVariablesTDS(s);

        }
        function IntializeGlobalVariablesTDS(gridTDS) {
            lastCountryID = gridTDS.cplastCountryID;
            currentEditableVisibleIndexTDS = -1;
            setValueFlagTDS = -1;
        }

        function changeCreditTotalSummaryTDS() {
            //debugger;
            var newDif = CreditOldValue - CreditNewValue;
            var CurrentSum = c_txt_Credit.GetText();
            var indx = CurrentSum.indexOf(',');
            if (indx != -1) {
                CurrentSum = CurrentSum.replace(/,/g, '');
            }


            var DebitAmount = 0;
            var CreditAmount = 0;

            for (var i = 0; i < 1000; i++) {
                if (gridTDS.GetRow(i)) {
                    if (gridTDS.GetRow(i).style.display != "none") {
                        gridTDS.batchEditApi.StartEdit(i, 2);
                        DebitAmount = DecimalRoundoff(parseFloat(DebitAmount), 2) + DecimalRoundoff(parseFloat(gridTDS.batchEditApi.GetCellValue(i, "WithDrawlTDS")), 2);
                        CreditAmount = DecimalRoundoff(parseFloat(CreditAmount), 2) + DecimalRoundoff(parseFloat(gridTDS.batchEditApi.GetCellValue(i, "ReceiptTDS")), 2);
                    }
                }
            }


            for (i = -1; i > -1000; i--) {
                if (gridTDS.GetRow(i)) {
                    if (gridTDS.GetRow(i).style.display != "none") {
                        gridTDS.batchEditApi.StartEdit(i, 2);
                        DebitAmount = DecimalRoundoff(parseFloat(DebitAmount), 2) + DecimalRoundoff(parseFloat(gridTDS.batchEditApi.GetCellValue(i, "WithDrawlTDS")), 2);
                        CreditAmount = DecimalRoundoff(parseFloat(CreditAmount), 2) + DecimalRoundoff(parseFloat(gridTDS.batchEditApi.GetCellValue(i, "ReceiptTDS")), 2);
                    }
                }
            }
            c_txt_Debit.SetValue(parseFloat(DecimalRoundoff((CurrentSum - newDif), 2)));

            //Rev Tanmoy
            //var noofvisiblerows = gridTDS.GetVisibleRowsOnPage();
            //var i, cnt = 1;

            //for (i = -1 ; cnt <= noofvisiblerows ; i--) {
            //    var Payment = (gridTDS.batchEditApi.GetCellValue(i, 'WithDrawlTDS') != null) ? (gridTDS.batchEditApi.GetCellValue(i, 'WithDrawlTDS')) : "0";
            //    var Recieve = (gridTDS.batchEditApi.GetCellValue(i, 'ReceiptTDS') != null) ? (gridTDS.batchEditApi.GetCellValue(i, 'ReceiptTDS')) : "0";

            //    DebitAmount = DebitAmount + parseFloat(Payment);
            //    CreditAmount = CreditAmount + parseFloat(Recieve);

            //    cnt++;
            //}

            //End Rev Tanmoy
            c_txt_Debit.SetValue(parseFloat(DecimalRoundoff(DebitAmount, 2)));
            c_txt_Credit.SetValue(parseFloat(DecimalRoundoff(CreditAmount, 2)));
        }
        function recalculateCreditTDS(oldVal) {
            if (oldVal != 0) {
                CreditNewValue = 0;
                CreditOldValue = oldVal;
                changeCreditTotalSummaryTDS();
            }
        }

        function GetDebitCredit(UniqueKey) {
            for (var i = 0; i < 1000; i++) {
                if (gridTDS.GetRow(i)) {
                    if (gridTDS.GetRow(i).style.display != "none") {
                        if (gridTDS.batchEditApi.GetCellValue(i, "UniqueID") == UniqueKey && gridTDS.batchEditApi.GetCellValue(i, "IsTDS") != "" && gridTDS.batchEditApi.GetCellValue(i, "IsTDS") != null) {
                            return gridTDS.batchEditApi.GetCellValue(i, "WithDrawlTDS") + "~" + gridTDS.batchEditApi.GetCellValue(i, "ReceiptTDS");
                        }
                    }
                }
            }

            for (i = -1; i > -1000; i--) {
                if (gridTDS.GetRow(i)) {
                    if (gridTDS.GetRow(i).style.display != "none") {
                        if (gridTDS.batchEditApi.GetCellValue(i, "UniqueID") == UniqueKey && gridTDS.batchEditApi.GetCellValue(i, "IsTDS") != "" && gridTDS.batchEditApi.GetCellValue(i, "IsTDS") != null) {
                            return gridTDS.batchEditApi.GetCellValue(i, "WithDrawlTDS") + "~" + gridTDS.batchEditApi.GetCellValue(i, "ReceiptTDS");
                        }
                    }
                }
            }
        }


        function SubAccountNewkeydownTDS(e) {
            gridTDS.batchEditApi.StartEdit(e.visibleIndex);
            var strMainAccountID = (gridTDS.GetEditor('MainAccountTDS').GetText() != null) ? gridTDS.GetEditor('MainAccountTDS').GetText() : "0";
            var MainAccountID = (gridTDS.GetEditor('gvColMainAccountTDS').GetValue() != null) ? gridTDS.GetEditor('gvColMainAccountTDS').GetValue() : "0";

            var OtherDetails = {}
            OtherDetails.SearchKey = $("#txtSubAccountSearchTDS").val();
            OtherDetails.MainAccountCode = MainAccountID;
            if (e.code == "Enter" || e.code == "NumpadEnter") {
                if ($("#txtSubAccountSearchTDS").val() == "")
                    return;
                var HeaderCaption = [];
                HeaderCaption.push("Sub Account Name [Unique Id]");
                HeaderCaption.push("Subledger Type");

                callonServer("/OMS/Management/Activities/Services/Master.asmx/GetSubAccountJournal", OtherDetails, "SubAccountTableTDS", HeaderCaption, "SubAccountIndexTDS", "SetSubAccountTDS");
            }
            else if (e.code == "ArrowDown") {
                if ($("input[SubAccountIndexTDS=0]"))
                    $("input[SubAccountIndexTDS=0]").focus();
            }
            else if (e.code == "Escape") {
                $('#SubAccountModelTDS').modal('hide');
                gridTDS.batchEditApi.StartEdit(globalRowIndexTDS, 2);
            }
        }

        function MainAccountButnClickTDS(s, e) {




            var txt = "<table border='1' width=\"100%\"><tr class=\"HeaderStyle\"><th>Main Account Name</th><th>Short Name</th><th>Subledger Type</th><th>TDS/TCS</th></tr><table>";
            document.getElementById("MainAccountTableTDS").innerHTML = txt;

            if (e.buttonIndex == 0) {
                $('#mainActMsgTDS').hide();
                var FullName = new Array("", "");

                var MainAccountID = gridTDS.GetEditor("gvMainAcCodeTDS").GetValue();
                if (MainAccountID == 'Customers' || MainAccountID == 'Vendors') {
                    if ($("#hdnIsPartyLedger").val() == "") {
                        $("#hdnIsPartyLedger").val('1');
                    }
                    else {
                        $("#hdnIsPartyLedger").val(parseFloat($("#hdnIsPartyLedger").val()) - 1);
                    }

                }





                shouldCheck = 1;
                //cMainAccountComboBox.AddItem(FullName, "");
                //cMainAccountComboBox.SetText("");
                gridTDS.batchEditApi.StartEdit(e.visibleIndex);
                var strMainAccountID = (gridTDS.GetEditor('MainAccountTDS').GetText() != null) ? gridTDS.GetEditor('MainAccountTDS').GetText() : "0";

                LastCr = parseFloat(gridTDS.GetEditor('ReceiptTDS').GetText());
                LastDr = parseFloat(gridTDS.GetEditor('WithDrawlTDS').GetText());

                if (strMainAccountID != "") {
                    var strMainAccountID = "PREVIOUS MAIN ACCOUNT :" + strMainAccountID;

                }
                // LabelMainAccount.SetText(strMainAccountID);
                $("#LabelMainAccountTDS").val(strMainAccountID);
                ////cMainAccountpopUp.Show();
                $('#MainAccountModelTDS').modal('show');
                //cMainAccountComboBox.Focus();

            }
        }

        function SubAccountButnClickTDS(s, e) {


            txt = " <table border='1' width=\"100%\"><tr class=\"HeaderStyle\"><th>Sub Account Name [Unique Id]</th><th>Sub Account Code</th></tr></table>";
            document.getElementById("SubAccountTableTDS").innerHTML = txt;
            var SubAcc = gridTDS.GetEditor('IsSubledgerTDS');
            IsSubAccount = SubAcc.GetText();
            $("#mainActMsgSubTDS").hide();
            if (IsSubAccount != 'None') {
                gridTDS.batchEditApi.StartEdit(e.visibleIndex);
                var strMainAccountID = (gridTDS.GetEditor('MainAccountTDS').GetText() != null) ? gridTDS.GetEditor('MainAccountTDS').GetText() : "0";
                var MainAccountID = (gridTDS.GetEditor('gvColMainAccountTDS').GetValue() != null) ? gridTDS.GetEditor('gvColMainAccountTDS').GetValue() : "0";

                //Add for found Main Accoun type Tanmoy
                var MainAccountType = "";
                $.ajax({
                    type: "POST",
                    url: 'CashBankEntryTDS.aspx/getMainAccountType',
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    dataType: "json",
                    data: JSON.stringify({ MainAccountCode: MainAccountID }),
                    success: function (msg) {
                        var data = msg.d;
                        MainAccountType = msg.d;
                    }
                });
                //Add for found Main Accoun type Tanmoy
                if (MainAccountType != "Lead" || $("#hdnIsLeadAvailableinTransactions").val() == "Yes") {
                    if (e.buttonIndex == 0) {
                        if (strMainAccountID.trim() != "") {
                            document.getElementById('hdnMainAccountIdTDS').value = MainAccountID;
                            var FullName = new Array("", "");
                            //cSubAcountComboBox.AddItem(FullName, "");
                            //cSubAcountComboBox.SetValue("");
                            $('#SubAccountModelTDS').modal('show');
                            ////cSubAcountComboBox.Show();
                            //cSubAcountComboBox.Focus();
                            var strSubLBLAccountID = (gridTDS.GetEditor('bthSubAccountTDS').GetText() != null) ? gridTDS.GetEditor('bthSubAccountTDS').GetText() : "0";
                            if (strSubLBLAccountID != "") {
                                var strSubLBLAccountID = "Previous Sub Account :" + strSubLBLAccountID;

                            }
                            // $("#LabelMainAccount").val(strSubLBLAccountID);
                        }
                    }
                }
            }
        }


        function OnKeyDownTDS(s, e) {



            if (e.htmlEvent.keyCode == 40 || e.htmlEvent.keyCode == 38)
                return ASPxClientUtils.PreventEvent(e.htmlEvent);
        }


        function OnEndCallbackTDS(s, e) {
            IntializeGlobalVariables(s);
            LoadingPanel.Hide();
            var pageStatus = document.getElementById('hdnPageStatus').value;

            var ViewStatus = document.getElementById('hdnView').value;

            if ($('#<%=hdnPayment.ClientID %>').val() == "YES") {
                $('#<%=hdnPayment.ClientID %>').val("NO");
                OnAddNewClick();
            }
            if (gridTDS.cpTotalAmount != null) {
                var total_receipt = gridTDS.cpTotalAmount.split('~')[0];
                var total_payment = gridTDS.cpTotalAmount.split('~')[1];
                c_txt_Debit.SetValue(total_receipt);
                ctxtTotalPayment.SetValue(total_payment);
                gridTDS.cpTotalAmount = null;
            }


            if (gridTDS.cpSaveSuccessOrFail == "outrange") {
                gridTDS.cpSaveSuccessOrFail = null;
                OnAddNewClick();
                chkAccount = 1;
                jAlert('The Numbering Scheme has exhausted, please update the Scheme and try adding the Voucher');
                cbtnSaveNew.SetVisible(true);
                cbtnSaveRecords.SetVisible(true);
                //document.getElementById('btnSaveNew').style.display = 'none';
                //document.getElementById('btnSaveRecords').style.display = 'none';
            }
            else if (gridTDS.cpSaveSuccessOrFail == "duplicate") {
                gridTDS.cpSaveSuccessOrFail = null;
                OnAddNewClick();
                chkAccount = 1;
                jAlert('Can not save as duplicate Voucher No.');
                cbtnSaveNew.SetVisible(true);
                cbtnSaveRecords.SetVisible(true);
                //document.getElementById('btnSaveNew').style.display = 'none';
                //document.getElementById('btnSaveRecords').style.display = 'none';
            }
            else if (gridTDS.cpSaveSuccessOrFail == "zeroAmount") {
                gridTDS.cpSaveSuccessOrFail = null;
                OnAddNewClick();
                chkAccount = 1;
                jAlert('Cannot save with ZERO Amount.');
                cbtnSaveNew.SetVisible(true);
                cbtnSaveRecords.SetVisible(true);
            }
            else if (gridTDS.cpSaveSuccessOrFail == "SubAccountMandatory") {
                gridTDS.cpSaveSuccessOrFail = null;
                OnAddNewClick();
                chkAccount = 1;
                jAlert('Sub-account ledger selection is set as mandatory in System Settings.\n Please select Sub-account ledger to proceed.');
                cbtnSaveNew.SetVisible(true);
                cbtnSaveRecords.SetVisible(true);
            }
            else if (gridTDS.cpSaveSuccessOrFail == "errorInsert") {
                gridTDS.cpSaveSuccessOrFail = null;
                OnAddNewClick();
                chkAccount = 1;
                jAlert('Try again later.');
                cbtnSaveNew.SetVisible(true);
                cbtnSaveRecords.SetVisible(true);
                //document.getElementById('btnSaveNew').style.display = 'none';
                //document.getElementById('btnSaveRecords').style.display = 'none';
            }
            else if (gridTDS.cpSaveSuccessOrFail == "EmptyProject") {
                gridTDS.cpSaveSuccessOrFail = null;
                OnAddNewClick();
                chkAccount = 1;
                jAlert('Please select project');
                cbtnSaveNew.SetVisible(true);
                cbtnSaveRecords.SetVisible(true);
              
            }
            else if (gridTDS.cpSaveSuccessOrFail == "AddLock") {
                gridTDS.cpSaveSuccessOrFail = null;
                OnAddNewClick();
                chkAccount = 1;
                jAlert('DATA is Freezed between ' + gridTDS.cpAddLockStatus + ' for Add.');
                cbtnSaveNew.SetVisible(true);
                cbtnSaveRecords.SetVisible(true);

            }
            else if (gridTDS.cpSaveSuccessOrFail == "mixedvalue") {
                gridTDS.cpSaveSuccessOrFail = null;
                OnAddNewClick();
                chkAccount = 1;
                jAlert('You must select all the ledgers either with Reverse Charge Applicable or all the ledgers </br>without Reverse charge applicable in a single entry to Save this entry.');
                cbtnSaveNew.SetVisible(true);
                cbtnSaveRecords.SetVisible(true);
                //document.getElementById('btnSaveNew').style.display = 'none';
                //document.getElementById('btnSaveRecords').style.display = 'none';

            }
            else if (gridTDS.cpSaveSuccessOrFail == "reverserequired") {
                gridTDS.cpSaveSuccessOrFail = null;
                OnAddNewClick();
                chkAccount = 1;
                jAlert('Selected Ledger(s) are mapped with Reverse Charge, please check Reverse Charge option.');
                cbtnSaveNew.SetVisible(true);
                cbtnSaveRecords.SetVisible(true);
                //document.getElementById('btnSaveNew').style.display = 'none';
                //document.getElementById('btnSaveRecords').style.display = 'none';
            }
            else if (gridTDS.cpSaveSuccessOrFail == "reversenotrequired") {
                gridTDS.cpSaveSuccessOrFail = null;
                OnAddNewClick();
                chkAccount = 1;
                jAlert('Selected Ledger(s) are not mapped with Reverse Charge, please un-check Reverse Charge option.');
                cbtnSaveNew.SetVisible(true);
                cbtnSaveRecords.SetVisible(true);
                //document.getElementById('btnSaveNew').style.display = 'none';
                //document.getElementById('btnSaveRecords').style.display = 'none';
            }
            else if (gridTDS.cpSaveSuccessOrFail == "reversetaxledgermissing") {
                gridTDS.cpSaveSuccessOrFail = null;
                OnAddNewClick();
                chkAccount = 1;
                jAlert('reversetaxledgermissing');
                cbtnSaveNew.SetVisible(true);
                cbtnSaveRecords.SetVisible(true);
                //document.getElementById('btnSaveNew').style.display = 'none';
                //document.getElementById('btnSaveRecords').style.display = 'none';
            }
            else if (gridTDS.cpSaveSuccessOrFail == "addressrequired") {
                gridTDS.cpSaveSuccessOrFail = null;
                OnAddNewClick();
                chkAccount = 1;

                jAlert("Please Enter Billing/Shipping and GSTIN Details to Calculate GST.", "Alert !!", function () {
                    page.SetActiveTabIndex(1);
                    //chinmoy edited for new billing shipping
                    //  cbsSave_BillingShipping.Focus();
                    cbtnSave_SalesBillingShiping.Focus();
                    page.tabs[0].SetEnabled(false);
                    $("#divcross").hide();
                });
                cbtnSaveNew.SetVisible(true);
                cbtnSaveRecords.SetVisible(true);
                //document.getElementById('btnSaveNew').style.display = 'none';
                //document.getElementById('btnSaveRecords').style.display = 'none';

            }
            else if (gridTDS.cpSaveSuccessOrFail == "taxREquired") {
                gridTDS.cpSaveSuccessOrFail = null;
                OnAddNewClick();
                chkAccount = 1;
                jAlert('Selected Ledger is tagged for GST calculation. Click on Charges to calculate GST and Proceed.');
                cbtnSaveNew.SetVisible(true);
                cbtnSaveRecords.SetVisible(true);
                //document.getElementById('btnSaveNew').style.display = 'none';
                //document.getElementById('btnSaveRecords').style.display = 'none';
            }
            else if (gridTDS.cpSaveSuccessOrFail == "successInsert") {

                if (gridTDS.cpVouvherNo != null) {
                    var JV_Number = gridTDS.cpVouvherNo;
                    var value = document.getElementById('hdnRefreshType').value;
                    var JV_Msg = "Cash/Bank Voucher No. " + JV_Number + " generated.";
                    var strSchemaType = document.getElementById('hdnSchemaType').value;

                    if (value == "E") {
                        if (JV_Number != "") {
                            var Type = gridTDS.cpType;
                            var newInvoiceId = gridTDS.cpAutoID;


                            var AutoPrint = document.getElementById('hdnAutoPrint').value;
                            if (AutoPrint == "Yes") {
                                if (Type == "P") {
                                    window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=PaymentVoucher~D&modulename=CBVUCHR&id=" + newInvoiceId, '_blank');
                                }
                                else {
                                    window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=ReceiptVoucher~D&modulename=CBVUCHR&id=" + newInvoiceId, '_blank');
                                }
                            }
                            if (strSchemaType == '1') {
                                jAlert(JV_Msg, 'Alert Dialog: [CashBank]', function (r) {
                                    if (r == true) {
                                        window.location.assign("CashBankEntryList.aspx");
                                    }
                                });
                            }
                            else {
                                window.location.assign("CashBankEntryList.aspx");
                            }
                            //}                           

                        }
                        else {
                            // window.location.reload();
                            window.location.assign("CashBankEntryList.aspx");
                        }
                    }
                    else if (value == "S") {
                        if (JV_Number != "") {
                            var Type = gridTDS.cpType;
                            var newInvoiceId = gridTDS.cpAutoID;
                            var AutoPrint = document.getElementById('hdnAutoPrint').value;
                            if (AutoPrint == "Yes") {
                                if (Type == "P") {
                                    window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=PaymentVoucher~D&modulename=CBVUCHR&id=" + newInvoiceId, '_blank');
                                }
                                else {
                                    window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=ReceiptVoucher~D&modulename=CBVUCHR&id=" + newInvoiceId, '_blank');
                                }
                            }

                            // if (strSchemaType == '1') {
                            jAlert(JV_Msg, 'Alert Dialog: [CashBank]', function (r) {
                                if (r == true) {
                                    window.location.assign("CashBankEntry.aspx?key=ADD");
                                }
                            });
                            //jAlert(JV_Msg, "Alert!!", function () {
                            //});
                            // }
                        }
                        else {
                            // window.location.reload();
                            window.location.assign("CashBankEntry.aspx?key=ADD");
                        }
                    }
                    // gridTDS.cpVouvherNo = null;
                }
                if ($('#<%=hdnBtnClick.ClientID %>').val() == "Save_Exit") {
                    if (gridTDS.cpExitNew == "YES") {
                        //window.location.reload();
                        //window.location.assign("CashBankEntryList.aspx");
                    }
                    else {
                        OnAddNewClick();
                    }
                    gridTDS.cpType = null;
                    gridTDS.cpAutoID = null;
                }
                if ($('#<%=hdnBtnClick.ClientID %>').val() == "Save_New") {

                    //'''''''''''''''''''''''''''Commented for New Page
                    $("#divNumberingScheme").show();
                    $("#divEnterBranch").hide();
                    var Campany_ID = '<%=Session["LastCompany"]%>';
                    var LocalCurrency = '<%=Session["LocalCurrency"]%>';
                    var basedCurrency = LocalCurrency.split("~");
                    cCmbCurrency.SetValue(basedCurrency[0]);
                    c_txt_Debit.SetValue("0.0");
                    ctxtTotalPayment.SetValue("0.0");
                    ctxtRate.SetValue("");
                    cddl_AmountAre.SetEnabled(true);

                }
            }
    if (ViewStatus == "1") {
        viewOnly();
    }
    else if (pageStatus == "delete") {
        $('#<%=hdnPageStatus.ClientID %>').val('');
        CustomDeleteID = "";
        cbtnSaveNew.SetVisible(true);
        cbtnSaveRecords.SetVisible(true);
        OnAddNewClick();
    }
    else if (pageStatus == "update") {
        $('#<%=hdnPageStatus.ClientID %>').val('');
            AddButtonClick();
            var VoucherType = $('#rbtnType').val();
        }
}

function OnCustomButtonClickTDS(s, e) {
    if (e.buttonID == 'CustomDeleteTDS') {



        gridTDS.batchEditApi.EndEdit();


        var ISTDS = gridTDS.batchEditApi.GetCellValue(e.visibleIndex, "IsTDS");

        if (ISTDS != "" && ISTDS != null) {
            return;
        }

        var noofvisiblerows = gridTDS.GetVisibleRowsOnPage();

        if (noofvisiblerows != "1") {
            var debit = gridTDS.batchEditApi.GetCellValue(e.visibleIndex, "WithDrawlTDS");
            var credit = gridTDS.batchEditApi.GetCellValue(e.visibleIndex, "ReceiptTDS");
            var UniqueKey = gridTDS.batchEditApi.GetCellValue(e.visibleIndex, "UniqueID");

            if (debit != 0)
                c_txt_Debit.SetValue(DecimalRoundoff(parseFloat(c_txt_Debit.GetValue()) - parseFloat(debit), 2));
            if (credit != 0)
                c_txt_Credit.SetValue(DecimalRoundoff(parseFloat(c_txt_Credit.GetValue()) - parseFloat(credit), 2));


            var DebitCreditTDS = GetDebitCredit(UniqueKey);

            if (DebitCreditTDS != "" && typeof (DebitCreditTDS) != "undefined") {
                var dr = DebitCreditTDS.split('~')[0];
                var cr = DebitCreditTDS.split('~')[1];
                if (parseFloat(dr) > 0) {
                    c_txt_Credit.SetValue(DecimalRoundoff(parseFloat(c_txt_Credit.GetValue()) - parseFloat(dr), 2));
                }
                if (parseFloat(cr) > 0) {
                    c_txt_Credit.SetValue(DecimalRoundoff(parseFloat(c_txt_Credit.GetValue()) - parseFloat(cr), 2));
                }
            }


            var Debit = parseFloat(c_txt_Debit.GetValue());
            var Credit = parseFloat(c_txt_Credit.GetValue());

            if (Debit == 0 && Credit == 0) {
                // cbtnSaveRecordsTDS.SetVisible(false);
                // cbtn_SaveRecordsTDS.SetVisible(false);

                var Amount = parseFloat(Debit) - parseFloat(Credit);

            }
            else if (Debit == Credit) {
                //  cbtnSaveRecordsTDS.SetVisible(true);
                // cbtn_SaveRecordsTDS.SetVisible(true);
            }
            else {
                //  cbtnSaveRecordsTDS.SetVisible(false);
                //  cbtn_SaveRecordsTDS.SetVisible(false);

                var Amount = parseFloat(Debit) - parseFloat(Credit);
            }
            if ($("#cpTaggedTDS").val() == "-99") {
                //  cbtnSaveRecordsTDS.SetVisible(false);
                //  cbtn_SaveRecordsTDS.SetVisible(false);

            }

            var UniqueID = gridTDS.batchEditApi.GetCellValue(e.visibleIndex, "UniqueID"); //gridTDS.GetEditor("gvColMainAccount").GetValue();



            DeleteTDSRows(UniqueID);

            gridTDS.DeleteRow(e.visibleIndex);

            //if (parseFloat($("#hdnIsPartyLedger").val()) > 1 && gridTDS.GetVisibleRowsOnPage()=="2") 


            var type = $('#<%=hdnMode.ClientID %>').val();
            if (type == '1') {
                var IsJournal = "";
                for (var i = 0; i < gridTDS.GetVisibleRowsOnPage() ; i++) {
                    var frontProduct = (gridTDS.batchEditApi.GetCellValue(i, 'gvColMainAccountTDS') != null) ? (gridTDS.batchEditApi.GetCellValue(i, 'gvColMainAccountTDS')) : "";

                    if (frontProduct == "") {
                        IsJournal = "N";
                        break;
                    }
                }

                if (IsJournal == "") {
                    gridTDS.StartEditRow(0);
                }
            }
        }
    }
}

function GetVisibleIndexTDS(s, e) {
    globalRowIndexTDS = e.visibleIndex;
    // EnableOrDisableTax();
}


function OnBatchEditStartEditingTDS(s, e) {
    currentEditableVisibleIndexTDS = e.visibleIndex;
    globalRowIndexTDS = e.visibleIndex;
    var currentCountryID = gridTDS.batchEditApi.GetCellValue(currentEditableVisibleIndexTDS, "gvColMainAccountTDS");

    //var IsTDSSource = gridTDS.batchEditApi.GetCellValue(currentEditableVisibleIndexTDS, "IsTDSSource");
    //if (IsTDSSource == "1") {
    //    if (e.focusedColumn.fieldName == "ReceiptTDS") {
    //        gridTDS.batchEditApi.EndEdit();
    //        e.cancel = true;
    //        return;
    //    }
    //}






    var ISTDS = gridTDS.batchEditApi.GetCellValue(currentEditableVisibleIndexTDS, "IsTDS");

    if (ISTDS != "" && ISTDS != null) {
        e.cancel = true;
    }
    var cityIDColumn = s.GetColumnByField("CityIDTDS");
}

function OnBatchEditEndEditingTDS(s, e) {
    currentEditableVisibleIndexTDS = -1;
    var cityIDColumn = s.GetColumnByField("CityIDTDS");

}

//////////////////////////  TDS End /////////////////////////////////////////////

function changePaymentTotalSummary() {
    var newDif = PaymentOldValue - PaymentNewValue;
    var CurrentSum = c_txt_Credit.GetText();
    var indx = CurrentSum.indexOf(',');
    if (indx != -1) {
        CurrentSum = CurrentSum.replace(/,/g, '');
    }
    c_txt_Credit.SetValue(parseFloat(CurrentSum - newDif));
    var newNetAmountDiff = NetAmountOldValue - PaymentNewValue;
    var CurrentNetSum = c_txtTotalNetAmount.GetText();
    c_txtTotalNetAmount.SetValue(parseFloat(CurrentNetSum - newNetAmountDiff));

}

//New By Indranil
function txtPercentageLostFocus(s, e) {
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
                ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt);
                GlobalCurTaxAmt = 0;
            }
            else {

                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                cgridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * s.GetText()) / 100) * -1);
                ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) * -1) - (GlobalCurTaxAmt * -1));
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
function CmbtaxClick(s, e) {
    GlobalCurTaxAmt = parseFloat(ctxtGstCstVat.GetText());
    gstcstvatGlobalName = s.GetText();
}
function taxAmountLostFocus(s, e) {
    var finalTaxAmt = parseFloat(s.GetValue());
    var totAmt = parseFloat(ctxtTaxTotAmt.GetText());
    var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
    var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
    if (sign == '(+)') {
        ctxtTaxTotAmt.SetValue(totAmt + finalTaxAmt - taxAmountGlobal);
    } else {
        ctxtTaxTotAmt.SetValue(totAmt + (finalTaxAmt * -1) - (taxAmountGlobal * -1));
    }
    SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''));
    //Set Running Total
    SetRunningTotal();
    RecalCulateTaxTotalAmountInline();
}
function QuotationTaxAmountGotFocus(s, e) {
    taxAmountGlobalCharges = parseFloat(s.GetValue());
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

                ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt);
                GlobalCurTaxAmt = 0;
            }
            else {

                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                cgridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * s.GetText()) / 100) * -1);

                ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) * -1) - (GlobalCurTaxAmt * -1));
                GlobalCurTaxAmt = 0;
            }
        }
    }
    //return;
    cgridTax.batchEditApi.EndEdit();

}

//-----------------------------------------------------------------------------------------------------

function recalculatePayment(oldVal) {
    if (oldVal != 0) {
        PaymentNewValue = 0;
        PaymentOldValue = oldVal;
        changePaymentTotalSummary();
    }
}
function changeReciptTotalSummary() {
    var newDif = ReciptOldValue - ReciptNewValue;
    var CurrentSum = c_txt_Debit.GetText();
    var indx = CurrentSum.indexOf(',');
    if (indx != -1) {
        CurrentSum = CurrentSum.replace(/,/g, '');
    }

    c_txt_Debit.SetValue(parseFloat(CurrentSum - newDif));
}
function ReceiptLostFocus(s, e) {
    ReciptNewValue = s.GetText();
    var indx = ReciptNewValue.indexOf(',');

    if (indx != -1) {
        ReciptNewValue = ReciptNewValue.replace(/,/g, '');
    }
    if (ReciptOldValue != ReciptNewValue) {
        changeReciptTotalSummary();
    }
}
function ReceiptTextChange(s, e) {


}
function Receipt_TextChange(s, e) {

}
function Payment_TextChange(s, e) {

}
function Calculate() {

}
var lastCRP = null;
function rbtnType_SelectedIndexChanged() {
    document.getElementById('<%= txtVoucherNo.ClientID %>').value = "";
    // var VoucherType = cComboType.GetValue();
    var VoucherType = document.getElementById('rbtnType').value;

    if (cCmbScheme.InCallback()) {
        lastCRP = VoucherType;
    }
    else {
        cCmbScheme.PerformCallback(VoucherType);
    }
    if (VoucherType == "P") {
        document.getElementById('divPaidTo').style.display = 'block';
        document.getElementById('divReceivedfrom').style.display = 'none';

    
        //chinmoy edited below code for new billing shipping
        //     LoadCustomerAddress('', $('#ddlBranch').val(), 'PO');
        if ($("#ddlBranch").val() != null && $("#ddlBranch").val() != "") {
            SetPurchaseBillingShippingAddress($('#ddlBranch').val());
        }
    }
    else {
        document.getElementById('divReceivedfrom').style.display = 'block';
        document.getElementById('divPaidTo').style.display = 'none';
        ClearBillingShipping();
    }
    //cComboType.SetEnabled(false);
    //$("#rbtnType").attr("disabled", "disabled");
}

        function ClearBillingShipping() {
            //shipping
            ctxtsAddress1.SetText('');
            ctxtsAddress2.SetText('');
            ctxtsAddress3.SetText('');
            ctxtslandmark.SetText('');
            ctxtShippingPin.SetText('');
            $('#hdShippingPin').val('');
            ctxtshippingCountry.SetText('');
            $('#hdCountryIdShipping').val('');
            ctxtshippingState.SetText('');
            $('#hdStateCodeShipping').val('');
            $('#hdStateIdShipping').val('');
            ctxtshippingCity.SetText('');
            $('#hdCityIdShipping').val('');
            ctxtSelectShippingArea.SetText('');
            $('#hdAreaIdShipping').val('');
            // ctxtDistanceShipping.SetText('');
            ctxtShippingGSTIN1.SetText('');
            ctxtShippingGSTIN2.SetText('');
            ctxtShippingGSTIN3.SetText('');

            //billing
            ctxtAddress1.SetText('');
            ctxtAddress2.SetText('');
            ctxtAddress3.SetText('');
            ctxtbillingPin.SetText('');
            $('#hdBillingPin').val('');
            ctxtbillingCountry.SetText('');
            $('#hdCountryIdBilling').val('');
            ctxtbillingState.SetText('');
            $('#hdStateIdBilling').val('');
            $('#hdStateCodeBilling').val('');
            ctxtbillingCity.SetText('');
            $('#hdCityIdBilling').val('');
            //var GSTIN = BillShipDet.GSTIN;
            //GSTIN1 = GSTIN.substring(0, 2);
            //GSTIN2 = GSTIN.substring(2, 12);
            //GSTIN3 = GSTIN.substring(12, 15);
            ctxtBillingGSTIN1.SetText('');
            ctxtBillingGSTIN2.SetText('');
            ctxtBillingGSTIN3.SetText('');

        }

function CmbSchemeEndCallback() {
    if (lastCRP) {
        cCmbScheme.PerformCallback(lastCRP);
        lastCRP = null;
    }

}
function OnAddNewClick() {

    gridTDS.AddNewRow();

}
var CustomDeleteID = "";

function PaymentgotFocus(s, e) {
    GlobargotPayMent = s.GetValue();

}
function PaymentLostFocus(s, e) {

    var PayVal = parseFloat((s.GetValue() != null) ? s.GetValue() : "0");
    if (GlobargotPayMent != null) {
        if (parseFloat(GlobargotPayMent) != PayVal) {
            var Curval = parseFloat(c_txt_Credit.GetText());
            var Totalval = PayVal + Curval - GlobargotPayMent;
            c_txt_Credit.SetValue(Totalval);
            GlobargotPayMent = null;
        }
    }

}
function RecptgotFocus(s, e) {
    GlobargotRecpt = s.GetValue();

}
function SumReceipt(s, e) {

    var recptVal = parseFloat((s.GetValue() != null) ? s.GetValue() : "0");
    if (GlobargotRecpt != null) {
        if (parseFloat(GlobargotRecpt) != recptVal) {
            var Curval = parseFloat(c_txt_Debit.GetText());
            var Totalval = recptVal + Curval - GlobargotRecpt;
            c_txt_Debit.SetValue(Totalval);
            GlobargotRecpt = null;
        }
    }

}
function ChangeVoucherType() {
    var colName;
    var val = "0";
    var AspRadio = document.getElementById("rbtnType");
    var AspRadio_ListItem = AspRadio.getElementsByTagName('input');
    for (var i = 0; i < AspRadio_ListItem.length; i++) {
        if (AspRadio_ListItem[i].checked) {
            val = AspRadio_ListItem[i].value;
        }
    }

    if (val == "R") {

        gridTDS.GetEditor('btnPayment').SetEnabled(false);

    }
    else {
        gridTDS.GetEditor('btnPayment').SetEnabled(true);
    }
    if (val == "P") {

        gridTDS.GetEditor('btnRecieve').SetEnabled(false);
    }
    else {
        gridTDS.GetEditor('btnRecieve').SetEnabled(true);
    }
}

function deleteAllRows() {
    var frontRow = 0;
    var backRow = -1;
    for (var i = 0; i <= gridTDS.GetVisibleRowsOnPage() + 100 ; i++) {
        gridTDS.DeleteRow(frontRow);
        gridTDS.DeleteRow(backRow);
        backRow--;
        frontRow++;
    }
    gridTDS.AddNewRow();

    c_txt_Credit.SetValue(0);
    c_txt_Debit.SetValue(0);

}
//function onBranchItems(e) {

//    //get the first row accounting value debjyoti 
//    gridTDS.batchEditApi.StartEdit(-1, 1);
//    var accountingDataMin = gridTDS.GetEditor('MainAccount').GetValue();
//    gridTDS.batchEditApi.EndEdit();

//    gridTDS.batchEditApi.StartEdit(0, 1);
//    var accountingDataplus = gridTDS.GetEditor('MainAccount').GetValue();
//    gridTDS.batchEditApi.EndEdit();


//    if (accountingDataMin != null || accountingDataplus != null) {
//        jConfirm('You have changed Branch. All the entries of ledger in this voucher to be reset to blank. \n You have to select and re-enter. Continue?', 'Confirmation Dialog', function (r) {

//            if (r == true) {
//                deleteAllRows();

//                MainAccount.PerformCallback(document.getElementById('ddlBranch').value);
//                oldBranchdata = document.getElementById('lstBranchItems').value;
//                $('#MandatoryBranch').hide();
//                BindCashBankAccountListByBranch(document.getElementById('lstBranchItems').value);
//            } else {
//                Bind_Branch_Edit(oldBranchdata);
//            }
//        });

//    }
//    else {
//        BindCashBankAccountListByBranch(document.getElementById('lstBranchItems').value);
//        //MainAccount.PerformCallback(document.getElementById('lstBranchItems').value);
//        MainAccount.PerformCallback(document.getElementById('ddlBranch').value);
//    }

//}
function AddBatchNew(s, e) {
    gridTDS.batchEditApi.StartEdit(e.visibleIndex);
    var keyCode = ASPxClientUtils.GetKeyCode(e.htmlEvent);
    if (keyCode === 13) {
        var mainAccountValue = (gridTDS.GetEditor('MainAccount').GetValue() != null) ? gridTDS.GetEditor('MainAccount').GetValue() : "";
        var btnRecieve = (gridTDS.GetEditor('btnRecieve').GetValue() != null) ? gridTDS.GetEditor('btnRecieve').GetValue() : "";
        var btnPayment = (gridTDS.GetEditor('btnPayment').GetValue() != null) ? gridTDS.GetEditor('btnPayment').GetValue() : "";
        if (mainAccountValue != "" && (btnRecieve != "0.0" || btnPayment != "0.0")) {
            gridTDS.AddNewRow();
            gridTDS.SetFocusedRowIndex();
        }
    }
    else if (keyCode === 9) {
        cbtnSaveNew.Focus();
    }
    else {
        return false;
    }

}
//...................Shortcut keys.................

document.onkeydown = function (e) {
    if (event.keyCode == 83 && event.altKey == true) {
        //run code for Alt+S -- ie, save!   
        StopDefaultAction(e);

        if (CustomDeleteID == "1") {

        }
        else {
            document.getElementById('btnSaveNew').click();
        }

    }
    else if ((event.keyCode == 120 || event.keyCode == 88) && event.altKey == true) {
        //run code for Alt+X -- ie, Save & Exit! 
        StopDefaultAction(e);
        if (CustomDeleteID == "1") {

        }
        else {
            document.getElementById('btnSaveRecords').click();
        }


        //return false;
    }
        //else if ((event.keyCode == 120 || event.keyCode == 65) && event.altKey == true) {

        //    if (document.getElementById('DivEntry').style.display != 'block') {
        //        if (document.getElementById('AddButton').style.display != 'none') {
        //            AddButtonClick();
        //        }
        //    }
        //}
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
//...................end............................
var currentEditableVisibleIndex;
var lastMainAccountID;
var setValueFlag;
function InstrumentDateChange() {
    var SelectedDate = new Date(cInstDate.GetDate());
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
           cInstDate.SetDate(MaxLockDate);
           return;
       }
       ///End Checking of Transaction Date For MaxLockDate


       ///Date Should Between Current Fin Year StartDate and EndDate
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
           //                   alert('Between');
       }
       else {
           // jAlert('Enter Date Is Outside Of Financial Year !!');
           jAlert('Enter Date Is Outside Of Financial Year !!', 'Alert Dialog: [CashBank]', function (r) {
               if (r == true) {
                   //cdtTDate.Focus();
               }
           });
           if (SelectedDateNumericValue < FinYearStartDateNumericValue) {
               cInstDate.SetDate(new Date(FinYearStartDate));
           }
           if (SelectedDateNumericValue > FinYearEndDatNumbericValue) {
               cInstDate.SetDate(new Date(FinYearEndDate));
           }
       }
       ///End OF Date Should Between Current Fin Year StartDate and EndDate
   }
   function InstrumentTypeSelectedIndexChanged() {
       $("#MandatoryInstrumentType").hide();
       $("#MandatoryInstNo").hide();

       var InstType = cComboInstrumentTypee.GetValue();

       if (InstType == "CH") {
           $('#<%=hdnInstrumentType.ClientID %>').val(0);
            document.getElementById("divInstrumentNo").style.display = 'none';
            document.getElementById("tdIDateDiv").style.display = 'none';
            document.getElementById("divDraweeBank").style.display = 'none';
        }
        else {
            $('#<%=hdnInstrumentType.ClientID %>').val(InstType);
            document.getElementById("divInstrumentNo").style.display = 'block';
            document.getElementById("tdIDateDiv").style.display = 'block';
            document.getElementById("divDraweeBank").style.display = 'block';
        }
    }
    function Currency_Rate() {

        var Campany_ID = '<%=Session["LastCompany"]%>';
            var LocalCurrency = '<%=Session["LocalCurrency"]%>';
            var basedCurrency = LocalCurrency.split("~");
            var Currency_ID = cCmbCurrency.GetValue();
            $('#<%=hdnCurrenctId.ClientID %>').val(Currency_ID);


            if (cCmbCurrency.GetText().trim() == basedCurrency[1]) {
                ctxtRate.SetValue("");
                ctxtRate.SetEnabled(false);
            }
            else {
                $.ajax({
                    type: "POST",
                    url: "ContraVoucher.aspx/GetRate",
                    data: JSON.stringify({ Currency_ID: Currency_ID, Campany_ID: Campany_ID, basedCurrency: basedCurrency[0] }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var data = msg.d;
                        ctxtRate.SetValue(data);


                    }
                });

                ctxtRate.SetEnabled(true);
            }
        }

        function CmbScheme_ValueChange() {
            //  debugger;

            var NoSchemeTypedtl = (cCmbScheme.GetValue() == null ? "" : cCmbScheme.GetValue());
            var schemetype = NoSchemeTypedtl.toString().split('~')[1];
            var schemelength = NoSchemeTypedtl.toString().split('~')[2];
            var branchID = (NoSchemeTypedtl.toString().split('~')[3] != null) ? NoSchemeTypedtl.toString().split('~')[3] : "";
            var fromdate = (NoSchemeTypedtl.toString().split('~')[4] != null) ? NoSchemeTypedtl.toString().split('~')[4] : "";
            var todate = (NoSchemeTypedtl.toString().split('~')[5] != null) ? NoSchemeTypedtl.toString().split('~')[5] : "";
            var branchStateID = (NoSchemeTypedtl.toString().split('~')[5] != null) ? NoSchemeTypedtl.toString().split('~')[6] : "0";


            document.getElementById('ddlSupplyState').value = branchStateID;

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

            //MainAccount.PerformCallback(branchID);
            $('#txtVoucherNo').attr('maxLength', schemelength);
            $('#<%=hdnBranchId.ClientID %>').val(branchID);
            $('#<%=hfIsFilter.ClientID %>').val(branchID);

            document.getElementById('ddlBranch').value = branchID;
            cddlCashBank.PerformCallback(branchID);
            document.getElementById('ddlEnterBranch').value = branchID;
            //var VoucherType = cComboType.GetValue();
            var VoucherType = document.getElementById('rbtnType').value;
            ClearBillingShipping();

            var CashBankText = cddlCashBank.GetText();
            if (CashBankText != "") {
                var arr = CashBankText.split('|');
                var strbranch = $('#<%=hdnBranchId.ClientID %>').val();
            }
            else {
                document.getElementById("pageheaderContent").style.display = 'none';
            }

            deleteAllRows();
            gridTDS.AddNewRow();
            gridTDS.batchEditApi.EndEdit();
            c_txt_Debit.SetValue("0.00");
            c_txt_Credit.SetValue("0.00");

            if (schemetype == '0') {
                $('#<%=hdnSchemaType.ClientID %>').val('0');
                document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = false;
                document.getElementById('<%= txtVoucherNo.ClientID %>').value = "";

            }
            else if (schemetype == '1') {
                $('#<%=hdnSchemaType.ClientID %>').val('1');
                document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;
                document.getElementById('<%= txtVoucherNo.ClientID %>').value = "Auto";
                $("#MandatoryBillNo").hide();
                cdtTDate.Focus();
            }
            else if (schemetype == '2') {
                $('#<%=hdnSchemaType.ClientID %>').val('2');
                document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;
                document.getElementById('<%= txtVoucherNo.ClientID %>').value = "Datewise";
            }
            else if (schemetype == 'n') {
                document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;
                document.getElementById('<%= txtVoucherNo.ClientID %>').value = "";
            }
            //$("#ddlBranch").attr("disabled", "disabled");

        <%--var val = cCmbScheme.GetValue();
        $.ajax({
            type: "POST",
            url: 'CashBankEntry.aspx/getSchemeType',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{sel_scheme_id:\"" + val + "\"}",
            success: function (type) {
                var schemetypeValue = type.d;
                var schemetype = schemetypeValue.toString().split('~')[0];
                var schemelength = schemetypeValue.toString().split('~')[1];
                $('#txtVoucherNo').attr('maxLength', schemelength);
                var branchID = schemetypeValue.toString().split('~')[2];
                if (schemetypeValue != "") {
                  
                    $('#<%=hdnBranchId.ClientID %>').val(branchID);
                        document.getElementById('ddlBranch').value = branchID;
                        var VoucherType = cComboType.GetValue();
                        ClearBillingShipping('all');
                        if (VoucherType == "P") {
                            LoadCustomerAddress('', $('#ddlBranch').val(), 'PO');
                        }


                        $("#ddlBranch").attr("disabled", "disabled");
                       
                        cddlCashBank.PerformCallback(branchID);
                        
                        MainAccount.PerformCallback(branchID);
                   
                        var CashBankText = cddlCashBank.GetText();
                        if (CashBankText != "") {
                            var arr = CashBankText.split('|');
                            var strbranch = $('#<%=hdnBranchId.ClientID %>').val();
                            PopulateCurrentBankBalance(arr[0], strbranch);
                        }
                        else {

                            document.getElementById("pageheaderContent").style.display = 'none';
                        }

                    }
                    if (schemetype == '0') {
                        $('#<%=hdnSchemaType.ClientID %>').val('0');
                        document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = false;
                        document.getElementById('<%= txtVoucherNo.ClientID %>').value = "";
                       
                    }
                    else if (schemetype == '1') {
                        $('#<%=hdnSchemaType.ClientID %>').val('1');
                        document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;
                        document.getElementById('<%= txtVoucherNo.ClientID %>').value = "Auto";
                        $("#MandatoryBillNo").hide();
                        cdtTDate.Focus();
                    }
                    else if (schemetype == '2') {
                        $('#<%=hdnSchemaType.ClientID %>').val('2');
                        document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;
                        document.getElementById('<%= txtVoucherNo.ClientID %>').value = "Datewise";
                    }
                    else if (schemetype == 'n') {
                        document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;
                        document.getElementById('<%= txtVoucherNo.ClientID %>').value = "";
                    }
                }
            });--%>


            if (schemetype == '0') {
                document.getElementById("txtVoucherNo").focus();
            } else {
                cCmbScheme.Focus();
            }

            if ($("#hdnProjectSelectInEntryModule").val() == "1")
                clookup_Project.gridView.Refresh();
        }

        function CountriesCombo_SelectedIndexChanged() {

            var currentValue = MainAccount.GetValue();
            ///  Numering Scheme Disable after selection of ledger
            if (currentValue != null && currentValue != '' && currentValue != '0' && cCmbScheme.GetValue() != null) {
                cCmbScheme.SetEnabled(false);
            }
            chkAccount = 1;
            if (lastMainAccountID == currentValue) {
                if (SubAccount_ReferenceID.GetSelectedIndex() < 0)
                    SubAccount_ReferenceID.SetSelectedIndex(0);
                return;
            }
            lastMainAccountID = currentValue;
            LoadingPanel.Show();
            if (currentValue != null) {
                SubAccount_ReferenceID.PerformCallback(currentValue + '~' + "");
            }
            else {
                LoadingPanel.Hide();
            }
           <%-- $.ajax({
                type: "POST",
                url: "CashBankEntry.aspx/CheckTdsByMainAccountID",
                data: JSON.stringify({ MainAccountID: currentValue }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var data = msg.d;

                    if (data == "NO") {
                        $('#<%=hdnPayment.ClientID %>').val(data);
                        gridTDS.GetEditor('TDS').SetValue(" ");
                    }
                    else {
                        $('#<%=hdnPayment.ClientID %>').val(data);
                        gridTDS.GetEditor('TDS').SetValue("Y");
                    }
                    
                }
            });--%>

            if (currentValue != "") {

                $("#rbtnType").attr("disabled", "disabled");
            }

        }
        function IntializeGlobalVariables(gridTDS) {
            lastMainAccountID = gridTDS.cplastMainAccountID;
            currentEditableVisibleIndex = -1;
            setValueFlag = -1;

        }

        //function Bind_Branch_Edit(obj) {
        //    var dropdownlistbox = document.getElementById("lstBranchItems")

        //    for (var i = 0; i < dropdownlistbox.options.length; i++) {
        //        if (dropdownlistbox.options[i].value == obj) {
        //            dropdownlistbox.options[i].selected = true;
        //        }
        //    }
        //    $('#lstBranchItems').trigger("chosen:updated");

        //    MainAccount.PerformCallback(document.getElementById('lstBranchItems').value);

        //}
        //function Bind_Cash_Bank_Edit(obj) {
        //    var dropdownlistbox = document.getElementById("lstCashItems")

        //    for (var i = 0; i < dropdownlistbox.options.length; i++) {
        //        if (dropdownlistbox.options[i].value == obj) {
        //            dropdownlistbox.options[i].selected = true;
        //        }
        //    }
        //    $('#lstCashItems').trigger("chosen:updated");
        //}
        function WithdrawalChangedNew(WithDrawType) {

            if (WithDrawType == "Cash") {
                var comboitem = cComboInstrumentTypee.FindItemByValue('C');
                if (comboitem != undefined && comboitem != null) {
                    cComboInstrumentTypee.RemoveItem(comboitem.index);
                }
                var comboitem = cComboInstrumentTypee.FindItemByValue('D');
                if (comboitem != undefined && comboitem != null) {
                    cComboInstrumentTypee.RemoveItem(comboitem.index);
                }
                var comboitem = cComboInstrumentTypee.FindItemByValue('E');
                if (comboitem != undefined && comboitem != null) {
                    cComboInstrumentTypee.RemoveItem(comboitem.index);
                }
                var comboitem = cComboInstrumentTypee.FindItemByValue('CH');
                if (comboitem == undefined || comboitem == null) {
                    cComboInstrumentTypee.AddItem("Cash", "CH");
                }
                cComboInstrumentTypee.SetValue("CH");
                InstrumentTypeSelectedIndexChanged();
            }
            else {
                var comboitem = cComboInstrumentTypee.FindItemByValue('C');
                if (comboitem == undefined || comboitem == null) {
                    cComboInstrumentTypee.AddItem("Cheque", "C");
                }
                var comboitem = cComboInstrumentTypee.FindItemByValue('D');
                if (comboitem == undefined || comboitem == null) {
                    cComboInstrumentTypee.AddItem("Draft", "D");
                }
                var comboitem = cComboInstrumentTypee.FindItemByValue('E');
                if (comboitem == undefined || comboitem == null) {
                    cComboInstrumentTypee.AddItem("E.Transfer", "E");
                }
                var comboitem = cComboInstrumentTypee.FindItemByValue('CH');
                if (comboitem != undefined && comboitem != null) {
                    cComboInstrumentTypee.RemoveItem(comboitem.index);
                    cComboInstrumentTypee.SetValue("C");
                    InstrumentTypeSelectedIndexChanged();
                }
            }
        }
      <%--  function acpEndCallbackGeneral(s, e) {
            LoadingPanel.Hide();
            if (cASPxCallbackGeneral.cpCBEdit != null) {
                var VoucherType = cASPxCallbackGeneral.cpCBEdit.split('*')[0];
                var CashBank_ID = cASPxCallbackGeneral.cpCBEdit.split('*')[1];
                var Currency = cASPxCallbackGeneral.cpCBEdit.split('*')[2];
                var InstrumentType = cASPxCallbackGeneral.cpCBEdit.split('*')[3];
                EditAddressSinglePage(CashBank_ID, 'CBE');
                if (VoucherType == "P") {
                    document.getElementById('divPaidTo').style.display = 'block';
                    document.getElementById('divReceivedfrom').style.display = 'none';
                }
                else {
                    document.getElementById('divReceivedfrom').style.display = 'block';
                    document.getElementById('divPaidTo').style.display = 'none';
                }
                $("#txtVoucherNo").attr("disabled", "disabled");
                $("#ddlBranch").attr("disabled", "disabled");
                $("#ddlEnterBranch").attr("disabled", "disabled");
                var setCurr = '<%=Session["LocalCurrency"]%>';
                var localCurrency = setCurr.split('~')[0];
                if (Currency != localCurrency) {
                    $('#<%=hdnCurrenctId.ClientID %>').val(Currency);
                    ctxtRate.SetEnabled(true);
                }
                else {
                    ctxtRate.SetEnabled(false);
                    $('#<%=hdnCurrenctId.ClientID %>').val("");
                }
                var WithdrawalType = "";
                if (InstrumentType == "CH") {
                    WithdrawalType = "Cash";
                    $('#<%=hdnInstrumentType.ClientID %>').val(0);
                    document.getElementById("divInstrumentNo").style.display = 'none';
                    document.getElementById("tdIDateDiv").style.display = 'none';
                }
                else {
                    $('#<%=hdnInstrumentType.ClientID %>').val(InstrumentType);
                }
                WithdrawalChangedNew(WithdrawalType);
                gridTDS.batchEditApi.StartEdit(-1, 1);
                if ($('#<%=hdnEditClick.ClientID %>').val() == 'T') {
                    OnAddNewClick();
                    $('#<%=hdnEditClick.ClientID %>').val("");
                }
                // cComboType.SetEnabled(false);
                $("#rbtnType").attr("disabled", "disabled");
                cddl_AmountAre.SetEnabled(false);
                var CashBankText = cddlCashBank.GetText();
                var arr = CashBankText.split('|');
                var strbranch = $("#ddlBranch").val();
                PopulateCurrentBankBalance(arr[0], strbranch);
                document.getElementById('hdnEditCBID').value = CashBank_ID;
                document.getElementById('divNumberingScheme').style.display = 'none';
                document.getElementById('divEnterBranch').style.display = 'Block';
                cASPxCallbackGeneral.cpCBEdit = null;
            }
            if (cASPxCallbackGeneral.cpView == "1") {
                viewOnly();
                cASPxCallbackGeneral.cpView = null;
            }

        }--%>
        function OnEndCallback(s, e) {
            IntializeGlobalVariables(s);
            LoadingPanel.Hide();
            var pageStatus = document.getElementById('hdnPageStatus').value;

            var ViewStatus = document.getElementById('hdnView').value;

            if ($('#<%=hdnPayment.ClientID %>').val() == "YES") {
                $('#<%=hdnPayment.ClientID %>').val("NO");
                OnAddNewClick();
            }
            if (gridTDS.cpTotalAmount != null) {
                var total_receipt = gridTDS.cpTotalAmount.split('~')[0];
                var total_payment = gridTDS.cpTotalAmount.split('~')[1];
                c_txt_Debit.SetValue(total_receipt);
                c_txt_Credit.SetValue(total_payment);
                gridTDS.cpTotalAmount = null;
            }

            <%--if (gridTDS.cpEdit != null) {                
                var VoucherType = gridTDS.cpEdit.split('*')[0];
                var VoucherNo = gridTDS.cpEdit.split('*')[1];
                var trnsDate = gridTDS.cpEdit.split('*')[2];
                var Branch = gridTDS.cpEdit.split('*')[3];
                var Cash_Bank = gridTDS.cpEdit.split('*')[4];
                var Currency = gridTDS.cpEdit.split('*')[5];
                var InstrumentType = gridTDS.cpEdit.split('*')[6];
                var InstrumentNo = gridTDS.cpEdit.split('*')[7];
                var Narration = gridTDS.cpEdit.split('*')[8];
                var receipt = gridTDS.cpEdit.split('*')[9];
                var payment = gridTDS.cpEdit.split('*')[10];
                var CashBank_ID = gridTDS.cpEdit.split('*')[11];
                var ReceivedFrom = gridTDS.cpEdit.split('*')[12];
                var PaidTo = gridTDS.cpEdit.split('*')[13];
                var Tax_Code = gridTDS.cpEdit.split('*')[14];
                var ContactNo = gridTDS.cpEdit.split('*')[15];
                var ReverseCharge = gridTDS.cpEdit.split('*')[16];
                var InstrumentDate = gridTDS.cpEdit.split('*')[17];
                var DraweeBank = gridTDS.cpEdit.split('*')[18];
                var EnteredBranchID = gridTDS.cpEdit.split('*')[19];
                var CashBankName = gridTDS.cpEdit.split('*')[20];

                document.getElementById('hdnEditCBID').value = CashBank_ID;
                var Transdt = new Date(trnsDate);
                EditAddressSinglePage(CashBank_ID, 'CBE');
                cdtTDate.SetDate(Transdt);
                var insDate = new Date(InstrumentDate);
                cInstDate.SetDate(insDate);
                if (VoucherType == "P") {
                    document.getElementById('divPaidTo').style.display = 'block';
                    document.getElementById('divReceivedfrom').style.display = 'none';
                    ctxtPaidTo.SetValue(PaidTo);
                }
                else {
                    document.getElementById('divReceivedfrom').style.display = 'block';
                    document.getElementById('divPaidTo').style.display = 'none';
                    ctxtReceivedFrom.SetValue(ReceivedFrom);
                }
                document.getElementById('txtVoucherNo').value = VoucherNo;
                $("#txtVoucherNo").attr("disabled", "disabled");
                $("#ddlBranch").attr("disabled", "disabled");
                $("#ddlEnterBranch").attr("disabled", "disabled");
                ctxtInstNobth.SetValue(InstrumentNo.trim());
                document.getElementById('txtNarration').value = Narration;
                ctxtReceivedFrom.SetValue(ReceivedFrom);
                ctxtPaidTo.SetValue(PaidTo);
                cddl_AmountAre.SetValue(Tax_Code);
                document.getElementById('rbtnType').value = VoucherType
                $("#rbtnType").attr("disabled", "disabled");
               // cComboType.SetValue(VoucherType);
               // cComboType.SetEnabled(false);
                cddl_AmountAre.SetEnabled(true);
                $("#txtContact").val(ContactNo);
                $("#txtDraweeBank").val(DraweeBank);
                $("#chk_reversemechenism").prop("checked", parseInt(ReverseCharge));
                var setCurr = '<%=Session["LocalCurrency"]%>';
                var localCurrency = setCurr.split('~')[0];
                if (Currency != localCurrency) {
                    cCmbCurrency.SetValue(Currency);
                    $('#<%=hdnCurrenctId.ClientID %>').val(Currency);
                    ctxtRate.SetEnabled(true);
                }
                else {
                    ctxtRate.SetEnabled(false);
                    $('#<%=hdnCurrenctId.ClientID %>').val("");
                }
                var WithdrawalType = "";
                if (InstrumentType == "CH") {
                    WithdrawalType = "Cash";
                    $('#<%=hdnInstrumentType.ClientID %>').val(0);
                    document.getElementById("divInstrumentNo").style.display = 'none';
                    document.getElementById("tdIDateDiv").style.display = 'none';
                }
                else {
                    $('#<%=hdnInstrumentType.ClientID %>').val(InstrumentType);
                }
                WithdrawalChangedNew(WithdrawalType);
                cComboInstrumentTypee.SetValue(InstrumentType);
                oldBranchdata = Branch;
                $('#<%=hdnInstrumentNo.ClientID %>').val(InstrumentNo.trim());
                $('#<%=hdnBranchId.ClientID %>').val(Branch);
                $('#<%=hdnCashBankId.ClientID %>').val(Cash_Bank);
                c_txt_Debit.SetValue(receipt);
                c_txt_Credit.SetValue(payment);
                $("#ddlBranch").val(Branch);
                $("#ddlEnterBranch").val(EnteredBranchID);
                cddlCashBank.PerformCallback(EnteredBranchID);

                gridTDS.batchEditApi.StartEdit(-1, 1);
                if ($('#<%=hdnEditClick.ClientID %>').val() == 'T') {
                    OnAddNewClick();
                    $('#<%=hdnEditClick.ClientID %>').val("");
                }
                gridTDS.cpEdit = null;
            }--%>
            if (gridTDS.cpSaveSuccessOrFail == "outrange") {
                gridTDS.cpSaveSuccessOrFail = null;
                OnAddNewClick();
                chkAccount = 1;
                jAlert('The Numbering Scheme has exhausted, please update the Scheme and try adding the Voucher');
                //cbtnSaveNew.SetVisible(true);
                //cbtnSaveRecords.SetVisible(true);
                //document.getElementById('btnSaveNew').style.display = 'none';
                //document.getElementById('btnSaveRecords').style.display = 'none';
            }
            else if (gridTDS.cpSaveSuccessOrFail == "duplicate") {
                gridTDS.cpSaveSuccessOrFail = null;
                OnAddNewClick();
                chkAccount = 1;
                jAlert('Can not save as duplicate Voucher No.');
                //cbtnSaveNew.SetVisible(true);
                //cbtnSaveRecords.SetVisible(true);
                //document.getElementById('btnSaveNew').style.display = 'none';
                //document.getElementById('btnSaveRecords').style.display = 'none';
            }
            else if (gridTDS.cpSaveSuccessOrFail == "zeroAmount") {
                gridTDS.cpSaveSuccessOrFail = null;
                OnAddNewClick();
                chkAccount = 1;
                jAlert('Cannot save with ZERO Amount.');
                //cbtnSaveNew.SetVisible(true);
                //cbtnSaveRecords.SetVisible(true);
            }
            else if (gridTDS.cpSaveSuccessOrFail == "SubAccountMandatory") {
                gridTDS.cpSaveSuccessOrFail = null;
                OnAddNewClick();
                chkAccount = 1;
                jAlert('Sub-account ledger selection is set as mandatory in System Settings.\n Please select Sub-account ledger to proceed.');
                //cbtnSaveNew.SetVisible(true);
                //cbtnSaveRecords.SetVisible(true);
            }
            else if (gridTDS.cpSaveSuccessOrFail == "errorInsert") {
                gridTDS.cpSaveSuccessOrFail = null;
                OnAddNewClick();
                chkAccount = 1;
                jAlert('Try again later.');
                //cbtnSaveNew.SetVisible(true);
                //cbtnSaveRecords.SetVisible(true);
                //document.getElementById('btnSaveNew').style.display = 'none';
                //document.getElementById('btnSaveRecords').style.display = 'none';
            }
            else if (gridTDS.cpSaveSuccessOrFail == "mixedvalue") {
                gridTDS.cpSaveSuccessOrFail = null;
                OnAddNewClick();
                chkAccount = 1;
                jAlert('You must select all the ledgers either with Reverse Charge Applicable or all the ledgers </br>without Reverse charge applicable in a single entry to Save this entry.');
                //cbtnSaveNew.SetVisible(true);
                //cbtnSaveRecords.SetVisible(true);
                //document.getElementById('btnSaveNew').style.display = 'none';
                //document.getElementById('btnSaveRecords').style.display = 'none';

            }
            else if (gridTDS.cpSaveSuccessOrFail == "reverserequired") {
                gridTDS.cpSaveSuccessOrFail = null;
                OnAddNewClick();
                chkAccount = 1;
                jAlert('Selected Ledger(s) are mapped with Reverse Charge, please check Reverse Charge option.');
                //cbtnSaveNew.SetVisible(true);
                //cbtnSaveRecords.SetVisible(true);
                //document.getElementById('btnSaveNew').style.display = 'none';
                //document.getElementById('btnSaveRecords').style.display = 'none';
            }
            else if (gridTDS.cpSaveSuccessOrFail == "reversenotrequired") {
                gridTDS.cpSaveSuccessOrFail = null;
                OnAddNewClick();
                chkAccount = 1;
                jAlert('Selected Ledger(s) are not mapped with Reverse Charge, please un-check Reverse Charge option.');
                //cbtnSaveNew.SetVisible(true);
                //cbtnSaveRecords.SetVisible(true);
                //document.getElementById('btnSaveNew').style.display = 'none';
                //document.getElementById('btnSaveRecords').style.display = 'none';
            }
            else if (gridTDS.cpSaveSuccessOrFail == "reversetaxledgermissing") {
                gridTDS.cpSaveSuccessOrFail = null;
                OnAddNewClick();
                chkAccount = 1;
                jAlert('reversetaxledgermissing');
                //cbtnSaveNew.SetVisible(true);
                //cbtnSaveRecords.SetVisible(true);
                //document.getElementById('btnSaveNew').style.display = 'none';
                //document.getElementById('btnSaveRecords').style.display = 'none';
            }
            else if (gridTDS.cpSaveSuccessOrFail == "addressrequired") {
                gridTDS.cpSaveSuccessOrFail = null;
                OnAddNewClick();
                chkAccount = 1;

                jAlert("Please Enter Billing/Shipping and GSTIN Details to Calculate GST.", "Alert !!", function () {
                    page.SetActiveTabIndex(1);
                    //chinmoy edited for new billing shipping
                    //  cbsSave_BillingShipping.Focus();
                    cbtnSave_SalesBillingShiping.Focus();
                    page.tabs[0].SetEnabled(false);
                    $("#divcross").hide();
                });
                //cbtnSaveNew.SetVisible(true);
                //cbtnSaveRecords.SetVisible(true);
                //document.getElementById('btnSaveNew').style.display = 'none';
                //document.getElementById('btnSaveRecords').style.display = 'none';

            }
            else if (gridTDS.cpSaveSuccessOrFail == "taxREquired") {
                gridTDS.cpSaveSuccessOrFail = null;
                OnAddNewClick();
                chkAccount = 1;
                jAlert('Selected Ledger is tagged for GST calculation. Click on Charges to calculate GST and Proceed.');
                //cbtnSaveNew.SetVisible(true);
                //cbtnSaveRecords.SetVisible(true);
                //document.getElementById('btnSaveNew').style.display = 'none';
                //document.getElementById('btnSaveRecords').style.display = 'none';
            }
            else if (gridTDS.cpSaveSuccessOrFail == "successInsert") {

                if (gridTDS.cpVouvherNo != null) {
                    var JV_Number = gridTDS.cpVouvherNo;
                    var value = document.getElementById('hdnRefreshType').value;
                    var JV_Msg = "Cash/Bank Voucher No. " + JV_Number + " generated.";
                    var strSchemaType = document.getElementById('hdnSchemaType').value;

                    if (value == "E") {
                        if (JV_Number != "") {
                            var Type = gridTDS.cpType;
                            var newInvoiceId = gridTDS.cpAutoID;

                            //if (newInvoiceId == "-20")
                            //{
                            //    var mismatch_Msg = 'Mismatch in Debit & Credit Posting.';
                            //    jAlert(mismatch_Msg, 'Alert Dialog: [CashBank]', function (r) {
                            //        if (r == true) {                                       
                            //        }
                            //    });
                            //}
                            //else
                            //{
                            var AutoPrint = document.getElementById('hdnAutoPrint').value;
                            if (AutoPrint == "Yes") {
                                if (Type == "P") {
                                    window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=PaymentVoucher~D&modulename=CBVUCHR&id=" + newInvoiceId, '_blank');
                                }
                                else {
                                    window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=ReceiptVoucher~D&modulename=CBVUCHR&id=" + newInvoiceId, '_blank');
                                }
                            }
                            if (strSchemaType == '1') {
                                jAlert(JV_Msg, 'Alert Dialog: [CashBank]', function (r) {
                                    if (r == true) {
                                        window.location.assign("CashBankEntryList.aspx");
                                    }
                                });
                            }
                            else {
                                window.location.assign("CashBankEntryList.aspx");
                            }
                            //}                           

                        }
                        else {
                            // window.location.reload();
                            window.location.assign("CashBankEntryList.aspx");
                        }
                    }
                    else if (value == "S") {
                        if (JV_Number != "") {
                            var Type = gridTDS.cpType;
                            var newInvoiceId = gridTDS.cpAutoID;
                            var AutoPrint = document.getElementById('hdnAutoPrint').value;
                            if (AutoPrint == "Yes") {
                                if (Type == "P") {
                                    window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=PaymentVoucher~D&modulename=CBVUCHR&id=" + newInvoiceId, '_blank');
                                }
                                else {
                                    window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=ReceiptVoucher~D&modulename=CBVUCHR&id=" + newInvoiceId, '_blank');
                                }
                            }

                            // if (strSchemaType == '1') {
                            jAlert(JV_Msg, 'Alert Dialog: [CashBank]', function (r) {
                                if (r == true) {
                                    window.location.assign("CashBankEntry.aspx?key=ADD");
                                }
                            });
                            //jAlert(JV_Msg, "Alert!!", function () {
                            //});
                            // }
                        }
                        else {
                            // window.location.reload();
                            window.location.assign("CashBankEntry.aspx?key=ADD");
                        }
                    }
                    // gridTDS.cpVouvherNo = null;
                }
                if ($('#<%=hdnBtnClick.ClientID %>').val() == "Save_Exit") {
                    if (gridTDS.cpExitNew == "YES") {
                        //window.location.reload();
                        //window.location.assign("CashBankEntryList.aspx");
                    }
                    else {
                        OnAddNewClick();
                    }
                    gridTDS.cpType = null;
                    gridTDS.cpAutoID = null;
                }
                if ($('#<%=hdnBtnClick.ClientID %>').val() == "Save_New") {

                    //'''''''''''''''''''''''''''Commented for New Page
                    $("#divNumberingScheme").show();
                    $("#divEnterBranch").hide();
                    var Campany_ID = '<%=Session["LastCompany"]%>';
                    var LocalCurrency = '<%=Session["LocalCurrency"]%>';
                    var basedCurrency = LocalCurrency.split("~");
                    cCmbCurrency.SetValue(basedCurrency[0]);
                    c_txt_Debit.SetValue("0.0");
                    c_txt_Credit.SetValue("0.0");
                    ctxtRate.SetValue("");
                    //window.location.assign("CashBankEntry.aspx?key=ADD");

                    //'''''''''''''''''''''''end''''''''''''''''''''''''''''''''''''''''''''''

                    //if (cComboInstrumentTypee.GetValue() == "CH") {
                    //    document.getElementById("divInstrumentNo").style.display = 'none';
                    //    document.getElementById("tdIDateDiv").style.display = 'none';
                    //}
                    //else {
                    //    document.getElementById("divInstrumentNo").style.display = 'block';
                    //    document.getElementById("tdIDateDiv").style.display = 'block';
                    //}

                    <%--$('#<%=lblHeading.ClientID %>').text("");
                    $('#<%=lblHeading.ClientID %>').text("Add Cash/Bank Voucher");

                    deleteAllRows();
                    OnAddNewClick();
                    if (document.getElementById('txtVoucherNo').value == "Auto") {
                        document.getElementById('txtVoucherNo').value = "Auto";
                        cComboInstrumentTypee.Focus();
                    }
                    else {
                        document.getElementById('txtVoucherNo').value = "";
                        gridTDS.batchEditApi.EndEdit();
                        $('#txtVoucherNo').focus();
                    }

                    var CashBankId = cddlCashBank.GetValue();
                    var CashBankText = cddlCashBank.GetText();
                    var strbranch = $('#<%=hdnBranchId.ClientID %>').val();
                    var arr = CashBankText.split('|');
                    PopulateCurrentBankBalance(arr[0], strbranch);
                    // cComboType.Focus();


                    gridTDS.cpType = null;
                    gridTDS.cpAutoID = null;--%>
                }
            }
            ////##### coded by Samrat Roy - 14/04/2017 - ref IssueLog(Voucher - 110) 
            ////This method is called when request is for View Only .
            // if (gridTDS.cpView == "1") {
            if (ViewStatus == "1") {
                viewOnly();
            }
            else if (pageStatus == "delete") {
                $('#<%=hdnPageStatus.ClientID %>').val('');
                CustomDeleteID = "";
                //cbtnSaveNew.SetVisible(true);
                //cbtnSaveRecords.SetVisible(true);

                //document.getElementById('btnSaveNew').style.display = 'none';
                //document.getElementById('btnSaveRecords').style.display = 'none';

                OnAddNewClick();
            }
            else if (pageStatus == "update") {
                $('#<%=hdnPageStatus.ClientID %>').val('');
                    AddButtonClick();
                    var VoucherType = $('#rbtnType').val();
                //if (VoucherType == "P") {
                //    gridTDS.GetEditor('btnRecieve').SetEnabled(false);
                //}
                //else {
                //    gridTDS.GetEditor('btnPayment').SetEnabled(false);
                //}
                }
            //else if (pageStatus == "first") {
            //    AddButtonClick();
            //}

    }
    function GridADD() {
        // AddButtonClick();
        // gridTDS.PerformCallback('Display');

    }

    function Gridupdate() {
        var VoucherType = $("#rbtnType").val();

        var CashBank_ID = getParameterByName('key');
        //var CashBank_ID = document.getElementById('hdnEditRfid').Value;
        var Currency = cCmbCurrency.GetValue();
        var InstrumentType = cComboInstrumentTypee.GetValue();
        // EditAddressSinglePage(CashBank_ID, 'CBE');
        if (VoucherType == "P") {
            document.getElementById('divPaidTo').style.display = 'block';
            document.getElementById('divReceivedfrom').style.display = 'none';
        }
        else {
            document.getElementById('divReceivedfrom').style.display = 'block';
            document.getElementById('divPaidTo').style.display = 'none';
        }
        $("#txtVoucherNo").attr("disabled", "disabled");
        $("#ddlBranch").attr("disabled", "disabled");
        $("#ddlEnterBranch").attr("disabled", "disabled");
        $("#ddl_AmountAre").attr("disabled", "disabled");
        var setCurr = '<%=Session["LocalCurrency"]%>';
        var localCurrency = setCurr.split('~')[0];
        if (Currency != localCurrency) {
            $('#<%=hdnCurrenctId.ClientID %>').val(Currency);
            ctxtRate.SetEnabled(true);
        }
        else {
            ctxtRate.SetEnabled(false);
            $('#<%=hdnCurrenctId.ClientID %>').val("");
        }

        var WithdrawalType = "";
        if (InstrumentType == "C") {
            WithdrawalType = "Cheque";
        }
        else if (InstrumentType == "E") {
            WithdrawalType = "E.Transfer";
        }
        else if (InstrumentType == "D") {
            WithdrawalType = "Draft";
        }
        else if (InstrumentType == "CH") {
            WithdrawalType = "Cash";
            $('#<%=hdnInstrumentType.ClientID %>').val(0);
            document.getElementById("divInstrumentNo").style.display = 'none';
            document.getElementById("tdIDateDiv").style.display = 'none';
        }
        else {
            $('#<%=hdnInstrumentType.ClientID %>').val(InstrumentType);
        }

    WithdrawalChangedNew(WithdrawalType);
    cComboInstrumentTypee.SetValue(InstrumentType);

    gridTDS.batchEditApi.StartEdit(-1, 1);
    if ($('#<%=hdnEditClick.ClientID %>').val() == 'T') {
        OnAddNewClick();
        $('#<%=hdnEditClick.ClientID %>').val("");
    }
    $("#rbtnType").attr("disabled", "disabled");
    var CashBankText = cddlCashBank.GetText();
    var arr = CashBankText.split('|');
    var strbranch = $("#ddlBranch").val();
    PopulateCurrentBankBalance(arr[0], strbranch);
    document.getElementById('hdnEditCBID').value = CashBank_ID;
    document.getElementById('divNumberingScheme').style.display = 'none';
    document.getElementById('divEnterBranch').style.display = 'Block';
    gridTDS.PerformCallback('Display');


    $("#tdSaveNewButton").hide();


}

function OnBatchEditStartEditing(s, e) {

    currentEditableVisibleIndex = e.visibleIndex;
    globalRowIndex = e.visibleIndex;

    if ($("#hdnIsTDS").val() == "1") {
        e.cancel = true;
    }

    //var currentMainAccount = gridTDS.batchEditApi.GetCellValue(currentEditableVisibleIndex, "MainAccount");
    //var SubAccountIDColumn = s.GetColumnByField("bthSubAccount");
    //if (!e.rowValues.hasOwnProperty(SubAccountIDColumn.index))
    //    return;
    //var cellInfo = e.rowValues[SubAccountIDColumn.index];


    //if (lastMainAccountID == currentMainAccount)
    //    if (SubAccount_ReferenceID.FindItemByValue(cellInfo.value) != null)
    //        SubAccount_ReferenceID.SetValue(cellInfo.value);
    //    else {

    //        if (e.focusedColumn.fieldName != "TaxAmount") {
    //            LoadingPanel.Show();
    //        }
    //        RefreshData(cellInfo, lastMainAccountID);

    //    }

    //else {
    //    if (currentMainAccount == null) {
    //        SubAccount_ReferenceID.SetSelectedIndex(-1);
    //        return;
    //    }
    //    lastMainAccountID = currentMainAccount;


    //    if (e.focusedColumn.fieldName != "TaxAmount") {
    //        LoadingPanel.Show();
    //    }
    //    RefreshData(cellInfo, lastMainAccountID);

    //}
}
//function RefreshData(cellInfo, MainAccountID) {
//    setValueFlag = cellInfo.value;

//    if (setValueFlag != null) {
//        SubAccount_ReferenceID.PerformCallback(MainAccountID + '~' + setValueFlag);
//    }
//        // SubAccount_ReferenceID.PerformCallback(MainAccountID + '~' + setValueFlag);            
//        //else if(document.getElementById('hdnCheckAdd').value == 'YES')
//        //{
//        //    document.getElementById('hdnCheckAdd').value = "";
//        //    SubAccount_ReferenceID.PerformCallback(MainAccountID + '~' + setValueFlag);
//        //}
//    else {
//        LoadingPanel.Hide();
//    }
//}
//function OnBatchEditEndEditing(s, e) {

//    currentEditableVisibleIndex = -1;
//    var SubAccountIDColumn = s.GetColumnByField("bthSubAccount");
//    if (!e.rowValues.hasOwnProperty(SubAccountIDColumn.index))
//        return;
//    var cellInfo = e.rowValues[SubAccountIDColumn.index];
//    if (SubAccount_ReferenceID.GetSelectedIndex() > -1 || cellInfo.text != SubAccount_ReferenceID.GetText()) {
//        cellInfo.value = SubAccount_ReferenceID.GetValue();
//        cellInfo.text = SubAccount_ReferenceID.GetText();
//        SubAccount_ReferenceID.SetValue(null);
//    }
//}
//function SubAccountCombo_EndCallback(s, e) {
//    if (setValueFlag == null || setValueFlag == "0" || setValueFlag == "") {
//        s.SetSelectedIndex(-1);
//    }
//    else {
//        if (SubAccount_ReferenceID.FindItemByValue(setValueFlag) != null) {
//            SubAccount_ReferenceID.SetValue(setValueFlag);
//            setValueFlag = null;
//        }
//    }
//    var reverseApplicable = gridTDS.GetEditor("ReverseApplicable");
//    reverseApplicable.SetValue(SubAccount_ReferenceID.cpReverseApplicable);                
//    $("#IsTaxApplicable").val(SubAccount_ReferenceID.cpIsTaxable);
//    var VoucherType = document.getElementById('rbtnType').value;
//    if (SubAccount_ReferenceID.cpReverseApplicable == "1" && VoucherType == "P") {
//        $("#chk_reversemechenism").prop("disabled", false);
//        $("#chk_reversemechenism").prop("checked", true);
//    }
//    else {
//        if ($("#chk_reversemechenism").prop('checked') == false) {
//            $("#chk_reversemechenism").prop("checked", false);
//        }
//    }
//    SubAccount_ReferenceID.cpReverseApplicable = null;
//    LoadingPanel.Hide();
//}
function OnInit(s, e) {
    IntializeGlobalVariables(s);
}
//....end....
function AddButtonClick() {
    cCmbScheme.SetEnabled(true);
    ctxtRate.SetEnabled(false);
    // document.getElementById('rbtnType').value = 'P';
    var VoucherType = document.getElementById('rbtnType').value;
    $('#<%=hdnPayment.ClientID %>').val('NO');
    $('#<%=hdnTaxGridBind.ClientID %>').val('NO');
               <%-- document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;--%>
    OnAddNewClick();
    var defaultbranch = '<%=Session["userbranchID"]%>';
    $('#<%=hdnBranchId.ClientID %>').val(defaultbranch);
    var SaveModeCB = '<%=Session["SaveModeCB"]%>';
    if (SaveModeCB == "") {
        cCmbScheme.Focus();
        if ($('#hdn_Mode').val() != "Edit") {
        }
        document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;
    }
    else {
        if (SaveModeCB == 'A') {
            cdtTDate.Focus();
        }
        if (SaveModeCB == 'M') {
            document.getElementById("txtVoucherNo").focus();
        }
                    <%--var InstrumentType = cComboInstrumentTypee.GetValue();
                    var WithdrawalType = "";
                    if (InstrumentType == "C") {
                        WithdrawalType = "Cheque";
                    }
                    else if (InstrumentType == "E") {
                        WithdrawalType = "E.Transfer";
                    }
                    else if (InstrumentType == "D") {
                        WithdrawalType = "Draft";
                    }
                    else if (InstrumentType == "CH") {
                        WithdrawalType = "Cash";
                        $('#<%=hdnInstrumentType.ClientID %>').val(0);
                        document.getElementById("divInstrumentNo").style.display = 'none';
                        document.getElementById("tdIDateDiv").style.display = 'none';

                    }
                    else {
                        $('#<%=hdnInstrumentType.ClientID %>').val(InstrumentType);

                    }
                    WithdrawalChangedNew(WithdrawalType);
                    cComboInstrumentTypee.SetValue(InstrumentType);--%>

    }

    //LoadCustomerAddress('', $('#ddlBranch').val(), 'PO');
}
        function deleteAllRows() {
            var frontRow = 0;
            var backRow = -1;
            for (var i = 0; i <= gridTDS.GetVisibleRowsOnPage() + 100 ; i++) {
                gridTDS.DeleteRow(frontRow);
                gridTDS.DeleteRow(backRow);
                backRow--;
                frontRow++;
            }

        }
        //function focusval(obj) {

        //}

        $(function () {

            // BindCashBankAccountList();
            //BindBranchList();
            // ListBranchBind();
            // ListAccountBind();

            // ListMainAccountBind();
            //  ListSubAccountBind();


            //BindWithFromList();

            //ListWithFromBind();
            // BindDepositIntoList();
            //ListDepositIntoBind();
            <%--$("#lstCashItems").chosen().change(function () {
        $('#MandatoryCashBank').hide();
        var CashBankId = $("#lstCashItems").val()
        var CashBankText = $("#lstCashItems").find("option:selected").text()
        var strbranch = $('#<%=hdnBranchId.ClientID %>').val();
        var arr = CashBankText.split('|');
        PopulateCurrentBankBalance(arr[0], strbranch);
        $('#<%=hdnCashBankId.ClientID %>').val(CashBankId);
        $('#<%=hdnCashBankText.ClientID %>').val(CashBankText);
        var SpliteDetails = CashBankText.split(']');
        var WithDrawType = SpliteDetails[1].trim();
        if (WithDrawType == "Cash") {
            var comboitem = cComboInstrumentTypee.FindItemByValue('C');
            if (comboitem != undefined && comboitem != null) {
                cComboInstrumentTypee.RemoveItem(comboitem.index);
            }
            var comboitem = cComboInstrumentTypee.FindItemByValue('D');
            if (comboitem != undefined && comboitem != null) {
                cComboInstrumentTypee.RemoveItem(comboitem.index);
            }
            var comboitem = cComboInstrumentTypee.FindItemByValue('E');
            if (comboitem != undefined && comboitem != null) {
                cComboInstrumentTypee.RemoveItem(comboitem.index);
            }
            var comboitem = cComboInstrumentTypee.FindItemByValue('CH');
            if (comboitem == undefined || comboitem == null) {
                cComboInstrumentTypee.AddItem("Cash", "CH");
            }
            cComboInstrumentTypee.SetValue("CH");
            InstrumentTypeSelectedIndexChanged();
        }
        else {
            var comboitem = cComboInstrumentTypee.FindItemByValue('C');
            if (comboitem == undefined || comboitem == null) {
                cComboInstrumentTypee.AddItem("Cheque", "C");
            }
            var comboitem = cComboInstrumentTypee.FindItemByValue('D');
            if (comboitem == undefined || comboitem == null) {
                cComboInstrumentTypee.AddItem("Draft", "D");
            }
            var comboitem = cComboInstrumentTypee.FindItemByValue('E');
            if (comboitem == undefined || comboitem == null) {
                cComboInstrumentTypee.AddItem("E.Transfer", "E");
            }
            var comboitem = cComboInstrumentTypee.FindItemByValue('CH');
            if (comboitem != undefined && comboitem != null) {
                cComboInstrumentTypee.RemoveItem(comboitem.index);
                cComboInstrumentTypee.SetValue("C");
                InstrumentTypeSelectedIndexChanged();
            }
        }
    })--%>
            <%--$("#lstMainAccount").chosen().change(function () {
        var MainAccountId = $("#lstMainAccount").val();
        var MainAccountText = $("#lstMainAccount").find("option:selected").text()
        $('#<%=hdnMainAccountId.ClientID %>').val(MainAccountId);
        $('#<%=hdnMainAccountText.ClientID %>').val(MainAccountText);

        BindSubAccountList();

        keyVal(MainAccountId);
    })--%>


            <%-- $("#lstMainAccountE").chosen().change(function () {
        var MainAccountEId = $("#lstMainAccountE").val();
        var MainAccountEText = $("#lstMainAccountE").find("option:selected").text()
        $('#<%=hdnMainAccountEId.ClientID %>').val(MainAccountEId);
        $('#<%=hdnMainAccountEText.ClientID %>').val(MainAccountEText);
    })--%>
            <%-- $("#lstSubAccount").chosen().change(function () {
        var SubAccountId = $("#lstSubAccount").val();
        var SubAccountText = $("#lstSubAccount").find("option:selected").text()
        $('#<%=hdnSubAccountId.ClientID %>').val(SubAccountId);
        $('#<%=hdnSubAccountText.ClientID %>').val(SubAccountText);
    })--%>
  <%--  $("#lstBranchItems").chosen().change(function () {
        var BranchId = $("#lstBranchItems").val();
        var BranchText = $("#lstBranchItems").find("option:selected").text()
        $('#<%=hdnBranchId.ClientID %>').val(BranchId);
        var a = $('#<%=hdnBranchId.ClientID %>').val();
        $('#<%=hdnBranchText.ClientID %>').val(BranchText);
    })--%>
        });
        //ProtoType
        String.prototype.trim = function () {
            return this.replace(/^\s+|\s+$/g, "");
        }
        String.prototype.ltrim = function () {
            return this.replace(/^\s+/, "");
        }
        String.prototype.rtrim = function () {
            return this.replace(/\s+$/, "");
        }
        //Global Variable
        FieldName = 'txtVoucherNo';
        IsSubAccountChange = "False";
        Param_SubAccountID = '';
        SubLedgerType = "";
        ActiveCurrencyID = "";
        ActiveCurrencyName = "";
        ActiveCurrencySymbol = "";

       <%-- function PageLoad() {
            var val = "0";
            var AspRadio = document.getElementById("rbtnType");
            var AspRadio_ListItem = AspRadio.getElementsByTagName('input');
            for (var i = 0; i < AspRadio_ListItem.length; i++) {
                if (AspRadio_ListItem[i].checked) {
                    val = AspRadio_ListItem[i].value;
                }
            }
            if (val == "R") {
            }
            else {

            }
            var ActiveCurrency = '<%=Session["ActiveCurrency"]%>'
            ActiveCurrencyID = ActiveCurrency.split('~')[0];
            ActiveCurrencyName = ActiveCurrency.split('~')[1];
            ActiveCurrencySymbol = ActiveCurrency.split('~')[2];
            FinYearCheckOnPageLoad();
        }--%>
        function checkTextAreaMaxLength(textBox, e, length) {

            var mLen = textBox["MaxLength"];
            if (null == mLen)
                mLen = length;
            var maxLength = parseInt(mLen);
            if (!checkSpecialKeys(e)) {
                if (textBox.value.length > maxLength - 1) {
                    if (window.event)//IE
                        e.returnValue = false;
                    else//Firefox
                        e.preventDefault();
                }
            }
        }

        function checkSpecialKeys(e) {
            if (e.keyCode != 8 && e.keyCode != 46 && e.keyCode != 37 && e.keyCode != 38 && e.keyCode != 39 && e.keyCode != 40)
                return false;
            else
                return true;
        }

       <%-- function OnComboInstTypeSelectedIndexChanged() {

            SetAllDisplayNone();
            //kaushik
            var txtSubAccountText = $('#<%=hdnSubAccountText.ClientID %>').val();
            var Mode = document.getElementById("hdn_Mode").value;
            var VoucherType;
            var InstType;
            if (Mode == "Entry") {
                //VoucherType = cComboType.GetValue();
                VoucherType = document.getElementById('rbtnType').value;
                InstType = cComboInstType.GetValue();
            }
            else {
                VoucherType = cSCmb_Type.GetValue();
                InstType = cComboInstType.GetValue();
            }
            AccountType = document.getElementById('hdnAccountType').value;
            if (VoucherType == "P") {
                if (AccountType == 'EXPENCES' && InstType != "CH") {
                    document.getElementById("tdPayeeLable").style.visibility = 'visible';
                    document.getElementById("tdPayeeValue").style.visibility = 'visible';
                }
                document.getElementById("tdpayment").style.display = 'inline'
                document.getElementById("tdpaymentValue").style.display = 'inline'
                document.getElementById("tdpaymentDiv").style.display = 'block'
                if (InstType == "E") {
                    ctxtInstNo.SetText('E - Net');
                }
                else {
                    CmbClientBankCI.PerformCallback("SetFocusOnInstType~");
                }
                if (InstType != "CH") {
                    document.getElementById("tdINoLable").style.visibility = 'inherit'
                    document.getElementById("tdINoDiv").style.display = 'block'
                    document.getElementById("tdINoValue").style.visibility = 'inherit'
                    document.getElementById("tdIDateLable").style.visibility = 'inherit'
                    document.getElementById("tdIDateDiv").style.display = 'block'
                    document.getElementById("tdIDateValue").style.visibility = 'inherit'
                }

            }
            if (VoucherType == "R") {
                if (InstType == "C" || InstType == "E") {
                    if (SubLedgerType.toUpperCase() == 'CUSTOMERS') {
                        document.getElementById("tdCBankLable").style.display = 'inline';
                        document.getElementById("tdCBankValue").style.visibility = 'visible';
                        var SubAccountID = $('#<%=hdnSubAccountId.ClientID %>').val();
                if (SubAccountID != '') {
                    SubAccountID = SubAccountID.split('~')[0];
                    CmbClientBankCI.PerformCallback("ClientBankBind~" + SubAccountID);
                }
                else {
                    SubAccountID = document.getElementById('hdn_SubAccountIDE').value;
                    CmbClientBankCI.PerformCallback("ClientBankBind~" + SubAccountID);
                }
            }
        }
        else {
            if (InstType != "CH") {
                document.getElementById("tdIBankLable").style.display = 'block';
                document.getElementById("tdIBankValue").style.display = 'block';

            }
        }
        document.getElementById("tdRecieveDiv").style.display = 'block'
        document.getElementById("tdRecieve").style.display = 'block'
        document.getElementById("tdRecieveValue").style.display = 'block'


        //kaushik

        if (InstType == "E") {
            ctxtInstNo.SetText('E - Net');
        }
        else {
            ctxtInstNo.SetText('');
        }
        if (InstType != "CH") {
            document.getElementById("tdINoLable").style.visibility = 'inherit'
            document.getElementById("tdINoDiv").style.display = 'block'
            document.getElementById("tdINoValue").style.visibility = 'inherit'
            document.getElementById("tdIDateLable").style.visibility = 'inherit'
            document.getElementById("tdIDateDiv").style.display = 'block'
            document.getElementById("tdIDateValue").style.visibility = 'inherit'
        }


    }
    if (VoucherType == "C") {
        //Contra Change
        document.getElementById("tdContraEntry").style.display = 'block'
        if (InstType != "CH") {
            document.getElementById("tdINoLable").style.visibility = 'inherit'
            document.getElementById("tdINoDiv").style.display = 'block'
            document.getElementById("tdINoValue").style.visibility = 'inherit'
            document.getElementById("tdIDateLable").style.visibility = 'inherit'
            document.getElementById("tdIDateDiv").style.display = 'block'
            document.getElementById("tdIDateValue").style.visibility = 'inherit'
        }
        else {
            document.getElementById("tdINoLable").style.visibility = 'hidden'
            document.getElementById("tdINoDiv").style.display = 'none'
            document.getElementById("tdINoValue").style.visibility = 'hidden'
            document.getElementById("tdIDateLable").style.visibility = 'hidden'
            document.getElementById("tdIDateDiv").style.display = 'none'
            document.getElementById("tdIDateValue").style.visibility = 'hidden'

        }

        if (InstType == "E") {
            ctxtInstNo.SetText('E - Net');
        }
        else {
            ctxtInstNo.SetText('');
        }
    }
}--%>
        function SetAllDisplayNone() {
            document.getElementById("tdIBankLable").style.display = 'none';
            document.getElementById("tdIBankValue").style.display = 'none';
            document.getElementById("tdCBankLable").style.display = 'none';
            document.getElementById("tdCBankValue").style.visibility = 'hidden'
            document.getElementById("tdAuthLable").style.display = 'none';
            document.getElementById("tdAuthValue").style.display = 'none';
            document.getElementById("tdPayeeLable").style.visibility = 'hidden';
            document.getElementById("tdPayeeValue").style.visibility = 'hidden';
            document.getElementById("tdContraEntry").style.display = 'none';
            document.getElementById("tdpayment").style.display = 'none';
            document.getElementById("tdpaymentValue").style.display = 'none';
            document.getElementById("tdpaymentDiv").style.display = 'none'
            document.getElementById("tdRecieve").style.display = 'none';
            document.getElementById("tdRecieveValue").style.display = 'none';
            document.getElementById("tdRecieveDiv").style.display = 'none'
            document.getElementById("tdINoLable").style.visibility = 'hidden'
            document.getElementById("tdINoDiv").style.display = 'none'
            document.getElementById("tdINoValue").style.visibility = 'hidden'
            document.getElementById("tdIDateLable").style.visibility = 'hidden'
            document.getElementById("tdIDateDiv").style.display = 'none'
            document.getElementById("tdIDateValue").style.visibility = 'hidden'

        }
<%--function CallBankAccount(obj1, obj2, obj3) {
    var CurrentSegment = document.getElementById('hdn_CurrentSegment').value;
    var Mode = document.getElementById("hdn_Mode").value;
    var strPutSegment = " and (MainAccount_ExchangeSegment=" + CurrentSegment + " or MainAccount_ExchangeSegment=0)";
    var strQuery_Table = "(Select MainAccount_AccountCode+\'-\'+MainAccount_Name+\' [ \'+MainAccount_BankAcNumber+\' ]\'+\' ~ \'+MainAccount_BankCashType as IntegrateMainAccount,MainAccount_AccountCode+\'~\'+MainAccount_BankCashType+\'~CASHBANK\' as MainAccount_AccountCode,MainAccount_Name,MainAccount_BankAcNumber from Master_MainAccount where (MainAccount_BankCashType=\'Bank\' or MainAccount_BankCashType=\'Cash\') and (MainAccount_BankCompany=\'" + '<%=Session["LastCompany"] %>' + "\' Or IsNull(MainAccount_BankCompany,'')='')" + strPutSegment + ") as t1";
    var strQuery_FieldName = " Top 10 * ";
    var strQuery_WhereClause = "MainAccount_AccountCode like (\'%RequestLetter%\') or MainAccount_Name like (\'%RequestLetter%\') or MainAccount_BankAcNumber like (\'%RequestLetter%\')";
    var strQuery_OrderBy = '';
    var strQuery_GroupBy = '';
    var CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);
    //kaushik
    ajax_showOptions(obj1, obj2, obj3, replaceChars(CombinedQuery), 'Main');
}--%>
        //function ListBranchBind() {

        //    var config = {
        //        '.chsn': {},
        //        '.chsn-deselect': { allow_single_deselect: true },
        //        '.chsn-no-single': { disable_search_threshold: 10 },
        //        '.chsn-no-results': { no_results_text: 'Oops, nothing found!' },
        //        '.chsn-width': { width: "100%" }
        //    }
        //    for (var selector in config) {
        //        $(selector).chosen(config[selector]);
        //    }
        //}
        //function ListMainAccountBind() {

        //    var config = {
        //        '.chsn': {},
        //        '.chsn-deselect': { allow_single_deselect: true },
        //        '.chsn-no-single': { disable_search_threshold: 10 },
        //        '.chsn-no-results': { no_results_text: 'Oops, nothing found!' },
        //        '.chsn-width': { width: "100%" }
        //    }
        //    for (var selector in config) {
        //        $(selector).chosen(config[selector]);
        //    }

        //}
        //function ListMainAccountEBind() {
        //    $('#lstMainAccountE').chosen();
        //}
        //function ListSubAccountBind() {
        //    var config = {
        //        '.chsn': {},
        //        '.chsn-deselect': { allow_single_deselect: true },
        //        '.chsn-no-single': { disable_search_threshold: 10 },
        //        '.chsn-no-results': { no_results_text: 'Oops, nothing found!' },
        //        '.chsn-width': { width: "100%" }
        //    }
        //    for (var selector in config) {
        //        $(selector).chosen(config[selector]);
        //    }
        //}
        function PopulateCurrentBankBalance(MainAccountID, BranchId) {
            if (MainAccountID.trim() == "" || BranchId.trim() == "") {
                return;
            }
            else {
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
                                document.getElementById('<%=B_BankBalance.ClientID %>').style.color = "Black";
                            }
                            else {
                                document.getElementById('<%=B_ImgSymbolBankBal.ClientID %>').innerHTML = '';
                                document.getElementById('<%=B_BankBalance.ClientID %>').innerHTML = '0.0';
                                document.getElementById('<%=B_BankBalance.ClientID %>').style.color = "Black";
                            }
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        jAlert(textStatus);
                    }
                });
            }
        }
        //function setDepositIntoBind(obj) {
        //    if (obj) {
        //        var lstDepositInto = document.getElementById("lstDepositIntoItems");
        //        for (var i = 0; i < lstDepositInto.options.length; i++) {

        //            var DepositIntoval = lstDepositInto.options[i].value;
        //            var n = DepositIntoval.indexOf("~");
        //            var res = DepositIntoval.substr(0, n);
        //            if (res == obj) {
        //                lstDepositInto.options[i].selected = true;
        //            }
        //        }
        //    }
        //}
        //........Bind Branch......
        //function BindBranchList() {
        //    var lBox = $('select[id$=lstBranchItems]');
        //    var lstBranchItems = [];
        //    //Customer or Lead radio button is clicked kaushik 21-11-2016
        //    lBox.empty();
        //    $.ajax({
        //        type: "POST",
        //        url: 'CashBankEntry.aspx/GetBranchList',
        //        contentType: "application/json; charset=utf-8",
        //        dataType: "json",
        //        success: function (msg) {
        //            var list = msg.d;
        //            if (list.length > 0) {

        //                for (var i = 0; i < list.length; i++) {

        //                    var id = '';
        //                    var name = '';
        //                    id = list[i].split('|')[1];
        //                    name = list[i].split('|')[0];

        //                    lstBranchItems.push('<option value="' +
        //                    id + '">' + name
        //                    + '</option>');
        //                }

        //                $(lBox).append(lstBranchItems.join(''));
        //                ListBranchBind();
        //                $('#lstBranchItems').trigger("chosen:updated");
        //                $('#lstBranchItems').prop('disabled', false).trigger("chosen:updated");

        //            }
        //            else {
        //                lBox.empty();
        //                ListBranchBind();
        //                $('#lstBranchItems').trigger("chosen:updated");
        //                $('#lstBranchItems').prop('disabled', true).trigger("chosen:updated");

        //            }
        //        },
        //        error: function (XMLHttpRequest, textStatus, errorThrown) {
        //            jAlert(textStatus);
        //        }
        //    });
        //}
        //......end....

        function FinYearCheckOnPageLoad() {
            var SelectedDate = new Date(cdtTDate.GetDate());
            var monthnumber = SelectedDate.getMonth();
            var monthday = SelectedDate.getDate();
            var year = SelectedDate.getYear();
            var SelectedDateValue = new Date(year, monthnumber, monthday);
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
                if (SelectedDateNumericValue < FinYearStartDateNumericValue) {
                    cdtTDate.SetDate(new Date(FinYearStartDate));
                }
                if (SelectedDateNumericValue > FinYearEndDatNumbericValue) {
                    cdtTDate.SetDate(new Date(FinYearEndDate));
                }
            }
        }
        function TDateChange() {
            var SelectedDate = new Date(cdtTDate.GetDate());
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
                cdtTDate.SetDate(MaxLockDate);
                return;
            }
            ///End Checking of Transaction Date For MaxLockDate
            ///Date Should Between Current Fin Year StartDate and EndDate
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
                //                   alert('Between');
            }
            else {
                // jAlert('Enter Date Is Outside Of Financial Year !!');
                jAlert('Enter Date Is Outside Of Financial Year !!', 'Alert Dialog: [CashBank]', function (r) {
                    if (r == true) {
                        cdtTDate.Focus();
                    }
                });
                if (SelectedDateNumericValue < FinYearStartDateNumericValue) {
                    cdtTDate.SetDate(new Date(FinYearStartDate));
                }
                if (SelectedDateNumericValue > FinYearEndDatNumbericValue) {
                    cdtTDate.SetDate(new Date(FinYearEndDate));
                }
            }
            cInstDate.SetDate(SelectedDate);
            ///End OF Date Should Between Current Fin Year StartDate and EndDate
        }

        function GetServerDateFormat(today) {
            if (today != "" && today != null) {
                var dd = today.getDate();
                var mm = today.getMonth() + 1;
                var yyyy = today.getFullYear();

                if (dd < 10) {
                    dd = '0' + dd;
                }
                if (mm < 10) {
                    mm = '0' + mm;
                }
                today = yyyy + '-' + mm + '-' + dd;
            }
            else {
                today = "";
            }

            return today;
        }

        function SaveButtonClick() {

            $('#<%=hdnBtnClick.ClientID %>').val("Save_Exit");
            $('#<%=hdnJNMode.ClientID %>').val('0'); //Entry     
            $('#<%=hdnRefreshType.ClientID %>').val('E');
            $('#<%=hdfIsDelete.ClientID %>').val('I');
            $('#<%=hdnPayment.ClientID %>').val('NO');
            $('#<%=hdnTaxGridBind.ClientID %>').val('NO');
            var PaidToYesNO = document.getElementById('hdnPaidToYesNO').value;
            var VoucherType = document.getElementById('rbtnType').value;
            var Branch = $('#ddlBranch').val();
            var CashBank = cddlCashBank.GetValue();
            var InstrumentType = cComboInstrumentTypee.GetValue();
            var InstrumentNo = ctxtInstNobth.GetValue();
            var InstType = document.getElementById('hdn_CashBankType_InstType').value;

            if ($("#hdnProjectSelectInEntryModule").val() == "1") {
                var ProjectCode = clookup_Project.GetText();
                if ($("#hdnProjectSelectInEntryModule").val() == "1" && $("#hdnProjectMandatory").val() == "1" && ProjectCode == "") {
                    jAlert("Please Select Project.");
                    return false;
                }
            }

            if (document.getElementById('<%= txtVoucherNo.ClientID %>').value.trim() == "") {
                $("#MandatoryBillNo").show();
                return false;
            }
            else if (Branch == null) {
                $("#MandatoryBranch").show();
                return false;
            }
            else if (CashBank == null) {
                $("#MandatoryCashBank").show();
                return false;
            }
            else if (InstrumentType == "NA") {
                $("#MandatoryInstrumentType").show();
                return false;
            }
            else if (InstType == "Yes" && InstrumentType == "C") {
                if (InstrumentNo == null) {
                    $("#MandatoryInstNo").show();
                    return false;
                }
            }
            if (PaidToYesNO == "Yes" && VoucherType == "P") {
                var strPaidTo = ctxtPaidTo.GetText().trim();
                if (strPaidTo == "") {
                    $("#MandatoryPaidTo").show();
                    return false;
                }
            }
            if ($('#<%=hdnInstrumentNo.ClientID %>').val() == "") {
                $('#<%=hdnInstrumentNo.ClientID %>').val(InstrumentNo);
            }
            //gridTDS.UpdateEdit();
            gridTDS.batchEditApi.EndEdit();
            var gridCount = gridTDS.GetVisibleRowsOnPage();
            //var VoucherType = cComboType.GetValue();

            var txtTotalAmount = c_txt_Debit.GetValue() != null ? c_txt_Debit.GetValue() : 0;
            var txtTotalPayment = c_txt_Credit.GetValue() != null ? c_txt_Credit.GetValue() : 0;
            var IsType = "";
            var frontRow = 0;
            var backRow = -1;
            IsType = "Y";
            for (var i = 0; i <= gridTDS.GetVisibleRowsOnPage() ; i++) {
                var frontProduct = (gridTDS.batchEditApi.GetCellValue(backRow, 'MainAccount') != null) ? (gridTDS.batchEditApi.GetCellValue(backRow, 'MainAccount')) : "";
                var backProduct = (gridTDS.batchEditApi.GetCellValue(frontRow, 'MainAccount') != null) ? (gridTDS.batchEditApi.GetCellValue(frontRow, 'MainAccount')) : "";
                if (frontProduct != "" || backProduct != "") {
                    IsType = "Y";
                    break;
                }
                backRow--;
                frontRow++;
            }
            if (gridCount > 0) {
                // if (chkAccount == 1) {
                if (IsType == "Y") {
                    if (VoucherType == "P") {
                        if (parseFloat(txtTotalAmount) >= parseFloat(txtTotalPayment)) {
                            //OnAddNewClick();
                            //cbtnSaveNew.SetVisible(false);
                            //cbtnSaveRecords.SetVisible(false);


                            //document.getElementById('btnSaveNew').style.display = 'block';
                            // document.getElementById('btnSaveRecords').style.display = 'block';
                            //SaveTDS();

                            var urlKeys = getUrlVars();
                            var CashBankVoucherID;
                            if (urlKeys.key != 'ADD') {
                                CashBankVoucherID = urlKeys.key;
                            }
                            else {
                                CashBankVoucherID = 0;
                            }
                            //debugger;
                            var PostingDate = GetServerDateFormat(cdtTDate.GetValue());

                            $.ajax({
                                type: "POST",
                                url: "CashBankEntryTDS.aspx/GetTotalBalanceByCashBankID",
                                data: JSON.stringify({ CashBankID: CashBank, CashBankVoucherID: CashBankVoucherID, PostingDate: PostingDate }),
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                async: false,
                                success: function (msg) {
                                    var data = msg.d;
                                    var VoucherAmount = data.toString().split('~')[0];
                                    var BalanceLimit = data.toString().split('~')[1];
                                    var BalanceExceed = data.toString().split('~')[2];
                                    var closingBalnc = data.toString().split('~')[3];
                                    if (BalanceLimit != '0.00') {
                                        var TotalVoucherAmount = parseFloat(txtTotalAmount) - parseFloat(txtTotalPayment); //+ parseFloat(VoucherAmount);
                                        var closingAmount = parseFloat(closingBalnc) - parseFloat(TotalVoucherAmount);
                                        if (parseFloat(closingAmount) < parseFloat(BalanceLimit)) {
                                            //if (parseFloat(TotalVoucherAmount) > parseFloat(BalanceLimit)) {

                                            if (BalanceExceed.trim() == 'W') {

                                                jConfirm('Cash/Bank Balance - Limit is exceed do you want to proceed?', 'Confirmation Dialog', function (r) {
                                                    if (r == true) {
                                                        OnAddNewClick();
                                                        //cbtnSaveNew.SetVisible(false);
                                                        //cbtnSaveRecords.SetVisible(false);


                                                        gridTDS.UpdateEdit();
                                                        chkAccount = 0;
                                                    }
                                                    else {

                                                    }
                                                });
                                            }
                                            else if (BalanceExceed.trim() == 'B') {
                                                jAlert('Cash/Bank Balance - Limit is exceed can not proceed');

                                            }
                                            else if (BalanceExceed.trim() == 'S') {
                                                //OnAddNewClick();
                                                //cbtnSaveNew.SetVisible(false);
                                                //cbtnSaveRecords.SetVisible(false);

                                                //gridTDS.UpdateEdit();
                                                //chkAccount = 0;
                                                jAlert('Please select Cash/Bank Balance - Limit exceed option.');
                                            }
                                            else if (BalanceExceed.trim() == 'I') {
                                                OnAddNewClick();
                                                //cbtnSaveNew.SetVisible(false);
                                                //cbtnSaveRecords.SetVisible(false);

                                                gridTDS.UpdateEdit();
                                                chkAccount = 0;
                                            }
                                        }
                                        else {
                                            OnAddNewClick();
                                            //cbtnSaveNew.SetVisible(false);
                                            //cbtnSaveRecords.SetVisible(false);

                                            gridTDS.UpdateEdit();
                                            chkAccount = 0;
                                        }
                                    }
                                    else {
                                        OnAddNewClick();
                                        //cbtnSaveNew.SetVisible(false);
                                        //cbtnSaveRecords.SetVisible(false);

                                        gridTDS.UpdateEdit();
                                        chkAccount = 0;
                                    }

                                }
                            });



                            //gridTDS.UpdateEdit();
                            //chkAccount = 0;
                        }
                        else {
                            chkAccount = 1;
                            jAlert('As per the selcted Voucher type, Payment column amount should be greater than Receipt column amount.');
                        }
                    }
                    if (VoucherType == "R") {
                        if (parseFloat(txtTotalAmount) >= parseFloat(txtTotalPayment)) {
                            OnAddNewClick();
                            //cbtnSaveNew.SetVisible(false);
                            //cbtnSaveRecords.SetVisible(false);
                            //document.getElementById('btnSaveNew').style.display = 'block';
                            // document.getElementById('btnSaveRecords').style.display = 'block';
                            //SaveTDS();
                            gridTDS.UpdateEdit();
                            chkAccount = 0;
                        }
                        else {
                            chkAccount = 1;
                            jAlert('As per the selcted Voucher type, Receipt column amount should be greater than Payment column amount.');
                        }
                    }
                }
                else {
                    //chkAccount = 0;
                    jAlert('Cannot Save. You must enter atleast one Record to save this entry.');
                }
            }
            else {
                //chkAccount = 0;
                jAlert('Cannot Save. You must enter atleast one Record to save this entry.');
            }
        }

        function DeleteTDSRows(uniqueID) {
            for (var i = 0; i < 1000; i++) {
                if (gridTDS.GetRow(i)) {
                    if (gridTDS.GetRow(i).style.display != "none") {
                        gridTDS.batchEditApi.StartEdit(i, 2);
                        if (gridTDS.GetEditor("UniqueID").GetText() == uniqueID) {
                            gridTDS.DeleteRow(i);
                        }
                    }
                }
            }

            for (i = -1; i > -1000; i--) {
                if (gridTDS.GetRow(i)) {
                    if (gridTDS.GetRow(i).style.display != "none") {
                        gridTDS.batchEditApi.StartEdit(i, 2);
                        if (gridTDS.GetEditor("UniqueID").GetText() == uniqueID) {
                            gridTDS.DeleteRow(i);
                        }
                    }
                }
            }
            if (gridTDS.GetVisibleItemsOnPage() == 0) gridTDS.AddNewRow();
        }

        function txtBillNo_TextChanged() {
            var VoucherNo = document.getElementById("txtVoucherNo").value;
            $.ajax({
                type: "POST",
                url: "CashBankEntry.aspx/CheckUniqueName",
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
        function SaveButtonClickNew() {

            $('#<%=hdnBtnClick.ClientID %>').val("Save_New");
            $('#<%=hdnJNMode.ClientID %>').val('0'); //Entry  
            $('#<%=hdnRefreshType.ClientID %>').val('S');
            $('#<%=hdfIsDelete.ClientID %>').val('I');
            $('#<%=hdnPayment.ClientID %>').val('NO');
            $('#<%=hdnTaxGridBind.ClientID %>').val('NO');
            var VoucherType = document.getElementById('rbtnType').value;

            var Branch = $('#ddlBranch').val();
            var CashBank = cddlCashBank.GetValue();
            var InstrumentType = cComboInstrumentTypee.GetValue();
            var InstrumentNo = ctxtInstNobth.GetValue();
            var CashBankId = cddlCashBank.GetValue();
            var InstType = document.getElementById('hdn_CashBankType_InstType').value;
            var PaidToYesNO = document.getElementById('hdnPaidToYesNO').value;

            if ($("#hdnProjectSelectInEntryModule").val() == "1") {
                var ProjectCode = clookup_Project.GetText();
                if ($("#hdnProjectSelectInEntryModule").val() == "1" && $("#hdnProjectMandatory").val() == "1" && ProjectCode == "") {
                    jAlert("Please Select Project.");
                    return false;
                }
            }

            if (document.getElementById('<%= txtVoucherNo.ClientID %>').value.trim() == "") {
                $("#MandatoryBillNo").show();

                return false;
            }
            else if (Branch == null) {
                $("#MandatoryBranch").show();
                return false;
            }
            else if (CashBank == null) {
                $("#MandatoryCashBank").show();
                return false;
            }
            else if (InstrumentType == "NA") {
                $("#MandatoryInstrumentType").show();
                return false;
            }
            else if (InstType == "Yes" && InstrumentType == "C") {
                if (InstrumentNo == null) {
                    $("#MandatoryInstNo").show();
                    return false;
                }
            }
            if (PaidToYesNO == "Yes" && VoucherType == "P") {
                var strPaidTo = ctxtPaidTo.GetText().trim();
                if (strPaidTo == "") {
                    $("#MandatoryPaidTo").show();
                    return false;
                }

            }
            if ($('#<%=hdnInstrumentNo.ClientID %>').val() == "") {
                $('#<%=hdnInstrumentNo.ClientID %>').val(InstrumentNo);
            }
            var CashBankText = cddlCashBank.GetText();
            // var SpliteDetails = CashBankText.split(']');
            // var WithDrawType = SpliteDetails[1].trim();
            var WithDrawType = "";
            if (InstrumentType == "C") {
                WithDrawType = "Cheque";
            }
            else if (InstrumentType == "E") {
                WithDrawType = "E.Transfer";
            }
            else if (InstrumentType == "D") {
                WithDrawType = "Draft";
            }
            else if (InstrumentType == "CH") {
                WithDrawType = "Cash";
            }
            WithdrawalChangedNew(WithDrawType);
            cComboInstrumentTypee.SetValue(InstrumentType);
            InstrumentTypeSelectedIndexChanged();
            //Code added by Sudip
            gridTDS.batchEditApi.EndEdit();
            var gridCount = gridTDS.GetVisibleRowsOnPage();

            var txtTotalAmount = c_txt_Debit.GetValue() != null ? c_txt_Debit.GetValue() : 0;
            var txtTotalPayment = c_txt_Credit.GetValue() != null ? c_txt_Credit.GetValue() : 0;
            // var VoucherType = cComboType.GetValue();

            var IsType = "";
            var frontRow = 0;
            var backRow = -1;
            IsType = "Y";
            for (var i = 0; i <= gridTDS.GetVisibleRowsOnPage() ; i++) {
                var frontProduct = (gridTDS.batchEditApi.GetCellValue(backRow, 'MainAccount') != null) ? (gridTDS.batchEditApi.GetCellValue(backRow, 'MainAccount')) : "";
                var backProduct = (gridTDS.batchEditApi.GetCellValue(frontRow, 'MainAccount') != null) ? (gridTDS.batchEditApi.GetCellValue(frontRow, 'MainAccount')) : "";
                if (frontProduct != "" || backProduct != "") {
                    IsType = "Y";
                    break;
                }
                backRow--;
                frontRow++;
            }
            if (gridCount > 0) {
                // if (chkAccount == 1) {
                if (IsType == "Y") {
                    //debugger;
                    if (VoucherType == "P") {
                        if (parseFloat(txtTotalAmount) >= parseFloat(txtTotalPayment)) {
                            //OnAddNewClick();
                            //cbtnSaveNew.SetVisible(false);
                            //cbtnSaveRecords.SetVisible(false);
                            //document.getElementById('btnSaveNew').style.display = 'block';
                            //document.getElementById('btnSaveRecords').style.display = 'block';
                            //SaveTDS();

                            var urlKeys = getUrlVars();
                            var CashBankVoucherID;
                            if (urlKeys.key != 'ADD') {
                                CashBankVoucherID = urlKeys.key;
                            }
                            else {
                                CashBankVoucherID = 0;
                            }
                            var PostingDate = GetServerDateFormat(cdtTDate.GetValue());

                            $.ajax({
                                type: "POST",
                                url: "CashBankEntryTDS.aspx/GetTotalBalanceByCashBankID",
                                data: JSON.stringify({ CashBankID: CashBank, CashBankVoucherID: CashBankVoucherID, PostingDate: PostingDate }),
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                async: false,
                                success: function (msg) {
                                    var data = msg.d;
                                    var VoucherAmount = data.toString().split('~')[0];
                                    var BalanceLimit = data.toString().split('~')[1];
                                    var BalanceExceed = data.toString().split('~')[2];
                                    var closingBalnc = data.toString().split('~')[3];
                                    if (BalanceLimit != '0.00') {
                                        var TotalVoucherAmount = parseFloat(txtTotalAmount) - parseFloat(txtTotalPayment); //+ parseFloat(VoucherAmount);
                                        var closingAmount = parseFloat(closingBalnc) - parseFloat(TotalVoucherAmount);
                                        if (parseFloat(closingAmount) < parseFloat(BalanceLimit)) {
                                            //if (parseFloat(TotalVoucherAmount) > parseFloat(BalanceLimit)) {

                                            if (BalanceExceed.trim() == 'W') {

                                                jConfirm('Cash/Bank Balance - Limit is exceed do you want to proceed?', 'Confirmation Dialog', function (r) {
                                                    if (r == true) {
                                                        OnAddNewClick();
                                                        //cbtnSaveNew.SetVisible(false);
                                                        //cbtnSaveRecords.SetVisible(false);


                                                        gridTDS.UpdateEdit();
                                                        chkAccount = 0;
                                                    }
                                                    else {

                                                    }
                                                });
                                            }
                                            else if (BalanceExceed.trim() == 'B') {
                                                jAlert('Cash/Bank Balance - Limit is exceed can not proceed');

                                            }
                                            else if (BalanceExceed.trim() == 'S') {
                                                //OnAddNewClick();
                                                //cbtnSaveNew.SetVisible(false);
                                                //cbtnSaveRecords.SetVisible(false);

                                                //gridTDS.UpdateEdit();
                                                //chkAccount = 0;
                                                jAlert('Please select Cash/Bank Balance - Limit exceed option.');

                                            }
                                            else if (BalanceExceed.trim() == 'I') {
                                                OnAddNewClick();
                                                //cbtnSaveNew.SetVisible(false);
                                                //cbtnSaveRecords.SetVisible(false);

                                                gridTDS.UpdateEdit();
                                                chkAccount = 0;

                                            }
                                        }
                                        else {
                                            OnAddNewClick();
                                            //cbtnSaveNew.SetVisible(false);
                                            //cbtnSaveRecords.SetVisible(false);

                                            gridTDS.UpdateEdit();
                                            chkAccount = 0;
                                        }
                                    }
                                    else {
                                        OnAddNewClick();
                                        //cbtnSaveNew.SetVisible(false);
                                        //cbtnSaveRecords.SetVisible(false);

                                        gridTDS.UpdateEdit();
                                        chkAccount = 0;
                                    }

                                }
                            });








                            //gridTDS.UpdateEdit();
                            //chkAccount = 0;
                        }
                        else {
                            chkAccount = 1;
                            jAlert('Payment amount can not be less than receipt amount ');

                        }
                    }


                    if (VoucherType == "R") {
                        if (parseFloat(txtTotalAmount) >= parseFloat(txtTotalPayment)) {
                            OnAddNewClick();
                            //cbtnSaveNew.SetVisible(false);
                            //cbtnSaveRecords.SetVisible(false);
                            //document.getElementById('btnSaveNew').style.display = 'block';
                            //document.getElementById('btnSaveRecords').style.display = 'block';

                            //SaveTDS();

                            gridTDS.UpdateEdit();
                            chkAccount = 0;
                        }
                        else {
                            chkAccount = 1;
                            jAlert('Receipt amount can not be less than payment amount');

                        }
                    }
                }
                else {
                    // chkAccount = 0;
                    jAlert('Cannot Save. You must enter atleast one Record to save this entry. 12');
                }
            }
            else {
                //chkAccount = 0;
                jAlert('Cannot Save. You must enter atleast one Record to save this entry.');
            }
        }


        function SaveTDS() {
            //debugger;
            myTableArray = [];

            $("#tbltdsDetails tr").each(function () {
                var arrayOfThisRow = [];
                var tableData = $(this).find('td');
                if (tableData.length > 0) {
                    //debugger;
                    var id = $(tableData[0]).html().trim();
                    var Myobj = {

                        DETID: $(tableData[0]).html().trim(),
                        VendorId: $(tableData[0]).html().trim(),
                        MainAccountID: $(tableData[0]).html().trim(),
                        PartyID: $(tableData[0]).html().trim(),
                        TDSTCS_Code: $(tableData[0]).html().trim(),
                        PaymentDate: $(tableData[0]).html().trim(),
                        Total_Tax: $(tableData[0]).html().trim(),
                        Tax_Amount: $(tableData[0]).html().trim(),
                        Surcharge: $(tableData[0]).html().trim(),
                        EduCess: $(tableData[0]).html().trim()

                    };


                    myTableArray.push(Myobj);
                }
            });

            var NewJSONstr = JSON.stringify(myTableArray);

            $("#hdnTDSData").val(NewJSONstr);

            console.log(myTableArray);
        }




        function GvCBSearch_EndCallBack() {
            if (cGvCBSearch.cpDelete != null) {
                jAlert(cGvCBSearch.cpDelete);
                cGvCBSearch.cpDelete = null;
            }
        }
        function CustomButtonClick(s, e) {
            if (e.buttonID == 'CustomBtnEdit') {
                s.GetRowValues(e.visibleIndex, 'ValueDate', function (value) {
                    if (value != null)
                    { jAlert("Voucher is Reconciled.Cannot Edit"); }
                    else {
                        $('#<%=hdnEditClick.ClientID %>').val('T'); //Edit
                        $('#<%=hdnJNMode.ClientID %>').val('1');//Edit
                        VisibleIndexE = e.visibleIndex;
                        $('#<%= lblHeading.ClientID %>').text("Modify Cash/Bank Voucher");
                        document.getElementById('DivEntry').style.display = 'block';
                        document.getElementById('divExportto').style.display = 'none';
                        document.getElementById('divAddButton').style.display = 'none';
                        document.getElementById('gridFilter').style.display = 'none';
                        document.getElementById('DivEdit').style.display = 'none';
                        btncross.style.display = "block";
                        $('#<%=hdn_Mode.ClientID %>').val('Edit');
                        //gridTDS.PerformCallback("Edit~" + VisibleIndexE);
                        cASPxCallbackGeneral.PerformCallback("Edit~" + VisibleIndexE);
                        LoadingPanel.Show();
                        chkAccount = 1;
                        document.getElementById('divNumberingScheme').style.display = 'none';
                        document.getElementById('divEnterBranch').style.display = 'Block';
                    }
                });
            }
            else if (e.buttonID == 'CustomBtnView') {
                $('#<%=hdnEditClick.ClientID %>').val('T'); //Edit
                $('#<%=hdnJNMode.ClientID %>').val('1');//Edit
                VisibleIndexE = e.visibleIndex;
                $('#<%= lblHeading.ClientID %>').text("View Cash/Bank Voucher");
                document.getElementById('DivEntry').style.display = 'block';
                document.getElementById('divExportto').style.display = 'none';
                document.getElementById('divAddButton').style.display = 'none';
                document.getElementById('gridFilter').style.display = 'none';
                document.getElementById('DivEdit').style.display = 'none';
                btncross.style.display = "block";
                $('#<%=hdn_Mode.ClientID %>').val('View');
                //gridTDS.PerformCallback("View~" + VisibleIndexE);
                cASPxCallbackGeneral.PerformCallback("View~" + VisibleIndexE);
                LoadingPanel.Show();
                chkAccount = 1;
                document.getElementById('divNumberingScheme').style.display = 'none';
            }
            else if (e.buttonID == 'CustomBtnDelete') {
                jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                    if (r == true) {
                        VisibleIndexE = e.visibleIndex;
                        cGvCBSearch.PerformCallback("Delete~" + VisibleIndexE);
                    }
                    else {
                        return false;
                    }
                });
            }
            else if (e.buttonID == 'CustomBtnPrint') {
                var keyValueindex = s.GetRowKey(e.visibleIndex);
                onPrintJv(keyValueindex);
            }
    }

    //function onPrintJv(id) {

    //    RecPayId = id;
    //    cDocumentsPopup.Show();
    //    cCmbDesignName.SetSelectedIndex(0);
    //    cSelectPanel.PerformCallback('Bindalldesignes');
    //    $('#btnOK').focus();
    //}

    //function PerformCallToGridBind() {
    //    cSelectPanel.PerformCallback('Bindsingledesign');
    //    cDocumentsPopup.Hide();
    //    return false;
    //}

    //function cSelectPanelEndCall(s, e) {

    //    if (cSelectPanel.cpSuccess != "") {
    //        var TotDocument = cSelectPanel.cpSuccess.split(',');
    //        var reportName = cCmbDesignName.GetValue();
    //        var module = 'CBVUCHR';
    //        window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + RecPayId, '_blank')
    //    }
    //    if (cSelectPanel.cpSuccess == "") {
    //        cCmbDesignName.SetSelectedIndex(0);
    //    }
    //}

    ////##### coded by Samrat Roy - 14/04/2017 - ref IssueLog(Voucher - 110) 
    ////This method is for disable all the attributes.
    function viewOnly() {
        if ($('#<%=hdn_Mode.ClientID %>').val().toUpperCase() == 'VIEW') {
            $('#DivEntry').find('input, textarea, button, select').attr('disabled', 'disabled');
            gridTDS.SetEnabled(false);
            cddlCashBank.SetEnabled(false);
            // cComboType.SetEnabled(false);
            $("#rbtnType").attr("disabled", "disabled");
            cdtTDate.SetEnabled(false);
            cCmbCurrency.SetEnabled(false);
            ctxtRate.SetEnabled(false);
            cComboInstrumentTypee.SetEnabled(false);
            ctxtInstNobth.SetEnabled(false);
            cInstDate.SetEnabled(false);
            ctxtReceivedFrom.SetEnabled(false);
            ctxtPaidTo.SetEnabled(false);
            //cbtnSaveNew.SetVisible(false);
            //cbtnSaveRecords.SetVisible(false);
            //document.getElementById('btnSaveNew').style.display = 'block';
            //document.getElementById('btnSaveRecords').style.display = 'block';
        }
        LoadingPanel.Hide();
    }
<%--function OnGetRowValuesOnDelete(values) {
    var ValueDate = new Date(values[0]);
    var monthnumber = ValueDate.getMonth();
    var monthday = ValueDate.getDate();
    var year = ValueDate.getYear();
    var ValueDateNumeric = new Date(year, monthnumber, monthday).getTime();
    var TransactionDate = new Date(values[1]);
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
    if (ValueDateNumeric == -2209008600000) {
        cDeleteMsgPopUp.Show();
    }
    else {
        jAlert('Entry Already Been Tagged.Please Remove Tag To Delete Entry!!!.');
    }
}--%>
        function RecieveTextChange(s, e) {
            var RecieveValue = (gridTDS.GetEditor('btnRecieve').GetValue() != null) ? parseFloat(gridTDS.GetEditor('btnRecieve').GetValue()) : "0";
            var PaymentValue = (gridTDS.GetEditor('btnPayment').GetValue() != null) ? gridTDS.GetEditor('btnPayment').GetValue() : "0";
            if (RecieveValue > 0) {
                recalculateCredit(gridTDS.GetEditor('btnPayment').GetValue());
                gridTDS.GetEditor('btnPayment').SetValue("0");
            }
        }
        <%--function BindCashBankAccountList() {

    var CurrentSegment = document.getElementById('hdn_CurrentSegment').value;
    var Mode = document.getElementById("hdn_Mode").value;
    var strPutSegment = " and (MainAccount_ExchangeSegment=" + CurrentSegment + " or MainAccount_ExchangeSegment=0)";
    var strQuery_Table = "(Select MainAccount_AccountCode+\'-\'+MainAccount_Name+\' [ \'+MainAccount_BankAcNumber+\' ] \'+MainAccount_BankCashType as IntegrateMainAccount,MainAccount_AccountCode+\'~\'+MainAccount_BankCashType+\'~CASHBANK\' as MainAccount_AccountCode,MainAccount_Name,MainAccount_BankAcNumber from Master_MainAccount where (MainAccount_BankCashType=\'Bank\' or MainAccount_BankCashType=\'Cash\') and (MainAccount_BankCompany=\'" + '<%=Session["LastCompany"] %>' + "\' Or IsNull(MainAccount_BankCompany,'')='') and (MainAccount_branchId=\'" + '<%=Session["userbranchID"] %>' + "\' Or IsNull(MainAccount_branchId,'')='0')" + strPutSegment + ") as t1";
    var strQuery_FieldName = " * ";
    var strQuery_WhereClause = "MainAccount_AccountCode like (\'%RequestLetter%\') or MainAccount_Name like (\'%RequestLetter%\') or MainAccount_BankAcNumber like (\'%RequestLetter%\')";
    var strQuery_OrderBy = '';
    var strQuery_GroupBy = '';
    var CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);
    var lBox = $('select[id$=lstCashItems]');
    var lstCashItems = [];
    //Customer or Lead radio button is clicked kaushik 21-11-2016
    lBox.empty();
    $.ajax({
        type: "POST",
        url: 'CashBankEntry.aspx/GetCashBankAccountList',
        data: "{CombinedQuery:\"" + CombinedQuery + "\",BranchID:0}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var list = msg.d;
            if (list.length > 0) {

                for (var i = 0; i < list.length; i++) {
                    var id = '';
                    var name = '';
                    id = list[i].split('|')[1];
                    name = list[i].split('|')[0];
                    lstCashItems.push('<option value="' +
                    id + '">' + name
                    + '</option>');
                }
                $(lBox).append(lstCashItems.join(''));
                ListAccountBind();
                $('#lstCashItems').trigger("chosen:updated");
                $('#lstCashItems').prop('disabled', false).trigger("chosen:updated");

            }
            else {
                lBox.empty();
                //$(lBox).append(listItems.join(''));
                //ListBind();

                $('#lstCashItems').trigger("chosen:updated");
                $('#lstCashItems').prop('disabled', true).trigger("chosen:updated");

            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            jAlert(textStatus);
        }
    });
}--%>

        <%-- function CashBankTagged(key) {
            $('#<%=hdnEditClick.ClientID %>').val('T'); //Edit
            $('#<%=hdnJNMode.ClientID %>').val('1');//Edit            
            $('#<%= lblHeading.ClientID %>').text("View Cash/Bank Voucher");
            document.getElementById('DivEntry').style.display = 'block';
            document.getElementById('divExportto').style.display = 'none';
            document.getElementById('divAddButton').style.display = 'none';
            document.getElementById('gridFilter').style.display = 'none';
            document.getElementById('DivEdit').style.display = 'none';        
            $('#<%=hdn_Mode.ClientID %>').val('View');
            gridTDS.PerformCallback("ViewTagged~" + key);
            LoadingPanel.Show();
            chkAccount = 1;
            document.getElementById('divNumberingScheme').style.display = 'none';

            }--%>



        <%--function BindCashBankAccountListByBranch(BranchID) {

    var CurrentSegment = document.getElementById('hdn_CurrentSegment').value;
    var Mode = document.getElementById("hdn_Mode").value;
    var strPutSegment = " and (MainAccount_ExchangeSegment=" + CurrentSegment + " or MainAccount_ExchangeSegment=0)";
    var strQuery_Table = "(Select MainAccount_AccountCode+\'-\'+MainAccount_Name+\' [ \'+MainAccount_BankAcNumber+\' ] \'+MainAccount_BankCashType as IntegrateMainAccount,MainAccount_ReferenceID,MainAccount_AccountCode+\'~\'+MainAccount_BankCashType+\'~CASHBANK\' as MainAccount_AccountCode,MainAccount_Name,MainAccount_BankAcNumber from Master_MainAccount where (MainAccount_BankCashType=\'Bank\' or MainAccount_BankCashType=\'Cash\') and (MainAccount_BankCompany=\'" + '<%=Session["LastCompany"] %>' + "\' Or IsNull(MainAccount_BankCompany,'')='') and (MainAccount_branchId=\'" + BranchID + "\' Or IsNull(MainAccount_branchId,'')='0')" + strPutSegment + ") as t1";
    var strQuery_FieldName = " * ";
    var strQuery_WhereClause = "MainAccount_AccountCode like (\'%RequestLetter%\') or MainAccount_Name like (\'%RequestLetter%\') or MainAccount_BankAcNumber like (\'%RequestLetter%\')";
    var strQuery_OrderBy = '';
    var strQuery_GroupBy = '';
    var CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);
    var lBox = $('select[id$=lstCashItems]');
    var lstCashItems = [];
    //Customer or Lead radio button is clicked kaushik 21-11-2016
    lBox.empty();
    $.ajax({
        type: "POST",
        url: 'CashBankEntry.aspx/GetCashBankAccountList',
        data: "{CombinedQuery:\"" + CombinedQuery + "\",BranchID:" + BranchID + "}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var list = msg.d;
            if (list.length > 0) {
                for (var i = 0; i < list.length; i++) {

                    var id = '';
                    var name = '';
                    id = list[i].split('|')[1];
                    name = list[i].split('|')[0];

                    lstCashItems.push('<option value="' +
                    id + '">' + name
                    + '</option>');
                }
                $(lBox).append(lstCashItems.join(''));
                ListAccountBind();
                $('#lstCashItems').trigger("chosen:updated");
                $('#lstCashItems').prop('disabled', false).trigger("chosen:updated");

            }
            else {
                lBox.empty();
                $('#lstCashItems').trigger("chosen:updated");
                $('#lstCashItems').prop('disabled', true).trigger("chosen:updated");
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            jAlert(textStatus);
        }
    });
}--%>
        //function ListAccountBind() {
        //    var config = {
        //        '.chsn': {},
        //        '.chsn-deselect': { allow_single_deselect: true },
        //        '.chsn-no-single': { disable_search_threshold: 10 },
        //        '.chsn-no-results': { no_results_text: 'Oops, nothing found!' },
        //        '.chsn-width': { width: "100%" }
        //    }
        //    for (var selector in config) {
        //        $(selector).chosen(config[selector]);
        //    }
        //}
    </script>
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


            if (name == "tab0") {
                // gridLookup.Focus();
            }
            if (name == "tab1") {

            }
        }
        function getParameterByName(name, url) {
            if (!url) url = window.location.href;
            name = name.replace(/[\[\]]/g, "\\$&");
            var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
                results = regex.exec(url);
            if (!results) return null;
            if (!results[2]) return '';
            return decodeURIComponent(results[2].replace(/\+/g, " "));
        }
    </script>
    <%--Added By : Samrat Roy -- New Billing/Shipping Section End--%>

    <script>

        $(function () {
            $("#chk_reversemechenism").prop("disabled", false);
            $("#chk_reversemechenism").on("change", function () {
                if ($("#chk_reversemechenism").prop("checked") == true) {

                }
                else {

                }
            })
        });

        //Function for Date wise filteration
        //function updateGridByDate() {

        //    if (cFormDate.GetDate() == null) {
        //        jAlert('Please select from date.', 'Alert', function () { cFormDate.Focus(); });
        //    }
        //    else if (ctoDate.GetDate() == null) {
        //        jAlert('Please select to date.', 'Alert', function () { ctoDate.Focus(); });
        //    }
        //    else if (ccmbBranchfilter.GetValue() == null) {
        //        jAlert('Please select Branch.', 'Alert', function () { ccmbBranchfilter.Focus(); });
        //    }
        //    else {
        //        localStorage.setItem("FromDateCashBank", cFormDate.GetDate().format('yyyy-MM-dd'));
        //        localStorage.setItem("ToDateCashBank", ctoDate.GetDate().format('yyyy-MM-dd'));
        //        localStorage.setItem("BranchCashBank", ccmbBranchfilter.GetValue());
        //        cGvCBSearch.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue());
        //    }
        //}

        function ProjectCodeSelected(s, e) {
            //debugger;
            if (clookupPopup_ProjectCode.GetGridView().GetFocusedRowIndex() == -1) {
                cProjectCodePopup.Hide();

                return;
            }
            var ProjectInlineLookUpData = clookupPopup_ProjectCode.GetGridView().GetRowKey(clookupPopup_ProjectCode.GetGridView().GetFocusedRowIndex());
            var ProjectInlinedata = ProjectInlineLookUpData.split('~')[0];
            gridTDS.batchEditApi.StartEdit(globalRowIndexTDS,5);
            var ProjectCode = clookupPopup_ProjectCode.GetValue();
            cProjectCodePopup.Hide();

            gridTDS.GetEditor("Project_Code").SetText(ProjectCode);
            gridTDS.GetEditor("ProjectId").SetText(ProjectInlinedata);

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

    </script>

    
    <script>
        $(document).ready(function () {

            $('.HtmlGrid > tbody > tr').click(function () {
                $('.HtmlGrid>tbody>tr').removeClass('hovered');
                $(this).addClass('hovered');
            });
        });
    </script>
    <script>
        //Hierarchy Start Tanmoy
        function Project_LostFocus() {
            //if (InsgridBatch.GetVisibleRowsOnPage() == 1) {
            //    InsgridBatch.batchEditApi.StartEdit(-1, 2);
            //}
            var projID = clookup_Project.GetValue();
            if (projID == null || projID == "") {
                $("#ddlHierarchy").val(0);
            }
        }

        function ProjectValueChange(s, e) {
            //debugger;
            var projID = clookup_Project.GetValue();

            $.ajax({
                type: "POST",
                url: 'CashBankEntryTDS.aspx/getHierarchyID',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ ProjID: projID }),
                success: function (msg) {
                    var data = msg.d;
                    $("#ddlHierarchy").val(data);
                }
            });
            clookup_CashFund.gridView.Refresh();
        }
        //Hierarchy End Tanmoy
    </script>
    <link href="CSS/CashBankEntryTDS.css" rel="stylesheet" />

    <style>
        /*Rev 1.0*/

        select
        {
            height: 30px !important;
            border-radius: 4px;
            -webkit-appearance: none;
            position: relative;
            z-index: 1;
            background-color: transparent;
            padding-left: 10px !important;
            padding-right: 22px !important;
        }

        .dxeButtonEditSys.dxeButtonEdit_PlasticBlue , .dxeTextBox_PlasticBlue
        {
            height: 30px;
            border-radius: 4px;
        }

        .dxeButtonEditButton_PlasticBlue
        {
            background: #094e8c !important;
            border-radius: 4px !important;
            padding: 0 4px !important;
        }

        .calendar-icon {
            position: absolute;
            bottom: 7px;
            right: 20px;
            z-index: 0;
            cursor: pointer;
        }

        .calendar-icon-2 {
            position: absolute;
            bottom: 7px;
            right: 4px;
            z-index: 0;
            cursor: pointer;
        }

        #ASPxFromDate , #ASPxToDate , #ASPxASondate , #ASPxAsOnDate , #FormDate , #toDate , #dtTDate , #InstDate
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        .dxeDisabled_PlasticBlue
        {
            z-index: 0 !important;
        }

        #ASPxFromDate_B-1 , #ASPxToDate_B-1 , #ASPxASondate_B-1 , #ASPxAsOnDate_B-1 , #FormDate_B-1 , #toDate_B-1 , #dtTDate_B-1 , #InstDate_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #ASPxFromDate_B-1 #ASPxFromDate_B-1Img , #ASPxToDate_B-1 #ASPxToDate_B-1Img , #ASPxASondate_B-1 #ASPxASondate_B-1Img , #ASPxAsOnDate_B-1 #ASPxAsOnDate_B-1Img ,
        #FormDate_B-1 #FormDate_B-1Img , #toDate_B-1 #toDate_B-1Img , #dtTDate_B-1 #dtTDate_B-1Img , #InstDate_B-1 #InstDate_B-1Img
        {
            display: none;
        }

        .dxtcLite_PlasticBlue > .dxtc-stripContainer .dxtc-activeTab, .dxgvFooter_PlasticBlue
        {
            background: #1b5ea4 !important;
        }

        .simple-select::after {
            /*content: '<';*/
            content: url(../../../assests/images/left-arw.png);
            position: absolute;
            top: 6px;
            right: -2px;
            font-size: 16px;
            transform: rotate(269deg);
            font-weight: 500;
            background: #094e8c;
            color: #fff;
            height: 18px;
            display: block;
            width: 26px;
            /* padding: 10px 0; */
            border-radius: 4px;
            text-align: center;
            line-height: 18px;
            z-index: 0;
        }
        .simple-select {
            position: relative;
                z-index: 0;
        }
        .simple-select:disabled::after
        {
            background: #1111113b;
        }
        select.btn
        {
            padding-right: 10px !important;
        }

        .panel-group .panel
        {
            box-shadow: 1px 1px 8px #1111113b;
            border-radius: 8px;
        }

        .dxpLite_PlasticBlue .dxp-current
        {
            background-color: #1b5ea4;
            padding: 3px 5px;
            border-radius: 2px;
        }

        #accordion {
            margin-bottom: 20px;
            margin-top: 10px;
        }

        .dxgvHeader_PlasticBlue {
    background: #1b5ea4 !important;
    color: #fff !important;
}
        #ShowGrid
        {
            margin-top: 10px;
        }

        .pt-25{
                padding-top: 25px !important;
        }

        .styled-checkbox {
        position: absolute;
        opacity: 0;
        z-index: 1;
    }

        .styled-checkbox + label {
            position: relative;
            /*cursor: pointer;*/
            padding: 0;
            margin-bottom: 0 !important;
        }

            .styled-checkbox + label:before {
                content: "";
                margin-right: 6px;
                display: inline-block;
                vertical-align: text-top;
                width: 16px;
                height: 16px;
                /*background: #d7d7d7;*/
                margin-top: 2px;
                border-radius: 2px;
                border: 1px solid #c5c5c5;
            }

        .styled-checkbox:hover + label:before {
            background: #094e8c;
        }


        .styled-checkbox:checked + label:before {
            background: #094e8c;
        }

        .styled-checkbox:disabled + label {
            color: #b8b8b8;
            cursor: auto;
        }

            .styled-checkbox:disabled + label:before {
                box-shadow: none;
                background: #ddd;
            }

        .styled-checkbox:checked + label:after {
            content: "";
            position: absolute;
            left: 3px;
            top: 9px;
            background: white;
            width: 2px;
            height: 2px;
            box-shadow: 2px 0 0 white, 4px 0 0 white, 4px -2px 0 white, 4px -4px 0 white, 4px -6px 0 white, 4px -8px 0 white;
            transform: rotate(45deg);
        }

        .dxgvEditFormDisplayRow_PlasticBlue td.dxgv, .dxgvDataRow_PlasticBlue td.dxgv, .dxgvDataRowAlt_PlasticBlue td.dxgv, .dxgvSelectedRow_PlasticBlue td.dxgv, .dxgvFocusedRow_PlasticBlue td.dxgv
        {
            padding: 6px 6px 6px !important;
        }

        #lookupCardBank_DDD_PW-1
        {
                left: -182px !important;
        }
        .plhead a>i
        {
                top: 9px;
        }

        .clsTo
        {
            display: flex;
    align-items: flex-start;
        }

        input[type="radio"], input[type="checkbox"]
        {
            margin-right: 5px;
        }
        .dxeCalendarDay_PlasticBlue
        {
                padding: 6px 6px;
        }

        .modal-dialog
        {
            width: 50%;
        }

        .modal-header
        {
            padding: 8px 4px 8px 10px;
            background: #094e8c !important;
        }

        .TableMain100 #ShowGrid , .TableMain100 #ShowGridList , .TableMain100 #ShowGridRet , .TableMain100 #ShowGridLocationwiseStockStatus ,
        #GvCBSearch
        {
            max-width: 98% !important;
        }

        /*div.dxtcSys > .dxtc-content > div, div.dxtcSys > .dxtc-content > div > div
        {
            width: 95% !important;
        }*/

        .btn-info
        {
                background-color: #1da8d1 !important;
                background-image: none;
        }

        .for-cust-icon {
            position: relative;
            z-index: 1;
        }

        .dxeDisabled_PlasticBlue, .aspNetDisabled
        {
            background: #f3f3f3 !important;
        }

        .dxeButtonDisabled_PlasticBlue
        {
            background: #b5b5b5 !important;
            border-color: #b5b5b5 !important;
        }

        #ddlValTech
        {
            width: 100% !important;
            margin-bottom: 0 !important;
        }

        .dis-flex
        {
            display: flex;
            align-items: baseline;
        }

        input + label
        {
            line-height: 1;
                margin-top: 3px;
        }

        .dxtlHeader_PlasticBlue
        {
            background: #094e8c !important;
        }

        .dxeBase_PlasticBlue .dxichCellSys
        {
            padding-top: 2px !important;
        }

        .pBackDiv
        {
            border-radius: 10px;
            box-shadow: 1px 1px 10px #1111112e;
        }
        .HeaderStyle th
        {
            padding: 5px;
        }

        .for-cust-icon {
            position: relative;
            z-index: 1;
        }

        .dxtcLite_PlasticBlue.dxtc-top > .dxtc-stripContainer
        {
            padding-top: 15px;
        }

        .pt-2
        {
            padding-top: 5px;
        }
        .pt-10
        {
            padding-top: 10px;
        }

        .pt-15
        {
            padding-top: 15px;
        }

        .pb-10
        {
            padding-bottom: 10px;
        }

        .pTop10 {
    padding-top: 20px;
}
        .custom-padd
        {
            padding-top: 4px;
    padding-bottom: 10px;
        }

        input + label
        {
                margin-right: 10px;
        }

        .btn
        {
            margin-bottom: 0;
        }

        .pl-10
        {
            padding-left: 10px;
        }

        /*.col-md-3>label, .col-md-3>span
        {
            margin-top: 0 !important;
        }*/

        .devCheck
        {
            margin-top: 5px;
        }

        .mtc-5
        {
            margin-top: 5px;
        }

        .mtc-10
        {
            margin-top: 10px;
        }

        select.btn
        {
           position: relative;
           z-index: 0;
        }

        select
        {
            margin-bottom: 0;
        }

        .form-control
        {
            background-color: transparent;
        }

        select.btn-radius {
    padding: 4px 8px 6px 11px !important;
}
        .mt-30{
            margin-top: 30px;
        }

        .makeFullscreen
        {
                z-index: 0;
        }

        .panel-fullscreen
        {
                z-index: 99 !important;
        }
        #massrecdt
        {
            width: 100%;
        }

        .mb-10{
            margin-bottom: 10px;
        }

        .crossBtn
        {
            top: 25px;
                right: 25px;
        }

        input[type="text"], input[type="password"], textarea
        {
                margin-bottom: 0;
        }
        /*Rev end 1.0*/
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hfVSFileName" runat="server" />

    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main clearfix">
        <div class="panel-heading">
        <div class="panel-title clearfix">
            <h3 class="pull-left">
                <asp:Label ID="lblHeading" runat="server" Text="Cash/Bank Voucher Add"></asp:Label>

            </h3>
            <div id="pageheaderContent" class="pull-right wrapHolder reverse content horizontal-images" style="display: none;">
                <ul>
                    <li>
                        <div class="lblHolder">
                            <table>
                                <tbody>
                                    <tr>
                                        <td>Current Balance </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div style="width: 100%;">
                                                <b style="text-align: left" id="B_ImgSymbolBankBal" runat="server"></b>
                                                <b style="text-align: center" id="B_BankBalance" runat="server"></b>
                                            </div>

                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </li>


                </ul>
            </div>
            <div id="btncross" runat="server" class="crossBtn" style="margin-left: 50px;"><a href="javascript:void(0);" onclick="ReloadPage()"><i class="fa fa-times"></i></a></div>
        </div>
    </div>
        <div class="form_main  clearfix">

        <div class="rgth pull-left full">
            <div id="DivEntry">
                <dxe:PanelContent runat="server">
                    <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ClientInstanceName="page" Width="100%">
                        <TabPages>
                            <dxe:TabPage Name="General" Text="General">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                        <dxe:ASPxCallbackPanel runat="server" ID="ASPxCallbackGeneral" ClientInstanceName="cASPxCallbackGeneral">
                                            <PanelCollection>
                                                <dxe:PanelContent runat="server">
                                                    <div id="divChangable" runat="server" style=" padding: 5px 0 5px 0; margin-bottom: 0px; border-radius: 4px; margin-bottom: 15px;">
                                                        <div class="row">
                                                            <div class="col-md-12" style="padding: 0 5px;">
                                                                <div class="col-md-2">
                                                                    <label for="exampleInputName2" style="margin-top: 8px">
                                                                        Voucher Type <b id="bTypeText" runat="server" style="width: 20%; display: none; font-size: 12px"></b>
                                                                    </label>
                                                                    <%--Rev 1.0: "simple-select" class add --%>
                                                                    <div class="simple-select">
                                                                        <asp:DropDownList ID="rbtnType" runat="server" Enabled="false" Width="100%" onchange="rbtnType_SelectedIndexChanged()">
                                                                            <asp:ListItem Text="Payment" Value="P" />

                                                                        </asp:DropDownList>

                                                                    </div>
                                                                </div>
                                                                <div class="col-md-2" id="divNumberingScheme" runat="server">
                                                                    <label style="margin-top: 8px">Numbering Scheme</label>
                                                                    <div>
                                                                        <dxe:ASPxComboBox ID="CmbScheme" EnableIncrementalFiltering="True" ClientInstanceName="cCmbScheme"
                                                                            SelectedIndex="0" EnableCallbackMode="false"
                                                                            TextField="SchemaName" ValueField="ID"
                                                                            runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True" OnCallback="CmbScheme_Callback">
                                                                            <ClientSideEvents ValueChanged="function(s,e){CmbScheme_ValueChange()}" EndCallback="CmbSchemeEndCallback"></ClientSideEvents>
                                                                        </dxe:ASPxComboBox>
                                                                        <%--  GotFocus="NumberingScheme_GotFocus"--%>
                                                                        <span id="MandatoryNumberingScheme" class="iconNumberScheme pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-2 lblmTop8" style="display: none" id="divEnterBranch" runat="server">

                                                                    <label>Unit <span style="color: red">*</span></label>
                                                                    <div>
                                                                        <asp:DropDownList ID="ddlEnterBranch" runat="server" DataSourceID="dsBranch"
                                                                            DataTextField="BANKBRANCH_NAME" DataValueField="BANKBRANCH_ID" Width="100%">
                                                                        </asp:DropDownList>

                                                                    </div>
                                                                </div>
                                                                <div class="col-md-3">
                                                                    <label style="margin-top: 8px">Document No.</label>
                                                                    <div>
                                                                        <asp:TextBox ID="txtVoucherNo" runat="server" Width="100%" MaxLength="30" onchange="txtBillNo_TextChanged()">                             
                                                                        </asp:TextBox>
                                                                        <span id="MandatoryBillNo" class="voucherno  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                                                    </div>
                                                                </div>

                                                                <div class="col-md-2">
                                                                    <label style="margin-top: 8px">Posting Date  </label>
                                                                    <div>
                                                                        <dxe:ASPxDateEdit ID="dtTDate" runat="server" ClientInstanceName="cdtTDate" EditFormat="Custom"
                                                                            Font-Size="12px" UseMaskBehavior="True" Width="100%" EditFormatString="dd-MM-yyyy" CssClass="pull-left">
                                                                            <ButtonStyle Width="13px"></ButtonStyle>
                                                                            <ClientSideEvents DateChanged="function(s,e){TDateChange();}" GotFocus="function(s,e){cdtTDate.ShowDropDown();}" LostFocus="function(s, e) { SetLostFocusonDemand(e)}"></ClientSideEvents>
                                                                        </dxe:ASPxDateEdit>
                                                                        <%--Rev 1.0--%>
                                                                        <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                                                                        <%--Rev end 1.0--%>
                                                                    </div>
                                                                </div>

                                                                <div class="col-md-2 lblmTop8">
                                                                    <label>For Unit <span style="color: red">*</span></label>
                                                                    <%--Rev 1.0: "simple-select" class add --%>
                                                                    <div class="simple-select">
                                                                        <asp:DropDownList ID="ddlBranch" runat="server" onchange="ddlBranch_SelectedIndexChanged()"
                                                                            DataTextField="BANKBRANCH_NAME" DataValueField="BANKBRANCH_ID" Width="100%">
                                                                        </asp:DropDownList>
                                                                        <span id="MandatoryBranch" class="iconBranch pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>

                                                                    </div>
                                                                </div>
                                                                <div class="clear"></div>
                                                                <div style="border-top: 1px solid #ccc; margin-top: 8px; height: 10px;">
                                                                </div>
                                                                <div class="clear"></div>
                                                                <div class="col-md-2" id="tdCashBankLabel">
                                                                    <label>Cash/Bank</label>
                                                                    <div>
                                                                        <dxe:ASPxComboBox ID="ddlCashBank" runat="server" ClientInstanceName="cddlCashBank" Width="100%" OnCallback="ddlCashBank_Callback">
                                                                            <ClientSideEvents EndCallback="CashBank_EndCallback" SelectedIndexChanged="CashBank_SelectedIndexChanged" GotFocus="CashBank_GotFocus" KeyDown="CashBank_GotFocus" />
                                                                        </dxe:ASPxComboBox>
                                                                        <span id="MandatoryCashBank" class="iconCashBank pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>

                                                                    </div>
                                                                </div>
                                                                <div class="col-md-1">
                                                                    <label>Currency  </label>
                                                                    <%--Rev 1.0: "simple-select" class add --%>
                                                                    <div class="simple-select">
                                                                        <dxe:ASPxComboBox ID="CmbCurrency" EnableIncrementalFiltering="True" ClientInstanceName="cCmbCurrency"
                                                                            TextField="Currency_AlphaCode" ValueField="Currency_ID" SelectedIndex="0" Native="true"
                                                                            runat="server" ValueType="System.String" EnableSynchronization="True" Width="100%" CssClass="pull-left">
                                                                            <ClientSideEvents ValueChanged="function(s,e){Currency_Rate()}" GotFocus="function(s,e){cCmbCurrency.ShowDropDown();}"></ClientSideEvents>
                                                                        </dxe:ASPxComboBox>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-1">
                                                                    <label>Rate  </label>
                                                                    <div>
                                                                        <dxe:ASPxTextBox runat="server" ID="txtRate" HorizontalAlign="Right" ClientInstanceName="ctxtRate" Width="100%" CssClass="pull-left">
                                                                            <MaskSettings Mask="<0..9999>.<0..99999>" IncludeLiterals="DecimalSymbol" />
                                                                        </dxe:ASPxTextBox>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-2">
                                                                    <label style="">Instrument Type</label>
                                                                    <%--Rev 1.0: "simple-select" class add --%>
                                                                    <div class="simple-select">
                                                                        <dxe:ASPxComboBox ID="cmbInstrumentType" runat="server" ClientInstanceName="cComboInstrumentTypee" Font-Size="12px"
                                                                            ValueType="System.String" Width="100%" EnableIncrementalFiltering="True" Native="true">
                                                                            <Items>
                                                                                <dxe:ListEditItem Text="Cheque" Value="C" Selected />
                                                                                <dxe:ListEditItem Text="Draft" Value="D" />
                                                                                <dxe:ListEditItem Text="E.Transfer" Value="E" />
                                                                                <dxe:ListEditItem Text="Cash" Value="CH" />
                                                                            </Items>
                                                                            <ClientSideEvents SelectedIndexChanged="InstrumentTypeSelectedIndexChanged" GotFocus="function(s,e){cComboInstrumentTypee.ShowDropDown();}" />
                                                                        </dxe:ASPxComboBox>
                                                                        <span id="MandatoryInstrumentType" class="iconInstrumentType pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-2" id="divInstrumentNo" style="">
                                                                    <label id="" style="">Instrument No</label>
                                                                    <div id="">
                                                                        <dxe:ASPxTextBox runat="server" ID="txtInstNobth" ClientInstanceName="ctxtInstNobth" Width="100%" MaxLength="30" CssClass="pull-left">
                                                                        </dxe:ASPxTextBox>
                                                                        <span id="MandatoryInstNo" class="iconInstNo pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-2" id="tdIDateDiv" style="">
                                                                    <label id="tdIDateLable" style="">Instrument Date</label>
                                                                    <div id="tdIDateValue" style="">
                                                                        <dxe:ASPxDateEdit ID="InstDate" runat="server" EditFormat="Custom" ClientInstanceName="cInstDate"
                                                                            UseMaskBehavior="True" Font-Size="12px" Width="100%" EditFormatString="dd-MM-yyyy">
                                                                            <%--<ClientSideEvents DateChanged="function(s,e){InstrumentDateChange();}" GotFocus="function(s,e){cInstDate.ShowDropDown();}"></ClientSideEvents>--%>
                                                                            <ClientSideEvents DateChanged="function(s,e){InstrumentDateChange();}"></ClientSideEvents>
                                                                            <ButtonStyle Width="13px">
                                                                            </ButtonStyle>
                                                                        </dxe:ASPxDateEdit>
                                                                        <%--Rev 1.0--%>
                                                                        <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                                                                        <%--Rev end 1.0--%>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-2" id="divDraweeBank">
                                                                    <label>Drawee Bank</label>
                                                                    <div>
                                                                        <asp:TextBox ID="txtDraweeBank" runat="server" Width="100%">                             
                                                                        </asp:TextBox>
                                                                    </div>
                                                                </div>
                                                                <div class="clear"></div>
                                                                <div class="col-md-3 lblmTop8" id="divReceivedfrom" style="display: none;">
                                                                    <label id="" style="">Received From</label>
                                                                    <div id="">
                                                                        <dxe:ASPxTextBox runat="server" ID="txtReceivedFrom" ClientInstanceName="ctxtReceivedFrom" Width="100%" MaxLength="30">
                                                                        </dxe:ASPxTextBox>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-3 lblmTop8" id="divPaidTo" style="">
                                                                    <label id="" style="">Paid To</label>
                                                                    <div id="">
                                                                        <dxe:ASPxTextBox runat="server" ID="txtPaidTo" ClientInstanceName="ctxtPaidTo" Width="100%" MaxLength="30">
                                                                        </dxe:ASPxTextBox>
                                                                        <span id="MandatoryPaidTo" class="iconInstNo pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-2 lblmTop8">
                                                                    <label>Contact </label>
                                                                    <div>
                                                                        <asp:TextBox ID="txtContact" runat="server" MaxLength="20"
                                                                            Width="100%"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-3 lblmTop8">
                                                                    <label>Narration </label>
                                                                    <div>
                                                                        <asp:TextBox ID="txtNarration" runat="server" MaxLength="500" onkeydown="checkTextAreaMaxLength(this,event,'500');"
                                                                            TextMode="MultiLine"
                                                                            Width="100%"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-2 lblmTop8" style="display: none;">
                                                                    <label>Reverse Charge </label>
                                                                    <div>
                                                                        <asp:CheckBox ID="chk_reversemechenism" runat="server"></asp:CheckBox>
                                                                    </div>
                                                                </div>

                                                                <div class="col-md-3">
                                                                    <label>Amounts are</label>
                                                                    <%--Rev 1.0: "simple-select" class add --%>
                                                                    <div class="simple-select">
                                                                        <asp:DropDownList ID="ddl_AmountAre" runat="server" DataSourceID="dsTaxType"
                                                                            DataTextField="taxGrp_Description" DataValueField="taxGrp_Id" Width="100%">
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                </div>
                                                                  
                                                                <div class="col-md-3">
                                                                    <label>Place Of Supply</label>
                                                                    <%--Rev 1.0: "simple-select" class add --%>
                                                                    <div class="simple-select">
                                                                        <asp:DropDownList ID="ddlSupplyState" runat="server" DataSourceID="dsSupplyState"
                                                                            DataTextField="state_name" DataValueField="state_id" Width="100%">
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-2">
                                                                    <label></label>
                                                                    <div style="padding-top: 12px;">
                                                                        <dxe:ASPxCheckBox ID="IsRcm" ClientInstanceName="IsRcm" Checked="false" Text="Reverse Mechanism" TextAlign="Left" runat="server">
                                                                            <ClientSideEvents CheckedChanged="RcmCheckChange" />
                                                                        </dxe:ASPxCheckBox>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-3" id="divprocode" runat="server">
                                                                    <label id="lblProject" runat="server">Project</label>
                                                                    <dxe:ASPxGridLookup ID="lookup_Project" runat="server" ClientInstanceName="clookup_Project" DataSourceID="EntityServerModeDatacashBankTDS"
                                                                        KeyFieldName="Proj_Id" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False">
                                                                        <Columns>
                                                                            <dxe:GridViewDataColumn FieldName="Proj_Code" Visible="true" VisibleIndex="1" Caption="Project Code" Settings-AutoFilterCondition="Contains" Width="200px">
                                                                            </dxe:GridViewDataColumn>
                                                                            <dxe:GridViewDataColumn FieldName="Proj_Name" Visible="true" VisibleIndex="2" Caption="Project Name" Settings-AutoFilterCondition="Contains" Width="200px">
                                                                            </dxe:GridViewDataColumn>
                                                                            <dxe:GridViewDataColumn FieldName="Customer" Visible="true" VisibleIndex="3" Caption="Customer" Settings-AutoFilterCondition="Contains" Width="200px">
                                                                            </dxe:GridViewDataColumn>
                                                                            <dxe:GridViewDataColumn FieldName="HIERARCHY_NAME" Visible="true" VisibleIndex="4" Caption="Hierarchy" Settings-AutoFilterCondition="Contains" Width="200px">
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
                                                                        <ClientSideEvents GotFocus="function(s,e){clookup_Project.ShowDropDown();}" LostFocus="Project_LostFocus" ValueChanged="ProjectValueChange" />
                                                                      
                                                                    </dxe:ASPxGridLookup>
                                                                    <dx:LinqServerModeDataSource ID="EntityServerModeDatacashBankTDS" runat="server" OnSelecting="EntityServerModeDatacashBankTDS_Selecting"
                                                                        ContextTypeName="ERPDataClassesDataContext" TableName="ProjectCodeBind" />
                                                                </div>
                                                               
                                                                      <div class="col-md-4" id="divhir" runat="server">
                                                                   <label>
                                                                    <dxe:ASPxLabel ID="lblHierarchy" runat="server" Text="Hierarchy">
                                                                    </dxe:ASPxLabel></label>
                                                                    <asp:DropDownList ID="ddlHierarchy" runat="server" Width="100%" Enabled="false">
                                                                    </asp:DropDownList>
                                                                </div>
                                                                  <%-- Rev Rajdip --%>
                                                                <div class="col-md-2 lblmTop8" id="dvrefcashbankreq" >
                                                                    <label id="Label1" runat="server">Ref. Cash Fund Req.</label>
                                                                     <dxe:ASPxGridLookup ID="lookup_CashFund"  runat="server" ClientInstanceName="clookup_CashFund" DataSourceID="EntityServerModeDataCashFund"
                                                                       KeyFieldName="Paymentreqhead_Id" Width="100%" TextFormatString="{1}" AutoGenerateColumns="False">
                                                        <Columns>
                                                             <dxe:GridViewDataColumn FieldName="Paymentreqhead_Id" Visible="true" VisibleIndex="1" Caption="Paymentreqhead Id" EditFormSettings-Visible="False" Settings-AutoFilterCondition="Contains" Width="0px">
                                                                </dxe:GridViewDataColumn>
                                                            <dxe:GridViewDataColumn FieldName="Paymentrequisition_Number" Visible="true" VisibleIndex="1" Caption="Payment Requisition No." Settings-AutoFilterCondition="Contains" Width="200px">
                                                              </dxe:GridViewDataColumn>
                                                          <dxe:GridViewDataColumn FieldName="Name" Visible="true" VisibleIndex="2" Caption="Name" Settings-AutoFilterCondition="Contains" Width="200px">
                                                          </dxe:GridViewDataColumn>
                                                          <%--<dxe:GridViewDataColumn FieldName="Customer" Visible="true" VisibleIndex="3" Caption="Customer" Settings-AutoFilterCondition="Contains" Width="200px">
                                                          </dxe:GridViewDataColumn>
                                                          <dxe:GridViewDataColumn FieldName="HIERARCHY_NAME" Visible="true" VisibleIndex="4" Caption="Hierarchy" EditFormSettings-Visible="False" Settings-AutoFilterCondition="Contains" Width="200px">
                                                          </dxe:GridViewDataColumn>--%>
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
                                                           <%--<ClientSideEvents GotFocus="function(s,e){clookup_Project.ShowDropDown();}" LostFocus="clookup_Project_LostFocus" ValueChanged="ProjectValueChange" />--%>

                                                          <%-- <ClearButton DisplayMode="Always">
                                                                                          </ClearButton>--%>
                                                           </dxe:ASPxGridLookup>
                                                           <dx:LinqServerModeDataSource ID="EntityServerModeDataCashFund" runat="server" OnSelecting="EntityServerModeDataCashFund_Selecting"
                                                               ContextTypeName="ERPDataClassesDataContext" TableName="Trans_Paymentreqhead" />
                                                                </div>
                                                                <%-- End Rev Rajdip --%>
                                                                <div class="clear"></div>
                                                                <div class="col-md-2" style="padding-top: 27px;" runat="server" id="dvTDSpop">
                                                                    <label class="mTop5">&nbsp;</label>
                                                                    <button class="btn btn-success btn-xs" type="button" data-toggle="modal" onclick="ShowTDSPopup();">Add TDS Details</button>
                                                                </div>
                                                                <div class="clear"></div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <dxe:ASPxGridView runat="server" OnBatchUpdate="gridTDS_BatchUpdate" KeyFieldName="CashReportID" ClientInstanceName="gridTDS" ID="gridTDS"
                                                        Width="100%" OnCellEditorInitialize="gridTDS_CellEditorInitialize" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false"
                                                        Settings-ShowFooter="false" OnCustomCallback="gridTDS_CustomCallback" OnDataBinding="gridTDS_DataBinding"
                                                        OnRowInserting="GridTDS_RowInserting" OnRowUpdating="GridTDS_RowUpdating" OnRowDeleting="GridTDS_RowDeleting"
                                                        SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="200"
                                                        CommandButtonInitialize="false" EnableCallBacks="true">

                                                        <SettingsPager Visible="false"></SettingsPager>
                                                        <Styles>
                                                            <Cell Wrap="False"></Cell>
                                                        </Styles>
                                                        <Columns>
                                                            <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="true" Width="50" VisibleIndex="0" Caption="Action">
                                                                <HeaderTemplate>
                                                                    Delete
                                                                </HeaderTemplate>
                                                                <CustomButtons>
                                                                    <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomDeleteTDS" Image-Url="/assests/images/crs.png">
                                                                    </dxe:GridViewCommandColumnCustomButton>
                                                                </CustomButtons>
                                                            </dxe:GridViewCommandColumn>

                                                            <dxe:GridViewDataButtonEditColumn FieldName="MainAccountTDS" Caption="Main Account" VisibleIndex="1">

                                                                <PropertiesButtonEdit>

                                                                    <ClientSideEvents ButtonClick="MainAccountButnClickTDS" KeyDown="MainAccountKeyDownTDS" />
                                                                    <Buttons>

                                                                        <dxe:EditButton Text="..." Width="20px">
                                                                        </dxe:EditButton>
                                                                    </Buttons>
                                                                </PropertiesButtonEdit>
                                                            </dxe:GridViewDataButtonEditColumn>

                                                            <%--                    <dxe:GridViewDataComboBoxColumn Caption="Main Account" FieldName="CountryID" VisibleIndex="1" Width="250">
                        <PropertiesComboBox ValueField="CountryID" ClientInstanceName="CountryID" TextField="CountryName" ClearButton-DisplayMode="Always" AllowMouseWheel="false">
                            <%-- <ValidationSettings RequiredField-IsRequired="true" Display="None"></ValidationSettings>
                            <ClientSideEvents SelectedIndexChanged="CountriesCombo_SelectedIndexChanged" />
                        </PropertiesComboBox>
                    </dxe:GridViewDataComboBoxColumn>--%>



                                                            <dxe:GridViewDataButtonEditColumn FieldName="bthSubAccountTDS" Caption="Sub Account" VisibleIndex="2">
                                                                <PropertiesButtonEdit>
                                                                    <ClientSideEvents ButtonClick="SubAccountButnClickTDS" KeyDown="SubAccountKeyDownTDS" />
                                                                    <Buttons>
                                                                        <dxe:EditButton Text="..." Width="20px">
                                                                        </dxe:EditButton>
                                                                    </Buttons>
                                                                </PropertiesButtonEdit>
                                                            </dxe:GridViewDataButtonEditColumn>


                                                            <%--                    <dxe:GridViewDataComboBoxColumn FieldName="CityID" Caption="Sub Account" VisibleIndex="2" Width="250">
                        <PropertiesComboBox TextField="CityName" ValueField="CityID">
                        </PropertiesComboBox>
                        <EditItemTemplate>
                            <dxe:ASPxComboBox runat="server" OnInit="CityCmb_Init" Width="100%" EnableIncrementalFiltering="true" TextField="CityName" ClearButton-DisplayMode="Always"
                                OnCallback="CityCmb_Callback" ValueField="CityID" ID="CityCmb" ClientInstanceName="CityID" EnableCallbackMode="true" AllowMouseWheel="false">
                                <ClientSideEvents EndCallback="CitiesCombo_EndCallback" />
                                <ValidationSettings RequiredField-IsRequired="true" Display="None"></ValidationSettings>
                            </dxe:ASPxComboBox>
                            <%--EnableCallbackMode="true"  OnInit="CityCmb_Init" 
                        </EditItemTemplate>
                    </dxe:GridViewDataComboBoxColumn>--%>




                                                            <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="WithDrawlTDS" Caption="Payment" Width="120" EditCellStyle-HorizontalAlign="Right">
                                                                <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                                    <MaskSettings Mask="&lt;0..99999999999&gt;.&lt;00..99&gt;" />
                                                                    <ClientSideEvents KeyDown="OnKeyDownTDS" LostFocus="WithDrawlTextChangeTDS"
                                                                        GotFocus="function(s,e){
                                    DebitGotFocusTDS(s,e);
                                    }" />

                                                                    <ClientSideEvents />
                                                                    <ValidationSettings Display="None"></ValidationSettings>
                                                                </PropertiesTextEdit>
                                                                <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="ReceiptTDS" Caption="Receipt" Width="120">
                                                                <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                                    <MaskSettings Mask="&lt;0..99999999999&gt;.&lt;00..99&gt;" />
                                                                    <ClientSideEvents KeyDown="OnKeyDownTDS" LostFocus="ReceiptTextChangeTDS"
                                                                        GotFocus="function(s,e){
                                    CreditGotFocusTDS(s,e);
                                    }" />
                                                                    <ClientSideEvents />
                                                                    <ValidationSettings Display="None"></ValidationSettings>
                                                                </PropertiesTextEdit>
                                                                <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                                            </dxe:GridViewDataTextColumn>


                                                            <dxe:GridViewDataButtonEditColumn FieldName="Project_Code" Caption="Project Code" VisibleIndex="5" Width="14%">
                                                                         <PropertiesButtonEdit>
                                                                            <ClientSideEvents ButtonClick="ProjectCodeButnClick" KeyDown="ProjectCodeKeyDown"/>
                                                                                  <Buttons>
                                                                            <dxe:EditButton Text="..." Width="20px">
                                                                            </dxe:EditButton>
                                                                                  </Buttons>
                                                                        </PropertiesButtonEdit>
                                                                     </dxe:GridViewDataButtonEditColumn>



                                                            <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="NarrationTDS" Caption="Narration">
                                                                <PropertiesTextEdit>
                                                                    <ClientSideEvents KeyDown="AddBatchNewTDS"></ClientSideEvents>
                                                                </PropertiesTextEdit>
                                                                <CellStyle Wrap="False" HorizontalAlign="Left" CssClass="gridcellleft"></CellStyle>
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataTextColumn FieldName="CashReportID" Caption="Srl No" ReadOnly="true" HeaderStyle-CssClass="hide" Width="0">
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataTextColumn FieldName="gvColMainAccountTDS" Caption="hidden Field Id" Width="0" HeaderStyle-CssClass="hide">
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataTextColumn FieldName="gvColSubAccountTDS" Caption="hidden Field Id" Width="0" HeaderStyle-CssClass="hide">
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataTextColumn FieldName="gvMainAcCodeTDS" Caption="hidden Field Id" Width="0" HeaderStyle-CssClass="hide">
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataTextColumn FieldName="IsSubledgerTDS" Caption="hidden Field Id" Width="0" HeaderStyle-CssClass="hide">
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataTextColumn FieldName="IsTDS" Caption="hidden Field Id" Width="0" HeaderStyle-CssClass="hide">
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataTextColumn FieldName="UniqueID" Caption="hidden Field Id" Width="0" HeaderStyle-CssClass="hide">
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataTextColumn FieldName="IsTDSSource" Caption="hidden Field Id" Width="0" HeaderStyle-CssClass="hide">
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataTextColumn FieldName="TDSPercentage" Caption="hidden Field Id" Width="0" HeaderStyle-CssClass="hide">
                                                            </dxe:GridViewDataTextColumn>

                                                             <dxe:GridViewDataTextColumn FieldName="ProjectId" Caption="ProjectId"  ReadOnly="True" Width="0"
                                                                             EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide"
                                                                             PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                                                                             <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                                                             </dxe:GridViewDataTextColumn>

                                                        </Columns>
                                                        <TotalSummary>
                                                            <dxe:ASPxSummaryItem SummaryType="Sum" FieldName="C2" Tag="C2_Sum" />
                                                        </TotalSummary>
                                                        <Settings ShowStatusBar="Hidden" />
                                                        <ClientSideEvents Init="OnInitTDS" EndCallback="OnEndCallbackTDS" BatchEditStartEditing="OnBatchEditStartEditingTDS"
                                                            BatchEditEndEditing="OnBatchEditEndEditingTDS"
                                                            CustomButtonClick="OnCustomButtonClickTDS" RowClick="GetVisibleIndexTDS" />
                                                        <SettingsDataSecurity AllowEdit="true" />


                                                        <SettingsEditing Mode="Batch" NewItemRowPosition="Bottom">
                                                            <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="row" />
                                                        </SettingsEditing>
                                                    </dxe:ASPxGridView>
                                                    <div class="text-center">
                                                        <table style="margin-left: 354px; margin-top: 10px">
                                                            <tr>
                                                                <td style="padding-right: 50px"><b>Total Amount</b></td>
                                                                <td style="width: 203px;">
                                                                    <dxe:ASPxTextBox ID="txtTotalAmount" runat="server" Width="105px" ClientInstanceName="c_txt_Debit" HorizontalAlign="Right" Font-Size="12px" ReadOnly="true">

                                                                        <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                                                    </dxe:ASPxTextBox>
                                                                </td>
                                                                <td>
                                                                    <dxe:ASPxTextBox ID="txtTotalPayment" runat="server" Width="105px" ClientInstanceName="c_txt_Credit" HorizontalAlign="Right" Font-Size="12px" ReadOnly="true">

                                                                        <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                                                    </dxe:ASPxTextBox>
                                                                </td>
                                                                <td style="padding-right: 50px; padding-left: 44px; display: none;"><b>Total Net Amount</b></td>
                                                                <td style="width: 203px; display: none;">
                                                                    <dxe:ASPxTextBox ID="txtTotalNetAmount" runat="server" Width="105px" ClientInstanceName="c_txtTotalNetAmount" HorizontalAlign="Right" Font-Size="12px" ReadOnly="true">

                                                                        <MaskSettings Mask="&lt;-999999999999..999999999999&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                                                    </dxe:ASPxTextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                    <table style="width: 100%;" id="BtnTable">
                                                        <tr>
                                                            <td style="padding: 15px 0;">
                                                                <span id="tdSaveNewButton">
                                                                    <%-- <% if (rights.CanAdd)
                                                                        { %>
                                                                    <a>--%>
                                                                    <dxe:ASPxButton ID="btnSaveNew" ClientInstanceName="cbtnSaveRecordsTDS" runat="server"
                                                                        AutoPostBack="false" CssClass="btn btn-success" TabIndex="0" Text="S&#818;ave & New"
                                                                        UseSubmitBehavior="False">
                                                                        <ClientSideEvents Click="function(s, e) {SaveButtonClickNew();}" />
                                                                    </dxe:ASPxButton>
                                                                    <%--   </a>
                                                                      <% } %>--%>
                                                                </span>
                                                                <span id="tdSaveButton">
                                                                    <dxe:ASPxButton ID="btnSaveRecords" ClientInstanceName="cbtn_SaveRecordsTDS" runat="server"
                                                                        AutoPostBack="false" CssClass="btn btn-primary" TabIndex="0" Text="Save & Ex&#818;it"
                                                                        UseSubmitBehavior="False">
                                                                        <ClientSideEvents Click="function(s, e) {SaveButtonClick();}" />
                                                                    </dxe:ASPxButton>

                                                                </span>
                                                                <div runat="server" id="divTDS">
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </dxe:PanelContent>
                                            </PanelCollection>
                                            <%--  <ClientSideEvents EndCallback="acpEndCallbackGeneral" />--%>
                                        </dxe:ASPxCallbackPanel>
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Billing/Shipping" Text="Billing/Shipping">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                        <ucBS:Purchase_BillingShipping runat="server" ID="Purchase_BillingShipping" />
                                        <asp:HiddenField runat="server" ID="hfTermsConditionDocType" Value="CBE" />
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
                </dxe:PanelContent>

            </div>

            <div id="HiddenField">
                <%--  <asp:HiddenField ID="hdnDefaultBranch" runat="server" />
                <asp:HiddenField ID="hdnType" runat="server" />
                <asp:HiddenField ID="hdnAccountType" runat="server" />
                <asp:HiddenField ID="txtMainAccount_hidden" runat="server" />
                <asp:HiddenField ID="txtSubAccount_hidden" runat="server" />
                <asp:HiddenField ID="hdn_Brch_NonBrch" runat="server" />
                <asp:HiddenField ID="hdn_SubLedgerType" runat="server" />
                <asp:HiddenField ID="hdn_MainAcc_Type" runat="server" />
                <asp:HiddenField ID="hdn_SubAccountIDE" runat="server" />
                <asp:HiddenField ID="txtBankAccounts_hidden" runat="server" />--%>
                <asp:HiddenField ID="hdnisprojectexists" runat="server"/>
                <input type="hidden" id="IsTaxApplicable" value="" />
                <asp:HiddenField ID="hdn_Mode" runat="server" Value="Edit" />
                <%-- <asp:HiddenField ID="hdn_PayeeIDOnUpdate" runat="server" />
                <asp:HiddenField ID="hdn_Brch_NonBrchE" runat="server" />
                <asp:HiddenField ID="hdn_CurrentSegment" runat="server" />--%>
                <asp:HiddenField ID="hdn_CashBankType_InstType" runat="server" />
                <asp:HiddenField ID="hdn_SegID_SegmentName" runat="server" />
                <%--  <asp:HiddenField ID="hdn_EditVoucher_SegmentID_Name" runat="server" />
                <asp:HiddenField ID="hdn_OriginalTDate" runat="server" />--%>
                <asp:HiddenField ID="hdnCashBankId" runat="server" />
                <asp:HiddenField ID="hdnCashBankText" runat="server" />
                <asp:HiddenField ID="hdnMainAccountId" runat="server" />
                <%--<asp:HiddenField ID="hdnMainAccountText" runat="server" />--%>
                <%--   <asp:HiddenField ID="hdnSubAccountId" runat="server" />--%>
                <%--  <asp:HiddenField ID="hdnSubAccountText" runat="server" />--%>
                <asp:HiddenField ID="hdnBranchId" runat="server" />
                <asp:HiddenField ID="hfIsFilter" runat="server" />
                <asp:HiddenField ID="TaxAmountOngrid" runat="server" />
                <asp:HiddenField ID="VisibleIndexForTax" runat="server" />


                <%--  <asp:HiddenField ID="hdnBranchText" runat="server" />--%>
                <%--  <asp:HiddenField ID="hdnIssueBankId" runat="server" />--%>
                <%--  <asp:HiddenField ID="hdnIssueBankText" runat="server" />--%>
                <asp:HiddenField ID="hdnJNMode" runat="server" />
                <%--  <asp:HiddenField ID="hdnReceive" runat="server" />--%>
                <%--    <asp:HiddenField ID="hdnMainAccountEId" runat="server" />--%>
                <%-- <asp:HiddenField ID="hdnMainAccountEText" runat="server" />--%>
                <%--  <asp:HiddenField ID="hdnMainAccountChange" runat="server" />--%>
                <asp:HiddenField ID="hdnBtnClick" runat="server" />
                <%--  <asp:HiddenField ID="hdnSegmentid" runat="server" />--%>
                <asp:HiddenField ID="hdnRefreshType" runat="server" />
                <asp:HiddenField ID="hdnSchemaType" runat="server" />

                <asp:HiddenField ID="hdnInstrumentType" runat="server" />
                <asp:HiddenField ID="hdnInstrumentNo" runat="server" />
                <asp:HiddenField ID="hdnCurrenctId" runat="server" />
                <asp:HiddenField ID="hdnEditClick" runat="server" />
                <%--  <asp:HiddenField ID="HdnbranchIDSession" runat="server" />--%>
                <asp:HiddenField ID="hdfIsDelete" runat="server" />
                <asp:HiddenField ID="hdnEditCBID" runat="server" />
                <asp:HiddenField ID="hdnEditRfid" runat="server" />
                <asp:HiddenField ID="hdnPayment" runat="server" />
                <asp:HiddenField ID="hdnTaxGridBind" runat="server" />
                <asp:HiddenField ID="hdnPageStatus" runat="server" />
                <asp:HiddenField ID="hdnAutoPrint" runat="server" />
                <asp:HiddenField ID="hdnView" runat="server" />
                <asp:HiddenField ID="hdnSubAccountYESNO" runat="server" />
                <asp:HiddenField ID="hdnPaidToYesNO" runat="server" />
                <%-- for Tax --%>

                <asp:HiddenField ID="hfVendorGSTIN" runat="server" />
                <asp:HiddenField runat="server" ID="HDItemLevelTaxDetails" />
                <asp:HiddenField runat="server" ID="HDHSNCodewisetaxSchemid" />
                <asp:HiddenField runat="server" ID="HDBranchWiseStateTax" />
                <asp:HiddenField runat="server" ID="HDStateCodeWiseStateIDTax" />

                <%--  --%>
                <%-- <asp:HiddenField ID="hdnCheckAdd" runat="server" />
                --%>
            </div>
            <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
                <ClientSideEvents ControlsInitialized="AllControlInitilize" />
            </dxe:ASPxGlobalEvents>
            <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="true" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
            </dxe:ASPxGridViewExporter>
            <asp:SqlDataSource ID="SqlDataSourceMainAccount" runat="server"
                SelectCommand=""></asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlDataSourceSubAccount" runat="server"
                SelectCommand=""></asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlSchematype" runat="server"
                SelectCommand=""></asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlCurrency" runat="server"
                SelectCommand="Select * From ((Select '0' as Currency_ID , 'Select' as Currency_AlphaCode) Union select Currency_ID,Currency_AlphaCode from Master_Currency )tbl Order By Currency_ID"></asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlCurrencyBind" runat="server"></asp:SqlDataSource>

            <%-- -------------------POPUPControl   FOR Main & Sub Account-------------------------------------%>
            <dxe:ASPxPopupControl ID="MainAccountpopUp" runat="server" ClientInstanceName="cMainAccountpopUp"
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
                        <%--  <dxe:ASPxGridLookup ID="MainAccountLookUp" runat="server" DataSourceID="EntityServerMainDataSource" ClientInstanceName="cMainAccountLookUp"
                    KeyFieldName="MainAccount_ReferenceID" Width="800" TextFormatString="{0}" ClientSideEvents-TextChanged="MainAccountSelected" 
                    ClientSideEvents-KeyDown="MainAccountlookUpKeyDown">
                    <Columns>
                        <dxe:GridViewDataColumn FieldName="MainAccount_Name" Caption="Main Account Name" Width="220">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataColumn>
                        <dxe:GridViewDataColumn FieldName="MainAccount_SubLedgerType" Caption="Type" Width="80">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataColumn>

                    </Columns>
                    <GridViewProperties Settings-VerticalScrollBarMode="Auto">
                   
                        <Templates>
                            <StatusBar>
                                <table class="OptionsTable" style="float: right">
                                    <tr>
                                        <td>
                                            
                                        </td>
                                    </tr>
                                </table>
                            </StatusBar>
                        </Templates>
                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                    </GridViewProperties>
                </dxe:ASPxGridLookup>--%>
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
                            OnItemsRequestedByFilterCondition="ASPxComboBox_OnItemsRequestedByFilterCondition_SQL" FilterMinLength="4"
                            OnItemRequestedByValue="ASPxComboBox_OnItemRequestedByValue_SQL" TextFormatString="{0}"
                            DropDownStyle="DropDown" DropDownRows="7">
                            <Columns>
                                <dxe:ListBoxColumn FieldName="Contact_Name" Caption="Sub Account Name" Width="320px" />
                            </Columns>
                            <ClientSideEvents ValueChanged="function(s, e) {GetSubAcountComboBox(e)}" KeyDown="SubAccountComboBoxKeyDown" />
                        </dxe:ASPxComboBox>
                        <%--  <dxe:ASPxGridLookup ID="SubAccountLookup" runat="server" ClientInstanceName="cSubAccountLookUp"
                    KeyFieldName="SubAccount_ReferenceID" Width="800" TextFormatString="{0}" ClientSideEvents-TextChanged="SubAccountSelected"
                    ClientSideEvents-KeyDown="SubAccountlookUpKeyDown">
                    <Columns>
                        <dxe:GridViewDataColumn FieldName="Contact_Name" Caption="Sub Account Name" Width="220">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataColumn>

                    </Columns>
                    <GridViewProperties Settings-VerticalScrollBarMode="Auto">

                        <Templates>
                            <StatusBar>
                                <table class="OptionsTable" style="float: right">
                                    <tr>
                                        <td>
                                            
                                        </td>
                                    </tr>
                                </table>
                            </StatusBar>
                        </Templates>
                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                    </GridViewProperties>
                </dxe:ASPxGridLookup>--%>
                    </dxe:PopupControlContentControl>
                </ContentCollection>
                <HeaderStyle BackColor="Blue" Font-Bold="True" ForeColor="White" />
            </dxe:ASPxPopupControl>
            <%--  <dx:LinqServerModeDataSource ID="EntityServerMainDataSource" runat="server" OnSelecting="EntityServerMainDataSource_Selecting"
        ContextTypeName="ERPDataClassesDataContext" TableName="v_MainAccountList" />--%>



            <%-- -------------------End   POPUPControl   FOR Main & Sub Account-------------------------------------%>
        </div>

    </div>
    </div>
    <dxe:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" ContainerElementID="gridBatch"
        Modal="True">
    </dxe:ASPxLoadingPanel>
    <dxe:ASPxCallbackPanel runat="server" ID="acpPaymentTDS" ClientInstanceName="cacpPaymentTDS">
        <PanelCollection>
            <dxe:PanelContent runat="server">
            </dxe:PanelContent>
        </PanelCollection>
    </dxe:ASPxCallbackPanel>
    <dxe:ASPxCallbackPanel runat="server" ID="acpCrossBtn" ClientInstanceName="cacpCrossBtn" OnCallback="acpCrossBtn_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="acpCrossBtnEndCall" />
    </dxe:ASPxCallbackPanel>
    <asp:SqlDataSource ID="dsBranch" runat="server"
        ConflictDetection="CompareAllValues"
        SelectCommand=""></asp:SqlDataSource>

    <%--Tax PopUp Start--%>


    <div class="modal fade" id="TDSmodal" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <!-- Modal MainAccount-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">TDS Deposit</h4>
                </div>
                <div class="modal-body">
                    <div class="clearfix">
                        <div class="col-sm-3">
                            <label>Section</label>
                            <div class="relative">
                                <dxe:ASPxComboBox runat="server" SelectedIndex="0" ID="tdsSection" ClientInstanceName="ctdsSection" DataSourceID="dsTDS" ValueField="ID" TextField="Section_Code" ValueType="System.String">
                                    <ClientSideEvents SelectedIndexChanged="selectTDSChange" />
                                </dxe:ASPxComboBox>
                                <asp:SqlDataSource runat="server" ID="dsTDS" SelectCommand="select '0~' as ID ,'Select' Section_Code Union ALL  Select Section_Code +'~'+Section_Description  ID,  Section_Code from tbl_master_TDS_Section  SEC inner join Master_TDSTCS TDS on TDS.TDSTCS_Code=SEC.Section_Code"></asp:SqlDataSource>
                            </div>
                        </div>
                        <div class="col-sm-3">
                            <label>TDS Deducted on</label>
                            <div class="relative">
                                <dxe:ASPxTextBox ID="txtDeductionON" ClientEnabled="false" runat="server" ClientInstanceName="ctxtDeductionON"></dxe:ASPxTextBox>
                            </div>
                        </div>
                        <div class="col-sm-3">
                            <label>Date of Deposit</label>
                            <div class="relative">
                                <dxe:ASPxDateEdit ID="tdsDate" ClientEnabled="false" runat="server" ClientInstanceName="ctdsDate" EditFormat="Custom"
                                    Font-Size="12px" UseMaskBehavior="True" Width="100%" EditFormatString="dd-MM-yyyy" CssClass="pull-left">
                                    <ButtonStyle Width="13px"></ButtonStyle>

                                </dxe:ASPxDateEdit>
                            </div>
                        </div>
                        <div class="col-sm-3">
                            <label>Quater</label>
                            <div class="relative">
                                <select id="ddlQuater" runat="server" class="form-control">
                                    <option value="Q1">Q1</option>
                                    <option value="Q2">Q2</option>
                                    <option value="Q3">Q3</option>
                                    <option value="Q4">Q4</option>

                                </select>
                            </div>
                        </div>

                        <div class="col-sm-3">
                            <label>Surcharge</label>
                            <div class="relative">
                                <dxe:ASPxTextBox ID="txtSurcharge" DisplayFormatString="0.00" runat="server" ClientEnabled="false" ClientInstanceName="ctxtSurcharge">

                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />

                                </dxe:ASPxTextBox>

                            </div>
                        </div>
                        <div class="col-sm-3">
                            <label>Education Cess</label>
                            <div class="relative">
                                <dxe:ASPxTextBox ID="txteduCess" DisplayFormatString="0.00" runat="server" ClientEnabled="false" ClientInstanceName="ctxteduCess">
                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                                </dxe:ASPxTextBox>

                            </div>
                        </div>
                        <div class="col-sm-3">
                            <label>Interest</label>
                            <div class="relative">
                                <dxe:ASPxTextBox ID="txtInterest" DisplayFormatString="0.00" runat="server" ClientInstanceName="ctxtInterest">
                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                                    <ClientSideEvents LostFocus="RecalculateTotal" />

                                </dxe:ASPxTextBox>

                            </div>
                        </div>
                        <div class="col-sm-3">
                            <label>Late Fees (u/s 234E)</label>
                            <div class="relative">
                                <dxe:ASPxTextBox ID="txtLateFees" DisplayFormatString="0.00" runat="server" ClientInstanceName="ctxtLateFees">
                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                                    <ClientSideEvents LostFocus="RecalculateTotal" />
                                </dxe:ASPxTextBox>

                            </div>
                        </div>

                        <div class="col-sm-3">
                            <label><b>Total</b></label>
                            <div class="relative">
                                <dxe:ASPxTextBox ID="txtTotal" DisplayFormatString="0.00" ClientEnabled="false" runat="server" ClientInstanceName="ctxtTotal">
                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                                </dxe:ASPxTextBox>

                            </div>
                        </div>

                        <div class="col-sm-3">
                            <label><b>Tax</b></label>
                            <div class="relative">
                                <dxe:ASPxTextBox ID="txtTax" DisplayFormatString="0.00" ClientEnabled="false" runat="server" ClientInstanceName="ctxtTax">
                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                                </dxe:ASPxTextBox>

                            </div>
                        </div>

                        <div class="col-sm-3">
                            <label>Others</label>
                            <div class="relative">
                                <dxe:ASPxTextBox ID="txtOthers" DisplayFormatString="0.00" runat="server" ClientInstanceName="ctxtOthers">
                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                                    <ClientSideEvents LostFocus="RecalculateTotal" />

                                </dxe:ASPxTextBox>

                            </div>
                        </div>
                        <div class="clear"></div>
                        <div class="bdbox clearfix">
                            <div class="headingTypeblo">Bank Details</div>
                            <div class="bdboxContent">
                                <div class="col-sm-3">
                                    <label>Bank Name</label>
                                    <div class="relative">
                                        <dxe:ASPxTextBox ID="txtBankName" runat="server" ClientInstanceName="ctxtBankName"></dxe:ASPxTextBox>

                                    </div>
                                </div>

                                <div class="col-sm-3">
                                    <label>Branch</label>
                                    <div class="relative">
                                        <dxe:ASPxTextBox ID="txtBankBrach" runat="server" ClientInstanceName="ctxtBankBranch"></dxe:ASPxTextBox>

                                    </div>
                                </div>

                                <div class="col-sm-3">
                                    <label>BSR Code</label>
                                    <div class="relative">
                                        <dxe:ASPxTextBox ID="txtBRS" runat="server" ClientInstanceName="ctxtBRS"></dxe:ASPxTextBox>

                                    </div>
                                </div>

                                <div class="col-sm-3">
                                    <label>Challan No</label>
                                    <div class="relative">
                                        <dxe:ASPxTextBox ID="txtChallanNo" runat="server" ClientInstanceName="ctxtChallanNo"></dxe:ASPxTextBox>

                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="clear"></div>
                        <div class="col-md-12">
                            <table class="HtmlGrid display  dataTable" id="tbltdsDetails">
                                <thead>
                                    <tr>
                                        <th>Select</th>
                                        <th class='hide'></th>
                                        <th class='hide'></th>
                                        <th class='hide'></th>
                                        <th>Document No.</th>
                                        <th>Party ID</th>
                                        <th>Section</th>
                                        <th>Payment/Credit Date</th>
                                        <th>Total Tax</th>
                                        <th>Amount of Tax</th>
                                        <th>Surcharge</th>
                                        <th>Edu. Cess</th>
                                    </tr>
                                </thead>

                            </table>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary m0" onclick="BindGridViaTDSData();">Save</button>
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Cancel</button>
                </div>
            </div>
        </div>
    </div>
    <%--Main/Sub Account Model--%>

    <div class="modal fade" id="MainAccountModel" role="dialog">
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
                                <th>Subledger Type</th>
                                <th>Reverse Applicable</th>
                                <th>HSN/SAC</th>
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


    <div class="modal fade" id="MainAccountModelTDS" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <!-- Modal MainAccount-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" onclick="closeModalTDS();">&times;</button>
                    <h4 class="modal-title">Main Account Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="MainAccountNewkeydownTDS(event)" id="txtMainAccountSearchTDS" autofocus width="100%" placeholder="Search by Main Account Name or Short Name" />

                    <div id="MainAccountTableTDS">
                        <table border='1' width="100%">
                            <tr class="HeaderStyle">
                                <th>Main Account Name</th>
                                <th>Short Name</th>
                                <th>Subledger Type</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" onclick="closeModalTDS();">Close</button>
                </div>
            </div>
        </div>
    </div>


    <div class="modal fade" id="SubAccountModelTDS" role="dialog" data-backdrop="static"
        data-keyboard="false">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" onclick="CloseSubModalTDS();">&times;</button>
                    <h4 class="modal-title">Sub Account Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="SubAccountNewkeydownTDS(event)" id="txtSubAccountSearchTDS" autofocus width="100%" placeholder="Search By Sub Account Name or Code" />

                    <div id="SubAccountTableTDS">
                        <table border='1' width="100%">
                            <tr class="HeaderStyle">
                                <th>Sub Account Name [Unique ID]</th>
                                <th>Sub Account Code</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" onclick="CloseSubModalTDS();">Close</button>
                </div>
            </div>

        </div>
    </div>

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





    <asp:HiddenField runat="server" ID="hdnIsTDS" />
    <asp:HiddenField runat="server" ID="hdnTDSData" />
    <asp:HiddenField runat="server" ID="hdnTDSSection" />
    <div id="HiddenFeild">
        <asp:HiddenField ID="hdnSegmentid" runat="server" />
        <asp:HiddenField ID="hdnMode" runat="server" />
        <asp:HiddenField ID="HiddenField1" runat="server" />
        <asp:HiddenField ID="hdnSchemaTypeTDS" runat="server" />

        <asp:HiddenField ID="HiddenField2" runat="server" />
        <asp:HiddenField ID="hdnJournalNo" runat="server" />
        <asp:HiddenField ID="hdnIBRef" runat="server" />
        <asp:HiddenField ID="HiddenField3" runat="server" />
        <asp:HiddenField ID="HiddenField4" runat="server" />

        <asp:HiddenField ID="hdnMainAccountIdTDS" runat="server" />
        <asp:HiddenField ID="HiddenSubMandatory" runat="server" />
        <asp:HiddenField ID="hdnIsValidate" runat="server" />
        <asp:HiddenField ID="hdnProjectSelectInEntryModule" runat="server" />
           <asp:HiddenField ID="hdnAllowProjectInDetailsLevel" runat="server" />
      <asp:HiddenField ID="hdnEditProjId" runat="server" />

    </div>

    <asp:SqlDataSource ID="dsSupplyState" runat="server" ConflictDetection="CompareAllValues"
        SelectCommand="select id as state_id,state+' (State Code:'+StateCode+')' as state_name   from tbl_master_state where ISNULL(state+' (State Code:'+StateCode+')','')<>''  order by state_name"></asp:SqlDataSource>
    <asp:SqlDataSource ID="dsTaxType" runat="server" ConflictDetection="CompareAllValues"
        SelectCommand="select taxGrp_Id,taxGrp_Description from tbl_master_taxgrouptype  order by taxGrp_Description"></asp:SqlDataSource>

     <asp:HiddenField ID="hdnProjectMandatory" runat="server" />

    <asp:HiddenField ID="hdnLockFromDate" runat="server" />
    <asp:HiddenField ID="hdnLockToDate" runat="server" />
    <asp:HiddenField ID="hdnLockFromDateCon" runat="server" />
    <asp:HiddenField ID="hdnLockToDateCon" runat="server" />
    <asp:HiddenField ID="hdnIsLeadAvailableinTransactions" runat="server" />

</asp:Content>
