<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                15-03-2023        2.0.36           Pallab              25733 : Master pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/Erp.Master" AutoEventWireup="True"
    Inherits="ERP.OMS.Management.Master.management_Master_Contact_general" CodeBehind="Contact_general.aspx.cs" EnableEventValidation="false" %>

<%@ OutputCache Duration='5' VaryByParam="ID" %>
<%@ Register Src="~/OMS/Management/Master/UserControls/GSTINSettings.ascx" TagPrefix="GSTIN" TagName="gstinSettings" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/assests/js/init.js"></script>
    <script type="text/javascript" src="/assests/js/ajax-dynamic-list.js"></script>
    <script src="/assests/pluggins/choosen/choosen.min.js"></script>
    <script src="/assests/pluggins/bootstrap-multiselect/bootstrap-multiselect.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {

            // if ($("#hdnDocumentSegmentActive").val() == "1") {

            var SegmentMAPID = $("#hdnSegmentMAPID").val();
            $.ajax({
                url: "Contact_general.aspx/PopulateAllModule",
                contentType: "application/json; charset=utf-8",
                datatype: "JSON",
                type: "POST",
                data: JSON.stringify({ SegmentMAPID: SegmentMAPID, SegmnetNo: '1' }),
                success: function (data) {

                    $("#cddlSegmentMandatory1").empty();
                    var grpdetl = data.d;

                    var opts = "";
                    for (i in grpdetl) {
                        if (grpdetl[i].IsChecked == "True") {
                            opts += "<option selected  value='" + grpdetl[i].Module_Id + "'>" + grpdetl[i].Module_Name + "</option>";
                        }
                        else {
                            opts += "<option   value='" + grpdetl[i].Module_Id + "'>" + grpdetl[i].Module_Name + "</option>";
                        }

                    }

                    $("#cddlSegmentMandatory1").empty().append(opts);


                    setTimeout(function () {
                        //$('#cddlSegmentMandatory1, #cddlSegmentMandatory2, #cddlSegmentMandatory3, #cddlSegmentMandatory4, #cddlSegmentMandatory5').multiselect('rebuild');
                        $('#cddlSegmentMandatory1').multiselect('rebuild');

                    }, 200)
                },
                error: function (data) {

                    jAlert("Please try again later");
                }
            });

            $.ajax({

                url: "Contact_general.aspx/PopulateAllModule",
                contentType: "application/json; charset=utf-8",
                datatype: "JSON",
                type: "POST",
                data: JSON.stringify({ SegmentMAPID: SegmentMAPID, SegmnetNo: '2' }),
                success: function (data) {

                    $("#cddlSegmentMandatory2").empty();
                    var grpdetl = data.d;
                    var opts = "";
                    for (i in grpdetl) {
                        if (grpdetl[i].IsChecked == "True") {
                            opts += "<option selected  value='" + grpdetl[i].Module_Id + "'>" + grpdetl[i].Module_Name + "</option>";
                        }
                        else {
                            opts += "<option   value='" + grpdetl[i].Module_Id + "'>" + grpdetl[i].Module_Name + "</option>";
                        }

                    }

                    $("#cddlSegmentMandatory2").empty().append(opts);

                    setTimeout(function () {
                        $('#cddlSegmentMandatory2').multiselect('rebuild');
                    }, 200)
                },
                error: function (data) {

                    jAlert("Please try again later");
                }
            });


            $.ajax({

                url: "Contact_general.aspx/PopulateAllModule",
                contentType: "application/json; charset=utf-8",
                datatype: "JSON",
                type: "POST",
                data: JSON.stringify({ SegmentMAPID: SegmentMAPID, SegmnetNo: '3' }),
                success: function (data) {
                    $("#cddlSegmentMandatory3").empty();
                    var grpdetl = data.d;
                    var opts = "";
                    for (i in grpdetl) {
                        if (grpdetl[i].IsChecked == "True") {
                            opts += "<option selected  value='" + grpdetl[i].Module_Id + "'>" + grpdetl[i].Module_Name + "</option>";
                        }
                        else {
                            opts += "<option   value='" + grpdetl[i].Module_Id + "'>" + grpdetl[i].Module_Name + "</option>";
                        }
                    }

                    $("#cddlSegmentMandatory3").empty().append(opts);
                    setTimeout(function () {
                        $('#cddlSegmentMandatory3').multiselect('rebuild');
                    }, 200)
                },
                error: function (data) {

                    jAlert("Please try again later");
                }
            });


            $.ajax({

                url: "Contact_general.aspx/PopulateAllModule",
                contentType: "application/json; charset=utf-8",
                datatype: "JSON",
                type: "POST",
                data: JSON.stringify({ SegmentMAPID: SegmentMAPID, SegmnetNo: '4' }),
                success: function (data) {

                    $("#cddlSegmentMandatory4").empty();
                    var grpdetl = data.d;
                    var opts = "";
                    for (i in grpdetl) {
                        if (grpdetl[i].IsChecked == "True") {
                            opts += "<option selected  value='" + grpdetl[i].Module_Id + "'>" + grpdetl[i].Module_Name + "</option>";
                        }
                        else {
                            opts += "<option   value='" + grpdetl[i].Module_Id + "'>" + grpdetl[i].Module_Name + "</option>";
                        }
                    }
                    $("#cddlSegmentMandatory4").empty().append(opts);
                    setTimeout(function () {
                        $('#cddlSegmentMandatory4').multiselect('rebuild');

                    }, 200)
                },
                error: function (data) {

                    jAlert("Please try again later");
                }
            });


            $.ajax({

                url: "Contact_general.aspx/PopulateAllModule",
                contentType: "application/json; charset=utf-8",
                datatype: "JSON",
                type: "POST",
                data: JSON.stringify({ SegmentMAPID: SegmentMAPID, SegmnetNo: '5' }),
                success: function (data) {
                    $("#cddlSegmentMandatory5").empty();
                    var grpdetl = data.d;

                    var opts = "";
                    for (i in grpdetl) {
                        if (grpdetl[i].IsChecked == "True") {
                            opts += "<option selected  value='" + grpdetl[i].Module_Id + "'>" + grpdetl[i].Module_Name + "</option>";
                        }
                        else {
                            opts += "<option   value='" + grpdetl[i].Module_Id + "'>" + grpdetl[i].Module_Name + "</option>";
                        }

                    }

                    //$("#cddlSegmentMandatory1").empty().append(opts);

                    //$("#cddlSegmentMandatory2").empty().append(opts);
                    //$("#cddlSegmentMandatory3").empty().append(opts);
                    //$("#cddlSegmentMandatory4").empty().append(opts);
                    $("#cddlSegmentMandatory5").empty().append(opts);
                    setTimeout(function () {
                        //$('#cddlSegmentMandatory1, #cddlSegmentMandatory2, #cddlSegmentMandatory3, #cddlSegmentMandatory4, #cddlSegmentMandatory5').multiselect('rebuild');
                        //$('#cddlSegmentMandatory1').multiselect('rebuild');
                        //$('#cddlSegmentMandatory2').multiselect('rebuild');
                        //$('#cddlSegmentMandatory3').multiselect('rebuild');
                        //$('#cddlSegmentMandatory4').multiselect('rebuild');
                        $('#cddlSegmentMandatory5').multiselect('rebuild');
                    }, 200)
                },
                error: function (data) {

                    jAlert("Please try again later");
                }
            });





            //  }
            $('#cddlSegmentMandatory1').multiselect({
                includeSelectAllOption: true,
                buttonWidth: '100%',
                enableFiltering: true,
                maxHeight: 200,
                //dropUp: true,
                enableCaseInsensitiveFiltering: true,
                //onDropdownHide: function (event) {
                //    //console.log(event)
                //},
                onChange: function () {
                    var selected = this.$select.val();
                    $('#hdnSegmentMandatory1').val(selected);

                    // console.log("selected", selected);
                },
                buttonText: function (options, select) {
                    if (options.length === 0) {
                        return 'No option selected ...';
                    }
                    else if (options.length > 2) {
                        return 'More than 2 options selected!';
                    } else {
                        var labels = [];
                        options.each(function () {
                            if ($(this).attr('label') !== undefined) {
                                labels.push($(this).attr('label'));
                            }
                            else {
                                labels.push($(this).html());
                            }
                        });
                        return labels.join(', ') + '';
                    }
                }
            }).multiselect('selectAll', false).multiselect('updateButtonText');


            $('#cddlSegmentMandatory2').multiselect({
                includeSelectAllOption: true,
                buttonWidth: '100%',
                enableFiltering: true,
                maxHeight: 200,
                //dropUp: true,
                enableCaseInsensitiveFiltering: true,
                //onDropdownHide: function (event) {
                //    //console.log(event)
                //},
                onChange: function () {
                    var selected = this.$select.val();
                    // ...
                    $('#hdnSegmentMandatory2').val(selected);

                    console.log("selected", selected);
                },
                buttonText: function (options, select) {
                    if (options.length === 0) {
                        return 'No option selected ...';
                    }
                    else if (options.length > 2) {
                        return 'More than 2 options selected!';
                    } else {
                        var labels = [];
                        options.each(function () {
                            if ($(this).attr('label') !== undefined) {
                                labels.push($(this).attr('label'));
                            }
                            else {
                                labels.push($(this).html());
                            }
                        });
                        return labels.join(', ') + '';
                    }
                }
            }).multiselect('selectAll', false).multiselect('updateButtonText');

            $('#cddlSegmentMandatory3').multiselect({
                includeSelectAllOption: true,
                buttonWidth: '100%',
                enableFiltering: true,
                maxHeight: 200,
                //dropUp: true,
                enableCaseInsensitiveFiltering: true,
                //onDropdownHide: function (event) {
                //    //console.log(event)
                //},
                onChange: function () {
                    var selected = this.$select.val();
                    // ...
                    $('#hdnSegmentMandatory3').val(selected);

                    console.log("selected", selected);
                },
                buttonText: function (options, select) {
                    if (options.length === 0) {
                        return 'No option selected ...';
                    }
                    else if (options.length > 2) {
                        return 'More than 2 options selected!';
                    } else {
                        var labels = [];
                        options.each(function () {
                            if ($(this).attr('label') !== undefined) {
                                labels.push($(this).attr('label'));
                            }
                            else {
                                labels.push($(this).html());
                            }
                        });
                        return labels.join(', ') + '';
                    }
                }
            }).multiselect('selectAll', false).multiselect('updateButtonText');

            $('#cddlSegmentMandatory4').multiselect({
                includeSelectAllOption: true,
                buttonWidth: '100%',
                enableFiltering: true,
                maxHeight: 200,
                enableCaseInsensitiveFiltering: true,
                onChange: function () {
                    var selected = this.$select.val();
                    $('#hdnSegmentMandatory4').val(selected);
                    console.log("selected", selected);
                },
                buttonText: function (options, select) {
                    if (options.length === 0) {
                        return 'No option selected ...';
                    }
                    else if (options.length > 2) {
                        return 'More than 2 options selected!';
                    } else {
                        var labels = [];
                        options.each(function () {
                            if ($(this).attr('label') !== undefined) {
                                labels.push($(this).attr('label'));
                            }
                            else {
                                labels.push($(this).html());
                            }
                        });
                        return labels.join(', ') + '';
                    }
                }
            }).multiselect('selectAll', false).multiselect('updateButtonText');

            $('#cddlSegmentMandatory5').multiselect({
                includeSelectAllOption: true,
                buttonWidth: '100%',
                enableFiltering: true,
                maxHeight: 200,
                enableCaseInsensitiveFiltering: true,
                onChange: function () {
                    var selected = this.$select.val();
                    $('#hdnSegmentMandatory5').val(selected);
                    console.log("selected", selected);
                },
                buttonText: function (options, select) {
                    if (options.length === 0) {
                        return 'No option selected ...';
                    }
                    else if (options.length > 2) {
                        return 'More than 2 options selected!';
                    } else {
                        var labels = [];
                        options.each(function () {
                            if ($(this).attr('label') !== undefined) {
                                labels.push($(this).attr('label'));
                            }
                            else {
                                labels.push($(this).html());
                            }
                        });
                        return labels.join(', ') + '';
                    }
                }
            }).multiselect('selectAll', false).multiselect('updateButtonText');
        })
        function CheckSegZero() {
            var Segment1 = ctxtSegment1.GetValue();
            if (Segment1 == "0") {
                ctxtSegment1.Focus();
                ctxtSegment2.SetValue("0");
                alert("Zero Value is not allowed for Segment1");
                return false;
            }
        }
        function CheckSegZero1() {
            var Segment2 = ctxtSegment2.GetValue();
            if (Segment2 == "0") {
                ctxtSegment2.Focus();
                ctxtSegment3.SetValue("0");
                alert("Zero Value is not allowed for Segment2");
                return false;
            }
        }
        function CheckSegZero2() {
            var Segment3 = ctxtSegment3.GetValue();
            if (Segment3 == "0") {
                ctxtSegment3.Focus();
                ctxtSegment4.SetValue("0");
                alert("Zero Value is not allowed for Segment3");
                return false;
            }
        }
        function CheckSegZero3() {
            var Segment4 = $("#txtSegment4").val();
            if (Segment4 == "0") {
                ctxtSegment4.Focus();
                ctxtSegment5.SetValue("0");
                alert("Zero Value is not allowed for Segment4");
                return false;
            }
        }

        function DateValidateBirth() {
            var DOB = new Date(ctxtDOB.GetDate());
            var monthnumber = DOB.getMonth();
            var monthday = DOB.getDate();
            var year = DOB.getYear();

            var SelectedDateValue = new Date(year, monthnumber, monthday);
            var Anniversary = new Date(txtAnniversary.GetDate());
            var monthnumber = Anniversary.getMonth();
            var monthday = Anniversary.getDate();
            var year = Anniversary.getYear();
            var SelectedAnniversary = new Date(year, monthnumber, monthday);

            if (DOB != "") {
                if (Anniversary != "") {
                    if (SelectedDateValue.getTime() >= SelectedAnniversary.getTime()) {
                        txtAnniversary.SetText("");
                        txtAnniversary.Focus();
                    }
                }
            }
        }
        function DateValidateAnivarsary() {
            if (txtAnniversary.GetDate()) {
                if (txtAnniversary.GetDate() > ctxtDOB.GetDate()) {
                    txtAnniversary.SetValue("");
                }
            }
        }




        function InvalidUDF() {
            jAlert("Udf is mandatory.", "Alert", function () { OpenUdf(); });

            //Chinmoy edited for version 2.0.5 start


            if (($("#hdnAutoNumStg").val() == "LDAutoNum1") && ($("#hdnTransactionType").val() == "LD")) {
                LDctxt_CustDocNo.SetText($("#hddnDocNo").val());
                if ($("#hddnDocNo").val() == "Auto") {
                    LDctxt_CustDocNo.SetEnabled(false);
                }
            }
            else if (($("#hdnAutoNumStg").val() == "1") || ($("#hdnAutoNumStg").val() == "AGAutoNum1") || ($("#hdnAutoNumStg").val() == "TRAutoNum1") || ($("#hdnAutoNumStg").val() == "RAAutoNum1")) {
                ctxt_CustDocNo.SetText($("#hddnDocNo").val());
                if ($("#hddnDocNo").val() == "Auto") {
                    ctxt_CustDocNo.SetEnabled(false);
                }
            }


            //End

        }

        function UniqueLeadCodeCheck() {

            var SchemeVal = $("#LDddl_numberingScheme").val();

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

                if (($("#hdnAutoNumStg").val() == "LDAutoNum1") && ($("#hdnTransactionType").val() == "LD")) {
                    uccName = LDctxt_CustDocNo.GetText();
                    Type = "MasterLead";
                }

                $.ajax({
                    type: "POST",
                    url: "Contact_general.aspx/CheckUniqueNumberingCode",
                    data: JSON.stringify({ uccName: uccName, Type: Type }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        CheckUniqueCode = msg.d;
                        if (CheckUniqueCode == true) {
                            alert('Please enter unique No.');
                            //jAlert('Please enter unique Sales Order No');
                            LDctxt_CustDocNo.SetValue('');
                            LDctxt_CustDocNo.Focus();
                        }

                    }

                });
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
                if (($("#hdnAutoNumStg").val() == "1") && ($("#hdnTransactionType").val() == "CL")) {
                    uccName = ctxt_CustDocNo.GetText();
                    Type = "Mastercustomerclient";
                }
                if (($("#hdnAutoNumStg").val() == "AGAutoNum1") && ($("#hdnTransactionType").val() == "AG")) {
                    uccName = ctxt_CustDocNo.GetText();
                    Type = "MasterSalesMen";
                }
                if (($("#hdnAutoNumStg").val() == "TRAutoNum1") && ($("#hdnTransactionType").val() == "TR")) {
                    uccName = ctxt_CustDocNo.GetText();
                    Type = "MasterTransPorter";
                }
                if (($("#hdnAutoNumStg").val() == "RAAutoNum1") && ($("#hdnTransactionType").val() == "RA")) {
                    uccName = ctxt_CustDocNo.GetText();
                    Type = "MasterInfluencer";
                }

                $.ajax({
                    type: "POST",
                    url: "Contact_general.aspx/CheckUniqueNumberingCode",
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

        function SendSMSChk() {
            var remember = document.getElementById('chksendSMS');
            if (remember.checked) {
                // if (document.getElementById("chksendSMS").checked == true) {
                $('#txtSMSPhnNo').removeClass('hidden');
                $('#chksendUniqueDiv').removeClass('hidden');

                //ctxtSMSPhnNo.removeClass('hidden');
                ctxtSMSPhnNo.Focus();
                ctxtSMSPhnNo.SetText('');
                // $('#txtSMSPhnNo').val('');
            }
            else {
                $('#txtSMSPhnNo').addClass('hidden');
                $('#chksendUniqueDiv').addClass('hidden');
                ctxtSMSPhnNo.SetText('');
            }
        }

        function SendUniqueChk() {
            if ($("#hdnAutoNumStg").val() != "LDAutoNum1") {
                var remember = document.getElementById('chksendUnique');
                if (remember.checked) {
                    //  $('#txtSMSPhnNo').focus();
                    ctxtSMSPhnNo.Focus();
                    var values = $('#LDtxtClentUcc').val();
                    //$('#txtSMSPhnNo').val(values);
                    ctxtSMSPhnNo.SetText(values);
                }
                else {
                    ctxtSMSPhnNo.Focus();
                    ctxtSMSPhnNo.SetText('');
                    //$('#txtSMSPhnNo').focus();
                    //$('#txtSMSPhnNo').val('');
                }
            }
        }

        function CheckContactStatus(Cid) {




            var contactType = '<%= Session["requesttype"] %>'; // to hide following alert box for saleman agents
            if (contactType != 'Salesman/Agents') {
                var chkid = Cid.GetValue();
                $.ajax({
                    type: "POST",
                    data: JSON.stringify({ Cid: chkid }),
                    url: 'Contact_general.aspx/CheckContactStatus',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var list = msg.d;

                        if (list == 1) {
                            jAlert('To active this customer From Dormant You must enter Billing address<br/> details: Address, Phone, Country, State, City, PIN Code.');
                            return false;
                        }


                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                    }
                });
            }

        }

        function registeredCheckChangeEvent() {

            var contype = '<%= Session["Contactrequesttype"] %>';
            if (contype != 'customer') {
                if ($("#radioregistercheck").find(":checked").val() == '1') {

                    $("#divGSTIN").show();
                }
                else {
                    $("#divGSTIN").hide();
                    ctxtGSTIN111.SetText('');
                    ctxtGSTIN222.SetText('');
                    ctxtGSTIN333.SetText('');

                }
            }
            if ($("#radioregistercheck").find(":checked").val() == '1') {
                ccmbTransCategory.SetValue("B2B");
            }
            else {
                ccmbTransCategory.SetValue("B2C");
            }
        }

        function changeFunc() {
            if (document.getElementById('hdIsMainAccountInUse').value == "IsInUse") {
                jAlert("Transaction exists for the selected Ledger. Cannot proceed.");
                ChangeselectedMainActvalue();
            } else {
                var MainAccount_val2 = document.getElementById("lstTaxRates_MainAccount").value;
                document.getElementById("hndTaxRates_MainAccount_hidden").value = document.getElementById("lstTaxRates_MainAccount").value;
            }
        }

        function ChangeselectedMainActvalue() {
            var lstTaxRates_MainAccount = document.getElementById("lstTaxRates_MainAccount");
            if (lstTaxRates_MainAccount != null) {
                if (document.getElementById("hndTaxRates_MainAccount_hidden").value != '') {
                    for (var i = 0; i < lstTaxRates_MainAccount.options.length; i++) {
                        if (lstTaxRates_MainAccount.options[i].value == document.getElementById("hndTaxRates_MainAccount_hidden").value) {
                            lstTaxRates_MainAccount.options[i].selected = true;
                        }
                    }
                    $('#lstTaxRates_MainAccount').trigger("chosen:updated");
                }
            }
        }
        function ChangeSourceMainAccount() {
            var fname = "%";
            var lReportTo = $('select[id$=lstTaxRates_MainAccount]');
            lReportTo.empty();

            $.ajax({
                type: "POST",
                url: "Contact_general.aspx/GetMainAccountList",
                data: JSON.stringify({ reqStr: fname }),
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

                            $('#lstTaxRates_MainAccount').append($('<option>').text(name).val(id));
                        }

                        $(lReportTo).append(listItems.join(''));

                        $('#lstTaxRates_MainAccount').trigger("chosen:updated");
                        ChangeselectedMainActvalue();
                    }
                    else {
                        $('#lstTaxRates_MainAccount').trigger("chosen:updated");
                    }
                }
            });
        }

        function RemoveSegDisabled(textbox) {
            var value = parseFloat($("#txtSeg" + textbox).val());
            if (value < 0) {
                $("#txtSeg" + textbox).val('0');
                alert("Negative Value is not allowed");
                return false;
            }
            if (value == 0) {
                alert("Zero Value is not allowed");
                return false;
            }

            var no = parseFloat(textbox) - parseFloat(1);
            if ($("#txtSeg" + no).val() != 0) {
                if ($("#txtSeg" + no).val() <= 0) {
                    alert("Negative Value is not allowed");
                    $("#txtSeg" + no).val('0');
                    return false;
                }
                //$("#txtSeg" + textbox).prop("disabled", false);
            }
            else {
                alert("Zero Value is not allowed");
                return false;
            }




        }

        function RemoveSegDisabledEdit(textbox) {
            if (textbox != 0) {
                if ($("#txtSeg" + textbox).val() != 0) {
                    var id = textbox - 1;
                    $("#txtSeg" + id).prop("disabled", false);
                }
                else {
                    var id = textbox - 1;
                    $("#txtSeg" + id).prop("disabled", true);
                }

            }


        }

        function GetDocumentSegment() {
            $.ajax({
                type: "POST",
                url: "Contact_general.aspx/GetDocumentSegment",
                //data: JSON.stringify({ reqStr: fname }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var list = msg.d;
                    var listItems = [];
                    if (list.length > 0) {

                        var table = document.getElementById("TblDocumentSegment");



                        var row = table.insertRow(0);
                        var cell1 = row.insertCell(0);
                        cell1.innerHTML = "<label >Size</label> ";
                        for (var i = 0; i < list.length; i++) {
                            var id = '';
                            var name = '';
                            id = list[i].split('|')[0];
                            name = list[i].split('|')[1];

                            var cell2 = row.insertCell(1);

                            //if(list.length!=id)
                            //{
                            //    var segid = parseFloat(id) + parseFloat(1);
                            //    cell2.innerHTML = "<input type='number' id=txtSeg" + id + " maxlength = '2' class='upper' value='0' Onblur='RemoveSegDisabled(" + segid + ")' />";

                            //}
                            //else
                            //{
                            //    cell2.innerHTML = "<input type='number' id=txtSeg" + id + " maxlength = '2' class='upper' value='0' Onblur='RemoveSegDisabled(" + id + ")' />";

                            //}


                            cell2.innerHTML = "<input type='number' placeholder='0' id=txtSeg" + id + " maxlength = '2'   Onblur='RemoveSegDisabled(" + id + ")' />";


                            //if (('txtSeg'+ id)!= "txtSeg1")
                            //{
                            //    $('#txtSeg' + id).attr('disabled', 'disabled');
                            //}
                        }

                        var row = table.insertRow(0);
                        var cell1 = row.insertCell(0);
                        cell1.innerHTML = "<label >Segment</label>";
                        for (var i = 0; i < list.length; i++) {
                            var id = '';
                            var name = '';
                            id = list[i].split('|')[0];
                            name = list[i].split('|')[1];
                            var cell2 = row.insertCell(1);
                            cell2.innerHTML = "<label >" + name + "</label>";

                        }

                    }
                    else {
                        // $('#lstTaxRates_MainAccount').trigger("chosen:updated");
                    }
                }
            });
        }
        $(document).ready(function () {

            var mod = '<%= Session["Contactrequesttype"] %>';
            if (mod == 'customer') {
                document.getElementById("lnkClose").href = 'CustomerMasterList.aspx';
            }
            else if (mod == 'Transporter') {
                document.getElementById("lnkClose").href = 'TransporterMasterList.aspx?requesttype=<%= Session["Contactrequesttype"] %>';
            }
            else {
                document.getElementById("lnkClose").href = 'frmContactMain.aspx?requesttype=<%= Session["Contactrequesttype"] %>';
            }



            var contype = '<%= Session["Contactrequesttype"] %>';

            if (contype == 'customer') {
                var Mode = document.getElementById('KeyVal_InternalID').value;
                if (Mode == "Add") {
                    // GetDocumentSegment();
                }
            }

            if (contype == 'customer' || contype == 'Transporter') {

                if ($("#radioregistercheck").find(":checked").val() == '1') {
                    $("#spanmandategstn").attr('style', 'display:inline;color:red');
                    // alert(1);
                    ctxtGSTIN111.SetEnabled(true);
                    ctxtGSTIN222.SetEnabled(true);
                    ctxtGSTIN333.SetEnabled(true);
                }
                else {
                    // alert(2);
                    ctxtGSTIN111.SetText('');
                    ctxtGSTIN222.SetText('');
                    ctxtGSTIN333.SetText('');

                    ctxtGSTIN111.SetEnabled(false);
                    ctxtGSTIN222.SetEnabled(false);
                    ctxtGSTIN333.SetEnabled(false);
                    $("#spanmandategstn").attr('style', 'display:none;');
                }
            }


            ChangeAssociatedEMPSource();



            function ChangeAssociatedEMPSource() {
                var fname = "%";
                var lAssociatedEmployee = $('select[id$=lstAssociatedEmployee]');
                lAssociatedEmployee.empty();

                $.ajax({
                    type: "POST",
                    url: "Contact_general.aspx/ALLEmployee",
                    data: JSON.stringify({ reqStr: fname }),
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

                                $('#lstAssociatedEmployee').append($('<option>').text(name).val(id));

                            }

                            $(lAssociatedEmployee).append(listItems.join(''));

                            lstAssociatedEmployee();
                            $('#lstAssociatedEmployee').trigger("chosen:updated");

                            ChangeselectedEmployeevalue();

                            if ($("#hdnLoginUserSalesmanAgentsInternalId").val() != "") {
                                $('#lstAssociatedEmployee').val($("#hdnLoginUserSalesmanAgentsInternalId").val());
                                $('#lstAssociatedEmployee').trigger("chosen:updated");
                            }

                        }
                        else {
                            $('#lstAssociatedEmployee').trigger("chosen:updated");

                        }
                    }
                });
            }
            function lstAssociatedEmployee() {

                $('#lstAssociatedEmployee').fadeIn();

            }

            function ChangeselectedEmployeevalue() {
                var lstAssociatedEmployee = document.getElementById("lstAssociatedEmployee");

                if (document.getElementById("hidAssociatedEmp").value != '') {
                    for (var i = 0; i < lstAssociatedEmployee.options.length; i++) {
                        if (lstAssociatedEmployee.options[i].value == document.getElementById("hidAssociatedEmp").value) {
                            lstAssociatedEmployee.options[i].selected = true;
                        }
                    }
                    $('#lstAssociatedEmployee').trigger("chosen:updated");
                }

            }


            $("#lstAssociatedEmployee").chosen().change(function () {
                var empcode = $(this).val();
                document.getElementById('hidAssociatedEmp').value = empcode;
                $.ajax({
                    type: "POST",
                    url: "Contact_general.aspx/EmployeeName",
                    data: JSON.stringify({ Empcode: empcode }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var list = msg.d;
                        var listItems = [];
                        if (list.length > 0) {


                            var empcode = '';
                            var name = '';
                            empcode = list[0].split('|')[1];
                            name = list[0].split('|')[0];

                            // txtClentUcc.SetText(empcode);
                            ctxtFirstNmae.SetText(name);

                        }

                    }
                });



            });


            function SetDocMaxLength(length) {
                $.ajax({
                    type: "POST",
                    url: "Contact_general.aspx/GetschemeLength",
                    data: JSON.stringify({ length: length }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (msg) {



                    }
                });
            }

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
                    schemeLength = 50;
                    ctxt_CustDocNo.SetText("");
                    ctxt_CustDocNo.SetEnabled(true);
                    $('#txt_CustDocNo input').attr('maxlength', schemeLength);
                }
                else if ($('#ddl_numberingScheme').val() == "0") {
                    ctxt_CustDocNo.SetText("");
                    ctxt_CustDocNo.SetEnabled(true);
                }

            });

            $('#LDddl_numberingScheme').change(function () {
                //

                var NoSchemeTypedtl = $(this).val();
                var NoSchemeId = NoSchemeTypedtl.toString().split('~')[0];
                var NoSchemeType = NoSchemeTypedtl.toString().split('~')[1];
                $("#hdnNumberingId").val(NoSchemeId);
                var schemeLength = NoSchemeTypedtl.toString().split('~')[2];
                if (NoSchemeType == '1') {
                    LDctxt_CustDocNo.SetText('Auto');
                    LDctxt_CustDocNo.SetEnabled(false);

                    $('#LDtxt_CustDocNo input').attr('maxlength', schemeLength);
                    $("#hddnDocNo").val('Auto');

                }
                else if (NoSchemeType == '0') {
                    schemeLength = 50;
                    LDctxt_CustDocNo.SetText("");
                    LDctxt_CustDocNo.SetEnabled(true);
                    $('#LDtxt_CustDocNo input').attr('maxlength', schemeLength);
                }
                else if ($('#LDddl_numberingScheme').val() == "0") {
                    LDctxt_CustDocNo.SetText("");
                    LDctxt_CustDocNo.SetEnabled(true);
                }

            });

        });



        //Debjyoti GstIN for Customer
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

            if ($("#<%=hddnApplicationMode.ClientID%>").val() === "E") {

                td_Applicablefrom.style.display = "block";
            }
            else {
                td_Applicablefrom.style.display = "block";
            }
        }

        function Gstin2TextChanged(s, e) {

            if (!e.htmlEvent.ctrlKey) {
                if (e.htmlEvent.key != 'Control') {
                    s.SetText(s.GetText().toUpperCase());
                }
            }

            if ($("#<%=hddnApplicationMode.ClientID%>").val() === "E") {

                td_Applicablefrom.style.display = "block";
            }
            else {
                td_Applicablefrom.style.display = "block";
            }



        }
        function Gstin3TextChanged(s, e) {

            if ($("#<%=hddnApplicationMode.ClientID%>").val() === "E") {

                td_Applicablefrom.style.display = "block";
            }
            else {
                td_Applicablefrom.style.display = "block";
            }
        }

        //End Here
        function AddNewexecutive() {


            var table = document.getElementById("executiveTable");
            var row = table.insertRow(0);
            var cell1 = row.insertCell(0);
            var cell2 = row.insertCell(1);
            cell1.innerHTML = "<input type='text'  maxlength = '18' class='upper' />";
            cell2.innerHTML = "<button type='button' value='' onclick='AddNewexecutive()' class='btn btn-primary btn-xs'><i class='fa fa-plus-circle'></i></button> <button type='button' value='' class='btn btn-danger btn-xs' onclick='removeExecutive(this.parentNode.parentNode)'><i class='fa fa-times-circle'></i></button>";

        }
        function removeExecutive(obj) {
            var rowIndex = obj.rowIndex;
            var table = document.getElementById("executiveTable");
            if (table.rows.length > 1) {
                table.deleteRow(rowIndex);
            } else {
                jAlert('Cannot delete all Vechile.');
            }
        }

        function SetVechileNo() {

            var flag = true;
            var table = document.getElementById("executiveTable");
            document.getElementById('VehicleNo_hidden').value = '';
            var data = '';
            if ('<%= Session["Contactrequesttype"] %>' == 'Transporter') {


                for (var i = 0, row; row = table.rows[i]; i++) {
                    for (var j = 0, col; col = row.cells[j]; j++) {
                        if (col.children[0].type != 'button') {
                            if (data == '') {
                                if (col.children[0].value == '') {

                                    //jAlert("Vehicle number required");                                    
                                    //flag = false;
                                }
                                else {

                                    data = col.children[0].value;
                                }
                            }
                            else {
                                if (col.children[0].value == '') {

                                    jAlert("Required");
                                    //  return false;
                                    flag = false;
                                }
                                else {
                                    //alert('4');
                                    data = data + '~' + col.children[0].value;
                                }
                            }
                        }
                    }

                    if (document.getElementById('VehicleNo_hidden').value == '') {
                        document.getElementById('VehicleNo_hidden').value = data.toUpperCase();
                        data = '';
                    }
                    else {
                        document.getElementById('VehicleNo_hidden').value = document.getElementById('VehicleNo_hidden').value + ',' + data.toUpperCase();
                        data = '';
                    }
                }


            }
            //  alert(document.getElementById('VehicleNo_hidden').value);

            return flag;
        }

        $(document).ready(function () {
            $('#cmbLegalStatus').change();

            //td_Applicablefrom.style.display = "block";
            if ('<%= Session["Contactrequesttype"] %>' == 'Transporter') {
                loadExecutiveNameFromField();
            }

            if ('<%= Session["Contactrequesttype"] %>' == 'customer') {

                var Mode = document.getElementById('KeyVal_InternalID').value;
                if (Mode != "Add") {
                    //var DocumentSegment = document.getElementById('HdnDocumentSegment').value;

                    //if (DocumentSegment != "") {
                    //   // LoadDocumentSegmentField();
                    //}
                    //else {
                    //   // GetDocumentSegment();
                    //}
                }

            }
        });

        function LoadDocumentSegmentField() {
            var table = document.getElementById("TblDocumentSegment");
            var DocumentSegment = document.getElementById('HdnDocumentSegment').value;

            if (DocumentSegment != "") {
                var values = DocumentSegment.split('~');


                var row = table.insertRow(0);
                var cell1 = row.insertCell(0);
                cell1.innerHTML = "<label style='width:113px' >Size</label> ";

                //for (var i = 0 ; i < values.length; i++) {
                for (var i = values.length - 1 ; i >= 0 ; i--) {

                    var id = i;
                    var cell2 = row.insertCell(1);

                    //if (values.length - 1 != id) {
                    //    var segid = parseFloat(id) + parseFloat(1);
                    //    cell2.innerHTML = "<input type='number' id=txtSeg" + id + " min='0' max='9' maxlength = '2' class='upper' value=" + values[i] + " Onblur='RemoveSegDisabled(" + segid + ")' />";

                    //}
                    //else {
                    //    cell2.innerHTML = "<input type='number' id=txtSeg" + id + "min='0' max='9' maxlength = '2' class='upper' value=" + values[i] + " Onblur='RemoveSegDisabled(" + id + ")' />";

                    //}

                    cell2.innerHTML = "<input style='width:113px' type='number' id=txtSeg" + id + "min='0' max='9' maxlength = '2'  value=" + values[i] + " Onblur='RemoveSegDisabled(" + id + ")' />";

                    //var a = values.length-1
                    //if (('txtSeg' + id) != "txtSeg0") {
                    //    $('#txtSeg' + id).attr('disabled', 'disabled');
                    //}
                }



                $.ajax({
                    type: "POST",
                    url: "Contact_general.aspx/GetDocumentSegment",
                    //data: JSON.stringify({ reqStr: fname }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var list = msg.d;
                        var listItems = [];
                        if (list.length > 0) {



                            var row = table.insertRow(0);
                            var cell1 = row.insertCell(0);
                            cell1.innerHTML = "<label >Segment</label>";
                            for (var i = 0; i < list.length; i++) {
                                var id = '';
                                var name = '';
                                id = list[i].split('|')[0];
                                name = list[i].split('|')[1];
                                var cell2 = row.insertCell(1);
                                cell2.innerHTML = "<label >" + name + "</label>";

                            }
                        }
                    }
                });

            }
        }









        function loadExecutiveNameFromField() {


            var table = document.getElementById("executiveTable");
            var exeName = document.getElementById('VehicleNo_hidden').value;
            //  alert(exeName);

            if (exeName != "") {
                var values = exeName.split(',');
                for (var i = 0 ; i < values.length; i++) {

                    if (table.rows[0].cells[0].children[0].value.trim() != '') {
                        var row = table.insertRow(0);
                        var cell1 = row.insertCell(0);
                        var cell2 = row.insertCell(1);
                        cell1.innerHTML = "<input type='text' class='upper'  value='" + values[i] + "'/>";
                        cell2.innerHTML = "<button type='button' value='' onclick='AddNewexecutive()' class='btn btn-primary btn-xs'><i class='fa fa-plus-circle'></i></button> <button type='button' value='' class='btn btn-danger btn-xs' onclick='removeExecutive(this.parentNode.parentNode)' ><i class='fa fa-times-circle'></i></button>";
                    }
                    else {
                        table.rows[0].cells[0].innerHTML = "<input type='text' class='upper'  value='" + values[i].toString() + "' novalidate />";

                    }
                }

            }

        }


        //Code for UDF Control 
        function OpenUdf() {
            if (document.getElementById('IsUdfpresent').value == '0') {
                jAlert("UDF not define.");
            }
            else {
                var url = 'frm_BranchUdfPopUp.aspx?Type=' + document.getElementById('hdKeyVal').value + '&&KeyVal_InternalID=' + document.getElementById('KeyVal_InternalID').value;
                popup.SetContentUrl(url);
                popup.Show();
            }
            return true;
        }
        // End Udf Code

        $(function () {

            $('#txtClentUcc').keyup(function () {
                var yourInput = ctxtClentUcc.GetText(); // $(this).val();
                re = /[`~!@#$%^&*_|+\=?;:'",<>\{\}\[\]\\\/]/gi;
                var isSplChar = re.test(yourInput);
                if (isSplChar) {
                    var no_spl_char = yourInput.replace(/[`~!@#$%^&*_|+\=?;:'",<>\{\}\[\]\\\/]/gi, '');
                    ctxtClentUcc.SetText(no_spl_char);
                }
            });

        });


        function fn_ctxtClentUcc_Name_TextChanged() {

            if (($("#hdnAutoNumStg").val() != "1") || ($("#hdnAutoNumStg").val() != "LDAutoNum1") || ($("#hdnAutoNumStg").val() != "AGAutoNum1")) {
                var procode = 0;
                //var ProductName = ctxtPro_Name.GetText();
                var clientName = ctxtClentUcc.GetText();
                $.ajax({
                    type: "POST",
                    url: "Contact_general.aspx/CheckUniqueName",
                    //data: "{'ProductName':'" + ProductName + "'}",
                    data: JSON.stringify({ clientName: clientName, procode: procode }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var data = msg.d;

                        if (data.split('~')[0] == "True") {
                            jAlert("Already exists for " + data.split('~')[1]);

                            ctxtClentUcc.SetText("");
                            //document.getElementById("Popup_Empcitys_ctxtPro_Code_I").focus();
                            document.getElementById("txtClentUcc").focus();

                            return false;
                        }
                    }

                });
            }
        }

        $(document).ready(function () {

            $("#<%=btnCancel.ClientID %>").click(function () {


                var url = "";
                if (mod == 'customer') {
                    url = 'CustomerMasterList.aspx';
                }
                else {
                    url = 'frmContactMain.aspx?requesttype=<%= Session["Contactrequesttype"] %>';
                }

                window.location.href = url;
                return false;
            });



          <%--  if ('<%= Session["Contactrequesttype"] %>' != 'customer' || '<%= Session["Contactrequesttype"] %>' != 'Transporter') {
               
                $('.forCustomer').hide();
            }--%>

            if ('<%= Session["Contactrequesttype"] %>' == 'customer') {

                $('.forCustomer').show();
                // $('.Trprofessionother').hide();
                // document.getElementById("Trprofessionother").style.display = 'none';
            }
            else if ('<%= Session["Contactrequesttype"] %>' == 'Transporter') {
                $('.forCustomer').show();
            }
            else { $('.forCustomer').hide(); }



        });
        function onlyNumbers(evt) {
            var e = event || evt; // for trans-browser compatibility
            var charCode = e.which || e.keyCode;

            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;

        }
        function ul() {
            window.opener.document.getElementById('iFrmInformation').setAttribute('src', 'CallUserInformation.aspx')
        }
        function ContactStatus() {
            var comboid = document.getElementById('ASPxPageControl1_cmbContactStatus');
            var comboval = comboid.value;
            if (comboval == '1') {
                document.getElementById("TrContact").style.display = 'none';
            }
            else {
                document.getElementById("TrContact").style.display = 'inline';
            }
        }
        function disp_prompt(name) {
            if (name == "tab0") {
                //alert(name);
                document.location.href = "Contact_general.aspx";
            }
            if (name == "tab1") {
                //alert(name);
                document.location.href = "Contact_Correspondence.aspx";
            }
            else if (name == "tab2") {
                //alert(name);
                document.location.href = "Contact_BankDetails.aspx";
            }
            else if (name == "tab3") {
                //alert(name);
                document.location.href = "Contact_DPDetails.aspx";
            }
            else if (name == "tab4") {
                //alert(name);
                document.location.href = "Contact_Document.aspx";
            }
            else if (name == "tab12") {
                //alert(name);
                document.location.href = "Contact_FamilyMembers.aspx";
            }
            else if (name == "tab5") {
                //alert(name);
                document.location.href = "Contact_Registration.aspx";
            }
            else if (name == "tab7") {
                //alert(name);
                document.location.href = "Contact_GroupMember.aspx";
            }
            else if (name == "tab8") {
                //alert(name);
                document.location.href = "Contact_Deposit.aspx";
            }
            else if (name == "tab9") {
                //alert(name);
                document.location.href = "Contact_Remarks.aspx";
            }
            else if (name == "tab10") {
                //alert(name);
                document.location.href = "Contact_Education.aspx";
            }
            else if (name == "tab11") {
                //alert(name);
                document.location.href = "contact_brokerage.aspx";
            }
            else if (name == "tab6") {
                //alert(name);
                document.location.href = "contact_other.aspx";
            }
            else if (name == "tab13") {
                document.location.href = "contact_Subscription.aspx";
            }
            else if (name == "tab14") {
                document.location.href = "Contact_tds.aspx";
            }
            else if (name == "tab15") {
                document.location.href = "Contact_Person.aspx";
            }
        }
        function CallList(obj1, obj2, obj3) {
            var sourceID = document.getElementById("cmbSource");
            if (sourceID.value == '21' || sourceID.value == '3' || sourceID.value == '4' || sourceID.value == '8' || sourceID.value == '10' || sourceID.value == '19' || sourceID.value == '20' || sourceID.value == '14' || sourceID.value == '24' || sourceID.value == '25')// || sourceID.value=='18')
            {
                //alert(sourceID.value);
                var obj4 = document.getElementById("cmbSource");
                var obj5 = obj4.value;
                //alert(obj5);
                ajax_showOptions(obj1, obj2, obj3, obj5, 'Sub');
            }
        }
        function LDCallList(obj1, obj2, obj3) {
            var sourceID = document.getElementById("LDcmbSource");
            if (sourceID.value == '21' || sourceID.value == '3' || sourceID.value == '4' || sourceID.value == '8' || sourceID.value == '10' || sourceID.value == '19' || sourceID.value == '20' || sourceID.value == '14' || sourceID.value == '24' || sourceID.value == '25')// || sourceID.value=='18')
            {
                //alert(sourceID.value);
                var obj4 = document.getElementById("LDcmbSource");
                var obj5 = obj4.value;
                //alert(obj5);
                ajax_showOptions(obj1, obj2, obj3, obj5, 'Sub');
            }
        }

        //$("cmbLegalStatus").blur(function () {

        //    if (($("#hdnAutoNumStg").val() == "1") && ($("#hdnTransactionType").val() == "CL") && (mode == "ADD")) {

        //        if ($("#ddl_numberingScheme").val() == "0") {
        //            jAlert("Please Select Numbering Scheme.");
        //            //setTimeout(function () { document.getElementById('ddl_numberingScheme').focus(); }, 500);

        //            ret = false;
        //            return false;

        //        }

        //        else if (ctxt_CustDocNo.GetText() == "") {
        //            jAlert("Please Enter Document No.");
        //            ret = false;
        //            return false;
        //        }

        //    }
        //});


        $("#cmbLegalStatus").on("blur", function () {

            if (($("#hdnAutoNumStg").val() == "1") && ($("#hdnTransactionType").val() == "CL") && (mode == "ADD")) {

                //if ($("#ddl_numberingScheme").val() == "0") {
                //    jAlert("Please Select Numbering Scheme.");
                //setTimeout(function () { document.getElementById('ddl_numberingScheme').focus(); }, 500);
                document.getElementById('ddl_numberingScheme').focus();
                //    ret = false;
                //    return false;

                //}

                //else if (ctxt_CustDocNo.GetText() == "") {
                //    jAlert("Please Enter Document No.");
                //    ret = false;
                //    return false;
                //}

            }

        });



        function legalStatus() {
            document.getElementById("cmbLegalStatus").focus();
            var elt = document.getElementById("cmbLegalStatus");
            var ss = elt.options[elt.selectedIndex].text;




            var isOpera = !!window.opera || navigator.userAgent.indexOf(' OPR/') >= 0;
            // Opera 8.0+ (UA detection to detect Blink/v8-powered Opera)
            var isFirefox = typeof InstallTrigger !== 'undefined';   // Firefox 1.0+
            var isSafari = Object.prototype.toString.call(window.HTMLElement).indexOf('Constructor') > 0;
            // At least Safari 3+: "[object HTMLElementConstructor]"
            var isChrome = !!window.chrome && !isOpera;              // Chrome 1+
            var isIE = /*@cc_on!@*/false || !!document.documentMode;   // At least IE6






            if ((ss.indexOf("Individual") <= -1)) {



                if (isIE == true) {


                    document.getElementById("td_lAnniversary").style.display = 'none';
                    document.getElementById("td_tAnniversary").style.display = 'none';
                    document.getElementById("td_lGender").style.display = 'none';
                    document.getElementById("td_dGender").style.display = 'none';
                    document.getElementById("td_lMarital").style.display = 'none';
                    document.getElementById("td_dMarital").style.display = 'none';

                }

                var eggs = document.getElementsByClassName('visF');


                for (var i = 0; i < eggs.length; i++) {
                    eggs[i].style.display = 'none';
                }
            }
            else {

                if (isIE == true) {

                    document.getElementById("td_lAnniversary").style.display = 'inline';
                    document.getElementById("td_tAnniversary").style.display = 'inline';
                    document.getElementById("td_lGender").style.display = 'block';
                    document.getElementById("td_dGender").style.display = 'block';
                    document.getElementById("td_lMarital").style.display = 'block';
                    document.getElementById("td_dMarital").style.display = 'block';


                }

                var eggs = document.getElementsByClassName('visF');
                for (var i = 0; i < eggs.length; i++) {
                    eggs[i].style.display = 'block';
                }
            }

            var SID1 = document.getElementById("cmbLegalStatus");

            if (SID1.value == '21') {

                // document.getElementById("td_red").style.display = 'inline';
                //document.getElementById("td_green").style.display = 'none';
                document.getElementById("td_one").style.display = 'inline';
                document.getElementById("td_two").style.display = 'none';
                //document.getElementById("td_only").style.display = 'none';

            }
            else if (SID1.value == '2' || SID1.value == '3' || SID1.value == '17' || SID1.value == '18' || SID1.value == '28' || SID1.value == '47' || SID1.value == '48') {

                // document.getElementById("td_red").style.display = 'inline';
                //document.getElementById("td_green").style.display = 'none';
                document.getElementById("td_one").style.display = 'none';
                document.getElementById("td_two").style.display = 'inline';
                //document.getElementById("td_only").style.display = 'none';
            }
            else {
                //document.getElementById("td_red").style.display = 'none';
                //document.getElementById("td_green").style.display = 'inline';
                document.getElementById("td_one").style.display = 'none';
                document.getElementById("td_two").style.display = 'none';
                //document.getElementById("td_only").style.display = 'inline';
            }
            if (SID1.value == '1' || SID1.value == '30' || SID1.value == '31' || SID1.value == '32' || SID1.value == '52' || SID1.value == '29' || SID1.value == '33' || SID1.value == '34' || SID1.value == '54') {


                document.getElementById("Trincorporation").style.display = 'none';
            }
            else {

                document.getElementById("Trincorporation").style.display = 'inline';
            }




        }
        function SourceStatus() {
            var sourceID = document.getElementById("cmbSource");
            if (sourceID.value == '21' || sourceID.value == '3' || sourceID.value == '4' || sourceID.value == '8' || sourceID.value == '10' || sourceID.value == '19' || sourceID.value == '20' || sourceID.value == '14' || sourceID.value == '24' || sourceID.value == '25' || sourceID.value == '18') {
                document.getElementById("TdRfferedBy").style.display = 'inline';
                document.getElementById("TdRfferedBy1").style.display = 'inline';
            }
            else {
                document.getElementById("TdRfferedBy").style.display = 'none';
                document.getElementById("TdRfferedBy1").style.display = 'none';
            }
        }

        function showcountry() {
            var countryID = document.getElementById("ddlnational");
            //if (countryID.value == '1')
            //    document.getElementById("td_country").style.display = 'none';
            //else
            //    document.getElementById("td_country").style.display = 'inline';
        }
        function hideotherstatus() {

            document.getElementById("Trprofessionother").style.display = 'none';
        }
        function ProfessionStatus() {
            var professionID = document.getElementById("cmbProfession");
            if (professionID.value == '20')
                document.getElementById("Trprofessionother").style.display = 'inline';
            else
                document.getElementById("Trprofessionother").style.display = 'none';
        }
        function PageLoad() {
            var PID = document.getElementById("cmbProfession");

            if (PID.value == '20')
                document.getElementById("Trprofessionother").style.display = 'inline';
            else
                document.getElementById("Trprofessionother").style.display = 'none';
            var countryID = document.getElementById("ddlnational");
            //if (countryID.value == '1')
            //    document.getElementById("td_country").style.display = 'none';
            //else
            //    document.getElementById("td_country").style.display = 'inline';
            //var ID = document.getElementById("cmbSource");
            ////  alert(ID.value);
            //if (ID.value == '21' || ID.value == '3' || ID.value == '4' || ID.value == '8' || ID.value == '10' || ID.value == '19' || ID.value == '20' || ID.value == '14' || ID.value == '18') {

            //    document.getElementById("TdRfferedBy").style.display = 'inline';
            //    document.getElementById("TdRfferedBy1").style.display = 'inline';

            //}
            //else {

            //    document.getElementById("TdRfferedBy").style.display = 'none';
            //    document.getElementById("TdRfferedBy1").style.display = 'none';



            //}
            var ID1 = document.getElementById("cmbLegalStatus");
            if (ID1.value == '21') {

                //document.getElementById("td_red").style.display = 'inline';
                //document.getElementById("td_green").style.display = 'none';
                document.getElementById("td_one").style.display = 'inline';
                document.getElementById("td_two").style.display = 'none';
                //document.getElementById("td_only").style.display = 'none';
            }
            else if (ID1.value == '2' || ID1.value == '3' || ID1.value == '17' || ID1.value == '18' || ID1.value == '28' || ID1.value == '47' || ID1.value == '48') {

                //document.getElementById("td_red").style.display = 'inline';
                //document.getElementById("td_green").style.display = 'none';
                document.getElementById("td_one").style.display = 'none';
                document.getElementById("td_two").style.display = 'inline';
                //document.getElementById("td_only").style.display = 'none';
            }
            else {
                //document.getElementById("td_red").style.display = 'none';
                //document.getElementById("td_green").style.display = 'inline';
                document.getElementById("td_one").style.display = 'none';
                document.getElementById("td_two").style.display = 'none';
                //document.getElementById("td_only").style.display = 'inline';




            }

            if (ID1.value == '1' || ID1.value == '30' || ID1.value == '31' || ID1.value == '32' || ID1.value == '52' || ID1.value == '29' || ID1.value == '33' || ID1.value == '34' || ID1.value == '54') {


                document.getElementById("Trincorporation").style.display = 'none';
            }
            else {

                document.getElementById("Trincorporation").style.display = 'inline';
            }
            var comboid = document.getElementById('cmbContactStatus');
            var comboval = comboid.value;
            if (comboval == '1') {
                document.getElementById("TrContact").style.display = 'none';
            }
            else {
                document.getElementById("TrContact").style.display = 'inline';

            }
        }
        function FillValues(chk) {
            var sel = document.getElementById('LitSpokenLanguage');
            sel.value = chk;
        }
        function FillValues1(chk) {
            var sel = document.getElementById('LitWrittenLanguage');
            sel.value = chk;
        }
        function keyVal(obj) {
            var objPhEmail = obj.split('~');
            document.getElementById('txtRPartner_hidden').value = objPhEmail[0];
            document.getElementById('TxtEmail').value = objPhEmail[1];
            document.getElementById('TxtPhone').value = objPhEmail[2];
        }
        function popup() {
            alert("Please type prefix of UCC");
            return false;

        }

        //function dateValidationFormat(e) {
        //    var v = e.target.value; //this.value;
        //    if (v.match(/^\d{2}$/) !== null) {
        //        e.target.value = v + '-';
        //    } else if (v.match(/^\d{2}\-\d{2}$/) !== null) {
        //        e.target.value = v + '-';
        //    }
        //}

        //function isDateKey(evt) {
        //    var charCode = (evt.which) ? evt.which : event.keyCode
        //    console.log(charCode);

        //    if (charCode > 31 && (charCode < 48 || charCode > 57))
        //        return false;
        //    return true;
        //}


        function fn_btnValidate(s, e) {

            //  e.processOnServer = false;
            var ret = true;
            var validateFlag = true;
            var contype = '<%= Session["Contactrequesttype"] %>';
            var mode = '<%= Session["InsertMode"] %>';
            $('#RequiredFieldValidator1').hide();
            if (contype == 'customer') {
                // ctxtClentUcc
                //var clientUcc = $('#txtClentUcc_I').val();
                var clientUcc = ctxtClentUcc.GetText();
                if (($("#hdnAutoNumStg").val() == "1") && ($("#hdnTransactionType").val() == "CL")) {
                    $("#hddnDocNo").val(ctxt_CustDocNo.GetText());
                }

                if ($('#ddlIdType').val() == 1) {
                    if (isNaN(clientUcc)) {
                        jAlert("Please type valid Phone No.");
                        //.processOnServer=false;
                        ret = false;

                    }
                } else if ($('#ddlIdType').val() == 2) {

                    var PAN = ctxtClentUcc.GetText().toUpperCase();
                    var panPat = /^([a-zA-Z]{5})(\d{4})([a-zA-Z]{1})$/;
                    var code = /([C,P,H,F,A,T,B,L,J,G])/;
                    var code_chk = PAN.substring(3, 4);

                    //var panPat = /^([a-zA-Z]{5})(\d{4})([a-zA-Z]{1})$/;
                    //var code = /([C,P,H,F,A,T,B,L,J,G])/;
                    //var code_chk = PAN.substring(3, 4);
                    // var regpan = /^([a-zA-Z]){5}([0-9]){4}([a-zA-Z]){1}?$/;

                    if (PAN.search(panPat) == -1) {
                        jAlert("Please type valid PAN No.");

                        ret = false;
                    }
                    else if (code.test(code_chk) == false) {
                        jAlert("Please type valid PAN No.");
                        ret = false;
                    }
                    //if (!regpan.test(clientUcc)) {
                    //          jAlert("Please type valid PAN No.");

                    //          ret = false;
                    //      }


                } else if ($('#ddlIdType').val() == 3) {
                    if (isNaN(clientUcc) || clientUcc.length != 12) {
                        jAlert("Please type valid Aadhar No.");
                        ret = false;
                    }
                }


                if (($("#hdnAutoNumStg").val() == "1") && ($("#hdnTransactionType").val() == "CL") && (mode == "ADD")) {

                    if ($("#ddl_numberingScheme").val() == "0") {
                        jAlert("Please Select Numbering Scheme.");
                        //setTimeout(function () { document.getElementById('ddl_numberingScheme').focus(); }, 500);

                        ret = false;
                        return false;

                    }

                    else if (ctxt_CustDocNo.GetText() == "") {
                        jAlert("Please Enter Unique ID.");
                        ret = false;
                        return false;
                    }

                }

                var txtbox = ctxtFirstNmae.GetText();
                if (txtbox == "") {
                    //alert("Please Insert Full Name.");
                    jAlert("Please Insert Full Name.");
                    //document.getElementById('txtFirstNmae').Focus();
                    $('#RequiredFieldValidator1').show();
                    ret = false;
                    return false;

                }



                // Page_ClientValidate();
                $('#invalidGst').css({ 'display': 'none' });
                var gst1 = ctxtGSTIN111.GetText().trim();
                var gst2 = ctxtGSTIN222.GetText().trim();
                var gst3 = ctxtGSTIN333.GetText().trim();

                var isregistered = $('#<%=radioregistercheck.ClientID %> input:checked').val();

                if (gst1.length == 0 && gst2.length == 0 && gst3.length == 0) {
                    if (isregistered == 1) {

                        jAlert('GSTIN is mandatory.');
                        ret = false;
                    }


                }
                else {
                    if (gst1.length != 2 || gst2.length != 10 || gst3.length != 3) {
                        $('#invalidGst').css({ 'display': 'block' });
                        ret = false;
                    }


                    var panPat = /^([a-zA-Z]{5})(\d{4})([a-zA-Z]{1})$/;
                    var panPat1 = /^([a-zA-Z]{4})(\d{5})([a-zA-Z]{1})$/;
                    var code = /([C,P,H,F,A,T,B,L,J,G])/;
                    var code_chk = gst2.substring(3, 4);
                    if (gst2.search(panPat) == -1 && gst2.search(panPat1) == -1) {
                        $('#invalidGst').css({ 'display': 'block' });
                        ret = false;
                    }
                    if (code.test(code_chk) == false) {
                        $('#invalidGst').css({ 'display': 'block' });
                        ret = false;
                    }
                }

                var isregisteredCheck = $('#<%=radioregistercheck.ClientID %> input:checked').val();
                var finalGST = (gst1 + gst2 + gst3);
                var GSTINOldval = $("#<%=hddnGSTIN2Val.ClientID%>").val();

                var txtbox = ccmbTransCategory.GetText();
                if ($("#hdnActiveEInvoice").val() == "1") {
                    if (txtbox == "Select") {
                        jAlert("Please Select Transaction Category.");

                        ret = false;
                    }
                }

                <%--if (cApplicableFrom.GetDate() === null && $("#<%=hddnApplicationMode.ClientID%>").val() === "E" && GSTINOldval.trim() !== finalGST.trim()) {
                    if (isregisteredCheck == "1") {

                        jAlert("Please enter Applicable from.");
                        ret = false;
                        validateFlag = false;
                    }
                } --%>

                if (GSTINOldval.trim() !== finalGST.trim() && isregisteredCheck == "1" && validateFlag == true && $("#<%=hddnApplicationMode.ClientID%>").val() === "E") {

                    var r = confirm("You have entered GSTIN Applicable date. Based on the applicable date all the transaction will be updated with entered GSTIN. \nAre you sure?");
                    if (r == true) {
                        ret = true;
                        if (cApplicableFrom.GetDate() === null && $("#<%=hddnApplicationMode.ClientID%>").val() === "E" && GSTINOldval.trim() !== finalGST.trim()) {
                            if (isregisteredCheck == "1") {
                                jAlert("Please enter Applicable from.");
                                ret = false;
                                validateFlag = false;
                            }
                        } 
                    }
                    else {
                        ret = false;
                    }


                    $("#<%=hddnGSTINFlag.ClientID%>").val("UPDATE");
                }
                else {
                    $("#<%=hddnGSTINFlag.ClientID%>").val("NotUPDATE");
                }


                if ($("#hdnDocumentSegmentSettings").val() == "1") {


                    // var table = document.getElementById("TblDocumentSegment");
                    document.getElementById('HdnDocumentSegment').value = '';
                    //var data = '';
                    //var row = table.rows[1];
                    //for (var j = 1, col; col = row.cells[j]; j++) {
                    //    if (data == '') {
                    //        if (col.children[0].value == '') {
                    //        }
                    //        else if (col.children[0].value == undefined) {
                    //        }
                    //        else {
                    //            data = col.children[0].value;
                    //        }
                    //    }
                    //    else {
                    //        data = data + '~' + col.children[0].value;
                    //    }
                    //}
                    //if (document.getElementById('HdnDocumentSegment').value == '') {
                    //    document.getElementById('HdnDocumentSegment').value = data.toUpperCase();
                    //    data = '';
                    //}
                    //else {
                    //    document.getElementById('HdnDocumentSegment').value = document.getElementById('HdnDocumentSegment').value + ',' + data.toUpperCase();
                    //    data = '';
                    //}

                    if (ctxtSegment1.GetValue() == null) {
                        ctxtSegment1.SetValue("0");
                    }
                    if (ctxtSegment2.GetValue() == null) {
                        ctxtSegment2.SetValue("0");
                    }
                    if (ctxtSegment3.GetValue() == null) {
                        ctxtSegment3.SetValue("0");
                    }
                    if (ctxtSegment4.GetValue() == null) {
                        ctxtSegment4.SetValue("0");
                    }

                    if (ctxtSegment5.GetValue() == null) {
                        ctxtSegment5.SetValue("0");
                    }


                    document.getElementById('HdnDocumentSegment').value = ctxtSegment1.GetValue() + '~' + ctxtSegment2.GetValue() + '~' + ctxtSegment3.GetValue() + '~' + ctxtSegment4.GetValue() + '~' + ctxtSegment5.GetValue();


                    var Document_Segments = document.getElementById('HdnDocumentSegment').value;

                    var TotalDocument_Segments = 0;
                    var DocumentSegment = Document_Segments.split("~");
                    for (i = 0; i < DocumentSegment.length; i++) {
                        TotalDocument_Segments = parseFloat(TotalDocument_Segments) + parseFloat(DocumentSegment[i]);
                    }
                    if (parseFloat(TotalDocument_Segments) > 15) {
                        jAlert("Document Segments value greater than 15.");
                        ret = false;
                    }


                    var Segment1 = ctxtSegment1.GetValue();
                    var Segment2 = ctxtSegment2.GetValue();
                    var Segment3 = ctxtSegment3.GetValue();
                    var Segment4 = ctxtSegment4.GetValue();
                    var Segment5 = ctxtSegment5.GetValue();

                    var SegmentUsedFor1 = ctxtUsedFor1.GetValue();
                    var SegmentUsedFor2 = ctxtUsedFor2.GetValue();
                    var SegmentUsedFor3 = ctxtUsedFor3.GetValue();
                    var SegmentUsedFor4 = ctxtUsedFor4.GetValue();
                    var SegmentUsedFor5 = ctxtUsedFor5.GetValue();

                    if (Segment2 != "0") {
                        if (Segment1 == "0") {

                            ctxtSegment1.SetValue(Segment2);
                            ctxtSegment2.SetValue("0");
                        }
                    }

                    if (Segment3 != "0") {
                        if (Segment2 == "0") {

                            ctxtSegment2.SetValue(Segment3);
                            ctxtSegment3.SetValue("0");
                        }
                    }

                    if (Segment4 != "0") {
                        if (Segment3 == "0") {

                            ctxtSegment3.SetValue(Segment4);
                            ctxtSegment4.SetValue("0");
                        }
                    }

                    if (Segment5 != "0") {
                        if (Segment4 == "0") {

                            ctxtSegment4.SetValue(Segment5);
                            ctxtSegment5.SetValue("0");
                        }
                    }

                    //if (Document_Segments != "")
                    //{
                    //    if (Segment1 == "0") {
                    //        jAlert("Please enter Segment1.");
                    //        ret = false;
                    //    }
                    //}


                    if (ctxtSegment1.GetValue() != "0") {
                        if (SegmentUsedFor1 == null) {
                            jAlert("Please enter Segment1 Name.");
                            ret = false;
                        }
                    }
                    if (ctxtSegment2.GetValue() != "0") {
                        if (SegmentUsedFor2 == null) {
                            jAlert("Please enter Segment2 Name.");

                            ret = false;
                        }
                    }

                    if (ctxtSegment3.GetValue() != "0") {
                        if (SegmentUsedFor3 == null) {
                            jAlert("Please enter Segment3 Name.");

                            ret = false;
                        }
                    }

                    if (ctxtSegment4.GetValue() != "0") {
                        if (SegmentUsedFor4 == null) {
                            jAlert("Please enter Segment4 Name.");
                            ret = false;
                        }
                    }

                    if (ctxtSegment5.GetValue() != "0") {
                        if (SegmentUsedFor5 == null) {
                            jAlert("Please enter Segment5 Name.");

                            ret = false;
                        }
                    }




                }

            }

            if (contype != 'Lead') {

                //var txtbox = document.getElementById('txtFirstNmae');
                var txtbox = ctxtFirstNmae.GetText();
                //var txt2 = document.getElementById('txtClentUcc');
                var txt2 = ctxtClentUcc.GetText();


                if (($("#hdnAutoNumStg").val() == "AGAutoNum1") && ($("#hdnTransactionType").val() == "AG") && (mode == "ADD")) {

                    if ($("#ddl_numberingScheme").val() == "0") {
                        jAlert("Please Select Numbering Scheme.");
                        ret = false;
                        return false;
                    }
                    else if (ctxt_CustDocNo.GetText() == "") {
                        jAlert("Please Enter Unique ID.");
                        ret = false;
                        return false;
                    }
                }

                if (($("#hdnAutoNumStg").val() == "TRAutoNum1") && ($("#hdnTransactionType").val() == "TR") && (mode == "ADD")) {

                    if ($("#ddl_numberingScheme").val() == "0") {
                        jAlert("Please Select Numbering Scheme.");
                        ret = false;
                        return false;
                    }
                    else if (ctxt_CustDocNo.GetText() == "") {
                        jAlert("Please Enter Unique ID.");
                        ret = false;
                        return false;
                    }
                }
                if (($("#hdnAutoNumStg").val() == "RAAutoNum1") && ($("#hdnTransactionType").val() == "RA") && (mode == "ADD")) {

                    if ($("#ddl_numberingScheme").val() == "0") {
                        jAlert("Please Select Numbering Scheme.");
                        ret = false;
                        return false;
                    }
                    else if (ctxt_CustDocNo.GetText() == "") {
                        jAlert("Please Enter Unique ID.");
                        ret = false;
                        return false;
                    }
                }

                if (txtbox == "") {
                    //alert("Please Insert Full Name.");
                    jAlert("Please Insert Full Name.");
                    //document.getElementById('txtFirstNmae').Focus();
                    $('#RequiredFieldValidator1').show();
                    ret = false;
                    return false;

                }
                if ($("#hdnAutoNumStg").val() == "") {

                    if (txt2 == "") {
                        alert("Please Insert Unique Code.");
                        document.getElementById('txtClentUcc').Focus();

                        ret = false;
                    }

                }



            }

            if (contype == 'Lead') {
                setvaluechosen();

                if (($("#hdnAutoNumStg").val() == "LDAutoNum1") && ($("#hdnTransactionType").val() == "LD") && (mode == "ADD")) {

                    if ($("#LDddl_numberingScheme").val() == "0") {
                        jAlert("Please Select Numbering Scheme.");
                        ret = false;
                        return false;

                    }
                    else if (LDctxt_CustDocNo.GetText() == "") {
                        jAlert("Please Enter Unique ID.");
                        ret = false;
                        return false;
                    }
                }



                if (($("#hdnAutoNumStg").val() == "LDAutoNum1") && ($("#hdnTransactionType").val() == "LD")) {
                    $("#hddnDocNo").val(LDctxt_CustDocNo.GetText());
                }
            }

            if (contype != 'Lead') {

                var selectoption = document.getElementById("cmbLegalStatus");
                var optionText = selectoption.options[selectoption.selectedIndex].text;

                //  alert(optionText);
                if (optionText == 'Local') {

                    ret = SetVechileNo();
                }

                var optionText = $('#<%=radioregistercheck.ClientID %> input:checked').val();
                if (optionText == '1' && contype == 'Transporter') {

                    var gst1 = ctxtGSTIN111.GetText().trim();
                    var gst2 = ctxtGSTIN222.GetText().trim();
                    var gst3 = ctxtGSTIN333.GetText().trim();

                    if (gst1.length == 0 && gst2.length == 0 && gst3.length == 0) {
                        ret = false;
                        jAlert("GSTIN is mandatory. ");

                    }
                    else {
                        if (gst1.length != 2 || gst2.length != 10 || gst3.length != 3) {
                            $('#invalidGst').css({ 'display': 'block' });
                            ret = false;

                            $("#spanmandategstn").attr('style', 'display:inline;color:red');
                        }


                        var panPat = /^([a-zA-Z]{5})(\d{4})([a-zA-Z]{1})$/;
                        var code = /([C,P,H,F,A,T,B,L,J,G])/;
                        var code_chk = gst2.substring(3, 4);
                        if (gst2.search(panPat) == -1) {
                            $('#invalidGst').css({ 'display': 'block' });
                            ret = false;

                            $("#spanmandategstn").attr('style', 'display:inline;color:red');
                        }

                        if (code.test(code_chk) == false) {
                            $('#invalidGst').css({ 'display': 'block' });
                            ret = false;

                            $("#spanmandategstn").attr('style', 'display:inline;color:red');
                        }
                    }

                    var isregisteredCheck = $('#<%=radioregistercheck.ClientID %> input:checked').val();
                    var finalGST = (gst1 + gst2 + gst3);
                    var GSTINOldval = $("#<%=hddnGSTIN2Val.ClientID%>").val();

                    if (cApplicableFrom.GetDate() === null && $("#<%=hddnApplicationMode.ClientID%>").val() === "E" && GSTINOldval.trim() !== finalGST.trim()) {
                        if (isregisteredCheck == "1") {

                            jAlert("Please enter Applicable from.");
                            ret = false;
                            validateFlag = false;
                        }
                    }

                    if (GSTINOldval.trim() !== finalGST.trim() && isregisteredCheck == "1" && validateFlag == true && $("#<%=hddnApplicationMode.ClientID%>").val() === "E") {

                        var r = confirm("You have entered GSTIN Applicable date. Based on the applicable date all the transaction will be updated with entered GSTIN. \nAre you sure?");
                        if (r == true) {
                            ret = true;
                        }
                        else {
                            ret = false;
                        }


                        $("#<%=hdnGSTINFlagTranspoter.ClientID%>").val("UPDATE");
                    }
                    else {
                        $("#<%=hdnGSTINFlagTranspoter.ClientID%>").val("NotUPDATE");
                    }

                }
            }

            if ($("#hdnAutoNumStg").val() == "1") {
                ctxtClentUcc.SetText("ABC")//this is for mandatory field
                $("#hddnDocNo").val(ctxt_CustDocNo.GetText());
            }
            if ($("#hdnAutoNumStg").val() == "AGAutoNum1") {
                ctxtClentUcc.SetText("ABC")//this is for mandatory field
                $("#hddnDocNo").val(ctxt_CustDocNo.GetText());
            }
            if ($("#hdnAutoNumStg").val() == "LDAutoNum1") {
                $("#LDtxtClentUcc").val('ABC')//this is for mandatory field
            }
            if ($("#hdnAutoNumStg").val() == "TRAutoNum1") {
                ctxtClentUcc.SetText("ABC")//this is for mandatory field
                $("#hddnDocNo").val(ctxt_CustDocNo.GetText());
            }
            if ($("#hdnAutoNumStg").val() == "RAAutoNum1") {
                ctxtClentUcc.SetText("ABC")//this is for mandatory field
                $("#hddnDocNo").val(ctxt_CustDocNo.GetText())

            }



            e.processOnServer = ret;
        }

        function OnContactInfoClick(keyValue, CompName) {
            var keyValue = ('<%=Session["KeyVal_InternalID"] %>');
         <%--   var CompName = ("<%=Session["name"] %>");--%>
                var url = 'insurance_contactPerson.aspx?id=' + keyValue;
                window.location.href = url;
            }
            function FillUCCCode(chk) {
                var sel = document.getElementById('txtClentUcc');
                sel.value = chk;
            }
            FieldName = 'cmbLegalStatus';

            //for leads
            function LeadCallList(obj1, obj2, obj3) {
                var obj4 = document.getElementById("<%=LDcmbSource.ClientID%>");
                var obj5 = obj4.value;
                //alert(obj5);
                ajax_showOptions(obj1, obj2, obj3, obj5);
                //alert(obj5);
                FieldName = '<%=LDcmbGender.ClientID%>';
            }
            function AtTheTimePageLoad() {
                FieldName = '<%=LDcmbLegalStatus.ClientID%>';

            document.getElementById("<%=LDtxtReferedBy_hidden.ClientID%>").style.display = 'none';
        }


        $(document).ready(function () {

            //for choosen
            ListBind();
            ChangeSource();
            ChangeSourceMainAccount();

            //if ($("#radioregistercheck").find(":checked").val() == '1') {
            //    cApplicableFrom.SetEnabled(true);
            //}
            //else {
            //    cApplicableFrom.SetEnabled(false);
            //}

            if ('<%=Convert.ToString(Session["requesttype"])%>' == "Customer/Client" || '<%=Convert.ToString(Session["requesttype"])%>' == "Transporter" || '<%=Convert.ToString(Session["requesttype"])%>' == "Relationship Partners") {
                    $('.mainAccount').show();
                }
                else {
                    $('.mainAccount').hide();
                }


                $("#RequiredFieldValidator9").css("display", "none");
                //end choosen

                $('#cmbLegalStatus').change(function (s, e) {
                    var legalstatus = $("#cmbLegalStatus").val();


                    if ((legalstatus != '1') && (legalstatus != '27') && (legalstatus != '29') && (legalstatus != '55') && (legalstatus != '54')) {
                        $('#cmbMaritalStatus').prop('selectedIndex', 0);
                        $('#txtAnniversary').val('');
                        $('#ASPxLabel18').text("Date of Incorporation");
                        //$('#td_lAnniversary').css('display', 'none');
                        //$('#td_tAnniversary').css('display', 'none');
                        //$('#td_lGender').css('display', 'none');
                        //$('#td_dGender').css('display', 'none');
                        //$('#td_lMarital').css('display', 'none');
                        //$('#td_dMarital').css('display', 'none');
                        //$('.txtAnniversary').attr('readonly', 'readonly');
                        //txtAnniversary.SetEnabled(false);

                        $('#cmbGender').attr("disabled", true);
                        $('#cmbGender').css("color", "lightgray");

                        $('#cmbMaritalStatus').attr("disabled", true);
                        $('#cmbMaritalStatus').css("color", "lightgray");

                    }
                    else {
                        $('#cmbMaritalStatus').prop('selectedIndex', 0);
                        $('#txtAnniversary').val('');
                        $('#ASPxLabel18').text("Date of Birth");
                        $('#ASPxLabel18').text("Date of Birth");

                        $('#cmbGender').attr("disabled", false);
                        $('#cmbGender').css("color", "black");

                        $('#cmbMaritalStatus').attr("disabled", false);
                        $('#cmbMaritalStatus').css("color", "black");
                    }
                })

                var lgsts = $('#cmbLegalStatus').val();

                if (lgsts != '') {

                    if ((lgsts != '1') && (lgsts != '27') && (lgsts != '29') && (lgsts != '55') && (lgsts != '54')) {
                        $('#ASPxLabel18').text("Date of Incorporation");
                        $('#cmbGender').attr("disabled", true);
                        $('#cmbGender').css("color", "lightgray");

                        $('#cmbMaritalStatus').attr("disabled", true);
                        $('#cmbMaritalStatus').css("color", "lightgray");
                    }
                    else {
                        $('#ASPxLabel18').text("Date of Birth");
                        // txtAnniversary.SetEnabled(true);

                        $('#cmbGender').attr("disabled", false);

                        $('#cmbMaritalStatus').attr("disabled", false);
                        $('#cmbMaritalStatus').css("color", "black");
                        $('#cmbGender').css("color", "black");
                    }
                }

            })



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
            function lstconverttounit() {

                $('#lstconverttounit').fadeIn();

            }
            function ChangeSource() {


                var fname = "%";
                var lconverttounit = $('select[id$=lstconverttounit]');
                lconverttounit.empty();
                //var CurrentSegment = document.getElementById('hdn_CurrentSegment').value;
                var CurrentSegment = "0";
                var sql = "select (ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') + ' [' + cnt_shortName +']') AS cnt_firstName, cnt_internalid from tbl_master_contact where (cnt_contactType='LD' OR cnt_contactType='EM') and (cnt_firstName like ('" + fname + "%') or cnt_lastName like ('" + fname + "%') or cnt_shortName like ('%" + fname + "%'))";

                $.ajax({
                    type: "POST",
                    url: "Contact_general.aspx/GetrefBy",
                    data: JSON.stringify({ query: sql }),
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

                                $('#lstconverttounit').append($('<option>').text(name).val(id));

                            }

                            $(lconverttounit).append(listItems.join(''));

                            lstconverttounit();
                            $('#lstconverttounit').trigger("chosen:updated");

                            Changeselectedvalue();


                        }
                        else {
                            //   alert("No records found");
                            //lstReferedBy();
                            $('#lstconverttounit').trigger("chosen:updated");

                        }
                    }
                });
                // }
            }

            function Changeselectedvalue() {
                var lstconverttounit = document.getElementById("lstconverttounit");
                if (document.getElementById("LDtxtReferedBy_hidden") != null) {
                    if (document.getElementById("LDtxtReferedBy_hidden").value != '') {

                        for (var i = 0; i < lstconverttounit.options.length; i++) {
                            if (lstconverttounit.options[i].value == document.getElementById("LDtxtReferedBy_hidden").value) {
                                lstconverttounit.options[i].selected = true;
                            }
                        }
                    }

                    $('#lstconverttounit').trigger("chosen:updated");
                }
            }

            function lstconverttounit() {
                $('#lstconverttounit').fadeIn();
            }
            function setvaluechosen() {
                //console.log('setval');
                document.getElementById("LDtxtReferedBy_hidden").value = document.getElementById("lstconverttounit").value;
                if (document.getElementById("LDtxtReferedBy_hidden").value != '') {
                } else {
                }
            }

            $(function () {

                $('body').on('change', '#cmbLegalStatus', function () {

                    var selectoption = document.getElementById("cmbLegalStatus");
                    var optionText = selectoption.options[selectoption.selectedIndex].text;

                    //  alert(optionText);
                    if (optionText == 'General') {
                        $("#pnlVehicleNo").attr('style', 'display:none;');
                    }
                    else {

                        $("#pnlVehicleNo").attr('style', 'display:block;');
                    }

                });

                $('body').on('click', '#radioregistercheck', function () {
                    var optionText = $('#<%=radioregistercheck.ClientID %> input:checked').val();

                    if (optionText == '1') {
                        $("#spanmandategstn").attr('style', 'display:inline;color:red');
                        td_Applicablefrom.style.display = "block";

                        //alert(1);
                        ctxtGSTIN111.SetEnabled(true);
                        ctxtGSTIN222.SetEnabled(true);
                        ctxtGSTIN333.SetEnabled(true);
                        cApplicableFrom.SetEnabled(true);
                    }
                    else {
                        //alert(2);
                        td_Applicablefrom.style.display = "block";
                        ctxtGSTIN111.SetText('');
                        ctxtGSTIN222.SetText('');
                        ctxtGSTIN333.SetText('');

                        ctxtGSTIN111.SetEnabled(false);
                        ctxtGSTIN222.SetEnabled(false);
                        ctxtGSTIN333.SetEnabled(false);
                        cApplicableFrom.SetEnabled(false);
                        $("#spanmandategstn").attr('style', 'display:none;');
                    }
                });


            }
            );
    </script>
    <style type="text/css">
        select#cddlSegmentMandatory1, select#cddlSegmentMandatory2, select#cddlSegmentMandatory3, select#cddlSegmentMandatory4, select#cddlSegmentMandatory5 {
            display: none;
        }

        .upper, .upper input {
            text-transform: uppercase !important;
        }

        #lstAssociatedEmployee {
            width: 100%;
            display: none !important;
        }

        .chosen-container.chosen-container-single {
            width: 100% !important;
        }

        #lstconverttounit {
            display: none !important;
        }

        .chosen-choices {
            width: 100% !important;
        }

        .inline {
            display: inline !important;
            float: left;
            padding-right: 5px;
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
            top: 3px;
            position: absolute;
        }

        #txtCreditLimit_EC {
            position: absolute;
        }

        #txtClentUcc_I {
            opacity: 1 !important;
        }

        #txtClentUcc {
            opacity: 1 !important;
        }

        .nestedinput {
            padding: 0;
            margin: 0;
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

        #executiveTable > tbody > tr > td:first-child {
            padding-right: 15px;
        }

        #executiveTable > tbody > tr > td:last-child {
            width: 105px;
        }

        .tp28 {
            top: 28px;
        }

        .padTble > tbody > tr > td {
            padding-right: 15px;
            padding-bottom: 3px;
            padding-left: 5px;
        }

        #Table1 {
            border: 1px solid #ddd;
        }

            #Table1 > tbody > tr:nth-child(odd) > td {
                background: #f1f1f1;
            }

        .gHesder {
            background: #6aa9ea !important;
            padding: 4px;
            font-weight: 600;
            color: #252525;
        }

        .tdpTop5 > td {
            padding-top: 5px;
        }

        .dSegment {
            margin-top: 5px;
            padding: 5px;
            background: #ececec;
            font-weight: 600 !important;
            width: 90%;
        }

        .mttHover {
            width: 287px;
        }

            .mttHover .form-control.multiselect-search {
                height: 32px !important;
            }

            .mttHover .multiselect-container.dropdown-menu > li > a:hover {
                background: #3177b2 !important;
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

        #ASPxFromDate , #ASPxToDate , #ASPxASondate , #ASPxAsOnDate , #txtDOB , #txtAnniversary
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        .dxeDisabled_PlasticBlue
        {
            z-index: 0 !important;
        }

        #ASPxFromDate_B-1 , #ASPxToDate_B-1 , #ASPxASondate_B-1 , #ASPxAsOnDate_B-1 , #txtDOB_B-1 , #txtAnniversary_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #ASPxFromDate_B-1 #ASPxFromDate_B-1Img , #ASPxToDate_B-1 #ASPxToDate_B-1Img , #ASPxASondate_B-1 #ASPxASondate_B-1Img , #ASPxAsOnDate_B-1 #ASPxAsOnDate_B-1Img ,
        #txtDOB_B-1 #txtDOB_B-1Img ,
        #txtAnniversary_B-1 #txtAnniversary_B-1Img
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
        .simple-select {
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

        /*.TableMain100 #ShowGrid , .TableMain100 #ShowGridList , .TableMain100 #ShowGridRet , .TableMain100 #ShowGridLocationwiseStockStatus 
        #B2B , 
        #grid_B2BUR , 
        #grid_IMPS , 
        #grid_IMPG ,
        #grid_CDNR ,
        #grid_CDNUR ,
        #grid_EXEMP ,
        #grid_ITCR ,
        #grid_HSNSUM
        {
            max-width: 98% !important;
        }*/

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
            margin-top: 24px !important;
        }

        .col-md-3
        {
            margin-top: 8px;
        }

        .chosen-container-single .chosen-single div::after
        {
                font-size: 17px;
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
    <%--  <div class="crossBtn" style="right: 28px;top: 14px;"><a href=""><i class="fa fa-times"></i></a></div>--%>
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main">
        <div class="panel-heading">
        <div class="panel-title">
            <%-- <h3>Contact List</h3>--%>
            <h3>
                <asp:Label ID="lblHeadTitle" runat="server"></asp:Label>
            </h3>
            <%--  <div class="crossBtn"><a href="frmContactMain.aspx?requesttype=<%= Session["Contactrequesttype"] %>"><i class="fa fa-times"></i></a></div>--%>
            <div class="crossBtn"><a id="lnkClose"><i class="fa fa-times"></i></a></div>

        </div>


        <%--debjyoti 22-12-2016--%>
        <dxe:ASPxPopupControl ID="ASPXPopupControl" runat="server"
            CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popup" Height="630px"
            Width="600px" HeaderText="Add/Modify UDF" Modal="true" AllowResize="true" ResizingMode="Postponed">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>

        <asp:HiddenField ID="hdKeyVal" runat="server" />
        <asp:HiddenField ID="KeyVal_InternalID" runat="server" />
        <asp:HiddenField runat="server" ID="IsUdfpresent" />
        <asp:HiddenField ID="hidAssociatedEmp" runat="server" />
        <asp:HiddenField ID="hddnApplicationMode" runat="server" />
        <asp:HiddenField ID="hddnGSTIN2Val" runat="server" />
        <asp:HiddenField ID="hddnGSTINFlag" runat="server" />
        <asp:HiddenField ID="hdnAutoNumStg" runat="server" />
        <asp:HiddenField ID="hddnDocNo" runat="server" />
        <asp:HiddenField ID="hdnTransactionType" runat="server" />
        <asp:HiddenField ID="hdnNumberingId" runat="server" />
        <asp:HiddenField ID="hdnGSTINFlagTranspoter" runat="server" />
        <asp:HiddenField runat="server" ID="hdnDocumentSegmentSettings" />
        <asp:HiddenField ID="hdnSegmentMandatory1" runat="server" />
        <asp:HiddenField ID="hdnSegmentMandatory2" runat="server" />
        <asp:HiddenField ID="hdnSegmentMandatory3" runat="server" />
        <asp:HiddenField ID="hdnSegmentMandatory4" runat="server" />
        <asp:HiddenField ID="hdnSegmentMandatory5" runat="server" />
        <asp:HiddenField ID="hdnSegmentMAPID" runat="server" />
        <%--End debjyoti 22-12-2016--%>
    </div>
        <div class="form_main">

        <table class="TableMain100">
            <tr>
                <td style="text-align: center;">
                    <asp:Label ID="lblName" runat="server" Font-Bold="True"></asp:Label>
                </td>

            </tr>

            <tr>
                <td>
                    <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" ClientInstanceName="page"
                        Font-Size="12px" OnActiveTabChanged="ASPxPageControl1_ActiveTabChanged" Width="100%">
                        <TabPages>
                            <dxe:TabPage Name="General" Text="General">
                                <%-- <TabTemplate>
                                <span style="font-size: x-small">General</span>&nbsp;<span style="color: Red;">*</span>
                            </TabTemplate>--%>
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">

                                        <table class="TableMain100" id="table_others" runat="server">
                                            <tr>
                                                <td style="width: 500px">
                                                    <asp:Panel ID="Panel1" runat="server" BorderColor="White" BorderWidth="2px" Width="100%" CssClass="row">

                                                        <div class="clearfix" id="divSelectEmployee" runat="server">
                                                            <div class="col-md-3">
                                                                <label>Select Employee </label>
                                                                <div class="reltv">
                                                                    <asp:ListBox ID="lstAssociatedEmployee" CssClass="chsn" runat="server" Width="100%" data-placeholder="---Select---"></asp:ListBox>

                                                                </div>
                                                            </div>
                                                        </div>

                                                        <%--Rev 1.0 : "simple-select" class add --%>
                                                        <div class="col-md-3 simple-select">

                                                            <dxe:ASPxLabel ID="ASPxLabel15" Visible="false" runat="server" Text="Rating">
                                                            </dxe:ASPxLabel>
                                                            <dxe:ASPxLabel ID="ASPxLabel20" runat="server" Text="Customer Type" CssClass="inline">
                                                            </dxe:ASPxLabel>
                                                            <span style="text-align: left; float: left; font-size: medium; color: Red; width: 8px;">*</span>
                                                            <asp:DropDownList ID="cmbRating" Visible="false" runat="server" Width="160px">
                                                            </asp:DropDownList>

                                                            <asp:DropDownList ID="cmbLegalStatus" runat="server" Width="100%" Onfo>
                                                            </asp:DropDownList>
                                                        </div>
                                                        <%--     .................ID TYPE.........................--%>
                                                        <%--Rev 1.0 : "simple-select" class add --%>
                                                        <div class="col-md-3 simple-select" id="dvIdType" runat="server">

                                                            <dxe:ASPxLabel ID="ASPxLabel19" runat="server" Text="ID Type" CssClass="inline">
                                                            </dxe:ASPxLabel>

                                                            <asp:DropDownList ID="ddlIdType" runat="server" Width="100%">
                                                                <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                                <asp:ListItem Value="1">Phone</asp:ListItem>
                                                                <asp:ListItem Value="2">PAN</asp:ListItem>
                                                                <asp:ListItem Value="3">Aadhar No.</asp:ListItem>
                                                            </asp:DropDownList>

                                                        </div>
                                                        <%--     .................ID TYPE  End.........................--%>
                                                        <%--Rev 1.0 : "simple-select" class add --%>
                                                        <div class="col-md-3 simple-select" id="divClientBranch" runat="server">
                                                            <dxe:ASPxLabel ID="ASPxLabel3" Visible="false" runat="server" Text="Last Name">
                                                            </dxe:ASPxLabel>
                                                            <dxe:ASPxLabel ID="ASPxLabel10" runat="server" Text="Branch" CssClass="inline">
                                                            </dxe:ASPxLabel>
                                                            <span style="text-align: left; float: left; font-size: medium; color: Red; width: 8px;">*</span>
                                                            <asp:DropDownList ID="cmbBranch" runat="server" Width="100%">
                                                            </asp:DropDownList>
                                                            <dxe:ASPxTextBox ID="txtLastName" Visible="false" runat="server" Width="100%">
                                                            </dxe:ASPxTextBox>
                                                        </div>

                                                        <div class="col-md-3" id="dvUniqueId" runat="server">

                                                            <dxe:ASPxLabel ID="ASPxLabel13" Visible="false" runat="server" Text="Source">
                                                            </dxe:ASPxLabel>
                                                            <asp:DropDownList ID="cmbSource" Visible="false" runat="server" Width="160px">
                                                            </asp:DropDownList>
                                                            <dxe:ASPxLabel ID="ASPxLabel17" Visible="false" runat="server" Text="Refered By">
                                                            </dxe:ASPxLabel>
                                                            <span style="text-align: left; float: left; font-size: medium; color: Red; width: 8px;" id="td_star"
                                                                runat="server" visible="false">*
                                                            </span>
                                                            <asp:TextBox ID="txtReferedBy" Visible="false" runat="server" Width="160px"></asp:TextBox>


                                                            <dxe:ASPxLabel ID="ASPxLabel2" Visible="false" runat="server" Text="Middle Name" CssClass="inline">
                                                            </dxe:ASPxLabel>
                                                            <div>
                                                                <dxe:ASPxLabel ID="ASPxLabel12" runat="server" Text="Unique ID" CssClass="inline ">
                                                                </dxe:ASPxLabel>
                                                                <span id="ASPxLabelS12" style="text-align: left; float: left; font-size: medium; color: Red; width: 8px;" runat="server">*</span>

                                                            </div>
                                                           
                                                            <%--txt unique code--%>
                                                            <div style="position: relative" id="dvClentUcc" runat="server">
                                                                <dxe:ASPxTextBox ID="txtMiddleName" Visible="false" runat="server" Width="160px" CssClass="upper">
                                                                </dxe:ASPxTextBox>
                                                                <dxe:ASPxTextBox ID="txtClentUcc" runat="server" Width="100%" MaxLength="50" CssClass="upper" ClientInstanceName="ctxtClentUcc">
                                                                    <ClientSideEvents TextChanged="function(s,e){fn_ctxtClentUcc_Name_TextChanged()}" />
                                                                </dxe:ASPxTextBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtClentUcc" ValidationGroup="contact" SetFocusOnError="true" ToolTip="Mandatory" class="tp28 pullrightClass fa fa-exclamation-circle abs iconRed" ErrorMessage=""></asp:RequiredFieldValidator>
                                                                <asp:LinkButton ID="LinkButton1" runat="server" Visible="false" OnClick="LinkButton1_Click" Style="color: #cc3300; text-decoration: underline; font-size: 8pt;">Make System Generate UCC</asp:LinkButton>
                                                                <asp:Label ID="lblErr" Text="" runat="server" Visible="false"></asp:Label>
                                                            </div>
                                                        </div>
                                                         <%--rev srijeeta mantis issue 0024515--%>
                                                        <div class="col-md-3" runat="server" id="Div1">
                                                             <div>
                                                                <dxe:ASPxLabel ID="ASPxLabel42" runat="server" Text="Alternative Code" Width="" maxLength="100">
                                                                </dxe:ASPxLabel>
                                                                
                                                            </div>

                                                            <dxe:ASPxTextBox ID="ASPxTextBox1" runat="server" ClientInstanceName="ctxt_Custaltcode" Width="100%" MaxLength="100">
                                                                
                                                            </dxe:ASPxTextBox>
                                                            <span id="altcode" style="display: none" class="validclass"></span>
                                                        </div>
                                                        <%--end of rev srijeeta mantis issue 0024515--%>
                                                        <%--chinmoy added for Auto numbering start--%>
                                                        <div class="col-md-2 lblmTop8" id="ddl_Num" runat="server" style="display: none">

                                                            <label>
                                                                <dxe:ASPxLabel ID="lbl_NumberingScheme" Width="120px" runat="server" Text="Numbering Scheme">
                                                                </dxe:ASPxLabel>
                                                            </label>
                                                            <asp:DropDownList ID="ddl_numberingScheme" runat="server" Width="100%">
                                                            </asp:DropDownList>


                                                        </div>

                                                        <div class="col-md-2 lblmTop8" runat="server" id="dvCustDocNo" style="display: none">
                                                            <label>
                                                                <dxe:ASPxLabel ID="lbl_CustDocNo" runat="server" Text="Unique ID" Width="">
                                                                </dxe:ASPxLabel>
                                                                <span style="color: red">*</span>
                                                            </label>

                                                            <dxe:ASPxTextBox ID="txt_CustDocNo" runat="server" ClientInstanceName="ctxt_CustDocNo" Width="100%" MaxLength="50">
                                                                <ClientSideEvents TextChanged="function(s, e) {UniqueCodeCheck();}" />
                                                            </dxe:ASPxTextBox>
                                                            <span id="MandatorySlOrderNo" style="display: none" class="validclass">
                                                                <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory" /></span>

                                                        </div>
                                                         
                                                        <div class="col-md-3 hide">
                                                            <div style="display: none" class="labelt">
                                                                <dxe:ASPxLabel ID="ASPxLabel4" runat="server" Text="Unique ID">
                                                                </dxe:ASPxLabel>
                                                            </div>
                                                            <div style="display: none">
                                                                <dxe:ASPxTextBox ID="txtAliasName" runat="server" Width="100%" CssClass="upper">
                                                                </dxe:ASPxTextBox>
                                                            </div>
                                                        </div>
                                                        
                                                         
                                                        <div style="clear: both">
                                                        </div>
                                                        <div class="col-md-3">
                                                            <dxe:ASPxLabel Visible="false" ID="ASPxLabel1" runat="server" Text="Salutation">
                                                            </dxe:ASPxLabel>
                                                            <dxe:ASPxLabel ID="ASPxLabel5" runat="server" Text="Full Name" CssClass="inline">
                                                            </dxe:ASPxLabel>

                                                            <span style="text-align: left; float: left; font-size: medium; color: Red; width: 8px;">*</span>

                                                            <div style="position: relative">
                                                                <asp:DropDownList Visible="false" ID="CmbSalutation" runat="server" Width="160px">
                                                                </asp:DropDownList>
                                                                <dxe:ASPxTextBox ID="txtFirstNmae" runat="server" Width="100%" MaxLength="100" CssClass="upper" ClientInstanceName="ctxtFirstNmae">
                                                                    <%-- <ValidationSettings ValidationGroup="a">
                                                                        </ValidationSettings>--%>
                                                                </dxe:ASPxTextBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtFirstNmae" ValidationGroup="contact" SetFocusOnError="false" class="tp28 pullrightClass fa fa-exclamation-circle abs iconRed" ToolTip="Mandatory" ErrorMessage=""></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>


                                                        <div class="col-md-3 lblmTop8">
                                                            <%--lbl D.O.B F--%>
                                                            <label class="labelt">

                                                                <dxe:ASPxLabel ID="ASPxLabel18" runat="server" Text="Date of Birth">
                                                                </dxe:ASPxLabel>

                                                            </label>
                                                            <%--txt D.O.B F--%>
                                                            <div>

                                                                <dxe:ASPxDateEdit ID="txtDOB" runat="server" Width="100%" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctxtDOB"
                                                                    UseMaskBehavior="True">
                                                                    <ClientSideEvents DateChanged="DateValidateBirth" />

                                                                    <ButtonStyle Width="13px">
                                                                    </ButtonStyle>
                                                                </dxe:ASPxDateEdit>
                                                                <%--Rev 1.0--%>
                                                                <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                                                                <%--Rev end 1.0--%>
                                                            </div>
                                                        </div>
                                                        <%--Rev 1.0 : "simple-select" class add --%>
                                                        <div class="col-md-3 lblmTop8  simple-select">
                                                            <%-- lbl Nationality--%>
                                                            <label class="labelt">
                                                                <dxe:ASPxLabel ID="ASPxLabel34" Visible="false" runat="server" Text="Contact Status">
                                                                </dxe:ASPxLabel>
                                                                <dxe:ASPxLabel ID="ASPxLabel35" Width="120px" runat="server" Text="Nationality">
                                                                </dxe:ASPxLabel>
                                                            </label>
                                                            <%-- lbl Nationality--%>
                                                            <div style="position: relative">
                                                                <div class="hide">
                                                                    <asp:DropDownList ID="cmbContactStatus" runat="server" Width="100%">
                                                                    </asp:DropDownList>
                                                                </div>
                                                                <asp:DropDownList ID="ddlnational" runat="server" Width="100%">
                                                                    <%--<asp:ListItem Value="1">Indian</asp:ListItem>
                                                                    <asp:ListItem Value="2">Others</asp:ListItem>--%>
                                                                </asp:DropDownList>



                                                            </div>
                                                        </div>
                                                        <div class="col-md-3 visF lblmTop8" id="td_lAnniversary">
                                                            <label class="labelt">
                                                                <dxe:ASPxLabel ID="ASPxLabel21" runat="server" Text="Anniversary Dates">
                                                                </dxe:ASPxLabel>
                                                            </label>
                                                            <%--txt Anniversary--%>
                                                            <%--<div id="td_tAnniversary">--%>
                                                            <div class="visF">
                                                                <dxe:ASPxDateEdit ID="txtAnniversary" runat="server" Width="100%" EditFormat="Custom" ClientInstanceName="txtAnniversary"
                                                                    EditFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                                                                    <ClientSideEvents DateChanged="DateValidateBirth" />

                                                                    <ButtonStyle Width="13px">
                                                                    </ButtonStyle>
                                                                </dxe:ASPxDateEdit>
                                                                <%--Rev 1.0--%>
                                                                <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                                                                <%--Rev end 1.0--%>
                                                            </div>
                                                            <%--</div>--%>
                                                        </div>
                                                        <div style="clear: both;"></div>
                                                        <%--Rev 1.0 : "simple-select" class add --%>
                                                        <div class="col-md-3 visF lblmTop8 simple-select">
                                                            <label id="td_lGender" class="labelt">
                                                                <div class="visF">
                                                                    <dxe:ASPxLabel ID="ASPxLabel14" runat="server" Text="Gender">
                                                                    </dxe:ASPxLabel>
                                                                </div>
                                                            </label>
                                                            <div id="td_dGender">
                                                                <div class="visF">
                                                                    <asp:DropDownList ID="cmbGender" runat="server" Width="100%">
                                                                        <asp:ListItem Value="2">--Select--</asp:ListItem>
                                                                        <asp:ListItem Value="0">Male</asp:ListItem>
                                                                        <asp:ListItem Value="1">Female</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>
                                                        </div>


                                                        <%--<div class="col-md-3 visF">
                                                            <div id="td_lGender">
                                                                <div class="visF">
                                                                    <dxe:ASPxLabel ID="ASPxLabel14" runat="server" Text="Gender">
                                                                    </dxe:ASPxLabel>
                                                                </div>
                                                            </div>
                                                            <div id="td_dGender">
                                                                <div class="visF">
                                                                    <asp:DropDownList ID="cmbGender" runat="server" TabIndex="7" Width="100%">
                                                                        <asp:ListItem Value="2">--Select--</asp:ListItem>
                                                                        <asp:ListItem Value="0">Male</asp:ListItem>
                                                                        <asp:ListItem Value="1">Female</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-3 visF">
                                                            <div id="td_lMarital">
                                                                <div class="visF">
                                                                    <dxe:ASPxLabel ID="ASPxLabel16" runat="server" Text="Marital Status">
                                                                    </dxe:ASPxLabel>
                                                                </div>
                                                            </div>
                                                            <div id="td_dMarital">
                                                                <div class="visF">
                                                                    <asp:DropDownList ID="cmbMaritalStatus" runat="server" TabIndex="8" Width="100%">
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>
                                                        </div>--%>




                                                        <%-- Code  Added  By sanjib on 18122016 to change 3 fields postion--%>
                                                        <%--<div class="col-md-3">--%>
                                                        <%-- lbl Nationality--%>
                                                        <%--<div>
                                                                <dxe:ASPxLabel ID="ASPxLabel19" Visible="false" runat="server" Text="Contact Status">
                                                                </dxe:ASPxLabel>
                                                                <dxe:ASPxLabel ID="ASPxLabel32" Width="120px" runat="server" Text="Nationality">
                                                                </dxe:ASPxLabel>
                                                            </div>--%>
                                                        <%-- lbl Nationality--%>
                                                        <%-- <div style="position: relative">
                                                                <asp:DropDownList Visible="false" ID="cmbContactStatus" runat="server" Width="100%" TabIndex="27">
                                                                </asp:DropDownList>

                                                                <asp:DropDownList ID="ddlnational" TabIndex="9" runat="server" Width="100%">--%>
                                                        <%--<asp:ListItem Value="1">Indian</asp:ListItem>
                                                                <%--    <asp:ListItem Value="2">Others</asp:ListItem>--%>
                                                        <%-- </asp:DropDownList>



                                                            </div>
                                                        </div>--%>
                                                        <%-- Code  Added  By Priti on 15122016 to add 3 fields--%>
                                                        <asp:Panel ID="pnlCredit" runat="server">
                                                            <div class="clear"></div>
                                                            <div class="col-md-3 mt-24">
                                                                <div>
                                                                </div>
                                                                <div style="position: relative">
                                                                    <div class="checkbox" style="padding-left: 0px !important;">
                                                                        <label>
                                                                            <dxe:ASPxCheckBox ID="ChkCreditcard" runat="server"></dxe:ASPxCheckBox>
                                                                            <dxe:ASPxLabel ID="lblCreditcard" Width="120px" runat="server" Text="Credit Hold">
                                                                            </dxe:ASPxLabel>
                                                                        </label>
                                                                    </div>


                                                                </div>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <div class="labelt">
                                                                    <dxe:ASPxLabel ID="lblcreditDays" Width="120px" runat="server" Text="Credit Days">
                                                                    </dxe:ASPxLabel>
                                                                </div>
                                                                <div style="position: relative">
                                                                    <dxe:ASPxTextBox ID="txtcreditDays" runat="server" Width="100%" MaxLength="4" Text="0" onkeypress="return onlyNumbers();">
                                                                    </dxe:ASPxTextBox>
                                                                    <%-- <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="" ValidationExpression="^[0-9]&" controltovalidate="txtcreditDays"></asp:RegularExpressionValidator>--%>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <div class="labelt">
                                                                    <dxe:ASPxLabel ID="lblCreditLimit" Width="100%" runat="server" Text="Credit Limit">
                                                                    </dxe:ASPxLabel>
                                                                </div>
                                                                <div style="position: relative">
                                                                    <dxe:ASPxTextBox ID="txtCreditLimit" runat="server" Width="100%" MaxLength="40" Text="0">
                                                                        <%--     Code added By Priti on 16122016 to use decimal value--%>
                                                                        <MaskSettings Mask="<0..999999999999g>.<0..99g>" />
                                                                    </dxe:ASPxTextBox>
                                                                    <%--  ...end...--%>
                                                                </div>
                                                            </div>
                                                            <div class="clear"></div>
                                                        </asp:Panel>
                                                        <%-- .......end.....--%>
                                                        <%--  <div id="td_country" class="col-md-3" style="display:none;">--%>

                                                        <%-- <asp:TextBox ID="txtcountry" runat="server" Width="100%"
                                                                TabIndex="29"></asp:TextBox>--%>
                                                        <%-- </div>--%>
                                                        <%--Rev 1.0 : "simple-select" class add --%>
                                                        <div class="col-md-3 visF lblmTop8 simple-select">
                                                            <label id="td_lMarital" class="labelt">
                                                                <div class="visF">
                                                                    <dxe:ASPxLabel ID="ASPxLabel16" runat="server" Text="Marital Status">
                                                                    </dxe:ASPxLabel>
                                                                </div>
                                                            </label>
                                                            <div id="td_dMarital">
                                                                <div class="visF">
                                                                    <asp:DropDownList ID="cmbMaritalStatus" runat="server" Width="100%">
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-3 visF lblmTop8">
                                                            <label id="td_lMaritals" class="labelt">
                                                                <div class="visF">
                                                                    <dxe:ASPxLabel ID="txtContactStatusclient" runat="server" Text="Status">
                                                                    </dxe:ASPxLabel>
                                                                </div>
                                                            </label>




                                                            <div id="td_dMaritals4">
                                                                <div class="visF">
                                                                    <dxe:ASPxComboBox ID="cmbContactStatusclient" ClientInstanceName="cCmbStatus" runat="server" SelectedIndex="0"
                                                                        ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True">
                                                                        <Items>
                                                                            <dxe:ListEditItem Text="Active" Value="A" />
                                                                            <dxe:ListEditItem Text="Dormant" Value="D" />
                                                                        </Items>
                                                                        <ClientSideEvents SelectedIndexChanged="function(s,e){CheckContactStatus(s);}" />
                                                                    </dxe:ASPxComboBox>
                                                                </div>

                                                            </div>
                                                        </div>
                                                        <div class="clear"></div>
                                                        <div class="col-md-3 mainAccount">
                                                            <div class="padBot5 lblmTop8" style="display: block;">
                                                                <span>Main Account</span>
                                                            </div>
                                                            <div class="Left_Content">
                                                                <asp:ListBox ID="lstTaxRates_MainAccount" CssClass="chsn" runat="server" Font-Size="12px" Width="100%" data-placeholder="Select..." onchange="changeFunc();"></asp:ListBox>
                                                                <asp:HiddenField ID="hndTaxRates_MainAccount_hidden" runat="server" />
                                                                <asp:HiddenField ID="hdIsMainAccountInUse" runat="server" />
                                                            </div>
                                                        </div>

                                                        <div id="td_registered" class="labelt col-md-3 lblmTop8" runat="server">
                                                            <label>Registered?</label>
                                                            <div class="visF">


                                                                <asp:RadioButtonList runat="server" ID="radioregistercheck" RepeatDirection="Horizontal" Width="130px">
                                                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="No" Value="0" Selected="True"></asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </div>
                                                        </div>

                                                        <%--Debjyoti GstIN--%>


                                                        <div id="divGSTIN" class="col-md-4 forCustomer lblmTop8">
                                                            <label class="labelt">GSTIN   </label>
                                                            <span id="spanmandategstn" style="color: red; display: none;">*</span>
                                                            <div class="relative">
                                                                <ul class="nestedinput">
                                                                    <li>
                                                                        <dxe:ASPxTextBox ID="txtGSTIN1" ClientInstanceName="ctxtGSTIN111" MaxLength="2" runat="server" Width="33px">
                                                                            <ClientSideEvents KeyPress="function(s,e){ fn_AllowonlyNumeric(s,e);}" />
                                                                        </dxe:ASPxTextBox>
                                                                    </li>
                                                                    <li class="dash">- </li>
                                                                    <li>
                                                                        <dxe:ASPxTextBox ID="txtGSTIN2" ClientInstanceName="ctxtGSTIN222" MaxLength="10" runat="server" Width="90px">
                                                                            <ClientSideEvents KeyUp="Gstin2TextChanged" />
                                                                            <%--<ClientSideEvents TextChanged="function(s, e) {Gstin2ValueChanged();}" />--%>
                                                                        </dxe:ASPxTextBox>
                                                                    </li>
                                                                    <li class="dash">- </li>
                                                                    <li>
                                                                        <dxe:ASPxTextBox ID="txtGSTIN3" ClientInstanceName="ctxtGSTIN333" MaxLength="3" runat="server" Width="50px">
                                                                            <ClientSideEvents KeyUp="Gstin3TextChanged" />
                                                                        </dxe:ASPxTextBox>
                                                                        <span id="invalidGst" class="fa fa-exclamation-circle iconRed " style="color: red; display: none; padding-left: 9px; left: 302px;" title="Invalid GSTIN"></span>
                                                                    </li>
                                                                </ul>
                                                                <a href="#" onclick="validateGSTIN();" style="padding-left: 10px">Validate GST</a>
                                                                <input class="hide" id="myInput" />
                                                            </div>
                                                        </div>
                                                        <div class="clear"></div>
                                                        <div class="col-md-3 visF lblmTop8" id="td_Applicablefrom">
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

                                                                <%--<input type="text"  onkeyup="dateValidationFormat(event)" onkeypress="return isDateKey(event)" maxlength="10" class="flatpickr"/>--%>
                                                            </div>
                                                        </div>
                                                        <%--Code Added By Sam on 09022018 for Mantis Issue 0015725 Section Start--%>

                                                        <div class="col-md-2 visF lblmTop8" id="printName" runat="server">
                                                            <label class="labelt">
                                                                <dxe:ASPxLabel ID="lblprntName" runat="server" Text="Print Name">
                                                                </dxe:ASPxLabel>
                                                            </label>
                                                            <div class="visF">
                                                                <dxe:ASPxTextBox ID="txtPname" ClientInstanceName="ctxtPname" MaxLength="50" runat="server" Width="100%">
                                                                </dxe:ASPxTextBox>
                                                            </div>

                                                            <%--Code Added By Sam on 09022018 for Mantis Issue 0015725 Section End--%>
                                                        </div>

                                                        <div class="col-md-2" id="dvTCSApplicable" runat="server" style="display: none">

                                                            <div class="Left_Content" style="padding-top: 26px">
                                                                <dxe:ASPxCheckBox ID="TCSApplicable" ClientInstanceName="cTCSApplicable" Checked="false" Text="TCS Applicable" TextAlign="Right" runat="server">
                                                                </dxe:ASPxCheckBox>
                                                            </div>

                                                        </div>

                                                        <div class="col-md-3 visF lblmTop8" id="dvTaxDeducteeType" runat="server" style="display: none">
                                                            <label class="labelt">
                                                                <dxe:ASPxLabel ID="ASPxLabel37" runat="server" Text="Tax Entity Type">
                                                                </dxe:ASPxLabel>
                                                            </label>
                                                            <div class="visF">
                                                                <dxe:ASPxComboBox ID="cmbTaxdeducteedType" ClientInstanceName="ccmbTaxdeducteedType" runat="server" Width="200px">
                                                                    <Items>
                                                                        <dxe:ListEditItem Selected="true" Text="Select" Value="0" />
                                                                        <dxe:ListEditItem Text="Government" Value="G" />
                                                                        <dxe:ListEditItem Text="Non-government" Value="NG" />
                                                                        <dxe:ListEditItem Text="Others" Value="O" />
                                                                    </Items>
                                                                </dxe:ASPxComboBox>
                                                            </div>

                                                            <%--Code Added By Sam on 09022018 for Mantis Issue 0015725 Section End--%>
                                                        </div>



                                                        <%-- Add section for Transaction Category--%>
                                                        <asp:HiddenField runat="server" ID="hdnActiveEInvoice" />
                                                        <div class="col-md-3 visF lblmTop8" id="DivTransactionCategory" runat="server" style="display: none">
                                                            <label class="labelt">
                                                                <dxe:ASPxLabel ID="ASPxLabel38" runat="server" Text="Transaction Category">
                                                                </dxe:ASPxLabel>
                                                            </label>
                                                            <div class="visF">
                                                                <dxe:ASPxComboBox ID="cmbTransCategory" ClientInstanceName="ccmbTransCategory" runat="server" Width="200px">
                                                                    <Items>
                                                                        <dxe:ListEditItem Text="Select" Value="0" />
                                                                        <dxe:ListEditItem Selected="true" Text="B2C" Value="B2C" />
                                                                        <dxe:ListEditItem Text="B2B" Value="B2B" />
                                                                        <dxe:ListEditItem Text="SEZWP" Value="SEZWP" />
                                                                        <dxe:ListEditItem Text="SEZWOP" Value="SEZWOP" />
                                                                        <dxe:ListEditItem Text="EXPWP" Value="EXPWP" />
                                                                        <dxe:ListEditItem Text="EXPWOP" Value="EXPWOP" />
                                                                        <dxe:ListEditItem Text="DEXP" Value="DEXP" />
                                                                    </Items>
                                                                </dxe:ASPxComboBox>
                                                            </div>

                                                            <%--Code Added By Sam on 09022018 for Mantis Issue 0015725 Section End--%>
                                                        </div>
                                                        <%-- Add section for Transaction Category--%>

                                                        <div class="col-md-3 visF lblmTop8" id="divTDSDeductee" runat="server">
                                                            <label class="labelt">
                                                                <dxe:ASPxLabel ID="ASPxLabel36" runat="server" Text="Tds Deductee Type">
                                                                </dxe:ASPxLabel>
                                                            </label>
                                                            <div class="visF">
                                                                <dxe:ASPxComboBox ID="cmbTDS" ClientInstanceName="ccmbTDS" runat="server" Width="200px">
                                                                    <Items>
                                                                        <dxe:ListEditItem Selected="true" Text="Select" Value="0" />
                                                                        <dxe:ListEditItem Text="Individual/HUF" Value="1" />
                                                                        <dxe:ListEditItem Text="Others" Value="2" />
                                                                    </Items>
                                                                </dxe:ASPxComboBox>
                                                            </div>

                                                            <%--Code Added By Sam on 09022018 for Mantis Issue 0015725 Section End--%>
                                                        </div>



                                                        <div class="col-md-3 visF lblmTop8" id="td_EnrollmentId" runat="server">
                                                            <label class="labelt">
                                                                <dxe:ASPxLabel ID="ASPxLabel32" runat="server" Text="Transporter Enrolment ID (TRANSIN)  ">
                                                                </dxe:ASPxLabel>
                                                            </label>
                                                            <div class="visF">
                                                                <dxe:ASPxTextBox ID="txt_EnrollmentId" ClientInstanceName="ctxt_EnrollmentId" MaxLength="50" runat="server" Width="200px">
                                                                </dxe:ASPxTextBox>
                                                            </div>

                                                            <%--Code Added By Sam on 09022018 for Mantis Issue 0015725 Section End--%>
                                                        </div>

                                                        <div class="col-md-2 lblmTop8" id="DivServiceBranch" runat="server">
                                                            <label>
                                                                <dxe:ASPxLabel ID="ASPxLabel39" Width="120px" runat="server" Text="Service Branch">
                                                                </dxe:ASPxLabel>
                                                            </label>
                                                            <asp:DropDownList ID="ddlServiceBranch" runat="server" Width="100%">
                                                            </asp:DropDownList>
                                                        </div>

                                                        <div class="col-md-3">
                                                            <label>
                                                                <dxe:ASPxLabel ID="ASPxLabel40" Width="120px" runat="server" Text="Deductee Type">
                                                                </dxe:ASPxLabel>
                                                            </label>
                                                            <dxe:ASPxComboBox ID="aspxDeducteesNew" ClientInstanceName="caspxDeducteesNew" runat="server" SelectedIndex="0"
                                                                ValueType="System.String" Width="100%" EnableSynchronization="True">
                                                            </dxe:ASPxComboBox>
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


                                                        <div class="clear"></div>
                                                        <%--Debjyoti GstIN--%>
                                                        <%-- Start Tranporter Vechile No --%>
                                                        <asp:Panel ID="pnlVehicleNo" runat="server" CssClass="col-md-6">
                                                            <label style="display: block;">Vehicle No.</label>
                                                            <%-- <table style="width: 200px;" class="pdri">
                                                            <tr>
                                                                <td style="width: 180px">
                                                                    <label style="display: block;">Vehicle No.</label>
                                                                </td>
                                                               
                                                            </tr>
                                                        </table>--%>
                                                            <table id="executiveTable" style="width: 320px;" class="nbackBtn" runat="server">
                                                                <tr>
                                                                    <td style="padding-right: 15px">
                                                                        <input type="text" maxlength="18" class='upper' />
                                                                    </td>

                                                                    <td>
                                                                        <button type="button" class="btn btn-primary btn-xs" onclick="AddNewexecutive()"><i class="fa fa-plus-circle"></i></button>
                                                                        <button type="button" class="btn btn-danger btn-xs" onclick="removeExecutive(this.parentNode.parentNode)"><i class="fa fa-times-circle"></i></button>
                                                                    </td>

                                                                </tr>
                                                            </table>
                                                            <asp:HiddenField ID="VehicleNo_hidden" runat="server" />
                                                        </asp:Panel>
                                                        <%-- END Tranporter Vechile No --%>
                                                        <div class="clear"></div>
                                                        <asp:Panel ID="PanelDocumentSegment" runat="server" CssClass="col-md-12">
                                                            <label style="display: block;" class="dSegment">Document Segment</label>
                                                            <table id="Table1" style="width: 90%" class="nbackBtn padTble" runat="server">
                                                                <tr>
                                                                    <td style="width: 120px" class="gHesder">Segment</td>
                                                                    <td class="gHesder" style="width: 100px; text-align: right">Size
                                                                    </td>
                                                                    <td class="gHesder">Name(Used For)
                                                                    </td>
                                                                    <td class="gHesder">Mandatory For
                                                                    </td>
                                                                </tr>
                                                                <tr class="tdpTop5">
                                                                    <td style="font-weight: 500;">Segment1
                                                                    </td>
                                                                    <td>
                                                                        <dxe:ASPxTextBox ID="txtSegment1" runat="server" ClientInstanceName="ctxtSegment1" Width="100%" RightToLeft="True" MaxLength="2" Text="0" onkeypress="return onlyNumbers();">
                                                                        </dxe:ASPxTextBox>
                                                                    </td>
                                                                    <td>
                                                                        <dxe:ASPxTextBox ID="txtUsedFor1" runat="server" Width="100%" ClientInstanceName="ctxtUsedFor1">
                                                                        </dxe:ASPxTextBox>
                                                                    </td>
                                                                    <td class="mttHover">
                                                                        <%--<dxe:ASPxComboBox ID="ddlSegmentMandatory1" runat="server" ClientInstanceName="cddlSegmentMandatory1" Width="100%">
                                                                               
                                                                        </dxe:ASPxComboBox>--%>
                                                                        <select class="form-control " id="cddlSegmentMandatory1" multiple></select>

                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="font-weight: 500;">Segment2
                                                                    </td>
                                                                    <td>
                                                                        <dxe:ASPxTextBox ID="txtSegment2" runat="server" ClientInstanceName="ctxtSegment2" RightToLeft="True" Width="100%" MaxLength="2" Text="0" onkeypress="return onlyNumbers();" onchange="CheckSegZero()">
                                                                        </dxe:ASPxTextBox>
                                                                    </td>
                                                                    <td>
                                                                        <dxe:ASPxTextBox ID="txtUsedFor2" runat="server" Width="100%" ClientInstanceName="ctxtUsedFor2">
                                                                        </dxe:ASPxTextBox>
                                                                    </td>
                                                                    <td class="mttHover">
                                                                        <%--<dxe:ASPxComboBox ID="ddlSegmentMandatory2" runat="server" ClientInstanceName="cddlSegmentMandatory2" Width="100%">
                                                                        </dxe:ASPxComboBox>--%>

                                                                        <select class="form-control " id="cddlSegmentMandatory2" multiple></select>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="font-weight: 500;">Segment3
                                                                    </td>
                                                                    <td>
                                                                        <dxe:ASPxTextBox ID="txtSegment3" runat="server" ClientInstanceName="ctxtSegment3" RightToLeft="True" Width="100%" MaxLength="2" Text="0" onkeypress="return onlyNumbers();" onchange="CheckSegZero1()">
                                                                        </dxe:ASPxTextBox>
                                                                    </td>
                                                                    <td>
                                                                        <dxe:ASPxTextBox ID="txtUsedFor3" runat="server" Width="100%" ClientInstanceName="ctxtUsedFor3">
                                                                        </dxe:ASPxTextBox>
                                                                    </td>
                                                                    <td class="mttHover">
                                                                        <%--<dxe:ASPxComboBox ID="ddlSegmentMandatory3" runat="server" ClientInstanceName="cddlSegmentMandatory3" Width="100%">
                                                                        </dxe:ASPxComboBox>--%>
                                                                        <select class="form-control " id="cddlSegmentMandatory3" multiple></select>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="font-weight: 500;">Segment4
                                                                    </td>
                                                                    <td>
                                                                        <dxe:ASPxTextBox ID="txtSegment4" runat="server" ClientInstanceName="ctxtSegment4" RightToLeft="True" Width="100%" MaxLength="2" Text="0" onkeypress="return onlyNumbers();" onchange="CheckSegZero2()">
                                                                        </dxe:ASPxTextBox>
                                                                    </td>
                                                                    <td>
                                                                        <dxe:ASPxTextBox ID="txtUsedFor4" runat="server" Width="100%" ClientInstanceName="ctxtUsedFor4">
                                                                        </dxe:ASPxTextBox>
                                                                    </td>
                                                                    <td class="mttHover">
                                                                        <%--<dxe:ASPxComboBox ID="ddlSegmentMandatory4" runat="server" ClientInstanceName="cddlSegmentMandatory4" Width="100%">
                                                                        </dxe:ASPxComboBox>--%>
                                                                        <select class="form-control " id="cddlSegmentMandatory4" multiple></select>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="font-weight: 500;">Segment5
                                                                    </td>
                                                                    <td>
                                                                        <dxe:ASPxTextBox ID="txtSegment5" runat="server" ClientInstanceName="ctxtSegment5" RightToLeft="True" Width="100%" MaxLength="2" Text="0" onkeypress="return onlyNumbers();" onchange="CheckSegZero3()">
                                                                        </dxe:ASPxTextBox>
                                                                    </td>
                                                                    <td>
                                                                        <dxe:ASPxTextBox ID="txtUsedFor5" runat="server" Width="100%" ClientInstanceName="ctxtUsedFor5">
                                                                        </dxe:ASPxTextBox>
                                                                    </td>
                                                                    <td class="mttHover">
                                                                        <%-- <dxe:ASPxComboBox ID="ddlSegmentMandatory5" runat="server" ClientInstanceName="cddlSegmentMandatory5" Width="100%">
                                                                        </dxe:ASPxComboBox>--%>
                                                                        <select class="form-control " id="cddlSegmentMandatory5" multiple></select>
                                                                    </td>
                                                                </tr>
                                                                <tr class="hide">
                                                                    <td></td>
                                                                    <td></td>
                                                                    <td></td>
                                                                    <td>
                                                                        <dxe:ASPxComboBox ID="ddlSegmentMandatory6" runat="server" ClientInstanceName="cddlSegmentMandatory6" Width="100%">
                                                                        </dxe:ASPxComboBox>
                                                                    </td>
                                                                </tr>
                                                            </table>

                                                            <asp:HiddenField ID="HdnDocumentSegment" runat="server" />
                                                            <asp:HiddenField ID="hdnUsedFor" runat="server" />
                                                            <asp:HiddenField ID="hdnMandatoryFor" runat="server" />
                                                        </asp:Panel>
                                                        <%--</div>--%>

                                                        <table class="TableMain100">

                                                            <tr style="display: none">
                                                                <td style="width: 64px">
                                                                    <dxe:ASPxLabel ID="ASPxLabel6" runat="server" Text="Profession">
                                                                    </dxe:ASPxLabel>
                                                                </td>
                                                                <td style="text-align: left; width: 163px;">
                                                                    <asp:DropDownList ID="cmbProfession" runat="server" Width="160px">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td style="width: 76px">
                                                                    <dxe:ASPxLabel ID="ASPxLabel9" runat="server" Text="Designation">
                                                                    </dxe:ASPxLabel>
                                                                </td>
                                                                <td style="text-align: left; font-size: medium; color: Red; width: 8px;"></td>
                                                                <td style="text-align: left; width: 164px;">
                                                                    <asp:DropDownList ID="cmbDesignation" runat="server" Width="160px">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td style="width: 68px">
                                                                    <dxe:ASPxLabel ID="ASPxLabel8" runat="server" Text="Job Responsibility">
                                                                    </dxe:ASPxLabel>
                                                                </td>
                                                                <td style="text-align: left; width: 166px;">
                                                                    <asp:DropDownList ID="cmbJobResponsibility" runat="server" Width="160px">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td style="width: 61px">
                                                                    <dxe:ASPxLabel ID="ASPxLabel7" runat="server" Text="Organization">
                                                                    </dxe:ASPxLabel>
                                                                </td>
                                                                <td style="text-align: left">
                                                                    <dxe:ASPxTextBox ID="txtOrganization" runat="server" Width="160px">
                                                                    </dxe:ASPxTextBox>
                                                                </td>
                                                            </tr>

                                                            <tr id="Trprofessionother" style="display: none">
                                                                <td style="width: 64px">
                                                                    <dxe:ASPxLabel ID="ASPxLabel31" runat="server" Text="Other Detail">
                                                                    </dxe:ASPxLabel>
                                                                </td>
                                                                <td style="width: 76px">
                                                                    <asp:TextBox ID="txtotheroccu" runat="server" Width="160px"></asp:TextBox>
                                                                </td>
                                                                <td style="text-align: left; font-size: medium; color: Green; width: 4px;">*
                                                                </td>
                                                                <td colspan="9"></td>
                                                            </tr>

                                                            <tr style="display: none">
                                                                <td style="width: 64px">
                                                                    <dxe:ASPxLabel ID="ASPxLabel22" runat="server" Text="Education">
                                                                    </dxe:ASPxLabel>
                                                                </td>
                                                                <td style="text-align: left; width: 163px;">
                                                                    <asp:DropDownList ID="cmbEducation" runat="server" Width="160px">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td style="width: 76px">
                                                                    <dxe:ASPxLabel ID="ASPxLabel11" runat="server" Text="Industry/Sector">
                                                                    </dxe:ASPxLabel>
                                                                </td>
                                                                <td style="text-align: left; font-size: medium; color: Red; width: 8px;"></td>
                                                                <td style="width: 164px">
                                                                    <asp:DropDownList ID="cmbIndustry" runat="server" Width="160px">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td style="width: 68px">
                                                                    <dxe:ASPxLabel ID="ASPxLabel23" runat="server" Text="Date Of Regn/Intr">
                                                                    </dxe:ASPxLabel>
                                                                </td>
                                                                <td style="text-align: left; width: 166px;">
                                                                    <dxe:ASPxDateEdit ID="txtDateRegis" runat="server" Width="156px" EditFormat="Custom"
                                                                        EditFormatString="dd MM yyyy" UseMaskBehavior="True">
                                                                        <ButtonStyle Width="13px">
                                                                        </ButtonStyle>
                                                                    </dxe:ASPxDateEdit>

                                                                    <dxe:ASPxLabel ID="ASPxLabel33" Visible="false" runat="server" Text="Anniversary Date">
                                                                    </dxe:ASPxLabel>



                                                                </td>
                                                                <td style="width: 61px" valign="top">
                                                                    <dxe:ASPxLabel ID="ASPxLabel28" Visible="false" runat="server" Text="Blood Group">
                                                                    </dxe:ASPxLabel>
                                                                </td>

                                                                <td>
                                                                    <asp:DropDownList ID="cmbBloodgroup" Visible="false" runat="server" Width="85px">
                                                                        <asp:ListItem Value="NULL">--Select--</asp:ListItem>
                                                                        <asp:ListItem Value="A+">A+</asp:ListItem>
                                                                        <asp:ListItem Value="A-">A-</asp:ListItem>
                                                                        <asp:ListItem Value="B+">B+</asp:ListItem>
                                                                        <asp:ListItem Value="B-">B-</asp:ListItem>
                                                                        <asp:ListItem Value="AB+">AB+</asp:ListItem>
                                                                        <asp:ListItem Value="AB-">AB-</asp:ListItem>
                                                                        <asp:ListItem Value="O+">O+</asp:ListItem>
                                                                        <asp:ListItem Value="O-">O-</asp:ListItem>
                                                                        <asp:ListItem Value="N/A">N/A</asp:ListItem>
                                                                    </asp:DropDownList><br />
                                                                    <asp:Label ID="l1" runat="server" Text="Allow web logIn"></asp:Label>
                                                                    <asp:CheckBox ID="chkAllow" runat="server" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td style="width: 961px">
                                                    <table class="TableMain100">
                                                        <tr style="display: none">
                                                            <td style="width: 64px">Branch name
                                                            </td>
                                                            <td style="width: 250px">ddl breach lst
                                                            </td>
                                                            <td style="width: 76px">
                                                                <dxe:ASPxLabel ID="ASPxLabel24" runat="server" Text="SPOC">
                                                                </dxe:ASPxLabel>
                                                            </td>
                                                            <td style="text-align: left; font-size: medium; color: Red; width: 4px;"></td>
                                                            <td style="width: 164px">
                                                                <asp:TextBox ID="txtRPartner" runat="server" Width="160px"></asp:TextBox>
                                                            </td>
                                                            <td style="width: 68px">
                                                                <dxe:ASPxLabel ID="ASPxLabel25" runat="server" Text="SPOC Email">
                                                                </dxe:ASPxLabel>
                                                            </td>
                                                            <td style="text-align: left; font-size: medium; color: Red; width: 8px;"></td>
                                                            <td style="width: 166px">
                                                                <asp:TextBox ID="TxtEmail" runat="server" Width="130px"></asp:TextBox></td>
                                                            <td style="width: 61px">
                                                                <dxe:ASPxLabel ID="ASPxLabel26" runat="server" Text="SPOC Phone">
                                                                </dxe:ASPxLabel>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="TxtPhone" runat="server" Width="130px"></asp:TextBox>
                                                            </td>
                                                        </tr>


                                                        <tr id="TrContact" style="display: none">
                                                            <td>
                                                                <dxe:ASPxLabel ID="ASPxLabel27" runat="server" Text="Reason">
                                                                </dxe:ASPxLabel>
                                                            </td>
                                                            <td style="text-align: left;" colspan="3">
                                                                <asp:TextBox ID="TxtContactStatus" runat="server" TextMode="MultiLine" Width="404px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr id="tr_contactperson">
                                                            <%--<td colspan="6"></td>
                                                        <td id="td_green" style="text-align: left; font-size: medium; color: Green; width: 8px;">*
                                                        </td>
                                                        <td id="td_red" style="text-align: left; font-size: medium; color: Red; width: 8px;">*
                                                        </td>--%>

                                                            <%--..................... Commented By Sam on 27092016........................--%>
                                                            <%-- <td id="td_only" style="padding-left: 19px">
                                                                <a href="javascript:void(0);" class="btn btn-success"
                                                                    onclick="OnContactInfoClick('<%#Eval("InternalId") %>')">Add Contact Person</a>
                                                            </td>--%>

                                                            <%--  ..................... Commented By Sam on 27092016 end........................--%>
                                                            <td id="td_one" style="padding-left: 19px">
                                                                <a href="javascript:void(0);" class="btn btn-success" style="display: none"
                                                                    onclick="OnContactInfoClick('<%#Eval("InternalId") %>')">Add 1 Contact Person</a>
                                                            </td>
                                                            <td id="td_two" style="padding-left: 19px">
                                                                <a href="javascript:void(0);" class="btn btn-success" style="display: none"
                                                                    onclick="OnContactInfoClick('<%#Eval("InternalId") %>')">Add 3 Contact Persons</a>
                                                            </td>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr style="display: none">
                                                <td>
                                                    <table>
                                                        <tr id="Trincorporation">
                                                            <td>
                                                                <dxe:ASPxLabel ID="ASPxLabel29" runat="server" Text="Place Of Incorporation">
                                                                </dxe:ASPxLabel>
                                                            </td>
                                                            <td style="text-align: left; font-size: medium; color: Red; width: 8px;">*
                                                            </td>
                                                            <td style="text-align: left;">
                                                                <asp:TextBox ID="txtincorporation" runat="server" Width="300px"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <dxe:ASPxLabel ID="ASPxLabel30" runat="server" Text="Date of commencement of business">
                                                                </dxe:ASPxLabel>
                                                            </td>
                                                            <td style="text-align: left; font-size: medium; color: Red; width: 8px;">*
                                                            </td>
                                                            <%--<td style="text-align: left;" colspan="3">
                                                                <asp:TextBox ID="TextBox2" runat="server" TextMode="MultiLine" Width="404px"
                                                                    TabIndex="29"></asp:TextBox>
                                                            </td>--%>
                                                            <td style="text-align: left; width: 164px;">
                                                                <dxe:ASPxDateEdit ID="txtFromDate" EditFormatString="dd-MM-yyyy" runat="server" EditFormat="Custom" Width="105px"
                                                                    UseMaskBehavior="True" DateOnError="Today">
                                                                    <ButtonStyle Width="13px">
                                                                    </ButtonStyle>
                                                                    <%--<DropDownButton Text="From">
                                                                    </DropDownButton>--%>
                                                                </dxe:ASPxDateEdit>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td></td>
                                                            <td></td>
                                                            <td style="text-align: left;"></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr id="TrLang" runat="server" visible="false">
                                                <td id="Td1" runat="server" style="width: 961px">
                                                    <asp:Panel ID="Panel2" Visible="false" runat="server" Width="100%" BorderColor="White" BorderWidth="1px">
                                                        <table class="TableMain100">
                                                            <tr>
                                                                <td>
                                                                    <table class="TableMain100">
                                                                        <tr>
                                                                            <td colspan="2" style="text-align: left">
                                                                                <span style="color: blue; font-size: 10pt;">Language Proficiencies </span>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 50%; vertical-align: top">
                                                                                <asp:Panel ID="PnlSpLAng" runat="server" Width="100%" BorderColor="White" BorderWidth="1px">
                                                                                    <table>
                                                                                        <tr>
                                                                                            <td style="text-align: left; color: Blue; font-size: small">Can Speak
                                                                                            </td>
                                                                                            <td style="vertical-align: top" class="gridcellleft">
                                                                                                <asp:TextBox ID="LitSpokenLanguage" runat="server" ForeColor="Maroon" BackColor="Transparent"
                                                                                                    BorderStyle="None" Width="309px"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td style="text-align: left; vertical-align: top; font-size: x-small; color: Red;">
                                                                                                <a href="frmLanguages.aspx?id=''&status=speak" onclick="window.open(this.href,'popupwindow','left=120,top=170,height=400,width=200,scrollbars=no,toolbar=no,location=center,menubar=no'); return false;"
                                                                                                    style="font-size: x-small; color: Red;">click to add</a>
                                                                                            </td>
                                                                                            <td></td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </asp:Panel>
                                                                            </td>
                                                                            <td style="width: 50%; vertical-align: top">
                                                                                <asp:Panel ID="PnlWrLang" runat="server" Width="100%" BorderColor="White" BorderWidth="1px">
                                                                                    <table>
                                                                                        <tr>
                                                                                            <td style="text-align: left; color: Blue; font-size: small">Can Write
                                                                                            </td>
                                                                                            <td style="vertical-align: top" class="gridcellleft">
                                                                                                <asp:TextBox ID="LitWrittenLanguage" runat="server" ForeColor="Maroon" BackColor="Transparent"
                                                                                                    BorderStyle="None" Width="313px"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td style="text-align: left; vertical-align: top; font-size: x-small; color: Red;">
                                                                                                <a href="frmLanguages.aspx?id=''&status=write" onclick="window.open(this.href,'popupwindow','left=120,top=170,height=400,width=200,scrollbars=no,toolbar=no,location=center,menubar=no'); return false;"
                                                                                                    style="color: Red; font-size: x-small;">click to add</a>
                                                                                            </td>
                                                                                            <td></td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </asp:Panel>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                    <asp:Label ID="lblmessage" runat="server" ForeColor="Red"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="display: none;">
                                                    <asp:TextBox ID="txtRPartner_hidden" runat="server" BackColor="White" BorderColor="White"
                                                        BorderStyle="None" ForeColor="White"></asp:TextBox>
                                                    <asp:TextBox ID="txtReferedBy_hidden" runat="server" BackColor="White" BorderColor="White"
                                                        BorderStyle="None" ForeColor="White"></asp:TextBox>
                                                    <asp:TextBox ID="txtcountry_hidden" runat="server" BackColor="silver"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                        <div id="table_leads" class="row" runat="server">

                                            <%--chinmoy added for Auto numbering start--%>
                                            <div class="col-md-3 lblmTop8" id="LDddl_Num" runat="server" style="display: none">

                                                <label>
                                                    <dxe:ASPxLabel ID="LDlbl_NumberingScheme" Width="120px" runat="server" Text="Numbering Scheme">
                                                    </dxe:ASPxLabel>
                                                </label>
                                                <asp:DropDownList ID="LDddl_numberingScheme" runat="server" Width="100%">
                                                </asp:DropDownList>


                                            </div>

                                            <div class="col-md-3 lblmTop8" runat="server" id="LDdvCustDocNo" style="display: none">
                                                <label>
                                                    <dxe:ASPxLabel ID="LDlbl_CustDocNo" runat="server" Text="Unique ID" Width="">
                                                    </dxe:ASPxLabel>
                                                    <span style="color: red">*</span>
                                                </label>

                                                <dxe:ASPxTextBox ID="LDtxt_CustDocNo" runat="server" ClientInstanceName="LDctxt_CustDocNo" Width="100%" MaxLength="50">
                                                    <ClientSideEvents TextChanged="function(s, e) {UniqueLeadCodeCheck();}" />
                                                </dxe:ASPxTextBox>


                                            </div>
                                            <%--end--%>
                                            <div class="col-md-3" runat="server" id="DvLDUniqueId">
                                                <label>
                                                    <dxe:ASPxLabel ID="LDASPxLabel12" runat="server" Text="Unique ID" CssClass="pdl8"></dxe:ASPxLabel>
                                                    <span style="color: red;">*</span>

                                                </label>
                                                <div style="position: relative">
                                                    <asp:TextBox ID="LDtxtClentUcc" runat="server" CssClass="form-control" MaxLength="50" Width="100%">
                                                    </asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="LDtxtClentUcc" ValidationGroup="contact" SetFocusOnError="true" class="tp2 fa fa-exclamation-circle iconRed" ToolTip="Mandatory"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                   
                                            <div class="col-md-3">
                                                <label>
                                                    <dxe:ASPxLabel ID="LDASPxLabel1" runat="server" Text="Salutation" Width="130px"></dxe:ASPxLabel>
                                                </label>
                                                <div>
                                                    <asp:DropDownList ID="LDCmbSalutation" CssClass="form-control" runat="server" Width="100%">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="clear"></div>
                                            <div class="col-md-3">
                                                <label>
                                                    <dxe:ASPxLabel ID="LDASPxLabel5" runat="server" Text="First Name" CssClass="pdl8"></dxe:ASPxLabel>
                                                    <span style="color: red;">*</span>

                                                </label>
                                                <div style="position: relative;">
                                                    <asp:TextBox ID="LDtxtFirstNmae" runat="server" CssClass="form-control text-uppercase" Style="text-transform: uppercase" MaxLength="20" Width="100%">
                                                    </asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator111" runat="server" ControlToValidate="LDtxtFirstNmae" ValidationGroup="contact"
                                                        SetFocusOnError="true" ToolTip="Mandatory" class="tp2 fa fa-exclamation-circle iconRed" ErrorMessage=""></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>
                                                    <dxe:ASPxLabel ID="LDASPxLabel2" runat="server" Text="Middle Name" CssClass="pdl8" Width="130px"></dxe:ASPxLabel>
                                                </label>
                                                <div style="">
                                                    <asp:TextBox ID="LDtxtMiddleName" runat="server" CssClass="form-control" MaxLength="20" Width="100%">
                                                    </asp:TextBox>

                                                </div>
                                                <div style="text-align: right; height: 1px">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="LDtxtFirstNmae"
                                                        Display="Dynamic" ErrorMessage="Mandatory" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>
                                                    <dxe:ASPxLabel ID="LDASPxLabel3" runat="server" Text="Last Name"></dxe:ASPxLabel>



                                                </label>
                                                <div style="position: relative">
                                                    <asp:TextBox ID="LDtxtLastName" runat="server" CssClass="form-control" MaxLength="20" Width="100%">
                                                    </asp:TextBox>

                                                </div>
                                            </div>
                                            <div class="clear"></div>
                                            <div class="col-md-3" style="display: none">
                                                <label>
                                                    <dxe:ASPxLabel ID="LDASPxLabel4" runat="server" Text="Unique ID" CssClass="pdl8"></dxe:ASPxLabel>
                                                </label>
                                                <div>
                                                    <asp:TextBox ID="LDtxtAliasName" runat="server" CssClass="form-control" Width="100%" MaxLength="50">
                                                    </asp:TextBox>
                                                </div>
                                            </div>
                                              
                                            <div class="col-md-3">
                                                <label>
                                                    <dxe:ASPxLabel ID="LDASPxLabel6" runat="server" Text="Profession" CssClass="pdl8" Width="130px"></dxe:ASPxLabel>
                                                </label>
                                                <div>

                                                    <asp:DropDownList ID="LDcmbProfession" runat="server" CssClass="form-control" Width="100%">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>
                                                    <dxe:ASPxLabel ID="LDASPxLabel7" runat="server" Text="Organization"></dxe:ASPxLabel>
                                                </label>
                                                <div>
                                                    <asp:TextBox ID="LDtxtOrganization" runat="server" CssClass="form-control" MaxLength="20" Width="100%">
                                                    </asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>
                                                    <dxe:ASPxLabel ID="LDASPxLabel8" runat="server" Text="Job Responsibility" CssClass="pdl8"></dxe:ASPxLabel>
                                                </label>
                                                <div>

                                                    <asp:DropDownList ID="LDcmbJobResponsibility" runat="server" CssClass="form-control" Width="100%">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>
                                                    <dxe:ASPxLabel ID="LDASPxLabel9" runat="server" Text="Designation" CssClass="pdl8"></dxe:ASPxLabel>
                                                </label>
                                                <div>

                                                    <asp:DropDownList ID="LDcmbDesignation" runat="server" CssClass="form-control" Width="100%">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>
                                                    <dxe:ASPxLabel ID="LDASPxLabel10" runat="server" Text="Branch"></dxe:ASPxLabel>
                                                </label>
                                                <div>
                                                    <asp:DropDownList ID="LDcmbBranch" runat="server" CssClass="form-control" Width="100%">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>

                                            <div class="col-md-3">
                                                <label>
                                                    <dxe:ASPxLabel ID="LDASPxLabel11" runat="server" Text="Industry/Sector" CssClass="pdl8"></dxe:ASPxLabel>
                                                </label>
                                                <div>
                                                    <asp:DropDownList ID="LDcmbIndustry" runat="server" CssClass="form-control" Width="100%">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>






                                            <div class="col-md-3 lblmTop8">
                                                <label>
                                                    <dxe:ASPxLabel ID="LDASPxLabel13" runat="server" Text="Source"></dxe:ASPxLabel>
                                                </label>
                                                <div>

                                                    <asp:DropDownList ID="LDcmbSource" runat="server" CssClass="form-control" Width="100%">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="clear"></div>
                                            <div class="col-md-3">
                                                <label>
                                                    <dxe:ASPxLabel ID="LDASPxLabel17" runat="server" Text="Referred By" CssClass="pdl8"></dxe:ASPxLabel>
                                                </label>
                                                <div>
                                                    <%-- <asp:TextBox ID="LDtxtReferedBy" runat="server" TabIndex="14" CssClass="form-control" MaxLength="20" Width="100%"></asp:TextBox>
                                                    <asp:TextBox ID="LDtxtReferedBy_hidden" runat="server" TabIndex="14"></asp:TextBox>--%>
                                                    <asp:HiddenField ID="LDtxtReferedBy_hidden" runat="server"></asp:HiddenField>
                                                    <asp:ListBox ID="lstconverttounit" CssClass="chsn" runat="server" Width="100%" data-placeholder="Select..."></asp:ListBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" ValidationGroup="a" runat="server" ControlToValidate="lstconverttounit" Display="Dynamic"
                                                        CssClass="fa fa-exclamation-circle ctcclass " ToolTip="Mandatory."
                                                        ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>
                                                    <dxe:ASPxLabel ID="LDASPxLabel15" runat="server" Text="Rating" CssClass="pdl8"></dxe:ASPxLabel>
                                                </label>
                                                <div>

                                                    <asp:DropDownList ID="LDcmbRating" runat="server" CssClass="form-control" Width="100%">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>
                                                    <dxe:ASPxLabel ID="LDASPxLabel16" runat="server" Text="Marital Status"></dxe:ASPxLabel>
                                                </label>
                                                <div>
                                                    <asp:DropDownList ID="LDcmbMaritalStatus" runat="server" CssClass="form-control" Width="100%">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>
                                                    <dxe:ASPxLabel ID="LDASPxLabel14" runat="server" Text="Gender" CssClass="pdl8"></dxe:ASPxLabel>
                                                </label>
                                                <div>
                                                    <asp:DropDownList ID="LDcmbGender" runat="server" CssClass="form-control" Width="100%">
                                                        <asp:ListItem Value="1">Male</asp:ListItem>
                                                        <asp:ListItem Value="0">Female</asp:ListItem>
                                                        <asp:ListItem Value="2">Not Applicable</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>
                                                    <dxe:ASPxLabel ID="LDASPxLabel20" runat="server" Text="Legal Status" CssClass="pdl8"></dxe:ASPxLabel>
                                                </label>
                                                <div>

                                                    <asp:DropDownList ID="LDcmbLegalStatus" runat="server" CssClass="form-control" Width="100%">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>
                                                    <dxe:ASPxLabel ID="LDASPxLabel19" runat="server" Text="Contact Status"></dxe:ASPxLabel>
                                                </label>
                                                <div>

                                                    <asp:DropDownList ID="LDcmbContactStatus" runat="server" CssClass="form-control" Width="100%">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>
                                                    <dxe:ASPxLabel ID="LDASPxLabel18" runat="server" Text="D.O.B." CssClass="pdl8"></dxe:ASPxLabel>
                                                </label>
                                                <div>
                                                    <dxe:ASPxDateEdit ID="LDtxtDOB" runat="server" EditFormat="Custom" UseMaskBehavior="True" Width="100%">
                                                        <ButtonStyle Width="13px">
                                                        </ButtonStyle>
                                                    </dxe:ASPxDateEdit>
                                                </div>
                                            </div>

                                            <div class="col-md-3">
                                                <label>
                                                    <dxe:ASPxLabel ID="LDASPxLabel21" runat="server" Text="Anniversary Date" Width="111px" CssClass="pdl8"></dxe:ASPxLabel>
                                                </label>
                                                <div>
                                                    <dxe:ASPxDateEdit ID="LDtxtAnniversary" runat="server" EditFormat="Custom" UseMaskBehavior="True" Width="100%">
                                                        <ButtonStyle Width="13px">
                                                        </ButtonStyle>
                                                    </dxe:ASPxDateEdit>
                                                </div>
                                            </div>
                                            <div class="clear"></div>
                                            <div class="col-md-3">
                                                <label>
                                                    <dxe:ASPxLabel ID="LDASPxLabel22" runat="server" Text="Education">
                                                    </dxe:ASPxLabel>
                                                </label>
                                                <div>
                                                    <asp:DropDownList ID="LDcmbEducation" runat="server" CssClass="form-control" Width="100%">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>
                                                    <dxe:ASPxLabel ID="LDASPxLabel28" runat="server" Text="Blood Group" CssClass="pdl8"></dxe:ASPxLabel>
                                                </label>
                                                <div>
                                                    <asp:DropDownList ID="LDcmbBloodgroup" runat="server" CssClass="form-control" Width="100%">
                                                        <asp:ListItem Value="A+">A+</asp:ListItem>
                                                        <asp:ListItem Value="A-">A-</asp:ListItem>
                                                        <asp:ListItem Value="B+">B+</asp:ListItem>
                                                        <asp:ListItem Value="B-">B-</asp:ListItem>
                                                        <asp:ListItem Value="AB+">AB+</asp:ListItem>
                                                        <asp:ListItem Value="AB-">AB-</asp:ListItem>
                                                        <asp:ListItem Value="O+">O+</asp:ListItem>
                                                        <asp:ListItem Value="O-">O-</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>
                                                    <dxe:ASPxLabel ID="lblEnteredOn" runat="server" Text="Entered On" CssClass="pdl8"></dxe:ASPxLabel>
                                                </label>
                                                <div>
                                                    <dxe:ASPxDateEdit ID="dt_EnteredOn" runat="server" Date="" Width="100%" ClientInstanceName="cdt_EnteredOn">
                                                        <TimeSectionProperties>
                                                            <TimeEditProperties EditFormatString="hh:mm tt" />
                                                        </TimeSectionProperties>
                                                    </dxe:ASPxDateEdit>

                                                </div>
                                            </div>
                                            <div class="clear"></div>
                                            <div runat="server" id="divSendSMS" class="col-md-6" style="margin-top: 18px;">
                                                <div class="row">


                                                    <asp:HiddenField ID="hdnsendsms" runat="server" />
                                                    <div class="col-md-3" style="padding-left: 15px;">
                                                        <input type="checkbox" name="chksendSMS" id="chksendSMS" onclick="SendSMSChk()" />
                                                        &nbsp;Send SMS
                                                        
                                                    </div>
                                                    <div class="col-md-3 hidden" id="chksendUniqueDiv">
                                                        <input type="checkbox" name="chksendUnique" id="chksendUnique" onclick="SendUniqueChk()" />&nbsp;Same As Unique ID
                                                       
                                                    </div>
                                                    <div class="col-md-6">
                                                        <dxe:ASPxTextBox ID="txtSMSPhnNo" runat="server" ClientInstanceName="ctxtSMSPhnNo" Text=""
                                                            Width="100%" CssClass="inline hidden pdl8">
                                                            <MaskSettings Mask="&lt;0..9999999999&gt;" IncludeLiterals="All" />
                                                            <ValidationSettings Display="None"></ValidationSettings>
                                                        </dxe:ASPxTextBox>
                                                        <%--<asp:TextBox ID="txtSMSPhnNo" runat="server" CssClass="form-control hidden col-md-3" MaxLength="10" TabIndex="24" Width="100%">
                                                        </asp:TextBox>--%>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="clear"></div>

                                        </div>
                                        <div class="" style="padding-top: 15px;">
                                            <asp:HiddenField ID="hdReferenceBy" runat="server" />
                                            <dxe:ASPxButton ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" ClientInstanceName="cbtnSave" AutoPostBack="false" OnClick="btnSave_Click"
                                                ValidationGroup="contact">
                                                <ClientSideEvents Click="fn_btnValidate" />
                                            </dxe:ASPxButton>
                                            <dxe:ASPxButton ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-danger" OnClick="btnCancel_Click">
                                            </dxe:ASPxButton>

                                            <asp:Button ID="btnUdf" runat="server" Text="UDF" CssClass="btn dxbButton btn-cust" OnClientClick="if(OpenUdf()){ return false;}" />

                                            <GSTIN:gstinSettings runat="server" ID="GstinSettingsButton" />
                                            <%-- <table>
                                                    <tr>
                                                        <td style="padding-right: 20px;">
                                            
                                                        </td>
                                                        <td>
                                           
                                                        </td>
                                                    </tr>
                                                </table>--%>
                                        </div>
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Correspondence" Text="Correspondence">

                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="BankDetails" Text="Bank">

                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="DPDetails" Visible="false" Text="DP">

                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
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
                            <dxe:TabPage Name="Other" Visible="false" Text="Other">

                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="GroupMember" Text="Group">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Deposit" Visible="false" Text="Deposit">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Remarks" Text="UDF" Visible="false">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Education" Visible="false" Text="Education">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Trad. Prof." Visible="false" Text="Trad.Prof">
                                <%--<TabTemplate ><span style="font-size:x-small">Trad.Prof</span>&nbsp;<span style="color:Red;">*</span> </TabTemplate>--%>
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="FamilyMembers" Visible="false" Text="Family">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Subscription" Visible="false" Text="Subscription">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>

                            <dxe:TabPage Name="TDS" Visible="false" Text="TDS">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                             <dxe:TabPage Text="Contact Person" Name="ContactPreson">
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
	                                            var Tab9 = page.GetTab(9);
	                                            var Tab10 = page.GetTab(10);
	                                            var Tab11 = page.GetTab(11);
	                                            var Tab12 = page.GetTab(12);
	                                             var Tab13=page.GetTab(13);
	                                            var Tab14=page.GetTab(14);
                                                var Tab15=page.GetTab(15);
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
	                                            else if(activeTab == Tab9)
	                                            {
	                                                disp_prompt('tab9');
	                                            }
	                                            else if(activeTab == Tab10)
	                                            {
	                                                disp_prompt('tab10');
	                                            }
	                                            else if(activeTab == Tab11)
	                                            {
	                                                disp_prompt('tab11');
	                                            }
	                                            else if(activeTab == Tab12)
	                                            {
	                                                disp_prompt('tab12');
	                                            }
	                                            else if(activeTab == Tab13)
	                                            {
	                                               disp_prompt('tab13');
	                                            }

                                                else if(activeTab == Tab14)
	                                            {
	                                               disp_prompt('tab14');
	                                            }
	                                            else if(activeTab == Tab15)
	                                            {
	                                               disp_prompt('tab15');
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
                <td>
                    <table style="width: 100%;">
                        <tr>

                            <%--<td align="center" style="text-align: ">
                                    <asp:HiddenField ID="hdReferenceBy" runat="server" />
                                    <table >
                                        <tr>
                                        
                                            <td style="padding-right:20px;">
                                                <dxe:ASPxButton ID="btnSave" runat="server" Text="Save" width="80px"  OnClick="btnSave_Click" ValidationGroup="a"
                                                    TabIndex="30">
                                                </dxe:ASPxButton>
                                            </td>
                                            
                                            <td >
                                                <dxe:ASPxButton ID="btnCancel" runat="server" Text="Cancel" width="80px" OnClick="btnCancel_Click" 
                                                    TabIndex="24">
                                                </dxe:ASPxButton>
                                            </td>
                                    
                                        </tr>
                                    </table>
                                   
                                </td>--%>
                        </tr>
                        <tr>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    </div>
    <asp:HiddenField ID="hdnLoginUserSalesmanAgentsInternalId" runat="server" />
</asp:Content>
