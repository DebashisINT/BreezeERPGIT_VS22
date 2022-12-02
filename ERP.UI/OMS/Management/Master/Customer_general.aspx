<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/PopUp.Master" AutoEventWireup="true" CodeBehind="Customer_general.aspx.cs"
    Inherits="ERP.OMS.Management.Master.Customer_general" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/assests/js/ajax-dynamic-list.js"></script>
    <script src="/assests/pluggins/choosen/choosen.min.js"></script>
    <script type="text/javascript">

        $(document).ready(function () {
            $('#ddl_numberingScheme').change(function () {               

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

                    ctxt_CustDocNo.SetText("");
                    ctxt_CustDocNo.SetEnabled(true);
                    $('#txt_CustDocNo input').attr('maxlength', schemeLength);
                }
                else if ($('#ddl_numberingScheme').val() == "0") {
                    ctxt_CustDocNo.SetText("");
                    ctxt_CustDocNo.SetEnabled(true);
                }

            });


            //for choosen
            ListBind();
            ChangeSourceMainAccount();
            $('.mainAccount').show();
            $("#RequiredFieldValidator9").css("display", "none");
            //end choosen
            $('#cmbLegalStatus').change(function (s, e) {
              //  debugger;
                var legalstatus = $("#cmbLegalStatus").val();

                if ((legalstatus != '1') && (legalstatus != '27') && (legalstatus != '29') && (legalstatus != '55') && (legalstatus != '54')) {
                    $('#cmbMaritalStatus').prop('selectedIndex', 0);
                    $('#txtAnniversary').val('');
                    $('#ASPxLabel18').text("Date of Incorporation");
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

            });

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
                    $('#cmbGender').attr("disabled", false);
                    $('#cmbMaritalStatus').attr("disabled", false);
                    $('#cmbMaritalStatus').css("color", "black");
                    $('#cmbGender').css("color", "black");
                }
            }

            $(".crossBtn, #btnCancel").click(function () {

                parent.ParentCustomerOnClose($("#KeyVal_InternalID").val(), $("#hdCustomerName").val(), $("#HdCustUniqueName").val());
            });

          

        });
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
        function CloseClick(s,e) {
            parent.ParentCustomerOnClose($("#KeyVal_InternalID").val(), $("#hdCustomerName").val(), $("#HdCustUniqueName").val());
            e.processOnServer = false;
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
                url: "customer_general.aspx/GetMainAccountList",
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
        function disp_prompt(name) {
           // debugger;
            document.location.href = "Customer_Correspondence.aspx?InternalId=" + $("#KeyVal_InternalID").val() + '&&CustName=' + $("#hdCustomerName").val() + '&&UniqueName=' + $("#HdCustUniqueName").val();

        }
        function fn_ctxtClentUcc_Name_TextChanged() {
           // debugger;
            var procode = 0;
            //var ProductName = ctxtPro_Name.GetText();
            var clientName = ctxtClentUcc.GetText();
            $.ajax({
                type: "POST",
                url: "Customer_general.aspx/CheckUniqueName",
                //data: "{'ProductName':'" + ProductName + "'}",
                data: JSON.stringify({ clientName: clientName, procode: procode }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                  //  debugger;
                    var data = msg.d;
                    if (data.split('~')[0] == "True") {
                        jAlert("Already exists for " + data.split('~')[1]);
                        ctxtClentUcc.SetText("");
                        ctxtClentUcc.focus();
                        return false;
                    }
                }

            });
        }
        function Gstin2TextChanged(s, e) {

            if (!e.htmlEvent.ctrlKey) {
                if (e.htmlEvent.key != 'Control') {
                    s.SetText(s.GetText().toUpperCase());
                }
            }

        }
        function fn_btnValidate(s, e) {         
            var ret = true;             
            $('#RequiredFieldValidator1').hide();
            var clientUcc = "";
            if ($('#ddlIdType').val() == 1) {
                if (isNaN(clientUcc)) {
                    jAlert("Please type valid Phone No.");
                    ret = false;
                    cbtnSave.SetVisible(true);
                }
            } else if ($('#ddlIdType').val() == 2) {
                var panPat = /^([a-zA-Z]{5})(\d{4})([a-zA-Z]{1})$/;
                var code = /([C,P,H,F,A,T,B,L,J,G])/;
                var code_chk = clientUcc.substring(3, 4);
                if (clientUcc.search(panPat) == -1) {
                    jAlert("Please type valid PAN No.");
                    ret = false;
                    cbtnSave.SetVisible(true);
                }
                if (code.test(code_chk) == false) {
                    jAlert("Please type valid PAN No.");
                    ret = false;
                    cbtnSave.SetVisible(true);
                }
            } else if ($('#ddlIdType').val() == 3) {
                if (isNaN(clientUcc) || clientUcc.length != 12) {
                    jAlert("Please type valid Aadhar No.");
                    ret = false;
                    cbtnSave.SetVisible(true);
                }
            }
            if (($("#hdnAutoNumStg").val() == "1") && ($("#hdnTransactionType").val() == "CL")) {
                $("#hddnDocNo").val(ctxt_CustDocNo.GetText());
                ctxtClentUcc.SetText("ABC");
            }
            else
            {
                clientUcc = ctxtClentUcc.GetText();
            }
            var contype = 'customer';
            if (contype == 'customer') {             
                if (($("#hdnAutoNumStg").val() == "1") && ($("#hdnTransactionType").val() == "CL") && ($("#InsertMode").val() == "Add")) {
                    if ($("#ddl_numberingScheme").val() == "0") {
                        jAlert("Please Select Numbering Scheme.");                

                        ret = false;
                        return false;
                        cbtnSave.SetVisible(true);
                    }
                    else if (ctxt_CustDocNo.GetText() == "") {
                        jAlert("Please Enter Document No.");
                        ret = false;
                        return false;
                        cbtnSave.SetVisible(true);
                    }
                }
                Page_ClientValidate();
                $('#invalidGst').css({ 'display': 'none' });
                var gst1 = ctxtGSTIN1.GetText().trim();
                var gst2 = ctxtGSTIN2.GetText().trim();
                var gst3 = ctxtGSTIN3.GetText().trim();
                if (gst1.length == 0 && gst2.length == 0 && gst3.length == 0) {                  
                }
                else {
                    if (gst1.length != 2 || gst2.length != 10 || gst3.length != 3) {
                        $('#invalidGst').css({ 'display': 'block' });
                        ret = false;
                        cbtnSave.SetVisible(true);
                    }
                    var panPat = /^([a-zA-Z]{5})(\d{4})([a-zA-Z]{1})$/;
                    var code = /([C,P,H,F,A,T,B,L,J,G])/;
                    var code_chk = gst2.substring(3, 4);
                    if (gst2.search(panPat) == -1) {
                        $('#invalidGst').css({ 'display': 'block' });
                        ret = false;
                        cbtnSave.SetVisible(true);
                    }
                    if (code.test(code_chk) == false) {
                        $('#invalidGst').css({ 'display': 'block' });
                        ret = false;
                        cbtnSave.SetVisible(true);
                    }
                }
            }
            if (contype != 'Lead') {              
                var txtbox = ctxtFirstNmae.GetText();                
                var txt2 = ctxtClentUcc.GetText();
                if (txtbox == "") {
                    jAlert("Please Insert Full Name.");                   
                    $('#RequiredFieldValidator1').show();
                    ret = false;
                    return false;
                    cbtnSave.SetVisible(true);
                }
                else if (txt2== "") {
                    alert("Please Insert Unique Code.");
                    ctxtClentUcc.Focus();                  
                    ret = false;
                    cbtnSave.SetVisible(true);
                }
            }
            if (contype != 'Lead') {              
                var selectoption = document.getElementById("cmbLegalStatus");
                var optionText = selectoption.options[selectoption.selectedIndex].text;             
                if (optionText == 'General') {
                    ret = SetVechileNo();
                }
            }
            e.processOnServer = ret;
           
            cbtnSave.SetVisible(false);
        }

        function UniqueCodeCheck() {
          //  debugger;
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
             

                $.ajax({
                    type: "POST",
                    url: "Customer_general.aspx/CheckUniqueNumberingCode",
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


    </script>
    <style type="text/css">
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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>
                <span>Add Customer/Client</span>
            </h3>
            <div class="crossBtn"><a id="a_crossBtn"><i class="fa fa-times"></i></a></div>
        </div>

        <asp:HiddenField ID="hdKeyVal" runat="server" />
        <asp:HiddenField ID="KeyVal_InternalID" runat="server" />
        <asp:HiddenField ID="hdCustomerName" runat="server" />
         <asp:HiddenField ID="HdCustUniqueName" runat="server" />
        <asp:HiddenField runat="server" ID="IsUdfpresent" />
        <asp:HiddenField ID="hidAssociatedEmp" runat="server" />
    </div>
    <div class="form_main">
        <table class="TableMain100">
            <tr>
                <td>
                    <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" ClientInstanceName="page"
                        Font-Size="12px" OnActiveTabChanged="ASPxPageControl1_ActiveTabChanged" Width="100%">
                        <TabPages>
                            <dxe:TabPage Name="General" Text="General">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                        <table class="TableMain100" id="table_others" runat="server">
                                            <tr>
                                                <td style="width: 500px">
                                                    <asp:Panel ID="Panel1" runat="server" BorderColor="White" BorderWidth="2px" Width="100%" CssClass="row">
                                                        <div class="clear"></div>
                                                        <div class="col-md-3">
                                                            <dxe:ASPxLabel ID="ASPxLabel20" runat="server" Text="Customer Type" CssClass="inline">
                                                            </dxe:ASPxLabel>
                                                            <span style="text-align: left; float: left; font-size: medium; color: Red; width: 8px;">*</span>
                                                            <asp:DropDownList ID="cmbLegalStatus" runat="server" Width="100%">
                                                            </asp:DropDownList>
                                                        </div>

                                                           <div class="col-md-3" runat="server" id="dvIdType">
                                                               <dxe:ASPxLabel ID="ASPxLabel19" runat="server" Text="Type" CssClass="inline">
                                                               </dxe:ASPxLabel>
                                                          
                                                                   <asp:DropDownList ID="ddlIdType" runat="server"  Width="100%">
                                                                       <%--<asp:ListItem Value="0">--Select--</asp:ListItem>--%>
                                                                       <asp:ListItem Value="1">Phone</asp:ListItem>
                                                                       <asp:ListItem Value="2">PAN</asp:ListItem>
                                                                       <asp:ListItem Value="3">Aadhar No.</asp:ListItem>
                                                                   </asp:DropDownList>
                                                               </div>

                                                        <%--Customer Type--%>
                                                        <div class="col-md-3" runat="server" id="dvUniqueId">
                                                            <dxe:ASPxLabel ID="ASPxLabel12" runat="server" Text="Unique ID" CssClass="inline ">
                                                            </dxe:ASPxLabel>
                                                            <span id="ASPxLabelS12" style="text-align: left; float: left; font-size: medium; color: Red; width: 8px;" runat="server">*</span>
                                                           
                                                            <%--txt unique code--%>
                                                            <div style="position: relative">
                                                                <dxe:ASPxTextBox ID="txtMiddleName" Visible="false" runat="server" Width="160px" CssClass="upper">
                                                                </dxe:ASPxTextBox>
                                                                <dxe:ASPxTextBox ID="txtClentUcc" runat="server" ClientInstanceName="ctxtClentUcc" Width="100%" MaxLength="50">
                                                                    <ClientSideEvents TextChanged="function(s,e){fn_ctxtClentUcc_Name_TextChanged()}" />
                                                                </dxe:ASPxTextBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtClentUcc" ValidationGroup="contact" SetFocusOnError="true" ToolTip="Mandatory" class="tp28 pullrightClass fa fa-exclamation-circle abs iconRed" ErrorMessage=""></asp:RequiredFieldValidator>
                                                                <asp:LinkButton ID="LinkButton1" runat="server" Visible="false" OnClick="LinkButton1_Click" Style="color: #cc3300; text-decoration: underline; font-size: 8pt;">Make System Generate UCC</asp:LinkButton>
                                                                <asp:Label ID="lblErr" Text="" runat="server" Visible="false"></asp:Label>
                                                                  
                                                                <span id="InvalidShortName" style="display: none;top: -18px;left: -95px; "  >
                                                                    <img id="mandatoryInvalidShortName" style="position: absolute; right: -18px; top: 29px;" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Invalid Unique ID">
                                                                </span>

                                                            </div>
                                                            <%--rev srijeeta--%>
                                                            <%--<div class="col-md-3" runat="server" id="Div1">--%>
                                                           

                                                            <div style="position: relative">
                                                                 <dxe:ASPxLabel ID="ASPxLabel1" runat="server" Text="Alternative_Code " CssClass="inline " MaxLength="100">
                                                            </dxe:ASPxLabel>
                                                            <dxe:ASPxTextBox ID="Alternative_Code" runat="server" ClientInstanceName="ctxtClentaltcode" Width="100%" MaxLength="100">
                                                                    
                                                                </dxe:ASPxTextBox>
                                                            </div>
                                                            <%--rev srijeeta--%>
                                                        </div> 


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
                                                                <dxe:ASPxLabel ID="lbl_CustDocNo" runat="server" Text="Document No" Width="">
                                                                </dxe:ASPxLabel>
                                                                <span style="color: red">*</span>
                                                            </label>

                                                            <dxe:ASPxTextBox ID="txt_CustDocNo" runat="server" ClientInstanceName="ctxt_CustDocNo" Width="100%" MaxLength="30">
                                                                <ClientSideEvents TextChanged="function(s, e) {UniqueCodeCheck();}" />
                                                            </dxe:ASPxTextBox>
                                                          

                                                        </div>



                                                        <%--end--%>




                                                        <%--Short Name--%>
                                                        <div class="col-md-3">
                                                            <dxe:ASPxLabel ID="ASPxLabel5" runat="server" Text="Full Name" CssClass="inline">
                                                            </dxe:ASPxLabel>
                                                            <span style="text-align: left; float: left; font-size: medium; color: Red; width: 8px;">*</span>
                                                            <div style="position: relative">
                                                                <dxe:ASPxTextBox ID="txtFirstNmae" runat="server" Width="100%" MaxLength="100" CssClass="upper" ClientInstanceName="ctxtFirstNmae">
                                                                </dxe:ASPxTextBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtFirstNmae" ValidationGroup="contact" SetFocusOnError="true" class="tp28 pullrightClass fa fa-exclamation-circle abs iconRed" ToolTip="Mandatory" ErrorMessage=""></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>
                                                        <%--Full Name--%>
                                                        <div style="clear: both;"></div>

                                                        <div class="col-md-3">
                                                            <%--lbl D.O.B F--%>
                                                            <div class="labelt">

                                                                <dxe:ASPxLabel ID="ASPxLabel18" runat="server" Text="Date of Birth">
                                                                </dxe:ASPxLabel>

                                                            </div>
                                                            <%--txt D.O.B F--%>
                                                            <div>

                                                                <dxe:ASPxDateEdit ID="txtDOB" runat="server" Width="100%" EditFormat="Custom" EditFormatString="dd-MM-yyyy"
                                                                    UseMaskBehavior="True">
                                                                    <ButtonStyle Width="13px">
                                                                    </ButtonStyle>
                                                                </dxe:ASPxDateEdit>

                                                            </div>
                                                        </div>
                                                        <%--D.O.B--%>
                                                        <div class="col-md-3">
                                                            <%-- lbl Nationality--%>
                                                            <div class="labelt">
                                                                <dxe:ASPxLabel ID="ASPxLabel34" Visible="false" runat="server" Text="Contact Status">
                                                                </dxe:ASPxLabel>
                                                                <dxe:ASPxLabel ID="ASPxLabel35" Width="120px" runat="server" Text="Nationality">
                                                                </dxe:ASPxLabel>
                                                            </div>
                                                            <%-- lbl Nationality--%>
                                                            <div style="position: relative">
                                                                <asp:DropDownList Visible="false" ID="cmbContactStatus" runat="server" Width="100%">
                                                                </asp:DropDownList>

                                                                <asp:DropDownList ID="ddlnational" runat="server" Width="100%">
                                                                    <%--<asp:ListItem Value="1">Indian</asp:ListItem>
                                                                    <asp:ListItem Value="2">Others</asp:ListItem>--%>
                                                                </asp:DropDownList>



                                                            </div>
                                                        </div>
                                                        <%--Nationality--%>
                                                        <div class="col-md-3 visF" id="td_lAnniversary">
                                                            <div class="labelt">
                                                                <dxe:ASPxLabel ID="ASPxLabel21" runat="server" Text="Anniversary Dates">
                                                                </dxe:ASPxLabel>
                                                            </div>
                                                            <%--txt Anniversary--%>
                                                            <%--<div id="td_tAnniversary">--%>
                                                            <div class="visF">
                                                                <dxe:ASPxDateEdit ID="txtAnniversary" runat="server" Width="100%" EditFormat="Custom" ClientInstanceName="txtAnniversary"
                                                                    EditFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                                                                    <ButtonStyle Width="13px">
                                                                    </ButtonStyle>
                                                                </dxe:ASPxDateEdit>
                                                            </div>
                                                            <%--</div>--%>
                                                        </div>
                                                        <%--Anniversary--%>
                                                        <div class="col-md-3 visF">
                                                            <div id="td_lGender" class="labelt">
                                                                <div class="visF">
                                                                    <dxe:ASPxLabel ID="ASPxLabel14" runat="server" Text="Gender">
                                                                    </dxe:ASPxLabel>
                                                                </div>
                                                            </div>
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
                                                        <%--Gender--%>
                                                        <div style="clear: both;"></div>

                                                        <asp:Panel ID="pnlCredit" runat="server">
                                                            <div class="clear"></div>
                                                            <div class="col-md-3">
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
                                                                    <dxe:ASPxTextBox ID="txtCreditLimit" runat="server" Width="100%" MaxLength="18" Text="0">
                                                                        <%--     Code added By Priti on 16122016 to use decimal value--%>
                                                                        <MaskSettings Mask="<0..999999999999g>.<0..99g>" />
                                                                    </dxe:ASPxTextBox>
                                                                    <%--  ...end...--%>
                                                                </div>
                                                            </div>
                                                            <div class="clear"></div>
                                                        </asp:Panel>
                                                        <%--Credit Hold,Credit Days,Credit Limit--%>
                                                        <div style="clear: both;"></div>

                                                        <div class="col-md-3 visF">
                                                            <div id="td_lMarital" class="labelt">
                                                                <div class="visF">
                                                                    <dxe:ASPxLabel ID="ASPxLabel16" runat="server" Text="Marital Status">
                                                                    </dxe:ASPxLabel>
                                                                </div>
                                                            </div>
                                                            <div id="td_dMarital">
                                                                <div class="visF">
                                                                    <asp:DropDownList ID="cmbMaritalStatus" runat="server" Width="100%">
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <%--Marital Status--%>
                                                        <div class="col-md-3 visF">
                                                            <div id="td_lMaritals" class="labelt">
                                                                <div class="visF">
                                                                    <dxe:ASPxLabel ID="txtContactStatusclient" runat="server" Text="Status">
                                                                    </dxe:ASPxLabel>
                                                                </div>
                                                            </div>

                                                            <div id="td_dMaritals4">
                                                                <div class="visF">
                                                                    <dxe:ASPxComboBox ID="cmbContactStatusclient" ClientInstanceName="cCmbStatus" runat="server" SelectedIndex="0"
                                                                        ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True">
                                                                        <Items>
                                                                            <dxe:ListEditItem Text="Active" Value="A" />
                                                                            <dxe:ListEditItem Text="Dormant" Value="D" />
                                                                        </Items>
                                                                    </dxe:ASPxComboBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <%--Status--%>
                                                        <div style="clear: both;"></div>

                                                        <div class="col-md-3 mainAccount hide">
                                                            <div class="padBot5 lblmTop8" style="display: block;">
                                                                <span>Main Account</span>
                                                            </div>
                                                            <div class="Left_Content">
                                                                <asp:ListBox ID="lstTaxRates_MainAccount" CssClass="chsn" runat="server" Font-Size="12px" Width="100%" data-placeholder="Select..." onchange="changeFunc();"></asp:ListBox>
                                                                <asp:HiddenField ID="hndTaxRates_MainAccount_hidden" runat="server" />
                                                                <asp:HiddenField ID="hdIsMainAccountInUse" runat="server" />
                                                            </div>
                                                        </div>
                                                        <%--Main Account--%>
                                                        <div id="divGSTIN" class="col-md-4 forCustomer">
                                                            <label class="labelt">GSTIN</label>
                                                            <span id="spanmandategstn" style="color: red; display: none;">*</span>
                                                            <div class="relative">
                                                                <ul class="nestedinput">
                                                                    <li>
                                                                        <dxe:ASPxTextBox ID="txtGSTIN1" ClientInstanceName="ctxtGSTIN1" MaxLength="2" runat="server" Width="33px">
                                                                            <ClientSideEvents KeyPress="function(s,e){ fn_AllowonlyNumeric(s,e);}" />
                                                                        </dxe:ASPxTextBox>
                                                                    </li>
                                                                    <li class="dash">- </li>
                                                                    <li>
                                                                        <dxe:ASPxTextBox ID="txtGSTIN2" ClientInstanceName="ctxtGSTIN2" MaxLength="10" runat="server" Width="90px">
                                                                            <ClientSideEvents KeyUp="Gstin2TextChanged" />
                                                                        </dxe:ASPxTextBox>
                                                                    </li>
                                                                    <li class="dash">- </li>
                                                                    <li>
                                                                        <dxe:ASPxTextBox ID="txtGSTIN3" ClientInstanceName="ctxtGSTIN3" MaxLength="3" runat="server" Width="50px">
                                                                        </dxe:ASPxTextBox>
                                                                        <span id="invalidGst" class="fa fa-exclamation-circle iconRed " style="color: red; display: none; padding-left: 9px; left: 302px;" title="Invalid GSTIN"></span>
                                                                    </li>
                                                                </ul>

                                                            </div>
                                                        </div>

                                                          <div class="col-md-2" id="dvTCSApplicable" runat="server">

                                                            <div class="Left_Content" style="padding-top:26px">
                                                                  <dxe:ASPxCheckBox ID="TCSApplicable" ClientInstanceName="cTCSApplicable" Checked="false" Text="TCS Applicable" TextAlign="Right" runat="server">
                                                                      
                                                                  </dxe:ASPxCheckBox>
                                                            </div>
                       
                                                          </div>

                                                        <%--GSTIN--%>
                                                        <div class="clear"></div>

                                                        <div class="col-md-12" style="padding-top: 15px;">
                                                            <asp:HiddenField ID="hdReferenceBy" runat="server" />
                                                            <dxe:ASPxButton ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" ClientInstanceName="cbtnSave" AutoPostBack="false" OnClick="btnSave_Click"
                                                                ValidationGroup="contact">
                                                                <ClientSideEvents Click="fn_btnValidate" />
                                                            </dxe:ASPxButton>
                                                           <%-- <dxe:ASPxButton ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-danger">
                                                                  <ClientSideEvents Click="CloseClick" />
                                                            </dxe:ASPxButton>--%>
                                                                <span class="btn btn-danger" id="btnCancel"><span>Cancel</span></span>


                                                            <asp:Button ID="btnUdf" runat="server" Visible="false" Text="UDF" CssClass="btn btn-primary dxbButton" OnClientClick="if(OpenUdf()){ return false;}" />
                                                        </div>
                                                        <%--Button--%>
                                                    </asp:Panel>


                                                </td>
                                            </tr>
                                        </table>
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Correspondence" Text="Correspondence">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                        </TabPages>
                        <ClientSideEvents ActiveTabChanged="disp_prompt"></ClientSideEvents>
                    </dxe:ASPxPageControl>
                </td>
            </tr>
        </table>

    </div>
    <asp:HiddenField runat="server" ID="hdnAutoNumStg" />
     <asp:HiddenField runat="server" ID="hdnTransactionType" />
    <asp:HiddenField runat="server" ID="hddnDocNo" />
     <asp:HiddenField runat="server" ID="hdnNumberingId" />
    <asp:HiddenField runat="server" ID="InsertMode" />
    <asp:HiddenField runat="server" ID="hdnSyncCustomertoFSMWhileCreating" />
</asp:Content>
