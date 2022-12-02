<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/PopUp.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="Schemapopup.aspx.cs" Inherits="ERP.Schemapopup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%-- <script type="text/javascript" src="/OMS/modalfiles/modal.js"></script>

    <script type="text/javascript" src="/Scripts/jquery.min.js"></script>

    <script type="text/javascript" src="/Scripts/vendor/modernizr-2.8.3-respond-1.4.2.min.js"></script>--%>
    <script src="/assests/js/jquery.alerts.js"></script>
    <link href="/assests/css/custom/jquery.alerts.css" rel="stylesheet" />
    <script type="text/javascript">
        function datewiseCheck() {
            debugger;
            var _chkDatewise = document.getElementById("chkDateWise");
            var length = (document.getElementById("drplenght").value != '' && document.getElementById("drplenght").value != null) ? document.getElementById("drplenght").value : "0";
            if (_chkDatewise.checked == true) {

                document.getElementById("txtprefix").readOnly = true;
                $("#txtsuffix").val("/TCURDATE");
                document.getElementById("txtdigit").value = (parseInt(length) - 9).toString();
                document.getElementById("txtsuffix").readOnly = true;
            }
            else {
                document.getElementById("txtprefix").readOnly = false;
                $("#txtsuffix").val("");
                document.getElementById("txtdigit").value = length;
                document.getElementById("txtsuffix").readOnly = false;
            }

        }
        function ReturnToParentPage() { var parentWindow = window.parent; parentWindow.CloseUpdateGroupPopup(); }
        function changeredio() {
            var chkval = $('input[type="radio"]:checked').val();


            var SchemaId = cdrp_type.GetValue();
            if (SchemaId != null && SchemaId != "" && SchemaId != "0") {
                $.ajax({
                    type: "POST",
                    url: "Schemapopup.aspx/BindLength",
                    data: JSON.stringify({ SchemaId: SchemaId }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (msg) {

                        $("#drplenght").empty();
                        var grpdetl = msg.d;

                        debugger;

                        var opts = "";
                        if (grpdetl[0].SchemaId > 0) {

                            for (var i = 5; i <= grpdetl[0].SchemaId; i++) {

                                opts = opts + "<option value='" + i + "' selected>" + i + "</option>";

                            }
                        }
                        $("#drplenght").empty().append(opts);

                    },
                    error: function (data) {
                        debugger;
                        jAlert("Please try again later");
                    }
                });
            }


         

            if (chkval == '1') {

                $('#txtdigit').removeAttr('readonly');
                $('#txtstart').removeAttr('readonly');
                $("#txtstart").css("background-color", "White");
                $("#txtdigit").css("background-color", "White");

                $('#txtprefix').removeAttr('readOnly', 'readonly');
                $('#txtsuffix').removeAttr('readOnly', 'readonly');
                $("#txtprefix").css("background-color", "White");
                $("#txtsuffix").css("background-color", "White");
                $("#txtstart").val("0");
                $("#txtdigit").val("0");

                $("#chkDateWise").attr('disabled', false);

                $('#txtLenhth').removeClass('hide');
                $('#lbllength').removeClass('hide');
                $('#tdlblbrnch').removeClass('hide');
                $('#tdtxtbrnch').removeClass('hide');

                if ((cdrp_type.GetValue() == "126") || (cdrp_type.GetValue() == "127") || (cdrp_type.GetValue() == "128") || (cdrp_type.GetValue() == "129") || (cdrp_type.GetValue() == "130") || (cdrp_type.GetValue() == "131") || (cdrp_type.GetValue() == "132"))
                {
                    var list = document.getElementById("<%= drplenght.ClientID%>");
                    list.options.length = 0;

                    var SchemaId = cdrp_type.GetValue();
                    if (SchemaId != null && SchemaId != "" && SchemaId != "0") {
                        $.ajax({
                            type: "POST",
                            url: "Schemapopup.aspx/BindLength",
                            data: JSON.stringify({ SchemaId: SchemaId }),
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: false,
                            success: function (msg) {

                                $("#drplenght").empty();
                                var grpdetl = msg.d;

                                debugger;

                                var opts = "";
                                if (grpdetl[0].SchemaId > 0) {

                                    for (var i = 5; i <= grpdetl[0].SchemaId; i++) {

                                        opts = opts + "<option value='" + i + "' selected>" + i + "</option>";

                                    }
                                }
                                $("#drplenght").empty().append(opts);

                            },
                            error: function (data) {
                                debugger;
                                jAlert("Please try again later");
                            }
                        });
                    }

                    $('#txtLenhth').removeClass('hide');
                    $('#lbllength').removeClass('hide');
                    //$('#tdlblbrnch').removeClass('hide');
                    //$('#tdtxtbrnch').removeClass('hide');
                    $('#tdlblbrnch').addClass('hide');
                    $('#tdtxtbrnch').addClass('hide');

                }


            } else if (chkval == '0') {

                $('#txtdigit').attr('readOnly', 'readonly');
                $('#txtstart').attr('readOnly', 'readonly');
                $("#txtstart").css("background-color", "lightgray");
                $("#txtdigit").css("background-color", "lightgray");

                $('#txtprefix').attr('readOnly', 'readonly');
                $('#txtsuffix').attr('readOnly', 'readonly');
                $("#txtprefix").css("background-color", "lightgray");
                $("#txtsuffix").css("background-color", "lightgray");
                $("#txtstart").val("0");
                $("#txtdigit").val("0");

                $("#chkDateWise").attr('disabled', true);
                $("#txtsuffix").val("");
                $("#chkDateWise").prop("checked", false);

                $('#txtLenhth').removeClass('hide');
                $('#lbllength').removeClass('hide');
                $('#tdlblbrnch').removeClass('hide');
                $('#tdtxtbrnch').removeClass('hide');

                if ((cdrp_type.GetValue() == "126") || (cdrp_type.GetValue() == "127") || (cdrp_type.GetValue() == "128") || (cdrp_type.GetValue() == "129") || (cdrp_type.GetValue() == "130") || (cdrp_type.GetValue() == "131") || (cdrp_type.GetValue() == "132")) {
                    $('#txtLenhth').addClass('hide');
                    $('#lbllength').addClass('hide');
                    $('#tdlblbrnch').addClass('hide');
                    $('#tdtxtbrnch').addClass('hide');
                }



            } else if (chkval == '2') {

                $('#txtdigit').removeAttr('readonly');
                $('#txtstart').removeAttr('readonly');
                $("#txtstart").css("background-color", "White");
                $("#txtdigit").css("background-color", "White");

                $('#txtprefix').removeAttr('readOnly', 'readonly');
                $('#txtsuffix').removeAttr('readOnly', 'readonly');
                $("#txtprefix").css("background-color", "White");
                $("#txtsuffix").css("background-color", "White");

                $("#txtstart").val("1");
                $("#txtdigit").val("4");

            } else {

                $('#txtdigit').attr('readOnly', 'readonly');
                $('#txtstart').attr('readOnly', 'readonly');
                $("#txtstart").css("background-color", "lightgray");
                $("#txtdigit").css("background-color", "lightgray");

                $("#lbl_txtprefix").css("display", "none");
                $("#lbltxtsuffix_reqName").css("display", "none");
                $("#lbl_txtdigit").css("display", "none");
                $("#lbl_txtstart").css("display", "none");
                $("#txtstart").val("0");
                $("#txtdigit").val("0");

            }

        }
        function onlyNumbers(evt) {
            var e = event || evt; // for trans-browser compatibility
            var charCode = e.which || e.keyCode;

            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;

        }
        function CloseWindow1() {
            window.parent.CallParent();
            parent.editwin.close();
        }

        function btnCalcel_Click() {

            window.parent.SchemaGrid.PerformCallback();
            window.parent.cPopup_Schema.Hide();
            //end
        }
        $('#popup_ok').click(function () {

            //alert("closeclick");
            window.parent.SchemaGrid.PerformCallback();
            window.parent.cPopup_Schema.Hide();

        });
        function Confirmsave() {
            //jAlert('Saved Succesfully');
            LoadingPanel.Hide();
            window.parent.saveallsuccess();
            //window.parent.SchemaGrid.PerformCallback();
            window.parent.cPopup_Schema.Hide();

        };


        function Validatedata() {
            // debugger;

            var rb = document.getElementById("<%=rdpschematype.ClientID%>");
            var radio = rb.getElementsByTagName("input");
            var label = rb.getElementsByTagName("label");
            var RadioVal;
            for (var i = 0; i < radio.length; i++) {
                if (radio[i].checked) {
                    RadioVal = radio[i].value;
                    break;
                }
            }

            var Totallen = $("#drplenght :selected").val();
            var prefixlen = $('#txtprefix').val().length;
            var suffixlen = $('#txtsuffix').val().length;
            // alert(Number(Totallen) + '-' + Number(prefixlen) + '-' + Number(suffixlen));
            if (RadioVal == "2") {
                var digitlen = Number(Totallen) - (Number(prefixlen) + Number(suffixlen)) - 8
            }
            else {
                var digitlen = Number(Totallen) - (Number(prefixlen) + Number(suffixlen))
            }
            $('#txtdigit').val(digitlen);

            if ($('#txtschname').val() == "") {
                jAlert('Please enter unique Schema name.');
                return false;
            }

            if (cvalidfrom.GetValue() == null) {
                jAlert('Please select a valid from date to proceed.', 'Alert');
                digitlen = -1;
            }

            if (cvalidto.GetValue() == null) {
                jAlert('Please select a valid to date to proceed.', 'Alert');
                digitlen = -1;

            }


            if (digitlen < 1) {
                return false;
            }
            else {
                $("#hddnLengthValue").val($("#drplenght").val());
                LoadingPanel.Show();
                return true;
            }
        }

        $(document).ready(function () {
            //  debugger;
          
            if ($("#hdnPageStatus").val() == "Edit") {
                SchemaTypeInEditchange();
                LengthLoad();
                $("#drplenght").val($("#HdnedittLength").val());
            }



            //    debugger;
            $('#<%= txtsuffix.ClientID %>, #<%= txtprefix.ClientID %>').keypress(function (e) {
                //var regex = new RegExp("^[a-zA-Z0-9-/\\[\\](){}]+$");
                var regex = new RegExp("^[a-zA-Z0-9-/\\[\\](){}]+$");
                if (e.charCode == 0) {
                    return true;
                }
                var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);
                if (regex.test(str)) {
                    return true;
                }

                e.preventDefault();
                return false;
            });

            $("#drp_type").change(function () {

                var rb = document.getElementById("<%=rdpschematype.ClientID%>");
                var radio = rb.getElementsByTagName("input");
                var label = rb.getElementsByTagName("label");
                var RadioVal;
                for (var i = 0; i < radio.length; i++) {
                    if (radio[i].checked) {
                        RadioVal = radio[i].value;
                        break;
                    }
                }

                $("#DivChkDatewise").css('display', 'none');
                // alert($("#drp_type :selected").text());
                var data = $("#drp_type :selected").val();

                $("#tdvouchertype").css('display', 'none');
                if (data == "21" || data == "24") {

                    $("#tdvouchertype").css('display', 'table-row');
                }
                //if (data == "Employee Onroll" || data == "Employee Offroll") {
                if (data == "4" || data == "5") {
                    $("#rdpschematypetr").css('display', 'none');
                    $('#txtdigit').removeAttr('readonly');
                    $('#txtstart').removeAttr('readonly');

                    //$("#tdcompany").css('display', 'none');
                    //$("#tdfinyear").css('display', 'none');
                    //$("#tdbranch").css('display', 'none');

                    $("#txtstart").css("background-color", "White");
                    $("#txtdigit").css("background-color", "White");

                    var $radios = $('input:radio[name=ctl00$ContentPlaceHolder1$rdpschematype]');
                    if ($radios.is(':checked') === false) {
                        $radios.filter('[value=1]').prop('checked', true);
                    }
                }
                else {
                    $("#rdpschematypetr").css('display', 'table-row');

                    $("#tdcompany").css('display', 'table-row');
                    $("#tdfinyear").css('display', 'table-row');
                    $("#tdbranch").css('display', 'table-row');


                    var len = $("#drplenght :selected").val();
                    if (RadioVal == "2") {
                        $('#txtdigit').val(len - 8);
                        //  alert(len - 8);
                    }
                    else {
                        $('#txtdigit').val(len);
                        //  alert(len - 8);
                    }
                    //$('#txtstart').attr('readOnly', 'readonly');
                }
                //----- Journal/CashBank Voucher Checking ---------
                if (data == "1" || data == "2" || data == "14") {
                    $("#DivChkDatewise").css('display', 'table-row');
                }

            });

            $('#txtschname').change(function () {
                
                //var data = $("#drp_type :selected").val();
                var data = cdrp_type.GetValue();
                var procode = 0;
                //var ProductName = ctxtPro_Name.GetText();
                var clientName = $('#txtschname').val();
                $.ajax({
                    type: "POST",
                    url: "Schemapopup.aspx/CheckUniqueName",
                    //data: "{'ProductName':'" + ProductName + "'}",
                    data: JSON.stringify({ clientName: clientName, procode: procode, typevalue: data }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var data = msg.d;

                        if (data == true) {
                            jAlert('Please use unique Schema name.')

                            //ctxtPro_Code.SetText("");
                            //document.getElementById("Popup_Empcitys_ctxtPro_Code_I").focus();
                            document.getElementById("txtschname").focus();

                            return false;
                        }
                    }

                });

                $('#txtstart').val('1');
            });

            $("#drplenght").change(function () {

                //if (data == "4" || data == "5") {

                //} else {
                var rb = document.getElementById("<%=rdpschematype.ClientID%>");
                var radio = rb.getElementsByTagName("input");
                var label = rb.getElementsByTagName("label");
                var RadioVal;
                for (var i = 0; i < radio.length; i++) {
                    if (radio[i].checked) {
                        RadioVal = radio[i].value;
                        break;
                    }
                }

                if (RadioVal == "2") {
                    var Totallen = $("#drplenght :selected").val();
                    var prefixlen = $('#txtprefix').val().length;
                    var suffixlen = $('#txtsuffix').val().length;

                    var digitlen = Number(Totallen) - (Number(prefixlen) + Number(suffixlen)) - 8
                    $('#txtdigit').val(digitlen);
                    // alert(digitlen);
                }
                else {
                    var Totallen = $("#drplenght :selected").val();
                    var prefixlen = $('#txtprefix').val().length;
                    var suffixlen = $('#txtsuffix').val().length;
                    // alert(Number(Totallen) + '-' + Number(prefixlen) + '-' + Number(suffixlen));
                    var digitlen = Number(Totallen) - (Number(prefixlen) + Number(suffixlen))
                    $('#txtdigit').val(digitlen);
                    // alert(RadioVal);
                }


                // }

            });

            window.onload = function () {

                var data = $("#drp_type :selected").val();

                var chkval = $('input[type="radio"]:checked').val();

                if (chkval == '1') {

                    $('#txtdigit').removeAttr('readonly');
                    $('#txtstart').removeAttr('readonly');
                    $("#txtstart").css("background-color", "White");
                    $("#txtdigit").css("background-color", "White");

                    $('#txtprefix').removeAttr('readOnly', 'readonly');
                    $('#txtsuffix').removeAttr('readOnly', 'readonly');
                    $("#txtprefix").css("background-color", "White");
                    $("#txtsuffix").css("background-color", "White");

                } else if (chkval == '0') {

                    $('#txtdigit').attr('readOnly', 'readonly');
                    $('#txtstart').attr('readOnly', 'readonly');
                    $("#txtstart").css("background-color", "lightgray");
                    $("#txtdigit").css("background-color", "lightgray");

                    $('#txtprefix').attr('readOnly', 'readonly');
                    $('#txtsuffix').attr('readOnly', 'readonly');
                    $("#txtprefix").css("background-color", "lightgray");
                    $("#txtsuffix").css("background-color", "lightgray");

                } else if (chkval == '2') {

                    $('#txtdigit').removeAttr('readonly');
                    $('#txtstart').removeAttr('readonly');
                    $("#txtstart").css("background-color", "White");
                    $("#txtdigit").css("background-color", "White");

                    $('#txtprefix').removeAttr('readOnly', 'readonly');
                    $('#txtsuffix').removeAttr('readOnly', 'readonly');
                    $("#txtprefix").css("background-color", "White");
                    $("#txtsuffix").css("background-color", "White");


                }
                else {

                    //$('#txtdigit').attr('readOnly', 'readonly');
                    //$('#txtstart').attr('readOnly', 'readonly');
                    //$("#txtstart").css("background-color", "lightgray");
                    //$("#txtdigit").css("background-color", "lightgray");

                    $("#txtstart").css("background-color", "White");
                    $("#txtdigit").css("background-color", "White");

                    //$("#lbl_txtprefix").css("display", "none");
                    //$("#lbltxtsuffix_reqName").css("display", "none");
                    //$("#lbl_txtdigit").css("display", "none");
                    //$("#lbl_txtstart").css("display", "none");


                }
                if (data == "21" || data == "24") {
                    $("#tdvouchertype").css('display', 'table-row');
                }
                if (data == "4" || data == "5") {
                    $("#rdpschematypetr").css('display', 'none');
                    $('#txtdigit').removeAttr('readonly');
                    $('#txtstart').removeAttr('readonly');

                    //$("#tdcompany").css('display', 'none');
                    //$("#tdfinyear").css('display', 'none');
                    //$("#tdbranch").css('display', 'none');

                    $("#tdcompany").css('display', 'table-row');
                    $("#tdfinyear").css('display', 'table-row');
                    $("#tdbranch").css('display', 'table-row');

                    //var $radios = $('input:radio[ID=ctl00$ContentPlaceHolder1$rdpschematype]');
                    var $radios = $('input:radio[ID=rdpschematype]');
                    if ($radios.is(':checked') === false) {
                        $radios.filter('[value=1]').prop('checked', true);
                    }
                }
                else {

                    $("#rdpschematypetr").css('display', 'table-row');

                    $("#tdcompany").css('display', 'table-row');
                    $("#tdfinyear").css('display', 'table-row');
                    $("#tdbranch").css('display', 'table-row');

                    //var $radios = $('input:radio[ID=ctl00$ContentPlaceHolder1$rdpschematype]');
                    var $radios = $('input:radio[ID=rdpschematype]');
                    if ($radios.is(':checked') === false) {
                        $radios.filter('[value=1]').prop('checked', true);
                    }
                }

            }


            //------------------Count No of digit by jQuery-----------------------------------------------

            $('#<%=txtprefix.ClientID %>').blur(function () {
                // implementation here

                var rb = document.getElementById("<%=rdpschematype.ClientID%>");
                var radio = rb.getElementsByTagName("input");
                var label = rb.getElementsByTagName("label");
                var RadioVal;
                for (var i = 0; i < radio.length; i++) {
                    if (radio[i].checked) {
                        RadioVal = radio[i].value;
                        break;
                    }
                }

                if (RadioVal == "2") {
                    var data = $("#drplenght :selected").val();
                    var txtprefix = $('#txtprefix').val();
                    if (txtprefix != "") {
                        var cal = Number(data) - Number(txtprefix.length) - 8;
                        $("#txtdigit").val(cal);
                        $('#txtdigit').attr('readOnly', 'readonly');
                    } else {
                        $('#txtdigit').removeAttr('readOnly', 'readonly');
                    }
                }
                else {
                    var data = $("#drplenght :selected").val();
                    var txtprefix = $('#txtprefix').val();
                    if (txtprefix != "") {
                        var cal = Number(data) - Number(txtprefix.length);
                        $("#txtdigit").val(cal);
                        $('#txtdigit').attr('readOnly', 'readonly');
                    } else {
                        $('#txtdigit').removeAttr('readOnly', 'readonly');
                    }
                }

            });

            $('#<%=txtsuffix.ClientID %>').blur(function () {
                // implementation here

                var rb = document.getElementById("<%=rdpschematype.ClientID%>");
                var radio = rb.getElementsByTagName("input");
                var label = rb.getElementsByTagName("label");
                var RadioVal;
                for (var i = 0; i < radio.length; i++) {
                    if (radio[i].checked) {
                        RadioVal = radio[i].value;
                        break;
                    }
                }

                if (RadioVal == "2") {
                    var data = $("#drplenght :selected").val();
                    var txtprefix = $('#txtprefix').val();
                    var txtsuffix = $('#txtsuffix').val();
                    if (txtprefix != "" && txtsuffix != "") {

                        var cal = (Number(data) - (Number(txtprefix.length) + Number(txtsuffix.length)) - 8);
                        $("#txtdigit").val(cal);

                        $('#txtdigit').attr('readOnly', 'readonly');
                    } else {
                        $('#txtdigit').removeAttr('readOnly', 'readonly');
                    }
                }
                else {
                    var data = $("#drplenght :selected").val();
                    var txtprefix = $('#txtprefix').val();
                    var txtsuffix = $('#txtsuffix').val();
                    if (txtprefix != "" && txtsuffix != "") {

                        var cal = (Number(data) - (Number(txtprefix.length) + Number(txtsuffix.length)));
                        $("#txtdigit").val(cal);

                        $('#txtdigit').attr('readOnly', 'readonly');
                    } else {
                        $('#txtdigit').removeAttr('readOnly', 'readonly');
                    }
                }
            });
            //End -----------------Count No of digit by jQuery-----------------------------------------------


            if ($("#hdnTypeID").val() == "134") {
                $("#trRcptChlnTyp").removeClass("hide");
                $("#rdpschematype_2text").removeClass("hide");
                cchkSupressZero.SetEnabled(false);
                $("#rdpschematype_2").removeClass('hide');
            }
            else if ($("#hdnTypeID").val() == "147" || $("#hdnTypeID").val() == "148") {
                $("#rdpschematype_2text").removeClass("hide");
                cchkSupressZero.SetEnabled(false);
                $("#rdpschematype_2").removeClass('hide');
            }
            else {
                $("#rdpschematype_2text").addClass("hide");
                $("#rdpschematype_2").addClass('hide');
                cchkSupressZero.SetEnabled(true);
                $("#trRcptChlnTyp").addClass("hide");
            }


        });


        function validtochange(s, e) {
            if (s.GetValue() == null) {
                s.SetDate(s.GetMaxDate());
            }
        }

        function validfromchange(s, e) {
            if (s.GetValue() == null) {
                s.SetDate(s.GetMinDate());
            }
        }


        function LengthLoad() {

            var SchemaId = $("#hdndrtType").val();//cdrp_type.GetValue();
            if (SchemaId != null && SchemaId != "" && SchemaId != "0") {
                $.ajax({
                    type: "POST",
                    url: "Schemapopup.aspx/BindLength",
                    data: JSON.stringify({ SchemaId: SchemaId }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (msg) {

                        $("#drplenght").empty();
                        var grpdetl = msg.d;

                        debugger;

                        var opts = "";
                        if (grpdetl[0].SchemaId > 0) {

                            for (var i = 5; i <= grpdetl[0].SchemaId; i++) {

                                opts = opts + "<option value='" + i + "' selected>" + i + "</option>";

                            }
                        }
                        $("#drplenght").empty().append(opts);

                    },
                    error: function (data) {
                        debugger;
                        jAlert("Please try again later");
                    }
                });
            }


        }




        function SchemaTypechange() {
            debugger;


            if ((cdrp_type.GetValue() == "126") || (cdrp_type.GetValue() == "127") || (cdrp_type.GetValue() == "128") || (cdrp_type.GetValue() == "129") || (cdrp_type.GetValue() == "130") || (cdrp_type.GetValue() == "131") || (cdrp_type.GetValue() == "132")) {
                cvalidfrom.SetEnabled(false);
                cvalidto.SetEnabled(false);
                //$("#ddl_branch").prop('disabled', true);
                $("#ddl_company").prop('disabled', true);

                var rb = document.getElementById("<%=rdpschematype.ClientID%>");
                var radio = rb.getElementsByTagName("input");
                var label = rb.getElementsByTagName("label");
                var RadioVal;
                for (var i = 0; i < radio.length; i++) {
                    if (radio[i].checked) {
                        RadioVal = radio[i].value;
                        break;
                    }
                }

                if (RadioVal == '1') {
                  var list = document.getElementById("<%= drplenght.ClientID%>");
                    list.options.length = 0;

                    var SchemaId = cdrp_type.GetValue();
                    if (SchemaId != null && SchemaId != "" && SchemaId != "0") {
                        $.ajax({
                            type: "POST",
                            url: "Schemapopup.aspx/BindLength",
                            data: JSON.stringify({ SchemaId: SchemaId }),
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: false,
                            success: function (msg) {

                                $("#drplenght").empty();
                                var grpdetl = msg.d;

                                debugger;

                                var opts = "";
                                if (grpdetl[0].SchemaId > 0) {

                                    for (var i = 5; i <= grpdetl[0].SchemaId; i++) {

                                        opts = opts + "<option value='" + i + "' selected>" + i + "</option>";

                                    }
                                }
                                $("#drplenght").empty().append(opts);

                            },
                            error: function (data) {
                                debugger;
                                jAlert("Please try again later");
                            }
                        });
                    }
            

                    $('#txtLenhth').removeClass('hide');
                    $('#lbllength').removeClass('hide');
                    //$('#tdlblbrnch').removeClass('hide');
                    //$('#tdtxtbrnch').removeClass('hide');
                    $('#tdlblbrnch').addClass('hide');
                    $('#tdtxtbrnch').addClass('hide');

                }
                if (RadioVal == '0') {

                    $('#txtLenhth').addClass('hide');
                    $('#lbllength').addClass('hide');
                    $('#tdlblbrnch').addClass('hide');
                    $('#tdtxtbrnch').addClass('hide');
                }
                cchkSupressZero.SetEnabled(true);

                $("#trRcptChlnTyp").addClass("hide");
                $("#rdpschematype_2text").addClass("hide");
                $("#rdpschematype_2").addClass('hide');
            }
            else if (cdrp_type.GetValue() == "134") {
                $("#trRcptChlnTyp").removeClass("hide");
                $("#rdpschematype_2text").removeClass("hide");
                cchkSupressZero.SetEnabled(false);
                $("#rdpschematype_2").removeClass('hide');
            }
            else if (cdrp_type.GetValue() == "147" || cdrp_type.GetValue() == "148") {
                $("#rdpschematype_2text").removeClass("hide");
                cchkSupressZero.SetEnabled(false);
                $("#rdpschematype_2").removeClass('hide');
            }
            else {

                $("#trRcptChlnTyp").addClass("hide");
                $("#rdpschematype_2text").addClass("hide");
                cchkSupressZero.SetEnabled(true);
                $("#rdpschematype_2").addClass('hide');

                $('#txtLenhth').removeClass('hide');
                $('#lbllength').removeClass('hide');
                $('#tdlblbrnch').removeClass('hide');
                $('#tdtxtbrnch').removeClass('hide');
                cvalidfrom.SetEnabled(true);
                cvalidto.SetEnabled(true);
                //$("#ddl_branch").prop('disabled', false);
                $("#ddl_company").prop('disabled', false);

                var rb = document.getElementById("<%=rdpschematype.ClientID%>");
                var radio = rb.getElementsByTagName("input");
                var label = rb.getElementsByTagName("label");
                var RadioVal;
                for (var i = 0; i < radio.length; i++) {
                    if (radio[i].checked) {
                        RadioVal = radio[i].value;
                        break;
                    }
                }

                if (RadioVal == '0' || RadioVal == '1') {
               var list = document.getElementById("<%= drplenght.ClientID%>");
                     list.options.length = 0;

                     var SchemaId = cdrp_type.GetValue();
                     if (SchemaId != null && SchemaId != "" && SchemaId != "0") {
                         $.ajax({
                             type: "POST",
                             url: "Schemapopup.aspx/BindLength",
                             data: JSON.stringify({ SchemaId: SchemaId }),
                             contentType: "application/json; charset=utf-8",
                             dataType: "json",
                             async: false,
                             success: function (msg) {

                                 $("#drplenght").empty();
                                 var grpdetl = msg.d;

                                 debugger;

                                 var opts = "";
                                 if (grpdetl[0].SchemaId > 0) {

                                     for (var i = 5; i <= grpdetl[0].SchemaId; i++) {

                                         opts = opts + "<option value='" + i + "' selected>" + i + "</option>";

                                     }
                                 }
                                 $("#drplenght").empty().append(opts);

                             },
                             error: function (data) {
                                 debugger;
                                 jAlert("Please try again later");
                             }
                         });
                     }

                 }

             }
     }



     function SchemaTypeInEditchange() {
         if (($("#hdndrtType").val() == "126") || ($("#hdndrtType").val() == "127") || ($("#hdndrtType").val() == "128") || ($("#hdndrtType").val() == "129") || ($("#hdndrtType").val() == "130") || ($("#hdndrtType").val() == "131") || ($("#hdndrtType").val() == "132")) {
             cvalidfrom.SetEnabled(false);
             cvalidto.SetEnabled(false);
             //$("#ddl_branch").prop('disabled', true);
             $("#ddl_company").prop('disabled', true);

             var rb = document.getElementById("<%=rdpschematype.ClientID%>");
            var radio = rb.getElementsByTagName("input");
            var label = rb.getElementsByTagName("label");
            var RadioVal;
            for (var i = 0; i < radio.length; i++) {
                if (radio[i].checked) {
                    RadioVal = radio[i].value;
                    break;
                }
            }

            if (RadioVal == '1') {
                var list = document.getElementById("<%= drplenght.ClientID%>");
                         list.options.length = 0;

                         var SchemaId = $("#hdndrtType").val();//cdrp_type.GetValue();
                         if (SchemaId != null && SchemaId != "" && SchemaId != "0") {
                             $.ajax({
                                 type: "POST",
                                 url: "Schemapopup.aspx/BindLength",
                                 data: JSON.stringify({ SchemaId: SchemaId }),
                                 contentType: "application/json; charset=utf-8",
                                 dataType: "json",
                                 async: false,
                                 success: function (msg) {

                                     $("#drplenght").empty();
                                     var grpdetl = msg.d;

                                     debugger;

                                     var opts = "";
                                     if (grpdetl[0].SchemaId > 0) {

                                         for (var i = 5; i <= grpdetl[0].SchemaId; i++) {

                                             opts = opts + "<option value='" + i + "' selected>" + i + "</option>";

                                         }
                                     }
                                     $("#drplenght").empty().append(opts);

                                 },
                                 error: function (data) {
                                     debugger;
                                     jAlert("Please try again later");
                                 }
                             });
                         }



                         $('#txtLenhth').removeClass('hide');
                         $('#lbllength').removeClass('hide');
                         //$('#tdlblbrnch').removeClass('hide');
                         //$('#tdtxtbrnch').removeClass('hide');
                         $('#tdlblbrnch').addClass('hide');
                         $('#tdtxtbrnch').addClass('hide');

                     }
                     if (RadioVal == '0') {

                         $('#txtLenhth').addClass('hide');
                         $('#lbllength').addClass('hide');
                         $('#tdlblbrnch').addClass('hide');
                         $('#tdtxtbrnch').addClass('hide');
                     }

                 }
                 else {
                     cvalidfrom.SetEnabled(true);
                     cvalidto.SetEnabled(true);

                     $('#txtLenhth').removeClass('hide');
                     $('#lbllength').removeClass('hide');
                     $('#tdlblbrnch').removeClass('hide');
                     $('#tdtxtbrnch').removeClass('hide');


            // $("#ddl_branch").prop('disabled', false);
                     $("#ddl_company").prop('disabled', false);

                     var rb = document.getElementById("<%=rdpschematype.ClientID%>");
            var radio = rb.getElementsByTagName("input");
            var label = rb.getElementsByTagName("label");
            var RadioVal;
            for (var i = 0; i < radio.length; i++) {
                if (radio[i].checked) {
                    RadioVal = radio[i].value;
                    break;
                }
            }

            if (RadioVal == '0' || RadioVal == '1') {
                var list = document.getElementById("<%= drplenght.ClientID%>");
                list.options.length = 0;


                var SchemaId = $("#hdndrtType").val();//cdrp_type.GetValue();
                if (SchemaId != null && SchemaId != "" && SchemaId != "0") {
                    $.ajax({
                        type: "POST",
                        url: "Schemapopup.aspx/BindLength",
                        data: JSON.stringify({ SchemaId: SchemaId }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,
                        success: function (msg) {

                            $("#drplenght").empty();
                            var grpdetl = msg.d;

                            debugger;

                            var opts = "";
                            if (grpdetl[0].SchemaId > 0) {

                                for (var i = 5; i <= grpdetl[0].SchemaId; i++) {

                                    opts = opts + "<option value='" + i + "' selected>" + i + "</option>";

                                }
                            }
                            $("#drplenght").empty().append(opts);

                        },
                        error: function (data) {
                            debugger;
                            jAlert("Please try again later");
                        }
                    });
                }

                         }

                     }
                 }


    </script>
    <style>
        #reqName {
            position: absolute;
            right: 0;
            top: 46px;
        }

        #lbl_txtprefix {
            position: absolute;
            right: 0;
            top: 167px;
        }

        #lbltxtsuffix_reqName {
            position: absolute;
            right: 0;
            top: 205px;
        }

        #lbl_txtdigit {
            position: absolute;
            right: -15px;
            top: 8px;
        }

        #lbl_txtstart {
            position: absolute;
            right: -15px;
            top: 8px;
        }

        #rdpschematype tr {
            float: left;
            margin-right: 15px;
        }

        .schemaTable > tbody > tr > td {
            padding: 3px 0;
        }

        .mutedStyle {
            font-size: 11px;
            color: #f54c4c;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="PopUpArea">
        <table style="width: 96%;" class="schemaTable">
            <tr>
                <td>Type: </td>
                <td>
                    <dxe:ASPxComboBox ID="drp_type" ValueType="System.String" runat="server" ClientInstanceName="cdrp_type" DataSourceID="SqlSchematype" TextField="type_name"
                        ValueField="type_Id" AppendDataBoundItems="true" Width="100%">
                        <ClientSideEvents SelectedIndexChanged="SchemaTypechange" />
                    </dxe:ASPxComboBox>
                </td>

            </tr>

            <tr id="trRcptChlnTyp" class="hide" runat="server">
                <td>Receipt Challan Type: </td>
                <td>
                    <dxe:ASPxComboBox ID="drp_RcptChlnTyp" ValueType="System.String" runat="server" ClientInstanceName="cdrp_RcptChlnTyp" DataSourceID="SqlRcptChlnType" TextField="TYPE"
                        ValueField="ID" AppendDataBoundItems="true" Width="100%">
                        <%--<ClientSideEvents SelectedIndexChanged="SchemaTypechange" />--%>
                    </dxe:ASPxComboBox>
                </td>

            </tr>

            <tr>
                <td>Scheme Name: </td>
                <td>
                    <asp:TextBox ID="txtschname" runat="server" MaxLength="80">
                        
                    </asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ID="reqName" CssClass="pullrightClass fa fa-exclamation-circle abs iconRed" ControlToValidate="txtschname" ToolTip="Mandatory" ErrorMessage="" />
                </td>
            </tr>
            <tr id="rdpschematypetr" style="display: none;">
                <td>Scheme Type: </td>
                <td>
                    <asp:RadioButtonList ID="rdpschematype" runat="server" onclick="changeredio();" TextAlign="Right">
                        <asp:ListItem Text="Manual" Value="0"></asp:ListItem>
                        <asp:ListItem Text="Auto" Value="1" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="" Value="2"><span id="rdpschematype_2text">DateWise</span></asp:ListItem>
                    </asp:RadioButtonList></td>
            </tr>
            <tr id="DivChkDatewise" style="display: none;">
                <td>Date Wise: </td>
                <td>
                    <asp:CheckBox ID="chkDateWise" runat="server" value="0" onchange="datewiseCheck();" />
                </td>
            </tr>
            <tr>
                <td class="clslblLength" id="lbllength">Length: </td>
                <td class="clstxtLength" id="txtLenhth">
                    <asp:DropDownList ID="drplenght" runat="server" Width="100%">
                        <Items>
                            <%--  <asp:ListItem Text="5" Value="5"></asp:ListItem>
                            <asp:ListItem Text="6" Value="6"></asp:ListItem>
                            <asp:ListItem Text="7" Value="7"></asp:ListItem>
                            <asp:ListItem Text="8" Value="8"></asp:ListItem>
                            <asp:ListItem Text="9" Value="9"></asp:ListItem>
                            <asp:ListItem Text="10" Value="10"></asp:ListItem>
                            <asp:ListItem Text="11" Value="11"></asp:ListItem>
                            <asp:ListItem Text="12" Value="12"></asp:ListItem>
                            <asp:ListItem Text="13" Value="13"></asp:ListItem>
                            <asp:ListItem Text="14" Value="14"></asp:ListItem>
                            <asp:ListItem Text="15" Value="15"></asp:ListItem>
                            <asp:ListItem Text="16" Value="16"></asp:ListItem>
                            <asp:ListItem Text="17" Value="17"></asp:ListItem>
                            <asp:ListItem Text="18" Value="18"></asp:ListItem>
                            <asp:ListItem Text="19" Value="19"></asp:ListItem>
                            <asp:ListItem Text="20" Value="20"></asp:ListItem>
                            <asp:ListItem Text="21" Value="21"></asp:ListItem>
                            <asp:ListItem Text="22" Value="22"></asp:ListItem>
                            <asp:ListItem Text="23" Value="23"></asp:ListItem>
                            <asp:ListItem Text="24" Value="24"></asp:ListItem>
                            <asp:ListItem Text="25" Value="25"></asp:ListItem>
                            <asp:ListItem Text="26" Value="26"></asp:ListItem>
                            <asp:ListItem Text="27" Value="27"></asp:ListItem>
                            <asp:ListItem Text="28" Value="28"></asp:ListItem>
                            <asp:ListItem Text="29" Value="29"></asp:ListItem>
                            <asp:ListItem Text="30" Value="30"></asp:ListItem>--%>
                        </Items>
                    </asp:DropDownList></td>

            </tr>
            <tr>
                <td>Prefix: </td>
                <td>
                    <asp:TextBox ID="txtprefix" runat="server" MaxLength="20"></asp:TextBox>
                    <asp:Label runat="server" Text="Prefix allowed characters / [] () {} a-z A-Z 0-9" CssClass="mutedStyle"></asp:Label>
                    <asp:Label ID="lbl_txtprefix" runat="server" Text="" class="pullrightClass fa fa-exclamation-circle abs iconRed" Visible="false" ToolTip="Mandatory"></asp:Label></td>

            </tr>
            <tr>
                <td>Suffix: </td>
                <td>
                    <asp:TextBox ID="txtsuffix" runat="server" MaxLength="20"></asp:TextBox>
                    <asp:Label runat="server" Text="Suffix allowed characters / [] () {} a-z A-Z 0-9" CssClass="mutedStyle"></asp:Label>
                    <%-- <asp:Label ID="lbltxtsuffix_reqName" runat="server" Text="" class="pullrightClass fa fa-exclamation-circle abs iconRed" Visible="false" ToolTip="Mandatory"></asp:Label> --%>
                    
                </td>

            </tr>
            <%-- <tr style="display: none;">--%>

            <tr>
                <td><span class="spanaduto">No. of Digit(s):</span> </td>
                <td class="relative">

                    <asp:TextBox ID="txtdigit" runat="server" onkeypress="return onlyNumbers();" MaxLength="5" BackColor="White"></asp:TextBox>
                    <asp:Label ID="lbl_txtdigit" runat="server" Text="" class="pullrightClass fa fa-exclamation-circle abs iconRed" Visible="false" ToolTip="Mandatory"></asp:Label>
                </td>
            </tr>
            <tr>
                <td><span class="spanaduto">Start No:</span></td>
                <td class="relative">

                    <asp:TextBox ID="txtstart" runat="server" onkeypress="return onlyNumbers();" MaxLength="5" BackColor="White" Text="1"></asp:TextBox>
                    <asp:Label ID="lbl_txtstart" runat="server" Text="" class="pullrightClass fa fa-exclamation-circle abs iconRed" Visible="false" ToolTip="Mandatory"></asp:Label>
                </td>

            </tr>
            <tr style="display: none">
                <td><span class="spanaduto">Prefill With: </span></td>
                <td>

                    <asp:TextBox ID="txtprefixwith" runat="server" MaxLength="5" Text="0"></asp:TextBox>

                </td>

            </tr>
            <tr id="tdcompany" style="display: none;">
                <td><span class="spanaduto">Select Company : </span></td>
                <td>

                    <asp:DropDownList ID="ddl_company" runat="server" DataSourceID="sqlcomp" DataTextField="cmp_Name"
                        DataValueField="cmp_internalid" AppendDataBoundItems="true" Width="100%">
                    </asp:DropDownList>

                </td>

            </tr>
            <tr id="tdfinyear" style="display: none;" class="hide">
                <td>Financial Year: </td>
                <td>
                    <asp:DropDownList ID="drpfinyear" runat="server" DataSourceID="Sqlfinyear" DataTextField="FinYear_Code"
                        DataValueField="FinYear_ID" AppendDataBoundItems="true" Width="100%">
                    </asp:DropDownList></td>

            </tr>
            <tr>
                <td>Valid From: </td>
                <td>
                    <dxe:ASPxDateEdit ID="validfrom" ClientInstanceName="cvalidfrom" UseMaskBehavior="True" EditFormat="Custom" Width="100%" runat="server" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy">
                        <ClientSideEvents DateChanged="validfromchange" />
                    </dxe:ASPxDateEdit>
            </tr>
            <tr>
                <td>Valid Upto: </td>
                <td>
                    <dxe:ASPxDateEdit ID="validto" ClientInstanceName="cvalidto" UseMaskBehavior="True" Width="100%" runat="server" DisplayFormatString="dd-MM-yyyy" EditFormat="Custom" EditFormatString="dd-MM-yyyy">
                        <DateRangeSettings StartDateEditID="validfrom"></DateRangeSettings>
                        <ClientSideEvents DateChanged="validtochange" />
                    </dxe:ASPxDateEdit>
            </tr>
            <tr>
                <td>Suppress Zero: </td>
                <td>
                    <dxe:ASPxCheckBox ID="chkSupressZero" ClientInstanceName="cchkSupressZero" runat="server">
                    </dxe:ASPxCheckBox>
                </td>
            </tr>
            <tr id="tdbranch" >
                <td id="tdlblbrnch" class="clasbr5anch"><span class="spanaduto">Select Branch: </span></td>
                <td id="tdtxtbrnch" class="txtbranch">

                    <asp:DropDownList ID="ddl_branch" runat="server" DataSourceID="sqlbranch" DataTextField="branch_description"
                        DataValueField="branch_id" AppendDataBoundItems="true" Width="100%">
                    </asp:DropDownList>

                </td>

            </tr>
            <tr>
                <td><span class="spanaduto">Is Active: </span></td>
                <td>

                    <asp:CheckBox ID="ChkActive" runat="server" Checked="true" />

                </td>

            </tr>
            <tr id="tdvouchertype" style="display: none;">
                <td><span class="spanaduto">Voucher Type: </span></td>
                <td>
                    <asp:DropDownList ID="ddlVoucherType" runat="server" Width="100%">
                        <Items>
                            <asp:ListItem Text="None" Value="None"></asp:ListItem>
                            <asp:ListItem Text="Advance" Value="Advance"></asp:ListItem>
                            <asp:ListItem Text="Non Advance" Value="NonAdvance"></asp:ListItem>
                            <asp:ListItem Text="On Account" Value="OnAccount"></asp:ListItem>
                        </Items>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label ID="lblmessage" runat="server" Visible="False" Font-Bold="True" ForeColor="Red"></asp:Label>
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td colspan="2" style="padding-left: 99px">
                    <asp:Button ID="btn_save" runat="server" Text="Save" class="btn btn-primary" OnClick="btn_save_Click" OnClientClick="return Validatedata();" />

                    <input id="btnCalcel" type="button" class="btn btn-danger" value="Cancel" onclick="btnCalcel_Click()" tabindex="12" style="display: none;" />

                </td>


            </tr>
        </table>
        <%--<asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator1" ToolTip="only number!" ControlToValidate="txtdigit" ValidationExpression="^[0-9]$" ErrorMessage="only number!" />
        <asp:RegularExpressionValidator runat="server" ID="rexNumber" ToolTip="only number!" ControlToValidate="txtstart" ValidationExpression="^[0-9]$" ErrorMessage="only number!" />--%>
        <asp:SqlDataSource ID="SqlSchematype" runat="server">
            <%--SelectCommand="select type_Id,type_name from tbl_master_idschematype where type_Isactive=1 order by ordernumber"--%>
        </asp:SqlDataSource>

        <asp:SqlDataSource ID="SqlRcptChlnType" runat="server"
            SelectCommand="select ID,TYPE from SRV_ReceiptChallanType where ISACTIVE=1 order by ID"></asp:SqlDataSource>

        <asp:SqlDataSource ID="Sqlfinyear" runat="server"
            SelectCommand="select FinYear_ID,FinYear_Code from Master_FinYear"></asp:SqlDataSource>

        <asp:SqlDataSource ID="sqlcomp" runat="server"
            SelectCommand="select cmp_internalid,cmp_Name from tbl_master_company"></asp:SqlDataSource>

        <asp:SqlDataSource ID="sqlbranch" runat="server"
            SelectCommand="select 0 as branch_id,'Select' as branch_description union all select branch_id,branch_description from tbl_master_branch"></asp:SqlDataSource>
    </div>
    <dxe:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" ContainerElementID="divSubmitButton"
        Modal="True">
    </dxe:ASPxLoadingPanel>
    <asp:HiddenField ID="hddnLengthValue" runat="server" />
    <asp:HiddenField ID="hdnPageStatus" runat="server" />
    <asp:HiddenField ID="HdnedittLength" runat="server" />
    <asp:HiddenField ID="hdndrtType" runat="server" />
    <asp:HiddenField ID="hdnTypeID" runat="server" />
    <asp:HiddenField ID="hdnidschemaId" runat="server" />
</asp:Content>
