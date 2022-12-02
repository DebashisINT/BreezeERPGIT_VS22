<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.management_frm_AddCandidate" CodeBehind="frm_AddCandidate.aspx.cs" %>

<%--<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe000001" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />

    <script type="text/javascript" src="/assests/js/loaddata1.js"></script>

    <script language="javascript" type="text/javascript">
        function SignOff() {
            window.parent.SignOff();
        }

    </script>

    <script type="text/javascript" src="/assests/js/init.js"></script>

    <script type="text/javascript" src="/assests/js/ajax-dynamic-list_wofolder.js"></script>

    <style type="text/css">
        /* Big box with list of options */
        #ajax_listOfOptions {
            position: absolute; /* Never change this one */
            width: 50px; /* Width of box */
            height: auto; /* Height of box */
            overflow: auto; /* Scrolling features */
            border: 1px solid Blue; /* Blue border */
            background-color: #FFF; /* White background color */
            text-align: left;
            font-size: 0.9em;
            z-index: 32767;
        }

            #ajax_listOfOptions div { /* General rule for both .optionDiv and .optionDivSelected */
                margin: 1px;
                padding: 1px;
                cursor: pointer;
                font-size: 0.9em;
            }

            #ajax_listOfOptions .optionDiv { /* Div for each item in list */
            }

            #ajax_listOfOptions .optionDivSelected { /* Selected item in the list */
                background-color: #DDECFE;
                color: Blue;
            }

        #ajax_listOfOptions_iframe {
            background-color: #F00;
            position: absolute;
            z-index: 3000;
        }
    </style>

    <link type="text/css" rel="stylesheet" href="../CSS/dhtmlgoodies_calendar.css?random=20051112" media="screen" />

    <script type="text/javascript" src="/assests/js/dhtmlgoodies_calendar.js?random=20060118"></script>

    <script type="text/javascript" src="/assests/js/init.js"></script>

    <script type="text/javascript" src="/assests/js/ajax-dynamic-list_wofolder.js"></script>

    <script language="javascript" type="text/javascript">
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
        FieldName = 'btnCancel';
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <asp:Panel ID="panel" runat="server" Width="100%">
            <table class="TableMain100">
                <tr class="EHEADER">
                    <td class="ColHead">
                        <span style="color: #3300cc"><strong>Candidate Information</strong></span>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center; width: 100%;">
                        <table style="border: solid 2px white;">
                            <tr>
                                <td class="gridcellleft">
                                    <span class="Ecoheadtxt">Candidate Name:</span>
                                </td>
                                <td class="gridcellleft">
                                    <asp:TextBox ID="txtName" runat="server" Width="200px" Font-Size="12px" TabIndex="1"
                                        ValidationGroup="a"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtName"
                                        Display="Dynamic" ErrorMessage="Candidate name required" ValidationGroup="a"
                                        Width="124px"></asp:RequiredFieldValidator></td>
                                <td style="width: 17px"></td>
                                <td class="gridcellleft">
                                    <span class="Ecoheadtxt">Residence Locality:</span>
                                </td>
                                <td class="gridcellleft">
                                    <asp:TextBox ID="txtLocality" runat="server" Width="200px" Font-Size="12px" TabIndex="2"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="gridcellleft">
                                    <span class="Ecoheadtxt">Source Type:</span>
                                </td>
                                <td class="gridcellleft">
                                    <asp:DropDownList ID="drpSourceType" runat="server" Width="203px" Font-Size="12px"
                                        TabIndex="3">
                                    </asp:DropDownList></td>
                                <td style="width: 17px"></td>
                                <td class="gridcellleft">
                                    <span class="Ecoheadtxt">Marital Status:</span>
                                </td>
                                <td class="gridcellleft">
                                    <asp:DropDownList ID="drpMaritalStatus" runat="server" Width="203px" Font-Size="12px"
                                        TabIndex="4">
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td colspan="5" style="display: none;">
                                    <asp:TextBox ID="txtReferedBy_hidden" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="gridcellleft">
                                    <span class="Ecoheadtxt">Source Name:</span>
                                </td>
                                <td class="gridcellleft">
                                    <asp:TextBox ID="txtReferedBy" runat="server" Width="200px" Font-Size="12px" TabIndex="5"></asp:TextBox>
                                </td>
                                <td style="width: 17px"></td>
                                <td class="gridcellleft">
                                    <span class="Ecoheadtxt">No. of Dependent:</span>
                                </td>
                                <td class="gridcellleft">
                                    <asp:TextBox ID="txtNoofDependent" runat="server" Width="200px" Font-Size="12px"
                                        TabIndex="6"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="gridcellleft">
                                    <span class="Ecoheadtxt">Sex:</span>
                                </td>
                                <td class="gridcellleft">
                                    <asp:DropDownList ID="drpSex" Width="203px" Font-Size="12px" runat="server" TabIndex="7">
                                        <asp:ListItem Text="Male" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="FeMale" Value="1"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 17px"></td>
                                <td class="gridcellleft">
                                    <span class="Ecoheadtxt">Phone:</span>
                                </td>
                                <td class="gridcellleft">
                                    <asp:TextBox ID="txtPhone" runat="server" Width="200px" Font-Size="12px" TabIndex="8"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="gridcellleft">
                                    <span class="Ecoheadtxt">Date of Birth:</span>
                                </td>
                                <td class="gridcellleft">
                                    <dxe:ASPxDateEdit ID="txtDOB" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                        TabIndex="9" Width="202px">
                                        <ButtonStyle Width="13px">
                                        </ButtonStyle>
                                    </dxe:ASPxDateEdit>
                                </td>
                                <td style="width: 17px"></td>
                                <td class="gridcellleft">
                                    <span class="Ecoheadtxt">Email Id:</span>
                                </td>
                                <td class="gridcellleft">
                                    <asp:TextBox ID="txtEmailId" runat="server" Width="200px" Font-Size="12px" TabIndex="9"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="gridcellleft">
                                    <span class="Ecoheadtxt">Qualification:</span>
                                </td>
                                <td class="gridcellleft">
                                    <asp:DropDownList ID="drpQualification" Width="203px" Font-Size="12px" runat="server"
                                        TabIndex="10">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 17px"></td>
                                <td class="gridcellleft">
                                    <span class="Ecoheadtxt">Professional Qualification:</span>
                                </td>
                                <td class="gridcellleft">
                                    <asp:DropDownList ID="drpProfQualification" Width="203px" Font-Size="12px" runat="server"
                                        TabIndex="11">
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td class="gridcellleft">
                                    <span class="Ecoheadtxt">Certifications:</span>
                                </td>
                                <td class="gridcellleft">
                                    <asp:TextBox ID="txtCertification" runat="server" Width="200px" Font-Size="12px"
                                        TabIndex="12"></asp:TextBox></td>
                                <td style="width: 17px"></td>
                                <td class="gridcellleft">
                                    <span class="Ecoheadtxt">Current Employer:</span>
                                </td>
                                <td class="gridcellleft">
                                    <asp:TextBox ID="txtCurrentEmployer" runat="server" Width="200px" Font-Size="12px"
                                        TabIndex="13"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="gridcellleft">
                                    <span class="Ecoheadtxt">Experience Yrs:</span>
                                </td>
                                <td class="gridcellleft">
                                    <asp:TextBox ID="txtExpYrs" runat="server" Width="200px" Font-Size="12px" TabIndex="14"></asp:TextBox></td>
                                <td style="width: 17px"></td>
                                <td class="gridcellleft">
                                    <span class="Ecoheadtxt">CurrentJobProfile:</span>
                                </td>
                                <td class="gridcellleft">
                                    <asp:DropDownList ID="drpCurrentJobProfile" Width="203PX" Font-Size="12px" runat="server"
                                        TabIndex="15">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="gridcellleft">
                                    <span class="Ecoheadtxt">Current CTC:</span>
                                </td>
                                <td class="gridcellleft">
                                    <asp:TextBox ID="txtCurrentCTC" runat="server" Width="200px" Font-Size="12px" TabIndex="16"></asp:TextBox></td>
                                <td style="width: 17px"></td>
                                <td class="gridcellleft">
                                    <span class="Ecoheadtxt">Desired CTC:</span>
                                </td>
                                <td class="gridcellleft">
                                    <asp:TextBox ID="txtDesiredCTC" runat="server" Width="200px" Font-Size="12px" TabIndex="17"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="gridcellleft">
                                    <span class="Ecoheadtxt">Previous CTC:</span>
                                </td>
                                <td class="gridcellleft">
                                    <asp:TextBox ID="txtPreviousCTC" runat="server" Width="200px" Font-Size="12px" TabIndex="18"></asp:TextBox>
                                </td>
                                <td style="width: 17px;"></td>
                                <td class="gridcellleft">
                                    <span class="Ecoheadtxt">Previous Employer:</span>
                                </td>
                                <td class="gridcellleft">
                                    <asp:TextBox ID="txtPreviousEmployer" runat="server" Width="200px" Font-Size="12px"
                                        TabIndex="19"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="gridcellleft">
                                    <span class="Ecoheadtxt">Probable Join Date:</span>
                                </td>
                                <td class="gridcellleft">
                                    <dxe:ASPxDateEdit ID="txtPJD" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                        TabIndex="20" Width="204px">
                                        <ButtonStyle Width="13px">
                                        </ButtonStyle>
                                    </dxe:ASPxDateEdit>
                                </td>
                                <td style="width: 17px;"></td>
                                <td class="gridcellleft">
                                    <span class="Ecoheadtxt">Previous Job Profile :</span>
                                </td>
                                <td class="gridcellleft">
                                    <asp:DropDownList ID="drpPreviousJobProfile" Width="203PX" Font-Size="12px" runat="server"
                                        TabIndex="21">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="gridcellleft">
                                    <span class="Ecoheadtxt">Reason For Change :</span>
                                </td>
                                <td class="gridcellleft">
                                    <asp:TextBox ID="txtReasonforChange" runat="server" TextMode="MultiLine" Width="198px"
                                        Font-Size="12px" TabIndex="22"></asp:TextBox>
                                </td>
                                <td style="width: 17px"></td>
                                <td class="gridcellleft">
                                    <%--<span class="Ecoheadtxt" >Actual Start Date/time:</span>--%>
                                </td>
                                <td class="gridcellleft">
                                    <%--<asp:TextBox ID="TextBox21" runat="server" Width="200px" Font-Size="12px"></asp:TextBox>--%>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="gridcellleft">
                                    <asp:Label ID="lblmessage" runat="server" ForeColor="Red"></asp:Label>&nbsp;</td>
                                <td style="width: 17px"></td>
                                <td colspan="2" class="gridcellright">
                                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btnUpdate" OnClick="btnSave_Click"
                                        TabIndex="23" ValidationGroup="a" />
                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btnUpdate" OnClick="btnCancel_Click"
                                        TabIndex="24" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
</asp:Content>

