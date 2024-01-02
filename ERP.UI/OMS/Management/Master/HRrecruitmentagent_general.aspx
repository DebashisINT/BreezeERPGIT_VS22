<%--================================================== Revision History =============================================
Rev Number  DATE              VERSION       DEVELOPER           CHANGES
1.0         15-03-2023        2.0.36        Pallab              25733 : Master pages design modification
2.0         02-01-2024       V2.0.42        Sanchita            Settings required to Check Duplicate Vendor Master Name. Mantis: 27126
====================================================== Revision History =============================================--%>

<%@ Page Title="Vendors/Service Providers" Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.Master.management_master_HRrecruitmentagent_general" CodeBehind="HRrecruitmentagent_general.aspx.cs" EnableEventValidation="false" %>

<%@ Register Src="~/OMS/Management/Master/UserControls/GSTINSettings.ascx" TagPrefix="GSTIN" TagName="gstinSettings" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/assests/pluggins/choosen/choosen.min.js"></script>

    <script>
        $(document).ready(function () {

            //chinmoy add for numbering scheme start

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

                    $('#txt_CustDocNo input').attr('maxlength', schemeLength);

                    $("#hddnDocNo").val('Auto');

                }
                else if (NoSchemeType == '0') {
                    schemeLength = 250;
                    ctxt_CustDocNo.SetText("");
                    ctxt_CustDocNo.SetEnabled(true);
                    $('#txt_CustDocNo input').attr('maxlength', schemeLength);

                }
                else if ($('#ddl_numberingScheme').val() == "0") {
                    ctxt_CustDocNo.SetText("");
                    ctxt_CustDocNo.SetEnabled(true);
                }

            });


            //End

            //Subhabrata
            $("#<%=ddlAssetLiability.ClientID%>").change(function () {
                debugger;
                var varddlAssetLiability = $("#<%=ddlAssetLiability.ClientID%>").val();
                if (varddlAssetLiability === "0") {
                    $('#lstTaxRates_MainAccount').prop('disabled', false).trigger("chosen:updated");
                }
                else {
                    $('#lstTaxRates_MainAccount').prop('disabled', true).trigger("chosen:updated");
                }

            });//End




            var type = ($("[id$='radioregistercheck']").find(":checked").val() != null) ? $("[id$='radioregistercheck']").find(":checked").val() : "";
            if (type == '1') {
                $("#<%= rdl_VendorType.ClientID %> input[type=radio]").prop('disabled', false);
                $('#DivVendorType').removeClass('hide');
                // $('#<%=rdl_VendorType.ClientID %>').find("input[value='R']").prop("checked", true);
                //$("#<%= rdl_VendorType.ClientID %> input[type=radio]").prop('checked', false);
            }
            else if (type == '0') {
                $("#<%= rdl_VendorType.ClientID %> input[type=radio]").prop('disabled', true);
                $("#<%= rdl_VendorType.ClientID %> input[type=radio]").prop('checked', false);
                $('#DivVendorType').addClass('hide');
            }

            $('#radioregistercheck').change(function () {
                debugger;
                var type = ($("[id$='radioregistercheck']").find(":checked").val() != null) ? $("[id$='radioregistercheck']").find(":checked").val() : "";
                if (type == '1') {
                    $("#<%= rdl_VendorType.ClientID %> input[type=radio]").prop('disabled', false);
                    $('#<%=rdl_VendorType.ClientID %>').find("input[value='R']").prop("checked", true);
                    $('#DivVendorType').removeClass('hide');
                    //$("#<%= rdl_VendorType.ClientID %> input[type=radio]").prop('checked', false);
                }
                else if (type == '0') {
                    $("#<%= rdl_VendorType.ClientID %> input[type=radio]").prop('disabled', true);
                   $("#<%= rdl_VendorType.ClientID %> input[type=radio]").prop('checked', false);
                   $('#DivVendorType').addClass('hide');
               }
            })
        }
       )
    </script>
    <script language="javascript" type="text/javascript">

        function InvalidUDF() {
            jAlert("Udf is mandatory.", "Alert", function () { OpenUdf(); });

        }

        function PannumberautoSet() {
            var contactType = '<%= Session["requesttype"] %>';
            var Gst2 = "";
            Gst2 = $("#txtGSTIN2_I").val().trim();
            if (Gst2 != "" && Gst2 != null && contactType != "" && contactType != null && contactType == "DV") {
                ctxtNumber.SetText($("#txtGSTIN2_I").val().trim());
            }
        }

        function validateGSTIN() {
            $("#myInput").removeClass("hide");
            $("#myInput").val($("#txtGSTIN1_I").val().trim() + $("#txtGSTIN2_I").val().trim() + $("#txtGSTIN3_I").val().trim());
            CopyFunction();


            window.open('https://services.gst.gov.in/services/searchtp');
        }

        function CopyFunction() {
            var copyText = document.getElementById("myInput");
            copyText.select();
            document.execCommand("copy");
            $("#myInput").addClass("hide");
        }


        function NameFromPan(s, e) {
            if (ctxtNumber.GetText() != "") {
                ctxtNameAsPerPan.SetEnabled(true);
            }
            else {
                ctxtNameAsPerPan.SetEnabled(false);
            }
        }

        function branchGridEndCallBack() {
            debugger;
            if (cbranchGrid.cpReceviedString) {
                if (cbranchGrid.cpReceviedString == 'SetAllRecordToDataTable') {
                    cBranchSelectPopup.Hide();
                }
            }

            if (cbranchGrid.cpBrselected) {

                if (cbranchGrid.cpBrselected == '1') {
                    jAlert("Individual branch selection not allowed when all branch option is checked.");
                    cbranchGrid.cpBrselected = null;
                    cBranchSelectPopup.Show();
                }
                else { cbranchGrid.cpBrselected = null; }
            }

            if (cbranchGrid.cpBrChecked) {
                if (cbranchGrid.cpBrChecked == '1') {
                    $('#<%= lblBranch.ClientID %>').attr('style', 'display:inline');
                    $('#<%=chkAllBranch.ClientID %>').prop('checked', true)
                    cbranchGrid.cpBrChecked = null;
                    $('#<%= hdnBranchAllSelected.ClientID %>').val('0');
                }
                else {
                    $('#<%= lblBranch.ClientID %>').attr('style', 'display:none');
                    $('#<%=chkAllBranch.ClientID %>').prop('checked', false)
                    cbranchGrid.cpBrChecked = null;
                    $('#<%= hdnBranchAllSelected.ClientID %>').val('1');
                }
            }
        }

        function ClearSelectedBranch() {
            cbranchGrid.PerformCallback('ClearSelectedBranch');
        }

        function SelectAllBranches(e) {
            debugger;
            if (e.checked == true) {

                ClearSelectedBranch();
                $('#<%= hdnBranchAllSelected.ClientID %>').val('0');
                $('#<%= lblBranch.ClientID %>').attr('style', 'display:inline');

            }
            else {
                $('#<%= hdnBranchAllSelected.ClientID %>').val('1');
                $('#<%= lblBranch.ClientID %>').attr('style', 'display:none');

            }
        }




        function CmbBranchChanged() {
            var branchCode = CmbBranch.GetValue();
            if (branchCode == 0) {
                $('#MultiProductButton').show();
            }
            else {
                $('#MultiProductButton').hide();
            }
        }

        function MultiBranchClick() {
            cbranchGrid.PerformCallback('SetAllSelectedRecord');
            cBranchSelectPopup.Show();
        }
        //Rev Bapi
        function MultiProductClick() {
            cproductGrid.PerformCallback('SetAllSelectedRecord');
            cProductSelectPopup.Show();
        }

        function productGridEndCallBack() {
            debugger;
            if (cproductGrid.cpReceviedString) {
                if (cproductGrid.cpReceviedString == 'SetAllRecordToDataTable') {
                    cProductSelectPopup.Hide();
                }
            }

            if (cproductGrid.cpPrselected) {

                if (cproductGrid.cpPrselected == '1') {
                    jAlert("Individual product selection not allowed when all product option is checked.");
                    cproductGrid.cpPrselected = null;
                    cProductSelectPopup.Show();
                }
                else { cproductGrid.cpPrselected = null; }
            }

            if (cproductGrid.cpPrselected) {
                if (cproductGrid.cpPrselected == '1') {
                    $('#<%= lblProduct.ClientID %>').attr('style', 'display:inline');
                    $('#<%=chkAllProduct.ClientID %>').prop('checked', true)
                    cproductGrid.cpPrselected = null;
                    $('#<%= hdnProductAllSelected.ClientID %>').val('0');
                }
                else {
                    $('#<%= lblProduct.ClientID %>').attr('style', 'display:none');
                    $('#<%=chkAllProduct.ClientID %>').prop('checked', false)
                    cproductGrid.cpPrselected = null;
                    $('#<%= hdnProductAllSelected.ClientID %>').val('1');
                }
            }
        }

        function ClearSelectedProduct() {
            cproductGrid.PerformCallback('ClearSelectedProduct');
        }

        function SelectAllProducts(e) {
            debugger;
            if (e.checked == true) {

                ClearSelectedProduct();
                $('#<%= hdnProductAllSelected.ClientID %>').val('0');
                $('#<%= lblProduct.ClientID %>').attr('style', 'display:inline');

            }
            else {
                $('#<%= hdnProductAllSelected.ClientID %>').val('1');
                $('#<%= lblProduct.ClientID %>').attr('style', 'display:none');

            }
        }

        function CmbProductChanged() {
            var productCode = CmbProduct.GetValue();
            if (productCode == 0) {
                $('#MultiBranchButton').show();
            }
            else {
                $('#MultiBranchButton').hide();
            }
        }
        function SaveSelectedProduct() {
            cproductGrid.PerformCallback('SetAllRecordToDataTable');
        }
        //End Rev Bapi

        function SaveSelectedBranch() {
            cbranchGrid.PerformCallback('SetAllRecordToDataTable');
        }

        function selectAll() {

            cbranchGrid.PerformCallback('SelectAllBranchesFromList');
            cBranchSelectPopup.Show();
        }
        function unselectAll() {
            cbranchGrid.PerformCallback('ClearSelectedBranch');
            cBranchSelectPopup.Show();
        }

        //rev Bapi
        function selectAllProduct() {

            cproductGrid.PerformCallback('SelectAllProductsFromList');
            cProductSelectPopup.Show();
        }
        function unselectAllProduct() {
            cproductGrid.PerformCallback('ClearSelectedProduct');
            cProductSelectPopup.Show();
        }

        // End Rev Bapi



        //Gstin settings


        //Chinmoy edited start 07//5/2018

        function changeFunc() {
            //debugger;
            var MainAccount_val2 = document.getElementById("lstTaxRates_MainAccount").value;
            if (MainAccount_val2 === "0") {
                if (document.getElementById('hdIsMainAccountInUse').value == "IsInUse") {
                    jAlert("Transaction exists for the selected Ledger. Cannot proceed.");
                    Changeselectedvalue();
                }
                else {
                    document.getElementById("hndTaxRates_MainAccount_hidden").value = document.getElementById("lstTaxRates_MainAccount").value;
                    ChangeSubSource();
                }
            }
            else {
                if (document.getElementById('hdIsMainAccountInUse').value == "IsInUse") {
                    jAlert("Transaction exists for the selected Ledger. Cannot proceed.");
                    Changeselectedvalue();
                } else {

                    document.getElementById("hndTaxRates_MainAccount_hidden").value = document.getElementById("lstTaxRates_MainAccount").value;
                    ChangeSubSource();
                }
            }

            <%-- if (MainAccount_val2 === "0") {
                //$("#<%=ddlAssetLiability.ClientID%>").prop('disabled', false);
                
                if (document.getElementById('hdIsMainAccountInUse').value == "IsInUse") {
                    jAlert("Transaction exists for the selected Ledger. Cannot proceed.");
                    Changeselectedvalue();
                }
                else {
                    document.getElementById("hndTaxRates_MainAccount_hidden").value = document.getElementById("lstTaxRates_MainAccount").value;
                }
             
            }--%>
           <%-- else {
                //$("#<%=ddlAssetLiability.ClientID%>").prop('disabled', true);
                if (document.getElementById('hdIsMainAccountInUse').value == "IsInUse") {
                    jAlert("Transaction exists for the selected Ledger. Cannot proceed.");
                    Changeselectedvalue();
                } else {

                    document.getElementById("hndTaxRates_MainAccount_hidden").value = document.getElementById("lstTaxRates_MainAccount").value;
                    ChangeSubSource();
                }
            }--%>



        }


        //Chinmoy edited end 07//5/2018
        function ValidatePanno() {
            debugger;
            // var PAN = document.getElementById('txtNumber_I').value;
            //var p= ctxtNumber.GetText();
            //var PAN = p.toUpperCase();
            //$("#hdnflag").val("0");
            //if (PAN != "")
            //{
            //var panPat = /^([a-zA-Z]{5})(\d{4})([a-zA-Z]{1})$/;
            //var code = /([C,P,H,F,A,T,B,L,J,G])/;
            //var code_chk = PAN.substring(3, 4);
            //if (PAN.search(panPat) == -1) {
            //   // jAlert("Please Enter Valid Pan No");
            //    $("#hdnflag").val("1");
            //    return;
            //}
            //if (code.test(code_chk) == false) {
            //  //  jAlert("Invaild PAN Card No or Use Capital Letter");
            //    $("#hdnflag").val("1");
            //    return;
            //}
            //}

            $("#hdnflag").val("0");
            var PAN = ctxtNumber.GetText().toUpperCase();
            var panPat = /^([a-zA-Z]{5})(\d{4})([a-zA-Z]{1})$/;
            var code = /([C,P,H,F,A,T,B,L,J,G])/;
            var code_chk = PAN.substring(3, 4);
            if (PAN != "") {
                if (PAN.search(panPat) == -1) {
                    $("#hdnflag").val("1");

                    return false;
                }
                if (code.test(code_chk) == false) {
                    $("#hdnflag").val("1");
                    return false;
                }

            }




        }

        function changeSubFunc() {
            document.getElementById("hndTaxRates_SubAccount_hidden").value = document.getElementById("lstTaxRates_SubAccount").value;
        }
        function ListBind() {

            var config = {
                '.chsn': {},
                '.chsn-deselect': { allow_single_deselect: true },
                '.chsn-no-single': { disable_search_threshold: 10 },
                '.chsn-no-results': { no_results_text: 'Oops, nothing found!' },
                '.chsn-width': { width: "100%" }
            }
            for (var selector in config) {
                $(selector).chosen(config[selector]);
            }

        }

        function ChangeSubSource() {
            var fname = "%";
            var mainAccount = document.getElementById("hndTaxRates_MainAccount_hidden").value;;
            var lReportTo = $('select[id$=lstTaxRates_SubAccount]');
            lReportTo.empty();

            $.ajax({
                type: "POST",
                url: "HRrecruitmentagent_general.aspx/GetSubAccountList",
                data: JSON.stringify({ reqStr: fname, mainreqStr: mainAccount }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var list = msg.d;
                    var listItems = [];
                    if (list.length > 0) {

                        for (var i = 0; i < list.length; i++) {
                            var id = '';
                            var name = '';
                            id = list[i].split('|')[1];
                            name = list[i].split('|')[0];
                            $('#lstTaxRates_SubAccount').append($('<option>').text(name).val(id));
                        }

                        $(lReportTo).append(listItems.join(''));
                        ListBind();
                        $('#lstTaxRates_SubAccount').trigger("chosen:updated");
                        $('#lstTaxRates_SubAccount').trigger("chosen:updated");
                    }
                    else {
                        $('#lstTaxRates_SubAccount').trigger("chosen:updated");
                    }
                }
            });
        }

        function Changeselectedvalue() {
            var lstTaxRates_MainAccount = document.getElementById("lstTaxRates_MainAccount");
            if (document.getElementById("hndTaxRates_MainAccount_hidden").value != '') {
                for (var i = 0; i < lstTaxRates_MainAccount.options.length; i++) {
                    if (lstTaxRates_MainAccount.options[i].value == document.getElementById("hndTaxRates_MainAccount_hidden").value) {
                        lstTaxRates_MainAccount.options[i].selected = true;
                    }
                }
                $('#lstTaxRates_MainAccount').trigger("chosen:updated");
            }
        }
        function ChangeSourceMainAccount() {
            var fname = "%";
            var lReportTo = $('select[id$=lstTaxRates_MainAccount]');
            lReportTo.empty();

            $.ajax({
                type: "POST",
                url: "HRrecruitmentagent_general.aspx/GetMainAccountList",
                data: JSON.stringify({ reqStr: fname }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var list = msg.d;
                    var listItems = [];
                    if (list.length > 0) {
                        $('#lstTaxRates_MainAccount').empty().append('<option selected="selected" value="">None</option>');//Subhabrata
                        for (var i = 0; i < list.length; i++) {
                            var id = '';
                            var name = '';
                            id = list[i].split('|')[1];
                            name = list[i].split('|')[0];

                            $('#lstTaxRates_MainAccount').append($('<option>').text(name).val(id));
                        }

                        $(lReportTo).append(listItems.join(''));

                        $('#lstTaxRates_MainAccount').trigger("chosen:updated");
                        Changeselectedvalue();
                    }
                    else {
                        $('#lstTaxRates_MainAccount').trigger("chosen:updated");
                    }
                }
            });
        }




        //Debjyoti Code for UDF
        function OpenUdf() {
            if (document.getElementById('IsUdfpresent').value == '0') {
                jAlert("UDF not define.");
            }
            else {
                var keyVal = document.getElementById('Keyval_internalId').value;
                var url = '/OMS/management/Master/frm_BranchUdfPopUp.aspx?Type=DV&&KeyVal_InternalID=' + keyVal;
                popup.SetContentUrl(url);
                popup.Show();
            }
            return true;
        }


        //Debjyoti Code for GSTIN 060217
        function fn_AllowonlyNumeric(s, e) {
            var theEvent = e.htmlEvent || window.event;
            var key = theEvent.keyCode || theEvent.which;
            var keychar = String.fromCharCode(key);
            if (key == 9 || key == 37 || key == 38 || key == 39 || key == 40 || key == 8) { //tab/ Left / Up / Right / Down Arrow, Backspace, Delete keys
                return;
            }
            var regex = /[0-9]/;

            if (!regex.test(keychar)) {
                theEvent.returnValue = false;
                if (theEvent.preventDefault)
                    theEvent.preventDefault();
            }
        }
        function Gstin2TextChanged(s, e) {

            if (!e.htmlEvent.ctrlKey) {
                if (e.htmlEvent.key != 'Control') {
                    s.SetText(s.GetText().toUpperCase());
                }
            }

        }

        $(document).ready(function () {

            if (ctxtNumber.GetText() != "") {
                ctxtNameAsPerPan.SetEnabled(true);
            }
            else {
                ctxtNameAsPerPan.SetEnabled(false);
            }


            $('#<%= lblBranch.ClientID %>').attr('style', 'display:none');

            <%--if (e.checked == true) {
             
                $('#<%= hdnBranchAllSelected.ClientID %>').val('0');
              

             }
             else {
                 $('#<%= hdnBranchAllSelected.ClientID %>').val('1');               

             }--%>





            if ($("#radioregistercheck").find(":checked").val() == '1') {

                // alert(1);
                ctxtGSTIN111.SetEnabled(true);
                ctxtGSTIN222.SetEnabled(true);
                ctxtGSTIN333.SetEnabled(true);
                cApplicableFrom.SetEnabled(true);
            }
            else {
                // alert(2);
                ctxtGSTIN111.SetText('');
                ctxtGSTIN222.SetText('');
                ctxtGSTIN333.SetText('');

                ctxtGSTIN111.SetEnabled(false);
                ctxtGSTIN222.SetEnabled(false);
                ctxtGSTIN333.SetEnabled(false);
                cApplicableFrom.SetEnabled(false);

            }



            ListBind();
            ChangeSource();
            ChangeSourceMainAccount();

            $('body').on('click', '#radioregistercheck', function () {
                var optionText = $('#<%=radioregistercheck.ClientID %> input:checked').val();
                if (optionText == '1') {
                    // $("#spanmandategstn").attr('style', 'display:inline;color:red');
                    //alert(1);
                    ctxtGSTIN111.SetEnabled(true);
                    ctxtGSTIN222.SetEnabled(true);
                    ctxtGSTIN333.SetEnabled(true);
                    cApplicableFrom.SetEnabled(true);
                }
                else {
                    //alert(2);
                    ctxtGSTIN111.SetText('');
                    ctxtGSTIN222.SetText('');
                    ctxtGSTIN333.SetText('');

                    ctxtGSTIN111.SetEnabled(false);
                    ctxtGSTIN222.SetEnabled(false);
                    ctxtGSTIN333.SetEnabled(false);
                    cApplicableFrom.SetEnabled(false);
                    // $("#spanmandategstn").attr('style', 'display:none;');
                }
            });




        });
            function validate() {
                debugger;
                var fName = ctxtFirstName.GetText();
                var retValue = true;
                var validateFlag = true;


                var sName = "";





                if (($("#hdnAutoNumStg").val() == "DVNumb1") && ($("#hdnTransactionType").val() == "DV") && ($("#hddnApplicationMode").val() == "Add")) {

                    if (($("#ddl_numberingScheme").val() == "0")) {
                        jAlert("Please Select Numbering Scheme.");

                        retValue = false;
                        return false;
                    }

                    else if (ctxt_CustDocNo.GetText() == "") {
                        jAlert("Please Enter Unique ID.");
                        retValue = false;
                        return false;
                    }
                }


                if (($("#hdnAutoNumStg").val() == "DVNumb0") && ($("#hdnTransactionType").val() == "")) {
                    sName = ctxtCode.GetText();
                }
                else if (($("#hdnAutoNumStg").val() == "DVNumb1") && ($("#hdnTransactionType").val() == "DV")) {
                    sName = ctxtCode.SetText("ABC");
                    $("#hddnDocNo").val(ctxt_CustDocNo.GetText());
                }

                if (fName.trim() == "") {
                    $('#MandatoryName').css({ 'display': 'block' });
                    retValue = false;
                }
                else {
                    $('#MandatoryName').css({ 'display': 'none' });
                }

                if (($("#hdnAutoNumStg").val() == "DVNumb0") && ($("#hdnTransactionType").val() == "")) {
                    if (sName.trim() == "") {
                        $('#MandatoryShortName').css({ 'display': 'block' });
                        retValue = false;
                    } else {
                        $('#MandatoryShortName').css({ 'display': 'none' });
                    }
                }


                //Debjyoti 060217
                $('#invalidGst').css({ 'display': 'none' });
                var gst1 = ctxtGSTIN111.GetText().trim();
                var gst2 = ctxtGSTIN222.GetText().trim();
                var gst3 = ctxtGSTIN333.GetText().trim();

                if (gst1.length == 0 && gst2.length == 0 && gst3.length == 0) {
                    var isregistered = $('#<%=radioregistercheck.ClientID %> input:checked').val();
                if (isregistered == 1) {
                    jAlert('GSTIN is mandatory.');
                    retValue = false;
                }

            }
            else {


                if (gst1.length != 2 || gst2.length != 10 || gst3.length != 3) {
                    $('#invalidGst').css({ 'display': 'block' });
                    retValue = false;
                }


                var panPat = /^([a-zA-Z]{5})(\d{4})([a-zA-Z]{1})$/;
                var panPat1 = /^([a-zA-Z]{4})(\d{5})([a-zA-Z]{1})$/;
                var code = /([C,P,H,F,A,T,B,L,J,G])/;
                var code_chk = gst2.substring(3, 4);
                if (gst2.search(panPat) == -1 && gst2.search(panPat1)) {
                    $('#invalidGst').css({ 'display': 'block' });
                    retValue = false;
                }
                if (code.test(code_chk) == false) {
                    $('#invalidGst').css({ 'display': 'block' });
                    retValue = false;
                }


                //Subhabrata
                var isregisteredCheck = $('#<%=radioregistercheck.ClientID %> input:checked').val();
                var finalGST = (gst1 + gst2 + gst3);
                var GSTINOldval = $("#<%=hddnGSTIN2Val.ClientID%>").val();



                if (cApplicableFrom.GetDate() === null && $("#<%=hddnApplicationMode.ClientID%>").val() === "Edit" && GSTINOldval.trim() !== finalGST.trim()) {
                    if (isregisteredCheck == "1") {

                        jAlert("Please enter Applicable from.");
                        retValue = false;
                        validateFlag = false;
                    }
                }
               <%-- else if (cApplicableFrom.GetDate() === null && $("#<%=hddnApplicationMode.ClientID%>").val() === "Edit") {
                    if (isregisteredCheck == "1") {

                        jAlert("Please enter Applicable from.");
                        retValue = false;
                        validateFlag = false;
                    }
                }--%>


                if (GSTINOldval.trim() !== finalGST.trim() && validateFlag == true && $("#<%=hddnApplicationMode.ClientID%>").val() === "Edit") {
                    if (isregisteredCheck == "1") {
                        var r = confirm("You have entered GSTIN Applicable date.Based on the applicable date all the transaction will be updated with entered GSTIN.\nAre you sure?");
                        if (r == true) {
                            retValue = true;
                        }
                        else {
                            retValue = false;
                        }
                        $("#<%=hddnGSTINFlag.ClientID%>").val("UPDATE");
                    }
                }
                else {
                    $("#<%=hddnGSTINFlag.ClientID%>").val("NotUPDATE");
                }




                //End



            }






            //End Here Debjyoti 060217
            return retValue;
        }

        function disp_prompt(name) {
            //console.log(name);
            if (name == "tab0") {
                //alert(name);
                document.location.href = "HRrecruitmentagent_general.aspx";
            }
            if (name == "tab1") {
                //alert(name);
                document.location.href = "HRrecruitmentagent_ContactPerson.aspx";
            }
            else if (name == "tab2") {
                //alert(name);
                document.location.href = "HRrecruitmentagent_Correspondence.aspx";
            }
            else if (name == "tab3") {
                //alert(name);
                document.location.href = "HRrecruitmentagent_BankDetails.aspx";
            }

            else if (name == "tab4") {
                document.location.href = "HRrecruitmentagent_Document.aspx";
            }
            else if (name == "tab6") {
                document.location.href = "HRrecruitmentagent_GroupMember.aspx";

            }
            else if (name == "tab5") {
                //alert(name);
                document.location.href = "HRrecruitmentagent_Registration.aspx";
            }
            else if (name == "tab7") {
                document.location.href = "vendors_tds.aspx";
            }

        }


        function UniqueCodeCheck() {
            debugger;
            var SchemeVal = $("#ddl_numberingScheme").val();

            var NoSchemeId = SchemeVal.toString().split('~')[0];
            if (SchemeVal == "0") {
                alert('Please Select Numbering Scheme');
                //ctxt_SlOrderNo.SetValue('');
                //ctxt_SlOrderNo.Focus();
            }

                //if (NoSchemeId == "0")
            else {
                var CheckUniqueCode = false;
                var uccName = "";
                var Type = "";

                if (($("#hdnAutoNumStg").val() == "DVNumb1") && ($("#hdnTransactionType").val() == "DV")) {
                    uccName = ctxt_CustDocNo.GetText();
                    Type = "MasterVendor";
                }

                $.ajax({
                    type: "POST",
                    url: "HRrecruitmentagent_general.aspx/CheckUniqueNumberingCode",
                    data: JSON.stringify({ uccName: uccName, Type: Type }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        CheckUniqueCode = msg.d;
                        if (CheckUniqueCode == true) {
                            alert('Please enter unique No.');
                            //jAlert('Please enter unique Sales Order No');
                            ctxt_CustDocNo.SetValue('');
                            ctxt_CustDocNo.Focus();
                        }

                    }

                });
            }
        }


        function UniqueCheck() {
            var code = 0;
            if (GetObjectID('HdId').value != '') {
                code = GetObjectID('HdId').value;
            }
            var reminderShortName = ctxtCode.GetText();
            if (reminderShortName.trim() == '')
                return;
            var CheckUniqueCode = false;
            $.ajax({
                type: "POST",
                url: "HRrecruitmentagent_general.aspx/CheckUniqueCode",
                data: JSON.stringify({ CategoriesShortCode: reminderShortName, Code: code }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    CheckUniqueCode = msg.d;
                    if (CheckUniqueCode != true) {
                        jAlert('Unique ID already exists.');
                        ctxtCode.SetText('');
                        ctxCode.Focus();
                    }
                }
            });
        }

        // Rev 2.0
        function UniqueVendorNameCheck() {
            var code = 0;
            if (GetObjectID('HdId').value != '') {
                code = GetObjectID('HdId').value;
            }
            var reminderFirstName = ctxtFirstName.GetText();
            if (reminderFirstName.trim() == '')
                return;
            var CheckUniqueVendName = false;
            $.ajax({
                type: "POST",
                url: "HRrecruitmentagent_general.aspx/CheckUniqueVendorName",
                data: JSON.stringify({ CategoriesFirstName: reminderFirstName, Code: code }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    CheckUniqueVendName = msg.d;
                    if (CheckUniqueVendName != true) {
                        jAlert('Vendor Name already exists.');
                        ctxtFirstName.SetText('');
                        ctxtFirstName.Focus();
                    }
                }
            });
        }
        // End of Rev 2.0

        function CallList(obj1, obj2, obj3) {
            if (obj1.value == "") {
                obj1.value = "%";
            }

            var obj4 = document.getElementById('<%=cmbSource.ClientID %>');
            var obj5 = obj4.value;


            if (obj5 != '18') {
                ajax_showOptionsTEST(obj1, obj2, obj3, obj5);
                if (obj1.value == "%") {
                    obj1.value = "";
                }
            }
        }
        function ListBind() {

            var config = {
                '.chsn': {},
                '.chsn-deselect': { allow_single_deselect: true },
                '.chsn-no-single': { disable_search_threshold: 10 },
                '.chsn-no-results': { no_results_text: 'Oops, nothing found!' },
                '.chsn-width': { width: "100%" }
            }
            for (var selector in config) {
                $(selector).chosen(config[selector]);
            }

        }
        function lstReferedBy() {

            // $('#lstReferedBy').chosen();
            $('#lstReferedBy').fadeIn();
        }

        function switchRefferedControl(obj) {
            switch (obj) {
                case "1":
                    document.getElementById("refferByDD").value = false;
                    $('#txtReferedBy').css({ 'display': 'block' });
                    $('#lstReferedBy_chosen').css({ 'display': 'none' });
                    return false;
                    break;
                case "2":
                    document.getElementById("refferByDD").value = false;
                    $('#txtReferedBy').css({ 'display': 'block' });
                    $('#lstReferedBy_chosen').css({ 'display': 'none' });
                    return false;
                    break;
                case "5":
                    document.getElementById("refferByDD").value = false;
                    $('#txtReferedBy').css({ 'display': 'block' });
                    $('#lstReferedBy_chosen').css({ 'display': 'none' });
                    return false;
                    break;
                case "6":
                    document.getElementById("refferByDD").value = false;
                    $('#txtReferedBy').css({ 'display': 'block' });
                    $('#lstReferedBy_chosen').css({ 'display': 'none' });
                    return false;
                    break;
                case "7":
                    document.getElementById("refferByDD").value = false;
                    $('#txtReferedBy').css({ 'display': 'block' });
                    $('#lstReferedBy_chosen').css({ 'display': 'none' });
                    return false;
                    break;
                case "9":
                    document.getElementById("refferByDD").value = false;
                    $('#txtReferedBy').css({ 'display': 'block' });
                    $('#lstReferedBy_chosen').css({ 'display': 'none' });
                    return false;
                    break;
                case "11":
                    document.getElementById("refferByDD").value = false;
                    $('#txtReferedBy').css({ 'display': 'block' });
                    $('#lstReferedBy_chosen').css({ 'display': 'none' });
                    return false;
                    break;
                case "12":
                    document.getElementById("refferByDD").value = false;
                    $('#txtReferedBy').css({ 'display': 'block' });
                    $('#lstReferedBy_chosen').css({ 'display': 'none' });
                    return false;
                    break;
                case "13":
                    document.getElementById("refferByDD").value = false;
                    $('#txtReferedBy').css({ 'display': 'block' });
                    $('#lstReferedBy_chosen').css({ 'display': 'none' });
                    return false;
                    break;
                case "15":
                    document.getElementById("refferByDD").value = false;
                    $('#txtReferedBy').css({ 'display': 'block' });
                    $('#lstReferedBy_chosen').css({ 'display': 'none' });
                    return false;
                    break;
                case "16":
                    document.getElementById("refferByDD").value = false;
                    $('#txtReferedBy').css({ 'display': 'block' });
                    $('#lstReferedBy_chosen').css({ 'display': 'none' });
                    return false;
                    break;

                case "17":
                    document.getElementById("refferByDD").value = false;
                    $('#txtReferedBy').css({ 'display': 'block' });
                    $('#lstReferedBy_chosen').css({ 'display': 'none' });
                    return false;
                    break;
                case "18":
                    document.getElementById("refferByDD").value = false;
                    $('#txtReferedBy').css({ 'display': 'block' });
                    $('#lstReferedBy_chosen').css({ 'display': 'none' });
                    return false;
                    break;
                case "0":
                    //dd
                    document.getElementById("refferByDD").value = true;
                    $('#txtReferedBy').css({ 'display': 'none' });
                    $('#lstReferedBy_chosen').css({ 'display': 'block' });
                    return true;
                    break;
                case "3":
                    //dd
                    document.getElementById("refferByDD").value = true;
                    $('#txtReferedBy').css({ 'display': 'none' });
                    $('#lstReferedBy_chosen').css({ 'display': 'block' });
                    return true;
                    break;
                case "4":
                    //dd
                    document.getElementById("refferByDD").value = true;
                    $('#txtReferedBy').css({ 'display': 'none' });
                    $('#lstReferedBy_chosen').css({ 'display': 'block' });
                    return true;
                    break;
                case "8":
                    //dd
                    document.getElementById("refferByDD").value = true;
                    $('#txtReferedBy').css({ 'display': 'none' });
                    $('#lstReferedBy_chosen').css({ 'display': 'block' });
                    return true;
                    break;
                case "10":
                    //dd
                    document.getElementById("refferByDD").value = true;
                    $('#txtReferedBy').css({ 'display': 'none' });
                    $('#lstReferedBy_chosen').css({ 'display': 'block' });
                    return true;
                    break;
                case "14":
                    document.getElementById("refferByDD").value = true;
                    $('#txtReferedBy').css({ 'display': 'none' });
                    $('#lstReferedBy_chosen').css({ 'display': 'block' });
                    return true;
                    break;
                case "20":
                    //dd
                    document.getElementById("refferByDD").value = true;
                    $('#txtReferedBy').css({ 'display': 'none' });
                    $('#lstReferedBy_chosen').css({ 'display': 'block' });
                    return true;
                    break;
                case "24":
                    //dd
                    document.getElementById("refferByDD").value = true;
                    $('#txtReferedBy').css({ 'display': 'none' });
                    $('#lstReferedBy_chosen').css({ 'display': 'block' });
                    return true;
                    break;
                case "25":
                    //dd
                    document.getElementById("refferByDD").value = true;
                    $('#txtReferedBy').css({ 'display': 'none' });
                    $('#lstReferedBy_chosen').css({ 'display': 'block' });
                    return true;
                    break;
                default:
                    //dd
                    document.getElementById("refferByDD").value = true;
                    $('#txtReferedBy').css({ 'display': 'none' });
                    $('#lstReferedBy_chosen').css({ 'display': 'block' });
                    return true;
                    break;
            }
        }

        function ChangeSource() {
            var InterId = "";
            if (document.getElementById("cmbSource").value) {
                InterId = document.getElementById("cmbSource").value;
            }
            var id = document.getElementById("cmbSource").value;
            if (switchRefferedControl(id)) {
                var lReferBy = $('select[id$=lstReferedBy]');
                lReferBy.empty();
                $.ajax({
                    type: "POST",
                    url: "HRrecruitmentagent_general.aspx/GetReffer",
                    data: JSON.stringify({ sourceId: id, hdKeyValIntId: InterId }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var list = msg.d;
                        var listItems = [];
                        if (list.length > 0) {

                            for (var i = 0; i < list.length; i++) {
                                var id = '';
                                var name = '';
                                id = list[i].split('|')[1];
                                name = list[i].split('|')[0];

                                listItems.push('<option value="' +
                                name + '">' + name
                                + '</option>');
                            }

                            $(lReferBy).append(listItems.join(''));

                            lstReferedBy();
                            $('#lstReferedBy').trigger("chosen:updated");

                            if (switchRefferedControl(document.getElementById("cmbSource").value)) {
                                var valRefferBy = document.getElementById('RefferedByValue').value;

                                if (valRefferBy) {
                                    var refferByChosen = document.getElementById("lstReferedBy");

                                    for (var i = 0; i < refferByChosen.options.length; i++) {
                                        if (refferByChosen.options[i].value == valRefferBy) {
                                            refferByChosen.options[i].selected = true;
                                        }
                                    }
                                    $('#lstReferedBy').trigger("chosen:updated");
                                    document.getElementById('RefferedByValue').value = null;
                                }
                            }

                        }
                        else {
                            jAlert("No records found");
                            lstReferedBy();
                            $('#lstReferedBy').trigger("chosen:updated");
                        }
                    }
                });
            }
        }

        FieldName = 'cmbLegalStatus';
    </script>

    <style>
        .vehiclecls {
            display: inline;
            color: red;
            font-size: 14px;
            padding-left: 15px;
        }

        .abs {
            position: absolute;
            right: -19px;
            top: 4px;
        }

        .chosen-container.chosen-container-single {
            width: 100% !important;
        }

        .chosen-choices {
            width: 100% !important;
        }

        #lstReferedBy {
            width: 200px;
        }

        #lstReferedBy {
            display: none !important;
        }

        #lstReferedBy_chosen {
            width: 100% !important;
        }

        .dxtcLite_PlasticBlue > .dxtc-content {
            overflow: visible !important;
        }

        .nestedinput li {
            list-style-type: none;
            display: inline-block;
            float: left;
        }

            .nestedinput li.dash {
                width: 26px;
                text-align: center;
                padding: 6px;
            }

            .nestedinput li .iconRed {
                position: absolute;
                right: -10px;
                top: 5px;
            }

        #txtNumber input {
            text-transform: uppercase;
        }

        /*Rev 1.0*/
        .outer-div-main {
            background: #ffffff;
            padding: 10px;
            border-radius: 10px;
            box-shadow: 1px 1px 10px #11111154;
        }

        /*.form_main {
            overflow: hidden;
        }*/

        label , .mylabel1, .clsTo, .dxeBase_PlasticBlue
        {
            color: #141414 !important;
            font-size: 14px !important;
                font-weight: 500 !important;
                margin-bottom: 0 !important;
                    line-height: 20px;
        }

        #GrpSelLbl .dxeBase_PlasticBlue
        {
                line-height: 20px !important;
        }

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
            bottom: 6px;
            right: 20px;
            z-index: 0;
            cursor: pointer;
        }

        #ASPxFromDate , #ASPxToDate , #ASPxASondate , #ASPxAsOnDate , #txtDOB , #txtAnniversary , #DateOfIncoorporation
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        .dxeDisabled_PlasticBlue
        {
            z-index: 0 !important;
        }

        #ASPxFromDate_B-1 , #ASPxToDate_B-1 , #ASPxASondate_B-1 , #ASPxAsOnDate_B-1 , #txtDOB_B-1 , #txtAnniversary_B-1 , #DateOfIncoorporation_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #ASPxFromDate_B-1 #ASPxFromDate_B-1Img , #ASPxToDate_B-1 #ASPxToDate_B-1Img , #ASPxASondate_B-1 #ASPxASondate_B-1Img , #ASPxAsOnDate_B-1 #ASPxAsOnDate_B-1Img ,
        #txtDOB_B-1 #txtDOB_B-1Img ,
        #txtAnniversary_B-1 #txtAnniversary_B-1Img , #DateOfIncoorporation_B-1 #DateOfIncoorporation_B-1Img
        {
            display: none;
        }

        .dxtcLite_PlasticBlue > .dxtc-stripContainer .dxtc-activeTab, .dxgvFooter_PlasticBlue
        {
            background: #1b5ea4 !important;
        }

        .simple-select::after , .simple-select-2::after {
            /*content: '<';*/
            content: url(../../../assests/images/left-arw.png);
            position: absolute;
            top: 26px;
            right: 13px;
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
        .simple-select-2::after
        {
            top: 34px !important;
        }
        .simple-select , .simple-select-2 {
            position: relative;
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

        .TableMain100 #ShowGrid , .TableMain100 #ShowGridList , .TableMain100 #ShowGridRet , .TableMain100 #ShowGridLocationwiseStockStatus 
        #B2B , 
        .TableMain100 #EmployeeGrid
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

        .col-md-3>label, .col-md-3>span
        {
            margin-top: 8px !important;
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

        .chosen-container-single .chosen-single div::after
        {
            font-size: 17px !important;
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
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main">
        <div class="panel-heading">
        <div class="panel-title">
            <h3>Vendors/Service Providers</h3>
            <div class="crossBtn"><a href="HRrecruitmentagent.aspx"><i class="fa fa-times"></i></a></div>
        </div>
    </div>
        <div class="form_main">
        <%--debjyoti 21-03-2016--%>
        <dxe:ASPxPopupControl ID="ASPXPopupControl" runat="server"
            CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popup" Height="630px"
            Width="600px" HeaderText="Add/Modify UDF" Modal="true" AllowResize="true" ResizingMode="Postponed">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>
        <asp:HiddenField runat="server" ID="IsUdfpresent" />
        <asp:HiddenField runat="server" ID="Keyval_internalId" />
        <asp:HiddenField ID="hidAssociatedEmp" runat="server" />
        <asp:HiddenField ID="hddnApplicationMode" runat="server" />
        <asp:HiddenField ID="hddnGSTIN2Val" runat="server" />
        <asp:HiddenField ID="hddnGSTINFlag" runat="server" />
        <asp:HiddenField ID="hdnAutoNumStg" runat="server" />
        <asp:HiddenField ID="hdnTransactionType" runat="server" />
        <asp:HiddenField ID="hdnNumberingId" runat="server" />
        <asp:HiddenField ID="hddnDocNo" runat="server" />
        <%--End debjyoti 21-03-2016--%>



        <%-- <div class="crossBtn" style="right: 28px;top: 14px;"><a href="HRrecruitmentagent.aspx"><i class="fa fa-times"></i></a></div>--%>
        <table class="TableMain100">
            <tr>
                <td style="text-align: center">
                    <asp:Label ID="lblHeader" runat="server" Font-Bold="True" Font-Size="15px" ForeColor="Navy"
                        Width="819px" Height="18px"></asp:Label></td>
            </tr>
            <tr>
                <td>
                    <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" ClientInstanceName="page" Width="100%">
                        <TabPages>
                            <dxe:TabPage Text="General" Name="General">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                        <div class="row">
                                            <div class="col-md-3 hide">
                                                <label>
                                                    <dxe:ASPxLabel ID="ASPxLabel1" runat="server" Text="Salutation">
                                                    </dxe:ASPxLabel>
                                                </label>
                                                <div>
                                                    <asp:DropDownList ID="CmbSalutation" runat="server" Width="100%">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>



                                            <%--chinmoy added for Auto numbering start--%>
                                            <div class="col-md-2 " id="ddl_Num" runat="server" style="display: none">

                                                <label>
                                                    <dxe:ASPxLabel ID="lbl_NumberingScheme" Width="120px" runat="server" Text="Numbering Scheme">
                                                    </dxe:ASPxLabel>
                                                </label>
                                                <asp:DropDownList ID="ddl_numberingScheme" runat="server" Width="100%">
                                                </asp:DropDownList>


                                            </div>

                                            <div class="col-md-2 " runat="server" id="dvCustDocNo" style="display: none">
                                                <label>
                                                    <dxe:ASPxLabel ID="lbl_CustDocNo" runat="server" Text="Unique ID" Width="">
                                                    </dxe:ASPxLabel>
                                                    <span style="color: red">*</span>
                                                </label>

                                                <dxe:ASPxTextBox ID="txt_CustDocNo" runat="server" ClientInstanceName="ctxt_CustDocNo" Width="100%" MaxLength="30">
                                                    <ClientSideEvents TextChanged="function(s, e) {UniqueCodeCheck();}" />
                                                </dxe:ASPxTextBox>

                                            </div>

                                           

                                            <%--end--%>

                                            <div class="col-md-2">
                                                <label>
                                                    <dxe:ASPxLabel ID="ASPxLabel5" runat="server" Text="Name">
                                                    </dxe:ASPxLabel>
                                                    <span style="color: red">*</span>
                                                </label>
                                                <div style="position: relative">
                                                    <dxe:ASPxTextBox ID="txtFirstName" runat="server" Width="100%" MaxLength="150" ClientInstanceName="ctxtFirstName" CssClass="upper">
                                                        <%-- <ValidationSettings ValidationGroup="a">
                                                        </ValidationSettings>--%>
                                                        <%--Rev 2.0--%>
                                                        <ClientSideEvents LostFocus="function(s, e) {
	                                                            UniqueVendorNameCheck();
                                                            }" />
                                                        <%--End of Rev 2.0--%>
                                                    </dxe:ASPxTextBox>

                                                    <span id="MandatoryName" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; display: none; position: absolute; right: -20px; top: 5px;"
                                                        title="Mandatory"></span>

                                                    <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtFirstName"
                                                        Display="Dynamic" ErrorMessage="Mandatory." SetFocusOnError="True" ValidationGroup="a"
                                                         ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                                </div>
                                            </div>

                                            <div class="col-md-2" runat="server" id="dvUniqueId">
                                                <label>
                                                    <dxe:ASPxLabel ID="ASPxLabel4" runat="server" Text="Unique ID">
                                                    </dxe:ASPxLabel>
                                                    <span style="color: red">*</span>
                                                </label>
                                                <div style="position: relative">
                                                    <dxe:ASPxTextBox ID="txtCode" runat="server" Width="100%" MaxLength="80" ClientInstanceName="ctxtCode" CssClass="upper">
                                                        <ClientSideEvents LostFocus="function(s, e) {
	                                                            UniqueCheck();
                                                            }" />
                                                    </dxe:ASPxTextBox>
                                                    <span id="MandatoryShortName" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; display: none; position: absolute; right: -20px; top: 5px;"
                                                        title="Mandatory"></span>

                                                    <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtCode"
                                                        Display="Dynamic" ErrorMessage="Mandatory."  ForeColor="Red" ValidationGroup="a"></asp:RequiredFieldValidator>--%>
                                                </div>
                                            </div>
                                             <%--rev srijeeta  mantis issue 0024515--%>
                                            <div class="col-md-2 " runat="server" id="Div1">
                                                <label>
                                                    <dxe:ASPxLabel ID="ASPxLabel7" runat="server" Text="Alternative Code" Width="" maxLength="100">
                                                    </dxe:ASPxLabel>
                                                    
                                                </label>

                                                <dxe:ASPxTextBox ID="ASPxTextBox1" runat="server" ClientInstanceName="ctxt_CustDocNoaltcode" Width="100%" MaxLength="100">
                                                </dxe:ASPxTextBox>

                                            </div>
                                            <%--end of rev srijeeta  mantis issue 0024515--%>
                                            <div class="col-md-2" style="display: none;">
                                                <label>
                                                    &nbsp;<dxe:ASPxLabel ID="ASPxLabel10" runat="server" Text="Branch" Width="59px">
                                                    </dxe:ASPxLabel>
                                                </label>
                                                <div>
                                                    <asp:DropDownList ID="cmbBranch" runat="server" Width="100%">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-md-2 hide">
                                                <label>
                                                    <dxe:ASPxLabel ID="ASPxLabel13" runat="server" Text="Source">
                                                    </dxe:ASPxLabel>
                                                </label>
                                                <div>
                                                    <asp:DropDownList ID="cmbSource" runat="server" Width="100%" onchange="ChangeSource()">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <label>
                                                    <dxe:ASPxLabel ID="ASPxLabel17" runat="server" Text="Referred By" Width="90px">
                                                    </dxe:ASPxLabel>
                                                </label>
                                                <div>
                                                    <asp:TextBox ID="txtReferedBy" runat="server" Width="100%" MaxLength="100" Style="display: none"></asp:TextBox>
                                                    <%--  <asp:TextBox ID="txtReferedBy_hidden" runat="server" Visible="False"></asp:TextBox>--%>
                                                    <asp:ListBox ID="lstReferedBy" CssClass="chsn" runat="server" Font-Size="12px" Width="100%" data-placeholder="Select..."></asp:ListBox>

                                                </div>
                                            </div>
                                            <div class="col-md-2 simple-select">
                                                <label>
                                                    <dxe:ASPxLabel ID="ASPxLabel19" runat="server" Text="Status" Width="95px">
                                                    </dxe:ASPxLabel>
                                                </label>
                                                <div>
                                                    <asp:DropDownList ID="cmbContactStatus" runat="server" Width="100%">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-md-2 simple-select">
                                                <label>
                                                    <dxe:ASPxLabel ID="ASPxLabel20" runat="server" Text="Legal Status" Width="110px">
                                                    </dxe:ASPxLabel>
                                                </label>
                                                <div>
                                                    <asp:DropDownList ID="cmbLegalStatus" runat="server" Width="100%">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="clear"></div>
                                            <div class="col-md-3 lblmTop8">
                                                <label style="margin-top: 5px">
                                                    <dxe:ASPxLabel ID="ASPxLabel2" runat="server" Text="Date Of Incorporation">
                                                    </dxe:ASPxLabel>
                                                </label>
                                                <div>
                                                    <%-- <dxe:ASPxDateEdit ID="DateOfIncoorporation" runat="server" DisplayFormatString="yyyy-MM-dd" EditFormatString="yyyy-MM-dd"
                                                        TabIndex="9">
                                                    </dxe:ASPxDateEdit>  --%>
                                                    <dxe:ASPxDateEdit ID="DateOfIncoorporation" runat="server" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                                                        ClientInstanceName="cDateOfIncoorporation" Width="100%">
                                                    </dxe:ASPxDateEdit>
                                                    <%--Rev 1.0--%>
                                                    <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                                                    <%--Rev end 1.0--%>
                                                </div>
                                            </div>

                                            <div class="col-md-3 lblmTop8">
                                                <div class="padBot5 lblmTop8" style="display: block;">
                                                    <span>Main Account</span>
                                                </div>
                                                <div class="Left_Content">
                                                    <asp:ListBox ID="lstTaxRates_MainAccount" CssClass="chsn" runat="server" Font-Size="12px" Width="100%" data-placeholder="Select..." onchange="changeFunc();"></asp:ListBox>
                                                    <asp:HiddenField ID="hndTaxRates_MainAccount_hidden" runat="server" />
                                                </div>
                                            </div>


                                            <div class="col-md-3" id="divBranch" style="display: block;">
                                                <label>
                                                    Branch :<%--<span style="color: Red;">*</span>  --%>
                                                </label>
                                                <div>
                                                    <div>
                                                        <asp:Label ID="lblSelectedBranch" runat="server"></asp:Label></div>

                                                    <dxe:ASPxComboBox ID="cmbMultiBranches" ClientInstanceName="CmbBranch" runat="server" Visible="false"
                                                        ValueType="System.String" DataSourceID="branchdtl" ValueField="branch_id"
                                                        TextField="branch_description" EnableIncrementalFiltering="true"
                                                        Width="90%" AutoPostBack="false">
                                                        <ClientSideEvents SelectedIndexChanged="CmbBranchChanged" Init="CmbBranchChanged" />
                                                    </dxe:ASPxComboBox>
                                                    <input type="button" onclick="MultiBranchClick()" class="btn btn-success btn-xs " value="Select Branch(s)" id="MultiBranchButton"></input>
                                                </div>
                                            </div>


                                            <%-- rev Bapi--%>
                                            <div class="col-md-3" id="divProduct" runat="server" style="display: block;">
                                                <label>
                                                    Preferred Product :                                          
                                                </label>
                                                <div>
                                                    <div>
                                                        <asp:Label ID="lblSelectedProduct" runat="server"></asp:Label></div>

                                                    <dxe:ASPxComboBox ID="cmbMultiProducts" ClientInstanceName="CmbProduct" runat="server" Visible="false"
                                                        ValueType="System.String" DataSourceID="productdtl" ValueField="product_id"
                                                        TextField="product_description" EnableIncrementalFiltering="true"
                                                        Width="90%" AutoPostBack="false">
                                                        <ClientSideEvents SelectedIndexChanged="CmbProductChanged" Init="CmbProductChanged" />
                                                    </dxe:ASPxComboBox>
                                                    <input type="button" onclick="MultiProductClick()" class="btn btn-success btn-xs " value="Select Product(s)" id="MultiProductButton"></input>
                                                </div>
                                            </div>






                                            <div class="col-md-3 hide" id="divtxtTaxRates_SubAccount">
                                                <div class="padBot5" style="display: block; height: auto;">
                                                    Sub Account
                                                </div>
                                                <div class="Left_Content">
                                                    <asp:ListBox ID="lstTaxRates_SubAccount" CssClass="chsn" runat="server" Font-Size="12px" Width="100%" data-placeholder="Select..." onchange="changeSubFunc();"></asp:ListBox>
                                                    <asp:HiddenField ID="hndTaxRates_SubAccount_hidden" runat="server" />
                                                </div>
                                            </div>

                                            <div class="clear"></div>
                                            <%--Debjyoti GSTIN in Vendor--%>
                                            <div id="td_registered" class="labelt col-md-2" runat="server">
                                                <div style="margin-top: 13px;">

                                                    <label>Registered?</label>
                                                    <asp:RadioButtonList runat="server" ID="radioregistercheck" RepeatDirection="Horizontal" Width="130px">
                                                        <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="No" Value="0" Selected="True"></asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </div>
                                            </div>
                                            <div class="col-md-4 lblmTop8">
                                                <label>GSTIN   </label>
                                                <div class="relative">
                                                    <ul class="nestedinput">
                                                        <li>
                                                            <dxe:ASPxTextBox ID="txtGSTIN1" ClientInstanceName="ctxtGSTIN111" MaxLength="2" runat="server" Width="50px">
                                                                <ClientSideEvents KeyPress="function(s,e){ fn_AllowonlyNumeric(s,e);}" />
                                                            </dxe:ASPxTextBox>
                                                        </li>
                                                        <li class="dash">- </li>
                                                        <li>
                                                            <dxe:ASPxTextBox ID="txtGSTIN2" ClientInstanceName="ctxtGSTIN222" MaxLength="10" runat="server" Width="110px">
                                                                <ClientSideEvents KeyUp="Gstin2TextChanged" />
                                                            </dxe:ASPxTextBox>
                                                        </li>
                                                        <li class="dash">- </li>
                                                        <li>
                                                            <dxe:ASPxTextBox ID="txtGSTIN3" ClientInstanceName="ctxtGSTIN333" MaxLength="3" runat="server" Width="50px">
                                                                <ClientSideEvents LostFocus="PannumberautoSet" />
                                                            </dxe:ASPxTextBox>
                                                            <span id="invalidGst" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; display: none; padding-left: 9px; left: 257px" title="Invalid GSTIN"></span>
                                                        </li>
                                                    </ul>
                                                    <a href="#" onclick="validateGSTIN();" style="padding-left: 10px; padding-top: 4px; display: inline-block;">Validate GST</a>
                                                    <input class="hide" id="myInput" />
                                                </div>
                                            </div>
                                            <div class="col-md-2 hide">
                                                <div style="width: 100%;">
                                                    Type:<span style="color: red"> *</span>
                                                </div>
                                                <div>
                                                    <dxe:ASPxComboBox ID="cmbSR" Enabled="false" ClientInstanceName="ccmbSR" runat="server" Width="170px" EnableIncrementalFiltering="True"
                                                        Height="25px" ValueType="System.String" AutoPostBack="false" EnableSynchronization="False">
                                                        <Items>
                                                            <%-- <dxe:ListEditItem Text="select" />--%>
                                                            <dxe:ListEditItem Text="Pan Card" Value="1" Selected="true" />

                                                        </Items>
                                                        <ClientSideEvents SelectedIndexChanged="function (s, e) {fn_ComboIndexChange(s.GetValue());}" />
                                                    </dxe:ASPxComboBox>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <div id="Number" style="width: 100%">
                                                    <label style="margin-top: 7px;">
                                                        PAN
                                                    <a href="#" style="left: -12px; top: 20px;">
                                                        <i id="I1" runat="server" class="fa fa-question" aria-hidden="true" onclick="AboutPanClick()"></i>
                                                    </a>
                                                    </label>
                                                    <div>
                                                        <dxe:ASPxTextBox ID="txtNumber" ClientInstanceName="ctxtNumber" ClientEnabled="true" MaxLength="10"
                                                            runat="server" Height="25px" Width="100%">
                                                            <ClientSideEvents LostFocus="NameFromPan" />
                                                        </dxe:ASPxTextBox>

                                                    </div>
                                                    <dxe:ASPxLabel ID="labelformat" Style="color: #03A9F4; width: 100px; width: 100%; font-size: 11px; font-weight: 600; display: inline-block;" Width="100px" runat="server" ForeColor="#0099FF" CssClass="formatcss" ClientInstanceName="lbformat" Text="Sample : AAAAA9999A"></dxe:ASPxLabel>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                            </div>

                                            <div class="clear"></div>
                                            <div class="col-md-3 visF" id="td_Applicablefrom">
                                                <label class="labelt">
                                                    <dxe:ASPxLabel ID="lbl_Applicablefrom" runat="server" Text="Applicable From">
                                                    </dxe:ASPxLabel>
                                                </label>
                                                <div class="visF">
                                                    <dxe:ASPxDateEdit ID="dt_ApplicableFrom" runat="server" Width="100%" EditFormat="Custom" ClientInstanceName="cApplicableFrom"
                                                        EditFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                                                        <ButtonStyle Width="13px">
                                                        </ButtonStyle>
                                                    </dxe:ASPxDateEdit>

                                                </div>
                                            </div>
                                            <%--Debjyoti GSTIN in Vendor--%>
                                            <div class="col-md-3">
                                                <label>
                                                    <dxe:ASPxLabel ID="ASPxLabel3" runat="server" Text="Name To Print in Cheque" Width="273px">
                                                    </dxe:ASPxLabel>
                                                </label>
                                                <div>
                                                    <asp:TextBox ID="txtNameInCheque" runat="server" Width="100%" MaxLength="200"></asp:TextBox>


                                                </div>
                                            </div>

                                            <div class="col-md-3 simple-select-2">
                                                <label>
                                                    <dxe:ASPxLabel ID="ASPxLabel6" runat="server" Text="Accounts Group">
                                                    </dxe:ASPxLabel>
                                                </label>
                                                <div>
                                                    <asp:DropDownList ID="ddlAssetLiability" runat="server" Width="100%">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="labelt col-md-3" style="margin-top: 12px;">
                                                <div id="DivVendorType" class="visF hide">

                                                    <label>Vendor Type</label>
                                                    <asp:RadioButtonList runat="server" ID="rdl_VendorType" RepeatDirection="Horizontal" Width="210px">
                                                        <asp:ListItem Text="Regular" Value="R"></asp:ListItem>
                                                        <asp:ListItem Text="Composition" Value="C"></asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </div>
                                            </div>

                                            <div class="clear"></div>

                                            <div class="Left_Content col-md-3" id="dvNameAsPerPan" runat="server" style="display: none">
                                                <asp:Label ID="Label3" runat="server" Text="Name as per PAN Card"></asp:Label>
                                                <div>
                                                    <dxe:ASPxTextBox ID="txtNameAsPerPan" ClientInstanceName="ctxtNameAsPerPan" MaxLength="250" HorizontalAlign="Left"
                                                        runat="server" Width="100%">
                                                    </dxe:ASPxTextBox>
                                                </div>
                                            </div>

                                            <div class="col-md-3" id="dvDeducteestat" runat="server" style="display: none">
                                                <asp:Label ID="Label4" runat="server" Text="Deductee Status" CssClass="newLbl"></asp:Label>

                                                <div class="Left_Content">
                                                    <dxe:ASPxComboBox ID="cmbDeducteestat" ClientInstanceName="ccmbDeducteestat" runat="server"
                                                        ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0">
                                                        <Items>
                                                            <dxe:ListEditItem Text="Select" Value="0" Selected="true" />
                                                            <dxe:ListEditItem Text="Company" Value="01" />
                                                            <dxe:ListEditItem Text="Other than Company" Value="02" />


                                                        </Items>
                                                    </dxe:ASPxComboBox>
                                                </div>
                                            </div>
                                            <div class="col-md-3" id="dvTaxDeducteedType" runat="server" style="display: none">
                                                <asp:Label ID="Label1" runat="server" Text="Tax Entity Type" CssClass="newLbl"></asp:Label>

                                                <div class="Left_Content">
                                                    <dxe:ASPxComboBox ID="cmbTaxdeducteedType" ClientInstanceName="ccmbTaxdeducteedType" runat="server"
                                                        ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0">
                                                        <Items>
                                                            <dxe:ListEditItem Selected="true" Text="Select" Value="0" />
                                                            <dxe:ListEditItem Text="Government" Value="G" />
                                                            <dxe:ListEditItem Text="Non-government" Value="NG" />
                                                            <dxe:ListEditItem Text="Others" Value="O" />
                                                        </Items>
                                                    </dxe:ASPxComboBox>
                                                </div>
                                            </div>

                                            <div class="clear"></div>
                                            <div class="col-md-12" style="padding-top: 15px">
                                                <dxe:ASPxButton ID="btnSave" runat="server" Text="Save" ValidationGroup="a"
                                                    OnClick="btnSave_Click" CssClass="btn btn-primary">
                                                    <ClientSideEvents Click="function(s, e) {
                                                                if(document.getElementById('lstReferedBy').value){
                                                                document.getElementById('RefferedByValue').value= document.getElementById('lstReferedBy').value;
                                                                }
	                                                            var valid= validate();
                                                                e.processOnServer = valid;
                                                        ValidatePanno();

                                                        }" />
                                                </dxe:ASPxButton>
                                                <asp:Button ID="btnUdf" runat="server" Text="UDF" CssClass="btn dxbButton btn-cust" OnClientClick="if(OpenUdf()){ return false;}" />

                                                <%-- <asp:Button ID="GstinSettingsButton" runat="server" Text="GSTIN Settings Branchwise"  CssClass="btn btn-primary dxbButton"  OnClientClick="openGstin();return false;"/>--%>
                                                <GSTIN:gstinSettings runat="server" ID="GstinSettingsButton" />
                                            </div>

                                        </div>

                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Text="Contact Person" Name="ContactPreson">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Text="Correspondence" Name="CorresPondence">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="BankDetails" Text="Bank Details">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <%-- <dxe:TabPage Name="DPDetails" Text="DP Details">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>--%>
                            <dxe:TabPage Name="Documents" Text="Documents">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Registration" Text="Registration">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="GroupMember" Text="Group Member">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>

                            <dxe:TabPage Name="TDS" Text="TDS">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>


                        </TabPages>
                        <ClientSideEvents ActiveTabChanged="function(s, e) {
	                                            var activeTab   = page.GetActiveTab();
	                                            var Tab0 = page.GetTab(0);
	                                            var Tab1 = page.GetTab(1);
	                                            var Tab2 = page.GetTab(2);
	                                            var Tab3 = page.GetTab(3);
	                                            var Tab4 = page.GetTab(4);
	                                            var Tab5 = page.GetTab(5);
	                                            var Tab6 = page.GetTab(6);
	                                            var Tab7 = page.GetTab(7);
                                                var Tab8 = page.GetTab(8);
	                                            
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
	                                            else if(activeTab == Tab3)
	                                            {
	                                                disp_prompt('tab3');
	                                            }
	                                            else if(activeTab == Tab4)
	                                            {
	                                                disp_prompt('tab4');
	                                            }
	                                            else if(activeTab == Tab5)
	                                            {
	                                                disp_prompt('tab5');
	                                            }
	                                            else if(activeTab == Tab6)
	                                            {
	                                                disp_prompt('tab6');
	                                            }
	                                            else if(activeTab == Tab7)
	                                            {
	                                                disp_prompt('tab7');
	                                            }
                                                else if(activeTab == Tab8)
	                                            {
	                                                disp_prompt('tab8');
	                                            }
	                                            
	                                            }"></ClientSideEvents>
                        <ContentStyle>
                            <Border BorderColor="#002D96" BorderStyle="Solid" BorderWidth="1px" />
                        </ContentStyle>
                        <LoadingPanelStyle ImageSpacing="6px">
                        </LoadingPanelStyle>
                    </dxe:ASPxPageControl>
                </td>
            </tr>
            <tr>
                <td style="height: 8px">
                    <table style="width: 100%;">
                        <tr>
                            <td align="right" style="width: 843px">
                                <asp:HiddenField ID="hdReferenceBy" runat="server" />
                                <asp:HiddenField ID="refferByDD" runat="server" />
                                <asp:HiddenField ID="RefferedByValue" runat="server" />
                                <asp:HiddenField ID="hdKeyVal_InternalID" runat="server" />
                                <asp:HiddenField ID="HdId" runat="server" />
                                <asp:HiddenField ID="hdIsMainAccountInUse" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>

        <asp:SqlDataSource ID="branchdtl" runat="server"
            SelectCommand="select '0' as branch_id ,  'Select' as branch_description union all   select branch_id,branch_description from tbl_master_branch order by branch_description"></asp:SqlDataSource>

        <asp:SqlDataSource ID="BranchdataSource" runat="server" SelectCommand="select branch_id,branch_code,branch_description from tbl_master_branch"></asp:SqlDataSource>


        <asp:HiddenField ID="hdnBranchAllSelected" runat="server" />


        <asp:SqlDataSource ID="productdtl" runat="server"
            SelectCommand="select '0' as product_id ,  'Select' as product_description  union all select sProducts_ID,sProducts_Name from MASTER_SPRODUCTS"></asp:SqlDataSource>

        <asp:SqlDataSource ID="ProductdataSource" runat="server" SelectCommand="select  sProducts_ID as product_id,sProducts_Code as product_code,sProducts_Name as product_description from MASTER_SPRODUCTS"></asp:SqlDataSource>


        <asp:HiddenField ID="hdnProductAllSelected" runat="server" />









        <asp:HiddenField ID="hdnflag" runat="server" />
    </div>
    </div>
    <dxe:ASPxPopupControl ID="BranchSelectPopup" runat="server" Width="700"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="cBranchSelectPopup"
        HeaderText="Select Branch" AllowResize="false" ResizingMode="Postponed" Modal="true">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">

                <div style="margin-bottom: 10px; margin-top: 10px;">
                    Apply for All Branch &nbsp; 
                     <asp:CheckBox ID="chkAllBranch" runat="server" OnClick="SelectAllBranches(this);" />
                    <asp:Label ID="lblBranch" runat="server" Text="All Branch Selected, No need to select individual Branch" CssClass="vehiclecls"></asp:Label>
                </div>



                <dxe:ASPxGridView ID="branchGrid" runat="server" KeyFieldName="branch_id" AutoGenerateColumns="False" DataSourceID="BranchdataSource"
                    Width="100%" ClientInstanceName="cbranchGrid" OnCustomCallback="branchGrid_CustomCallback"
                    SelectionMode="Multiple" SettingsBehavior-AllowFocusedRow="true">
                    <Columns>

                        <dxe:GridViewCommandColumn ShowSelectCheckbox="true" VisibleIndex="0" Width="60" Caption="Select" />


                        <dxe:GridViewDataTextColumn Caption="Branch Code" FieldName="branch_code"
                            VisibleIndex="1" FixedStyle="Left">
                            <CellStyle CssClass="gridcellleft" Wrap="true">
                            </CellStyle>
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Branch Description" FieldName="branch_description"
                            VisibleIndex="1" FixedStyle="Left">
                            <CellStyle CssClass="gridcellleft" Wrap="true">
                            </CellStyle>
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>





                    </Columns>

                    <SettingsPager NumericButtonCount="20" PageSize="10" ShowSeparators="True" Mode="ShowPager">
                        <FirstPageButton Visible="True">
                        </FirstPageButton>
                        <LastPageButton Visible="True">
                        </LastPageButton>
                    </SettingsPager>
                    <SettingsSearchPanel Visible="True" />
                    <Settings ShowGroupPanel="False" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                    <SettingsLoadingPanel Text="Please Wait..." />
                    <ClientSideEvents EndCallback="branchGridEndCallBack" />
                </dxe:ASPxGridView>
                <br />
                <input type="button" value="Ok" class="btn btn-primary" onclick="SaveSelectedBranch()" />
                <div style="float: right;">
                    <input type="button" runat="server" value="Select All" onclick="selectAll()" />
                    <input type="button" runat="server" value="Deselect All" onclick="unselectAll()" />
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>


    <dxe:ASPxPopupControl ID="ProductSelectPopup" runat="server" Width="1000"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="cProductSelectPopup"
        HeaderText="Select Product" AllowResize="false" ResizingMode="Postponed" Modal="true">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">

                <div style="margin-bottom: 10px; margin-top: 10px; display: none;">
                    Apply for All Product &nbsp; 
                     <asp:CheckBox ID="chkAllProduct" runat="server" OnClick="SelectAllProducts(this);" />
                    <asp:Label ID="lblProduct" runat="server" Text="All Product Selected, No need to select individual Product" CssClass=""></asp:Label>
                </div>



                <dxe:ASPxGridView ID="productGrid" runat="server" KeyFieldName="product_id" AutoGenerateColumns="False" DataSourceID="ProductdataSource"
                    Width="100%" ClientInstanceName="cproductGrid" OnCustomCallback="productGrid_CustomCallback"
                    SelectionMode="Multiple" SettingsBehavior-AllowFocusedRow="true">
                    <Columns>

                        <dxe:GridViewCommandColumn ShowSelectCheckbox="true" VisibleIndex="0" Width="60" Caption="Select" />

                        <dxe:GridViewDataTextColumn Caption="Product Code" FieldName="product_code"
                            VisibleIndex="1" FixedStyle="Left">
                            <CellStyle CssClass="gridcellleft" Wrap="true">
                            </CellStyle>
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Product" FieldName="product_description"
                            VisibleIndex="1" FixedStyle="Left">
                            <CellStyle CssClass="gridcellleft" Wrap="true">
                            </CellStyle>
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>


                    </Columns>

                    <SettingsPager NumericButtonCount="20" PageSize="10" ShowSeparators="True" Mode="ShowPager">
                        <FirstPageButton Visible="True">
                        </FirstPageButton>
                        <LastPageButton Visible="True">
                        </LastPageButton>
                    </SettingsPager>
                    <SettingsSearchPanel Visible="True" />
                    <Settings ShowGroupPanel="False" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                    <SettingsLoadingPanel Text="Please Wait..." />
                    <ClientSideEvents EndCallback="productGridEndCallBack" />
                </dxe:ASPxGridView>
                <br />
                <input type="button" value="Ok" class="btn btn-primary" onclick="SaveSelectedProduct()" />
                <div style="float: right;">
                    <input type="button" runat="server" value="Select All" onclick="selectAllProduct()" />
                    <input type="button" runat="server" value="Deselect All" onclick="unselectAllProduct()" />
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>

    <script>
        function AboutPanClick() {
            $('#PanModel').modal('show');
        }
    </script>
    <%-- Pan No Details --%>
    <div class="modal fade pmsModal w30" id="PanModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">PAN</h4>
                </div>
                <div class="modal-body">
                    <div id="SalesManTable">
                        <font color="red">
                               1.First three characters are alphabetic series running from AAA to ZZZ<br />
                               2.Fourth character of PAN represents the status of the PAN holder.C — Company P — Person H — HUF(Hindu Undivided Family) <br />
                               3.F — Firm A — Association of Persons (AOP) T — AOP (Trust) B — Body of Individuals (BOI) L — Local Authority J — Artificial Juridical Person G — Government<br />
                               4.Fifth character represents the first character of the PAN holder’s last name/surname.<br />
                               5.Next four characters are sequential number running from 0001 to 9999.<br />
                               6.Last character in the PAN is an alphabetic check digit.<br />
                    </font>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>

        </div>
    </div>
    <%-- End Pan No Details --%>
</asp:Content>
