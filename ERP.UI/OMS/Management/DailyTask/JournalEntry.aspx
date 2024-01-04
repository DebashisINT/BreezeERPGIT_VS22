<%--=======================================================Revision History=========================================================================
    1.0 Rev Work start: Copy Feature required for Journal Vouchers Date:-27.05.2022 Mantise no:24911
    2.0     Priti       V2.0.36  02-02-2023     0025253: listing view upgradation required of Journals of Accounts & Finance
    3.0     Pallab      V2.0.37  04-04-2023     0025830: Journal Voucher module design modification
    4.0     Sanchita    V2.0.40  08-02-2023     26801 : Entered On, Entered By, Modified On, Modified By column required
                                                in the Journal Details list view 
    5.0     Sanchita    V2.0.40  21-09-2023     26831 : Data Freeze is not working properly for Journal   
    6.0     Priti       V2.0.41  03-11-2023     0026956 : Duplicate Journal got Saved   
    7.0     Sanchita    V2.0.42  04-01-2023     27150: After deleting any rows from the Journal Voucher along with Blank row Debit and Credit value not refreshing.
=========================================================End Revision History========================================================================--%>

<%@ Page Title="Journal Entry" EnableEventValidation="false" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="JournalEntry.aspx.cs" Inherits="ERP.OMS.Management.DailyTask.JournalVoucherEntry" %>

