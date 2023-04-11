<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                23-03-2023        2.0.36           Pallab              25733 : Master pages design modification
2.0                10-04-2023        2.0.38           Priti               0025801 : Unable to delete HSN code from HSN master though the user rights are given
====================================================== Revision History =============================================--%>

<%@ Page Title="Products" Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.Store.Master.management_master_Store_sProducts" CodeBehind="sProducts.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../../../CentralData/JSScript/GenericJScript.js"></script>
    <script src="../../Activities/JS/SearchPopup.js"></script>
    <link href="../../Activities/CSS/SearchPopup.css" rel="stylesheet" />
    <script src="JS/SearchMultiPopup.js"></script>
    <script type="text/javascript">

        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }

        function UniqueCodeProductCheck() {

            var SchemeVal = $("#ddl_numberingScheme").val();

            var NoSchemeId = SchemeVal.toString().split('~')[0];
            if (SchemeVal == "0") {
                jAlert('Please Select Numbering Scheme');
                //ctxt_SlOrderNo.SetValue('');
                //ctxt_SlOrderNo.Focus();
            }

                //if (NoSchemeId == "0")
            else {
                var CheckUniqueCode = false;
                var uccName = "";
                var Type = "";
                if (($("#hdnAutoNumStg").val() == "PDAutoNum1") && ($("#hdnTransactionType").val() == "PD")) {
                    uccName = ctxt_CustDocNo.GetText();
                    Type = "MasterProduct";
                }


                $.ajax({
                    type: "POST",
                    url: "sProducts.aspx/CheckUniqueNumberingCode",
                    data: JSON.stringify({ uccName: uccName, Type: Type }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        CheckUniqueCode = msg.d;
                        if (CheckUniqueCode == true) {
                            jAlert('Please enter unique No.');
                            //jAlert('Please enter unique Sales Order No');
                            ctxt_CustDocNo.SetValue('');
                            ctxt_CustDocNo.Focus();
                        }

                    }

                });
            }
        }


    </script>
     <%--Rev work start 10.06.2022 Mantise issue:0024783: Customer & Product master import required for ERP--%>
    <script type="text/javascript">
        function ImportUpdatePopOpenProductTarget(e) {

            $("#modalimport").modal('show');
        }
        function ViewLogData() {
            cGvJvSearch.Refresh();
        }
        function ShowLogData(haslog) {
            debugger;
            $('#btnViewLog').click();
        }
    </script>
    <%--Rev work close 10.06.2022 Mantise issue:0024783: Customer & Product master import required for ERP--%>
    <style>
        .dynamicPopupTbl > tbody > tr > th {
            padding: 7px 75px 7px 6px;
        }

        .myImage {
            max-height: 100px;
            max-width: 100px;
        }

        .boxarea {
            border: 1px solid #a7a6a64a;
            position: relative;
            margin: 5px 15px;
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
    <%--Rev work start 03.06.2022 Mantise issue:0024783: Customer & Product master import required for ERP--%>
    <style>
        .VerySmall {
            width: 320px;
        }
    </style>
    <%--Rev work close 03.06.2022 Mantise issue:0024783: Customer & Product master import required for ERP--%>
    <%-- For multiselection when click on ok button--%>
    <script type="text/javascript">
        function SetSelectedValues(Id, Name, ArrName) {
            if (ArrName == 'EmpSource') {
                var key = Id;
                if (key != null && key != '') {
                    $('#EmpModel').modal('hide');
                    cbtnVendors.SetText(Name);
                    GetObjectID('hdVendorsID').value = key;

                }
                else {
                    cbtnVendors.SetText('');
                    GetObjectID('hdVendorsID').value = '';
                }
            }

        }

    </script>
    <%-- For multiselection when click on ok button--%>

    <%-- For multiselection--%>




    <script type="text/javascript">

        ////Ref Bapi
    <%-- For multiselection--%>
        $(document).ready(function () {
            $('#EmpModel').on('shown.bs.modal', function () {
                $('#txtEmpSearch').focus();
            })
        })
        var EmpArr = new Array();
        $(document).ready(function () {
            var EmpObj = new Object();
            EmpObj.Name = "EmpSource";
            EmpObj.ArraySource = EmpArr;
            arrMultiPopup.push(EmpObj);
        })
        function VendorButnClick(s, e) {

            var txt = "<table border='1' width=\"100%\"><tr class=\"HeaderStyle\"><th>Vendor Search</th></tr><table>";
            $("#txtEmpSearch").val("");
            document.getElementById("EmpTable").innerHTML = txt;
            $('#EmpModel').modal('show');
        }

        function Emp_KeyDown(s, e) {
            if (e.htmlEvent.key == "Enter") {
                $('#EmpModel').modal('show');
            }
        }

        function Empkeydown(e) {

            var OtherDetails = {}

            if ($.trim($("#txtEmpSearch").val()) == "" || $.trim($("#txtEmpSearch").val()) == null) {
                return false;
            }
            OtherDetails.SearchKey = $("#txtEmpSearch").val();

            if (e.code == "Enter" || e.code == "NumpadEnter") {

                var HeaderCaption = [];
                HeaderCaption.push("Vendor Name");

                if ($("#txtEmpSearch").val() != "") {

                    callonServerM("../Master/sProducts.aspx/GetVendorsList", OtherDetails, "EmpTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "EmpSource");

                }
            }
            else if (e.code == "ArrowDown") {
                if ($("input[dPropertyIndex=0]"))
                    $("input[dPropertyIndex=0]").focus();
            }
        }


        function SetfocusOnseach(indexName) {
            if (indexName == "dPropertyIndex")
                $('#txtEmpSearch').focus();
            else
                $('#txtEmpSearch').focus();
        }

        ////
            <%-- For multiselection--%>


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





        function popupClose(s, e) {
            grid.Refresh();
            grid.cpCopy = null;
        }



        //Added for lite popup
        function CustomerButnClick(s, e) {
            var txt = "<table border='1' width=\"100%\"><tr class=\"HeaderStyle\"><th>Class Name</th></tr><table>";
            $("#txtClassNameSearch").val("");
            document.getElementById("ClassTable").innerHTML = txt;
            cPopClass.Show();

        }

        function HSNButnClick(s, e) {
            var txt = "<table border='1' width=\"100%\"><tr class=\"HeaderStyle\"><th style='width:150px;'>Code</th><th>Description</th></tr><table>";
            $("#txtHSNSearch").val("");
            document.getElementById("HSNTable").innerHTML = txt;
            cPopHSN.Show();
        }


        function Customer_KeyDown(s, e) {


            if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
                s.OnButtonClick(0);
                $("#txtClassNameSearch").focus();
            }
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
                callonServer("../Master/sProducts.aspx/GetHSNDetails", OtherDetails, "HSNTable", HeaderCaption, "HSNIndex", "SetHSN");
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
                cPopHSN.Hide();
            }
        }


        function Customerkeydown(e) {

            var OtherDetails = {}
            OtherDetails.SearchKey = $("#txtClassNameSearch").val();
            if (e.code == "Enter" || e.code == "NumpadEnter") {
                if ($("#txtClassNameSearch").val() == "")
                    return;
                var HeaderCaption = [];
                HeaderCaption.push("Class Name");
                //callonServer("../Master/sProducts.aspx/GetMainAccount", OtherDetails, "MainAccountTable", HeaderCaption, "MainAccountSIIndex", "SetMainAccountSI");
                callonServer("../Master/sProducts.aspx/GetClassDetails", OtherDetails, "ClassTable", HeaderCaption, "customerIndex", "SetCustomer");
            }
            else if (e.code == "ArrowDown") {
                if ($("input[customerIndex=0]"))
                    $("input[customerIndex=0]").focus();
            }
            else if (e.code == "Escape") {
                cPopClass.Hide();
                cPopClass.Focus();
            }




        }




        function SetCustomer(id, Name) {

            var key = id;
            $('#ClassId').val(id)
            if (key != null && key != '') {
                // $('#CustModel').modal('hide');
                cProClassCode.SetText(Name);
                cPopClass.Hide();
            }
        }


        function SizeUOMChange() {

            $("#covergaeUOM").val($("#SizeUOM").val());
            var first = ctxtPackingQty.GetValue();
            var second = ctxtpacking.GetValue();

            if (ctxtpackingSaleUom.GetText() == "") {
                first = 0;
            }

            if (ccmbPackingUom.GetText() == "") {
                second = 0;
                //Rev Rajdip
                ccmbPackingUom.SetValue("");
                //End Rev rajdip
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
                callonServer("../Master/sProducts.aspx/GetMainAccount", OtherDetails, "MainAccountTable", HeaderCaption, "MainAccountSIIndex", "SetMainAccountSI");
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
                callonServer("../Master/sProducts.aspx/GetMainAccount", OtherDetails, "MainAccountTableSR", HeaderCaption, "MainAccountSRIndex", "SetMainAccountSR");
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
                callonServer("../Master/sProducts.aspx/GetMainAccount", OtherDetails, "MainAccountTablePI", HeaderCaption, "MainAccountPIIndex", "SetMainAccountPI");
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
                callonServer("../Master/sProducts.aspx/GetMainAccount", OtherDetails, "MainAccountTablePR", HeaderCaption, "MainAccountPRIndex", "SetMainAccountPR");
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

        function ValueSelected(e, indexName) {
            if (indexName == "MainAccountSIIndex") {
                if (e.code == "Enter" || e.code == "NumpadEnter") {
                    var Code = e.target.parentElement.parentElement.cells[0].innerText;
                    var name = e.target.parentElement.parentElement.cells[1].children[0].value;

                    $("#hdnSIMainAccount").val(Code);
                    cSIMainAccount.SetText(name);
                    cSIMainAccount_active.SetText(name);
                    cMainAccountModelSI.Hide();
                } else if (e.code == "ArrowDown") {
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
                        $('#txtMainAccountSearch').focus();
                    }
                }

            }


            else if (indexName == "HSNIndex") {
                if (e.code == "Enter" || e.code == "NumpadEnter") {
                    var Code = e.target.parentElement.parentElement.cells[0].innerText;
                    var name = e.target.parentElement.parentElement.cells[1].children[0].value;

                    $("#hdnHSN").val(Code);
                    cHSNCode.SetText(name);

                    cPopHSN.Hide();
                } else if (e.code == "ArrowDown") {
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
                        $('#txtHSNSearch').focus();
                    }
                }

            }


            else if (indexName == "customerIndex") {
                if (e.code == "Enter" || e.code == "NumpadEnter") {
                    var Code = e.target.parentElement.parentElement.cells[0].innerText;
                    var name = e.target.parentElement.parentElement.cells[1].children[0].value;

                    $("#ClassId").val(Code);
                    cProClassCode.SetText(name);

                    cPopClass.Hide();
                } else if (e.code == "ArrowDown") {
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
                        $('#txtClassNameSearch').focus();
                    }
                }

            }


            else if (indexName == "MainAccountSRIndex") {
                if (e.code == "Enter" || e.code == "NumpadEnter") {
                    var Code = e.target.parentElement.parentElement.cells[0].innerText;
                    var name = e.target.parentElement.parentElement.cells[1].children[0].value;

                    $("#hdnSRMainAccount").val(Code);
                    cSRMainAccount.SetText(name);
                    cSRMainAccount_active.SetText(name)
                    cMainAccountModelSR.Hide();
                } else if (e.code == "ArrowDown") {
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
                        $('#txtMainAccountSRSearch').focus();
                    }
                }

            }
            else if (indexName == "MainAccountPIIndex") {
                if (e.code == "Enter" || e.code == "NumpadEnter") {
                    var Code = e.target.parentElement.parentElement.cells[0].innerText;
                    var name = e.target.parentElement.parentElement.cells[1].children[0].value;

                    $("#hdnPIMainAccount").val(Code);
                    cPIMainAccount.SetText(name);
                    cPIMainAccount_active.SetText(name);
                    cMainAccountModelPI.Hide();
                } else if (e.code == "ArrowDown") {
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
                        $('#txtMainAccountPISearch').focus();
                    }
                }

            }
            else if (indexName == "MainAccountPRIndex") {
                if (e.code == "Enter" || e.code == "NumpadEnter") {
                    var Code = e.target.parentElement.parentElement.cells[0].innerText;
                    var name = e.target.parentElement.parentElement.cells[1].children[0].value;

                    $("#hdnPRMainAccount").val(Code);
                    cPRMainAccount.SetText(name);
                    cPRMainAccount_active.SetText(name);
                    cMainAccountModelPR.Hide();
                } else if (e.code == "ArrowDown") {
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
                        $('#txtMainAccountPRSearch').focus();
                    }
                }

            }
        }




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
                            if (cbtnSave_citys.IsVisible())
                                cbtnSave_citys.DoClick();
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

        //chinmoy comment 2-07-2019

        //function SetHSnPanelEndCallBack() {
        //    LoadingPanel.Hide();
        //    if (cSetHSnPanel.cpHsnCode) {
        //        if (cSetHSnPanel.cpHsnCode != "") {
        //            //cHsnLookUp.gridView.SelectItemsByKey(cSetHSnPanel.cpHsnCode);
        //            cSetHSnPanel.cpHsnCode = null;
        //        } else {
        //            cHsnLookUp.Clear();
        //        }
        //    }
        //}
        //End
        function CmbProClassCodeChanged(s, e) {
            if (s.GetValue() != null) {
                //Chinmoy comment 08-07-2019
                //Start
                //cSetHSnPanel.PerformCallback(s.GetValue());

                //LoadingPanel.SetText('Please wait searching HSN...');
                //LoadingPanel.Show();
                //End
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
        }

        function PackingDetailsOkClick() {

            $('#invalidPackingUom').css({ 'display': 'none' });

            if (ccmbPackingUom.GetValue() == null) {
                $('#invalidPackingUom').css({ 'display': 'block' });
            } else {
                cpackingDetails.Hide();
            }
        }

        function ServicetaxOkClick() {
            cServiceTaxPopup.Hide();
        }
        function isInventoryChanged(s, e) {
            //changeControlStateWithInventory(s.GetValue());

            changeControlStateWithInventory();
        }
        function ServiceItemChanged(s, e) {
            var ServiceItem = ccmbServiceItem.GetValueString();
            if (ServiceItem == "1") {
                cHSNCode.SetEnabled(false);
                cCmbTradingLotUnits.SetEnabled(true);


            }
            else {
                cHSNCode.SetEnabled(true);
                cCmbTradingLotUnits.SetValue('');
                cCmbTradingLotUnits.SetEnabled(false);
            }

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
                    //var ServiceItem = ccmbServiceItem.GetValueString();
                    //if (ServiceItem == "1") {
                    //    cCmbTradingLotUnits.SetEnabled(true);

                    //}
                    //else {
                    //    cCmbTradingLotUnits.SetText('');
                    //    cCmbTradingLotUnits.SetEnabled(false);
                    //}
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

                    var ServiceItem = ccmbServiceItem.GetValueString();
                    if (ServiceItem == "1") {

                        cCmbTradingLotUnits.SetEnabled(true);
                    }
                    else {
                        cCmbTradingLotUnits.SetText('');
                        cCmbTradingLotUnits.SetEnabled(false);
                    }

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
                //Rev Rajdip
                cchkConseiderInstockval.SetEnabled(false);
                cchkConseiderInstockval.SetChecked(false);
                //End Rev Rajdip
                $('#btnBarCodeConfig').attr('disabled', false);
                $('#btnProdConfig').attr('disabled', false);

                $('#btnServiceTaxConfig').attr('disabled', 'disabled');
                cAspxServiceTax.SetValue('');

                $('#btnTDS').attr('disabled', 'disabled');
                cmb_tdstcs.SetValue('');

            } else {
                cCmbProType.SetText('');
                cCmbProType.SetEnabled(false);
                //Rev Rajdip
                cchkConseiderInstockval.SetEnabled(true);
                //End Rev Rajdip
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


                if (($("#hdnUOMNoninventory").val() == "0")) {

                    cCmbTradingLotUnits.SetText('');
                    cCmbTradingLotUnits.SetEnabled(false);
                }


                ctxtDeliveryLot.SetText('1');
                ctxtDeliveryLot.SetEnabled(false);

                if (($("#hdnUOMNoninventory").val() == "0")) {
                    cCmbDeliveryLotUnit.SetText('');
                    cCmbDeliveryLotUnit.SetEnabled(false);
                }
                if (($("#hdnUOMNoninventory").val() == "0")) {
                    ccmbStockUom.SetText('');
                    ccmbStockUom.SetEnabled(false);
                }

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

            //chinmoy added for mantis:0021231 start
            if ($("#hdnUOMNoninventory").val() == "1") {
                var InventoryValue = ccmbIsInventory.GetValue();
                if (InventoryValue == "0") {
                    cCmbTradingLotUnits.SetEnabled(true);
                    cCmbDeliveryLotUnit.SetEnabled(true);
                    ccmbStockUom.SetEnabled(true);
                }
            }
            //End
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
        var _RevisionNo, _DesignNo;
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
                _RevisionNo = ctxtRevisionNo.GetText();
                _DesignNo = ctxtDesignNo.GetText();

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


        function fn_PopOpen() {
            //Note Please Add Attribute On fn_CopyProducts()

            //Surojit
            if ($('#hdnProductMasterComponentMandatoryVisible').val() == "0") {
                $('#divProductMasterComponentMandatory').hide();
            }
            else {
                $('#divProductMasterComponentMandatory').show();
            }
            //Surojit

            cbtnSave_citys.SetVisible(true);
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

            $("#hdVendorsID").val("");
            DeSelectAll('EmpSource');
            //Rev Tanmoy
            cchkComponentService.SetChecked(false);

            gridModelLookup.gridView.UnselectRows();
            //Rev Tanmoy End

            //Rev Rajdip
            cchkConseiderInstockval.SetEnabled(false);
            //End Rev Rajdip
            cSIMainAccount.SetEnabled(true);
            cSRMainAccount.SetEnabled(true);
            cPIMainAccount.SetEnabled(true);
            cPRMainAccount.SetEnabled(true);


            document.getElementById('Keyval_internalId').value = 'Add';
            //document.getElementById('btnUdf').disabled =true;
            //Rev Rajdip
            cbtnSave_CopyValues.SetVisible(false);
            //End Rev Rajdip
            cPopup_Empcitys.SetHeaderText('Add Products');
            document.getElementById('hiddenedit').value = "";
            ctxtPro_Code.SetText('');
            ctxt_CustDocNo.SetText('');
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
            //rev srijeeta
            ctxtpackageqty.SetText('');
            //end of rev srijeeta
            ctxtActiveCostPrice.SetText('');
            ctxtMrp.SetText('');
            ctxtActiveCostPrice.SetText('');
            ccmbStockUom.SetSelectedIndex(-1);
            ctxtMinLvl.SetText('');
            ctxtReorderLvl.SetText('');
            ctxtReorderQty.SetText('');
            ctxtMaxLvl.SetText('');
            ctxtActiveSalesPrice.SetText('');
            ctxtActiveMinSalesPrice.SetText('');
            ctxtActiveMRPSalesPrice.SetText('');
            ctxtActivePurPrice.SetText('');
            //rev srijeeta
            ctxtactivepackageqty.SetText('');
            //end of rev srijeeta
            ctxtCostPrice.SetText('');
            ctxtHeight.SetText('0.00');
            ctxtWidth.SetText('0.00');
            ctxtThickness.SetText('0.00');
            cddlSize.SetSelectedIndex(0);
            //Rev Bapi
            cbtnVendors.SetText('');
            hdVendorsID.Value = '';
            $("#SizeUOM").val('1');
            //Rev Rajdip
            //ctxtSeries.SetText('');
            //ctxtFinish.SetText('');
            // ctxtSubCat.SetText('');
            //End Rev Rajdip

            ctxtLeadtime.SetText('0');
            $("#txtCoverage").val('0.00');
            $("#dvCovg").text('');
            $("#txtVolumn").val('0');
            $("#volumeuom").text('');
            ctxtWeight.SetText('0');
            ctxtPro_Printname.SetText('');

            //ccmbNegativeStk.SetSelectedIndex(0);
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
            //chinmoy comment 22-07-2019
            //cHsnLookUp.Clear();
            //End
            //Debjyoti 31-01-2017

            if (($("#hdnAutoNumStg").val() == "PDAutoNum1") && ($("#hdnTransactionType").val() == "PD")) {
                $("#ddl_Num").show();
                ctxt_CustDocNo.SetEnabled(true);
            }
            else {
                ctxtPro_Code.SetEnabled(true);
            }


            ccmbIsInventory.SetEnabled(true);
            ccmbIsInventory.SetSelectedIndex(0);
            changeControlStateWithInventory();
            $('#reOrderError').css({ 'display': 'None' });
            $('#mrpError').css({ 'display': 'None' });
            cAspxServiceTax.SetValue('');
            $('#btnPackingConfig').attr('disabled', 'disabled');

            //packing details
            ctxtPackingQty.SetValue(1); //0020190: When creating a new Product, in the "Configure UOM conversion" the Quantity and Alt. Quantity will be "1" by default

            if ($("#hdnPackagingQtyZeroProductMaster").val() == "1") {
                ctxtpacking.SetValue();//0023354
            }
            else {
                ctxtpacking.SetValue(1);//0020190: When creating a new Product, in the "Configure UOM conversion" the Quantity and Alt. Quantity will be "1" by default
                ccmbPackingUom.SetSelectedIndex(-1);
            }


            cchkOverideConvertion.SetChecked(false); //Surojit 08-02-2018
            cchkIsMandatory.SetChecked(false); //Surojit 11-02-2018


            //packing details End Here
            caspxInstallation.SetValue('0');
            ccmbBrand.SetValue('');
            cmb_tdstcs.SetValue('');




            cPopup_Empcitys.SetWidth(window.screen.width - 50);
            //cPopup_Empcitys.SetHeight(window.innerHeight.height - 70);
            cPopup_Empcitys.Show();
            //Chinmoy comment 22-07-2019
            //cHsnLookUp.SetEnabled(true);
            //End
            if (($("#hdnAutoNumStg").val() == "PDAutoNum1") && ($("#hdnTransactionType").val() == "PD")) {
                $("#ddl_numberingScheme").val(0)
                $("#ddl_numberingScheme").focus();
            }
            else {
                ctxtPro_Code.Focus();
            }
            //ccmbStatusad.SetSelectedIndex(0);
        }
        //Rev Rajdip
        function fn_CopyProducts(keyValue) {
            debugger;

            //chinmoy added 01-04-2020 start	
            if (($("#hdnAutoNumStg").val() == "PDAutoNum1") && ($("#hdnTransactionType").val() == "PD")) {
                $("#ddl_Num").show();
                $("#dvCustDocNo").show();
                $("#dvShortName").hide();
                $("#ddl_numberingScheme").val(0);
                ctxt_CustDocNo.SetText("");
                ctxt_CustDocNo.SetEnabled(true);
            }
            else {
                $("#ddl_Num").hide();
                $("#dvCustDocNo").hide();
                $("#dvShortName").show();
                ctxtPro_Code.SetText("");
                ctxtPro_Code.SetEnabled(true);
            }
            //End



            //Surojit
            if ($('#hdnProductMasterComponentMandatoryVisible').val() == "0") {
                $('#divProductMasterComponentMandatory').hide();
            }
            else {
                $('#divProductMasterComponentMandatory').show();
            }
            //Surojit
            cbtnSave_citys.SetVisible(true);
            document.getElementById('btnUdf').disabled = false;
            cPopup_Empcitys.SetHeaderText('Copy Products');

            ctxtHeight.SetText('0.00');
            ctxtWidth.SetText('0.00');
            ctxtThickness.SetText('0.00');
            cddlSize.SetSelectedIndex(0);
            $("#SizeUOM").val('1');
            //Rev Rajdip
            // ctxtSeries.SetText('');
            //ctxtFinish.SetText('');
            //End Rev Rajdip

            ctxtLeadtime.SetText('0');
            $("#txtCoverage").val('');
            $("#dvCovg").text('');
            $("#txtVolumn").val('0');
            $("#volumeuom").text('');
            ctxtWeight.SetText('0');
            //ctxtSubCat.SetText('');
            //rev 24514
            ctxtPro_Printname.SetText('');
            //End of rev 24514


            grid.PerformCallback('Copy~' + keyValue);
            document.getElementById('Keyval_internalId').value = 'ProductMaster' + keyValue;
            cPopup_Empcitys.Show();
            grid.Refresh();
        }
        //End Rev Rajdip

        function afterFileUpload() {
            if (document.getElementById('hiddenedit').value == '') {
                grid.PerformCallback('savecity~' + GetObjectID('fileName').value);
            }
            else {
                grid.PerformCallback('updatecity~' + GetObjectID('hiddenedit').value + "~" + GetObjectID('fileName').value);
            }

        }
        //Rev Rajdip
        function afterFileUploadCopyProductSave() {
            // if (document.getElementById('hiddenedit').value == '') {
            //grid.PerformCallback('savecopy~' + GetObjectID('fileName').value);
            grid.PerformCallback('savecopy~' + GetObjectID('hiddenedit').value);
            //}
            //else {
            //    grid.PerformCallback('updatecity~' + GetObjectID('hiddenedit').value + "~" + GetObjectID('fileName').value);
            //}

        }
        //End Rev Rajdip


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
                    grid.PerformCallback('updatecity_active~' + GetObjectID('hiddenedit').value);
                }
            }


        }

        function btnSave_citys() {

            if (($("#hdnAutoNumStg").val() == "PDAutoNum1") && ($("#hdnTransactionType").val() == "PD") && (document.getElementById('Keyval_internalId').value == "Add")) {

                ctxtPro_Code.SetText("ABC");
                $("#hddnDocNo").val(ctxt_CustDocNo.GetText());
                if ($("#ddl_numberingScheme").val() == "0") {
                    jAlert("Please Select Numbering Scheme.");
                    return false;
                }
                else if (ctxt_CustDocNo.GetText() == "") {
                    jAlert("Please Enter Unique ID.");
                    return false;
                }
            }
            if (($("#hdnAutoNumStg").val() == "PDAutoNum1") && ($("#hdnTransactionType").val() == "PD") && (document.getElementById('Keyval_internalId').value != "Add")) {
                ctxtPro_Code.SetText("ABC");
                $("#hddnDocNo").val(ctxt_CustDocNo.GetText());
            }

            //var name = document.getElementById('txtPro_Name').value;
           

            var MinSalePrice = ctxtMinSalePrice.GetValue();
            var Mrp = ctxtMrp.GetText();
            if (parseFloat(MinSalePrice) > parseFloat(Mrp)) {
                jAlert("MRP must be greater than Minimum Sell Price");
                return;
            }
            //End Rev Rajdip
            var PackingUom = ccmbPackingUom.GetValue();
            var ConvertionOverideVisible = $('#hdnConvertionOverideVisible').val();
            var ShowUOMConversionInEntry = $('#hdnShowUOMConversionInEntry').val();
            var ServiceItem = ccmbServiceItem.GetValueString();

            if ((((PackingUom == "0" || PackingUom == "" || PackingUom == null)) && (ShowUOMConversionInEntry == "1")) && ccmbIsInventory.GetValue() == 1) {
                jAlert(' "Show UOM Conversion In Entry" is Activated.You must Select alternate UOM from Product Master - Configure UOM Coinversion');
                return false;
            }
            if ($("#hdnAltNameMandatory").val() == "1") {
                if (ctxtPro_Printname.GetValue() == null) {
                    jAlert("Alternate Name Mandatory.")
                    return false
                }
            }

            if ($("#hdnProductTypeMandatory").val() == "1") {
                if (ccmbIsInventory.GetValue() == 1) {
                    if (cCmbProType.GetValue() == null) {
                        jAlert("Product Type Mandatory.")
                        return false
                    }
                }
            }

            if ($("#hdnUOMConverMandatoryProductMaster").val() == "1") {
                if (ccmbIsInventory.GetValue() == 1) {
                    if (ccmbPackingUom.GetValue() == null ) {
                        jAlert("UOM Conversion Mandatory.")
                        return false
                    }

                    if (ctxtpacking.GetValue() == '0.0000') {
                        jAlert("UOM Conversion Mandatory.")
                        return false
                    }
                }
            }

            if (ccmbIsInventory.GetValue() == 0) {
                if ((ctxtPro_Code.GetText() != '') && (ctxtPro_Name.GetText().trim() != '')) {
                    if (upload1.GetText().trim() != '') {
                        upload1.Upload();
                    } else {
                        var name = ctxtPro_Name.GetText();
                        // if (name.match(/"(.*?)"/)) {
                        let result = name.includes("\"") ? "Yes" : "No";
                        if (result == 'Yes') {

                            jConfirm('Avoid Double Quotes in Product Name as it affects IRN Generation if you are using eInvoicing?', 'Confirmation Dialog', function (r) {
                                if (r == true)
                                { }
                                else
                                {
                                    afterFileUpload();
                                }
                            });

                        }
                        else {
                            afterFileUpload();
                        }
                    }
                }
            } else {
                if ((ctxtPro_Code.GetText() != '') && (ctxtPro_Name.GetText().trim() != '') && (cCmbTradingLotUnits.GetText().trim() != '') && (cCmbDeliveryLotUnit.GetText().trim() != '') && (ccmbStockUom.GetValue() != null)) {

                    if (validReorder() && validMRP()) {
                        if (upload1.GetText().trim() != '') {
                            upload1.Upload();
                        } else {
                            var name = ctxtPro_Name.GetText();

                            let result = name.includes("\"") ? "Yes" : "No";
                            if (result == 'Yes') {

                                jConfirm('Avoid Double Quotes in Product Name as it affects IRN Generation if you are using eInvoicing?', 'Confirmation Dialog', function (r) {
                                    if (r == true)
                                    { }
                                    else
                                    {
                                        afterFileUpload();
                                    }
                                });

                            }
                            else {
                                afterFileUpload();
                            }

                        }
                    }
                }
            }
            if (cProClassCode.GetValue() == null) {
            }
            //grid.Refresh();

        }

        //Rev Rajdip For Copy To Products
        function btnSave_CopytoProducts() {

            var PackingUom = ccmbPackingUom.GetValue();
            //document.getElementById('hiddenedit').value = "";
            var ConvertionOverideVisible = $('#hdnConvertionOverideVisible').val();
            var ShowUOMConversionInEntry = $('#hdnShowUOMConversionInEntry').val();
            var ServiceItem = ccmbServiceItem.GetValueString();



            if (($("#hdnAutoNumStg").val() == "PDAutoNum1") && ($("#hdnTransactionType").val() == "PD")) {
                ctxtPro_Code.SetText("ABC");
                $("#hddnDocNo").val(ctxt_CustDocNo.GetText());
                if ($("#ddl_numberingScheme").val() == "0") {
                    jAlert("Please Select Numbering Scheme.");
                    return false;
                }
                else if (ctxt_CustDocNo.GetText() == "") {
                    jAlert("Please Enter Unique ID.");
                    return false;
                }
            }



            if (((((PackingUom == "0" || PackingUom == "" || PackingUom == null)) && (ShowUOMConversionInEntry == "1")) && ccmbIsInventory.GetValue() == 1))
                //Rev Rajdip
            {
                jAlert('"Show UOM Conversion In Entry" is Activated.You must Select alternate UOM from Product Master - Configure UOM Coinversion');
                return false;
            }

            if ($("#hdnAltNameMandatory").val() == "1") {
                if (ctxtPro_Printname.GetValue() == null) {
                    jAlert("Alternate Name Mandatory.")
                    return false
                }
            }

            if ($("#hdnProductTypeMandatory").val() == "1") {
                if (ccmbIsInventory.GetValue() == 1) {
                    if (cCmbProType.GetValue() == null) {
                        jAlert("Product Type Mandatory.")
                        return false
                    }
                }
            }
            if ($("#hdnUOMConverMandatoryProductMaster").val() == "1") {
                if (ccmbIsInventory.GetValue() == 1) {
                    if (ccmbPackingUom.GetValue() == null ) {
                        jAlert("UOM Conversion Mandatory.")
                        return false
                    }
                    if (ctxtpacking.GetValue() == '0.0000') {
                        jAlert("UOM Conversion Mandatory.")
                        return false
                    }
                }
            }
            if (($("#hdnAutoNumStg").val() == "PDAutoNum1") && ($("#hdnTransactionType").val() == "PD")) {
                ctxtPro_Code.SetText("MandatoryChk");
            }


            if (ccmbIsInventory.GetValue() == 0) {
                if ((ctxtPro_Code.GetText() != '') && (ctxtPro_Name.GetText().trim() != '')) {
                    if (upload1.GetText().trim() != '') {
                        upload1.Upload();
                    } else {
                        afterFileUploadCopyProductSave();
                    }
                }
            } else {
                if ((ctxtPro_Code.GetText() != '') && (ctxtPro_Name.GetText().trim() != '') && (cCmbTradingLotUnits.GetText().trim() != '') && (cCmbDeliveryLotUnit.GetText().trim() != '') && (ccmbStockUom.GetValue() != null)) {

                    if (validReorder() && validMRP()) {
                        if (upload1.GetText().trim() != '') {
                            upload1.Upload();
                        } else {
                            afterFileUploadCopyProductSave();
                        }
                    }
                }
            }

            if (cProClassCode.GetValue() == null) {

            }
            //grid.Refresh();

        }
        //End Rev Rajdip

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

        function AvoidDoubleQuotes() {
            var retval = true;

            var name = ctxtPro_Name.GetText();
            if (!name.match(/"(.*?)"/)) {
                
                jConfirm('Avoid Double Quotes in Product Name as it affects IRN Generation if you are using eInvoicing?', 'Confirmation Dialog', function (r) {
                    if (r == true) {
                        retval = false;
                    }
                    else
                    {
                        retval = true;
                    }
                });
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
                            grid.PerformCallback('savecity~');
                        }
                        else {
                            //alert("in update");
                            grid.PerformCallback('updatecity~' + GetObjectID('hiddenedit').value);
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

            gridModelLookup.gridView.UnselectRows();
            /*-------------------------------------------------Arindam-----------------------------------------------------------*/

            var url = '/OMS/management/master/View/ViewProduct.html?v=0.07&&id=' + keyValue;

            CAspxDirectCustomerViewPopup.SetWidth(window.screen.width - 200);
            CAspxDirectCustomerViewPopup.SetHeight(window.innerHeight - 100);
            CAspxDirectCustomerViewPopup.SetContentUrl(url);
            CAspxDirectCustomerViewPopup.RefreshContentUrl();
            CAspxDirectCustomerViewPopup.Show();




            /*-------------------------------------------------Arindam-----------------------------------------------------------*/
            //  fn_Editcity(keyValue);
            //  cbtnSave_citys.SetVisible(false);

        }

        function fn_Editcity(keyValue) {
            debugger;
            //Note :: Please Add Attribute on btnSave_CopytoProducts()
            document.getElementById('HiddenField_status').value = '0';
            cbtnSave_citys.SetVisible(true);
            document.getElementById('btnUdf').disabled = false;
            cPopup_Empcitys.SetHeaderText('Modify Products');

            ctxtHeight.SetText('0.00');
            ctxtWidth.SetText('0.00');
            ctxtThickness.SetText('0.00');
            cddlSize.SetSelectedIndex(0);
            $("#SizeUOM").val('1');
            //End Rev
            //ctxtSeries.SetText('');
            //ctxtFinish.SetText('');
            //End Rev Rajdip

            ctxtLeadtime.SetText('0');
            $("#txtCoverage").val('');
            $("#dvCovg").text('');
            $("#txtVolumn").val('0');
            $("#volumeuom").text('');
            ctxtWeight.SetText('0');
            //Rev Bapi
            cbtnVendors.SetText('');
            hdVendorsID.Value = '';
            //ctxtSubCat.SetText('');
            ctxtPro_Printname.SetText('');

            if (($("#hdnAutoNumStg").val() == "PDAutoNum1") && ($("#hdnTransactionType").val() == "PD")) {
                $("#ddl_Num").hide();
                ctxt_CustDocNo.SetEnabled(false);

            }
            var OtherDetails = {}
            OtherDetails.key = keyValue;

            gridModelLookup.gridView.UnselectRows();
            grid.PerformCallback('Edit~' + keyValue);

            ///////////////////////////

            function GetVendorsEditValue() {
                debugger;
                $.ajax({
                    type: "POST",
                    url: "../Master/sProducts.aspx/GetVendorsDetailsEdit",
                    data: JSON.stringify(OtherDetails),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {


                    }
                });

                LoadDocument();
            }
            /////////////////////////////////////////////

            document.getElementById('Keyval_internalId').value = 'ProductMaster' + keyValue;
            function selectValue() {
                var type = ($("[id$='rdl_MultipleType']").find(":checked").val() != null) ? $("[id$='rdl_MultipleType']").find(":checked").val() : "";
                if (type == 'S') {
                    $('#Multipletype').hide();
                    $('#singletype').show();
                    $('#tdCashBankLabel').show();
                    $("#ClearSingle").hide();
                }
                else if (type == 'M') {

                    $('#tdCashBankLabel').hide();

                    $('#Multipletype').show();
                    $('#singletype').hide();
                    $("#ClearSingle").show();
                }
            }
            grid.Refresh();

        }


        function fn_activeEdit(keyValue, status) {
            debugger;

            document.getElementById('HiddenField_status').value = '1';
            gridModelLookup.gridView.UnselectRows();

            cbtnSave_citys.SetVisible(true);
            document.getElementById('btnUdf').disabled = false;
            cPopup_Empcitys_active.SetHeaderText('Product Attribute');
            grid.PerformCallback('Active~' + keyValue);
            document.getElementById('Keyval_internalId').value = 'ProductMaster' + keyValue;
        }

        function fn_Deletecity(keyValue) {
            //if (confirm("Confirm Delete?")) {
            //    grid.PerformCallback('Delete~' + keyValue);
            //}
            jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    grid.PerformCallback('Delete~' + keyValue);
                }
            });
            grid.Refresh();
        }


        function componentEndCallBack(s, e) {
            console.log(e);
            // cPopup_Empcitys.Show();
        }

        var mainAccountInUse = [];
        function grid_EndCallBack() {
            debugger;

            if (grid.cpinsert != null) {
                if (grid.cpinsert == 'Success') {
                    jAlert('Saved Successfully');
                    //alert('Saved Successfully');
                    //................CODE  UPDATED BY sAM ON 18102016.................................................
                    ctxtPro_Name.GetInputElement().readOnly = false;
                    //................CODE ABOVE UPDATED BY sAM ON 18102016.................................................
                    cPopup_Empcitys.Hide();
                    grid.Refresh();
                }
                else if (grid.cpinsert == 'Validation') {
                    jAlert('MRP Must be greater than Minimum selling Price');
                }
                else if (grid.cpinsert == 'fail') {
                    jAlert("Error On Insertion \n 'Please Try Again!!'")
                }
                else if (grid.cpinsert == 'UDFManddratory') {
                    jAlert("UDF is set as Mandatory. Please enter values.", "Alert", function () { OpenUdf(); });

                }
                else {
                    //Rev Rajdip
                    jAlert(grid.cpinsert);
                    return;
                    //End Rev Rajdip
                    //cPopup_Empcitys.Hide();
                }
            }
            if (grid.cpMainAccountInUse != null) {
                if (grid.cpMainAccountInUse != '') {
                    for (var mainCount = 0; mainCount < grid.cpMainAccountInUse.split('~').length; mainCount++) {
                        mainAccountInUse.push(grid.cpMainAccountInUse.split('~')[mainCount]);
                    }
                }
            }

            if (grid.cpEdit != null) {
                var col = grid.cpEdit.split('~')[15];
                var size = grid.cpEdit.split('~')[16];

                //................. Code Added By Sam on 25102016....................
                var sizeapplicable = grid.cpEdit.split('~')[18];
                var colorapplicable = grid.cpEdit.split('~')[19];


                //................. Code Added By Sam on 25102016....................

                if (($("#hdnAutoNumStg").val() == "PDAutoNum1") && ($("#hdnTransactionType").val() == "PD")) {
                    ctxt_CustDocNo.SetText(grid.cpEdit.split('~')[1]);
                }
                else {
                    ctxtPro_Code.SetText(grid.cpEdit.split('~')[1]);
                }
                ctxtPro_Name.SetText(grid.cpEdit.split('~')[2]);
                //ctxtPro_Name.GetInputElement().readOnly = true;
                ctxtPro_Description.SetText(grid.cpEdit.split('~')[3]);
                cCmbProType.SetValue(grid.cpEdit.split('~')[4]);
                //chinmoy edited 19-07-2019
                //cCmbProClassCode.SetSelectedIndex(-1);
                $("#ClassId").val(grid.cpEdit.split('~')[6]);

                //End
                // cCmbProClassCode.SetValue(grid.cpEdit.split('~')[6]);
                ctxtGlobalCode.SetText(grid.cpEdit.split('~')[7]);
                GlobalCode = grid.cpEdit.split('~')[7];
                ctxtTradingLot.SetText(grid.cpEdit.split('~')[8]);
                cCmbTradingLotUnits.SetValue(grid.cpEdit.split('~')[9]);
                cCmbQuoteCurrency.SetValue(grid.cpEdit.split('~')[10]);
                ctxtQuoteLot.SetText(grid.cpEdit.split('~')[11]);
                cCmbQuoteLotUnit.SetValue(grid.cpEdit.split('~')[12]);
                ctxtDeliveryLot.SetText(grid.cpEdit.split('~')[13]);
                cCmbDeliveryLotUnit.SetValue(grid.cpEdit.split('~')[14]);
                if (col != '') {
                    cCmbProductColor.SetValue(grid.cpEdit.split('~')[15]);
                    ProdColor = grid.cpEdit.split('~')[15];
                }
                else {
                    cCmbProductColor.SetValue('0');
                    ProdColor = 0;
                }
                if (size != '') {
                    cCmbProductSize.SetValue(grid.cpEdit.split('~')[16]);
                    ProdSize = grid.cpEdit.split('~')[16];
                }
                else {
                    cCmbProductSize.SetValue('0');
                    ProdSize = 0;
                }


                if (sizeapplicable == 'True') {
                    Rrdblapp.SetSelectedIndex(0);
                    SizeApp = 0;
                }
                else {
                    Rrdblapp.SetSelectedIndex(1);
                    SizeApp = 1;
                }

                if (colorapplicable == 'True') {
                    RrdblappColor.SetSelectedIndex(0);
                    ColApp = 0;
                }
                else {
                    RrdblappColor.SetSelectedIndex(1);
                    ColApp = 1;
                }
                GetObjectID('hiddenedit').value = grid.cpEdit.split('~')[17];

                //Code Added by Debjyoti 30-12-2016
                if (grid.cpEdit.split('~')[20] != '0') {
                    cCmbBarCodeType.SetValue(grid.cpEdit.split('~')[20]);
                    barCodeType = grid.cpEdit.split('~')[20];
                }
                ctxtBarCodeNo.SetText(grid.cpEdit.split('~')[21]);
                BarCode = grid.cpEdit.split('~')[21];


                //Code added by debjyoti 04-01-2017
                if (grid.cpEdit.split('~')[22] == 'False')
                    ccmbIsInventory.SetValue('0');
                else
                    ccmbIsInventory.SetValue('1');

                cCmbStockValuation.SetValue(grid.cpEdit.split('~')[23].trim());
                if (grid.cpEdit.split('~')[24].trim() == '0') {
                    ctxtSalePrice.SetText('');
                    ctxtActiveSalesPrice.SetText(grid.cpEdit.split('~')[24].trim());
                }
                else {
                    ctxtSalePrice.SetText(grid.cpEdit.split('~')[24].trim());
                    ctxtActiveSalesPrice.SetText(grid.cpEdit.split('~')[24].trim());
                }



                if (grid.cpEdit.split('~')[25].trim() == '0') {
                    ctxtMinSalePrice.SetText('');
                    ctxtActiveMinSalesPrice.SetText('');
                }
                else {
                    ctxtMinSalePrice.SetText(grid.cpEdit.split('~')[25].trim());
                    ctxtActiveMinSalesPrice.SetText(grid.cpEdit.split('~')[25].trim());
                }


                if (grid.cpEdit.split('~')[26].trim() == '0') {
                    ctxtPurPrice.SetText('');
                    ctxtactivepackageqty.SetText('');
                }
                else {
                    ctxtPurPrice.SetText(grid.cpEdit.split('~')[26].trim());
                    ctxtActivePurPrice.SetText(grid.cpEdit.split('~')[26].trim());
                }
                //rev srijeeta
                if (grid.cpEdit.split('~')[104].trim() == '0') {
                    ctxtpackageqty.SetText('');
                    ctxtactivepackageqty.SetText('');
                }
                else {
                    ctxtpackageqty.SetText(grid.cpEdit.split('~')[104].trim());
                    ctxtactivepackageqty.SetText(grid.cpEdit.split('~')[104].trim());
                }
                //end of rev srijeeta

                if (grid.cpEdit.split('~')[27].trim() == '0') {
                    ctxtActiveMRPSalesPrice.SetText('');
                    ctxtMrp.SetText('');
                }
                else {
                    ctxtActiveMRPSalesPrice.SetText(grid.cpEdit.split('~')[27].trim());
                    ctxtMrp.SetText(grid.cpEdit.split('~')[27].trim());
                }



                ccmbStockUom.SetValue(grid.cpEdit.split('~')[28]);
                ctxtMinLvl.SetText(grid.cpEdit.split('~')[29]);

                ctxtMinLvl_active.SetText(grid.cpEdit.split('~')[29]);//Arindam

                ctxtReorderLvl.SetText(grid.cpEdit.split('~')[30]);

                ctxtReorderLvl_active.SetText(grid.cpEdit.split('~')[30]);//Arindam

                ccmbNegativeStk.SetValue(grid.cpEdit.split('~')[31].trim());
                cCmbTaxCodeSale.SetValue(grid.cpEdit.split('~')[32].trim());
                taxCodeSale = grid.cpEdit.split('~')[32].trim();

                cCmbTaxCodePur.SetValue(grid.cpEdit.split('~')[33].trim());
                taxCodePur = grid.cpEdit.split('~')[33].trim();

                if (grid.cpEdit.split('~')[34].trim() == '') {
                    cCmbTaxScheme.SetValue(0);
                    taxScheme = 0;
                }
                else {
                    cCmbTaxScheme.SetValue(grid.cpEdit.split('~')[34].trim());
                    taxScheme = grid.cpEdit.split('~')[34].trim();
                }
                if (grid.cpEdit.split('~')[35].trim() == 'True') {
                    cChkAutoApply.SetChecked(true);
                    autoApply = true;
                    GetCheckBoxValue(true);
                } else {
                    cChkAutoApply.SetChecked(false);
                    autoApply = false;
                    GetCheckBoxValue(false);
                }

                cProdImage.SetImageUrl(grid.cpEdit.split('~')[36].trim());
                document.getElementById('fileName').value = grid.cpEdit.split('~')[36].trim();
                gridLookup.Clear();
                cComponentPanel.PerformCallback(grid.cpEdit.split('~')[37].trim());
                cCmbStatus.SetValue(grid.cpEdit.split('~')[38].trim());

                //  ctxtHsnCode.SetText(grid.cpEdit.split('~')[39].trim()); 
                //chinmoy comment 22-07-2019
                //cHsnLookUp.gridView.SelectItemsByKey(grid.cpEdit.split('~')[39].trim());
                $("#hdnHSN").val(grid.cpEdit.split('~')[39].trim())
                //End
                cAspxServiceTax.SetValue(grid.cpEdit.split('~')[40].trim());
                // ccmbStatusad.SetValue(grid.cpEdit.split('~')[31].trim());
                //Debjyoti 31-01-2017
                //packing details
                ctxtPackingQty.SetValue(grid.cpEdit.split('~')[41].trim());
                ctxtpacking.SetValue(grid.cpEdit.split('~')[42].trim());

                if (grid.cpEdit.split('~')[43].trim() != "0") {
                    ccmbPackingUom.SetValue(grid.cpEdit.split('~')[43].trim());
                }

                //Rev Surojit 14-06-2019
                if (grid.cpEdit.split('~')[41].trim() == "0.00000" && grid.cpEdit.split('~')[42].trim() == "0.00000") {
                    ctxtPackingQty.SetValue(1); //0020190: When creating a new Product, in the "Configure UOM conversion" the Quantity and Alt. Quantity will be "1" by default
                    if ($("#hdnPackagingQtyZeroProductMaster").val() == "1") {
                        ctxtpacking.SetValue();//0023354
                    }
                    else {
                        ctxtpacking.SetValue(1); //0020190: When creating a new Product, in the "Configure UOM conversion" the Quantity and Alt. Quantity will be "1" by default
                    }
                }
                //End of rev Surojit 14-06-2019

                //packing details End Here
                console.log(grid.cpEdit.split('~')[44]);
                if (grid.cpEdit.split('~')[44] == 'False')
                    caspxInstallation.SetValue('0');
                else
                    caspxInstallation.SetValue('1');
                ccmbBrand.SetValue(grid.cpEdit.split('~')[45].trim());

                if (grid.cpEdit.split('~')[46] == 'True')
                    ccmbIsCapitalGoods.SetValue('1');
                else
                    ccmbIsCapitalGoods.SetValue('0');
                //  ccmbIsCapitalGoods.SetValue(grid.cpEdit.split('~')[46].trim());

                cmb_tdstcs.SetValue(grid.cpEdit.split('~')[47]);

                if (grid.cpEdit.split('~')[48] == "True") {
                    ccmbOldUnit.SetValue('1');
                }
                else {
                    ccmbOldUnit.SetValue('0');
                }


                //ccmbsalesInvoice.SetValue(grid.cpEdit.split('~')[49].trim());
                //ccmbSalesReturn.SetValue(grid.cpEdit.split('~')[50].trim());
                //ccmbPurInvoice.SetValue(grid.cpEdit.split('~')[51].trim());
                //ccmbPurReturn.SetValue(grid.cpEdit.split('~')[52].trim());

                $("#hdnSIMainAccount").val(grid.cpEdit.split('~')[49].trim());
                $("#hdnSRMainAccount").val(grid.cpEdit.split('~')[50].trim());
                $("#hdnPIMainAccount").val(grid.cpEdit.split('~')[51].trim());
                $("#hdnPRMainAccount").val(grid.cpEdit.split('~')[52].trim());


                //Subhabrata
                console.log(grid.cpEdit.split('~')[53]);
                if (grid.cpEdit.split('~')[53] == "True") {
                    cchkFurtherance.SetValue(true);
                }
                else {
                    cchkFurtherance.SetValue(false);
                }

                //alert(grid.cpEdit.split('~')[54]);
                if (grid.cpEdit.split('~')[54] == 'True') {
                    ccmbServiceItem.SetValue('1');
                }
                else if (grid.cpEdit.split('~')[54] == 'False') {
                    ccmbServiceItem.SetValue('0');
                }
                else {
                    ccmbServiceItem.SetValue('0');
                }

                var ServiceItem = ccmbServiceItem.GetValueString();
                if (ServiceItem == "1") {
                    cHSNCode.SetEnabled(false);
                }
                else {
                    cHSNCode.SetEnabled(true);
                }
                cSIMainAccount.SetText(grid.cpEdit.split('~')[55].trim());
                cSRMainAccount.SetText(grid.cpEdit.split('~')[56].trim());
                cPIMainAccount.SetText(grid.cpEdit.split('~')[57].trim());
                cPRMainAccount.SetText(grid.cpEdit.split('~')[58].trim());



                cSIMainAccount_active.SetText(grid.cpEdit.split('~')[55].trim());
                cSRMainAccount_active.SetText(grid.cpEdit.split('~')[56].trim());
                cPIMainAccount_active.SetText(grid.cpEdit.split('~')[57].trim());
                cPRMainAccount_active.SetText(grid.cpEdit.split('~')[58].trim());



                if (grid.cpEdit.split('~')[59].trim() == "1") {
                    cSIMainAccount.SetEnabled(false);
                }
                if (grid.cpEdit.split('~')[60].trim() == "1") {
                    cSRMainAccount.SetEnabled(false);
                }
                if (grid.cpEdit.split('~')[61].trim() == "1") {
                    cPIMainAccount.SetEnabled(false);
                }
                if (grid.cpEdit.split('~')[62].trim() == "1") {
                    cPRMainAccount.SetEnabled(false);
                }

                ctxtReorderQty.SetText(grid.cpEdit.split('~')[63]);
                ctxtMaxLvl.SetText(grid.cpEdit.split('~')[64]);


                ctxtMaxLvl_active.SetText(grid.cpEdit.split('~')[64]);//arindam
                ctxtReorderQty_active.SetText(grid.cpEdit.split('~')[63]);//arindam


                //Surojit 08-02-2019
                if (grid.cpEdit.split('~')[65].trim() == 'True') {
                    cchkOverideConvertion.SetChecked(true);
                } else {
                    cchkOverideConvertion.SetChecked(false);
                }

                if (grid.cpEdit.split('~')[66].trim() == "0") {
                    cchkOverideConvertion.SetEnabled(false);
                    $('#tblOverideConvertion').hide();
                }
                else {
                    cchkOverideConvertion.SetEnabled(true);
                    $('#tblOverideConvertion').show();
                }
                //Surojit 08-02-2019

                //Surojit 11-02-2018
                if (grid.cpEdit.split('~')[67].trim() == 'True') {
                    cchkIsMandatory.SetChecked(true);
                } else {
                    cchkIsMandatory.SetChecked(false);
                }
                //Surojit 11-02-2018

                //Surojit 14-02-2018
                if (grid.cpEdit.split('~')[68].trim() == "0") {
                    cchkIsMandatory.SetEnabled(false);
                    $('#divProductMasterComponentMandatory').hide();
                } else {
                    cchkIsMandatory.SetEnabled(true);
                    $('#divProductMasterComponentMandatory').show();
                }
                //Surojit 14-02-2018
                if (($("#hdnAutoNumStg").val() == "PDAutoNum1") && ($("#hdnTransactionType").val() == "PD")) {

                    ctxt_CustDocNo.SetEnabled(false);
                }
                else {
                    ctxtPro_Code.SetEnabled(false);
                }
                ccmbIsInventory.SetEnabled(false);

                changeControlStateWithInventory();
                if (document.getElementById('HiddenField_status').value != '1') {
                    cPopup_Empcitys.SetWidth(window.screen.width - 50);
                    // cPopup_Empcitys.SetHeight(window.screen.height - 70);
                    cPopup_Empcitys.Show();
                }


                if (document.getElementById('HiddenField_status').value == '1') {
                    //cPopup_Empcitys.Hide();
                    //cPopup_Empcitys_active.SetWidth(700);

                    cPopup_Empcitys_active.Show();
                    //document.getElementById('HiddenField_status').value = '0';

                }

                ctxtHeight.SetText(grid.cpEdit.split('~')[69].trim());
                ctxtWidth.SetText(grid.cpEdit.split('~')[70].trim());
                ctxtThickness.SetText(grid.cpEdit.split('~')[71].trim());
                cddlSize.SetValue(grid.cpEdit.split('~')[72].trim());
                $("#SizeUOM").val(grid.cpEdit.split('~')[73].trim());
                //Rev Rajdip
                //ctxtSeries.SetText(grid.cpEdit.split('~')[74].trim());
                cddlSeries.SetValue(grid.cpEdit.split('~')[74].trim());
                //ctxtFinish.SetText(grid.cpEdit.split('~')[75].trim());
                //ctxtSubCat.SetText(grid.cpEdit.split('~')[83].trim());
                cddlsubcat.SetValue(grid.cpEdit.split('~')[83].trim());
                cddlfinish.SetValue(grid.cpEdit.split('~')[75].trim());
                //End Rev Rajdip

                ctxtLeadtime.SetText(grid.cpEdit.split('~')[76].trim());
                $("#txtCoverage").val(grid.cpEdit.split('~')[77].trim());
                $("#txtVolumn").val(grid.cpEdit.split('~')[79].trim());
                $("#dvCovg").text(cddlSize.GetText() + '²');
                $("#dvvolume").text(cddlSize.GetText() + '³');
                ctxtWeight.SetText(grid.cpEdit.split('~')[81].trim());
                ctxtPro_Printname.SetText(grid.cpEdit.split('~')[82].trim());

                cProClassCode.SetText(grid.cpEdit.split('~')[84].trim());
                cHSNCode.SetText(grid.cpEdit.split('~')[85].trim());
                //Rev Rajdip
                var CONSITEMINSTKVAL = grid.cpEdit.split('~')[86].trim();
                if (CONSITEMINSTKVAL == "1") {
                    cchkConseiderInstockval.SetChecked(true);
                    cchkConseiderInstockval.SetEnabled(false);
                }
                else {
                    cchkConseiderInstockval.SetChecked(false);
                }
                var productapplication = grid.cpEdit.split('~')[87].trim();
                if (productapplication != "0") {
                    cddlproductapplication.SetValue(grid.cpEdit.split('~')[87].trim());//ProductApplication
                }
                var productnature = grid.cpEdit.split('~')[88].trim();
                if (productnature != "0") {
                    cddlproductnature.SetValue(grid.cpEdit.split('~')[88].trim());//ProductNature
                }

                ctxtdimension.SetValue(grid.cpEdit.split('~')[89].trim());
                ctxtpedestalno.SetValue(grid.cpEdit.split('~')[90].trim());
                ctxtcatno.SetValue(grid.cpEdit.split('~')[91].trim());
                ctxtwarranty.SetValue(grid.cpEdit.split('~')[92].trim());
                //End Rev Rajdip

                //Tanmoy 24-04-2020
                if (grid.cpEdit.split('~')[93].trim() == '1') {
                    cchkComponentService.SetChecked(true);
                } else {
                    cchkComponentService.SetChecked(false);
                }

                cModelPanel.PerformCallback('SetModelGrid' + '~' + grid.cpEdit.split('~')[0]);
                //Tanmoy 24-04-2020
                if (grid.cpEdit.split('~')[94] == '1')
                    ccmbReplaceable.SetValue('1');
                else
                    ccmbReplaceable.SetValue('0');

                ctxtDesignNo.SetText(grid.cpEdit.split('~')[95].trim());
                ctxtRevisionNo.SetText(grid.cpEdit.split('~')[96].trim());

                if (grid.cpEdit.split('~')[97].trim() != "") {
                    ccmbItemType.SetValue(grid.cpEdit.split('~')[97].trim());
                }

                //Mantis Issue 24530
                CcmbAppliArea.SetValue(grid.cpEdit.split('~')[98].trim());
                ctxtMovement.SetText(grid.cpEdit.split('~')[99].trim());
                //alert(grid.cpEdit.split('~')[100].trim())
                ctxtActiveCostPrice.SetText(grid.cpEdit.split('~')[100].trim());
                ctxtCostPrice.SetText(grid.cpEdit.split('~')[100].trim());

                cbtnSave_CopyValues.SetVisible(false);
                cbtnSave_citys.SetVisible(true);
                ctxtPro_Name.Focus();
                //End of Mantis Issue 24530

                //Code Added by Bapi
                if (grid.cpEdit.split('~')[101] != '0') {

                    cbtnVendors.SetValue(grid.cpEdit.split('~')[101]);

                    hdVendorsID.value = grid.cpEdit.split('~')[102];

                }


                arrMultiPopup.pop();
                var ProdObjEdit = new Object();
                var ProdArrEdit = new Object();
                ProdArrEdit = JSON.parse(grid.cpEdit.split('~')[103]);
                ProdObjEdit.Name = "EmpSource";
                ProdObjEdit.ArraySource = ProdArrEdit;
                arrMultiPopup.push(ProdObjEdit);
                var OtherDetails = {}

                if ($.trim($("#txtEmpSearch").val()) == "" || $.trim($("#txtEmpSearch").val()) == null) {
                    return false;
                }
                OtherDetails.SearchKey = $("#txtEmpSearch").val();

                if (e.code == "Enter" || e.code == "NumpadEnter") {

                    var HeaderCaption = [];
                    HeaderCaption.push("Vendor Name");

                    if ($("#txtEmpSearch").val() != "") {

                        callonServerM("../Master/sProducts.aspx/GetVendorsList", OtherDetails, "EmpTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "EmpSource");

                    }
                }


                //end rev bapi

                //Mantis Issue 24530

                //CcmbAppliArea.SetValue(grid.cpEdit.split('~')[98].trim());
                //ctxtMovement.SetText(grid.cpEdit.split('~')[99].trim());
                //alert(grid.cpEdit.split('~')[100].trim())
                //ctxtActiveCostPrice.SetText(grid.cpEdit.split('~')[100].trim());
                //ctxtCostPrice.SetText(grid.cpEdit.split('~')[100].trim());

                //cbtnSave_CopyValues.SetVisible(false);
                //cbtnSave_citys.SetVisible(true);
                //ctxtPro_Name.Focus();
                //End of Mantis Issue 24530
            }

            // debugger;

            //Rev Rajdip For copy to Products
            if (grid.cpCopy != null) {
                cbtnSave_CopyValues.SetVisible(true);
                cbtnSave_citys.SetVisible(false);
                var col = grid.cpCopy.split('~')[15];
                var size = grid.cpCopy.split('~')[16];

                //................. Code Added By Sam on 25102016....................
                var sizeapplicable = grid.cpCopy.split('~')[18];
                var colorapplicable = grid.cpCopy.split('~')[19];


                //................. Code Added By Sam on 25102016....................
                //if (($("#hdnAutoNumStg").val() == "PDAutoNum1") && ($("#hdnTransactionType").val() == "PD")) {
                //    ctxt_CustDocNo.SetText(grid.cpCopy.split('~')[1]);
                //}
                //else {
                //    ctxtPro_Code.SetText(grid.cpCopy.split('~')[1]);
                //}
                ctxtPro_Name.SetText(grid.cpCopy.split('~')[2]);
                //ctxtPro_Name.GetInputElement().readOnly = true;
                ctxtPro_Description.SetText(grid.cpCopy.split('~')[3]);
                cCmbProType.SetValue(grid.cpCopy.split('~')[4]);
                //chinmoy edited 19-07-2019
                //cCmbProClassCode.SetSelectedIndex(-1);
                $("#ClassId").val(grid.cpCopy.split('~')[6]);
                //End
                // cCmbProClassCode.SetValue(grid.cpEdit.split('~')[6]);
                ctxtGlobalCode.SetText(grid.cpCopy.split('~')[7]);
                GlobalCode = grid.cpCopy.split('~')[7];
                ctxtTradingLot.SetText(grid.cpCopy.split('~')[8]);
                cCmbTradingLotUnits.SetValue(grid.cpCopy.split('~')[9]);
                cCmbQuoteCurrency.SetValue(grid.cpCopy.split('~')[10]);
                ctxtQuoteLot.SetText(grid.cpCopy.split('~')[11]);
                cCmbQuoteLotUnit.SetValue(grid.cpCopy.split('~')[12]);
                ctxtDeliveryLot.SetText(grid.cpCopy.split('~')[13]);
                cCmbDeliveryLotUnit.SetValue(grid.cpCopy.split('~')[14]);
                if (col != '') {
                    cCmbProductColor.SetValue(grid.cpCopy.split('~')[15]);
                    ProdColor = grid.cpCopy.split('~')[15];
                }
                else {
                    cCmbProductColor.SetValue('0');
                    ProdColor = 0;
                }
                if (size != '') {
                    cCmbProductSize.SetValue(grid.cpCopy.split('~')[16]);
                    ProdSize = grid.cpCopy.split('~')[16];
                }
                else {
                    cCmbProductSize.SetValue('0');
                    ProdSize = 0;
                }


                if (sizeapplicable == 'True') {
                    Rrdblapp.SetSelectedIndex(0);
                    SizeApp = 0;
                }
                else {
                    Rrdblapp.SetSelectedIndex(1);
                    SizeApp = 1;
                }

                if (colorapplicable == 'True') {
                    RrdblappColor.SetSelectedIndex(0);
                    ColApp = 0;
                }
                else {
                    RrdblappColor.SetSelectedIndex(1);
                    ColApp = 1;
                }
                GetObjectID('hiddenedit').value = grid.cpCopy.split('~')[17];

                //Code Added by Debjyoti 30-12-2016
                if (grid.cpCopy.split('~')[20] != '0') {
                    cCmbBarCodeType.SetValue(grid.cpCopy.split('~')[20]);
                    barCodeType = grid.cpCopy.split('~')[20];
                }
                ctxtBarCodeNo.SetText(grid.cpCopy.split('~')[21]);
                BarCode = grid.cpCopy.split('~')[21];


                //Code added by debjyoti 04-01-2017
                if (grid.cpCopy.split('~')[22] == 'False')
                    ccmbIsInventory.SetValue('0');
                else
                    ccmbIsInventory.SetValue('1');

                cCmbStockValuation.SetValue(grid.cpCopy.split('~')[23].trim());
                if (grid.cpCopy.split('~')[24].trim() == '0')
                    ctxtSalePrice.SetText('');
                else
                    ctxtSalePrice.SetText(grid.cpCopy.split('~')[24].trim());

                if (grid.cpCopy.split('~')[25].trim() == '0')
                    ctxtMinSalePrice.SetText('');
                else
                    ctxtMinSalePrice.SetText(grid.cpCopy.split('~')[25].trim());

                if (grid.cpCopy.split('~')[26].trim() == '0')
                    ctxtPurPrice.SetText('');
                else
                    ctxtPurPrice.SetText(grid.cpCopy.split('~')[26].trim());
                //rev srijeeta
                if (grid.cpCopy.split('~')[104].trim() == '0')
                    ctxtpackageqty.SetText('');
                else
                    ctxtpackageqty.SetText(grid.cpCopy.split('~')[104].trim());
                //end of rev srijeeta

                if (grid.cpCopy.split('~')[27].trim() == '0')
                    ctxtMrp.SetText('');
                else
                    ctxtMrp.SetText(grid.cpCopy.split('~')[27].trim());

                ccmbStockUom.SetValue(grid.cpCopy.split('~')[28]);
                ctxtMinLvl.SetText(grid.cpCopy.split('~')[29]);

                ctxtMinLvl_active.SetText(grid.cpCopy.split('~')[29]);//Arindam

                ctxtReorderLvl.SetText(grid.cpCopy.split('~')[30]);

                ctxtReorderLvl_active.SetText(grid.cpCopy.split('~')[30]);//Arindam

                ccmbNegativeStk.SetValue(grid.cpCopy.split('~')[31].trim());
                cCmbTaxCodeSale.SetValue(grid.cpCopy.split('~')[32].trim());
                taxCodeSale = grid.cpCopy.split('~')[32].trim();

                cCmbTaxCodePur.SetValue(grid.cpCopy.split('~')[33].trim());
                taxCodePur = grid.cpCopy.split('~')[33].trim();

                if (grid.cpCopy.split('~')[34].trim() == '') {
                    cCmbTaxScheme.SetValue(0);
                    taxScheme = 0;
                }
                else {
                    cCmbTaxScheme.SetValue(grid.cpCopy.split('~')[34].trim());
                    taxScheme = grid.cpCopy.split('~')[34].trim();
                }
                if (grid.cpCopy.split('~')[35].trim() == 'True') {
                    cChkAutoApply.SetChecked(true);
                    autoApply = true;
                    GetCheckBoxValue(true);
                } else {
                    cChkAutoApply.SetChecked(false);
                    autoApply = false;
                    GetCheckBoxValue(false);
                }
                cProdImage.SetImageUrl(grid.cpCopy.split('~')[36].trim());
                document.getElementById('fileName').value = grid.cpCopy.split('~')[36].trim();
                gridLookup.Clear();
                cComponentPanel.PerformCallback(grid.cpCopy.split('~')[37].trim());
                cCmbStatus.SetValue(grid.cpCopy.split('~')[38].trim());

                //  ctxtHsnCode.SetText(grid.cpEdit.split('~')[39].trim()); 
                //chinmoy comment 22-07-2019
                //cHsnLookUp.gridView.SelectItemsByKey(grid.cpEdit.split('~')[39].trim());
                $("#hdnHSN").val(grid.cpCopy.split('~')[39].trim())
                //End
                cAspxServiceTax.SetValue(grid.cpCopy.split('~')[40].trim());
                // ccmbStatusad.SetValue(grid.cpEdit.split('~')[31].trim());
                //Debjyoti 31-01-2017
                //packing details
                ctxtPackingQty.SetValue(grid.cpCopy.split('~')[41].trim());
                ctxtpacking.SetValue(grid.cpCopy.split('~')[42].trim());

                if (grid.cpCopy.split('~')[43].trim() != "0") {
                    ccmbPackingUom.SetValue(grid.cpCopy.split('~')[43].trim());
                }

                //Rev Surojit 14-06-2019
                if (grid.cpCopy.split('~')[41].trim() == "0.00000" && grid.cpCopy.split('~')[42].trim() == "0.00000") {
                    ctxtPackingQty.SetValue(1); //0020190: When creating a new Product, in the "Configure UOM conversion" the Quantity and Alt. Quantity will be "1" by default
                    ctxtpacking.SetValue(1); //0020190: When creating a new Product, in the "Configure UOM conversion" the Quantity and Alt. Quantity will be "1" by default
                }
                //End of rev Surojit 14-06-2019

                //packing details End Here
                console.log(grid.cpCopy.split('~')[44]);
                if (grid.cpCopy.split('~')[44] == 'False')
                    caspxInstallation.SetValue('0');
                else
                    caspxInstallation.SetValue('1');
                ccmbBrand.SetValue(grid.cpCopy.split('~')[45].trim());

                if (grid.cpCopy.split('~')[46] == 'True')
                    ccmbIsCapitalGoods.SetValue('1');
                else
                    ccmbIsCapitalGoods.SetValue('0');
                //  ccmbIsCapitalGoods.SetValue(grid.cpEdit.split('~')[46].trim());

                cmb_tdstcs.SetValue(grid.cpCopy.split('~')[47]);

                if (grid.cpCopy.split('~')[48] == "True")
                    ccmbOldUnit.SetValue('1');
                else
                    ccmbOldUnit.SetValue('0');


                //ccmbsalesInvoice.SetValue(grid.cpEdit.split('~')[49].trim());
                //ccmbSalesReturn.SetValue(grid.cpEdit.split('~')[50].trim());
                //ccmbPurInvoice.SetValue(grid.cpEdit.split('~')[51].trim());
                //ccmbPurReturn.SetValue(grid.cpEdit.split('~')[52].trim());

                $("#hdnSIMainAccount").val(grid.cpCopy.split('~')[49].trim());
                $("#hdnSRMainAccount").val(grid.cpCopy.split('~')[50].trim());
                $("#hdnPIMainAccount").val(grid.cpCopy.split('~')[51].trim());
                $("#hdnPRMainAccount").val(grid.cpCopy.split('~')[52].trim());


                //Subhabrata
                console.log(grid.cpCopy.split('~')[53]);
                if (grid.cpCopy.split('~')[53] == "True")
                    cchkFurtherance.SetValue(true);
                else
                    cchkFurtherance.SetValue(false);

                //alert(grid.cpEdit.split('~')[54]);
                if (grid.cpCopy.split('~')[54] == 'True')
                    ccmbServiceItem.SetValue('1');
                else if (grid.cpCopy.split('~')[54] == 'False')
                    ccmbServiceItem.SetValue('0');
                else
                    ccmbServiceItem.SetValue('0');


                cSIMainAccount.SetText(grid.cpCopy.split('~')[55].trim());
                cSRMainAccount.SetText(grid.cpCopy.split('~')[56].trim());
                cPIMainAccount.SetText(grid.cpCopy.split('~')[57].trim());
                cPRMainAccount.SetText(grid.cpCopy.split('~')[58].trim());



                cSIMainAccount_active.SetText(grid.cpCopy.split('~')[55].trim());
                cSRMainAccount_active.SetText(grid.cpCopy.split('~')[56].trim());
                cPIMainAccount_active.SetText(grid.cpCopy.split('~')[57].trim());
                cPRMainAccount_active.SetText(grid.cpCopy.split('~')[58].trim());




                var productapplication = grid.cpCopy.split('~')[86].trim();
                if (productapplication != "0") {
                    cddlproductapplication.SetValue(grid.cpCopy.split('~')[86].trim());//ProductApplication
                }
                var productnature = grid.cpCopy.split('~')[87].trim();
                if (productnature != "0") {
                    cddlproductnature.SetValue(grid.cpCopy.split('~')[87].trim());//ProductNature
                }


                if (grid.cpCopy.split('~')[59].trim() == "1") {
                    cSIMainAccount.SetEnabled(true);
                }
                if (grid.cpCopy.split('~')[60].trim() == "1") {
                    cSRMainAccount.SetEnabled(true);
                }
                if (grid.cpCopy.split('~')[61].trim() == "1") {
                    cPIMainAccount.SetEnabled(true);
                }
                if (grid.cpCopy.split('~')[62].trim() == "1") {
                    cPRMainAccount.SetEnabled(true);
                }

                ctxtReorderQty.SetText(grid.cpCopy.split('~')[63]);
                ctxtMaxLvl.SetText(grid.cpCopy.split('~')[64]);


                ctxtMaxLvl_active.SetText(grid.cpCopy.split('~')[64]);//arindam
                ctxtReorderQty_active.SetText(grid.cpCopy.split('~')[63]);//arindam


                //Surojit 08-02-2019
                if (grid.cpCopy.split('~')[65].trim() == 'True') {
                    cchkOverideConvertion.SetChecked(true);
                } else {
                    cchkOverideConvertion.SetChecked(false);
                }

                if (grid.cpCopy.split('~')[66].trim() == "0") {
                    cchkOverideConvertion.SetEnabled(true);
                    $('#tblOverideConvertion').hide();
                }
                else {
                    cchkOverideConvertion.SetEnabled(true);
                    $('#tblOverideConvertion').show();
                }
                //Surojit 08-02-2019

                //Surojit 11-02-2018
                if (grid.cpCopy.split('~')[67].trim() == 'True') {
                    cchkIsMandatory.SetChecked(true);
                } else {
                    cchkIsMandatory.SetChecked(false);
                }
                //Surojit 11-02-2018

                //Surojit 14-02-2018
                if (grid.cpCopy.split('~')[68].trim() == "0") {
                    cchkIsMandatory.SetEnabled(false);
                    $('#divProductMasterComponentMandatory').hide();
                } else {
                    cchkIsMandatory.SetEnabled(true);
                    $('#divProductMasterComponentMandatory').show();
                }
                //Surojit 14-02-2018



                if (($("#hdnAutoNumStg").val() == "PDAutoNum1") && ($("#hdnTransactionType").val() == "PD")) {

                    ctxt_CustDocNo.SetEnabled(true);
                }
                else {
                    ctxtPro_Code.SetEnabled(true);
                }

                ccmbIsInventory.SetEnabled(true);

                changeControlStateWithInventory();
                if (document.getElementById('HiddenField_status').value != '1') {
                    cPopup_Empcitys.SetWidth(window.screen.width - 50);
                    // cPopup_Empcitys.SetHeight(window.screen.height - 70);
                    cPopup_Empcitys.Show();
                }


                if (document.getElementById('HiddenField_status').value == '1') {
                    //cPopup_Empcitys.Hide();
                    //cPopup_Empcitys_active.SetWidth(700);

                    cPopup_Empcitys_active.Show();
                    //document.getElementById('HiddenField_status').value = '0';

                }

                ctxtHeight.SetText(grid.cpCopy.split('~')[69].trim());
                ctxtWidth.SetText(grid.cpCopy.split('~')[70].trim());
                ctxtThickness.SetText(grid.cpCopy.split('~')[71].trim());
                cddlSize.SetValue(grid.cpCopy.split('~')[72].trim());
                $("#SizeUOM").val(grid.cpCopy.split('~')[73].trim());
                //Rev Rajdip
                //ctxtSeries.SetText(grid.cpCopy.split('~')[74].trim());
                cddlSeries.SetValue(grid.cpCopy.split('~')[74].trim());
                //ctxtFinish.SetText(grid.cpCopy.split('~')[75].trim());
                cddlfinish.SetValue(grid.cpCopy.split('~')[74].trim());
                //End Rev Rajdip

                ctxtLeadtime.SetText(grid.cpCopy.split('~')[76].trim());
                $("#txtCoverage").val(grid.cpCopy.split('~')[77].trim());
                $("#txtVolumn").val(grid.cpCopy.split('~')[79].trim());
                $("#dvCovg").text(cddlSize.GetText() + '²');
                $("#dvvolume").text(cddlSize.GetText() + '³');
                ctxtWeight.SetText(grid.cpCopy.split('~')[81].trim());
                ctxtPro_Printname.SetText(grid.cpCopy.split('~')[82].trim());
                //ctxtSubCat.SetText(grid.cpCopy.split('~')[83].trim());
                cddlsubcat.SetValue(grid.cpCopy.split('~')[83].trim());
                cProClassCode.SetText(grid.cpCopy.split('~')[84].trim());
                cHSNCode.SetText(grid.cpCopy.split('~')[85].trim());

                ctxtdimension.SetValue(grid.cpCopy.split('~')[86].trim());
                ctxtpedestalno.SetValue(grid.cpCopy.split('~')[87].trim());

                ctxtcatno.SetValue(grid.cpCopy.split('~')[90].trim());
                ctxtwarranty.SetValue(grid.cpCopy.split('~')[91].trim());
                //$('#btnServiceTaxConfig').attr('disabled', 'disabled');
                //newly comment
                //document.getElementById("btnServiceTaxConfig").disabled = false;
                //document.getElementById("btnTDS").disabled = false;

                //document.getElementById("cbtnSave_citys").Style.display = "None";
                //document.getElementById("cbtnSave_CopyValues").Style.display = "block";

                CcmbAppliArea.SetValue(grid.cpCopy.split('~')[95].trim());
                ctxtMovement.SetText(grid.cpCopy.split('~')[96].trim());

                ctxtPro_Name.Focus();

                //Tanmoy 24-04-2020
                if (grid.cpEdit.split('~')[92].trim() == '1') {
                    cchkComponentService.SetChecked(true);
                } else {
                    cchkComponentService.SetChecked(false);
                }

                cModelPanel.PerformCallback('SetModelGrid' + '~' + grid.cpCopy.split('~')[0]);
                //Tanmoy 24-04-2020
            }
            //End Rev Rajdip For Copy to Products
            if (grid.cpUpdate != null) {
                if (grid.cpUpdate == 'Success') {
                    jAlert('Saved Successfully');
                    cPopup_Empcitys.Hide();
                    cPopup_Empcitys_active.Hide();
                    grid.Refresh();
                }
                else {
                    jAlert("Error on Updation\n'Please Try again!!'")
                    //cPopup_Empcitys.Hide();
                }
            }
            if (grid.cpUpdateValid != null) {
                if (grid.cpUpdateValid == "StateInvalid") {
                    jAlert("Please Select proper country state and city");
                    //cPopup_Empcitys.Show();
                    //cCmbState.Focus();
                    //alert(GetObjectID('<%#hiddenedit.ClientID%>').value);
                    //grid.PerformCallback('Edit~'+GetObjectID('<%#hiddenedit.ClientID%>').value);
                    //grid.cpUpdateValid=null;
                }
            }
            if (grid.cpDelete != null) {
                if (grid.cpDelete == 'Success') {
                    jAlert('Deleted Successfully');
                    grid.Refresh();
                }
                //else
                //    jAlert("Error on deletion\n'Please Try again!!'")
            }
            //debjyoti
            if (grid.cpErrormsg != null) {
                jAlert(grid.cpErrormsg);
                grid.cpErrormsg = null;
            }

            if (grid.cpExists != null) {
                if (grid.cpExists == "Exists") {
                    jAlert('Record already Exists');
                    //cPopup_Empcitys.Hide();
                }
                else {
                    jAlert("Error on operation \n 'Please Try again!!'")
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
            if (document.getElementById('hiddenedit').value != '') {
                var ServiceItem = ccmbServiceItem.GetValueString();
                if (ServiceItem == "1") {
                    cHSNCode.SetEnabled(false);
                }
                else {
                    cHSNCode.SetEnabled(true);
                }
            }

            $('.dxpc-closeBtn').click(function () {
                fn_btnCancel();
            });

            $('#ddl_numberingScheme').change(function () {
                //

                var NoSchemeTypedtl = $(this).val();
                var NoSchemeId = NoSchemeTypedtl.toString().split('~')[0];
                var NoSchemeType = NoSchemeTypedtl.toString().split('~')[1];
                $("#hdnNumberingId").val(NoSchemeId);
                var schemeLength = NoSchemeTypedtl.toString().split('~')[2];

                if (NoSchemeType == '1') {
                    ctxt_CustDocNo.SetText('Auto');
                    ctxt_CustDocNo.SetEnabled(false);

                    //SetDocMaxLength(schemeLength);
                    $('#txt_CustDocNo input').attr('maxlength', schemeLength);
                    $("#hddnDocNo").val('Auto');

                }
                else if (NoSchemeType == '0') {
                    schemeLength = 80
                    ctxt_CustDocNo.SetText("");
                    ctxt_CustDocNo.SetEnabled(true);
                    $('#txt_CustDocNo input').attr('maxlength', schemeLength);
                }
                else if ($('#ddl_numberingScheme').val() == "0") {
                    ctxt_CustDocNo.SetText("");
                    ctxt_CustDocNo.SetEnabled(true);
                }

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
            //chinmoy comment below line 30-07-2019
            //if (procode != "") {
            $.ajax({
                type: "POST",
                url: "sProducts.aspx/CheckUniqueName",
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
        // }
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
    </style>


    <script type="text/javascript">
        function OnAddBusinessClick(keyValue) {
            var url = '../../master/AssignIndustry.aspx?id1=' + keyValue + '&EntType=product';
            window.location.href = url;
        }
        function PopupOpen(obj) {
            var URL = '/OMS/Management/Master/Product_Document.aspx?idbldng=' + obj + '&type=Product';
            //OnMoreInfoClick(URL, "Products Document Details", '1000px', '400px', "Y");
            //document.getElementById("marketsGrid_DXPEForm_efnew_DXEditor1_I").focus();
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

        function OpenAvailableStock(ProductId, ProductCode) {
            if (ProductId == "") {
                jAlert("Please select a Product First.");
            } else {
                $('#HDSelectedProduct').val(ProductId);
                cShowAvailableStock.Show();
                cAvailableStockgrid.PerformCallback();
            }
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
        function widthFunction(s, e) {
            var width = ASPxClientUtils.GetDocumentClientWidth() * 0.5; // 50%  
            s.SetWidth(width);
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

        /*Rev 1.0*/

        /*select
        {
            height: 30px !important;
            border-radius: 4px;
            -webkit-appearance: none;
            position: relative;
            z-index: 1;
            background-color: transparent;
            padding-left: 10px !important;
            padding-right: 22px !important;
        }*/

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

        #ASPxFromDate , #ASPxToDate , #ASPxASondate , #ASPxAsOnDate , #txtDOB , #txtAnniversary , #txtcstVdate , #txtLocalVdate ,
        #txtCINVdate , #txtincorporateDate , #txtErpValidFrom , #txtErpValidUpto , #txtESICValidFrom , #txtESICValidUpto , #FormDate , #toDate
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        .dxeDisabled_PlasticBlue
        {
            z-index: 0 !important;
        }

        #ASPxFromDate_B-1 , #ASPxToDate_B-1 , #ASPxASondate_B-1 , #ASPxAsOnDate_B-1 , #txtDOB_B-1 , #txtAnniversary_B-1 , #txtcstVdate_B-1 ,
        #txtLocalVdate_B-1 , #txtCINVdate_B-1 , #txtincorporateDate_B-1 , #txtErpValidFrom_B-1 , #txtErpValidUpto_B-1 , #txtESICValidFrom_B-1 ,
        #txtESICValidUpto_B-1 , #FormDate_B-1 , #toDate_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #ASPxFromDate_B-1 #ASPxFromDate_B-1Img , #ASPxToDate_B-1 #ASPxToDate_B-1Img , #ASPxASondate_B-1 #ASPxASondate_B-1Img , #ASPxAsOnDate_B-1 #ASPxAsOnDate_B-1Img ,
        #txtDOB_B-1 #txtDOB_B-1Img ,
        #txtAnniversary_B-1 #txtAnniversary_B-1Img ,
        #txtcstVdate_B-1 #txtcstVdate_B-1Img ,
        #txtLocalVdate_B-1 #txtLocalVdate_B-1Img , #txtCINVdate_B-1 #txtCINVdate_B-1Img , #txtincorporateDate_B-1 #txtincorporateDate_B-1Img ,
        #txtErpValidFrom_B-1 #txtErpValidFrom_B-1Img , #txtErpValidUpto_B-1 #txtErpValidUpto_B-1Img , #txtESICValidFrom_B-1 #txtESICValidFrom_B-1Img ,
        #txtESICValidUpto_B-1 #txtESICValidUpto_B-1Img , #FormDate_B-1 #FormDate_B-1Img , #toDate_B-1 #toDate_B-1Img
        {
            display: none;
        }

        .dxtcLite_PlasticBlue > .dxtc-stripContainer .dxtc-activeTab, .dxgvFooter_PlasticBlue
        {
            background: #1b5ea4 !important;
        }

        /*select.btn
        {
            padding-right: 10px !important;
        }*/

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

        .TableMain100 #ShowGrid , .TableMain100 #ShowGridList , .TableMain100 #ShowGridRet , .TableMain100 #EmployeeGrid , .TableMain100 #GrdEmployee,
        .TableMain100 #GrdHolidays , #cityGrid
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
                margin-top: 7px;
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

        .col-md-3>label, .col-md-3>span
        {
            margin-top: 0 !important;
        }

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

        /*select.btn
        {
           position: relative;
           z-index: 0;
        }*/

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

        .panel-title h3
        {
            padding-top: 0;
            padding-bottom: 0;
        }

        .btn-radius
        {
            padding: 4px 11px !important;
            border-radius: 4px !important;
        }

        .crossBtn
        {
             right: 30px;
             top: 25px;
        }

        .mb-10
        {
            margin-bottom: 10px;
        }

        .btn-cust
        {
            background-color: #108b47 !important;
            color: #fff;
        }

        .btn-cust:hover
        {
            background-color: #097439 !important;
            color: #fff;
        }

        .gHesder
        {
            background: #1b5ca0 !important;
            color: #ffffff !important;
            padding: 6px 0 6px !important;
        }

        .close
        {
             color: #fff;
             opacity: .5;
             font-weight: 400;
        }

        .mt-24
        {
            margin-top: 24px;
        }

        .col-md-3
        {
            margin-top: 8px;
        }

        #upldBigLogo , #upldSmallLogo
        {
            width: 100%;
        }

        #DivSetAsDefault
        {
            margin-top: 25px;
        }

        /*.dxeDisabled_PlasticBlue, .aspNetDisabled {
            opacity: 0.4 !important;
            color: #ffffff !important;
        }*/
                /*.padTopbutton {
            padding-top: 27px;
        }*/
        /*#lookup_project
        {
            max-width: 100% !important;
        }*/
        /*Rev end 1.0*/
    </style>
    <script>
        function gridRowclick(s, e) {

            $('#cityGrid').find('tr').removeClass('rowActive');
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
    </script>
    <script>
        $(document).ready(function () {
            $('.navbar-minimalize').click(function (e) {
                console.log('nav');
                grid.Refresh();
            });
        })
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
        $(function () {
            cModelPanel.PerformCallback('BindModelGrid' + '~' + "All");
        });
        function selectAllModel() {
            gridModelLookup.gridView.SelectRows();
        }
        function unselectAllModel() {
            gridModelLookup.gridView.UnselectRows();
        }
        function CloseModelLookup() {
            gridModelLookup.ConfirmCurrentSelection();
            gridModelLookup.HideDropDown();
            gridModelLookup.Focus();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:HiddenField runat="server" ID="hdnProductMasterComponentMandatoryVisible" />
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main">
        <div class="panel-heading">
        <div class="panel-title">
            <h3>Products</h3>
        </div>
    </div>
        <div class="form_main">
        <div class="Main">
            <%-- <div class="TitleArea">
                <strong><span style="color: #000099">marketss</span></strong>
            </div>--%>
            <div class="SearchArea">
                <div class="FilterSide clearfix">
                    <div>
                        <%--<a href="javascript:ShowHideFilter('s');"><span style="color: #000099; text-decoration: underline">Show Filter</span></a>--%>
                        <% if (rights.CanAdd)
                           { %>
                        <a class="btn btn-success btn-radius " href="javascript:void(0);" onclick="fn_PopOpen()" id="AddBtn"><span class="btn-icon"><i class="fa fa-plus"></i></span><span><u>A</u>dd New</span> </a><%} %>
                        <% if (rights.CanExport)
                           { %>
                        <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-radius " OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" OnChange="if(!AvailableExportOption()){return false;}" AutoPostBack="true">
                            <asp:ListItem Value="0">Export to</asp:ListItem>
                            <asp:ListItem Value="1">PDF</asp:ListItem>
                            <asp:ListItem Value="2">XLS</asp:ListItem>
                            <asp:ListItem Value="3">RTF</asp:ListItem>
                            <asp:ListItem Value="4">CSV</asp:ListItem>
                        </asp:DropDownList>
                        <%} %>
                        <%--Rev work start 10.06.2022 Mantise issue:0024783: Customer & Product master import required for ERP--%>
                        <asp:LinkButton ID="lnlDownloaderexcel" runat="server" OnClick="lnlDownloaderexcel_Click" CssClass="btn btn-info btn-radius pull-rigth mBot0">Download Format</asp:LinkButton>
                        <button type="button" onclick="ImportUpdatePopOpenProductTarget();" class="btn btn-primary btn-radius">Import(Add/Update)</button>
                        <button type="button" class="btn btn-warning btn-radius" data-toggle="modal" data-target="#modalSS" id="btnViewLog" onclick="ViewLogData();">View Log</button>
                        <%--Rev work close 10.06.2022 Mantise issue:0024783: Customer & Product master import required for ERP--%>
                    </div>                     
                    <%-- <div>
                        <a class="btn btn-primary" href="javascript:ShowHideFilter('All');"><span>All Records</span></a>
                    </div>--%>
                </div>
                <div class="clear"></div>
                <%--<div class="ExportSide pull-right">
                    <div>
                        <dxe:ASPxComboBox ID="cmbExport" runat="server" AutoPostBack="true"
                            Font-Bold="False" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged"
                            ValueType="System.Int32" Width="130px">
                            <Items>
                                <dxe:ListEditItem Text="Select" Value="0" />
                                <dxe:ListEditItem Text="PDF" Value="1" />
                                <dxe:ListEditItem Text="XLS" Value="2" />
                                <dxe:ListEditItem Text="RTF" Value="3" />
                                <dxe:ListEditItem Text="CSV" Value="4" />
                            </Items>
                            <Border BorderColor="black" />
                            <DropDownButton Text="Export">
                            </DropDownButton>
                        </dxe:ASPxComboBox>
                    </div>
                </div>--%>
            </div>
            <div class="clear"></div>
            <%--debjyoti 22-12-2016--%>
            <dxe:ASPxPopupControl ID="ASPXPopupControl" runat="server"
                CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popup" Height="630px"
                Width="600px" HeaderText="Add/Modify UDF" Modal="true" AllowResize="true" ResizingMode="Postponed">
                <HeaderTemplate>
                    <span>UDF</span>
                    <dxe:ASPxImage ID="ASPxImage3" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader">
                        <ClientSideEvents Click="function(s, e){ 
                                                            popup.Hide();
                                                        }" />
                    </dxe:ASPxImage>
                </HeaderTemplate>
                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">
                    </dxe:PopupControlContentControl>
                </ContentCollection>
            </dxe:ASPxPopupControl>
            <asp:HiddenField runat="server" ID="IsUdfpresent" />
            <asp:HiddenField runat="server" ID="Keyval_internalId" />
            <%--End debjyoti 22-12-2016--%>


            <div class="GridViewArea relative">
                <dxe:ASPxGridView ID="cityGrid" runat="server" AutoGenerateColumns="False" ClientInstanceName="grid"
                    KeyFieldName="sProducts_ID" Width="100%"
                    DataSourceID="EntityServerModeDataSource" OnCustomCallback="cityGrid_CustomCallback"
                    CssClass="pull-left" SettingsBehavior-AllowFocusedRow="true" SettingsDataSecurity-AllowEdit="false" Settings-VerticalScrollableHeight="280" Settings-VerticalScrollBarMode="Auto"
                    SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" Settings-HorizontalScrollBarMode="Auto">
                    <SettingsSearchPanel Visible="True" Delay="7000" />
                    <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowFilterRow="true" ShowFilterRowMenu="True" />
                    <Columns>
                        <%-- Rev Sayantani--%>
                        <%-- <dxe:GridViewDataTextColumn FieldName="sProducts_ID" Visible="false" SortOrder="Descending" Width="0">
                             <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                              <Settings AllowAutoFilterTextInputTimer="False" />
                         </dxe:GridViewDataTextColumn>--%>
                        <dxe:GridViewDataTextColumn FieldName="sProducts_ID" Visible="false" ShowInCustomizationForm="false" SortOrder="Descending" Width="0">
                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                        <%--   End of Rev Sayantani--%>
                        <dxe:GridViewDataTextColumn Caption="Short Name (Unique)" FieldName="sProducts_Code" ReadOnly="True"
                            Visible="True" VisibleIndex="0" FixedStyle="Left" Width="160px">

                            <Settings AllowAutoFilterTextInputTimer="False" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="Name" FieldName="sProducts_Name" ReadOnly="True"
                            Visible="True" VisibleIndex="1" FixedStyle="Left" Width="160px">

                            <Settings AllowAutoFilterTextInputTimer="False" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="Description" FieldName="sProducts_Description" ReadOnly="True"
                            Visible="True" VisibleIndex="2" Width="200px">

                            <Settings AllowAutoFilterTextInputTimer="False" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Brand" FieldName="Brand_Name" ReadOnly="True"
                            Visible="True" VisibleIndex="3">
                            <Settings AutoFilterCondition="Contains" />

                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>


                        <dxe:GridViewDataTextColumn Caption="Inventory" FieldName="sProduct_IsInventory" ReadOnly="True"
                            Visible="True" VisibleIndex="3" Width="80px">
                            <Settings AutoFilterCondition="Contains" />

                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>


                        <dxe:GridViewDataTextColumn Caption="Service Item?" FieldName="Is_ServiceItem" ReadOnly="True"
                            Visible="True" VisibleIndex="3" Width="80px">
                            <Settings AutoFilterCondition="Contains" />

                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>

                        <%--<dxe:GridViewDataTextColumn Caption="Class Name" FieldName="ProductClass_Name" ReadOnly="True"
                            Visible="True" FixedStyle="Left" VisibleIndex="4">
                            <Settings AutoFilterCondition="Contains" />
                            <EditFormSettings Visible="True" />
                        </dxe:GridViewDataTextColumn>--%>

                        <dxe:GridViewDataTextColumn Caption="Class Name" FieldName="ProductClass_Name" VisibleIndex="4" Width="140px">
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>


                        <%--  <dxe:GridViewDataTextColumn Caption="HSN Code" FieldName="HSNCODE" ReadOnly="True"
                            Visible="True" FixedStyle="Left" VisibleIndex="5">
                            <EditFormSettings Visible="True" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>--%>
                        <dxe:GridViewDataTextColumn Caption="HSN/SAC" FieldName="HSNCODE" VisibleIndex="5">
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Capital Goods?" FieldName="sProduct_IsCapitalGoods" ReadOnly="True"
                            Visible="True" VisibleIndex="6">
                            <Settings AutoFilterCondition="Contains" />
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Sales Invoice Ledger" FieldName="sInv_MainAccount" ReadOnly="True"
                            Visible="True" VisibleIndex="7" Width="160px">
                            <Settings AutoFilterCondition="Contains" />
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Sales Return Ledger" FieldName="sRet_MainAccount" ReadOnly="True"
                            Visible="True" VisibleIndex="8" Width="160px">
                            <Settings AutoFilterCondition="Contains" />
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>


                        <dxe:GridViewDataTextColumn Caption="Purchase Invoice Ledger" FieldName="pInv_MainAccount" ReadOnly="True"
                            Visible="True" VisibleIndex="9" Width="160px">
                            <Settings AutoFilterCondition="Contains" />
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>


                        <dxe:GridViewDataTextColumn Caption="Purchase Return Ledger" FieldName="pRet_MainAccount" ReadOnly="True"
                            Visible="True" VisibleIndex="10" Width="160px">
                            <Settings AutoFilterCondition="Contains" />
                            <Settings AllowAutoFilterTextInputTimer="False" />

                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="Available Stock" FieldName="Available_Stock" ReadOnly="True"
                            Visible="True" VisibleIndex="11" Width="100px">
                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <Settings AutoFilterCondition="Contains" />
                            <Settings AllowAutoFilterTextInputTimer="False" />

                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Quantity" FieldName="Product_Quantity" ReadOnly="True"
                            Visible="True" VisibleIndex="12" Width="100px">
                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <Settings AutoFilterCondition="Contains" />
                            <Settings AllowAutoFilterTextInputTimer="False" />

                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="UOM" FieldName="UOM_Name" ReadOnly="True"
                            Visible="True" VisibleIndex="13" Width="130px">
                            <Settings AutoFilterCondition="Contains" />
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Alt. Quantity" FieldName="Packing_Quantity" ReadOnly="True"
                            Visible="True" VisibleIndex="14" Width="100px">
                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <Settings AutoFilterCondition="Contains" />
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Alt. UOM" FieldName="Alt_UOM_Name" ReadOnly="True"
                            Visible="True" VisibleIndex="15" Width="130px">
                            <Settings AutoFilterCondition="Contains" />
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Override Conversion" FieldName="OverideConvertion" ReadOnly="True"
                            Visible="True" VisibleIndex="16" Width="130px">
                            <Settings AutoFilterCondition="Contains" />
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="Replaceable" FieldName="sProduct_IsReplaceable" ReadOnly="True"
                            Visible="True" VisibleIndex="17" Width="130px">
                            <Settings AutoFilterCondition="Contains" />
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="Component" FieldName="isComponentService" ReadOnly="True"
                            Visible="True" VisibleIndex="18" Width="130px">
                            <Settings AutoFilterCondition="Contains" />
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn ReadOnly="True" CellStyle-HorizontalAlign="Center" Width="0">
                            <HeaderStyle HorizontalAlign="Center" />

                            <CellStyle HorizontalAlign="Center"></CellStyle>
                            <HeaderTemplate>
                            </HeaderTemplate>
                            <DataItemTemplate>
                                <div class='floatedBtnArea'>
                                    <% if (rights.CanView)
                                       { %>
                                    <a href="javascript:void(0);" id="DocunemtInsert" onclick="PopupOpen(<%#Eval("sProducts_ID")%>)" title="" class="" style="text-decoration: none;">
                                        <span class='ico ColorThree'><i class='fa fa-file-word-o'></i></span><span class='hidden-xs'>Document</span>
                                    </a>
                                    <%} %>
                                    <%if (rights.CanEdit)
                                      { %>
                                    <a href="javascript:void(0);" onclick="fn_Editcity('<%# Container.KeyValue %>')" title="" class="">
                                        <%--  <a href="javascript:void(0);" onclick="fn_Editcity(<%#Eval("sProducts_ID")%>)" title="" class="">--%>
                                        <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Edit</span></a>
                                    <%} %>

                                    <%if (rights.CanSpecialEdit)
                                      { %>
                                    <a href="javascript:void(0);" onclick="fn_activeEdit('<%# Container.KeyValue %>','<%#Eval("sProduct_Status")%>')" title="" class="">
                                        <span class='ico ColorFour'><i class='fa fa-file-text-o'></i></span><span class='hidden-xs'>Product Attribute</span></a>
                                    <%} %>


                                    <%if (rights.CanView)
                                      { %>
                                    <a href="javascript:void(0);" onclick="fn_ViewProduct('<%# Container.KeyValue %>')" title="" class="">
                                        <span class='ico ColorFive'><i class='fa fa-eye'></i></span><span class='hidden-xs'>View</span></a>
                                    <%} %>

                                    <%if (rights.CanDelete)
                                      { %>
                                    <a href="javascript:void(0);" onclick="fn_Deletecity('<%# Container.KeyValue %>')" title="" class="">
                                        <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>Delete</span></a>
                                    <%} %>
                                    <%if (rights.CanIndustry)
                                      { %>
                                    <a href="javascript:void(0);" onclick="OnAddBusinessClick('<%# (Convert.ToString( Eval("sProducts_Code"))).Replace("'",@"\'").Replace("\"","&quot") %>')" title="" class="" style="text-decoration: none;">
                                        <span class='ico ColorSeven'><i class='fa fa-user'></i></span><span class='hidden-xs'>Add Industry</span>
                                    </a>

                                    <%} %>


                                    <%--                                  <% if (rights.Imagaeupload)--%>

                                    <% if (rights.CanEdit)
                                       { %>

                                    <a href="javascript:void(0);" onclick="PopupOpentoProductUpload('<%#Eval("sProducts_ID")%>','<%# (Convert.ToString( Eval("sProducts_Code"))).Replace("'",@"\'").Replace("\"","&quot") %>')" title="" class="" style="text-decoration: none;">
                                        <span class='ico ColorFour'><i class='fa fa-upload'></i></span><span class='hidden-xs'>Image Upload</span>
                                    </a>

                                    <%} %>
                                    <%-- Rev Rajdip--%>
                                    <%if (rights.CanAdd)
                                      { %>
                                    <a href="javascript:void(0);" onclick="fn_CopyProducts('<%# Container.KeyValue %>')" title="" class="">
                                        <%--  <a href="javascript:void(0);" onclick="fn_Editcity(<%#Eval("sProducts_ID")%>)" title="" class="">--%>
                                        <span class='ico editColor'><i class='fa fa-files-o' aria-hidden='true'></i></span><span class='hidden-xs'>Copy</span></a>
                                    <%} %>
                                    <%--End Rev Rajdip--%>
                                    <a href="javascript:void(0);" onclick="OpenAvailableStock('<%#Eval("sProducts_ID")%>','<%# (Convert.ToString( Eval("sProducts_Code"))).Replace("'",@"\'").Replace("\"","&quot") %>')" title="" class="" style="text-decoration: none;">
                                        <span class='ico ColorFive'><i class='fa fa-user'></i></span><span class='hidden-xs'>Available Stock</span>
                                    </a>
                                </div>
                            </DataItemTemplate>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>

                    </Columns>
                    <%-- --Rev Sayantani--%>
                    <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" />
                    <%--  <SettingsCookies Enabled="true" StorePaging="true" StoreColumnsVisiblePosition="true"  Version="2.0"/>--%>
                    <%-- -- End of Rev Sayantani --%>
                    <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                    <SettingsPager PageSize="10">
                        <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100" />
                    </SettingsPager>
                    <SettingsBehavior ColumnResizeMode="NextColumn" />
                    <ClientSideEvents EndCallback="function (s, e) {grid_EndCallBack();}" RowClick="gridRowclick" />
                </dxe:ASPxGridView>
                <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
                    ContextTypeName="ERPDataClassesDataContext" TableName="ALLMASTERPAGELISTING" />
            </div>
            <div class="PopUpArea">

                <%--Packing Details popup--%>
                <dxe:ASPxPopupControl ID="ASPxPopupControl2" runat="server" ClientInstanceName="cpackingDetails"
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
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" />
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
                                            <%-- <MaskSettings Mask="<0..99999999>.<0000..99>" />--%>
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" />
                                            <ClientSideEvents LostFocus="SizeUOMChange" />
                                        </dxe:ASPxTextBox>
                                    </td>
                                    <td style="padding-right: 7px">
                                        <dxe:ASPxComboBox ID="cmbPackingUom" ClientInstanceName="ccmbPackingUom" runat="server" SelectedIndex="0"
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




                <%--Service Tax popup--%>
                <dxe:ASPxPopupControl ID="ASPxPopupControl1" runat="server" ClientInstanceName="cServiceTaxPopup"
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

                                <asp:Label ID="Label4" runat="server" Text="Service Category" CssClass="newLbl"></asp:Label>
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

                                    <asp:Label ID="Label15" runat="server" Text="TDS Section" CssClass="newLbl"></asp:Label>
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






                <%--taxCode popup--%>
                <dxe:ASPxPopupControl ID="TaxCodePopup" runat="server" ClientInstanceName="cTaxCodePopup"
                    Width="360px" HeaderText="Set Tax Codes" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
                    PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
                    Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
                    ContentStyle-CssClass="pad">
                    <HeaderTemplate>
                        <span>Set Tax Codes</span>
                        <dxe:ASPxImage ID="ASPxImage11" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader">
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

                                    <asp:Label ID="Label9" runat="server" Text="Select Tax Code Scheme -Sales" CssClass="newLbl"></asp:Label>
                                </div>
                                <div class="Left_Content">
                                    <dxe:ASPxComboBox ID="CmbTaxCodeSale" ClientInstanceName="cCmbTaxCodeSale" runat="server" SelectedIndex="0"
                                        ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True">
                                    </dxe:ASPxComboBox>
                                </div>
                            </div>
                            <div class="col-md-12">
                                <div class="cityDiv" style="height: auto;">

                                    <asp:Label ID="Label10" runat="server" Text="Select Tax Code Scheme -Purchases" CssClass="newLbl"></asp:Label>
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

                                        <asp:Label ID="Label11" runat="server" Text="Apply Auto Selection in Entries" CssClass="newLbl"></asp:Label>
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

                                        <asp:Label ID="Label12" runat="server" Text="Select Tax Scheme" CssClass="newLbl"></asp:Label>
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




                <%--Barcode popup--%>
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
                <%--Barcode popup End Here--%>
                <%--Product Attribute popup 
            added by debjyoti 04-01-2017--%>
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
                                        <dxe:ASPxComboBox ID="CmbProductColor" ClientInstanceName="cCmbProductColor" ClearButton-DisplayMode="Always" runat="server"
                                            ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0">
                                        </dxe:ASPxComboBox>
                                    </div>
                                </div>


                                <div class="col-md-6" style="margin-top: 25px">
                                    <div class="cityDiv" style="height: auto;">
                                    </div>
                                    <div class="Left_Content">

                                        <dxe:ASPxRadioButtonList ID="rdblappColor" ClientInstanceName="RrdblappColor" runat="server" RepeatDirection="Horizontal" Width="100%">
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
                                        <dxe:ASPxComboBox ID="CmbProductSize" ClientInstanceName="cCmbProductSize" ClearButton-DisplayMode="Always" runat="server"
                                            ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0">
                                        </dxe:ASPxComboBox>
                                    </div>
                                </div>
                                <div class="col-md-6" style="margin-top: 25px">
                                    <div class="cityDiv" style="height: auto;">
                                    </div>
                                    <div class="Left_Content">

                                        <dxe:ASPxRadioButtonList ID="rdblapp" ClientInstanceName="Rrdblapp" runat="server" RepeatDirection="Horizontal" Width="100%">
                                            <Items>
                                                <dxe:ListEditItem Text="Applicable" Value="1" Selected="true" />
                                                <dxe:ListEditItem Text="Not Applicable" Value="0" />

                                            </Items>
                                        </dxe:ASPxRadioButtonList>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="cityDiv" style="height: auto;">
                                        <asp:Label ID="Label27" runat="server" Text="Design No." CssClass="newLbl"></asp:Label>
                                    </div>
                                    <div class="Left_Content">
                                        <dxe:ASPxTextBox ID="txtDesignNo" ClientInstanceName="ctxtDesignNo" runat="server" Width="100%"></dxe:ASPxTextBox>
                                    </div>
                                </div>
                                <div class="col-md-6" style="margin-top: 0px">
                                    <div class="cityDiv" style="height: auto;">
                                        <asp:Label ID="Label28" runat="server" Text="Revision No." CssClass="newLbl"></asp:Label>
                                    </div>
                                    <div class="Left_Content">
                                        <dxe:ASPxTextBox ID="txtRevisionNo" ClientInstanceName="ctxtRevisionNo" runat="server" Width="100%"></dxe:ASPxTextBox>
                                    </div>
                                </div>

                                <%--Product Component--%>
                                <div class="clear"></div>
                                <%--  <div class="col-md-12" style="margin-top: 7px">Components</div>
                                <div class="clear"></div>--%>
                                <div class="col-md-6">
                                    <div class="cityDiv" style="height: auto;">
                                        <asp:Label ID="Label30" runat="server" Text="Components" CssClass="newLbl"></asp:Label>
                                        <div id="divProductMasterComponentMandatory" runat="server">
                                            (Mandatory tick/untick)
                                        <dxe:ASPxCheckBox runat="server" ID="chkIsMandatory" ClientInstanceName="cchkIsMandatory"></dxe:ASPxCheckBox>
                                        </div>
                                    </div>
                                    <div class="Left_Content">
                                        <dxe:ASPxCallbackPanel runat="server" ID="ComponentPanel" ClientInstanceName="cComponentPanel" OnCallback="Component_Callback">
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
                                <%--  <div id="divPosInstallation" runat="server">--%>
                                <%--Installation Required--%>
                                <div class="col-md-6" id="divPosInstallation" runat="server">
                                    <div class="cityDiv" style="height: auto;">
                                        <asp:Label ID="Label8" runat="server" Text="Installation Required" CssClass="newLbl"></asp:Label>
                                    </div>
                                    <div class="Left_Content">
                                        <dxe:ASPxComboBox ID="aspxInstallation" ClientInstanceName="caspxInstallation" runat="server"
                                            ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="1">
                                            <Items>
                                                <dxe:ListEditItem Text="Yes" Value="1" />
                                                <dxe:ListEditItem Text="No" Value="0" />
                                            </Items>
                                        </dxe:ASPxComboBox>
                                    </div>
                                </div>
                                <%--  </div>--%>
                                <div style="clear: both"></div>
                                <%--Brand --%>
                                <div class="col-md-6">
                                    <div class="cityDiv" style="height: auto;">

                                        <asp:Label ID="Label13" runat="server" Text="Brand" CssClass="newLbl"></asp:Label>
                                    </div>
                                    <div class="Left_Content">
                                        <dxe:ASPxComboBox ID="cmbBrand" ClientInstanceName="ccmbBrand" ClearButton-DisplayMode="Always" runat="server"
                                            ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="1">
                                        </dxe:ASPxComboBox>
                                    </div>
                                </div>
                                <div class="col-md-6" id="divPosOldUnit" runat="server">
                                    <div class="cityDiv" style="height: auto;">

                                        <asp:Label ID="Label16" runat="server" Text="Old Unit?" CssClass="newLbl"></asp:Label>
                                    </div>
                                    <div class="Left_Content">
                                        <dxe:ASPxComboBox ID="cmbOldUnit" ClientInstanceName="ccmbOldUnit" runat="server"
                                            ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="1">
                                            <Items>
                                                <dxe:ListEditItem Text="Yes" Value="1" />
                                                <dxe:ListEditItem Text="No" Value="0" />
                                            </Items>
                                        </dxe:ASPxComboBox>
                                    </div>
                                </div>
                                <div style="clear: both"></div>
                                <div class="col-md-6">
                                    <div class="cityDiv" style="height: auto;">
                                        <asp:Label ID="lblItemType" runat="server" Text="Item Type" CssClass="newLbl"></asp:Label>
                                    </div>
                                    <div class="Left_Content">
                                        <dxe:ASPxComboBox ID="cmbItemType" ClientInstanceName="ccmbItemType" runat="server"
                                            ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0">
                                            <Items>
                                                <dxe:ListEditItem Text="None" Value="None" />
                                                <dxe:ListEditItem Text="Consumable" Value="Consumable" />
                                                <dxe:ListEditItem Text="Sellable" Value="Sellable" />
                                                <dxe:ListEditItem Text="Both" Value="Both" />
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
                                                <%-- <dxe:ASPxTextBox ID="txtSeries" ClientInstanceName="ctxtSeries" runat="server"></dxe:ASPxTextBox>--%>

                                                <dxe:ASPxComboBox ID="ddlSeries" ClientInstanceName="cddlSeries" ClearButton-DisplayMode="Always" runat="server"
                                                    ValueType="System.String" Width="160px" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0">
                                                </dxe:ASPxComboBox>
                                            </div>
                                        </td>
                                        <td>
                                            <label>Surface</label>
                                            <div>
                                                <%--<dxe:ASPxTextBox ID="txtFinish" ClientInstanceName="ctxtFinish" runat="server"></dxe:ASPxTextBox>--%>
                                                <dxe:ASPxComboBox ID="ddlfinish" ClientInstanceName="cddlfinish" ClearButton-DisplayMode="Always" runat="server"
                                                    ValueType="System.String" Width="160px" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0">
                                                </dxe:ASPxComboBox>
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
                                                <%--   <dxe:ASPxTextBox ID="txtSubCat" MaxLength="100" ClientInstanceName="ctxtSubCat" DisplayFormatString="0.00" HorizontalAlign="Right" runat="server">
                                                    
                                                </dxe:ASPxTextBox>--%>
                                                <dxe:ASPxComboBox ID="ddlsubcat" ClientInstanceName="cddlsubcat" ClearButton-DisplayMode="Always" runat="server"
                                                    ValueType="System.String" Width="160px" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0">
                                                </dxe:ASPxComboBox>
                                            </div>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td>
                                            <label>Application</label>
                                            <div>
                                                <%--   <dxe:ASPxTextBox ID="txtSubCat" MaxLength="100" ClientInstanceName="ctxtSubCat" DisplayFormatString="0.00" HorizontalAlign="Right" runat="server">
                                                    
                                                </dxe:ASPxTextBox>--%>
                                                <dxe:ASPxComboBox ID="ddlproductapplication" ClientInstanceName="cddlproductapplication" ClearButton-DisplayMode="Always" runat="server"
                                                    ValueType="System.String" Width="160px" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0">
                                                </dxe:ASPxComboBox>
                                            </div>
                                        </td>
                                        <td>
                                            <label>Nature</label>
                                            <div>
                                                <%--   <dxe:ASPxTextBox ID="txtSubCat" MaxLength="100" ClientInstanceName="ctxtSubCat" DisplayFormatString="0.00" HorizontalAlign="Right" runat="server">
                                                    
                                                </dxe:ASPxTextBox>--%>
                                                <dxe:ASPxComboBox ID="ddlproductnature" ClientInstanceName="cddlproductnature" ClearButton-DisplayMode="Always" runat="server"
                                                    ValueType="System.String" Width="160px" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0">
                                                </dxe:ASPxComboBox>
                                            </div>
                                        </td>
                                        <td>
                                            <label>Dimension</label>
                                            <div>
                                                <dxe:ASPxTextBox ID="txtdimension" ClientInstanceName="ctxtdimension" HorizontalAlign="Right" Text="0" DisplayFormatString="0.00" runat="server" Width="100%">

                                                    <MaskSettings Mask="&lt;0..99999999999&gt;.&lt;00..99&gt;" />
                                                </dxe:ASPxTextBox>
                                            </div>
                                        </td>
                                        <td>
                                            <label>Pedestal No.</label>
                                            <div>
                                                <dxe:ASPxTextBox ID="txtpedestalno" ClientInstanceName="ctxtpedestalno" HorizontalAlign="Right" runat="server" Width="100%">

                                                    <%--<MaskSettings Mask="&lt;0..99999999999&gt;.&lt;00..99&gt;" />--%>
                                                </dxe:ASPxTextBox>
                                            </div>
                                        </td>
                                        <td>
                                            <label>Cat. No.</label>
                                            <div>
                                                <dxe:ASPxTextBox ID="txtcatno" ClientInstanceName="ctxtcatno" HorizontalAlign="Right" runat="server" Width="100%">

                                                    <%--    <MaskSettings Mask="&lt;0..99999999999&gt;.&lt;00..99&gt;" />--%>
                                                </dxe:ASPxTextBox>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <label>Warranty</label>
                                            <div>
                                                <dxe:ASPxTextBox ID="txtwarranty" ClientInstanceName="ctxtwarranty" HorizontalAlign="Right" runat="server" Width="100%">

                                                    <%-- <MaskSettings Mask="&lt;0..99999999999&gt;.&lt;00..99&gt;" />--%>
                                                </dxe:ASPxTextBox>
                                                <br />
                                                <br />
                                            </div>
                                        </td>

                                        <td>
                                            <label>Application Area</label>
                                            <div>
                                                <dxe:ASPxComboBox ID="cmbAppliArea" ClientInstanceName="CcmbAppliArea" runat="server"
                                                    ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0">
                                                    <Items>
                                                        <dxe:ListEditItem Text="Select" Value="0" />
                                                        <dxe:ListEditItem Text="EXTERIOR WALL" Value="1" />
                                                        <dxe:ListEditItem Text="EXTERIOR FLOOR" Value="2" />
                                                        <dxe:ListEditItem Text="STAIRS" Value="3" />
                                                        <dxe:ListEditItem Text="HIGHLIGHTER WALL" Value="4" />
                                                        <dxe:ListEditItem Text="PARKING TILES" Value="5" />
                                                        <dxe:ListEditItem Text="BEDROOM FLOOR" Value="6" />
                                                        <dxe:ListEditItem Text="BEDROOM WALL" Value="7" />
                                                        <dxe:ListEditItem Text="BATHROOM FLOOR" Value="8" />
                                                        <dxe:ListEditItem Text="BATHROOM WALL" Value="9" />
                                                        <dxe:ListEditItem Text="KID'S BEDROOM FLOOR" Value="10" />
                                                        <dxe:ListEditItem Text="KID'S BEDROOM WALL" Value="11" />
                                                        <dxe:ListEditItem Text="KID'S BATHROOM FLOOR" Value="12" />
                                                        <dxe:ListEditItem Text="KID'S BATHROOM WALL" Value="13" />
                                                        <dxe:ListEditItem Text="LIVING AREA FLOOR" Value="14" />
                                                        <dxe:ListEditItem Text="LIVING AREA WALL" Value="15" />
                                                        <dxe:ListEditItem Text="LIFT FACIA" Value="16" />
                                                        <dxe:ListEditItem Text="KITCHEN FLOOR" Value="17" />
                                                        <dxe:ListEditItem Text="KITCHEN WALL" Value="18" />
                                                        <dxe:ListEditItem Text="KITCHEN COUNTER TOP" Value="19" />

                                                    </Items>
                                                </dxe:ASPxComboBox>
                                                <br />
                                                <br />
                                            </div>
                                        </td>
                                        <td>
                                            <label>Movement</label>
                                            <div>
                                                <dxe:ASPxTextBox ID="txtMovement" ClientInstanceName="ctxtMovement" HorizontalAlign="Left" runat="server" Width="100%">

                                                    <%-- <MaskSettings Mask="&lt;0..99999999999&gt;.&lt;00..99&gt;" />--%>
                                                </dxe:ASPxTextBox>
                                                <br />
                                                <br />
                                            </div>
                                        </td>
                                    </tr>
                                </table>

                            </div>
                            <div style="clear: both"></div>
                            <div class="boxarea clearfix">
                                <%--<span class="boxareaH">Product Coverage Area</span>--%>
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
                                                <dxe:ASPxTextBox ID="txtWidth" ClientInstanceName="ctxtWidth" HorizontalAlign="Right" runat="server" Width="100%">
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
                            <%--</div>--%>
                        </dxe:PopupControlContentControl>
                    </ContentCollection>
                    <HeaderStyle BackColor="LightGray" ForeColor="Black" />
                    <ClientSideEvents Init="OnInitProductAttribute" />
                </dxe:ASPxPopupControl>

                <dxe:ASPxPopupControl ID="Popup_Empcitys" runat="server" ClientInstanceName="cPopup_Empcitys"
                    Width="1000px" HeaderText="Add/Modify products" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
                    PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
                    Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
                    ContentStyle-CssClass="pad">
                    <ClientSideEvents CloseUp="popupClose" />
                    <ContentCollection>
                        <dxe:PopupControlContentControl runat="server">
                            <%--<div style="Width:400px;background-color:#FFFFFF;margin:0px;border:1px solid red;">--%>
                            <div class="ProductMainContaint">

                                <div class="row ">
                                    <div class="col-md-12 ">
                                        <div class="col-md-6 " style="padding: 0">

                                            <div class="col-md-6">
                                                <div runat="server" id="dvShortName" style="margin-top: 5px">
                                                    <label class="cityDiv" style="height: auto; margin-bottom: 5px;">
                                                        <%--Code--%>
                                                Short Name (Unique)
                                               <%-- <asp:Label ID="LblCode" runat="server" Text="Short Name (Unique)" CssClass="newLbl"></asp:Label>--%><span style="color: red;"> *</span>

                                                    </label>
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
                                                <div class="row" id="">
                                                    <div class="col-md-6 lblmTop8" id="ddl_Num" runat="server" style="display: none">

                                                        <label>
                                                            <dxe:ASPxLabel ID="lbl_NumberingScheme" Width="130px" runat="server" Text="Numbering Scheme">
                                                            </dxe:ASPxLabel>
                                                        </label>
                                                        <asp:DropDownList ID="ddl_numberingScheme" runat="server" Width="100%">
                                                        </asp:DropDownList>


                                                    </div>

                                                    <div class="col-md-6 lblmTop8" runat="server" id="dvCustDocNo" style="display: none">
                                                        <label>
                                                            <dxe:ASPxLabel ID="lbl_CustDocNo" runat="server" Text="Unique ID" Width="">
                                                            </dxe:ASPxLabel>
                                                            <span style="color: red">*</span>
                                                        </label>

                                                        <dxe:ASPxTextBox ID="txt_CustDocNo" runat="server" ClientInstanceName="ctxt_CustDocNo" Width="100%" MaxLength="80">
                                                            <ClientSideEvents TextChanged="function(s, e) {UniqueCodeProductCheck();}" />
                                                        </dxe:ASPxTextBox>


                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-6 ">
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
                                                    <div class="col-md-6 ">
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

                                            </div>


                                            <%--chinmoy added for Auto numbering start--%>




                                            <%--end--%>


                                            <div class="col-md-6 lblmTop8">
                                                <label class="cityDiv" style="height: auto; margin-bottom: 5px;">
                                                    Name<span style="color: red;">*</span>
                                                    <%--<asp:Label ID="LblName" runat="server" Text="Name" CssClass="newLbl"></asp:Label>--%>
                                                </label>
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
                                            <div class="col-md-3 lblmTop8">
                                                <div class="cityDiv" style="height: auto;">
                                                    <%--Inventory Item--%>
                                                    <asp:Label ID="Label2" runat="server" Text="Inventory Item?" CssClass="newLbl"></asp:Label>
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
                                            <div class="col-md-3 lblmTop8">
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
                                                        <ClientSideEvents SelectedIndexChanged="ServiceItemChanged" />

                                                    </dxe:ASPxComboBox>
                                                </div>
                                            </div>


                                        </div>




                                        <div class="col-md-6 lblmTop8">
                                            <div class="cityDiv" style="height: auto;">
                                                <%--Description--%>
                                                <asp:Label ID="LblDecs" runat="server" Text="Description" CssClass="newLbl"></asp:Label>
                                            </div>
                                            <div class="Left_Content">
                                                <dxe:ASPxMemo ID="txtPro_Description" ClientInstanceName="ctxtPro_Description" MaxLength="5000"
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
                                                <%--   <dxe:ASPxButtonEdit ID="ProClassCode" runat="server" ReadOnly="true" ClientInstanceName="cProClassCode" Width="200px">
                                                    <Buttons>
                                                        <dxe:EditButton>
                                                        </dxe:EditButton>
                                                    </Buttons>
                                                    <ClientSideEvents ButtonClick="function(s,e){CustomerButnClick();}" KeyDown="function(s,e){Customer_KeyDown(s,e);}" />
                                          </dxe:ASPxButtonEdit>--%>

                                                <dxe:ASPxButtonEdit ID="ProClassCode" runat="server" ReadOnly="true" ClientInstanceName="cProClassCode" Width="100%">
                                                    <Buttons>
                                                        <dxe:EditButton>
                                                        </dxe:EditButton>
                                                    </Buttons>
                                                    <ClientSideEvents ButtonClick="CustomerButnClick" KeyDown="Customer_KeyDown" />
                                                </dxe:ASPxButtonEdit>

                                                <asp:HiddenField ID="ClassId" runat="server" />
                                            </div>
                                        </div>

                                        <div class="col-md-2">
                                            <div class="cityDiv" style="height: auto;">
                                                <%--Product Class Code--%>
                                                <asp:Label ID="Label1" runat="server" Text="Status" CssClass="newLbl"></asp:Label>
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
                                                <asp:Label ID="Label7" runat="server" Text="HSN Code" CssClass="newLbl"></asp:Label>
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
                                            <%--  Rev 2.0--%>
                                            <%--Start HSN Pdf link add Tanmoy 18-07-2019 --%>
<%--                                            <span style="color: #b11212;">Not sure about HSN code ? <a href="http://www.cbic.gov.in/resources//htdocs-cbec/gst/goods-rates-booklet-03July2017.pdf" target="_blank">Look up here</a></span>--%>
                                            <%--End HSN Pdf link add Tanmoy 18-07-2019 --%>
                                          
                                            <span style="color: #b11212;">Not sure about HSN code ? <a href="https://services.gst.gov.in/services/searchhsnsac" target="_blank">Look up here</a></span>
                                           <%-- Rev 2.0 End--%>
                                        </div>

                                        <div class="col-md-2">
                                            <div class="cityDiv" style="height: auto;">
                                                <asp:Label ID="Label21" runat="server" Text="Furtherance to Business" CssClass="newLbl"></asp:Label>
                                            </div>
                                            <div class="Left_Content">
                                                <dxe:ASPxCheckBox ID="chkFurtherance" ClientInstanceName="cchkFurtherance" runat="server">
                                                </dxe:ASPxCheckBox>
                                            </div>
                                        </div>
                                        <%-- REV RAJDIP --%>
                                        <div class="col-md-2">
                                            <div class="cityDiv" style="height: auto;">
                                                <asp:Label ID="Label25" runat="server" Text="Consider In Stock Valuation" CssClass="newLbl"></asp:Label>
                                            </div>
                                            <div class="Left_Content">
                                                <dxe:ASPxCheckBox ID="chkConseiderInstockval" ClientInstanceName="cchkConseiderInstockval" runat="server">
                                                </dxe:ASPxCheckBox>
                                            </div>
                                        </div>
                                        <%-- END REV RAJDIP --%>
                                    </div>




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

                                            <div class="col-md-2">
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

                                            <div class="col-md-2">
                                                <div class="cityDiv" style="height: auto;">
                                                    <span class="newLbl pull-right">MRP </span>
                                                </div>
                                                <div class="Left_Content">
                                                    <dxe:ASPxTextBox ID="txtMrp" ClientInstanceName="ctxtMrp" HorizontalAlign="Right" MaxLength="18" DisplayFormatString="{0:0.00}"
                                                        runat="server" Width="94%">
                                                        <MaskSettings Mask="<0..99999999>.<0..99>" />
                                                        <%-- <ClientSideEvents KeyPress="function(s,e){ fn_AllowonlyNumeric(s,e);}" />--%>
                                                    </dxe:ASPxTextBox>
                                                    <%--<span id="valid" style="position: absolute;right: 11px;top: 30px;color: red;">*</span>--%>
                                                    <asp:Label runat="server" ID="valid" Style="position: absolute; right: 11px; top: 30px; color: red;"></asp:Label>
                                                    <span id="mrpError" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none; right: -2px; top: 27px;" title="Must be greater than Min Sale Price"></span>

                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="cityDiv" style="height: auto;">
                                                    <span class="newLbl pull-right">Cost @</span>
                                                </div>
                                                <div class="Left_Content">
                                                    <dxe:ASPxTextBox ID="txtCostPrice" ClientInstanceName="ctxtCostPrice" HorizontalAlign="Right" MaxLength="18" DisplayFormatString="{0:0.00}"
                                                        runat="server" Width="100%">
                                                        <MaskSettings Mask="<0..99999999>.<0..99>" />
                                                        <ClientSideEvents KeyPress="function(s,e){ fn_AllowonlyNumeric(s,e);}" />
                                                    </dxe:ASPxTextBox>
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



                                            <%--Bapi Preffered Vendors--%>

                                            <div class="col-md-3" id="divVendors" runat="server">
                                                <label>
                                                    <dxe:ASPxLabel ID="ASPxLabel1" Style="color: #b5285f;" runat="server" Text="Preferred Vendors">
                                                    </dxe:ASPxLabel>

                                                </label>
                                                <div>
                                                    <dxe:ASPxButtonEdit ID="btnVendors" runat="server" ReadOnly="true" ClientInstanceName="cbtnVendors" Width="100%" TabIndex="5">
                                                        <Buttons>
                                                            <dxe:EditButton>
                                                            </dxe:EditButton>
                                                        </Buttons>
                                                        <ClientSideEvents ButtonClick="function(s,e){VendorButnClick();}" KeyDown="function(s,e){Emp_KeyDown(s,e);}" />
                                                    </dxe:ASPxButtonEdit>

                                                </div>
                                            </div>
                                            <%--rev srijeeta--%>

                                            <div class="col-md-3">
                                                <div class="cityDiv" style="height: auto;">
                                                    <span class="newLbl pull-right">Package Quantity</span>
                                                </div>
                                                <div class="Left_Content">
                                                    <dxe:ASPxTextBox ID="txtpackageqty" ClientInstanceName="ctxtpackageqty" HorizontalAlign="Right" MaxLength="18" DisplayFormatString="{0:0.00}"
                                                        runat="server" Width="100%">
                                                        <MaskSettings Mask="<0..99999999>.<0..99>" />
                                                        <ClientSideEvents KeyPress="function(s,e){ fn_AllowonlyNumeric(s,e);}" />
                                                    </dxe:ASPxTextBox>
                                                </div>
                                            </div>
                                            <%--end of rev srijeeta--%>

                                            <!--Vendor Modal -->
                                            <div class="modal fade" id="EmpModel" role="dialog">
                                                <div class="modal-dialog">
                                                    <!-- Modal content-->
                                                    <div class="modal-content">
                                                        <div class="modal-header">
                                                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                                                            <h4 class="modal-title">Vendor Search</h4>
                                                        </div>
                                                        <div class="modal-body">
                                                            <input type="text" onkeydown="Empkeydown(event)" id="txtEmpSearch" width="100%" placeholder="Search By Vendor Name" />
                                                            <div id="EmpTable">
                                                                <table border='1' width="100%">

                                                                    <tr class="HeaderStyle" style="font-size: small">
                                                                        <th>Select</th>
                                                                        <th class="hide">id</th>
                                                                        <th>Vendor Name</th>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </div>
                                                        <div class="modal-footer">
                                                            <button type="button" class="btnOkformultiselection btn-default" onclick="DeSelectAll('EmpSource')">Deselect All</button>
                                                            <button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal" onclick="OKPopup('EmpSource')">OK</button>
                                                            <%--<button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal">Close</button>--%>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <!--Vendor Modal -->


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
                                                    <asp:Label ID="Label5" runat="server" Text="Reorder Level" CssClass="newLbl pull-right"></asp:Label>
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
                                                    <asp:Label ID="Label6" runat="server" Text="Negative Stock" CssClass="newLbl"></asp:Label>
                                                </div>
                                                <div class="Left_Content">
                                                    <dxe:ASPxComboBox ID="cmbNegativeStk" ClientInstanceName="ccmbNegativeStk" runat="server"
                                                        ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="2">
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
                                                    <asp:Label ID="LblType" runat="server" Text="Type" CssClass="newLbl"></asp:Label>
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
                                                    <asp:Label ID="Label3" runat="server" Text="Stock Valuation Tech." CssClass="newLbl"></asp:Label>
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
                                                    <asp:Label ID="Label17" runat="server" Text="Sales" CssClass="newLbl"></asp:Label>
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
                                                    <asp:Label ID="Label18" runat="server" Text="Sales Return" CssClass="newLbl"></asp:Label>
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
                                                    <asp:Label ID="Label19" runat="server" Text="Purchase" CssClass="newLbl"></asp:Label>
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
                                                    <asp:Label ID="Label20" runat="server" Text="Purchase Return" CssClass="newLbl"></asp:Label>
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

                                        <div class="boxarea clearfix">
                                            <div class="col-md-3" id="DivServiceComponent" runat="server" style="margin: 15px">

                                                <div class="">
                                                    <div class="cityDiv hide" style="height: auto;">
                                                        <asp:Label ID="Label26" runat="server" Text="&nbsp;" CssClass="newLbl"></asp:Label>
                                                    </div>
                                                    <div class="Left_Content">
                                                        <dxe:ASPxCheckBox ID="chkComponentService" ClientInstanceName="cchkComponentService" runat="server">
                                                        </dxe:ASPxCheckBox>
                                                        <span>Component for service</span>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class=" col-md-3" id="DivServiceModel" runat="server" style="">
                                                <div class="">
                                                    <div class="cityDiv" style="height: auto;">
                                                        <asp:Label ID="lblServiceModel" runat="server" Text="Model(s)" CssClass="newLbl"></asp:Label>
                                                    </div>
                                                    <div class="Left_Content">
                                                        <dxe:ASPxCallbackPanel runat="server" ID="ModelPanel" ClientInstanceName="cModelPanel" OnCallback="Model_Callback">
                                                            <PanelCollection>
                                                                <dxe:PanelContent runat="server">
                                                                    <dxe:ASPxGridLookup ID="lookup_Model" SelectionMode="Multiple" runat="server" ClientInstanceName="gridModelLookup"
                                                                        OnDataBinding="lookup_Model_DataBinding"
                                                                        KeyFieldName="ModelID" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                                                        <Columns>
                                                                            <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                                                            <dxe:GridViewDataColumn FieldName="ModelDesc" Visible="true" VisibleIndex="1" Width="200px" Caption="Model(s)" Settings-AutoFilterCondition="Contains">
                                                                                <Settings AutoFilterCondition="Contains" />
                                                                            </dxe:GridViewDataColumn>
                                                                        </Columns>
                                                                        <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                                                            <Templates>
                                                                                <StatusBar>
                                                                                    <table class="OptionsTable" style="float: right">
                                                                                        <tr>
                                                                                            <td>
                                                                                                <%--<div class="hide">--%>
                                                                                                <dxe:ASPxButton ID="ASPxButton3" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAllModel" UseSubmitBehavior="False" />
                                                                                                <%--</div>--%>
                                                                                                <dxe:ASPxButton ID="ASPxButton4" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAllModel" UseSubmitBehavior="False" />
                                                                                                <dxe:ASPxButton ID="ASPxButton5" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseModelLookup" UseSubmitBehavior="False" />
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </StatusBar>
                                                                            </Templates>
                                                                            <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" />
                                                                            <SettingsPager Mode="ShowPager">
                                                                            </SettingsPager>

                                                                            <SettingsPager PageSize="20">
                                                                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,20,50,100,150,200" />
                                                                            </SettingsPager>

                                                                            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                                                        </GridViewProperties>

                                                                    </dxe:ASPxGridLookup>
                                                                </dxe:PanelContent>
                                                            </PanelCollection>
                                                        </dxe:ASPxCallbackPanel>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-3" id="DivReplaceable" runat="server">
                                                <div class="cityDiv" style="height: auto;">
                                                    <asp:Label ID="Label29" runat="server" Text="Replaceable" CssClass="newLbl"></asp:Label>
                                                </div>
                                                <div>
                                                    <dxe:ASPxComboBox ID="cmbReplaceable" ClientInstanceName="ccmbReplaceable" runat="server"
                                                        ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0">
                                                        <Items>
                                                            <dxe:ListEditItem Text="No" Value="0" />
                                                            <dxe:ListEditItem Text="Yes" Value="1" />
                                                        </Items>
                                                    </dxe:ASPxComboBox>
                                                </div>
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


                                        <dxe:ASPxButton ID="btnSave_citys" CausesValidation="true" ClientInstanceName="cbtnSave_citys" runat="server" ValidationGroup="product" EncodeHtml="false"
                                            AutoPostBack="False" Text="<u>S</u>ave" CssClass="btn btn-primary">
                                            <ClientSideEvents Click="function (s, e) {btnSave_citys();}" />
                                        </dxe:ASPxButton>
                                        <%-- Rev Rajdip For Copy --%>
                                        <dxe:ASPxButton ID="btnSave_CopyValues" CausesValidation="true" ClientInstanceName="cbtnSave_CopyValues" runat="server" ValidationGroup="product" EncodeHtml="false"
                                            AutoPostBack="False" Text="<u>S</u>ave Copy" CssClass="btn btn-primary">
                                            <ClientSideEvents Click="function (s, e) {btnSave_CopytoProducts();}" />
                                        </dxe:ASPxButton>

                                        <%-- End Rev For Rajdip --%>
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






                <dxe:ASPxPopupControl ID="Popup_Empcitys_active" runat="server" ClientInstanceName="cPopup_Empcitys_active"
                    Width="700px" HeaderText="Add/Modify products" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
                    PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
                    Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
                    ContentStyle-CssClass="pad">
                    <ContentCollection>
                        <dxe:PopupControlContentControl runat="server">
                            <%--<div style="Width:400px;background-color:#FFFFFF;margin:0px;border:1px solid red;">--%>
                            <div class="ProductMainContaint">

                                <div class="col-md-12" style="padding: 0px !important;">

                                    <div class="boxarea clearfix">
                                        <span class="boxareaH">Rate</span>



                                        <%--Debjyoti Sale price & min sale price--%>

                                        <div class="col-md-3">
                                            <div class="cityDiv" style="height: auto;">
                                                <span class="newLbl pull-right">Sell @</span>
                                            </div>
                                            <div class="Left_Content">
                                                <dxe:ASPxTextBox ID="txtActiveSalesPrice" ClientInstanceName="ctxtActiveSalesPrice" HorizontalAlign="Right" MaxLength="18" DisplayFormatString="{0:0.00}"
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
                                                <dxe:ASPxTextBox ID="txtActiveMinSalesPrice" ClientInstanceName="ctxtActiveMinSalesPrice" HorizontalAlign="Right" MaxLength="18" DisplayFormatString="{0:0.00}"
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
                                                <dxe:ASPxTextBox ID="txtActiveMRPSalesPrice" ClientInstanceName="ctxtActiveMRPSalesPrice" HorizontalAlign="Right" MaxLength="18" DisplayFormatString="{0:0.00}"
                                                    runat="server" Width="100%">
                                                    <MaskSettings Mask="<0..99999999>.<0..99>" />
                                                    <%-- <ClientSideEvents KeyPress="function(s,e){ fn_AllowonlyNumeric(s,e);}" />--%>
                                                </dxe:ASPxTextBox>
                                                <span id="mrpErrors" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; right: -2px; top: 27px; display: none" title="Must be greater than Min Sale Price"></span>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="cityDiv" style="height: auto;">
                                                <span class="newLbl pull-right">Buy @</span>
                                            </div>
                                            <div class="Left_Content">
                                                <dxe:ASPxTextBox ID="txtActivePurPrice" ClientInstanceName="ctxtActivePurPrice" HorizontalAlign="Right" MaxLength="18" DisplayFormatString="{0:0.00}"
                                                    runat="server" Width="100%">
                                                    <MaskSettings Mask="<0..99999999>.<0..99>" />
                                                    <ClientSideEvents KeyPress="function(s,e){ fn_AllowonlyNumeric(s,e);}" />
                                                </dxe:ASPxTextBox>
                                            </div>
                                        </div>
                                        <%--rev srijeeta --%>>
                                        <div class="col-md-3">
                                            <div class="cityDiv" style="height: auto;">
                                                <span class="newLbl pull-right">Package Quantity</span>
                                            </div>
                                            <div class="Left_Content">
                                                <dxe:ASPxTextBox ID="txtactivepackageqty" ClientInstanceName="ctxtactivepackageqty" HorizontalAlign="Right" MaxLength="18" DisplayFormatString="{0:0.00}"
                                                    runat="server" Width="100%">
                                                    <MaskSettings Mask="<0..99999999>.<0..99>" />
                                                    <ClientSideEvents KeyPress="function(s,e){ fn_AllowonlyNumeric(s,e);}" />
                                                </dxe:ASPxTextBox>
                                            </div>
                                        </div>
                                        <%-- end of rev srijeeta--%>>
                                        
                                        <div class="col-md-3">
                                            <div class="cityDiv" style="height: auto;">
                                                <span class="newLbl pull-right">Cost @</span>
                                            </div>
                                            <div class="Left_Content">
                                                <dxe:ASPxTextBox ID="txtActiveCostPrice" ClientInstanceName="ctxtActiveCostPrice" HorizontalAlign="Right" MaxLength="18" DisplayFormatString="{0:0.00}"
                                                    runat="server" Width="100%">
                                                    <MaskSettings Mask="<0..99999999>.<0..99>" />
                                                    <ClientSideEvents KeyPress="function(s,e){ fn_AllowonlyNumeric(s,e);}" />
                                                </dxe:ASPxTextBox>
                                            </div>
                                        </div>
                                    </div>

                                </div>

                                <div class="">

                                    <div class="clear"></div>
                                    <div class="col-md-12" style="padding: 0px !important;">
                                        <div class="boxarea clearfix">
                                            <span class="boxareaH">Inventory</span>
                                            <div class="col-md-3">
                                                <div class="cityDiv" style="height: auto;">
                                                    <span class="newLbl">Min Level        </span>
                                                </div>
                                                <div class="Left_Content">
                                                    <dxe:ASPxTextBox ID="ASPxTextBox11" ClientInstanceName="ctxtMinLvl_active" MaxLength="18" DisplayFormatString="{0:0.00}"
                                                        runat="server" Width="100%">
                                                        <MaskSettings Mask="<0..99999999>.<0..99>" />
                                                        <ClientSideEvents KeyPress="function(s,e){ fn_AllowonlyNumeric(s,e);}" />
                                                    </dxe:ASPxTextBox>
                                                </div>
                                            </div>




                                            <div class="col-md-3">
                                                <div class="cityDiv" style="height: auto;">
                                                    <span class="newLbl">Max Level        </span>
                                                </div>
                                                <div class="Left_Content">
                                                    <dxe:ASPxTextBox ID="ASPxTextBox12" ClientInstanceName="ctxtMaxLvl_active" MaxLength="18" DisplayFormatString="{0:0.00}"
                                                        runat="server" Width="100%">
                                                        <MaskSettings Mask="<0..99999999>.<0..99>" />
                                                        <ClientSideEvents KeyPress="function(s,e){ fn_AllowonlyNumeric(s,e);}" />
                                                    </dxe:ASPxTextBox>
                                                </div>
                                            </div>





                                            <div class="col-md-3">
                                                <div class="cityDiv" style="height: auto;">
                                                    <asp:Label ID="Label33" runat="server" Text="Reorder Level" CssClass="newLbl"></asp:Label>
                                                </div>
                                                <div class="Left_Content">
                                                    <dxe:ASPxTextBox ID="ASPxTextBox13" ClientInstanceName="ctxtReorderLvl_active" MaxLength="18" DisplayFormatString="{0:0.00}"
                                                        runat="server" Width="100%">
                                                        <ClientSideEvents KeyPress="function(s,e){ fn_AllowonlyNumeric(s,e);}" />
                                                        <%--<MaskSettings Mask="&lt;0..9999999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />--%>
                                                        <%--<MaskSettings Mask="&lt;0..9999999999g&gt;.&lt;00..99&gt;" />--%>
                                                        <MaskSettings Mask="<0..99999999>.<0..99>" />

                                                    </dxe:ASPxTextBox>
                                                    <span id="reOrderError1" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; right: -6px; top: 29px; display: none" title="Must be greater than Min level"></span>
                                                </div>
                                            </div>

                                            <div class="col-md-3">
                                                <div class="cityDiv" style="height: auto;">
                                                    <asp:Label ID="Label34" runat="server" Text="Reorder Quantity" CssClass="newLbl"></asp:Label>
                                                </div>
                                                <div class="Left_Content">
                                                    <dxe:ASPxTextBox ID="ASPxTextBox14" ClientInstanceName="ctxtReorderQty_active" MaxLength="18" DisplayFormatString="{0:0.00}"
                                                        runat="server" Width="100%">
                                                        <ClientSideEvents KeyPress="function(s,e){ fn_AllowonlyNumeric(s,e);}" />
                                                        <%--<MaskSettings Mask="&lt;0..9999999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />--%>
                                                        <MaskSettings Mask="&lt;0..9999999999g&gt;.&lt;00..99&gt;" />
                                                    </dxe:ASPxTextBox>
                                                    <span id="reOrderQuantityError1" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; right: -6px; top: 29px; display: none" title="Must be greater than Min level"></span>
                                                </div>
                                            </div>

                                        </div>
                                        <%--End here--%>
                                    </div>
                                    <div class="col-md-12" style="padding: 0px !important;">

                                        <%-- <div style="clear: both"></div>--%>
                                        <div class="boxarea clearfix">
                                            <span class="boxareaH">Ledger Mapping</span>
                                            <div class="col-md-3">
                                                <div class="cityDiv" style="height: auto;">
                                                    <asp:Label ID="Label38" runat="server" Text="Sales" CssClass="newLbl"></asp:Label>
                                                </div>
                                                <div class="Left_Content">

                                                    <dxe:ASPxButtonEdit ID="ASPxButtonEdit1" runat="server" ReadOnly="true" ClientInstanceName="cSIMainAccount_active" Width="100%">
                                                        <Buttons>
                                                            <dxe:EditButton>
                                                            </dxe:EditButton>
                                                        </Buttons>
                                                        <ClientSideEvents ButtonClick="MainAccountButnClick" KeyDown="MainAccountKeyDown" />
                                                    </dxe:ASPxButtonEdit>

                                                </div>
                                            </div>

                                            <div class="col-md-3">
                                                <div class="cityDiv" style="height: auto;">
                                                    <asp:Label ID="Label39" runat="server" Text="Sales Return" CssClass="newLbl"></asp:Label>
                                                </div>
                                                <dxe:ASPxButtonEdit ID="ASPxButtonEdit2" runat="server" ReadOnly="true" ClientInstanceName="cSRMainAccount_active" Width="100%">
                                                    <Buttons>
                                                        <dxe:EditButton>
                                                        </dxe:EditButton>
                                                    </Buttons>
                                                    <ClientSideEvents ButtonClick="SRMainAccountButnClick" KeyDown="MainAccountKeyDown" />
                                                </dxe:ASPxButtonEdit>
                                            </div>

                                            <div class="col-md-3">
                                                <div class="cityDiv" style="height: auto;">
                                                    <asp:Label ID="Label40" runat="server" Text="Purchase" CssClass="newLbl"></asp:Label>
                                                </div>
                                                <dxe:ASPxButtonEdit ID="ASPxButtonEdit3" runat="server" ReadOnly="true" ClientInstanceName="cPIMainAccount_active" Width="100%">
                                                    <Buttons>
                                                        <dxe:EditButton>
                                                        </dxe:EditButton>
                                                    </Buttons>
                                                    <ClientSideEvents ButtonClick="PIMainAccountButnClick" KeyDown="MainAccountKeyDown" />
                                                </dxe:ASPxButtonEdit>
                                            </div>

                                            <div class="col-md-3">
                                                <div class="cityDiv" style="height: auto;">
                                                    <asp:Label ID="Label41" runat="server" Text="Purchase Return" CssClass="newLbl"></asp:Label>
                                                </div>
                                                <dxe:ASPxButtonEdit ID="ASPxButtonEdit4" runat="server" ReadOnly="true" ClientInstanceName="cPRMainAccount_active" Width="100%">
                                                    <Buttons>
                                                        <dxe:EditButton>
                                                        </dxe:EditButton>
                                                    </Buttons>
                                                    <ClientSideEvents ButtonClick="PRMainAccountButnClick" KeyDown="MainAccountKeyDown" />
                                                </dxe:ASPxButtonEdit>
                                            </div>

                                        </div>
                                    </div>


                                    <div class="clear"></div>
                                    <div class=" clearfix" style="padding-left: 16px">
                                        <dxe:ASPxButton ID="ASPxButton1" CausesValidation="true" ClientInstanceName="cbtnSave_citys_active" runat="server" ValidationGroup="product" EncodeHtml="false"
                                            AutoPostBack="False" Text="<u>S</u>ave" CssClass="btn btn-primary">
                                            <ClientSideEvents Click="function (s, e) {SaveActiveDormant();}" />
                                        </dxe:ASPxButton>
                                        <dxe:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="False" Text="<u>C</u>ancel" CssClass="btn btn-danger" EncodeHtml="false">
                                            <ClientSideEvents Click="function (s, e) {fn_btnCancel_active();}" />
                                        </dxe:ASPxButton>

                                        <br style="clear: both;" />
                                    </div>
                                    <div class="clear"></div>
                                </div>
                        </dxe:PopupControlContentControl>
                    </ContentCollection>
                    <ContentStyle VerticalAlign="Top" CssClass="pad"></ContentStyle>

                    <HeaderStyle BackColor="LightGray" ForeColor="Black" />

                </dxe:ASPxPopupControl>









                <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="true" PaperKind="A3" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
                </dxe:ASPxGridViewExporter>
            </div>
            <div class="HiddenFieldArea" style="display: none;">
                <asp:HiddenField runat="server" ID="hiddenedit" />
            </div>
        </div>
    </div>
    </div>
        <dxe:ASPxPopupControl ID="AssignValuePopup" runat="server" ClientInstanceName="AssignValuePopup"
            Width="200px" HeaderText="Add / Edit Key Value" PopupHorizontalAlign="WindowCenter"
            BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
            ContentStyle-CssClass="pad">
            <ContentStyle VerticalAlign="Top" CssClass="pad">
            </ContentStyle>
            <ContentCollection>
                <dxe:PopupControlContentControl ID="AssignValuePopupContent" runat="server">
                    <div id="generatedForm">
                    </div>
                    <div id="SubmitFrm">

                        <asp:TextBox ID="KeyField" runat="server" Style="display: none;"></asp:TextBox>
                        <asp:TextBox ID="ValueField" runat="server" Style="display: none;"></asp:TextBox>
                        <asp:TextBox ID="RexPageName" runat="server" Style="display: none;"></asp:TextBox>


                        <asp:Button ID="Button1" runat="server" Text="Save" CssClass="btn btn-primary" OnClientClick="return SaveDataToResource()" OnClick="BTNSave_clicked" Style="margin-left: 155px;" />

                    </div>
                </dxe:PopupControlContentControl>
            </ContentCollection>
            <HeaderStyle BackColor="LightGray" ForeColor="Black" />
        </dxe:ASPxPopupControl>

        <asp:SqlDataSource ID="ProductDataSource" runat="server"
            SelectCommand="select sProducts_ID,sProducts_Code,sProducts_Name from Master_sProducts"></asp:SqlDataSource>
        <asp:SqlDataSource ID="tdstcs" runat="server"
            SelectCommand="Select  TDSTCS_ID,ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' as tdsdescription ,ltrim(rtrim(tdstcs_code)) tdscode  from master_tdstcs "></asp:SqlDataSource>


        <dxe:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" ContainerElementID="ProductMainContaint"
            Modal="True">
        </dxe:ASPxLoadingPanel>


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
                        <input type="text" onkeydown="Customerkeydown(event)" id="txtClassNameSearch" autofocus width="100%" placeholder="Search by Class Name" />

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
        <%-- <asp:HiddenField runat="server" ID="classId" />--%>

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


        <dxe:ASPxPopupControl runat="server" ID="MainAccountModelSI" ClientInstanceName="cMainAccountModelSI"
            Width="500px" Height="300px" HeaderText="Search Main Account" PopupHorizontalAlign="WindowCenter"
            BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
            ContentStyle-CssClass="pad">

            <HeaderTemplate>
                <span>Search Main Account</span>
                <dxe:ASPxImage ID="ASPxImage12" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader">
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
                <dxe:ASPxImage ID="ASPxImage13" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader">
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
                <dxe:ASPxImage ID="ASPxImage14" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader">
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

        <dxe:ASPxPopupControl runat="server" ID="MainAccountModelPR" ClientInstanceName="cMainAccountModelPR"
            Width="500px" Height="300px" HeaderText="Search Main Account" PopupHorizontalAlign="WindowCenter"
            BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
            ContentStyle-CssClass="pad">

            <HeaderTemplate>
                <span>Search Main Account</span>
                <dxe:ASPxImage ID="ASPxImage15" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader">
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
        <%--Rev 1.0 Subhra 01-04-2019--%>
        <div>
            <asp:HiddenField runat="server" ID="hdnConvertionOverideVisible" />
            <asp:HiddenField runat="server" ID="hdnShowUOMConversionInEntry" />
            <asp:HiddenField runat="server" ID="hdnAltNameMandatory" />
            <asp:HiddenField runat="server" ID="hdnUOMNoninventory" />
            <asp:HiddenField runat="server" ID="hdnPackagingQtyZeroProductMaster" />
            <asp:HiddenField runat="server" ID="hdnProductTypeMandatory" />
            <asp:HiddenField runat="server" ID="hdnUOMConverMandatoryProductMaster" />
        </div>
        <%--End of Rev 1.0 Subhra 01-04-2019--%>

        <dxe:ASPxPopupControl ID="AspxDirectCustomerViewPopup" runat="server"
            CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="CAspxDirectCustomerViewPopup" Height="650px"
            Width="40px" HeaderText="View Product" Modal="true" AllowResize="False">

            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                </dxe:PopupControlContentControl>
            </ContentCollection>

        </dxe:ASPxPopupControl>

        <!--Class Modal -->
        <div class="modal fade" id="CustModel" role="dialog" style="z-index: 999">
            <div class="modal-dialog">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">Class Search</h4>
                    </div>
                    <div class="modal-body">
                        <input type="text" onkeydown="Customerkeydown(event)" id="txtCustSearch" autofocus width="100%" placeholder="Search By Class Name" />
                        <div id="CustomerTable">
                            <table border='1' width="100%" class="dynamicPopupTbl">
                                <tr class="HeaderStyle">
                                    <th class="hide">Id</th>
                                    <th>Class Name</th>

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
        <!--class Modal -->
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
        <%--Rev work start 10.06.2022 Mantise issue:0024783: Customer & Product master import required for ERP--%>
        <div class="modal fade" id="modalimport" role="dialog">
        <div class="modal-dialog VerySmall">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Select File to Import (Add/Update)</h4>
                </div>
                <div class="modal-body">

                    <div class="col-md-12">
                        <div id="divproduct">

                            <div>
                                <asp:FileUpload ID="OFDBankSelect" accept=".xls,.xlsx" runat="server" Width="100%" />
                                <div class="pTop10  mTop5">
                                    <asp:Button ID="BtnSaveexcel" runat="server" Text="Import(Add/Update)"  OnClick="BtnSaveexcel_Click1" CssClass="btn btn-primary" />
                                </div>
                            </div>
                        </div>

                    </div>
                </div>

            </div>
        </div>
    </div>
    <div class="modal fade" id="modalSS" role="dialog">
        <div class="modal-dialog fullWidth">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Product Log</h4>
                </div>
                <div class="modal-body">

                    <dxe:ASPxGridView ID="GvJvSearch" runat="server" AutoGenerateColumns="False" SettingsBehavior-AllowSort="true"
                        ClientInstanceName="cGvJvSearch" KeyFieldName="CustLogId" Width="100%" OnDataBinding="GvJvSearch_DataBinding" Settings-VerticalScrollBarMode="Auto" Settings-VerticalScrollableHeight="400">

                        <SettingsBehavior ConfirmDelete="false" ColumnResizeMode="NextColumn" />
                        <Styles>
                            <Header SortingImageSpacing="5px" ImageSpacing="5px"></Header>
                            <FocusedRow HorizontalAlign="Left" VerticalAlign="Top" CssClass="gridselectrow"></FocusedRow>
                            <LoadingPanel ImageSpacing="10px"></LoadingPanel>
                            <FocusedGroupRow CssClass="gridselectrow"></FocusedGroupRow>
                            <Footer CssClass="gridfooter"></Footer>
                        </Styles>
                        <Columns>
                            <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="0" FieldName="ProdLogId" Caption="LogID" SortOrder="Descending">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="CreatedDatetime" Caption="Date" Width="10%">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                                <PropertiesTextEdit DisplayFormatString="dd/MM/yyyy"></PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="ProductCode" Caption="Product Code" Width="10%">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                                <PropertiesTextEdit DisplayFormatString="dd/MM/yyyy"></PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="LoopNumber" Caption="Row Number" Width="13%">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="ProdName" Width="8%" Caption="Product Name">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="FileName" Width="14%" Caption="File Name">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                                <PropertiesTextEdit DisplayFormatString="dd/MM/yyyy"></PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="Description" Caption="Description" Width="10%" Settings-AllowAutoFilter="False">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="Status" Caption="Status" Width="14%" Settings-AllowAutoFilter="False">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                        <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />
                        <SettingsSearchPanel Visible="false" />
                        <SettingsPager NumericButtonCount="200" PageSize="200" ShowSeparators="True" Mode="ShowPager">
                            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="200,400,600" />
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </SettingsPager>
                    </dxe:ASPxGridView>
                </div>
            </div>
        </div>
    </div>
    <%--Rev work close 10.06.2022 Mantise issue:0024783: Customer & Product master import required for ERP--%>
        <asp:HiddenField runat="server" ID="HDSelectedProduct" />
        <asp:HiddenField ID="hdnAutoNumStg" runat="server" />
        <asp:HiddenField ID="hddnDocNo" runat="server" />
        <asp:HiddenField ID="hdnTransactionType" runat="server" />
        <asp:HiddenField ID="hdnNumberingId" runat="server" />
        <asp:HiddenField ID="hdVendorsID" runat="server" />
        <div runat="server" id="jsonProducts" class="hide"></div>
</asp:Content>
