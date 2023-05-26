<%--/**********************************************************************************************************************
 * Rev 1.0      Sanchita    V2.0.38     26-05-2023      Party Invoice No & Party Invoice Date fields required in the 
 *                                                      Consolidated Vendor Opening Module. Refer: 25889
 * **********************************************************************************************************************/--%>
<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="ConsolidatedVendor.aspx.cs" Inherits="OpeningEntry.ERP.ConsolidatedVendor" EnableEventValidation="false" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script src="js/SearchPopup.js"></script>
    <%--<link href="js/SearchPopup.css" rel="stylesheet" />--%>

    <style type="text/css">
        .inline {
            display: inline !important;
        }

        .dxtcLite_PlasticBlue.dxtc-top > .dxtc-content {
            overflow: visible !important;
        }

        .abs {
            position: absolute;
            right: -20px;
            top: 10px;
        }

        .fa.fa-exclamation-circle:before {
            font-family: FontAwesome;
        }

        .tp2 {
            right: -18px;
            top: 7px;
            position: absolute;
        }

        #txtCreditLimit_EC {
            position: absolute;
        }

        #gridReplacement_DXStatus span > a {
            display: none;
        }

        #aspxGridTax_DXEditingErrorRow0 {
            display: none;
        }

        .horizontal-images.content li {
            float: left;
        }

        #rdl_SaleInvoice {
            margin-top: 3px;
        }

            #rdl_SaleInvoice > tbody > tr > td {
                padding-right: 5px;
            }

        #DivBilling [class^="col-md"], #DivShipping [class^="col-md"] {
            padding-top: 5px;
            padding-bottom: 5px;
        }

        #tbldescripion > tbody > tr > td, #tbldescripion2 > tbody > tr > td {
            padding-right: 15px;
        }

        .dxeErrorFrameSys.dxeErrorCellSys {
            position: absolute;
        }
    </style>

    <script>


        var RetId;
        function OnClickRetention(id, docno, docamount, ret_amount, ret_adjusted, ret_unpaidadjusted) {
            RetId = id;
            ctxtInvoiceNumber.SetText(docno);
            ctxtTotAmount.SetText(docamount);
            crtxtRetunAmount.SetText(ret_amount);
            ctxtAlreadyReturned.SetText(ret_adjusted);
            ctxtReamainingReturned.SetText(ret_unpaidadjusted);

            if (parseFloat(ret_unpaidadjusted) > 0)
                $("#newModal").modal('show');
            else {
                jAlert('Remaining Retention Amount Not Available.')
            }

        }


        function saveRetention() {


            var Details = {};
            Details.invoice_id = RetId;
            Details.schema_id = $("#CmbScheme").val();
            Details.doc_no = $("#txtBillNo").val();
            Details.trans_date = tDate.GetText();
            Details.Ret_Amount = ctxtReturnAmount.GetValue();


            var val = document.getElementById("CmbScheme").value;
            var Branchval = $("#ddlBranch").val();

            if (val == "" || val == "0") {
                jAlert('Select numbering schema.');

            }
            else if (document.getElementById('<%= txtBillNo.ClientID %>').value == "") {
                jAlert('Enter Journal No');
                document.getElementById('<%= txtBillNo.ClientID %>').focus();
            }

            else {

                $.ajax({
                    type: "POST",
                    url: "ConsolidatedVendor.aspx/SaveRetentionDetails",
                    data: JSON.stringify(Details),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (msg) {
                        // alert(msg.d)

                        if (msg.d == 'success') {
                            jAlert('Data Saved.');
                            cOpeningGrid.Refresh()
                            cOpeningGrid.Refresh()

                            $("#newModal").modal('hide');
                        }
                        else {
                            jAlert('Try again later');
                        }

                        //ctxtReturnAmount

                    },
                    error: function (msg) {
                        jAlert('Try again later');
                    }
                });
            }

    }


        function CmbScheme_ValueChange() {
            //var val = cCmbScheme.GetValue();
            // deleteAllRows();
            //InsgridBatch.AddNewRow();
            var val = document.getElementById("CmbScheme").value;
            $("#MandatoryBillNo").hide();

            if (val != "0") {
                $.ajax({
                    type: "POST",
                    url: 'ConsolidatedCustomer.aspx/getSchemeType',
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







                        if (schemetype == '0') {

                            document.getElementById('<%= txtBillNo.ClientID %>').disabled = false;
                            document.getElementById('<%= txtBillNo.ClientID %>').value = "";
                            //document.getElementById("txtBillNo").focus();
                            setTimeout(function () { $("#txtBillNo").focus(); }, 200);

                        }
                        else if (schemetype == '1') {

                            document.getElementById('<%= txtBillNo.ClientID %>').disabled = true;
                            document.getElementById('<%= txtBillNo.ClientID %>').value = "Auto";
                            tDate.Focus();
                        }
                        else if (schemetype == '2') {

                            document.getElementById('<%= txtBillNo.ClientID %>').disabled = true;
                    document.getElementById('<%= txtBillNo.ClientID %>').value = "Datewise";
                }
                    }
                });
}
else {
    document.getElementById('<%= txtBillNo.ClientID %>').disabled = true;
                document.getElementById('<%= txtBillNo.ClientID %>').value = "";
            }

        }




        function txtBillNo_TextChanged() {
            var VoucherNo = document.getElementById("txtBillNo").value;


            if (VoucherNo != "") {
                $("#MandatoryBillNo").hide();
            }

            $.ajax({
                type: "POST",
                url: "ConsolidatedCustomer.aspx/CheckUniqueName",
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


        function Callback_EndCallback() {

            // alert('');
            $("#ddldetails").val(0);
        }


        function GetContactPerson(e) {

            var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());
            if (key != null && key != '') {

                //     cContactPerson.PerformCallback('BindContactPerson~' + key);

            }
        }
        $(function () {


            $("#ddl_Branch").focus();
            TypeCheck();

            if ($("#hdncus").val() == "1") {

                $("#divgrid").attr('style', 'display:block');
            }

            else {
                $("#divgrid").attr('style', 'display:none');

            }

            $("#ddltype").on('change', function () {
                TypeCheck();

            })

            if ($("#hiddnmodid").val() != "0") {

                $("#tbldescripion").attr('style', 'display:none');
                $("#tbldescripion2").attr('style', 'display:none');
            }
        });
        function TypeCheck() {

            if ($("#ddltype").val() == "PB" || $("#ddltype").val() == "RET") {
                $("#tbldescripion").removeAttr('style');
                $("#tblconsolidatevendor").attr('style', 'display:none');
                $("#tbldescripion2").attr('style', 'display:none');

            }
            else if ($("#ddltype").val() == "CDB" || $("#ddltype").val() == "CCR") {

                $("#tbldescripion").attr('style', 'display:none');
                $("#tbldescripion2").attr('style', 'display:none');
                $("#tblconsolidatevendor").removeAttr('style');

            }


            else {
                $("#tbldescripion").attr('style', 'display:none');
                $("#tblconsolidatevendor").attr('style', 'display:none');
                $("#tbldescripion2").removeAttr('style');
            }

            if ($("#ddltype").val() == "PB" && $("#hdnProject").val() == "Yes") {
                $("#tblretention").removeAttr('style');
            }
            else {
                $("#tblretention").attr('style', 'display:none');
            }


        }









        function MainAccountNewkeydownRO(e) {
            var OtherDetails = {}
            OtherDetails.SearchKey = $("#txtMainAccountSearchRO").val();
            OtherDetails.branchId = $("#ddl_Branch").val();
            if (e.code == "Enter" || e.code == "NumpadEnter") {
                if ($("#txtMainAccountSearchRO").val() == "")
                    return;
                var HeaderCaption = [];
                HeaderCaption.push("Main Account Name");
                HeaderCaption.push("Subledger Type");
                HeaderCaption.push("Reverse Applicable");
                HeaderCaption.push("HSN/SAC");

                //callonServer("/OMS/Management/Activities/Services/Master.asmx/GetMainAccountCashBank", OtherDetails, "MainAccountTable", HeaderCaption, "MainAccountIndex", "SetMainAccount");
                callonServer("Services/OpeningMaster.asmx/GetMainAccountCashBankByProcedure", OtherDetails, "MainAccountTableRO", HeaderCaption, "MainAccountIndexRO", "SetMainAccountRO");
            }
            else if (e.code == "ArrowDown") {
                if ($("input[MainAccountIndexRO=0]"))
                    $("input[MainAccountIndexRO=0]").focus();
            }
            else if (e.code == "Escape") {
                //  
                $('#MainAccountModelRO').modal('hide');

            }
        }

        function SetAmountandPercentage(s, e) {

            
            if ((parseFloat(s.GetValue()) + parseFloat(ctxt_docamt.GetValue())) > parseFloat(ctxt_disamt.GetValue())) {
                    
                jAlert('O/S amount + Retention amount can not be greater than doc amount.', 'Alert', function () {
                    s.SetFocus();
                })
             }
           

        }

        function oslostfocus(s, e) {
            if ((parseFloat(s.GetValue()) + parseFloat(crtxtRetAmount.GetValue())) > parseFloat(ctxt_disamt.GetValue())) {

                jAlert('O/S amount + Retention amount can not be greater than doc amount.', 'Alert', function () {
                    s.SetFocus();
                })
            }
        }


        function MainAccountButnClickRO(s, e) {
            if (e.buttonIndex == 0) {
                var txt = "<table border='1' width=\"100%\"  class=\"dynamicPopupTbl\"><tr class=\"HeaderStyle\" ><th>Main Account Name</th><th>Subledger Type</th><th>Reverse Applicable</th><th>HSN/SAC</th></tr><table>";
                document.getElementById("MainAccountTableRO").innerHTML = txt;
                $('#MainAccountModelRO').modal('show');


            }
        }



        function ValueSelected(e, indexName) {
            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var Id = e.target.parentElement.parentElement.cells[0].innerText;
                var name = e.target.parentElement.parentElement.cells[1].children[0].value;
                if (Id) {
                    if (indexName == "ProdIndex") {
                        SetProduct(Id, name);
                    }
                    else if (indexName == "salesmanIndex") {
                        OnFocus(Id, name);
                    }
                        // Added By Chinmoy
                        //Start
                    else if (indexName == "BillingAreaIndex") {
                        SetBillingArea(Id, name);
                    }
                    else if (indexName == "ShippingAreaIndex") {
                        SetShippingArea(Id, name);
                    }
                    else if (indexName == "customeraddressIndex") {
                        SetCustomeraddress(Id, name);
                    }

                    else if (indexName == "MainAccountIndexRO") {
                        $('#MainAccountModelRO').modal('hide');
                        var IsSub = e.target.parentElement.parentElement.children[2].innerText;
                        var RevApp = e.target.parentElement.parentElement.children[3].innerText;
                        if (RevApp == 'Yes') {
                            RevApp = '1';
                        }
                        else {
                            RevApp = '0';
                        }
                        var TaxAble = e.target.parentElement.parentElement.children[4].innerText;
                        SetMainAccountRO(Id, name, IsSub, RevApp, TaxAble);
                    }
                        //End
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
                    if (indexName == "ProdIndex")
                        $('#txtProdSearch').focus();
                    else if (indexName == "salesmanIndex")
                        ctxtCreditDays.Focus();
                        // Added By Chinmoy
                        //Start
                    else if (indexName == "BillingAreaIndex")
                        $('#txtbillingArea').focus();
                    else if (indexName == "ShippingAreaIndex")
                        $('#txtshippingArea').focus();
                    else if (indexName == "customeraddressIndex")
                        ('#txtshippingShipToParty').focus();
                    else if (indexName == "MainAccountIndexRO")
                        $('#txtMainAccountSearchRO').focus();
                        //End
                    else
                        $('#txtCustSearch').focus();
                }
            }
        }

        function SetMainAccountRO(Id, name, e) {

            $('#MainAccountModelRO').modal('hide');
            cbtnGL.SetText(name);
            $('#hdnROMainAc').val(Id);
        }


        function dueLostFocus(s, e) {
            if (parseFloat(ctxtReamainingReturned.GetText()) < parseFloat(s.GetValue())) {
                jAlert('You can return maximum ' + parseFloat(ctxtReamainingReturned.GetText()), 'Alert', function () {
                    s.SetFocus();
                });

            }
        }












        function saveClientClick(s, e) {

            var ProjectCode = clookup_Project.GetText();
            if ($("#hdnProjectSelectInEntryModule").val() == "1" && $("#hddnProjectMandatory").val() == "1" && ProjectCode == "") {
                
                flag = false;
                jAlert("Please Select Project.");
                return false;
            }

            if (parseFloat(crtxtRetAmount.GetText()) > 0 && cbtnGL.GetText() == "") {
                flag = false;
                jAlert("Please Select retention GL to proceed.");
                return false;
            }


            if (gridLookup.GetValue() == null) {

                jAlert('Vendor is mandatory', "Alert", function () {


                    gridLookup.Focus();
                    gridLookup.ShowDropDown();
                });
            }

            else {

                if ($("#ddltype").val() == "PB" || $("#ddltype").val() == "RET") {

                    if (ctxt_doccno.GetText() == '') {

                        jAlert('Doc Number is mandatory', "Alert", function () {


                            ctxt_doccno.Focus();
                        });
                    }
                    else if (tsdate.GetText() == '') {

                        jAlert('Doc Date is mandatory', "Alert", function () {

                            tsdate.Focus();

                        });
                    }
                    else if (ctxt_docamt.GetText() == '' || ctxt_docamt.GetText() == '0.00') {

                        jAlert('O/S Amount is mandatory', "Alert", function () {

                            ctxt_docamt.Focus();

                        });
                    }
                        //else {
                        //    submitFunc();
                        //}

                    else {
                        if (ctxt_disamt.GetText() == '' || ctxt_disamt.GetText() == '0.00') {

                            submitFunc();
                        }
                        else if (parseFloat(ctxt_docamt.GetText()) > parseFloat(ctxt_disamt.GetText())) {

                            jAlert('O/S must be less than or equal than Document amount', "Alert", function () {

                                ctxt_disamt.Focus();

                            });

                        }

                        else {
                            submitFunc();

                        }

                    }
                }

                else if ($("#ddltype").val() == "CDB" || $("#ddltype").val() == "CCR") {

                    if (dt_vendor.GetText() == '') {

                        jAlert('Date is mandatory', "Alert", function () {
                            dt_vendor.Focus();

                        });
                    }
                    else if (ctxt_vendor_amt.GetText() == '' || ctxt_vendor_amt.GetText() == '0.00') {

                        jAlert('Amount is mandatory', "Alert", function () {

                            ctxt_vendor_amt.Focus();
                        });
                    }
                    else {
                        submitFunc();

                    }
                }

                else {

                    if (ctxt_doccno2.GetText() == '') {

                        jAlert('Doc Number is mandatory', "Alert", function () {
                            ctxt_doccno2.Focus();
                        });
                    }
                    else if (tsdate2.GetText() == '') {

                        jAlert('Doc Date is mandatory', "Alert", function () {
                            tsdate2.Focus();


                        });
                    }
                    else if (ctxt_docamt2.GetText() == '' || ctxt_docamt2.GetText() == '0.00') {

                        jAlert('Amount is mandatory', "Alert", function () {


                            ctxt_docamt2.Focus();
                        });
                    }
                    else {
                        submitFunc();

                    }

                }
            }
        }


        function submitFunc() {

            if ($("#hiddnmodid").val() == "0") {
                cOpeningGrid.PerformCallback('TemporaryData~' + 0);
            }
            else {
                var mod = $("#hiddnmodid").val();
                cOpeningGrid.PerformCallback('ModifyData~' + mod);

                // cOpeningGrid.PerformCallback('Display~' + 0);
            }
        }



        function OnClickDelete(keyValue, OSAmount, Unpaidamt) {
            jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    if (parseFloat(OSAmount) != parseFloat(Unpaidamt)) {
                        // alert();
                        jAlert('Already this document was used');

                    }
                    else {
                        cOpeningGrid.PerformCallback('Delete~' + keyValue);
                        cOpeningGrid.PerformCallback('Display~' + 0);
                    }

                }
            });
        }

        function ClearData(s, e) {

            $("#tbldescripion").attr('style', 'display:none');
            $("#tbldescripion2").attr('style', 'display:none');
            if (cOpeningGrid.cpSaveSuccessOrFail == "Success") {

                ctxt_doccno.SetText('');
                tsdate.SetText('');

                tstartdate.SetText('');
                ctxt_fullbill.SetText('');
                trefdate.SetText('');
                ctxt_docamt.SetText('');
                ctxt_disamt.SetText('');
                ctxt_commprcntg.SetText('');
                ctxt_commAmt.SetText('');


                ctxt_doccno2.SetText('');
                tsdate2.SetText('');
                ctxt_docamt2.SetText('');
                ctxt_commprcntg2.SetText('');
                ctxt_commAmt2.SetText('');



                dt_vendor.SetText('');
                ctxt_vendor_amt.SetText('');

                // Rev 1.0
                ctxtPartyInvNo.SetText('');
                cdtPartyInvDate.SetText('');
                // End of Rev 1.0

                jAlert('Saved Successfully');
                TypeCheck();

            }
            else if (cOpeningGrid.cpSaveSuccessOrFail == "Duplicate") {

                ctxt_doccno.SetText('');
                ctxt_doccno2.SetText('');
                jAlert('Duplicate Document Number');
                TypeCheck();
            }
            else if (cOpeningGrid.cpSaveSuccessOrFail == "Mentatory") {
                jAlert('Please select Project.');
                TypeCheck();
            }
            // cOpeningGrid.Refrsh();


            //  alert();




        }
        function fn_AllowonlyNumeric(s, e) {
            var theEvent = e.htmlEvent || window.event;
            var key = theEvent.keyCode || theEvent.which;

            var txt = s.GetText();

            if (key != 8 || key != 13) txt += String.fromCharCode(key);

            var regex = /^[0-9]*\.?[0-9]*$/;
            if (!regex.test(txt)) {
                theEvent.returnValue = false;
                if (theEvent.preventDefault) theEvent.preventDefault();
            }
        }

        /* Rev 1.0 ( PartyInvNo and PartyInvDate ) */
        function onOpeningEdit(mod, cus, type, brnch, DocNumber, Date, FullBill, DueDate, RefDate, DocAmount, OSAmount, Commper, Commamount, Unpaidamt
            , retAmount, gl, duedate, gl_name, PartyInvNo, PartyInvDate) {

            $("#hiddnmodid").val(mod);
            $("#ddl_Branch").val(brnch);
            $("#ddltype").val(type);

            // gridLookup.SetValue(cus)

            cQuotationComponentPanel.PerformCallback('Fetch~' + cus);

            if ($("#ddltype").val() == "PB" || $("#ddltype").val() == "RET") {
                $("#tbldescripion").removeAttr('style');
                $("#tbldescripion2").attr('style', 'display:none');

                ctxt_doccno.SetText(DocNumber);
                tsdate.SetText(Date);

                tstartdate.SetText(DueDate);
                ctxt_fullbill.SetText(FullBill);
                trefdate.SetText(RefDate);
                ctxt_docamt.SetText(OSAmount);
                ctxt_disamt.SetText(DocAmount);
                ctxt_commprcntg.SetText(Commper);
                ctxt_commAmt.SetText(Commamount);


                crtxtRetAmount.SetText(retAmount);
                cdtDueDate.SetText(duedate);
                cbtnGL.SetText(gl_name);
                $("#hdnROMainAc").val(gl);


            }

            else if ($("#ddltype").val() == "CDB" || $("#ddltype").val() == "CCR") {

                $("#tbldescripion").attr('style', 'display:none');
                $("#tbldescripion2").attr('style', 'display:none');
                $("#tblconsolidatevendor").removeAttr('style');


                dt_vendor.SetText(Date);
                ctxt_vendor_amt.SetText(OSAmount);

            }



            else {

                $("#tbldescripion").attr('style', 'display:none');
                $("#tbldescripion2").removeAttr('style');


                ctxt_doccno2.SetText(DocNumber);
                tsdate2.SetText(Date);
                ctxt_docamt2.SetText(OSAmount);
                ctxt_commprcntg2.SetText(Commper);
                ctxt_commAmt2.SetText(Commamount);
            }

            // Rev 1.0
            ctxtPartyInvNo.SetText(PartyInvNo);
            cdtPartyInvDate.SetText(PartyInvDate);
            // End of Rev 1.0

            if (parseFloat(OSAmount) != parseFloat(Unpaidamt)) {
                $("#FinalSave").attr('style', 'display:none;');

            }
            else {
                $("#FinalSave").attr('style', 'display:inline-block;');

            }

            if ($("#ddltype").val() == "PB" && $("#hdnProject").val() == "Yes") {
                $("#tblretention").removeAttr('style');
            }
            else {
                $("#tblretention").attr('style', 'display:none');
            }
        }

        function CloseGridLookup() {
            gridLookup.ConfirmCurrentSelection();
            gridLookup.HideDropDown();
            gridLookup.Focus();
            clookup_Project.gridView.Refresh();
        }


        $(function () {
            cQuotationComponentPanel.PerformCallback('Fetch' + '~' + $("#ddl_Branch").val());

            $('body').on('change', '#ddl_Branch', function () {

                cQuotationComponentPanel.PerformCallback('Fetch' + '~' + $("#ddl_Branch").val());
            });

        });

    </script>

    <script>

        //Hierarchy Start Tanmoy
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
                url: 'ConsolidatedVendor.aspx/getHierarchyID',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ ProjID: projID }),
                success: function (msg) {
                    var data = msg.d;
                    $("#ddlHierarchy").val(data);
                }
            });
        }

        function CustomerlostFocus() {
            clookup_Project.gridView.Refresh();
        }

        function Customer_EndCallback(s, e) {
            // debugger;
            if (cQuotationComponentPanel.cpProjectID != null) {
                clookup_Project.gridView.SelectItemsByKey(cQuotationComponentPanel.cpProjectID);
            }
        }
        //Hierarchy End Tanmoy
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <div class="panel-heading">
        <div class="panel-title clearfix">
            <h3 class="pull-left">Consolidated  Vendor</h3>
            <div id="pageheaderContent" class=" pull-right wrapHolder content horizontal-images" style="display: none;">
                <ul>
                    <li>
                        <div class="lblHolder">
                            <table>
                                <tbody>
                                    <tr>
                                        <td>Total Debit</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dxe:ASPxLabel ID="lblTotDebit" runat="server" Text="ASPxLabel" ClientInstanceName="clblTotDebit"></dxe:ASPxLabel>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </li>
                    <li>
                        <div class="lblHolder">
                            <table>
                                <tbody>
                                    <tr>
                                        <td>Total Credit</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dxe:ASPxLabel ID="lblTotCredit" runat="server" Text="ASPxLabel" ClientInstanceName="clblTotCredit"></dxe:ASPxLabel>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </li>
                </ul>
            </div>
        </div>
        <div class="crossBtn"><a href="./ConsolidatedVendorList.aspx"><i class="fa fa-times"></i></a></div>
    </div>


    <div class="form_main">
        <div class="clearfix" style="background: #f7f5f5; padding: 10px; padding-bottom: 20px;">
            <div class="row" id="salepurchaseret">
                <div class="col-md-2">
                    <label>Branch <span class="red">*</span></label>
                    <div class="relative">
                        <asp:DropDownList ID="ddl_Branch" runat="server" Width="100%" TabIndex="1">
                        </asp:DropDownList>
                        <span id="MandatoryBranch" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; display: none; padding-left: 9px;"
                            title="Mandatory"></span>
                    </div>
                </div>
                <div class="col-md-2">
                    <label>Vendor <span class="red">*</span></label>
                    <div class="relative">

                        <dxe:ASPxCallbackPanel runat="server" ID="ComponentQuotationPanel" ClientInstanceName="cQuotationComponentPanel" OnCallback="ComponentQuotation_Callback">
                            <panelcollection>
                            <dxe:PanelContent runat="server">
                                <dxe:ASPxGridLookup ID="lookup_Customer" runat="server" TabIndex="2" ClientInstanceName="gridLookup"     OnDataBinding="lookup_vendor_DataBinding"
                                    KeyFieldName="cnt_internalid" Width="100%" TextFormatString="{1}" AutoGenerateColumns="False" >
                                    <Columns>
                                        <dxe:GridViewDataColumn FieldName="shortname" Visible="true" VisibleIndex="0" Caption="Short Name" Width="200px" Settings-AutoFilterCondition="Contains">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataColumn>
                                        <dxe:GridViewDataColumn FieldName="Name" Visible="true" VisibleIndex="1" Caption="Name" Settings-AutoFilterCondition="Contains" Width="200px">

                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataColumn>
                                        <dxe:GridViewDataColumn FieldName="Type" Visible="true" VisibleIndex="2" Caption="Type" Settings-AutoFilterCondition="Contains" Width="200px">

                                            <Settings AutoFilterCondition="Contains" />
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
                                    <ClientSideEvents TextChanged="function(s, e) { GetContactPerson(e)}" lostfocus="CustomerlostFocus" />
                                    <ClearButton DisplayMode="Auto">
                                    </ClearButton>
                                </dxe:ASPxGridLookup>
                            </dxe:PanelContent>
                        </panelcollection>
                            <clientsideevents endcallback="Customer_EndCallback" />
                        </dxe:ASPxCallbackPanel>

                        <span id="MandatoryAccount" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; display: none; padding-left: 9px;" title="Mandatory"></span>
                    </div>
                </div>
                <div class="col-md-2">
                    <label>Type <span class="red">*</span></label>
                    <div class="relative">
                        <asp:DropDownList ID="ddltype" runat="server" Width="100%" TabIndex="3">
                            <asp:ListItem Text="Purchase Bill" Value="PB"></asp:ListItem>
                            <asp:ListItem Text="Advance" Value="ADV"></asp:ListItem>
                            <asp:ListItem Text="Debit Note" Value="DN"></asp:ListItem>
                            <asp:ListItem Text="Credit Note" Value="CN"></asp:ListItem>
                            <asp:ListItem Text="Return" Value="RET"></asp:ListItem>
                            <asp:ListItem Text="Consolidated Debit" Value="CDB"></asp:ListItem>
                            <asp:ListItem Text="Consolidated Credit" Value="CCR"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>

                <div class="col-md-2">
                    <%--<dxe:ASPxLabel ID="lblProject" runat="server" Text="Project">
                    </dxe:ASPxLabel>--%>
                    <label id="lblProject" runat="server">Project</label>
                    <dxe:ASPxGridLookup ID="lookup_Project" runat="server" ClientInstanceName="clookup_Project" DataSourceID="EntityServerModeDataSalesChallan"
                        KeyFieldName="Proj_Id" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" TabIndex="4">
                        <columns>                                               
                        <dxe:GridViewDataColumn FieldName="Proj_Code" Visible="true" VisibleIndex="1" Caption="Project Code" Settings-AutoFilterCondition="Contains" Width="200px">
                        </dxe:GridViewDataColumn>
                        <dxe:GridViewDataColumn FieldName="Proj_Name" Visible="true" VisibleIndex="2" Caption="Project Name" Settings-AutoFilterCondition="Contains" Width="200px">
                        </dxe:GridViewDataColumn>
                        <dxe:GridViewDataColumn FieldName="Customer" Visible="true" VisibleIndex="3" Caption="Customer"  Settings-AutoFilterCondition="Contains" Width="200px">
                            </dxe:GridViewDataColumn>
                            <dxe:GridViewDataColumn FieldName="HIERARCHY_NAME" Visible="true" VisibleIndex="4" Caption="Hierarchy" Settings-AutoFilterCondition="Contains" Width="200px">
                            </dxe:GridViewDataColumn>                                                
                                            </columns>
                        <gridviewproperties settings-verticalscrollbarmode="Auto">
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

                                                <SettingsBehavior AllowFocusedRow="True" AllowSelectSingleRowOnly="True"></SettingsBehavior>                                                

                                                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                             </gridviewproperties>
                        <clientsideevents gotfocus="function(s,e){clookup_Project.ShowDropDown();}" lostfocus="clookup_Project_LostFocus" valuechanged="ProjectValueChange" />

                        <clearbutton displaymode="Always">
                                            </clearbutton>
                    </dxe:ASPxGridLookup>
                    <dx:LinqServerModeDataSource ID="EntityServerModeDataSalesChallan" runat="server" OnSelecting="EntityServerModeDataSalesChallan_Selecting"
                        ContextTypeName="ERPDataClassesDataContext" TableName="ProjectCodeBind" />
                </div>
                <div class="col-md-3">
                    <dxe:ASPxLabel ID="lblHierarchy" runat="server" Text="Hierarchy">
                    </dxe:ASPxLabel>
                    <asp:DropDownList ID="ddlHierarchy" runat="server" Width="100%" Enabled="false">
                    </asp:DropDownList>
                </div>
            </div>


        </div>
        <div id="tbldescripion" style="display: none;" runat="server">

            <div class="row">
                <div class="col-md-3">
                    <label>Doc Number <span class="red">*</span></label>
                    <div>
                        <dxe:ASPxTextBox ID="txt_Docno" runat="server" ClientInstanceName="ctxt_doccno" TabIndex="4" Width="100%">
                        </dxe:ASPxTextBox>

                    </div>
                </div>
                <div class="col-md-3">
                    <label>Date <span class="red">*</span></label>
                    <div>
                        <dxe:ASPxDateEdit ID="dt_date" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="tsdate" TabIndex="5" Width="100%">
                            <buttonstyle width="13px">
                            </buttonstyle>

                        </dxe:ASPxDateEdit>


                    </div>
                </div>
                <div class="col-md-3">
                    <label>Due Date</label>
                    <div class="hide">
                        <dxe:ASPxTextBox ID="txt_fullbill" runat="server" ClientInstanceName="ctxt_fullbill" Width="100%">
                            <clientsideevents keypress="function(s,e){ fn_AllowonlyNumeric(s,e);}" />
                            <masksettings mask="<0..999999999>.<00..99>" allowmousewheel="false" />
                        </dxe:ASPxTextBox>


                    </div>
                    <div>
                        <dxe:ASPxDateEdit ID="dtdate_Due" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="tstartdate" TabIndex="7" Width="100%">
                            <buttonstyle width="13px">
                            </buttonstyle>

                        </dxe:ASPxDateEdit>


                    </div>
                </div>
                <div class="col-md-3">
                    <label>Ref Date</label>
                    <div>
                        <dxe:ASPxDateEdit ID="dtdate_Ref" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="trefdate" TabIndex="8" Width="100%">
                            <buttonstyle width="13px">
                            </buttonstyle>

                        </dxe:ASPxDateEdit>


                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-3">
                    <label>Doc Amount</label>
                    <div>
                        <dxe:ASPxTextBox ID="txt_disamt" runat="server" ClientInstanceName="ctxt_disamt" TabIndex="9" Width="100%">

                            <clientsideevents keypress="function(s,e){ fn_AllowonlyNumeric(s,e);}" />
                            <masksettings mask="<0..999999999>.<00..99>" allowmousewheel="false" />
                        </dxe:ASPxTextBox>


                    </div>
                </div>
                <div class="col-md-3">
                    <label>O/S Amount <span class="red">*</span> </label>
                    <div>


                        <dxe:ASPxTextBox ID="txt_docamt" runat="server" ClientInstanceName="ctxt_docamt" TabIndex="10" Width="100%">

                            <clientsideevents keypress="function(s,e){ fn_AllowonlyNumeric(s,e);}" lostfocus="oslostfocus"/>
                            <masksettings mask="<0..999999999>.<00..99>" allowmousewheel="false" />
                        </dxe:ASPxTextBox>


                    </div>

                </div>
                <div class="col-md-3">
                    <label>Comm%</label>
                    <div>
                        <dxe:ASPxTextBox ID="txt_commprcntg" runat="server" ClientInstanceName="ctxt_commprcntg" TabIndex="11" Width="100%">

                            <clientsideevents keypress="function(s,e){ fn_AllowonlyNumeric(s,e);}"  />
                            <masksettings mask="<0..999>.<00..99>" allowmousewheel="false" />
                        </dxe:ASPxTextBox>

                    </div>
                </div>
                <div class="col-md-3">
                    <label>Commm Amount</label>
                    <div>

                        <dxe:ASPxTextBox ID="txt_commAmt" runat="server" ClientInstanceName="ctxt_commAmt" TabIndex="12" Width="100%">
                            <clientsideevents keypress="function(s,e){ fn_AllowonlyNumeric(s,e);}" />
                            <masksettings mask="<0..999999999>.<00..99>" allowmousewheel="false" />
                        </dxe:ASPxTextBox>

                    </div>
                </div>
            </div>


        </div>
        <div id="tblretention" style="display: none;" runat="server">
                <div class="row">
                    <%--<div class="col-md-3">
                        <label for="" >Retention Percentage</label>
                        <div >
                            <dxe:ASPxTextBox ID="crtxtRetPercentage" ClientInstanceName="crtxtRetPercentage" runat="server" Width="100%">
                                <masksettings mask="&lt;0..99&gt;.&lt;00..99&gt;" allowmousewheel="false" />
                                <clientsideevents lostfocus="SetAmountandPercentage" />
                            </dxe:ASPxTextBox>
                        </div>
                    </div>--%>
                    <div class="col-md-3">
                        <label for="" >Retention Amt </label>
                        <div >
                            <dxe:ASPxTextBox ID="crtxtRetAmount" ClientInstanceName="crtxtRetAmount" runat="server" Width="100%">
                                <masksettings mask="&lt;0..999999999&gt;.&lt;00..99&gt;" allowmousewheel="false" />
                                <clientsideevents lostfocus="SetAmountandPercentage" />
                            </dxe:ASPxTextBox>
                        </div>
                    </div>

                    <div class="col-md-3">
                        <label for="" >Due Date</label>
                        <div >
                            <dxe:ASPxDateEdit ID="dtDueDate" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy" runat="server" ClientInstanceName="cdtDueDate" Width="100%">
                            </dxe:ASPxDateEdit>
                        </div>
                    </div>



                    <div class="col-md-3 ">
                        <label for="" >Retention GL</label>
                        <div >
                            <dxe:ASPxButtonEdit ID="btnGL" runat="server" ReadOnly="true" ClientInstanceName="cbtnGL" Width="100%">
                                <buttons>
                                        <dxe:EditButton>
                                        </dxe:EditButton>
                                    </buttons>
                                <clientsideevents buttonclick="MainAccountButnClickRO"
                                    keydown="MainAccountNewkeydownRO" />
                            </dxe:ASPxButtonEdit>
                        </div>
                    </div>
                </div>
            </div>
        <div id="tbldescripion2" style="display: none;" runat="server">
            <div class="row">
                <div class="col-md-3">
                    <label>Doc Number <span class="red">*</span> </label>
                    <div>
                        <dxe:ASPxTextBox ID="txt_Docno2" runat="server" ClientInstanceName="ctxt_doccno2" TabIndex="13" Width="100%">
                        </dxe:ASPxTextBox>

                    </div>
                </div>
                <div class="col-md-3">
                    <label>Date <span class="red">*</span> </label>
                    <div>

                        <dxe:ASPxDateEdit ID="dt_date2" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="tsdate2" TabIndex="14" Width="100%">
                            <buttonstyle width="13px">
                            </buttonstyle>

                        </dxe:ASPxDateEdit>
                    </div>
                </div>
                <div class="col-md-3">
                    <label>Amount <span class="red">*</span> </label>
                    <div>

                        <dxe:ASPxTextBox ID="txt_docamt2" runat="server" ClientInstanceName="ctxt_docamt2" TabIndex="15" Width="100%">

                            <clientsideevents keypress="function(s,e){ fn_AllowonlyNumeric(s,e);}" />
                            <masksettings mask="<0..999999999>.<00..99>" allowmousewheel="false" />
                        </dxe:ASPxTextBox>
                    </div>
                </div>
                <div class="col-md-3">
                    <label>Comm %</label>
                    <div>
                        <dxe:ASPxTextBox ID="txt_commprcntg2" runat="server" ClientInstanceName="ctxt_commprcntg2" TabIndex="16" Width="100%">

                            <clientsideevents keypress="function(s,e){ fn_AllowonlyNumeric(s,e);}" />
                            <masksettings mask="<0..999999999>.<00..99>" allowmousewheel="false" />
                        </dxe:ASPxTextBox>

                    </div>
                </div>
                <div class="col-md-3">
                    <label>Comm Amount</label>
                    <div>

                        <dxe:ASPxTextBox ID="txt_commAmt2" runat="server" ClientInstanceName="ctxt_commAmt2" TabIndex="17" Width="100%">

                            <clientsideevents keypress="function(s,e){ fn_AllowonlyNumeric(s,e);}" />
                            <masksettings mask="<0..999999999>.<00..99>" allowmousewheel="false" />
                        </dxe:ASPxTextBox>

                    </div>
                </div>
            </div>


        </div>
        <div id="tblconsolidatevendor" style="display: none;" runat="server">
            <div class="row">
                <div class="col-md-3">
                    <label>Date <span class="red">*</span> </label>
                    <div>

                        <dxe:ASPxDateEdit ID="dt_vendor" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="dt_vendor" Width="100%">
                            <buttonstyle width="13px">
                            </buttonstyle>
                        </dxe:ASPxDateEdit>
                    </div>
                </div>
                <div class="col-md-3">
                    <label>Amount <span class="red">*</span> </label>
                    <div>

                        <dxe:ASPxTextBox ID="txt_vendor_amt" runat="server" ClientInstanceName="ctxt_vendor_amt" Width="100%">
                            <clientsideevents keypress="function(s,e){ fn_AllowonlyNumeric(s,e);}" />
                            <masksettings mask="<0..999999999>.<00..99>" allowmousewheel="false" />
                        </dxe:ASPxTextBox>

                    </div>
                </div>
                <div class="col-md-3"></div>
                <div class="col-md-3"></div>

            </div>

        </div>
        <%--Rev 1.0--%>
        <div class="row">
            <div class="col-md-3">
                <label>Party Invoice No </label>
                <div>
                    <dxe:ASPxTextBox ID="txtPartyInvNo" runat="server" ClientInstanceName="ctxtPartyInvNo" TabIndex="18" Width="100%">
                    </dxe:ASPxTextBox>
                </div>
            </div>
            <div class="col-md-3">
                <label>Party Invoice Date </label>
                <div>
                    <dxe:ASPxDateEdit ID="dtPartyInvDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cdtPartyInvDate" TabIndex="19" Width="100%">
                        <buttonstyle width="13px">
                        </buttonstyle>

                    </dxe:ASPxDateEdit>
                </div>
            </div>
	    </div>
        <%--End of Rev 1.0--%>
    </div>
    <div style="padding-top: 15px;">
        <dxe:ASPxButton ID="FinalSave" runat="server" AutoPostBack="false" CssClass="btn btn-primary"  UseSubmitBehavior="False"
            Text="<u>S</u>ave" ClientInstanceName="cFinalSave" VerticalAlign="Bottom" EncodeHtml="false" TabIndex="20">
            <clientsideevents click="saveClientClick" />
        </dxe:ASPxButton>
    </div>

    <div class="GridViewArea" id="divgrid">


        <asp:DropDownList ID="ddldetails" runat="server" CssClass="btn btn-sm btn-primary" Style="float: right; margin-right: 2px !important;"
            OnSelectedIndexChanged="cmbExport2_SelectedIndexChanged" AutoPostBack="true">
            <asp:ListItem Value="0">Export to</asp:ListItem>
            <asp:ListItem Value="1">PDF</asp:ListItem>
            <asp:ListItem Value="2">XLS</asp:ListItem>
            <asp:ListItem Value="3">RTF</asp:ListItem>
            <asp:ListItem Value="4">CSV</asp:ListItem>
        </asp:DropDownList>



        <dxe:ASPxGridView ID="OpeningGrid" runat="server" AutoGenerateColumns="False" ClientInstanceName="cOpeningGrid"
            OnCustomCallback="OpeningGrid_CustomCallback" OnDataBinding="grid_DataBinding" KeyField="ModId" ClientSideEvents-BeginCallback="Callback_EndCallback"
            SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollBarMode="Auto" Settings-VerticalScrollableHeight="350"
            Width="100%" Settings-HorizontalScrollBarMode="Visible">

            <settings showgrouppanel="false" showstatusbar="Hidden" showfilterrow="true" showfilterrowmenu="True" />
            <columns>

                    <dxe:GridViewDataTextColumn Caption="Branch" FieldName="Branch" ReadOnly="True" Visible="True" VisibleIndex="0">
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="Vendor Name" FieldName="Customer" ReadOnly="True" Visible="True" VisibleIndex="1">
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="Type" FieldName="TypeName" ReadOnly="True" Visible="True" VisibleIndex="2">
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                <%--Start Project add Tanmoy--%>

                <dxe:GridViewDataTextColumn Caption="Project" FieldName="Proj_Name" ReadOnly="True" Visible="True" VisibleIndex="3">
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>


                    <dxe:GridViewDataTextColumn Caption="Hierarchy" FieldName="HIERARCHY_NAME" ReadOnly="True" Visible="True" VisibleIndex="4" width="80px">
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                <%--Start Project add Tanmoy--%>

                    <dxe:GridViewDataTextColumn Caption="Document Number" FieldName="DocNumber" ReadOnly="True" Visible="True" VisibleIndex="5">
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>


                    <dxe:GridViewDataTextColumn Caption="Date" FieldName="Date" ReadOnly="True" Visible="True" VisibleIndex="6" width="80px">
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                    <%--Rev 1.0--%>
                    <dxe:GridViewDataTextColumn Caption="Party Invoice No" FieldName="PartyInvNo" ReadOnly="True" Visible="True" VisibleIndex="7">
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>


                    <dxe:GridViewDataTextColumn Caption="Party Invoice Date" FieldName="PartyInvDate" ReadOnly="True" Visible="True" VisibleIndex="8" width="80px">
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <%--End of Rev 1.0--%>


                    <dxe:GridViewDataTextColumn Caption="Full Bill Amount" FieldName="FullBill" ReadOnly="True" Visible="False" VisibleIndex="9">
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="Due Date" FieldName="DueDate" ReadOnly="True" Visible="True" VisibleIndex="10" >
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="Ref Date" FieldName="RefDate" ReadOnly="True" Visible="True" VisibleIndex="11" width="80px">
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>


                    <dxe:GridViewDataTextColumn Caption="Doc Amount" FieldName="DocAmount" ReadOnly="True" Visible="True" VisibleIndex="12">
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>


                    <dxe:GridViewDataTextColumn Caption="OS Amount" FieldName="OSAmount" ReadOnly="True" Visible="True" VisibleIndex="13">
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>


                    <dxe:GridViewDataTextColumn Caption="Comm %" FieldName="Commper" ReadOnly="True" Visible="True" VisibleIndex="14">
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>


                    <dxe:GridViewDataTextColumn Caption="Comm Amount" FieldName="Commamount" ReadOnly="True" Visible="True" VisibleIndex="15">
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>


                    <dxe:GridViewDataTextColumn Caption="Type" FieldName="Type" ReadOnly="True" Visible="False" VisibleIndex="16">
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>


                    <dxe:GridViewDataTextColumn Caption="Unpaid Amount" FieldName="Unpaidamt" ReadOnly="True" Visible="True" VisibleIndex="17">
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>


                    <dxe:GridViewDataTextColumn Caption="Type" FieldName="Branch" ReadOnly="True" Visible="False" VisibleIndex="18">
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn ReadOnly="True" CellStyle-HorizontalAlign="Center" VisibleIndex="19">
                        <HeaderStyle HorizontalAlign="Center" />


                        <CellStyle HorizontalAlign="Center"></CellStyle>
                        <HeaderTemplate>
                            Actions
                        </HeaderTemplate>
                        <DataItemTemplate>
                            <%--Rev 1.0 (columns PartyInvNo and PartyInvDate added) --%>
                            <% if (rights.CanEdit)
                               { %>
                            <a href="javascript:void(0);" onclick="onOpeningEdit('<%#Eval("ModId")%>','<%#Eval("CustomerId")%>',
                                '<%#Eval("Type")%>','<%#Eval("branch_id")%>','<%#Eval("DocNumber")%>','<%#Eval("Date")%>',
                                '<%#Eval("FullBill")%>','<%#Eval("DueDate")%>','<%#Eval("RefDate")%>','<%#Eval("DocAmount")%>'
                                ,'<%#Eval("OSAmount")%>','<%#Eval("Commper")%>','<%#Eval("Commamount")%>','<%#Eval("Unpaidamt")%>'
                                ,'<%#Eval("Ven_ret_amount")%>','<%#Eval("Ven_ret_GL")%>','<%#Eval("Ven_ret_duedate")%>','<%#Eval("MainAccount_Name")%>'
                                ,'<%#Eval("PartyInvNo")%>','<%#Eval("PartyInvDate")%>')"
                                title="Edit" class="pad">
                                <img src="/assests/images/Edit.png" /></a>

                           <% if (rights.CanEdit)
                               { %>
                            <a href="javascript:void(0);" onclick="OnClickRetention('<%#Eval("ModId")%>','<%#Eval("DocNumber")%>'
                                ,'<%#Eval("DocAmount")%>','<%#Eval("Ven_ret_amount")%>','<%#Eval("Ven_ret_adjusted")%>'
                                ,'<%#Eval("Ven_unpaid_ret_amount")%>')" title="Retention" class="pad">
                                <img src="/assests/images/changeIcon.png" /></a>

                            <% } %>

                            <% } %>
                            <% if (rights.CanDelete)
                               { %>
                            <a href="javascript:void(0);" onclick="OnClickDelete('<%#Eval("ModId")%>','<%#Eval("OSAmount")%>','<%#Eval("Unpaidamt")%>')" title="Edit" class="pad">
                                <img src="/assests/images/Delete.png" /></a>

                            <% } %>


                        </DataItemTemplate>
                    </dxe:GridViewDataTextColumn>

                </columns>
            <settingspager pagesize="10">
                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                </settingspager>
            <settings showgrouppanel="True" showstatusbar="Hidden" showhorizontalscrollbar="False" showfilterrow="true" showfilterrowmenu="true" />
            <settingsbehavior columnresizemode="NextColumn" />
            <clientsideevents endcallback="ClearData" />
        </dxe:ASPxGridView>
    </div>


    <div class="clear"></div>
    <div style="padding-top: 10px;"></div>
    <div class="clear"></div>
    <div class=""></div>


   



    <asp:SqlDataSource runat="server" ID="dsCustomer"
        SelectCommand="Proc_Opening_VendorConsolidate" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:Parameter Type="String" Name="Action" DefaultValue="Customerbind" />
        </SelectParameters>
    </asp:SqlDataSource>

    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="true" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>

    <asp:HiddenField runat="server" ID="hdnROMainAc" />
    <asp:HiddenField runat="server" ID="hdncus" />
    <asp:HiddenField runat="server" ID="hiddnmodid" />
    <asp:HiddenField ID="hdnProjectSelectInEntryModule" runat="server" />
    <asp:HiddenField runat="server" ID="hdnCust_id" />
     <asp:HiddenField ID="hddnProjectMandatory" runat="server" />










      <div class="modal fade pmsModal w40" id="newModal" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Retention Details</h4>
                </div>
                <div class="modal-body">
                    <div class="col-md-12">
                        <div class="form-group row">
                            <label for="" class="col-sm-4 col-form-label">Invoice Number</label>
                            <div class="col-sm-8">
                                <dxe:ASPxTextBox runat="server" ClientInstanceName="ctxtInvoiceNumber" ID="txtInvoiceNumber" ClientEnabled="false" Width="100%">
                                </dxe:ASPxTextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label for="" class="col-sm-4 col-form-label">Total Amount </label>
                            <div class="col-sm-8">
                                <dxe:ASPxTextBox runat="server" ClientInstanceName="ctxtTotAmount" ID="ctxtTotAmount" ClientEnabled="false" Width="100%">
                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />

                                </dxe:ASPxTextBox>
                            </div>
                        </div>
                        <%--<div class="form-group row">
                            <label for="" class="col-sm-4 col-form-label">Retention Percentage</label>
                            <div class="col-sm-8">
                                <dxe:ASPxTextBox ID="crtxtRetPercentage" ClientInstanceName="crtxtRetPercentage" ClientEnabled="false" runat="server" Width="100%">
                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />

                                </dxe:ASPxTextBox>
                            </div>
                        </div>--%>
                        <div class="form-group row">
                            <label for="" class="col-sm-4 col-form-label">Retention Amt </label>
                            <div class="col-sm-8">
                                <dxe:ASPxTextBox ID="ASPxTextBox1" ClientInstanceName="crtxtRetunAmount" ClientEnabled="false" runat="server" Width="100%">
                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />

                                </dxe:ASPxTextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label for="" class="col-sm-4 col-form-label">Retention Adjusted</label>
                            <div class="col-sm-8">
                                <dxe:ASPxTextBox ID="txtAlreadyReturned" ClientInstanceName="ctxtAlreadyReturned" ClientEnabled="false" runat="server" Width="100%">
                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />

                                </dxe:ASPxTextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label for="" class="col-sm-4 col-form-label">Retention Remaining</label>
                            <div class="col-sm-8">
                                <dxe:ASPxTextBox ID="txtRemaining" ClientInstanceName="ctxtReamainingReturned" ClientEnabled="false" runat="server" Width="100%">
                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />

                                </dxe:ASPxTextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label for="" class="col-sm-4  col-form-label">Retention Adjustment</label>
                            <div class="col-sm-8">
                                <dxe:ASPxTextBox ID="txtReturnAmount" ClientInstanceName="ctxtReturnAmount" runat="server" Width="100%">
                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                    <ClientSideEvents LostFocus="dueLostFocus" />
                                </dxe:ASPxTextBox>

                            </div>
                        </div>
                        <div class="form-group row">
                            <label for="" class="col-sm-4  col-form-label">Journal Numbering</label>
                            <div class="col-sm-8">
                                <asp:DropDownList ID="CmbScheme" runat="server" DataSourceID="SqlSchematype"
                                    DataTextField="SchemaName" DataValueField="ID" Width="100%"
                                    onchange="CmbScheme_ValueChange()">
                                </asp:DropDownList>

                            </div>
                        </div>
                        <div class="form-group row">
                            <label for="" class="col-sm-4  col-form-label">Document Number</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtBillNo" runat="server" Width="100%" meta:resourcekey="txtBillNoResource1" MaxLength="30" onchange="txtBillNo_TextChanged()"></asp:TextBox>

                            </div>
                        </div>
                        <div class="form-group row">
                            <label for="" class="col-sm-4  col-form-label">Posting Date</label>
                            <div class="col-sm-8">
                                <dxe:ASPxDateEdit ID="tDate" runat="server" EditFormat="Custom" ClientInstanceName="tDate" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                                    UseMaskBehavior="True" Width="100%" meta:resourcekey="tDateResource1">
                                   
                                </dxe:ASPxDateEdit>

                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-success" onclick="saveRetention();">Save</button>
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>

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



    <div class="modal fade" id="MainAccountModelRO" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <!-- Modal MainAccount-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Main Account Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="MainAccountNewkeydownRO(event)" id="txtMainAccountSearchRO" autofocus width="100%" placeholder="Search by Main Account Name or Short Name" />

                    <div id="MainAccountTableRO">
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
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>



    <asp:HiddenField ID="hdnProject" runat="server" />


</asp:Content>
