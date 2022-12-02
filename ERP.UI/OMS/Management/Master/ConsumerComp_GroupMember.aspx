<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.Master.management_master_ConsumerComp_GroupMember" CodeBehind="ConsumerComp_GroupMember.aspx.cs" %>

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
                //document.location.href="ConsumerComp_GroupMember.aspx"; 
            }
        }
    </script>

    <div>
        <table class="TableMain100">
            <tr>
                <td class="EHEADER"></td>
            </tr>
            <tr>
                <td>
                    <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="6" ClientInstanceName="page" Font-Size="12px" Width="100%">
                        <TabPages>
                            <dxe:TabPage Text="General" Name="General">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Contact Person" Text="Contact Person">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
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
                                        <table width="100%">
                                            <tr>
                                                <td align="center">
                                                    <asp:Panel ID="GridPanel" runat="server" Width="100%">
                                                        <dxe:ASPxGridView ID="GroupMasterGrid" runat="server" AutoGenerateColumns="False" Width="100%"
                                                            DataSourceID="GroupMaster">
                                                            <Columns>
                                                                <dxe:GridViewDataTextColumn FieldName="GroupName" VisibleIndex="0">
                                                                    <CellStyle CssClass="gridcellleft" HorizontalAlign="Left">
                                                                    </CellStyle>
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn FieldName="GroupType" VisibleIndex="1">
                                                                    <CellStyle CssClass="gridcellleft" HorizontalAlign="Left">
                                                                    </CellStyle>
                                                                </dxe:GridViewDataTextColumn>
                                                            </Columns>
                                                            <Styles>
                                                                <LoadingPanel ImageSpacing="10px">
                                                                </LoadingPanel>
                                                                <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                                </Header>
                                                            </Styles>
                                                            <StylesPager>
                                                                <Summary Width="100%">
                                                                </Summary>
                                                            </StylesPager>
                                                            <SettingsPager ShowSeparators="True">
                                                                <FirstPageButton Visible="True">
                                                                </FirstPageButton>
                                                                <LastPageButton Visible="True">
                                                                </LastPageButton>
                                                            </SettingsPager>

                                                        </dxe:ASPxGridView>

                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 90%; text-align: right" align="right">
                                                    <asp:Button ID="BtnAdd" runat="server" Text="Add" OnClick="BtnAdd_Click" /></td>
                                            </tr>
                                            <tr>
                                                <td style="width: 100%" align="center">
                                                    <asp:Panel ID="TablePanel" runat="server" Width="90%" Visible="False">
                                                        <table width="40%" style="">
                                                            <tr>
                                                                <td style="text-align: left; color: Blue">
                                                                    <asp:Table ID="TableBind" runat="server" Width="100%"></asp:Table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: right">
                                                                    <asp:Button ID="BtnSave" runat="server" OnClick="BtnSave_Click" Text="Save" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>

                                                </td>
                                            </tr>
                                        </table>
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
	                                            }"></ClientSideEvents>
                        <ContentStyle>
                            <Border BorderColor="#002D96" BorderStyle="Solid" BorderWidth="1px" />
                        </ContentStyle>
                        <LoadingPanelStyle ImageSpacing="6px">
                        </LoadingPanelStyle>
                    </dxe:ASPxPageControl>
                </td>
            </tr>


        </table>
        <asp:SqlDataSource ID="GroupMaster" runat="server" 
            SelectCommand="select tbl_master_groupMaster.gpm_Description as GroupName, tbl_master_groupMaster.gpm_Type as GroupType&#13;&#10;from tbl_trans_group INNER JOIN tbl_master_groupMaster ON tbl_trans_group.grp_groupMaster = tbl_master_groupMaster.gpm_id where tbl_trans_group.grp_contactId = @ContactId">
            <SelectParameters>
                <asp:SessionParameter Name="ContactId" SessionField="KeyVal_InternalID" Type="string" />

            </SelectParameters>
        </asp:SqlDataSource>
        <asp:TextBox ID="Counter" runat="server" Visible="False"></asp:TextBox>
    </div>
</asp:Content>
