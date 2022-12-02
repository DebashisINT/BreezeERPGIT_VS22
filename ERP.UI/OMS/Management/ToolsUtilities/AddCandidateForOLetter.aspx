<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.ToolsUtilities.management_utilities_AddCandidateForOLetter" CodeBehind="AddCandidateForOLetter.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <script type="text/javascript" src="/assests/js/init.js"></script>

    <script type="text/javascript" src="/assests/js/ajax-dynamic-list.js"></script>

    <!--___________________These files are for datetime calander___________________-->
    <link type="text/css" rel="stylesheet" href="../../CSS/dhtmlgoodies_calendar.css?random=20051112"
        media="screen" />

    <script type="text/javascript" src="/assests/js/dhtmlgoodies_calendar.js?random=20060118"></script>

    <script type="text/javascript" src="/assests/js/init.js"></script>

    <script type="text/javascript" src="/assests/js/ajax-dynamic-list.js"></script>

    <!--___________________________________________________________________________-->
    <%--</head>--%>
    <%--<body  style="margin: 0px 0px 0px 0px; background-color: #DDECFE" onload="clearPreloadPage()">--%>

    <script type="text/javascript">
        $(document).ready(function () {
            $("#<%=btnCancel.ClientID %>").click(function () {
                var url = '<%= Page.ResolveUrl("~/OMS/management/ToolsUtilities/OfferLetter_AddCandidate.aspx")%>';
                window.location.href = url;
                return false;
            });
        });

        function CallList(obj1, obj2, obj3) {
            var obj5 = '';
            ajax_showOptions(obj1, obj2, obj3, obj5);
        }

        function ValidatePage() {
            if (document.getElementById("txtName").value == '') {
                alert('Candidate Name is Required.');
                return false;
            }
                //      else if(document.getElementById("txtNoofDependent").value !='')
                //      {
                //          if(document.getElementById("txtNoofDependent").value.length !=10)
                //          {
                //           alert('PAN No should be 10 digits.');
                //           return false;
                //           }
                //      }


            else if (document.getElementById("txtLocality").value == '') {
                alert('Address is Mandatory.');
                return false;
            }
            else if (document.getElementById("drpMaritalStatus").value == "0") {
                alert('Please Select Marital Status.');
                return false;
            }

            else if (document.getElementById("txtPhone").value == '') {
                alert('Mobile No is Mandatory.');
                return false;
            }
            else if (document.getElementById("txtPhone").value.length != 10) {
                alert('Mobile No should be 10 digits.');
                return false;
            }
            else if (txtDOB.GetText() == '01-01-0100' || txtDOB.GetText() == '01-01-1900') {
                alert('Date of birth is Mandatory.');
                return false;
            }
            else if (document.getElementById("drpQualification").value == "0") {
                alert('Please select Qualification.');
                return false;
            }
            else if (document.getElementById("cmbDept").value == "0") {
                alert('Please Select Department.');
                return false;
            }
            else if (txtPJD.GetText() == '01-01-0100' || txtPJD.GetText() == '01-01-1900') {
                alert('Joining date is Mandatory.');
                return false;
            }
            else if (document.getElementById("cmbOrganization").value == "0") {
                alert('Please Select Organization.');
                return false;
            }
            else if (document.getElementById("cmbBranch").value == "0") {
                alert('Please Select Branch.');
                return false;
            }
            else if (document.getElementById("cmbDesg").value == "0") {
                alert('Please Select Designation.');
                return false;
            }
            else if (document.getElementById("txtAprovedCTC").value == '') {
                alert('CTC Can not be blank.');
                return false;
            }
            else if (document.getElementById("EmpType").value == "0") {
                alert('Please Select Employee Type.');
                return false;
            }
            else if (document.getElementById("txtReportTo_hidden").value == '') {
                alert('Please Select Reporting Head.');
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
                alert('Please Add Candidate Details.')
            }
            else {
                mode = 'edit';
                var url = '../frmAddDocumentCandidate.aspx?id=AddCandidateForOLetter.aspx?id=' + id + '&mode=' + mode + '&id1=Candidate&id2=' + id;
                window.location.href = url;
                //popup.SetContentUrl(url);
                //popup.Show();
            }
        }
        function OnDocumentView(keyValue) {
            var url = '<%= Page.ResolveUrl("~/OMS/Reports/viewImage.aspx?id=")%>' + keyValue;
            window.open(url, '_blank');
            //popup.contentUrl = url;
            //popup.Show();
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Candidate Information</h3>
        </div>
    </div>
    <div class="form_main">
        <table class="TableMain100">
            <tr>
                <td style="vertical-align: top; padding-left: 30px;">
                    <div class="crossBtn">
                        <asp:HyperLink ID="goBackCrossBtn" NavigateUrl="#" runat="server">        
                            <i class="fa fa-times"></i>
                        </asp:HyperLink>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="text-align: center; width: 100%;">
                    <table cellpadding="2" cellspacing="0" border="0" class="TableMain100">
                        <tr>
                            <td colspan="6"><b>(<span style="color: red">*</span>) Marks are mandatory Field.</b>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft" valign="top">
                                <span style="color: red">*</span></td>
                            <td class="gridcellleft" valign="top">
                                <span class="Ecoheadtxt">Candidate Name:</span>
                            </td>
                            <td class="gridcellleft" valign="top" style="text-align: left;">
                                <asp:DropDownList ID="CmbSalutation" runat="server" Width="100px" TabIndex="1" Font-Size="11px">
                                </asp:DropDownList><br />
                                <asp:TextBox ID="txtName" runat="server" Width="200px" Font-Size="11px" TabIndex="1"
                                    ValidationGroup="a"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtName"
                                    Display="Dynamic" ErrorMessage="Required." ValidationGroup="a" Width="124px"></asp:RequiredFieldValidator></td>
                            <td style="width: 17px" valign="top">
                                <span style="color: red">*</span>
                            </td>
                            <td class="gridcellleft" valign="top">
                                <span class="Ecoheadtxt">Residence Locality:</span>
                            </td>
                            <td class="gridcellleft">
                                <asp:TextBox ID="txtLocality" runat="server" Width="200px" Font-Size="11px" Height="50px"
                                    TabIndex="2" TextMode="MultiLine"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft"></td>
                            <td class="gridcellleft">
                                <span class="Ecoheadtxt">Source Type:</span>
                            </td>
                            <td class="gridcellleft" style="text-align: left;">
                                <asp:DropDownList ID="drpSourceType" runat="server" Width="203px" Font-Size="11px"
                                    TabIndex="3">
                                </asp:DropDownList></td>

                            <td style="width: 17px">
                                <span style="color: red">*</span>
                            </td>
                            <td class="gridcellleft">
                                <span class="Ecoheadtxt">Father's Name:</span>
                            </td>
                            <td class="gridcellleft">
                                <asp:TextBox ID="txtFatherName" runat="server" Width="200px" Font-Size="11px" TabIndex="4"></asp:TextBox>
                            </td>

                        </tr>
                        <tr>
                            <td class="gridcellleft"></td>
                            <td class="gridcellleft">
                                <span class="Ecoheadtxt">Source Name:</span>
                            </td>
                            <td class="gridcellleft">
                                <asp:TextBox ID="txtReferedBy" runat="server" Width="200px" Font-Size="11px" TabIndex="5"></asp:TextBox>
                                <%--<asp:TextBox ID="txtReferedBy_hidden" runat="server"></asp:TextBox>--%>
                                <asp:HiddenField ID="txtReferedBy_hidden" runat="server" />
                            </td>
                            <td style="width: 17px">
                                <span style="color: red">*</span>
                            </td>
                            <td class="gridcellleft">
                                <span class="Ecoheadtxt">PAN No:</span>
                            </td>
                            <td class="gridcellleft">
                                <asp:TextBox ID="txtNoofDependent" runat="server" Width="200px" Font-Size="11px"
                                    TabIndex="6" MaxLength="10"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <span style="color: red">*</span></td>
                            <td class="gridcellleft">
                                <span class="Ecoheadtxt">Sex:</span>
                            </td>
                            <td class="gridcellleft" style="text-align: left;">
                                <asp:DropDownList ID="drpSex" Width="203px" Font-Size="11px" runat="server" TabIndex="7">
                                    <asp:ListItem Text="Male" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="FeMale" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 17px">
                                <span style="color: red">*</span>
                            </td>
                            <td class="gridcellleft">
                                <span class="Ecoheadtxt">Mobile No:</span>
                            </td>
                            <td class="gridcellleft">
                                <asp:TextBox ID="txtPhone" runat="server" Width="200px" Font-Size="11px" TabIndex="8"
                                    MaxLength="10"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtPhone"
                                    Display="Dynamic" ErrorMessage="Required." ValidationGroup="a" Width="124px"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <span style="color: red">*</span></td>
                            <td class="gridcellleft">
                                <span class="Ecoheadtxt">Date of Birth:</span>
                            </td>
                            <td class="gridcellleft">
                                <dxe:ASPxDateEdit ID="txtDOB" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                    TabIndex="9" Width="202px">
                                    <ButtonStyle Width="13px">
                                    </ButtonStyle>
                                </dxe:ASPxDateEdit>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtDOB"
                                    ErrorMessage="Required." Display="Dynamic"></asp:RequiredFieldValidator></td>
                            <td style="width: 17px">
                                <span style="color: red">*</span>
                            </td>
                            <td class="gridcellleft">
                                <span class="Ecoheadtxt">Email Id:</span>
                            </td>
                            <td class="gridcellleft">
                                <asp:TextBox ID="txtEmailId" runat="server" Width="200px" Font-Size="11px" TabIndex="9"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <td class="gridcellleft">
                                <span style="color: red">*</span>
                            </td>
                            <td class="gridcellleft">
                                <span class="Ecoheadtxt">Marital Status:</span>
                            </td>
                            <td class="gridcellleft" style="text-align: left;">
                                <asp:DropDownList ID="drpMaritalStatus" runat="server" Width="203px" Font-Size="11px"
                                    TabIndex="10">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 17px"></td>
                            <td class="gridcellleft">
                                <span class="Ecoheadtxt"></span>
                            </td>
                            <td class="gridcellleft"></td>

                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <span style="color: red">*</span></td>
                            <td class="gridcellleft">
                                <span class="Ecoheadtxt">Qualification:</span>
                            </td>
                            <td class="gridcellleft" style="text-align: left;">
                                <asp:DropDownList ID="drpQualification" Width="203px" Font-Size="11px" runat="server"
                                    TabIndex="10">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 17px"></td>
                            <td class="gridcellleft">
                                <span class="Ecoheadtxt">Professional Qualification:</span>
                            </td>
                            <td class="gridcellleft" style="text-align: left;">
                                <asp:DropDownList ID="drpProfQualification" Width="203px" Font-Size="11px" runat="server"
                                    TabIndex="11">
                                </asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td class="gridcellleft"></td>
                            <td class="gridcellleft">
                                <span class="Ecoheadtxt">Certifications:</span>
                            </td>
                            <td class="gridcellleft">
                                <asp:TextBox ID="txtCertification" runat="server" Width="200px" Font-Size="11px"
                                    TabIndex="12"></asp:TextBox></td>
                            <td style="width: 17px"></td>
                            <td class="gridcellleft">
                                <span class="Ecoheadtxt">Current Employer:</span>
                            </td>
                            <td class="gridcellleft">
                                <asp:TextBox ID="txtCurrentEmployer" runat="server" Width="200px" Font-Size="11px"
                                    TabIndex="13"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <span style="color: red">*</span></td>
                            <td class="gridcellleft">
                                <span class="Ecoheadtxt">Experience Yrs:</span>
                            </td>
                            <td class="gridcellleft">
                                <asp:TextBox ID="txtExpYrs" runat="server" Width="200px" Font-Size="11px" TabIndex="14"
                                    MaxLength="2"></asp:TextBox></td>
                            <td style="width: 17px"></td>
                            <td class="gridcellleft">
                                <span class="Ecoheadtxt">Current Job Profile :</span>
                            </td>
                            <td class="gridcellleft" style="text-align: left;">
                                <asp:DropDownList ID="drpCurrentJobProfile" Width="203PX" Font-Size="11px" runat="server"
                                    TabIndex="15">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <span style="color: red">*</span></td>
                            <td class="gridcellleft">
                                <span class="Ecoheadtxt">Current CTC:</span>
                            </td>
                            <td class="gridcellleft">
                                <asp:TextBox ID="txtCurrentCTC" runat="server" Width="200px" Font-Size="11px" TabIndex="16"></asp:TextBox></td>
                            <td style="width: 17px"></td>
                            <td class="gridcellleft">
                                <span class="Ecoheadtxt">Desired CTC:</span>
                            </td>
                            <td class="gridcellleft">
                                <asp:TextBox ID="txtDesiredCTC" runat="server" Width="200px" Font-Size="11px" TabIndex="17"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="gridcellleft"></td>
                            <td class="gridcellleft">
                                <span class="Ecoheadtxt">Previous CTC:</span>
                            </td>
                            <td class="gridcellleft">
                                <asp:TextBox ID="txtPreviousCTC" runat="server" Width="200px" Font-Size="11px" TabIndex="18"></asp:TextBox>
                            </td>
                            <td style="width: 17px;"></td>
                            <td class="gridcellleft">
                                <span class="Ecoheadtxt">Previous Employer:</span>
                            </td>
                            <td class="gridcellleft">
                                <asp:TextBox ID="txtPreviousEmployer" runat="server" Width="200px" Font-Size="11px"
                                    TabIndex="19"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <span style="color: red">*</span></td>
                            <td class="gridcellleft">
                                <span class="Ecoheadtxt">Probable Join Date:</span>
                            </td>
                            <td class="gridcellleft">
                                <dxe:ASPxDateEdit ID="txtPJD" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                    TabIndex="20" Width="204px">
                                    <ButtonStyle Width="13px">
                                    </ButtonStyle>
                                </dxe:ASPxDateEdit>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtPJD"
                                    ErrorMessage="Required."></asp:RequiredFieldValidator>
                            </td>
                            <td style="width: 17px;"></td>
                            <td class="gridcellleft">
                                <span class="Ecoheadtxt">Previous Job Profile :</span>
                            </td>
                            <td class="gridcellleft" style="text-align: left;">
                                <asp:DropDownList ID="drpPreviousJobProfile" Width="203PX" Font-Size="11px" runat="server"
                                    TabIndex="21">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft"></td>
                            <td class="gridcellleft">
                                <span class="Ecoheadtxt">Reason For Change :</span>
                            </td>
                            <td class="gridcellleft">
                                <asp:TextBox ID="txtReasonforChange" runat="server" TextMode="MultiLine" Width="198px"
                                    Font-Size="11px" TabIndex="22"></asp:TextBox>
                            </td>
                            <td style="width: 17px">
                                <span style="color: red"></span>
                            </td>
                            <td class="gridcellleft">
                                <span class="Ecoheadtxt"></span>
                            </td>
                            <td class="gridcellleft"></td>

                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <span style="color: red">*</span></td>
                            <td class="gridcellleft">
                                <span class="Ecoheadtxt">Organization :</span>
                            </td>
                            <td class="gridcellleft" style="text-align: left;">
                                <asp:DropDownList ID="cmbOrganization" runat="server" Width="205px" Font-Size="11px"
                                    TabIndex="24">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 17px">
                                <span style="color: red">*</span>
                            </td>
                            <td class="gridcellleft">
                                <span class="Ecoheadtxt">Branch :</span>
                                <%--<span class="Ecoheadtxt" >Actual Start Date/time:</span>--%>
                            </td>
                            <td class="gridcellleft" style="text-align: left;">
                                <asp:DropDownList ID="cmbBranch" runat="server" Width="203px" Font-Size="11px" TabIndex="25">
                                </asp:DropDownList>
                                <%--<asp:TextBox ID="TextBox21" runat="server" Width="200px" Font-Size="11px"></asp:TextBox>--%>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <span style="color: red">*</span></td>
                            <td class="gridcellleft">
                                <span class="Ecoheadtxt">Designation :</span>
                            </td>
                            <td class="gridcellleft" style="text-align: left;">
                                <asp:DropDownList ID="cmbDesg" runat="server" Width="205px" Font-Size="11px" TabIndex="26">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 17px">
                                <span style="color: red">*</span>
                            </td>
                            <td class="gridcellleft">
                                <span class="Ecoheadtxt">Aproved CTC :</span>
                                <%--<span class="Ecoheadtxt" >Actual Start Date/time:</span>--%>
                            </td>
                            <td class="gridcellleft">
                                <asp:TextBox ID="txtAprovedCTC" runat="server" Width="200px" Font-Size="11px" TabIndex="27"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtAprovedCTC"
                                    Display="Dynamic" ErrorMessage="Required." ValidationGroup="a" Width="124px"></asp:RequiredFieldValidator>
                                <%--<asp:TextBox ID="TextBox21" runat="server" Width="200px" Font-Size="11px"></asp:TextBox>--%>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <span style="color: red">*</span>
                            </td>
                            <td class="gridcellleft">
                                <span class="Ecoheadtxt">Employee Type :</span>
                            </td>
                            <td class="gridcellleft" style="text-align: left;">
                                <asp:DropDownList ID="EmpType" runat="server" Width="205px" Font-Size="11px" TabIndex="28">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 17px">
                                <span style="color: red">*</span>
                            </td>
                            <td class="gridcellleft">
                                <span class="Ecoheadtxt">Report To :</span>
                            </td>
                            <td class="gridcellleft">
                                <asp:TextBox ID="txtReportTo" runat="server" Width="200px" TabIndex="29"></asp:TextBox>
                                <asp:HiddenField ID="txtReportTo_hidden" runat="server" />
                            </td>
                        </tr>

                        <tr>
                            <td class="gridcellleft">
                                <span style="color: red">*</span>
                            </td>
                            <td class="gridcellleft">
                                <span class="Ecoheadtxt">Department :</span>
                            </td>
                            <td class="gridcellleft" style="text-align: left;">
                                <asp:DropDownList ID="cmbDept" runat="server" Width="205px" TabIndex="30">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 17px"></td>
                            <td class="gridcellleft"></td>
                            <td class="gridcellleft"></td>
                        </tr>

                        <tr>
                            <td class="gridcellleft"></td>
                            <td colspan="2" class="gridcellleft">
                                <asp:Label ID="lblmessage" runat="server" ForeColor="Red"></asp:Label>&nbsp;</td>
                            <td style="width: 17px"></td>
                            <td colspan="2" class="gridcellright">
                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click"
                                    TabIndex="31" ValidationGroup="a" />
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-danger" OnClientClick="Close()"
                                    TabIndex="32" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <table class="TableMain100">
            <tr>
                <td align="center">
                    <table style="width: 90%;">
                        <tr>
                            <td id="ShowFilter">
                                <%--<% if (rights.CanAdd)
                                               { %>--%>
                                <a href="javascript:void(0);" onclick="Show()" class="btn btn-primary"><span>Add New</span> </a>
                                <%--<%} %>--%>
                                <%--<a href="javascript:ShowHideFilter('s');" class="btn btn-success"><span>Show Filter</span></a>--%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <dxe:ASPxGridView ID="EmployeeDocumentGrid" runat="server" AutoGenerateColumns="False"
                                    ClientInstanceName="gridDocument" KeyFieldName="Id" Font-Size="11px" OnRowCommand="EmployeeDocumentGrid_RowCommand"
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
                                        <dxe:GridViewDataHyperLinkColumn Caption="Actions" FieldName="Src" VisibleIndex="5"
                                            Width="6%">
                                            <DataItemTemplate>
                                                <%--<a href='viewImage.aspx?id=<%#Eval("Src") %>' class='HTMLPopulation<%#Eval("Slno") %>'>View</a>--%>
                                                <%--<a href='viewImage.aspx?id=<%#Eval("Src") %>' target="_blank">View</a>--%>
                                                <a onclick="OnDocumentView('<%#Eval("Src") %>')" title="More Info" class="pad" style="text-decoration: none; cursor: pointer;">
                                                    <img src="/OMS/images/show.png" /></a>
                                                <asp:LinkButton ID="btn_delete" runat="server" OnClientClick="return confirm('Are you sure you want to delete?');" CommandArgument='<%# Container.KeyValue %>' CommandName="delete"> 
                                                    <img src="/assests/images/Delete.png" />
                                                </asp:LinkButton>
                                            </DataItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <CellStyle HorizontalAlign="Center"></CellStyle>
                                        </dxe:GridViewDataHyperLinkColumn>
                                        <%--<dxe:GridViewCommandColumn VisibleIndex="6" ShowDeleteButton="true">
                                            <HeaderTemplate>
                                            </HeaderTemplate>
                                        </dxe:GridViewCommandColumn>--%>
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
                    </table>

                </td>
            </tr>
            <tr>
                <td style="display: none">
                    <asp:TextBox ID="LabelID" runat="server" Text=""></asp:TextBox>
                    <asp:TextBox ID="LabelMode" runat="server" Text=""></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
