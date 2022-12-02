<%@ Page Title="Companies" Language="C#" AutoEventWireup="True" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.Master.management_master_rootcompany_general" CodeBehind="rootcompany_general.aspx.cs" %>

<%--<%@ Register Assembly="DevExpress.Web.v9.1, Version=9.1.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%--<%@ Register Assembly="DevExpress.Web.v9.1, Version=9.1.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxTabControl" TagPrefix="dxe" %>--%>
<%--<%@ Register Assembly="DevExpress.Web.v9.1, Version=9.1.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxClasses" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.1, Version=9.1.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.1, Version=9.1.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.1, Version=9.1.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxTabControl" TagPrefix="dxe" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--    <link type="text/css" href="../../CSS/style.css" rel="Stylesheet" />

    <script type="text/javascript" src="/assests/js/loaddata1.js"></script>--%>
    <%--    <style>
        #ASPxPageControl1_AT0T, #ASPxPageControl1_T1T, #ASPxPageControl1_T4T {
            font-size: 12px !important;
        }
    </style>--%>


    <script language="javascript" type="text/javascript">


        //Code for UDF Control 
        function OpenUdf() {
            if (document.getElementById('IsUdfpresent').value == '0') {
                jAlert("UDF not define.");
            }
            else {
                var keyVal = document.getElementById('Keyval_internalId').value;
                var url = '/OMS/management/Master/frm_BranchUdfPopUp.aspx?Type=Cmp&&KeyVal_InternalID=' + keyVal;
                popup.SetContentUrl(url);
                popup.Show();
            }
            return true;
        }
        // End Udf Code
        //Debjyoti Gstin  



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


        function IsCompanyDataValid() {
            Page_ClientValidate();
            var returnData = true;
            $('#invalidGst').css({ 'display': 'none' });
            $('#invalidPan').css({ 'display': 'none' });
            $('#badd1').css({ 'display': 'none' });
            var gst1 = ctxtGSTIN1.GetText().trim();
            var gst2 = ctxtGSTIN2.GetText().trim();
            var gst3 = ctxtGSTIN3.GetText().trim();
            var deductCategory = cdrdCategory.GetText().trim();

            if (gst1.length == 0 && gst2.length == 0 && gst3.length == 0) {

            }
            else {
                if (gst1.length != 2 || gst2.length != 10 || gst3.length != 3) {
                    $('#invalidGst').css({ 'display': 'block' });
                    returnData = false;
                }


                var panPat = /^([a-zA-Z]{5})(\d{4})([a-zA-Z]{1})$/;
                var code = /([C,P,H,F,A,T,B,L,J,G])/;
                var code_chk = gst2.substring(3, 4);
                if (gst2.search(panPat) == -1) {
                    $('#invalidGst').css({ 'display': 'block' });
                    returnData = false;
                }
                if (code.test(code_chk) == false) {
                    $('#invalidGst').css({ 'display': 'block' });
                    returnData = false;
                }

            }

            //if (deductCategory == "") {
            //    $('#badd1').css({ 'display': 'block' });
            //    returnData = false;
            //}
            var panVal = $('#txtlocalSalesTax').val();
            var regpan = /^([a-zA-Z]){4}([0-9]){5}([a-zA-Z]){1}?$/;
            if ($('#txtlocalSalesTax').val() != "") {
                if (!regpan.test(panVal)) {
                    alert("Invaild TAN No."); // valid pan card number
                    returnData = false;
                }
            }



            //for pan Card Validation
            var panPat1 = /^([a-zA-Z]{5})(\d{4})([a-zA-Z]{1})$/;
            var code1 = /([C,P,H,F,A,T,B,L,J,G])/;
            var panCard = document.getElementById('txtPanNo').value.trim();

            if (panCard.length != 0) {
                var code_chk1 = panCard.substring(3, 4);
                if (panCard.search(panPat1) == -1) {
                    $('#invalidPan').css({ 'display': 'block' });
                    returnData = false;
                }
                if (code1.test(code_chk1) == false) {
                    $('#invalidPan').css({ 'display': 'block' });
                    returnData = false;
                }
            }
            return returnData;
        }

        //End Here

        function disp_prompt(name) {
            if (name == "tab0") {
                //alert(name);
                //document.location.href="rootcompany_general.aspx"; 
            }
            if (name == "tab1") {
                //alert(name);
                document.location.href = "rootComp_Correspondence.aspx";
            }
            if (name == "tab2") {
                //alert(name);
                document.location.href = "rootComp_exchange.aspx";
            }
            else if (name == "tab3") {
                //alert(name);
                document.location.href = "rootComp_dpMembership.aspx";
            }
            else if (name == "tab4") {
                //alert(name);<a href="">rootComp_document.aspx</a>
                document.location.href = "rootComp_document.aspx";
            }
            else if (name == "tab5") {
                //alert(name);<a href="">rootComp_document.aspx</a>
                document.location.href = "rootcompany_deductorinfo.aspx";
            }
            else if (name == "tab6") {
                //alert(name);<a href="">rootComp_document.aspx</a>
                document.location.href = "rootComp_Remarks.aspx";
            }


            else if (name == "tab7") {
                document.location.href = "rootcompany_logo.aspx";
            }
        }

        function ValidateGeneral() {

            var obj0 = document.getElementById('txtCompname').value;
            var obj1 = document.getElementById('txtOnRole').value;
            var obj2 = document.getElementById('txtOffRole').value;

            if (obj0.length < 1) {
                alert("Company Name Required !");
                return false;
            }
            if (obj1.length < 1) {
                alert("Short Name [On Roll Employee] Required !");
                return false;
            }
            if (obj2.length < 1) {
                alert("Short Name [Off Roll Employee] Required !");
                return false;
            }
            if (obj1.length < 4) {
                alert("Short Name [On Roll Employee] should be 4 Characters !");
                return false;
            }
            if (obj2.length < 4) {
                alert("Short Name [Off Roll Employee] should be 4 Characters !");
                return false;
            }


            var statenm = txtCompname.GetText();
            if (statenm.trim() == '')
                //if (trim(ctxtStateName.GetText()) == '')
            {
                alert('Please enter company name !');
                txtCompname.Focus();
            }
        }
    </script>

    <style type="text/css">
        .pullleftClass {
            position: absolute;
            right: -19px;
            top: 5px;
            ;
        }

        #RequiredFieldValidator1 {
            position: absolute;
            right: -19px;
            top: 5px;
        }
        /*.abs1 {
            position:absolute;
            right: 50px;
            top: 410px;
        }
           .abs2 {
            position:absolute;
            right: 608px;
            top: 410px;
        }*/
        #lblVF_erp {
            position: absolute;
            right: -19px;
            top: 5px;
        }

        #lblVUESIC {
            position: absolute;
            right: 9px;
            top: 8px;
        }

        .col-md-3 label {
            margin-top: 8px;
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
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>&nbsp;<asp:Label ID="Label1" runat="server"></asp:Label>
                &nbsp;Company</h3>
            <div class="crossBtn"><a href="root_Companies.aspx"><i class="fa fa-times"></i></a></div>
        </div>
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
    <asp:HiddenField runat="server" ID="IsUdfpresent" />
    <asp:HiddenField runat="server" ID="Keyval_internalId" />
    <%--End debjyoti 22-12-2016--%>

    <div class="form_main">
        <table class="TableMain100">
            <tr>
                <td>
                    <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" ClientInstanceName="page"
                        Width="100%">
                        <TabPages>
                            <dxe:TabPage Text="General" Name="General">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                        <div class="">
                                            <div class="col-md-3">
                                                <label>Parent Company </label>
                                                <div>
                                                    <asp:DropDownList ID="drpParentComp" runat="server" Width="100%">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>Company Name <em style="color: red">*</em> </label>
                                                <div class="relative">
                                                    <asp:TextBox ID="txtCompname" runat="server" Width="100%" MaxLength="50"></asp:TextBox>
                                                    <asp:RequiredFieldValidator Display="Dynamic" runat="server" ID="rfvComname" ControlToValidate="txtCompname"
                                                        SetFocusOnError="true" ErrorMessage="" ValidationGroup="rqCompany" ForeColor="Red" CssClass="pullleftClass fa fa-exclamation-circle iconRed" ToolTip="Mandatory">                                                        
                                                    </asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>Nature Of Business </label>
                                                <div>
                                                    <asp:TextBox ID="txtNameofBusiness" runat="server" Width="100%" MaxLength="50"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>Directors </label>
                                                <div>
                                                    <asp:TextBox ID="txtDirectors" runat="server" TextMode="MultiLine" Width="100%" Height="40px" MaxLength="50"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>Authorised Signatories </label>
                                                <div>
                                                    <asp:TextBox ID="txtAuthorised" runat="server" TextMode="MultiLine" Width="100%" Height="40px" MaxLength="50"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>CST Registration No </label>
                                                <div>
                                                    <asp:TextBox ID="txtRegnNo" runat="server" Width="100%" MaxLength="50"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>CST Validity Date </label>

                                                <dxe:ASPxDateEdit ID="txtcstVdate" runat="server" EditFormat="Custom" EditFormatString="dd MMMM yyyy"
                                                    UseMaskBehavior="True" Width="100%">
                                                    <ButtonStyle Width="13px">
                                                    </ButtonStyle>
                                                </dxe:ASPxDateEdit>

                                            </div>
                                            <div class="col-md-3">
                                                <label>PAN No. </label>
                                                <div>
                                                    <asp:TextBox ID="txtPanNo" runat="server" Width="100%" MaxLength="10"></asp:TextBox>
                                                    <span id="invalidPan" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; display: none; padding-left: 9px; right: -6px; top: 32px;" title="Invalid PAN No."></span>
                                                </div>
                                            </div>
                                            <div class="clear"></div>
                                            <div class="col-md-3">
                                                <label>
                                                    VAT / TIN Registration No. 
                                                </label>
                                                <div>
                                                    <asp:TextBox ID="txtVatRegNo" runat="server" Width="100%" MaxLength="20"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>
                                                    Service Tax Regn. No. 
                                                </label>
                                                <div>
                                                    <asp:TextBox ID="txtservicetaxNo" runat="server" Width="100%" MaxLength="50"></asp:TextBox>
                                                </div>
                                            </div>


                                            <div class="col-md-3">
                                                <label>TAN </label>
                                                <div>
                                                    <asp:TextBox ID="txtlocalSalesTax" runat="server" Width="100%" MaxLength="50"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>Local Sales Tax Validity Date </label>
                                                <div>
                                                    <dxe:ASPxDateEdit ID="txtLocalVdate" runat="server" EditFormat="Custom"
                                                        UseMaskBehavior="True" Width="100%">
                                                        <ButtonStyle Width="13px">
                                                        </ButtonStyle>
                                                    </dxe:ASPxDateEdit>
                                                </div>
                                            </div>
                                            <div class="clear"></div>
                                            <div class="col-md-3">
                                                <label>CIN </label>
                                                <div>
                                                    <asp:TextBox ID="txtCIN" runat="server" Width="100%" MaxLength="50"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>CIN Validity Date </label>
                                                <div>
                                                    <dxe:ASPxDateEdit ID="txtCINVdate" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                        Width="100%">
                                                        <ButtonStyle Width="13px">
                                                        </ButtonStyle>
                                                    </dxe:ASPxDateEdit>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>Date of Incorporation </label>
                                                <div>
                                                    <dxe:ASPxDateEdit ID="txtincorporateDate" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                        Width="100%">
                                                        <ButtonStyle Width="13px">
                                                        </ButtonStyle>
                                                    </dxe:ASPxDateEdit>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>Currency </label>
                                                <div>
                                                    <asp:DropDownList ID="ddlcurrency" runat="server" Width="100%">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="clear"></div>
                                            <div class="col-md-3">
                                                <label>EPF Registration No. </label>
                                                <div>
                                                    <asp:TextBox ID="txtErpRegistration" runat="server" Width="100%" MaxLength="20"></asp:TextBox>
                                                    <%--   <asp:Label ID="l_EPFRegistrationNo" runat="server" Text="" Tooltip="Mandatory"  CssClass="pullleftClass fa fa-exclamation-circle" Visible="false"></asp:Label>--%>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>EPF Registration No (Valid from)</label>
                                                <div>
                                                    <dxe:ASPxDateEdit ID="txtErpValidFrom" runat="server" EditFormat="Custom" UseMaskBehavior="True" Width="100%" EditFormatString="dd-MM-yyyy">
                                                        <ButtonStyle Width="13px">
                                                        </ButtonStyle>
                                                    </dxe:ASPxDateEdit>

                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>EPF Registration No (Valid upto)</label>
                                                <div>
                                                    <dxe:ASPxDateEdit ID="txtErpValidUpto" runat="server" EditFormat="Custom" UseMaskBehavior="True" Width="100%" EditFormatString="dd-MM-yyyy">
                                                        <ButtonStyle Width="13px">
                                                        </ButtonStyle>
                                                    </dxe:ASPxDateEdit>
                                                    <asp:Label ID="lblVF_erp" runat="server" Text="" CssClass="pullleftClass fa fa-exclamation-circle" ToolTip="EPF Registration No. Valid Upto should be greater than Valid From" ForeColor="Red" Visible="false"></asp:Label>
                                                    <%-- <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Valid Upto should be greater than Valid From" ControlToCompare="txtErpValidFrom" 
                                                        ControlToValidate="txtErpValidUpto" ForeColor="Red" Operator="GreaterThan" ValidationGroup="rqCompany" Type="Date"></asp:CompareValidator>--%>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>ESIC Registration No. </label>
                                                <div>
                                                    <asp:TextBox ID="txtESIC" runat="server" MaxLength="20" Width="100%"></asp:TextBox>
                                                    <%--    <asp:Label ID="l_ESICRegistrationNo" runat="server" Text="" Tooltip="Mandatory"  CssClass="pullleftClass fa fa-exclamation-circle" Visible="false"></asp:Label>--%>
                                                </div>
                                            </div>
                                            <div class="clear"></div>
                                            <div class="col-md-3">
                                                <label>ESIC Registration No (Valid from)</label>
                                                <div>
                                                    <dxe:ASPxDateEdit ID="txtESICValidFrom" runat="server" EditFormat="Custom" UseMaskBehavior="True" Width="100%" EditFormatString="dd-MM-yyyy">
                                                        <ButtonStyle Width="13px">
                                                        </ButtonStyle>
                                                    </dxe:ASPxDateEdit>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>ESIC Registration No (Valid upto)</label>
                                                <div style="position: relative">
                                                    <dxe:ASPxDateEdit ID="txtESICValidUpto" runat="server" EditFormat="Custom" UseMaskBehavior="True" Width="100%" EditFormatString="dd-MM-yyyy">
                                                        <ButtonStyle Width="13px">
                                                        </ButtonStyle>
                                                    </dxe:ASPxDateEdit>
                                                    <asp:Label ID="lblVUESIC" runat="server" Text="" CssClass=" fa fa-exclamation-circle" ToolTip="ESIC Registration No. Valid Upto should be greater than Valid From" ForeColor="Red" Visible="false"></asp:Label>

                                                </div>
                                            </div>


                                            <div class="col-md-3" style="display: none">
                                                <label>Shortname[On Roll Employee]<span style="color: red;"> *</span> </label>
                                                <div class="relative">
                                                    <asp:TextBox ID="txtOnRole" runat="server" Width="100%" MaxLength="15"></asp:TextBox>
                                                    <%-- <asp:RequiredFieldValidator Display="Dynamic" runat="server" ID="RequiredFieldValidator1" ControlToValidate="txtOnRole"
                                                        SetFocusOnError="true" ErrorMessage="" ValidationGroup="rqCompany" ForeColor="Red" CssClass="pullleftClass fa fa-exclamation-circle" ToolTip="Mandatory">                                                        
                                                    </asp:RequiredFieldValidator>--%>
                                                </div>
                                            </div>
                                            <div class="col-md-3" style="display: none">
                                                <label>Shortname[Off Roll Employee]<span style="color: red;"> *</span> </label>
                                                <div class="relative">
                                                    <asp:TextBox ID="txtOffRole" runat="server" Width="100%" MaxLength="15"></asp:TextBox>
                                                    <%-- <asp:RequiredFieldValidator Display="Dynamic" runat="server" ID="RequiredFieldValidator2" ControlToValidate="txtOffRole"
                                                        SetFocusOnError="true" ErrorMessage="" ValidationGroup="rqCompany" ForeColor="Red" CssClass="pullleftClass fa fa-exclamation-circle" ToolTip="Mandatory">                                                        
                                                    </asp:RequiredFieldValidator>--%>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>On Roll Scheme <span style="color: red;">*</span> </label>
                                                <div class="relative">
                                                    <asp:DropDownList ID="drpdwn_schema_on" runat="server" Width="100%">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="validate_schema_on" runat="server" SetFocusOnError="true" Display="Dynamic"
                                                        ControlToValidate="drpdwn_schema_on" ValidationGroup="rqCompany" ForeColor="Red" CssClass="pullleftClass fa fa-exclamation-circle iconRed"
                                                        ToolTip="Mandatory"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>Off Roll Scheme <span style="color: red;">*</span> </label>
                                                <div class="relative">
                                                    <asp:DropDownList ID="drpdwn_schema_off" runat="server" Width="100%">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="validate_schema_off" runat="server" SetFocusOnError="true" Display="Dynamic"
                                                        ControlToValidate="drpdwn_schema_off" ValidationGroup="rqCompany" ForeColor="Red" CssClass="pullleftClass fa fa-exclamation-circle iconRed"
                                                        ToolTip="Mandatory"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>

                                            <div class="col-md-3" style="display: none;">

                                                <label>
                                                    Kra Prefix :
                                                        Kra Intermediatory ID
                                                </label>
                                                <div style="display: none;">
                                                    <asp:TextBox runat="server" ID="txtKraPrefix" MaxLength="8" Width="100%"></asp:TextBox>
                                                    <asp:TextBox ID="txtKraintermideatoryid" runat="server" MaxLength="12" Width="100%"></asp:TextBox>
                                                </div>
                                            </div>

                                            <%--Debjyoti For GSTIN--%>
                                            <div class="col-md-3">
                                                <label>GSTIN   </label>
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
                                                            </dxe:ASPxTextBox>
                                                        </li>
                                                        <li class="dash">- </li>
                                                        <li>
                                                            <dxe:ASPxTextBox ID="txtGSTIN3" ClientInstanceName="ctxtGSTIN3" MaxLength="3" runat="server" Width="50px">
                                                            </dxe:ASPxTextBox>

                                                        </li>
                                                        <li><span id="invalidGst" class="fa fa-exclamation-circle iconRed " style="color: red; display: none; padding: 5px 0 0 9px;" title="Invalid GSTIN"></span></li>
                                                    </ul>

                                                    <%-- </div>--%>
                                                </div>




                                            </div>
                                            <div class="col-md-3">
                                                <label>Category(deductor/collector)</label>
                                                <div class="relative">
                                                    <dxe:ASPxComboBox ID="drdCategory" ClientInstanceName="cdrdCategory" Enabled="true"
                                                        runat="server" Width="100%" MaxLength="20" DataSourceID="drCategory" ValueField="deductcategory_value" TextField="deductcategory_description">
                                                    </dxe:ASPxComboBox>
                                                    <%-- <span id="badd1" style="display: none ;position: absolute;    right: 37px;top: 79px;" class="mandt" >--%>
                                                    <span id="badd1" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; display: none;" title="Empty Category"></span>
                                                    <%--       <img id="grid_DXPEForm_DXEFL_DXEditor2_EiI" title="Mandatory" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-YRohc" alt="Required" />--%>
                                                    <asp:SqlDataSource runat="server" ID="drCategory" SelectCommand="SELECT [deductcategory_value], [deductcategory_description] deductcategory_description FROM [Tbl_master_deductorcategory]"></asp:SqlDataSource>
                                                </div>

                                            </div>
                                            <div class="col-md-3">
                                                <label>MSME/Udyam RC No.</label>
                                                <div class="relative">
                                                    <asp:TextBox ID="txtMSMEUdyamRCNo" runat="server" Width="100%" MaxLength="25"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="clear"></div>
                                            <div class="col-md-12 " style="margin-top: 8px;">
                                                <asp:Button ID="btnSave" runat="server" Text="Save" ValidationGroup="rqCompany" CssClass="btn btn-primary" OnClick="btnSave_Click"
                                                    OnClientClick="if(!IsCompanyDataValid()){ return false;}" Width="73px" />
                                                <a href="root_Companies.aspx" class="btn btn-danger">Cancel</a>

                                                <%--  <input type="button" value="UDF" class="btn btn-primary dxbButton" onclick="OpenUdf()" />--%>
                                                <asp:Button ID="btnUdf" runat="server" Text="UDF" CssClass="btn btn-primary dxbButton" OnClientClick="if(OpenUdf()){ return false;}" />
                                            </div>
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Text="Correspondence" Name="CorresPondence">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Exchange Segment" Visible="False" Text="Exchange Segment">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="DP Memberships" Visible="False" Text="DP Memberships">
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
                            <dxe:TabPage Name="Deductor Info(TDS)" Text="Deductor Info(TDS)">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="UDF" Text="UDF" Visible="false">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Logo" Text="Logo">
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
	                                            }"></ClientSideEvents>
                        <ContentStyle>
                            <Border BorderColor="#002D96" BorderStyle="Solid" BorderWidth="1px" />
                        </ContentStyle>
                        <LoadingPanelStyle ImageSpacing="6px">
                        </LoadingPanelStyle>
                        <TabStyle Font-Size="12px">
                        </TabStyle>
                    </dxe:ASPxPageControl>
                </td>
            </tr>
            <tr>
                <td></td>
            </tr>
        </table>
    </div>
</asp:Content>
