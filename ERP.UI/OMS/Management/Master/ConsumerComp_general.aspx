<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.Master.management_master_ConsumerComp_general" CodeBehind="ConsumerComp_general.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script language="javascript" type="text/javascript">

        function disp_prompt(name) {
            if (name == "tab0") {
                //alert(name);
                document.location.href = "ConsumerComp_general.aspx";
            }
            if (name == "tab1") {
                //alert(name);
                document.location.href = "ConsumerComp_ContactPerson.aspx";
            }
            else if (name == "tab2") {
                //alert(name);
                document.location.href = "ConsumerComp_Correspondence.aspx";
            }
            else if (name == "tab3") {
                //alert(name);
                document.location.href = "ConsumerComp_BankDetails.aspx";
            }
            else if (name == "tab4") {
                //alert(name);
                document.location.href = "ConsumerComp_DPDetails.aspx";
            }
            else if (name == "tab5") {
                //alert(name);
                document.location.href = "ConsumerComp_Document.aspx";
            }
            else if (name == "tab6") {
                //alert(name);
                document.location.href = "ConsumerComp_GroupMember.aspx";
            }

        }
        function CallList(obj1, obj2, obj3) {
            //alert('rrr');

            var obj4 = document.getElementById("ASPxPageControl1_cmbSource");
            var obj5 = obj4.value;
            //alert(obj5);
            ajax_showOptions(obj1, obj2, obj3, obj5);
        }
        FieldName = 'ASPxPageControl1_cmbLegalStatus';
    </script>

    <div>
        <table class="TableMain100">
            <tr>
                <td class="EHEADER"></td>

            </tr>
            <tr>
                <td>
                    <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" ClientInstanceName="page" Font-Size="12px">
                        <TabPages>
                            <dxe:TabPage Text="General" Name="General">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                        <table style="width: 90%">
                                            <tr>
                                                <td class="Ecoheadtxt">
                                                    <dxe:ASPxLabel ID="ASPxLabel1" runat="server" Text="Salutation"></dxe:ASPxLabel>
                                                </td>
                                                <td class="Ecoheadtxt" style="text-align: left">

                                                    <asp:DropDownList ID="CmbSalutation" runat="server" Width="170px" TabIndex="1">
                                                    </asp:DropDownList>

                                                </td>
                                                <td class="Ecoheadtxt">
                                                    <dxe:ASPxLabel ID="ASPxLabel5" runat="server" Text="Name"></dxe:ASPxLabel>
                                                </td>
                                                <td class="Ecoheadtxt" style="text-align: left">
                                                    <dxe:ASPxTextBox ID="txtFirstName" runat="server" Width="170px" TabIndex="2">
                                                        <ValidationSettings ValidationGroup="a">
                                                        </ValidationSettings>
                                                    </dxe:ASPxTextBox>

                                                </td>
                                                <td class="Ecoheadtxt" style="width: 158px">&nbsp;<dxe:ASPxLabel ID="ASPxLabel4" runat="server" Text="Code" Width="35px"></dxe:ASPxLabel>
                                                </td>
                                                <td class="Ecoheadtxt" style="text-align: left">
                                                    <dxe:ASPxTextBox ID="txtCode" runat="server" Width="170px" TabIndex="3">
                                                    </dxe:ASPxTextBox>

                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4" style="text-align: right; height: 1px">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtFirstName"
                                                        Display="Dynamic" ErrorMessage="Must Fill Name" SetFocusOnError="True" ValidationGroup="a" Font-Bold="True"></asp:RequiredFieldValidator>
                                                </td>
                                                <td colspan="2" style="text-align: right; height: 1px">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtCode"
                                                        Display="Dynamic" ErrorMessage="Code Required" Font-Bold="True" ValidationGroup="a"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="Ecoheadtxt">&nbsp;<dxe:ASPxLabel ID="ASPxLabel10" runat="server" Text="Branch" Width="59px"></dxe:ASPxLabel>
                                                </td>
                                                <td class="Ecoheadtxt" style="text-align: left">
                                                    <asp:DropDownList ID="cmbBranch" runat="server" Width="170px" TabIndex="4">
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="Ecoheadtxt">&nbsp;<dxe:ASPxLabel ID="ASPxLabel13" runat="server" Text="Source"></dxe:ASPxLabel>
                                                </td>
                                                <td class="Ecoheadtxt" style="text-align: left">
                                                    <asp:DropDownList ID="cmbSource" runat="server" Width="170px" TabIndex="5">
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="Ecoheadtxt" style="width: 158px">&nbsp;<dxe:ASPxLabel ID="ASPxLabel17" runat="server" Text="Refered By" Width="73px"></dxe:ASPxLabel>
                                                </td>
                                                <td class="Ecoheadtxt" style="text-align: left">
                                                    <asp:TextBox ID="txtReferedBy" runat="server" TabIndex="6" Width="165px"></asp:TextBox><asp:TextBox ID="txtReferedBy_hidden" runat="server" Visible="False"></asp:TextBox>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td class="Ecoheadtxt">&nbsp;<dxe:ASPxLabel ID="ASPxLabel19" runat="server" Text="Contact Status" Width="95px"></dxe:ASPxLabel>
                                                </td>
                                                <td class="Ecoheadtxt" style="text-align: left">
                                                    <asp:DropDownList ID="cmbContactStatus" runat="server" Width="170px" TabIndex="7">
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="Ecoheadtxt">
                                                    <dxe:ASPxLabel ID="ASPxLabel20" runat="server" Text="Legal Status" Width="70px"></dxe:ASPxLabel>
                                                </td>
                                                <td class="Ecoheadtxt">
                                                    <asp:DropDownList ID="cmbLegalStatus" runat="server" Width="170px" TabIndex="8">
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="Ecoheadtxt" style="width: 158px">
                                                    <dxe:ASPxLabel ID="ASPxLabel2" runat="server" Text="Date Of Incoorporation" Width="140px"></dxe:ASPxLabel>
                                                </td>
                                                <td class="Ecoheadtxt" style="text-align: left">
                                                    <dxe:ASPxDateEdit ID="DateOfIncoorporation" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" DateOnError="Null" TabIndex="9" UseMaskBehavior="True">
                                                    </dxe:ASPxDateEdit>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="Ecoheadtxt">&nbsp;</td>
                                                <td class="Ecoheadtxt" style="text-align: left">&nbsp;</td>
                                                <td class="Ecoheadtxt">&nbsp;</td>
                                                <td class="Ecoheadtxt" style="text-align: left;">&nbsp;
                                                </td>

                                                <td class="Ecoheadtxt" style="width: 158px"></td>
                                                <td class="Ecoheadtxt" style="text-align: right">
                                                    <table>
                                                        <tr>
                                                            <td style="height: 43px">
                                                                <dxe:ASPxButton ID="btnSave" runat="server" Text="Save" ValidationGroup="a" TabIndex="10" OnClick="btnSave_Click"></dxe:ASPxButton>
                                                            </td>

                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>

                                        </table>
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Text="Contact Person" Name="ContactPreson">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server"></dxe:ContentControl>
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
                            <dxe:TabPage Name="DPDetails" Text="DP Details">
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

                            <dxe:TabPage Name="GroupMember" Text="Group Member">
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
                    </dxe:ASPxPageControl>
                </td>
            </tr>

            <tr>
                <td style="height: 8px">

                    <table style="width: 100%;">
                        <tr>
                            <td align="right" style="width: 843px">
                                <asp:HiddenField ID="hdReferenceBy" runat="server" />

                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
