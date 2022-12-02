<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.Master.management_master_OutsourcingComp_Document" CodeBehind="OutsourcingComp_Document.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        function Show() {

            var url = "frmAddDocuments.aspx?id=OutsourcingComp_Document.aspx&id1=OutsourcingComp";

            popup.SetContentUrl(url);

            popup.Show();

        }
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
                //document.location.href="OutsourcingComp_Document.aspx"; 
            }
            else if (name == "tab6") {
                //alert(name);
                document.location.href = "OutsourcingComp_GroupMember.aspx";
            }
        }
        function OnDocumentView(keyValue) {
            var url = 'viewImage.aspx?id=' + keyValue;
            popup.contentUrl = url;
            popup.Show();

        }
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
                    <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="5" ClientInstanceName="page">
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
                            <dxe:TabPage Name="DPDetails" Text="DP Details">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Documents" Text="Documents">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                        <table class="TableMain100">
                                            <tr>
                                                <td id="ShowFilter">
                                                    <a href="javascript:void(0);" onclick="Show();" class="btn btn-success"><span>Add New</span> </a>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="gridcellcenter">
                                                    <dxe:ASPxGridView ID="EmployeeDocumentGrid" runat="server" AutoGenerateColumns="False"
                                                        ClientInstanceName="gridDocument" KeyFieldName="Id" Width="100%" Font-Size="12px"
                                                        OnRowDeleting="EmployeeDocumentGrid_RowDeleting">
                                                        <Columns>
                                                            <dxe:GridViewDataTextColumn FieldName="Id" ReadOnly="True" VisibleIndex="0" Visible="False">
                                                                <EditFormSettings Visible="False" />
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataTextColumn FieldName="Type" VisibleIndex="0" Caption="DocumentType"
                                                                Width="25%">
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataTextColumn FieldName="FileName" VisibleIndex="1" Caption="DocumentName"
                                                                Width="25%">
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataTextColumn FieldName="Src" VisibleIndex="2" Visible="False">
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataTextColumn FieldName="FilePath" ReadOnly="True" VisibleIndex="2"
                                                                Caption="Document Physical Location" Width="25%">
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataHyperLinkColumn Caption="View" FieldName="Src" VisibleIndex="3"
                                                                Width="15%">
                                                                <DataItemTemplate>
                                                                    <%-- <a href='viewImage.aspx?id=<%#Eval("Src") %>' target="_blank">View</a>--%>
                                                                    <a onclick="OnDocumentView('<%#Eval("Src") %>')" style="cursor: pointer;">View</a>
                                                                </DataItemTemplate>
                                                            </dxe:GridViewDataHyperLinkColumn>
                                                            <dxe:GridViewCommandColumn VisibleIndex="4" ShowDeleteButton="true">
                                                                <%--<DeleteButton Visible="True">
                                                    </DeleteButton>--%>
                                                                <HeaderTemplate>
                                                                    <%--<%if (Session["PageAccess"].ToString().Trim() == "All" || Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "DelAdd")
                                                          { %>
                                                        <dxe:ASPxHyperLink ID="ASPxHyperLink1" runat="server" Text="Add New" ClientSideEvents-Click="function(s, e) {Show();}"
                                                            Font-Size="12px" Font-Underline="true">
                                                        </dxe:ASPxHyperLink>
                                                        <%} %>--%>
                                                                </HeaderTemplate>
                                                            </dxe:GridViewCommandColumn>
                                                        </Columns>
                                                        <Settings ShowStatusBar="Visible" />
                                                        <SettingsEditing Mode="PopupEditForm" PopupEditFormHeight="250px" PopupEditFormHorizontalAlign="Center"
                                                            PopupEditFormModal="True" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="500px"
                                                            EditFormColumnCount="1" />
                                                        <Styles>
                                                            <LoadingPanel ImageSpacing="10px">
                                                            </LoadingPanel>
                                                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                            </Header>
                                                        </Styles>
                                                        <SettingsText PopupEditFormCaption="Add/Modify Family Relationship" ConfirmDelete="Confirm delete?" />
                                                        <SettingsPager NumericButtonCount="20" PageSize="20" AlwaysShowPager="True" ShowSeparators="True">
                                                            <FirstPageButton Visible="True">
                                                            </FirstPageButton>
                                                            <LastPageButton Visible="True">
                                                            </LastPageButton>
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
                        <%--<TabStyle Font-Size="12px">
                        </TabStyle>--%>
                    </dxe:ASPxPageControl>
                    <asp:TextBox ID="txtID" runat="server" Visible="false"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
