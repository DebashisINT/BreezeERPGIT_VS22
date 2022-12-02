<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.Master.management_Master_Lead_general" CodeBehind="Lead_general.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <!--___________________These files are for datetime calander___________________-->
    <link type="text/css" rel="stylesheet" href="/OMS/css/dhtmlgoodies_calendar.css?random=20051112" media="screen" />
    <script type="text/javascript" src="/assests/js/dhtmlgoodies_calendar.js?random=20060118"></script>
    <!--___________________________________________________________________________-->
    <script type="text/javascript">
        //function ul() {
        //    window.opener.document.getElementById('iFrmInformation').setAttribute('src', 'CallUserInformation.aspx')
        //}
        function disp_prompt(name) {
            if (name == "tab0") {
                //alert(name);
                document.location.href = "Lead_general.aspx";
            }
            if (name == "tab1") {
                //alert(name);
                document.location.href = "Lead_Correspondence.aspx";
            }
            else if (name == "tab2") {
                //alert(name);
                document.location.href = "Lead_BankDetails.aspx";
            }
            else if (name == "tab3") {
                //alert(name);
                document.location.href = "Lead_DPDetails.aspx";
            }
            else if (name == "tab4") {
                //alert(name);
                document.location.href = "Lead_Document.aspx";
            }
            else if (name == "tab5") {
                //alert(name);
                document.location.href = "Lead_FamilyMembers.aspx";
            }
            else if (name == "tab6") {
                //alert(name);
                document.location.href = "Lead_Registration.aspx";
            }
            else if (name == "tab7") {
                //alert(name);
                document.location.href = "Lead_GroupMember.aspx";
            }
            else if (name == "tab8") {
                //alert(name);
                document.location.href = "Lead_Remarks.aspx";
            }
        }
        function CallList(obj1, obj2, obj3) {
            var obj4 = document.getElementById("<%=cmbSource.ClientID%>");
            var obj5 = obj4.value;
            //alert(obj5);
            ajax_showOptions(obj1, obj2, obj3, obj5);
            //alert(obj5);
            FieldName = '<%=cmbGender.ClientID%>';
        }
        function AtTheTimePageLoad() {
            FieldName = '<%=cmbLegalStatus.ClientID%>';

            document.getElementById("<%=txtReferedBy_hidden.ClientID%>").style.display = 'none';
        }
        function ul() {
            window.opener.document.getElementById('iFrmInformation').setAttribute('src', 'CallUserInformation.aspx')
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Add Lead</h3>
        </div>
        <div class="crossBtn"><a href="Lead.aspx"><i class="fa fa-times"></i></a></div>
    </div>
    <div class="form_main">
        <table class="TableMain100">
            <tr>
                <td>
                    <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" ClientInstanceName="page" Font-Size="12px" Width="100%">
                        <TabPages>
                            <dxe:TabPage Text="General" Name="General">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">

                                        <table style="width: 100%">
                                            <tr>
                                                <td>
                                                    <dxe:ASPxLabel ID="ASPxLabel1" runat="server" Text="Salutation"></dxe:ASPxLabel>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="CmbSalutation" CssClass="form-control" runat="server" TabIndex="1">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <dxe:ASPxLabel ID="ASPxLabel5" runat="server" Text="First Name"></dxe:ASPxLabel>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtFirstNmae" runat="server" CssClass="form-control" TabIndex="2">
                                                    </asp:TextBox>
                                                </td>
                                                <td>
                                                    <dxe:ASPxLabel ID="ASPxLabel2" runat="server" Text="Middle Name"></dxe:ASPxLabel>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtMiddleName" runat="server" CssClass="form-control" TabIndex="3">
                                                    </asp:TextBox>

                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4" style="text-align: right; height: 1px">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtFirstNmae"
                                                        Display="Dynamic" ErrorMessage="Must Fill First Name" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                </td>
                                                <td colspan="2" style="text-align: right; height: 1px"></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <dxe:ASPxLabel ID="ASPxLabel3" runat="server" Text="Last Name"></dxe:ASPxLabel>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control" TabIndex="4">
                                                    </asp:TextBox>
                                                </td>
                                                <td style="display:none">
                                                    <dxe:ASPxLabel ID="ASPxLabel4" runat="server" Text="Short Name (Alias)"></dxe:ASPxLabel>
                                                </td>
                                                <td  style="display:none">
                                                    <asp:TextBox ID="txtAliasName" runat="server" CssClass="form-control" TabIndex="5">
                                                    </asp:TextBox>
                                                </td>
                                                <td>
                                                    <dxe:ASPxLabel ID="ASPxLabel6" runat="server" Text="Profession"></dxe:ASPxLabel>
                                                </td>
                                                <td>

                                                    <asp:DropDownList ID="cmbProfession" runat="server" CssClass="form-control" TabIndex="6">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <dxe:ASPxLabel ID="ASPxLabel7" runat="server" Text="Organization"></dxe:ASPxLabel>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtOrganization" runat="server" CssClass="form-control" TabIndex="7">
                                                    </asp:TextBox>
                                                </td>
                                                <td>
                                                    <dxe:ASPxLabel ID="ASPxLabel8" runat="server" Text="Job Responsibility"></dxe:ASPxLabel>
                                                </td>
                                                <td>

                                                    <asp:DropDownList ID="cmbJobResponsibility" runat="server" CssClass="form-control" TabIndex="8">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <dxe:ASPxLabel ID="ASPxLabel9" runat="server" Text="Designation"></dxe:ASPxLabel>
                                                </td>
                                                <td>

                                                    <asp:DropDownList ID="cmbDesignation" runat="server" CssClass="form-control" TabIndex="9">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <dxe:ASPxLabel ID="ASPxLabel10" runat="server" Text="Branch"></dxe:ASPxLabel>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="cmbBranch" runat="server" CssClass="form-control" TabIndex="10">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <dxe:ASPxLabel ID="ASPxLabel11" runat="server" Text="Industry/Sector"></dxe:ASPxLabel>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="cmbIndustry" runat="server" CssClass="form-control" TabIndex="11">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <dxe:ASPxLabel ID="ASPxLabel12" runat="server" Text="Short Name"></dxe:ASPxLabel>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtClentUcc" runat="server" CssClass="form-control" TabIndex="12" MaxLength="50">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <dxe:ASPxLabel ID="ASPxLabel13" runat="server" Text="Source"></dxe:ASPxLabel>
                                                </td>
                                                <td>

                                                    <asp:DropDownList ID="cmbSource" runat="server" CssClass="form-control" TabIndex="13">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <dxe:ASPxLabel ID="ASPxLabel17" runat="server" Text="Referred By"></dxe:ASPxLabel>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtReferedBy" runat="server" TabIndex="14" CssClass="form-control"></asp:TextBox>
                                                    <asp:TextBox ID="txtReferedBy_hidden" runat="server" TabIndex="14"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <dxe:ASPxLabel ID="ASPxLabel15" runat="server" Text="Rating"></dxe:ASPxLabel>
                                                </td>
                                                <td>

                                                    <asp:DropDownList ID="cmbRating" runat="server" CssClass="form-control" TabIndex="15">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <dxe:ASPxLabel ID="ASPxLabel16" runat="server" Text="Marital Status"></dxe:ASPxLabel>
                                                </td>
                                                <td style="text-align: left;">
                                                    <asp:DropDownList ID="cmbMaritalStatus" runat="server" CssClass="form-control" TabIndex="16">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <dxe:ASPxLabel ID="ASPxLabel14" runat="server" Text="Gender"></dxe:ASPxLabel>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="cmbGender" runat="server" CssClass="form-control" TabIndex="17">
                                                        <asp:ListItem Value="1">Male</asp:ListItem>
                                                        <asp:ListItem Value="0">Female</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <dxe:ASPxLabel ID="ASPxLabel20" runat="server" Text="Legal Status"></dxe:ASPxLabel>
                                                </td>
                                                <td>

                                                    <asp:DropDownList ID="cmbLegalStatus" runat="server" CssClass="form-control" TabIndex="18">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <dxe:ASPxLabel ID="ASPxLabel19" runat="server" Text="Contact Status"></dxe:ASPxLabel>
                                                </td>
                                                <td>

                                                    <asp:DropDownList ID="cmbContactStatus" runat="server" CssClass="form-control" TabIndex="19">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <dxe:ASPxLabel ID="ASPxLabel18" runat="server" Text="D.O.B."></dxe:ASPxLabel>
                                                </td>
                                                <td style="text-align: left;">
                                                    <dxe:ASPxDateEdit ID="txtDOB" runat="server" EditFormat="Custom" Width="100%" UseMaskBehavior="True" TabIndex="20">
                                                        <ButtonStyle Width="13px">
                                                        </ButtonStyle>
                                                    </dxe:ASPxDateEdit>
                                                </td>
                                                <td>
                                                    <dxe:ASPxLabel ID="ASPxLabel21" runat="server" Text="Anniversary Date" Width="111px"></dxe:ASPxLabel>
                                                </td>
                                                <td>
                                                    <dxe:ASPxDateEdit ID="txtAnniversary" runat="server" Width="100%" EditFormat="Custom" UseMaskBehavior="True" TabIndex="21">
                                                        <ButtonStyle Width="13px">
                                                        </ButtonStyle>
                                                    </dxe:ASPxDateEdit>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <dxe:ASPxLabel ID="ASPxLabel22" runat="server" Text="Education">
                                                    </dxe:ASPxLabel>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="cmbEducation" runat="server" CssClass="form-control" TabIndex="22">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <dxe:ASPxLabel ID="ASPxLabel28" runat="server" Text="Blood Group"></dxe:ASPxLabel>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="cmbBloodgroup" runat="server" CssClass="form-control" TabIndex="23">
                                                        <asp:ListItem Value="A+">A+</asp:ListItem>
                                                        <asp:ListItem Value="A-">A-</asp:ListItem>
                                                        <asp:ListItem Value="B+">B+</asp:ListItem>
                                                        <asp:ListItem Value="B-">B-</asp:ListItem>
                                                        <asp:ListItem Value="AB+">AB+</asp:ListItem>
                                                        <asp:ListItem Value="AB-">AB-</asp:ListItem>
                                                        <asp:ListItem Value="O+">O+</asp:ListItem>
                                                        <asp:ListItem Value="O-">O-</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td"></td>
                                                <td></td>
                                            </tr>
                                        </table>


                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Text="CorresPondence" Name="CorresPondence">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server"></dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="BankDetails" Text="Bank Details">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="DPDetails" Text="DP Details" Visible="false">
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
                            <dxe:TabPage Name="FamilyMembers" Text="Family Members">
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
                            <dxe:TabPage Name="Remarks" Text="Remarks">
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
                                <table>
                                    <tr>
                                        <td style="height: 43px">
                                            <dxe:ASPxButton ID="btnSave" runat="server" Text="Save and Proceed" CssClass="btn btn-primary" OnClick="btnSave_Click" TabIndex="26"></dxe:ASPxButton>
                                        </td>
                                        <td style="height: 43px">
                                            <dxe:ASPxButton ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" TabIndex="27" Visible="false">
                                            </dxe:ASPxButton>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