<%--Rev Work start: Copy Feature required for Journal Vouchers Date:-27.05.2022 Mantise no:24911--%>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="../Activities/JS/SearchPopup.js"></script>
    <link href="../Activities/CSS/SearchPopup.css" rel="stylesheet" />
    <script>
        // Rev 5.0
        function SetLostFocusonDemand(e) {
            if ((new Date($("#hdnLockFromDate").val()) <= tDate.GetDate()) && (tDate.GetDate() <= new Date($("#hdnLockToDate").val()))) {
                jAlert("DATA is Freezed between   " + $("#hdnLockFromDateCon").val() + " to " + $("#hdnLockToDateCon").val() + " for Add.");
            }
        }
        function SetTDSLostFocusonDemand(e) {
            if ((new Date($("#hdnLockFromDate").val()) <= tDateTDS.GetDate()) && (tDateTDS.GetDate() <= new Date($("#hdnLockToDate").val()))) {
                jAlert("DATA is Freezed between   " + $("#hdnLockFromDateCon").val() + " to " + $("#hdnLockToDateCon").val() + " for Add.");
            }
        }
        // End of Rev 5.0
        function componentEndCallBack(s, e) {
            // clookup_GRNOverhead.gridView.Refresh();
        }
        function PanelGRNOverheadTDSEndCallBack(s, e) {
            //  clookup_GRNOverheadTDS.gridView.Refresh();
        }

        var globalRowIndex;
        var globalRowIndexTDS;
        <%--Rev Work start: Copy Feature required for Journal Vouchers Date:-27.05.2022 Mantise no:24911--%>
        var Mode;
        <%--Rev Work close: Copy Feature required for Journal Vouchers Date:-27.05.2022 Mantise no:24911--%>

        isTDSSelected = 1;
        function OnAddButtonClick() {
            $("#divIsPartyJournal").hide();
            document.getElementById('tblBtnSavePanel').style.display = 'block';
            document.getElementById('divAddNew').style.display = 'block';
            document.getElementById('divAddNewTDS').style.display = 'none';
            document.getElementById('ddl_AmountAre').value = 3;
            TblSearch.style.display = "none";
            btncross.style.display = "block";
            $('#<%=hdnMode.ClientID %>').val('0'); //Entry
            $('#<%= lblHeading.ClientID %>').text("Add Journal Voucher");
            var defaultbranch = '<%=Session["userbranchID"]%>';
            $('#<%=hdnBranchId.ClientID %>').val(defaultbranch);
            var CmbScheme = document.getElementById("<%=CmbScheme.ClientID%>");
            CmbScheme.options[0].selected = true;

            // CountryID.PerformCallback(document.getElementById('ddlBranch').value);

            //grid.AddNewRow();
            grid.batchEditApi.EndEdit();
            document.getElementById("<%=CmbScheme.ClientID%>").focus();

            cbtnSaveRecords.SetVisible(false);
            cbtn_SaveRecords.SetVisible(false);
            //loadCurrencyMassage.style.display = "block";

        }
        <%--Rev Work start: Copy Feature required for Journal Vouchers Date:-27.05.2022 Mantise no:24911--%>
        function OnCopy(visibleIndex, IStds, Visible, RET_VISIBLE, JvID) {

            VisibleIndexE = visibleIndex;
            document.getElementById('hdnVal').value = 'Copy';
            document.getElementById('hdnId').value = JvID;
            var Type = 'Copy';
            Mode = 'Copy';
            if (IStds == "True") {
                alert("This is a Journal with TDS. This will not be copied.");
                return;
            }
            SetNumberingSchemeDataSource(JvID, Type);
            $("#divIsPartyJournal").hide();
            document.getElementById('tblBtnSavePanel').style.display = 'block';
            document.getElementById('divAddNew').style.display = 'block';
            document.getElementById('divAddNewTDS').style.display = 'none';
            document.getElementById('ddl_AmountAre').value = 3;
            TblSearch.style.display = "none";
            btncross.style.display = "block";
            $('#<%=hdnMode.ClientID %>').val('0'); //Entry
           <%-- $('#<%=hdnMode.ClientID %>').val('1');--%>
            $('#<%= lblHeading.ClientID %>').text("Copy Journal Voucher");
            var defaultbranch = '<%=Session["userbranchID"]%>';
            $('#<%=hdnBranchId.ClientID %>').val(defaultbranch);
            var CmbScheme = document.getElementById("<%=CmbScheme.ClientID%>");
            CmbScheme.options[0].selected = true;

            grid.AddNewRow();
            grid.batchEditApi.EndEdit();
            document.getElementById("<%=CmbScheme.ClientID%>").focus();

            grid.PerformCallback('Edit~' + VisibleIndexE);

            //cbtnSaveRecords.SetVisible(false);
            $("#tdSaveButton").hide();
            //cbtn_SaveRecords.SetVisible(false);
        }
        function SetNumberingSchemeDataSource(JvID, Type) {

            $.ajax({
                type: "POST",
                url: 'JournalEntry.aspx/GetNumberingSchemeByType',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ JvID: JvID, Type: Type }),
                success: function (msg) {
                    var returnObject = msg.d.NumberingSchema;
                    //if (returnObject.NumberingSchema) {
                    //    SetDataSourceOnComboBox(cCmbScheme, returnObject.NumberingSchema);
                    //}
                    $("#CmbScheme").empty();
                    var ddl = document.getElementById("<%=CmbScheme.ClientID %>");
                    /*for (i = 0; i < returnObject.length; i++) {
                        var option = document.createElement("OPTION");
                        option.innerHTML = returnObject[i].Name;
                        option.value = returnObject[i].Id;                     
                        ddl.options.add(option);
                    }
                    for (i = 0; i < returnObject.length; i++) {
                        var option = document.createElement("OPTION");
                        option.innerHTML = returnObject[i].Name;
                        option.value = returnObject[i].Id;
                        ddl1.options.add(option);
                    }*/
                    $.each(returnObject, function (key, value) {
                        $('#<%=CmbScheme.ClientID%>').append($("<option></option>").val(value.Id).html(value.Name));
                    });
                    //$.each(returnObject, function (data, value) {
                    //    $("#CmbScheme").append($("<option></option>").val(value.Id).html(value.Name));
                    //})
                }
            });
        }
        <%--Rev Work close: Copy Feature required for Journal Vouchers Date:-27.05.2022 Mantise no:24911--%>
        $(document).ready(function () {
            if ($("#hdnType").val() == "Edit") {
                clookup_Project.SetEnabled(true);
                clookupTDS_Project.SetEnabled(true);
            }
            <%--Rev Work start: Copy Feature required for Journal Vouchers Date:-27.05.2022 Mantise no:24911--%>
            var JvID = 0;
            var Type = 'Add';
            SetNumberingSchemeDataSource(JvID, Type);
            Mode = 'Add';
            var val = document.getElementById("CmbScheme").value;
            $('#hdnSchemeVal').val(val);
            <%--Rev Work close: Copy Feature required for Journal Vouchers Date:-27.05.2022 Mantise no:24911--%>
        });

        function EnableDisableTDS(flag) {
            if (flag) {
                ctxtTDSAmount.SetEnabled(true);
                ccmbtds.SetEnabled(true);

                //Add Section Nil TDS 
                chkNILRateTDS.SetEnabled(false);
                chkNILRateTDS.SetChecked(false);
                //End section Nil TDS
            }
            else {
                ctxtTDSAmount.SetEnabled(false);
                ccmbtds.SetEnabled(false);

                //Add Section Nil TDS 
                chkNILRateTDS.SetEnabled(true);
                //End section Nil TDS

            }
        }

        function TDScheckchange(s, e) {
            EnableDisableTDS(chkTDSJournal.GetChecked());
        }


        function ProjectCodeSelected(s, e) {
            //debugger;
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

            setTimeout(function () { grid.batchEditApi.StartEdit(globalRowIndex, 6); }, 200);

        }

        function ProjectCodeSelectedTDS(s, e) {
            if (clookupPopupTDS_ProjectCode.GetGridView().GetFocusedRowIndex() == -1) {
                cProjectCodePopupTDS.Hide();

                return;
            }
            var ProjectInlineLookUpData = clookupPopupTDS_ProjectCode.GetGridView().GetRowKey(clookupPopupTDS_ProjectCode.GetGridView().GetFocusedRowIndex());
            var ProjectInlinedata = ProjectInlineLookUpData.split('~')[0];
            gridTDS.batchEditApi.StartEdit(globalRowIndexTDS);
            var ProjectCode = clookupPopupTDS_ProjectCode.GetValue();
            cProjectCodePopupTDS.Hide();

            gridTDS.GetEditor("Project_Code").SetText(ProjectCode);
            gridTDS.GetEditor("ProjectId").SetText(ProjectInlinedata);

            SetTDSProjectCode();
            setTimeout(function () { gridTDS.batchEditApi.StartEdit(globalRowIndexTDS, 6); }, 500);

        }



        function lookup_ProjectCodeKeyDown(s, e) {
            if (e.htmlEvent.key == "Escape") {
                cProjectCodePopup.Hide();

            }
        }

        function lookup_ProjectCodeKeyDownTDS(s, e) {
            if (e.htmlEvent.key == "Escape") {
                cProjectCodePopupTDS.Hide();

            }
        }


        function ProjectCodeButnClick(s, e) {
            if (e.buttonIndex == 0) {

                if ($("#hdnAllowProjectInDetailsLevel").val() != "0") {
                    clookupPopup_ProjectCode.Clear();


                    if (clookupPopup_ProjectCode.Clear()) {
                        //cProjectCodePopup.Show();
                        //CHINMOY ADDED 21-01-2020 MANTIS: 21627
                        if ($("#hdnProjectSelectInEntryModule").val() == "1") {
                            cProjectCodePopup.Show();
                        }
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

        function ProjectCodeTDSButnClick(s, e) {
            if (e.buttonIndex == 0) {
                if ($("#hdnAllowProjectInDetailsLevel").val() != "0") {
                    clookupPopupTDS_ProjectCode.Clear();


                    if (clookupPopupTDS_ProjectCode.Clear()) {
                        cProjectCodePopupTDS.Show();
                        clookupPopupTDS_ProjectCode.Focus();
                    }
                    //cProjectCodeCallback.PerformCallback('Type~' + Type + "~" + InvoiceNo);

                    if ($("#hdnProjectSelectInEntryModule").val() == "1") {
                        if ((clookupTDS_Project.GetGridView().GetRowKey(clookupTDS_Project.GetGridView().GetFocusedRowIndex()) != null)) {
                            cProjectCodeTDSCallback.PerformCallback('ProjectId~' + (clookupTDS_Project.GetGridView().GetRowKey(clookupTDS_Project.GetGridView().GetFocusedRowIndex())));
                        }
                        else {
                            cProjectCodeTDSCallback.PerformCallback('ProjectId~' + "0");
                        }
                    }
                    else {
                        cProjectCodeTDSCallback.PerformCallback('ProjectId~' + "0");
                    }
                }
            }
        }

        function ProjectCodeGotFocus(s, e) {
            if ($("#hdnProjectSelectInEntryModule").val() == "0") {
                //setTimeout(function () { grid.batchEditApi.StartEdit(globalRowIndex,6); }, 200);
                grid.batchEditApi.StartEdit(globalRowIndex, 6);
            }
        }
        function ProjectCodeKeyDown(s, e) {


            if (e.htmlEvent.key == "Enter" || e.htmlEvent.key == "NumpadEnter") {


                s.OnButtonClick(0);
            }
        }

        function ProjectCodeTDSKeyDown(s, e) {


            if (e.htmlEvent.key == "Enter" || e.htmlEvent.key == "NumpadEnter") {


                s.OnButtonClick(0);
            }
        }

        function ProjectCodeCallback_endcallback() {

            clookupPopup_ProjectCode.ShowDropDown();;
            clookupPopup_ProjectCode.Focus();
            clookupPopup_ProjectCode.Clear()

        }



        function ProjectCodeTDSCallback_endcallback() {

            clookupPopupTDS_ProjectCode.ShowDropDown();;
            clookupPopupTDS_ProjectCode.Focus();
            clookupPopupTDS_ProjectCode.Clear()

        }



        function OnAddButtonClickTDS() {
            $("#divIsPartyJournal").hide();
            document.getElementById('tblBtnSavePanel').style.display = 'block';
            document.getElementById('divAddNew').style.display = 'none';
            document.getElementById('divAddNewTDS').style.display = 'block';
            document.getElementById('ddl_AmountAre').value = 3;
            TblSearch.style.display = "none";
            btncross.style.display = "block";
            $('#<%=hdnMode.ClientID %>').val('0'); //Entry
            $('#<%= lblHeading.ClientID %>').text("Add TDS Journal Voucher");
            var defaultbranch = '<%=Session["userbranchID"]%>';
            $('#<%=hdnBranchId.ClientID %>').val(defaultbranch);
            var CmbScheme = document.getElementById("<%=CmbScheme.ClientID%>");
            CmbScheme.options[0].selected = true;

            // CountryID.PerformCallback(document.getElementById('ddlBranch').value);

            gridTDS.AddNewRow();
            gridTDS.batchEditApi.EndEdit();
            document.getElementById("<%=CmbScheme.ClientID%>").focus();

            cbtnSaveRecordsTDS.SetVisible(false);
            cbtn_SaveRecordsTDS.SetVisible(false);
        }



        var ValidGrid = true;
        function ValidateGrid() {

            for (var i = 0; i < 1000; i++) {
                if (grid.GetRow(i)) {
                    if (grid.GetRow(i).style.display != "none") {
                        grid.batchEditApi.StartEdit(i, 2);
                        var IsSubledger = grid.GetEditor("IsSubledger").GetText();
                        var MainAccount = grid.GetEditor("MainAccount").GetText();
                        var bthSubAccount = grid.GetEditor("bthSubAccount").GetText();

                        if ($("#HiddenSubMandatory").val() == "Yes") {
                            if (MainAccount != "" && MainAccount != null && MainAccount != 'undefined') {
                                if (IsSubledger != 'None' && IsSubledger != "") {
                                    if (bthSubAccount == "" || bthSubAccount == null || bthSubAccount == 'undefined') {
                                        ValidGrid = false;
                                    }
                                }
                            }
                        }


                    }
                }

            }

            for (var i = -1000; i < 0; i++) {
                if (grid.GetRow(i)) {
                    if (grid.GetRow(i).style.display != "none") {
                        grid.batchEditApi.StartEdit(i, 2);
                        var IsSubledger = grid.GetEditor("IsSubledger").GetText();
                        var MainAccount = grid.GetEditor("MainAccount").GetText();
                        var bthSubAccount = grid.GetEditor("bthSubAccount").GetText();

                        if ($("#HiddenSubMandatory").val() == "Yes") {
                            if (MainAccount != "" && MainAccount != null && MainAccount != 'undefined') {
                                if (IsSubledger != 'None' && IsSubledger != "") {
                                    if (bthSubAccount == "" || bthSubAccount == null || bthSubAccount == 'undefined') {
                                        ValidGrid = false;
                                    }
                                }
                            }
                        }
                    }
                }
            }

        }
        var count = 0;
        function ValidateGridTDS() {
            gridTDS.batchEditApi.EndEdit();

            count = 0;

            for (var i = 0; i < 1000; i++) {
                if (gridTDS.GetRow(i)) {
                    if (gridTDS.GetRow(i).style.display != "none") {
                        //  gridTDS.batchEditApi.StartEdit(i, 2);
                        var IsSubledger = gridTDS.GetEditor("IsSubledgerTDS").GetText();
                        var MainAccount = gridTDS.GetEditor("MainAccountTDS").GetText();
                        var bthSubAccount = gridTDS.GetEditor("bthSubAccountTDS").GetText();

                        if (MainAccount != "" && MainAccount != null && MainAccount != 'undefined') {
                            count = count + 1;
                        }

                        if ($("#HiddenSubMandatory").val() == "Yes") {
                            if (MainAccount != "" && MainAccount != null && MainAccount != 'undefined') {
                                count = count + 1;
                                if (IsSubledger != 'None' && IsSubledger != "") {
                                    if (bthSubAccount == "" || bthSubAccount == null || bthSubAccount == 'undefined') {
                                        Validgrid = false;
                                    }
                                }
                            }
                        }


                    }
                }

            }

            for (var i = -1000; i < 0; i++) {
                if (gridTDS.GetRow(i)) {
                    if (gridTDS.GetRow(i).style.display != "none") {
                        // gridTDS.batchEditApi.StartEdit(i, 2);
                        var IsSubledger = gridTDS.GetEditor("IsSubledgerTDS").GetText();
                        var MainAccount = gridTDS.GetEditor("MainAccountTDS").GetText();
                        var bthSubAccount = gridTDS.GetEditor("bthSubAccountTDS").GetText();

                        if (MainAccount != "" && MainAccount != null && MainAccount != 'undefined') {
                            count = count + 1;
                        }

                        if ($("#HiddenSubMandatory").val() == "Yes") {
                            if (MainAccount != "" && MainAccount != null && MainAccount != 'undefined') {
                                count = count + 1;
                                if (IsSubledger != 'None' && IsSubledger != "") {
                                    if (bthSubAccount == "" || bthSubAccount == null || bthSubAccount == 'undefined') {
                                        Validgrid = false;
                                    }
                                }
                            }
                        }
                    }
                }
            }




            var count = 0;
            for (var i = 0; i < 1000; i++) {
                if (gridTDS.GetRow(i)) {
                    if (gridTDS.GetRow(i).style.display != "none") {
                        //gridTDS.batchEditApi.StartEdit(i, 2);
                        var IsTDS = gridTDS.GetRow(i).children[14].children[0].innerText;


                        if (IsTDS == "1") {
                            count = count + 1;

                        }
                    }
                }

            }

            for (var i = -1000; i < 0; i++) {
                if (gridTDS.GetRow(i)) {
                    if (gridTDS.GetRow(i).style.display != "none") {
                        //gridTDS.batchEditApi.StartEdit(i, 2);
                        var IsTDS = gridTDS.GetRow(i).children[14].children[0].innerText;


                        if (IsTDS == "1") {
                            count = count + 1;
                        }
                    }
                }
            }


            if (count == 0) {
                isTDSSelected = 0;
                Validgrid = false;
            }
        }

        function GetVisibleIndex(s, e) {
            globalRowIndex = e.visibleIndex;
            // EnableOrDisableTax();
        }

        function GetVisibleIndexTDS(s, e) {
            globalRowIndexTDS = e.visibleIndex;
            // EnableOrDisableTax();
        }


        function OnlyNarration() {

        }
        function OnlyNarrationTDS() {

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

        function RcmCheckChangeTDS() {
            if (IsRcmTDS.GetChecked()) {
                var Listddl_AmountAreTDS = document.getElementById("ddl_AmountAreTDS");
                Listddl_AmountAreTDS.value = "1";
                Listddl_AmountAreTDS.disabled = true;
                var item = Listddl_AmountAreTDS.item(0);
                item.style.display = 'block';
            }
            else {
                var Listddl_AmountAreTDS = document.getElementById("ddl_AmountAreTDS");
                //Listddl_AmountAre.options.hide("Exclusive Tax");
                Listddl_AmountAreTDS.disabled = false;
                var item = Listddl_AmountAreTDS.item(0);
                item.style.display = 'none';
                Listddl_AmountAreTDS.value = "3";
            }
            // else
            //ddl_AmountAre.refresh();
        }


        function CloseSubModal() {
            $('#SubAccountModel').modal('hide');

            var updatedindex = globalRowIndex;

            if (checkIsPartyjornal()) {
                $("#divIsPartyJournal").show();
            }
            else {
                $("#divIsPartyJournal").hide();
            }


            grid.batchEditApi.StartEdit(updatedindex, 2);

        }

        function CloseSubModalTDS() {
            $('#SubAccountModelTDS').modal('hide');

            var updatedindex = globalRowIndexTDS;
            gridTDS.batchEditApi.StartEdit(updatedindex, 2);

        }

        $(document).ready(function () {
            $('#MainAccountModel').on('shown.bs.modal', function () {
                $('#txtMainAccountSearch').val("");
                $('#txtMainAccountSearch').focus();
            })
            $('#MainAccountModelTDS').on('shown.bs.modal', function () {
                $('#txtMainAccountSearchTDS').val("");
                $('#txtMainAccountSearchTDS').focus();
            })
            $('#SubAccountModel').on('shown.bs.modal', function () {
                $('#txtSubAccountSearch').val("");
                $('#txtSubAccountSearch').focus();
            })
            $('#SubAccountModelTDS').on('shown.bs.modal', function () {
                $('#txtSubAccountSearchTDS').val("");
                $('#txtSubAccountSearchTDS').focus();
            })
            $('#SubAccountModel').on('hide.bs.modal', function () {

                var updatedindex = globalRowIndex;

                if (checkIsPartyjornal()) {
                    $("#divIsPartyJournal").show();
                }
                else {
                    $("#divIsPartyJournal").hide();
                }


                grid.batchEditApi.StartEdit(updatedindex, 2);



                //grid.batchEditApi.StartEdit(globalRowIndex, 2);
            })
            $('#SubAccountModelTDS').on('hide.bs.modal', function () {

                var updatedindex = globalRowIndexTDS;

                gridTDS.batchEditApi.StartEdit(updatedindex, 2);



                //grid.batchEditApi.StartEdit(globalRowIndex, 2);
            })
        });

        function closeModal() {

            var updatedindex = globalRowIndex;
            var MainAccountID = grid.GetEditor("gvMainAcCode").GetValue();
            if (MainAccountID == 'Customers' || MainAccountID == 'Vendors') {
                if ($("#hdnIsPartyLedger").val() == "") {
                    $("#hdnIsPartyLedger").val('1');
                }
                else {
                    $("#hdnIsPartyLedger").val(parseFloat($("#hdnIsPartyLedger").val()) + 1);
                }

            }
            // if (parseFloat($("#hdnIsPartyLedger").val()) > 1 && grid.GetVisibleRowsOnPage()=="2")
            if (checkIsPartyjornal()) {
                $("#divIsPartyJournal").show();
            }
            else {
                $("#divIsPartyJournal").hide();
            }
            $('#MainAccountModel').modal('hide');
            grid.batchEditApi.StartEdit(updatedindex, 1);
        }


        function closeModalTDS() {

            var updatedindex = globalRowIndex;
            var MainAccountID = gridTDS.GetEditor("gvMainAcCodeTDS").GetValue();

            $('#MainAccountModelTDS').modal('hide');
            gridTDS.batchEditApi.StartEdit(updatedindex, 1);
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

            var startDate = new Date();
            startDate = tDate.GetDate().format('yyyy-MM-dd');
            cPanellookup_GRNOverhead.PerformCallback('BindOverheadCostGrid' + '~' + startDate);
            //clookup_GRNOverhead.gridView.Refresh();"
        }

        //Add section for TDS Posting date change Tanmoy 09-12-2020
        function TDSDateChange() {

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



            var startDate = new Date();
            startDate = tDateTDS.GetDate().format('yyyy-MM-dd');
            cPanellookup_GRNOverheadTDS.PerformCallback('BindOverheadCostGridTDS' + '~' + startDate);
            //clookup_GRNOverheadTDS.gridView.Refresh();
        }


        //Add section for TDS Posting date change Tanmoy 09-12-2020 End

        //-------------- New for Pop Up -------------------------------------
        var IsSubAccount = '';
        function MainPopUpHide() {

            var MainAccountID = grid.GetEditor("gvMainAcCode").GetValue();
            if (MainAccountID == 'Customers' || MainAccountID == 'Vendors') {
                if ($("#hdnIsPartyLedger").val() == "") {
                    $("#hdnIsPartyLedger").val('1');
                }
                else {
                    $("#hdnIsPartyLedger").val(parseFloat($("#hdnIsPartyLedger").val()) + 1);
                }

            }

            //cMainAccountpopUp.Hide();
            grid.batchEditApi.StartEdit(globalRowIndex, 1);
        }
        function SubPopUpHide() {
            //cSubAcountComboBox.Hide();
            grid.batchEditApi.StartEdit(globalRowIndex, 2);
        }

        var LastCr = 0.00;
        var LastDr = 0.00;

        function MainAccountButnClick(s, e) {

            var txt = "<table border='1' width=\"100%\"><tr class=\"HeaderStyle\"><th>Main Account Name</th><th>Short Name</th><th>Subledger Type</th></tr><table>";
            document.getElementById("MainAccountTable").innerHTML = txt;

            if (e.buttonIndex == 0) {
                $('#mainActMsg').hide();
                var FullName = new Array("", "");

                var MainAccountID = grid.GetEditor("gvMainAcCode").GetValue();
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
                grid.batchEditApi.StartEdit(e.visibleIndex);
                var strMainAccountID = (grid.GetEditor('MainAccount').GetText() != null) ? grid.GetEditor('MainAccount').GetText() : "0";

                LastCr = parseFloat(grid.GetEditor('Receipt').GetText());
                LastDr = parseFloat(grid.GetEditor('WithDrawl').GetText());

                if (strMainAccountID != "") {
                    var strMainAccountID = "PREVIOUS MAIN ACCOUNT :" + strMainAccountID;

                }
                // LabelMainAccount.SetText(strMainAccountID);
                $("#LabelMainAccount").val(strMainAccountID);
                ////cMainAccountpopUp.Show();
                $('#MainAccountModel').modal('show');
                //cMainAccountComboBox.Focus();

            }
        }



        function MainAccountButnClickTDS(s, e) {


            // debugger;

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







        function SubAccountButnClick(s, e) {

            //debugger;
            txt = " <table border='1' width=\"100%\"><tr class=\"HeaderStyle\"><th>Sub Account Name [Unique Id]</th><th>Sub Account Code</th></tr></table>";
            document.getElementById("SubAccountTable").innerHTML = txt;
            var SubAcc = grid.GetEditor('IsSubledger');
            IsSubAccount = SubAcc.GetText();
            $("#mainActMsgSub").hide();
            if (IsSubAccount != 'None') {
                grid.batchEditApi.StartEdit(e.visibleIndex);
                var strMainAccountID = (grid.GetEditor('MainAccount').GetText() != null) ? grid.GetEditor('MainAccount').GetText() : "0";
                var MainAccountID = (grid.GetEditor('gvColMainAccount').GetValue() != null) ? grid.GetEditor('gvColMainAccount').GetValue() : "0";

                //Add for found Main Accoun type Tanmoy
                var MainAccountType = "";
                $.ajax({
                    type: "POST",
                    url: 'JournalEntry.aspx/getMainAccountType',
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
                            document.getElementById('hdnMainAccountId').value = MainAccountID;
                            var FullName = new Array("", "");
                            //cSubAcountComboBox.AddItem(FullName, "");
                            //cSubAcountComboBox.SetValue("");
                            $('#SubAccountModel').modal('show');
                            ////cSubAcountComboBox.Show();
                            //cSubAcountComboBox.Focus();
                            var strSubLBLAccountID = (grid.GetEditor('bthSubAccount').GetText() != null) ? grid.GetEditor('bthSubAccount').GetText() : "0";
                            if (strSubLBLAccountID != "") {
                                var strSubLBLAccountID = "Previous Sub Account :" + strSubLBLAccountID;

                            }
                            // $("#LabelMainAccount").val(strSubLBLAccountID);
                        }
                    }
                }
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
                    url: 'JournalEntry.aspx/getMainAccountType',
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





        function MainAccountKeyDown(s, e) {
            if (e.htmlEvent.key == "Enter") {

                s.OnButtonClick(0);
                //$('#MainAccountModel').modal('show');
            }
            //if (e.htmlEvent.key == "Tab") {

            //    s.OnButtonClick(0);
            //}
        }

        function MainAccountKeyDownTDS(s, e) {
            if (e.htmlEvent.key == "Enter") {

                s.OnButtonClick(0);
                //$('#MainAccountModel').modal('show');
            }
            //if (e.htmlEvent.key == "Tab") {

            //    s.OnButtonClick(0);
            //}
        }


        function SubAccountKeyDown(s, e) {
            $("#mainActMsgSub").hide();
            if (e.htmlEvent.key == "Enter") {

                s.OnButtonClick(0);
            }
            if (e.htmlEvent.key == "Delete") {

                var subAccountText = "";
                var subAccountID = "";
                grid.batchEditApi.StartEdit(globalRowIndex, 3);

                grid.GetEditor("bthSubAccount").SetText(subAccountText);
                grid.GetEditor("gvColSubAccount").SetText(subAccountID);
                //cSubAcountComboBox.Hide();
                setTimeout(function () { grid.batchEditApi.StartEdit(globalRowIndex, 2); }, 500);
            }


            // if (e.htmlEvent.key == "Tab") {

            //   s.OnButtonClick(0);
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






        function MainAccountComboBoxKeyDown(s, e) {
            if (e.htmlEvent.key == "Escape") {


                var MainAccountID = grid.GetEditor("gvMainAcCode").GetValue();
                if (MainAccountID == 'Customers' || MainAccountID == 'Vendors') {
                    if ($("#hdnIsPartyLedger").val() == "") {
                        $("#hdnIsPartyLedger").val('1');
                    }
                    else {
                        $("#hdnIsPartyLedger").val(parseFloat($("#hdnIsPartyLedger").val()) + 1);
                    }

                }


                ////cMainAccountpopUp.Hide();
                grid.batchEditApi.StartEdit(globalRowIndex, 1);
            }
            //if (e.htmlEvent.key == "Enter") {
            //    var MainAccountText = //cMainAccountComboBox.GetText();
            //    console.log(MainAccountText,'next');



            //    //if (!//cMainAccountComboBox.FindItemByText(MainAccountText) && MainAccountText != "") {

            //    //    jAlert("Main Account does not Exist.");
            //    //    //cMainAccountComboBox.SetText("");
            //    //    return;
            //    //} 

            //}
        }

        function SubAccountComboBoxKeyDown(s, e) {
            if (e.htmlEvent.key == "Escape") {
                //cSubAcountComboBox.Hide();
                grid.batchEditApi.StartEdit(globalRowIndex, 2);
            }
            if (e.htmlEvent.key == "Enter") {
                GetSubAcountComboBox(e);
            }
        }

        var shouldCheck = 0;

        function GetMainAcountComboBox(Id, Name, Code, IsSub) {
            $('#mainActMsg').hide();

            //var MainAccountText = //cMainAccountComboBox.GetText();
            var MainAccountText = Name;
            console.log(MainAccountText, shouldCheck);

            if (shouldCheck != 1) {
                return;
            }


            //if (!//cMainAccountComboBox.FindItemByText(MainAccountText)) {
            //    //  jAlert("Main Account does not Exist.", 'Alert', function () { shouldCheck = 1; //cMainAccountComboBox.Focus(); });
            //    $('#mainActMsg').show();
            //    shouldCheck = 1;
            //    ////cMainAccountComboBox.SetText("");
            //    //  return;
            //} else {

            //if (e.keyCode == 27)//escape 
            //{
            //    grid.batchEditApi.StartEdit(globalRowIndex, 1);
            //    return;
            //}
            //cMainAccountpopUp.Hide();


            var MainAccountID = Id;
            //var ReverseApplicable = //cMainAccountComboBox.GetSelectedItem().texts[2];
            //var TaxApplicable = //cMainAccountComboBox.GetSelectedItem().texts[3];
            //var MainAcCode = //cMainAccountComboBox.GetSelectedItem().texts[4];
            var MainAcCode = Code;
            // IsSubAccount = //cMainAccountComboBox.GetSelectedItem().texts[1];
            IsSubAccount = IsSub;

            // grid.batchEditApi.StartEdit(globalRowIndex);
            grid.batchEditApi.StartEdit(globalRowIndex, 2);
            grid.GetEditor("IsSubledger").SetText(IsSubAccount);
            grid.GetEditor("MainAccount").SetText(MainAccountText);
            grid.GetEditor("gvColMainAccount").SetText(MainAccountID);
            grid.GetEditor("gvMainAcCode").SetValue(IsSub);
            // grid.GetEditor("ReverseApplicable").SetValue(ReverseApplicable);
            shouldCheck = 0;//
            grid.GetEditor("bthSubAccount").SetValue("");
            grid.GetEditor("Receipt").SetValue("");
            grid.GetEditor("WithDrawl").SetValue("");
            grid.GetEditor("gvColSubAccount").SetValue("");

            // clookup_Project.SetEnabled(false);

            if (LastDr != 0)
                c_txt_Debit.SetValue(DecimalRoundoff(parseFloat(c_txt_Debit.GetValue()) - parseFloat(LastDr), 2));
            if (LastCr != 0)
                c_txt_Credit.SetValue(DecimalRoundoff(parseFloat(c_txt_Credit.GetValue()) - parseFloat(LastCr), 2));

            var Debit = parseFloat(c_txt_Debit.GetValue());
            var Credit = parseFloat(c_txt_Credit.GetValue());

            if (Debit == 0 && Credit == 0) {
                cbtnSaveRecords.SetVisible(false);
                cbtn_SaveRecords.SetVisible(false);

                var Amount = parseFloat(Debit) - parseFloat(Credit);
                var div = document.getElementById('loadCurrencyMassage');
                var txt = "<label><span style='color: red; font-weight: bold; font-size: medium;'>**  Mismatch detected.</span></label>";
                div.innerHTML = txt;

                loadCurrencyMassage.style.display = "block";
            }
            else if (Debit == Credit) {
                cbtnSaveRecords.SetVisible(true);
                cbtn_SaveRecords.SetVisible(true);
                loadCurrencyMassage.style.display = "none";
            }
            else if (Debit != Credit) {
                cbtnSaveRecords.SetVisible(false);
                cbtn_SaveRecords.SetVisible(false);

                var Amount = parseFloat(Debit) - parseFloat(Credit);
                var div = document.getElementById('loadCurrencyMassage');
                var txt = "<label><span style='color: red; font-weight: bold; font-size: medium;'>**  Mismatch detected. Amount : " + DecimalRoundoff(Amount, 2) + "</span></label>";
                div.innerHTML = txt;
                loadCurrencyMassage.style.display = "block";
            }


            if ($("#cpTagged").val() == "-99") {
                cbtnSaveRecords.SetVisible(false);
                cbtn_SaveRecords.SetVisible(false);

            }

            LastDr = 0.00;
            LastCr = 0.00;




            if (IsSub == 'Customers' || IsSub == 'Vendors') {
                if ($("#hdnIsPartyLedger").val() == "") {
                    $("#hdnIsPartyLedger").val('1');
                }
                else {
                    $("#hdnIsPartyLedger").val(parseFloat($("#hdnIsPartyLedger").val()) + 1);
                }

            }

            //if (parseFloat($("#hdnIsPartyLedger").val()) > 1 && grid.GetVisibleRowsOnPage()=="2") 
            if (checkIsPartyjornal()) {
                $("#divIsPartyJournal").show();
            }
            else {
                $("#divIsPartyJournal").hide();
            }

            grid.batchEditApi.StartEdit(globalRowIndex, 2);
            //cddl_AmountAre.SetEnabled(false);
            //$("#IsTaxApplicable").val(TaxApplicable);
            //var VoucherType = document.getElementById('rbtnType').value;
            //if (ReverseApplicable == "1" && VoucherType == "P") {
            //    $("#chk_reversemechenism").prop("disabled", false);
            //    $("#chk_reversemechenism").prop("checked", true);
            //}
            //else {
            //    if ($("#chk_reversemechenism").prop('checked') == false) {
            //        $("#chk_reversemechenism").prop("checked", false);
            //    }
            //}
            //}

            if ($("#hdnProjectSelectInEntryModule").val() == "1") {
                if ((clookup_Project.GetGridView().GetRowKey(clookup_Project.GetGridView().GetFocusedRowIndex()) != null)) {

                    var GlobeProj = clookup_Project.GetValue();
                    var ProjectLookUpData = clookup_Project.GetGridView().GetRowKey(clookup_Project.GetGridView().GetFocusedRowIndex());
                    grid.GetEditor("Project_Code").SetText(GlobeProj);
                    grid.GetEditor("ProjectId").SetText(ProjectLookUpData);
                }
            }

        }


        function GetMainAcountComboBoxTDS(Id, Name, Code, IsSub, IsTDS) {

            gridTDS.batchEditApi.StartEdit(globalRowIndexTDS, 2);
            var uniqueindex = globalRowIndexTDS;
            var IsTDSCheck = gridTDS.GetEditor("IsTDSSource").GetValue();
            var UniqueID = gridTDS.batchEditApi.GetCellValue(globalRowIndexTDS, "UniqueID"); //gridTDS.GetEditor("gvColMainAccount").GetValue();

            chkTDSJournal.SetEnabled(false);
            ctxtTDSAmount.SetEnabled(false);
            ccmbtds.SetEnabled(false);


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
            // clookupTDS_Project.SetEnabled(false);
            if (LastDr != 0)
                c_txt_DebitTDS.SetValue(DecimalRoundoff(parseFloat(c_txt_DebitTDS.GetValue()) - parseFloat(LastDr), 2));
            if (LastCr != 0)
                c_txt_CreditTDS.SetValue(DecimalRoundoff(parseFloat(c_txt_CreditTDS.GetValue()) - parseFloat(LastCr), 2));

            var Debit = parseFloat(c_txt_DebitTDS.GetValue());
            var Credit = parseFloat(c_txt_CreditTDS.GetValue());

            if (Debit == 0 && Credit == 0) {
                cbtnSaveRecordsTDS.SetVisible(false);
                cbtn_SaveRecordsTDS.SetVisible(false);

                var Amount = parseFloat(Debit) - parseFloat(Credit);
                var div = document.getElementById('loadCurrencyMassage');
                var txt = "<label><span style='color: red; font-weight: bold; font-size: medium;'>**  Mismatch detected.</span></label>";
                div.innerHTML = txt;

                loadCurrencyMassage.style.display = "block";
            }
            else if (Debit == Credit) {
                cbtnSaveRecordsTDS.SetVisible(true);
                cbtn_SaveRecordsTDS.SetVisible(true);
                loadCurrencyMassage.style.display = "none";
            }
            else if (Debit != Credit) {
                cbtnSaveRecordsTDS.SetVisible(false);
                cbtn_SaveRecordsTDS.SetVisible(false);

                var Amount = parseFloat(Debit) - parseFloat(Credit);
                var div = document.getElementById('loadCurrencyMassage');
                var txt = "<label><span style='color: red; font-weight: bold; font-size: medium;'>**  Mismatch detected. Amount : " + DecimalRoundoff(Amount, 2) + "</span></label>";
                div.innerHTML = txt;
                loadCurrencyMassage.style.display = "block";
            }


            if ($("#cpTaggedTDS").val() == "-99") {
                cbtnSaveRecordsTDS.SetVisible(false);
                cbtn_SaveRecordsTDS.SetVisible(false);

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
                    data: "{TDSCode:\"" + IsTDS + "\",tdsdate:\"" + tDateTDS.GetText() + "\"}",
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
                            if (chkNILRateTDS.GetChecked()) {
                                gridTDS.GetEditor("TDSPercentage").SetValue("0");
                            }
                            else {
                                gridTDS.GetEditor("TDSPercentage").SetValue(response.d.trim().split('~')[2]);
                                if (response.d.trim().split('~')[2] == 0) {
                                    chkNILRateTDS.SetEnabled(false);
                                    chkNILRateTDS.SetChecked(false);
                                }
                            }

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
            if ($("#hdnProjectSelectInEntryModule").val() == "1") {
                if ((clookupTDS_Project.GetGridView().GetRowKey(clookupTDS_Project.GetGridView().GetFocusedRowIndex()) != null)) {
                    var GlobeProj = clookupTDS_Project.GetValue();
                    var ProjectLookUpData = clookupTDS_Project.GetGridView().GetRowKey(clookupTDS_Project.GetGridView().GetFocusedRowIndex());
                    gridTDS.GetEditor("Project_Code").SetText(GlobeProj);
                    gridTDS.GetEditor("ProjectId").SetText(ProjectLookUpData);
                }
            }

            chkNILRateTDS.SetEnabled(false);

            gridTDS.batchEditApi.StartEdit(uniqueindex, 2);

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













        $(function () {
            $('#MainAccountModel').modal({
                backdrop: 'static',
                keyboard: false
            });
        })


        function GetSubAcountComboBox(Id, Name) {
            //if (//cSubAcountComboBox.GetText() != "") {
            //    if (!//cSubAcountComboBox.FindItemByValue(//cSubAcountComboBox.GetValue())) {
            //        //jAlert("Sub Account does not Exist.", "Alert", function () { //cSubAcountComboBox.SetValue(); //cSubAcountComboBox.Focus(); });
            //        //return;
            //        $("#mainActMsgSub").show();
            //    }

            //    else {
            //        if (e.keyCode == 27)//escape 
            //        {
            //            grid.batchEditApi.StartEdit(globalRowIndex, 2);
            //            return;
            //        }
            //var subAccountText = //cSubAcountComboBox.GetText();



            setTimeout(function () { grid.batchEditApi.StartEdit(globalRowIndex, 3); }, 500);



            var subAccountText = Name;
            //var subAccountID = //cSubAcountComboBox.GetValue();
            //  grid.batchEditApi.StartEdit(globalRowIndex);
            var subAccountID = Id;
            grid.batchEditApi.StartEdit(globalRowIndex, 3);

            grid.GetEditor("bthSubAccount").SetText(subAccountText);
            grid.GetEditor("gvColSubAccount").SetText(subAccountID);
            //cSubAcountComboBox.Hide();
            grid.batchEditApi.StartEdit(globalRowIndex, 3);

            var updatedIndex = globalRowIndex;

            if (checkIsPartyjornal()) {
                $("#divIsPartyJournal").show();
            }
            else {
                $("#divIsPartyJournal").hide();
            }


            setTimeout(function () { grid.batchEditApi.StartEdit(updatedIndex, 3); }, 500);
            //}
            //}
        }

        function SetTDSProjectCode() {
            gridTDS.batchEditApi.StartEdit(globalRowIndexTDS, 5);
            var Project_Code = gridTDS.GetEditor("Project_Code").GetText();
            var ProjectId = gridTDS.GetEditor("ProjectId").GetText();
            var UniqueID = gridTDS.batchEditApi.GetCellValue(globalRowIndexTDS, "UniqueID");

            gridTDS.batchEditApi.EndEdit();
            for (var i = 0; i < 1000; i++) {
                if (gridTDS.GetRow(i)) {
                    if (gridTDS.GetRow(i).style.display != "none") {
                        //gridTDS.batchEditApi.StartEdit(i, 2);
                        //if (gridTDS.GetEditor("UniqueID").GetText() == uniqueID) {
                        if (gridTDS.batchEditApi.GetCellValue(i, "UniqueID") == UniqueID) {
                            gridTDS.batchEditApi.SetCellValue(i, "Project_Code", Project_Code);
                            gridTDS.batchEditApi.SetCellValue(i, "ProjectId", SubAccId);
                        }
                    }
                }
            }

            for (i = -1; i > -1000; i--) {
                if (gridTDS.GetRow(i)) {
                    if (gridTDS.GetRow(i).style.display != "none") {
                        if (gridTDS.batchEditApi.GetCellValue(i, "UniqueID") == UniqueID) {
                            gridTDS.batchEditApi.SetCellValue(i, "Project_Code", Project_Code);
                            gridTDS.batchEditApi.SetCellValue(i, "ProjectId", ProjectId);
                        }
                    }
                }
            }


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
            obj.TDSCode = TDSCode;
            obj.tdsdate = tDateTDS.GetText();


            $.ajax({
                type: "POST",
                url: 'JournalEntry.aspx/GetTDSSubLedger',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                data: JSON.stringify(obj),
                success: function (response) {

                    if (response.d != "" && response.d != null) {
                        if (chkNILRateTDS.GetChecked()) {
                            UpdateTDSRateByUniqueID(UniqueID, "0.00~0");
                        }
                        else {
                            UpdateTDSRateByUniqueID(UniqueID, response.d);
                            if (response.d == "0.00~0") {
                                chkNILRateTDS.SetEnabled(false);
                                chkNILRateTDS.SetChecked(false);
                            }
                        }

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
            gridTDS.batchEditApi.EndEdit();
            for (var i = 0; i < 1000; i++) {
                if (gridTDS.GetRow(i)) {
                    if (gridTDS.GetRow(i).style.display != "none") {
                        //gridTDS.batchEditApi.StartEdit(i, 2);
                        //if (gridTDS.GetEditor("UniqueID").GetText() == uniqueID) {
                        if (gridTDS.batchEditApi.GetCellValue(i, "UniqueID") == uniqueID) {
                            gridTDS.batchEditApi.SetCellValue(i, "bthSubAccountTDS", subAccountText);
                            gridTDS.batchEditApi.SetCellValue(i, "gvColSubAccountTDS", subAccountID);
                        }
                    }
                }
            }

            for (i = -1; i > -1000; i--) {
                if (gridTDS.GetRow(i)) {
                    if (gridTDS.GetRow(i).style.display != "none") {
                        if (gridTDS.batchEditApi.GetCellValue(i, "UniqueID") == uniqueID) {
                            gridTDS.batchEditApi.SetCellValue(i, "bthSubAccountTDS", subAccountText);
                            gridTDS.batchEditApi.SetCellValue(i, "gvColSubAccountTDS", subAccountID);
                        }
                    }
                }
            }
        }


        //-------------------------------------------------------------------------------
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

                // Rev Sayantani
                if ($('#hdnProjectSelectInEntryModule').val() == "1") {
                    VisibleIndexE = e.visibleIndex;



                    var row = s.GetRow(e.visibleIndex);
                    // Rev Sayantani
                    if (row.children[9].innerHTML == "1") {
                        jAlert('This is an Auto Journal created from the Sales(POS) module. Goto Sales(POS) module to Edit/Delete.', 'Alert');
                        return;
                    }

                    // End of Rev Sayantani
                    if (row.children[8].innerHTML != "True") {
                        $('#<%= lblHeading.ClientID %>').text("Modify Journal Voucher");
                        $('#<%=hdnMode.ClientID %>').val('1');


                        $("#tdSaveButton").hide();


                        document.getElementById('div_Edit').style.display = 'none';
                        document.getElementById('<%= txtBillNo.ClientID %>').disabled = true;
                        document.getElementById('tblBtnSavePanel').style.display = 'block';
                        document.getElementById('divAddNew').style.display = 'block';
                        btncross.style.display = "block";
                        TblSearch.style.display = "none";
                        //cGvJvSearch.GetRowValues(VisibleIndexE, "BranchID", MainAccountCallBack);

                        grid.PerformCallback('Edit~' + VisibleIndexE);
                    }
                    else {
                        $('#<%= lblHeading.ClientID %>').text("Modify TDS Journal Voucher");
                        $('#<%=hdnMode.ClientID %>').val('1');


                        $("#tdSaveButtonTDS").hide();


                        document.getElementById('div_EditTDS').style.display = 'none';
                        document.getElementById('<%= txtBillNoTDS.ClientID %>').disabled = true;
                        document.getElementById('tblBtnSavePanelTDS').style.display = 'block';
                        document.getElementById('divAddNewTDS').style.display = 'block';
                        btncross.style.display = "block";
                        TblSearch.style.display = "none";
                        gridTDS.PerformCallback('Edit~' + VisibleIndexE);

                    }
                }
                else {
                    VisibleIndexE = e.visibleIndex;



                    var row = s.GetRow(e.visibleIndex);
                    // Rev Sayantani
                    if (row.children[8].innerHTML == "1") {
                        jAlert('This is an Auto Journal created from the Sales(POS) module. Goto Sales(POS) module to Edit/Delete.', 'Alert');
                        return;
                    }

                    // End of Rev Sayantani
                    if (row.children[7].innerHTML != "True") {
                        $('#<%= lblHeading.ClientID %>').text("Modify Journal Voucher");
                        $('#<%=hdnMode.ClientID %>').val('1');


                        $("#tdSaveButton").hide();


                        document.getElementById('div_Edit').style.display = 'none';
                        document.getElementById('<%= txtBillNo.ClientID %>').disabled = true;
                        document.getElementById('tblBtnSavePanel').style.display = 'block';
                        document.getElementById('divAddNew').style.display = 'block';
                        btncross.style.display = "block";
                        TblSearch.style.display = "none";
                        //cGvJvSearch.GetRowValues(VisibleIndexE, "BranchID", MainAccountCallBack);

                        grid.PerformCallback('Edit~' + VisibleIndexE);
                    }
                    else {
                        $('#<%= lblHeading.ClientID %>').text("Modify TDS Journal Voucher");
                        $('#<%=hdnMode.ClientID %>').val('1');


                        $("#tdSaveButtonTDS").hide();


                        document.getElementById('div_EditTDS').style.display = 'none';
                        document.getElementById('<%= txtBillNoTDS.ClientID %>').disabled = true;
                        document.getElementById('tblBtnSavePanelTDS').style.display = 'block';
                        document.getElementById('divAddNewTDS').style.display = 'block';
                        btncross.style.display = "block";
                        TblSearch.style.display = "none";
                        gridTDS.PerformCallback('Edit~' + VisibleIndexE);

                    }
                }
                LoadingPanel.Show();
            }

            if (e.buttonID == 'CustomBtnView') {
                VisibleIndexE = e.visibleIndex;
                $('#<%= lblHeading.ClientID %>').text("View Journal Voucher");
                $('#<%=hdnMode.ClientID %>').val('1');
                document.getElementById('div_Edit').style.display = 'none';
                document.getElementById('<%= txtBillNo.ClientID %>').disabled = true;

                document.getElementById('divAddNew').style.display = 'block';
                btncross.style.display = "block";
                TblSearch.style.display = "none";
                //cGvJvSearch.GetRowValues(VisibleIndexE, "BranchID", MainAccountCallBack);
                grid.PerformCallback('Edit~' + VisibleIndexE);
                document.getElementById('tblBtnSavePanel').style.display = 'none';
                LoadingPanel.Show();
            }

            else if (e.buttonID == 'CustomBtnDelete') {

                VisibleIndexE = e.visibleIndex;
                var row = s.GetRow(e.visibleIndex);
                if (row.children[8].innerHTML == "1") {
                    jAlert('This journal is Auto Created from POS.Can not delete.', 'Alert');
                    return;
                }

                jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                    if (r == true) {
                        cGvJvSearch.PerformCallback('PCB_DeleteBtnOkE~' + VisibleIndexE);
                    }
                });
            }
            else if (e.buttonID == 'CustomBtnPrint') {
                var keyValueindex = e.visibleIndex;
                //jConfirm('Do you want to Print?', 'Confirmation Dialog', function (r) {
                //    if (r == true) {
                //cGvJvSearch.GetRowValues(keyValueindex, "JvID", onPrintJv)
                var keyValueindex = s.GetRowKey(e.visibleIndex);
                onPrintJv(keyValueindex);
                //            }
                //        });
                //    }
            }
        }

        function onPrint(visibleIndex) {
            var keyValueindex = visibleIndex;
            //jConfirm('Do you want to Print?', 'Confirmation Dialog', function (r) {
            //    if (r == true) {
            //cGvJvSearch.GetRowValues(keyValueindex, "JvID", onPrintJv)
            var keyValueindex = cGvJvSearch.GetRowKey(visibleIndex);
            onPrintJv(keyValueindex);
        }


        function OnView(visibleIndex) {
            VisibleIndexE = visibleIndex;
            $('#<%= lblHeading.ClientID %>').text("View Journal Voucher");
            $('#<%=hdnMode.ClientID %>').val('1');
            document.getElementById('div_Edit').style.display = 'none';
            document.getElementById('<%= txtBillNo.ClientID %>').disabled = true;

            document.getElementById('divAddNew').style.display = 'block';
            btncross.style.display = "block";
            TblSearch.style.display = "none";
            //cGvJvSearch.GetRowValues(VisibleIndexE, "BranchID", MainAccountCallBack);
            grid.PerformCallback('Edit~' + VisibleIndexE);
            document.getElementById('tblBtnSavePanel').style.display = 'none';
            LoadingPanel.Show();
        }

        var NillTDS = true;

        function IsNillTDSCheck(val) {
            NillTDS = true;
            $.ajax({
                type: "POST",
                url: "JournalEntry.aspx/IsNillTDSCheck",
                data: JSON.stringify({ ID: val }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {
                    var status = msg.d;
                    if (status == "True") {
                        NillTDS = false;
                    }
                    else {
                        NillTDS = true;
                    }
                }
            });
        }

        function OnEdit(visibleIndex, IStds, Visible, RET_VISIBLE, JvID) {
            // debugger;
            VisibleIndexE = visibleIndex;
            // var row = s.GetRow(e.visibleIndex);
            //rev for Nil Rated TDS checking for Edit Tanmoy
            IsNillTDSCheck(JvID);
            if (NillTDS) {
                //End of rev for Nil Rated TDS checking for Edit Tanmoy
                // Rev Sayantani
                if (Visible == "1") {
                    // jAlert('This is an Auto Journal created from the Sales(POS) module. Goto Sales(POS) module to Edit/Delete.', 'Alert');
                    jAlert('TDS Challan is already done against this Document. Click on View to View this Document.', 'Alert');

                    return;
                }


                if (RET_VISIBLE == "1") {
                    jAlert('This is an Auto Journal created from the retention module.Can not edit.', 'Alert');
                    return;
                }

                // End of Rev Sayantani
                if (IStds != "True") {
                    $('#<%= lblHeading.ClientID %>').text("Modify Journal Voucher");
                    $('#<%=hdnMode.ClientID %>').val('1');


                    $("#tdSaveButton").hide();


                    document.getElementById('div_Edit').style.display = 'none';
                    document.getElementById('<%= txtBillNo.ClientID %>').disabled = true;
                    document.getElementById('tblBtnSavePanel').style.display = 'block';
                    document.getElementById('divAddNew').style.display = 'block';
                    btncross.style.display = "block";
                    TblSearch.style.display = "none";
                    //cGvJvSearch.GetRowValues(VisibleIndexE, "BranchID", MainAccountCallBack);

                    grid.PerformCallback('Edit~' + VisibleIndexE);
                }
                else {
                    $('#<%= lblHeading.ClientID %>').text("Modify TDS Journal Voucher");
                    $('#<%=hdnMode.ClientID %>').val('1');


                    $("#tdSaveButtonTDS").hide();


                    document.getElementById('div_EditTDS').style.display = 'none';
                    document.getElementById('<%= txtBillNoTDS.ClientID %>').disabled = true;
                    document.getElementById('tblBtnSavePanelTDS').style.display = 'block';
                    document.getElementById('divAddNewTDS').style.display = 'block';
                    btncross.style.display = "block";
                    TblSearch.style.display = "none";
                    gridTDS.PerformCallback('Edit~' + VisibleIndexE);

                }
            }
            else {
                jAlert('This Journal is NIL TDS. Can not modify.', 'Alert');
                return;
            }
            //clookupTDS_Project.SetEnabled(false);
            // clookup_Project.SetEnabled(false);
            LoadingPanel.Show();
        }

        function OnGetRowValuesOnDelete(visibleIndex, IStds, RET_VISIBLE, JvID) {
            VisibleIndexE = visibleIndex;
            //var row = s.GetRow(e.visibleIndex);
            //rev for Nil Rated TDS checking for Edit Tanmoy
            IsNillTDSCheck(JvID);
            if (NillTDS) {
                //End of rev for Nil Rated TDS checking for Edit Tanmoy
                if (IStds == "1") {
                    jAlert('This journal is Auto Created from POS.Can not delete.', 'Alert');
                    return;
                }

                if (RET_VISIBLE == "1") {
                    jAlert('This is an Auto Journal created from the retention module.Can not delete.', 'Alert');
                    return;
                }

                jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                    if (r == true) {
                        cGvJvSearch.PerformCallback('PCB_DeleteBtnOkE~' + VisibleIndexE);
                    }
                });
            }
            else {
                jAlert('This Journal is NIL TDS. Can not delete.', 'Alert');
                return;
            }
        }


        function onPrintJv(id) {

            RecPayId = id;
            cDocumentsPopup.Show();
            cSelectPanel.cpSuccess = "";
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

            if (cSelectPanel.cpSuccess != "") {
                var TotDocument = cSelectPanel.cpSuccess.split(',');
                var reportName = cCmbDesignName.GetValue();
                var module = 'JOURNALVOUCHER';
                window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + RecPayId, '_blank')
            }
            if (cSelectPanel.cpSuccess == "") {
                cCmbDesignName.SetSelectedIndex(0);
            }
        }




        //function onPrintJv(id) {
        //    window.location.href = "../../reports/XtraReports/JournalVoucherReportViewer.aspx?id=" + id;
        //}

        function MainAccountCallBack(branch) {
            // CountryID.PerformCallback(branch);
        }

        function GvJvSearch_EndCallBack() {
            if (cGvJvSearch.cpJVDelete != undefined && cGvJvSearch.cpJVDelete != null) {
                jAlert(cGvJvSearch.cpJVDelete);
                cGvJvSearch.cpJVDelete = null;
                //rev 2.0
                // updateGridByDate()
                //cGvJvSearch.PerformCallback('PCB_BindAfterDelete');
                cGvJvSearch.Refresh();
                //rev 2.0 END
            }
        }
        function GridFullInfo_EndCallBack() {
            if (cGvJvSearch.cpJVDelete != undefined && cGvJvSearch.cpJVDelete != null) {
                jAlert(cGvJvSearch.cpJVDelete);
                cGvJvSearch.cpJVDelete = null;
                cGvJvSearch.PerformCallback('PCB_BindAfterDelete');
            }
        }


        var currentEditableVisibleIndex;
        var currentEditableVisibleIndexTDS;


        var preventEndEditOnLostFocus = false;
        var lastCountryID;
        var setValueFlag;
        var setValueFlagTDS;


        var debitOldValue;
        var debitNewValue;
        var CreditOldValue;
        var CreditNewValue;

        function CountriesCombo_SelectedIndexChanged(s, e) {
            var currentValue = grid.GetEditor('gvColMainAccount').GetValue();
            var Narration = grid.GetEditor('Narration');
            var currentValue = s.GetValue();
            if (lastCountryID == currentValue) {
                Narration.SetValue(NarrationText);
                return;
            }
            lastCountryID = currentValue;
            //CityID.PerformCallback(currentValue + '~' + "");
        }
        function IntializeGlobalVariables(grid) {
            lastCountryID = grid.cplastCountryID;
            currentEditableVisibleIndex = -1;
            setValueFlag = -1;
        }
        function OnInit(s, e) {
            IntializeGlobalVariables(s);

        }
        function OnInitTDS(s, e) {
            IntializeGlobalVariablesTDS(s);

        }
        function IntializeGlobalVariablesTDS(gridTDS) {
            lastCountryID = gridTDS.cplastCountryID;
            currentEditableVisibleIndexTDS = -1;
            setValueFlagTDS = -1;
        }


        function OnEndCallback(s, e) {

            //IntializeGlobalVariables(s);
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
                var PlaceOfSupply = grid.cpEdit.split('~')[6];
                var TaxOption = grid.cpEdit.split('~')[7];
                var IsPartyJournal = grid.cpEdit.split('~')[8];
                var PartyCount = grid.cpEdit.split('~')[9];
                var IsRCMD = grid.cpEdit.split('~')[10];
                var projJId = grid.cpEdit.split('~')[11];
                var Transaction_Date = grid.cpEdit.split('~')[12];
                var GRN_IDs = grid.cpEdit.split('~')[13];
                $("#hdnIsPartyLedger").val(PartyCount);
                if (IsPartyJournal == 'True') {
                    $("#divIsPartyJournal").show();
                }

                if (IsRCMD == 'True') {
                    IsRcm.SetChecked(1);
                }
                else {
                    IsRcm.SetChecked(0);
                }
                cPanellookup_GRNOverhead.PerformCallback('BindOverheadCostGridEdit' + '~' + Transaction_Date + '~' + BranchID + '~' + GRN_IDs);

                document.getElementById('txtBillNo').value = VoucherNo;
                document.getElementById('txtNarration').value = Narration;
                document.getElementById('ddlSupplyState').value = PlaceOfSupply;
                document.getElementById('ddl_AmountAre').value = TaxOption;
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
                document.getElementById('<%= ddlBranch.ClientID %>').disabled = true;
                var Transdt = new Date(trDate);
                tDate.SetDate(Transdt);

                //Bind again the main account with respect to branch
                // CountryID.PerformCallback(BranchID);
                var strSchemaType = document.getElementById('hdnSchemaType').value;
                var RefreshType = document.getElementById('hdnRefreshType').value;

                c_txt_Credit.SetValue(Credit);
                c_txt_Debit.SetValue(Debit);

                if (Debit == Credit) {
                    cbtnSaveRecords.SetVisible(true);
                    cbtn_SaveRecords.SetVisible(true);
                    loadCurrencyMassage.style.display = "none";
                }
                else {
                    cbtnSaveRecords.SetVisible(false);
                    cbtn_SaveRecords.SetVisible(false);

                    var Amount = parseFloat(Debit) - parseFloat(Credit);
                    var div = document.getElementById('loadCurrencyMassage');
                    var txt = "<label><span style='color: red; font-weight: bold; font-size: medium;'>**  Mismatch detected. Amount : " + DecimalRoundoff(Amount, 2) + "</span></label>";
                    div.innerHTML = txt;

                    loadCurrencyMassage.style.display = "block";
                }

                if (grid.cpCheck == "-99") {
                    $("#cpTagged").val("-99");
                    $("#tdSaveButton").show();
                    $("#btnSaveRecords").hide()
                    $("#btn_SaveRecords").hide();
                    $('#<%=lbl_quotestatusmsg.ClientID %>').html("Tagged with another module. Cannot modify.");
                    //Rev 6.0
                    cbtnSaveRecords.SetVisible(true);
                    cbtn_SaveRecords.SetVisible(true);
                    //Rev 6.0 End
                }

                if (IsRcm.GetChecked()) {
                    var Listddl_AmountAre = document.getElementById("ddl_AmountAre");
                    Listddl_AmountAre.value = "1";
                    Listddl_AmountAre.disabled = true;
                    var item = Listddl_AmountAre.item(0);
                    item.style.display = 'block';
                }
                else {
                    var Listddl_AmountAre = document.getElementById("ddl_AmountAre");
                    Listddl_AmountAre.disabled = false;
                    var item = Listddl_AmountAre.item(0);
                    item.style.display = 'none';

                }

                // if($("#hdnProjectSelectInEntryModule").val()=="1")
                clookup_Project.gridView.SelectItemsByKey(projJId);

                var projID = clookup_Project.GetValue();
                $.ajax({
                    type: "POST",
                    url: 'JournalEntry.aspx/getHierarchyID',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({ ProjID: projID }),
                    success: function (msg) {
                        var data = msg.d;
                        $("#ddlHierarchy").val(data);
                    }
                });

                grid.cpEdit = null;
            }

            var value = document.getElementById('hdnRefreshType').value;

            if (grid.cpSaveSuccessOrFail == "outrange") {
                jAlert('Can Not Add More Journal Voucher as Journal Scheme Exausted.<br />Update The Scheme and Try Again');
                //Rev 6.0
                if ($("#hdnMode").val() == "0") {
                    cbtnSaveRecords.SetVisible(true);
                }
                cbtn_SaveRecords.SetVisible(true);
                //Rev 6.0 End
            }
            else if (grid.cpSaveSuccessOrFail == "duplicate") {
                jAlert('Can Not Save as Duplicate Journal Voucher No. Found');
                //Rev 6.0
                if ($("#hdnMode").val() == "0") {
                    cbtnSaveRecords.SetVisible(true);
                }
                cbtn_SaveRecords.SetVisible(true);
                //Rev 6.0 End
            }
            else if (grid.cpSaveSuccessOrFail == "Subaccountmandatory") {
                jAlert('Sub account set as mandatory. Please Select Sub Account to Proceed.');
                //Rev 6.0
                if ($("#hdnMode").val() == "0") {
                    cbtnSaveRecords.SetVisible(true);
                }
                cbtn_SaveRecords.SetVisible(true);
                //Rev 6.0 End
                return;
            }
            else if (grid.cpSaveSuccessOrFail == "errorInsert") {
                jAlert('Try again later.');
                //Rev 6.0
                if ($("#hdnMode").val() == "0") {
                    cbtnSaveRecords.SetVisible(true);
                }
                cbtn_SaveRecords.SetVisible(true);
                //Rev 6.0 End
            }
            else if (grid.cpSaveSuccessOrFail == "HasError") {
                jAlert('Selected Ledgers are not mapped with RCM Ledger in Masters - Accounts - Tax Component Scheme. Cannot Proceed.');
                //Rev 6.0
                if ($("#hdnMode").val() == "0") {
                    cbtnSaveRecords.SetVisible(true);
                }
                cbtn_SaveRecords.SetVisible(true);
                //Rev 6.0 End
                for (var i = 0; i <= grid.GetVisibleRowsOnPage(); i++) {
                    grid.batchEditApi.StartEdit(i, 1);
                }


                grid.AddNewRow();
            }
            else if (grid.cpSaveSuccessOrFail == "successInsert") {
                //Rev 6.0
                if ($("#hdnMode").val() == "0") {
                    cbtnSaveRecords.SetVisible(true);
                }
                cbtn_SaveRecords.SetVisible(true);
                //Rev 6.0 End
                $("#divIsPartyJournal").hide();
                var JV_Number = grid.cpVouvherNo;
                var JV_Msg = "Journal Voucher No. " + JV_Number + " generated.";
                var strSchemaType = document.getElementById('hdnSchemaType').value;
                var AutoPrint = document.getElementById('hdnAutoPrint').value;

                if (value == "E") {
                    var IsComplete = "0";

                    if (JV_Number != "") {
                        if (AutoPrint == "Yes") {
                            var reportName = 'JournalVoucher~D'
                            var module = 'JOURNALVOUCHER'
                            window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + JV_Number, '_blank')
                        }
                        jAlert(JV_Msg, 'Alert Dialog: [Journal Voucher]', function (r) {
                            if (r == true) {
                                window.location.reload();
                            }
                        });
                    } else {
                        window.location.reload();
                    }
                }
                else if (value == "S") {
                    var IsComp = "0";

                    if (JV_Number != "") {
                        if (AutoPrint == "Yes") {
                            var reportName = 'JournalVoucher~D'
                            var module = 'JOURNALVOUCHER'
                            window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + JV_Number, '_blank')
                        }
                        jAlert(JV_Msg, 'Alert Dialog: [Journal Voucher]', function (r) {
                            if (r == true) {
                                IsComp = "1";
                            }
                        });
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

                        $('#<%= lblHeading.ClientID %>').text("Add Journal Voucher");

                        cbtnSaveRecords.SetVisible(false);
                        cbtn_SaveRecords.SetVisible(false);


                        var Amount = parseFloat(Debit) - parseFloat(Credit);
                        var div = document.getElementById('loadCurrencyMassage');
                        var txt = "<label><span style='color: red; font-weight: bold; font-size: medium;'>**  Mismatch detected.</span></label>";
                        div.innerHTML = txt;

                        loadCurrencyMassage.style.display = "block";

                        document.getElementById('div_Edit').style.display = 'block';
                        document.getElementById('<%= txtBillNo.ClientID %>').disabled = true;
                        //cCmbScheme.SetValue("0");
                        document.getElementById('<%= txtBillNo.ClientID %>').value = "";
                        document.getElementById('<%= txtBillNo.ClientID %>').disabled = true;
                        c_txt_Debit.SetValue("0");
                        c_txt_Credit.SetValue("0");
                        document.getElementById('<%= txtNarration.ClientID %>').value = "";
                        //cCmbScheme.Focus();

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
                    else {
                        grid.AddNewRow();
                    }
                }
            }
            else {
                grid.AddNewRow();
            }

        }



        function OnEndCallbackTDS(s, e) {
            //IntializeGlobalVariables(s);
            LoadingPanel.Hide();
            gridTDS.cpNewRowIndex = undefined;
            if (gridTDS.cpEdit != null) {
                //gridTDS.ShowLoadingPanel();
                //LoadingPanel.Show();

                var VoucherNo = gridTDS.cpEdit.split('~')[0];
                var Narration = gridTDS.cpEdit.split('~')[1];
                var BranchID = gridTDS.cpEdit.split('~')[2];
                var Credit = gridTDS.cpEdit.split('~')[3];
                var Debit = gridTDS.cpEdit.split('~')[4];
                var trDate = gridTDS.cpEdit.split('~')[5];
                var PlaceOfSupply = gridTDS.cpEdit.split('~')[6];
                var TaxOption = gridTDS.cpEdit.split('~')[7];
                var IsPartyJournal = gridTDS.cpEdit.split('~')[8];
                var PartyCount = gridTDS.cpEdit.split('~')[9];
                var IsRCMD = gridTDS.cpEdit.split('~')[10];
                var projJId = gridTDS.cpEdit.split('~')[11];
                var IsSalary = gridTDS.cpEdit.split('~')[12];


                var CONSIDERTDS = gridTDS.cpEdit.split('~')[13];
                var TDS_CODE = gridTDS.cpEdit.split('~')[14];
                var TDS_Amount = gridTDS.cpEdit.split('~')[15];

                var Transaction_Date = gridTDS.cpEdit.split('~')[16];
                var GRN_IDs = gridTDS.cpEdit.split('~')[17];
                //Add section for Nil TDS Tanmoy
                var NILRateTDS = gridTDS.cpEdit.split('~')[18];

                cPanellookup_GRNOverheadTDS.PerformCallback('BindOverheadCostGridEditTDS' + '~' + Transaction_Date + '~' + BranchID + '~' + GRN_IDs);

                ccmbtds.SetValue(TDS_CODE);
                ccmbtds.SetEnabled(false);

                //Add section for Nil TDS Tanmoy Start
                if (NILRateTDS == 'True') {
                    //$("#chkNILRateTDS").prop("checked", true);
                    chkNILRateTDS.SetChecked(true);
                }
                else {
                    //$("#chkNILRateTDS").prop("checked", false);
                    chkNILRateTDS.SetChecked(false);
                }

                if (NILRateTDS == 'False') {
                    ctxtTDSAmount.SetValue(TDS_Amount);
                }
                //Add section for Nil TDS Tanmoy End
                ctxtTDSAmount.SetEnabled(false);


                if (IsSalary == "True") {
                    chkIsSalary.SetChecked(true);
                }
                else {
                    chkIsSalary.SetChecked(false);
                }


                if (CONSIDERTDS == "True") {
                    chkTDSJournal.SetChecked(true);
                }
                else {
                    chkTDSJournal.SetChecked(false);
                }



                chkTDSJournal.SetEnabled(false);


                $("#hdnIsPartyLedger").val(PartyCount);
                if (IsPartyJournal == 'True') {
                    $("#divIsPartyJournal").show();
                }

                if (IsRCMD == 'True') {
                    IsRcmTDS.SetChecked(1);
                }
                else {
                    IsRcmTDS.SetChecked(0);
                }

                console.log(IsSalary);

                document.getElementById('txtBillNoTDS').value = VoucherNo;
                document.getElementById('txtNarrationTDS').value = Narration;
                document.getElementById('ddlSupplyStateTDS').value = PlaceOfSupply;
                document.getElementById('ddl_AmountAreTDS').value = TaxOption;



                document.getElementById('ddlBranchTDS').value = BranchID;
                document.getElementById('<%= ddlBranchTDS.ClientID %>').disabled = true;
                var Transdt = new Date(trDate);
                tDateTDS.SetDate(Transdt);

                //Bind again the main account with respect to branch
                // CountryID.PerformCallback(BranchID);
                var strSchemaType = document.getElementById('hdnSchemaTypeTDS').value;
                var RefreshType = document.getElementById('hdnRefreshType').value;

                c_txt_CreditTDS.SetValue(Credit);
                c_txt_DebitTDS.SetValue(Debit);

                if (Debit == Credit) {
                    cbtnSaveRecordsTDS.SetVisible(true);
                    cbtn_SaveRecordsTDS.SetVisible(true);
                    loadCurrencyMassage.style.display = "none";
                }
                else {
                    cbtnSaveRecordsTDS.SetVisible(false);
                    cbtn_SaveRecordsTDS.SetVisible(false);

                    var Amount = parseFloat(Debit) - parseFloat(Credit);
                    var div = document.getElementById('loadCurrencyMassage');
                    var txt = "<label><span style='color: red; font-weight: bold; font-size: medium;'>**  Mismatch detected. Amount : " + DecimalRoundoff(Amount, 2) + "</span></label>";
                    div.innerHTML = txt;

                    loadCurrencyMassage.style.display = "block";
                }

                if (gridTDS.cpCheck == "-99") {
                    $("#cpTaggedTDS").val("-99");
                    $("#tdSaveButtonTDS").show();
                    $("#btnSaveRecordsTDS").hide()
                    $("#btn_SaveRecordsTDS").hide();
                    $('#<%=lbl_quotestatusmsgTDS.ClientID %>').html("Tagged with another module. Cannot modify.");

                }



                if (IsRcmTDS.GetChecked()) {
                    var Listddl_AmountAre = document.getElementById("ddl_AmountAreTDS");
                    Listddl_AmountAre.value = "1";
                    Listddl_AmountAre.disabled = true;
                    var item = Listddl_AmountAre.item(0);
                    item.style.display = 'block';
                }
                else {
                    var Listddl_AmountAre = document.getElementById("ddl_AmountAreTDS");
                    Listddl_AmountAre.disabled = false;
                    var item = Listddl_AmountAre.item(0);
                    item.style.display = 'none';

                }

                UpdateTrColor();
                //if ($("#hdnProjectSelectInEntryModule").val() == "1")
                clookupTDS_Project.gridView.SelectItemsByKey(projJId);

                var projID = clookupTDS_Project.GetValue();
                $.ajax({
                    type: "POST",
                    url: 'JournalEntry.aspx/getHierarchyID',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({ ProjID: projID }),
                    success: function (msg) {
                        var data = msg.d;
                        $("#ddlHierarchyTDS").val(data);
                    }
                });

                gridTDS.cpEdit = null;
            }

            var value = document.getElementById('hdnRefreshType').value;

            if (gridTDS.cpSaveSuccessOrFail == "outrange") {
                jAlert('Can Not Add More Journal Voucher as Journal Scheme Exausted.<br />Update The Scheme and Try Again');
                //Rev 6.0
                if ($("#hdnMode").val() == "0") {
                    cbtnSaveRecordsTDS.SetVisible(true);
                }
                cbtn_SaveRecordsTDS.SetVisible(true);
                //Rev 6.0 End
            }
            else if (gridTDS.cpSaveSuccessOrFail == "duplicate") {
                jAlert('Can Not Save as Duplicate Journal Voucher No. Found');
                //Rev 6.0
                if ($("#hdnMode").val() == "0") {
                    cbtnSaveRecordsTDS.SetVisible(true);
                }
                cbtn_SaveRecordsTDS.SetVisible(true);
                //Rev 6.0 End
            }
            else if (gridTDS.cpSaveSuccessOrFail == "Subaccountmandatory") {
                jAlert('Sub account set as mandatory. Please Select Sub Account to Proceed.');
                //Rev 6.0
                if ($("#hdnMode").val() == "0") {
                    cbtnSaveRecordsTDS.SetVisible(true);
                }
                cbtn_SaveRecordsTDS.SetVisible(true);
                //Rev 6.0 End
                return;
            }
            else if (gridTDS.cpSaveSuccessOrFail == "InValidTDS") {
                jAlert('Please select only one TDS ledger to Proceed.');
                //Rev 6.0
                if ($("#hdnMode").val() == "0") {
                    cbtnSaveRecordsTDS.SetVisible(true);
                }
                cbtn_SaveRecordsTDS.SetVisible(true);
                //Rev 6.0 End
                return;
            }

            else if (gridTDS.cpSaveSuccessOrFail == "errorInsert") {
                jAlert('Try again later.');
                //Rev 6.0
                if ($("#hdnMode").val() == "0") {
                    cbtnSaveRecordsTDS.SetVisible(true);
                }
                cbtn_SaveRecordsTDS.SetVisible(true);
                //Rev 6.0 End
            }
            else if (gridTDS.cpSaveSuccessOrFail == "HasError") {
                jAlert('Selected Ledgers are not mapped with RCM Ledger in Masters - Accounts - Tax Component Scheme. Cannot Proceed.');
                //Rev 6.0
                if ($("#hdnMode").val() == "0") {
                    cbtnSaveRecordsTDS.SetVisible(true);
                }
                cbtn_SaveRecordsTDS.SetVisible(true);
                //Rev 6.0 End
                for (var i = 0; i <= gridTDS.GetVisibleRowsOnPage(); i++) {
                    gridTDS.batchEditApi.StartEdit(i, 1);
                }


                gridTDS.AddNewRow();
            }
            //else if (grid.cpNilTDSCheckZeroAmt == "Faild") {
            //    jConfirm('This is a NIL TDS Journal, value should be zero against ', 'Confirmation Dialog', function (r) {
            //        if (r == true) {

            //        }
            //    });
            //}
            //else if (grid.cpNilTDSChecknotZeroAmt == "Faild") {
            //    jConfirm('This is a TDS Journal, value should be non-zero against', 'Confirmation Dialog', function (r) {
            //        if (r == true) {

            //        }
            //    });
            //}
            else if (gridTDS.cpSaveSuccessOrFail == "successInsert") {
                //Rev 6.0
                if ($("#hdnMode").val() == "0") {
                    cbtnSaveRecordsTDS.SetVisible(true);
                }
                cbtn_SaveRecordsTDS.SetVisible(true);
                //Rev 6.0 End
                $("#divIsPartyJournal").hide();
                var JV_Number = gridTDS.cpVouvherNo;
                var JV_Msg = "Journal Voucher No. " + JV_Number + " generated.";
                var strSchemaType = document.getElementById('hdnSchemaType').value;
                var AutoPrint = document.getElementById('hdnAutoPrint').value;

                if (value == "E") {
                    var IsComplete = "0";

                    if (JV_Number != "") {
                        if (AutoPrint == "Yes") {
                            var reportName = 'JournalVoucher~D'
                            var module = 'JOURNALVOUCHER'
                            window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + JV_Number, '_blank')
                        }
                        jAlert(JV_Msg, 'Alert Dialog: [Journal Voucher]', function (r) {
                            if (r == true) {
                                window.location.reload();
                            }
                        });
                    } else {
                        window.location.reload();
                    }
                }
                else if (value == "S") {
                    var IsComp = "0";

                    if (JV_Number != "") {
                        if (AutoPrint == "Yes") {
                            var reportName = 'JournalVoucher~D'
                            var module = 'JOURNALVOUCHER'
                            window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + JV_Number, '_blank')
                        }
                        jAlert(JV_Msg, 'Alert Dialog: [Journal Voucher]', function (r) {
                            if (r == true) {
                                IsComp = "1";
                            }
                        });
                    }
                    else {
                        IsComp = "1";
                    }

                    if (IsComp == "1") {
                <%--$('#<%=hdnMode.ClientID %>').val('0');
                    document.getElementById('div_Edit').style.display = 'block';
                    cbtnSaveRecords.SetVisible(false);
                    cbtn_SaveRecords.SetVisible(false);
                    gridTDS.PerformCallback('BlanckEdit');--%>

                        gridTDS.AddNewRow();
                        $('#<%=hdnMode.ClientID %>').val('0');

                        $('#<%= lblHeading.ClientID %>').text("Add TDS Journal Voucher");

                        cbtnSaveRecords.SetVisible(false);
                        cbtn_SaveRecords.SetVisible(false);


                        var Amount = parseFloat(Debit) - parseFloat(Credit);
                        var div = document.getElementById('loadCurrencyMassage');
                        var txt = "<label><span style='color: red; font-weight: bold; font-size: medium;'>**  Mismatch detected.</span></label>";
                        div.innerHTML = txt;

                        loadCurrencyMassage.style.display = "block";

                        document.getElementById('div_EditTDS').style.display = 'block';
                        document.getElementById('<%= txtBillNoTDS.ClientID %>').disabled = true;
                        //cCmbScheme.SetValue("0");
                        document.getElementById('<%= txtBillNoTDS.ClientID %>').value = "";
                        document.getElementById('<%= txtBillNoTDS.ClientID %>').disabled = true;
                        c_txt_Debit.SetValue("0");
                        c_txt_Credit.SetValue("0");
                        document.getElementById('<%= txtNarrationTDS.ClientID %>').value = "";
                        //cCmbScheme.Focus();

                        if (strSchemaType == "0") {
                            document.getElementById('<%= txtBillNoTDS.ClientID %>').disabled = false;
                    document.getElementById('<%= txtBillNoTDS.ClientID %>').value = "";
                    //document.getElementById("txtBillNo").focus();
                    //cCmbScheme.Focus();

                    document.getElementById("CmbSchemeTDS").focus();
                }
                else if (strSchemaType == "1") {
                    document.getElementById('<%= txtBillNoTDS.ClientID %>').disabled = true;
                    document.getElementById('<%= txtBillNoTDS.ClientID %>').value = "Auto";
                    gridTDS.batchEditApi.StartEdit(-1, 1);
                }
                else if (strSchemaType == "2") {
                    document.getElementById('<%= txtBillNoTDS.ClientID %>').disabled = true;
                    document.getElementById('<%= txtBillNoTDS.ClientID %>').value = "Datewise";
                    gridTDS.batchEditApi.StartEdit(-1, 1);
                }
                else {
                    //cCmbScheme.SetValue("0");
                    //cCmbScheme.Focus();

                    document.getElementById('<%= txtBillNoTDS.ClientID %>').disabled = true;
                    document.getElementById('<%= txtBillNoTDS.ClientID %>').value = "";

                    var CmbScheme = document.getElementById("<%=CmbSchemeTDS.ClientID%>");
                            CmbScheme.options[0].selected = true;
                            document.getElementById("CmbSchemeTDS").focus();
                        }
                    }
                    else {
                        gridTDS.AddNewRow();
                    }
                }
            }
            else {
                gridTDS.AddNewRow();
            }

        }



        function UpdateTrColor() {
            for (var i = 0; i < 1000; i++) {
                if (gridTDS.GetRow(i)) {
                    if (gridTDS.GetRow(i).style.display != "none") {
                        gridTDS.batchEditApi.StartEdit(i, 2);
                        var row = gridTDS.GetRow(i);

                        //if (row.children[11].innerText.trim() != "" && row.children[11].innerText.trim() != null) {
                        //    $(row).addClass(" rowRed");
                        //}
                        if (gridTDS.batchEditApi.GetCellValue(i, "IsTDS").trim() != "" && gridTDS.batchEditApi.GetCellValue(i, "IsTDS").trim() != null) {
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






        var NarrationText;
        function CitiesCombo_EndCallback(s, e) {
            //if (setValueFlag == -1)
            //    s.SetSelectedIndex(0);
            //else if (setValueFlag > -1) {
            //    CityID.SetSelectedItem(CityID.FindItemByValue(setValueFlag));
            //    setValueFlag = -1;
            //}

            if (CityID.cpIsRCM != null) {
                //grid.ShowLoadingPanel();
                //LoadingPanel.Show();
                var Narration = grid.GetEditor('Narration');
                var IsRcm = CityID.cpIsRCM;
                var TaxOption = document.getElementById('ddl_AmountAre').value;
                if (TaxOption == 1) {
                    if (IsRcm == 1) {
                        Narration.SetValue('RCM');
                        NarrationText = 'RCM';
                    }
                    else if (IsRcm == 0) {
                        Narration.SetValue('');
                        NarrationText = '';
                    }
                }
            }

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

            if (chkIsSalary.GetChecked()) {
                return;
            }


            if (ISTDS != "" && ISTDS != null) {

                e.cancel = true;
            }
            var cityIDColumn = s.GetColumnByField("CityIDTDS");
        }


        function OnBatchEditStartEditing(s, e) {
            currentEditableVisibleIndex = e.visibleIndex;
            globalRowIndex = e.visibleIndex;
            var currentCountryID = grid.batchEditApi.GetCellValue(currentEditableVisibleIndex, "gvColMainAccount");
            var cityIDColumn = s.GetColumnByField("CityID");




            //if (!e.rowValues.hasOwnProperty(cityIDColumn.index))
            //    return;
            //var cellInfo = e.rowValues[cityIDColumn.index];

            //if (lastCountryID == currentCountryID) {
            //    if (CityID.FindItemByValue(cellInfo.value) != null) {
            //        CityID.SetValue(cellInfo.value);
            //    }
            //    else {
            //        RefreshData(cellInfo, lastCountryID);
            //        LoadingPanel.Show();
            //    }
            //}
            //else {
            //    if (currentCountryID == null) {
            //        CityID.SetSelectedIndex(-1);
            //        return;
            //    }
            //    lastCountryID = currentCountryID;
            //    RefreshData(cellInfo, lastCountryID);
            //    LoadingPanel.Show();
            //}

            //setValueFlag = cellInfo.value;
            //CityID.PerformCallback(currentCountryID);
            //LoadingPanel.Show();
        }
        function RefreshData(cellInfo, countryID) {
            setValueFlag = cellInfo.value;
            CityID.PerformCallback(countryID + '~' + setValueFlag);

            //setTimeout(function () {
            //    CityID.PerformCallback(countryID);
            //}, 0);
        }
        //Debjyoti 

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



        function OnCustomButtonClick(s, e) {
            if (e.buttonID == 'CustomDelete') {
                grid.batchEditApi.EndEdit();
                var noofvisiblerows = grid.GetVisibleRowsOnPage();

                if (noofvisiblerows != "1") {
                    var debit = grid.batchEditApi.GetCellValue(e.visibleIndex, "WithDrawl");
                    var credit = grid.batchEditApi.GetCellValue(e.visibleIndex, "Receipt");
                    // Rev 7.0
                    if (debit == null) {
                        debit = 0;
                    }
                    if (credit == null) {
                        credit = 0;
                    }
                    // End of Rev 7.0
                    if (debit != 0)
                        c_txt_Debit.SetValue(DecimalRoundoff(parseFloat(c_txt_Debit.GetValue()) - parseFloat(debit), 2));
                    if (credit != 0)
                        c_txt_Credit.SetValue(DecimalRoundoff(parseFloat(c_txt_Credit.GetValue()) - parseFloat(credit), 2));

                    var Debit = parseFloat(c_txt_Debit.GetValue());
                    var Credit = parseFloat(c_txt_Credit.GetValue());

                    if (Debit == 0 && Credit == 0) {
                        cbtnSaveRecords.SetVisible(false);
                        cbtn_SaveRecords.SetVisible(false);

                        var Amount = parseFloat(Debit) - parseFloat(Credit);
                        var div = document.getElementById('loadCurrencyMassage');
                        var txt = "<label><span style='color: red; font-weight: bold; font-size: medium;'>**  Mismatch detected.</span></label>";
                        div.innerHTML = txt;

                        loadCurrencyMassage.style.display = "block";
                    }
                    else if (Debit == Credit) {
                        cbtnSaveRecords.SetVisible(true);
                        cbtn_SaveRecords.SetVisible(true);
                        loadCurrencyMassage.style.display = "none";
                    }
                    else {
                        cbtnSaveRecords.SetVisible(false);
                        cbtn_SaveRecords.SetVisible(false);

                        var Amount = parseFloat(Debit) - parseFloat(Credit);
                        var div = document.getElementById('loadCurrencyMassage');
                        var txt = "<label><span style='color: red; font-weight: bold; font-size: medium;'>**  Mismatch detected. Amount: " + DecimalRoundoff(Amount, 2) + "</span></label>";
                        div.innerHTML = txt;
                        loadCurrencyMassage.style.display = "block";
                    }
                    if ($("#cpTagged").val() == "-99") {
                        cbtnSaveRecords.SetVisible(false);
                        cbtn_SaveRecords.SetVisible(false);

                    }

                    var MainAccountID = grid.batchEditApi.GetCellValue(e.visibleIndex, "gvMainAcCode"); //grid.GetEditor("gvColMainAccount").GetValue();

                    //	Customers //	Vendors
                    if (MainAccountID == 'Customers' || MainAccountID == 'Vendors') {
                        if ($("#hdnIsPartyLedger").val() == "") {
                            $("#hdnIsPartyLedger").val('1');
                        }
                        else {
                            $("#hdnIsPartyLedger").val(parseFloat($("#hdnIsPartyLedger").val()) - 1);
                        }

                    }



                    grid.DeleteRow(e.visibleIndex);

                    //if (parseFloat($("#hdnIsPartyLedger").val()) > 1 && grid.GetVisibleRowsOnPage()=="2") 
                    if (checkIsPartyjornal()) {
                        $("#divIsPartyJournal").show();
                    }
                    else {
                        $("#divIsPartyJournal").hide();
                    }

                    var type = $('#<%=hdnMode.ClientID %>').val();
                    if (type == '1') {
                        var IsJournal = "";
                        for (var i = 0; i < grid.GetVisibleRowsOnPage(); i++) {
                            var frontProduct = (grid.batchEditApi.GetCellValue(i, 'gvColMainAccount') != null) ? (grid.batchEditApi.GetCellValue(i, 'gvColMainAccount')) : "";

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

                    // Rev 7.0
                    if (debit == null) {
                        debit = 0;
                    }
                    if (credit == null) {
                        credit = 0;
                    }
                    // End of Rev 7.0
                    if (debit != 0)
                        c_txt_DebitTDS.SetValue(DecimalRoundoff(parseFloat(c_txt_DebitTDS.GetValue()) - parseFloat(debit), 2));
                    if (credit != 0)
                        c_txt_CreditTDS.SetValue(DecimalRoundoff(parseFloat(c_txt_CreditTDS.GetValue()) - parseFloat(credit), 2));


                    var DebitCreditTDS = GetDebitCredit(UniqueKey);

                    if (DebitCreditTDS != "" && DebitCreditTDS != undefined) {
                        var dr = DebitCreditTDS.split('~')[0];
                        var cr = DebitCreditTDS.split('~')[1];
                        if (dr != undefined && dr != "" && dr != null) {
                            if (parseFloat(dr) > 0) {
                                c_txt_CreditTDS.SetValue(DecimalRoundoff(parseFloat(c_txt_CreditTDS.GetValue()) - parseFloat(dr), 2));
                            }
                        }
                        if (cr != undefined && cr != "" && cr != null) {
                            if (parseFloat(cr) > 0) {
                                c_txt_CreditTDS.SetValue(DecimalRoundoff(parseFloat(c_txt_CreditTDS.GetValue()) - parseFloat(cr), 2));
                            }
                        }
                    }


                    var Debit = parseFloat(c_txt_DebitTDS.GetValue());
                    var Credit = parseFloat(c_txt_CreditTDS.GetValue());

                    if (Debit == 0 && Credit == 0) {
                        cbtnSaveRecordsTDS.SetVisible(false);
                        cbtn_SaveRecordsTDS.SetVisible(false);

                        var Amount = parseFloat(Debit) - parseFloat(Credit);
                        var div = document.getElementById('loadCurrencyMassage');
                        var txt = "<label><span style='color: red; font-weight: bold; font-size: medium;'>**  Mismatch detected.</span></label>";
                        div.innerHTML = txt;

                        loadCurrencyMassage.style.display = "block";
                    }
                    else if (Debit == Credit) {
                        cbtnSaveRecordsTDS.SetVisible(true);
                        cbtn_SaveRecordsTDS.SetVisible(true);
                        loadCurrencyMassage.style.display = "none";
                    }
                    else {
                        cbtnSaveRecordsTDS.SetVisible(false);
                        cbtn_SaveRecordsTDS.SetVisible(false);

                        var Amount = parseFloat(Debit) - parseFloat(Credit);
                        var div = document.getElementById('loadCurrencyMassage');
                        var txt = "<label><span style='color: red; font-weight: bold; font-size: medium;'>**  Mismatch detected. Amount: " + DecimalRoundoff(Amount, 2) + "</span></label>";
                        div.innerHTML = txt;
                        loadCurrencyMassage.style.display = "block";
                    }
                    if ($("#cpTaggedTDS").val() == "-99") {
                        cbtnSaveRecordsTDS.SetVisible(false);
                        cbtn_SaveRecordsTDS.SetVisible(false);

                    }

                    var UniqueID = gridTDS.batchEditApi.GetCellValue(e.visibleIndex, "UniqueID"); //gridTDS.GetEditor("gvColMainAccount").GetValue();


                    if (UniqueID != "")
                        DeleteTDSRows(UniqueID);
                    else
                        gridTDS.DeleteRow(e.visibleIndex);

                    //if (parseFloat($("#hdnIsPartyLedger").val()) > 1 && gridTDS.GetVisibleRowsOnPage()=="2") 


                    var type = $('#hdnMode').val();
                    if (type == '1') {
                        var IsJournal = "";
                        for (var i = 0; i < gridTDS.GetVisibleRowsOnPage(); i++) {
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




        function CreditGotFocus(s, e) {
            CreditOldValue = s.GetText();
            var indx = CreditOldValue.indexOf(',');
            if (indx != -1) {
                CreditOldValue = CreditOldValue.replace(/,/g, '');
            }
        }

        function CreditGotFocusTDS(s, e) {
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

            c_txt_Credit.SetValue(parseFloat(DecimalRoundoff((CurrentSum - newDif), 2)));
        }
        function recalculateCredit(oldVal) {
            if (oldVal != 0) {
                CreditNewValue = 0;
                CreditOldValue = oldVal;
                changeCreditTotalSummary();
            }
        }



        function changeCreditTotalSummaryTDS() {
            var newDif = CreditOldValue - CreditNewValue;
            var CurrentSum = c_txt_CreditTDS.GetText();
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
            //c_txt_DebitTDS.SetValue(parseFloat(DecimalRoundoff((CurrentSum - newDif), 2)));
            c_txt_DebitTDS.SetValue(parseFloat(DecimalRoundoff(DebitAmount, 2)));
            c_txt_CreditTDS.SetValue(parseFloat(DecimalRoundoff(CreditAmount, 2)));
        }
        function recalculateCreditTDS(oldVal) {
            if (oldVal != 0) {
                CreditNewValue = 0;
                CreditOldValue = oldVal;
                changeCreditTotalSummaryTDS();
            }
        }

        function DebitGotFocus(s, e) {
            debitOldValue = s.GetText();
            var indx = debitOldValue.indexOf(',');
            if (indx != -1) {
                debitOldValue = debitOldValue.replace(/,/g, '');
            }
        }

        function DebitGotFocusTDS(s, e) {
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


        function DebitLostFocusTDS(s, e) {
            debitNewValue = s.GetText();
            var indx = debitNewValue.indexOf(',');

            if (indx != -1) {
                debitNewValue = debitNewValue.replace(/,/g, '');
            }
            if (debitOldValue != debitNewValue) {
                changeDebitTotalSummaryTDS();
            }
        }

        function recalculateDebit(oldVal) {
            if (oldVal != 0) {
                debitNewValue = 0;
                debitOldValue = oldVal;
                changeDebitTotalSummary();
            }
        }

        function recalculateDebitTDS(oldVal) {
            if (oldVal != 0) {
                debitNewValue = 0;
                debitOldValue = oldVal;
                changeDebitTotalSummaryTDS();
            }
        }

        function changeDebitTotalSummary() {
            var newDif = debitOldValue - debitNewValue;
            var CurrentSum = c_txt_Debit.GetText();
            var indx = CurrentSum.indexOf(',');
            if (indx != -1) {
                CurrentSum = CurrentSum.replace(/,/g, '');
            }

            c_txt_Debit.SetValue(parseFloat(DecimalRoundoff((CurrentSum - newDif), 2)));
        }


        function changeDebitTotalSummaryTDS() {








            var newDif = debitOldValue - debitNewValue;
            var CurrentSum = c_txt_DebitTDS.GetText();
            var indx = CurrentSum.indexOf(',');
            if (indx != -1) {
                CurrentSum = CurrentSum.replace(/,/g, '');
            }



            gridTDS.batchEditApi.EndEdit();
            var DebitAmount = 0;
            var CreditAmount = 0;

            for (var i = 0; i < 1000; i++) {
                if (gridTDS.GetRow(i)) {
                    if (gridTDS.GetRow(i).style.display != "none") {
                        //gridTDS.batchEditApi.StartEdit(i, 2);
                        DebitAmount = DecimalRoundoff(parseFloat(DebitAmount), 2) + DecimalRoundoff(parseFloat(gridTDS.batchEditApi.GetCellValue(i, "WithDrawlTDS")), 2);
                        CreditAmount = DecimalRoundoff(parseFloat(CreditAmount), 2) + DecimalRoundoff(parseFloat(gridTDS.batchEditApi.GetCellValue(i, "ReceiptTDS")), 2);
                    }
                }
            }


            for (i = -1; i > -1000; i--) {
                if (gridTDS.GetRow(i)) {
                    if (gridTDS.GetRow(i).style.display != "none") {
                        //gridTDS.batchEditApi.StartEdit(i, 2);
                        DebitAmount = DecimalRoundoff(parseFloat(DebitAmount), 2) + DecimalRoundoff(parseFloat(gridTDS.batchEditApi.GetCellValue(i, "WithDrawlTDS")), 2);
                        CreditAmount = DecimalRoundoff(parseFloat(CreditAmount), 2) + DecimalRoundoff(parseFloat(gridTDS.batchEditApi.GetCellValue(i, "ReceiptTDS")), 2);
                    }
                }
            }
            //c_txt_DebitTDS.SetValue(parseFloat(DecimalRoundoff((CurrentSum - newDif), 2)));


            c_txt_DebitTDS.SetValue(parseFloat(DecimalRoundoff(DebitAmount, 2)));
            c_txt_CreditTDS.SetValue(parseFloat(DecimalRoundoff(CreditAmount, 2)));
        }

        function CalculateSummary(grid, rowValues, visibleIndex, isDeleting) {
            //ctxtTDebit
            var originalValue = grid.batchEditApi.GetCellValue(visibleIndex, "WithDrawl");
            var newValue = rowValues[(grid.GetColumnByField("WithDrawl").index)].value;
            var dif = isDeleting ? -newValue : newValue - originalValue;
            c_txt_Debit.SetValue(DecimalRoundoff(parseFloat(c_txt_Debit.GetValue()) + dif), 2);
            //ctxtTCredit
            var CoriginalValue = grid.batchEditApi.GetCellValue(visibleIndex, "Receipt");
            var CnewValue = rowValues[(grid.GetColumnByField("Receipt").index)].value;
            var Cdif = isDeleting ? -CnewValue : CnewValue - CoriginalValue;
            c_txt_Credit.SetValue(DecimalRoundoff(parseFloat(c_txt_Credit.GetValue()) + Cdif), 2);

            var Debit = parseFloat(c_txt_Debit.GetValue());
            var Credit = parseFloat(c_txt_Credit.GetValue());

            if (Debit == 0 && Credit == 0) {
                cbtnSaveRecords.SetVisible(false);
                cbtn_SaveRecords.SetVisible(false);

                var Amount = parseFloat(Debit) - parseFloat(Credit);
                var div = document.getElementById('loadCurrencyMassage');
                var txt = "<label><span style='color: red; font-weight: bold; font-size: medium;'>**  Mismatch detected.</span></label>";
                div.innerHTML = txt;

                loadCurrencyMassage.style.display = "block";
            }
            else if (Debit == Credit) {
                cbtnSaveRecords.SetVisible(true);
                cbtn_SaveRecords.SetVisible(true);
                loadCurrencyMassage.style.display = "none";
            }
            else {
                cbtnSaveRecords.SetVisible(false);
                cbtn_SaveRecords.SetVisible(false);

                var Amount = parseFloat(Debit) - parseFloat(Credit);
                var div = document.getElementById('loadCurrencyMassage');
                var txt = "<label><span style='color: red; font-weight: bold; font-size: medium;'>**  Mismatch detected. Amount: " + DecimalRoundoff(Amount, 2) + "</span></label>";
                div.innerHTML = txt;

                loadCurrencyMassage.style.display = "block";
            }
            if ($("#cpTagged").val() == "-99") {
                cbtnSaveRecords.SetVisible(false);
                cbtn_SaveRecords.SetVisible(false);

            }
        }
        //End here

        function OnBatchEditEndEditing(s, e) {
            //Debjyoti
            //  CalculateSummary(s, e.rowValues, e.visibleIndex, false);
            //End here
            currentEditableVisibleIndex = -1;
            var cityIDColumn = s.GetColumnByField("CityID");
            //if (!e.rowValues.hasOwnProperty(cityIDColumn.index))
            //    return;
            // var cellInfo = e.rowValues[cityIDColumn.index];
            //if (CityID.GetSelectedIndex() > -1 || cellInfo.text != CityID.GetText()) {
            //    cellInfo.value = CityID.GetValue();
            //    cellInfo.text = CityID.GetText();
            //    CityID.SetValue(null);
            //}
        }

        function OnBatchEditEndEditingTDS(s, e) {
            currentEditableVisibleIndexTDS = -1;
            var cityIDColumn = s.GetColumnByField("CityIDTDS");

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
            console.log(e);
            var keyCode = ASPxClientUtils.GetKeyCode(e.htmlEvent);
            var noofvisiblerows = grid.GetVisibleRowsOnPage();
            // var row = grid.GetVisibleIndex();
            if ((keyCode === 13)) {
                var mainAccountValue = (grid.GetEditor('MainAccount').GetValue() != null) ? grid.GetEditor('MainAccount').GetValue() : "";
                if (mainAccountValue != "") {
                    grid.AddNewRow();
                    //grid.SetFocusedRowIndex(globalRowIndex,1);
                    //grid.GetEditor("MainAccount").Focus(); // grid.SetFocusedRowIndex();
                    // return;

                    setTimeout(function () { grid.batchEditApi.StartEdit(globalRowIndex, 1); }, 500);

                }
            }
            else if (keyCode === 9) {
                // setTimeout(function () { grid.batchEditApi.StartEdit(globalRowIndex, 1); }, 500);
                document.getElementById("txtNarration").focus();
            }
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
            var mainAccountValue = (grid.GetEditor('MainAccount').GetValue() != null) ? grid.GetEditor('MainAccount').GetValue() : "";

            if (mainAccountValue != "") {
                DebitLostFocus(s, e);
                var withDrawlValue = (grid.GetEditor('WithDrawl').GetValue() != null) ? parseFloat(grid.GetEditor('WithDrawl').GetValue()) : "0";
                var receiptValue = (grid.GetEditor('Receipt').GetValue() != null) ? grid.GetEditor('Receipt').GetValue() : "0";

                if (withDrawlValue > 0) {
                    recalculateCredit(grid.GetEditor('Receipt').GetValue());
                    grid.GetEditor('Receipt').SetValue("0");
                    //grid.GetEditor('Receipt').SetEnabled(false);
                }

                var Debit = parseFloat(c_txt_Debit.GetValue());
                var Credit = parseFloat(c_txt_Credit.GetValue());

                if (Debit == 0 && Credit == 0) {
                    cbtnSaveRecords.SetVisible(false);
                    cbtn_SaveRecords.SetVisible(false);
                    var Amount = parseFloat(Debit) - parseFloat(Credit);
                    var div = document.getElementById('loadCurrencyMassage');
                    var txt = "<label><span style='color: red; font-weight: bold; font-size: medium;'>**  Mismatch detected.</span></label>";
                    div.innerHTML = txt;
                    loadCurrencyMassage.style.display = "block";
                }
                else if (Debit == Credit) {
                    cbtnSaveRecords.SetVisible(true);
                    cbtn_SaveRecords.SetVisible(true);
                    loadCurrencyMassage.style.display = "none";
                }
                else {
                    cbtnSaveRecords.SetVisible(false);
                    cbtn_SaveRecords.SetVisible(false);

                    var Amount = parseFloat(Debit) - parseFloat(Credit);
                    var div = document.getElementById('loadCurrencyMassage');
                    var txt = "<label><span style='color: red; font-weight: bold; font-size: medium;'>**  Mismatch detected. Amount: " + DecimalRoundoff(Amount, 2) + "</span></label>";
                    div.innerHTML = txt;

                    loadCurrencyMassage.style.display = "block";
                }
                if ($("#cpTagged").val() == "-99") {
                    cbtnSaveRecords.SetVisible(false);
                    cbtn_SaveRecords.SetVisible(false);

                }
            }
            else {
                grid.GetEditor('WithDrawl').SetValue("0");
            }
        }

        function ProjectInlineLost_Focus(s, e) {
            setTimeout(function () { gridTDS.batchEditApi.StartEdit(globalRowIndexTDS, 6); }, 500);
        }


        function WithDrawlTextChangeTDS(s, e) {
            var mainAccountValue = (gridTDS.GetEditor('MainAccountTDS').GetValue() != null) ? gridTDS.GetEditor('MainAccountTDS').GetValue() : "";
            var uniqueIndex = globalRowIndexTDS;
            if (mainAccountValue != "") {
                //DebitLostFocusTDS(s, e);
                var withDrawlValue = (gridTDS.GetEditor('WithDrawlTDS').GetValue() != null) ? parseFloat(gridTDS.GetEditor('WithDrawlTDS').GetValue()) : "0";
                var receiptValue = (gridTDS.GetEditor('ReceiptTDS').GetValue() != null) ? gridTDS.GetEditor('ReceiptTDS').GetValue() : "0";

                if (withDrawlValue > 0) {
                    //  gridTDS.GetEditor('ReceiptTDS').SetValue("0");
                }

                var Amount = (gridTDS.GetEditor('WithDrawlTDS').GetValue() != null) ? parseFloat(gridTDS.GetEditor('WithDrawlTDS').GetValue()) : "0";
                var UniqueID = gridTDS.GetEditor('UniqueID').GetText();// gridTDS.batchEditApi.GetCellValue(e.visibleIndex, "UniqueID");
                if (!chkIsSalary.GetChecked())
                    UpdateTDSValue(UniqueID, Amount);

                changeDebitTotalSummaryTDS();


                gridTDS.batchEditApi.StartEdit(globalRowIndexTDS, 2);

                var Debit = parseFloat(c_txt_DebitTDS.GetValue());
                var Credit = parseFloat(c_txt_CreditTDS.GetValue());

                if (Debit == 0 && Credit == 0) {
                    cbtnSaveRecordsTDS.SetVisible(false);
                    cbtn_SaveRecordsTDS.SetVisible(false);
                    var Amount = parseFloat(Debit) - parseFloat(Credit);
                    var div = document.getElementById('loadCurrencyMassage');
                    var txt = "<label><span style='color: red; font-weight: bold; font-size: medium;'>**  Mismatch detected.</span></label>";
                    div.innerHTML = txt;
                    loadCurrencyMassage.style.display = "block";
                }
                else if (Debit == Credit) {
                    cbtnSaveRecordsTDS.SetVisible(true);
                    cbtn_SaveRecordsTDS.SetVisible(true);
                    loadCurrencyMassage.style.display = "none";
                }
                else {
                    cbtnSaveRecordsTDS.SetVisible(false);
                    cbtn_SaveRecordsTDS.SetVisible(false);

                    var Amount = parseFloat(Debit) - parseFloat(Credit);
                    var div = document.getElementById('loadCurrencyMassage');
                    var txt = "<label><span style='color: red; font-weight: bold; font-size: medium;'>**  Mismatch detected. Amount: " + DecimalRoundoff(Amount, 2) + "</span></label>";
                    div.innerHTML = txt;

                    loadCurrencyMassage.style.display = "block";
                }
                if ($("#cpTaggedTDS").val() == "-99") {
                    cbtnSaveRecordsTDS.SetVisible(false);
                    cbtn_SaveRecordsTDS.SetVisible(false);

                }




            }
            else {
                // gridTDS.GetEditor('WithDrawlTDS').SetValue("0");
            }

            setTimeout(function () { gridTDS.batchEditApi.StartEdit(uniqueIndex, 4); }, 500);

        }


        function round5(x) {
            return x % 5 < 2.5 ? (x % 5 === 0 ? x : Math.floor(x / 5) * 5) : Math.ceil(x / 5) * 5
        }

        function round10(x) {
            return x % 10 < 5 ? (x % 10 === 0 ? x : Math.floor(x / 10) * 10) : Math.ceil(x / 10) * 10
        }

        function round1(x) {
            return x % 1 < .5 ? (x % 1 === 0 ? x : Math.floor(x / 1) * 1) : Math.ceil(x / 1) * 1
        }


        function UpdateTDSValue(UniqueID, Amount) {
            gridTDS.batchEditApi.EndEdit();
            for (var i = 0; i < 1000; i++) {
                if (gridTDS.GetRow(i)) {
                    if (gridTDS.GetRow(i).style.display != "none") {
                        // gridTDS.batchEditApi.StartEdit(i, 2);
                        var ISTDS = gridTDS.batchEditApi.GetCellValue(i, "IsTDS");
                        if (gridTDS.batchEditApi.GetCellValue(i, "UniqueID") == UniqueID && ISTDS != "" && ISTDS != null) {
                            var TDSPercentage = gridTDS.batchEditApi.GetCellValue(i, "TDSPercentage");
                            var newamt = DecimalRoundoff(parseFloat(Amount), 2) * (parseFloat(TDSPercentage.split('~')[0]) / 100);

                            //var newamt = DecimalRoundoff(parseFloat(Amount), 2) * (parseFloat(TDSPercentage) / 100);
                            var ro = TDSPercentage.split('~')[1];

                            if (ro == "1") {
                                newamt = round1(newamt);
                            }
                            else if (ro == "2") {
                                newamt = round5(newamt);
                            }
                            else if (ro == "3") {
                                newamt = round10(newamt);
                            }

                            gridTDS.batchEditApi.SetCellValue(i, "ReceiptTDS", newamt);

                        }
                    }
                }
            }

            for (i = -1; i > -1000; i--) {
                if (gridTDS.GetRow(i)) {
                    if (gridTDS.GetRow(i).style.display != "none") {
                        //gridTDS.batchEditApi.StartEdit(i, 2);
                        var ISTDS = gridTDS.batchEditApi.GetCellValue(i, "IsTDS");
                        if (gridTDS.batchEditApi.GetCellValue(i, "UniqueID") == UniqueID && ISTDS != "" && ISTDS != null) {
                            var TDSPercentage = gridTDS.batchEditApi.GetCellValue(i, "TDSPercentage");
                            var newamt = DecimalRoundoff(parseFloat(Amount), 2) * (parseFloat(TDSPercentage.split('~')[0]) / 100);

                            //var newamt = DecimalRoundoff(parseFloat(Amount), 2) * (parseFloat(TDSPercentage) / 100);
                            var ro = TDSPercentage.split('~')[1];

                            if (ro == "1") {
                                newamt = round1(newamt);
                            }
                            else if (ro == "2") {
                                newamt = round5(newamt);
                            }
                            else if (ro == "3") {
                                newamt = round10(newamt);
                            }

                            gridTDS.batchEditApi.SetCellValue(i, "ReceiptTDS", newamt);
                        }
                    }
                }
            }

            gridTDS.batchEditApi.StartEdit(i, 2);
        }








        function ReceiptTextChange(s, e) {
            var mainAccountValue = (grid.GetEditor('MainAccount').GetValue() != null) ? grid.GetEditor('MainAccount').GetValue() : "";

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

                if (Debit == 0 && Credit == 0) {
                    cbtnSaveRecords.SetVisible(false);
                    cbtn_SaveRecords.SetVisible(false);

                    var Amount = parseFloat(Debit) - parseFloat(Credit);
                    var div = document.getElementById('loadCurrencyMassage');
                    var txt = "<label><span style='color: red; font-weight: bold; font-size: medium;'>**  Mismatch detected.</span></label>";
                    div.innerHTML = txt;


                    loadCurrencyMassage.style.display = "block";
                }
                else if (Debit == Credit) {
                    cbtnSaveRecords.SetVisible(true);
                    cbtn_SaveRecords.SetVisible(true);
                    loadCurrencyMassage.style.display = "none";
                }
                else {
                    cbtnSaveRecords.SetVisible(false);
                    cbtn_SaveRecords.SetVisible(false);

                    var Amount = parseFloat(Debit) - parseFloat(Credit);
                    var div = document.getElementById('loadCurrencyMassage');
                    var txt = "<label><span style='color: red; font-weight: bold; font-size: medium;'>**  Mismatch detected. Amount: " + DecimalRoundoff(Amount, 2) + "</span></label>";
                    div.innerHTML = txt;

                    loadCurrencyMassage.style.display = "block";
                }
                if ($("#cpTagged").val() == "-99") {
                    cbtnSaveRecords.SetVisible(false);
                    cbtn_SaveRecords.SetVisible(false);

                }
            }
            else {
                grid.GetEditor('Receipt').SetValue("0");
            }

            //chinmoy added for narration focus
            if ($("#hdnProjectSelectInEntryModule").val() == "0") {
                setTimeout(function () { grid.batchEditApi.StartEdit(globalRowIndex, 6); }, 1000);

            }
            //End
        }


        function ReceiptTextChangeTDS(s, e) {


            var IsTDSSource = gridTDS.batchEditApi.GetCellValue(globalRowIndexTDS, "IsTDSSource");
            var uniqueIndex = globalRowIndexTDS;
            if (IsTDSSource == "1") {

                gridTDS.GetEditor('ReceiptTDS').SetValue("0");
                if ($("#hdnProjectSelectInEntryModule").val() == "1") {
                    setTimeout(function () { gridTDS.batchEditApi.StartEdit(uniqueIndex, 5); }, 200);
                }
                else {
                    setTimeout(function () { gridTDS.batchEditApi.StartEdit(uniqueIndex, 6); }, 200);
                }
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

                var Debit = parseFloat(c_txt_DebitTDS.GetValue());
                var Credit = parseFloat(c_txt_CreditTDS.GetValue());

                if (Debit == 0 && Credit == 0) {
                    cbtnSaveRecordsTDS.SetVisible(false);
                    cbtn_SaveRecordsTDS.SetVisible(false);

                    var Amount = parseFloat(Debit) - parseFloat(Credit);
                    var div = document.getElementById('loadCurrencyMassage');
                    var txt = "<label><span style='color: red; font-weight: bold; font-size: medium;'>**  Mismatch detected.</span></label>";
                    div.innerHTML = txt;


                    loadCurrencyMassage.style.display = "block";
                }
                else if (Debit == Credit) {
                    cbtnSaveRecordsTDS.SetVisible(true);
                    cbtn_SaveRecordsTDS.SetVisible(true);
                    loadCurrencyMassage.style.display = "none";
                }
                else {
                    cbtnSaveRecordsTDS.SetVisible(false);
                    cbtn_SaveRecordsTDS.SetVisible(false);

                    var Amount = parseFloat(Debit) - parseFloat(Credit);
                    var div = document.getElementById('loadCurrencyMassage');
                    var txt = "<label><span style='color: red; font-weight: bold; font-size: medium;'>**  Mismatch detected. Amount: " + DecimalRoundoff(Amount, 2) + "</span></label>";
                    div.innerHTML = txt;

                    loadCurrencyMassage.style.display = "block";
                }
                if ($("#cpTagged").val() == "-99") {
                    cbtnSaveRecordsTDS.SetVisible(false);
                    cbtn_SaveRecordsTDS.SetVisible(false);

                }
            }
            else {
                gridTDS.GetEditor('ReceiptTDS').SetValue("0");
            }
            if ($("#hdnProjectSelectInEntryModule").val() == "1") {
                setTimeout(function () { gridTDS.batchEditApi.StartEdit(uniqueIndex, 5); }, 200);
            }
            else
                setTimeout(function () { gridTDS.batchEditApi.StartEdit(uniqueIndex, 6); }, 200);
        }







        function deleteAllRows() {
            var frontRow = 0;
            var backRow = -1;
            for (var i = 0; i <= grid.GetVisibleRowsOnPage() + 100; i++) {
                grid.DeleteRow(frontRow);
                grid.DeleteRow(backRow);
                backRow--;
                frontRow++;
            }
            grid.AddNewRow();

            ctxtTotalPayment.SetValue(0);
            c_txt_Debit.SetValue(0);

        }

        function Look_up_Project() {
            clookup_Project.gridView.Refresh();
        }





        function CmbScheme_ValueChange() {
    //var val = cCmbScheme.GetValue();
    <%--Rev Work start: Copy Feature required for Journal Vouchers Date:-27.05.2022 Mantise no:24911--%>

            //deleteAllRows();
            if (Mode != 'Copy') {
                deleteAllRows();
            }
    <%--Rev Work close: Copy Feature required for Journal Vouchers Date:-27.05.2022 Mantise no:24911--%>
            //InsgridBatch.AddNewRow();
            var val = document.getElementById("CmbScheme").value;
    <%--Rev Work start: Copy Feature required for Journal Vouchers Date:-27.05.2022 Mantise no:24911--%>
            $('#hdnSchemeVal').val(val);
    <%--Rev Work close: Copy Feature required for Journal Vouchers Date:-27.05.2022 Mantise no:24911--%>
            $("#MandatoryBillNo").hide();

            if (val != "0") {
                $.ajax({
                    type: "POST",
                    url: 'JournalEntry.aspx/getSchemeType',
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

                        $("#hdnToUnit").val(branchID);
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



                        document.getElementById('ddlSupplyState').value = branchStateID;
                        $('#<%=hdnBranchId.ClientID %>').val(branchID);
                        $('#<%=hfIsFilter.ClientID %>').val(branchID);
                        if (schemetypeValue != "") {
                            document.getElementById('ddlBranch').value = branchID;
                            document.getElementById('<%= ddlBranch.ClientID %>').disabled = true;
                            // CountryID.PerformCallback(branchID);
                        }
                        // debugger;
                        if (schemetype == '0') {
                            $('#<%=hdnSchemaType.ClientID %>').val('0');
                    document.getElementById('<%= txtBillNo.ClientID %>').disabled = false;
                    document.getElementById('<%= txtBillNo.ClientID %>').value = "";
                    //document.getElementById("txtBillNo").focus();
                    setTimeout(function () { $("#txtBillNo").focus(); }, 200);

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
                        clookup_Project.gridView.Refresh();
                        var startDate = new Date();
                        startDate = tDate.GetDate().format('yyyy-MM-dd');
                        cPanellookup_GRNOverhead.PerformCallback('BindOverheadCostGrid' + '~' + startDate);

                        //clookup_GRNOverhead.gridView.Refresh();
                    }
                });
            }
            else {
                document.getElementById('<%= txtBillNo.ClientID %>').disabled = true;
                document.getElementById('<%= txtBillNo.ClientID %>').value = "";
            }
            if ($("#hdnProjectSelectInEntryModule").val() == "1")
                clookup_Project.gridView.Refresh();
        }






        function CmbSchemeTDS_ValueChange() {
            //var val = cCmbScheme.GetValue();
            deleteAllRows();
            //InsgridBatch.AddNewRow();
            var val = document.getElementById("CmbSchemeTDS").value;
            $("#MandatoryBillNoTDS").hide();

            if (val != "0") {
                $.ajax({
                    type: "POST",
                    url: 'JournalEntry.aspx/getSchemeType',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: "{sel_scheme_id:\"" + val + "\"}",
                    success: function (type) {
                        console.log(type);

                        var schemetypeValue = type.d;
                        var schemetype = schemetypeValue.toString().split('~')[0];
                        var schemelength = schemetypeValue.toString().split('~')[1];
                        $('#txtBillNoTDS').attr('maxLength', schemelength);
                        var branchID = schemetypeValue.toString().split('~')[2];
                        var branchStateID = schemetypeValue.toString().split('~')[3];

                        var fromdate = schemetypeValue.toString().split('~')[4];
                        var todate = schemetypeValue.toString().split('~')[5];

                        var dt = new Date();

                        tDateTDS.SetDate(dt);

                        if (dt < new Date(fromdate)) {
                            tDateTDS.SetDate(new Date(fromdate));
                        }

                        if (dt > new Date(todate)) {
                            tDateTDS.SetDate(new Date(todate));
                        }




                        tDateTDS.SetMinDate(new Date(fromdate));
                        tDateTDS.SetMaxDate(new Date(todate));



                        document.getElementById('ddlSupplyStateTDS').value = branchStateID;
                        $('#<%=hdnBranchId.ClientID %>').val(branchID);
                        $('#<%=hfIsFilter.ClientID %>').val(branchID);
                        if (schemetypeValue != "") {
                            document.getElementById('ddlBranchTDS').value = branchID;
                            document.getElementById('<%= ddlBranchTDS.ClientID %>').disabled = true;
                            // CountryID.PerformCallback(branchID);
                        }
                        if (schemetype == '0') {
                            $('#<%=hdnSchemaType.ClientID %>').val('0');
                            document.getElementById('<%= txtBillNoTDS.ClientID %>').disabled = false;
                            document.getElementById('<%= txtBillNoTDS.ClientID %>').value = "";
                            //document.getElementById("txtBillNo").focus();
                            setTimeout(function () { $("#txtBillNoTDS").focus(); }, 200);

                        }
                        else if (schemetype == '1') {
                            $('#<%=hdnSchemaType.ClientID %>').val('1');
                            document.getElementById('<%= txtBillNoTDS.ClientID %>').disabled = true;
                            document.getElementById('<%= txtBillNoTDS.ClientID %>').value = "Auto";
                            tDateTDS.Focus();
                        }
                        else if (schemetype == '2') {
                            $('#<%=hdnSchemaType.ClientID %>').val('2');
                            document.getElementById('<%= txtBillNoTDS.ClientID %>').disabled = true;
                            document.getElementById('<%= txtBillNoTDS.ClientID %>').value = "Datewise";
                        }
                        var startDate = new Date();
                        startDate = tDateTDS.GetDate().format('yyyy-MM-dd');
                        cPanellookup_GRNOverheadTDS.PerformCallback('BindOverheadCostGridTDS' + '~' + startDate);
                        //clookup_GRNOverheadTDS.gridView.Refresh();
                    }
                });
            }
            else {
                document.getElementById('<%= txtBillNoTDS.ClientID %>').disabled = true;
                document.getElementById('<%= txtBillNoTDS.ClientID %>').value = "";
            }
            if ($("#hdnProjectSelectInEntryModule").val() == "1")
                clookupTDS_Project.gridView.Refresh();
        }






        function GoToNextRow() {
            var gridcount = grid.GetVisibleRowsOnPage();
            grid.batchEditApi.StartEdit(gridcount - 2, 2);
            grid.GetEditor('CountryID').Focus();
        }

        function deleteAllRows() {
            var frontRow = 0;
            var backRow = -1;
            for (var i = 0; i <= grid.GetVisibleRowsOnPage() + 100; i++) {
                grid.DeleteRow(frontRow);
                grid.DeleteRow(backRow);
                backRow--;
                frontRow++;
            }
            grid.AddNewRow();

            c_txt_Debit.SetValue(0);
            c_txt_Credit.SetValue(0);

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

                //clookup_GRNOverhead.gridView.Refresh();
                //clookup_GRNOverheadTDS.gridView.Refresh();
            }
        }

        // Rev 5.0
        function AddContraLockStatus(LockDate) {
            //var LockDate = tDate.GetDate();
            $.ajax({
                type: "POST",
                url: "JournalEntry.aspx/GetAddLock",
                data: JSON.stringify({ LockDate: LockDate }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {
                    var currentRate = msg.d;
                    if (currentRate != null && currentRate == "-9") {
                        $("#hdnValAfterLock").val("-9");
                    }
                    else {
                        $("#hdnValAfterLock").val("1");
                    }

                }
            });
        }
        // End of Rev 5.0

        function SaveButtonClick() {

            //Rev 6.0
            if (cbtnSaveRecords.IsVisible() == true) {
                cbtnSaveRecords.SetVisible(false);
                cbtn_SaveRecords.SetVisible(false);
                //Rev 6.0 End

                var ProjectCode = clookup_Project.GetText();
                // Rev 5.0
                AddContraLockStatus(tDate.GetDate());
                // End of Rev 5.0

                if ($("#hdnProjectSelectInEntryModule").val() == "1" && $("#hdnProjectMandatory").val() == "1" && ProjectCode == "") {
                    jAlert("Please Select Project.");
                    //Rev 6.0
                    cbtnSaveRecords.SetVisible(true);
                    cbtn_SaveRecords.SetVisible(true);
                    //Rev 6.0 End
                    return false;
                }

                //Rev 6.0
                //if (cbtnSaveRecords.IsVisible() == true)
                //{
                //Rev 6.0 End

                ValidGrid = true;
                ValidateGrid();

                var val = document.getElementById("CmbScheme").value;
                var Branchval = $("#ddlBranch").val();
                $("#MandatoryBillNo").hide();

                if (document.getElementById('<%= txtBillNo.ClientID %>').value == "") {
                    //jAlert('Enter Journal No');
                    $("#MandatoryBillNo").show();
                    document.getElementById('<%= txtBillNo.ClientID %>').focus();
                    //Rev 6.0
                    cbtnSaveRecords.SetVisible(true);
                    cbtn_SaveRecords.SetVisible(true);
                    //Rev 6.0 End
                }
                else if (Branchval == "0") {
                    document.getElementById('<%= ddlBranch.ClientID %>').focus();
                    jAlert('Enter Branch');
                    //Rev 6.0
                    cbtnSaveRecords.SetVisible(true);
                    cbtn_SaveRecords.SetVisible(true);
                    //Rev 6.0 End
                }
                else if (ValidGrid == false) {
                    jAlert('Sub Account Set as Mandatory. please select sub account to proceed.');
                    //Rev 6.0
                    cbtnSaveRecords.SetVisible(true);
                    cbtn_SaveRecords.SetVisible(true);
                    //Rev 6.0 End
                }
                // Rev 5.0
                else if ($("#hdnValAfterLock").val() == "-9") {
                    jAlert("DATA is Freezed between   " + $("#hdnLockFromDateCon").val() + " to " + $("#hdnLockToDateCon").val() + " for Add.");
                    //Rev 6.0
                    cbtnSaveRecords.SetVisible(true);
                    cbtn_SaveRecords.SetVisible(true);
                    //Rev 6.0 End
                }
                // End of Rev 5.0
                else {
                    grid.batchEditApi.EndEdit();

                    var frontRow = 0;
                    var backRow = -1;
                    var IsJournal = "";
                    for (var i = 0; i <= grid.GetVisibleRowsOnPage(); i++) {
                        var frontProduct = (grid.batchEditApi.GetCellValue(backRow, 'gvColMainAccount') != null) ? (grid.batchEditApi.GetCellValue(backRow, 'gvColMainAccount')) : "";
                        var backProduct = (grid.batchEditApi.GetCellValue(frontRow, 'gvColMainAccount') != null) ? (grid.batchEditApi.GetCellValue(frontRow, 'gvColMainAccount')) : "";

                        if (frontProduct != "" || backProduct != "") {
                            IsJournal = "Y";
                            break;
                        }

                        backRow--;
                        frontRow++;
                    }

                    if (IsJournal == "Y") {

                        var Count = grid.GetVisibleRowsOnPage();
                        $("#hdnIsValidate").val(Count);

                        $('#<%=hdnRefreshType.ClientID %>').val('S');

                        grid.UpdateEdit();
                        $("#ddl_AmountAre").focus();
                        c_txt_Debit.SetValue("0");
                        c_txt_Credit.SetValue("0");

                        // grid.batchEditApi.StartEdit(globalRowIndex, 1);
                    }
                    else {
                        jAlert('Please add atleast single record first');
                        //Rev 6.0
                        cbtnSaveRecords.SetVisible(true);
                        cbtn_SaveRecords.SetVisible(true);
                        //Rev 6.0 End
                    }
                }
            }
        }

        function SaveButtonClickTDS() {

            //Rev 6.0
            if (cbtnSaveRecordsTDS.IsVisible() == true) {
                cbtnSaveRecordsTDS.SetVisible(false);
                cbtn_SaveRecordsTDS.SetVisible(false);
                //Rev 6.0 End

                var ProjectCode = clookupTDS_Project.GetText();
                if ($("#hdnProjectSelectInEntryModule").val() == "1" && $("#hdnProjectMandatory").val() == "1" && ProjectCode == "") {
                    jAlert("Please Select Project.");
                    //Rev 6.0               
                    cbtnSaveRecordsTDS.SetVisible(true);
                    cbtn_SaveRecordsTDS.SetVisible(true);
                    //Rev 6.0 End
                    return false;
                }

                // Rev 5.0
                AddContraLockStatus(tDateTDS.GetDate());
                // End of Rev 5.0
                //Rev 6.0
                //if (cbtnSaveRecordsTDS.IsVisible() == true) {
                //Rev 6.0 End
                ValidGrid = true;
                isTDSSelected = 1;
                ValidateGridTDS();




                var val = document.getElementById("CmbSchemeTDS").value;
                var Branchval = $("#ddlBranchTDS").val();
                $("#MandatoryBillNoTDS").hide();
                if (isTDSSelected == 0 && !chkTDSJournal.GetChecked()) {
                    jAlert('You must select a TDS Main Account to proceed.');
                }
                else if (document.getElementById('<%= txtBillNoTDS.ClientID %>').value == "") {
                    //jAlert('Enter Journal No');
                    $("#MandatoryBillNoTDS").show();
                    document.getElementById('<%= txtBillNoTDS.ClientID %>').focus();
                    //Rev 6.0               
                    cbtnSaveRecordsTDS.SetVisible(true);
                    cbtn_SaveRecordsTDS.SetVisible(true);
                    //Rev 6.0 End
                }
                else if (Branchval == "0") {
                    document.getElementById('<%= ddlBranchTDS.ClientID %>').focus();
                    jAlert('Enter Branch');
                    //Rev 6.0               
                    cbtnSaveRecordsTDS.SetVisible(true);
                    cbtn_SaveRecordsTDS.SetVisible(true);
                    //Rev 6.0 End
                }
                else if (ValidGrid == false) {
                    jAlert('Sub Account Set as Mandatory. please select sub account to proceed.');
                    //Rev 6.0               
                    cbtnSaveRecordsTDS.SetVisible(true);
                    cbtn_SaveRecordsTDS.SetVisible(true);
                    //Rev 6.0 End
                }
                else if (count > 2 && chkTDSJournal.GetChecked()) {
                    jAlert('You can not add more than 2 rows with consider as TDS checkbox tick.');
                    //Rev 6.0               
                    cbtnSaveRecordsTDS.SetVisible(true);
                    cbtn_SaveRecordsTDS.SetVisible(true);
                    //Rev 6.0 End
                }
                else if (chkTDSJournal.GetChecked() && (ccmbtds.GetValue() == 0 || ccmbtds.GetValue() == null || ccmbtds.GetValue() == "")) {
                    jAlert('You must select TDS Section as TDS checkbox tick.');
                    //Rev 6.0               
                    cbtnSaveRecordsTDS.SetVisible(true);
                    cbtn_SaveRecordsTDS.SetVisible(true);
                    //Rev 6.0 End
                }
                // Rev 5.0
                else if ($("#hdnValAfterLock").val() == "-9") {
                    jAlert("DATA is Freezed between   " + $("#hdnLockFromDateCon").val() + " to " + $("#hdnLockToDateCon").val() + " for Add.");
                    //Rev 6.0               
                    cbtnSaveRecordsTDS.SetVisible(true);
                    cbtn_SaveRecordsTDS.SetVisible(true);
                    //Rev 6.0 End
                }
                // End of Rev 5.0
                else {
                    gridTDS.batchEditApi.EndEdit();

                    var frontRow = 0;
                    var backRow = -1;
                    var IsJournal = "";
                    for (var i = 0; i <= gridTDS.GetVisibleRowsOnPage(); i++) {
                        var frontProduct = (gridTDS.batchEditApi.GetCellValue(backRow, 'gvColMainAccountTDS') != null) ? (gridTDS.batchEditApi.GetCellValue(backRow, 'gvColMainAccountTDS')) : "";
                        var backProduct = (gridTDS.batchEditApi.GetCellValue(frontRow, 'gvColMainAccountTDS') != null) ? (gridTDS.batchEditApi.GetCellValue(frontRow, 'gvColMainAccountTDS')) : "";

                        if (frontProduct != "" || backProduct != "") {
                            IsJournal = "Y";
                            break;
                        }

                        backRow--;
                        frontRow++;
                    }

                    if (IsJournal == "Y") {

                        var Count = gridTDS.GetVisibleRowsOnPage();
                        $("#hdnIsValidate").val(Count);

                        $('#<%=hdnRefreshType.ClientID %>').val('S');
                        gridTDS.UpdateEdit();
                        c_txt_DebitTDS.SetValue("0");
                        c_txt_CreditTDS.SetValue("0");

                    }
                    else {
                        jAlert('Please add atleast single record first');
                        //Rev 6.0               
                        cbtnSaveRecordsTDS.SetVisible(true);
                        cbtn_SaveRecordsTDS.SetVisible(true);
                        //Rev 6.0 End
                    }
                }
            }
        }
        function SaveExitButtonClick() {


            //Rev 6.0
            if (cbtn_SaveRecords.IsVisible() == true) {
                cbtn_SaveRecords.SetVisible(false);
                //Rev 6.0 End
                grid.AddNewRow();
                var ProjectCode = clookup_Project.GetText();
                // Rev 5.0
                AddContraLockStatus(tDate.GetDate());
                // End of Rev 5.0

                if ($("#hdnProjectSelectInEntryModule").val() == "1" && $("#hdnProjectMandatory").val() == "1" && ProjectCode == "") {
                    jAlert("Please Select Project.");
                    //Rev 6.0

                    cbtn_SaveRecords.SetVisible(true);
                    //Rev 6.0 End
                    return false;
                }
                // Rev 5.0
                else if ($("#hdnValAfterLock").val() == "-9") {
                    jAlert("DATA is Freezed between   " + $("#hdnLockFromDateCon").val() + " to " + $("#hdnLockToDateCon").val() + " for Add.");
                    //Rev 6.0

                    cbtn_SaveRecords.SetVisible(true);
                    //Rev 6.0 End
                    return false;
                }
                // End of Rev 5.0
                //Rev 6.0
                // if (cbtn_SaveRecords.IsVisible() == true) {
                //Rev 6.0 End
                var val = document.getElementById("CmbScheme").value;
                var Branchval = $("#ddlBranch").val();
                $("#MandatoryBillNo").hide();
                ValidGrid = true;
                ValidateGrid();
                if (document.getElementById('<%= txtBillNo.ClientID %>').value == "") {
                    //jAlert('Enter Journal No');
                    $("#MandatoryBillNo").show();
                    //Rev 6.0

                    cbtn_SaveRecords.SetVisible(true);
                    //Rev 6.0 End
                    document.getElementById('<%= txtBillNo.ClientID %>').focus();
                }
                else if (Branchval == "0") {
                    document.getElementById('<%= ddlBranch.ClientID %>').focus();
                    jAlert('Enter Branch');
                    //Rev 6.0

                    cbtn_SaveRecords.SetVisible(true);
                    //Rev 6.0 End
                }
                else if (ValidGrid == false) {
                    jAlert('Sub Account Set as Mandatory. please select sub account to proceed.');
                    //Rev 6.0

                    cbtn_SaveRecords.SetVisible(true);
                    //Rev 6.0 End
                }
                else {
                    grid.batchEditApi.EndEdit();

                    var frontRow = 0;
                    var backRow = -1;
                    var IsJournal = "";
                    for (var i = 0; i <= grid.GetVisibleRowsOnPage(); i++) {
                        var frontProduct = (grid.batchEditApi.GetCellValue(backRow, 'MainAccount') != null) ? (grid.batchEditApi.GetCellValue(backRow, 'MainAccount')) : "";
                        var backProduct = (grid.batchEditApi.GetCellValue(frontRow, 'MainAccount') != null) ? (grid.batchEditApi.GetCellValue(frontRow, 'MainAccount')) : "";

                        if (frontProduct != "" || backProduct != "") {
                            IsJournal = "Y";
                            break;
                        }

                        backRow--;
                        frontRow++;
                    }

                    if (IsJournal == "Y") {

                        var Count = grid.GetVisibleRowsOnPage();
                        $("#hdnIsValidate").val(Count);
                        $('#<%=hdnRefreshType.ClientID %>').val('E');
                        grid.UpdateEdit();
                    }
                    else {
                        jAlert('Please add atleast single record first');
                        //Rev 6.0

                        cbtn_SaveRecords.SetVisible(true);
                        //Rev 6.0 End
                    }
                }
            }
        }



        function SaveExitButtonClickTDS() {
            //Rev 6.0
            if (cbtn_SaveRecordsTDS.IsVisible() == true) {
                cbtn_SaveRecordsTDS.SetVisible(false);
                //Rev 6.0 End
                gridTDS.AddNewRow();

                var ProjectCode = clookupTDS_Project.GetText();
                if ($("#hdnProjectSelectInEntryModule").val() == "1" && $("#hdnProjectMandatory").val() == "1" && ProjectCode == "") {
                    jAlert("Please Select Project.");
                    //Rev 6.0              
                    cbtn_SaveRecordsTDS.SetVisible(true);
                    //Rev 6.0 End
                    return false;
                }

                // Rev 5.0
                AddContraLockStatus(tDateTDS.GetDate());
                // End of Rev 5.0
                //Rev 6.0
                //if (cbtn_SaveRecordsTDS.IsVisible() == true) {
                //Rev 6.0 End
                var val = document.getElementById("CmbSchemeTDS").value;
                var Branchval = $("#ddlBranchTDS").val();
                $("#MandatoryBillNoTDS").hide();
                isTDSSelected = 1;
                ValidGrid = true;
                ValidateGridTDS();
                if (isTDSSelected == 0 && !chkTDSJournal.GetChecked()) {
                    jAlert('You must select a TDS Main Account to proceed.');
                    //Rev 6.0              
                    cbtn_SaveRecordsTDS.SetVisible(true);
                    //Rev 6.0 End
                }
                else if (document.getElementById('<%= txtBillNoTDS.ClientID %>').value == "") {
                    //jAlert('Enter Journal No');
                    $("#MandatoryBillNoTDS").show();
                    document.getElementById('<%= txtBillNoTDS.ClientID %>').focus();
                    //Rev 6.0              
                    cbtn_SaveRecordsTDS.SetVisible(true);
                    //Rev 6.0 End
                }
                else if (Branchval == "0") {
                    document.getElementById('<%= ddlBranchTDS.ClientID %>').focus();
                    jAlert('Enter Branch');
                    //Rev 6.0              
                    cbtn_SaveRecordsTDS.SetVisible(true);
                    //Rev 6.0 End
                }
                else if (ValidGrid == false) {
                    jAlert('Sub Account Set as Mandatory. please select sub account to proceed.');
                    //Rev 6.0              
                    cbtn_SaveRecordsTDS.SetVisible(true);
                    //Rev 6.0 End
                }
                else if (count > 2 && chkTDSJournal.GetChecked()) {
                    jAlert('You can not add more than 2 rows with consider as TDS checkbox tick.');
                    //Rev 6.0              
                    cbtn_SaveRecordsTDS.SetVisible(true);
                    //Rev 6.0 End
                }
                else if (chkTDSJournal.GetChecked() && (ccmbtds.GetValue() == 0 || ccmbtds.GetValue() == null || ccmbtds.GetValue() == "")) {
                    jAlert('You must select TDS Section as TDS checkbox tick.');
                    //Rev 6.0              
                    cbtn_SaveRecordsTDS.SetVisible(true);
                    //Rev 6.0 End
                }
                // Rev 5.0
                else if ($("#hdnValAfterLock").val() == "-9") {
                    jAlert("DATA is Freezed between   " + $("#hdnLockFromDateCon").val() + " to " + $("#hdnLockToDateCon").val() + " for Add.");
                    //Rev 6.0              
                    cbtn_SaveRecordsTDS.SetVisible(true);
                    //Rev 6.0 End
                }
                // End of Rev 5.0
                else {
                    gridTDS.batchEditApi.EndEdit();

                    var frontRow = 0;
                    var backRow = -1;
                    var IsJournal = "";
                    for (var i = 0; i <= gridTDS.GetVisibleRowsOnPage(); i++) {
                        var frontProduct = (gridTDS.batchEditApi.GetCellValue(backRow, 'MainAccountTDS') != null) ? (grid.batchEditApi.GetCellValue(backRow, 'MainAccountTDS')) : "";
                        var backProduct = (gridTDS.batchEditApi.GetCellValue(frontRow, 'MainAccountTDS') != null) ? (grid.batchEditApi.GetCellValue(frontRow, 'MainAccountTDS')) : "";

                        if (frontProduct != "" || backProduct != "") {
                            IsJournal = "Y";
                            break;
                        }

                        backRow--;
                        frontRow++;
                    }

                    if (IsJournal == "Y") {

                        var Count = gridTDS.GetVisibleRowsOnPage();
                        $("#hdnIsValidateTDS").val(Count);
                        $('#<%=hdnRefreshType.ClientID %>').val('E');
                        gridTDS.UpdateEdit();
                    }
                    else {
                        jAlert('Please add atleast single record first');
                        //Rev 6.0              
                        cbtn_SaveRecordsTDS.SetVisible(true);
                        //Rev 6.0 End
                    }
                }
            }
        }





        function OnKeyDown(s, e) {
            if (e.htmlEvent.keyCode == 40 || e.htmlEvent.keyCode == 38)
                return ASPxClientUtils.PreventEvent(e.htmlEvent);
        }

        function OnKeyDownTDS(s, e) {



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
                url: "JournalEntry.aspx/CheckUniqueName",
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
            $("#divIsPartyJournal").hide();
            $('#MainAccountModel').modal('hide');
            IsRcm.SetChecked(0);
            RcmCheckChange();
            RcmCheckChangeTDS();
            $('#ddlBranch').blur(function () {
                if (grid.GetVisibleRowsOnPage() == 1) {
                    grid.batchEditApi.StartEdit(-1, 1);
                }
            })

        });

        function SignOff() {
            window.parent.SignOff();
        }

        var isCtrl = false;
        document.onkeydown = function (e) {
            if (event.keyCode == 83 && event.altKey == true) { //run code for Ctrl+S -- ie, Save & New  
                StopDefaultAction(e);
                var debit = parseFloat(c_txt_Debit.GetValue());
                var credit = parseFloat(c_txt_Credit.GetValue());
                if ((debit == credit) && (debit != 0) && (credit != 0)) {
                    //SaveButtonClick();
                    document.getElementById('btnSaveRecords').click();
                    return false;
                }
            }
            else if (event.keyCode == 88 && event.altKey == true) { //run code for Ctrl+X -- ie, Save & Exit!   
                console.log(event);
                StopDefaultAction(e);
                var debit = parseFloat(c_txt_Debit.GetValue());
                var credit = parseFloat(c_txt_Credit.GetValue());
                if ((debit == credit) && (debit != 0) && (credit != 0)) {
                    document.getElementById('btn_SaveRecords').click();
                    //SaveExitButtonClick();
                    return false;
                }
            }
            else if (event.keyCode == 65 && event.altKey == true) {
                StopDefaultAction(e);
                if (document.getElementById('divAddNew').style.display != 'block') {
                    OnAddButtonClick();
                }
            }
            else if (event.keyCode == 84 && event.altKey == true) {
                StopDefaultAction(e);
                if (document.getElementById('divAddNewTDS').style.display != 'block') {
                    OnAddButtonClickTDS();
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
            window.location.reload();
        }

        var isFirstTime = true;
        function AllControlInitilize() {
            //document.getElementById('AddButton').style.display = 'inline-block';
            if (isFirstTime) {

                if (localStorage.getItem('FromDateJournal')) {
                    var fromdatearray = localStorage.getItem('FromDateJournal').split('-');
                    var fromdate = new Date(fromdatearray[0], parseFloat(fromdatearray[1]) - 1, fromdatearray[2], 0, 0, 0, 0);
                    cFormDate.SetDate(fromdate);
                }

                if (localStorage.getItem('ToDateJournal')) {
                    var todatearray = localStorage.getItem('ToDateJournal').split('-');
                    var todate = new Date(todatearray[0], parseFloat(todatearray[1]) - 1, todatearray[2], 0, 0, 0, 0);
                    ctoDate.SetDate(todate);
                }
                if (localStorage.getItem('BranchJournal')) {
                    if (ccmbBranchfilter.FindItemByValue(localStorage.getItem('BranchJournal'))) {
                        ccmbBranchfilter.SetValue(localStorage.getItem('BranchJournal'));
                    }

                }
                TDScheckchange(null, null);
                //updateGridByDate();

                isFirstTime = false;
            }
        }

        //Function for Date wise filteration
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
                localStorage.setItem("FromDateJournal", cFormDate.GetDate().format('yyyy-MM-dd'));
                localStorage.setItem("ToDateJournal", ctoDate.GetDate().format('yyyy-MM-dd'));
                localStorage.setItem("BranchJournal", ccmbBranchfilter.GetValue());

                $("#hfFromDate").val(cFormDate.GetDate().format('yyyy-MM-dd'));
                $("#hfToDate").val(ctoDate.GetDate().format('yyyy-MM-dd'));
                $("#hfBranchID").val(ccmbBranchfilter.GetValue());
                $("#hfIsFilter").val("Y");

                if (page.activeTabIndex == 0) {

                    //rev 2.0
                    // cGvJvSearch.Refresh();
                    $("#hFilterType").val("All");
                    cCallbackPanel.PerformCallback("");
                    //end rev 2.0

                    //cGvJvSearch.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue());
                }
                else if (page.activeTabIndex == 1) {
                    cGvJvSearchFullInfo.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue());
                }
            }
        }


        //// Pop Up /////

        //rev 2.0
        function CallbackPanelEndCall(s, e) {
            cGvJvSearch.Refresh();
        }
        //end rev 2.0

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

                callonServer("/OMS/Management/Activities/Services/Master.asmx/GetMainAccountJournal", OtherDetails, "MainAccountTable", HeaderCaption, "MainAccountIndex", "SetMainAccount");
            }
            else if (e.code == "ArrowDown") {
                if ($("input[MainAccountIndex=0]"))
                    $("input[MainAccountIndex=0]").focus();
            }
            else if (e.code == "Escape") {
                //  
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
        }

        function MainAccountNewkeydownTDS(e) {
            // $("#hdnTDSval").val("");
            var OtherDetails = {}
            OtherDetails.SearchKey = $("#txtMainAccountSearchTDS").val();
            OtherDetails.branchId = $("#ddlBranchTDS").val();
            OtherDetails.TDSCode = ccmbtds.GetValue();
            OtherDetails.considerTDS = chkTDSJournal.GetChecked();
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






        function SubAccountNewkeydown(e) {
            grid.batchEditApi.StartEdit(e.visibleIndex);
            var strMainAccountID = (grid.GetEditor('MainAccount').GetText() != null) ? grid.GetEditor('MainAccount').GetText() : "0";
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
                grid.batchEditApi.StartEdit(globalRowIndex, 2);
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


        function SetMainAccount(Id, name, e) {

            $('#MainAccountModel').modal('hide');
            var Code = e.parentElement.cells[2].innerText;
            var IsSub = e.parentElement.cells[3].innerText;


            GetMainAcountComboBox(Id, name, Code, IsSub);


            grid.batchEditApi.StartEdit(globalRowIndex, 2);

        }

        function SetMainAccountTDS(Id, name, e) {

            $('#MainAccountModelTDS').modal('hide');
            var Code = e.parentElement.cells[2].innerText;
            var IsSub = e.parentElement.cells[3].innerText;
            var IsTDS = e.parentElement.cells[4].innerText;
            $("#hdnTDSval").val(IsTDS);


            GetMainAcountComboBoxTDS(Id, name, Code, IsSub, IsTDS);





            gridTDS.batchEditApi.StartEdit(globalRowIndexTDS, 2);

        }


        function SetSubAccount(Id, name) {
            $('#SubAccountModel').modal('hide');
            GetSubAcountComboBox(Id, name);
            grid.batchEditApi.StartEdit(globalRowIndex, 3);
        }


        function SetSubAccountTDS(Id, name) {
            $('#SubAccountModelTDS').modal('hide');
            GetSubAcountComboBoxTDS(Id, name);
            gridTDS.batchEditApi.StartEdit(globalRowIndexTDS, 3);
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



        function journalledger(keyid, docno) {

      <%--      $('#<%= lblHeading.ClientID %>').text("VIew Journal Voucher");
            $('#<%=hdnMode.ClientID %>').val('1');
            document.getElementById('div_Edit').style.display = 'none';
            document.getElementById('<%= txtBillNo.ClientID %>').disabled = true;
            document.getElementById('hdnMode').value = "VIEW";
            document.getElementById('divAddNew').style.display = 'block';
            //   btncross.style.display = "block";
            TblSearch.style.display = "none";
            cbtnSaveRecords.SetVisible(false);
            cbtn_SaveRecords.SetVisible(false);
            grid.PerformCallback('ViewLeger~' + keyid);--%>



            ///  VisibleIndexE = e.visibleIndex;
            $('#<%= lblHeading.ClientID %>').text("View Journal Voucher");
            $('#<%=hdnMode.ClientID %>').val('1');
            document.getElementById('div_Edit').style.display = 'none';
            document.getElementById('<%= txtBillNo.ClientID %>').disabled = true;

            document.getElementById('divAddNew').style.display = 'block';
            //  btncross.style.display = "block";
            TblSearch.style.display = "none";
            //cGvJvSearch.GetRowValues(VisibleIndexE, "BranchID", MainAccountCallBack);
            grid.PerformCallback('View~' + keyid + '~' + docno);
            //grid.PerformCallback('Edit~' + keyid);

            document.getElementById('tblBtnSavePanel').style.display = 'none';
            LoadingPanel.Show();


        }

        function checkIsPartyjornal() {
            var RepeatedRow = [];
            var validgrid = true;


            for (var i = 0; i < 1000; i++) {
                if (grid.GetRow(i)) {
                    if (grid.GetRow(i).style.display != "none") {
                        grid.batchEditApi.StartEdit(i, 2);
                        if ((grid.GetEditor("gvColMainAccount").GetText().trim() != "") && (grid.GetEditor("gvColSubAccount").GetText().trim() != "") && (grid.GetEditor("gvMainAcCode").GetText().trim() == "Customers" || grid.GetEditor("gvMainAcCode").GetText().trim() == "Vendors")) {
                            var RepeatedRowCount = {};
                            RepeatedRowCount.MainAccount = grid.GetEditor("gvColMainAccount").GetText().trim();
                            RepeatedRowCount.SubAccount = grid.GetEditor("gvColSubAccount").GetText().trim();
                            RepeatedRowCount.IsSubLedger = grid.GetEditor("IsSubledger").GetText().trim();
                            RepeatedRow.push(RepeatedRowCount);
                        }

                    }
                }
            }

            for (i = -1; i > -1000; i--) {
                if (grid.GetRow(i)) {
                    if (grid.GetRow(i).style.display != "none") {
                        grid.batchEditApi.StartEdit(i, 2);
                        if ((grid.GetEditor("gvColMainAccount").GetText().trim() != "") && (grid.GetEditor("gvColSubAccount").GetText().trim() != "") && (grid.GetEditor("gvMainAcCode").GetText().trim() == "Customers" || grid.GetEditor("gvMainAcCode").GetText().trim() == "Vendors")) {
                            var RepeatedRowCount = {};
                            RepeatedRowCount.MainAccount = grid.GetEditor("gvColMainAccount").GetText().trim();
                            RepeatedRowCount.SubAccount = grid.GetEditor("gvColSubAccount").GetText().trim();
                            RepeatedRowCount.IsSubLedger = grid.GetEditor("IsSubledger").GetText().trim();
                            RepeatedRow.push(RepeatedRowCount);
                        }
                    }
                }
            }
            if (parseInt(grid.GetVisibleRowsOnPage()) == "2") {
                if (RepeatedRow.length == 2) {
                    return true;
                }
                else {
                    return false;
                }

            }
            else {
                return false;
            }

        }


    </script>

    <script>
        function gridRowclick(s, e) {
            $('#GvJvSearch').find('tr').removeClass('rowActive');
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
                        $(value).css({ 'opacity': '1' });
                    }, 100);
                });
            }, 200);
        }
        $(document).ready(function () {
            //Toggle fullscreen expandEntryGrid
            $("#expandcGvJvSearch").click(function (e) {
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
                    cGvJvSearch.SetHeight(browserHeight - 150);
                    cGvJvSearch.SetWidth(cntWidth);
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
                    cGvJvSearch.SetHeight(300);
                    var cntWidth = $this.parent('.makeFullscreen').width();
                    cGvJvSearch.SetWidth(cntWidth);
                }
            });
            $("#expandgrid").click(function (e) {
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
                    grid.SetHeight(browserHeight - 150);
                    grid.SetWidth(cntWidth);
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
                    grid.SetHeight(300);
                    var cntWidth = $this.parent('.makeFullscreen').width();
                    grid.SetWidth(cntWidth);
                }
            });
            $("#expandgridTDS").click(function (e) {
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
                    gridTDS.SetHeight(browserHeight - 150);
                    gridTDS.SetWidth(cntWidth);
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
                    gridTDS.SetHeight(300);
                    var cntWidth = $this.parent('.makeFullscreen').width();
                    gridTDS.SetWidth(cntWidth);
                }
            });
        });

    </script>

    <script>
        function clookup_Project_LostFocus() {
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
                url: 'JournalEntry.aspx/getHierarchyID',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ ProjID: projID }),
                success: function (msg) {
                    var data = msg.d;
                    $("#ddlHierarchy").val(data);
                }
            });
        }

        function clookup_Project_LostFocusTDS() {
            var projID = clookupTDS_Project.GetValue();
            if (projID == null || projID == "") {
                $("#ddlHierarchyTDS").val(0);
            }
        }

        function ProjectValueChangeTDS(s, e) {
            //debugger;
            var projID = clookupTDS_Project.GetValue();
            $.ajax({
                type: "POST",
                url: 'JournalEntry.aspx/getHierarchyID',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ ProjID: projID }),
                success: function (msg) {
                    var data = msg.d;
                    $("#ddlHierarchyTDS").val(data);
                }
            });
        }
    </script>
    <script>
        function OverheadCost_gotFocus() {
            //clookup_GRNOverhead.gridView.Refresh();
            var startDate = new Date();
            startDate = tDate.GetDate().format('yyyy-MM-dd');
            //cPanellookup_GRNOverhead.PerformCallback('BindOverheadCostGrid' + '~' + startDate);
            clookup_GRNOverhead.ShowDropDown();
        }

        function OverheadCostTDS_gotFocus() {
            //clookup_GRNOverheadTDS.gridView.Refresh();
            clookup_GRNOverheadTDS.ShowDropDown();
        }

        function chkIsSalary_Change() {
            if (chkIsSalary.GetChecked()) {
                chkNILRateTDS.SetChecked(false);
            }
        }

        function chkNILRateTDS_Change() {
            if (chkNILRateTDS.GetChecked()) {
                chkIsSalary.SetChecked(false);
                chkTDSJournal.SetChecked(false);
                chkTDSJournal.SetEnabled(false);
            }
            else {
                chkTDSJournal.SetEnabled(true);
            }
        }
        $(document).ready(function () {
            //var text = clookup_GRNOverhead.GetText();
            //$("#divOverheadCost").attr("title", text);
            $("body").tooltip({
                selector: '#divOverheadCost',
                template: '<div class="tooltip CUSTOM-CLASS" role="tooltip"><div class="arrow"></div><div class="tooltip-inner"></div></div>'
            });
        });
        function OverHeadcomponentEndCallBack() {
            var text = clookup_GRNOverhead.GetText();
            $("#divOverheadCost").attr("title", text);
            $("#divOverheadCost").attr("data-original-title", text);
            $("#divOverheadCost").tooltip({
                customClass: 'tooltip-custom',
                template: '<div class="tooltip CUSTOM-CLASS" role="tooltip"><div class="arrow"></div><div class="tooltip-inner"></div></div>'
            });
        }
    </script>
    <link href="CSS/JournalEntry.css" rel="stylesheet" />
    <style>
        .CUSTOM-CLASS .tooltip-inner {
            background: #268c7e
        }
    </style>

    <style>
        /*Rev 3.0*/


        select {
            height: 30px !important;
            border-radius: 4px;
            /*-webkit-appearance: none;*/
            position: relative;
            z-index: 1;
            background-color: transparent;
            padding-left: 10px !important;
            padding-right: 22px !important;
        }

        .simple-select select {
            -webkit-appearance: none;
        }

        .dxeButtonEditSys.dxeButtonEdit_PlasticBlue, .dxeTextBox_PlasticBlue {
            height: 30px;
            border-radius: 4px;
        }

        .dxeButtonEditButton_PlasticBlue {
            background: #094e8c !important;
            border-radius: 4px !important;
            padding: 0 4px !important;
        }

        .calendar-icon {
            position: absolute;
            bottom: 6px;
            right: 10px;
            z-index: 0;
            cursor: pointer;
        }

        .right-20 {
            right: 20px !important;
        }

        #ASPxFromDate, #ASPxToDate, #ASPxASondate, #ASPxAsOnDate, #FormDate, #toDate, #tDate {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        .dxeDisabled_PlasticBlue {
            z-index: 0 !important;
        }

        #ASPxFromDate_B-1, #ASPxToDate_B-1, #ASPxASondate_B-1, #ASPxAsOnDate_B-1, #FormDate_B-1, #toDate_B-1, #tDate_B-1 {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

            #ASPxFromDate_B-1 #ASPxFromDate_B-1Img, #ASPxToDate_B-1 #ASPxToDate_B-1Img, #ASPxASondate_B-1 #ASPxASondate_B-1Img, #ASPxAsOnDate_B-1 #ASPxAsOnDate_B-1Img,
            #FormDate_B-1 #FormDate_B-1Img, #toDate_B-1 #toDate_B-1Img, #tDate_B-1 #tDate_B-1Img {
                display: none;
            }

        .dxtcLite_PlasticBlue > .dxtc-stripContainer .dxtc-activeTab, .dxgvFooter_PlasticBlue {
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
            line-height: 19px;
            z-index: 0;
        }

        .simple-select {
            position: relative;
            z-index: 0;
        }

            .simple-select:disabled::after {
                background: #1111113b;
            }

        select.btn {
            padding-right: 10px !important;
        }

        .panel-group .panel {
            box-shadow: 1px 1px 8px #1111113b;
            border-radius: 8px;
        }

        .dxpLite_PlasticBlue .dxp-current {
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

        #ShowGrid {
            margin-top: 10px;
        }

        .pt-25 {
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

        .dxgvEditFormDisplayRow_PlasticBlue td.dxgv, .dxgvDataRow_PlasticBlue td.dxgv, .dxgvDataRowAlt_PlasticBlue td.dxgv, .dxgvSelectedRow_PlasticBlue td.dxgv, .dxgvFocusedRow_PlasticBlue td.dxgv {
            padding: 6px 6px 6px !important;
        }

        #lookupCardBank_DDD_PW-1 {
            left: -182px !important;
        }

        .plhead a > i {
            top: 9px;
        }

        .clsTo {
            display: flex;
            align-items: flex-start;
        }

        input[type="radio"], input[type="checkbox"] {
            margin-right: 5px;
        }

        .dxeCalendarDay_PlasticBlue {
            padding: 6px 6px;
        }

        .modal-dialog {
            width: 50%;
        }

        .modal-header {
            padding: 8px 4px 8px 10px;
            background: #094e8c !important;
        }

        /*.TableMain100 #ShowGrid , .TableMain100 #ShowGridList , .TableMain100 #ShowGridRet , .TableMain100 #ShowGridLocationwiseStockStatus 
        {
            max-width: 98% !important;
        }*/

        /*div.dxtcSys > .dxtc-content > div, div.dxtcSys > .dxtc-content > div > div
        {
            width: 95% !important;
        }*/

        .btn-info {
            background-color: #1da8d1 !important;
            background-image: none;
        }

        .for-cust-icon {
            position: relative;
            z-index: 1;
        }

        .dxeDisabled_PlasticBlue, .aspNetDisabled {
            background: #f3f3f3 !important;
        }

        .dxeButtonDisabled_PlasticBlue {
            background: #b5b5b5 !important;
            border-color: #b5b5b5 !important;
        }

        #ddlValTech {
            width: 100% !important;
            margin-bottom: 0 !important;
        }

        .dis-flex {
            display: flex;
            align-items: baseline;
        }

        input + label {
            line-height: 1;
            margin-top: 3px;
        }

        .dxtlHeader_PlasticBlue {
            background: #094e8c !important;
        }

        .dxeBase_PlasticBlue .dxichCellSys {
            padding-top: 2px !important;
        }

        .pBackDiv {
            border-radius: 10px;
            box-shadow: 1px 1px 10px #1111112e;
        }

        .HeaderStyle th {
            padding: 5px;
        }

        .for-cust-icon {
            position: relative;
            z-index: 1;
        }

        .dxtcLite_PlasticBlue.dxtc-top > .dxtc-stripContainer {
            padding-top: 15px;
        }

        .pt-2 {
            padding-top: 5px;
        }

        .pt-10 {
            padding-top: 10px;
        }

        .pt-15 {
            padding-top: 15px;
        }

        .pb-10 {
            padding-bottom: 10px;
        }

        .pTop10 {
            padding-top: 20px;
        }

        .custom-padd {
            padding-top: 4px;
            padding-bottom: 10px;
        }

        input + label {
            margin-right: 10px;
        }

        .btn {
            margin-bottom: 0;
        }

        .pl-10 {
            padding-left: 10px;
        }

        .col-md-3 > label, .col-md-3 > span {
            margin-top: 0 !important;
        }

        .devCheck {
            margin-top: 5px;
        }

        .mtc-5 {
            margin-top: 5px;
        }

        .mtc-10 {
            margin-top: 10px;
        }

        select.btn {
            position: relative;
            z-index: 0;
        }

        select {
            margin-bottom: 0;
        }

        .form-control {
            background-color: transparent;
        }

        select.btn-radius {
            padding: 4px 8px 6px 11px !important;
        }

        .mt-30 {
            margin-top: 30px;
        }

        .makeFullscreen {
            z-index: 0;
        }

        .panel-fullscreen {
            z-index: 99 !important;
        }

        .crossBtn {
            right: 25px;
            top: 25px;
        }

        #txtBillNo {
            margin-bottom: 0;
        }

        .col-md-3 {
            margin-bottom: 10px;
        }

        .lblmTop8 > span, .lblmTop8 > label {
            margin-top: 0 !important;
        }

        /*@media only screen and (max-width: 1444px) and (min-width: 1150px)
        {
            #gridFilter
            {
                    margin-top: 20px !important;
            }
        }*/
        /*Rev end 3.0*/
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 3.0: "outer-div-main" class add --%>
    <div class="outer-div-main">
        <div class="panel-heading">
            <div class="panel-title clearfix">
                <h3 class="clearfix pull-left" style="padding: 0;">
                    <span class="pull-left" style="margin-top: 8px;">
                        <asp:Label ID="lblHeading" runat="server" Text="Journal Voucher"></asp:Label></span>

                    <span id="loadCurrencyMassage" class="pull-left" style="display: none;">
                        <label><span style="color: red; font-weight: bold; font-size: medium;">**  Mismatch detected.</span></label>
                    </span>

                    <span id="pageheaderContent" class="pull-right reverse wrapHolder content horizontal-images" style="width: 230px">
                        <div class="Top clearfix">
                            <ul>
                                <%--<li>
                                <div id="divContactPhone" class="lblHolder" style="display: none;">
                                    <table>
                                        <tbody>
                                            <tr>
                                                <td>Contact Person's Phone</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span id="lblContactPhone" class="classout">N/A</span></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </li>--%>
                                <li>
                                    <div id="divIsPartyJournal" class="lblHolder" style="display: none">
                                        <table>
                                            <tbody>
                                                <tr>
                                                    <td>Is Party Journal?</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="lblGSTIN">Yes</span>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                </li>
                            </ul>
                        </div>
                    </span>
                </h3>

                <div id="btncross" class="crossBtn" runat="server" style="display: none; margin-left: 50px;"><a href="javascript:ReloadPage()"><i class="fa fa-times"></i></a></div>
                <%-- <div id="btncross" style="display: none; margin-left: 50px;" class="crossBtn CloseBtn">
                <asp:ImageButton ID="imgClose" runat="server" ImageUrl="/assests/images/CrIcon.png" OnClick="imgClose_Click" />
            </div>--%>
            </div>
        </div>
        <div class="form_main">
            <div id="TblSearch" class="clearfix">
                <div class="clearfix">
                    <div style="padding-right: 5px;">
                        <% if (rights.CanAdd)
                            { %>
                        <%--Rev 3.0: "btn-radius" class removed--%>
                        <a href="javascript:void(0);" onclick="OnAddButtonClick()" class="btn btn-success "><span class="btn-icon"><i class="fa fa-plus"></i></span><span>Journal</span> </a>
                        <a href="javascript:void(0);" onclick="OnAddButtonClickTDS()" class="btn btn-success"><span class="btn-icon"><i class="fa fa-plus"></i></span><span><u>T</u>DS Journal</span> </a>
                        <% } %>

                        <% if (rights.CanExport)
                            { %>
                        <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary " OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                            <asp:ListItem Value="0">Export to</asp:ListItem>
                            <asp:ListItem Value="1">PDF</asp:ListItem>
                            <asp:ListItem Value="2">XLS</asp:ListItem>
                            <asp:ListItem Value="3">RTF</asp:ListItem>
                            <asp:ListItem Value="4">CSV</asp:ListItem>
                        </asp:DropDownList>
                        <% } %>
                        <%--Rev end 3.0--%>
                        <table class="padTabtype2 pull-right" id="gridFilter">
                            <tr>
                                <td>
                                    <label>From Date</label></td>
                                <%--Rev 3.0: "for-cust-icon" class add --%>
                                <td class="for-cust-icon">
                                    <dxe:ASPxDateEdit ID="FormDate" runat="server" AllowNull="false" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDate" Width="100%">
                                        <ButtonStyle Width="13px">
                                        </ButtonStyle>
                                    </dxe:ASPxDateEdit>
                                    <%--Rev 3.0--%>
                                    <img src="/assests/images/calendar-icon.png" class="calendar-icon" />
                                    <%--Rev end 3.0--%>
                                </td>
                                <td>
                                    <label>To Date</label>
                                </td>
                                <%--Rev 3.0: "for-cust-icon" class add --%>
                                <td class="for-cust-icon">
                                    <dxe:ASPxDateEdit ID="toDate" runat="server" AllowNull="false" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDate" Width="100%">
                                        <ButtonStyle Width="13px">
                                        </ButtonStyle>
                                    </dxe:ASPxDateEdit>
                                    <%--Rev 3.0--%>
                                    <img src="/assests/images/calendar-icon.png" class="calendar-icon" />
                                    <%--Rev end 3.0--%>
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
                <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" ClientInstanceName="page"
                    Font-Size="12px" Width="100%">
                    <TabPages>
                        <dxe:TabPage Name="AdvanceRec" Text="Journal Summary">
                            <ContentCollection>
                                <dxe:ContentControl runat="server">
                                    <div class="clearfix relative">
                                        <div class="makeFullscreen ">
                                            <span class="fullScreenTitle">Purchase Indent/Requisition</span>
                                            <span class="makeFullscreen-icon half hovered " data-instance="cGvJvSearch" title="Maximize Grid" id="expandcGvJvSearch">
                                                <i class="fa fa-expand"></i>
                                            </span>
                                            <%--Rev 5.0--%>
                                            <div id="spnEditLock" runat="server" style="display: none; color: red; text-align: center"></div>
                                            <div id="spnDeleteLock" runat="server" style="display: none; color: red; text-align: center"></div>
                                            <%--End of Rev 5.0--%>
                                            <dxe:ASPxGridView ID="GvJvSearch" SettingsBehavior-AllowFocusedRow="true" runat="server" AutoGenerateColumns="False" SettingsBehavior-AllowSort="true"
                                                ClientInstanceName="cGvJvSearch" KeyFieldName="VoucherNumber" Width="100%"
                                                OnCustomCallback="GvJvSearch_CustomCallback" OnCustomButtonInitialize="GvJvSearch_CustomButtonInitialize" DataSourceID="EntityServerModeDataSource">
                                                <ClientSideEvents CustomButtonClick="CustomButtonClick" EndCallback="function(s, e) {GvJvSearch_EndCallBack();}" />
                                                <SettingsBehavior ConfirmDelete="True" ColumnResizeMode="NextColumn" />
                                                <SettingsSearchPanel Visible="True" Delay="5000" />
                                                <Styles>
                                                    <Header SortingImageSpacing="5px" ImageSpacing="5px"></Header>
                                                    <FocusedRow HorizontalAlign="Left" VerticalAlign="Top" CssClass="gridselectrow"></FocusedRow>
                                                    <LoadingPanel ImageSpacing="10px"></LoadingPanel>
                                                    <FocusedGroupRow CssClass="gridselectrow"></FocusedGroupRow>
                                                    <Footer CssClass="gridfooter"></Footer>
                                                </Styles>
                                                <Columns>
                                                    <%-- --Rev Sayantani--%>
                                                    <%--<dxe:GridViewDataTextColumn Visible="False" VisibleIndex="0" FieldName="JvID" Caption="JvID" SortOrder="Descending">
                                                <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                            </dxe:GridViewDataTextColumn>--%>
                                                    <dxe:GridViewDataTextColumn Visible="False" ShowInCustomizationForm="false" VisibleIndex="0" FieldName="JvID" Caption="JvID" SortOrder="Descending">
                                                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <%-- --Rev Sayantani--%>
                                                    <%-- 0024170 uncomment <PropertiesTextEdit DisplayFormatString="dd/MM/yyyy"></PropertiesTextEdit>--%>
                                                    <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="JournalVoucher_TransactionDate" FixedStyle="Left" Caption="Posting Date" Width="10%">
                                                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                                        <PropertiesTextEdit DisplayFormatString="dd-MM-yyyy"></PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="BillNumber" FixedStyle="Left" Caption="Document No." Width="15%">
                                                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <%-- Rev Sayantani--%>
                                                    <%--<dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="VoucherNumber" FixedStyle="Left" Caption="JV Number" Visible="false">
                                                <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                            </dxe:GridViewDataTextColumn>--%>
                                                    <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="VoucherNumber" FixedStyle="Left" Caption="JV Number" Visible="false" ShowInCustomizationForm="false">
                                                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <%-- End of Rev Sayantani--%>
                                                    <%-- Rev Sayantani--%>
                                                    <%--  <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="BranchNameCode" Width="20%" Caption="Unit" Visible="false">
                                                <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                            </dxe:GridViewDataTextColumn>--%>
                                                    <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="BranchNameCode" Width="20%" Caption="Unit" Visible="false" ShowInCustomizationForm="false">
                                                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <%-- End of Rev Sayantani--%>
                                                    <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="Narration" Caption="Narration" Width="35%" Settings-AllowAutoFilter="False">
                                                        <CellStyle CssClass="gridcellleft"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="JournalVoucher_CreateUser" Caption="Entered By" Width="8%" Settings-AllowAutoFilter="False">
                                                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="JournalVoucher_ModifyDateTime" Caption="Last Update On" Width="15%" Settings-AllowAutoFilter="False">
                                                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                                        <PropertiesTextEdit DisplayFormatString="dd-MM-yyyy hh:mm:ss"></PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn VisibleIndex="8" FieldName="JournalVoucher_ModifyUser" Caption="Updated By" Width="10%" Settings-AllowAutoFilter="False">
                                                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <%-- Rev Sayantani--%>
                                                    <%-- <dxe:GridViewDataTextColumn Visible="False" FieldName="IBRef"></dxe:GridViewDataTextColumn>--%>
                                                    <dxe:GridViewDataTextColumn Visible="False" ShowInCustomizationForm="false" FieldName="IBRef"></dxe:GridViewDataTextColumn>
                                                    <%--  Rev Sayantani --%>
                                                    <%-- Rev Sayantani--%>
                                                    <%-- <dxe:GridViewDataTextColumn Visible="False" FieldName="BranchID"></dxe:GridViewDataTextColumn>--%>
                                                    <dxe:GridViewDataTextColumn Visible="False" ShowInCustomizationForm="false" FieldName="BranchID"></dxe:GridViewDataTextColumn>
                                                    <%--  Rev Sayantani--%>
                                                    <%--Rev Sayantani--%>
                                                    <%--<dxe:GridViewDataTextColumn Visible="False" FieldName="WhichTypeItem"></dxe:GridViewDataTextColumn>--%>
                                                    <dxe:GridViewDataTextColumn Visible="False" ShowInCustomizationForm="false" FieldName="WhichTypeItem"></dxe:GridViewDataTextColumn>
                                                    <%-- Rev Sayantani--%>
                                                    <dxe:GridViewDataTextColumn Width="0" Visible="False" FieldName="IsTDS"></dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn Width="0" Visible="False" FieldName="visible"></dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn VisibleIndex="9" FieldName="Proj_Name" Caption="Project Name" Width="15%" Settings-AllowAutoFilter="true">
                                                        <CellStyle CssClass="gridcellleft"></CellStyle>
                                                        <Settings AllowAutoFilterTextInputTimer="True" />
                                                    </dxe:GridViewDataTextColumn>
                                                    <%--      <dxe:GridViewCommandColumn VisibleIndex="10" Width="13%" ButtonType="Image" Caption="Actions">
                                                <CustomButtons>--%>
                                                    <%--  <dxe:GridViewCommandColumnCustomButton ID="CustomBtnView" Image-ToolTip="View" Styles-Style-CssClass="pad">
                                                        <Image Url="/assests/images/viewIcon.png"></Image>
                                                    </dxe:GridViewCommandColumnCustomButton>--%>

                                                    <%--   <dxe:GridViewCommandColumnCustomButton ID="CustomBtnEdit"  Image-ToolTip="Edit" Styles-Style-CssClass="pad">
                                                        <Image Url="/assests/images/Edit.png"></Image>
                                                    </dxe:GridViewCommandColumnCustomButton>--%>

                                                    <%-- <dxe:GridViewCommandColumnCustomButton ID="CustomBtnDelete"   Image-ToolTip="Delete" Styles-Style-CssClass="pad">
                                                        <Image Url="/assests/images/Delete.png"></Image>
                                                    </dxe:GridViewCommandColumnCustomButton>--%>
                                                    <%--Print Journal Voucher--%>
                                                    <%--<dxe:GridViewCommandColumnCustomButton ID="CustomBtnPrint" Image-ToolTip="Print" Styles-Style-CssClass="pad">
                                                        <Image Url="/assests/images/Print.png"></Image>
                                                    </dxe:GridViewCommandColumnCustomButton>--%>
                                                    <%--End Print Journal Voucher--%>
                                                    <%--</CustomButtons>
                                            </dxe:GridViewCommandColumn>--%>

                                                    <dxe:GridViewDataTextColumn Caption="" VisibleIndex="10" Width="0">
                                                        <CellStyle HorizontalAlign="Center">
                                                        </CellStyle>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <HeaderTemplate>
                                                        </HeaderTemplate>
                                                        <DataItemTemplate>
                                                            <div class='floatedBtnArea'>
                                                                <% if (rights.CanView)
                                                                    { %>
                                                                <a href="javascript:void(0);" onclick="OnView('<%# Container.VisibleIndex %>')" class="">
                                                                    <span class='ico ColorThree'><i class='fa fa-eye'></i></span><span class='hidden-xs'>View</span></a>
                                                                <%} %>
                                                                <%--Rev 5.0 [ style='<%#Eval("Editlock")%>' added ]--%>
                                                                <% if (rights.CanEdit)
                                                                    { %>
                                                                <a href="javascript:void(0);" onclick="OnEdit('<%# Container.VisibleIndex %>','<%#Eval("IsTDS") %>','<%#Eval("visible") %>','<%#Eval("visible_RETENTION") %>','<%#Eval("JvID") %>')" class=""
                                                                    style='<%#Eval("Editlock")%>'>
                                                                    <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Edit</span></a>
                                                                <%} %>
                                                                <%--Rev Work start: Copy Feature required for Journal Vouchers Date:-27.05.2022 Mantise no:24911--%>
                                                                <% if (rights.CanAdd)
                                                                    { %>
                                                                <a href="javascript:void(0);" onclick="OnCopy('<%# Container.VisibleIndex %>','<%#Eval("IsTDS") %>','<%#Eval("visible") %>','<%#Eval("visible_RETENTION") %>','<%#Eval("JvID") %>')" class="">
                                                                    <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Copy</span></a>
                                                                <%} %>
                                                                <%--Rev 5.0 [ style='<%#Eval("Deletelock")%>'> added ]  --%>
                                                                <%--Rev Work close: Copy Feature required for Journal Vouchers Date:-27.05.2022 Mantise no:24911--%>
                                                                <% if (rights.CanDelete)
                                                                    { %>
                                                                <a href="javascript:void(0);" onclick="OnGetRowValuesOnDelete('<%# Container.VisibleIndex %>','<%#Eval("IsTDS") %>','<%#Eval("visible_RETENTION") %>','<%#Eval("JvID") %>')"
                                                                    class="" style='<%#Eval("Deletelock")%>'>
                                                                    <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>Delete</span></a>
                                                                <%} %>
                                                                <% if (rights.CanPrint)
                                                                    { %>
                                                                <a href="javascript:void(0);" onclick="onPrint('<%# Container.VisibleIndex %>')" class="pad" title="">
                                                                    <span class='ico ColorFour'><i class='fa fa-print'></i></span><span class='hidden-xs'>Print</span>
                                                                </a><%} %>
                                                            </div>
                                                        </DataItemTemplate>
                                                    </dxe:GridViewDataTextColumn>




                                                </Columns>
                                                <%-- --Rev Sayantani--%>
                                                <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" />
                                                <SettingsCookies Enabled="true" StorePaging="true" StoreColumnsVisiblePosition="false" />
                                                <%-- -- End of Rev Sayantani --%>
                                                <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />
                                                <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                                                <SettingsSearchPanel Visible="true" />
                                                <ClientSideEvents RowClick="gridRowclick" />
                                                <SettingsPager NumericButtonCount="10" PageSize="10" Position="Bottom" ShowSeparators="True" Mode="ShowPager">
                                                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                                                    <FirstPageButton Visible="True">
                                                    </FirstPageButton>
                                                    <LastPageButton Visible="True">
                                                    </LastPageButton>
                                                </SettingsPager>
                                            </dxe:ASPxGridView>
                                            <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
                                                ContextTypeName="ERPDataClassesDataContext" TableName="v_JournalEntryList" />
                                        </div>
                                    </div>
                                </dxe:ContentControl>
                            </ContentCollection>
                        </dxe:TabPage>

                        <dxe:TabPage Name="FullJournalRecord" Text="Journal Details">
                            <ContentCollection>
                                <dxe:ContentControl runat="server">
                                    <div class="clearfix">
                                        <dxe:ASPxGridView ID="GridFullInfo" runat="server" AutoGenerateColumns="False" SettingsBehavior-AllowSort="true"
                                            ClientInstanceName="cGvJvSearchFullInfo" KeyFieldName="JvID" Width="100%"
                                            OnCustomCallback="GridFullInfo_CustomCallback" OnCustomButtonInitialize="GridFullInfo_CustomButtonInitialize" OnDataBinding="GridFullInfo_DataBinding" OnSummaryDisplayText="GridFullInfo_SummaryDisplayText">
                                            <ClientSideEvents EndCallback="function(s, e) {GridFullInfo_EndCallBack();}" />
                                            <SettingsBehavior ConfirmDelete="True" ColumnResizeMode="NextColumn" />

                                            <Styles>
                                                <Header SortingImageSpacing="5px" ImageSpacing="5px"></Header>
                                                <FocusedRow HorizontalAlign="Left" VerticalAlign="Top" CssClass="gridselectrow"></FocusedRow>
                                                <LoadingPanel ImageSpacing="10px"></LoadingPanel>
                                                <FocusedGroupRow CssClass="gridselectrow"></FocusedGroupRow>
                                                <Footer CssClass="gridfooter"></Footer>
                                            </Styles>
                                            <SettingsPager NumericButtonCount="10" PageSize="10" ShowSeparators="True" Mode="ShowPager">
                                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                                                <FirstPageButton Visible="True">
                                                </FirstPageButton>
                                                <LastPageButton Visible="True">
                                                </LastPageButton>
                                            </SettingsPager>
                                            <Columns>

                                                <dxe:GridViewDataTextColumn VisibleIndex="0" FieldName="JV_DATE" Caption="Posting Date">
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="JV_NO" Width="150px" Caption="Document No.">
                                                    <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                                </dxe:GridViewDataTextColumn>

                                                <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="MainAccount" Width="150px" Caption="Ledger Desc.">
                                                    <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="SubAccount" Width="150px" Caption="Subledger Desc.">
                                                    <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                                </dxe:GridViewDataTextColumn>


                                                <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="JV_NARRATION" Width="300px" Caption="NARRATION">
                                                    <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="Proj_Name" Width="300px" Caption="Project Name" Settings-AllowAutoFilter="True">
                                                    <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                                    <Settings AllowAutoFilterTextInputTimer="true" />
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="JV_DR_AMT" Caption="Debit Amount">
                                                    <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="JV_CR_AMT" Caption="Credit Amount">
                                                    <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                                </dxe:GridViewDataTextColumn>

                                                <dxe:GridViewDataTextColumn VisibleIndex="8" Visible="true" FieldName="CGSTRate" Caption="CGST Rate">
                                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn Visible="true" VisibleIndex="9" FieldName="CGSTRate" Caption="CGST Rate">
                                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn Visible="true" VisibleIndex="10" FieldName="IGSTRate" Caption="IGST Rate">
                                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                                </dxe:GridViewDataTextColumn>

                                                <dxe:GridViewDataTextColumn Visible="true" VisibleIndex="11" FieldName="UTGSTRate" Caption="UTGST Rate">
                                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                                </dxe:GridViewDataTextColumn>



                                                <dxe:GridViewDataTextColumn VisibleIndex="12" FieldName="CGSTAmount" Caption="CGST Amt">
                                                    <CellStyle CssClass="gridcellleft"></CellStyle>
                                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn VisibleIndex="13" FieldName="SGSTAmount" Caption="SGST Amt">
                                                    <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn VisibleIndex="14" FieldName="IGSTAmount" Caption="IGST Amt">
                                                    <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn VisibleIndex="15" FieldName="UTGSTAmount" Caption="UTGST Amt">
                                                    <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                                </dxe:GridViewDataTextColumn>

                                                <dxe:GridViewDataTextColumn VisibleIndex="16" FieldName="RCM" Caption="RCM">
                                                    <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                                </dxe:GridViewDataTextColumn>

                                                <dxe:GridViewDataTextColumn VisibleIndex="17" FieldName="ITC" Caption="ITC">
                                                    <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <%--Rev 4.0--%>
                                                <dxe:GridViewDataTextColumn VisibleIndex="18" FieldName="EnteredOn" Caption="Entered On">
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn VisibleIndex="19" FieldName="EnteredBy" Caption="Entered By">
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn VisibleIndex="20" FieldName="ModifiedOn" Caption="Modified On">
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn VisibleIndex="21" FieldName="ModifiedBy" Caption="Modified By">
                                                </dxe:GridViewDataTextColumn>
                                                <%--End of Rev 4.0--%>
                                            </Columns>
                                            <Settings ShowFooter="true" ShowColumnHeaders="true" ShowFilterRow="true" ShowGroupFooter="VisibleIfExpanded" />
                                            <TotalSummary>
                                                <dxe:ASPxSummaryItem FieldName="JV_DR_AMT" SummaryType="Sum" />
                                                <dxe:ASPxSummaryItem FieldName="JV_CR_AMT" SummaryType="Sum" />
                                                <dxe:ASPxSummaryItem FieldName="CGSTAmount" SummaryType="Sum" />
                                                <dxe:ASPxSummaryItem FieldName="SGSTAmount" SummaryType="Sum" />
                                                <dxe:ASPxSummaryItem FieldName="IGSTAmount" SummaryType="Sum" />
                                                <dxe:ASPxSummaryItem FieldName="UTGSTAmount" SummaryType="Sum" />
                                            </TotalSummary>
                                            <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" HorizontalScrollBarMode="Visible" />
                                            <SettingsSearchPanel Visible="True" />
                                        </dxe:ASPxGridView>
                                    </div>
                                </dxe:ContentControl>
                            </ContentCollection>
                        </dxe:TabPage>
                    </TabPages>
                </dxe:ASPxPageControl>




            </div>
            <div id="divAddNew" class="clearfix" style="display: none">
                <div class="clearfix">
                </div>
                <div style="padding: 8px 0; margin-bottom: 0px; border-radius: 4px;" class="clearfix">
                    <div class="col-md-3" id="div_Edit">
                        <label>Select Numbering Scheme</label>
                        <%--Rev 3.0 : "simple-select" class add--%>
                        <div class="simple-select">
                            <%-- <dxe:ASPxComboBox ID="CmbScheme" EnableIncrementalFiltering="True" ClientInstanceName="cCmbScheme" DataSourceID="SqlSchematype"
                            TextField="SchemaName" ValueField="ID" TabIndex="1" SelectedIndex="0"
                            runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True">
                            <ClientSideEvents ValueChanged="function(s,e){CmbScheme_ValueChange()}"></ClientSideEvents>
                        </dxe:ASPxComboBox>--%>
                            <%--Rev Work start: Copy Feature required for Journal Vouchers Date:-27.05.2022 Mantise no:24911--%>
                            <%--  <asp:DropDownList ID="CmbScheme" runat="server" DataSourceID="SqlSchematype"
                            DataTextField="SchemaName" DataValueField="ID" Width="100%"
                            onchange="CmbScheme_ValueChange()">
                        </asp:DropDownList>--%>
                            <asp:DropDownList ID="CmbScheme" runat="server"
                                DataTextField="SchemaName" DataValueField="ID" Width="100%" onchange="CmbScheme_ValueChange()">
                            </asp:DropDownList>
                            <%--Rev Work close: Copy Feature required for Journal Vouchers Date:-27.05.2022 Mantise no:24911--%>
                            <%-- <asp:RadioButtonList ID="rblScheme" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table" Width="100%"
                            onclick="rblScheme_ValueChange()">
                        </asp:RadioButtonList>--%>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <label>Document No.</label>
                        <div>
                            <asp:TextBox ID="txtBillNo" runat="server" Width="100%" meta:resourcekey="txtBillNoResource1" MaxLength="30" onchange="txtBillNo_TextChanged()"></asp:TextBox>
                            <span id="MandatoryBillNo" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                            <span id="duplicateMandatoryBillNo" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Duplicate Journal No."></span>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <label style="">Posting Date</label>
                        <div>
                            <%--Rev 5.0 [ LostFocus="function(s, e) { SetLostFocusonDemand(e)}" added ]--%>
                            <dxe:ASPxDateEdit ID="tDate" runat="server" EditFormat="Custom" ClientInstanceName="tDate"
                                UseMaskBehavior="True" Width="100%" meta:resourcekey="tDateResource1">
                                <ClientSideEvents DateChanged="function(s,e){DateChange()}" LostFocus="function(s, e) { SetLostFocusonDemand(e)}" />
                            </dxe:ASPxDateEdit>
                            <%--Rev 3.0--%>
                            <img src="/assests/images/calendar-icon.png" class="calendar-icon right-20" />
                            <%--Rev end 3.0--%>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <label>Unit</label>
                        <%--Rev 3.0 : "simple-select" class add--%>
                        <div class="simple-select">
                            <asp:DropDownList ID="ddlBranch" runat="server" DataSourceID="dsBranch" Enabled="false"
                                DataTextField="BANKBRANCH_NAME" DataValueField="BANKBRANCH_ID" Width="100%"
                                meta:resourcekey="ddlBranchResource1" onchange="ddlBranch_ChangeIndex()" onfocus="BranchGotFocus()">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <label>Place Of Supply</label>
                        <%--Rev 3.0 : "simple-select" class add--%>
                        <div class="simple-select">
                            <asp:DropDownList ID="ddlSupplyState" runat="server" DataSourceID="dsSupplyState"
                                DataTextField="state_name" DataValueField="state_id" Width="100%">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <label>Amounts are</label>
                        <%--Rev 3.0 : "simple-select" class add--%>
                        <div class="simple-select">
                            <asp:DropDownList ID="ddl_AmountAre" runat="server" DataSourceID="dsTaxType"
                                DataTextField="taxGrp_Description" DataValueField="taxGrp_Id" Width="100%">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <label></label>
                        <div style="padding-top: 4px;">
                            <dxe:ASPxCheckBox ID="IsRcm" ClientInstanceName="IsRcm" Checked="false" Text="Reverse Mechanism" TextAlign="Right" runat="server">
                                <ClientSideEvents CheckedChanged="RcmCheckChange" />
                            </dxe:ASPxCheckBox>
                        </div>
                    </div>
                    <div class="col-md-3" id="divOverheadCost" runat="server">
                        <label id="Label27" runat="server">Overhead Cost</label>
                        <dxe:ASPxCallbackPanel runat="server" ID="Panellookup_GRNOverhead" ClientInstanceName="cPanellookup_GRNOverhead" OnCallback="Panellookup_GRNOverhead_Callback">
                            <PanelCollection>
                                <dxe:PanelContent runat="server">
                                    <asp:HiddenField runat="server" ID="OldSelectedKeyvalue" />
                                    <dxe:ASPxGridLookup ID="lookup_GRNOverhead" SelectionMode="Multiple" runat="server" ClientInstanceName="clookup_GRNOverhead"
                                        OnDataBinding="lookup_GRNOverhead_DataBinding"
                                        KeyFieldName="PurchaseChallan_Id" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                        <Columns>
                                            <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                            <dxe:GridViewDataColumn FieldName="PurchaseChallan_Number" Visible="true" VisibleIndex="1" Caption="GRN" Settings-AutoFilterCondition="Contains" Width="150" />
                                            <dxe:GridViewDataColumn FieldName="cnt_firstName" Visible="true" VisibleIndex="2" Caption="Vendor Name" Settings-AutoFilterCondition="Contains" Width="150" />
                                            <dxe:GridViewDataColumn FieldName="branch_description" Visible="true" VisibleIndex="3" Caption="Unit" Settings-AutoFilterCondition="Contains" Width="150" />
                                            <dxe:GridViewDataTextColumn Caption="Total Amount" FieldName="Challan_TotalAmount" Width="150" VisibleIndex="4" HeaderStyle-HorizontalAlign="Right">
                                                <PropertiesTextEdit>
                                                    <MaskSettings Mask="<0..999999999999>.<0..9999>" AllowMouseWheel="false" />
                                                </PropertiesTextEdit>
                                            </dxe:GridViewDataTextColumn>
                                        </Columns>
                                        <GridViewProperties Settings-VerticalScrollBarMode="Auto">
                                            <Templates>
                                                <StatusBar>
                                                    <table class="OptionsTable" style="float: right">
                                                        <tr>
                                                            <td>
                                                                <%--<dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridQuotationLookup" />--%>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </StatusBar>
                                            </Templates>
                                            <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" />
                                            <SettingsPager Mode="ShowPager" PageSize="10" Visible="true">
                                                <PageSizeItemSettings Items="10,20,30"></PageSizeItemSettings>
                                            </SettingsPager>
                                            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                        </GridViewProperties>
                                        <ClientSideEvents GotFocus="function(s, e) { OverheadCost_gotFocus();}" LostFocus="function(s, e) { OverHeadcomponentEndCallBack();}" />
                                    </dxe:ASPxGridLookup>
                                    <%--<dx:LinqServerModeDataSource ID="EntityServerModeDataOverheadCost" runat="server" OnSelecting="EntityServerModeDataOverheadCost_Selecting"
                                    ContextTypeName="ERPDataClassesDataContext" TableName="v_OverHeadCostPurchaseServiceInvoice" DataSourceID="EntityServerModeDataOverheadCost" />
                                    --%>
                                </dxe:PanelContent>
                            </PanelCollection>
                            <ClientSideEvents EndCallback="componentEndCallBack" />
                        </dxe:ASPxCallbackPanel>
                    </div>
                    <%--  Rev Sayantani--%>
                    <div class="col-md-2 lblmTop8">
                        <label id="lblProject" runat="server">Project</label>
                        <dxe:ASPxGridLookup ID="lookup_Project" runat="server" ClientInstanceName="clookup_Project" DataSourceID="EntityServerModeDataJournal"
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
                            <ClientSideEvents GotFocus="function(s,e){clookup_Project.ShowDropDown();}" LostFocus="clookup_Project_LostFocus" ValueChanged="ProjectValueChange" />

                        </dxe:ASPxGridLookup>
                        <dx:LinqServerModeDataSource ID="EntityServerModeDataJournal" runat="server" OnSelecting="EntityServerModeDataJournal_Selecting"
                            ContextTypeName="ERPDataClassesDataContext" TableName="V_ProjectList" />
                    </div>
                    <%--End of Rev Sayantani--%>

                    <div class="col-md-4 lblmTop8">
                        <%-- <label id="Label1" runat="server">Hierarchy</label>--%>
                        <label>
                            <dxe:ASPxLabel ID="lblHierarchy" runat="server" Text="Hierarchy">
                            </dxe:ASPxLabel>
                        </label>
                        <asp:DropDownList ID="ddlHierarchy" runat="server" Width="100%">
                        </asp:DropDownList>
                    </div>


                </div>
                <div class="clearfix">
                    <br />
                    <div class="makeFullscreen ">
                        <span class="fullScreenTitle">Add Journal Voucher</span>
                        <span class="makeFullscreen-icon half hovered " data-instance="grid" title="Maximize Grid" id="expandgrid">
                            <i class="fa fa-expand"></i>
                        </span>
                        <dxe:ASPxGridView runat="server" OnBatchUpdate="grid_BatchUpdate" KeyFieldName="CashReportID" ClientInstanceName="grid" ID="grid"
                            Width="100%" OnCellEditorInitialize="grid_CellEditorInitialize" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false"
                            Settings-ShowFooter="false" OnCustomCallback="grid_CustomCallback" OnDataBinding="grid_DataBinding"
                            OnRowInserting="Grid_RowInserting" OnRowUpdating="Grid_RowUpdating" OnRowDeleting="Grid_RowDeleting"
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
                                        <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomDelete" Image-Url="/assests/images/crs.png">
                                        </dxe:GridViewCommandColumnCustomButton>
                                    </CustomButtons>
                                </dxe:GridViewCommandColumn>

                                <dxe:GridViewDataButtonEditColumn FieldName="MainAccount" Caption="Main Account" VisibleIndex="1">

                                    <PropertiesButtonEdit>

                                        <ClientSideEvents ButtonClick="MainAccountButnClick" KeyDown="MainAccountKeyDown" />
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



                                <dxe:GridViewDataButtonEditColumn FieldName="bthSubAccount" Caption="Sub Account" VisibleIndex="2">
                                    <PropertiesButtonEdit>
                                        <ClientSideEvents ButtonClick="SubAccountButnClick" KeyDown="SubAccountKeyDown" />
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




                                <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="WithDrawl" Caption="Debit" Width="120" EditCellStyle-HorizontalAlign="Right">
                                    <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                        <MaskSettings Mask="&lt;0..99999999999&gt;.&lt;00..99&gt;" />
                                        <ClientSideEvents KeyDown="OnKeyDown" LostFocus="WithDrawlTextChange"
                                            GotFocus="function(s,e){
                                    DebitGotFocus(s,e);
                                    }" />

                                        <ClientSideEvents />
                                        <ValidationSettings Display="None"></ValidationSettings>
                                    </PropertiesTextEdit>
                                    <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="Receipt" Caption="Credit" Width="120">
                                    <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                        <MaskSettings Mask="&lt;0..99999999999&gt;.&lt;00..99&gt;" />
                                        <ClientSideEvents KeyDown="OnKeyDown" LostFocus="ReceiptTextChange"
                                            GotFocus="function(s,e){
                                    CreditGotFocus(s,e);
                                    }" />
                                        <ClientSideEvents />
                                        <ValidationSettings Display="None"></ValidationSettings>
                                    </PropertiesTextEdit>
                                    <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>


                                <dxe:GridViewDataButtonEditColumn FieldName="Project_Code" Caption="Project Code" VisibleIndex="5" Width="14%" ReadOnly="true">
                                    <PropertiesButtonEdit>
                                        <ClientSideEvents ButtonClick="ProjectCodeButnClick" KeyDown="ProjectCodeKeyDown" GotFocus="ProjectCodeGotFocus" />
                                        <Buttons>
                                            <dxe:EditButton Text="..." Width="20px">
                                            </dxe:EditButton>
                                        </Buttons>
                                    </PropertiesButtonEdit>
                                </dxe:GridViewDataButtonEditColumn>



                                <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="Narration" Caption="Narration">
                                    <PropertiesTextEdit>
                                        <ClientSideEvents KeyDown="AddBatchNew"></ClientSideEvents>
                                    </PropertiesTextEdit>
                                    <CellStyle Wrap="False" HorizontalAlign="Left" CssClass="gridcellleft"></CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="CashReportID" Caption="Srl No" ReadOnly="true" HeaderStyle-CssClass="hide" Width="0">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="gvColMainAccount" Caption="hidden Field Id" Width="0" HeaderStyle-CssClass="hide">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="gvColSubAccount" Caption="hidden Field Id" Width="0" HeaderStyle-CssClass="hide">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="gvMainAcCode" Caption="hidden Field Id" Width="0" HeaderStyle-CssClass="hide">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="IsSubledger" Caption="hidden Field Id" Width="0" HeaderStyle-CssClass="hide">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="ProjectId" Caption="ProjectId" ReadOnly="True" Width="0"
                                    EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide"
                                    PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                                    <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                                </dxe:GridViewDataTextColumn>

                            </Columns>
                            <TotalSummary>
                                <dxe:ASPxSummaryItem SummaryType="Sum" FieldName="C2" Tag="C2_Sum" />
                            </TotalSummary>
                            <Settings ShowStatusBar="Hidden" />
                            <ClientSideEvents Init="OnInit" EndCallback="OnEndCallback" BatchEditStartEditing="OnBatchEditStartEditing" BatchEditEndEditing="OnBatchEditEndEditing"
                                CustomButtonClick="OnCustomButtonClick" RowClick="GetVisibleIndex" />
                            <SettingsDataSecurity AllowEdit="true" />


                            <SettingsEditing Mode="Batch" NewItemRowPosition="Bottom">
                                <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="row" />
                            </SettingsEditing>
                        </dxe:ASPxGridView>
                    </div>
                </div>
                <div class="text-center">
                    <table class="padTabtype2 pull-right" id="TotalAmount" style="margin-right: 12px; margin-top: 5px;">
                        <tr>
                            <td style="padding-right: 5px">Total Debit</td>
                            <td style="padding-right: 30px">
                                <dxe:ASPxTextBox ID="txt_Debit" runat="server" Width="105px" ClientInstanceName="c_txt_Debit" HorizontalAlign="Right" Font-Size="12px" ReadOnly="true">
                                    <MaskSettings Mask="&lt;0..999999999999999&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                </dxe:ASPxTextBox>
                            </td>
                            <td style="padding-right: 5px">Total Credit</td>
                            <td>
                                <dxe:ASPxTextBox ID="txt_Credit" runat="server" Width="105px" ClientInstanceName="c_txt_Credit" HorizontalAlign="Right" Font-Size="12px" ReadOnly="true">
                                    <MaskSettings Mask="&lt;0..999999999999999&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                </dxe:ASPxTextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="clearfix" style="background: #f5f4f3; padding: 8px 0; margin-bottom: 15px; border-radius: 4px; border: 1px solid #ccc; margin-top: 42px;">
                    <div class="col-md-12">
                        <label>Main Narration</label>
                        <div>
                            <asp:TextBox ID="txtNarration" Font-Names="Arial" runat="server" TextMode="MultiLine"
                                Width="100%" onkeyup="OnlyNarration(this,'Narration',event)" meta:resourcekey="txtNarrationResource1" Height="40px"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div>
                    <asp:Label ID="lbl_quotestatusmsg" runat="server" Text="" Font-Bold="true" ForeColor="Red" Font-Size="Medium"></asp:Label>
                </div>
                <table style="float: left;" id="tblBtnSavePanel">

                    <tr>

                        <td style="width: 100px;" id="tdSaveButton" runat="Server">

                            <dxe:ASPxButton ID="btnSaveRecords" ClientInstanceName="cbtnSaveRecords" runat="server" AutoPostBack="False" Text="S&#818;ave & New" CssClass="btn btn-success" UseSubmitBehavior="False">
                                <ClientSideEvents Click="function(s, e) {SaveButtonClick();}" />
                            </dxe:ASPxButton>
                        </td>
                        <td style="width: 100px;" id="td_SaveButton" runat="Server">
                            <dxe:ASPxButton ID="btn_SaveRecords" ClientInstanceName="cbtn_SaveRecords" runat="server" AutoPostBack="False" Text="Save & Ex&#818;it" CssClass="btn btn-primary" UseSubmitBehavior="False">
                                <ClientSideEvents Click="function(s, e) {SaveExitButtonClick();}" />
                            </dxe:ASPxButton>
                        </td>
                        <td style="width: 100px">
                            <dxe:ASPxButton ID="btnDiscardEntry" runat="server" AccessKey="D" AllowFocus="False" Visible="false"
                                AutoPostBack="False" Text="D&#818;iscard Entered Records" CssClass="btn btn-primary" meta:resourcekey="btnDiscardEntryResource1" UseSubmitBehavior="False">
                                <ClientSideEvents Click="function(s, e) {DiscardButtonClick();}" />
                            </dxe:ASPxButton>
                        </td>
                        <td id="tdadd" style="width: 100px">
                            <dxe:ASPxButton ID="btnadd" ClientInstanceName="cbtnadd" runat="server" AccessKey="L" AutoPostBack="False" ClientVisible="false"
                                Text="Add Entry To L&#818;ist" CssClass="btn btn-primary" meta:resourcekey="btnaddResource1" Visible="false" UseSubmitBehavior="False">
                                <ClientSideEvents Click="function(s, e) {SubAccountCheck();}" />
                            </dxe:ASPxButton>
                        </td>
                        <td id="tdnew" style="width: 100px; height: 16px; display: none">
                            <dxe:ASPxButton ID="btnnew" ClientInstanceName="cbtnnew" runat="server" AutoPostBack="False" Text="N&#818;ew Entry" ClientVisible="false"
                                CssClass="btn btn-primary" AccessKey="N" Font-Bold="False" Font-Underline="False" BackColor="Tan" meta:resourcekey="btnnewResource1" UseSubmitBehavior="False">
                                <ClientSideEvents Click="function(s, e) {NewButtonClick();}" />
                            </dxe:ASPxButton>
                        </td>
                        <td style="width: 100px">
                            <dxe:ASPxButton ID="btnCancelEntry" runat="server" AccessKey="C" AutoPostBack="False" Text="C&#818;ancel Entry" CssClass="btn btn-primary" meta:resourcekey="btnCancelEntryResource1" ClientVisible="false" UseSubmitBehavior="False">
                                <ClientSideEvents Click="function(s, e) {CancelButtonClick();}" />
                            </dxe:ASPxButton>
                        </td>

                        <td style="width: 100px">
                            <dxe:ASPxButton ID="btnUnsaveData" runat="server" AccessKey="R" AutoPostBack="False" Text="R&#818;efresh" CssClass="btn btn-primary" meta:resourcekey="btnUnsaveDataResource1" ClientVisible="false" UseSubmitBehavior="False">
                                <ClientSideEvents Click="function(s, e) {RefreshButtonClick();}" />
                            </dxe:ASPxButton>
                        </td>

                    </tr>
                </table>

            </div>

            <div>

                <%--New block for TDS Journal--%>


                <div id="divAddNewTDS" class="clearfix" style="display: none">
                    <div class="clearfix">
                    </div>
                    <div style="background: #f5f4f3; padding: 8px 0; margin-bottom: 0px; border-radius: 4px; border: 1px solid #ccc;" class="clearfix">
                        <div class="col-md-3" id="div_EditTDS">
                            <label>Select Numbering Scheme</label>
                            <div>
                                <%-- <dxe:ASPxComboBox ID="CmbScheme" EnableIncrementalFiltering="True" ClientInstanceName="cCmbScheme" DataSourceID="SqlSchematype"
                            TextField="SchemaName" ValueField="ID" TabIndex="1" SelectedIndex="0"
                            runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True">
                            <ClientSideEvents ValueChanged="function(s,e){CmbScheme_ValueChange()}"></ClientSideEvents>
                        </dxe:ASPxComboBox>--%>
                                <asp:DropDownList ID="CmbSchemeTDS" runat="server" DataSourceID="SqlSchematype"
                                    DataTextField="SchemaName" DataValueField="ID" Width="100%"
                                    onchange="CmbSchemeTDS_ValueChange()">
                                </asp:DropDownList>
                                <%-- <asp:RadioButtonList ID="rblScheme" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table" Width="100%"
                            onclick="rblScheme_ValueChange()">
                        </asp:RadioButtonList>--%>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <label>Document No.</label>
                            <div>
                                <asp:TextBox ID="txtBillNoTDS" runat="server" Width="100%" meta:resourcekey="txtBillNoResource1" MaxLength="30" onchange="txtBillNoTDS_TextChanged()"></asp:TextBox>
                                <span id="MandatoryBillNoTDS" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                <span id="duplicateMandatoryBillNoTDS" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Duplicate Journal No."></span>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <label style="">Posting Date</label>
                            <div>
                                <%--Rev 5.0 [ LostFocus="function(s, e) { SetTDSLostFocusonDemand(e)}" added ]--%>
                                <dxe:ASPxDateEdit ID="tDateTDS" runat="server" EditFormat="Custom" ClientInstanceName="tDateTDS" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                                    UseMaskBehavior="True" Width="100%" meta:resourcekey="tDateResource1">
                                    <ClientSideEvents DateChanged="function(s,e){TDSDateChange()}" LostFocus="function(s, e) { SetTDSLostFocusonDemand(e)}" />
                                </dxe:ASPxDateEdit>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <label>Unit</label>
                            <div>
                                <asp:DropDownList ID="ddlBranchTDS" runat="server" DataSourceID="dsBranch" Enabled="false"
                                    DataTextField="BANKBRANCH_NAME" DataValueField="BANKBRANCH_ID" Width="100%"
                                    meta:resourcekey="ddlBranchResource1" onchange="ddlBranch_ChangeIndex()" onfocus="BranchGotFocus()">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <label>Place Of Supply</label>
                            <div>
                                <asp:DropDownList ID="ddlSupplyStateTDS" runat="server" DataSourceID="dsSupplyState"
                                    DataTextField="state_name" DataValueField="state_id" Width="100%">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <label>Amounts are</label>
                            <div>
                                <asp:DropDownList ID="ddl_AmountAreTDS" runat="server" DataSourceID="dsTaxType"
                                    DataTextField="taxGrp_Description" DataValueField="taxGrp_Id" Width="100%">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <label></label>
                            <div style="padding-top: 12px;">
                                <dxe:ASPxCheckBox ID="IsRcmTDS" ClientInstanceName="IsRcmTDS" Checked="false" Text="Reverse Mechanism" TextAlign="Right" runat="server">
                                    <ClientSideEvents CheckedChanged="RcmCheckChangeTDS" />
                                </dxe:ASPxCheckBox>
                            </div>

                        </div>
                        <div class="col-md-3">
                            <label></label>
                            <div style="padding-top: 12px;">
                                <dxe:ASPxCheckBox ID="chkIsSalary" ClientInstanceName="chkIsSalary" Checked="false" Text="Salary Voucher ?" TextAlign="Right" runat="server">
                                    <ClientSideEvents CheckedChanged="function(s, e) { chkIsSalary_Change();}" />
                                </dxe:ASPxCheckBox>
                            </div>
                        </div>
                        <div class="clear"></div>
                        <div class="col-md-3">
                            <label></label>
                            <div style="padding-top: 12px;">
                                <dxe:ASPxCheckBox ID="chkTDSJournal" ClientInstanceName="chkTDSJournal" Checked="false" Text="Consider as TDS journal?" TextAlign="Right" runat="server">
                                    <ClientSideEvents CheckedChanged="TDScheckchange" />
                                </dxe:ASPxCheckBox>
                            </div>
                        </div>
                        <div runat="server" id="tds">
                            <div class="col-md-3">
                                <label>TDS Section</label>

                                <div class="relative">
                                    <dxe:ASPxComboBox ID="cmbtds" ClientInstanceName="ccmbtds" Width="100%" runat="server" ValueType="System.String" class="form-control" ValueField="tdscode" TextField="tdsdescription"
                                        EnableIncrementalFiltering="true" DataSourceID="DTtds">
                                    </dxe:ASPxComboBox>
                                    <asp:SqlDataSource ID="DTtds" SelectCommand="Select  ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' as tdsdescription ,ltrim(rtrim(tdstcs_code)) tdscode 
					from master_tdstcs tds inner join (SELECT TDSTCSRates_Code from Config_MULTITDSTCSRates GROUP BY TDSTCSRates_Code) rate on rate.TDSTCSRates_Code=tds.TDSTCS_Code
					order by tdsdescription"
                                        runat="server"></asp:SqlDataSource>
                                </div>
                            </div>
                            <div class="col-md-2">
                                <div style="margin-top: 35px;">
                                    <%--<asp:CheckBox ID="chkNILRateTDS" runat="server" Text="NIL rate TDS?" TextAlign="Right"></asp:CheckBox>--%>
                                    <dxe:ASPxCheckBox ID="chkNILRateTDS" ClientInstanceName="chkNILRateTDS" Checked="false" Text="NIL TDS ?" TextAlign="Right" runat="server">
                                        <ClientSideEvents CheckedChanged="function(s, e) { chkNILRateTDS_Change();}" />
                                    </dxe:ASPxCheckBox>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-3">
                            <label>TDS Amount</label>
                            <div>
                                <dxe:ASPxTextBox ID="txtTDSAmount" ClientInstanceName="ctxtTDSAmount" DisplayFormatString="0.00" TextAlign="right" runat="server" Width="100%">
                                    <MaskSettings Mask="&lt;0..99999999999&gt;.&lt;00..99&gt;" />
                                </dxe:ASPxTextBox>
                            </div>
                        </div>
                        <div class="clear"></div>
                        <div class="col-md-3" id="div1" runat="server">
                            <label id="Label1" runat="server">Overhead Cost</label>
                            <dxe:ASPxCallbackPanel runat="server" ID="Panellookup_GRNOverheadTDS" ClientInstanceName="cPanellookup_GRNOverheadTDS" OnCallback="Panellookup_GRNOverheadTDS_Callback">
                                <PanelCollection>
                                    <dxe:PanelContent runat="server">
                                        <asp:HiddenField runat="server" ID="HiddenField1" />
                                        <dxe:ASPxGridLookup ID="lookup_GRNOverheadTDS" SelectionMode="Multiple" AllowUserInput="false" runat="server" ClientInstanceName="clookup_GRNOverheadTDS"
                                            OnDataBinding="lookup_GRNOverheadTDS_DataBinding"
                                            KeyFieldName="PurchaseChallan_Id" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                            <Columns>
                                                <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                                <dxe:GridViewDataColumn FieldName="PurchaseChallan_Number" Visible="true" VisibleIndex="1" Caption="GRN" Settings-AutoFilterCondition="Contains" Width="150px" />
                                                <dxe:GridViewDataColumn FieldName="cnt_firstName" Visible="true" VisibleIndex="2" Caption="Vendor Name" Settings-AutoFilterCondition="Contains" Width="150px" />
                                                <dxe:GridViewDataColumn FieldName="branch_description" Visible="true" VisibleIndex="3" Caption="Unit" Settings-AutoFilterCondition="Contains" Width="150px" />
                                                <dxe:GridViewDataTextColumn Caption="Total Amount" FieldName="Challan_TotalAmount" Width="150" VisibleIndex="4" HeaderStyle-HorizontalAlign="Right">
                                                    <PropertiesTextEdit>
                                                        <MaskSettings Mask="<0..999999999999>.<0..9999>" AllowMouseWheel="false" />
                                                    </PropertiesTextEdit>
                                                </dxe:GridViewDataTextColumn>
                                            </Columns>
                                            <GridViewProperties Settings-VerticalScrollBarMode="Auto">
                                                <Templates>
                                                    <StatusBar>
                                                        <table class="OptionsTable" style="float: right">
                                                            <tr>
                                                                <td>
                                                                    <%--<dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridQuotationLookup" />--%>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </StatusBar>
                                                </Templates>
                                                <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" />
                                                <SettingsPager Mode="ShowPager" PageSize="10" Visible="true">
                                                    <PageSizeItemSettings Items="10,20,30"></PageSizeItemSettings>
                                                </SettingsPager>
                                                <%--  <SettingsPager Mode="ShowAllRecords">
                                            </SettingsPager>--%>
                                                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                            </GridViewProperties>
                                            <ClientSideEvents GotFocus="OverheadCostTDS_gotFocus" />
                                        </dxe:ASPxGridLookup>

                                        <%-- <dx:LinqServerModeDataSource ID="EntityServerModeDataOverheadCostTDS" runat="server" OnSelecting="EntityServerModeDataOverheadCostTDS_Selecting"
                            ContextTypeName="ERPDataClassesDataContextTDS" TableName="v_OverHeadCostPurchaseServiceInvoice" DataSourceID="EntityServerModeDataOverheadCostTDS"/>--%>
                                    </dxe:PanelContent>
                                </PanelCollection>
                                <ClientSideEvents EndCallback="PanelGRNOverheadTDSEndCallBack" />
                            </dxe:ASPxCallbackPanel>
                        </div>



                        <div class="col-md-3">
                            <label id="lbl_ProjectTDS" runat="server" style="margin-top: 0">Project </label>
                            <div>
                                <dxe:ASPxGridLookup ID="lookupTDS_Project" runat="server" ClientInstanceName="clookupTDS_Project" DataSourceID="EntityServerModeDataSourceProjectForTDS"
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
                                    <ClientSideEvents GotFocus="function(s,e){clookupTDS_Project.ShowDropDown();}" LostFocus="clookup_Project_LostFocusTDS" ValueChanged="ProjectValueChangeTDS" />

                                </dxe:ASPxGridLookup>
                                <dx:LinqServerModeDataSource ID="EntityServerModeDataSourceProjectForTDS" runat="server" OnSelecting="EntityServerModeDataSourceProjectForTDS_Selecting"
                                    ContextTypeName="ERPDataClassesDataContext" TableName="V_ProjectLists" />
                            </div>
                        </div>
                        <div class="col-md-4">
                            <%-- <label id="Label1" runat="server">Hierarchy</label>--%>
                            <label>
                                <dxe:ASPxLabel ID="lblHierarchyTDS" runat="server" Text="Hierarchy">
                                </dxe:ASPxLabel>
                            </label>
                            <asp:DropDownList ID="ddlHierarchyTDS" runat="server" Width="100%">
                            </asp:DropDownList>
                        </div>


                        <div class="col-md-3 hide">
                            <dxe:ASPxCheckBox ID="chkNullrated" ClientInstanceName="cIsNullRatedTDS" Checked="false" Text="Null Rated TDS" TextAlign="Right" runat="server">
                            </dxe:ASPxCheckBox>
                        </div>

                    </div>
                    <div class="clearfix">
                        <br />
                        <div class="makeFullscreen ">

                            <span class="makeFullscreen-icon half hovered " data-instance="gridTDS" title="Maximize Grid" id="expandgridTDS">
                                <i class="fa fa-expand"></i>
                            </span>
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




                                    <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="WithDrawlTDS" Caption="Debit" Width="120" EditCellStyle-HorizontalAlign="Right">
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
                                    <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="ReceiptTDS" Caption="Credit" Width="120">
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
                                            <ClientSideEvents ButtonClick="ProjectCodeTDSButnClick" KeyDown="ProjectCodeTDSKeyDown" LostFocus="ProjectInlineLost_Focus" />
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

                                    <dxe:GridViewDataTextColumn FieldName="ProjectId" Caption="ProjectId" ReadOnly="True" Width="0"
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
                        </div>
                    </div>
                    <div class="text-center">
                        <table class="padTabtype2 pull-right" id="TotalAmountTDS" style="margin-right: 12px; margin-top: 5px;">
                            <tr>
                                <td style="padding-right: 5px">Total Debit</td>
                                <td style="padding-right: 30px">
                                    <dxe:ASPxTextBox ID="txt_DebitTDS" runat="server" Width="105px" ClientInstanceName="c_txt_DebitTDS" HorizontalAlign="Right" Font-Size="12px" ReadOnly="true">
                                        <MaskSettings Mask="&lt;0..999999999999999&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                    </dxe:ASPxTextBox>
                                </td>
                                <td style="padding-right: 5px">Total Credit</td>
                                <td>
                                    <dxe:ASPxTextBox ID="txt_CreditTDS" runat="server" Width="105px" ClientInstanceName="c_txt_CreditTDS" HorizontalAlign="Right" Font-Size="12px" ReadOnly="true">
                                        <MaskSettings Mask="&lt;0..999999999999999&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                    </dxe:ASPxTextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="clearfix" style="background: #f5f4f3; padding: 8px 0; margin-bottom: 15px; border-radius: 4px; border: 1px solid #ccc; margin-top: 36px;">
                        <div class="col-md-12">
                            <label>Main Narration</label>
                            <div>
                                <asp:TextBox ID="txtNarrationTDS" Font-Names="Arial" runat="server" TextMode="MultiLine"
                                    Width="100%" onkeyup="OnlyNarrationTDS(this,'Narration',event)" meta:resourcekey="txtNarrationResource1" Height="40px"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div>
                        <asp:Label ID="lbl_quotestatusmsgTDS" runat="server" Text="" Font-Bold="true" ForeColor="Red" Font-Size="Medium"></asp:Label>
                    </div>
                    <table style="float: left;" id="tblBtnSavePanelTDS">

                        <tr>

                            <td style="width: 100px;" id="tdSaveButtonTDS" runat="Server">

                                <dxe:ASPxButton ID="btnSaveRecordsTDS" ClientInstanceName="cbtnSaveRecordsTDS" runat="server" AutoPostBack="False" Text="S&#818;ave & New" CssClass="btn btn-primary" UseSubmitBehavior="False">
                                    <ClientSideEvents Click="function(s, e) {SaveButtonClickTDS();}" />
                                </dxe:ASPxButton>
                            </td>
                            <td style="width: 100px;" id="td2" runat="Server">
                                <dxe:ASPxButton ID="btn_SaveRecordsTDS" ClientInstanceName="cbtn_SaveRecordsTDS" runat="server" AutoPostBack="False" Text="Save & Ex&#818;it" CssClass="btn btn-primary" UseSubmitBehavior="False">
                                    <ClientSideEvents Click="function(s, e) {SaveExitButtonClickTDS();}" />
                                </dxe:ASPxButton>
                            </td>
                        </tr>
                    </table>

                </div>


                <%--End New Block for TDS Jourrnal--%>
            </div>



















        </div>
    </div>
    <div id="DvDataSource">
        <asp:SqlDataSource ID="dsBranch" runat="server" ConflictDetection="CompareAllValues"
            SelectCommand="SELECT BRANCH_id AS BANKBRANCH_ID , RTRIM(BRANCH_DESCRIPTION)+' ['+ISNULL(RTRIM(BRANCH_CODE),'')+']' AS BANKBRANCH_NAME  FROM TBL_MASTER_BRANCH"></asp:SqlDataSource>
        <asp:SqlDataSource ID="dsSupplyState" runat="server" ConflictDetection="CompareAllValues"
            SelectCommand="select id as state_id,state+' (State Code:'+StateCode+')' as state_name   from tbl_master_state where ISNULL(state+' (State Code:'+StateCode+')','')<>''  order by state_name"></asp:SqlDataSource>
        <asp:SqlDataSource ID="dsTaxType" runat="server" ConflictDetection="CompareAllValues"
            SelectCommand="select taxGrp_Id,taxGrp_Description from tbl_master_taxgrouptype  order by taxGrp_Description"></asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDataSourceMainAccount" runat="server"
            SelectCommand=""></asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDataSourceSubAccount" runat="server"
            SelectCommand=""></asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlSchematype" runat="server"
            SelectCommand="Select * From ((Select '0' as ID,'Select' as SchemaName) Union (Select ID,SchemaName  + 
            (Case When (SELECT Isnull(branch_description,'') FROM tbl_master_branch where branch_id=Branch) Is Null Then '' 
            Else ' ('+(SELECT Isnull(branch_description,'') FROM tbl_master_branch where branch_id=Branch)+')' End) From tbl_master_Idschema  Where TYPE_ID='1' AND IsActive=1 AND Isnull(Branch,'') in (select s FROM dbo.GetSplit(',',@userbranchHierarchy)) AND Isnull(comapanyInt,'')=@LastCompany AND financial_year_id=(Select Top 1 FinYear_ID FROM Master_FinYear WHERE FinYear_Code=@LastFinYear))) as x Order By ID asc">
            <SelectParameters>
                <asp:SessionParameter Name="LastCompany" SessionField="LastCompany" />
                <asp:SessionParameter Name="LastFinYear" SessionField="LastFinYear" />
                <asp:SessionParameter Name="userbranchHierarchy" SessionField="userbranchHierarchy" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
    <div id="HiddenFeild">
        <asp:HiddenField ID="hdnSegmentid" runat="server" />
        <asp:HiddenField ID="hdnMode" runat="server" />
        <asp:HiddenField ID="hdnSchemaType" runat="server" />
        <asp:HiddenField ID="hdnSchemaTypeTDS" runat="server" />

        <asp:HiddenField ID="hdnRefreshType" runat="server" />
        <asp:HiddenField ID="hdnJournalNo" runat="server" />
        <asp:HiddenField ID="hdnIBRef" runat="server" />
        <asp:HiddenField ID="hdnBranchId" runat="server" />
        <asp:HiddenField ID="hdnMainAccountId" runat="server" />

        <asp:HiddenField ID="hdnMainAccountIdTDS" runat="server" />
        <asp:HiddenField ID="HiddenSubMandatory" runat="server" />
        <asp:HiddenField ID="hdnIsValidate" runat="server" />
        <asp:HiddenField ID="hdnToUnit" runat="server" />
        <asp:HiddenField ID="hdnType" runat="server" />
        <asp:HiddenField ID="hdnProjectSelectInEntryModule" runat="server" />
        <asp:HiddenField ID="hdnTDSval" runat="server" />
        <%--Rev Work start: Copy Feature required for Journal Vouchers Date:-27.05.2022 Mantise no:24911--%>
        <asp:HiddenField ID="hdnId" runat="server" />
        <asp:HiddenField ID="hdnVal" runat="server" />
        <asp:HiddenField ID="hdnSchemeVal" runat="server" />
        <%--Rev Work close: Copy Feature required for Journal Vouchers Date:-27.05.2022 Mantise no:24911--%>

        <%--Rev 5.0--%>
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
        <%--End of Rev 5.0--%>
    </div>

    <div style="display: none">
        <%-- <asp:Button ID="btnPrint" runat="server" Text="Print" OnClick="btnPrint_Click" meta:resourcekey="btnPrintResource1" />--%>
        <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
    </div>
    <dxe:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" ContainerElementID="grid"
        Modal="True">
    </dxe:ASPxLoadingPanel>
    <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
        <ClientSideEvents ControlsInitialized="AllControlInitilize" />
    </dxe:ASPxGlobalEvents>

    <%--DEBASHIS--%>
    <div class="PopUpArea">
        <dxe:ASPxPopupControl ID="ASPxDocumentsPopup" runat="server" ClientInstanceName="cDocumentsPopup"
            Width="350px" HeaderText="Select Design(s)" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentStyle VerticalAlign="Top"></ContentStyle>
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <dxe:ASPxCallbackPanel runat="server" ID="SelectPanel" ClientInstanceName="cSelectPanel" OnCallback="SelectPanel_Callback" ClientSideEvents-EndCallback="cSelectPanelEndCall">
                        <ClientSideEvents EndCallback="cSelectPanelEndCall"></ClientSideEvents>
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

    <div>
        <asp:HiddenField ID="hfIsFilter" runat="server" />
        <asp:HiddenField ID="hfFromDate" runat="server" />
        <asp:HiddenField ID="hfToDate" runat="server" />
        <asp:HiddenField ID="hfBranchID" runat="server" />
        <asp:HiddenField ID="hdnIsPartyLedger" runat="server" />


    </div>


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
                    <button type="button" class="btn btn-default" onclick="closeModal();">Close</button>
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
                        <table border='1' width="100%">
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

    <%--Chinmoy added inline Project code start 13-12-2019--%>

    <dxe:ASPxPopupControl ID="ProjectCodePopupTDS" runat="server" ClientInstanceName="cProjectCodePopupTDS"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Height="400"
        Width="700" HeaderText="Select Document Number" AllowResize="true" ResizingMode="Postponed" Modal="true">
        <%--  <headertemplate>
                <span>Select Document Number</span>
            </headertemplate>--%>
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <label><strong>Search By Document Number</strong></label>
                <%--   <span style="color: red;">[Press ESC key to Cancel]</span>--%>
                <dxe:ASPxCallbackPanel runat="server" ID="ProjectCodeCallbackTDS" ClientInstanceName="cProjectCodeTDSCallback"
                    OnCallback="ProjectCodeCallbackTDS_Callback">
                    <PanelCollection>
                        <dxe:PanelContent runat="server">
                            <dxe:ASPxGridLookup ID="lookupPopup_ProjectCodeTDS" runat="server" ClientInstanceName="clookupPopupTDS_ProjectCode" Width="800"
                                KeyFieldName="ProjectId" TextFormatString="{0}" MultiTextSeparator=", " ClientSideEvents-TextChanged="ProjectCodeSelectedTDS"
                                ClientSideEvents-KeyDown="lookup_ProjectCodeKeyDownTDS" OnDataBinding="lookup_ProjectCode_DataBindingTDS">
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
                    <ClientSideEvents EndCallback="ProjectCodeTDSCallback_endcallback" />
                </dxe:ASPxCallbackPanel>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="Blue" Font-Bold="True" ForeColor="White" />
    </dxe:ASPxPopupControl>



    <%--//End--%>


    <asp:HiddenField ID="hdnAutoPrint" runat="server" />
    <asp:HiddenField ID="cpTagged" runat="server" />
    <asp:HiddenField ID="hdnAllowProjectInDetailsLevel" runat="server" />
    <asp:HiddenField ID="hdnEditProjId" runat="server" />
    <asp:HiddenField ID="hdnProjectMandatory" runat="server" />

    <asp:HiddenField ID="hdnIsLeadAvailableinTransactions" runat="server" />

    <%--  REV 2.0--%>
    <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="CallbackPanelEndCall" />
    </dxe:ASPxCallbackPanel>
    <asp:HiddenField ID="hFilterType" runat="server" />
    <%--END REV 2.0--%>
</asp:Content>
