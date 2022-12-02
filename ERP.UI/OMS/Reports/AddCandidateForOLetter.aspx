<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.management_AddCandidateForOLetter" CodeBehind="AddCandidateForOLetter.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%-- <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />--%>

    <script type="text/javascript" src="/assests/js/loaddata1.js"></script>
    <script language="javascript" type="text/javascript">
        //function SignOff()
        //   {
        //   window.parent.SignOff();
        //   }

    </script>
    <script type="text/javascript" src="/assests/js/init.js"></script>
    <script type="text/javascript" src="/assests/js/ajax-dynamic-list_rootfile.js"></script>
    <%--<style type="text/css">
	
	/* Big box with list of options */
	#ajax_listOfOptions{
		position:absolute;	/* Never change this one */
		width:50px;	/* Width of box */
		height:auto;	/* Height of box */
		overflow:auto;	/* Scrolling features */
		border:1px solid Blue;	/* Blue border */
		background-color:#FFF;	/* White background color */
		text-align:left;
		font-size:0.9em;
		z-index:32767;
	}
	#ajax_listOfOptions div{	/* General rule for both .optionDiv and .optionDivSelected */
		margin:1px;		
		padding:1px;
		cursor:pointer;
		font-size:0.9em;
	}
	#ajax_listOfOptions .optionDiv{	/* Div for each item in list */
		
	}
	#ajax_listOfOptions .optionDivSelected{ /* Selected item in the list */
		background-color:#DDECFE;
		color:Blue;
	}
	#ajax_listOfOptions_iframe{
		background-color:#F00;
		position:absolute;
		z-index:3000;
	}
	
	form{
		display:inline;
	}
	
	</style>--%>
    <!--___________________These files are for datetime calander___________________-->
    <%--<link type="text/css" rel="stylesheet" href="../CSS/dhtmlgoodies_calendar.css?random=20051112"
        media="screen" />--%>
    <%--<script type="text/javascript" src="/assests/js/dhtmlgoodies_calendar.js?random=20060118"></script>
    <script type="text/javascript" src="/assests/js/init.js"></script>
    <script type="text/javascript" src="/assests/js/ajax-dynamic-list.js"></script>--%>

    <!--___________________________________________________________________________-->

    <script language="javascript" type="text/javascript">

        function CallList(obj1, obj2, obj3) {
            var obj5 = '';
            ajax_showOptions(obj1, obj2, obj3, obj5);
        }

        function ValidatePage() {

            //      else if(document.getElementById("txtNoofDependent").value !='')
            //      {
            //          if(document.getElementById("txtNoofDependent").value.length !=10)
            //          {
            //           alert('PAN No should be 10 digits.');
            //           return false;
            //           }
            //      }

            visibleValidation();
            if (document.getElementById("txtName").value == '') {
                //alert('Candidate Name is Required.');
                $("#MandatoryName").show();
                (document.getElementById("txtName")).focus();
                return false;
            }
            else if (document.getElementById("txtLocality").value == '') {
                //alert('Address is Mandatory.');
                $("#MandatoryAddress").show();
                (document.getElementById("txtLocality")).focus();
                return false;
            }
            else if (document.getElementById("txtPhone").value == '') {
                //alert('Mobile No is Mandatory.');
                $("#MandatoryMobile").show();
                (document.getElementById("txtPhone")).focus();
                return false;
            }
            else if (document.getElementById("txtPhone").value.length != 10) {
                //alert('Mobile No should be 10 digits.');
                $("#MandatoryMobile").show();
                (document.getElementById("txtPhone")).focus();
                return false;
            }
            else if (txtDOB.GetText() == '01-01-0100' || txtDOB.GetText() == '01-01-1900') {
                //alert('Date of birth is Mandatory.');
                $("#MandatoryBirth").show();
                return false;
            }
            else if (document.getElementById("drpMaritalStatus").value == "0") {
                //alert('Please Select Marital Status.');
                $("#MandatoryMarital").show();
                (document.getElementById("drpMaritalStatus")).focus();
                return false;
            }
            else if (document.getElementById("drpQualification").value == "0") {
                //alert('Please select Qualification.');
                $("#MandatoryQualification").show();
                (document.getElementById("drpQualification")).focus();
                return false;
            }
            else if (txtPJD.GetText() == '01-01-0100' || txtPJD.GetText() == '01-01-1900') {
                //alert('Joining date is Mandatory.');
                $("#MandatoryJoin").show();
                return false;
            }
            else if (document.getElementById("cmbOrganization").value == "0") {
                //alert('Please Select Organization.');
                $("#MandatoryOrganization").show();
                (document.getElementById("cmbOrganization")).focus();
                return false;
            }
            else if (document.getElementById("cmbBranch").value == "0") {
                //alert('Please Select Branch.');
                $("#MandatoryBranch").show();
                (document.getElementById("cmbBranch")).focus();
                return false;
            }
            else if (document.getElementById("cmbDesg").value == "0") {
                //alert('Please Select Designation.');
                $("#MandatoryDesignation").show();
                (document.getElementById("cmbDesg")).focus();
                return false;
            }
            else if (document.getElementById("txtAprovedCTC").value == '') {
                //alert('CTC Can not be blank.');
                $("#MandatoryCTC").show();
                (document.getElementById("txtAprovedCTC")).focus();
                return false;
            }
            else if (document.getElementById("EmpType").value == "0") {
                //alert('Please Select Employee Type.');
                $("#MandatoryType").show();
                (document.getElementById("EmpType")).focus();
                return false;
            }
            else if (document.getElementById("txtReportTo_hidden").value == '') {
                //alert('Please Select Reporting Head.');
                $("#MandatoryReportTo").show();
                (document.getElementById("txtReportTo_hidden")).focus();
                return false;
            }
            else if (document.getElementById("cmbDept").value == "0") {
                //alert('Please Select Department.');
                $("#MandatoryDepartment").show();
                (document.getElementById("cmbDept")).focus();
                return false;
            }
        }

        function Close() {
            parent.editwin.close();
        }

        //-------------------For Documents-----------------
        function Show() {
            var id = document.getElementById('LabelID').value;
            var mode = document.getElementById('LabelMode').value;

            if (id == 'ADD') {
                alert('Please Add Canidate Details.')
            }
            else {
                mode = 'edit';
                var url = 'frmAddDocumentCandidate.aspx?id=AddCandidateForOLetter.aspx?id=' + id + '&mode=' + mode + '&id1=Candidate&id2=' + id;
                popup.SetContentUrl(url);
                popup.Show();
            }

        }
        function OnDocumentView(keyValue) {
            var url = 'viewImage.aspx?id=' + keyValue;
            popup.contentUrl = url;
            popup.Show();

        }



        //-------------------------------------------------


        function ondropdown() {
            //alert('123');
            var txtbox = document.getElementById("txtReferedBy");
            txtbox.value = "";
        }
        function call_ajax(obj1, obj2, obj3) {
            var set_value
            var ob = document.getElementById("drpSourceType")
            set_value = ob.value;
            ajax_showOptions(obj1, obj2, obj3, set_value)
        }

        function MaskMoney(evt) {
            if (!(evt.keyCode == 46 || (evt.keyCode >= 48 && evt.keyCode <= 57))) return false;
            var parts = evt.srcElement.value.split('.');
            if (parts.length > 2) return false;
            if (evt.keyCode == 46) return (parts.length == 1);
            if (parts[0].length >= 14) return false;
            if (parts.length == 2 && parts[1].length >= 2) return false;
        }

        FieldName = 'btnCancel';
    </script>
    
     <!-- Choosen list by Sudip on 20-12-2016 -->
    <script src="/assests/pluggins/choosen/choosen.min.js"></script>
    <script type="text/javascript">
        function visibleValidation() {
            $("#MandatoryName").hide();
            $("#MandatoryAddress").hide();
            $("#MandatoryMarital").hide();
            $("#MandatoryMobile").hide();
            $("#MandatoryBirth").hide();
            $("#MandatoryQualification").hide();
            $("#MandatoryJoin").hide();
            $("#MandatoryOrganization").hide();
            $("#MandatoryBranch").hide();
            $("#MandatoryCTC").hide();
            $("#MandatoryType").hide();
            $("#MandatoryReportTo").hide();
            $("#MandatoryDepartment").hide();
            $("#MandatoryDesignation").hide();
        }
        $(document).ready(function () {
            ListBind();
        });
        function ListBind() {

            var config = {
                '.chsn': {},
                '.chsn-deselect': { allow_single_deselect: true },
                '.chsn-no-single': { disable_search_threshold: 10 },
                '.chsn-no-results': { no_results_text: 'Oops, nothing found!' },
                '.chsn-width': { width: "180px" }
            }
            for (var selector in config) {
                $(selector).chosen(config[selector]);
            }

        }
        function lstReportTo() {
            $('#lstReportTo').fadeIn();
        }
        function FuncTrigger()
        {
            $('#lstReportTo').trigger("chosen:updated");

            var lstReportTo = document.getElementById("lstReportTo");
            if (document.getElementById("txtReportTo_hidden").value != '') {
                for (var i = 0; i < lstReportTo.options.length; i++) {
                    if (lstReportTo.options[i].value == document.getElementById("txtReportTo_hidden").value) {
                        lstReportTo.options[i].selected = true;
                    }
                }
                $('#lstTaxRates_SubAccount').trigger("chosen:updated");
            }

           // $('#lstReportTo').trigger("chosen:updated");
        }
        function changeFunc() {
            var MainAccount_val2 = document.getElementById("lstReportTo").value;
            document.getElementById("txtReportTo_hidden").value = document.getElementById("lstReportTo").value;
        }
    </script>
    <style type="text/css">
        #lstReportTo {
            width: 100%;
        }

        #lstReportTo {
            display: none !important;
        }

        #lstReportTo, #lstReportTo_chosen {
            width: 100% !important;
        }
        #prepage {
            display:none;
        }
        .pullleftClass {
            right: -3px;
            top: 27px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Candidate Information</h3>
        </div>
    </div>
    <div class="form_main">
        <%--   <asp:Panel ID="panel" runat="server" Width="100%">--%>
        <table class="TableMain100">
            <tr>
                <td style="vertical-align: top; padding-left: 30px;">
                    <div class="crossBtn"><a href="GenerateOfferLater.aspx"><i class="fa fa-times"></i></a></div>
                </td>
            </tr>
            <tr>
                <td style="width: 100%;">
                    <div class="row">
                        <div class="col-md-3 relative">
                            <label>Candidate Name:<span style="color: red">*</span></label>
                             <div>
                                <asp:DropDownList ID="CmbSalutation" runat="server" Width="15%" TabIndex="1" Font-Size="11px" CssClass="pull-left">
                                </asp:DropDownList>
                                <asp:TextBox ID="txtName" runat="server" Width="80%" Font-Size="11px" TabIndex="2" MaxLength="50"
                                    ValidationGroup="a" CssClass="pull-right"></asp:TextBox>
                                <span id="MandatoryName" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtName"
                                    SetFocusOnError="true" class="tp2 fa fa-exclamation-circle iconRed" ToolTip="Mandatory" ValidationGroup="a"></asp:RequiredFieldValidator>
                             </div>
                        </div>
                        <div class="col-md-3 relative">
                            <label>Residence Locality <span style="color: red">*</span>  </label>
                            <div>
                                <asp:TextBox ID="txtLocality" runat="server" Width="100%" Font-Size="11px" Height="50px" MaxLength="100"
                                    TabIndex="3" TextMode="MultiLine"></asp:TextBox>
                                <span id="MandatoryAddress" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                            </div>
                        </div>
                        <div class="col-md-3 relative">
                            <label>
                                Source Type:
                            </label>
                            <div>
                                <asp:DropDownList ID="drpSourceType" runat="server" Width="100%" Font-Size="11px"
                                    TabIndex="4">
                                </asp:DropDownList></div>
                        </div>
                        <div class="col-md-3 relative">
                            <label>
                                Father's Name:
                            </label>
                            <div>
                                <asp:TextBox ID="txtFatherName" runat="server" Width="100%" Font-Size="11px" TabIndex="5" MaxLength="50"></asp:TextBox>
                            </div>
                        </div>
                        <div class="clear"></div>
                        <div class="col-md-3 relative">
                            <label>
                                Source Name:
                            </label>
                            <div>
                                <asp:TextBox ID="txtReferedBy" runat="server" Width="100%" Font-Size="11px" TabIndex="6"></asp:TextBox>
                                <%--<asp:TextBox ID="txtReferedBy_hidden" runat="server"></asp:TextBox>--%>
                                <asp:HiddenField ID="txtReferedBy_hidden" runat="server" />
                            </div>
                        </div>
                        <div class="col-md-3 relative">
                            <label>
                                PAN No:
                            </label>
                            <div>
                                <asp:TextBox ID="txtNoofDependent" runat="server" Width="100%" Font-Size="11px"
                                    TabIndex="7" MaxLength="10"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-3 relative">
                            <label>
                                Sex:
                            </label>
                            <div>
                                <asp:DropDownList ID="drpSex" Width="100%" Font-Size="11px" runat="server" TabIndex="8">
                                    <asp:ListItem Text="Male" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="FeMale" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-3 relative">
                            <label>Mobile No <span style="color: red">*</span></label>
                            <div>
                                <asp:TextBox ID="txtPhone" runat="server" Width="100%" Font-Size="11px" TabIndex="9"
                                    MaxLength="10"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtPhone"
                                    SetFocusOnError="true" class="tp2 fa fa-exclamation-circle iconRed" ToolTip="Mandatory" ValidationGroup="a"></asp:RequiredFieldValidator>
                                <span id="MandatoryMobile" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                            </div>
                        </div>
                        <div class="col-md-3 relative">
                            <label>Date of Birth <span style="color: red">*</span></label>
                            <div>
                                <dxe:ASPxDateEdit ID="txtDOB" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                    TabIndex="10" Width="100%">
                                    <ButtonStyle Width="13px">
                                    </ButtonStyle>
                                </dxe:ASPxDateEdit>
                                <span id="MandatoryBirth" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtDOB"
                                    SetFocusOnError="true" class="tp2 fa fa-exclamation-circle iconRed" ToolTip="Mandatory"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-md-3 relative">
                            <label>Email Id:</label>
                            <asp:TextBox ID="txtEmailId" runat="server" Width="100%" Font-Size="11px" TabIndex="11" MaxLength="100"></asp:TextBox>
                        </div>
                        <div class="col-md-3 relative">
                            <label>Marital Status <span style="color: red">*</span></label>
                            <div>
                                <asp:DropDownList ID="drpMaritalStatus" runat="server" Width="100%" Font-Size="11px"
                                    TabIndex="12">
                                </asp:DropDownList>
                                <span id="MandatoryMarital" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                            </div>
                        </div>
                        <div class="col-md-3 relative">
                            <label>Qualification<span style="color: red">*</span></label>
                             <div>
                                <asp:DropDownList ID="drpQualification" Width="100%" Font-Size="11px" runat="server"
                                    TabIndex="13">
                                </asp:DropDownList>
                                <span id="MandatoryQualification" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                            </div>
                        </div>
                        <div class="clear"></div>
                        <div class="col-md-3 relative">
                            <label>Professional Qualification:</label>
                            <div>
                                <asp:DropDownList ID="drpProfQualification" Width="100%" Font-Size="11px" runat="server"
                                    TabIndex="14">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-3 relative">
                            <label>Certifications:</label>
                            <div>
                                <asp:TextBox ID="txtCertification" runat="server" Width="100%" Font-Size="11px"
                                    TabIndex="15"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-3 relative">
                            <label>Current Employer:</label>
                            <div>
                                <asp:TextBox ID="txtCurrentEmployer" runat="server" Width="100%" Font-Size="11px"
                                    TabIndex="16"></asp:TextBox></div>
                        </div>
                        <div class="col-md-3 relative">
                            <label>Experience Yrs:</label>
                            <div>
                                <asp:TextBox ID="txtExpYrs" runat="server" Width="100%" Font-Size="11px" TabIndex="17"
                                    MaxLength="2"></asp:TextBox></div>
                        </div>
                        <div class="col-md-3 relative">
                             <label>Current Job Profile :</label>
                            <div>
                                <asp:DropDownList ID="drpCurrentJobProfile" Width="100%" Font-Size="11px" runat="server"
                                    TabIndex="18">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-3 relative">
                            <label>Current CTC:</label>
                            <div>
                                <asp:TextBox ID="txtCurrentCTC" runat="server" Width="100%" Font-Size="11px" TabIndex="19"></asp:TextBox></div>
                        </div>
                        <div class="col-md-3 relative">
                            <label>Desired CTC:</label>
                            <div>
                                <asp:TextBox ID="txtDesiredCTC" runat="server" Width="100%" Font-Size="11px" TabIndex="20"></asp:TextBox></div>
                        </div>
                        <div class="col-md-3 relative">
                            <label>Previous CTC:</label>
                             <div>
                                <asp:TextBox ID="txtPreviousCTC" runat="server" Width="100%" Font-Size="11px" TabIndex="21"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-3 relative">
                            <label>Previous Employer:</label>
                            <div>
                                <asp:TextBox ID="txtPreviousEmployer" runat="server" Width="100%" Font-Size="11px"  MaxLength="100"
                                    TabIndex="22"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-3 relative">
                            <label>Probable Join Date <span style="color: red">*</span></label>
                            <div>
                                <dxe:ASPxDateEdit ID="txtPJD" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                    TabIndex="23" Width="100%">
                                    <ButtonStyle Width="13px">
                                    </ButtonStyle>
                                </dxe:ASPxDateEdit>
                                <span id="MandatoryJoin" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtPJD"
                                    SetFocusOnError="true" class="tp2 fa fa-exclamation-circle iconRed" ToolTip="Mandatory"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-md-3 relative">
                            <label>Previous Job Profile :</label>
                            <div>
                                <asp:DropDownList ID="drpPreviousJobProfile" Width="100%" Font-Size="11px" runat="server"
                                    TabIndex="24">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-3 relative">
                            <label>Reason For Change :</label>
                            <div>
                                <asp:TextBox ID="txtReasonforChange" runat="server" TextMode="MultiLine" Width="100%"
                                    Font-Size="11px" TabIndex="25"></asp:TextBox>
                            </div>
                        </div>
                        <div class="clear"></div>
                        <div class="col-md-3 relative">
                            <label>Organization <span style="color: red">*</span></label>
                            <div>
                                <asp:DropDownList ID="cmbOrganization" runat="server" Width="100%" Font-Size="11px"
                                    TabIndex="26">
                                </asp:DropDownList>
                                <span id="MandatoryOrganization" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                            </div>
                        </div>
                        <div class="col-md-3 relative">
                            <label>Branch <span style="color: red">*</span></label>
                            <div>
                                <asp:DropDownList ID="cmbBranch" runat="server" Width="100%" Font-Size="11px" TabIndex="27">
                                </asp:DropDownList>
                                <span id="MandatoryBranch" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                <%--<asp:TextBox ID="TextBox21" runat="server" Width="200px" Font-Size="11px"></asp:TextBox>--%>
                            </div>
                        </div>
                        <div class="col-md-3 relative">
                            <label>Designation <span style="color: red">*</span></label>
                            <div>
                                <asp:DropDownList ID="cmbDesg" runat="server" Width="100%" Font-Size="11px" TabIndex="28">
                                </asp:DropDownList>
                                <span id="MandatoryDesignation" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                            </div>
                        </div>
                        <div class="col-md-3 relative">
                            <label>Aproved CTC <span style="color: red">*</span></label>
                            <div>
                                <asp:TextBox ID="txtAprovedCTC" runat="server" Width="100%" Font-Size="11px" TabIndex="29"></asp:TextBox>
                                <span id="MandatoryCTC" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>


                                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtAprovedCTC"
                                    SetFocusOnError="true" class="tp2 fa fa-exclamation-circle iconRed" ToolTip="Mandatory" ValidationGroup="a"></asp:RequiredFieldValidator>
                                <%--<asp:TextBox ID="TextBox21" runat="server" Width="200px" Font-Size="11px"></asp:TextBox>--%>
                            </div>
                        </div>
                        <div class="col-md-3 relative">
                            <label>Employee Type <span style="color: red">*</span></label>
                            <div>
                                <asp:DropDownList ID="EmpType" runat="server" Width="100%" Font-Size="11px" TabIndex="30">
                                </asp:DropDownList>
                                <span id="MandatoryType" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                            </div>
                        </div>
                        <div class="col-md-3 relative">
                            <label>Report To <span style="color: red">*</span></label>
                            <div>
                                <%--<asp:TextBox ID="txtReportTo" runat="server" Width="200px" TabIndex="31"></asp:TextBox>--%>
                                <asp:ListBox ID="lstReportTo" CssClass="chsn" runat="server" Font-Size="12px" Width="100%" data-placeholder="Select..." onchange="changeFunc();" TabIndex="31"></asp:ListBox>
                                <asp:HiddenField ID="txtReportTo_hidden" runat="server" />
                                <span id="MandatoryReportTo" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                            </div>
                        </div>
                        <div class="col-md-3 relative">
                            <label>Department <span style="color: red">*</span></label>
                            <div>
                                <asp:DropDownList ID="cmbDept" runat="server" Width="100%" TabIndex="32">
                                </asp:DropDownList>
                                <span id="MandatoryDepartment" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                            </div>
                        </div>
                        <div class="clear"></div>
                        <div class="col-md-12">
                                <asp:Label ID="lblmessage" runat="server" ForeColor="Red"></asp:Label>
                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click"
                                    TabIndex="33" ValidationGroup="a" />
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-danger" OnClientClick="Close()"
                                    TabIndex="34" />
                            </div>
                    </div>
                    <table cellpadding="2" cellspacing="0" border="0" class="TableMain100">
                        
                        <tr>
                            <td class="gridcellleft">
                                
                            </td>
                            <td class="gridcellleft">
                                
                            </td>
                            
                            <td style="width: 17px">
                                
                            </td>
                            <td class="gridcellleft">
                                
                            </td>
                            
                        </tr>

                        <tr>
                            <td class="gridcellleft">
                                
                            </td>
                            <td class="gridcellleft">
                                
                            </td>
                            
                            <td style="width: 17px"></td>
                            <td class="gridcellleft"></td>
                            <td class="gridcellleft"></td>
                        </tr>

                        <tr>
                            <td class="gridcellleft"></td>
                            <td colspan="2" class="gridcellleft">
                                &nbsp;</td>
                            <td style="width: 17px"></td>
                            
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <table class="TableMain100">
            <tr>
                <td align="center">
                    <dxe:ASPxGridView ID="EmployeeDocumentGrid" runat="server" AutoGenerateColumns="False"
                        ClientInstanceName="gridDocument" KeyFieldName="Id" Width="850px" Font-Size="11px"
                        OnRowDeleting="EmployeeDocumentGrid_RowDeleting">
                        <Columns>
                            <dxe:GridViewDataTextColumn FieldName="Id" ReadOnly="True" VisibleIndex="0" Visible="False">
                                <EditFormSettings Visible="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Type" VisibleIndex="1" Caption="DocumentType"
                                Width="25%">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="FileName" VisibleIndex="2" Caption="DocumentName"
                                Width="25%">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Src" VisibleIndex="3" Visible="False">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="FilePath" ReadOnly="True" VisibleIndex="4"
                                Caption="Document Physical Location" Width="25%">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataHyperLinkColumn Caption="View" FieldName="Src" VisibleIndex="5"
                                Width="15%">
                                <DataItemTemplate>
                                    <%--<a href='viewImage.aspx?id=<%#Eval("Src") %>' class='HTMLPopulation<%#Eval("Slno") %>'>View</a>--%>
                                    <%--<a href='viewImage.aspx?id=<%#Eval("Src") %>' target="_blank">View</a>--%>
                                    <a onclick="OnDocumentView('<%#Eval("Src") %>')" style="cursor: pointer;">View</a>
                                </DataItemTemplate>
                            </dxe:GridViewDataHyperLinkColumn>
                            <dxe:GridViewCommandColumn VisibleIndex="6" ShowDeleteButton="true">
                                <%--<DeleteButton Visible="True">
                                                        </DeleteButton>--%>
                                <HeaderTemplate>
                                    <%if (Session["PageAccess"].ToString().Trim() == "All" || Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "DelAdd")
                                      { %>
                                    <a href="javascript:void(0);" onclick="Show();"><span style="color: #000099; text-decoration: underline">Add New</span> </a>
                                    <%} %>
                                </HeaderTemplate>
                            </dxe:GridViewCommandColumn>
                        </Columns>
                        <SettingsCommandButton>
                            <DeleteButton Text="Delete"></DeleteButton>
                        </SettingsCommandButton>
                        <Settings ShowStatusBar="Visible" />
                        <SettingsEditing Mode="PopupEditForm" PopupEditFormHeight="250px" PopupEditFormHorizontalAlign="Center"
                            PopupEditFormModal="True" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="500px"
                            EditFormColumnCount="1" />
                        <Styles>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                            </Header>
                            <FocusedRow CssClass="gridselectrow">
                            </FocusedRow>
                        </Styles>
                        <SettingsText PopupEditFormCaption="Add/Modify Family Relationship" ConfirmDelete="Confirm delete?" />
                        <SettingsPager NumericButtonCount="20" PageSize="20">
                        </SettingsPager>
                        <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True" />
                    </dxe:ASPxGridView>
                    <dxe:ASPxPopupControl ID="ASPXPopupControl" runat="server" ContentUrl="frmAddDocuments.aspx"
                        CloseAction="CloseButton" Top="100" Left="400" ClientInstanceName="popup" Height="500px"
                        Width="900px" HeaderText="Add Document">
                        <ContentCollection>
                            <dxe:PopupControlContentControl runat="server">
                            </dxe:PopupControlContentControl>
                        </ContentCollection>
                    </dxe:ASPxPopupControl>
                </td>
            </tr>
            <tr>
                <td style="display: none">
                    <asp:TextBox ID="LabelID" runat="server" Text=""></asp:TextBox>
                    <asp:TextBox ID="LabelMode" runat="server" Text=""></asp:TextBox>
                </td>
            </tr>
        </table>
        <%--</asp:Panel>--%>
    </div>
</asp:Content>
