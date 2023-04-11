<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                10-04-2023        2.0.37           Pallab              Transactions pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" EnableViewStateMac="false" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="PosSalesInvoice.aspx.cs" Inherits="ERP.OMS.Management.Activities.PosSalesInvoice" %>


<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<%@ Register Src="~/OMS/Management/Activities/UserControls/ucPaymentDetails.ascx" TagPrefix="uc1" TagName="ucPaymentDetails" %>
<%@ Register Src="~/OMS/Management/Activities/UserControls/UOMConversion.ascx" TagPrefix="uc3" TagName="UOMConversionControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type='text/javascript'>
        var SecondUOM = [];
        var ModuleName = 'POS';
        var strProAlt = '';
    </script>
    <link rel="stylesheet" href="http://yui.yahooapis.com/pure/0.6.0/pure-min.css" />
    <link href="https://cdn.datatables.net/1.10.19/css/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="https://cdn.datatables.net/1.10.19/js/jquery.dataTables.min.js"></script>
    <script src="https://cdn.datatables.net/fixedcolumns/3.3.0/js/dataTables.fixedColumns.min.js"></script>
    <link href="CSS/PosSalesInvoice.css" rel="stylesheet" />
    <script src="../Activities/JS/ProductStockIN.js?v1.00.00.09"></script>

    <script src="../../Tax%20Details/Js/TaxDetailsItemlevel.js?var=1.2" type="text/javascript"></script>
    <%--Use for set focus on UOM after press ok on UOM--%>
    <script>
        $(document).ready(function () {
            $('#UOMModal').on('hide.bs.modal', function sub() {
                grid.batchEditApi.StartEdit(globalRowIndex, 6);
            })
        });

        function SaveWithApprover() {
            $("#hdnApprover").val("1");
            cpopupApprover.Hide();
            grid.AddNewRow();
            grid.UpdateEdit();
        }
    </script>
    <%--Use for set focus on UOM after press ok on UOM--%>


    <%--chinmoy js added for add on fly Product--%>
    <%--start--%>
    <style>
        .myImage {
            max-height: 100px;
            max-width: 100px;
        }

        .boxarea {
            border: 1px solid #a7a6a64a;
            position: relative;
            margin: 15px;
            padding-top: 8px;
            padding-bottom: 7px;
        }

        .boxareaH {
            position: absolute;
            font-size: 14px;
            font-weight: bold;
            top: -13px;
            left: 9px;
            /* border: 1px solid #ccc; */
            background: #edf3f4;
            padding: 3px 5px;
            color: #b11212;
        }
    </style>
    <script type="text/javascript">

        //document.onkeydown = function (e) { 
        //  //  console.log(e);
        //    if (e.ctrlKey && (e.key == 's' || e.key == 'S'))
        //    {
        //        console.log('save not');
        //        if (cPopup_Empcitys.IsVisible()) {
        //            console.log('save');
        //            cbtnSave_citys.DoClick();
        //        }
        //        return false;
        //    }

        //    if (e.ctrlKey && (e.key == 'a' || e.key == 'A')) { 
        //        if (!cPopup_Empcitys.IsVisible()) {
        //            console.log('new');
        //            fn_PopOpen();
        //            return false;
        //        }

        //    }

        //    //if (event.keyCode == 17) isCtrl = true;
        //    //if (event.keyCode == 83 && isCtrl == true) { 
        //    //    console.log(e);
        //    //    return false;

        //    //} 

        //}






        //Added for lite popup

        function ClassButnClick(s, e) {
            var txt = "<table border='1' width=\"100%\"><tr class=\"HeaderStyle\"><th>Class Name</th></tr><table>";
            $("#txtClassNameSearch").val("");
            document.getElementById("ClassTable").innerHTML = txt;
            cPopClass.Show();

        }

        function Class_KeyDown(s, e) {


            if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
                s.OnButtonClick(0);
                $("#txtClassNameSearch").focus();
            }
        }

        function Classkeydown(e) {

            var OtherDetails = {}
            OtherDetails.SearchKey = $("#txtClassNameSearch").val();
            if (e.code == "Enter" || e.code == "NumpadEnter") {
                if ($("#txtClassNameSearch").val() == "")
                    return;
                var HeaderCaption = [];
                HeaderCaption.push("Class Name");
                //callonServer("../Master/sProducts.aspx/GetMainAccount", OtherDetails, "MainAccountTable", HeaderCaption, "MainAccountSIIndex", "SetMainAccountSI");
                callonServer("../Activities/PosSalesInvoice.aspx/GetClassDetails", OtherDetails, "ClassTable", HeaderCaption, "classIndex", "SetClass");
            }
            else if (e.code == "ArrowDown") {
                if ($("input[classIndex=0]"))
                    $("input[classIndex=0]").focus();
            }
            else if (e.code == "Escape") {
                cPopClass.Hide();
                cPopClass.Focus();
            }


        }

        var PosGstId = "";
        function PopulatePosGst(e) {

            PosGstId = cddl_PosGst.GetValue();
            if (PosGstId == "S") {
                cddl_PosGst.SetValue("S");
            }
            else if (PosGstId == "B") {
                cddl_PosGst.SetValue("B");
            }
        }

        function GetShippingStateName() {

            return $('#lblShippingState').text();
        }

        function GetBillingStateName() {
            return $('#lblBillingState').text();
        }

        function GetPosForGstValue() {
            cddl_PosGst.ClearItems();
            if (cddl_PosGst.GetItemCount() == 0) {
                cddl_PosGst.AddItem(GetShippingStateName() + '[Shipping]', "S");
                cddl_PosGst.AddItem(GetBillingStateName() + '[Billing]', "B");
            }
            else if (cddl_PosGst.GetItemCount() > 2) {
                cddl_PosGst.ClearItems();
                //cddl_PosGst.RemoveItem(0);
                //cddl_PosGst.RemoveItem(0);
            }

            if (PosGstId == "" || PosGstId == null) {
                cddl_PosGst.SetValue("B");
            }
            else {
                cddl_PosGst.SetValue(PosGstId);
            }
        }


        function SetClass(id, Name) {

            var key = id;
            $('#ClassId').val(id)
            if (key != null && key != '') {
                // $('#CustModel').modal('hide');
                cProClassCode.SetText(Name);
                cPopClass.Hide();
                cCmbStatus.SetFocus();
            }
        }

        function HSNButnClick(s, e) {
            var txt = "<table border='1' width=\"100%\"><tr class=\"HeaderStyle\"><th style='width:150px;'>Code</th><th>Description</th></tr><table>";
            $("#txtHSNSearch").val("");
            document.getElementById("HSNTable").innerHTML = txt;
            cPopHSN.Show();
            $("#txtHSNSearch").focus();
        }

        function HSNCode_KeyDown(s, e) {

            if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
                s.OnButtonClick(0);
                $("#txtHSNSearch").focus();
            }
        }


        function HSNkeydown(e) {

            var OtherDetails = {}
            OtherDetails.SearchKey = $("#txtHSNSearch").val();
            if (e.code == "Enter" || e.code == "NumpadEnter") {
                if ($("#txtHSNSearch").val() == "")
                    return;
                var HeaderCaption = [];
                HeaderCaption.push("Code");
                HeaderCaption.push("Description");
                //callonServer("../Master/sProducts.aspx/GetMainAccount", OtherDetails, "MainAccountTable", HeaderCaption, "MainAccountSIIndex", "SetMainAccountSI");
                callonServer("../Activities/PosSalesInvoice.aspx/GetHSNDetails", OtherDetails, "HSNTable", HeaderCaption, "HSNIndex", "SetHSN");
            }
            else if (e.code == "ArrowDown") {
                if ($("input[HSNIndex=0]"))
                    $("input[HSNIndex=0]").focus();
            }
            else if (e.code == "Escape") {
                cPopHSN.Hide();
                cPopHSN.Focus();
            }


        }

        function SetHSN(id, Name) {

            var key = id;
            $('#hdnHSN').val(id)
            if (key != null && key != '') {
                // $('#CustModel').modal('hide');
                cHSNCode.SetText(id);
                cCmbTradingLotUnits.SetFocus();
                cPopHSN.Hide();
            }
        }
        function SizeUOMChange() {

            $("#covergaeUOM").val($("#SizeUOM").val());
            var first = ctxtPackingQty.GetValue();
            var second = ctxtpacking.GetValue();

            if (ctxtpackingSaleUom.GetText() == "") {
                first = 0;
            }

            if (ccmbPackingUomPro.GetText() == "") {
                second = 0;
            }



            if ($("#SizeUOM").val() == "1") {
                var cov = parseFloat(ctxtHeight.GetText()) * parseFloat(ctxtWidth.GetText() * parseFloat(first));
                var vol = parseFloat(ctxtHeight.GetText()) * parseFloat(ctxtWidth.GetText()) * parseFloat(ctxtThickness.GetText() * parseFloat(first));
            }
            else {
                var cov = parseFloat(ctxtHeight.GetText()) * parseFloat(ctxtWidth.GetText() * parseFloat(second));
                var vol = parseFloat(ctxtHeight.GetText()) * parseFloat(ctxtWidth.GetText()) * parseFloat(ctxtThickness.GetText() * parseFloat(second));
            }

            if (cov == 0) {
                $("#txtCoverage").attr("disabled", false);
            }
            else {
                $("#txtCoverage").attr("disabled", true);
            }

            $("#txtCoverage").val(cov);
            $("#txtVolumn").val(vol);
            $("#dvCovg").text(cddlSize.GetText() + '²');
            $("#dvvolume").text(cddlSize.GetText() + '³');

        }















        //*************************************************  SI Main Account  *********************************************************************************

        function MainAccountButnClick() {
            var txt = "<table border='1' width=\"100%\"><tr class=\"HeaderStyle\"><th>Main Account Name</th></tr><table>";
            $("#txtMainAccountSearch").val("");
            document.getElementById("MainAccountTable").innerHTML = txt;
            cMainAccountModelSI.Show();
        }

        function MainAccountNewkeydown(e) {
            var OtherDetails = {}
            OtherDetails.SearchKey = $("#txtMainAccountSearch").val();
            if (e.code == "Enter" || e.code == "NumpadEnter") {
                if ($("#txtMainAccountSearch").val() == "")
                    return;
                var HeaderCaption = [];
                HeaderCaption.push("Main Account Name");
                callonServer("purchaseinvoice.aspx/GetMainAccount", OtherDetails, "MainAccountTable", HeaderCaption, "MainAccountSIIndex", "SetMainAccountSI");
            }
            else if (e.code == "ArrowDown") {
                if ($("input[MainAccountSIIndex=0]"))
                    $("input[MainAccountSIIndex=0]").focus();
            }
            else if (e.code == "Escape") {
                cMainAccountModelSI.Hide();
                cMainAccountModelSI.Focus();
            }
        }

        function SetMainAccountSI(Id, name, e) {
            $("#hdnSIMainAccount").val(Id);
            cSIMainAccount.SetText(name);
            cSIMainAccount_active.SetText(name);
            cMainAccountModelSI.Hide();
        }

        //************************************************* End SI Main Account  *********************************************************************************



        //*************************************************  SR Main Account  *********************************************************************************

        function SRMainAccountButnClick() {
            var txt = "<table border='1' width=\"100%\"><tr class=\"HeaderStyle\"><th>Main Account Name</th></tr><table>";
            document.getElementById("MainAccountTableSR").innerHTML = txt;
            $("#txtMainAccountSRSearch").val("");
            cMainAccountModelSR.Show();
        }
        function MainAccountSRNewkeydown(e) {
            var OtherDetails = {}
            OtherDetails.SearchKey = $("#txtMainAccountSRSearch").val();
            if (e.code == "Enter" || e.code == "NumpadEnter") {
                if ($("#txtMainAccountSRSearch").val() == "")
                    return;
                var HeaderCaption = [];
                HeaderCaption.push("Main Account Name");
                callonServer("purchaseinvoice.aspx/GetMainAccount", OtherDetails, "MainAccountTableSR", HeaderCaption, "MainAccountSRIndex", "SetMainAccountSR");
            }
            else if (e.code == "ArrowDown") {
                if ($("input[MainAccountSRIndex=0]"))
                    $("input[MainAccountSRIndex=0]").focus();
            }
            else if (e.code == "Escape") {
                cMainAccountModelSR.Hide();
            }
        }
        function SetMainAccountSR(Id, name, e) {
            $("#hdnSRMainAccount").val(Id);
            cSRMainAccount.SetText(name);
            cSRMainAccount_active.SetText(name)
            cMainAccountModelSR.Hide();
        }

        //************************************************* End SR Main Account  *********************************************************************************


        //*************************************************  PI Main Account  *********************************************************************************

        function PIMainAccountButnClick() {
            var txt = "<table border='1' width=\"100%\"><tr class=\"HeaderStyle\"><th>Main Account Name</th></tr><table>";
            document.getElementById("MainAccountTablePI").innerHTML = txt;
            $("#txtMainAccountPISearch").val("");
            cMainAccountModelPI.Show();
        }

        function MainAccountPINewkeydown(e) {
            var OtherDetails = {}
            OtherDetails.SearchKey = $("#txtMainAccountPISearch").val();
            if (e.code == "Enter" || e.code == "NumpadEnter") {
                if ($("#txtMainAccountPISearch").val() == "")
                    return;
                var HeaderCaption = [];
                HeaderCaption.push("Main Account Name");
                callonServer("purchaseinvoice.aspx/GetMainAccount", OtherDetails, "MainAccountTablePI", HeaderCaption, "MainAccountPIIndex", "SetMainAccountPI");
            }
            else if (e.code == "ArrowDown") {
                if ($("input[MainAccountPIIndex=0]"))
                    $("input[MainAccountPIIndex=0]").focus();
            }
            else if (e.code == "Escape") {
                cMainAccountModelPI.Hide();
            }
        }
        function SetMainAccountPI(Id, name, e) {
            $("#hdnPIMainAccount").val(Id);
            cPIMainAccount.SetText(name);
            cPIMainAccount_active.SetText(name);
            cMainAccountModelPI.Hide();
        }

        //************************************************* End PI Main Account  *********************************************************************************



        //*************************************************  PR Main Account  *********************************************************************************

        function PRMainAccountButnClick() {
            var txt = "<table border='1' width=\"100%\"><tr class=\"HeaderStyle\"><th>Main Account Name</th></tr><table>";
            document.getElementById("MainAccountTablePR").innerHTML = txt;
            $("#txtMainAccountPRSearch").val("");
            cMainAccountModelPR.Show();
        }

        function MainAccountPRNewkeydown(e) {
            var OtherDetails = {}
            OtherDetails.SearchKey = $("#txtMainAccountPRSearch").val();
            if (e.code == "Enter" || e.code == "NumpadEnter") {
                if ($("#txtMainAccountPRSearch").val() == "")
                    return;
                var HeaderCaption = [];
                HeaderCaption.push("Main Account Name");
                callonServer("purchaseinvoice.aspx/GetMainAccount", OtherDetails, "MainAccountTablePR", HeaderCaption, "MainAccountPRIndex", "SetMainAccountPR");
            }
            else if (e.code == "ArrowDown") {
                if ($("input[MainAccountPRIndex=0]"))
                    $("input[MainAccountPRIndex=0]").focus();
            }
            else if (e.code == "Escape") {
                cMainAccountModelPR.Hide();
            }
        }
        function SetMainAccountPR(Id, name, e) {
            $("#hdnPRMainAccount").val(Id);
            cPRMainAccount.SetText(name);
            cPRMainAccount_active.SetText(name);
            cMainAccountModelPR.Hide();
        }

        //************************************************* End PR Main Account  *********************************************************************************

        function MainAccountKeyDown(s, e) {
            if (e.htmlEvent.key == "Enter" || e.htmlEvent.key == "NumEnter") {
                s.OnButtonClick(0);
            }
        }

        //function ValueSelected(e, indexName) {
        //    if (indexName == "MainAccountSIIndex") {
        //        if (e.code == "Enter" || e.code == "NumpadEnter") {
        //            var Code = e.target.parentElement.parentElement.cells[0].innerText;
        //            var name = e.target.parentElement.parentElement.cells[1].children[0].value;

        //            $("#hdnSIMainAccount").val(Code);
        //            cSIMainAccount.SetText(name);
        //            cSIMainAccount_active.SetText(name);
        //            cMainAccountModelSI.Hide();
        //        } else if (e.code == "ArrowDown") {
        //            thisindex = parseFloat(e.target.getAttribute(indexName));
        //            thisindex++;
        //            if (thisindex < 10)
        //                $("input[" + indexName + "=" + thisindex + "]").focus();
        //        }

        //        else if (e.code == "ArrowUp") {
        //            thisindex = parseFloat(e.target.getAttribute(indexName));
        //            thisindex--;
        //            if (thisindex > -1)
        //                $("input[" + indexName + "=" + thisindex + "]").focus();
        //            else {
        //                $('#txtMainAccountSearch').focus();
        //            }
        //        }

        //    }

        //    else if (indexName == "HSNIndex") {
        //        if (e.code == "Enter" || e.code == "NumpadEnter") {
        //            var Code = e.target.parentElement.parentElement.cells[0].innerText;
        //            var name = e.target.parentElement.parentElement.cells[1].children[0].value;

        //            $("#hdnHSN").val(Code);
        //            cHSNCode.SetText(name);

        //            cPopHSN.Hide();
        //        } else if (e.code == "ArrowDown") {
        //            thisindex = parseFloat(e.target.getAttribute(indexName));
        //            thisindex++;
        //            if (thisindex < 10)
        //                $("input[" + indexName + "=" + thisindex + "]").focus();
        //        }

        //        else if (e.code == "ArrowUp") {
        //            thisindex = parseFloat(e.target.getAttribute(indexName));
        //            thisindex--;
        //            if (thisindex > -1)
        //                $("input[" + indexName + "=" + thisindex + "]").focus();
        //            else {
        //                $('#txtHSNSearch').focus();
        //            }
        //        }

        //    }



        //    else if (indexName == "customerIndex") {
        //        if (e.code == "Enter" || e.code == "NumpadEnter") {
        //            var Code = e.target.parentElement.parentElement.cells[0].innerText;
        //            var name = e.target.parentElement.parentElement.cells[1].children[0].value;

        //            $("#ClassId").val(Code);
        //            cProClassCode.SetText(name);

        //            cPopClass.Hide();
        //        } else if (e.code == "ArrowDown") {
        //            thisindex = parseFloat(e.target.getAttribute(indexName));
        //            thisindex++;
        //            if (thisindex < 10)
        //                $("input[" + indexName + "=" + thisindex + "]").focus();
        //        }

        //        else if (e.code == "ArrowUp") {
        //            thisindex = parseFloat(e.target.getAttribute(indexName));
        //            thisindex--;
        //            if (thisindex > -1)
        //                $("input[" + indexName + "=" + thisindex + "]").focus();
        //            else {
        //                $('#txtClassNameSearch').focus();
        //            }
        //        }

        //    }




        //    else if (indexName == "MainAccountSRIndex") {
        //        if (e.code == "Enter" || e.code == "NumpadEnter") {
        //            var Code = e.target.parentElement.parentElement.cells[0].innerText;
        //            var name = e.target.parentElement.parentElement.cells[1].children[0].value;

        //            $("#hdnSRMainAccount").val(Code);
        //            cSRMainAccount.SetText(name);
        //            cSRMainAccount_active.SetText(name)
        //            cMainAccountModelSR.Hide();
        //        } else if (e.code == "ArrowDown") {
        //            thisindex = parseFloat(e.target.getAttribute(indexName));
        //            thisindex++;
        //            if (thisindex < 10)
        //                $("input[" + indexName + "=" + thisindex + "]").focus();
        //        }

        //        else if (e.code == "ArrowUp") {
        //            thisindex = parseFloat(e.target.getAttribute(indexName));
        //            thisindex--;
        //            if (thisindex > -1)
        //                $("input[" + indexName + "=" + thisindex + "]").focus();
        //            else {
        //                $('#txtMainAccountSRSearch').focus();
        //            }
        //        }

        //    }
        //    else if (indexName == "MainAccountPIIndex") {
        //        if (e.code == "Enter" || e.code == "NumpadEnter") {
        //            var Code = e.target.parentElement.parentElement.cells[0].innerText;
        //            var name = e.target.parentElement.parentElement.cells[1].children[0].value;

        //            $("#hdnPIMainAccount").val(Code);
        //            cPIMainAccount.SetText(name);
        //            cPIMainAccount_active.SetText(name);
        //            cMainAccountModelPI.Hide();
        //        } else if (e.code == "ArrowDown") {
        //            thisindex = parseFloat(e.target.getAttribute(indexName));
        //            thisindex++;
        //            if (thisindex < 10)
        //                $("input[" + indexName + "=" + thisindex + "]").focus();
        //        }

        //        else if (e.code == "ArrowUp") {
        //            thisindex = parseFloat(e.target.getAttribute(indexName));
        //            thisindex--;
        //            if (thisindex > -1)
        //                $("input[" + indexName + "=" + thisindex + "]").focus();
        //            else {
        //                $('#txtMainAccountPISearch').focus();
        //            }
        //        }

        //    }
        //    else if (indexName == "MainAccountPRIndex") {
        //        if (e.code == "Enter" || e.code == "NumpadEnter") {
        //            var Code = e.target.parentElement.parentElement.cells[0].innerText;
        //            var name = e.target.parentElement.parentElement.cells[1].children[0].value;

        //            $("#hdnPRMainAccount").val(Code);
        //            cPRMainAccount.SetText(name);
        //            cPRMainAccount_active.SetText(name);
        //            cMainAccountModelPR.Hide();
        //        } else if (e.code == "ArrowDown") {
        //            thisindex = parseFloat(e.target.getAttribute(indexName));
        //            thisindex++;
        //            if (thisindex < 10)
        //                $("input[" + indexName + "=" + thisindex + "]").focus();
        //        }

        //        else if (e.code == "ArrowUp") {
        //            thisindex = parseFloat(e.target.getAttribute(indexName));
        //            thisindex--;
        //            if (thisindex > -1)
        //                $("input[" + indexName + "=" + thisindex + "]").focus();
        //            else {
        //                $('#txtMainAccountPRSearch').focus();
        //            }
        //        }

        //    }
        //}




        //End lite Pop up

        var PurReturnOldValue = '';
        function cmbPurReturnGotFocus(s, e) {
            PurReturnOldValue = s.GetValue();
        }

        function mainAccountPurReturn(s, e) {
            for (var i = 0; i < mainAccountInUse.length; i++) {
                if (mainAccountInUse[i] == 'purchaseReturn') {

                    jAlert("Transaction exists for the selected Ledger. Cannot proceed.");
                    s.SetValue(PurReturnOldValue);
                }
            }
        }


        var PurInvoiceOldValue = '';
        function cmbPurInvoiceGotFocus(s, e) {
            PurInvoiceOldValue = s.GetValue();
        }

        function mainAccountPurInvoice(s, e) {

            for (var i = 0; i < mainAccountInUse.length; i++) {

                if (mainAccountInUse[i] == 'purchaseInvoice') {
                    jAlert("Transaction exists for the selected Ledger. Cannot proceed.");
                    s.SetValue(PurInvoiceOldValue);
                }

            }

        }

        var salesReturnOldValue = '';
        function cmbsalesReturnGotFocus(s, e) {
            salesReturnOldValue = s.GetValue();
        }

        function mainAccountSalesReturn(s, e) {
            for (var i = 0; i < mainAccountInUse.length; i++) {
                if (mainAccountInUse[i] == 'salesReturn') {
                    jAlert("Transaction exists for the selected Ledger. Cannot proceed.");
                    s.SetValue(salesReturnOldValue);
                }
            }
        }


        var salesInvoiceOldValue = '';
        function cmbsalesInvoiceGotFocus(s, e) {
            salesInvoiceOldValue = s.GetValue();
        }

        function mainAccountSalesInvoice(s, e) {
            for (var i = 0; i < mainAccountInUse.length; i++) {
                if (mainAccountInUse[i] == 'SalesInvoice') {

                    jAlert("Transaction exists for the selected Ledger. Cannot proceed.");
                    s.SetValue(salesInvoiceOldValue);
                }
            }
        }


        $(function () {
            var vAnotherKeyWasPressed = false;
            var ALT_CODE = 18;

            //When some key is pressed
            $(window).keydown(function (event) {
                var vKey = event.keyCode ? event.keyCode : event.which ? event.which : event.charCode;
                vAnotherKeyWasPressed = vKey != ALT_CODE;
                if (!LoadingPanel.IsVisible()) {
                    if (event.altKey && (event.key == 's' || event.key == 'S')) {
                        console.log('save not');
                        if (cPopup_Empcitys.IsVisible()) {
                            if (cbtnSave_Product.IsVisible())
                                cbtnSave_Product.DoClick();
                        }
                        return false;
                    }

                    if (event.altKey && (event.key == 'a' || event.key == 'A')) {
                        if (!cPopup_Empcitys.IsVisible()) {
                            if (document.getElementById('AddBtn') != null) {
                                console.log('new');
                                fn_PopOpen();
                                return false;
                            }

                        }

                    }

                    if (event.altKey && (event.key == 'c' || event.key == 'C')) {
                        console.log('save not');
                        if (cPopup_Empcitys.IsVisible()) {
                            fn_btnCancel();
                        }
                        return false;
                    }
                }
            });

            //When some key is left
            $(window).keyup(function (event) {

                var vKey = event.keyCode ? event.keyCode : event.which ? event.which : event.charCode;

            });
        });



        function SetHSnPanelEndCallBack() {
            LoadingPanel.Hide();
            if (cSetHSnPanel.cpHsnCode) {
                if (cSetHSnPanel.cpHsnCode != "") {
                    //cHsnLookUp.gridView.SelectItemsByKey(cSetHSnPanel.cpHsnCode);
                    cSetHSnPanel.cpHsnCode = null;
                } else {
                    cHsnLookUp.Clear();
                }
            }
        }
        function CmbProClassCodeChanged(s, e) {
            if (s.GetValue() != null) {
                cSetHSnPanel.PerformCallback(s.GetValue());
                LoadingPanel.SetText('Please wait searching HSN...');
                LoadingPanel.Show();
            }

        }
        function cCmbTradingLotUnitsLostFocus() {
            var saleUomVal = cCmbTradingLotUnits.GetValue();
            if (saleUomVal == null) {
                $('#btnPackingConfig').attr('disabled', 'disabled');
            }
            else {
                $('#btnPackingConfig').attr('disabled', false);
            }
        }

        function ShowTdsSection() {
            ctdsPopup.Show();
        }

        function ShowPackingDetails() {
            $('#invalidPackingUom').css({ 'display': 'none' });
            ctxtpackingSaleUom.SetText(cCmbTradingLotUnits.GetText());
            ctxtpackingSaleUom.SetEnabled(false);
            cpackingDetails.Show();
            ctxtPackingQty.SetFocus();
        }

        function PackingDetailsOkClick() {

            $('#invalidPackingUom').css({ 'display': 'none' });

            if (ccmbPackingUomPro.GetValue() == null) {
                $('#invalidPackingUom').css({ 'display': 'block' });
            } else {
                cpackingDetails.Hide();
            }
            cbtnSave_Product.SetFocus();
        }

        function ServicetaxOkClick() {
            cServiceTaxPopup.Hide();
        }
        function isInventoryChanged(s, e) {
            //changeControlStateWithInventory(s.GetValue());
            changeControlStateWithInventory();
        }
        function isCapitalChanged(s, e) {

            var Inv = ccmbIsInventory.GetValue();
            var cap = ccmbIsCapitalGoods.GetValue();
            if (Inv != 1) {
                if (cap == 1) {
                    cCmbProType.SetEnabled(true);
                    cCmbStockValuation.SetEnabled(true);
                    //HsnChange
                    //   caspxHsnCode.SetEnabled(true);
                    // cHsnLookUp.SetEnabled(true);
                    ctxtQuoteLot.SetEnabled(true);
                    cCmbQuoteLotUnit.SetEnabled(true);
                    ctxtTradingLot.SetEnabled(true);
                    cCmbTradingLotUnits.SetEnabled(true);
                    ctxtDeliveryLot.SetEnabled(true);
                    cCmbDeliveryLotUnit.SetEnabled(true);
                    ccmbStockUom.SetEnabled(true);
                    ctxtMinLvl.SetEnabled(true);
                    ctxtReorderLvl.SetEnabled(true);
                    ccmbNegativeStk.SetEnabled(true);


                    $('#btnBarCodeConfig').attr('disabled', false);
                    $('#btnProdConfig').attr('disabled', false);

                    $('#btnServiceTaxConfig').attr('disabled', 'disabled');
                    cAspxServiceTax.SetValue('');

                    $('#btnTDS').attr('disabled', 'disabled');
                    cmb_tdstcs.SetValue('');
                }
                else {
                    cCmbProType.SetText('');
                    cCmbProType.SetEnabled(false);

                    cCmbStockValuation.SetValue('A');
                    cCmbStockValuation.SetEnabled(false);

                    //caspxHsnCode.SetText('');
                    //caspxHsnCode.SetEnabled(false);
                    // cHsnLookUp.SetEnabled(false);
                    // cHsnLookUp.Clear();

                    ctxtQuoteLot.SetText('0');
                    ctxtQuoteLot.SetEnabled(false);

                    cCmbQuoteLotUnit.SetText('0');
                    cCmbQuoteLotUnit.SetEnabled(false);

                    ctxtTradingLot.SetText('0');
                    ctxtTradingLot.SetEnabled(false);

                    cCmbTradingLotUnits.SetText('');
                    cCmbTradingLotUnits.SetEnabled(false);

                    ctxtDeliveryLot.SetText('0');
                    ctxtDeliveryLot.SetEnabled(false);

                    cCmbDeliveryLotUnit.SetText('');
                    cCmbDeliveryLotUnit.SetEnabled(false);


                    ccmbStockUom.SetText('');
                    ccmbStockUom.SetEnabled(false);

                    ctxtMinLvl.SetText('0');
                    ctxtMinLvl.SetEnabled(false);

                    ctxtReorderLvl.SetText('0');
                    ctxtReorderLvl.SetEnabled(false);

                    ccmbNegativeStk.SetValue('I');
                    ccmbNegativeStk.SetEnabled(false);

                    //Product Configuration
                    $('#btnProdConfig').attr('disabled', 'disabled');
                    $('#btnBarCodeConfig').attr('disabled', 'disabled');
                    $('#btnServiceTaxConfig').attr('disabled', false);
                    $('#btnTDS').attr('disabled', false);
                    $('#btnPackingConfig').attr('disabled', 'disabled');
                }
            }
        }


        function changeControlStateWithInventory(obj) {
            obj = ccmbIsInventory.GetValue();
            if (obj == 1) {
                cCmbProType.SetEnabled(true);
                cCmbStockValuation.SetEnabled(true);
                //HsnChange
                //   caspxHsnCode.SetEnabled(true);
                // cHsnLookUp.SetEnabled(true);
                ctxtQuoteLot.SetEnabled(true);
                cCmbQuoteLotUnit.SetEnabled(true);
                ctxtTradingLot.SetEnabled(true);
                cCmbTradingLotUnits.SetEnabled(true);
                ctxtDeliveryLot.SetEnabled(true);
                cCmbDeliveryLotUnit.SetEnabled(true);
                ccmbStockUom.SetEnabled(true);
                ctxtMinLvl.SetEnabled(true);
                ctxtReorderLvl.SetEnabled(true);
                ccmbNegativeStk.SetEnabled(true);
                ccmbServiceItem.SetValue('0');
                ccmbServiceItem.SetEnabled(false);

                $('#btnBarCodeConfig').attr('disabled', false);
                $('#btnProdConfig').attr('disabled', false);

                $('#btnServiceTaxConfig').attr('disabled', 'disabled');
                cAspxServiceTax.SetValue('');

                $('#btnTDS').attr('disabled', 'disabled');
                cmb_tdstcs.SetValue('');

            } else {
                cCmbProType.SetText('');
                cCmbProType.SetEnabled(false);

                cCmbStockValuation.SetValue('A');
                cCmbStockValuation.SetEnabled(false);

                //caspxHsnCode.SetText('');
                //caspxHsnCode.SetEnabled(false);
                // cHsnLookUp.SetEnabled(false);
                // cHsnLookUp.Clear();

                ctxtQuoteLot.SetText('1');
                ctxtQuoteLot.SetEnabled(false);

                cCmbQuoteLotUnit.SetText('');
                cCmbQuoteLotUnit.SetEnabled(false);

                ctxtTradingLot.SetText('1');
                ctxtTradingLot.SetEnabled(false);

                cCmbTradingLotUnits.SetText('');
                cCmbTradingLotUnits.SetEnabled(false);

                ctxtDeliveryLot.SetText('1');
                ctxtDeliveryLot.SetEnabled(false);

                cCmbDeliveryLotUnit.SetText('');
                cCmbDeliveryLotUnit.SetEnabled(false);

                ccmbStockUom.SetText('');
                ccmbStockUom.SetEnabled(false);

                ctxtMinLvl.SetText('0');
                ctxtMinLvl.SetEnabled(false);

                ctxtReorderLvl.SetText('0');
                ctxtReorderLvl.SetEnabled(false);


                ccmbServiceItem.SetEnabled(true);

                ccmbNegativeStk.SetValue('I');
                ccmbNegativeStk.SetEnabled(false);

                //Product Configuration
                $('#btnProdConfig').attr('disabled', 'disabled');
                $('#btnBarCodeConfig').attr('disabled', 'disabled');
                $('#btnServiceTaxConfig').attr('disabled', false);
                $('#btnTDS').attr('disabled', false);
                $('#btnPackingConfig').attr('disabled', 'disabled');
            }
        }
        //Code for UDF Control 
        function OpenUdf() {
            if (document.getElementById('IsUdfpresent').value == '0') {
                jAlert("UDF not define.");
            }
            else {
                var keyVal = document.getElementById('Keyval_internalId').value;
                var url = '/OMS/management/Master/frm_BranchUdfPopUp.aspx?Type=Prd&&KeyVal_InternalID=' + keyVal;
                popup.SetContentUrl(url);
                popup.Show();
            }
            return true;
        }
        // End Udf Code
        //Declare some global variable for poopUP
        var barCodeType;
        var BarCode, GlobalCode;
        var taxCodeSale, taxCodePur, taxScheme;
        var autoApply;
        var ProdColor, ProdSize, ColApp, SizeApp;
        var tdsValue = '';
        //Declare some global variable for poopUP End Here

        //Close the particular popup on esc
        function OnInitTax(s, e) {
            ASPxClientUtils.AttachEventToElement(window.document, "keydown", function (evt) {
                if (evt.keyCode == ASPxClientUtils.StringToShortcutCode("ESCAPE"))
                    cTaxCodePopup.Hide();
            });
        }
        function OnInitBarCode(s, e) {
            ASPxClientUtils.AttachEventToElement(window.document, "keydown", function (evt) {
                if (evt.keyCode == ASPxClientUtils.StringToShortcutCode("ESCAPE"))
                    cBarCodePopUp.Hide();
            });
        }
        function OnInitProductAttribute(s, e) {
            ASPxClientUtils.AttachEventToElement(window.document, "keydown", function (evt) {
                if (evt.keyCode == ASPxClientUtils.StringToShortcutCode("ESCAPE"))
                    cproductAttributePopUp.Hide();
            });
        }

        //for Image Upload

        function OnUploadComplete(args) {
            console.log(args.callbackData);
            document.getElementById('fileName').value = args.callbackData;
            // cProdImage.SetImageUrl(args.callbackData);
            afterFileUpload();

        }

        function uploadClick() {
            Callback1.PerformCallback('');
        }


        function onFileUploadStart(s, e) {
            uploadInProgress = true;
            uploadErrorOccurred = false;
        }
        //Image upload end here


        //code added by debjyoti 04-01-2017
        function ShowProductAttribute() {

            cproductAttributePopUp.Show();
            cCmbProductColor.SetFocus();
        }

        function productAttributeOkClik() {
            //Surojit 04-03-2019
            var ismandatory = cchkIsMandatory.GetValue();
            var textvalue = GridLookup_I.value;
            if (ismandatory && (textvalue == "" || textvalue == null)) {
                jAlert("Please select atleast one components!");
                return false;
            }
            else {

                ProdSize = cCmbProductSize.GetValue();
                ProdColor = cCmbProductColor.GetValue();
                ColApp = RrdblappColor.GetSelectedIndex();
                SizeApp = Rrdblapp.GetSelectedIndex();




                cproductAttributePopUp.Hide();
            }
            //Surojit 04-03-2019
        }


        function ShowBarCode() {
            cBarCodePopUp.Show();
        }
        function BarCodeOkClick() {
            barCodeType = cCmbBarCodeType.GetSelectedIndex();
            BarCode = ctxtBarCodeNo.GetText();
            GlobalCode = ctxtGlobalCode.GetText();
            cBarCodePopUp.Hide();
        }

        function ShowTaxCode() {
            cTaxCodePopup.Show();
        }

        function ShowServiceTax() {
            cServiceTaxPopup.Show();
        }


        function taxCodeOkClick() {
            taxCodeSale = cCmbTaxCodeSale.GetValue();
            taxCodePur = cCmbTaxCodePur.GetValue();
            autoApply = cChkAutoApply.GetChecked();
            taxScheme = cCmbTaxScheme.GetValue();
            cTaxCodePopup.Hide();
        }

        function GetCheckBoxValue(value) {
            //var value = s.GetChecked();
            if (value == true) {
                cCmbTaxCodePur.SetValue(0);
                cCmbTaxCodePur.SetEnabled(false);

                cCmbTaxCodeSale.SetValue(0);
                cCmbTaxCodeSale.SetEnabled(false);

                cCmbTaxScheme.SetEnabled(true);

            } else {
                cCmbTaxScheme.SetValue(0);
                cCmbTaxScheme.SetEnabled(false);
                cCmbTaxCodePur.SetEnabled(true);
                cCmbTaxCodeSale.SetEnabled(true);
            }
        }

        function CloseGridLookup() {
            gridLookup.ConfirmCurrentSelection();
            gridLookup.HideDropDown();
            gridLookup.Focus();
        }
        //changes end here 04-01-2017


        function fn_AllowonlyNumeric(s, e) {
            var theEvent = e.htmlEvent || window.event;
            var key = theEvent.keyCode || theEvent.which;
            var keychar = String.fromCharCode(key);
            if (key == 9 || key == 37 || key == 38 || key == 39 || key == 40 || key == 8 || key == 46) { //tab/ Left / Up / Right / Down Arrow, Backspace, Delete keys
                return;
            }
            var regex = /[0-9\b]/;

            if (!regex.test(keychar)) {
                theEvent.returnValue = false;
                if (theEvent.preventDefault)
                    theEvent.preventDefault();
            }
        }


        function fn_ProductPopOpen() {
            var url = '/OMS/management/Store/Master/ProductPopup.html?var=4.9';
            cPosView.SetContentUrl(url);
            cPosView.RefreshContentUrl();

            cPosView.Show();

        }


        function fn_productSave() {
            cPosView.Hide();
        }

        function fn_PopOpen() {
            //Surojit
            if ($('#hdnProductMasterComponentMandatoryVisible').val() == "0") {
                $('#divProductMasterComponentMandatory').hide();
            }
            else {
                $('#divProductMasterComponentMandatory').show();
            }
            //Surojit

            cbtnSave_Product.SetVisible(true);
            mainAccountInUse = [];
            //ccmbsalesInvoice.SetSelectedIndex(0); SI
            //ccmbSalesReturn.SetSelectedIndex(0);
            //ccmbPurInvoice.SetSelectedIndex(0);
            //ccmbPurReturn.SetSelectedIndex(0);

            $("#hdnSIMainAccount").val("");
            $("#hdnSRMainAccount").val("");
            $("#hdnPIMainAccount").val("");
            $("#hdnPRMainAccount").val("");

            cSIMainAccount.SetText("");
            cSRMainAccount.SetText("");
            cPIMainAccount.SetText("");
            cPRMainAccount.SetText("");

            cProClassCode.SetText("");
            cHSNCode.SetText("");
            $("#hdnHSN").val("");
            $("#ClassId").val("");





            cSIMainAccount.SetEnabled(true);
            cSRMainAccount.SetEnabled(true);
            cPIMainAccount.SetEnabled(true);
            cPRMainAccount.SetEnabled(true);


            document.getElementById('Keyval_internalId').value = 'Add';
            //document.getElementById('btnUdf').disabled =true;

            cPopup_Empcitys.SetHeaderText('Add Products');
            document.getElementById('hiddenedit').value = "";
            ctxtPro_Code.SetText('');
            ctxtPro_Name.SetText('');
            ctxtPro_Description.SetText('');
            cCmbProType.SetSelectedIndex(-1);
            //chinmoy edited 19-07-2019
            //cCmbProClassCode.SetSelectedIndex(-1);
            $("#ClassId").val();
            //End
            ctxtGlobalCode.SetText('');
            ctxtTradingLot.SetText('');
            cCmbTradingLotUnits.SetSelectedIndex(-1);
            cCmbQuoteCurrency.SetSelectedIndex(-1);
            ctxtQuoteLot.SetText('');
            cCmbQuoteLotUnit.SetSelectedIndex(-1);
            ctxtDeliveryLot.SetText('');
            cCmbDeliveryLotUnit.SetSelectedIndex(-1);
            cCmbProductColor.SetSelectedIndex(0);
            cCmbProductSize.SetSelectedIndex(0);
            RrdblappColor.SetSelectedIndex(0);
            Rrdblapp.SetSelectedIndex(0);
            gridLookup.Clear();
            //Debjyoti Code Added:30-12-2016
            //Reason: Barcode Type and No
            cCmbBarCodeType.SetSelectedIndex(-1);
            barCodeType = -1;
            BarCode = "";
            GlobalCode = "";
            ctxtBarCodeNo.SetText('');


            //End Debjyoti 30-12-2016
            taxCodeSale = 0;
            taxCodePur = 0;
            taxScheme = 0;
            autoApply = false;

            ProdColor = 0;
            ProdSize = 0;
            ColApp = 0;
            SizeApp = 0;
            //Debjyoti 04-01-2017
            ccmbIsInventory.SetSelectedIndex(0);
            cCmbStockValuation.SetSelectedIndex(1);
            ctxtSalePrice.SetText('');
            ctxtMinSalePrice.SetText('');
            ctxtPurPrice.SetText('');
            ctxtMrp.SetText('');
            ccmbStockUom.SetSelectedIndex(-1);
            ctxtMinLvl.SetText('');
            ctxtReorderLvl.SetText('');
            ctxtReorderQty.SetText('');
            ctxtMaxLvl.SetText('');


            ctxtHeight.SetText('0.00');
            ctxtWidth.SetText('0.00');
            ctxtThickness.SetText('0.00');
            cddlSize.SetSelectedIndex(0);
            $("#SizeUOM").val('1');
            ctxtSeries.SetText('');
            ctxtFinish.SetText('');
            ctxtLeadtime.SetText('0');
            $("#txtCoverage").val('0.00');
            $("#dvCovg").text('');
            $("#txtVolumn").val('0');
            $("#volumeuom").text('');
            ctxtWeight.SetText('0');
            ctxtSubCat.SetText('');
            ctxtPro_Printname.SetText('');

            ccmbNegativeStk.SetSelectedIndex(0);
            cCmbTaxCodeSale.SetSelectedIndex(0);
            cCmbTaxCodePur.SetSelectedIndex(0);
            cCmbTaxScheme.SetSelectedIndex(0);
            cChkAutoApply.SetChecked(false);
            GetCheckBoxValue(false);
            document.getElementById('fileName').value = '';
            cProdImage.SetImageUrl('');
            upload1.ClearText();
            //  gridLookup.SetValue(0);
            cCmbStatus.SetSelectedIndex(0);
            //caspxHsnCode.SetText('');
            //cHsnLookUp.Clear();
            //Debjyoti 31-01-2017
            ctxtPro_Code.SetEnabled(true);
            ccmbIsInventory.SetEnabled(true);
            ccmbIsInventory.SetSelectedIndex(0);
            changeControlStateWithInventory();
            $('#reOrderError').css({ 'display': 'None' });
            $('#mrpError').css({ 'display': 'None' });
            cAspxServiceTax.SetValue('');
            $('#btnPackingConfig').attr('disabled', 'disabled');

            //packing details
            ctxtPackingQty.SetValue(1); //0020190: When creating a new Product, in the "Configure UOM conversion" the Quantity and Alt. Quantity will be "1" by default
            ctxtpacking.SetValue(1); //0020190: When creating a new Product, in the "Configure UOM conversion" the Quantity and Alt. Quantity will be "1" by default

            cchkOverideConvertion.SetChecked(false); //Surojit 08-02-2018
            cchkIsMandatory.SetChecked(false); //Surojit 11-02-2018

            //ccmbPackingUomPro.SetSelectedIndex(-1);
            //packing details End Here
            caspxInstallation.SetValue('0');
            ccmbBrand.SetValue('');
            cmb_tdstcs.SetValue('');
            cPopup_Empcitys.SetWidth(window.screen.width - 50);
            //cPopup_Empcitys.SetHeight(window.innerHeight.height - 70);
            cPopup_Empcitys.Show();
            //cHsnLookUp.SetEnabled(true);
            ctxtPro_Code.Focus();
            //ccmbStatusad.SetSelectedIndex(0);
        }
        function afterFileUpload() {
            if (document.getElementById('hiddenedit').value == '') {
                cgridprod.PerformCallback('savecity~' + GetObjectID('fileName').value);
            }
            else {
                cgridprod.PerformCallback('updatecity~' + GetObjectID('hiddenedit').value + "~" + GetObjectID('fileName').value);
            }

        }


        function SaveActiveDormant() {
            var retval = true;
            var minlvl = (ctxtMinLvl_active.GetValue() != null) ? ctxtMinLvl_active.GetValue() : "0";
            var reordLvl = (ctxtReorderLvl_active.GetValue() != null) ? ctxtReorderLvl_active.GetValue() : "0";

            if ((parseFloat(reordLvl) != 0) || (parseFloat(minlvl) != 0)) {
                if ((parseFloat(reordLvl) <= parseFloat(minlvl)))     //|| (((parseFloat(ctxtMinLvl.GetValue())) == 0) && ((parseFloat(ctxtReorderLvl.GetValue())) == 0)))
                {
                    $('#reOrderError1').css({ 'display': 'block' });
                    retval = false;
                }
            }
            else if ((((parseFloat(ctxtMinLvl.GetValue())) == 0) && ((parseFloat(ctxtReorderLvl.GetValue())) == 0))) {
                $('#reOrderError1').css({ 'display': 'None' });
            }
            else {
                $('#reOrderError1').css({ 'display': 'None' });
            }
            if (retval == false) {
                return false
            }

            else {
                if (document.getElementById('hiddenedit').value != '') {
                    cgridprod.PerformCallback('updatecity_active~' + GetObjectID('hiddenedit').value);
                }
            }


        }

        function btnSave_citys() {
            var PackingUom = ccmbPackingUomPro.GetValue();
            var ConvertionOverideVisible = $('#hdnConvertionOverideVisible').val();
            var ShowUOMConversionInEntry = $('#hdnShowUOMConversionInEntry').val();
            var ServiceItem = ccmbServiceItem.GetValueString();
            if ((((PackingUom == "0" || PackingUom == "" || PackingUom == null)) && (ShowUOMConversionInEntry == "1")) && ccmbIsInventory.GetValue() == 1) {
                jAlert(' "Show UOM Conversion In Entry" is Activated.You must Select alternate UOM from Product Master - Configure UOM Coinversion');
                return false;
            }

            if (ccmbIsInventory.GetValue() == 0) {
                if ((ctxtPro_Code.GetText() != '') && (ctxtPro_Name.GetText().trim() != '')) {
                    if (upload1.GetText().trim() != '') {
                        upload1.Upload();
                    } else {
                        afterFileUpload();
                    }
                }
            } else {
                if ((ctxtPro_Code.GetText() != '') && (ctxtPro_Name.GetText().trim() != '') && (cCmbTradingLotUnits.GetText().trim() != '') && (cCmbDeliveryLotUnit.GetText().trim() != '') && (ccmbStockUom.GetValue() != null)) {

                    if (validReorder() && validMRP()) {
                        if (upload1.GetText().trim() != '') {
                            upload1.Upload();
                        } else {
                            afterFileUpload();
                        }
                    }
                }
            }




        }

        function validReorder() {
            var retval = true;
            var minlvl = (ctxtMinLvl.GetValue() != null) ? ctxtMinLvl.GetValue() : "0";
            var reordLvl = (ctxtReorderLvl.GetValue() != null) ? ctxtReorderLvl.GetValue() : "0";

            if ((parseFloat(reordLvl) != 0) || (parseFloat(minlvl) != 0)) {
                if ((parseFloat(reordLvl) <= parseFloat(minlvl)))     //|| (((parseFloat(ctxtMinLvl.GetValue())) == 0) && ((parseFloat(ctxtReorderLvl.GetValue())) == 0)))
                {
                    $('#reOrderError').css({ 'display': 'block' });
                    retval = false;
                }
            }
            else if ((((parseFloat(ctxtMinLvl.GetValue())) == 0) && ((parseFloat(ctxtReorderLvl.GetValue())) == 0))) {
                $('#reOrderError').css({ 'display': 'None' });
            }
            else {
                $('#reOrderError').css({ 'display': 'None' });
            }
            return retval;
        }

        function validMRP() {
            var retval = true;
            var txtMinSalePrice = (ctxtMinSalePrice.GetValue() != null) ? ctxtMinSalePrice.GetValue() : "0";
            var txtMrp = (ctxtMrp.GetValue() != null) ? ctxtMrp.GetValue() : "0";

            if (parseFloat(txtMrp) != 0 && parseFloat(txtMrp) < parseFloat(txtMinSalePrice)) {
                $('#mrpError').css({ 'display': 'block' });
                retval = false;
            }
            else {
                $('#mrpError').css({ 'display': 'None' });
            }
            return retval;
        }

        function btnSave_citysOld() {

            var valiEmail = false;

            var validPhNo = false;

            var CheckUniqueCode = false;

            var reg = /^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,4})$/;
            if (reg.test(ctxtMarkets_Email.GetText())) {
                valiEmail = true;
            }

            if (!isNaN(ctxtMarkets_Phones.GetText()) && ctxtMarkets_Phones.GetText().length == 10) {
                validPhNo = true;
            }


            //for unique code ajax call
            var MarketsCode = ctxtMarkets_Code.GetText();
            $.ajax({
                type: "POST",
                url: "sMarkets.aspx/CheckUniqueCode",
                data: "{'MarketsCode':'" + MarketsCode + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    //                    CheckUniqueCode = msg.d;

                    if (document.getElementById('hiddenedit').value == '') {
                        CheckUniqueCode = msg.d;
                    }
                    else {
                        CheckUniqueCode == false
                    }

                    if (CheckUniqueCode == false && ctxtMarkets_Code.GetText() != '' && ctxtMarkets_Name.GetText() != '' && (ctxtMarkets_Email.GetText() == '' || valiEmail == true) && (ctxtMarkets_Phones.GetText() == '' || validPhNo == true)) {
                        if (document.getElementById('hiddenedit').value == '') {
                            //alert("in add");
                            cgridprod.PerformCallback('savecity~');
                        }
                        else {
                            //alert("in update");
                            cgridprod.PerformCallback('updatecity~' + GetObjectID('hiddenedit').value);
                        }
                    }
                    else if (CheckUniqueCode == true) {
                        jAlert('Please enter unique market code');
                        ctxtMarkets_Code.Focus();
                    }
                    else if (ctxtMarkets_Code.GetText() == '') {
                        jAlert('Please Enter Markets Code');
                        ctxtMarkets_Code.Focus();
                    }
                    else if (ctxtMarkets_Name.GetText() == '') {
                        jAlert('Please Enter Markets Name');
                        ctxtMarkets_Name.Focus();
                    }
                    else if (!reg.test(ctxtMarkets_Email.GetText())) {
                        jAlert('Please enter valid email');
                        ctxtMarkets_Email.Focus();
                    }
                    else if (isNaN(ctxtMarkets_Phones.GetText()) || ctxtMarkets_Phones.GetText().length != 10) {
                        jAlert('Please enter valid Phone No');
                        ctxtMarkets_Phones.Focus();
                    }

                }

            });
        }


        function fn_btnCancel() {
            cPopup_Empcitys.Hide();
            $("#txtPro_Code_EC, #txtPro_Name_EC, #txtQuoteLot_EC, #txtTradingLot_EC, #txtDeliveryLot_EC").hide();
        }

        function fn_btnCancel_active() {
            cPopup_Empcitys_active.Hide();
        }
        function fn_ViewProduct(keyValue) {
            /*-------------------------------------------------Arindam-----------------------------------------------------------*/

            var url = '/OMS/management/master/View/ViewProduct.html?v=0.07&&id=' + keyValue;

            CAspxDirectCustomerViewPopup.SetWidth(window.screen.width - 50);
            CAspxDirectCustomerViewPopup.SetHeight(window.innerHeight - 70);
            CAspxDirectCustomerViewPopup.SetContentUrl(url);
            CAspxDirectCustomerViewPopup.RefreshContentUrl();
            CAspxDirectCustomerViewPopup.Show();




            /*-------------------------------------------------Arindam-----------------------------------------------------------*/
            //  fn_Editcity(keyValue);
            //  cbtnSave_citys.SetVisible(false);

        }

        function fn_Editcity(keyValue) {
            cbtnSave_Product.SetVisible(true);
            document.getElementById('btnUdf').disabled = false;
            cPopup_Empcitys.SetHeaderText('Modify Products');

            ctxtHeight.SetText('0.00');
            ctxtWidth.SetText('0.00');
            ctxtThickness.SetText('0.00');
            cddlSize.SetSelectedIndex(0);
            $("#SizeUOM").val('1');
            ctxtSeries.SetText('');
            ctxtFinish.SetText('');
            ctxtLeadtime.SetText('0');
            $("#txtCoverage").val('');
            $("#dvCovg").text('');
            $("#txtVolumn").val('0');
            $("#volumeuom").text('');
            ctxtWeight.SetText('0');
            ctxtSubCat.SetText('');
            ctxtPro_Printname.SetText('');


            cgridprod.PerformCallback('Edit~' + keyValue);
            document.getElementById('Keyval_internalId').value = 'ProductMaster' + keyValue;
        }

        function fn_activeEdit(keyValue, status) {
            //if (status == 'A') {
            //    jAlert('Product is already active');
            //    return;
            //}
            // else {
            document.getElementById('HiddenField_status').value = '1';
            // cPopup_Empcitys_active.SetWidth(window.screen.width - 50);

            //cPopup_Empcitys_active.Show();

            cbtnSave_Product.SetVisible(true);
            document.getElementById('btnUdf').disabled = false;
            cPopup_Empcitys_active.SetHeaderText('Modify Products');
            cgridprod.PerformCallback('Active~' + keyValue);
            document.getElementById('Keyval_internalId').value = 'ProductMaster' + keyValue;

            // }


        }

        function fn_Deletecity(keyValue) {
            //if (confirm("Confirm Delete?")) {
            //    cgridprod.PerformCallback('Delete~' + keyValue);
            //}
            jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    cgridprod.PerformCallback('Delete~' + keyValue);
                }
            });
        }


        function componentEndCallBack(s, e) {
            console.log(e);
            // cPopup_Empcitys.Show();
        }

        var mainAccountInUse = [];
        function grid_EndCallBack() {

            if (cgridprod.cpinsert != null) {
                if (cgridprod.cpinsert == 'Success') {
                    jAlert('Saved Successfully');
                    //alert('Saved Successfully');
                    //................CODE  UPDATED BY sAM ON 18102016.................................................
                    ctxtPro_Name.GetInputElement().readOnly = false;
                    //................CODE ABOVE UPDATED BY sAM ON 18102016.................................................
                    cPopup_Empcitys.Hide();

                }
                else if (cgridprod.cpinsert == 'fail') {
                    jAlert("Error On Insertion \n 'Please Try Again!!'")
                }
                else if (cgridprod.cpinsert == 'UDFManddratory') {
                    jAlert("UDF is set as Mandatory. Please enter values.", "Alert", function () { OpenUdf(); });

                }
                else {
                    jAlert(cgridprod.cpinsert);
                    //cPopup_Empcitys.Hide();
                }
            }

        }
        function OnCmbCountryName_ValueChange() {
            cCmbState.PerformCallback("BindState~" + cCmbCountryName.GetValue());
        }
        function CmbState_EndCallback() {
            cCmbState.SetSelectedIndex(0);
            cCmbState.Focus();
        }
        function OnCmbStateName_ValueChange() {
            cCmbCity.PerformCallback("BindCity~" + cCmbState.GetValue());
        }
        function CmbCity_EndCallback() {
            cCmbCity.SetSelectedIndex(0);
            cCmbCity.Focus();
        }
        $(document).ready(function () {
            $('.dxpc-closeBtn').click(function () {
                fn_btnCancel();
            });
        });
    </script>

    <script type="text/javascript">
        function fn_ctxtPro_Name_TextChanged(s, e) {
            var procode = 0;
            if (GetObjectID('hiddenedit').value != '') {
                procode = GetObjectID('hiddenedit').value;
            }

            //var ProductName = ctxtPro_Name.GetText();
            var ProductName = ctxtPro_Code.GetText().trim();
            $.ajax({
                type: "POST",
                url: "purchaseinvoice.aspx/CheckUniqueNameProduct",
                //data: "{'ProductName':'" + ProductName + "'}",
                data: JSON.stringify({ ProductName: ProductName, procode: procode }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var data = msg.d;

                    if (data == true) {
                        jAlert("Please enter unique name", "Alert", function () { ctxtPro_Code.SetFocus(); });
                        ctxtPro_Code.SetText("");
                        // ctxtPro_Code.SetFocus();
                        //document.getElementById("Popup_Empcitys_ctxtPro_Code_I").focus();
                        //  document.getElementById("txtPro_Code_I").focus();

                        return false;
                    }
                }

            });
        }
    </script>

    <style type="text/css">
        #Popup_Empcitys_active_DXPWMB-1 {
            display: none;
        }

        .cityDiv {
            height: 25px;
        }

        .cityTextbox {
            height: 25px;
            width: 50px;
        }

        .Top {
            height: 90px;
            width: 100%;
            padding-top: 5px;
            valign: top;
        }

        .Footer {
            height: 30px;
            width: 400px;
            padding-top: 10px;
        }

        .ScrollDiv {
            height: 250px;
            width: 400px;
            overflow-x: hidden;
            overflow-y: scroll;
        }

        .ContentDiv {
            width: 100%;
            height: 300px;
            border: 2px;
        }



        .TitleArea {
            height: 20px;
            padding-left: 10px;
            padding-right: 3px;
            background-image: url( '../images/EHeaderBack.gif' );
            background-repeat: repeat-x;
            background-position: bottom;
            text-align: center;
        }

        .FilterSide {
            float: left;
            width: 50%;
        }

        .SearchArea {
            width: 100%;
            height: 30px;
            padding-top: 5px;
        }

        .newLbl {
            margin: 5px 0 !important;
            display: block;
        }

        .sText {
            font-size: 10px;
        }

        #grid_DXMainTable > tbody > tr > td:nth-child(20) {
            display: none !important;
        }
    </style>


    <script type="text/javascript">
        function OnAddBusinessClick(keyValue) {
            var url = '../../master/AssignIndustry.aspx?id1=' + keyValue + '&EntType=product';
            window.location.href = url;
        }
        function PopupOpen(obj) {
            var URL = '/OMS/Management/Master/Product_Document.aspx?idbldng=' + obj + '&type=Product';
            //OnMoreInfoClick(URL, "Products Document Details", '1000px', '400px', "Y");
            //document.getElementById("marketscgridprod_DXPEForm_efnew_DXEditor1_I").focus();
            window.location.href = URL;
        }



        var KEYCODE_ENTER = 13;
        var KEYCODE_ESC = 27;



        $(document).keyup(function (e) {
            if (e.keyCode == KEYCODE_ESC) {
                // cPopup_Empcitys.Hide();
                //$('#cPopup_Empcitys').hide();
            }
        });

        function PopupOpentoProductUpload(obj, prod_name) {

            ///  alert(prod_name);
            var URL = '/OMS/Management/Master/Product-Multipleimage.aspx?prodid=' + obj + '&name=' + prod_name;
            window.location.href = URL;

        }

    </script>


    <script type="text/javascript">

        var counter = 0;


        function fetchLebel() {

            $("#generatedForm").html("");
            counter = 0;


            $(".newLbl").each(function () {

                var newField = "<div style='width:500px; margin-left:5px; float:left; margin-bottom:5px;'><label id='LblKey" + counter + "' style='width:110px; float:left;'>" + $(this).text() + "</label>";
                newField += "<input type='text' id='TxtKey" + counter + "' value='" + $(this).text() + "' style='margin-left:41px; width:250px;' />";

                //alert($(this).attr("id").split('_')[4]);

                if (String($(this).attr("id").split('_')[2]) != "undefined") {
                    newField += "<input type='text' id='HddnKey" + counter + "' value='" + $(this).attr("id").split('_')[2] + "' style='display:none; margin-left:41px; width:250px;' />";
                }
                else {
                    //alert($(this).attr("id"));
                    newField += "<input type='text' id='HddnKey" + counter + "' value='" + $(this).attr("id") + "' style='display:none; margin-left:41px; width:250px;' />";
                }
                newField += "</div>";

                $("#generatedForm").append(newField);

                counter++;

            });

            AssignValuePopup.Show();

        }


        function tdsOkClick() {
            tdsValue = cmb_tdstcs.GetValue();
            ctdsPopup.Hide();
        }

        function SaveDataToResource() {

            var key = "";
            var value = "";

            for (var i = 0; i < counter; i++) {

                if (key == "") {

                    key = $("#HddnKey" + i).val();
                    value = $("#TxtKey" + i).val();

                }
                else {

                    key += "," + $("#HddnKey" + i).val();
                    value += "," + $("#TxtKey" + i).val();

                }

            }

            $("#AssignValuePopup_KeyField").val(key);
            $("#AssignValuePopup_ValueField").val(value);
            $("#AssignValuePopup_RexPageName").val("ProductValues");


            return true;

        }


    </script>
    <style>
        .imageArea {
            width: 150px;
            height: 100px !important;
            overflow: hidden;
        }

        .popUpHeader {
            float: right;
        }

        .blll {
            margin: 0;
            padding: 0 !important;
            margin-top: 6px;
        }

        .dxeErrorCellSys.dxeNoBorderLeft {
            position: absolute;
        }

        .mkSht {
            width: 100%;
        }

            .mkSht > tbody > tr > td {
                padding: 2px 5px;
            }

        .multiply {
            padding-top: 18px !important;
            font-size: 14px;
            font-weight: 600;
            color: #b11212;
        }
    </style>

    <%--End--%>
    <%--  Chinmoy js end--%>
    <%--Vehicle Script--%>
    <script>
        $(document).ready(function () {
            $('#ddl_numberingScheme').focus();
        });
        var cpSelectedKeys = [];
        function VehicleSelectionChanged(s, e) {
            if (e.isChangedOnServer) return;
            globalindexcheck = e.visibleIndex;
            var key = s.GetRowKey(e.visibleIndex);
            if (e.isSelected) {
                cpSelectedKeys.push(key);
            }
            else {
                cpSelectedKeys = RemoveElementFromArray(cpSelectedKeys, key);

            }
            appcode = cpSelectedKeys;

        }

        function RemoveElementFromArray(array, element) {
            var index = array.indexOf(element);
            if (index < 0) return array;
            array[index] = null;
            var result = [];
            for (var i = 0; i < array.length; i++) {
                if (array[i] === null)
                    continue;
                result.push(array[i]);
            }
            return result;
        }

        function LoadOldSelectedKeyvalue() {
            var x = gridvehicleLookup.gridView.GetSelectedKeysOnPage();
            var Ids = "";
            for (var i = 0; i < x.length; i++) {
                Ids = Ids + ',' + x[i];
            }
            document.getElementById('OldSelectedKeyvalue').value = Ids;
        }

        function BeginComponentCallback() {
        }


    </script>



    <script>



        var ColumnIndex = 0;
        $(document).ready(function () {
            var ShowOldUnitInPOS = $('#hdnShowOldUnitInPOS').val();
            if (ShowOldUnitInPOS == 0) {
                document.getElementById('unitValueID').style.display = 'none';
                document.getElementById('UnitValueCombo').style.display = 'none';
                document.getElementById('oldunitButton').style.display = 'none';
                document.getElementById('unitvaluelbl').style.display = 'none';
                document.getElementById('unitValueText').style.display = 'none';
                $('.clsbnrLblLessOldVal').hide();


            }
            else {
                document.getElementById('unitValueID').style.display = 'table-cell';
                document.getElementById('UnitValueCombo').style.display = 'table-cell';
                document.getElementById('oldunitButton').style.display = 'table-cell';
                document.getElementById('unitvaluelbl').style.display = 'table-cell';
                document.getElementById('unitValueText').style.display = 'table-cell';
                document.getElementsByClassName('clsbnrLblLessOldVal').style.display = 'table-cell';
                $('.clsbnrLblLessOldVal').show();
                //document.getElementById('txtRemarks').style.width="500px";
            }
            document.getElementById('txtRemarks').style.width = "100%";






            $('#idOutstanding').on("click", function () {

                $("#<%=drdExport.ClientID%>").val('0');
                cOutstandingPopup.Show();
                var CustomerId = $("#<%=hdnCustomerId.ClientID%>").val();
                var BranchId = $("#<%=ddl_Branch.ClientID%>").val();
                $("#<%=hddnBranchId.ClientID%>").val(BranchId);
                var AsOnDate = tstartdate.GetDate().format('yyyy-MM-dd');
                $("#<%=hddnAsOnDate.ClientID%>").val(AsOnDate);
                $("#<%=hddnOutStandingBlock.ClientID%>").val('1');
                //Clear Row
                var rw = $("[id$='CustomerOutstanding_DXMainTable']").find("tr")
                for (var RowClount = 0; RowClount < rw.length; RowClount++) {
                    rw[RowClount].remove();
                }



                //cCustomerOutstanding.Refresh();

                //cCustomerOutstanding.PerformCallback('BindOutStanding~' + CustomerId + '~' + BranchId + '~' + AsOnDate);
                var CheckUniqueCode = false;
                $.ajax({
                    type: "POST",
                    url: "SalesOrderAdd.aspx/GetCustomerOutStanding",
                    data: JSON.stringify({ strAsOnDate: AsOnDate, strCustomerId: CustomerId, BranchId: BranchId }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    //async:false,
                    success: function (msg) {

                        CheckUniqueCode = msg.d;
                        if (CheckUniqueCode == true) {
                            cCustomerOutstanding.Refresh();

                        }
                    }
                });


                //cCustomerOutstanding.Refresh();
                //cOutstandingPopup.Show();

            });




        });

        //








        var globalOldUnitTotalValue = 0;
        var globalNetAmount = 0;
        var CustomerCurrentDateAmount = 0;
        var isExecutiveHasLedger = 0;
        var financerDetails;
        var executiveList;
        var Pre_TotalAmt = 0;
        var Pre_Qty = 0;
        var Pre_Price = 0;
        var Pre_Discount = 0;

        var taxSchemeUpdatedDate = '<%=Convert.ToString(Cache["SchemeMaxDate"])%>';


        function SetDataSourceOnComboBox(ControlObject, Source) {
            ControlObject.ClearItems();
            for (var count = 0; count < Source.length; count++) {
                ControlObject.AddItem(Source[count].Name, Source[count].Id);
            }
            ControlObject.SetSelectedIndex(0);
        }

        function GetAllDetailsByBranch() {


            var OtherDetails = {}
            OtherDetails.BranchId = $('#ddl_Branch').val();
            OtherDetails.EntryType = $('#HdPosType').val();
            $.ajax({
                type: "POST",
                url: "POSSalesInvoice.aspx/GetAllDetailsByBranch",
                data: JSON.stringify(OtherDetails),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var returnObject = msg.d;

                    if (returnObject.SalesMan) {
                        SetDataSourceOnComboBox(cddl_SalesAgent, returnObject.SalesMan);
                    }
                    if (returnObject.ChallanNumberScheme) {
                        SetDataSourceOnComboBox(cchallanNoScheme, returnObject.ChallanNumberScheme);
                    }

                    if (returnObject.Financer) {
                        SetDataSourceOnComboBox(ccmbFinancer, returnObject.Financer);
                        ccmbFinancer.SetSelectedIndex(-1);
                        financerDetails = returnObject.Financer;

                    }

                    if (returnObject.Executive) {
                        executiveList = returnObject.Executive;
                    }
                    financerIndexChange();
                    ccmbExecName.SetValue('');
                }
            });

        }

        function UOMGotFocus(s, e) {

        }


        function InvoiceExists() {
            jAlert("Selected Invoice Number Already Exists.", 'Alert', function () { ctxt_PLQuoteNo.SetEnabled(true); });

        }

        function AvailableStockClick() {
            if ($('#HDSelectedProduct').val() == "") {
                jAlert("Please select a Product First.");
            } else {
                cShowAvailableStock.Show();
                cAvailableStockgrid.PerformCallback();
            }
        }




        function SetTotalDownPaymentAmount() {
            var totDownPay = parseFloat(ctxtEmiOtherCharges.GetValue()) + parseFloat(ctxtprocFee.GetValue()) + parseFloat(ctxtdownPayment.GetValue());
            ctxtTotDpAmt.SetValue(totDownPay);

            var InvoiceValue = parseFloat(cbnrLblInvValue.GetValue());
            ctxtFinanceAmt.SetValue(InvoiceValue - totDownPay);

        }

        function SetDownPayment() {
            var InvoiceValue = parseFloat(cbnrLblInvValue.GetValue());
            var FinanceAmount = parseFloat(ctxtFinanceAmt.GetValue());

            ctxtdownPayment.SetValue(InvoiceValue - FinanceAmount);
        }

        function SetOtherChargesLbl() {
            var finalOtherCharges = parseFloat(Math.round(ctxtQuoteTaxTotalAmt.GetValue() * 100) / 100).toFixed(2);
            if (finalOtherCharges == 0) {
                $('#otherChargesId').hide();
            } else {
                $('#otherChargesId').show();
            }
            cbnrOtherChargesvalue.SetValue(finalOtherCharges);
            SetRunningBalance();
        }


        function ccmbExecNameEndCallBack() {
            if (ccmbExecName.cpFinancerHasLedger) {
                if (ccmbExecName.cpFinancerHasLedger != '') {
                    ccmbExecName.ShowDropDown();
                    isExecutiveHasLedger = parseFloat(ccmbExecName.cpFinancerHasLedger);
                    ccmbExecName.cpFinancerHasLedger = null;
                    if (isExecutiveHasLedger == 0) {
                        jAlert("No ledger is mapped for the selected Financer.", "Alert", function () {
                            ccmbFinancer.Focus();
                        });
                    }
                }
            }
        }

        function Updated() {
            jAlert('Updated Successfully.', 'Alert', function () {
                window.location.assign("PosSalesInvoiceList.aspx");
            });
        }

        function ParentCustomerOnClose(newCustId, CustomerName, CustUniqueName, BillingStateText, BillingStateCode, ShippingStateText, ShippingStateCode) {
            AspxDirectAddCustPopup.Hide();
            if (newCustId.trim() != '') {
                page.SetActiveTabIndex(0);
                GetObjectID('hdnCustomerId').value = newCustId;

                GetObjectID('lblBillingStateText').value = BillingStateText;
                GetObjectID('lblBillingStateValue').value = BillingStateCode;

                GetObjectID('lblShippingStateText').value = ShippingStateText;
                GetObjectID('lblShippingStateValue').value = ShippingStateCode;

                var FullName = new Array(CustUniqueName, CustomerName);
                ctxtCustName.SetText(CustomerName);
                $('#DeleteCustomer').val("yes");
                page.GetTabByName('[B]illing/Shipping').SetEnabled(true);
                loadAddressbyCustomerID(newCustId);
                cddl_SalesAgent.Focus();

            }
        }






        function SetInvoiceLebelValue() {

            var invValue = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) - (parseFloat(cbnrLblLessAdvanceValue.GetValue()) + parseFloat(cbnrLblLessOldMainVal.GetValue())) + parseFloat(cbnrOtherChargesvalue.GetValue());

            if (document.getElementById('HdPosType').value == 'Crd') {
                if (invValue < 0) {
                    var newAdvAmount = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) - parseFloat(cbnrLblLessOldMainVal.GetValue());
                    cbnrLblLessAdvanceValue.SetValue(parseFloat(Math.round(Math.abs(newAdvAmount) * 100) / 100).toFixed(2));
                }
            }

            if (document.getElementById('HdPosType').value == 'Fin') {
                if (invValue < 0) {
                    var newAdvAmountfin = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) - parseFloat(cbnrLblLessOldMainVal.GetValue());
                    cbnrLblLessAdvanceValue.SetValue(parseFloat(Math.round(Math.abs(ctxtdownPayment.GetValue()) * 100) / 100).toFixed(2));
                }
            }



            if (document.getElementById('HdPosType').value == 'Crd')
                invValue = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) - (parseFloat(cbnrLblLessAdvanceValue.GetValue()) + parseFloat(cbnrLblLessOldMainVal.GetValue())) + parseFloat(cbnrOtherChargesvalue.GetValue());
            else if (document.getElementById('HdPosType').value == 'Fin')
                invValue = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) - parseFloat(cbnrLblLessOldMainVal.GetValue()) + parseFloat(cbnrOtherChargesvalue.GetValue());


            cbnrLblInvValue.SetValue(parseFloat(Math.round(Math.abs(invValue) * 100) / 100).toFixed(2));


            SetRunningBalance();

        }

        function DiscountGotChange() {
            globalNetAmount = parseFloat(grid.GetEditor("TotalAmount").GetValue());
            ProductGetTotalAmount = globalNetAmount;

            var _Amount = (grid.GetEditor('Amount').GetText() != null) ? grid.GetEditor('Amount').GetText() : "0";
            var _Quantity = (grid.GetEditor('Quantity').GetText() != null) ? grid.GetEditor('Quantity').GetText() : "0";
            var _Pre_Price = (grid.GetEditor('SalePrice').GetText() != null) ? grid.GetEditor('SalePrice').GetText() : "0";
            var _Discount = (grid.GetEditor('Discount').GetText() != null) ? grid.GetEditor('Discount').GetText() : "0";

            Pre_TotalAmt = _Amount;
            Pre_Qty = _Quantity;
            Pre_Price = _Pre_Price;
            Pre_Discount = _Discount;

        }

        function challanNoSchemeSelectedIndexChanged() {
            var schemeValue = cchallanNoScheme.GetValue();
            if (schemeValue == null) {
                ctxtChallanNo.SetEnabled(false);
                ctxtChallanNo.SetText('');
            }
            else if (schemeValue.split('~')[1] == '1') {
                ctxtChallanNo.SetEnabled(false);
                ctxtChallanNo.SetText('Auto');
            }
            else if (schemeValue.split('~')[1] == '0') {
                ctxtChallanNo.SetEnabled(true);
                ctxtChallanNo.SetText('');
            }
        }

        function challanNoSchemeEndCallback() {
            if (lastChallan) {
                cchallanNoScheme.PerformCallback(lastChallan);
                lastChallan = null;
            }
        }

        function CustomerReceiptEndCallback() {
            if (caspxCustomerReceiptGridview.cpCustomerReceiptTotalAmount) {
                if (caspxCustomerReceiptGridview.cpCustomerReceiptTotalAmount != '') {
                    ctxtAdvnceReceipt.SetValue(caspxCustomerReceiptGridview.cpCustomerReceiptTotalAmount);
                    cbnrLblLessAdvanceValue.SetValue(caspxCustomerReceiptGridview.cpCustomerReceiptTotalAmount);
                    SetInvoiceLebelValue();
                    caspxCustomerReceiptGridview.cpCustomerReceiptTotalAmount = null;

                    //if (caspxCustomerReceiptGridview.cpReceiptList) {
                    //    $('#hdAddvanceReceiptNo').val(caspxCustomerReceiptGridview.cpReceiptList);
                    //    caspxCustomerReceiptGridview.cpReceiptList = null;
                    //}
                }
            }

            if (caspxCustomerReceiptGridview.cpReceiptList != null) {
                $('#hdAddvanceReceiptNo').val(caspxCustomerReceiptGridview.cpReceiptList);
                caspxCustomerReceiptGridview.cpReceiptList = null;
            }

            if (caspxCustomerReceiptGridview.cpTotalTransectionAmount) {
                if (caspxCustomerReceiptGridview.cpTotalTransectionAmount != "") {
                    CustomerCurrentDateAmount = parseFloat(caspxCustomerReceiptGridview.cpTotalTransectionAmount);
                    caspxCustomerReceiptGridview.cpTotalTransectionAmount = null;
                }
            }

        }
        function CustomerReceiptSaveandExitClick() {
            cpopupCustomerRecipt.Hide();
            caspxCustomerReceiptGridview.PerformCallback('SaveCustomerReceiptGridview');
            //    if (document.getElementById('HdPosType').value != 'Crd'  ) {
            ccmbUcpaymentCashLedger.Focus();
            //} else {
            //    cbtn_SaveRecords.Focus();
            //}

        }

        function SelectAllCustomerReceipt() {
            caspxCustomerReceiptGridview.PerformCallback('SelectAllRecords');
        }

        function UnSelectAllCustomerReceipt() {
            caspxCustomerReceiptGridview.PerformCallback('UnSelectAllRecords');
        }

        function RevertCustomerReceipt() {
            caspxCustomerReceiptGridview.PerformCallback('Revert');
        }

        function AdvanceReceiptOnClick() {
            //var custId = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());
            var custId = GetObjectID('hdnCustomerId').value;
            if (document.getElementById('hdAddOrEdit').value == "Add") {
                caspxCustomerReceiptGridview.PerformCallback('BindCustomerGridByInternalId~' + custId + '~' + tstartdate.GetDate().format('yyyy-MM-dd'));
            }
            cpopupCustomerRecipt.Show();
        }

        function onMainGridKeyPress(e) {
            console.log("key pressed", e.code);
            if (e.code == "Tab") {
                ccmbOldUnit.Focus();
            }
        }

        function oldUnitGridClearClick() {
            ClearOldUnitData();
            coldUnitProductLookUp.Focus();
            $('#HdDiscountAmount').val('0');

        }
        function oldunitPopupSaveAndEXitClick() {
            cOldUnitPopUpControl.Hide();
            ctxtRemarks.Focus();
        }
        function oldUnitButtonShouldVissible() {
            if (ccmbOldUnit.GetValue() == "0")
                $('#OldUnitSelectionButton').hide();
            else
                $('#OldUnitSelectionButton').show();
        }

        function ccmbOldUnitTextChanged() {
            oldUnitButtonShouldVissible();
            if (ccmbOldUnit.GetValue() == "1") {
                OldUnitButtonOnClick();
                coldUnitProductLookUp.Focus();
            } else {
                if (parseFloat(ctxtunitValue.GetValue()) > 0) {
                    jConfirm("Old Unit already entered. Selecting 'No' will clear Old Unit data. Wish to proceed?", "Alert", function (data) {
                        if (data == true) {
                            cOldUnitGrid.PerformCallback('DeleteAllRecord');
                        } else {
                            ccmbOldUnit.SetValue('1');
                            $('#OldUnitSelectionButton').show();
                        }
                    });
                }
            }
        }
        function oldUnitGridRowChange() {
            if (cOldUnitGrid.GetVisibleRowsOnPage() > 0) {
                if (document.getElementById('hdAddOrEdit').value != "Edit") {
                    coldunitPopupSaveAndClickClick.SetVisible(true);
                }
            } else {
                coldunitPopupSaveAndClickClick.SetVisible(false);
            }
        }

        function OldUnitGridEndCallback() {
            if (cOldUnitGrid.cpReturnString) {
                if (cOldUnitGrid.cpReturnString != "") {
                    if (cOldUnitGrid.cpReturnString == 'AddDataToTable') {
                        ClearOldUnitData();
                        coldUnitProductLookUp.Focus();
                        cOldUnitGrid.cpReturnString = null;
                    }
                }
            }

            if (cOldUnitGrid.cpTotalOldUnit) {
                if (cOldUnitGrid.cpTotalOldUnit != "") {
                    ctxtunitValue.SetValue(parseFloat(cOldUnitGrid.cpTotalOldUnit));
                    cbnrLblLessOldMainVal.SetText(ctxtunitValue.GetText());
                    SetInvoiceLebelValue();
                    if (parseFloat(ctxtunitValue.GetValue()) == 0) {
                        ccmbOldUnit.SetValue('0');
                        $('#OldUnitSelectionButton').hide();
                    } else {
                        ccmbOldUnit.SetValue('1');
                        $('#OldUnitSelectionButton').show();
                    }
                }
            }
            oldUnitGridRowChange();
        }

        function ClearOldUnitData() {
            coldUnitProductLookUp.Clear();
            ctxtOldUnitUom.SetText('');
            ctxtOldUnitqty.SetText('');
            ctxtoldUnitValue.SetText('');
        }

        function OldUnitButtonOnClick() {
            cOldUnitPopUpControl.Show();
            coldUnitProductLookUp.Focus();
            cOldUnitGrid.PerformCallback('DisplayOldUnit');

        }

        function oldUnitProductTextChanged(s, e) {
            var key = coldUnitProductLookUp.GetGridView().GetRowKey(coldUnitProductLookUp.GetGridView().GetFocusedRowIndex());
            ctxtOldUnitUom.SetText(key.split('|@|')[1]);
        }

        function fn_EditOldUnit(keyVal) {
            coldUnitUpdatePanel.PerformCallback(keyVal);
        }

        function fn_removeOldUnit(keyVal) {
            cOldUnitGrid.PerformCallback("DeleteFromTable~" + keyVal);
        }

        function oldUnitGridAddClick() {
            $('#mandetoryOldUnit').attr('style', 'display:none');
            var focusedRow = coldUnitProductLookUp.gridView.GetFocusedRowIndex();

            var MRP = parseFloat(coldUnitProductLookUp.gridView.GetRow(focusedRow).children[5].innerText);

            if (coldUnitProductLookUp.GetValue() == null) {

                $('#mandetoryOldUnit').attr('style', 'display:block');
            }
            else if (MRP != 0 && ctxtoldUnitValue.GetValue() > MRP) {
                var roundOfValue = parseFloat(Math.round(Math.abs(MRP) * 100) / 100).toFixed(2);
                jAlert("Old Unit Value cannot be Greater then MRP defined.", "Alert", function () { ctxtoldUnitValue.Focus(); });
            }
            else {
                cOldUnitGrid.PerformCallback("AddDataToTable");
            }
        }

        function OnfinancerEndCallback(s, e) {
            if (lastFinancer) {
                ccmbFinancer.PerformCallback(lastFinancer);
                lastFinancer = null;
            }
        }

        function OnSalesAgentEndCallback(s, e) {
            if (lastSalesman) {
                cddl_SalesAgent.PerformCallback(lastSalesman);
                lastSalesman = null;
            } else {
                cmbUcpaymentCashLedgerChanged(ccmbUcpaymentCashLedger);
            }
        }


        function financerIndexChange(s, e) {
            var financer = ccmbFinancer.GetValue();
            if (financer == "") {
                return;
            }

            var financerMainAccount = $.grep(financerDetails, function (element) { return element.Id == financer; })
            if (financerMainAccount.length > 0) {
                isExecutiveHasLedger = 1;
            }

            var executiveListBranch = $.grep(executiveList, function (element) { return element.otherDetails == financer; })
            SetDataSourceOnComboBox(ccmbExecName, executiveListBranch);
            //ccmbExecName.PerformCallback(ccmbFinancer.GetValue());
        }

        function isDeliveryTypeChanged(s, e) {
            var type = ccmbDeliveryType.GetValue();
            document.getElementById('ddDeliveredFrom').value = $('#sessionBranch').val();
            if (type == 'S') {
                $('#ddDeliveredFrom').attr('disabled', 'disabled');
            }
            else {
                $('#ddDeliveredFrom').attr('disabled', false);
            }

            if (type == "D") {

                cchallanNoScheme.SetEnabled(true);
                tstartdate.SetEnabled(true);

            } else {
                cchallanNoScheme.SetSelectedIndex(0);
                cchallanNoScheme.SetEnabled(false);
                ctxtChallanNo.SetEnabled(false);
                if ($("#ISAllowBackdatedEntry").val() == "No") {
                    tstartdate.SetEnabled(false);
                }
                //  tstartdate.SetDate(new Date);
                cdeliveryDate.SetDate(tstartdate.GetDate());
                DateCheck();

                if (isDeliveryTypeChanged != "") {
                    if ($('#ddl_numberingScheme').val().split('~')[1] == "0") {
                        tstartdate.SetEnabled(true);
                    }
                }

            }

        }


        (function (global) {

            if (typeof (global) === "undefined") {
                throw new Error("window is undefined");
            }

            var _hash = "!";
            var noBackPlease = function () {
                global.location.href += "#";

                // making sure we have the fruit available for juice (^__^)
                global.setTimeout(function () {
                    global.location.href += "!";
                }, 50);
            };

            global.onhashchange = function () {
                if (global.location.hash !== _hash) {
                    global.location.hash = _hash;
                }
            };

            global.onload = function () {
                noBackPlease();

                // disables backspace on page except on input fields and textarea..
                document.body.onkeydown = function (e) {
                    var elm = e.target.nodeName.toLowerCase();
                    if (e.which === 8 && (elm !== 'input' && elm !== 'textarea')) {
                        e.preventDefault();
                    }
                    // stopping event bubbling up the DOM tree..
                    e.stopPropagation();
                };
            }

        })(window);

        var isCtrl = false;
        //document.onkeyup = function (e) {
        //    if (event.keyCode == 17) {
        //        isCtrl = false;
        //    }
        //    else if (event.keyCode == 27) {
        //        btnCancel_Click();
        //    }
        //}

        document.onkeydown = function (e) {
            if (event.keyCode == 18) isCtrl = true;
            if (event.keyCode == 78 && event.altKey == true) { //run code for Alt + n -- ie, Save & New  
                StopDefaultAction(e);
                Save_ButtonClick();
            }
            else if (event.keyCode == 88 && event.altKey == true) { //run code for Ctrl+X -- ie, Save & Exit!     
                StopDefaultAction(e);
                if (document.getElementById('hdAddOrEdit').value != "Add") {
                    cbtn_SaveRecordsEdit.DoClick();
                } else {
                    SaveExit_ButtonClick();
                }
            }
            else if (event.keyCode == 85 && event.altKey == true) { //run code for Ctrl+X -- ie, Save & Exit!     
                StopDefaultAction(e);
                OpenUdf();
            }
                //Rev Subhra 25-07-2019
            else if (event.keyCode == 75 && event.altKey == true) { //run code for Alt+K -- ie, Press OK on Billing & Shipping!   
                if (page.activeTabIndex == 1) {
                    btnSave_QuoteAddress();
                }
            }
            else if (event.keyCode == 84 && event.altKey == true) { //run code for Alt+T -- ie, Press OK on Billing & Shipping!     
                if (page.activeTabIndex == 1) {
                    BillingCheckChange();
                }
            }
            else if (event.keyCode == 86 && event.altKey == true) { //run code for Alt+V -- ie, Set Focus on Vehicle! 
                if (page.activeTabIndex == 0) {
                    ctxtVehicles.SetFocus();
                }
            }
            //End of Rev Subhra 25-07-2019
        }

        function StopDefaultAction(e) {
            if (e.preventDefault) { e.preventDefault() }
            else { e.stop() };

            e.returnValue = false;
            e.stopPropagation();
        }
    </script>

    <%--Debu Section--%>
    <script type="text/javascript">
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
            //var roundedOfAmount = Math.round(totalInlineTaxAmount);
            var roundedOfAmount = totalInlineTaxAmount;
            ctxtTaxTotAmt.SetValue(totalInlineTaxAmount);


            var diffDisc = roundedOfAmount - totalInlineTaxAmount;
            if (diffDisc > 0)
                document.getElementById('taxroundedOf').innerText = 'Adjustment ' + Math.abs(diffDisc.toFixed(3));
            else if (diffDisc < 0)
                document.getElementById('taxroundedOf').innerText = 'Adjustment ' + Math.abs(diffDisc.toFixed(3));
            else
                document.getElementById('taxroundedOf').innerText = '';
        }

        function ShowTaxPopUp(type) {
            if (type == "IY") {
                $('#ContentErrorMsg').hide();
                $('#content-6').show();


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

        function gridFocusedRowChanged(s, e) {
            globalRowIndex = e.visibleIndex;

            //var ProductIDColumn = s.GetColumnByField("ProductID");
            //if (!e.rowValues.hasOwnProperty(ProductIDColumn.index))
            //    return;
            //var cellInfo = e.rowValues[ProductIDColumn.index];

            //if (cCmbProduct.FindItemByValue(cellInfo.value) != null) {
            //    cCmbProduct.SetValue(cellInfo.value);
            //}
            //else {
            //    cCmbProduct.SetSelectedIndex(-1);
            //}

        }

        function OnBatchEditEndEditing(s, e) {
            var ProductIDColumn = s.GetColumnByField("ProductID");
            if (!e.rowValues.hasOwnProperty(ProductIDColumn.index))
                return;
            var cellInfo = e.rowValues[ProductIDColumn.index];
            if (cCmbProduct.GetSelectedIndex() > -1 || cellInfo.text != cCmbProduct.GetText()) {
                cellInfo.value = cCmbProduct.GetValue();
                cellInfo.text = cCmbProduct.GetText();
                cCmbProduct.SetValue(null);
            }
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
                ctxtTaxTotAmt.SetValue(Math.round(totAmt + finalTaxAmt - taxAmountGlobal));
            } else {
                ctxtTaxTotAmt.SetValue(Math.round(totAmt + (finalTaxAmt * -1) - (taxAmountGlobal * -1)));
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
            ctxtTaxTotAmt.SetValue(Math.round(totAmt + calculatedValue - GlobalCurTaxAmt));

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
                    ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) - GlobalCurTaxAmt));
                }


                var isValid = taxValue.indexOf('~');
                if (isValid != -1) {
                    var rate = parseFloat(taxValue.split('~')[1]);
                    var ProdAmt = parseFloat(ctxtprodBasicAmt.GetValue());

                    GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());


                    cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * rate) / 100);
                    ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * rate) / 100) - GlobalCurTaxAmt));
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

                    ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt));
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

                        ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt));
                        GlobalCurTaxAmt = 0;
                    }
                    else {

                        GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                        cgridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * s.GetText()) / 100) * -1);

                        ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) * -1) - (GlobalCurTaxAmt * -1)));
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

                        ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt));
                        GlobalCurTaxAmt = 0;
                    }
                    else {

                        GlobalCurTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
                        gridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * s.GetText()) / 100) * -1);

                        ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) * -1) - (GlobalCurTaxAmt * -1)));
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

                        ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt));
                        GlobalCurTaxAmt = 0;
                    }
                    else {

                        GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                        cgridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * s.GetText()) / 100) * -1);

                        ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) * -1) - (GlobalCurTaxAmt * -1)));
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

                        ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(cgridTax.GetEditor("TaxField").GetValue())) / 100) - GlobalCurTaxAmt));
                        GlobalCurTaxAmt = 0;
                    }
                    else {

                        GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                        cgridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * cgridTax.GetEditor("TaxField").GetValue()) / 100) * -1);

                        ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(cgridTax.GetEditor("TaxField").GetValue())) / 100) * -1) - (GlobalCurTaxAmt * -1)));
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
            if (e.buttonIndex == 0) {

                if (cddl_AmountAre.GetValue() != null) {
                    var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";

                    if (ProductID.trim() != "") {
                        globalNetAmount = parseFloat(grid.GetEditor("TotalAmount").GetValue());
                        document.getElementById('setCurrentProdCode').value = ProductID.split('||')[0];
                        document.getElementById('HdSerialNo').value = grid.GetEditor('SrlNo').GetText();
                        ctxtTaxTotAmt.SetValue(0);
                        ccmbGstCstVat.SetSelectedIndex(0);
                        $('.RecalculateInline').hide();
                        caspxTaxpopUp.Show();
                        //Set Product Gross Amount and Net Amount

                        var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
                        var SpliteDetails = ProductID.split("||@||");
                        var strMultiplier = SpliteDetails[7];
                        var strFactor = SpliteDetails[8];
                        var strRate = (ctxt_Rate.GetValue() != null && ctxt_Rate.GetValue() != "0") ? ctxt_Rate.GetValue() : "1";
                        //var strRate = "1";
                        var strStkUOM = SpliteDetails[4];
                        // var strSalePrice = SpliteDetails[6];
                        var strSalePrice = (grid.GetEditor('SalePrice').GetValue() != null) ? grid.GetEditor('SalePrice').GetValue() : "";
                        if (strRate == 0) {
                            strRate = 1;
                        }

                        var StockQuantity = strMultiplier * QuantityValue;
                        var Amount = QuantityValue * strFactor * (strSalePrice / strRate);
                        var discountAmt = (grid.GetEditor('Discount').GetValue() / 100);

                        var netAmt = Amount - (Amount * discountAmt);

                        clblTaxProdGrossAmt.SetText(parseFloat((Amount * 100) / 100).toFixed(2));
                        clblProdNetAmt.SetText(parseFloat(Math.round(netAmt * 100) / 100).toFixed(2));
                        document.getElementById('HdProdGrossAmt').value = Amount;
                        document.getElementById('HdProdNetAmt').value = netAmt;

                        //End Here

                        //Set Discount Here
                        if (parseFloat(grid.GetEditor('Discount').GetValue()) > 0) {
                            var discount = Math.round((Amount * grid.GetEditor('Discount').GetValue() / 100)).toFixed(2);
                            clblTaxDiscount.SetText(discount);
                        }
                        else {
                            clblTaxDiscount.SetText('0.00');
                        }
                        //End Here 


                        //Checking is gstcstvat will be hidden or not
                        if (cddl_AmountAre.GetValue() == "2") {
                            $('.GstCstvatClass').hide();
                            $('.gstGrossAmount').show();
                            clblTaxableGross.SetText("(Taxable)");
                            clblTaxableNet.SetText("(Taxable)");
                            $('.gstNetAmount').show();
                            //   var gstRate = parseFloat(cddlVatGstCst.GetValue().split('~')[1]);
                            var gstRate = 0;
                            if (gstRate) {
                                if (gstRate != 0) {
                                    var gstDis = (gstRate / 100) + 1;
                                    if (cddlVatGstCst.GetValue().split('~')[2] == "G") {
                                        $('.gstNetAmount').hide();
                                        clblTaxProdGrossAmt.SetText(Math.round(Amount / gstDis).toFixed(2));
                                        document.getElementById('HdProdGrossAmt').value = Math.round(Amount / gstDis).toFixed(2);
                                        clblGstForGross.SetText(Math.round(Amount - parseFloat(document.getElementById('HdProdGrossAmt').value)).toFixed(2));
                                        clblTaxableNet.SetText("");
                                    }
                                    else {
                                        $('.gstGrossAmount').hide();
                                        clblProdNetAmt.SetText(Math.round(grid.GetEditor('Amount').GetValue() / gstDis).toFixed(2));
                                        document.getElementById('HdProdNetAmt').value = Math.round(grid.GetEditor('Amount').GetValue() / gstDis).toFixed(2);
                                        clblGstForNet.SetText(Math.round(grid.GetEditor('Amount').GetValue() - parseFloat(document.getElementById('HdProdNetAmt').value)).toFixed(2));
                                        clblTaxableGross.SetText("");
                                    }
                                }


                            } else {
                                $('.gstGrossAmount').hide();
                                $('.gstNetAmount').hide();
                                clblTaxableGross.SetText("");
                                clblTaxableNet.SetText("");
                            }
                        }
                        else if (cddl_AmountAre.GetValue() == "1") {
                            $('.GstCstvatClass').show();
                            $('.gstGrossAmount').hide();
                            $('.gstNetAmount').hide();
                            clblTaxableGross.SetText("");
                            clblTaxableNet.SetText("");

                            //Get Customer Shipping StateCode
                            var shippingStCode = '';

                            shippingStCode = $('#lblShippingState').val();// CmbState1.GetText();

                            shippingStCode = shippingStCode.substring(shippingStCode.lastIndexOf('(')).replace('(State Code:', '').replace(')', '').trim();

                            //Debjyoti 09032017
                            if (shippingStCode.trim() != '') {
                                for (var cmbCount = 1; cmbCount < ccmbGstCstVat.GetItemCount() ; cmbCount++) {
                                    //Check if gstin is blank then delete all tax
                                    if (ccmbGstCstVat.GetItem(cmbCount).value.split('~')[5] != "") {

                                        if (ccmbGstCstVat.GetItem(cmbCount).value.split('~')[5] == shippingStCode) {

                                            //if its state is union territories then only UTGST will apply
                                            if (shippingStCode == "4" || shippingStCode == "26" || shippingStCode == "25" || shippingStCode == "7" || shippingStCode == "31" || shippingStCode == "34") {
                                                if (ccmbGstCstVat.GetItem(cmbCount).value.split('~')[4] == 'I' || ccmbGstCstVat.GetItem(cmbCount).value.split('~')[4] == 'C' || ccmbGstCstVat.GetItem(cmbCount).value.split('~')[4] == 'S') {
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




                        }
                        //End here

                        if (globalRowIndex > -1) {
                            cgridTax.PerformCallback(grid.keys[globalRowIndex] + '~' + cddl_AmountAre.GetValue());
                        } else {

                            cgridTax.PerformCallback('New~' + cddl_AmountAre.GetValue());
                            //Set default combo
                            cgridTax.cpComboCode = grid.GetEditor('ProductID').GetValue().split('||@||')[9];
                        }

                        ctxtprodBasicAmt.SetValue(grid.GetEditor('Amount').GetValue());
                    } else {
                        grid.batchEditApi.StartEdit(globalRowIndex, 13);
                    }
                }
            }
        }
        function taxAmtButnClick1(s, e) {
            console.log(grid.GetFocusedRowIndex());
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


            var uniqueIndex = globalRowIndex;
            SetTotalTaxableAmount(uniqueIndex, 13);


            return false;


        }

        var taxJson;
        function cgridTax_EndCallBack(s, e) {
            //cgridTax.batchEditApi.StartEdit(0, 1);
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
                ctxtTaxTotAmt.SetValue(Math.round(cgridTax.cpUpdated.split('~')[1]));
                var gridValue = parseFloat(cgridTax.cpUpdated.split('~')[1]);
                var ddValue = parseFloat(ctxtGstCstVat.GetValue());
                ctxtTaxTotAmt.SetValue(Math.round(gridValue + ddValue));
                cgridTax.cpUpdated = "";
            }

            else {
                var totAmt = ctxtTaxTotAmt.GetValue();
                cgridTax.CancelEdit();
                caspxTaxpopUp.Hide();
                grid.batchEditApi.StartEdit(globalRowIndex, 13);
                grid.GetEditor("TaxAmount").SetValue(totAmt);
                cbnrLblTaxAmtval.SetText(DecimalRoundoff(totAmt, 2));
                var totalGst = 0;
                var GSTType = 'G';

                if (cgridTax.cpTotalGST != null) {

                    if (cgridTax.cpGSTType != null) {
                        GSTType = cgridTax.cpGSTType;
                        cgridTax.cpGSTType = null;
                    }

                    totalGst = parseFloat(cgridTax.cpTotalGST);
                    var qty = grid.GetEditor("Quantity").GetValue();
                    var price = grid.GetEditor("SalePrice").GetValue();
                    var Discount = grid.GetEditor("Discount").GetValue();

                    var finalAmt = qty * price;


                    //if (GSTType=="G")
                    //    grid.GetEditor("Amount").SetValue(DecimalRoundoff((finalAmt - totalGst), 2));
                    //else if (GSTType == "N") {

                    grid.GetEditor("Amount").SetValue(DecimalRoundoff((finalAmt - (finalAmt * (Discount / 100)) - totalGst), 2));
                    cbnrLblTaxableAmtval.SetText(grid.GetEditor("Amount").GetText());
                    //}
                    cgridTax.cpTotalGST = null;

                }

                var totalNetAmount = DecimalRoundoff((parseFloat(totAmt) + parseFloat(grid.GetEditor("Amount").GetValue())), 2);
                grid.GetEditor("TotalAmount").SetValue(totalNetAmount);



                var finalNetAmount = parseFloat(grid.GetEditor("TotalAmount").GetValue());
                var finalAmount = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) + (finalNetAmount - globalNetAmount);
                cbnrlblAmountWithTaxValue.SetValue(parseFloat(Math.round(Math.abs(finalAmount) * 100) / 100).toFixed(2));
                SetInvoiceLebelValue();

            }

            if (cgridTax.GetVisibleRowsOnPage() == 0) {
                $('.cgridTaxClass').hide();
                ccmbGstCstVat.Focus();
            }
            //Debjyoti Check where any Gst Present or not
            // If Not then hide the hole section

            SetRunningTotal();
            ShowTaxPopUp("IY");
            RecalCulateTaxTotalAmountInline();
        }

        function recalculateTax() {
            cmbGstCstVatChange(ccmbGstCstVat);
        }
        function recalculateTaxCharge() {
            ChargecmbGstCstVatChange(ccmbGstCstVatcharge);
        }



    </script>

    <%-- Tanmoy Section --%>
    <script>
        function SendSMSChk() {
            var customerID = $('#hdnCustomerId').val();
            if (customerID != "") {

                $.ajax({
                    type: "POST",
                    url: "PosSalesInvoice.aspx/GetPhnNoForSMS",
                    data: JSON.stringify({ InternalID: customerID }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var gridPackingQty = msg.d;
                        if (gridPackingQty == "") {
                            jAlert("Customer dont have Mobile no.");
                            var remember = document.getElementById('chksendSMS');
                            remember.checked = false;
                            $("#hdnCustMobile").val('');
                        }
                        else {
                            $("#hdnCustMobile").val(gridPackingQty);
                        }
                    }
                });


                var remember = document.getElementById('chksendSMS');
                if (remember.checked) {
                    $("#hdnSendSMS").val("1");
                }
                else {
                    $("#hdnSendSMS").val("0");
                }
            }
            else {
                jAlert("Please Select Customer.");
                var remember = document.getElementById('chksendSMS');
                remember.checked = false;
            }
        }
    </script>



    <%--Debu Section End--%>

    <%-- ------Subhra Address and Billing Sectin Start-----25-01-2017---------%>
    <script type="text/javascript">

        function SettingTabStatus() {
            if (GetObjectID('hdnCustomerId').value != null && GetObjectID('hdnCustomerId').value != '' && GetObjectID('hdnCustomerId').value != '0') {
                page.GetTabByName('[B]illing/Shipping').SetEnabled(true);
            }
        }






        function btnSave_QuoteAddress() {
            checking = true;

            // Billing Start
            if (ctxtAddress1.GetText().trim() == '' || ctxtAddress1.GetText() == null) {
                $('#badd1').attr('style', 'display:block');
                checking = false;
                //return false;
            }
            else { $('#badd1').attr('style', 'display:none'); }


            // pin

            if (!$('#hdBillingPin').val()) {
                $('#bpin').attr('style', 'display:block');
                checking = false;
                //return false;
            }
            else { $('#bpin').attr('style', 'display:none'); }
            // Billing End

            // Shipping Start

            if (ctxtsAddress1.GetText().trim() == '' || ctxtsAddress1.GetText() == null) {
                $('#sadd1').attr('style', 'display:block');
                checking = false;
                //return false;
            }
            else { $('#sadd1').attr('style', 'display:none'); }


            // pin

            if (!$('#hdShippingPin').val()) {
                $('#spin').attr('style', 'display:block');
                checking = false;
                //return false;
            }
            else { $('#spin').attr('style', 'display:none'); }

            // Shipping End







            if (checking == true) {


                $('#hdlblShippingCountry').val($('#lblShippingCountry').text());
                $('#hdlblShippingState').val($('#lblShippingState').text());
                $('#hdlblShippingCity').val($('#lblShippingCity').text());
                $('#hdlblBillingCountry').val($('#lblBillingCountry').text());
                $('#hdlblBillingState').val($('#lblBillingState').text());
                $('#hdlblBillingCity').val($('#lblBillingCity').text());

                GetPosForGstValue();

                var custID = GetObjectID('hdnCustomerId').value;
                cComponentPanel.PerformCallback('save~1');

                GetObjectID('hdnAddressDtl').value = '1';
                page.SetActiveTabIndex(0);
                //   gridLookup.Focus();
                cddl_SalesAgent.Focus();
                gridquotationLookup.HideDropDown();
                $('.crossBtn').show();
            }
            else {
                page.SetActiveTabIndex(1);
                $('.crossBtn').show();
            }
        }


        function ClosebillingLookup() {
            billingLookup.ConfirmCurrentSelection();
            billingLookup.HideDropDown();
            billingLookup.Focus();
        }


        //Subhra-----23-01-2017-------
        var Billing_state;
        var Billing_city;
        var Billing_pin;
        var billing_area;

        var Shipping_state;
        var Shipping_city;
        var Shipping_pin;
        var Shipping_area;





        function cmbArea_endcallback(s, e) {
            if (billing_area != 0) {
                s.SetValue(billing_area);
            }
            billing_area = 0;
        }

        function cmbshipArea_endcallback(s, e) {
            if (Shipping_area != 0) {
                s.SetValue(Shipping_area);
            }
            Shipping_area = 0;
        }


        function Panel_endcallback() {

            if (cComponentPanel.cpEParameter == "Edit") {
                $('#DeleteCustomer').val("");
                cContactPerson.PerformCallback('BindContactPerson~' + GetObjectID('hdnCustomerId').value);
                cComponentPanel.cpEParameter = null;
            }
        }

        function AddcustomerClick() {
            var isLighterPage = $("#hidIsLigherContactPage").val();
            debugger;
            if (isLighterPage == 1) {
                var url = '/OMS/management/Master/customerPopup.html?var=1.1.5.9';
                AspxDirectAddCustPopup.SetContentUrl(url);
                //AspxDirectAddCustPopup.ClearVerticalAlignedElementsCache();
                AspxDirectAddCustPopup.RefreshContentUrl();
                AspxDirectAddCustPopup.Show();
            }
            else {
                var url = '';
                //var url = '/OMS/management/Master/Contact_general.aspx?id=' + 'ADD';
                var url = '/OMS/management/Master/Customer_general.aspx';
                // window.location.href = url;
                AspxDirectAddCustPopup.SetContentUrl(url);
                AspxDirectAddCustPopup.Show();
            }
        }

        function disp_prompt(name) {
            if (name == "tab1") {
                var custID = GetObjectID('hdnCustomerId').value;
                if (custID == null && custID == '') {
                    jAlert('Please select a customer');
                    page.SetActiveTabIndex(0);
                    return;
                }
                //else {
                //    page.SetActiveTabIndex(1);
                //    cComponentPanel.PerformCallback('Edit~1');
                //}
            }
        }

    </script>
    <%-- ------Subhra Address and Billing Section End-----25-01-2017---------%>

    <%--Sam Section Start--%>
    <script type="text/javascript">
        $(document).ready(function () {

            //if (document.getElementById('hdAddOrEdit').value == "Edit") {
            //    if (document.getElementById('HdPosType').value == 'Fin') {
            //        var newAdvAmountfin = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) - parseFloat(cbnrLblLessOldMainVal.GetValue());
            //        if (ctxtAdvnceReceipt.GetValue() > 0) {
            //            if (ctxtAdvnceReceipt.GetValue() > ctxtdownPayment.GetValue()) {
            //                cbnrLblLessAdvanceValue.SetValue(parseFloat(Math.round(Math.abs(ctxtdownPayment.GetValue()) * 100) / 100).toFixed(2));
            //            }
            //            else {
            //                cbnrLblLessAdvanceValue.SetValue(parseFloat(Math.round(Math.abs(ctxtAdvnceReceipt.GetValue()) * 100) / 100).toFixed(2));
            //            }
            //        }
            //    }
            //}

            //16.10.2019
            //if (document.getElementById('hdAddOrEdit').value != "Edit") {
            //    createCookie("MenuCloseOpen", "0", 30);
            //}
            $('body').addClass('mini-navbar');
            if ($("#paymentDetails")) {

                //$('html, body').animate({
                //    scrollTop: $("#myDiv").offset().top
                //}, 500);
            }
            cProductsPopup.Hide();
            if (GetObjectID('hdnCustomerId').value == null || GetObjectID('hdnCustomerId').value == '') {
                page.GetTabByName('[B]illing/Shipping').SetEnabled(false);
            }
            //$('#ApprovalCross').click(function () {

            //    window.parent.popup.Hide();
            //    window.parent.cgridPendingApproval.Refresh()();
            //})



            $('#ddl_VatGstCst_I').blur(function () {
                if (grid.GetVisibleRowsOnPage() == 1) {
                    grid.batchEditApi.StartEdit(-1, 2);
                }
            });
            $('#ddl_AmountAre').blur(function () {
                var id = cddl_AmountAre.GetValue();
                if (id == '1' || id == '3') {
                    if (grid.GetVisibleRowsOnPage() == 1) {
                        grid.batchEditApi.StartEdit(-1, 2);
                    }
                }
            });


            setPosView(document.getElementById('HdPosType').value);
            setBannerView($('#HdPosType').val());
            setViewMode();
        });


        function setViewMode() {
            if ($('#HdViewmode').val() == 'Yes') {
                $('#divSubmitButton').hide();
            }
        }

        function setBannerView(type) {
            if (type == 'Cash') {
                $('.clsbnrLblLessAdvance').hide();
            }
        }

        function setPosView(type) {
            if (type == 'Cash') {
                $('#FinancerTable').hide();
            }
            else if (type == 'Crd') {
                $('#FinancerTable').hide();
            }
            else if (type == 'Fin') {

            }
        }




        function UniqueCodeCheck() {

            var QuoteNo = ctxt_PLQuoteNo.GetText();
            if (QuoteNo != '') {
                var CheckUniqueCode = false;
                $.ajax({
                    type: "POST",
                    url: "SalesInvoice.aspx/CheckUniqueCode",
                    data: JSON.stringify({ QuoteNo: QuoteNo }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        CheckUniqueCode = msg.d;
                        if (CheckUniqueCode == true) {
                            //jAlert('Please enter unique PI/Quotation number');
                            $('#duplicateQuoteno').attr('style', 'display:block');
                            ctxt_PLQuoteNo.SetValue('');
                            ctxt_PLQuoteNo.Focus();
                        }
                        else {
                            $('#duplicateQuoteno').attr('style', 'display:none');
                        }
                    }
                });
            }
        }



        var lastFinancer = null;
        var lastSalesman = null;
        var lastChallan = null;

        $(document).ready(function () {
            var schemaid = $('#ddl_numberingScheme').val();

            if (schemaid != null) {
                if (schemaid == '') {
                    ctxt_PLQuoteNo.SetEnabled(false);
                }
            }
            $('#ddl_numberingScheme').change(function () {

                if ($('#ddl_numberingScheme').val() == "") {
                    return;
                }

                var NoSchemeTypedtl = $(this).val();
                debugger;
                var NoSchemeType = NoSchemeTypedtl.toString().split('~')[1];
                var quotelength = NoSchemeTypedtl.toString().split('~')[2];



                if ($('#ddl_numberingScheme').val().split('~')[1] == "0") {
                    tstartdate.SetEnabled(true);
                } else {
                    if ($("#ISAllowBackdatedEntry").val() == "No") {
                        tstartdate.SetEnabled(false);
                    }
                    // tstartdate.SetDate(new Date);
                    cdeliveryDate.SetDate(tstartdate.GetDate());
                }

                var fromdate = NoSchemeTypedtl.toString().split('~')[5];
                var todate = NoSchemeTypedtl.toString().split('~')[6];

                var dt = new Date();

                //tstartdate.SetDate(dt);

                if (dt < new Date(fromdate)) {
                    tstartdate.SetDate(new Date(fromdate));
                }

                if (dt > new Date(todate)) {
                    tstartdate.SetDate(new Date(todate));
                }




                tstartdate.SetMinDate(new Date(fromdate));
                tstartdate.SetMaxDate(new Date(todate));

                var branchID = (NoSchemeTypedtl.toString().split('~')[3] != null) ? NoSchemeTypedtl.toString().split('~')[3] : "";

                //  if (document.getElementById('HdPosType').value == "Fin") {
                //    if (ccmbFinancer.InCallback())
                //      lastFinancer = branchID;
                //    else
                //    ccmbFinancer.PerformCallback(branchID);
                //  }

                if (branchID != "") document.getElementById('ddl_Branch').value = branchID;
                // if (document.getElementById('HdPosType').value != 'Crd') {
                $('#HdSelectedBranch').val(branchID);
                // }

                if ($('#hdBasketId').val() != "") {
                    document.getElementById('hdnPageStatus').value = 'Rebindbasketgrid';
                    grid.PerformCallback('rebindgridFromBasket');
                }

                if (cddl_SalesAgent.InCallback())
                    lastSalesman = branchID;
                // else
                //  cddl_SalesAgent.PerformCallback(branchID);
                GetAllDetailsByBranch();

                loadMainAccountByBranchIdForPayDet(branchID);

                if (cchallanNoScheme.InCallback())
                    lastChallan = 'BindChallanScheme~' + NoSchemeTypedtl.toString().split('~')[3];
                //else
                // cchallanNoScheme.PerformCallback('BindChallanScheme~' + NoSchemeTypedtl.toString().split('~')[3])


                //ctxt_PLQuoteNo.SetMaxLength(quotelength);
                if (NoSchemeType == '1') {
                    ctxt_PLQuoteNo.SetText('Auto');
                    ctxt_PLQuoteNo.SetEnabled(false);
                    //ctxt_PLQuoteNo.SetClientEnabled(false);

                    tstartdate.Focus();
                }
                else if (NoSchemeType == '0') {
                    ctxt_PLQuoteNo.SetEnabled(true);
                    ctxt_PLQuoteNo.GetInputElement().maxLength = quotelength;
                    //ctxt_PLQuoteNo.SetClientEnabled(true);
                    ctxt_PLQuoteNo.SetText('');
                    ctxt_PLQuoteNo.Focus();
                }
                else if (NoSchemeType == '2') {
                    ctxt_PLQuoteNo.SetText('Datewise');
                    ctxt_PLQuoteNo.SetEnabled(false);
                    //ctxt_PLQuoteNo.SetClientEnabled(false);

                    tstartdate.Focus();
                }
                else {
                    ctxt_PLQuoteNo.SetText('');
                    ctxt_PLQuoteNo.SetEnabled(false);
                    //ctxt_PLQuoteNo.SetClientEnabled(true);
                }
            });


        });

        function SetFocusonDemand(e) {
            var key = cddl_AmountAre.GetValue();
            if (key == '1' || key == '3') {
                if (grid.GetVisibleRowsOnPage() == 1) {
                    grid.batchEditApi.StartEdit(-1, 2);
                }
            }
            else if (key == '2') {
                cddlVatGstCst.Focus();
            }

        }

        function PopulateGSTCSTVAT(e) {
            var key = cddl_AmountAre.GetValue();
            //deleteAllRows();

            if (key == 1) {

                grid.GetEditor('TaxAmount').SetEnabled(true);
                cddlVatGstCst.SetEnabled(false);
                //cddlVatGstCst.PerformCallback('1');
                cddlVatGstCst.SetSelectedIndex(0);
                cbtn_SaveRecords.SetVisible(true);
                grid.GetEditor('ProductID').Focus();
                if (grid.GetVisibleRowsOnPage() == 1) {
                    grid.batchEditApi.StartEdit(-1, 2);
                }

            }
            else if (key == 2) {
                grid.GetEditor('TaxAmount').SetEnabled(true);

                cddlVatGstCst.SetEnabled(true);
                cddlVatGstCst.PerformCallback('2');
                cddlVatGstCst.Focus();
                cbtn_SaveRecords.SetVisible(true);
            }
            else if (key == 3) {

                grid.GetEditor('TaxAmount').SetEnabled(false);

                //cddlVatGstCst.PerformCallback('3');
                cddlVatGstCst.SetSelectedIndex(0);
                cddlVatGstCst.SetEnabled(false);
                cbtn_SaveRecords.SetVisible(false);
                if (grid.GetVisibleRowsOnPage() == 1) {
                    grid.batchEditApi.StartEdit(-1, 2);
                }


            }

        }

        //Date Function Start

        function Startdate(s, e) {
            grid.batchEditApi.EndEdit();
            var frontRow = 0;
            var backRow = -1;
            var IsProduct = "";
            for (var i = 0; i <= grid.GetVisibleRowsOnPage() ; i++) {
                var frontProduct = (grid.batchEditApi.GetCellValue(backRow, 'ProductID') != null) ? (grid.batchEditApi.GetCellValue(backRow, 'ProductID')) : "";
                var backProduct = (grid.batchEditApi.GetCellValue(frontRow, 'ProductID') != null) ? (grid.batchEditApi.GetCellValue(frontRow, 'ProductID')) : "";

                if (frontProduct != "" || backProduct != "") {
                    IsProduct = "Y";
                    break;
                }

                backRow--;
                frontRow++;
            }


            var t = s.GetDate();
            ccmbGstCstVat.PerformCallback(t);
            ccmbGstCstVatcharge.PerformCallback(t);
            ctaxUpdatePanel.PerformCallback('DeleteAllTax');
            cbnrOtherChargesvalue.SetText('0.00');
            SetRunningBalance();
            if (IsProduct == "Y") {
                $('#<%=hdfIsDelete.ClientID %>').val('D');
                $('#<%=HdUpdateMainGrid.ClientID %>').val('True');
                grid.UpdateEdit();
            }

            if (t == "")
            { $('#MandatorysDate').attr('style', 'display:block'); }
            else { $('#MandatorysDate').attr('style', 'display:none'); }
        }
        function Enddate(s, e) {

            var t = s.GetDate();
            if (t == "")
            { $('#MandatoryEDate').attr('style', 'display:block'); }
            else { $('#MandatoryEDate').attr('style', 'display:none'); }



            var sdate = tstartdate.GetValue();
            //var edate = tenddate.GetValue();

            //var startDate = new Date(sdate);
            //var endDate = new Date(edate);

            //if (startDate > endDate) {

            //    flag = false;
            //    $('#MandatoryEgSDate').attr('style', 'display:block');
            //}
            //else { $('#MandatoryEgSDate').attr('style', 'display:none'); }
        }

        //Date Function End

        // Popup Section

        function ShowCustom() {

            cPopup_wareHouse.Show();


        }

        // Popup Section End

    </script>
    <%--Sam Section End--%>

    <%--Sudip--%>
    <script>
        var IsProduct = "";
        var currentEditableVisibleIndex;
        var preventEndEditOnLostFocus = false;
        var lastProductID;
        var setValueFlag;
        var canCallBack = true;

        function GridCallBack() {
            grid.PerformCallback('Display');
            canCallBack = true;
        }
        function AllControlInitilize() {




            if (canCallBack) {



                grid.AddNewRow();
                var tbQuotation = grid.GetEditor("SrlNo");
                tbQuotation.SetValue(1);
                grid.batchEditApi.EndEdit();


                if ($('#isBasketContainComponent').val() == 'yes') {
                    jAlert("You have selected Products for which Component exist. Components are shown for respective products. Enter Quantity and Values as applicable.", "Alert", function () { $('#ddl_numberingScheme').focus(); });
                }

                //document.getElementById('HdPosType').value != 'Crd' &&
                if (document.getElementById('HdPosType').value != 'IST') {
                    //  cmbUcpaymentCashLedgerChanged(ccmbUcpaymentCashLedger);
                    $('#HdSelectedBranch').val(document.getElementById('ddl_Branch').value);
                } else {
                    $('#idCashbalanace').hide();
                }


                // if (document.getElementById('hdAddOrEdit').value != "Edit") {
                if ($('#hdBasketId').val() != "") {
                    SetInvoiceLebelValue();
                    loadAddressbyCustomerIDForBasket(GetObjectID('hdnCustomerId').value);
                }
                // }


                if (document.getElementById('hdAddOrEdit').value == "Edit") {
                    isExecutiveHasLedger = 1;
                    $('#customerReceiptButtonSet').hide();
                    if (ccmbOldUnit.GetValue() == "1") {
                        $('#OldUnitSelectionButton').show();
                    } else {
                        $('#OldUnitSelectionButton').hide();
                    }
                    ctxt_PLQuoteNo.SetEnabled(true);
                    SetInvoiceLebelValue();
                } else {
                    //Add block
                    $('#otherChargesId').hide();
                    $('#hdHsnList').val(',');
                    $('#ddl_numberingScheme').focus();
                    GetAllDetailsByBranch();
                }

                if (document.getElementById('HdPosType').value == 'Fin') {
                    $('#HeaderTextforPaymentDetails')[0].innerText = 'Down Payment Details';
                }


                if ($("#hdnShowUOMConversionInEntry").val() == "1") {
                    $("#btnSecondUOM").removeClass('hide');
                }
                else {
                    $("#btnSecondUOM").addClass('hide');
                }
                isDeliveryTypeChanged();
                SetTotalTaxableAmountOnEdit();
                canCallBack = false;
            }
        }






        function ReBindGrid_Currency() {
            var frontRow = 0;
            var backRow = -1;
            var IsProduct = "";
            for (var i = 0; i <= grid.GetVisibleRowsOnPage() ; i++) {
                var frontProduct = (grid.batchEditApi.GetCellValue(backRow, 'ProductID') != null) ? (grid.batchEditApi.GetCellValue(backRow, 'ProductID')) : "";
                var backProduct = (grid.batchEditApi.GetCellValue(frontRow, 'ProductID') != null) ? (grid.batchEditApi.GetCellValue(frontRow, 'ProductID')) : "";

                if (frontProduct != "" || backProduct != "") {
                    IsProduct = "Y";
                    break;
                }

                backRow--;
                frontRow++;
            }

            if (IsProduct == "Y") {
                $('#hdfIsDelete').val('D');
                grid.UpdateEdit();
                grid.PerformCallback('CurrencyChangeDisplay');
            }
        }


        function cmbContactPersonEndCall(s, e) {
            if (cContactPerson.cpDueDate != null) {
                var DeuDate = cContactPerson.cpDueDate;
                var myDate = new Date(DeuDate);

                cContactPerson.cpDueDate = null;
            }

            if (cContactPerson.cpTotalDue != null) {
                var TotalDue = cContactPerson.cpTotalDue;
                var TotalCustDue = "";
                if (TotalDue >= 0) {
                    TotalCustDue = TotalDue + ' Cr';
                    document.getElementById('lblTotalDues').style.color = "red";
                }
                else {
                    TotalDue = TotalDue * (-1);
                    TotalCustDue = TotalDue + ' Db';
                    document.getElementById('lblTotalDues').style.color = "black";
                }

                document.getElementById('lblTotalDues').innerHTML = TotalCustDue;
                pageheaderContent.style.display = "block";
                divDues.style.display = "block";
                cContactPerson.cpTotalDue = null;
            }

        }

        function OnEndCallback(s, e) {
            debugger;
            LoadingPanel.Hide();
            var value = document.getElementById('hdnRefreshType').value;
            $('#<%=hdnDeleteSrlNo.ClientID %>').val("");

            if (grid.GetVisibleRowsOnPage() == 0) {
                OnAddNewClick();

            }
            //Debjyoti Check grid needs to be refreshed or not

            if (grid.cpTaggingTotalAmount) {
                if (grid.cpTaggingTotalAmount != '') {
                    var returnTaggingAmount = parseFloat(grid.cpTaggingTotalAmount);
                    cbnrlblAmountWithTaxValue.SetValue(parseFloat(Math.round(Math.abs(returnTaggingAmount) * 100) / 100).toFixed(2));
                    SetInvoiceLebelValue();
                    grid.cpTaggingTotalAmount = null;
                }
            }


            if ($('#HdUpdateMainGrid').val() == 'True') {
                $('#HdUpdateMainGrid').val('False');
                grid.PerformCallback('DateChangeDisplay');
            }

            if (grid.cpComponent) {
                if (grid.cpComponent == 'true') {
                    grid.cpComponent = null;
                    OnAddNewClick();
                }
            }

            if (grid.cpSaveSuccessOrFail == "outrange") {
                jAlert('Can Not Add More Invoice (POS) Number as Invoice (POS) Scheme Exausted.<br />Update The Scheme and Try Again');
                // OnAddNewClick();
                grid.StartEditRow(0);
                grid.cpSaveSuccessOrFail = '';
            }
                //Rev Rajdip
            else if (grid.cpSaveSuccessOrFail == "nullCredit") {
                OnAddNewClick();
                grid.cpSaveSuccessOrFail = null;
                jAlert('Credit Days must be greater than Zero(0)');
                grid.cpSaveSuccessOrFail = '';
            }
            else if (grid.cpSaveSuccessOrFail == "DueDateLess") {
                OnAddNewClick();
                grid.cpSaveSuccessOrFail = null;
                jAlert('Due date must be greater than Today');
                grid.cpSaveSuccessOrFail = '';
            }
                //End Rev rajdip
            else if (grid.cpSaveSuccessOrFail == "duplicate") {
                //  OnAddNewClick();
                grid.StartEditRow(0);
                jAlert('Cannot save. Duplicate Invoice Number entered.');
                grid.cpSaveSuccessOrFail = '';
            }
            else if (grid.cpSaveSuccessOrFail == "duplicateChallan") {
                //  OnAddNewClick();
                grid.StartEditRow(0);
                jAlert('Can Not Save as Duplicate Challan Numbe No. Found');
                grid.cpSaveSuccessOrFail = '';
            }
            else if (grid.cpSaveSuccessOrFail == "quantityTagged") {
                //OnAddNewClick();
                grid.StartEditRow(0);
                jAlert('Proforma is tagged in Sale Order. So, Quantity of selected products cannot be less than Ordered Quantity.');
                grid.cpSaveSuccessOrFail = '';
            }
            else if (grid.cpSaveSuccessOrFail == "errorInsert") {
                //OnAddNewClick();
                grid.StartEditRow(0);
                jAlert('Please try again later.');
                grid.cpSaveSuccessOrFail = '';
            }


            else if (grid.cpSaveSuccessOrFail == "nullAmount") {
                //  OnAddNewClick();
                grid.StartEditRow(0);
                jAlert('total amount cant not be zero(0).');
                grid.cpSaveSuccessOrFail = '';
            }
            else if (grid.cpSaveSuccessOrFail == "nullQuantity") {
                //OnAddNewClick();
                grid.StartEditRow(0);
                jAlert('Please fill Quantity');
                grid.cpSaveSuccessOrFail = '';
            }
            else if (grid.cpSaveSuccessOrFail == "AddLock") {
               // LoadingPanel.Hide();
                jAlert('Adding Invoice(POS) is not allowed as the Data is freezed from ' + grid.cpAddLockStatus);
                OnAddNewClick();
                grid.cpSaveSuccessOrFail = '';
            }
            else if (grid.cpSaveSuccessOrFail == "duplicateProduct") {
                //  OnAddNewClick();
                grid.StartEditRow(0);
                jAlert('Can not save Duplicate Product in the Invoice (POS) List.');
                grid.cpSaveSuccessOrFail = '';
            }

            else if (grid.cpSaveSuccessOrFail == "MoreThanStock") {
                grid.StartEditRow(0);
                jAlert('Product entered quantity more than stock quantity.Can not proceed.');
                grid.cpSaveSuccessOrFail = '';
            }

            else if (grid.cpSaveSuccessOrFail == "BillingSHippingRequired") {
                grid.StartEditRow(0);
                jAlert('Please Re-Check the Billing/Shipping Details.', "Alert", function () { page.SetActiveTabIndex(1); });
                grid.cpSaveSuccessOrFail = '';
            }

            else if (grid.cpSaveSuccessOrFail == "minSalePriceMust") {
                //OnAddNewClick();
                grid.StartEditRow(0);
                jAlert('Sale Price Should be equal or higher than Min Sale Price');
                grid.cpSaveSuccessOrFail = '';
            }
            else if (grid.cpSaveSuccessOrFail == "MRPLess") {
                //OnAddNewClick();
                grid.StartEditRow(0);
                jAlert('Sale Price Should be equal or less than MRP');
                grid.cpSaveSuccessOrFail = '';
            }
            else if (grid.cpSaveSuccessOrFail == "InValidReceipt") {
                grid.StartEditRow(0);
                jAlert('Mismatched found of HSN for the selected Product(s) and Advance(s). Correct and proceed.');
                grid.cpSaveSuccessOrFail = '';
            }
            else if (grid.cpSaveSuccessOrFail == "ShowApprovalPopup") {

                ctxtCreditDayspopup.SetText(ctxtCreditDays.GetText())
                CreditDayspopup_LostFocus(null, null);
                cpopupApprover.Show();

                grid.cpSaveSuccessOrFail = '';
            }


            else if (grid.cpSaveSuccessOrFail == "checkWarehouse") {
                var SrlNo = grid.cpProductSrlIDCheck;
                //OnAddNewClick();
                grid.StartEditRow(0);
                var msg = "Product Sales Quantity must be equal to Warehouse Quantity for Product. " + SrlNo;
                //var msg = 'You must enter Stock details for type "Already Delivered".';
                jAlert(msg);
                grid.cpSaveSuccessOrFail = '';
            }
            else {
                var Quote_Number = grid.cpQuotationNo;
                grid.cpQuotationNo = null;
                var Quote_Msg = "Invoice No. '" + Quote_Number + "' saved.";

                if (value == "E") {
                    if (grid.cpApproverStatus == "approve") {
                        window.parent.popup.Hide();
                        window.parent.cgridPendingApproval.PerformCallback();
                    }
                    else if (grid.cpApproverStatus == "rejected") {
                        window.parent.popup.Hide();
                        window.parent.cgridPendingApproval.PerformCallback();
                    }
                    else {
                        debugger;
                        if (Quote_Number != "") {
                            if (grid.cpGeneratedInvoice) {
                                var newInvoiceId = grid.cpGeneratedInvoice;
                                var reportName = "";
                                //--------------------------------------Rev Subhra 22-05-2019-------------------------------------------------------------
                                //if (document.getElementById('HdPosType').value == "Cash") {
                                //    reportName = "POS-Cash~D";
                                //    window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=Invoice_POS&id=' + newInvoiceId + '&PrintOption=1', '_blank');

                                //} else if (document.getElementById('HdPosType').value == "Crd") {
                                //    reportName = "POS-Credit~D";
                                //    window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=Invoice_POS&id=' + newInvoiceId + '&PrintOption=1', '_blank');

                                //} else if (document.getElementById('HdPosType').value == "Fin") {
                                //    reportName = "POS-Finance~D";
                                //    window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=Invoice_POS&id=' + newInvoiceId + '&PrintOption=1', '_blank');
                                //    window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=Invoice_POS&id=' + newInvoiceId + '&PrintOption=3', '_blank');

                                //} else if (document.getElementById('HdPosType').value == "IST") {
                                //    reportName = "InterstateStockTransfer-GST~D";
                                //    window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=Invoice_POS&id=' + newInvoiceId + '&PrintOption=1', '_blank');
                                //}

                                //if (grid.cpIsInstallRequired) {
                                //    window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=IC-Default~D&modulename=Install_Coupon&id=" + newInvoiceId, '_blank');
                                //}
                                if ($("#hdnApprover").val() != "1") {
                                    if (document.getElementById('hdnPosDocPrintDesignBasedOnTaxCategory').value == 1) {

                                        if (document.getElementById('HdPosType').value == "Cash") {
                                            reportName = "POS-Cash~D";
                                            window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=Invoice_POS&id=' + newInvoiceId + '&PrintOption=1', '_blank');

                                        } else if (document.getElementById('HdPosType').value == "Crd") {
                                            reportName = "POS-Credit~D";
                                            window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=Invoice_POS&id=' + newInvoiceId + '&PrintOption=1', '_blank');

                                        } else if (document.getElementById('HdPosType').value == "Fin") {
                                            reportName = "POS-Finance~D";
                                            window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=Invoice_POS&id=' + newInvoiceId + '&PrintOption=1', '_blank');
                                            window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=Invoice_POS&id=' + newInvoiceId + '&PrintOption=3', '_blank');

                                        } else if (document.getElementById('HdPosType').value == "IST") {
                                            reportName = "InterstateStockTransfer-GST~D";
                                            window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=Invoice_POS&id=' + newInvoiceId + '&PrintOption=1', '_blank');
                                        }

                                        if (grid.cpIsInstallRequired) {
                                            window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=IC-Default~D&modulename=Install_Coupon&id=" + newInvoiceId, '_blank');
                                        }
                                    }
                                    else {
                                        if (grid.cpIsCGSTorIGST == "CGST") {
                                            reportName = "POS-CGST~D";

                                            window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=Invoice_POS&id=' + newInvoiceId + '&PrintOption=1', '_blank');
                                            if (ccmbDeliveryType.GetText() == "Already Delivered") {
                                                window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + "GatePass~D" + '&modulename=Invoice_POS&id=' + newInvoiceId + '&PrintOption=1', '_blank');
                                            }
                                        }
                                        else if (grid.cpIsCGSTorIGST == "IGST") {
                                            reportName = "POS-IGST~D";
                                            window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=Invoice_POS&id=' + newInvoiceId + '&PrintOption=1', '_blank');
                                            if (ccmbDeliveryType.GetText() == "Already Delivered") {
                                                window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + "GatePass~D" + '&modulename=Invoice_POS&id=' + newInvoiceId + '&PrintOption=1', '_blank');
                                                window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + "GatePass~D" + '&modulename=Invoice_POS&id=' + newInvoiceId + '&PrintOption=2', '_blank');
                                                window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + "GatePass~D" + '&modulename=Invoice_POS&id=' + newInvoiceId + '&PrintOption=3', '_blank');
                                            }
                                        }

                                    }
                                }
                                //-----------------------------------------End of Rev-----------------------------------------
                                window.location.assign("PosSalesInvoiceList.aspx");
                            }
                        }
                        else {
                            window.location.assign("PosSalesInvoiceList.aspx");
                        }
                    }

                }
                else if (value == "N") {
                    if (grid.cpApproverStatus == "approve") {
                        window.parent.popup.Hide();
                        window.parent.cgridPendingApproval.PerformCallback();
                    }
                    else {
                        if (Quote_Number != "") {

                            var newInvoiceId = grid.cpGeneratedInvoice;

                            var reportName = "";
                            //Rev----------------------------Subhra---------03-06-2019----------------------
                            //if (document.getElementById('HdPosType').value == "Cash") {
                            //   reportName = "POS-Cash~D";
                            //  window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=Invoice_POS&id=' + newInvoiceId + '&PrintOption=1', '_blank');

                            // } else if (document.getElementById('HdPosType').value == "Crd") {
                            //    reportName = "POS-Credit~D";
                            //    window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=Invoice_POS&id=' + newInvoiceId + '&PrintOption=1', '_blank');

                            // } else if (document.getElementById('HdPosType').value == "Fin") {
                            //  reportName = "POS-Finance~D";
                            //  window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=Invoice_POS&id=' + newInvoiceId + '&PrintOption=1', '_blank');
                            //  window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=Invoice_POS&id=' + newInvoiceId + '&PrintOption=3', '_blank');

                            // } else if (document.getElementById('HdPosType').value == "IST") {
                            //   reportName = "InterstateStockTransfer-GST~D";
                            //  window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=Invoice_POS&id=' + newInvoiceId + '&PrintOption=1', '_blank');
                            // }




                            // if (grid.cpIsInstallRequired) {
                            //   window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=IC-Default~D&modulename=Install_Coupon&id=" + newInvoiceId, '_blank');
                            //   grid.cpIsInstallRequired = null;
                            //  }
                            if ($("#hdnApprover").val() != "1") {
                                if (document.getElementById('hdnPosDocPrintDesignBasedOnTaxCategory').value == 1 && $("#hdnApprover").val() != "1") {

                                    if (document.getElementById('HdPosType').value == "Cash") {
                                        reportName = "POS-Cash~D";
                                        window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=Invoice_POS&id=' + newInvoiceId + '&PrintOption=1', '_blank');

                                    } else if (document.getElementById('HdPosType').value == "Crd") {
                                        reportName = "POS-Credit~D";
                                        window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=Invoice_POS&id=' + newInvoiceId + '&PrintOption=1', '_blank');

                                    } else if (document.getElementById('HdPosType').value == "Fin") {
                                        reportName = "POS-Finance~D";
                                        window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=Invoice_POS&id=' + newInvoiceId + '&PrintOption=1', '_blank');
                                        window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=Invoice_POS&id=' + newInvoiceId + '&PrintOption=3', '_blank');

                                    } else if (document.getElementById('HdPosType').value == "IST") {
                                        reportName = "InterstateStockTransfer-GST~D";
                                        window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=Invoice_POS&id=' + newInvoiceId + '&PrintOption=1', '_blank');
                                    }

                                    if (grid.cpIsInstallRequired) {
                                        window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=IC-Default~D&modulename=Install_Coupon&id=" + newInvoiceId, '_blank');
                                    }
                                }
                                else {
                                    if (grid.cpIsCGSTorIGST == "CGST") {
                                        reportName = "POS-CGST~D";
                                        window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=Invoice_POS&id=' + newInvoiceId + '&PrintOption=1', '_blank');
                                        if (ccmbDeliveryType.GetText() == "Already Delivered") {
                                            window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + "GatePass~D" + '&modulename=Invoice_POS&id=' + newInvoiceId + '&PrintOption=1', '_blank');
                                        }
                                    }
                                    else if (grid.cpIsCGSTorIGST == "IGST") {
                                        reportName = "POS-IGST~D";
                                        window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=Invoice_POS&id=' + newInvoiceId + '&PrintOption=1', '_blank');
                                        if (ccmbDeliveryType.GetText() == "Already Delivered") {
                                            window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + "GatePass~D" + '&modulename=Invoice_POS&id=' + newInvoiceId + '&PrintOption=1', '_blank');
                                            window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + "GatePass~D" + '&modulename=Invoice_POS&id=' + newInvoiceId + '&PrintOption=2', '_blank');
                                            window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + "GatePass~D" + '&modulename=Invoice_POS&id=' + newInvoiceId + '&PrintOption=3', '_blank');
                                        }
                                    }

                                }
                            }
                            //-----------------------------------------End of Rev-----------------------------------------


                            window.location.reload();
                        }
                        else {
                            window.location.assign("PosSalesInvoice.aspx?key=ADD");
                        }
                    }
                }
                else {
                    var pageStatus = document.getElementById('hdnPageStatus').value;
                    if (pageStatus == "first") {
                        OnAddNewClick();
                        grid.batchEditApi.EndEdit();


                        $('#hdnPageStatus').val('');

                        var LocalCurrency = '1~INR~Rs.';//'<%=Session["LocalCurrency"]%>';
                        var basedCurrency = LocalCurrency.split("~");
                        if ($("#ddl_Currency").val() == basedCurrency[0]) {
                            ctxt_Rate.SetEnabled(false);
                        }
                    }
                    else if (pageStatus == "update") {
                        OnAddNewClick();
                        $('#hdnPageStatus').val('');
                        //document.getElementById("ddlInventory").disabled = true;

                        var LocalCurrency = '1~INR~Rs.';//'<%=Session["LocalCurrency"]%>';
                        var basedCurrency = LocalCurrency.split("~");
                        if ($("#ddl_Currency").val() == basedCurrency[0]) {
                            ctxt_Rate.SetEnabled(false);
                        }
                    }
                    else if (pageStatus == "Quoteupdate") {
                        grid.StartEditRow(0);
                        $('#<%=hdnPageStatus.ClientID %>').val('');
                    }
                    else if (pageStatus == "delete") {
                        grid.StartEditRow(0);

                        $('#<%=hdnPageStatus.ClientID %>').val('');
                    }
                    else if (pageStatus == "Rebindbasketgrid") {
                        grid.StartEditRow(0);
                        $('#<%=hdnPageStatus.ClientID %>').val('');
                    }
                    else {
                        grid.StartEditRow(0);
                    }
}


                //if (grid.cpReturnParameter) {
                //    if (grid.cpReturnParameter != '') {
                //        if (grid.cpReturnParameter == 'UpdateExistingData') {
                //            jAlert('Updated Successfully.', 'Alert', function () {
                //                window.location.assign("PosSalesInvoiceList.aspx");
                //            });
                //        }
                //        grid.cpReturnParameter = null;
                //    }
                //}

}

    if (gridquotationLookup.GetValue() != null) {
        grid.GetEditor('ComponentNumber').SetEnabled(false);
        grid.GetEditor('ProductName').SetEnabled(false);
        grid.GetEditor('Description').SetEnabled(false);
    }
    cProductsPopup.Hide();
}

function Save_ButtonClick() {
    LoadingPanel.Show();



    flag = true;
    grid.batchEditApi.EndEdit();


    if (ccmbDeliveryType.GetValue() == "S") {
        var shippingStCode = '';

        shippingStCode = $('#lblShippingStateText').val();

        shippingStCode = shippingStCode.substring(shippingStCode.lastIndexOf('(')).replace('(State Code:', '').replace(')', '').trim();


        if (FindIsAddressinIGST($('#ddl_Branch').val(), shippingStCode)) {
            flag = false;
            jAlert('This Sale Invoice is "Self Type". Shipping address must be local branch address.')
        }
    }



    if (document.getElementById('HdPosType').value == "Fin") {
        if (parseFloat(ctxtFinanceAmt.GetValue()) <= 0) {
            jAlert("You have entered wrong Financer Due. Please re-check.", "Alert", ctxtFinanceAmt.Focus());
            flag = false;
        }
    }

    if (document.getElementById('PaymentTable')) {
        var table = document.getElementById('PaymentTable');
        if (table.rows[table.rows.length - 1].children[0].children[1].value != "-Select-") {
            flag = validatePaymentDetails(table.rows[table.rows.length - 1]);
        }

    }

    if (parseFloat(ctxtunitValue.GetValue()) != 0 && cOldUnitGrid.GetVisibleRowsOnPage() == 0) {
        jAlert("Selected data is having Old Unit value as " + parseFloat(Math.round(Math.abs(parseFloat($('#HdDiscountAmount').val())) * 100) / 100).toFixed(2) + ". Please select 'Yes' in Old Unit to enter product details and proceed.", "Alert", function () { ccmbOldUnit.Focus(); });
        flag = false;
        LoadingPanel.Hide();
    }

    if (flag) {
        if ($('#hdBasketId').val() != "") {
            var receivedDisAmtByTab = parseFloat($('#HdDiscountAmount').val());
            var enteredDiscountAmt = parseFloat(ctxtunitValue.GetValue());
            if (receivedDisAmtByTab != enteredDiscountAmt) {
                flag = false;
                LoadingPanel.Hide();
                jAlert("Selected data is having Old Unit value as " + parseFloat(Math.round(Math.abs(receivedDisAmtByTab) * 100) / 100).toFixed(2) + ". Please select 'Yes' in Old Unit to enter product details and proceed.", "Alert", function () { ccmbOldUnit.Focus(); });
            }
        }
    }
    if (flag) {
        flag = isEnteredAmountValid();
    }

    if (flag) {
        if (document.getElementById('HdPosType').value != 'Crd' && document.getElementById('HdPosType').value != 'Fin') {
            var EnteredCashAmount = parseFloat($('#cmbUcpaymentCashLedgerAmt').val());
            if (CustomerCurrentDateAmount + EnteredCashAmount >= 200000) {
                jAlert("Cannot Receive more than  1,99,999.00 on a single day.");
                flag = false;
                LoadingPanel.Hide();
            }
        }
    }

    //Delivery Date Checking
    //if (cdeliveryDate.GetDate() == null) {
    //    $('#MandatorysdeliveryDate').attr('style', 'display:block');
    //    flag = false;
    //    LoadingPanel.Hide();
    //} else if (cdeliveryDate.GetDate() < tstartdate.GetDate()) {
    //    $('#MandatorysdeliveryDate').attr('style', 'display:block');
    //    flag = false;
    //    LoadingPanel.Hide();
    //}
    //else {
    //    $('#MandatorysdeliveryDate').attr('style', 'display:none');
    //}

    // Quote no validation Start
    var QuoteNo = ctxt_PLQuoteNo.GetText();
    if (QuoteNo == '' || QuoteNo == null) {
        $('#MandatorysQuoteno').attr('style', 'display:block');
        flag = false;
        LoadingPanel.Hide();
    }
    else {
        $('#MandatorysQuoteno').attr('style', 'display:none');
    }
    // Quote no validation End
    if (ccmbDeliveryType.GetValue() == "0") {
        $('#mandetorydeliveryType').show();
        flag = false;
        LoadingPanel.Hide();
    } else {
        $('#mandetorydeliveryType').hide();
    }

    if (ccmbDeliveryType.GetValue() == 'D') {
        if (cchallanNoScheme.GetValue() == null) {
            $('#mandetorydchallanNoScheme').attr('style', 'display:block');
            flag = false;
            LoadingPanel.Hide();
        } else {
            $('#mandetorydchallanNoScheme').attr('style', 'display:none');
            if (ctxtChallanNo.GetText().trim() == '') {
                $('#mandetorydtxtChallanNo').attr('style', 'display:block');
                flag = false;
                LoadingPanel.Hide();
            } else {
                $('#mandetorydtxtChallanNo').attr('style', 'display:none');
            }
        }
    }

    // Quote Date validation Start
    var sdate = tstartdate.GetValue();
    var edate = tenddate.GetValue();

    var startDate = new Date(sdate);
    var endDate = new Date(edate);
    if (sdate == null || sdate == "") {
        flag = false;
        LoadingPanel.Hide();
        $('#MandatorysDate').attr('style', 'display:block');
    }
    else { $('#MandatorysDate').attr('style', 'display:none'); }
    if (sdate == "") {
        flag = false;
        LoadingPanel.Hide();
        $('#MandatoryEDate').attr('style', 'display:block');
    }
    else {
        $('#MandatoryEDate').attr('style', 'display:none');

    }
    // Quote Date validation End

    // Quote Customer validation Start
    var customerId = GetObjectID('hdnCustomerId').value
    if (customerId == '' || customerId == null) {

        $('#MandatorysCustomer').attr('style', 'display:block');
        flag = false;
        LoadingPanel.Hide();
    }
    else {
        $('#MandatorysCustomer').attr('style', 'display:none');
    }

    if (GetObjectID('hdnCustomerId').value == '' || GetObjectID('hdnCustomerId').value == null) {

        $('#MandatorysCustomer').attr('style', 'display:block');
        flag = false;
        LoadingPanel.Hide();
    }
    else {
        $('#MandatorysCustomer').attr('style', 'display:none');
    }


    // Quote Customer validation End
    //var amtare = cddl_AmountAre.GetValue();
    //if (amtare == '2') {
    //    var taxcodeid = cddlVatGstCst.GetValue();
    //    if (taxcodeid == '' || taxcodeid == null) {
    //        $('#Mandatorytaxcode').attr('style', 'display:block');
    //        flag = false;
    //    }
    //    else {
    //        $('#Mandatorytaxcode').attr('style', 'display:none');
    //    }
    //}

    var frontRow = 0;
    var backRow = -1;
    var IsProduct = "";
    for (var i = 0; i <= grid.GetVisibleRowsOnPage() ; i++) {
        var frontProduct = (grid.batchEditApi.GetCellValue(backRow, 'ProductID') != null) ? (grid.batchEditApi.GetCellValue(backRow, 'ProductID')) : "";
        var backProduct = (grid.batchEditApi.GetCellValue(frontRow, 'ProductID') != null) ? (grid.batchEditApi.GetCellValue(frontRow, 'ProductID')) : "";

        if (frontProduct != "" || backProduct != "") {
            IsProduct = "Y";
            break;
        }

        backRow--;
        frontRow++;
    }

    if (flag != false) {
        if (IsProduct == "Y") {


            if (issavePacking == 1) {
                SaveSendUOM('POS');
                if (aarr.length > 0) {
                    $.ajax({
                        type: "POST",
                        url: "PosSalesInvoice.aspx/SetSessionPacking",
                        data: "{'list':'" + JSON.stringify(aarr) + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            //var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                            var customerval = (GetObjectID('hdnCustomerId').value != null) ? GetObjectID('hdnCustomerId').value : "";
                            $('#<%=hdfLookupCustomer.ClientID %>').val(customerval);

                            $('#<%=hdnRefreshType.ClientID %>').val('N');
                            $('#<%=hdfIsDelete.ClientID %>').val('I');
                            grid.batchEditApi.EndEdit();

                            //   if (document.getElementById('HdPosType').value != 'Crd' ) {
                            SelectAllData(gridUpdateEdit);
                            //} else {
                            //    gridUpdateEdit();
                            //}
                        }
                    });
                }
                else {
                    SaveSendUOM('SC');
                    //var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                    var customerval = (GetObjectID('hdnCustomerId').value != null) ? GetObjectID('hdnCustomerId').value : "";
                    $('#<%=hdfLookupCustomer.ClientID %>').val(customerval);

                    $('#<%=hdnRefreshType.ClientID %>').val('N');
                    $('#<%=hdfIsDelete.ClientID %>').val('I');
                    grid.batchEditApi.EndEdit();

                    //   if (document.getElementById('HdPosType').value != 'Crd' ) {
                    SelectAllData(gridUpdateEdit);
                    //} else {
                    //    gridUpdateEdit();
                    //}
                }
            }
            else {
                SaveSendUOM('SC');
                if (aarr.length > 0) {
                    $.ajax({
                        type: "POST",
                        url: "PosSalesInvoice.aspx/SetSessionPacking",
                        data: "{'list':'" + JSON.stringify(aarr) + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            //var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                            var customerval = (GetObjectID('hdnCustomerId').value != null) ? GetObjectID('hdnCustomerId').value : "";
                            $('#<%=hdfLookupCustomer.ClientID %>').val(customerval);

                            $('#<%=hdnRefreshType.ClientID %>').val('N');
                            $('#<%=hdfIsDelete.ClientID %>').val('I');
                            grid.batchEditApi.EndEdit();

                            //   if (document.getElementById('HdPosType').value != 'Crd' ) {
                            SelectAllData(gridUpdateEdit);
                            //} else {
                            //    gridUpdateEdit();
                            //}
                        }
                    });
                }
                else {
                    SaveSendUOM('SC');
                    //var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                    var customerval = (GetObjectID('hdnCustomerId').value != null) ? GetObjectID('hdnCustomerId').value : "";
                    $('#<%=hdfLookupCustomer.ClientID %>').val(customerval);

                    $('#<%=hdnRefreshType.ClientID %>').val('N');
                    $('#<%=hdfIsDelete.ClientID %>').val('I');
                    grid.batchEditApi.EndEdit();

                    //   if (document.getElementById('HdPosType').value != 'Crd' ) {
                    SelectAllData(gridUpdateEdit);
                    //} else {
                    //    gridUpdateEdit();
                    //}
                }
            }


        }
        else {
            jAlert('Please add atleast single record first');
            LoadingPanel.Hide();
        }
    } else {
        LoadingPanel.Hide();
    }
}


function isEnteredAmountValid() {
    var returnValue = true;
    var enteredAmount = 0;
    var otherCharges = parseFloat(cbnrOtherChargesvalue.GetValue());
    if (document.getElementById('HdPosType').value != 'IST') {
        enteredAmount = parseFloat(GetPaymentTotalEnteredAmount());
    }

    //- parseFloat(ctxtprocFee.GetValue()) - parseFloat(ctxtEmiOtherCharges.GetValue())
    var unPaidAmount = parseFloat(ctxtunitValue.GetValue()) + parseFloat(cbnrLblLessAdvanceValue.GetValue()) + parseFloat(ctxtFinanceAmt.GetValue()) + enteredAmount - otherCharges;
    // if (document.getElementById('HdPosType').value != 'Fin') {



    if (document.getElementById('HdPosType').value != 'Crd' && document.getElementById('HdPosType').value != 'Fin' && document.getElementById('HdPosType').value != 'IST') {

        if (parseFloat(cbnrlblAmountWithTaxValue.GetValue()) != unPaidAmount) {
            jAlert("Mismatch detected in between Invoice Amount and Payment Amount. Cannot proceed.", "Alert", function () {
                $('#cmbUcpaymentCashLedgerAmt').focus();
            });
            returnValue = false;
        }
    }
    else if (document.getElementById('HdPosType').value == 'Fin') {
        var runningBal = parseFloat(clblRunningBalanceCapsul.GetValue());
        if (runningBal != 0) {
            jAlert("Mismatch detected in between Invoice Amount and Payment Amount. Cannot proceed.", "Alert", function () {

            });
            returnValue = false;
        }
    }
    //}
    //else {
    //    if (parseFloat(clblRunningBalanceCapsul.GetValue()) != parseFloat(ctxtFinanceAmt.GetValue())) {
    //        jAlert("Mismatch detected in between Invoice Amount and Payment Amount. Cannot proceed.", "Alert", function () {
    //            ctxtFinanceAmt.Focus();
    //        });
    //        returnValue = false;
    //    }
    //}

    return returnValue;
}

function SaveExit_ButtonClick(s, e) {
    LoadingPanel.Show();

    flag = true;
    grid.batchEditApi.EndEdit();

    if (ccmbDeliveryType.GetValue() == "S") {
        var shippingStCode = '';

        shippingStCode = $('#lblShippingStateText').val();

        shippingStCode = shippingStCode.substring(shippingStCode.lastIndexOf('(')).replace('(State Code:', '').replace(')', '').trim();


        if (FindIsAddressinIGST($('#ddl_Branch').val(), shippingStCode)) {
            flag = false;
            jAlert('This Sale Invoice is "Self Type". Shipping address must be local branch address.')
        }
    }


    if (document.getElementById('HdPosType').value == "Fin") {
        if (parseFloat(ctxtFinanceAmt.GetValue()) <= 0) {
            jAlert("You have entered wrong Financer Due. Please re-check.", "Alert", ctxtFinanceAmt.Focus());
            flag = false;
        }
    }


    if (document.getElementById('PaymentTable')) {
        if (document.getElementById('hdAddOrEdit').value == "Add") {
            var table = document.getElementById('PaymentTable');
            if (table.rows[table.rows.length - 1].children[0].children[1].value != "-Select-") {
                flag = validatePaymentDetails(table.rows[table.rows.length - 1]);
            }
        }
    }

    if (parseFloat(ctxtunitValue.GetValue()) != 0 && cOldUnitGrid.GetVisibleRowsOnPage() == 0) {
        jAlert("Selected data is having Old Unit value as " + parseFloat(Math.round(Math.abs(parseFloat($('#HdDiscountAmount').val())) * 100) / 100).toFixed(2) + ". Please select 'Yes' in Old Unit to enter product details and proceed.", "Alert", function () { ccmbOldUnit.Focus(); });
        flag = false;
        LoadingPanel.Hide();
    }


    if (flag) {
        if ($('#hdBasketId').val() != "") {
            var receivedDisAmtByTab = parseFloat($('#HdDiscountAmount').val());
            var enteredDiscountAmt = parseFloat(ctxtunitValue.GetValue());
            if (receivedDisAmtByTab != enteredDiscountAmt) {
                flag = false;
                jAlert("Selected data is having Old Unit value as " + parseFloat(Math.round(Math.abs(receivedDisAmtByTab) * 100) / 100).toFixed(2) + ". Please select 'Yes' in Old Unit to enter product details and proceed.", "Alert", function () { ccmbOldUnit.Focus(); });
                LoadingPanel.Hide();
            }
        }
    }
    if (flag) {
        flag = isEnteredAmountValid();
    }

    if (flag) {
        if (document.getElementById('HdPosType').value != 'Crd' && document.getElementById('HdPosType').value != 'Fin') {
            var EnteredCashAmount = parseFloat($('#cmbUcpaymentCashLedgerAmt').val());
            if (CustomerCurrentDateAmount + EnteredCashAmount >= 200000) {
                jAlert("Cannot Receive more than  1,99,999.00 on a single day.");
                flag = false;
                LoadingPanel.Hide();
            }
        }
    }
    //Delivery Date Checking
    //if (cdeliveryDate.GetDate() == null) {
    //    $('#MandatorysdeliveryDate').attr('style', 'display:block');
    //    flag = false;
    //    LoadingPanel.Hide();
    //} else if (cdeliveryDate.GetDate() < tstartdate.GetDate()) {
    //    $('#MandatorysdeliveryDate').attr('style', 'display:block');
    //    flag = false;
    //    LoadingPanel.Hide();
    //}
    //else {
    //    $('#MandatorysdeliveryDate').attr('style', 'display:none');
    //}


    // Quote no validation Start
    var QuoteNo = ctxt_PLQuoteNo.GetText();
    if (QuoteNo == '' || QuoteNo == null) {
        $('#MandatorysQuoteno').attr('style', 'display:block');
        flag = false;
        LoadingPanel.Hide();
    }
    else {
        $('#MandatorysQuoteno').attr('style', 'display:none');
    }
    // Quote no validation End
    if (ccmbDeliveryType.GetValue() == "0") {
        $('#mandetorydeliveryType').show();
        flag = false;
        LoadingPanel.Hide();
    } else {
        $('#mandetorydeliveryType').hide();
    }

    if (ccmbDeliveryType.GetValue() == 'D') {
        if (cchallanNoScheme.GetValue() == null) {
            $('#mandetorydchallanNoScheme').attr('style', 'display:block');
            flag = false;
            LoadingPanel.Hide();
        } else {
            $('#mandetorydchallanNoScheme').attr('style', 'display:none');
            if (ctxtChallanNo.GetText().trim() == '') {
                $('#mandetorydtxtChallanNo').attr('style', 'display:block');
                flag = false;
                LoadingPanel.Hide();
            } else {
                $('#mandetorydtxtChallanNo').attr('style', 'display:none');
            }
        }
    }


    // Quote Date validation Start
    var sdate = tstartdate.GetValue();
    var edate = tenddate.GetValue();

    var startDate = new Date(sdate);
    var endDate = new Date(edate);
    if (sdate == null || sdate == "") {
        flag = false;
        $('#MandatorysDate').attr('style', 'display:block');
    }
    else { $('#MandatorysDate').attr('style', 'display:none'); }
    if (sdate == "") {
        flag = false;
        $('#MandatoryEDate').attr('style', 'display:block');
    }
    else {
        $('#MandatoryEDate').attr('style', 'display:none');

    }

    if (flag) {
        if (document.getElementById('HdPosType').value == 'Fin') {
            if (isExecutiveHasLedger == 0) {
                jAlert("No ledger is mapped for the selected Financer.", "Alert", function () {
                    ccmbFinancer.Focus();
                });
                flag = false;
                LoadingPanel.Hide();
            }


        }
    }
    // Quote Date validation End

    // Quote Customer validation Start
    var customerId = GetObjectID('hdnCustomerId').value
    if (customerId == '' || customerId == null) {
        $('#MandatorysCustomer').attr('style', 'display:block');
        flag = false;
        LoadingPanel.Hide();
    }
    else {
        $('#MandatorysCustomer').attr('style', 'display:none');
    }

    if (GetObjectID('hdnCustomerId').value == '' || GetObjectID('hdnCustomerId').value == null) {

        $('#MandatorysCustomer').attr('style', 'display:block');
        flag = false;
        LoadingPanel.Hide();
    }
    else {
        $('#MandatorysCustomer').attr('style', 'display:none');
    }
    // Quote Customer validation End

    //var amtare = cddl_AmountAre.GetValue();
    //if (amtare == '2') {
    //    var taxcodeid = cddlVatGstCst.GetValue();
    //    if (taxcodeid == '' || taxcodeid == null) {
    //        $('#Mandatorytaxcode').attr('style', 'display:block');
    //        flag = false;
    //    }
    //    else {
    //        $('#Mandatorytaxcode').attr('style', 'display:none');
    //    }
    //}

    var frontRow = 0;
    var backRow = -1;
    var IsProduct = "";
    for (var i = 0; i <= grid.GetVisibleRowsOnPage() ; i++) {
        var frontProduct = (grid.batchEditApi.GetCellValue(backRow, 'ProductID') != null) ? (grid.batchEditApi.GetCellValue(backRow, 'ProductID')) : "";
        var backProduct = (grid.batchEditApi.GetCellValue(frontRow, 'ProductID') != null) ? (grid.batchEditApi.GetCellValue(frontRow, 'ProductID')) : "";

        if (frontProduct != "" || backProduct != "") {
            IsProduct = "Y";
            break;
        }

        backRow--;
        frontRow++;
    }

    if (flag != false) {
        if (IsProduct == "Y") {
            SaveSendUOM('SC');
            if (aarr.length > 0) {
                $.ajax({
                    type: "POST",
                    url: "PosSalesInvoice.aspx/SetSessionPacking",
                    data: "{'list':'" + JSON.stringify(aarr) + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        //divSubmitButton.style.display = "none";
                        //var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                        var customerval = (GetObjectID('hdnCustomerId').value != null) ? GetObjectID('hdnCustomerId').value : "";
                        $('#<%=hdfLookupCustomer.ClientID %>').val(customerval);

                        $('#<%=hdnRefreshType.ClientID %>').val('E');
                        $('#<%=hdfIsDelete.ClientID %>').val('I');
                        grid.batchEditApi.EndEdit();



                        if (document.getElementById('HdPosType').value != 'IST') {

                            SelectAllData(gridUpdateEdit);

                        } else {
                            gridUpdateEdit();
                        }

                        // grid.UpdateEdit();
                    }
                });
            }
            else {
                SaveSendUOM('POS');
                //divSubmitButton.style.display = "none";
                //var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                var customerval = (GetObjectID('hdnCustomerId').value != null) ? GetObjectID('hdnCustomerId').value : "";
                $('#<%=hdfLookupCustomer.ClientID %>').val(customerval);

                $('#<%=hdnRefreshType.ClientID %>').val('E');
                $('#<%=hdfIsDelete.ClientID %>').val('I');
                grid.batchEditApi.EndEdit();



                if (document.getElementById('HdPosType').value != 'IST') {

                    SelectAllData(gridUpdateEdit);

                } else {
                    gridUpdateEdit();
                }

                // grid.UpdateEdit();
            }
        }
        else {
            jAlert('Please add atleast single record first');
            LoadingPanel.Hide();
        }
    } else {
        LoadingPanel.Hide();
        if (document.getElementById('hdAddOrEdit').value != "Add") {
            e.processOnServer = false;
        }
    }

}

function gridUpdateEdit() {

    if (document.getElementById('hdAddOrEdit').value != "Edit") {
        OnAddNewClick();
        grid.UpdateEdit();
    }

    // grid.PerformCallback('UpdateExistingData');
}

function QuantityTextChange(s, e) {
    debugger;
    var WishToProceed = true;

    if (document.getElementById('HdPosType').value == "Fin") {
        if (s.GetValue() > 1)
            WishToProceed = confirm("You have entered more than 1 quantity. Wish to Proceed?");
    }
    if (WishToProceed == true) {
        pageheaderContent.style.display = "block";
        var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
        var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
        var key = gridquotationLookup.GetGridView().GetRowKey(gridquotationLookup.GetGridView().GetFocusedRowIndex());

        if (parseFloat(QuantityValue) != parseFloat(ProductGetQuantity)) {
            if (ProductID != null) {
                var SpliteDetails = ProductID.split("||@||");
                var strMultiplier = SpliteDetails[7];
                var strFactor = SpliteDetails[8];
                var strRate = (ctxt_Rate.GetValue() != null && ctxt_Rate.GetValue() != "0") ? ctxt_Rate.GetValue() : "1";

                if (key != null && key != '') {
                    var IsComponentProduct = SpliteDetails[15];
                    var ComponentProduct = SpliteDetails[16];
                    var TotalQty = (grid.GetEditor('TotalQty').GetText() != null) ? grid.GetEditor('TotalQty').GetText() : "0";
                    var BalanceQty = (grid.GetEditor('BalanceQty').GetText() != null) ? grid.GetEditor('BalanceQty').GetText() : "0";
                    var CurrQty = 0;

                    BalanceQty = parseFloat(BalanceQty);
                    TotalQty = parseFloat(TotalQty);
                    QuantityValue = parseFloat(QuantityValue);

                    if (TotalQty > QuantityValue) {
                        CurrQty = BalanceQty + (TotalQty - QuantityValue);
                    }
                    else {
                        CurrQty = BalanceQty - (QuantityValue - TotalQty);
                    }

                    if (CurrQty < 0) {
                        grid.GetEditor("TotalQty").SetValue(TotalQty);
                        grid.GetEditor("Quantity").SetValue(TotalQty);
                        var OrdeMsg = 'Balance Quantity of selected Product from tagged document is (' + ((QuantityValue - TotalQty) + BalanceQty) + '). <br/>Cannot enter quantity more than balance quantity.';
                        grid.batchEditApi.EndEdit();
                        jAlert(OrdeMsg, 'Alert Dialog: [Balace Quantity ]', function (r) {
                            grid.batchEditApi.StartEdit(globalRowIndex, 6);
                        });
                        return false;
                    }
                    else {
                        grid.GetEditor("TotalQty").SetValue(QuantityValue);
                        grid.GetEditor("BalanceQty").SetValue(CurrQty);
                    }
                }
                else {
                    grid.GetEditor("TotalQty").SetValue(QuantityValue);
                    grid.GetEditor("BalanceQty").SetValue(QuantityValue);
                }

                var strProductID = SpliteDetails[0];
                var strProductName = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
                var ddlbranch = $("[id*=ddl_Branch]");
                var strBranch = ddlbranch.find("option:selected").text();

                var strStkUOM = SpliteDetails[4];
                var strSalePrice = SpliteDetails[6];

                if (strRate == 0) {
                    strRate = 1;
                }

                var StockQuantity = strMultiplier * QuantityValue;
                var Amount = QuantityValue * strFactor * (strSalePrice / strRate);

                $('#<%= lblStkQty.ClientID %>').text(StockQuantity);
                $('#<%= lblStkUOM.ClientID %>').text(strStkUOM);
                $('#<%= lblProduct.ClientID %>').text(strProductName);
                $('#<%= lblbranchName.ClientID %>').text(strBranch);

                var IsPackingActive = SpliteDetails[10];
                var Packing_Factor = SpliteDetails[11];
                var Packing_UOM = SpliteDetails[12];
                var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;

                if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
                    $('#<%= lblPackingStk.ClientID %>').text(PackingValue);
                    divPacking.style.display = "block";
                } else {
                    divPacking.style.display = "none";
                }

                //var tbStockQuantity = grid.GetEditor("StockQuantity");
                //tbStockQuantity.SetValue(StockQuantity);

                var tbAmount = grid.GetEditor("Amount");
                tbAmount.SetValue(DecimalRoundoff(Amount, 2));

                var tbTotalAmount = grid.GetEditor("TotalAmount");
                tbTotalAmount.SetValue(DecimalRoundoff(Amount, 2));

                ColumnIndex = 1;
                DiscountTextChange(s, e);



                //  cacpAvailableStock.PerformCallback(strProductID);
            }
            else {
                jAlert('Select a product first.');
                grid.GetEditor('Quantity').SetValue('0');
                grid.GetEditor('ProductID').Focus();
            }
        }
    } else {
        grid.batchEditApi.StartEdit(globalRowIndex, 5);
        var oldAmount = grid.GetEditor("TotalAmount").GetValue();

        grid.GetEditor('Quantity').SetValue('0');
        cbnrlblAmountWithTaxValue.SetValue(parseFloat(cbnrlblAmountWithTaxValue.GetValue()) - oldAmount);
        SetInvoiceLebelValue();

    }



}

/// Code Added By Sam on 23022017 after make editable of sale price field Start

function SalePriceTextChange(s, e) {
    pageheaderContent.style.display = "block";
    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    var Saleprice = (grid.GetEditor('SalePrice').GetValue() != null) ? grid.GetEditor('SalePrice').GetValue() : "0";
    if (Saleprice > 0) {
        if ($("#hdnRateType").val() == "2") {
            if ($("#ProductMinPrice").val() <= Saleprice && $("#ProductMaxPrice").val() >= Saleprice) {

            }
            else {
                jAlert("Product Min price :" + $("#ProductMinPrice").val() + " and Max price :" + $("#ProductMaxPrice").val(), "Alert", function () {
                    grid.batchEditApi.StartEdit(globalRowIndex, 10);
                    return;
                });
                return;
            }
        }
    }

    if (parseFloat(Pre_Price) != parseFloat(Saleprice)) {
        var ProductID = grid.GetEditor('ProductID').GetValue();
        if (ProductID != null) {
            var SpliteDetails = ProductID.split("||@||");
            console.log(SpliteDetails);

            if (parseFloat(s.GetValue()) < parseFloat(SpliteDetails[17])) {
                jAlert("Sale price cannot be lesser than Min Sale Price locked as: " + parseFloat(Math.round(Math.abs(parseFloat(SpliteDetails[17])) * 100) / 100).toFixed(2), "Alert", function () {
                    grid.batchEditApi.StartEdit(globalRowIndex, 10);
                    return;
                });
                s.SetValue(parseFloat(SpliteDetails[6]));
                return;
            }



            if ($('#hdBasketId').val() == "") {
                if (parseFloat(SpliteDetails[18]) != 0 && parseFloat(s.GetValue()) > parseFloat(SpliteDetails[18])) {
                    jAlert("Sale price cannot be greater than MRP locked as: " + parseFloat(Math.round(Math.abs(parseFloat(SpliteDetails[18])) * 100) / 100).toFixed(2), "Alert", function () {
                        grid.batchEditApi.StartEdit(globalRowIndex, 10);
                        return;
                    });
                    s.SetValue(parseFloat(SpliteDetails[6]));
                    return;
                }
            }



            var strMultiplier = SpliteDetails[7];
            var strFactor = SpliteDetails[8];
            var strRate = (ctxt_Rate.GetValue() != null && ctxt_Rate.GetValue() != "0") ? ctxt_Rate.GetValue() : "1";
            //var strRate = "1";
            var strStkUOM = SpliteDetails[4];
            //var strSalePrice = SpliteDetails[6];

            var strProductID = SpliteDetails[0];
            var strProductName = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
            var ddlbranch = $("[id*=ddl_Branch]");
            var strBranch = ddlbranch.find("option:selected").text();

            if (strRate == 0) {
                strRate = 1;
            }

            var StockQuantity = strMultiplier * QuantityValue;
            var Discount = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";

            var Amount = QuantityValue * strFactor * (Saleprice / strRate);
            var amountAfterDiscount = parseFloat(Amount) - ((parseFloat(Discount) * parseFloat(Amount)) / 100);

            var tbAmount = grid.GetEditor("Amount");
            tbAmount.SetValue(DecimalRoundoff(amountAfterDiscount, 2));

            var tbTotalAmount = grid.GetEditor("TotalAmount");
            tbTotalAmount.SetValue(DecimalRoundoff(amountAfterDiscount, 2));

            //GetShipping State Value
            var ShippingStateCode = '';

            if (cddl_PosGst.GetValue() == "S")
                ShippingStateCode = $('#lblShippingStateValue').val();
            else
                ShippingStateCode = $('#lblBillingStateValue').val();

            caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"), SpliteDetails[19], DecimalRoundoff(Amount, 2), DecimalRoundoff(amountAfterDiscount, 2), 'I', ShippingStateCode, $('#ddl_Branch').val())

            var finalNetAmount = parseFloat(tbTotalAmount.GetValue());
            var finalAmount = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) + (finalNetAmount - globalNetAmount);
            cbnrlblAmountWithTaxValue.SetValue(parseFloat(Math.round(Math.abs(finalAmount) * 100) / 100).toFixed(2));
            //cbnrLblTaxableAmtval.SetText(grid.GetEditor("Amount").GetText());
            //cbnrLblTaxAmtval.SetText(grid.GetEditor("TaxAmount").GetText());

            SetInvoiceLebelValue();

            $('#<%= lblProduct.ClientID %>').text(strProductName);
            $('#<%= lblbranchName.ClientID %>').text(strBranch);

            var IsPackingActive = SpliteDetails[10];
            var Packing_Factor = SpliteDetails[11];
            var Packing_UOM = SpliteDetails[12];
            var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;

            if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
                $('#<%= lblPackingStk.ClientID %>').text(PackingValue);
                divPacking.style.display = "block";
            } else {
                divPacking.style.display = "none";
            }




            cacpAvailableStock.PerformCallback(strProductID);

            var uniqueIndex = globalRowIndex;
            SetTotalTaxableAmount(uniqueIndex, 11);
        }
        else {
            jAlert('Select a product first.');
            grid.GetEditor('SalePrice').SetValue('0');
            grid.GetEditor('ProductID').Focus();
        }
    }
}


function SetTotalTaxableAmount(inx, vindex) {
    var count = grid.GetVisibleRowsOnPage();
    var totalAmount = 0;
    var totaltxAmount = 0;
    var totalQuantity = 0;
    for (var i = 0; i < count + 10; i++) {
        if (grid.GetRow(i)) {
            if (grid.GetRow(i).style.display != "none") {
                grid.batchEditApi.StartEdit(i, 2);
                totalAmount = totalAmount + DecimalRoundoff(grid.GetEditor("Amount").GetValue(), 2);
                totalQuantity = totalQuantity + DecimalRoundoff(grid.GetEditor("Quantity").GetValue(), 4);
                if (grid.GetEditor("TaxAmount").GetValue() != null) {
                    totaltxAmount = totaltxAmount + DecimalRoundoff(grid.GetEditor("TaxAmount").GetValue(), 2);
                }
                //totalAmount = totalAmount + DecimalRoundoff(grid.batchEditApi.GetCellValue(i, "Amount"), 2);
                //totaltxAmount = totaltxAmount + DecimalRoundoff(grid.batchEditApi.GetCellValue(i, "TaxAmount"), 2);


                //if (globalRowIndex == i) {
                //    if ($(grid.GetRow(i).children[10].children[1].children[0].children[0].children[0].children[0].children[0]).val().replace('{', '').replace("}", "").split(':')[1].split(',')[0].replace(/&quot;/g, '\\"').replace(/['"]+/g, '').replace('\\', '').split('\\')[0].trim() != "")
                //        totalAmount = totalAmount + DecimalRoundoff($(grid.GetRow(i).children[10].children[1].children[0].children[0].children[0].children[0].children[0]).val().replace('{', '').replace("}", "").split(':')[1].split(',')[0].replace(/&quot;/g, '\\"').replace(/['"]+/g, '').replace('\\', '').split('\\')[0], 2);
                //    if ($(grid.GetRow(i).children[11].children[1].children[0].children[0].children[0].children[0].children[0]).val().trim() != "")
                //        totaltxAmount = totaltxAmount + DecimalRoundoff($(grid.GetRow(i).children[11].children[1].children[0].children[0].children[0].children[0].children[0]).val(), 2);
                //}
                //else {
                //    if (grid.GetRow(i).children[10].children[0].innerText.trim() != "")
                //        totalAmount = totalAmount + DecimalRoundoff(grid.GetRow(i).children[10].children[0].innerText, 2);
                //    if (grid.GetRow(i).children[11].children[0].innerText.trim() != "")
                //        totaltxAmount = totaltxAmount + DecimalRoundoff(grid.GetRow(i).children[11].children[0].innerText, 2);

                //}
            }
        }
    }

    for (i = -1; i > -count - 10; i--) {
        if (grid.GetRow(i)) {
            if (grid.GetRow(i).style.display != "none") {
                grid.batchEditApi.StartEdit(i, 2);
                totalAmount = totalAmount + DecimalRoundoff(grid.GetEditor("Amount").GetValue(), 2);
                totalQuantity = totalQuantity + DecimalRoundoff(grid.GetEditor("Quantity").GetValue(), 4);
                if (grid.GetEditor("TaxAmount").GetValue() != null) {
                    totaltxAmount = totaltxAmount + DecimalRoundoff(grid.GetEditor("TaxAmount").GetValue(), 2);
                }
                //totalAmount = totalAmount + DecimalRoundoff(grid.batchEditApi.GetCellValue(i, "Amount"), 2);
                //totaltxAmount = totaltxAmount + DecimalRoundoff(grid.batchEditApi.GetCellValue(i, "TaxAmount"), 2);



                //if (globalRowIndex == i) {
                //    if ($(grid.GetRow(i).children[10].children[1].children[0].children[0].children[0].children[0].children[0]).val().replace('{', '').replace("}", "").split(':')[1].split(',')[0].replace(/&quot;/g, '\\"').replace(/['"]+/g, '').replace('\\', '').split('\\')[0].trim() != "")
                //        totalAmount = totalAmount + DecimalRoundoff($(grid.GetRow(i).children[10].children[1].children[0].children[0].children[0].children[0].children[0]).val().replace('{', '').replace("}", "").split(':')[1].split(',')[0].replace(/&quot;/g, '\\"').replace(/['"]+/g, '').replace('\\', '').split('\\')[0], 2);
                //    if ($(grid.GetRow(i).children[11].children[1].children[0].children[0].children[0].children[0].children[0]).val().trim() != "")
                //        totaltxAmount = totaltxAmount + DecimalRoundoff($(grid.GetRow(i).children[11].children[1].children[0].children[0].children[0].children[0].children[0]).val(), 2);
                //}
                //else {
                //    if (grid.GetRow(i).children[10].children[0].innerText.trim() != "")
                //        totalAmount = totalAmount + DecimalRoundoff(grid.GetRow(i).children[10].children[0].innerText, 2);
                //    if (grid.GetRow(i).children[11].children[0].innerText.trim() != "")
                //        totaltxAmount = totaltxAmount + DecimalRoundoff(grid.GetRow(i).children[11].children[0].innerText, 2);

                //}
            }
        }
    }

    // globalRowIndex = inx;

    grid.batchEditApi.EndEdit()
    cbnrLblTaxableAmtval.SetText(DecimalRoundoff(totalAmount, 2));
    cbnrLblTaxAmtval.SetText(DecimalRoundoff(totaltxAmount, 2));
    cbnrLblTotalQty.SetText(DecimalRoundoff(totalQuantity, 4));
    //grid.batchEditApi.StartEdit(globalRowIndex, vindex);
    setTimeout(function () { grid.batchEditApi.StartEdit(inx, vindex); }, 200)
}


function SetTotalTaxableAmountOnEdit() {

    var totalAmount = 0;
    var totaltxAmount = 0;
    var totalQuantity = 0;
    for (var i = 0; i < 1000; i++) {
        if (grid.GetRow(i)) {
            if (grid.GetRow(i).style.display != "none") {
                grid.batchEditApi.StartEdit(i, 2);
                totalAmount = totalAmount + DecimalRoundoff(grid.GetEditor("Amount").GetValue(), 2);
                totalQuantity = totalQuantity + DecimalRoundoff(grid.GetEditor("Quantity").GetValue(), 4);
                if (grid.GetEditor("TaxAmount").GetValue() != null)
                    totaltxAmount = totaltxAmount + DecimalRoundoff(grid.GetEditor("TaxAmount").GetValue(), 2);
            }
        }
    }

    for (i = -1; i > -1000; i--) {
        if (grid.GetRow(i)) {
            if (grid.GetRow(i).style.display != "none") {
                grid.batchEditApi.StartEdit(i, 2);
                totalAmount = totalAmount + DecimalRoundoff(grid.GetEditor("Amount").GetValue(), 2);
                totalQuantity = totalQuantity + DecimalRoundoff(grid.GetEditor("Quantity").GetValue(), 4);
                if (grid.GetEditor("TaxAmount").GetValue() != null)
                    totaltxAmount = totaltxAmount + DecimalRoundoff(grid.GetEditor("TaxAmount").GetValue(), 2);

            }
        }
    }

    // globalRowIndex = inx;
    //setTimeout(function () { grid.batchEditApi.StartEdit(globalRowIndex, vindex); }, 500)

    cbnrLblTaxableAmtval.SetText(DecimalRoundoff(totalAmount, 2));
    cbnrLblTaxAmtval.SetText(DecimalRoundoff(totaltxAmount, 2));
    cbnrLblTotalQty.SetText(DecimalRoundoff(totalQuantity, 4));
}

/// Code Above Added By Sam on 23022017 after make editable of sale price field End




function DiscountTextChange(s, e) {
    //var Amount = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
    var Discount = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";
    var Total_Amount = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    var ProductID = grid.GetEditor('ProductID').GetValue();

    if (ProductID != null) {
        var SpliteDetails = ProductID.split("||@||");
        var strFactor = SpliteDetails[8];
        var strRate = (ctxt_Rate.GetValue() != null && ctxt_Rate.GetValue() != "0") ? ctxt_Rate.GetValue() : "1";
        var strSalePrice = (grid.GetEditor('SalePrice').GetValue() != null) ? grid.GetEditor('SalePrice').GetValue() : "0";
        if (strSalePrice == '0') {
            strSalePrice = SpliteDetails[6];
        }
        if (strRate == 0) {
            strRate = 1;
        }
        var Amount = QuantityValue * strFactor * (strSalePrice / strRate);

        var amountAfterDiscount = parseFloat(Amount) - ((parseFloat(Discount) * parseFloat(Amount)) / 100);

        var tbAmount = grid.GetEditor("Amount");
        tbAmount.SetValue(DecimalRoundoff(amountAfterDiscount, 2));

        var IsPackingActive = SpliteDetails[10];
        var Packing_Factor = SpliteDetails[11];
        var Packing_UOM = SpliteDetails[12];
        var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;

        if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
            $('#<%= lblPackingStk.ClientID %>').text(PackingValue);
            divPacking.style.display = "block";
        } else {
            divPacking.style.display = "none";
        }

        var tbTotalAmount = grid.GetEditor("TotalAmount");
        tbTotalAmount.SetValue(DecimalRoundoff(amountAfterDiscount, 2));


        var ShippingStateCode = '';

        // ShippingStateCode = $('#lblShippingStateValue').val();//CmbState1.GetValue();

        if (cddl_PosGst.GetValue() == "S")
            ShippingStateCode = $('#lblShippingStateValue').val();
        else
            ShippingStateCode = $('#lblBillingStateValue').val();

        caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"), SpliteDetails[19], DecimalRoundoff(Amount, 2), DecimalRoundoff(amountAfterDiscount, 2), 'I', ShippingStateCode, $('#ddl_Branch').val());


        var finalNetAmount = parseFloat(tbTotalAmount.GetValue());
        var finalAmount = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) + (finalNetAmount - globalNetAmount);
        cbnrlblAmountWithTaxValue.SetValue(parseFloat(Math.round(Math.abs(finalAmount) * 100) / 100).toFixed(2));
        //cbnrLblTaxableAmtval.SetText(grid.GetEditor("Amount").GetText());
        //cbnrLblTaxAmtval.SetText(grid.GetEditor("TaxAmount").GetText());
        SetInvoiceLebelValue();
    }
    else {
        jAlert('Select a product first.');
        grid.GetEditor('Discount').SetValue('0');
        grid.GetEditor('ProductID').Focus();
    }
    //Debjyoti 
    //  grid.GetEditor('TaxAmount').SetValue(0);

    var _TotalAmount = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";

    if (parseFloat(_TotalAmount) != parseFloat(ProductGetTotalAmount)) {
        ctaxUpdatePanel.PerformCallback('DelQtybySl~' + grid.GetEditor("SrlNo").GetValue());
        cbnrOtherChargesvalue.SetText('0.00');
        SetRunningBalance();
    }

    //if (ColumnIndex == 1) {
    //    ColumnIndex = 0;
    //}
    //else{
    //    var uniqueIndex = globalRowIndex;
    //    SetTotalTaxableAmount(0, 11);
    //}

}
function AddBatchNew(s, e) {
    var ProductIDValue = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";

    var globalRow_Index = 0;
    if (globalRowIndex > 0) {
        globalRow_Index = globalRowIndex + 1;
    }
    else {
        globalRow_Index = globalRowIndex - 1;
    }


    var keyCode = ASPxClientUtils.GetKeyCode(e.htmlEvent);
    if (keyCode === 13) {
        if (ProductIDValue != "") {
            //var noofvisiblerows = grid.GetVisibleRowsOnPage();
            //var i;
            //var cnt = 2;

            grid.batchEditApi.EndEdit();

            grid.AddNewRow();
            grid.SetFocusedRowIndex();
            var noofvisiblerows = grid.GetVisibleRowsOnPage();

            var tbQuotation = grid.GetEditor("SrlNo");
            tbQuotation.SetValue(noofvisiblerows);

            grid.batchEditApi.StartEdit(globalRow_Index, 2);
            //grid.batchEditApi.StartEdit(-1, 1);
        }
    }
}
function OnAddNewClick(callback) {
    if (gridquotationLookup.GetValue() == null) {
        grid.AddNewRow();

        var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
        var tbQuotation = grid.GetEditor("SrlNo");
        tbQuotation.SetValue(noofvisiblerows);
    }
    else {
        QuotationNumberChanged();
    }



}

function Save_TaxClick() {
    if (gridTax.GetVisibleRowsOnPage() > 0) {
        gridTax.UpdateEdit();
    }
    else {
        gridTax.PerformCallback('SaveGst');
    }
    cbnrOtherChargesvalue.SetText('0.00');
    SetInvoiceLebelValue();
    cPopup_Taxes.Hide();
}

var Warehouseindex;
function OnCustomButtonClick(s, e) {

    if (e.buttonID == 'CustomDelete') {
        grid.batchEditApi.StartEdit(e.visibleIndex);
        var SrlNo = grid.batchEditApi.GetCellValue(e.visibleIndex, 'SrlNo');
        var totalNetAmount = grid.batchEditApi.GetCellValue(e.visibleIndex, 'TotalAmount');

        grid.batchEditApi.EndEdit();

        $('#<%=hdnRefreshType.ClientID %>').val('');
        $('#<%=hdnDeleteSrlNo.ClientID %>').val(SrlNo);
        var noofvisiblerows = grid.GetVisibleRowsOnPage();

        if (gridquotationLookup.GetValue() != null) {
            var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";
            var messege = "";
            if (type == "QO") {
                messege = "Cannot Delete using this button as the Proforma is linked with this Sale Invoice.<br /> Click on Plus(+) sign to Add or Delete Product from last column !";
            }
            else if (type == "SO") {
                messege = "Cannot Delete using this button as the Sales Order is linked with this Sale Invoice.<br /> Click on Plus(+) sign to Add or Delete Product from last column !";
            }
            else if (type == "SC") {
                messege = "Cannot Delete using this button as the Sales Challan is linked with this Sale Invoice.<br /> Click on Plus(+) sign to Add or Delete Product from last column !";
            }

            jAlert(messege, 'Alert Dialog: [Delete Challan Products]', function (r) {
            });
        }
        else {
            if (noofvisiblerows != "1") {

                var newTotalNetAmount = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) - parseFloat(totalNetAmount);
                cbnrlblAmountWithTaxValue.SetValue(parseFloat(Math.round(Math.abs(newTotalNetAmount) * 100) / 100).toFixed(2));
                SetInvoiceLebelValue();

                var prodIDForHsn = grid.batchEditApi.GetCellValue(e.visibleIndex, 'ProductID').split("||@||");
                if (prodIDForHsn.length > 19) {
                    var HSNSac = prodIDForHsn[19];
                    RemoveHSnSacFromList(HSNSac);
                }


                grid.DeleteRow(e.visibleIndex);

                var uniqueIndex = globalRowIndex;
                SetTotalTaxableAmount(0, 2);

                $('#<%=hdfIsDelete.ClientID %>').val('D');
                    grid.UpdateEdit();
                    grid.PerformCallback("Display");

                    $('#<%=hdnPageStatus.ClientID %>').val('delete');
                    //grid.batchEditApi.StartEdit(-1, 2);
                    //grid.batchEditApi.StartEdit(0, 2);
            }
        }
    }
    else if (e.buttonID == 'AddNew') {
        var ProductIDValue = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
        if (ProductIDValue != "") {
            var SpliteDetails = ProductIDValue.split("||@||");
            var IsComponentProduct = SpliteDetails[15];
            var ComponentProduct = SpliteDetails[16];

            if (IsComponentProduct == "Y") {
                var messege = "Selected product is defined with components.<br/> Would you like to proceed with components?";
                jConfirm(messege, 'Confirmation Dialog', function (r) {
                    if (r == true) {
                        grid.batchEditApi.StartEdit(e.visibleIndex, 2);
                        grid.GetEditor("IsComponentProduct").SetValue("Y");
                        $('#<%=hdfIsDelete.ClientID %>').val('C');

                        grid.UpdateEdit();
                        grid.PerformCallback('Display~fromComponent');
                        //grid.batchEditApi.StartEdit(globalRowIndex, 3);
                    }
                    else {
                        OnAddNewClick();
                    }
                });
                document.getElementById('popup_ok').focus();
            }
            else {
                OnAddNewClick();
            }
        }
        else {
            grid.batchEditApi.StartEdit(e.visibleIndex, 2);
        }
    }
    else if (e.buttonID == 'CustomWarehouse') {

        var index = e.visibleIndex;

        if (ccmbDeliveryType.GetValue() != "D") {
            jAlert("Only Applicable for delivery type 'Already Delivered'.");
            return;
        }

        grid.batchEditApi.StartEdit(index, 2)
        Warehouseindex = index;

        var SrlNo = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
        var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
        var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
        //var StkQuantityValue = (grid.GetEditor('StockQuantity').GetValue() != null) ? grid.GetEditor('StockQuantity').GetValue() : "0";

        $("#spnCmbWarehouse").hide();
        $("#spnCmbBatch").hide();
        $("#spncheckComboBox").hide();
        $("#spntxtQuantity").hide();

        if (ProductID != "" && parseFloat(QuantityValue) != 0) {
            var SpliteDetails = ProductID.split("||@||");
            var strProductID = SpliteDetails[0];
            var strDescription = SpliteDetails[1];
            var strUOM = SpliteDetails[2];
            $("#warehousestrProductID").val(strProductID);
            var strStkUOM = SpliteDetails[4];
            var strMultiplier = SpliteDetails[7];
            var strProductName = strDescription;
            //var strProductName = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "";
            var StkQuantityValue = QuantityValue * strMultiplier;
            var Ptype = SpliteDetails[14];
            $('#hdfProductType').val(Ptype);

            document.getElementById('<%=lblProductName.ClientID %>').innerHTML = strProductName;
            document.getElementById('<%=txt_SalesAmount.ClientID %>').innerHTML = QuantityValue;
            document.getElementById('<%=txt_SalesUOM.ClientID %>').innerHTML = strUOM;
            document.getElementById('<%=txt_StockAmount.ClientID %>').innerHTML = StkQuantityValue;
            document.getElementById('<%=txt_StockUOM.ClientID %>').innerHTML = strStkUOM;

            $('#<%=hdfProductID.ClientID %>').val(strProductID);
            $('#<%=hdfProductSerialID.ClientID %>').val(SrlNo);
            $('#<%=hdfProductSerialID.ClientID %>').val(SrlNo);
            $('#<%=hdnProductQuantity.ClientID %>').val(QuantityValue);
            //cacpAvailableStock.PerformCallback(strProductID);


            //strProductID

            strProAlt = strProductID;




            SecondUOMProductId = strProductID;

            if (Ptype == "W") {
                div_Warehouse.style.display = 'block';
                div_Batch.style.display = 'none';
                div_Serial.style.display = 'none';
                div_Quantity.style.display = 'block';
                cCmbWarehouse.PerformCallback('BindWarehouse');
                cGrdWarehouse.PerformCallback('Display~' + SrlNo);

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

                SelectedWarehouseID = "0";
                cPopup_Warehouse.Show();
            }
            else {
                //div_Warehouse.style.display = 'none';
                //div_Batch.style.display = 'none';
                //div_Serial.style.display = 'none';
                //div_Quantity.style.display = 'none';
                jAlert("Please enter Quantity !");
                //var strconfirm = confirm("No Warehouse or Batch or Serial is actived.");
                //if (strconfirm == true) {
                //    grid.batchEditApi.StartEdit(index, 5);
                //}
                //else {
                //    grid.batchEditApi.StartEdit(index, 5);
                //}

                //jAlert("No Warehouse or Batch or Serial is actived.");
            }
        }
        else if (ProductID != "" && parseFloat(QuantityValue) == 0) {
            jAlert('Please enter Quantity.');
        }
    }
}

function FinalWarehouse() {
    cGrdWarehouse.PerformCallback('WarehouseFinal');
    //Rev Subhra 26-04-2019
    setTimeout(function () {
        grid.batchEditApi.StartEdit(globalRowIndex, 10);
    }, 200)
    //grid.batchEditApi.StartEdit(globalRowIndex, 10);
    //End of Rev Subhra 26-04-2019
}

function closeWarehouse(s, e) {
    e.cancel = false;
    cGrdWarehouse.PerformCallback('WarehouseDelete');
}

function OnWarehouseEndCallback(s, e) {
    var Ptype = document.getElementById('hdfProductType').value;

    if (cGrdWarehouse.cpIsSave == "Y") {
        cPopup_Warehouse.Hide();
        //grid.batchEditApi.StartEdit(Warehouseindex, 5);
        //Rev Subhra 26-04-2019
        if (aarr) {
            var FilterSerial = $.grep(aarr, function (e) { return e.productid == strProAlt });
            if (FilterSerial.length > 0) {
                ctxtQuantity.SetValue(FilterSerial[0].Quantity);
                ctxtAltQuantity.SetValue(FilterSerial[0].packing);
                ccmbSecondUOM.SetValue(FilterSerial[0].PackingUom);
            }
        }

        setTimeout(function () {
            grid.batchEditApi.StartEdit(globalRowIndex, 10);
        }, 200)
        grid.batchEditApi.StartEdit(globalRowIndex, 10);
        //End of Rev Subhra 26-04-2019

    }
    else if (cGrdWarehouse.cpIsSave == "N") {
        jAlert('Sales Quantity must be equal to Warehouse Quantity.');
    }
    else {
        if (document.getElementById("myCheck").checked == true) {

            if (aarr) {
                var FilterSerial = $.grep(aarr, function (e) { return e.productid == strProAlt });
                if (FilterSerial.length > 0) {
                    ctxtQuantity.SetValue(FilterSerial[0].Quantity);
                    ctxtAltQuantity.SetValue(FilterSerial[0].packing);
                    ccmbSecondUOM.SetValue(FilterSerial[0].PackingUom);
                }
            }

            if (IsPostBack == "N") {
                checkListBox.PerformCallback('BindSerial~' + PBWarehouseID + '~' + PBBatchID);

                IsPostBack = "";
                PBWarehouseID = "";
                PBBatchID = "";
            }

            if (Ptype == "W" || Ptype == "WB") {
                cCmbWarehouse.SetFocus();
            }
            else if (Ptype == "B") {
                cCmbBatch.SetFocus();
            }
            else {
                ctxtserial.SetFocus();
            }
        }
        else {
            if (aarr) {
                var FilterSerial = $.grep(aarr, function (e) { return e.productid == strProAlt });
                if (FilterSerial.length > 0) {
                    ctxtQuantity.SetValue(FilterSerial[0].Quantity);
                    ctxtAltQuantity.SetValue(FilterSerial[0].packing);
                    ccmbSecondUOM.SetValue(FilterSerial[0].PackingUom);
                }
            }

            if (Ptype == "W" || Ptype == "WB" || Ptype == "WS" || Ptype == "WBS") {
                cCmbWarehouse.SetFocus();
            }
            else if (Ptype == "B" || Ptype == "BS") {
                cCmbBatch.SetFocus();
            }
            else if (Ptype == "S") {
                checkComboBox.SetFocus();
            }
        }
    }


}

var SelectWarehouse = "0";
var SelectBatch = "0";
var SelectSerial = "0";
var SelectedWarehouseID = "0";

function CallbackPanelEndCall(s, e) {
    if (cCallbackPanel.cpEdit != null) {
        var strWarehouse = cCallbackPanel.cpEdit.split('~')[0];
        var strBatchID = cCallbackPanel.cpEdit.split('~')[1];
        var strSrlID = cCallbackPanel.cpEdit.split('~')[2];
        var strQuantity = cCallbackPanel.cpEdit.split('~')[3];
        var strAltQuantity = cCallbackPanel.cpEdit.split('~')[4];

        SelectWarehouse = strWarehouse;
        SelectBatch = strBatchID;
        SelectSerial = strSrlID;

        cCmbWarehouse.PerformCallback('BindWarehouse');
        cCmbBatch.PerformCallback('BindBatch~' + strWarehouse);
        checkListBox.PerformCallback('EditSerial~' + strWarehouse + '~' + strBatchID + '~' + strSrlID);

        cCmbWarehouse.SetValue(strWarehouse);
        ctxtQuantity.SetValue(strQuantity);
        ctxtAltQuantity.SetValue(strAltQuantity);
    }
}

function acpAvailableStockEndCall(s, e) {
    if (cacpAvailableStock.cpstock != null) {
        divAvailableStk.style.display = "block";
        //divpopupAvailableStock.style.display = "block";

        var AvlStk = cacpAvailableStock.cpstock + " " + document.getElementById('<%=lblStkUOM.ClientID %>').innerHTML;
        document.getElementById('<%=lblAvailableStk.ClientID %>').innerHTML = AvlStk;
        document.getElementById('<%=lblAvailableStock.ClientID %>').innerHTML = cacpAvailableStock.cpstock;
        document.getElementById('<%=lblAvailableStockUOM.ClientID %>').innerHTML = document.getElementById('<%=lblStkUOM.ClientID %>').innerHTML;

        document.getElementById('<%=lblInvoiced.ClientID %>').innerHTML = cacpAvailableStock.cpActualStock;
        document.getElementById('<%=lblActStock.ClientID %>').innerHTML = cacpAvailableStock.cpbalanceStock;

        cCmbWarehouse.cpstock = null;
        cacpAvailableStock.cpActualStock = null;
        cacpAvailableStock.cpbalanceStock = null;



        //var uniqueIndex = globalRowIndex;
        //SetTotalTaxableAmount(uniqueIndex, 11);
    }
}
function ctaxUpdatePanelEndCall(s, e) {
    if (ctaxUpdatePanel.cpstock != null) {
        divAvailableStk.style.display = "block";
        //divpopupAvailableStock.style.display = "block";

        var AvlStk = ctaxUpdatePanel.cpstock + " " + document.getElementById('<%=lblStkUOM.ClientID %>').innerHTML;
        document.getElementById('<%=lblAvailableStk.ClientID %>').innerHTML = AvlStk;
     <%--   document.getElementById('<%=lblAvailableStock.ClientID %>').innerHTML = ctaxUpdatePanel.cpstock;
        document.getElementById('<%=lblAvailableStockUOM.ClientID %>').innerHTML = document.getElementById('<%=lblStkUOM.ClientID %>').innerHTML;--%>

        document.getElementById('<%=lblInvoiced.ClientID %>').innerHTML = ctaxUpdatePanel.cpActualStock;
        document.getElementById('<%=lblActStock.ClientID %>').innerHTML = ctaxUpdatePanel.cpbalanceStock;



        ctaxUpdatePanel.cpstock = null;
        ctaxUpdatePanel.cpstock = null;
        ctaxUpdatePanel.cpstock = null;
        //    grid.batchEditApi.StartEdit(globalRowIndex, 5);
        cproductLookUp.Clear();
        return false;
    }
}

function CmbWarehouseEndCallback(s, e) {
    if (SelectWarehouse != "0") {
        cCmbWarehouse.SetValue(SelectWarehouse);
        SelectWarehouse = "0";
    }
    else {
        cCmbWarehouse.SetEnabled(true);
        cCmbWarehouse.ShowDropDown();
        cCmbWarehouse.SetFocus();
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

function Save_TaxesClick() {
    grid.batchEditApi.EndEdit();
    var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
    var i, cnt = 1;
    var sumAmount = 0, sumTaxAmount = 0, sumDiscount = 0, sumNetAmount = 0, sumDiscountAmt = 0;

    cnt = 1;
    for (i = -1 ; cnt <= noofvisiblerows ; i--) {
        var Amount = (grid.batchEditApi.GetCellValue(i, 'Amount') != null) ? (grid.batchEditApi.GetCellValue(i, 'Amount')) : "0";
        var TaxAmount = (grid.batchEditApi.GetCellValue(i, 'TaxAmount') != null) ? (grid.batchEditApi.GetCellValue(i, 'TaxAmount')) : "0";
        var Discount = (grid.batchEditApi.GetCellValue(i, 'Discount') != null) ? (grid.batchEditApi.GetCellValue(i, 'Discount')) : "0";
        var NetAmount = (grid.batchEditApi.GetCellValue(i, 'TotalAmount') != null) ? (grid.batchEditApi.GetCellValue(i, 'TotalAmount')) : "0";
        var sumDiscountAmt = ((parseFloat(Discount) * parseFloat(Amount)) / 100);

        sumAmount = sumAmount + parseFloat(Amount);
        sumTaxAmount = sumTaxAmount + parseFloat(TaxAmount);
        sumDiscount = sumDiscount + parseFloat(sumDiscountAmt);
        sumNetAmount = sumNetAmount + parseFloat(NetAmount);

        cnt++;
    }

    if (sumAmount == 0 && sumTaxAmount == 0 && Discount == 0) {
        cnt = 1;
        for (i = 0 ; cnt <= noofvisiblerows ; i++) {
            var Amount = (grid.batchEditApi.GetCellValue(i, 'Amount') != null) ? (grid.batchEditApi.GetCellValue(i, 'Amount')) : "0";
            var TaxAmount = (grid.batchEditApi.GetCellValue(i, 'TaxAmount') != null) ? (grid.batchEditApi.GetCellValue(i, 'TaxAmount')) : "0";
            var Discount = (grid.batchEditApi.GetCellValue(i, 'Discount') != null) ? (grid.batchEditApi.GetCellValue(i, 'Discount')) : "0";
            var NetAmount = (grid.batchEditApi.GetCellValue(i, 'TotalAmount') != null) ? (grid.batchEditApi.GetCellValue(i, 'TotalAmount')) : "0";
            var sumDiscountAmt = ((parseFloat(Discount) * parseFloat(Amount)) / 100);

            sumAmount = sumAmount + parseFloat(Amount);
            sumTaxAmount = sumTaxAmount + parseFloat(TaxAmount);
            sumDiscount = sumDiscount + parseFloat(sumDiscountAmt);
            sumNetAmount = sumNetAmount + parseFloat(NetAmount);

            cnt++;
        }
    }

    //Debjyoti 
    document.getElementById('HdChargeProdAmt').value = sumAmount;
    document.getElementById('HdChargeProdNetAmt').value = sumNetAmount;
    //End Here

    ctxtProductAmount.SetValue((Math.round(sumAmount * 100) / 100).toFixed(2));
    ctxtProductTaxAmount.SetValue((Math.round(sumTaxAmount * 100) / 100).toFixed(2));
    ctxtProductDiscount.SetValue((Math.round(sumDiscount * 100) / 100).toFixed(2));
    ctxtProductNetAmount.SetValue((Math.round(sumNetAmount * 100) / 100).toFixed(2));
    clblChargesTaxableGross.SetText("");
    clblChargesTaxableNet.SetText("");

    //Checking is gstcstvat will be hidden or not
    if (cddl_AmountAre.GetValue() == "2") {

        $('.lblChargesGSTforGross').show();
        $('.lblChargesGSTforNet').show();

        //Set Gross Amount with GstValue
        //Get The rate of Gst
        var gstRate = 0;
        if (gstRate) {
            if (gstRate != 0) {
                var gstDis = (gstRate / 100) + 1;
                if (cddlVatGstCst.GetValue().split('~')[2] == "G") {
                    $('.lblChargesGSTforNet').hide();
                    ctxtProductAmount.SetText(Math.round(sumAmount / gstDis).toFixed(2));
                    document.getElementById('HdChargeProdAmt').value = Math.round(sumAmount / gstDis).toFixed(2);
                    clblChargesGSTforGross.SetText(Math.round(sumAmount - parseFloat(document.getElementById('HdChargeProdAmt').value)).toFixed(2));
                    clblChargesTaxableGross.SetText("(Taxable)");

                }
                else {
                    $('.lblChargesGSTforGross').hide();
                    ctxtProductNetAmount.SetText(Math.round(sumNetAmount / gstDis).toFixed(2));
                    document.getElementById('HdChargeProdNetAmt').value = Math.round(sumNetAmount / gstDis).toFixed(2);
                    clblChargesGSTforNet.SetText(Math.round(sumNetAmount - parseFloat(document.getElementById('HdChargeProdNetAmt').value)).toFixed(2));
                    clblChargesTaxableNet.SetText("(Taxable)");
                }
            }

        } else {
            $('.lblChargesGSTforGross').hide();
            $('.lblChargesGSTforNet').hide();
        }
    }
    else if (cddl_AmountAre.GetValue() == "1") {
        $('.lblChargesGSTforGross').hide();
        $('.lblChargesGSTforNet').hide();

        //Debjyoti 09032017
        for (var cmbCount = 1; cmbCount < ccmbGstCstVatcharge.GetItemCount() ; cmbCount++) {
            if (ccmbGstCstVatcharge.GetItem(cmbCount).value.split('~')[5] == '19') {
                if (ccmbGstCstVatcharge.GetItem(cmbCount).value.split('~')[4] == 'I') {
                    ccmbGstCstVatcharge.RemoveItem(cmbCount);
                    cmbCount--;
                }
            } else {
                if (ccmbGstCstVatcharge.GetItem(cmbCount).value.split('~')[4] == 'S' || ccmbGstCstVatcharge.GetItem(cmbCount).value.split('~')[4] == 'C') {
                    ccmbGstCstVatcharge.RemoveItem(cmbCount);
                    cmbCount--;
                }
            }
        }






    }
    //End here





    //Set Total amount
    ctxtTotalAmount.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + parseFloat(ctxtProductNetAmount.GetValue()));

    gridTax.PerformCallback('Display');
    //Checking is gstcstvat will be hidden or not
    if (cddl_AmountAre.GetValue() == "2") {
        $('.chargeGstCstvatClass').hide();
    }
    else if (cddl_AmountAre.GetValue() == "1") {
        $('.chargeGstCstvatClass').show();
    }
    //End here
    $('.RecalculateCharge').hide();
    cPopup_Taxes.Show();
    gridTax.StartEditRow(0);
}

var chargejsonTax;
function OnTaxEndCallback(s, e) {
    GetPercentageData();
    $('.gridTaxClass').show();
    if (gridTax.GetVisibleRowsOnPage() == 0) {
        $('.gridTaxClass').hide();
        ccmbGstCstVatcharge.Focus();
    }
    else {
        gridTax.StartEditRow(0);
    }
    //check Json data
    if (gridTax.cpJsonChargeData) {
        if (gridTax.cpJsonChargeData != "") {
            chargejsonTax = JSON.parse(gridTax.cpJsonChargeData);
            gridTax.cpJsonChargeData = null;
        }
    }

    //Set Total Charges And total Amount
    if (gridTax.cpTotalCharges) {
        if (gridTax.cpTotalCharges != "") {
            ctxtQuoteTaxTotalAmt.SetValue(gridTax.cpTotalCharges);
            ctxtTotalAmount.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + parseFloat(ctxtProductNetAmount.GetValue()));
            gridTax.cpTotalCharges = null;
        }
    }
    SetOtherChargesLbl();
    SetInvoiceLebelValue();
    SetChargesRunningTotal();
    ShowTaxPopUp("IN");
    RecalCulateTaxTotalAmountCharges();

}

function GetPercentageData() {
    var Amount = ctxtProductAmount.GetValue();
    var GlobalTaxAmt = 0;
    var noofvisiblerows = gridTax.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
    var i, cnt = 1;
    var sumAmount = 0, totalAmount = 0;
    for (i = 0 ; cnt <= noofvisiblerows ; i++) {
        var totLength = gridTax.batchEditApi.GetCellValue(i, 'TaxName').length;
        var sign = gridTax.batchEditApi.GetCellValue(i, 'TaxName').substring(totLength - 3);
        var DisAmount = (gridTax.batchEditApi.GetCellValue(i, 'Amount') != null) ? (gridTax.batchEditApi.GetCellValue(i, 'Amount')) : "0";

        if (sign == '(+)') {
            sumAmount = sumAmount + parseFloat(DisAmount);
        }
        else {
            sumAmount = sumAmount - parseFloat(DisAmount);
        }

        cnt++;
    }

    totalAmount = (parseFloat(Amount)) + (parseFloat(sumAmount));
    // ctxtTotalAmount.SetValue(totalAmount);
}



function PercentageTextChange(s, e) {
    //var Amount = ctxtProductAmount.GetValue();
    var Amount = gridTax.GetEditor("calCulatedOn").GetValue();
    var GlobalTaxAmt = 0;
    //var Percentage = (gridTax.GetEditor('Percentage').GetValue() != null) ? gridTax.GetEditor('Percentage').GetValue() : "0";
    var Percentage = s.GetText();
    var totLength = gridTax.GetEditor("TaxName").GetText().length;
    var sign = gridTax.GetEditor("TaxName").GetText().substring(totLength - 3);
    Sum = ((parseFloat(Amount) * parseFloat(Percentage)) / 100);

    if (sign == '(+)') {
        GlobalTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
        gridTax.GetEditor("Amount").SetValue(Sum);
        ctxtQuoteTaxTotalAmt.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + parseFloat(Sum) - GlobalTaxAmt);
        //ctxtTotalAmount.SetValue(parseFloat(Amount) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue())); 
        ctxtTotalAmount.SetValue(parseFloat(ctxtProductNetAmount.GetValue()) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
        GlobalTaxAmt = 0;
    }
    else {
        GlobalTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
        gridTax.GetEditor("Amount").SetValue(Sum);
        ctxtQuoteTaxTotalAmt.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) - parseFloat(Sum) + GlobalTaxAmt);
        //ctxtTotalAmount.SetValue(parseFloat(Amount) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue())); 
        ctxtTotalAmount.SetValue(parseFloat(ctxtProductNetAmount.GetValue()) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
        GlobalTaxAmt = 0;
    }

    SetOtherChargeTaxValueOnRespectiveRow(0, Sum, gridTax.GetEditor("TaxName").GetText());
    SetChargesRunningTotal();

    RecalCulateTaxTotalAmountCharges();
}

function RecalCulateTaxTotalAmountCharges() {
    var totalTaxAmount = 0;
    for (var i = 0; i < chargejsonTax.length; i++) {

        if (chargejsonTax[i].SchemeName != "-Select-") {
            gridTax.batchEditApi.StartEdit(i, 3);
            var totLength = gridTax.GetEditor("TaxName").GetText().length;
            var sign = gridTax.GetEditor("TaxName").GetText().substring(totLength - 3);
            if (sign == '(+)') {
                totalTaxAmount = totalTaxAmount + parseFloat(gridTax.GetEditor("Amount").GetValue());
            } else {
                totalTaxAmount = totalTaxAmount - parseFloat(gridTax.GetEditor("Amount").GetValue());
            }

            gridTax.batchEditApi.EndEdit();
        }
    }

    totalTaxAmount = totalTaxAmount + parseFloat(ctxtGstCstVatCharge.GetValue());

    ctxtQuoteTaxTotalAmt.SetValue(totalTaxAmount);
    ctxtTotalAmount.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + parseFloat(ctxtProductNetAmount.GetValue()));
}

//Set Running Total for Charges And Tax 
function SetChargesRunningTotal() {
    var runningTot = parseFloat(ctxtProductNetAmount.GetValue());
    for (var i = 0; i < chargejsonTax.length; i++) {
        gridTax.batchEditApi.StartEdit(i, 3);
        if (chargejsonTax[i].applicableOn == "R") {
            gridTax.GetEditor("calCulatedOn").SetValue(runningTot);
            var totLength = gridTax.GetEditor("TaxName").GetText().length;
            var taxNameWithSign = gridTax.GetEditor("Percentage").GetText();
            var sign = gridTax.GetEditor("TaxName").GetText().substring(totLength - 3);
            var ProdAmt = parseFloat(gridTax.GetEditor("calCulatedOn").GetValue());
            var Amount = gridTax.GetEditor("calCulatedOn").GetValue();
            var GlobalTaxAmt = 0;

            var Percentage = gridTax.GetEditor("Percentage").GetText();
            var totLength = gridTax.GetEditor("TaxName").GetText().length;
            var sign = gridTax.GetEditor("TaxName").GetText().substring(totLength - 3);
            Sum = ((parseFloat(Amount) * parseFloat(Percentage)) / 100);

            if (sign == '(+)') {
                GlobalTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
                gridTax.GetEditor("Amount").SetValue(Sum);
                ctxtQuoteTaxTotalAmt.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + parseFloat(Sum) - GlobalTaxAmt);
                ctxtTotalAmount.SetValue(parseFloat(Amount) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
                //ctxtTotalAmount.SetText(parseFloat(ctxtTotalAmount.GetValue()) + parseFloat(Sum) - GlobalTaxAmt);
                GlobalTaxAmt = 0;
            }
            else {
                GlobalTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
                gridTax.GetEditor("Amount").SetValue(Sum);
                ctxtQuoteTaxTotalAmt.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) - parseFloat(Sum) + GlobalTaxAmt);
                ctxtTotalAmount.SetValue(parseFloat(Amount) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
                //ctxtTotalAmount.SetText(parseFloat(ctxtTotalAmount.GetValue()) - parseFloat(Sum) + GlobalTaxAmt);
                GlobalTaxAmt = 0;
            }

            SetOtherChargeTaxValueOnRespectiveRow(0, Sum, gridTax.GetEditor("TaxName").GetText());


        }
        runningTot = runningTot + parseFloat(gridTax.GetEditor("Amount").GetValue());
        gridTax.batchEditApi.EndEdit();
    }
}

/////////////////// QuotationTaxAmountTextChange By Sam on 23022017
var taxAmountGlobalCharges;
function QuotationTaxAmountGotFocus(s, e) {
    taxAmountGlobalCharges = parseFloat(s.GetValue());
}


function QuotationTaxAmountTextChange(s, e) {
    //var Amount = ctxtProductAmount.GetValue();
    var Amount = gridTax.GetEditor("calCulatedOn").GetValue();
    var GlobalTaxAmt = 0;
    //var Percentage = (gridTax.GetEditor('Percentage').GetValue() != null) ? gridTax.GetEditor('Percentage').GetValue() : "0";
    //var Percentage = s.GetText();
    var totLength = gridTax.GetEditor("TaxName").GetText().length;
    var sign = gridTax.GetEditor("TaxName").GetText().substring(totLength - 3);
    //Sum = ((parseFloat(Amount) * parseFloat(Percentage)) / 100);

    if (sign == '(+)') {
        GlobalTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
        //gridTax.GetEditor("Amount").SetValue(Sum);
        ctxtQuoteTaxTotalAmt.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + GlobalTaxAmt - taxAmountGlobalCharges);
        ctxtTotalAmount.SetValue(parseFloat(ctxtProductNetAmount.GetValue()) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
        GlobalTaxAmt = 0;
        SetOtherChargeTaxValueOnRespectiveRow(0, s.GetValue(), gridTax.GetEditor("TaxName").GetText());
    }
    else {
        GlobalTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
        //gridTax.GetEditor("Amount").SetValue(Sum);
        ctxtQuoteTaxTotalAmt.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) - GlobalTaxAmt + taxAmountGlobalCharges);
        ctxtTotalAmount.SetValue(parseFloat(ctxtProductNetAmount.GetValue()) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
        GlobalTaxAmt = 0;
        SetOtherChargeTaxValueOnRespectiveRow(0, s.GetValue(), gridTax.GetEditor("TaxName").GetText());
    }

    RecalCulateTaxTotalAmountCharges();

}



////////////

var AmountOldValue;
var AmountNewValue;

function AmountTextChange(s, e) {
    AmountLostFocus(s, e);
    var RecieveValue = (grid.GetEditor('Amount').GetValue() != null) ? parseFloat(grid.GetEditor('Amount').GetValue()) : "0";
}

function AmountLostFocus(s, e) {
    AmountNewValue = s.GetText();
    var indx = AmountNewValue.indexOf(',');

    if (indx != -1) {
        AmountNewValue = AmountNewValue.replace(/,/g, '');
    }
    if (AmountOldValue != AmountNewValue) {
        changeReciptTotalSummary();
    }
}

function AmountGotFocus(s, e) {
    AmountOldValue = s.GetText();
    var indx = AmountOldValue.indexOf(',');
    if (indx != -1) {
        AmountOldValue = AmountOldValue.replace(/,/g, '');
    }
}

function changeReciptTotalSummary() {
    var newDif = AmountOldValue - AmountNewValue;
    var CurrentSum = ctxtSumTotal.GetText();
    var indx = CurrentSum.indexOf(',');
    if (indx != -1) {
        CurrentSum = CurrentSum.replace(/,/g, '');
    }

    ctxtSumTotal.SetValue(parseFloat(CurrentSum - newDif));
}

function CmbWarehouse_ValueChange() {
    debugger;
    var WarehouseID = cCmbWarehouse.GetValue();
    var type = document.getElementById('hdfProductType').value;

    if (type == "WBS" || type == "WB") {
        cCmbBatch.PerformCallback('BindBatch~' + WarehouseID);
    }
    else if (type == "WS") {
        checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + "0");
    }
    $.ajax({
        type: "POST",
        url: "Services/Master.asmx/GetWarehouseWisePRoductStock",
        data: JSON.stringify({ WarehouseID: WarehouseID, productid: $("#warehousestrProductID").val() }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            $("#<%=lblAvailableStock.ClientID %>").html(msg.d.toString())

        }
    });


    }
    function CmbBatch_ValueChange() {
        var WarehouseID = cCmbWarehouse.GetValue();
        var BatchID = cCmbBatch.GetValue();
        var type = document.getElementById('hdfProductType').value;

        if (type == "WBS") {
            checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + BatchID);
        }
        else if (type == "BS") {
            checkListBox.PerformCallback('BindSerial~' + "0" + '~' + BatchID);
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
        var altQty = (ctxtAltQuantity.GetValue() != null) ? ctxtAltQuantity.GetValue() : "0";
        var altUOM = (ccmbSecondUOM.GetValue() != null) ? ccmbSecondUOM.GetValue() : "0";




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
                if (Ptype = 'WS') {
                    cCmbWarehouse.PerformCallback('BindWarehouse');
                } else {
                    cCmbWarehouse.PerformCallback('BindWarehouse');
                    cCmbBatch.PerformCallback('BindBatch~' + "");
                    checkListBox.PerformCallback('BindSerial~' + "" + '~' + "");
                    ctxtQuantity.SetValue("0");
                }
            }
            UpdateText();
            cGrdWarehouse.PerformCallback('SaveDisplay~' + WarehouseID + '~' + WarehouseName + '~' + BatchID + '~' + BatchName + '~' + SerialID + '~' + SerialName + '~' + Qty + '~' + SelectedWarehouseID + '~' + altQty + '~' + altUOM);
            SelectedWarehouseID = "0";
        }
    }

    var IsPostBack = "";
    var PBWarehouseID = "";
    var PBBatchID = "";


    //$(document).ready(function () {
    //    $('#ddl_VatGstCst_I').blur(function () {
    //        if (grid.GetVisibleRowsOnPage() == 1) {
    //            grid.batchEditApi.StartEdit(-1, 2);
    //        }
    //    })
    //    $('#ddl_AmountAre').blur(function () {
    //        var id = cddl_AmountAre.GetValue();
    //        if (id == '1' || id == '3') {
    //            if (grid.GetVisibleRowsOnPage() == 1) {
    //                grid.batchEditApi.StartEdit(-1, 2);
    //            }
    //        }
    //    })


    //});

    function deleteAllRows() {
        var frontRow = 0;
        var backRow = -1;
        for (var i = 0; i <= grid.GetVisibleRowsOnPage() + 100 ; i++) {
            grid.DeleteRow(frontRow);
            grid.DeleteRow(backRow);
            backRow--;
            frontRow++;
        }
        OnAddNewClick();
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

    function AutoCalculateMandateOnChange(element) {
        $("#spnCmbWarehouse").hide();
        $("#spnCmbBatch").hide();
        $("#spncheckComboBox").hide();
        $("#spntxtQuantity").hide();

        if (document.getElementById("myCheck").checked == true) {
            divSingleCombo.style.display = "block";
            divMultipleCombo.style.display = "none";

            checkComboBox.Focus();
        }
        else {
            divSingleCombo.style.display = "none";
            divMultipleCombo.style.display = "block";

            ctxtserial.Focus();
        }
    }

    function fn_Deletecity(keyValue) {
        var WarehouseID = (cCmbWarehouse.GetValue() != null) ? cCmbWarehouse.GetValue() : "0";
        var BatchID = (cCmbBatch.GetValue() != null) ? cCmbBatch.GetValue() : "0";

        cGrdWarehouse.PerformCallback('Delete~' + keyValue);
        checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + BatchID);
    }
    function fn_Edit(keyValue) {
        //cGrdWarehouse.PerformCallback('EditWarehouse~' + keyValue);
        SelectedWarehouseID = keyValue;
        cCallbackPanel.PerformCallback('EditWarehouse~' + keyValue);
    }
    </script>
    <script type="text/javascript">
        // <![CDATA[
        var textSeparator = ";";
        var selectedChkValue = "";

        function OnListBoxSelectionChanged(listBox, args) {
            if (args.index == 0)
                args.isSelected ? listBox.SelectAll() : listBox.UnselectAll();
            UpdateSelectAllItemState();
            UpdateText();
        }
        function UpdateSelectAllItemState() {
            IsAllSelected() ? checkListBox.SelectIndices([0]) : checkListBox.UnselectIndices([0]);
        }
        function IsAllSelected() {
            var selectedDataItemCount = checkListBox.GetItemCount() - (checkListBox.GetItem(0).selected ? 0 : 1);
            return checkListBox.GetSelectedItems().length == selectedDataItemCount;
        }
        function UpdateText() {
            var selectedItems = checkListBox.GetSelectedItems();
            selectedChkValue = GetSelectedItemsText(selectedItems);
            //checkComboBox.SetText(GetSelectedItemsText(selectedItems));

            var serialLength = GetSelectedItemsCount(selectedItems);
            checkComboBox.SetText(serialLength + " Items");
            //checkComboBox.SetText(selectedItems.length + " Items");

            var val = GetSelectedItemsText(selectedItems);
            $("#abpl").attr('data-content', val);
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
        $(function () {
            $('[data-toggle="popover"]').popover();
        })
        // ]]>
    </script>
    <script>
        var ProductGetQuantity = "0";
        var ProductGetTotalAmount = "0";

        function ProductsGotFocus(s, e) {
            pageheaderContent.style.display = "block";
            var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
            var strProductName = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
            //var ProductID = (cCmbProduct.GetValue() != null) ? cCmbProduct.GetValue() : "0";
            //var strProductName = (cCmbProduct.GetText() != null) ? cCmbProduct.GetText() : "0";

            var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
            ProductGetQuantity = QuantityValue;

            var ddlbranch = $("[id*=ddl_Branch]");
            var strBranch = ddlbranch.find("option:selected").text();

            var SpliteDetails = ProductID.split("||@||");
            var strProductID = SpliteDetails[0];
            var strDescription = SpliteDetails[1];
            var strUOM = SpliteDetails[2];
            var strStkUOM = SpliteDetails[4];
            var strSalePrice = SpliteDetails[6];
            var IsPackingActive = SpliteDetails[10];
            var Packing_Factor = SpliteDetails[11];
            var Packing_UOM = SpliteDetails[12];
            var strProductShortCode = SpliteDetails[14];
            var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;

            strProductName = strDescription;

            if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
                $('#<%= lblPackingStk.ClientID %>').text(PackingValue);
                divPacking.style.display = "block";
            } else {
                divPacking.style.display = "none";
            }

            $('#<%= lblStkQty.ClientID %>').text(QuantityValue);
            $('#<%= lblStkUOM.ClientID %>').text(strStkUOM);
            $('#<%= lblProduct.ClientID %>').text(strProductName);
            $('#<%= lblbranchName.ClientID %>').text(strBranch);
            globalNetAmount = parseFloat(grid.GetEditor("TotalAmount").GetValue());
            //if (ProductID != "0") {
            //   cacpAvailableStock.PerformCallback(strProductID);
            //}
        }



        function QuantityGotFocus(s, e) {
            ProductsGotFocus(s, e);

            debugger;
            //Surojit 15-03-2019
            var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
            var strProductName = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
            var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
            var SpliteDetails = ProductID.split("||@||");
            var strProductID = SpliteDetails[0];
            var strDescription = SpliteDetails[1];
            var strUOM = SpliteDetails[2];
            var strStkUOM = SpliteDetails[4];
            var strSalePrice = SpliteDetails[6];
            var IsPackingActive = SpliteDetails[10];
            var Packing_Factor = SpliteDetails[11];
            var Packing_UOM = SpliteDetails[12];
            var strProductShortCode = SpliteDetails[14];
            var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;
            IsInventory = '';
            strProductName = strDescription;

            var isOverideConvertion = SpliteDetails[26];
            var packing_saleUOM = SpliteDetails[25];
            var sProduct_SaleUom = SpliteDetails[24];
            var sProduct_quantity = SpliteDetails[22];
            var packing_quantity = SpliteDetails[20];

            var slno = (grid.GetEditor('SrlNo').GetText() != null) ? grid.GetEditor('SrlNo').GetText() : "0";

            var ConvertionOverideVisible = $('#hdnConvertionOverideVisible').val();
            var ShowUOMConversionInEntry = $('#hdnShowUOMConversionInEntry').val();
            var type = 'add';
            var gridprodqty = parseFloat(grid.GetEditor('Quantity').GetText()).toFixed(4);
            var gridPackingQty = '';
            //var gridPackingQty = grid.GetEditor('QuoteDetails_PackingQty').GetText();
            if (SpliteDetails.length > 27) {
                if (SpliteDetails[27] == "1") {
                    IsInventory = 'Yes';

                    type = 'edit';

                    if (SpliteDetails[28] != '') {
                        if (parseFloat(SpliteDetails[28]) > 0) {
                            gridPackingQty = SpliteDetails[28];
                        }
                    }
                }
            }
            //else {
            //    if (SpliteDetails.length == 25) {
            //        var isOverideConvertion = SpliteDetails[23];
            //        var packing_saleUOM = SpliteDetails[22];
            //        var sProduct_SaleUom = SpliteDetails[21];
            //        var sProduct_quantity = SpliteDetails[19];
            //        var packing_quantity = SpliteDetails[17];

            //        if (SpliteDetails[16] != '') {
            //            if (parseFloat(SpliteDetails[16]) > 0) {
            //                gridPackingQty = SpliteDetails[16];
            //            }
            //        }
            //        if (SpliteDetails[24] == "1") {
            //            IsInventory = 'Yes';
            //        }
            //    }

            //}
            debugger;
            var ComponentID = (grid.GetEditor('ComponentID').GetText() != null) ? grid.GetEditor('ComponentID').GetText() : "";


            if (ShowUOMConversionInEntry == "1" && SpliteDetails.length > 1) {
                if (ComponentID != "" && ComponentID != null && ComponentID != "0") {
                    var actiontxt = "";
                    var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";

                    if (type == "QO") {
                        actiontxt = "POSSalesInvoicePackingQtyQuotation";
                    }
                    else if (type == "SO") {
                        actiontxt = "POSSalesInvoicePackingQtyOrder";
                    }
                    else if (type == "SC") {

                        actiontxt = "";
                    }

                    $.ajax({
                        type: "POST",
                        url: "Services/Master.asmx/GetMultiUOMDetails",
                        data: JSON.stringify({ orderid: strProductID, action: actiontxt, module: 'SalesInvoice', strKey: ComponentID }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {

                            gridPackingQty = msg.d;
                            if (IsInventory == 'Yes') {
                                ShowUOM(type, "Sales Invoice", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);
                            }
                        }
                    });
                }
                else {
                    if (IsInventory == 'Yes') {
                        ShowUOM(type, "Sales Invoice", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);
                    }
                }
            }
            //Surojit 15-03-2019
        }


        var issavePacking = 0;

        function SetDataToGrid(Quantity, packing, PackingUom, PackingSelectUom, productid, slno) {
            issavePacking = 1;
            grid.batchEditApi.StartEdit(globalRowIndex);
            grid.GetEditor('Quantity').SetValue(Quantity);
            <%--Use for set focus on UOM after press ok on UOM--%>
            //setTimeout(function () {
            //    grid.batchEditApi.StartEdit(globalRowIndex, 6);
            //}, 200)

            var uniqueIndex = globalRowIndex;
            SetTotalTaxableAmount(uniqueIndex, 6);

            <%--Use for set focus on UOM after press ok on UOM--%>
        }

        function SetFoucs() {
            //debugger;
        }

        $(function () {
            $(".allownumericwithdecimal").on("keypress keyup blur", function (event) {
                //this.value = this.value.replace(/[^0-9\.]/g,'');
                $(this).val($(this).val().replace(/[^0-9\.]/g, ''));
                if ((event.which != 46 || $(this).val().indexOf('.') != -1) && (event.which < 48 || event.which > 57)) {
                    event.preventDefault();
                }
            });
        });
        var IsInventory = '';

        function ProductsGotFocusFromID(s, e) {
            pageheaderContent.style.display = "block";
            var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
            if (ProductID == "") {
                return;
            }

            var strProductName = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
            //var ProductID = (cCmbProduct.GetValue() != null) ? cCmbProduct.GetValue() : "0";
            //var strProductName = (cCmbProduct.GetText() != null) ? cCmbProduct.GetText() : "0";

            var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";

            var ddlbranch = $("[id*=ddl_Branch]");
            var strBranch = ddlbranch.find("option:selected").text();

            var SpliteDetails = ProductID.split("||@||");
            var strProductID = SpliteDetails[0];
            var strDescription = SpliteDetails[1];
            var strUOM = SpliteDetails[2];
            var strStkUOM = SpliteDetails[4];
            var strSalePrice = SpliteDetails[6];
            var IsPackingActive = SpliteDetails[10];
            var Packing_Factor = SpliteDetails[11];
            var Packing_UOM = SpliteDetails[12];
            var strProductShortCode = SpliteDetails[14];
            var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;

            $('#HDSelectedProduct').val(strProductID);
            strProductName = strDescription;

            if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
                $('#<%= lblPackingStk.ClientID %>').text(PackingValue);
                divPacking.style.display = "block";
            } else {
                divPacking.style.display = "none";
            }

            $('#<%= lblStkQty.ClientID %>').text(QuantityValue);
            $('#<%= lblStkUOM.ClientID %>').text(strStkUOM);
            $('#<%= lblProduct.ClientID %>').text(strProductName);
            $('#<%= lblbranchName.ClientID %>').text(strBranch);

            if (ProductID != "0") {
                cacpAvailableStock.PerformCallback(strProductID);
            }
        }




        function PopulateCurrentBankBalance(MainAccountID) {

            var BranchId = $('#ddl_Branch').val();
            $.ajax({
                type: "POST",
                url: 'PosSalesInvoice.aspx/GetCurrentBankBalance',
                data: JSON.stringify({ MainAccountID: MainAccountID, BranchId: BranchId }),

                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var list = msg.d;

                    if (msg.d.length > 0) {
                        document.getElementById("pageheaderContent").style.display = 'block';
                        if (msg.d.split('~')[0] != '') {

                            document.getElementById('<%=B_BankBalance.ClientID %>').innerHTML = msg.d.split('~')[0];
                            document.getElementById('<%=B_BankBalance.ClientID %>').style.color = "Black";
                        }
                        else {
                            document.getElementById('<%=B_BankBalance.ClientID %>').innerHTML = '0.0';
                            document.getElementById('<%=B_BankBalance.ClientID %>').style.color = "Black";

                        }
                    }

                },

            });

        }


    </script>


    <%--End Sudip--%>




    <%--Batch Product Popup Start--%>

    <script>
        function ProductKeyDown(s, e) {
            if (e.htmlEvent.key == "Enter") {

                s.OnButtonClick(0);
            }

        }

        function ProductButnClick(s, e) {
            if (e.buttonIndex == 0) {

                if (!GetObjectID('hdnCustomerId').value) {
                    jAlert("Please Select Customer first.", "Alert", function () { $('#txtCustSearch').focus(); })
                    return;
                }

                if (!isTaxLoaded) {
                    jAlert("Please Wait. System is getting ready.", "Alert", function () { })
                    return;
                }

                $('#txtProdSearch').val('');
                $('#ProductModel').modal('show');
            }
        }

        function SetHsnSac(newHsnSac) {
            newHsnSac = newHsnSac.trim();
            if (newHsnSac != "") {
                var existsHsnSac = $('#hdHsnList').val();
                if (existsHsnSac.indexOf(',' + newHsnSac + ',') == -1) {
                    existsHsnSac = existsHsnSac + newHsnSac + ',';
                    $('#hdHsnList').val(existsHsnSac);
                }
            }
        }

        function RemoveHSnSacFromList(newHsnSac) {
            newHsnSac = newHsnSac.trim();
            if (newHsnSac != "") {
                var existsHsnSac = $('#hdHsnList').val();

                existsHsnSac = existsHsnSac.replace(newHsnSac + ',', '');
                $('#hdHsnList').val(existsHsnSac);

            }
        }

        function ProductSelected(s, e) {
            if (cproductLookUp.GetGridView().GetFocusedRowIndex() == -1) {
                cProductpopUp.Hide();
                grid.batchEditApi.StartEdit(globalRowIndex, 5);
                return;
            }

            var LookUpData = cproductLookUp.GetGridView().GetRowKey(cproductLookUp.GetGridView().GetFocusedRowIndex());
            var ProductCode = cproductLookUp.GetValue();
            if (!ProductCode) {
                LookUpData = null;
            }
            cProductpopUp.Hide();
            grid.batchEditApi.StartEdit(globalRowIndex);
            //Delete hsn
            if (grid.GetEditor("ProductID").GetText() != "") {
                var previousProductId = grid.GetEditor("ProductID").GetText();
                RemoveHSnSacFromList(previousProductId.split("||@||")[19]);
            }


            grid.GetEditor("ProductID").SetText(LookUpData);
            grid.GetEditor("ProductName").SetText(ProductCode);

            pageheaderContent.style.display = "block";
            cddl_AmountAre.SetEnabled(false);

            var tbDescription = grid.GetEditor("Description");
            var tbUOM = grid.GetEditor("UOM");
            var tbSalePrice = grid.GetEditor("SalePrice");

            var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
            var SpliteDetails = ProductID.split("||@||");
            var strProductID = SpliteDetails[0];
            var strDescription = SpliteDetails[1];
            var strUOM = SpliteDetails[2];
            var strStkUOM = SpliteDetails[4];
            var strSalePrice = SpliteDetails[6];
            $('#HDSelectedProduct').val(strProductID);


            var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
            var IsPackingActive = SpliteDetails[10];
            var Packing_Factor = SpliteDetails[11];
            var Packing_UOM = SpliteDetails[12];
            SetHsnSac(SpliteDetails[19]);
            var strRate = (ctxt_Rate.GetValue() != null && ctxt_Rate.GetValue() != "0") ? ctxt_Rate.GetValue() : "1";
            if (strRate == 0) {
                strSalePrice = strSalePrice;
            }
            else {
                strSalePrice = strSalePrice / strRate;
            }

            tbDescription.SetValue(strDescription);
            tbUOM.SetValue(strUOM);
            tbSalePrice.SetValue(strSalePrice);



            var totalNetAmount = grid.GetEditor("TotalAmount").GetValue();

            var newTotalNetAmount = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) - parseFloat(totalNetAmount);
            cbnrlblAmountWithTaxValue.SetValue(parseFloat(Math.round(Math.abs(newTotalNetAmount) * 100) / 100).toFixed(2));
            SetInvoiceLebelValue();


            grid.GetEditor("Quantity").SetValue("0.00");
            grid.GetEditor("Discount").SetValue("0.00");
            grid.GetEditor("Amount").SetValue("0.00");
            grid.GetEditor("TaxAmount").SetValue("0.00");
            grid.GetEditor("TotalAmount").SetValue("0.00");

            var ddlbranch = $("[id*=ddl_Branch]");
            var strBranch = ddlbranch.find("option:selected").text();

            $('#<%= lblStkQty.ClientID %>').text("0.00");
            $('#<%= lblStkUOM.ClientID %>').text(strStkUOM);
            $('#<%= lblProduct.ClientID %>').text(strDescription);
            $('#<%= lblbranchName.ClientID %>').text(strBranch);


            ctaxUpdatePanel.PerformCallback('DelProdbySl~' + grid.GetEditor("SrlNo").GetValue() + '~' + strProductID);
            cbnrOtherChargesvalue.SetText('0.00');
            SetRunningBalance();
            grid.batchEditApi.StartEdit(globalRowIndex, 5);
        }



        function ProductlookUpKeyDown(s, e) {
            if (e.htmlEvent.key == "Escape") {
                cProductpopUp.Hide();
                grid.batchEditApi.StartEdit(globalRowIndex, 5);
            }
        }
    </script>

    <%--Batch Product Popup End--%>

    <%--Compnent Tag Start--%>

    <script>
        function DateCheck() {
            var startDate = new Date();
            startDate = tstartdate.GetValueString();
            //var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());
            var key = GetObjectID('hdnCustomerId').value;
            var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";
            var componentType = gridquotationLookup.GetGridView().GetRowKey(gridquotationLookup.GetGridView().GetFocusedRowIndex());
            if (type != "")
                cQuotationComponentPanel.PerformCallback('DateCheckOnChanged' + '~' + key + '~' + startDate + '~' + '@');

            if (key != null && key != '' && type != "") {
                cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + key + '~' + startDate + '~' + 'DateCheck' + '~' + type);
            }

            if (componentType != null && componentType != '') {
                grid.PerformCallback('GridBlank');
            }

            cdtChallandate.SetDate(tstartdate.GetDate());


            if (GetObjectID('hdnCustomerId').value) {
                var custId = GetObjectID('hdnCustomerId').value;
                caspxCustomerReceiptGridview.PerformCallback('BindCustomerGridByInternalId~' + custId + '~' + tstartdate.GetDate().format('yyyy-MM-dd'));
            }


        }
        function componentEndCallBack(s, e) {
            // gridquotationLookup.gridView.Refresh();
            if (grid.GetVisibleRowsOnPage() == 0) {
                OnAddNewClick(function () { gridquotationLookup.Focus(); });
            }

        }
        function selectValue() {
            var startDate = new Date();
            startDate = tstartdate.GetValueString();
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

            cQuotationComponentPanel.PerformCallback('DateCheckOnChanged' + '~' + key + '~' + startDate + '~' + '@');

            if (key != null && key != '' && type != "") {
                cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + key + '~' + startDate + '~' + 'DateCheck' + '~' + type);
            }

            var componentType = gridquotationLookup.GetValue();
            if (componentType != null && componentType != '') {
                grid.PerformCallback('GridBlank');
            }
        }
        function CloseGridQuotationLookup() {
            gridquotationLookup.ConfirmCurrentSelection();
            gridquotationLookup.HideDropDown();
            gridquotationLookup.Focus();
        }
        function CloseGridVehicleLookup() {
            gridvehicleLookup.ConfirmCurrentSelection();
            gridvehicleLookup.HideDropDown();
            gridvehicleLookup.Focus();
        }


        function QuotationNumberChanged() {
            var quote_Id = gridquotationLookup.GetValue();
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
                        cComponentDatePanel.PerformCallback('BindComponentDate' + '~' + quote_Id);
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
        }
        function ChangeState(value) {

            cgridproducts.PerformCallback('SelectAndDeSelectProducts' + '~' + value);
        }
        function PerformCallToGridBind() {
            grid.PerformCallback('BindGridOnQuotation' + '~' + '@');
            cQuotationComponentPanel.PerformCallback('BindComponentGridOnSelection');
            $('#hdnPageStatus').val('Quoteupdate');
            cProductsPopup.Hide();
            return false;
        }


        function SetRunningBalance() {
            var paymentValue = 0;
            if (document.getElementById('HdPosType').value != 'IST') {
                paymentValue = parseFloat(GetPaymentTotalEnteredAmount());
            }
            //  SetDownPayment();
            if (document.getElementById('HdPosType').value == 'Fin') {
                SetTotalDownPaymentAmount();
            }
            var InvoiceValue = parseFloat(cbnrLblInvValue.GetValue());
            var FinanceAmount = parseFloat(ctxtFinanceAmt.GetValue());
            var otherCharges = parseFloat(cbnrOtherChargesvalue.GetValue());
            var procFee = parseFloat(ctxtprocFee.GetValue());
            var EmiCardOtCharge = parseFloat(ctxtEmiOtherCharges.GetValue());

            var runningBalance = 0;
            runningBalance = parseFloat(Math.round((InvoiceValue - paymentValue - FinanceAmount) * 100) / 100).toFixed(2);

            if (document.getElementById('HdPosType').value == 'Fin') {

                if (parseFloat(Math.round((InvoiceValue - paymentValue - FinanceAmount) * 100) / 100) < parseFloat(ctxtAdvnceReceipt.GetValue())) {
                    if (parseFloat(ctxtAdvnceReceipt.GetValue()) > 0)
                        runningBalance = 0.00;
                    else
                        runningBalance = runningBalance - parseFloat(ctxtAdvnceReceipt.GetValue());
                } else {
                    runningBalance = runningBalance - parseFloat(ctxtAdvnceReceipt.GetValue());
                }
            }



            clblRunningBalanceCapsul.SetValue(runningBalance);
        }
        //--------------------------------Rev Rajdip---------------------------------------
        function CreditDays_TextChanged(s, e) {
            var CreditDays = ctxtCreditDays.GetValue();
            var newdate = new Date();
            var today = new Date();

            today = tstartdate.GetDate();
            today.setDate(today.getDate() + Math.round(CreditDays));

            cdt_SaleInvoiceDue.SetDate(today);
        }
        function CreditDays_LostFocus() {
            var CreditDays = ctxtCreditDays.GetValue();
            var newdate = new Date();
            var today = new Date();

            today = tstartdate.GetDate();
            today.setDate(today.getDate() + Math.round(CreditDays));

            cdt_SaleInvoiceDue.SetDate(today);
            cdt_SaleInvoiceDue.SetEnabled(false);
        }

        function CreditDayspopup_LostFocus(s, e) {
            var CreditDays = ctxtCreditDayspopup.GetValue();
            var newdate = new Date();
            var today = new Date();

            today = tstartdate.GetDate();
            today.setDate(today.getDate() + Math.round(CreditDays));

            cdtDueDate.SetDate(today);
            cdtDueDate.SetEnabled(false);
        }



        function refferencegotfocus() {
            var CreditDays = ctxtCreditDays.GetValue();
            var newdate = new Date();
            var today = new Date();

            today = tstartdate.GetDate();
            today.setDate(today.getDate() + Math.round(CreditDays));

            cdt_SaleInvoiceDue.SetDate(today);

        }
        function ddlSalesAgentgotfocus() {
            // var CreditDays = ctxtCreditDays.GetValue();
            //var newdate = new Date();
            //var today = new Date();

            //today = tstartdate.GetDate();
            //today.setDate(today.getDate() + Math.round(CreditDays));

            //cdt_SaleInvoiceDue.SetDate(today);
            cddl_SalesAgent.ShowDropDown();
        }
        //-------------------------------End Rev Rajdip------------------------------------
    </script>

    <%--Compnent Tag End--%>

    <%--Receipt/Payment Popup Start--%>
    <script>
        function ShowReceiptPayment() {
            uri = "CustomerReceiptPayment.aspx?key=ADD&IsTagged=Y";
            capcReciptPopup.SetContentUrl(uri);
            capcReciptPopup.Show();
        }

    </script>
    <%--Receipt/Payment Popup End--%>

    <script>
        //Code for UDF Control 
        function OpenUdf() {
            if (document.getElementById('IsUdfpresent').value == '0') {
                jAlert("UDF not define.");
            }
            else {
                var keyVal = document.getElementById('Keyval_internalId').value;
                var url = '/OMS/management/Master/frm_BranchUdfPopUp.aspx?Type=POS&&KeyVal_InternalID=' + keyVal;
                popup.SetContentUrl(url);
                popup.Show();
            }
            return true;
        }
        $(window).load(function () {
            localStorage.setItem('LCmini-navbarcss', 'Yes');
        })
        // End Udf Code
    </script>


    <link href="CSS/SearchPopup.css" rel="stylesheet" />
    <style>
        #pageheaderContent .lblHolder {
            min-width: 111px;
        }

        #pageheaderContent {
            width: auto;
            margin-bottom: 7px;
        }
    </style>

    <style>
        /*Rev 1.0*/

        select
        {
            height: 30px !important;
            border-radius: 4px !important;
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
            bottom: 13px;
            right: 10px;
            z-index: 0;
            cursor: pointer;
        }

        #FormDate , #toDate , #dtTDate , #dt_PLQuote , #deliveryDate
        {
            position: relative !important;
            z-index: 1 !important;
            background: transparent;
        }

        .dxeDisabled_PlasticBlue
        {
            z-index: 0 !important;
        }

        #FormDate_B-1 , #toDate_B-1 , #dtTDate_B-1 , #dt_PLQuote_B-1 , #deliveryDate_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #FormDate_B-1 #FormDate_B-1Img , #toDate_B-1 #toDate_B-1Img , #dtTDate_B-1 #dtTDate_B-1Img , #dt_PLQuote_B-1 #dt_PLQuote_B-1Img ,
        #deliveryDate_B-1 #deliveryDate_B-1Img
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
            margin-right: 0px !important;
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
        #GrdQuotation
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

        .dxeDisabled_PlasticBlue, .aspNetDisabled , .dxeDisabled_PlasticBlue
        {
            background: #f3f3f3 !important;
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

        #txtProdSearch
        {
            margin-bottom: 10px;
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

        #massrecdt
        {
            width: 100%;
        }

        /*.col-sm-3 , .col-md-3{
            margin-bottom: 10px;
        }*/

        .crossBtn
        {
            top: 25px;
                right: 25px;
        }

        input[type="text"], input[type="password"], textarea
        {
                margin-bottom: 0;
        }

        .typeNotification span
        {
             color: #ffffff !important;
        }

        #rdl_Salesquotation
        {
            margin-top: 8px;
    line-height: 20px;
        }

        #ASPxLabel8
        {
            line-height: 16px;
        }

        .lblmTop8>span, .lblmTop8>label
        {
                margin-top: 0 !important;
        }

        #OFDBankSelect
        {
            height: 30px;
            border-radius: 4px;
        }

        .mt-28{
            margin-top: 28px;
        }

        .mb-10{
            margin-bottom: 10px;
        }

        /*.col-md-3>label, .col-md-3>span
        {
            margin-top: 0;
        }*/

        #CallbackPanel_LPV
        {
            top: 450px !important;
        }

        /*.GridViewArea
        {
            z-index: 0;
        }*/

        select.btn
        {
            height: 34px !important;
        }

        .makeFullscreen >table
        {
            z-index: 0;
        }
        .makeFullscreen .makeFullscreen-icon.half
        {
                z-index: 0;
        }

        .lblmBot4 > span, .lblmBot4 > label
        {
                margin-bottom: 0px !important;
        }

        .col-md-3>label, .col-md-3>span
        {
            margin-top: 0px !important;
        }

        .btn
        {
            padding: 5px 10px;
        }
        .mbot5 .col-md-8 {
    margin-bottom: 5px;
}

        .wrapHolder
        {
            height: auto;
        }

        #idOutstanding
        {
            min-height: 18px !important;
        }

        .bod-table > tbody > tr > td
        {
            background: #fff !important;
        }
        .dxeDisabled_PlasticBlue
        {
            background: #f3f3f3 !important;
        }
        .dxeButtonDisabled_PlasticBlue
        {
            background: #b5b5b5 !important;
            border-color: #b5b5b5 !important;
        }
        

        /*Rev end 1.0*/
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="JS/PosSalesInvoice.js?var=2.6"></script>
    <%--<script src="JS/SearchPopup.js?var=1.0"></script>--%>
    <script src="JS/SearchPopupDatatable.js"></script>
    <script src="UserControls/Js/ucPaymentDetails.js?var=1.0"></script>

    <asp:HiddenField ID="hfVSFileName" runat="server" />
    <%--Rev 2.0: "outer-div-main" class add --%>
    <div class="outer-div-main clearfix">
        <div class="panel-title clearfix" id="myDiv">
        <h3 class="pull-left">
            <asp:Label ID="lblHeadTitle" Text="" runat="server"></asp:Label>
            <%--<label>Add Proforma Invoice/ Quotation</label>--%>
            <%--<div class="pull-right" style="font-size: 14px;"> Login into: <span class="backBranch"></span></div>--%>

        </h3>


        <div id="pageheaderContent" class=" pull-right wrapHolder content horizontal-images">
            <div class="Top clearfix">
                <ul>

                    <li>
                        <div class="lblHolder" id="divDues">
                            <table>
                                <tr>
                                    <td>Customer Balance</td>
                                </tr>
                                <tr>
                                    <td>
                                        <div id="idOutstanding">
                                            <asp:Label ID="lblOutstanding" runat="server" ToolTip="Click here to show details."></asp:Label>
                                        </div>
                                    </td>
                                </tr>
                                <%--                                <tr>
                                    <td class="lower">
                                        <asp:Label ID="lblTotalDues" runat="server"></asp:Label>
                                    </td>
                                </tr>--%>
                            </table>
                        </div>
                    </li>


                    <li>
                        <div class="lblHolder" id="idCashbalanace">
                            <table>
                                <tbody>
                                    <tr>
                                        <td>Cash Balance </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div style="width: 100%;">
                                                <b style="text-align: center" id="B_BankBalance" runat="server">0.00</b>

                                            </div>

                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </li>

                    <li style="cursor: pointer">
                        <div class="lblHolder" id="divAvailableStk" onclick="AvailableStockClick()">
                            <table>
                                <tr>
                                    <td>Available Stock</td>
                                </tr>
                                <tr>
                                    <td style="color: blue">
                                        <asp:Label ID="lblAvailableStk" runat="server" Text="0.0"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </li>


                    <li>
                        <div class="lblHolder" id="divinvoiced">
                            <table>
                                <tbody>
                                    <tr>
                                        <td>Currently Invoiced</td>
                                    </tr>
                                    <tr>
                                        <td>

                                            <asp:Label ID="lblInvoiced" runat="server" Text="0.0"></asp:Label>

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
                                        <td>Actual Stock</td>
                                    </tr>
                                    <tr>
                                        <td>

                                            <asp:Label ID="lblActStock" runat="server" Text="0.0"></asp:Label>

                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </li>


                    <li class="hide">

                        <div class="lblHolder " id="divPacking" style="display: none;">
                            <table>
                                <tr>
                                    <td>Packing Quantity</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblPackingStk" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </li>
                </ul>
                <ul style="display: none;">
                    <li>
                        <div class="lblHolder">
                            <table>
                                <tr>
                                    <td>Selected Branch</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblbranchName" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </li>
                    <li>
                        <div class="lblHolder">
                            <table>
                                <tr>
                                    <td>Selected Product</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblProduct" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </li>
                    <li>
                        <div class="lblHolder">
                            <table>
                                <tr>
                                    <td>Stock Quantity</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblStkQty" runat="server" Text="0.00"></asp:Label>
                                        <asp:Label ID="lblStkUOM" runat="server" Text=" "></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </li>
                </ul>
            </div>
        </div>
        <div id="ApprovalCross" runat="server" class="crossBtn"><a href=""><i class="fa fa-times"></i></a></div>
        <div id="divcross" runat="server" class="crossBtn"><a href="PosSalesInvoiceList.aspx"><i class="fa fa-times"></i></a></div>

    </div>
        <div class="form_main">
        <asp:Panel ID="pnl_quotation" runat="server">
            <div class="">
                <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ClientInstanceName="page" Width="100%">
                    <TabPages>
                        <dxe:TabPage Name="General" Text="General">
                            <ContentCollection>
                                <dxe:ContentControl runat="server">
                                    <div class="">
                                        <div style="">
                                            <table class="bod-table">
                                                <tbody>
                                                    <tr>

                                                        <td id="divScheme" runat="server" style="padding-top: 10px; width: 177px">
                                                            <dxe:ASPxLabel ID="lbl_NumberingScheme" runat="server" Text="Numbering Scheme">
                                                            </dxe:ASPxLabel>
                                                            <div class="simple-select">
                                                                <asp:DropDownList ID="ddl_numberingScheme" runat="server" Width="100%">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </td>

                                                        <td class="relative">
                                                            <dxe:ASPxLabel ID="lbl_SaleInvoiceNo" runat="server" Text="Document No.">
                                                            </dxe:ASPxLabel>
                                                            <dxe:ASPxTextBox ID="txt_PLQuoteNo" runat="server" ClientInstanceName="ctxt_PLQuoteNo" Width="100%">
                                                                <ClientSideEvents TextChanged="function(s, e) {UniqueCodeCheck();}" />
                                                            </dxe:ASPxTextBox>
                                                            <span id="MandatorysQuoteno" style="display: none" class="errorField">
                                                                <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"> </img>
                                                            </span><span id="duplicateQuoteno" class="validclass" style="display: none">
                                                                <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Duplicate number"> </img>
                                                            </span></td>

                                                        <td class="relative">
                                                            <dxe:ASPxLabel ID="lbl_SaleInvoiceDt" runat="server" Text="Posting Date">
                                                            </dxe:ASPxLabel>
                                                            <dxe:ASPxDateEdit ID="dt_PLQuote" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="tstartdate" Width="100%">
                                                                <ButtonStyle Width="13px">
                                                                </ButtonStyle>
                                                                <ClientSideEvents DateChanged="function(s, e) {DateCheck();}" GotFocus="function(s,e){tstartdate.ShowDropDown();}" LostFocus="function(s, e) { SetLostFocusonDemand(e)}"  />
                                                            </dxe:ASPxDateEdit>
                                                            <%--Rev 1.0--%>
                                                            <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                                                            <%--Rev end 1.0--%>
                                                        </td>

                                                        <td class="relative" id="tdDeliveryType" runat="server">
                                                            <dxe:ASPxLabel ID="ASPxLabel4" runat="server" Text="Delivery Type">
                                                            </dxe:ASPxLabel>
                                                            <dxe:ASPxComboBox ID="cmbDeliveryType" ClientInstanceName="ccmbDeliveryType" runat="server"
                                                                ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0">
                                                                <Items>
                                                                    <%--<dxe:ListEditItem Text="-Select-" Value="0" />--%>
                                                                    <dxe:ListEditItem Text="Our" Value="O" />
                                                                    <dxe:ListEditItem Text="Self" Value="S" />
                                                                    <dxe:ListEditItem Text="Already Delivered" Value="D" />
                                                                    <%--<dxe:ListEditItem Text="Intimation Approx" Value="I" />--%>
                                                                </Items>
                                                                <ClientSideEvents SelectedIndexChanged="isDeliveryTypeChanged" GotFocus="function(s,e){ccmbDeliveryType.ShowDropDown();}" />
                                                            </dxe:ASPxComboBox>

                                                            <span id="mandetorydeliveryType" style="display: none;" class="errorField">
                                                                <img id="mandetorydelivery" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                                            </span>
                                                        </td>


                                                        <td class="relative">
                                                            <dxe:ASPxLabel ID="ASPxLabel10" runat="server" Text="Delivery Date">
                                                            </dxe:ASPxLabel>
                                                            <dxe:ASPxDateEdit ID="deliveryDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cdeliveryDate" Width="100%">
                                                                <ButtonStyle Width="13px">
                                                                </ButtonStyle>
                                                                <ClientSideEvents GotFocus="function(s,e){cdeliveryDate.ShowDropDown();}" />
                                                            </dxe:ASPxDateEdit>
                                                            <span id="MandatorysdeliveryDate" style="display: none" class="errorField">
                                                                <img id="MandatorysdeliveryDateid" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"> </img>
                                                            </span>
                                                            <%--Rev 1.0--%>
                                                            <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                                                            <%--Rev end 1.0--%>
                                                        </td>



                                                        <td class="relative">
                                                            <dxe:ASPxLabel ID="lbl_Customer" runat="server" Text="Customer">
                                                            </dxe:ASPxLabel>
                                                            <% if (rights.CanAdd && hdAddOrEdit.Value != "Edit")
                                                               { %>
                                                            <a href="#" onclick="AddcustomerClick()" style="position: absolute; top: 4px; margin-left: 5px;"><i id="openlink" class="fa fa-plus-circle" aria-hidden="true"></i></a>
                                                            <% } %>

                                                            <dxe:ASPxButtonEdit ID="txtCustName" runat="server" ReadOnly="true" ClientInstanceName="ctxtCustName" Width="100%">

                                                                <Buttons>
                                                                    <dxe:EditButton>
                                                                    </dxe:EditButton>

                                                                </Buttons>
                                                                <ClientSideEvents ButtonClick="function(s,e){CustomerButnClick();}" KeyDown="function(s,e){CustomerKeyDown(s,e);}" />
                                                            </dxe:ASPxButtonEdit>






                                                            <span id="MandatorysCustomer" style="display: none" class="errorField">
                                                                <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>



                                                        </td>
                                                        <td style="width: 180px">
                                                            <dxe:ASPxLabel ID="ASPxLabel3" runat="server" Text="Salesman(ISD)">
                                                            </dxe:ASPxLabel>
                                                            <dxe:ASPxComboBox ID="ddl_SalesAgent" ClientInstanceName="cddl_SalesAgent" runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True"
                                                                OnCallback="ddl_SalesAgent_Callback" EnableIncrementalFiltering="True" SelectedIndex="0">
                                                                <%-- Rev Rajdip --%>
                                                                <%--<ClientSideEvents GotFocus="function(s,e){cddl_SalesAgent.ShowDropDown();}" EndCallback=" OnSalesAgentEndCallback" />--%>
                                                                <ClientSideEvents GotFocus="ddlSalesAgentgotfocus" EndCallback=" OnSalesAgentEndCallback" />
                                                                <%-- End Rev Rajdip --%>
                                                            </dxe:ASPxComboBox>
                                                        </td>
                                                    </tr>
                                                    <tr>

                                                        <td>
                                                            <dxe:ASPxLabel ID="lbl_Refference" runat="server" Text="Reference">
                                                            </dxe:ASPxLabel>
                                                            <dxe:ASPxTextBox ID="txt_Refference" ClientInstanceName="ctxt_Refference" runat="server" Width="100%" ClientSideEvents-GotFocus="refferencegotfocus">
                                                            </dxe:ASPxTextBox>

                                                        </td>
                                                        <%-- Rev Rajdip --%>
                                                        <td id="Creditdatediv" runat="server">
                                                            <dxe:ASPxLabel ID="ASPxLabel13" runat="server" Text="Credit Days">
                                                            </dxe:ASPxLabel>
                                                            <dxe:ASPxTextBox ID="txtCreditDays" ClientInstanceName="ctxtCreditDays" runat="server" Width="100%">
                                                                <MaskSettings Mask="<0..999999999>" AllowMouseWheel="false" />
                                                                <ClientSideEvents TextChanged="CreditDays_TextChanged" LostFocus="CreditDays_LostFocus" />
                                                            </dxe:ASPxTextBox>
                                                        </td>
                                                        <td id="DueDatediv" runat="server">
                                                            <dxe:ASPxLabel ID="lbl_DueDate" runat="server" Text="Due Date">
                                                            </dxe:ASPxLabel>
                                                            <dxe:ASPxDateEdit ID="dt_SaleInvoiceDue" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cdt_SaleInvoiceDue" Width="100%">
                                                                <ButtonStyle Width="13px">
                                                                </ButtonStyle>
                                                                <ClientSideEvents GotFocus="function(s,e){cdt_SaleInvoiceDue.ShowDropDown();}" />
                                                            </dxe:ASPxDateEdit>
                                                        </td>
                                                        <%-- End Rev Rajdip --%>
                                                        <td id="challanNoSchemedd" runat="server" colspan="1">
                                                            <dxe:ASPxLabel ID="ASPxLabel9" runat="server" Text="Challan Numbering Scheme">
                                                            </dxe:ASPxLabel>
                                                            <div>
                                                                <dxe:ASPxComboBox ID="challanNoScheme" runat="server" ClientEnabled="false" ClientInstanceName="cchallanNoScheme" OnCallback="challanNoScheme_Callback" Width="100%">
                                                                    <ClientSideEvents EndCallback="challanNoSchemeEndCallback" SelectedIndexChanged="challanNoSchemeSelectedIndexChanged"
                                                                        GotFocus="function(s,e){cchallanNoScheme.ShowDropDown();}" />
                                                                </dxe:ASPxComboBox>
                                                            </div>
                                                            <span>
                                                                <img id="mandetorydchallanNoScheme" style="display: none" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                                            </span>
                                                        </td>

                                                        <td>
                                                            <label>Challan No </label>
                                                            <dxe:ASPxTextBox ID="txtChallanNo" ClientInstanceName="ctxtChallanNo" runat="server" Width="100%" ClientEnabled="false">
                                                            </dxe:ASPxTextBox>
                                                            <span>
                                                                <img id="mandetorydtxtChallanNo" style="display: none" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                                            </span>
                                                        </td>
                                                        <td>
                                                            <label>Challan Date</label>
                                                            <dxe:ASPxDateEdit ID="dtChallandate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cdtChallandate" ClientEnabled="false" Width="100%">
                                                                <ButtonStyle Width="13px">
                                                                </ButtonStyle>
                                                            </dxe:ASPxDateEdit>
                                                            <%--Rev 1.0--%>
                                                            <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                                                            <%--Rev end 1.0--%>
                                                        </td>


                                                        <td>
                                                            <asp:RadioButtonList ID="rdl_SaleInvoice" runat="server" RepeatDirection="Horizontal" onchange="return selectValue();">
                                                                <asp:ListItem Text="PI/Quotation" Value="QO"></asp:ListItem>
                                                                <asp:ListItem Text="Order" Value="SO"></asp:ListItem>
                                                                <%--  <asp:ListItem Text="Challan" Value="SC"></asp:ListItem>--%>
                                                            </asp:RadioButtonList>
                                                            <dxe:ASPxCallbackPanel runat="server" ID="ComponentQuotationPanel" ClientInstanceName="cQuotationComponentPanel" OnCallback="ComponentQuotation_Callback">
                                                                <PanelCollection>
                                                                    <dxe:PanelContent runat="server">
                                                                        <dxe:ASPxGridLookup ID="lookup_quotation" SelectionMode="Multiple" runat="server" ClientInstanceName="gridquotationLookup"
                                                                            OnDataBinding="lookup_quotation_DataBinding"
                                                                            KeyFieldName="ComponentID" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                                                            <Columns>
                                                                                <dxe:GridViewCommandColumn VisibleIndex="0" Width="60" Caption=" " />
                                                                                <dxe:GridViewDataColumn FieldName="ComponentNumber" Visible="true" VisibleIndex="1" Caption="Document Number" Width="180" Settings-AutoFilterCondition="Contains">
                                                                                    <Settings AutoFilterCondition="Contains" />
                                                                                </dxe:GridViewDataColumn>
                                                                                <dxe:GridViewDataColumn FieldName="ComponentDate" Visible="true" VisibleIndex="2" Caption="Posting Date" Width="100" Settings-AutoFilterCondition="Contains">
                                                                                    <Settings AutoFilterCondition="Contains" />
                                                                                </dxe:GridViewDataColumn>
                                                                                <dxe:GridViewDataColumn FieldName="CustomerName" Visible="true" VisibleIndex="3" Caption="Customer Name" Width="150" Settings-AutoFilterCondition="Contains">
                                                                                    <Settings AutoFilterCondition="Contains" />
                                                                                </dxe:GridViewDataColumn>
                                                                            </Columns>
                                                                            <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                                                                <Templates>
                                                                                    <StatusBar>
                                                                                        <table class="OptionsTable" style="float: right">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <dxe:ASPxButton ID="ASPxButton9" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridQuotationLookup" UseSubmitBehavior="False" />
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </StatusBar>
                                                                                </Templates>
                                                                                <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" />
                                                                                <SettingsPager Mode="ShowAllRecords">
                                                                                </SettingsPager>
                                                                                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                                                            </GridViewProperties>
                                                                            <ClientSideEvents ValueChanged="function(s, e) { QuotationNumberChanged();}" GotFocus="function(s,e){gridquotationLookup.ShowDropDown();}" />
                                                                        </dxe:ASPxGridLookup>
                                                                    </dxe:PanelContent>
                                                                </PanelCollection>
                                                                <ClientSideEvents EndCallback="componentEndCallBack" />
                                                            </dxe:ASPxCallbackPanel>
                                                        </td>
                                                        <td></td>


                                                        <%--  <td>
                                                         <select class="form-control">
                                                             <option>Select Barcode</option>
                                                             <option>Select Model</option>
                                                             <option>Select Serial</option>
                                                         </select>
                                                     </td>
                                                     <td>
                                                         <div class="input-group">
                                                             <input type="text" class="form-control" placeholder="Username" aria-describedby="basic-addon1">
                                                             <span class="input-group-addon btn-primary" style="padding: 5px;"><i class="fa fa-plus-circle" aria-hidden="true"></i></span>
                                                        </div>
                                                     </td>--%>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <div class="" id="divposGst">
                                                                <dxe:ASPxLabel ID="lbl_PosForGst" runat="server" Text="Place Of Supply [GST]">
                                                                </dxe:ASPxLabel>
                                                                <span style="color: red">*</span>
                                                                <dxe:ASPxComboBox ID="ddl_PosGst" runat="server" ValueType="System.String" Width="100%" ClientInstanceName="cddl_PosGst" TabIndex="18">
                                                                    <ClientSideEvents SelectedIndexChanged="function(s, e) { PopulatePosGst(e)}" />
                                                                </dxe:ASPxComboBox>
                                                            </div>
                                                            </td>
                                                        <td>                                          
                                                            <dxe:ASPxLabel ID="ASPxLabel14" runat="server" Text="Transaction Category">
                                                            </dxe:ASPxLabel>
                                                            <div class="simple-select">
                                                            <asp:DropDownList ID="drdTransCategory" runat="server" Width="100%">
                                                                <asp:ListItem  Selected="True" Text="Select" Value="0"></asp:ListItem>
                                                                <asp:ListItem Text="B2B" Value="B2B" />
                                                                <asp:ListItem Text="SEZWP" Value="SEZWP" />
                                                                <asp:ListItem Text="SEZWOP" Value="SEZWOP" />
                                                                <asp:ListItem Text="EXPWP" Value="EXPWP" />
                                                                <asp:ListItem Text="EXPWOP" Value="EXPWOP" />
                                                                <asp:ListItem Text="DEXP" Value="DEXP" />
                                                            </asp:DropDownList>
                                                            </div>
                                                        </td>                                                 
                                                        <td>
                                                             <asp:CheckBox ID="CB_ReverseCharge" runat="server" Text="Reverse Charge" TextAlign="Right" Checked="false"></asp:CheckBox>
                                                        </td>
                                                        <td>
                                                            <dxe:ASPxLabel ID="lblVatGstCst" runat="server" Text="Select GST" ClientVisible="false">
                                                            </dxe:ASPxLabel>
                                                            <dxe:ASPxComboBox ID="ddl_VatGstCst" runat="server" ClientInstanceName="cddlVatGstCst" ClientVisible="false" OnCallback="ddl_VatGstCst_Callback" Width="100%">
                                                                <ClientSideEvents EndCallback="Onddl_VatGstCstEndCallback" />
                                                            </dxe:ASPxComboBox>
                                                            <span id="Mandatorytaxcode" style="display: none" class="validclass">
                                                                <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                                                        </td>
                                                        
                                                        
                                                        <td></td>
                                                        <td></td>
                                                        <td></td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                        <table class="bod-table " style="margin-top: 5px" id="FinancerTable" runat="server">
                                            <tbody>
                                                <tr>
                                                    <td>
                                                        <label>Financer</label>
                                                        <dxe:ASPxComboBox ID="cmbFinancer" runat="server" ClientInstanceName="ccmbFinancer" Width="100%" OnCallback="cmbFinancer_Callback">
                                                            <ClientSideEvents SelectedIndexChanged="financerIndexChange" GotFocus="function(s,e){ccmbFinancer.ShowDropDown();}" EndCallback="OnfinancerEndCallback" />
                                                        </dxe:ASPxComboBox>
                                                    </td>
                                                    <td>
                                                        <label>Exec. Name</label>
                                                        <dxe:ASPxComboBox ID="cmbExecName" runat="server" ClientInstanceName="ccmbExecName" OnCallback="cmbExecName_Callback" Width="100%">
                                                            <ClientSideEvents GotFocus="function(s,e){ccmbExecName.ShowDropDown();}" EndCallback="ccmbExecNameEndCallBack" />
                                                        </dxe:ASPxComboBox>
                                                    </td>
                                                    <td>
                                                        <label>EMI Det.</label>
                                                        <dxe:ASPxTextBox ID="txtEmiDetails" ClientInstanceName="ctxtEmiDetails" runat="server" Width="100%" MaxLength="100">
                                                        </dxe:ASPxTextBox>
                                                    </td>

                                                    <td>
                                                        <label>Scheme</label>
                                                        <dxe:ASPxTextBox ID="txtScheme" ClientInstanceName="ctxtScheme" runat="server" Width="100%" MaxLength="200">
                                                        </dxe:ASPxTextBox>
                                                    </td>

                                                    <td>
                                                        <label>SF Code</label>
                                                        <dxe:ASPxTextBox ID="txtSfCode" ClientInstanceName="ctxtSfCode" runat="server" Width="100%">
                                                        </dxe:ASPxTextBox>
                                                    </td>

                                                    <td>
                                                        <label>DBD</label>
                                                        <dxe:ASPxTextBox ID="txtDBD" ClientInstanceName="ctxtDBD" runat="server" Width="100%">
                                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                        </dxe:ASPxTextBox>
                                                    </td>

                                                    <td>
                                                        <label>DBD %</label>
                                                        <dxe:ASPxTextBox ID="txtDbdPercen" ClientInstanceName="ctxtDbdPercen" runat="server" Width="100%">
                                                            <MaskSettings Mask="<0..100>.<0..99>" AllowMouseWheel="false" />
                                                        </dxe:ASPxTextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label>Downpayment</label>
                                                        <dxe:ASPxTextBox ID="txtdownPayment" ClientInstanceName="ctxtdownPayment" runat="server" Width="100%">
                                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                            <ClientSideEvents LostFocus="SetRunningBalance"></ClientSideEvents>
                                                        </dxe:ASPxTextBox>
                                                    </td>

                                                    <td>
                                                        <label>Proc. Fee</label>
                                                        <dxe:ASPxTextBox ID="txtprocFee" ClientInstanceName="ctxtprocFee" runat="server" Width="100%">
                                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                            <ClientSideEvents LostFocus="SetRunningBalance"></ClientSideEvents>
                                                        </dxe:ASPxTextBox>
                                                    </td>


                                                    <td>
                                                        <label>EMI Card/Other Charges</label>
                                                        <dxe:ASPxTextBox ID="txtEmiOtherCharges" ClientInstanceName="ctxtEmiOtherCharges" runat="server" Width="100%">
                                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                            <ClientSideEvents LostFocus="SetRunningBalance"></ClientSideEvents>
                                                        </dxe:ASPxTextBox>
                                                    </td>

                                                    <td>
                                                        <label>Total DP Amt.</label>
                                                        <dxe:ASPxTextBox ID="txtTotDpAmt" ClientInstanceName="ctxtTotDpAmt" runat="server" Width="100%" ClientEnabled="false">
                                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                        </dxe:ASPxTextBox>
                                                    </td>


                                                    <td>
                                                        <label>Financer Due</label>
                                                        <dxe:ASPxTextBox ID="txtFinanceAmt" ClientInstanceName="ctxtFinanceAmt" runat="server" Width="100%" ClientEnabled="false">
                                                            <MaskSettings Mask="&lt;-999999999..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                            <ClientSideEvents LostFocus="SetRunningBalance"></ClientSideEvents>
                                                        </dxe:ASPxTextBox>
                                                    </td>

                                                    <td>
                                                        <label>Finance Challan No</label>
                                                        <dxe:ASPxTextBox ID="txtfinChallanNo" ClientInstanceName="ctxtfinChallanNo" runat="server" Width="100%" MaxLength="10">
                                                        </dxe:ASPxTextBox>
                                                    </td>


                                                    <td>
                                                        <label>Finance Challan Date</label>
                                                        <dxe:ASPxDateEdit ID="finChallandate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cfinChallandate" Width="100%">
                                                            <ButtonStyle Width="13px">
                                                            </ButtonStyle>
                                                        </dxe:ASPxDateEdit>
                                                    </td>


                                                </tr>

                                            </tbody>


                                        </table>

                                        <div style="margin-top: 8px;">
                                            <dxe:ASPxGridView runat="server" KeyFieldName="QuotationID" OnCustomUnboundColumnData="grid_CustomUnboundColumnData" ClientInstanceName="grid" ID="grid"
                                                Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" Settings-ShowFooter="false"
                                                OnBatchUpdate="grid_BatchUpdate"
                                                OnCustomCallback="grid_CustomCallback"
                                                OnDataBinding="grid_DataBinding"
                                                OnCellEditorInitialize="grid_CellEditorInitialize"
                                                OnRowInserting="Grid_RowInserting"
                                                OnRowUpdating="Grid_RowUpdating"
                                                OnRowDeleting="Grid_RowDeleting"
                                                OnHtmlRowPrepared="grid_HtmlRowPrepared"
                                                SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="120">
                                                <SettingsPager Visible="false"></SettingsPager>
                                                <Columns>
                                                    <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="3%" VisibleIndex="0" Caption="#">
                                                        <CustomButtons>
                                                            <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomDelete" Image-Url="/assests/images/crs.png">
                                                                <Image Url="/assests/images/crs.png">
                                                                </Image>
                                                            </dxe:GridViewCommandColumnCustomButton>
                                                        </CustomButtons>
                                                        <%-- <HeaderCaptionTemplate>
                                                            <dxe:ASPxHyperLink ID="btnNew" runat="server" Text="New" ForeColor="White">
                                                                <ClientSideEvents Click="function (s, e) { OnAddNewClick();}" />
                                                            </dxe:ASPxHyperLink>
                                                        </HeaderCaptionTemplate>--%>
                                                    </dxe:GridViewCommandColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="SrlNo" Caption="Sl" ReadOnly="true" VisibleIndex="1" Width="2%">
                                                        <PropertiesTextEdit>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="ComponentNumber" Caption="Doc No." VisibleIndex="2" ReadOnly="True" Width="9%">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataButtonEditColumn FieldName="ProductName" Caption="Product" VisibleIndex="3" Width="14%">
                                                        <PropertiesButtonEdit>
                                                            <ClientSideEvents ButtonClick="ProductButnClick" KeyDown="ProductKeyDown" GotFocus="ProductsGotFocusFromID" />
                                                            <Buttons>
                                                                <dxe:EditButton Text="..." Width="20px">
                                                                </dxe:EditButton>
                                                            </Buttons>
                                                        </PropertiesButtonEdit>
                                                    </dxe:GridViewDataButtonEditColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="Description" Caption="Description" VisibleIndex="4" ReadOnly="True" Width="15%">
                                                        <CellStyle Wrap="True"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="Quantity" Caption="Qty." VisibleIndex="5" Width="6%" PropertiesTextEdit-MaxLength="14" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesTextEdit DisplayFormatString="0.0000" Style-HorizontalAlign="Right">
                                                            <ClientSideEvents GotFocus="QuantityGotFocus" LostFocus="QuantityTextChange"></ClientSideEvents>
                                                            <Style HorizontalAlign="Right"></Style>
                                                            <MaskSettings AllowMouseWheel="False" Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" />
                                                        </PropertiesTextEdit>
                                                        <HeaderStyle HorizontalAlign="Right" />
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="UOM" Caption="UOM" VisibleIndex="6" ReadOnly="true" Width="3%"
                                                        PropertiesTextEdit-ClientSideEvents-GotFocus="UOMGotFocus">
                                                        <PropertiesTextEdit>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>
                                                    <%--Caption="Warehouse"--%>
                                                    <dxe:GridViewCommandColumn VisibleIndex="7" Caption="Stock" Width="4%">
                                                        <CustomButtons>
                                                            <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomWarehouse" Image-Url="/assests/images/warehouse.png" Image-ToolTip="Warehouse">
                                                                <Image ToolTip="Warehouse" Url="/assests/images/warehouse.png">
                                                                </Image>
                                                            </dxe:GridViewCommandColumnCustomButton>
                                                        </CustomButtons>
                                                    </dxe:GridViewCommandColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="StockQuantity" Caption="Stock Qty" VisibleIndex="8" Visible="false">
                                                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                                        <PropertiesTextEdit>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="StockUOM" Caption="Stock UOM" VisibleIndex="9" ReadOnly="true" Visible="false">
                                                        <PropertiesTextEdit>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="SalePrice" Caption="Sale Price" VisibleIndex="10" Width="6%" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                            <ClientSideEvents LostFocus="SalePriceTextChange" GotFocus="ProductsGotFocus" />
                                                        </PropertiesTextEdit>
                                                        <HeaderStyle HorizontalAlign="Right" />
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataSpinEditColumn FieldName="Discount" Caption="Disc(%)" VisibleIndex="11" Width="5%" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesSpinEdit MinValue="0" MaxValue="100" AllowMouseWheel="false" DisplayFormatString="0.00" MaxLength="6" Style-HorizontalAlign="Right">
                                                            <SpinButtons ShowIncrementButtons="false"></SpinButtons>
                                                            <ClientSideEvents LostFocus="DiscountTextChange" GotFocus="DiscountGotChange" />
                                                            <Style HorizontalAlign="Right"></Style>
                                                        </PropertiesSpinEdit>
                                                        <HeaderStyle HorizontalAlign="Right" />
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataSpinEditColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="Amount" Caption="Amount" VisibleIndex="12" Width="6%" ReadOnly="true" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                            <MaskSettings AllowMouseWheel="False" Mask="&lt;0..999999999&gt;.&lt;00..99&gt;"></MaskSettings>
                                                            <Style HorizontalAlign="Right"></Style>
                                                        </PropertiesTextEdit>
                                                        <HeaderStyle HorizontalAlign="Right" />
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataButtonEditColumn FieldName="TaxAmount" Caption="Tax/Charges" VisibleIndex="13" Width="6%" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesButtonEdit>
                                                            <ClientSideEvents ButtonClick="taxAmtButnClick" GotFocus="taxAmtButnClick1" KeyDown="TaxAmountKeyDown" />

                                                            <Buttons>
                                                                <dxe:EditButton Text="..." Width="20px">
                                                                </dxe:EditButton>
                                                            </Buttons>
                                                        </PropertiesButtonEdit>

                                                        <HeaderStyle HorizontalAlign="Right" />
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataButtonEditColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="TotalAmount" Caption="Net Amount" VisibleIndex="14" Width="6%" ReadOnly="true" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                            <MaskSettings AllowMouseWheel="False" Mask="&lt;0..999999999&gt;.&lt;00..99&gt;"></MaskSettings>

                                                            <Style HorizontalAlign="Right"></Style>
                                                        </PropertiesTextEdit>

                                                        <HeaderStyle HorizontalAlign="Right" />
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="2.5%" VisibleIndex="15" Caption=" ">
                                                        <CustomButtons>
                                                            <dxe:GridViewCommandColumnCustomButton Text=" " ID="AddNew" Image-Url="/assests/images/add.png">
                                                                <Image Url="/assests/images/add.png">
                                                                </Image>
                                                            </dxe:GridViewCommandColumnCustomButton>

                                                        </CustomButtons>
                                                    </dxe:GridViewCommandColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="ComponentID" Caption="Component ID" VisibleIndex="16" ReadOnly="True" Width="0">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="TotalQty" Caption="Total Qty" VisibleIndex="18" ReadOnly="True" Width="0">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="BalanceQty" Caption="Balance Qty" VisibleIndex="17" ReadOnly="True" Width="0">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="IsComponentProduct" Caption="IsComponentProduct" VisibleIndex="19" ReadOnly="True" Width="0">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="IsLinkedProduct" Caption="IsLinkedProduct" VisibleIndex="20" ReadOnly="True" Width="0">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn CellStyle-Wrap="False" CellStyle-CssClass="hide" EditCellStyle-Wrap="False" FieldName="ProductID" PropertiesTextEdit-ValidationSettings-ErrorImage-IconID="ghg" Caption="hidden Field Id" ReadOnly="True" Width="0" PropertiesTextEdit-Height="15px" PropertiesTextEdit-Style-CssClass="abcd">
                                                        <PropertiesTextEdit Height="15px" Style-Wrap="False">
                                                            <ValidationSettings>
                                                                <ErrorImage IconID="ghg"></ErrorImage>
                                                            </ValidationSettings>

                                                            <Style CssClass="abcd"></Style>
                                                        </PropertiesTextEdit>
                                                        <CellStyle Wrap="True" CssClass="abcd"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="DocDetailsID" Caption="Doc Details ID" ReadOnly="True" Width="0">
                                                    </dxe:GridViewDataTextColumn>
                                                </Columns>
                                                <ClientSideEvents EndCallback="OnEndCallback" CustomButtonClick="OnCustomButtonClick" RowClick="GetVisibleIndex"
                                                    BatchEditStartEditing="gridFocusedRowChanged" />
                                                <SettingsDataSecurity AllowEdit="true" />
                                                <SettingsEditing Mode="Batch" NewItemRowPosition="Bottom">
                                                    <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="row" />
                                                </SettingsEditing>
                                                <Settings VerticalScrollableHeight="120" VerticalScrollBarMode="Auto" />
                                                <SettingsBehavior ColumnResizeMode="Disabled" />
                                            </dxe:ASPxGridView>
                                        </div>
                                        <div style="background: #f5f4f3; padding: 8px 0; margin-bottom: 0px; border-radius: 4px; border: 1px solid #ccc;" class="clearfix col-md-12 hide">


                                            <div class="col-md-3 hide">
                                                <dxe:ASPxLabel ID="lbl_Branch" runat="server" Text="Unit">
                                                </dxe:ASPxLabel>
                                                <asp:DropDownList ID="ddl_Branch" runat="server" Width="100%">
                                                </asp:DropDownList>
                                            </div>




                                            <div class="col-md-2 hide">
                                                <dxe:ASPxLabel ID="ASPxLabel8" runat="server" Text="Delivered From">
                                                </dxe:ASPxLabel>
                                                <asp:DropDownList ID="ddDeliveredFrom" runat="server" Width="100%">
                                                </asp:DropDownList>
                                            </div>

                                            <div style="clear: both">
                                            </div>

                                            <div class="col-md-3 hide">
                                                <dxe:ASPxLabel ID="lbl_ContactPerson" runat="server" Text="Contact Person">
                                                </dxe:ASPxLabel>
                                                <dxe:ASPxComboBox ID="cmbContactPerson" runat="server" OnCallback="cmbContactPerson_Callback" ClientSideEvents-EndCallback="cmbContactPersonEndCall" Width="100%" ClientInstanceName="cContactPerson" Font-Size="12px">
                                                    <ClientSideEvents EndCallback="cmbContactPersonEndCall" />
                                                </dxe:ASPxComboBox>
                                            </div>

                                            <div class="col-md-3">
                                                <%-- <dxe:ASPxLabel ID="ASPxLabel3" runat="server" Text="Salesman/Agents">
                                                </dxe:ASPxLabel>
                                                <asp:DropDownList ID="ddl_SalesAgent" runat="server" Width="100%" >
                                                </asp:DropDownList>--%>
                                            </div>
                                            <div style="clear: both">
                                            </div>
                                            <div class="col-md-3 ">
                                            </div>
                                            <div class="col-md-3 hide">
                                                <dxe:ASPxLabel ID="lbl_InvoiceNO" ClientInstanceName="clbl_InvoiceNO" runat="server" Text="Posting Date">
                                                </dxe:ASPxLabel>
                                                <div style="width: 100%; height: 23px; border: 1px solid #e6e6e6;">
                                                    <dxe:ASPxCallbackPanel runat="server" ID="ComponentDatePanel" ClientInstanceName="cComponentDatePanel" OnCallback="ComponentDatePanel_Callback">
                                                        <PanelCollection>
                                                            <dxe:PanelContent runat="server">
                                                                <dxe:ASPxTextBox ID="txt_InvoiceDate" ClientInstanceName="ctxt_InvoiceDate" runat="server" Width="100%" ClientEnabled="false">
                                                                </dxe:ASPxTextBox>
                                                            </dxe:PanelContent>
                                                        </PanelCollection>
                                                    </dxe:ASPxCallbackPanel>
                                                </div>
                                            </div>


                                            <div style="clear: both;"></div>
                                            <div class="col-md-3 hide">
                                                <dxe:ASPxLabel ID="lbl_Currency" runat="server" Text="Currency">
                                                </dxe:ASPxLabel>
                                                <asp:DropDownList ID="ddl_Currency" runat="server" Width="100%">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-3 hide">
                                                <dxe:ASPxLabel ID="lbl_Rate" runat="server" Text="Rate">
                                                </dxe:ASPxLabel>
                                                <dxe:ASPxTextBox ID="txt_Rate" ClientInstanceName="ctxt_Rate" runat="server" Width="100%" Height="28px">
                                                    <MaskSettings Mask="<0..999999999>.<0..9999>" AllowMouseWheel="false" />
                                                    <ClientSideEvents LostFocus="ReBindGrid_Currency" />
                                                </dxe:ASPxTextBox>
                                            </div>
                                            <div class="col-md-3 hide">
                                                <dxe:ASPxLabel ID="lbl_AmountAre" runat="server" Text="Amounts are">
                                                </dxe:ASPxLabel>
                                                <dxe:ASPxComboBox ID="ddl_AmountAre" runat="server" ClientIDMode="Static" ClientInstanceName="cddl_AmountAre" Width="100%">
                                                    <ClientSideEvents SelectedIndexChanged="function(s, e) { PopulateGSTCSTVAT(e)}" />
                                                    <ClientSideEvents LostFocus="function(s, e) { SetFocusonDemand(e)}" />
                                                </dxe:ASPxComboBox>
                                            </div>

                                        </div>
                                        <div style="clear: both;"></div>



                                        <div class="greyd">
                                            <table class="bod-table none">
                                                <tbody>
                                                    <tr>
                                                        <td class="hd" width="80px" id="unitValueID" runat="server">Old unit</td>
                                                        <td style="width: 100px" id="UnitValueCombo" runat="server">
                                                            <dxe:ASPxComboBox ID="cmbOldUnit" runat="server" ClientIDMode="Static" ClientInstanceName="ccmbOldUnit" SelectedIndex="1" Width="100%">
                                                                <Items>
                                                                    <dxe:ListEditItem Text="Yes" Value="1" />
                                                                    <dxe:ListEditItem Text="No" Value="0" />
                                                                </Items>
                                                                <ClientSideEvents TextChanged="ccmbOldUnitTextChanged"></ClientSideEvents>
                                                            </dxe:ASPxComboBox>

                                                        </td>
                                                        <td id="oldunitButton" runat="server">
                                                            <input type="button" value="Old Unit Selection" onclick="OldUnitButtonOnClick()" class="btn btn-small btn-primary" style="display: none" id="OldUnitSelectionButton" />
                                                        </td>
                                                        <td class="hd" id="unitvaluelbl" runat="server">Unit Value</td>
                                                        <td id="unitValueText" runat="server">
                                                            <dxe:ASPxTextBox ID="txtunitValue" MaxLength="80" ClientInstanceName="ctxtunitValue" ClientEnabled="false" runat="server" Width="100%">
                                                                <MaskSettings Mask="<0..999999999999>.<0..99>" AllowMouseWheel="false" />
                                                            </dxe:ASPxTextBox>
                                                        </td>

                                                        <td class="hd" runat="server"><b>V&#818;</b>ehicle(s)</td>
                                                        <td runat="server">
                                                            <div class="col-md-12">

                                                                <div class="relative">
                                                                    <dxe:ASPxCallbackPanel runat="server" ID="ASPxCallbackPanelVehicle" ClientInstanceName="cASPxCallbackPanelVehicle" OnCallback="ASPxCallbackPanelVehicle_Callback" Width="100%">
                                                                        <PanelCollection>
                                                                            <dxe:PanelContent runat="server">
                                                                                <asp:HiddenField runat="server" ID="OldSelectedKeyvalue" />
                                                                                <%--<dxe:ASPxGridLookup ID="poupVehicles" SelectionMode="Multiple" runat="server" TabIndex="7" ClientInstanceName="gridvehicleLookup"
                                                                                    OnDataBinding="poupVehicles_DataBinding" GridViewClientSideEvents-SelectionChanged="VehicleSelectionChanged"
                                                                                    KeyFieldName="vehicle_Id" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                                                                    <Columns>
                                                                                        <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                                                                        <dxe:GridViewDataColumn FieldName="vehicle_regNo" Visible="true" VisibleIndex="1" Caption="Vehicle No" Width="180" Settings-AutoFilterCondition="Contains">
                                                                                            <Settings AutoFilterCondition="Contains" />
                                                                                        </dxe:GridViewDataColumn>
                                                                                        <dxe:GridViewDataColumn FieldName="vehicle_Id" Visible="false" VisibleIndex="2" Caption="ID" Width="100" Settings-AutoFilterCondition="Contains">
                                                                                            <Settings AutoFilterCondition="Contains" />
                                                                                        </dxe:GridViewDataColumn>
                                                                                    </Columns>
                                                                                    <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                                                                        <Templates>
                                                                                            <StatusBar>
                                                                                                <table class="OptionsTable" style="float: right">
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                            <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridVehicleLookup" UseSubmitBehavior="False" />
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </StatusBar>
                                                                                        </Templates>
                                                                                        <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" />
                                                                                        <SettingsPager Mode="ShowAllRecords">
                                                                                        </SettingsPager>
                                                                                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                                                                    </GridViewProperties>
                                                                                    <ClientSideEvents ValueChanged="function(s, e) { QuotationNumberChanged();}" GotFocus="function(s,e){gridvehicleLookup.ShowDropDown();  }" DropDown="LoadOldSelectedKeyvalue" />
                                                                                </dxe:ASPxGridLookup>--%>
                                                                                <dxe:ASPxTextBox ID="txtVehicles" ClientInstanceName="ctxtVehicles" runat="server" MaxLength="1000" Width="100%" aut></dxe:ASPxTextBox>
                                                                            </dxe:PanelContent>
                                                                        </PanelCollection>
                                                                        <ClientSideEvents EndCallback="componentEndCallBack" BeginCallback="BeginComponentCallback" />
                                                                    </dxe:ASPxCallbackPanel>
                                                                </div>
                                                            </div>
                                                        </td>


                                                        <td class="hd">Remarks</td>

                                                        <td id="tdremarks">
                                                            <dxe:ASPxTextBox ID="txtRemarks" MaxLength="300" ClientInstanceName="ctxtRemarks" runat="server" Width="100%">
                                                            </dxe:ASPxTextBox>
                                                        </td>
                                                        <td class="hd">No Commission</td>

                                                        <td id="">
                                                            <dxe:ASPxCheckBox ID="chkNocommission" ClientInstanceName="cchkNocommission" runat="server" Width="100%">
                                                            </dxe:ASPxCheckBox>
                                                        </td>
                                                        <td class="hd" id="lblAdvnceRecptNo" runat="server">
                                                            <input type="button" value="Advance / Return" onclick="AdvanceReceiptOnClick()" class="btn btn-small btn-primary" />

                                                        </td>
                                                        <td id="lblAdvnceRecptNovalue" runat="server">
                                                            <dxe:ASPxTextBox ID="txtAdvnceReceipt" MaxLength="80" ClientInstanceName="ctxtAdvnceReceipt" ClientEnabled="False" runat="server" Width="100%">
                                                                <MaskSettings Mask="<0..999999999999>.<0..99>" AllowMouseWheel="false" />
                                                            </dxe:ASPxTextBox>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>




                                        <div class="content reverse horizontal-images clearfix" style="width: 100%; margin-right: 0; padding: 8px; height: auto; border-top: 1px solid #ccc; border-bottom: 1px solid #ccc; border-radius: 0;">
                                            <ul>
                                                <li class="clsbnrLblTotalQty">
                                                    <div class="horizontallblHolder">
                                                        <table>
                                                            <tbody>
                                                                <tr>
                                                                    <td>
                                                                        <dxe:ASPxLabel ID="ASPxLabel2" runat="server" Text="Total Quantity" ClientInstanceName="cbnrLblTotalQty" />
                                                                    </td>
                                                                    <td>
                                                                        <dxe:ASPxLabel ID="ASPxLabel12" runat="server" Text="0.00" ClientInstanceName="cbnrLblTotalQty" />
                                                                    </td>
                                                                </tr>

                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </li>
                                                <li class="clsbnrLblTaxableAmt">
                                                    <div class="horizontallblHolder">
                                                        <table>
                                                            <tbody>
                                                                <tr>
                                                                    <td>
                                                                        <dxe:ASPxLabel ID="bnrLblTaxableAmt" runat="server" Text="Taxable Amt" ClientInstanceName="cbnrLblTaxableAmt" />
                                                                    </td>
                                                                    <td>
                                                                        <dxe:ASPxLabel ID="bnrLblTaxableAmtval" runat="server" Text="0.00" ClientInstanceName="cbnrLblTaxableAmtval" />
                                                                    </td>
                                                                </tr>

                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </li>
                                                <li class="clsbnrLblTaxAmt">
                                                    <div class="horizontallblHolder" id="">
                                                        <table>
                                                            <tbody>
                                                                <tr>
                                                                    <td>
                                                                        <dxe:ASPxLabel ID="bnrLblTaxAmt" runat="server" Text="Tax Amt" ClientInstanceName="cbnrLblTaxAmt" />
                                                                    </td>
                                                                    <td>
                                                                        <dxe:ASPxLabel ID="bnrLblTaxAmtval" runat="server" Text="0.00" ClientInstanceName="cbnrLblTaxAmtval" />
                                                                    </td>
                                                                </tr>

                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </li>
                                                <li class="clsbnrLblAmtWithTax" runat="server" id="oldUnitBanerLbl">
                                                    <div class="horizontallblHolder">
                                                        <table>
                                                            <tbody>
                                                                <tr>
                                                                    <td>
                                                                        <dxe:ASPxLabel ID="bnrLblAmtWithTax" runat="server" Text="Amount With Tax" ClientInstanceName="cbnrLblAmtWithTax" />
                                                                    </td>
                                                                    <td>
                                                                        <dxe:ASPxLabel ID="bnrlblAmountWithTaxValue" runat="server" Text="0.00" ClientInstanceName="cbnrlblAmountWithTaxValue" />
                                                                    </td>
                                                                </tr>

                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </li>
                                                <li class="clsbnrLblLessOldVal">
                                                    <div class="horizontallblHolder" id="">
                                                        <table>
                                                            <tbody>
                                                                <tr>
                                                                    <td>
                                                                        <dxe:ASPxLabel ID="bnrLblLessOldVal" runat="server" Text="Less Old Unit Value" ClientInstanceName="cbnrLblLessOldVal" />
                                                                    </td>
                                                                    <td>
                                                                        <dxe:ASPxLabel ID="bnrLblLessOldMainVal" runat="server" Text=" 0.00" ClientInstanceName="cbnrLblLessOldMainVal"></dxe:ASPxLabel>
                                                                    </td>
                                                                </tr>

                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </li>
                                                <li class="clsbnrLblLessAdvance" id="idclsbnrLblLessAdvance" runat="server">
                                                    <div class="horizontallblHolder">
                                                        <table>
                                                            <tbody>
                                                                <tr>
                                                                    <td>
                                                                        <dxe:ASPxLabel ID="bnrLblLessAdvance" runat="server" Text="Advance Adjusted" ClientInstanceName="cbnrLblLessAdvance" />
                                                                    </td>
                                                                    <td>
                                                                        <dxe:ASPxLabel ID="bnrLblLessAdvanceValue" runat="server" Text="0.00" ClientInstanceName="cbnrLblLessAdvanceValue" />
                                                                    </td>
                                                                </tr>

                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </li>

                                                <li class="clsbnrLblInvVal" id="otherChargesId">
                                                    <div class="horizontallblHolder">
                                                        <table>
                                                            <tbody>
                                                                <tr>
                                                                    <td>
                                                                        <dxe:ASPxLabel ID="cbnrOtherCharges" runat="server" Text="Other Charges" ClientInstanceName="cbnrOtherCharges" />
                                                                    </td>
                                                                    <td>
                                                                        <dxe:ASPxLabel ID="bnrOtherChargesvalue" runat="server" Text="0.00" ClientInstanceName="cbnrOtherChargesvalue" />
                                                                    </td>
                                                                </tr>

                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </li>



                                                <li class="clsbnrLblInvVal">
                                                    <div class="horizontallblHolder">
                                                        <table>
                                                            <tbody>
                                                                <tr>
                                                                    <td>
                                                                        <dxe:ASPxLabel ID="bnrLblInvVal" runat="server" Text="Invoice Value" ClientInstanceName="cbnrLblInvVal" />
                                                                    </td>
                                                                    <td>
                                                                        <dxe:ASPxLabel ID="bnrLblInvValue" runat="server" Text="0.00" ClientInstanceName="cbnrLblInvValue" />
                                                                    </td>
                                                                </tr>

                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </li>




                                                <li class="clsbnrLblInvVal">
                                                    <div class="horizontallblHolder" style="border-color: #f14327;">
                                                        <table>
                                                            <tbody>
                                                                <tr>
                                                                    <td style="background: #f14327;">
                                                                        <dxe:ASPxLabel ID="ASPxLabel11" runat="server" Text="Running Balance" ClientInstanceName="cbnrLblInvVal" />
                                                                    </td>
                                                                    <td>
                                                                        <strong>
                                                                            <dxe:ASPxLabel ID="lblRunningBalanceCapsul" runat="server" Text="0.00" ClientInstanceName="clblRunningBalanceCapsul" />
                                                                        </strong>
                                                                    </td>
                                                                </tr>

                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </li>
                                                <li class="clsbnrLblInvVal hide">
                                                    <div runat="server" id="divSendSMS">

                                                        <strong>

                                                            <input type="checkbox" runat="server" name="chksendSMS" id="chksendSMS" class="hide" onclick="SendSMSChk()" />&nbsp;Send SMS
                                                             <asp:HiddenField ID="hdnSendSMS" runat="server"></asp:HiddenField>
                                                            <asp:HiddenField ID="hdnCustMobile" runat="server"></asp:HiddenField>
                                                            <asp:HiddenField ID="hdnsendsmsSettings" runat="server" />
                                                        </strong>

                                                    </div>
                                                </li>

                                            </ul>

                                        </div>



                                        <uc1:ucPaymentDetails runat="server" ID="PaymentDetails" />




                                        <uc3:UOMConversionControl runat="server" ID="UOMConversionControl" />












                                        <div class="">
                                            <div style="display: none;">
                                                <a href="javascript:void(0);" onclick="OnAddNewClick()" class="btn btn-primary"><span>Add New</span> </a>
                                            </div>
                                            <div>
                                                <br />
                                            </div>

                                        </div>
                                        <div style="clear: both;"></div>
                                        <br />
                                        <div class="col-md-12" id="divSubmitButton">
                                            <asp:Label ID="lbl_quotestatusmsg" runat="server" Text="" Font-Bold="true" ForeColor="Red" Font-Size="Medium"></asp:Label>
                                            <dxe:ASPxButton ID="btn_SaveRecords" ClientInstanceName="cbtn_SaveRecords" runat="server" AutoPostBack="False" Text="Save & N&#818;ew" CssClass="btn btn-success" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                <ClientSideEvents Click="function(s, e) {Save_ButtonClick();}" />
                                            </dxe:ASPxButton>
                                            <dxe:ASPxButton ID="ASPxButton1" ClientInstanceName="cbtn_SaveRecords" runat="server" AutoPostBack="False" Text="Save & Ex&#818;it" CssClass="btn btn-success" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                <ClientSideEvents Click="function(s, e) {SaveExit_ButtonClick(s,e);}" />
                                            </dxe:ASPxButton>

                                            <dxe:ASPxButton ID="ASPxButton7" ClientInstanceName="cbtn_SaveRecordsEdit" OnClick="Edit_Click" runat="server" AutoPostBack="True" Text="Save & Ex&#818;it" CssClass="btn btn-primary" UseSubmitBehavior="False">
                                                <ClientSideEvents Click="function(s, e) {SaveExit_ButtonClick(s,e);}" />
                                            </dxe:ASPxButton>

                                            <%--   <asp:Button ID="ASPxButton2" runat="server" Text="UDF" CssClass="btn btn-primary" OnClientClick="if(OpenUdf()){ return false;}" />--%>
                                            <dxe:ASPxButton ID="ASPxButton2" ClientInstanceName="cbtn_SaveRecords" runat="server" AutoPostBack="False" Text="U&#818;DF" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                <ClientSideEvents Click="function(s, e) {if(OpenUdf()){ return false}}" />
                                            </dxe:ASPxButton>
                                            <%--  Text="T&#818;axes"--%>
                                            <dxe:ASPxButton ID="ASPxButton3" ClientInstanceName="cbtn_SaveRecords" runat="server" AutoPostBack="False" Text="T&#818;ax & Charges" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                <ClientSideEvents Click="function(s, e) {Save_TaxesClick();}" />
                                            </dxe:ASPxButton>

                                        </div>


                                    </div>
                                </dxe:ContentControl>
                            </ContentCollection>
                            <%--test generel--%>
                        </dxe:TabPage>

                        <dxe:TabPage Name="[B]illing/Shipping" Text="Billing/Shipping">
                            <ContentCollection>
                                <dxe:ContentControl runat="server">

                                    <dxe:ASPxCallbackPanel runat="server" ID="ComponentPanel" ClientInstanceName="cComponentPanel" OnCallback="ComponentPanel_Callback">
                                        <PanelCollection>
                                            <dxe:PanelContent runat="server">
                                                <div>
                                                    <table>
                                                        <tr>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>
                                                    </table>
                                                    <div class="row">
                                                        <div class="col-md-6 mbot5" id="DivBilling">
                                                            <div class="clearfix" style="background: #fff; border: 1px solid #ccc; padding: 0px 0 10px 0;">
                                                                <h5 class="headText" style="margin: 0; padding: 10px 15px 10px; background: #ececec; margin-bottom: 10px">Billing Address</h5>
                                                                <div style="padding-right: 8px">
                                                                    <div class="col-md-4" style="height: auto;">
                                                                        <asp:Label ID="LblType" runat="server" Text="Select Address:" CssClass="newLbl"></asp:Label>
                                                                    </div>
                                                                    <div class="col-md-8">
                                                                        <dxe:ASPxButtonEdit ID="ASPxButtonEdit1" runat="server" ReadOnly="true" ClientInstanceName="txtSelectBillingAdd" Width="100%">
                                                                            <Buttons>
                                                                                <dxe:EditButton>
                                                                                </dxe:EditButton>
                                                                            </Buttons>
                                                                            <ClientSideEvents ButtonClick="function(s,e){SelectBillingAddClick();}" KeyDown="function(s,e){SelectBillingAddKeyDown(s,e);}" />
                                                                        </dxe:ASPxButtonEdit>
                                                                    </div>
                                                                    <div class="col-md-4" style="height: auto; margin-bottom: 5px;">
                                                                        Address1: <span style="color: red;">*</span>
                                                                    </div>
                                                                    <div class="col-md-8">

                                                                        <div class="Left_Content relative">
                                                                            <dxe:ASPxTextBox ID="txtAddress1" MaxLength="80" ClientInstanceName="ctxtAddress1"
                                                                                runat="server" Width="100%">
                                                                            </dxe:ASPxTextBox>
                                                                            <span id="badd1" style="display: none" class="mandt">
                                                                                <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                                                            </span>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-4" style="height: auto; margin-bottom: 5px;">
                                                                        <%--Code--%>
                                                                            Address2:
                                                                           

                                                                    </div>
                                                                    <%--Start of Address2 --%>
                                                                    <div class="col-md-8">

                                                                        <div class="Left_Content relative">
                                                                            <dxe:ASPxTextBox ID="txtAddress2" MaxLength="80" ClientInstanceName="ctxtAddress2"
                                                                                runat="server" Width="100%">
                                                                            </dxe:ASPxTextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-4" style="height: auto; margin-bottom: 5px;">
                                                                        Address3: 
                                                                    </div>
                                                                    <%--Start of Address3 --%>

                                                                    <div class="col-md-8">

                                                                        <div class="Left_Content relative">
                                                                            <dxe:ASPxTextBox ID="txtAddress3" MaxLength="80" ClientInstanceName="ctxtAddress3"
                                                                                runat="server" Width="100%">
                                                                            </dxe:ASPxTextBox>
                                                                        </div>
                                                                    </div>
                                                                    <%--Start of Landmark --%>
                                                                    <div class="col-md-4" style="height: auto; margin-bottom: 5px;">
                                                                        <%--Code--%>
                                                                            Landmark:
                                                                             

                                                                    </div>
                                                                    <div class="col-md-8">

                                                                        <div class="Left_Content relative">
                                                                            <dxe:ASPxTextBox ID="txtlandmark" MaxLength="80" ClientInstanceName="ctxtlandmark"
                                                                                runat="server" Width="100%">
                                                                            </dxe:ASPxTextBox>
                                                                        </div>
                                                                    </div>

                                                                    <%--start of Pin/Zip.--%>
                                                                    <div class="col-md-4" style="height: auto;">
                                                                        <%--Type--%>
                                                                        <asp:Label ID="Label8" runat="server" Text="Pin/Zip (6 Characters):" CssClass=""></asp:Label><span style="color: red;">*</span>
                                                                    </div>
                                                                    <div class="col-md-8">

                                                                        <div class="Left_Content relative">

                                                                            <dxe:ASPxTextBox ID="txtbillingPin" MaxLength="6" ClientInstanceName="ctxtbillingPin"
                                                                                runat="server" Width="100%">

                                                                                <ClientSideEvents LostFocus="BillingPinChange" />
                                                                            </dxe:ASPxTextBox>
                                                                            <asp:HiddenField ID="hdBillingPin" runat="server"></asp:HiddenField>

                                                                            <span id="bpin" style="display: none" class="mandt">
                                                                                <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                                                            </span>

                                                                        </div>
                                                                    </div>
                                                                    <div class="clear"></div>


                                                                    <div class="col-md-4" style="height: auto;">
                                                                        <%--Type--%>
                                                                        <asp:Label ID="Label2" runat="server" Text="Country:" CssClass=""></asp:Label><span style="color: red;">*</span>
                                                                    </div>
                                                                    <div class="col-md-8">

                                                                        <div class="Left_Content relative">
                                                                            <asp:Label runat="server" Text="" ID="lblBillingCountry" Width="100%" class="labelClass"></asp:Label>
                                                                            <asp:HiddenField ID="hdlblBillingCountry" runat="server" />
                                                                            <asp:HiddenField ID="lblBillingCountryValue" runat="server"></asp:HiddenField>
                                                                        </div>
                                                                    </div>
                                                                    <div class="clear"></div>
                                                                    <%--End of State--%>
                                                                    <div class="col-md-4" style="height: auto;">
                                                                        <%--Type--%>
                                                                        <asp:Label ID="Label4" runat="server" Text="State:" CssClass=""></asp:Label><span style="color: red;">*</span>
                                                                    </div>
                                                                    <div class="col-md-8">

                                                                        <div class="Left_Content relative">
                                                                            <asp:Label runat="server" Text="" ID="lblBillingState" Width="100%" class="labelClass"></asp:Label>
                                                                            <asp:HiddenField ID="hdlblBillingState" runat="server" />
                                                                            <asp:HiddenField ID="lblBillingStateText" runat="server"></asp:HiddenField>
                                                                            <asp:HiddenField ID="lblBillingStateValue" runat="server"></asp:HiddenField>
                                                                        </div>
                                                                    </div>
                                                                    <div class="clear"></div>

                                                                    <%--start of City/district.--%>
                                                                    <div class="col-md-4" style="height: auto;">
                                                                        <asp:Label ID="Label6" runat="server" Text="City/District:" CssClass=""></asp:Label><span style="color: red;">*</span>
                                                                    </div>
                                                                    <div class="col-md-8">

                                                                        <div class="Left_Content relative">
                                                                            <asp:Label runat="server" Text="" ID="lblBillingCity" Width="100%" class="labelClass"></asp:Label>
                                                                            <asp:HiddenField ID="hdlblBillingCity" runat="server" />
                                                                            <asp:HiddenField ID="lblBillingCityValue" runat="server"></asp:HiddenField>
                                                                        </div>
                                                                    </div>
                                                                    <div class="clear"></div>

                                                                    <%--start of Area--%>
                                                                    <div class="col-md-4" style="height: auto;">
                                                                        <%--Type--%>
                                                                        <asp:Label ID="Label10" runat="server" Text="Area:" CssClass=""></asp:Label>
                                                                    </div>
                                                                    <div class="col-md-8">

                                                                        <div class="Left_Content">
                                                                            <dxe:ASPxComboBox ID="CmbArea" ClientInstanceName="CmbArea" runat="server"
                                                                                ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0">
                                                                                <ClearButton DisplayMode="Always"></ClearButton>
                                                                                <ClientSideEvents EndCallback="cmbArea_endcallback"></ClientSideEvents>
                                                                            </dxe:ASPxComboBox>
                                                                        </div>
                                                                    </div>

                                                                    <div class="clear"></div>

                                                                    <div class="col-md-4" style="height: auto;">
                                                                        <asp:Label ID="LabelGST" runat="server" Text="GSTIN:" CssClass="newLbl"></asp:Label>
                                                                    </div>
                                                                    <div class="col-md-8">
                                                                        <div class="Left_Content">
                                                                            <ul class="nestedinput">
                                                                                <li>
                                                                                    <dxe:ASPxTextBox ID="txtBillingGSTIN1" ClientInstanceName="ctxtBillingGSTIN1" MaxLength="2" runat="server" Width="33px" ReadOnly="true">
                                                                                    </dxe:ASPxTextBox>
                                                                                </li>
                                                                                <li class="dash">-</li>
                                                                                <li>
                                                                                    <dxe:ASPxTextBox ID="txtBillingGSTIN2" ClientInstanceName="ctxtBillingGSTIN2" MaxLength="10" runat="server" Width="90px" ReadOnly="true">
                                                                                    </dxe:ASPxTextBox>
                                                                                </li>
                                                                                <li class="dash">-</li>
                                                                                <li>
                                                                                    <dxe:ASPxTextBox ID="txtBillingGSTIN3" ClientInstanceName="ctxtBillingGSTIN3" MaxLength="3" runat="server" Width="50px" ReadOnly="true">
                                                                                    </dxe:ASPxTextBox>
                                                                                </li>
                                                                            </ul>
                                                                        </div>
                                                                    </div>


                                                                    <div class="clear"></div>
                                                                    <div class="col-md-12" style="height: auto;">

                                                                        <a class="[ form-group ]" id="shiptosame" href="#" onclick="javascript: BillingCheckChange()">
                                                                            <div class="[ btn-group ]">
                                                                                <label for="fancy-checkbox-success" class="[ btn btn-default active ]">
                                                                                    Ship <b>T&#818;</b>o Same Address >>
                                                                                </label>
                                                                            </div>
                                                                        </a>

                                                                    </div>


                                                                </div>
                                                            </div>
                                                        </div>


                                                        <div class="col-md-6 mbot5" id="DivShipping">
                                                            <div class="clearfix" style="background: #fff; border: 1px solid #ccc; padding: 0px 0 10px 0;">

                                                                <h5 class="headText" style="margin: 0; padding: 10px 15px 10px; background: #ececec; margin-bottom: 10px">Shipping Address</h5>
                                                                <div style="padding-right: 8px">
                                                                    <div class="col-md-4" style="height: auto;">
                                                                        <label>Select Address:</label>
                                                                    </div>
                                                                    <div class="col-md-8">
                                                                        <dxe:ASPxButtonEdit ID="txtSelectShippingAdd" runat="server" ReadOnly="true" ClientInstanceName="ctxtSelectShippingAdd" Width="100%">
                                                                            <Buttons>
                                                                                <dxe:EditButton>
                                                                                </dxe:EditButton>
                                                                            </Buttons>
                                                                            <ClientSideEvents ButtonClick="function(s,e){SelectShippingAddClick();}" KeyDown="function(s,e){SelectShippingAddKeyDown(s,e);}" />
                                                                        </dxe:ASPxButtonEdit>
                                                                    </div>
                                                                    <div class="col-md-4" style="height: auto; margin-bottom: 5px;">
                                                                        Address1: <span style="color: red;">*</span>

                                                                    </div>
                                                                    <div class="col-md-8">

                                                                        <div class="Left_Content relative">
                                                                            <dxe:ASPxTextBox ID="txtsAddress1" MaxLength="80" ClientInstanceName="ctxtsAddress1"
                                                                                runat="server" Width="100%">
                                                                            </dxe:ASPxTextBox>
                                                                            <span id="sadd1" style="display: none" class="mandt">
                                                                                <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                                                            </span>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-4" style="height: auto; margin-bottom: 5px;">
                                                                        Address2:
                                                                           
                                                                    </div>
                                                                    <div class="col-md-8">

                                                                        <div class="Left_Content">
                                                                            <dxe:ASPxTextBox ID="txtsAddress2" MaxLength="80" ClientInstanceName="ctxtsAddress2"
                                                                                runat="server" Width="100%">
                                                                            </dxe:ASPxTextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-4" style="height: auto; margin-bottom: 5px;">
                                                                        Address3: 
                                                                    </div>
                                                                    <div class="col-md-8">

                                                                        <div class="Left_Content">
                                                                            <dxe:ASPxTextBox ID="txtsAddress3" MaxLength="80" ClientInstanceName="ctxtsAddress3"
                                                                                runat="server" Width="100%">
                                                                            </dxe:ASPxTextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-4" style="height: auto; margin-bottom: 5px;">
                                                                        Landmark: 
                                                                    </div>
                                                                    <div class="col-md-8">

                                                                        <div class="Left_Content">
                                                                            <dxe:ASPxTextBox ID="txtslandmark" MaxLength="80" ClientInstanceName="ctxtslandmark"
                                                                                runat="server" Width="100%">
                                                                            </dxe:ASPxTextBox>
                                                                        </div>
                                                                    </div>



                                                                    <div class="col-md-4" style="height: auto;">
                                                                        <%--Type--%>
                                                                        <asp:Label ID="Label9" runat="server" Text="Pin/Zip (6 Characters):" CssClass=""></asp:Label><span style="color: red;">*</span>
                                                                    </div>
                                                                    <div class="col-md-8">

                                                                        <div class="Left_Content relative">

                                                                            <dxe:ASPxTextBox ID="txtShippingPin" MaxLength="6" ClientInstanceName="ctxtShippingPin"
                                                                                runat="server" Width="100%">

                                                                                <ClientSideEvents LostFocus="ShippingPinChange" />
                                                                            </dxe:ASPxTextBox>
                                                                            <asp:HiddenField ID="hdShippingPin" runat="server"></asp:HiddenField>


                                                                            <span id="spin" style="display: none" class="mandt">
                                                                                <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                                                            </span>
                                                                        </div>
                                                                    </div>
                                                                    <div class="clear"></div>
                                                                    <%--End of Pin/Zip.--%>




                                                                    <div class="col-md-4" style="height: auto;">

                                                                        <asp:Label ID="Label3" runat="server" Text="Country:" CssClass=""></asp:Label><span style="color: red;">*</span>
                                                                    </div>
                                                                    <div class="col-md-8">

                                                                        <div class="Left_Content relative">
                                                                            <asp:Label runat="server" Text="" ID="lblShippingCountry" Width="100%" class="labelClass"></asp:Label>
                                                                            <asp:HiddenField ID="hdlblShippingCountry" runat="server" />
                                                                            <asp:HiddenField ID="lblShippingCountryValue" runat="server"></asp:HiddenField>
                                                                        </div>
                                                                    </div>
                                                                    <div class="clear"></div>
                                                                    <%--End of Country--%>
                                                                    <div class="col-md-4" style="height: auto;">
                                                                        <%--Type--%>
                                                                        <asp:Label ID="Label5" runat="server" Text="State:" CssClass=""></asp:Label><span style="color: red;">*</span>
                                                                    </div>
                                                                    <div class="col-md-8">

                                                                        <div class="Left_Content relative">

                                                                            <asp:Label runat="server" Text="" ID="lblShippingState" Width="100%" class="labelClass"></asp:Label>
                                                                            <asp:HiddenField ID="hdlblShippingState" runat="server" />
                                                                            <asp:HiddenField ID="lblShippingStateText" runat="server"></asp:HiddenField>
                                                                            <asp:HiddenField ID="lblShippingStateValue" runat="server"></asp:HiddenField>

                                                                        </div>
                                                                    </div>
                                                                    <div class="clear"></div>
                                                                    <%--End of State--%>
                                                                    <div class="col-md-4" style="height: auto;">
                                                                        <%--Type--%>
                                                                        <asp:Label ID="Label7" runat="server" Text="City/District:" CssClass=""></asp:Label><span style="color: red;">*</span>
                                                                    </div>
                                                                    <div class="col-md-8">

                                                                        <div class="Left_Content relative">
                                                                            <asp:Label runat="server" Text="" ID="lblShippingCity" Width="100%" class="labelClass"></asp:Label>
                                                                            <asp:HiddenField ID="hdlblShippingCity" runat="server" />
                                                                            <asp:HiddenField ID="lblShippingCityValue" runat="server"></asp:HiddenField>

                                                                        </div>
                                                                    </div>
                                                                    <div class="clear"></div>
                                                                    <%--End of City/District--%>

                                                                    <div class="col-md-4" style="height: auto;">
                                                                        <%--Type--%>
                                                                        <asp:Label ID="Label11" runat="server" Text="Area:" CssClass=""></asp:Label>
                                                                    </div>
                                                                    <div class="col-md-8">

                                                                        <div class="Left_Content">
                                                                            <dxe:ASPxComboBox ID="CmbArea1" ClientInstanceName="CmbArea1" runat="server"
                                                                                ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0">
                                                                                <ClearButton DisplayMode="Always"></ClearButton>
                                                                                <ClientSideEvents EndCallback="cmbshipArea_endcallback"></ClientSideEvents>
                                                                            </dxe:ASPxComboBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="clear"></div>
                                                                    <div class="col-md-4" style="height: auto;">
                                                                        <asp:Label ID="Label36" runat="server" Text="GSTIN:" CssClass="newLbl"></asp:Label>
                                                                    </div>
                                                                    <div class="col-md-8">
                                                                        <div class="Left_Content">
                                                                            <ul class="nestedinput">
                                                                                <li>
                                                                                    <dxe:ASPxTextBox ID="txtShippingGSTIN1" ClientInstanceName="ctxtShippingGSTIN1" MaxLength="2" runat="server" Width="33px" ReadOnly="true">
                                                                                    </dxe:ASPxTextBox>
                                                                                </li>
                                                                                <li class="dash">-</li>
                                                                                <li>
                                                                                    <dxe:ASPxTextBox ID="txtShippingGSTIN2" ClientInstanceName="ctxtShippingGSTIN2" MaxLength="10" runat="server" Width="90px" ReadOnly="true">
                                                                                    </dxe:ASPxTextBox>
                                                                                </li>
                                                                                <li class="dash">-</li>
                                                                                <li>
                                                                                    <dxe:ASPxTextBox ID="txtShippingGSTIN3" ClientInstanceName="ctxtShippingGSTIN3" MaxLength="3" runat="server" Width="50px" ReadOnly="true">
                                                                                    </dxe:ASPxTextBox>
                                                                                </li>
                                                                            </ul>
                                                                        </div>
                                                                    </div>



                                                                    <div class="clear"></div>
                                                                    <div class="col-md-12" style="height: auto;">


                                                                        <a class="[ form-group ]" id="billtoSame" href="#" onclick="javascript: ShippingCheckChange()">
                                                                            <div class="[ btn-group ]">

                                                                                <label for="fancy-checkbox-successShipping" class="[ btn btn-default active ]">
                                                                                    << Bill To Same Address
                                                                                </label>
                                                                            </div>
                                                                        </a>


                                                                    </div>




                                                                </div>

                                                            </div>
                                                        </div>
                                                    </div>
                                                    <%--End of Address Type--%>




                                                    <%--End of Area--%>


                                                    <div class="clear"></div>
                                                    <div class="col-md-12 pdLeft0" style="padding-top: 10px">
                                                        <%--   <button class="btn btn-primary">OK</button> ValidationGroup="Address"--%>

                                                        <dxe:ASPxButton ID="btnSave_citys" CausesValidation="true" ClientInstanceName="cbtnSave_citys" runat="server"
                                                            AutoPostBack="False" Text="O&#818;K" CssClass="btn btn-primary" UseSubmitBehavior="false">
                                                            <ClientSideEvents Click="function (s, e) {btnSave_QuoteAddress();}" />
                                                        </dxe:ASPxButton>

                                                    </div>
                                                </div>
                                            </dxe:PanelContent>
                                        </PanelCollection>
                                        <ClientSideEvents EndCallback="Panel_endcallback" />
                                    </dxe:ASPxCallbackPanel>



                                </dxe:ContentControl>
                            </ContentCollection>
                        </dxe:TabPage>

                    </TabPages>
                    <ClientSideEvents ActiveTabChanged="function(s, e) {
	                                            var activeTab   = page.GetActiveTab();
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




            <%--Sudip--%>
            <div class="PopUpArea">
                <asp:HiddenField ID="HdChargeProdAmt" runat="server" />
                <asp:HiddenField ID="HdChargeProdNetAmt" runat="server" />
                <%--ChargesTax--%>
                <dxe:ASPxPopupControl ID="Popup_Taxes" runat="server" ClientInstanceName="cPopup_Taxes"
                    Width="900px" Height="300px" HeaderText="Other Charges" PopupHorizontalAlign="WindowCenter"
                    BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
                    Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
                    ContentStyle-CssClass="pad">
                    <ContentStyle VerticalAlign="Top" CssClass="pad">
                    </ContentStyle>
                    <ContentCollection>
                        <dxe:PopupControlContentControl runat="server">
                            <div class="Top clearfix">
                                <div id="content-5" class="col-md-12  wrapHolder content horizontal-images" style="width: 100%; margin-right: 0;">
                                    <ul>
                                        <li>
                                            <div class="lblHolder">
                                                <table>
                                                    <tbody>
                                                        <tr>
                                                            <td>Gross Amount Total
                                                                <dxe:ASPxLabel ID="ASPxLabel6" runat="server" Text="ASPxLabel" ClientInstanceName="clblChargesTaxableGross"></dxe:ASPxLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <dxe:ASPxLabel ID="txtProductAmount" runat="server" Text="ASPxLabel" ClientInstanceName="ctxtProductAmount">
                                                                </dxe:ASPxLabel>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </li>
                                        <li class="lblChargesGSTforGross">
                                            <div class="lblHolder">
                                                <table>
                                                    <tbody>
                                                        <tr>
                                                            <td>GST</td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <dxe:ASPxLabel ID="lblChargesGSTforGross" runat="server" Text="ASPxLabel" ClientInstanceName="clblChargesGSTforGross">
                                                                </dxe:ASPxLabel>
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
                                                            <td>Total Discount</td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <dxe:ASPxLabel ID="txtProductDiscount" runat="server" Text="ASPxLabel" ClientInstanceName="ctxtProductDiscount">
                                                                </dxe:ASPxLabel>
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
                                                            <td>Total Charges</td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <dxe:ASPxLabel ID="txtProductTaxAmount" runat="server" Text="ASPxLabel" ClientInstanceName="ctxtProductTaxAmount"></dxe:ASPxLabel>
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
                                                            <td>Net Amount Total
                                                                <dxe:ASPxLabel ID="ASPxLabel7" runat="server" Text="ASPxLabel" ClientInstanceName="clblChargesTaxableNet"></dxe:ASPxLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <dxe:ASPxLabel ID="txtProductNetAmount" runat="server" Text="ASPxLabel" ClientInstanceName="ctxtProductNetAmount"></dxe:ASPxLabel>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </li>
                                        <li class="lblChargesGSTforNet">
                                            <div class="lblHolder">
                                                <table>
                                                    <tbody>
                                                        <tr>
                                                            <td>GST</td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <dxe:ASPxLabel ID="lblChargesGSTforNet" runat="server" Text="ASPxLabel" ClientInstanceName="clblChargesGSTforNet">
                                                                </dxe:ASPxLabel>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </li>
                                    </ul>
                                </div>
                                <div class="clear">
                                </div>
                                <%--Error Msg--%>

                                <div class="col-md-8 hide" id="ErrorMsgCharges">
                                    <div class="lblHolder">
                                        <table>
                                            <tbody>
                                                <tr>
                                                    <td>Status
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Tax Code/Charges Not Defined.
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>

                                </div>

                                <div class="clear">
                                </div>
                                <div class="col-md-12 gridTaxClass" style="">
                                    <dxe:ASPxGridView runat="server" KeyFieldName="TaxID" ClientInstanceName="gridTax" ID="gridTax"
                                        Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false"
                                        Settings-ShowFooter="false" OnCustomCallback="gridTax_CustomCallback" OnBatchUpdate="gridTax_BatchUpdate"
                                        OnRowInserting="gridTax_RowInserting" OnRowUpdating="gridTax_RowUpdating" OnRowDeleting="gridTax_RowDeleting"
                                        OnDataBinding="gridTax_DataBinding">
                                        <Settings VerticalScrollableHeight="150" VerticalScrollBarMode="Auto"></Settings>
                                        <SettingsPager Visible="false"></SettingsPager>
                                        <Columns>
                                            <dxe:GridViewDataTextColumn FieldName="TaxName" Caption="Tax" VisibleIndex="0" Width="40%" ReadOnly="true">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="calCulatedOn" Caption="Calculated On" VisibleIndex="0" Width="20%" ReadOnly="true">
                                                <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                </PropertiesTextEdit>
                                                <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                            </dxe:GridViewDataTextColumn>

                                            <dxe:GridViewDataTextColumn FieldName="Percentage" Caption="Percentage" VisibleIndex="1" Width="20%">
                                                <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00">
                                                    <MaskSettings Mask="&lt;-999999999..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                    <ClientSideEvents LostFocus="PercentageTextChange" />
                                                    <ClientSideEvents />
                                                </PropertiesTextEdit>
                                                <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="Amount" Caption="Amount" VisibleIndex="2" Width="20%">
                                                <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00">
                                                    <MaskSettings Mask="&lt;-999999999..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                    <ClientSideEvents LostFocus="QuotationTaxAmountTextChange" GotFocus="QuotationTaxAmountGotFocus" />
                                                </PropertiesTextEdit>
                                                <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                        </Columns>
                                        <ClientSideEvents EndCallback="OnTaxEndCallback" />
                                        <SettingsDataSecurity AllowEdit="true" />
                                        <SettingsEditing Mode="Batch" NewItemRowPosition="Bottom">
                                            <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="row" />
                                        </SettingsEditing>
                                    </dxe:ASPxGridView>
                                </div>
                                <div class="col-md-12">
                                    <table style="" class="chargesDDownTaxClass">
                                        <tr class="chargeGstCstvatClass">
                                            <td style="padding-top: 10px; padding-right: 25px"><span><strong>GST</strong></span></td>
                                            <td style="padding-top: 10px; width: 200px;">
                                                <dxe:ASPxComboBox ID="cmbGstCstVatcharge" ClientInstanceName="ccmbGstCstVatcharge" runat="server" SelectedIndex="0"
                                                    ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" TextFormatString="{0}"
                                                    OnCallback="cmbGstCstVatcharge_Callback">

                                                    <Columns>
                                                        <dxe:ListBoxColumn FieldName="Taxes_Name" Caption="Tax Component ID" Width="250" />
                                                        <dxe:ListBoxColumn FieldName="TaxCodeName" Caption="Tax Component Name" Width="250" />

                                                    </Columns>
                                                    <ClientSideEvents SelectedIndexChanged="ChargecmbGstCstVatChange"
                                                        GotFocus="chargeCmbtaxClick" />

                                                </dxe:ASPxComboBox>



                                            </td>
                                            <td style="padding-left: 15px; padding-top: 10px; width: 200px;">
                                                <dxe:ASPxTextBox ID="txtGstCstVatCharge" MaxLength="80" ClientInstanceName="ctxtGstCstVatCharge" ReadOnly="true" Text="0.00"
                                                    runat="server" Width="100%">
                                                    <MaskSettings Mask="&lt;-999999999..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                </dxe:ASPxTextBox>

                                            </td>
                                            <td style="padding-left: 15px; padding-top: 10px">
                                                <input type="button" onclick="recalculateTaxCharge()" class="btn btn-info btn-small RecalculateCharge" value="Recalculate GST" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="clear">
                                    <br />
                                </div>



                                <div class="col-sm-3">
                                    <div>
                                        <dxe:ASPxButton ID="btn_SaveTax" ClientInstanceName="cbtn_SaveTax" runat="server" AutoPostBack="False" Text="Save" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="false">
                                            <ClientSideEvents Click="function(s, e) {Save_TaxClick();}" />
                                        </dxe:ASPxButton>
                                        <dxe:ASPxButton ID="ASPxButton5" ClientInstanceName="cbtn_tax_cancel" runat="server" AutoPostBack="False" UseSubmitBehavior="false" Text="Cancel" CssClass="btn btn-danger">
                                            <ClientSideEvents Click="function(s, e) {cPopup_Taxes.Hide();}" />
                                        </dxe:ASPxButton>
                                    </div>
                                </div>

                                <div class="col-sm-9">
                                    <table class="pull-right">
                                        <tr>
                                            <td style="padding-right: 30px"><strong>Total Charges</strong></td>
                                            <td>
                                                <div>
                                                    <dxe:ASPxTextBox ID="txtQuoteTaxTotalAmt" runat="server" Width="100%" ClientInstanceName="ctxtQuoteTaxTotalAmt" Text="0.00" HorizontalAlign="Right" Font-Size="12px" ReadOnly="true">
                                                        <MaskSettings Mask="&lt;-999999999..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                        <%-- <MaskSettings Mask="<0..999999999999g>.<0..99g>" />--%>
                                                    </dxe:ASPxTextBox>
                                                </div>

                                            </td>
                                            <td style="padding-right: 30px; padding-left: 5px"><strong>Total Amount</strong></td>
                                            <td>
                                                <div>
                                                    <dxe:ASPxTextBox ID="txtTotalAmount" runat="server" Width="100%" ClientInstanceName="ctxtTotalAmount" Text="0.00" HorizontalAlign="Right" Font-Size="12px" ReadOnly="true">
                                                        <MaskSettings Mask="&lt;-999999999..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                        <%--<MaskSettings Mask="<0..999999999999g>.<0..99g>" />--%>
                                                    </dxe:ASPxTextBox>
                                                </div>
                                            </td>

                                        </tr>
                                    </table>
                                </div>
                                <div class="col-sm-2" style="padding-top: 8px;">
                                    <span></span>
                                </div>
                                <div class="col-sm-4">
                                </div>
                                <div class="col-sm-2" style="padding-top: 8px;">
                                    <span></span>
                                </div>
                                <div class="col-sm-4">
                                </div>
                            </div>
                        </dxe:PopupControlContentControl>
                    </ContentCollection>
                    <HeaderStyle BackColor="LightGray" ForeColor="Black" />
                </dxe:ASPxPopupControl>
                <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A3" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
                </dxe:ASPxGridViewExporter>
                <dxe:ASPxPopupControl ID="Popup_Warehouse" runat="server" ClientInstanceName="cPopup_Warehouse"
                    Width="900px" HeaderText="Warehouse Details" PopupHorizontalAlign="WindowCenter" Height="500px"
                    BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
                    Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
                    ContentStyle-CssClass="pad">
                    <ClientSideEvents Closing="function(s, e) {
	closeWarehouse(s, e);}" />
                    <ContentStyle VerticalAlign="Top" CssClass="pad">
                    </ContentStyle>
                    <ContentCollection>
                        <dxe:PopupControlContentControl runat="server">
                            <div class="Top clearfix">
                                <div id="content-5" class="pull-right wrapHolder content horizontal-images" style="width: 100%; margin-right: 0px;">
                                    <ul>
                                        <li>
                                            <div class="lblHolder">
                                                <table>
                                                    <tbody>
                                                        <tr>
                                                            <td>Selected Product</td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblProductName" runat="server"></asp:Label></td>
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
                                                            <td>Entered Quantity </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="txt_SalesAmount" runat="server"></asp:Label>
                                                                <asp:Label ID="txt_SalesUOM" runat="server"></asp:Label>

                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </li>
                                        <li>
                                            <div class="lblHolder" id="divpopupAvailableStock">
                                                <table>
                                                    <tbody>
                                                        <tr>
                                                            <td>Available Stock</td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblAvailableStock" runat="server"></asp:Label>
                                                                <asp:Label ID="lblAvailableStockUOM" runat="server"></asp:Label>

                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </li>




                                        <li style="display: none;">
                                            <div class="lblHolder">
                                                <table>
                                                    <tbody>
                                                        <tr>
                                                            <td>Stock Quantity </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="txt_StockAmount" runat="server"></asp:Label>
                                                                <asp:Label ID="txt_StockUOM" runat="server"></asp:Label></td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </li>

                                    </ul>
                                </div>

                                <div class="clear">
                                    <br />
                                </div>
                                <div class="clearfix col-md-12" style="background: #f5f4f3; padding: 8px 0; margin-bottom: 15px; border-radius: 4px; border: 1px solid #ccc;">
                                    <div>
                                        <div class="col-md-3" id="div_Warehouse">
                                            <div style="margin-bottom: 5px;">
                                                Warehouse
                                            </div>
                                            <div class="Left_Content" style="">
                                                <dxe:ASPxComboBox ID="CmbWarehouse" EnableIncrementalFiltering="True" ClientInstanceName="cCmbWarehouse" SelectedIndex="0"
                                                    TextField="WarehouseName" ValueField="WarehouseID" runat="server" Width="100%" OnCallback="CmbWarehouse_Callback">
                                                    <ClientSideEvents ValueChanged="function(s,e){CmbWarehouse_ValueChange()}" EndCallback="CmbWarehouseEndCallback"></ClientSideEvents>
                                                </dxe:ASPxComboBox>
                                                <span id="spnCmbWarehouse" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                            </div>
                                        </div>
                                        <div class="col-md-3" id="div_Batch">
                                            <div style="margin-bottom: 5px;">
                                                Batch/Lot
                                            </div>
                                            <div class="Left_Content" style="">
                                                <dxe:ASPxComboBox ID="CmbBatch" EnableIncrementalFiltering="True" ClientInstanceName="cCmbBatch"
                                                    TextField="BatchName" ValueField="BatchID" runat="server" Width="100%" OnCallback="CmbBatch_Callback">
                                                    <ClientSideEvents ValueChanged="function(s,e){CmbBatch_ValueChange()}" EndCallback="CmbBatchEndCall"></ClientSideEvents>
                                                </dxe:ASPxComboBox>
                                                <span id="spnCmbBatch" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                            </div>
                                        </div>
                                        <div class="col-md-4" id="div_Serial">
                                            <div style="margin-bottom: 5px;">
                                                Serial No  (
                                                <input type="checkbox" id="myCheck" name="BarCode" onchange="AutoCalculateMandateOnChange(this)">Barcode )
                                            </div>
                                            <div class="" id="divMultipleCombo">
                                                <%--<dxe:ASPxComboBox ID="CmbSerial" EnableIncrementalFiltering="True" ClientInstanceName="cCmbSerial"
                                                    TextField="SerialName" ValueField="SerialID" runat="server" Width="100%" OnCallback="CmbSerial_Callback">
                                                </dxe:ASPxComboBox>--%>
                                                <dxe:ASPxDropDownEdit ClientInstanceName="checkComboBox" ID="ASPxDropDownEdit1" Width="85%" CssClass="pull-left" runat="server" AnimationType="None">
                                                    <DropDownWindowStyle BackColor="#EDEDED" />
                                                    <DropDownWindowTemplate>
                                                        <dxe:ASPxListBox Width="100%" ID="listBox" ClientInstanceName="checkListBox" SelectionMode="CheckColumn" OnCallback="CmbSerial_Callback"
                                                            runat="server">
                                                            <Border BorderStyle="None" />
                                                            <BorderBottom BorderStyle="Solid" BorderWidth="1px" BorderColor="#DCDCDC" />

                                                            <ClientSideEvents SelectedIndexChanged="OnListBoxSelectionChanged" EndCallback="listBoxEndCall" />
                                                        </dxe:ASPxListBox>
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td style="padding: 4px">
                                                                    <dxe:ASPxButton ID="ASPxButton4" AutoPostBack="False" UseSubmitBehavior="false" runat="server" Text="Close" Style="float: right">
                                                                        <ClientSideEvents Click="function(s, e){ checkComboBox.HideDropDown(); }" />
                                                                    </dxe:ASPxButton>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </DropDownWindowTemplate>
                                                    <ClientSideEvents TextChanged="SynchronizeListBoxValues" DropDown="SynchronizeListBoxValues" GotFocus="function(s, e){ s.ShowDropDown(); }" />
                                                </dxe:ASPxDropDownEdit>
                                                <span id="spncheckComboBox" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                                <div class="pull-left">
                                                    <i class="fa fa-commenting" id="abpl" aria-hidden="true" style="font-size: 16px; cursor: pointer; margin: 3px 0 0 5px;" title="Serial No " data-container="body" data-toggle="popover" data-placement="right" data-content=""></i>
                                                </div>
                                            </div>
                                            <div class="" id="divSingleCombo" style="display: none;">
                                                <dxe:ASPxTextBox ID="txtserial" runat="server" Width="85%" ClientInstanceName="ctxtserial" HorizontalAlign="Left" Font-Size="12px" MaxLength="49">
                                                    <ClientSideEvents TextChanged="txtserialTextChanged" />
                                                </dxe:ASPxTextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-3" id="div_Quantity">
                                            <div style="margin-bottom: 2px;">
                                                Quantity
                                            </div>
                                            <div class="Left_Content" style="">
                                                <dxe:ASPxTextBox ID="txtQuantity" runat="server" ClientInstanceName="ctxtQuantity" HorizontalAlign="Right" DisplayFormatString="0.0000" Font-Size="12px" Width="100%" Height="15px">
                                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" IncludeLiterals="DecimalSymbol" />
                                                    <%--<ClientSideEvents TextChanged="function(s, e) {SaveWarehouse();}" />--%>
                                                </dxe:ASPxTextBox>
                                                <span id="spntxtQuantity" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                            </div>
                                        </div>
                                        <div class="col-md-3" id="div_AltQuantity">
                                            <div style="margin-bottom: 2px;">
                                                Alt. Quantity
                                            </div>
                                            <div class="Left_Content" style="">
                                                <dxe:ASPxTextBox ID="txtAltQuantity" runat="server" ClientInstanceName="ctxtAltQuantity" HorizontalAlign="Right" Font-Size="12px" Width="100%" Height="15px">
                                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" IncludeLiterals="DecimalSymbol" />

                                                </dxe:ASPxTextBox>

                                            </div>
                                            <div style="margin-bottom: 2px;">
                                                Alt. UOM
                                            </div>
                                            <dxe:ASPxComboBox ID="cmbSecondUOM" ClientEnabled="false" ClientInstanceName="ccmbSecondUOM" runat="server" SelectedIndex="0" DataSourceID="AltUomSelect"
                                                ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" ValueField="UOM_ID" TextField="UOM_Name">
                                            </dxe:ASPxComboBox>

                                        </div>

                                    </div>





                                    <div class="col-md-3">
                                        <div>
                                        </div>
                                        <div class="Left_Content" style="padding-top: 14px">
                                            <dxe:ASPxButton ID="btnWarehouse" ClientInstanceName="cbtnWarehouse" UseSubmitBehavior="false" Width="50px" runat="server" AutoPostBack="False" Text="Add" CssClass="btn btn-primary">
                                                <ClientSideEvents Click="function(s, e) {SaveWarehouse();}" />
                                            </dxe:ASPxButton>
                                            <button id="btnSecondUOM" type="button" onclick="AlternateUOMDetails('POS')" class="btn btn-success">2nd UOM</button>
                                        </div>
                                    </div>
                                </div>

                            </div>
                            <div class="clearfix">
                                <dxe:ASPxGridView ID="GrdWarehouse" runat="server" KeyFieldName="SrlNo" AutoGenerateColumns="False"
                                    Width="100%" ClientInstanceName="cGrdWarehouse" OnCustomCallback="GrdWarehouse_CustomCallback" OnDataBinding="GrdWarehouse_DataBinding"
                                    SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="200" SettingsBehavior-AllowSort="false">
                                    <Columns>
                                        <dxe:GridViewDataTextColumn Caption="Warehouse Name" FieldName="WarehouseName"
                                            VisibleIndex="0">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Available Quantity" FieldName="AvailableQty" Visible="false"
                                            VisibleIndex="1">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Quantity" FieldName="SalesQuantity"
                                            VisibleIndex="2">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Conversion Foctor" FieldName="ConversionMultiplier" Visible="false"
                                            VisibleIndex="3">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Stock Quantity" FieldName="StkQuantity" Visible="false"
                                            VisibleIndex="4">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Balance Stock" FieldName="BalancrStk" Visible="false"
                                            VisibleIndex="5">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Batch/Lot Number" FieldName="BatchNo"
                                            VisibleIndex="6">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Mfg Date" FieldName="MfgDate"
                                            VisibleIndex="7">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Expiry Date" FieldName="ExpiryDate"
                                            VisibleIndex="8">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Serial Number" FieldName="SerialNo"
                                            VisibleIndex="9">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn VisibleIndex="10" Width="80px">
                                            <DataItemTemplate>
                                                <a href="javascript:void(0);" onclick="fn_Edit('<%# Container.KeyValue %>')" title="Delete">
                                                    <img src="../../../assests/images/Edit.png" /></a>

                                                <a href="javascript:void(0);" onclick="fn_Deletecity('<%# Container.KeyValue %>')" title="Delete">
                                                    <img src="/assests/images/crs.png" /></a>
                                            </DataItemTemplate>
                                        </dxe:GridViewDataTextColumn>
                                    </Columns>
                                    <ClientSideEvents EndCallback="OnWarehouseEndCallback" />
                                    <SettingsPager Visible="false"></SettingsPager>
                                    <SettingsLoadingPanel Text="Please Wait..." />
                                </dxe:ASPxGridView>
                            </div>
                            <div class="clearfix">
                                <br />
                                <div style="align-content: center">
                                    <dxe:ASPxButton ID="btnWarehouseSave" UseSubmitBehavior="false" ClientInstanceName="cbtnWarehouseSave" Width="50px" runat="server" AutoPostBack="False" Text="OK" CssClass="btn btn-primary">
                                        <ClientSideEvents Click="function(s, e) {FinalWarehouse();}" />
                                    </dxe:ASPxButton>
                                </div>
                            </div>
            </div>
            </dxe:PopupControlContentControl>
                    </ContentCollection>
                    <headerstyle backcolor="LightGray" forecolor="Black" />
        </dxe:ASPxPopupControl>
    </div>
        <div>
        <asp:HiddenField ID="HdUpdateMainGrid" runat="server" />
        <asp:HiddenField ID="hdfIsDelete" runat="server" />
        <asp:HiddenField ID="hdfLookupCustomer" runat="server" />
        <asp:HiddenField ID="hdfProductID" runat="server" />
        <asp:HiddenField ID="hdfProductType" runat="server" />
        <asp:HiddenField ID="hdfProductSerialID" runat="server" />
        <asp:HiddenField ID="hdnProductQuantity" runat="server" />
        <asp:HiddenField ID="hdnRefreshType" runat="server" />
        <asp:HiddenField ID="hdnPageStatus" runat="server" />
        <asp:HiddenField ID="hdnDeleteSrlNo" runat="server" />
        <asp:HiddenField ID="hdAddOrEdit" runat="server" />
        <%--Subhra--%>
        <asp:HiddenField ID="hdntab2" runat="server"></asp:HiddenField>


        <%-- Surojit 15-03-2019 --%>
        <asp:HiddenField runat="server" ID="hdnConvertionOverideVisible" />
        <asp:HiddenField runat="server" ID="hdnShowUOMConversionInEntry" />
        <%-- Surojit 15-03-2019 --%>
        <%-- Subhra 23-04-2019 --%>
        <asp:HiddenField runat="server" ID="hdnShowOldUnitInPOS" />
        <%-- Subhra 23-04-2019 --%>
    </div>
    </div>
    <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="CallbackPanelEndCall" />
    </dxe:ASPxCallbackPanel>

    <dxe:ASPxCallbackPanel runat="server" ID="acpAvailableStock" ClientInstanceName="cacpAvailableStock" OnCallback="acpAvailableStock_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="acpAvailableStockEndCall" />
    </dxe:ASPxCallbackPanel>

    <%--End Sudip--%>

    <asp:HiddenField ID="hdnCustomerId" runat="server" />
    <asp:HiddenField ID="hdnAddressDtl" runat="server" />
    <%--Debu Section--%>

    <%--Batch Product Popup Start--%>

    <dxe:ASPxPopupControl ID="ProductpopUp" runat="server" ClientInstanceName="cProductpopUp"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Height="400"
        Width="700" HeaderText="Select Product" AllowResize="true" ResizingMode="Postponed" Modal="true">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <label><strong>Search By product Name</strong></label>
                <dxe:ASPxGridLookup ID="productLookUp" runat="server" ClientInstanceName="cproductLookUp" OnDataBinding="productLookUp_DataBinding" EnableCallbackMode="true"
                    KeyFieldName="Products_ID" Width="800" TextFormatString="{0}" ClientSideEvents-TextChanged="ProductSelected" ClientSideEvents-KeyDown="ProductlookUpKeyDown"
                    GridViewProperties-EnableCallBacks="true">

                    <Columns>
                        <dxe:GridViewDataColumn FieldName="Products_Name" Caption="Name" Width="220">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataColumn>
                        <dxe:GridViewDataColumn FieldName="IsInventory" Caption="Inventory" Width="60">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataColumn>
                        <dxe:GridViewDataColumn FieldName="HSNSAC" Caption="HSN/SAC" Width="80">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataColumn>
                        <dxe:GridViewDataColumn FieldName="ClassCode" Caption="Class" Width="200">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataColumn>
                        <dxe:GridViewDataColumn FieldName="BrandName" Caption="Brand" Width="100">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataColumn>
                        <dxe:GridViewDataColumn FieldName="sProducts_isInstall" Caption="Installation Reqd." Width="120">
                            <Settings AutoFilterCondition="Contains" />
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
                        <SettingsPager PageSize="5">
                        </SettingsPager>
                    </GridViewProperties>
                </dxe:ASPxGridLookup>

            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="Blue" Font-Bold="True" ForeColor="White" />
    </dxe:ASPxPopupControl>



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
                                                        }" />
            </dxe:ASPxImage>
        </HeaderTemplate>
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <asp:HiddenField runat="server" ID="setCurrentProdCode" />
                <asp:HiddenField runat="server" ID="HdSerialNo" />
                <asp:HiddenField runat="server" ID="HdProdGrossAmt" />
                <asp:HiddenField runat="server" ID="HdProdNetAmt" />
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
                                <MaskSettings Mask="<0..999999999>.<0..99>" AllowMouseWheel="false" />
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
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
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
                                            <MaskSettings Mask="<0..999999999999>.<0..99>" AllowMouseWheel="false" />
                                        </PropertiesTextEdit>
                                        <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="Amount" Caption="Amount" ReadOnly="true">
                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                            <ClientSideEvents LostFocus="taxAmountLostFocus" GotFocus="taxAmountGotFocus" />
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                        </PropertiesTextEdit>
                                        <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                </Columns>
                                <%--  <SettingsPager Mode="ShowAllRecords"></SettingsPager>--%>
                                <SettingsDataSecurity AllowEdit="true" />
                                <SettingsEditing Mode="Batch">
                                    <BatchEditSettings EditMode="row" />
                                </SettingsEditing>
                                <ClientSideEvents EndCallback=" cgridTax_EndCallBack " RowClick="GetTaxVisibleIndex" />

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
                                            <MaskSettings Mask="<-999999999..999999999>.<0..99>" AllowMouseWheel="false" />
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
                        <td colspan="3">
                            <div class="pull-left">
                                <asp:Button ID="Button1" runat="server" UseSubmitBehavior="false" Text="Ok" CssClass="btn btn-primary mTop" OnClientClick="return BatchUpdate();" Width="85px" />
                                <asp:Button ID="Button2" runat="server" UseSubmitBehavior="false" Text="Cancel" CssClass="btn btn-danger mTop" Width="85px" OnClientClick="cgridTax.CancelEdit(); caspxTaxpopUp.Hide(); return false;" />
                                <span id="taxroundedOf"></span>
                            </div>
                            <table class="pull-right">
                                <tr>
                                    <td style="padding-top: 10px; padding-right: 5px"><strong>Total Charges</strong></td>
                                    <td>
                                        <dxe:ASPxTextBox ID="txtTaxTotAmt" MaxLength="80" ClientInstanceName="ctxtTaxTotAmt" Text="0.00" ReadOnly="true"
                                            runat="server" Width="100%" CssClass="pull-left mTop">
                                            <MaskSettings Mask="&lt;-999999999..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
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
    <%--End debjyoti 22-12-2016--%>
    <%-- <dxe:ASPxCallbackPanel runat="server" ID="taxUpdatePanel" ClientInstanceName="ctaxUpdatePanel" OnCallback="taxUpdatePanel_Callback">
                <PanelCollection>
                    <dxe:PanelContent runat="server">
                    </dxe:PanelContent>
                </PanelCollection>
                <ClientSideEvents EndCallback="ctaxUpdatePanelEndCall" />
            </dxe:ASPxCallbackPanel>--%>



    <dxe:ASPxCallback ID="taxUpdatePanel" runat="server" OnCallback="taxUpdatePanel_Callback" ClientInstanceName="ctaxUpdatePanel">
        <ClientSideEvents EndCallback="ctaxUpdatePanelEndCall" />
    </dxe:ASPxCallback>

    <%--Debu Section End--%>
        </asp:Panel>
        <asp:HiddenField ID="hidIsLigherContactPage" runat="server" />
    </div>
    <div style="display: none">
        <dxe:ASPxDateEdit ID="dt_PlQuoteExpiry" runat="server" Date="" Width="100%" EditFormatString="dd-MM-yyyy" ClientInstanceName="tenddate">
            <ClientSideEvents DateChanged="Enddate" />
        </dxe:ASPxDateEdit>
    </div>
    <%--Compnent Tag Start--%>

    <dxe:ASPxPopupControl ID="ASPxProductsPopup" runat="server" ClientInstanceName="cProductsPopup"
        Width="900px" HeaderText="Select Tax" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
        <HeaderTemplate>
            <strong><span style="color: #fff">Select Products</span></strong>

            <dxe:ASPxImage ID="closeAspxImg" runnat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader pull-right">
                <ClientSideEvents Click="function(s, e){ 
                                                            cProductsPopup.Hide();
                                                        }" />

                <%-- <dxe:ASPxImage ID="ASPxImage3" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader pull-right">
                <ClientSideEvents Click="function(s, e){ 
                                                            cProductsPopup.Hide();
                                                        }" />--%>
            </dxe:ASPxImage>
        </HeaderTemplate>
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <div style="padding: 7px 0;">
                    <input type="button" value="Select All Products" onclick="ChangeState('SelectAll')" class="btn btn-primary"></input>
                    <input type="button" value="De-select All Products" onclick="ChangeState('UnSelectAll')" class="btn btn-primary"></input>
                    <input type="button" value="Revert" onclick="ChangeState('Revart')" class="btn btn-primary"></input>
                </div>
                <dxe:ASPxGridView runat="server" KeyFieldName="ComponentDetailsID" ClientInstanceName="cgridproducts" ID="grid_Products"
                    Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" SettingsPager-Mode="ShowAllRecords" OnCustomCallback="cgridProducts_CustomCallback"
                    Settings-ShowFooter="false" AutoGenerateColumns="False" Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Visible">
                    <SettingsBehavior AllowDragDrop="False" AllowSort="False"></SettingsBehavior>
                    <SettingsPager Visible="false"></SettingsPager>
                    <Columns>
                        <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="40" Caption=" " VisibleIndex="0" />
                        <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="SrlNo" Width="40" ReadOnly="true" Caption="Sl. No.">
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="ComponentNumber" Width="120" ReadOnly="true" Caption="Number">
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="ProductID" ReadOnly="true" Caption="Product" Width="0">
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="ProductsName" ReadOnly="true" Width="100" Caption="Product Name">
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="ProductDescription" Width="200" ReadOnly="true" Caption="Product Description">
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="Bal Quantity" FieldName="Quantity" Width="70" VisibleIndex="6">
                            <PropertiesTextEdit>
                                <MaskSettings Mask="<0..999999999999>.<0..99>" AllowMouseWheel="false" />
                            </PropertiesTextEdit>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="ComponentID" ReadOnly="true" Caption="ComponentID" Width="0">
                        </dxe:GridViewDataTextColumn>
                    </Columns>
                    <SettingsDataSecurity AllowEdit="true" />
                </dxe:ASPxGridView>
                <div class="text-center">
                    <asp:Button ID="Button3" runat="server" UseSubmitBehavior="false" Text="OK" CssClass="btn btn-primary  mLeft mTop" OnClientClick="return PerformCallToGridBind();" />
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <ContentStyle VerticalAlign="Top" CssClass="pad"></ContentStyle>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
    </dxe:ASPxPopupControl>

    <%--Compnent Tag End--%>

    <%-- Customer Payment & Recipt --%>

    <dxe:ASPxPopupControl ID="apcReciptPopup" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="capcReciptPopup" Height="630px"
        Width="1200px" HeaderText="Customer Receipt/Payment" Modal="true" AllowResize="true" ResizingMode="Postponed">
        <HeaderTemplate>
            <span>Customer Receipt/Payment</span>
        </HeaderTemplate>
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>



    <%-- UDF Module Start --%>
    <dxe:ASPxPopupControl ID="ASPXPopupControl2" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popup" Height="630px"
        Width="600px" HeaderText="Add/Modify UDF" Modal="true" AllowResize="true" ResizingMode="Postponed">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>

    <%--Customer Popup--%>
    <dxe:ASPxPopupControl ID="DirectAddCustPopup" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="AspxDirectAddCustPopup" Height="680px"
        Width="1100px" HeaderText="Add New Customer" Modal="true" AllowResize="false" ResizingMode="Postponed">

        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>


    <asp:HiddenField runat="server" ID="IsUdfpresent" />
    <asp:HiddenField runat="server" ID="Keyval_internalId" />
    <asp:HiddenField runat="server" ID="sessionBranch" />
    <asp:HiddenField runat="server" ID="HdPosType" />
    <asp:HiddenField runat="server" ID="HdViewmode" />
    <asp:HiddenField runat="server" ID="hdAddvanceReceiptNo" />
    <asp:HiddenField runat="server" ID="hdBasketId" />
    <asp:HiddenField runat="server" ID="HdDiscountAmount" />
    <asp:HiddenField runat="server" ID="isBasketContainComponent" />
    <%-- Subhra 22-05-2019 --%>
    <asp:HiddenField runat="server" ID="hdnPosDocPrintDesignBasedOnTaxCategory" />
    <%-- Subhra 22-05-2019 --%>
    <%-- UDF Module End--%>

    <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
        <ClientSideEvents ControlsInitialized="AllControlInitilize" />
    </dxe:ASPxGlobalEvents>



    <dxe:ASPxPopupControl ID="OldUnitPopUpControl" runat="server" Width="1100"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="cOldUnitPopUpControl"
        HeaderText="Old Unit Details" AllowResize="false" ResizingMode="Postponed" Modal="true">
        <ContentCollection>

            <dxe:PopupControlContentControl runat="server">

                <dxe:ASPxCallbackPanel runat="server" ID="oldUnitUpdatePanel" ClientInstanceName="coldUnitUpdatePanel" OnCallback="oldUnitUpdatePanel_Callback">
                    <PanelCollection>
                        <dxe:PanelContent runat="server">
                            <div class="row">
                                <div class="col-md-3">
                                    <label style="margin-top: 0px !important">Select Old Unit</label>

                                    <dxe:ASPxGridLookup ID="oldUnitProductLookUp" runat="server" DataSourceID="oldUnitDataSource" ClientInstanceName="coldUnitProductLookUp"
                                        KeyFieldName="sProducts_ID" Width="100%" TextFormatString="{0}+{1}" MultiTextSeparator=", ">
                                        <Columns>

                                            <dxe:GridViewDataColumn FieldName="sProducts_Code" Caption="Name" Width="100">
                                                <Settings AutoFilterCondition="Contains" />
                                            </dxe:GridViewDataColumn>

                                            <dxe:GridViewDataColumn FieldName="sProducts_Name" Caption="Description" Width="180">
                                                <Settings AutoFilterCondition="Contains" />
                                            </dxe:GridViewDataColumn>

                                            <dxe:GridViewDataColumn FieldName="sProduct_IsInventory" Caption="Inventory" Width="50">
                                                <Settings AutoFilterCondition="Contains" />
                                            </dxe:GridViewDataColumn>
                                            <dxe:GridViewDataColumn FieldName="hsnCode" Caption="HSN/SAC" Width="80">
                                                <Settings AutoFilterCondition="Contains" />
                                            </dxe:GridViewDataColumn>
                                            <dxe:GridViewDataColumn FieldName="ProductClass_Code" Caption="Class" Width="200">
                                                <Settings AutoFilterCondition="Contains" />
                                            </dxe:GridViewDataColumn>
                                            <dxe:GridViewDataColumn FieldName="MRP" Caption="MRP" Width="0">
                                                <Settings AutoFilterCondition="Contains" />
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
                                        <ClientSideEvents TextChanged="oldUnitProductTextChanged" />
                                    </dxe:ASPxGridLookup>
                                    <span id="mandetoryOldUnit" style="display: none; top: -18px; left: -95px;">
                                        <img id="mandetoryOldUnitimg" style="position: absolute; right: -2px; top: 24px;" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                    </span>
                                </div>
                                <div class="col-md-1">
                                    <label>UOM</label>
                                    <dxe:ASPxTextBox ID="txtOldUnitUom" runat="server" ClientInstanceName="ctxtOldUnitUom" Width="100%" ClientEnabled="False">
                                    </dxe:ASPxTextBox>
                                </div>
                                <div class="col-md-1">
                                    <label>Quantity</label>
                                    <dxe:ASPxTextBox ID="txtOldUnitqty" runat="server" ClientInstanceName="ctxtOldUnitqty" Width="100%">
                                        <MaskSettings Mask="<1..9999999>" AllowMouseWheel="false" />
                                    </dxe:ASPxTextBox>
                                </div>
                                <div class="col-md-2">
                                    <label>Value</label>
                                    <dxe:ASPxTextBox ID="txtoldUnitValue" runat="server" ClientInstanceName="ctxtoldUnitValue" Width="100%">
                                        <MaskSettings Mask="<0..9999999>.<0..99>" AllowMouseWheel="false" />
                                    </dxe:ASPxTextBox>
                                </div>





                                <div class="col-md-5 pdTop15">

                                    <dxe:ASPxButton ID="oldUnitGridAdd" runat="server" Text="Add" UseSubmitBehavior="false" AutoPostBack="false" CssClass="btn btn-primary mTop16">
                                        <ClientSideEvents Click="oldUnitGridAddClick" />
                                    </dxe:ASPxButton>

                                    <dxe:ASPxButton ID="ASPxButton8" runat="server" Text="Clear" UseSubmitBehavior="false" AutoPostBack="false" CssClass="btn btn-danger mTop16">
                                        <ClientSideEvents Click="oldUnitGridClearClick" />
                                    </dxe:ASPxButton>
                                </div>
                            </div>
                            <div class="clear"></div>


                            <asp:SqlDataSource runat="server" ID="oldUnitDataSource"
                                SelectCommand="prc_PosSalesInvoice" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:Parameter Type="String" Name="action" DefaultValue="OldUnitProductDetails" />
                                </SelectParameters>
                            </asp:SqlDataSource>


                        </dxe:PanelContent>
                    </PanelCollection>
                </dxe:ASPxCallbackPanel>


                <div class="GridViewArea">
                    <dxe:ASPxGridView ID="OldUnitGrid" runat="server" AutoGenerateColumns="False" ClientInstanceName="cOldUnitGrid"
                        Width="100%" OnCustomCallback="OldUnitGrid_CustomCallback" CssClass="pull-left" KeyFieldName="oldUnit_id">
                        <Columns>

                            <dxe:GridViewDataTextColumn Caption="Product Details" FieldName="Product_Des" ReadOnly="True"
                                Visible="True" FixedStyle="Left" VisibleIndex="0">
                                <EditFormSettings Visible="True" />
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Caption="UOM" FieldName="oldUnit_Uom" ReadOnly="True"
                                Visible="True" FixedStyle="Left" VisibleIndex="0">
                                <EditFormSettings Visible="True" />
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Caption="Quantity" FieldName="oldUnit_qty" ReadOnly="True"
                                Visible="True" FixedStyle="Left" VisibleIndex="0">
                                <EditFormSettings Visible="True" />
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Caption="Value" FieldName="oldUnit_value" ReadOnly="True"
                                Visible="True" FixedStyle="Left" VisibleIndex="0">
                                <EditFormSettings Visible="True" />
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>



                            <dxe:GridViewDataTextColumn ReadOnly="False" Width="12%" CellStyle-HorizontalAlign="Center">
                                <HeaderStyle HorizontalAlign="Center" />
                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                <HeaderTemplate>
                                    Actions
                                </HeaderTemplate>
                                <DataItemTemplate>

                                    <%if (hdAddOrEdit.Value != "Edit")
                                      { %>
                                    <a href="javascript:void(0);" onclick="fn_EditOldUnit('<%# Container.KeyValue %>')" title="Edit" class="pad">
                                        <img src="/assests/images/Edit.png" /></a>

                                    <a href="javascript:void(0);" onclick="fn_removeOldUnit('<%# Container.KeyValue %>')" title="Delete" class="pad">
                                        <img src="/assests/images/Delete.png" /></a>
                                    <%} %>
                                </DataItemTemplate>
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                        <ClientSideEvents EndCallback="OldUnitGridEndCallback"></ClientSideEvents>
                    </dxe:ASPxGridView>
                </div>
                <div class="clear"></div>
                <div style="padding-top: 5px;">
                    <dxe:ASPxButton ID="oldunitPopupSaveAndClickClick" UseSubmitBehavior="false" ClientInstanceName="coldunitPopupSaveAndClickClick" runat="server" Text="Ok" AutoPostBack="false" CssClass="btn btn-primary mTop16">
                        <ClientSideEvents Click="oldunitPopupSaveAndEXitClick" />
                    </dxe:ASPxButton>
                </div>


            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>









    <dxe:ASPxPopupControl ID="popupCustomerRecipt" runat="server" Width="1100"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="cpopupCustomerRecipt"
        HeaderText="Customer Receipt / Return" AllowResize="false" ResizingMode="Postponed" Modal="true">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">

                <dxe:ASPxGridView ID="aspxCustomerReceiptGridview" runat="server" AutoGenerateColumns="False" ClientInstanceName="caspxCustomerReceiptGridview"
                    Width="100%" OnCustomCallback="aspxCustomerReceiptGridview_CustomCallback" CssClass="pull-left" KeyFieldName="ReceiptPayment_ID"
                    SettingsPager-Mode="ShowAllRecords" OnDataBinding="aspxCustomerReceiptGridview_DataBinding">
                    <Columns>

                        <dxe:GridViewCommandColumn ShowSelectCheckbox="true" Width="50" Caption="Select" />

                        <dxe:GridViewDataTextColumn Caption="Type" FieldName="Vc_type" ReadOnly="True"
                            Visible="True" FixedStyle="Left" VisibleIndex="0">
                            <EditFormSettings Visible="True" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>


                        <dxe:GridViewDataTextColumn Caption="Customer Name" FieldName="CustomerName" ReadOnly="True"
                            Visible="True" FixedStyle="Left" VisibleIndex="0">
                            <EditFormSettings Visible="True" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Voucher / Return Number" FieldName="ReceiptPayment_VoucherNumber" ReadOnly="True"
                            Visible="True" FixedStyle="Left" VisibleIndex="0">
                            <EditFormSettings Visible="True" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Transaction Date" FieldName="ReceiptPayment_TransactionDate" ReadOnly="True"
                            Visible="True" FixedStyle="Left" VisibleIndex="0">
                            <EditFormSettings Visible="True" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>


                        <dxe:GridViewDataTextColumn Caption="Amount" FieldName="Curent_Available_Amount" ReadOnly="True"
                            Visible="True" FixedStyle="Left" VisibleIndex="0">
                            <HeaderStyle HorizontalAlign="Right" />
                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <EditFormSettings Visible="True" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>


                    </Columns>
                    <ClientSideEvents EndCallback="CustomerReceiptEndCallback"></ClientSideEvents>
                </dxe:ASPxGridView>
                <div id="customerReceiptButtonSet">
                    <input type="button" value="Select All" onclick="SelectAllCustomerReceipt()" style="margin-top: 10px;" class="btn  btn-primary" />
                    <input type="button" value="Un-Select All" onclick="UnSelectAllCustomerReceipt()" style="margin-top: 10px;" class="btn  btn-primary" />
                    <input type="button" value="Revert" onclick="RevertCustomerReceipt()" style="margin-top: 10px;" class="btn  btn-primary" />

                    <input type="button" value="Save & Exit" onclick="CustomerReceiptSaveandExitClick()" style="margin-top: 10px;" class="btn btn-success" />
                </div>

            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>



    <dxe:ASPxPopupControl ID="ShowAvailableStock" runat="server" Width="1000"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="cShowAvailableStock" Height="500"
        HeaderText="Show Available Stock" AllowResize="false" ResizingMode="Postponed">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">

                <dxe:ASPxGridView ID="AvailableStockgrid" runat="server" KeyFieldName="branch_id" AutoGenerateColumns="False"
                    Width="100%" ClientInstanceName="cAvailableStockgrid" OnCustomCallback="AvailableStockgrid_CustomCallback" KeyboardSupport="true"
                    SettingsBehavior-AllowSelectSingleRowOnly="true" OnDataBinding="AvailableStockgrid_DataBinding" SettingsBehavior-AllowFocusedRow="true"
                    OnHtmlRowPrepared="AvailableStockgrid_HtmlRowPrepared">
                    <Columns>


                        <dxe:GridViewDataTextColumn Caption="Branch Code" FieldName="branch_code"
                            VisibleIndex="0" FixedStyle="Left">
                            <CellStyle CssClass="gridcellleft" Wrap="true">
                            </CellStyle>
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Name" FieldName="branch_description"
                            VisibleIndex="0" FixedStyle="Left">
                            <CellStyle CssClass="gridcellleft" Wrap="true">
                            </CellStyle>
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Opening" FieldName="IN_QTY_OP"
                            VisibleIndex="0" FixedStyle="Left">
                            <CellStyle CssClass="gridcellleft" Wrap="true">
                            </CellStyle>
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="In Quantity" FieldName="IN_QTY"
                            VisibleIndex="0" FixedStyle="Left">
                            <CellStyle CssClass="gridcellleft" Wrap="true">
                            </CellStyle>
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Out Quantity" FieldName="OUT_QTY"
                            VisibleIndex="0" FixedStyle="Left">
                            <CellStyle CssClass="gridcellleft" Wrap="true">
                            </CellStyle>
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Display" FieldName="DisplayCount"
                            VisibleIndex="0" FixedStyle="Left">
                            <CellStyle CssClass="gridcellleft" Wrap="true">
                            </CellStyle>
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Dent" FieldName="dentCount"
                            VisibleIndex="0" FixedStyle="Left">
                            <CellStyle CssClass="gridcellleft" Wrap="true">
                            </CellStyle>
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <%--    <dxe:GridViewDataTextColumn Caption="Stolen" FieldName="stolenCount"
                            VisibleIndex="0" FixedStyle="Left">
                            <CellStyle CssClass="gridcellleft" Wrap="true">
                            </CellStyle>
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>--%>


                        <dxe:GridViewDataTextColumn Caption="Available Quantity" FieldName="Available"
                            VisibleIndex="0" FixedStyle="Left">
                            <CellStyle CssClass="gridcellleft" Wrap="true">
                            </CellStyle>
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                    </Columns>



                    <SettingsSearchPanel Visible="True" />
                    <Settings ShowGroupPanel="False" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                    <SettingsLoadingPanel Text="Please Wait..." />



                    <SettingsPager PageSize="15">
                        <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                    </SettingsPager>
                </dxe:ASPxGridView>




            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>
    <asp:HiddenField runat="server" ID="HDSelectedProduct" />


    <dxe:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" ContainerElementID="divSubmitButton" Text="Please wait.."
        Modal="True">
    </dxe:ASPxLoadingPanel>


    <asp:HiddenField runat="server" ID="uniqueId" />



    <asp:HiddenField ID="DeleteCustomer" runat="server"></asp:HiddenField>


    <asp:HiddenField runat="server" ID="hdHsnList" />



    <!--Customer Modal -->
    <div class="modal fade" id="CustModel" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Customer Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Customerkeydown(event)" id="txtCustSearch" autofocus width="100%" placeholder="Search By Customer Name,Unique Id and Phone No." />

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




    <!--Product Modal -->
    <div class="modal fade" id="ProductModel" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Product Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="prodkeydown(event)" id="txtProdSearch" autofocus width="100%" placeholder="Search By Product Name or Description" />

                    <div id="ProductTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Product Name</th>
                                <th>Product Description</th>
                                <th>HSN/SAC</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <% if (rightsProd.CanAdd)
                       { %>
                    <button type="button" class="btn btn-success btn-radius" onclick="fn_ProductPopOpen();">
                        <span class="btn-icon"><i class="fa fa-plus"></i></span>
                        Add New
                    </button>
                    <% } %>
                    <button type="button" class="btn btn-danger btn-radius" data-dismiss="modal">Close</button>
                </div>
            </div>

        </div>
    </div>


    <!--Select Address Modal -->
    <div class="modal fade" id="addressModel" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Select Address</h4>
                </div>
                <div class="modal-body">
                    <div id="AddressTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Address</th>
                                <th>Address Type</th>
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




    <dxe:ASPxPopupControl ID="SecondUOMpopup" runat="server" ClientInstanceName="cSecondUOM" ShowCloseButton="false"
        Width="850px" HeaderText="Second UOM Details" PopupHorizontalAlign="WindowCenter"
        BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <div class="clearfix boxStyle hide">
                    <div class="col-md-3">
                        <label>Length</label>
                        <dxe:ASPxTextBox runat="server" ID="txtLength" ClientInstanceName="ctxtLength">
                            <ClientSideEvents LostFocus="SizeLostFocus" />

                            <MaskSettings Mask="&lt;0..99999999999&gt;.&lt;00..99&gt;" />

                        </dxe:ASPxTextBox>
                    </div>
                    <div class="col-md-3">
                        <label>Width</label>
                        <dxe:ASPxTextBox runat="server" ID="txtWidth" ClientInstanceName="ctxtWidth">
                            <ClientSideEvents LostFocus="SizeLostFocus" />
                            <MaskSettings Mask="&lt;0..99999999999&gt;.&lt;00..99&gt;" />

                        </dxe:ASPxTextBox>
                    </div>
                    <div class="col-md-3">
                        <label>Total</label>
                        <dxe:ASPxTextBox runat="server" ID="txtTotal" ClientEnabled="false" ClientInstanceName="ctxtTotal">
                            <MaskSettings Mask="&lt;0..99999999999&gt;.&lt;00..99&gt;" />
                        </dxe:ASPxTextBox>
                    </div>
                    <div class="col-md-3 padTop23 pdLeft0">
                        <label></label>
                        <button type="button" onclick="AddSecondUOMDetails();" class="btn btn-primary">Add</button>
                    </div>
                </div>
                <div class="clearfix"></div>
                <div class="col-md-12">
                    <table id="dataTbl" class="display nowrap" style="width: 100%">
                        <thead>
                            <tr>
                                <th class="hide">GUID</th>
                                <th class="hide">WarehouseID</th>
                                <th class="hide">ProductId</th>
                                <th>Selection</th>
                                <th>SL</th>
                                <th>Branch</th>
                                <th>Warehouse</th>
                                <th>Size</th>
                                <th>Total</th>

                            </tr>
                        </thead>
                        <tbody id="tbodySecondUOM">
                        </tbody>
                    </table>
                </div>
                <div class="col-md-12 text-right pdTop15">
                    <button class="btn btn-success" type="button" onclick="SaveSecondUOMDetails();">OK</button>
                    <button class="btn btn-danger hide" type="button" onclick="return cSecondUOM.Hide();">Cancel</button>
                </div>
                <asp:HiddenField ID="hfDocId" runat="server" />
            </dxe:PopupControlContentControl>
        </ContentCollection>

    </dxe:ASPxPopupControl>


    <dxe:ASPxPopupControl ID="ASPxPopupControl1" runat="server" ClientInstanceName="cOutstandingPopup"
        Width="1300px" HeaderText="Select Tax" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
        <HeaderTemplate>
            <strong><span style="color: #fff">Customer Outstanding</span></strong>

            <dxe:ASPxImage ID="ASPxImage2" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader pull-right">
                <ClientSideEvents Click="function(s, e){ 
                                                            cOutstandingPopup.Hide();
                                                        }" />
            </dxe:ASPxImage>
        </HeaderTemplate>
        <ContentCollection>

            <dxe:PopupControlContentControl runat="server">
                <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="cmbExport1_SelectedIndexChanged" AutoPostBack="true">
                    <asp:ListItem Value="0">Export to</asp:ListItem>
                    <asp:ListItem Value="1">PDF</asp:ListItem>
                    <asp:ListItem Value="2">XLSX</asp:ListItem>
                    <asp:ListItem Value="3">RTF</asp:ListItem>
                    <asp:ListItem Value="4">CSV</asp:ListItem>
                </asp:DropDownList>
                <dxe:ASPxGridView runat="server" KeyFieldName="SLNO" ClientInstanceName="cCustomerOutstanding" ID="CustomerOutstanding"
                    DataSourceID="EntityServerModeDataSource" OnSummaryDisplayText="ShowGridCustOut_SummaryDisplayText"
                    Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" OnCustomCallback="cCustomerOutstanding_CustomCallback"
                    Settings-ShowFooter="true" AutoGenerateColumns="False" Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Visible"
                    OnHtmlFooterCellPrepared="ShowGridCustOut_HtmlFooterCellPrepared" OnHtmlDataCellPrepared="ShowGridCustOut_HtmlDataCellPrepared">

                    <SettingsBehavior AllowDragDrop="true" AllowSort="true"></SettingsBehavior>
                    <SettingsPager Visible="true"></SettingsPager>
                    <Columns>
                        <dxe:GridViewDataTextColumn Caption="Customer Name" FieldName="PARTYNAME" GroupIndex="0"
                            VisibleIndex="0">
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="BRANCH_DESCRIPTION" Width="55%" ReadOnly="true" Caption="UNIT">
                        </dxe:GridViewDataTextColumn>
                        <%--<dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="PARTYNAME" Width="100" ReadOnly="true" Caption="Customer">
                                </dxe:GridViewDataTextColumn>--%>
                        <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="DOC_TYPE" ReadOnly="true" Caption="Doc. Type" Width="100%">
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="ISOPENING" ReadOnly="true" Caption="Opening?" Width="30%">
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="DOC_NO" ReadOnly="true" Width="95%" Caption="Document No">
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="DOC_DATE" Width="50%" ReadOnly="true" Caption="Document Date">
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="DUE_DATE" Width="50%" ReadOnly="true" Caption="Due Date">
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="DOC_AMOUNT" ReadOnly="true" Caption="Document Amt." Width="50%">
                            <CellStyle HorizontalAlign="Right">
                            </CellStyle>
                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <HeaderStyle HorizontalAlign="Right" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="8" FieldName="BAL_AMOUNT" ReadOnly="true" Caption="Balance Amount" Width="50%">
                            <CellStyle HorizontalAlign="Right">
                            </CellStyle>
                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <HeaderStyle HorizontalAlign="Right" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="9" FieldName="DAYS" Width="20%" ReadOnly="true" Caption="Days">
                        </dxe:GridViewDataTextColumn>
                    </Columns>
                    <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" AllowSort="False" EnableRowHotTrack="true" ColumnResizeMode="Control" />
                    <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
                    <SettingsEditing Mode="EditForm" />
                    <SettingsContextMenu Enabled="true" />
                    <SettingsBehavior AutoExpandAllGroups="true" />
                    <Settings ShowGroupPanel="true" ShowStatusBar="Visible" ShowFilterRow="true" />
                    <SettingsSearchPanel Visible="false" />
                    <SettingsPager PageSize="10">
                        <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                    </SettingsPager>
                    <TotalSummary>
                        <dxe:ASPxSummaryItem FieldName="DOC_TYPE" SummaryType="Custom" Tag="Item_DocType" />
                        <dxe:ASPxSummaryItem FieldName="BAL_AMOUNT" SummaryType="Custom" Tag="Item_BalAmt"></dxe:ASPxSummaryItem>
                    </TotalSummary>

                    <SettingsDataSecurity AllowEdit="true" />

                </dxe:ASPxGridView>

                <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
                    ContextTypeName="ERPDataClassesDataContext" TableName="PARTYOUTSTANDINGDET_REPORT" />
                <div style="display: none">
                    <dxe:ASPxGridViewExporter ID="exporter1" GridViewID="CustomerOutstanding" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
                    </dxe:ASPxGridViewExporter>
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <ContentStyle VerticalAlign="Top" CssClass="pad"></ContentStyle>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
    </dxe:ASPxPopupControl>
    <asp:HiddenField runat="server" ID="hddnBranchId" />
    <asp:HiddenField runat="server" ID="hddnAsOnDate" />

    <asp:HiddenField runat="server" ID="hddnOutStandingBlock" />
    <asp:HiddenField runat="server" ID="ISAllowBackdatedEntry" />
    <asp:HiddenField runat="server" ID="warehousestrProductID" />
    <%-- add on fly product by chinmoy  01-08-2019
    Start--%>

    <dxe:ASPxPopupControl ID="Popup_Empcitys" runat="server" ClientInstanceName="cPopup_Empcitys"
        Width="1000px" HeaderText="Add/Modify products" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <%--<div style="Width:400px;background-color:#FFFFFF;margin:0px;border:1px solid red;">--%>
                <div class="ProductMainContaint">

                    <div class="row ">
                        <div class="col-md-12 ">
                            <div class="col-md-6 " style="padding: 0">
                                <div class="col-md-6">
                                    <div class="cityDiv" style="height: auto; margin-bottom: 5px;">
                                        <%--Code--%>
                                            Short Name (Unique)
                                           <%-- <asp:Label ID="LblCode" runat="server" Text="Short Name (Unique)" CssClass="newLbl"></asp:Label>--%><span style="color: red;"> *</span>

                                    </div>
                                    <div class="Left_Content">
                                        <dxe:ASPxTextBox ID="txtPro_Code" MaxLength="80" ClientInstanceName="ctxtPro_Code"
                                            runat="server" Width="100%" CssClass="upper">
                                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" ValidationGroup="product" ErrorTextPosition="Right" ErrorImage-ToolTip="Mandatory" SetFocusOnError="True">
                                                <ErrorImage ToolTip="Mandatory"></ErrorImage>

                                                <RequiredField IsRequired="True" ErrorText="Mandatory" />
                                            </ValidationSettings>
                                            <ClientSideEvents TextChanged="function(s,e){fn_ctxtPro_Name_TextChanged()}" />

                                        </dxe:ASPxTextBox>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="cityDiv" style="height: auto; margin-bottom: 5px;">
                                        Name<span style="color: red;">*</span>
                                        <%--<asp:Label ID="LblName" runat="server" Text="Name" CssClass="newLbl"></asp:Label>--%>
                                    </div>
                                    <div class="Left_Content" style="">
                                        <dxe:ASPxTextBox ID="txtPro_Name" ClientInstanceName="ctxtPro_Name" runat="server" MaxLength="100"
                                            Width="100%" CssClass="upper">
                                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" ValidationGroup="product" ErrorTextPosition="Right" ErrorImage-ToolTip="Mandatory" SetFocusOnError="True">
                                                <ErrorImage ToolTip="Mandatory"></ErrorImage>

                                                <RequiredField IsRequired="True" ErrorText="Mandatory" />
                                            </ValidationSettings>
                                            <%--<ClientSideEvents TextChanged="function(s,e){fn_ctxtPro_Name_TextChanged()}" />--%>
                                        </dxe:ASPxTextBox>
                                    </div>
                                </div>

                                <%--place here--%>
                                <div class="col-md-3">
                                    <div class="cityDiv" style="height: auto;">
                                        <%--Inventory Item--%>
                                        <asp:Label ID="Label1" runat="server" Text="Inventory Item?" CssClass="newLbl"></asp:Label>
                                    </div>
                                    <div class="Left_Content">
                                        <dxe:ASPxComboBox ID="cmbIsInventory" ClientInstanceName="ccmbIsInventory" runat="server"
                                            ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0">
                                            <Items>
                                                <dxe:ListEditItem Text="Yes" Value="1" />
                                                <dxe:ListEditItem Text="No" Value="0" />
                                            </Items>

                                            <ClientSideEvents SelectedIndexChanged="isInventoryChanged" />
                                        </dxe:ASPxComboBox>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="cityDiv" style="height: auto;">
                                        <asp:Label ID="Label22" runat="server" Text="Service Item?" CssClass="newLbl"></asp:Label>
                                    </div>
                                    <div class="Left_Content">
                                        <dxe:ASPxComboBox ID="cmbServiceItem" ClientInstanceName="ccmbServiceItem" runat="server"
                                            ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0">
                                            <Items>
                                                <dxe:ListEditItem Text="No" Value="0" />
                                                <dxe:ListEditItem Text="Yes" Value="1" />

                                            </Items>


                                        </dxe:ASPxComboBox>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="cityDiv" style="height: auto;">
                                        <%--Inventory Item--%>
                                        <asp:Label ID="Label14" runat="server" Text="Capital Goods?" CssClass="newLbl"></asp:Label>
                                    </div>
                                    <div class="Left_Content">
                                        <dxe:ASPxComboBox ID="cmbIsCapitalGoods" ClientInstanceName="ccmbIsCapitalGoods" runat="server"
                                            ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="1">
                                            <Items>
                                                <dxe:ListEditItem Text="Yes" Value="1" />
                                                <dxe:ListEditItem Text="No" Value="0" />
                                            </Items>

                                            <ClientSideEvents SelectedIndexChanged="isCapitalChanged" />
                                        </dxe:ASPxComboBox>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="" style="height: auto;">
                                        <%--Description--%>
                                        <asp:Label ID="Label24" runat="server" Text="Alternate Name" CssClass="newLbl"></asp:Label>
                                    </div>
                                    <div class="Left_Content">
                                        <dxe:ASPxTextBox ID="txtPro_Printname" ClientInstanceName="ctxtPro_Printname" MaxLength="100"
                                            runat="server" Width="100%" CssClass="upper">
                                        </dxe:ASPxTextBox>
                                    </div>
                                </div>

                            </div>




                            <div class="col-md-6">
                                <div class="cityDiv" style="height: auto; margin-top: -5px;">
                                    <%--Description--%>
                                    <asp:Label ID="LblDecs" runat="server" Text="Description" CssClass="newLbl"></asp:Label>
                                </div>
                                <div class="Left_Content">
                                    <dxe:ASPxMemo ID="txtPro_Description" ClientInstanceName="ctxtPro_Description" MaxLength="300"
                                        runat="server" Width="100%" Height="60px" Text='<%# Bind("txtMarkets_Description") %>' CssClass="upper">
                                    </dxe:ASPxMemo>
                                </div>
                            </div>




                            <div class="clear"></div>
                        </div>
                        <div class="col-md-4" style="display: none">
                            <div class="">

                                <div class="imageArea" style="height: auto; margin-bottom: 5px;">
                                    <dxe:ASPxImage ID="ProdImage" runat="server" ClientInstanceName="cProdImage" CssClass="myImage">
                                    </dxe:ASPxImage>
                                </div>


                                <div class="Left_Content">
                                    <%--<dxe:ASPxCallbackPanel  ID="ASPxCallback1" runat="server" ClientInstanceName="Callback1" OnCallback="ASPxCallback1_Callback">
                                            <PanelCollection>
                                                  <dxe:PanelContent ID="PanelContent3" runat="server">
                                                         <button type="button" onclick="uploadClick()">Upload</button>
                                                       <asp:FileUpload ID="FileUpload1" runat="server"></asp:FileUpload>
                                                   
                                                      </dxe:PanelContent>
                                                </PanelCollection>
                                            </dxe:ASPxCallbackPanel>--%>

                                    <dxe:ASPxUploadControl ID="ASPxUploadControl1" runat="server" ClientInstanceName="upload1" OnFileUploadComplete="ASPxUploadControl1_FileUploadComplete"
                                        ShowProgressPanel="True" CssClass="pull-left">
                                        <ValidationSettings MaxFileSize="2194304" AllowedFileExtensions=".jpg, .jpeg, .gif, .png" ErrorStyle-CssClass="validationMessage" />
                                        <ClientSideEvents FileUploadComplete="function(s, e) { OnUploadComplete(e); }" />
                                    </dxe:ASPxUploadControl>
                                    <dxe:ASPxButton ID="btnUpload" runat="server" AutoPostBack="False" Text="Upload" ClientInstanceName="btnUpload" CssClass="pull-right btn btn-primary btn-small blll hide">
                                        <ClientSideEvents Click="function(s, e) {
                                                     upload1.Upload(); 
                                                    }"></ClientSideEvents>
                                    </dxe:ASPxButton>

                                    <asp:HiddenField runat="server" ID="fileName" />

                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="Top">


                        <%--Product Image--%>


                        <%--Product Image--%>

                        <div class="boxarea clearfix">
                            <span class="boxareaH">Miscellaneous</span>


                            <%--<div class="clear"></div>--%>
                            <%--End of Inventory Type--%>

                            <div class="col-md-3">
                                <div class="cityDiv" style="height: auto;">
                                    <%--Product Class Code--%>
                                    <asp:Label ID="LblPCcode" runat="server" Text="Class Name" CssClass="newLbl"></asp:Label>
                                </div>
                                <div class="Left_Content">
                                    <%-- <dxe:ASPxComboBox ID="CmbProClassCode" ClientInstanceName="cCmbProClassCode" runat="server" SelectedIndex="0" ClearButton-DisplayMode="Always"
                                        ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True">
                                        <ClearButton DisplayMode="Always"></ClearButton>
                                        <ClientSideEvents SelectedIndexChanged="CmbProClassCodeChanged" />
                                    </dxe:ASPxComboBox>--%>
                                    <dxe:ASPxButtonEdit ID="ProClassCode" runat="server" ReadOnly="true" ClientInstanceName="cProClassCode" Width="100%">
                                        <Buttons>
                                            <dxe:EditButton>
                                            </dxe:EditButton>
                                        </Buttons>
                                        <ClientSideEvents ButtonClick="ClassButnClick" KeyDown="Class_KeyDown" />
                                    </dxe:ASPxButtonEdit>

                                    <asp:HiddenField ID="ClassId" runat="server" />
                                </div>
                            </div>

                            <div class="col-md-3">
                                <div class="cityDiv" style="height: auto;">
                                    <%--Product Class Code--%>
                                    <asp:Label ID="Label12" runat="server" Text="Status" CssClass="newLbl"></asp:Label>
                                </div>
                                <div class="Left_Content">
                                    <dxe:ASPxComboBox ID="CmbStatus" ClientInstanceName="cCmbStatus" runat="server" SelectedIndex="0"
                                        ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True">
                                        <Items>
                                            <dxe:ListEditItem Text="Active" Value="A" />
                                            <dxe:ListEditItem Text="Dormant" Value="D" />
                                        </Items>
                                    </dxe:ASPxComboBox>
                                </div>
                            </div>

                            <div class="col-md-3">
                                <div class="cityDiv" style="height: auto;">
                                    <%--Product Class Code--%>
                                    <asp:Label ID="Label13" runat="server" Text="HSN Code" CssClass="newLbl"></asp:Label>
                                </div>
                                <div class="Left_Content">

                                    <dxe:ASPxButtonEdit ID="HSNCode" runat="server" ReadOnly="true" ClientInstanceName="cHSNCode" Width="100%">
                                        <Buttons>
                                            <dxe:EditButton>
                                            </dxe:EditButton>
                                        </Buttons>
                                        <ClientSideEvents ButtonClick="HSNButnClick" KeyDown="HSNCode_KeyDown" />
                                    </dxe:ASPxButtonEdit>

                                    <asp:HiddenField ID="hdnHSN" runat="server" />


                                </div>
                            </div>

                            <div class="col-md-3">
                                <div class="cityDiv" style="height: auto;">
                                    <asp:Label ID="Label21" runat="server" Text="Furtherance to Business" CssClass="newLbl"></asp:Label>
                                </div>
                                <div class="Left_Content">
                                    <dxe:ASPxCheckBox ID="chkFurtherance" ClientInstanceName="cchkFurtherance" runat="server">
                                    </dxe:ASPxCheckBox>
                                </div>
                            </div>


                        </div>



                        <%--Bar Code type and bar code added by Debjyoti 30-12-2016--%>
                        <%--<div class="col-md-6">
                                <div class="cityDiv" style="height: auto;">
                                    
                                    <asp:Label ID="lblBarCodeType" runat="server" Text="Barcode Type" CssClass="newLbl"></asp:Label>
                                </div>
                                <div class="Left_Content">
                                    <dxe:ASPxComboBox ID="CmbBarCodeType" ClientInstanceName="cCmbBarCodeType" runat="server" SelectedIndex="0" TabIndex="6" ClearButton-DisplayMode="Always"
                                        ValueType="System.String" Width="226px" EnableSynchronization="True" EnableIncrementalFiltering="True">
                                    </dxe:ASPxComboBox>
                                </div>
                            </div>

                             <div class="col-md-6">
                                <div class="cityDiv" style="height: auto;">
                                    
                                    <asp:Label ID="lblMpc" runat="server" Text="MPC No." CssClass="newLbl"></asp:Label>
                                </div>
                                <div class="Left_Content">
                                    <dxe:ASPxTextBox ID="txtMpcNo" ClientInstanceName="ctxtMpcNo" MaxLength="50" TabIndex="7"
                                        runat="server" Width="226px">
                                    </dxe:ASPxTextBox>
                                </div>
                            </div>
                            <div style="clear: both"></div>--%>
                        <%--Bar Code type and bar code added by Debjyoti 30-12-2016--%>


                        <%--                            <div class="col-md-6">
                                <div class="cityDiv" style="height: auto;">
                                    
                                    <asp:Label ID="LblGlobalCode" runat="server" Text="Global Code(UPC)" CssClass="newLbl"></asp:Label>
                                </div>
                                <div class="Left_Content">
                                    <dxe:ASPxTextBox ID="txtGlobalCode" ClientInstanceName="ctxtGlobalCode" MaxLength="30" TabIndex="8"
                                        runat="server" Width="226px" Text='<%# Bind("txtMarkets_Description") %>'>
                                    </dxe:ASPxTextBox>
                                </div>
                            </div>--%>
                        <div class="clear"></div>
                        <div class="col-md-3" style="display: none">
                            <div class="cityDiv" style="height: auto;">
                                <%--Quote Currency--%>
                                <asp:Label ID="LblQCurrency" runat="server" Text="Quote Currency" CssClass="newLbl"></asp:Label>
                            </div>
                            <div class="Left_Content">
                                <dxe:ASPxComboBox ID="CmbQuoteCurrency" ClientInstanceName="cCmbQuoteCurrency" runat="server" SelectedIndex="0" ClearButton-DisplayMode="Always"
                                    ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True">
                                    <ClearButton DisplayMode="Always"></ClearButton>
                                </dxe:ASPxComboBox>
                            </div>
                        </div>



                        <div class="col-md-3" style="display: none">
                            <div class="cityDiv lblmTop8" style="height: auto; margin-bottom: 5px;">
                                <span>UOM Factor <span style="color: red;">*</span></span>
                                <%--<asp:Label ID="LblQLot" runat="server" Text="Quote Lot" CssClass="newLbl"></asp:Label>--%>
                            </div>
                            <div class="Left_Content">
                                <dxe:ASPxTextBox ID="txtQuoteLot" ClientInstanceName="ctxtQuoteLot" MaxLength="8"
                                    runat="server" Width="100%" Text='<%# Bind("txtMarkets_Description") %>'>

                                    <ClientSideEvents KeyPress="function(s,e){ fn_AllowonlyNumeric(s,e);}" />
                                </dxe:ASPxTextBox>
                            </div>
                        </div>

                        <div class="col-md-3" style="display: none">
                            <div class="cityDiv" style="height: auto;">
                                <%--Quote Lot Unit<span style="color:red;"> *</span>--%>
                                <span class="newLbl">Quote UOM<span style="color: red;"> *</span></span>
                                <%--<asp:Label ID="LblQLotUnit" runat="server" Text="Quote Lot unit" CssClass="newLbl"></asp:Label>--%>
                            </div>
                            <div class="Left_Content">
                                <dxe:ASPxComboBox ID="CmbQuoteLotUnit" ClientInstanceName="cCmbQuoteLotUnit" runat="server" ClearButton-DisplayMode="Always"
                                    ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0">
                                    <ClearButton DisplayMode="Always"></ClearButton>

                                </dxe:ASPxComboBox>
                            </div>
                        </div>







                        <div class="col-md-3" style="display: none">
                            <div class="cityDiv" style="height: auto;">
                                <span class="newLbl">Sale UOM Factor<span style="color: red;"> *</span></span>
                                <%--<asp:Label ID="LblTradingLot" runat="server" Text="Trading Lot" CssClass="newLbl"></asp:Label>--%>
                            </div>
                            <div class="Left_Content">
                                <dxe:ASPxTextBox ID="txtTradingLot" ClientInstanceName="ctxtTradingLot" MaxLength="8"
                                    runat="server" Width="100%" Text='<%# Bind("txtMarkets_Description") %>'>

                                    <ClientSideEvents KeyPress="function(s,e){ fn_AllowonlyNumeric(s,e);}" />
                                </dxe:ASPxTextBox>
                            </div>
                        </div>


                        <div class="col-md-6" style="padding: 0px !important;">

                            <div class="boxarea clearfix">
                                <span class="boxareaH">Sales</span>



                                <div class="col-md-3">
                                    <div class="cityDiv" style="height: auto;">
                                        <%--Trading Lot Units--%>
                                        <%--<asp:Label ID="LblTLotUnit" runat="server" Text="Trading Lot Units" CssClass="newLbl"></asp:Label>--%>
                                        <span class="newLbl">Unit<span style="color: red;"> *</span></span>
                                    </div>
                                    <div class="Left_Content">
                                        <dxe:ASPxComboBox ID="CmbTradingLotUnits" ClientInstanceName="cCmbTradingLotUnits" runat="server" ClearButton-DisplayMode="Always"
                                            ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0">
                                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" ValidationGroup="product" ErrorTextPosition="Right" ErrorImage-ToolTip="Mandatory" SetFocusOnError="True">
                                                <ErrorImage ToolTip="Mandatory"></ErrorImage>
                                                <RequiredField IsRequired="True" ErrorText="Mandatory" />
                                            </ValidationSettings>
                                            <ClearButton DisplayMode="Always"></ClearButton>
                                            <ClientSideEvents LostFocus="cCmbTradingLotUnitsLostFocus" SelectedIndexChanged="cCmbTradingLotUnitsLostFocus" />
                                        </dxe:ASPxComboBox>
                                    </div>
                                </div>

                                <%--Debjyoti Sale price & min sale price--%>

                                <div class="col-md-3">
                                    <div class="cityDiv" style="height: auto;">
                                        <span class="newLbl pull-right">Sell @</span>
                                    </div>
                                    <div class="Left_Content">
                                        <dxe:ASPxTextBox ID="txtSalePrice" ClientInstanceName="ctxtSalePrice" HorizontalAlign="Right" MaxLength="18" DisplayFormatString="{0:0.00}"
                                            runat="server" Width="100%">

                                            <MaskSettings Mask="<0..99999999>.<0..99>" />

                                            <%-- <ClientSideEvents KeyPress="function(s,e){ fn_AllowonlyNumeric(s,e);}" />--%>
                                        </dxe:ASPxTextBox>
                                    </div>
                                </div>

                                <div class="col-md-3">
                                    <div class="cityDiv" style="height: auto;">
                                        <span class="newLbl pull-right">Minimum Sell @</span>
                                    </div>
                                    <div class="Left_Content">
                                        <dxe:ASPxTextBox ID="txtMinSalePrice" ClientInstanceName="ctxtMinSalePrice" HorizontalAlign="Right" MaxLength="18" DisplayFormatString="{0:0.00}"
                                            runat="server" Width="100%">
                                            <MaskSettings Mask="<0..99999999>.<0..99>" />
                                            <%--<ClientSideEvents KeyPress="function(s,e){ fn_AllowonlyNumeric(s,e);}" />--%>
                                        </dxe:ASPxTextBox>
                                    </div>
                                </div>

                                <div class="col-md-3">
                                    <div class="cityDiv" style="height: auto;">
                                        <span class="newLbl pull-right">MRP </span>
                                    </div>
                                    <div class="Left_Content">
                                        <dxe:ASPxTextBox ID="txtMrp" ClientInstanceName="ctxtMrp" HorizontalAlign="Right" MaxLength="18" DisplayFormatString="{0:0.00}"
                                            runat="server" Width="100%">
                                            <MaskSettings Mask="<0..99999999>.<0..99>" />
                                            <%-- <ClientSideEvents KeyPress="function(s,e){ fn_AllowonlyNumeric(s,e);}" />--%>
                                        </dxe:ASPxTextBox>
                                        <span id="mrpError" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; right: -2px; top: 27px; display: none" title="Must be greater than Min Sale Price"></span>
                                    </div>
                                </div>

                            </div>

                        </div>

                        <%--End here--%>
                        <%--<div class="clear"></div>--%>
                        <div class="col-md-6" style="padding: 0px !important;">
                            <div class="boxarea clearfix">
                                <span class="boxareaH">Purchases</span>
                                <div class="col-md-3" style="display: none">
                                    <div class="cityDiv" style="height: auto;">
                                        <%--Purchase UOM <span style="color:red;"> *</span>--%>
                                        <span class="newLbl">Purchase UOM Factor<span style="color: red;"> *</span></span>
                                        <%--<asp:Label ID="LblDeliveryLot" runat="server" Text="Delivery Lot" CssClass="newLbl"></asp:Label>--%>
                                    </div>
                                    <div class="Left_Content">
                                        <dxe:ASPxTextBox ID="txtDeliveryLot" ClientInstanceName="ctxtDeliveryLot" MaxLength="8"
                                            runat="server" Width="100%" Text='<%# Bind("txtMarkets_Description") %>'>

                                            <ClientSideEvents KeyPress="function(s,e){ fn_AllowonlyNumeric(s,e);}" />
                                        </dxe:ASPxTextBox>
                                    </div>
                                </div>

                                <div class="col-md-3">
                                    <div class="cityDiv" style="height: auto;">
                                        <%--Delivery Lot Unit--%>
                                        <%--<asp:Label ID="LblDeliveryLotUnit" runat="server" Text="Delivery Lot Unit" CssClass="newLbl"></asp:Label>--%>

                                        <span class="newLbl">Unit<span style="color: red;"> *</span></span>
                                    </div>
                                    <div class="Left_Content">
                                        <dxe:ASPxComboBox ID="CmbDeliveryLotUnit" ClientInstanceName="cCmbDeliveryLotUnit" runat="server" ClearButton-DisplayMode="Always"
                                            ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0">
                                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" ValidationGroup="product" ErrorTextPosition="Right" ErrorImage-ToolTip="Mandatory" SetFocusOnError="True">
                                                <ErrorImage ToolTip="Mandatory"></ErrorImage>
                                                <RequiredField IsRequired="True" ErrorText="Mandatory" />
                                            </ValidationSettings>
                                            <ClearButton DisplayMode="Always"></ClearButton>
                                        </dxe:ASPxComboBox>
                                    </div>
                                </div>



                                <%--Debjyoti Purchase price & MRP--%>

                                <div class="col-md-3">
                                    <div class="cityDiv" style="height: auto;">
                                        <span class="newLbl pull-right">Buy @</span>
                                    </div>
                                    <div class="Left_Content">
                                        <dxe:ASPxTextBox ID="txtPurPrice" ClientInstanceName="ctxtPurPrice" HorizontalAlign="Right" MaxLength="18" DisplayFormatString="{0:0.00}"
                                            runat="server" Width="100%">
                                            <MaskSettings Mask="<0..99999999>.<0..99>" />
                                            <ClientSideEvents KeyPress="function(s,e){ fn_AllowonlyNumeric(s,e);}" />
                                        </dxe:ASPxTextBox>
                                    </div>
                                </div>
                            </div>
                        </div>



                        <div class="clear"></div>
                        <div class="col-md-6" style="padding: 0px !important;">
                            <div class="boxarea clearfix">
                                <span class="boxareaH">Inventory</span>
                                <div class="col-md-3">
                                    <div class="cityDiv" style="height: auto;">
                                        <span class="newLbl pull-right">Min Level        </span>
                                    </div>
                                    <div class="Left_Content">
                                        <dxe:ASPxTextBox ID="txtMinLvl" ClientInstanceName="ctxtMinLvl" HorizontalAlign="Right" MaxLength="18" DisplayFormatString="{0:0.00}"
                                            runat="server" Width="100%">
                                            <MaskSettings Mask="<0..99999999>.<0..99>" />
                                            <ClientSideEvents KeyPress="function(s,e){ fn_AllowonlyNumeric(s,e);}" />
                                        </dxe:ASPxTextBox>
                                    </div>
                                </div>




                                <div class="col-md-3">
                                    <div class="cityDiv" style="height: auto;">
                                        <span class="newLbl pull-right">Max Level        </span>
                                    </div>
                                    <div class="Left_Content">
                                        <dxe:ASPxTextBox ID="txtMaxLvl" ClientInstanceName="ctxtMaxLvl" HorizontalAlign="Right" MaxLength="18" DisplayFormatString="{0:0.00}"
                                            runat="server" Width="100%">
                                            <MaskSettings Mask="<0..99999999>.<0..99>" />
                                            <ClientSideEvents KeyPress="function(s,e){ fn_AllowonlyNumeric(s,e);}" />
                                        </dxe:ASPxTextBox>
                                    </div>
                                </div>





                                <div class="col-md-3">
                                    <div class="cityDiv" style="height: auto;">
                                        <asp:Label ID="Label15" runat="server" Text="Reorder Level" CssClass="newLbl pull-right"></asp:Label>
                                    </div>
                                    <div class="Left_Content">
                                        <dxe:ASPxTextBox ID="txtReorderLvl" ClientInstanceName="ctxtReorderLvl" HorizontalAlign="Right" MaxLength="18" DisplayFormatString="{0:0.00}"
                                            runat="server" Width="100%">
                                            <ClientSideEvents KeyPress="function(s,e){ fn_AllowonlyNumeric(s,e);}" />
                                            <%--<MaskSettings Mask="&lt;0..9999999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />--%>
                                            <%--<MaskSettings Mask="&lt;0..9999999999g&gt;.&lt;00..99&gt;" />--%>
                                            <MaskSettings Mask="<0..99999999>.<0..99>" />

                                        </dxe:ASPxTextBox>
                                        <span id="reOrderError" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; right: -6px; top: 29px; display: none" title="Must be greater than Min level"></span>
                                    </div>
                                </div>

                                <div class="col-md-3">
                                    <div class="cityDiv" style="height: auto;">
                                        <asp:Label ID="Label23" runat="server" Text="Reorder Quantity" CssClass="newLbl pull-right"></asp:Label>
                                    </div>
                                    <div class="Left_Content">
                                        <dxe:ASPxTextBox ID="txtReorderQty" ClientInstanceName="ctxtReorderQty" MaxLength="18" HorizontalAlign="Right" DisplayFormatString="{0:0.00}"
                                            runat="server" Width="100%">
                                            <ClientSideEvents KeyPress="function(s,e){ fn_AllowonlyNumeric(s,e);}" />
                                            <%--<MaskSettings Mask="&lt;0..9999999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />--%>
                                            <MaskSettings Mask="&lt;0..9999999999g&gt;.&lt;00..99&gt;" />
                                        </dxe:ASPxTextBox>
                                        <span id="reOrderQuantityError" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; right: -6px; top: 29px; display: none" title="Must be greater than Min level"></span>
                                    </div>
                                </div>




                                <div class="clear"></div>




                                <div class="col-md-3">
                                    <div class="cityDiv" style="height: auto;">
                                        <asp:Label ID="Label16" runat="server" Text="Negative Stock" CssClass="newLbl"></asp:Label>
                                    </div>
                                    <div class="Left_Content">
                                        <dxe:ASPxComboBox ID="cmbNegativeStk" ClientInstanceName="ccmbNegativeStk" runat="server"
                                            ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0">
                                            <Items>
                                                <dxe:ListEditItem Text="Warn" Value="W" />
                                                <dxe:ListEditItem Text="Ignore" Value="I" />
                                                <dxe:ListEditItem Text="Block" Value="B" />
                                            </Items>
                                        </dxe:ASPxComboBox>
                                    </div>
                                </div>




                                <%--Debjyoti Add Inventory Type--%>

                                <div class="col-md-3">
                                    <div class="cityDiv" style="height: auto;">
                                        <%--Type--%>
                                        <asp:Label ID="Label17" runat="server" Text="Type" CssClass="newLbl"></asp:Label>
                                    </div>
                                    <div class="Left_Content">
                                        <dxe:ASPxComboBox ID="CmbProType" ClientInstanceName="cCmbProType" runat="server" ClearButton-DisplayMode="Always"
                                            ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0">
                                            <ClearButton DisplayMode="Always"></ClearButton>
                                        </dxe:ASPxComboBox>
                                    </div>
                                </div>

                                <%--End of Inventory Type--%>
                                <%--Debjyoti Stock Valuation Tech.--%>
                                <div class="col-md-3">
                                    <div class="cityDiv" style="height: auto;">
                                        <%--Inventory Item--%>
                                        <asp:Label ID="Label18" runat="server" Text="Stock Valuation Tech." CssClass="newLbl"></asp:Label>
                                    </div>
                                    <div class="Left_Content">
                                        <dxe:ASPxComboBox ID="CmbStockValuation" ClientInstanceName="cCmbStockValuation" runat="server"
                                            ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="1">
                                            <Items>
                                                <dxe:ListEditItem Text="LIFO" Value="L" />
                                                <dxe:ListEditItem Text="FIFO" Value="F" />
                                                <dxe:ListEditItem Text="Average" Value="A" />
                                                <%--<dxe:ListEditItem Text="RATED" Value="R" />--%>
                                            </Items>
                                        </dxe:ASPxComboBox>
                                    </div>
                                </div>

                                <div class="col-md-3">
                                    <div class="cityDiv" style="height: auto;">
                                        <%--  <asp:Label ID="Label4" runat="server" Text="Stock UOM" CssClass="newLbl"></asp:Label>--%>
                                        <span class="newLbl">Stock Unit<span style="color: red;"> *</span></span>
                                    </div>
                                    <div class="Left_Content">
                                        <dxe:ASPxComboBox ID="cmbStockUom" ClientInstanceName="ccmbStockUom" runat="server" ClearButton-DisplayMode="Always"
                                            ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0">
                                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" ValidationGroup="product" ErrorTextPosition="Right" ErrorImage-ToolTip="Mandatory" SetFocusOnError="True">
                                                <ErrorImage ToolTip="Mandatory"></ErrorImage>
                                                <RequiredField IsRequired="True" ErrorText="Mandatory" />
                                            </ValidationSettings>
                                            <ClearButton DisplayMode="Always"></ClearButton>
                                        </dxe:ASPxComboBox>
                                    </div>
                                </div>


                            </div>
                            <%--End here--%>
                        </div>
                        <div class="col-md-6" style="padding: 0px !important;">

                            <%-- <div style="clear: both"></div>--%>
                            <div class="boxarea clearfix">
                                <span class="boxareaH">Ledger Mapping</span>
                                <div class="col-md-3">
                                    <div class="cityDiv" style="height: auto;">
                                        <asp:Label ID="Label19" runat="server" Text="Sales" CssClass="newLbl"></asp:Label>
                                    </div>
                                    <div class="Left_Content">

                                        <dxe:ASPxButtonEdit ID="SIMainAccount" runat="server" ReadOnly="true" ClientInstanceName="cSIMainAccount" Width="100%">
                                            <Buttons>
                                                <dxe:EditButton>
                                                </dxe:EditButton>
                                            </Buttons>
                                            <ClientSideEvents ButtonClick="MainAccountButnClick" KeyDown="MainAccountKeyDown" />
                                        </dxe:ASPxButtonEdit>
                                        <%--<dxe:ASPxComboBox ID="cmbsalesInvoice" ClientInstanceName="ccmbsalesInvoice" runat="server" TabIndex="25"
                                            ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0">
                                            <ClientSideEvents SelectedIndexChanged="mainAccountSalesInvoice" GotFocus="cmbsalesInvoiceGotFocus" />
                                        </dxe:ASPxComboBox>--%>
                                    </div>
                                </div>

                                <div class="col-md-3">
                                    <div class="cityDiv" style="height: auto;">
                                        <asp:Label ID="Label20" runat="server" Text="Sales Return" CssClass="newLbl"></asp:Label>
                                    </div>
                                    <dxe:ASPxButtonEdit ID="SRMainAccount" runat="server" ReadOnly="true" ClientInstanceName="cSRMainAccount" Width="100%">
                                        <Buttons>
                                            <dxe:EditButton>
                                            </dxe:EditButton>
                                        </Buttons>
                                        <ClientSideEvents ButtonClick="SRMainAccountButnClick" KeyDown="MainAccountKeyDown" />
                                    </dxe:ASPxButtonEdit>
                                </div>

                                <div class="col-md-3">
                                    <div class="cityDiv" style="height: auto;">
                                        <asp:Label ID="Label25" runat="server" Text="Purchase" CssClass="newLbl"></asp:Label>
                                    </div>
                                    <dxe:ASPxButtonEdit ID="PIMainAccount" runat="server" ReadOnly="true" ClientInstanceName="cPIMainAccount" Width="100%">
                                        <Buttons>
                                            <dxe:EditButton>
                                            </dxe:EditButton>
                                        </Buttons>
                                        <ClientSideEvents ButtonClick="PIMainAccountButnClick" KeyDown="MainAccountKeyDown" />
                                    </dxe:ASPxButtonEdit>
                                </div>

                                <div class="col-md-3">
                                    <div class="cityDiv" style="height: auto;">
                                        <asp:Label ID="Label26" runat="server" Text="Purchase Return" CssClass="newLbl"></asp:Label>
                                    </div>
                                    <dxe:ASPxButtonEdit ID="PRMainAccount" runat="server" ReadOnly="true" ClientInstanceName="cPRMainAccount" Width="100%">
                                        <Buttons>
                                            <dxe:EditButton>
                                            </dxe:EditButton>
                                        </Buttons>
                                        <ClientSideEvents ButtonClick="PRMainAccountButnClick" KeyDown="MainAccountKeyDown" />
                                    </dxe:ASPxButtonEdit>
                                </div>

                            </div>
                        </div>
                        <div style="clear: both"></div>

                        <%--Code commented and added by debjyoti--%>
                        <%--Reason: Product attribute now showing on popup--%>
                        <div class="col-md-12">
                            <div class="cityDiv" style="height: auto;">

                                <%--<asp:Label ID="Label1" runat="server" Text="(s)" CssClass="newLbl"></asp:Label>--%>
                            </div>
                            <div class="Left_Content" style="padding-top: 10px">
                                <button type="button" class="btn btn-info btn-small" onclick="ShowProductAttribute()" id="btnProdConfig">Configure Product Attribute</button>
                                <button type="button" class="btn btn-info btn-small" onclick="ShowBarCode()" id="btnBarCodeConfig" style="display: none">Configure Barcode</button>
                                <button type="button" class="btn btn-info btn-small" onclick="ShowTaxCode()" style="display: none">Configure Tax</button>
                                <button type="button" class="btn btn-info btn-small" onclick="ShowServiceTax()" id="btnServiceTaxConfig">Configure Service Category</button>
                                <button type="button" class="btn btn-info btn-small" onclick="ShowPackingDetails()" id="btnPackingConfig">Configure UOM Conversion</button>
                                <button type="button" class="btn btn-info btn-small" onclick="ShowTdsSection()" id="btnTDS">Configure TDS Section</button>
                            </div>
                        </div>


                        <div class="col-md-2">
                            <div class="cityDiv" style="height: auto;">

                                <%--<asp:Label ID="Label7" runat="server" Text="Bar Code" CssClass="newLbl"></asp:Label>--%>
                            </div>
                            <div class="Left_Content" style="padding-top: 10px">
                            </div>
                        </div>

                        <div class="col-md-2">
                            <div class="cityDiv" style="height: auto;">

                                <%--<asp:Label ID="Label8" runat="server" Text="Tax Codes" CssClass="newLbl"></asp:Label>--%>
                            </div>
                            <div class="Left_Content" style="padding-top: 10px">
                            </div>
                        </div>

                        <div class="col-md-2">
                            <div class="cityDiv" style="height: auto;">
                            </div>
                            <div class="Left_Content" style="padding-top: 10px">
                            </div>
                        </div>

                        <div class="col-md-3">
                            <div class="cityDiv" style="height: auto;">
                            </div>
                            <div class="Left_Content" style="padding-top: 10px">
                            </div>
                        </div>


                        <%-- //......................... Code Commented and Updated  by Sam on 04-10-2014............................--%>


                        <%-- <div class="col-md-6">
                                <div class="cityDiv" style="height: auto;">
                                     
                                    <asp:Label ID="LblProductColor" runat="server" Text="Product Color" CssClass="newLbl"></asp:Label>
                                </div>
                                <div class="Left_Content">
                                    <dxe:ASPxComboBox ID="CmbProductColor" ClientInstanceName="cCmbProductColor" ClearButton-DisplayMode="Always" runat="server" TabIndex="16"
                                        ValueType="System.String" Width="226px" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0">
                                    </dxe:ASPxComboBox>
                                </div>
                            </div>


                            <div class="col-md-6" style="margin-top: 25px">
                                <div class="cityDiv" style="height: auto;">
                                    
                                </div>
                                <div class="Left_Content">
                                    
                                    <dxe:ASPxRadioButtonList ID="rdblappColor" ClientInstanceName="RrdblappColor"  runat="server" RepeatDirection="Horizontal" Width="226px" TabIndex="17">
                                        <Items>
                                             <dxe:ListEditItem Text="Applicable" Value="1" Selected="true" />
                                            <dxe:ListEditItem Text="Not Applicable" Value="0" />
                                           
                                        </Items>
                                    </dxe:ASPxRadioButtonList>
                                </div>
                            </div>--%>

                        <%-- //......................... Code Commented and Updated  by Sam on 04-10-2014............................--%>
                        <%--   <div style="clear: both"></div>

                            <div class="col-md-6" >
                                <div class="cityDiv" style="height: auto;">
                                     
                                    <asp:Label ID="LblProductSize" runat="server" Text="Product Size" CssClass="newLbl"></asp:Label>
                                </div>
                                <div class="Left_Content">
                                    <dxe:ASPxComboBox ID="CmbProductSize" ClientInstanceName="cCmbProductSize" ClearButton-DisplayMode="Always" runat="server" TabIndex="18"
                                        ValueType="System.String" Width="226px" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0">
                                    </dxe:ASPxComboBox>
                                </div>
                            </div>
                            <div class="col-md-6" style="margin-top: 25px">
                                <div class="cityDiv" style="height: auto;">
                                </div>
                                <div class="Left_Content">

                                    <dxe:ASPxRadioButtonList ID="rdblapp" ClientInstanceName="Rrdblapp" runat="server" RepeatDirection="Horizontal" Width="226px" TabIndex="19">
                                        <Items>
                                            <dxe:ListEditItem Text="Applicable" Value="1" Selected="true" />
                                            <dxe:ListEditItem Text="Not Applicable" Value="0" />
                                            
                                        </Items>
                                    </dxe:ASPxRadioButtonList>
                                </div>
                            </div>--%>
                    </div>
                    <div class="ContentDiv" style="height: auto">
                        <div style="display: none">
                            <div style="height: 20px; width: 280px; background-color: Gray; padding-left: 120px;">
                                <h5>Static Code</h5>
                            </div>
                            <div style="height: 20px; width: 130px; padding-left: 70px; background-color: Gray; float: left;">
                                Exchange
                            </div>
                            <div style="height: 20px; width: 200px; background-color: Gray; text-align: left;">
                                Value
                            </div>
                            <div class="ScrollDiv">
                                <div class="cityDiv" style="padding-top: 5px;">
                                    NSE Code
                                </div>
                                <div style="padding-top: 5px;">
                                    <dxe:ASPxTextBox ID="txtNseCode" ClientInstanceName="ctxtNseCode" runat="server"
                                        CssClass="cityTextbox">
                                    </dxe:ASPxTextBox>
                                </div>
                                <br style="clear: both;" />
                                <div class="cityDiv">
                                    BSE Code
                                </div>
                                <div>
                                    <dxe:ASPxTextBox ID="txtBseCode" ClientInstanceName="ctxtBseCode" runat="server"
                                        CssClass="cityTextbox">
                                    </dxe:ASPxTextBox>
                                </div>
                                <br style="clear: both;" />
                                <div class="cityDiv">
                                    MCX Code
                                </div>
                                <div>
                                    <dxe:ASPxTextBox ID="txtMcxCode" ClientInstanceName="ctxtMcxCode" runat="server"
                                        CssClass="cityTextbox">
                                    </dxe:ASPxTextBox>
                                </div>
                                <br style="clear: both;" />
                                <div class="cityDiv">
                                    MCXSX Code
                                </div>
                                <div>
                                    <dxe:ASPxTextBox ID="txtMcsxCode" ClientInstanceName="ctxtMcsxCode" runat="server"
                                        CssClass="cityTextbox">
                                    </dxe:ASPxTextBox>
                                </div>
                                <br style="clear: both;" />
                                <div class="cityDiv">
                                    NCDEX Code
                                </div>
                                <div>
                                    <dxe:ASPxTextBox ID="txtNcdexCode" ClientInstanceName="ctxtNcdexCode" runat="server"
                                        CssClass="cityTextbox">
                                    </dxe:ASPxTextBox>
                                </div>
                                <br style="clear: both;" />
                                <div class="cityDiv">
                                    CDSL Code
                                </div>
                                <div>
                                    <dxe:ASPxTextBox ID="txtCdslCode" ClientInstanceName="ctxtCdslCode" CssClass="cityTextbox"
                                        runat="server">
                                    </dxe:ASPxTextBox>
                                </div>
                                <br style="clear: both;" />
                                <div class="cityDiv">
                                    NSDL Code
                                </div>
                                <div>
                                    <dxe:ASPxTextBox ID="txtNsdlCode" ClientInstanceName="ctxtNsdlCode" CssClass="cityTextbox"
                                        runat="server">
                                    </dxe:ASPxTextBox>
                                </div>
                                <br style="clear: both;" />
                                <div class="cityDiv">
                                    NDML Code
                                </div>
                                <div>
                                    <dxe:ASPxTextBox ID="txtNdmlCode" ClientInstanceName="ctxtNdmlCode" runat="server"
                                        CssClass="cityTextbox">
                                    </dxe:ASPxTextBox>
                                </div>
                                <br style="clear: both;" />
                                <div class="cityDiv">
                                    CVL Code
                                </div>
                                <div>
                                    <dxe:ASPxTextBox ID="txtCvlCode" ClientInstanceName="ctxtCvlCode" runat="server"
                                        CssClass="cityTextbox">
                                    </dxe:ASPxTextBox>
                                </div>
                                <br style="clear: both;" />
                                <div class="cityDiv">
                                    DOTEX Code
                                </div>
                                <div>
                                    <dxe:ASPxTextBox ID="txtDotexCode" ClientInstanceName="ctxtDotexCode" runat="server"
                                        CssClass="cityTextbox">
                                    </dxe:ASPxTextBox>
                                </div>
                            </div>
                        </div>

                        <br style="clear: both;" />
                        <div class="col-md-12"></div>
                        <div class="Footer clearfix" style="padding-left: 16px">


                            <dxe:ASPxButton ID="ASPxButton6" CausesValidation="true" ClientInstanceName="cbtnSave_Product" runat="server" ValidationGroup="product" EncodeHtml="false"
                                AutoPostBack="False" Text="<u>S</u>ave" CssClass="btn btn-primary">
                                <ClientSideEvents Click="function (s, e) {btnSave_citys();}" />
                            </dxe:ASPxButton>


                            <dxe:ASPxButton ID="btnCancel_citys" runat="server" AutoPostBack="False" Text="<u>C</u>ancel" CssClass="btn btn-danger" EncodeHtml="false">
                                <ClientSideEvents Click="function (s, e) {fn_btnCancel();}" />
                            </dxe:ASPxButton>
                            <asp:Button ID="btnUdf" runat="server" Text="UDF" CssClass="btn btn-primary dxbButton" OnClientClick="if(OpenUdf()){ return false;}" />
                            <input type="button" value="Assing Values" style="display: none;" onclick="fetchLebel()" class="btn btn-primary" />

                            <br style="clear: both;" />
                        </div>
                        <br style="clear: both;" />
                    </div>
                    <%-- </div>--%>
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <ContentStyle VerticalAlign="Top" CssClass="pad"></ContentStyle>

        <HeaderStyle BackColor="LightGray" ForeColor="Black" />

    </dxe:ASPxPopupControl>


    <dxe:ASPxPopupControl runat="server" ID="MainAccountModelSI" ClientInstanceName="cMainAccountModelSI"
        Width="500px" Height="300px" HeaderText="Search Main Account" PopupHorizontalAlign="WindowCenter"
        BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">

        <HeaderTemplate>
            <span>Search Main Account</span>
            <dxe:ASPxImage ID="ASPxImage1" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader">
                <ClientSideEvents Click="function(s, e){ 
                                cMainAccountModelSI.Hide();
                            }" />
            </dxe:ASPxImage>
        </HeaderTemplate>


        <ContentStyle VerticalAlign="Top" CssClass="pad">
        </ContentStyle>
        <ContentCollection>
            <dxe:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                <div class="modal-body">
                    <input type="text" onkeydown="MainAccountNewkeydown(event)" id="txtMainAccountSearch" autofocus width="100%" placeholder="Search by Main Account Name or Short Name" />

                    <div id="MainAccountTable">
                        <table border='1' width="100%">
                            <tr class="HeaderStyle">
                                <th>Main Account Name</th>
                            </tr>
                        </table>
                    </div>
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <ClientSideEvents Shown="function(){ $('#txtMainAccountSearch').focus();}" />
    </dxe:ASPxPopupControl>
    <asp:HiddenField runat="server" ID="hdnSIMainAccount" />

    <dxe:ASPxPopupControl runat="server" ID="MainAccountModelSR" ClientInstanceName="cMainAccountModelSR"
        Width="500px" Height="300px" HeaderText="Search Main Account" PopupHorizontalAlign="WindowCenter"
        BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">

        <HeaderTemplate>
            <span>Search Main Account</span>
            <dxe:ASPxImage ID="ASPxImage1" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader">
                <ClientSideEvents Click="function(s, e){ 
                                cMainAccountModelSR.Hide();
                            }" />
            </dxe:ASPxImage>
        </HeaderTemplate>


        <ContentStyle VerticalAlign="Top" CssClass="pad">
        </ContentStyle>
        <ContentCollection>
            <dxe:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                <div class="modal-body">
                    <input type="text" onkeydown="MainAccountSRNewkeydown(event)" id="txtMainAccountSRSearch" autofocus width="100%" placeholder="Search by Main Account Name or Short Name" />

                    <div id="MainAccountTableSR">
                        <table border='1' width="100%">
                            <tr class="HeaderStyle">
                                <th>Main Account Name</th>
                            </tr>
                        </table>
                    </div>
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <ClientSideEvents Shown="function(){ $('#txtMainAccountSRSearch').focus();}" />
    </dxe:ASPxPopupControl>
    <asp:HiddenField runat="server" ID="hdnSRMainAccount" />


    <dxe:ASPxPopupControl runat="server" ID="MainAccountModelPI" ClientInstanceName="cMainAccountModelPI"
        Width="500px" Height="300px" HeaderText="Search Main Account" PopupHorizontalAlign="WindowCenter"
        BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">

        <HeaderTemplate>
            <span>Search Main Account</span>
            <dxe:ASPxImage ID="ASPxImage1" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader">
                <ClientSideEvents Click="function(s, e){ 
                                cMainAccountModelPI.Hide();
                            }" />
            </dxe:ASPxImage>
        </HeaderTemplate>


        <ContentStyle VerticalAlign="Top" CssClass="pad">
        </ContentStyle>
        <ContentCollection>
            <dxe:PopupControlContentControl ID="PopupControlContentControl3" runat="server">
                <div class="modal-body">
                    <input type="text" onkeydown="MainAccountPINewkeydown(event)" id="txtMainAccountPISearch" autofocus width="100%" placeholder="Search by Main Account Name or Short Name" />

                    <div id="MainAccountTablePI">
                        <table border='1' width="100%">
                            <tr class="HeaderStyle">
                                <th>Main Account Name</th>
                            </tr>
                        </table>
                    </div>
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <ClientSideEvents Shown="function(){ $('#txtMainAccountPISearch').focus();}" />
    </dxe:ASPxPopupControl>
    <asp:HiddenField runat="server" ID="hdnPIMainAccount" />
    <asp:HiddenField runat="server" ID="hdnCrDateMandatory" />
    <dxe:ASPxPopupControl runat="server" ID="MainAccountModelPR" ClientInstanceName="cMainAccountModelPR"
        Width="500px" Height="300px" HeaderText="Search Main Account" PopupHorizontalAlign="WindowCenter"
        BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">

        <HeaderTemplate>
            <span>Search Main Account</span>
            <dxe:ASPxImage ID="ASPxImage1" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader">
                <ClientSideEvents Click="function(s, e){ 
                                cMainAccountModelPR.Hide();
                            }" />
            </dxe:ASPxImage>
        </HeaderTemplate>


        <ContentStyle VerticalAlign="Top" CssClass="pad">
        </ContentStyle>
        <ContentCollection>
            <dxe:PopupControlContentControl ID="PopupControlContentControl4" runat="server">
                <div class="modal-body">
                    <input type="text" onkeydown="MainAccountPRNewkeydown(event)" id="txtMainAccountPRSearch" autofocus width="100%" placeholder="Search by Main Account Name or Short Name" />

                    <div id="MainAccountTablePR">
                        <table border='1' width="100%">
                            <tr class="HeaderStyle">
                                <th>Main Account Name</th>
                            </tr>
                        </table>
                    </div>
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <ClientSideEvents Shown="function(){ $('#txtMainAccountPRSearch').focus();}" />
    </dxe:ASPxPopupControl>
    <asp:HiddenField runat="server" ID="hdnPRMainAccount" />
    <asp:HiddenField runat="server" ID="HiddenField_status" />

    <dxe:ASPxPopupControl ID="AspxDirectCustomerViewPopup" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="CAspxDirectCustomerViewPopup" Height="650px"
        Width="1020px" HeaderText="View Product" Modal="true" AllowResize="False">

        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>

    <asp:SqlDataSource ID="ProductDataSource" runat="server"
        SelectCommand="select sProducts_ID,sProducts_Code,sProducts_Name from Master_sProducts"></asp:SqlDataSource>
    <asp:SqlDataSource ID="HsnDataSource" runat="server"
        SelectCommand="select * from tbl_HSN_Master"></asp:SqlDataSource>
    <asp:SqlDataSource ID="tdstcs" runat="server"
        SelectCommand="Select  TDSTCS_ID,ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' as tdsdescription ,ltrim(rtrim(tdstcs_code)) tdscode  from master_tdstcs "></asp:SqlDataSource>

    <asp:SqlDataSource ID="SqlClassSource" runat="server"
        SelectCommand="select ProductClass_Name from Master_ProductClass order by ProductClass_Name"></asp:SqlDataSource>

    <asp:SqlDataSource ID="SqlHSNDataSource" runat="server"
        SelectCommand="select distinct sProducts_HsnCode Code  from master_sproducts where sProducts_HsnCode<>''  union all select  distinct SERVICE_CATEGORY_CODE   from Master_sProducts MP inner join TBL_MASTER_SERVICE_TAX sac on MP.sProducts_serviceTax=sac.TAX_ID "></asp:SqlDataSource>

    <dxe:ASPxPopupControl ID="productAttributePopUp" runat="server" ClientInstanceName="cproductAttributePopUp"
        Width="550px" HeaderText="Set Product Attribute(s)" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">

        <HeaderTemplate>
            <span>Set Product Attribute(s)</span>
            <dxe:ASPxImage ID="img" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader">
                <ClientSideEvents Click="function(s, e){
                                cCmbProductSize.SetValue(ProdSize);
                                cCmbProductColor.SetValue(ProdColor);
                                 RrdblappColor.SetSelectedIndex(ColApp);
                                Rrdblapp.SetSelectedIndex(SizeApp);
                                cproductAttributePopUp.Hide();
                            }" />
            </dxe:ASPxImage>
        </HeaderTemplate>

        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <div class="row">
                    <div class="col-md-6">
                        <div class="cityDiv" style="height: auto;">

                            <asp:Label ID="LblProductColor" runat="server" Text="Product Color" CssClass="newLbl"></asp:Label>
                        </div>
                        <div class="Left_Content">
                            <dxe:ASPxComboBox ID="CmbProductColor" ClientInstanceName="cCmbProductColor" ClearButton-DisplayMode="Always" runat="server" TabIndex="16"
                                ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0">
                            </dxe:ASPxComboBox>
                        </div>
                    </div>


                    <div class="col-md-6" style="margin-top: 25px">
                        <div class="cityDiv" style="height: auto;">
                        </div>
                        <div class="Left_Content">

                            <dxe:ASPxRadioButtonList ID="rdblappColor" ClientInstanceName="RrdblappColor" runat="server" RepeatDirection="Horizontal" Width="100%" TabIndex="17">
                                <Items>
                                    <dxe:ListEditItem Text="Applicable" Value="1" Selected="true" />
                                    <dxe:ListEditItem Text="Not Applicable" Value="0" />

                                </Items>
                            </dxe:ASPxRadioButtonList>
                        </div>
                    </div>

                    <div style="clear: both"></div>

                    <div class="col-md-6">
                        <div class="cityDiv" style="height: auto;">

                            <asp:Label ID="LblProductSize" runat="server" Text="Product Size" CssClass="newLbl"></asp:Label>
                        </div>
                        <div class="Left_Content">
                            <dxe:ASPxComboBox ID="CmbProductSize" ClientInstanceName="cCmbProductSize" ClearButton-DisplayMode="Always" runat="server" TabIndex="18"
                                ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0">
                            </dxe:ASPxComboBox>
                        </div>
                    </div>
                    <div class="col-md-6" style="margin-top: 25px">
                        <div class="cityDiv" style="height: auto;">
                        </div>
                        <div class="Left_Content">

                            <dxe:ASPxRadioButtonList ID="rdblapp" ClientInstanceName="Rrdblapp" runat="server" RepeatDirection="Horizontal" Width="100%" TabIndex="19">
                                <Items>
                                    <dxe:ListEditItem Text="Applicable" Value="1" Selected="true" />
                                    <dxe:ListEditItem Text="Not Applicable" Value="0" />

                                </Items>
                            </dxe:ASPxRadioButtonList>
                        </div>
                    </div>

                    <%--Product Component--%>
                    <div class="clear"></div>
                    <div class="col-md-12" style="margin-top: 7px">Components</div>
                    <div class="clear"></div>
                    <div class="col-md-6" style="margin-top: 7px">
                        <div class="cityDiv" style="height: auto; margin-bottom: 5px;">

                            <div id="divProductMasterComponentMandatory" runat="server">
                                (Mandatory tick/untick)
                                        <dxe:ASPxCheckBox runat="server" ID="chkIsMandatory" ClientInstanceName="cchkIsMandatory"></dxe:ASPxCheckBox>
                            </div>
                        </div>
                        <div class="Left_Content">
                            <dxe:ASPxCallbackPanel runat="server" ID="ASPxCallbackPanel1" ClientInstanceName="cComponentConfigProductPanel" OnCallback="Component_Callback">
                                <PanelCollection>
                                    <dxe:PanelContent runat="server">


                                        <dxe:ASPxGridLookup ID="GridLookup" runat="server" SelectionMode="Multiple" DataSourceID="ProductDataSource" ClientInstanceName="gridLookup"
                                            KeyFieldName="sProducts_ID" Width="100%" TextFormatString="{0}" MultiTextSeparator=", ">
                                            <Columns>
                                                <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="60" Caption=" " />
                                                <dxe:GridViewDataColumn FieldName="sProducts_Code" Caption="Product Code" Width="150" />
                                                <dxe:GridViewDataColumn FieldName="sProducts_Name" Caption="Product Name" Width="300" />
                                            </Columns>
                                            <%--<GridViewProperties  Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords"   >--%>
                                            <GridViewProperties Settings-VerticalScrollBarMode="Auto">

                                                <Templates>
                                                    <StatusBar>
                                                        <table class="OptionsTable" style="float: right">
                                                            <tr>
                                                                <td>
                                                                    <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridLookup" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </StatusBar>
                                                </Templates>
                                                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                            </GridViewProperties>
                                        </dxe:ASPxGridLookup>

                                    </dxe:PanelContent>
                                </PanelCollection>
                                <ClientSideEvents EndCallback="componentEndCallBack" />
                            </dxe:ASPxCallbackPanel>
                        </div>
                    </div>



                    <div id="divPosInstallation" runat="server">


                        <%--Installation Required--%>
                        <div class="col-md-6">
                            <div class="cityDiv" style="height: auto;">

                                <asp:Label ID="Label27" runat="server" Text="Installation Required" CssClass="newLbl"></asp:Label>
                            </div>
                            <div class="Left_Content">
                                <dxe:ASPxComboBox ID="aspxInstallation" ClientInstanceName="caspxInstallation" runat="server" TabIndex="16"
                                    ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="1">
                                    <Items>
                                        <dxe:ListEditItem Text="Yes" Value="1" />
                                        <dxe:ListEditItem Text="No" Value="0" />
                                    </Items>
                                </dxe:ASPxComboBox>
                            </div>
                        </div>
                    </div>
                    <div style="clear: both"></div>





                    <%--Brand --%>
                    <div class="col-md-6">
                        <div class="cityDiv" style="height: auto;">

                            <asp:Label ID="Label28" runat="server" Text="Brand" CssClass="newLbl"></asp:Label>
                        </div>
                        <div class="Left_Content">
                            <dxe:ASPxComboBox ID="cmbBrand" ClientInstanceName="ccmbBrand" ClearButton-DisplayMode="Always" runat="server" TabIndex="16"
                                ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="1">
                            </dxe:ASPxComboBox>
                        </div>
                    </div>


                    <div class="col-md-6" id="divPosOldUnit" runat="server">
                        <div class="cityDiv" style="height: auto;">

                            <asp:Label ID="Label29" runat="server" Text="Old Unit?" CssClass="newLbl"></asp:Label>
                        </div>
                        <div class="Left_Content">
                            <dxe:ASPxComboBox ID="ASPxComboBox1" ClientInstanceName="ccmbOldUnit" runat="server" TabIndex="16"
                                ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="1">
                                <Items>
                                    <dxe:ListEditItem Text="Yes" Value="1" />
                                    <dxe:ListEditItem Text="No" Value="0" />
                                </Items>
                            </dxe:ASPxComboBox>
                        </div>
                    </div>
                </div>
                <div style="clear: both"></div>

                <div class="boxarea clearfix">
                    <%--<span class="boxareaH">Product Size</span>--%>
                    <table class="mkSht">

                        <tr>
                            <td>
                                <label>Product Series</label>
                                <div>
                                    <dxe:ASPxTextBox ID="txtSeries" ClientInstanceName="ctxtSeries" runat="server"></dxe:ASPxTextBox>

                                </div>
                            </td>
                            <td>
                                <label>Surface</label>
                                <div>
                                    <dxe:ASPxTextBox ID="txtFinish" ClientInstanceName="ctxtFinish" runat="server"></dxe:ASPxTextBox>
                                </div>
                            </td>
                            <td>
                                <label>Lead Time</label>
                                <div>
                                    <dxe:ASPxTextBox ID="txtLeadtime" ClientInstanceName="ctxtLeadtime" runat="server"></dxe:ASPxTextBox>
                                </div>
                            </td>
                            <td>
                                <label class="pull-right">Weight</label>
                                <div>
                                    <dxe:ASPxTextBox ID="txtWeight" ClientInstanceName="ctxtWeight" DisplayFormatString="0.00" HorizontalAlign="Right" runat="server">
                                        <MaskSettings Mask="&lt;0..99999999999&gt;.&lt;00..99&gt;" />
                                    </dxe:ASPxTextBox>
                                </div>
                            </td>
                            <td>
                                <label>Sub-Category</label>
                                <div>
                                    <dxe:ASPxTextBox ID="txtSubCat" MaxLength="100" ClientInstanceName="ctxtSubCat" DisplayFormatString="0.00" HorizontalAlign="Right" runat="server">
                                    </dxe:ASPxTextBox>
                                </div>
                            </td>

                        </tr>

                    </table>

                </div>
                <div style="clear: both"></div>
                <div class="boxarea clearfix">
                    <span class="boxareaH">Product Coverage Area</span>
                    <table class="mkSht">
                        <tr>
                            <td>
                                <label class="pull-right">Length</label>
                                <div>
                                    <dxe:ASPxTextBox ID="txtHeight" ClientInstanceName="ctxtHeight" HorizontalAlign="Right" Text="0" DisplayFormatString="0.00" runat="server" Width="100%">
                                        <ClientSideEvents LostFocus="SizeUOMChange" />
                                        <MaskSettings Mask="&lt;0..99999999999&gt;.&lt;00..99&gt;" />
                                    </dxe:ASPxTextBox>
                                </div>
                            </td>
                            <td class="multiply" style="color: blue; font-size: 20px;">x</td>
                            <td>
                                <label class="pull-right">Width</label>
                                <div>
                                    <dxe:ASPxTextBox ID="ASPxTextBox1" ClientInstanceName="ctxtWidth" HorizontalAlign="Right" runat="server" Width="100%">
                                        <ClientSideEvents LostFocus="SizeUOMChange" />
                                        <MaskSettings Mask="&lt;0..99999999999&gt;.&lt;00..99&gt;" />
                                    </dxe:ASPxTextBox>

                                </div>
                            </td>

                            <td>
                                <label class="pull-right">Thickness</label>
                                <div>
                                    <dxe:ASPxTextBox ID="txtThickness" ClientInstanceName="ctxtThickness" HorizontalAlign="Right" runat="server" Width="100%">
                                        <ClientSideEvents LostFocus="SizeUOMChange" />
                                        <MaskSettings Mask="&lt;0..99999999999&gt;.&lt;00..99&gt;" />
                                    </dxe:ASPxTextBox>

                                </div>
                            </td>
                            <td>
                                <label>UOM</label>
                                <div>
                                    <dxe:ASPxComboBox ID="ddlSize" ClientInstanceName="cddlSize" runat="server" ClearButton-DisplayMode="Always"
                                        ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0">
                                        <ClientSideEvents ValueChanged="SizeUOMChange" />
                                    </dxe:ASPxComboBox>
                                </div>
                            </td>
                            <td style="width: 102px;">
                                <label>For</label>
                                <div>
                                    <select id="SizeUOM" runat="server" class="form-control" onchange="SizeUOMChange();">
                                        <option selected value="1">First UOM</option>
                                        <option value="2">Second UOM</option>
                                    </select>
                                </div>
                            </td>
                        </tr>

                    </table>
                    <table class="mkSht">
                        <tr>

                            <td>
                                <label class="pull-right">Coverage Area</label>
                                <input value="0.00" runat="server" id="txtCoverage" style="text-align: right;" disabled="disabled" type="text" />

                            </td>
                            <td>
                                <label>&nbsp;</label>
                                <div runat="server" id="dvCovg"></div>
                            </td>

                            <td>
                                <label class="pull-right">Volume</label><input value="0.00" style="text-align: right;" runat="server" id="txtVolumn" disabled="disabled" type="text" /></td>
                            <td>
                                <label>&nbsp;</label>
                                <div runat="server" id="dvvolume">Unit</div>
                            </td>
                        </tr>
                        <tr>

                            <td>
                                <div class="red sText" style="color: blue; font-size: 15px;">Length*Width</div>
                            </td>
                            <td></td>
                            <td>
                                <div class="red sText" style="color: blue; font-size: 15px;">Length*Width*Thickness</div>
                            </td>
                            <td></td>
                        </tr>
                    </table>

                </div>
                <div style="clear: both"></div>

                <div class="boxarea clearfix">
                    <table class="mkSht hide">
                        <tr>
                            <td>
                                <label>Coverage Per</label></td>
                            <td>


                                <div>
                                    <select id="covergaeUOM" class="form-control" disabled="disabled">
                                        <option value="1">First UOM</option>
                                        <option value="2">Second UOM</option>
                                    </select>

                                </div>
                            </td>
                            <td>
                                <div>
                                </div>
                            </td>
                            <td></td>

                        </tr>
                    </table>
                </div>

                <div style="clear: both"></div>


                <div class="col-md-6" style="margin-top: 25px">
                    <div class="cityDiv" style="height: auto;">
                    </div>
                    <div class="Left_Content">
                        <button class="btn btn-primary" type="button" onclick="productAttributeOkClik()">Ok</button>

                    </div>
                </div>
                </div>

            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
        <ClientSideEvents Init="OnInitProductAttribute" />
    </dxe:ASPxPopupControl>

    <%--Packing Details popup--%>
    <dxe:ASPxPopupControl ID="ASPxPopupControl3" runat="server" ClientInstanceName="cpackingDetails"
        Width="500px" HeaderText="Packing Details" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <HeaderTemplate>
            <span>UOM Conversion</span>
            <dxe:ASPxImage ID="ASPxImage5" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader">
                <ClientSideEvents Click="function(s, e){ 
                                $('#invalidPackingUom').css({ 'display': 'none' });
                                cpackingDetails.Hide();
                            }" />
            </dxe:ASPxImage>
        </HeaderTemplate>
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <div style="clear: both"></div>

                <table>
                    <tr>
                        <td class="pull-left">Quantity</td>
                        <%--Rev 1.0 11-03-2019--%>
                        <%--<td>Sale UOM</td>--%>
                        <%--<td>UOM</td>--%>
                        <%--mantis issue number 19833--%>
                        <td>Main Unit</td>
                        <%--End of Rev 1.0 11-03-2019--%>
                        <td></td>
                        <%--Rev 1.0 07-03-2019--%>
                        <%-- <td>Packing</td>
                                    <td>Select UOM</td>--%>
                        <%--mantis issue number 19833--%>
                        <td class="pull-left">Alt. Quantity</td>
                        <td>Alt. Unit</td>
                        <%--End of Rev 1.0 07-03-2019--%>
                    </tr>
                    <tr>
                        <td style="padding-right: 7px">
                            <dxe:ASPxTextBox ID="txtPackingQty" HorizontalAlign="Right" ClientInstanceName="ctxtPackingQty" MaxLength="50" runat="server" Width="100%">
                                <MaskSettings Mask="<0..99999999>.<0..99>" />
                                <ClientSideEvents LostFocus="SizeUOMChange" />
                            </dxe:ASPxTextBox>
                        </td>
                        <td style="padding-right: 7px">
                            <dxe:ASPxTextBox ID="txtpackingSaleUom" ClientInstanceName="ctxtpackingSaleUom" MaxLength="50" runat="server" Width="100%">
                                <ClientSideEvents LostFocus="SizeUOMChange" />
                            </dxe:ASPxTextBox>
                        </td>
                        <td style="padding-right: 7px">
                            <span>=</span>

                        </td>
                        <td style="padding-right: 7px">
                            <dxe:ASPxTextBox ID="txtpacking" ClientInstanceName="ctxtpacking" HorizontalAlign="Right" MaxLength="50" runat="server" Width="100%">
                                <MaskSettings Mask="<0..99999999>.<0..99>" />
                                <ClientSideEvents LostFocus="SizeUOMChange" />
                            </dxe:ASPxTextBox>
                        </td>
                        <td style="padding-right: 7px">
                            <dxe:ASPxComboBox ID="cmbPackingUomPro" ClientInstanceName="ccmbPackingUomPro" runat="server" SelectedIndex="0"
                                ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True">
                                <ClientSideEvents SelectedIndexChanged="SizeUOMChange" />
                            </dxe:ASPxComboBox>
                            <%--Rev Subhra 02-04-2019--%>
                            <%--<span id="invalidPackingUom" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; display: none; padding-left: 9px;" title="Invalid GSTIN"></span>--%>
                            <span id="invalidPackingUom" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; display: none; padding-left: 9px;" title="Invalid 2nd UOM"></span>
                            <%--End of Rev Subhra 02-04-2019--%>
                        </td>
                    </tr>

                </table>

                <table id="tblOverideConvertion">
                    <tr>
                        <td>
                            <dxe:ASPxCheckBox runat="server" ID="chkOverideConvertion" ClientInstanceName="cchkOverideConvertion"></dxe:ASPxCheckBox>
                        </td>
                        <td>Do not override UOM conversion in transaction.   
                        </td>
                    </tr>
                </table>



                <div style="clear: both"></div>
                <div class="" style="margin-top: 12px">
                    <div class="cityDiv" style="height: auto;">
                    </div>
                    <div class="Left_Content">
                        <button type="button" class="btn btn-primary" onclick="PackingDetailsOkClick()">Ok</button>

                    </div>
                </div>

            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
        <ClientSideEvents Init="OnInitTax" />
    </dxe:ASPxPopupControl>
    <%--Packing Details popup end Here--%>

    <dxe:ASPxPopupControl ID="BarCodePopUp" runat="server" ClientInstanceName="cBarCodePopUp"
        Width="360px" HeaderText="Set Barcode" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <HeaderTemplate>
            <span>Set Barcode</span>
            <dxe:ASPxImage ID="ASPxImage2" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader">
                <ClientSideEvents Click="function(s, e){
                                cCmbBarCodeType.SetSelectedIndex(barCodeType);
                                ctxtBarCodeNo.SetText(BarCode);
                                ctxtGlobalCode.SetText(GlobalCode);
                                cBarCodePopUp.Hide();
                            }" />
            </dxe:ASPxImage>
        </HeaderTemplate>

        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <div style="clear: both"></div>
                <div class="col-md-12">
                    <div class="cityDiv" style="height: auto;">

                        <asp:Label ID="lblBarCodeType" runat="server" Text="Barcode Type" CssClass="newLbl"></asp:Label>
                    </div>
                    <div class="Left_Content">
                        <dxe:ASPxComboBox ID="CmbBarCodeType" ClientInstanceName="cCmbBarCodeType" runat="server" SelectedIndex="0" TabIndex="6" ClearButton-DisplayMode="Always"
                            ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True">
                        </dxe:ASPxComboBox>
                    </div>
                </div>

                <div class="col-md-12">
                    <div class="cityDiv" style="height: auto;">

                        <asp:Label ID="lblBarcodeNo" runat="server" Text="Barcode No." CssClass="newLbl"></asp:Label>
                    </div>
                    <div class="Left_Content">
                        <dxe:ASPxTextBox ID="txtBarCodeNo" ClientInstanceName="ctxtBarCodeNo" MaxLength="50" TabIndex="7"
                            runat="server" Width="100%">
                        </dxe:ASPxTextBox>
                    </div>
                </div>

                <div style="clear: both"></div>

                <div class="col-md-12">
                    <div class="cityDiv" style="height: auto;">
                        <%--Global Code--%>
                        <asp:Label ID="LblGlobalCode" runat="server" Text="Global Code(UPC)" CssClass="newLbl"></asp:Label>
                    </div>
                    <div class="Left_Content">
                        <dxe:ASPxTextBox ID="txtGlobalCode" ClientInstanceName="ctxtGlobalCode" MaxLength="30" TabIndex="8"
                            runat="server" Width="100%" Text='<%# Bind("txtMarkets_Description") %>'>
                        </dxe:ASPxTextBox>
                    </div>
                </div>

                <div style="clear: both"></div>
                <div class="col-md-6" style="margin-top: 25px">
                    <div class="cityDiv" style="height: auto;">
                    </div>
                    <div class="Left_Content">
                        <button type="button" class="btn btn-primary" onclick="BarCodeOkClick()">Ok</button>

                    </div>
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
        <ClientSideEvents Init="OnInitBarCode" />
    </dxe:ASPxPopupControl>
    <dxe:ASPxCallbackPanel ClientInstanceName="cgridprod" ID="gridPro" runat="server" OnCallback="cityGrid_CustomCallback" ClientSideEvents-EndCallback="grid_EndCallBack">
    </dxe:ASPxCallbackPanel>
    <%--taxCode popup--%>
    <dxe:ASPxPopupControl ID="TaxCodePopup" runat="server" ClientInstanceName="cTaxCodePopup"
        Width="360px" HeaderText="Set Tax Codes" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <HeaderTemplate>
            <span>Set Tax Codes</span>
            <dxe:ASPxImage ID="ASPxImage1" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader">
                <ClientSideEvents Click="function(s, e){
                                cCmbTaxCodeSale.SetValue(taxCodeSale);
                                cCmbTaxCodePur.SetValue(taxCodePur);
                                 cChkAutoApply.SetChecked(autoApply);
                                cCmbTaxScheme.SetValue(taxScheme);
                                GetCheckBoxValue(autoApply);
                                cTaxCodePopup.Hide();
                            }" />
            </dxe:ASPxImage>
        </HeaderTemplate>
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <div style="clear: both"></div>
                <div class="col-md-12">
                    <div class="cityDiv" style="height: auto;">

                        <asp:Label ID="Label30" runat="server" Text="Select Tax Code Scheme -Sales" CssClass="newLbl"></asp:Label>
                    </div>
                    <div class="Left_Content">
                        <dxe:ASPxComboBox ID="CmbTaxCodeSale" ClientInstanceName="cCmbTaxCodeSale" runat="server" SelectedIndex="0"
                            ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True">
                        </dxe:ASPxComboBox>
                    </div>
                </div>
                <div class="col-md-12">
                    <div class="cityDiv" style="height: auto;">

                        <asp:Label ID="Label31" runat="server" Text="Select Tax Code Scheme -Purchases" CssClass="newLbl"></asp:Label>
                    </div>
                    <div class="Left_Content">
                        <dxe:ASPxComboBox ID="CmbTaxCodePur" ClientInstanceName="cCmbTaxCodePur" runat="server" SelectedIndex="0"
                            ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True">
                        </dxe:ASPxComboBox>
                    </div>
                </div>
                <div style="clear: both"></div>
                <div class="hide">
                    <div class="col-md-12">
                        <div class="cityDiv" style="height: auto;">

                            <asp:Label ID="Label32" runat="server" Text="Apply Auto Selection in Entries" CssClass="newLbl"></asp:Label>
                        </div>
                        <div class="Left_Content">
                            <dxe:ASPxCheckBox runat="server" ID="ChkAutoApply" ClientInstanceName="cChkAutoApply">
                                <ClientSideEvents CheckedChanged="function(s, e) { 
                                            GetCheckBoxValue(s.GetChecked()); 
                                        }" />
                            </dxe:ASPxCheckBox>
                        </div>
                    </div>

                    <div style="clear: both"></div>
                    <div class="col-md-12">
                        <div class="cityDiv" style="height: auto;">

                            <asp:Label ID="Label33" runat="server" Text="Select Tax Scheme" CssClass="newLbl"></asp:Label>
                        </div>
                        <div class="Left_Content">
                            <dxe:ASPxComboBox ID="CmbTaxScheme" ClientInstanceName="cCmbTaxScheme" runat="server" SelectedIndex="0"
                                ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True">
                            </dxe:ASPxComboBox>
                        </div>
                    </div>
                </div>


                <div style="clear: both"></div>
                <div class="col-md-6" style="margin-top: 25px">
                    <div class="cityDiv" style="height: auto;">
                    </div>
                    <div class="Left_Content">
                        <button type="button" class="btn btn-primary" onclick="taxCodeOkClick()">Ok</button>

                    </div>
                </div>

            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
        <ClientSideEvents Init="OnInitTax" />
    </dxe:ASPxPopupControl>

    <%--TaxCode popup End Here--%>

    <%--Service Tax popup--%>
    <dxe:ASPxPopupControl ID="ASPxPopupControl4" runat="server" ClientInstanceName="cServiceTaxPopup"
        Width="360px" HeaderText="Service Category" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <HeaderTemplate>
            <span>Set Service Category</span>
            <dxe:ASPxImage ID="ASPxImage4" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader">
                <ClientSideEvents Click="function(s, e){ 
                                cServiceTaxPopup.Hide();
                            }" />
            </dxe:ASPxImage>
        </HeaderTemplate>
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <div style="clear: both"></div>
                <div class="cityDiv" style="height: auto;">

                    <asp:Label ID="Label34" runat="server" Text="Service Category" CssClass="newLbl"></asp:Label>
                </div>
                <div class="Left_Content">
                    <dxe:ASPxComboBox ID="AspxServiceTax" ClientInstanceName="cAspxServiceTax" runat="server" SelectedIndex="0" DropDownWidth="800"
                        ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True"
                        ValueField="TAX_ID" IncrementalFilteringMode="Contains" CallbackPageSize="30" TextFormatString="{0} {1}" ItemStyle-Wrap="True">
                        <Columns>
                            <dxe:ListBoxColumn FieldName="SERVICE_CATEGORY_CODE" Caption="Code" Width="45" />
                            <dxe:ListBoxColumn FieldName="SERVICE_TAX_NAME" Caption="Name" Width="250" />
                            <%-- <dxe:ListBoxColumn FieldName="ACCOUNT_HEAD_TAX_RECEIPTS" Caption="Receipts" Width="65" />
                                        <dxe:ListBoxColumn FieldName="ACCOUNT_HEAD_OTHERS_RECEIPTS" Caption="Oth Receipts" Width="65" />
                                        <dxe:ListBoxColumn FieldName="ACCOUNT_HEAD_PENALTIES" Caption="Penalties" Width="65" />
                                        <dxe:ListBoxColumn FieldName="ACCOUNT_HEAD_DeductRefund" Caption="A/C Head (Deduct Refund)" Width="120" />--%>
                        </Columns>

                    </dxe:ASPxComboBox>
                </div>



                <div style="clear: both"></div>
                <div class="col-md-6" style="margin-top: 25px">
                    <div class="cityDiv" style="height: auto;">
                    </div>
                    <div class="Left_Content">
                        <button type="button" class="btn btn-primary" onclick="ServicetaxOkClick()">Ok</button>

                    </div>
                </div>

            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
        <ClientSideEvents Init="OnInitTax" />
    </dxe:ASPxPopupControl>
    <%--TaxCode popup End Here--%>

    <%--Tds Section start Here--%>
    <dxe:ASPxPopupControl ID="tdsPopup" runat="server" ClientInstanceName="ctdsPopup"
        Width="450" HeaderText="Set TDS" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <HeaderTemplate>
            <span>Set TDS Codes</span>
            <dxe:ASPxImage ID="ASPxImage6" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader">
                <ClientSideEvents Click="function(s, e){ 
                                cmb_tdstcs.SetValue(tdsValue);
                                ctdsPopup.Hide();
                            }" />
            </dxe:ASPxImage>
        </HeaderTemplate>
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <div style="clear: both"></div>
                <div class="col-md-12">
                    <div class="cityDiv" style="height: auto;">

                        <asp:Label ID="Label35" runat="server" Text="TDS Section" CssClass="newLbl"></asp:Label>
                    </div>
                    <div class="Left_Content">
                        <dxe:ASPxComboBox ID="cmb_tdstcs" ClientInstanceName="cmb_tdstcs" DataSourceID="tdstcs" Width="100%" ItemStyle-Wrap="True"
                            ClearButton-DisplayMode="Always" runat="server" TextField="tdscode" ValueField="TDSTCS_ID">
                        </dxe:ASPxComboBox>
                    </div>
                </div>

                <div style="clear: both"></div>
                <div class="col-md-6" style="margin-top: 25px">
                    <div class="cityDiv" style="height: auto;">
                    </div>
                    <div class="Left_Content">
                        <button type="button" class="btn btn-primary" onclick="tdsOkClick()">Ok</button>

                    </div>
                </div>

            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
        <ClientSideEvents Init="OnInitTax" />
    </dxe:ASPxPopupControl>
    <%--TDS popup End Here--%>

    <div class="HiddenFieldArea" style="display: none;">
        <asp:HiddenField runat="server" ID="hiddenedit" />
    </div>


    <dxe:ASPxPopupControl ID="PosView" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="cPosView" Height="650px"
        Width="1200px" HeaderText="Product" Modal="true">

        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>



    <dxe:ASPxPopupControl runat="server" ID="PopClass" ClientInstanceName="cPopClass"
        Width="500px" Height="300px" HeaderText="Search Class" PopupHorizontalAlign="WindowCenter"
        BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">

        <HeaderTemplate>
            <span>Search Class</span>
            <dxe:ASPxImage ID="ASPxImage12" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader">
                <ClientSideEvents Click="function(s, e){ 
                                cPopClass.Hide();
                            }" />
            </dxe:ASPxImage>
        </HeaderTemplate>


        <ContentStyle VerticalAlign="Top" CssClass="pad">
        </ContentStyle>
        <ContentCollection>
            <dxe:PopupControlContentControl ID="PopupControlContentControl5" runat="server">
                <div class="modal-body">
                    <input type="text" onkeydown="Classkeydown(event)" id="txtClassNameSearch" autofocus width="100%" placeholder="Search by Class Name" />

                    <div id="ClassTable">
                        <table border='1' width="100%">
                            <tr class="HeaderStyle">
                                <th>Class Name</th>
                            </tr>
                        </table>
                    </div>
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <ClientSideEvents Shown="function(){ $('#txtClassNameSearch').focus();}" />
    </dxe:ASPxPopupControl>
    <dxe:ASPxPopupControl runat="server" ID="PopHSN" ClientInstanceName="cPopHSN"
        Width="1200px" Height="300px" HeaderText="Search HSN" PopupHorizontalAlign="WindowCenter"
        BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">

        <HeaderTemplate>
            <span>Search HSNCode</span>
            <dxe:ASPxImage ID="ASPxImage12" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader">
                <ClientSideEvents Click="function(s, e){ 
                                cPopHSN.Hide();
                            }" />
            </dxe:ASPxImage>
        </HeaderTemplate>


        <ContentStyle VerticalAlign="Top" CssClass="pad">
        </ContentStyle>
        <ContentCollection>
            <dxe:PopupControlContentControl ID="PopupControlContentControl6" runat="server">
                <div class="modal-body">
                    <input type="text" onkeydown="HSNkeydown(event)" id="txtHSNSearch" autofocus width="100%" placeholder="Search by HSN Code" />

                    <div id="HSNTable">
                        <table class="dynamicPopupTbl">
                            <tr>
                                <th class="hide">id</th>
                                <th style="width: 150px;">Code</th>
                                <th>Description</th>
                            </tr>
                        </table>
                    </div>
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <ClientSideEvents Shown="function(){ $('#txtHSNSearch').focus();}" />
    </dxe:ASPxPopupControl>

    <%--Product Pop up End--%>
    <%-- chinmoy ended prpduct popup --%>
    <script>
        $(document).ready(function () {
            $('body').addClass('mini-navbar');
            //$('.navbar-minimalize').click(function () {
            //    grid.SetWidth = ''
            //})
        });
    </script>

    <dxe:ASPxPopupControl ID="popupApprover" runat="server" ClientInstanceName="cpopupApprover"
        Width="900px" HeaderText="Approver" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
        <HeaderTemplate>
            <strong><span style="color: #fff">Select Approver</span></strong>

        </HeaderTemplate>
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <div class="row">
                    <div class="col-md-3">
                        <label>Credit Days</label>
                        <dxe:ASPxTextBox ID="txtCreditDayspopup" runat="server" ClientInstanceName="ctxtCreditDayspopup" Width="100%">
                            <MaskSettings Mask="<0..999999999>" AllowMouseWheel="false" />
                            <ClientSideEvents TextChanged="CreditDays_TextChanged" LostFocus="CreditDayspopup_LostFocus" />
                        </dxe:ASPxTextBox>
                    </div>
                    <div class="col-md-3">
                        <label>Due Date</label>
                        <dxe:ASPxDateEdit ID="dtDueDate" Width="100%" ClientEnabled="false" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy" runat="server" ClientInstanceName="cdtDueDate"></dxe:ASPxDateEdit>
                    </div>
                </div>
                <div>
                    <label style="font-size: 16px; margin: 5px 0; text-transform: uppercase; font-weight: 500 !important;">Approver List</label>
                    <dxe:ASPxGridView ID="ASPxGridView1" runat="server" KeyFieldName="id" AutoGenerateColumns="False" OnCustomCallback="grid_CustomCallback"
                        Width="100%" ClientInstanceName="cgrids" SettingsBehavior-AllowFocusedRow="true"
                        SettingsBehavior-AllowSelectSingleRowOnly="false" SettingsBehavior-AllowSelectByRowClick="true"
                        SettingsBehavior-ColumnResizeMode="Control" Settings-VerticalScrollableHeight="200" Settings-VerticalScrollBarMode="Auto"
                        DataSourceID="dsGvDS" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false">
                        <SettingsSearchPanel Visible="True" Delay="5000" />
                        <%--SettingsCookies-Enabled="true" SettingsCookies-StorePaging="true" SettingsCookies-StoreFiltering="true" SettingsCookies-StoreGroupingAndSorting="true" --%>

                        <Columns>
                            <dxe:GridViewCommandColumn ShowSelectCheckbox="true" VisibleIndex="0"></dxe:GridViewCommandColumn>
                            <dxe:GridViewDataTextColumn Caption="Sl No." FieldName="sl" Width="50" Visible="false" SortOrder="Descending"
                                VisibleIndex="1" FixedStyle="Left">
                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Name" FieldName="name" Width="50%"
                                VisibleIndex="2" FixedStyle="Left">
                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="email" FieldName="email"
                                VisibleIndex="3" Width="50%">
                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>

                        </Columns>
                        <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                        <SettingsPager PageSize="10">
                            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,20,50,100,150,200" />
                        </SettingsPager>
                        <Settings ShowGroupPanel="True" ShowFooter="true" ShowGroupFooter="VisibleIfExpanded" ShowStatusBar="Hidden" ShowHorizontalScrollBar="true" ShowFilterRow="true" ShowFilterRowMenu="true" />
                        <SettingsLoadingPanel Text="Please Wait..." />
                    </dxe:ASPxGridView>
                </div>
                <div class="text-center">
                    <asp:Button ID="Button4" runat="server" UseSubmitBehavior="false" Text="OK" CssClass="btn btn-primary  mLeft mTop" OnClientClick="return SaveWithApprover();" />
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <ContentStyle VerticalAlign="Top" CssClass="pad"></ContentStyle>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
    </dxe:ASPxPopupControl>
    <asp:SqlDataSource ID="dsGvDS" SelectCommand="SELECT v.id,name,email FROM MODULEWISE_APPROVER_LIST lst	
inner join  v_approvers v on v.id=lst.APPROVER_ID and MODULE_ID=1"
        runat="server"></asp:SqlDataSource>

    <asp:HiddenField ID="hdnApprover" runat="server" Value="0" />
    <asp:SqlDataSource ID="AltUomSelect" runat="server"
        SelectCommand="select UOM_ID,UOM_Name from Master_UOM "></asp:SqlDataSource>

    <asp:HiddenField runat="server" ID="ProductMinPrice" />
    <asp:HiddenField runat="server" ID="ProductMaxPrice" />
    <asp:HiddenField runat="server" ID="hdnRateType" />

    <asp:HiddenField ID="hdnLockFromDate" runat="server" />
    <asp:HiddenField ID="hdnLockToDate" runat="server" />
    <asp:HiddenField ID="hdnLockFromDateCon" runat="server" />
    <asp:HiddenField ID="hdnLockToDateCon" runat="server" />
      <asp:HiddenField runat="server" ID="hdnProductDaynamicRate" />
</asp:Content>
