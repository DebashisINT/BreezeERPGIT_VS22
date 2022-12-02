<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.Master.management_master_OutsourcingComp_general" CodeBehind="OutsourcingComp_general.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script language="javascript" type="text/javascript">

        function disp_prompt(name) {
            if (name == "tab0") {
                //alert(name);
                document.location.href = "OutsourcingComp_general.aspx";
            }
            if (name == "tab1") {
                //alert(name);
                document.location.href = "OutsourcingComp_ContactPerson.aspx";
            }
            else if (name == "tab2") {
                //alert(name);
                document.location.href = "OutsourcingComp_Correspondence.aspx";
            }
            else if (name == "tab3") {
                //alert(name);
                document.location.href = "OutsourcingComp_BankDetails.aspx";
            }
            else if (name == "tab4") {
                //alert(name);
                document.location.href = "OutsourcingComp_DPDetails.aspx";
            }
            else if (name == "tab5") {
                //alert(name);
                document.location.href = "OutsourcingComp_Document.aspx";
            }
            else if (name == "tab6") {
                //alert(name);
                document.location.href = "OutsourcingComp_GroupMember.aspx";
            }

        }
        function CallList(obj1, obj2, obj3) {
            //alert('rrr');

            var obj4 = document.getElementById("cmbSource");
            var obj5 = obj4.value;
            //alert(obj5);
            ajax_showOptions(obj1, obj2, obj3, obj5);
        }
        FieldName = 'cmbLegalStatus';
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Outsourcing Agents/Companies</h3>
        </div>

    </div>
    <div class="form_main">
        <table class="TableMain100">
            <tr>
                <td style="vertical-align: top; padding-left: 30px;">
                    <div class="crossBtn"><a href="OutsourcingComp.aspx"><i class="fa fa-times"></i></a></div>
                </td>
            </tr>
            <tr>
                <td style="text-align: center">
                    <asp:Label ID="lblHeader" runat="server" Font-Bold="True" Font-Size="15px" ForeColor="Navy"
                        Width="819px" Height="18px"></asp:Label></td>
            </tr>
            <tr>
                <td>
                    <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" ClientInstanceName="page"
                        Font-Size="12px" Width="100%">
                        <TabPages>
                            <dxe:TabPage Text="General" Name="General">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                        <table class="TableMain100">
                                            <tr>
                                                <td class="Ecoheadtxt">
                                                    <dxe:ASPxLabel ID="ASPxLabel1" runat="server" Text="Salutation">
                                                    </dxe:ASPxLabel>
                                                </td>
                                                <td class="Ecoheadtxt" style="text-align: left">
                                                    <asp:DropDownList ID="CmbSalutation" runat="server" Width="170px" TabIndex="1">
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="Ecoheadtxt">
                                                    <dxe:ASPxLabel ID="ASPxLabel5" runat="server" Text="Name">
                                                    </dxe:ASPxLabel><span style="color:red"> *</span>
                                                </td>
                                                <td class="Ecoheadtxt" style="text-align: left">
                                                    <dxe:ASPxTextBox ID="txtFirstName" runat="server" Width="170px" TabIndex="2">
                                                        <ValidationSettings ValidationGroup="a">
                                                        </ValidationSettings>
                                                    </dxe:ASPxTextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtFirstName"
                                                        Display="Dynamic" ErrorMessage="Mandatory" SetFocusOnError="True" ValidationGroup="a"
                                                        Font-Bold="True" ForeColor="red"></asp:RequiredFieldValidator>
                                                </td>
                                                <td class="Ecoheadtxt" style="width: 158px">
                                                    <dxe:ASPxLabel ID="ASPxLabel4" runat="server" Text="Short Name" >
                                                    </dxe:ASPxLabel><span style="color:red"> *</span>
                                                </td>
                                                <td class="Ecoheadtxt" style="text-align: left">
                                                    <dxe:ASPxTextBox ID="txtCode" runat="server" Width="170px" TabIndex="3">
                                                    </dxe:ASPxTextBox>
                                                     <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtCode"
                                                        Display="Dynamic" ErrorMessage="Mandatory" Font-Bold="True" ValidationGroup="a" ForeColor="red"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4" style="text-align: right; height: 1px">
                                                    
                                                </td>
                                                <td colspan="2" style="text-align: right; height: 1px">
                                                   
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="Ecoheadtxt"><dxe:ASPxLabel ID="ASPxLabel10" runat="server" Text="Branch" Width="59px">
                                                </dxe:ASPxLabel>
                                                </td>
                                                <td class="Ecoheadtxt" style="text-align: left">
                                                    <asp:DropDownList ID="cmbBranch" runat="server" Width="170px" TabIndex="4">
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="Ecoheadtxt"><dxe:ASPxLabel ID="ASPxLabel13" runat="server" Text="Source">
                                                </dxe:ASPxLabel>
                                                </td>
                                                <td class="Ecoheadtxt" style="text-align: left">
                                                    <asp:DropDownList ID="cmbSource" runat="server" Width="170px" TabIndex="5">
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="Ecoheadtxt" style="width: 158px"><dxe:ASPxLabel ID="ASPxLabel17" runat="server" Text="Referred By" Width="73px">
                                                </dxe:ASPxLabel>
                                                </td>
                                                <td class="Ecoheadtxt" style="text-align: left">
                                                    <asp:TextBox ID="txtReferedBy" runat="server" TabIndex="6" Width="170px"></asp:TextBox><asp:TextBox
                                                        ID="txtReferedBy_hidden" runat="server" Visible="False"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="Ecoheadtxt"><dxe:ASPxLabel ID="ASPxLabel19" runat="server" Text="Contact Status" Width="95px">
                                                </dxe:ASPxLabel>
                                                </td>
                                                <td class="Ecoheadtxt" style="text-align: left">
                                                    <asp:DropDownList ID="cmbContactStatus" runat="server" Width="170px" TabIndex="7">
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="Ecoheadtxt">
                                                    <dxe:ASPxLabel ID="ASPxLabel20" runat="server" Text="Legal Status" Width="70px">
                                                    </dxe:ASPxLabel>
                                                </td>
                                                <td class="Ecoheadtxt">
                                                    <asp:DropDownList ID="cmbLegalStatus" runat="server" Width="170px" TabIndex="8">
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="Ecoheadtxt" style="width: 158px">
                                                    <dxe:ASPxLabel ID="ASPxLabel2" runat="server" Text="Date Of Incorporation" Width="140px">
                                                    </dxe:ASPxLabel>
                                                </td>
                                                <td class="Ecoheadtxt" style="text-align: left">
                                                    <%-- <dxe:ASPxDateEdit ID="DateOfIncoorporation" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" DateOnError="Null"
                                                        TabIndex="9" UseMaskBehavior="True">
                                                    </dxe:ASPxDateEdit>--%>
                                                    <dxe:ASPxDateEdit ID="DateOfIncorporation" runat="server"
                                                        EditFormat="Custom" EditFormatString="dd-MM-yyyy" DateOnError="Null"
                                                        UseMaskBehavior="True">
                                                    </dxe:ASPxDateEdit>
                                                </td>
                                            </tr>
                                            <tr>
                                                
                                                <td colspan="6" class="Ecoheadtxt" style="padding-left:164px">
                                                    <table>
                                                        <tr>
                                                            <td style="height: 43px">
                                                                <dxe:ASPxButton ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" ValidationGroup="a" TabIndex="10"
                                                                    OnClick="btnSave_Click">
                                                                </dxe:ASPxButton>
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
                            <dxe:TabPage Name="DPDetails" Visible="false" Text="DP Details">
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

