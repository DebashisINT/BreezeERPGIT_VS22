<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.management_sales_Slodging" CodeBehind="sales_Slodging.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function disp_prompt(name) {
            if (name == "tab0") {
                //alert(name);
                document.location.href = "sales_Stotal.aspx";
            }
            if (name == "tab1") {
                //alert(name);
                document.location.href = "sales_Sconveyence.aspx";
            }
            else if (name == "tab2") {
                //alert(name);
                document.location.href = "sales_Stravelling.aspx";
            }
            else if (name == "tab3") {
                //alert(name);
                //document.location.href="sales_Slodging.aspx"; 
            }
            else if (name == "tab4") {
                //alert(name);
                document.location.href = "sales_Sfooding.aspx";
            }
            else if (name == "tab5") {
                //alert(name);
                document.location.href = "sales_SbusinessP.aspx";
            }
            else if (name == "tab6") {
                //alert(name);
                document.location.href = "sales_Sother.aspx";
            }

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table class="TableMain100">
            <tr>
                <td>
                    <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="3" ClientInstanceName="page"
                        Font-Size="12px">
                        <TabPages>
                            <dxe:TabPage Text="Total Expenses" Name="Total">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Text="Conveyence" Name="Conveyence">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Travelling" Text="Travelling">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Lodging" Text="Lodging">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                        <dxe:ASPxGridView ID="gridLodging" ClientInstanceName="grid" DataSourceID="SqlLodging"
                                            runat="server" KeyFieldName="expnd_internalId" Width="100%" AutoGenerateColumns="False"
                                            OnInitNewRow="gridLodging_InitNewRow">
                                            <Columns>
                                                <dxe:GridViewDataTextColumn FieldName="expnd_internalId" VisibleIndex="0" Visible="False">
                                                    <EditFormSettings Visible="False" />
                                                    <CellStyle CssClass="gridcellleft">
                                                    </CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="expnd_empId" VisibleIndex="0" Visible="False">
                                                    <EditFormSettings Visible="False" />
                                                    <CellStyle CssClass="gridcellleft">
                                                    </CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="expnd_compId" VisibleIndex="0" Visible="False">
                                                    <EditFormSettings Visible="False" />
                                                    <CellStyle CssClass="gridcellleft">
                                                    </CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="expnd_branchId" VisibleIndex="0" Visible="False">
                                                    <EditFormSettings Visible="False" />
                                                    <CellStyle CssClass="gridcellleft">
                                                    </CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="expnd_activityId" VisibleIndex="0" Visible="False">
                                                    <EditFormSettings Visible="False" />
                                                    <CellStyle CssClass="gridcellleft">
                                                    </CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataComboBoxColumn FieldName="expnd_currencyType" Visible="False"
                                                    VisibleIndex="0">
                                                    <PropertiesComboBox DataSourceID="SqlCurrency" ValueField="curr_id" TextField="curr_symbol"
                                                        ValueType="System.String">
                                                    </PropertiesComboBox>
                                                    <EditFormSettings Visible="True" Caption="Currency Type" VisibleIndex="0" />
                                                    <EditFormCaptionStyle Wrap="False">
                                                    </EditFormCaptionStyle>
                                                    <CellStyle CssClass="gridcellleft">
                                                    </CellStyle>
                                                </dxe:GridViewDataComboBoxColumn>
                                                <dxe:GridViewDataComboBoxColumn Caption="Lodging Type" FieldName="expnd_LodgingType"
                                                    VisibleIndex="0">
                                                    <PropertiesComboBox ValueType="System.String">
                                                        <Items>
                                                            <dxe:ListEditItem Text="Budget " Value="Budget"></dxe:ListEditItem>
                                                            <dxe:ListEditItem Text="Hotel – Luxury " Value="Hotel – Luxury"></dxe:ListEditItem>
                                                            <dxe:ListEditItem Text="Service apartments " Value="Service apartments"></dxe:ListEditItem>
                                                            <dxe:ListEditItem Text="Guest house " Value="Guest house"></dxe:ListEditItem>
                                                            <dxe:ListEditItem Text="other " Value="other"></dxe:ListEditItem>
                                                        </Items>
                                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="Text" SetFocusOnError="True">
                                                            <RequiredField ErrorText="Required" IsRequired="True" />
                                                        </ValidationSettings>
                                                    </PropertiesComboBox>
                                                    <EditFormSettings Visible="True" VisibleIndex="1" />
                                                    <EditFormCaptionStyle Wrap="False">
                                                    </EditFormCaptionStyle>
                                                    <CellStyle CssClass="gridcellleft">
                                                    </CellStyle>
                                                </dxe:GridViewDataComboBoxColumn>
                                                <dxe:GridViewDataDateColumn FieldName="expnd_LodgingDateFrom" Visible="False" VisibleIndex="0">
                                                    <PropertiesDateEdit DisplayFormatString="" EditFormat="Custom" EditFormatString="dd/MM/yyyy">
                                                    </PropertiesDateEdit>
                                                    <EditFormSettings Visible="True" Caption="From" VisibleIndex="2" />
                                                    <EditFormCaptionStyle Wrap="False">
                                                    </EditFormCaptionStyle>
                                                    <CellStyle CssClass="gridcellleft">
                                                    </CellStyle>
                                                </dxe:GridViewDataDateColumn>
                                                <dxe:GridViewDataTextColumn FieldName="expnd_LodgingDateFrom1" Caption="From" VisibleIndex="1">
                                                    <EditFormSettings Visible="False" />
                                                    <CellStyle CssClass="gridcellleft">
                                                    </CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataDateColumn FieldName="expnd_LodgingDateTo" Caption="To" VisibleIndex="2">
                                                    <PropertiesDateEdit DisplayFormatString="{0:dd MM yyyy}" EditFormat="Custom" EditFormatString="dd-MM-yyyy"
                                                        UseMaskBehavior="true">
                                                    </PropertiesDateEdit>
                                                    <EditFormSettings Visible="True" Caption="To" VisibleIndex="3" />
                                                    <EditFormCaptionStyle Wrap="False">
                                                    </EditFormCaptionStyle>
                                                    <CellStyle CssClass="gridcellleft">
                                                    </CellStyle>
                                                </dxe:GridViewDataDateColumn>
                                                <dxe:GridViewDataTextColumn FieldName="expnd_LodgingExpnseAmount" Caption="Amount"
                                                    VisibleIndex="3">
                                                    <EditFormSettings Visible="True" Caption="Expense Amount" VisibleIndex="4" />
                                                    <EditFormCaptionStyle Wrap="False">
                                                    </EditFormCaptionStyle>
                                                    <CellStyle CssClass="gridcellleft">
                                                    </CellStyle>
                                                    <PropertiesTextEdit>
                                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="Text" SetFocusOnError="True">
                                                            <RegularExpression ErrorText="Enter Valid Amount" ValidationExpression="^(\d{0,13}\.\d{0,5}|\d{0,13})$" />
                                                            <RequiredField ErrorText="Required" IsRequired="True" />
                                                        </ValidationSettings>
                                                    </PropertiesTextEdit>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="expnd_LodgingRefNo" Caption="Reference No"
                                                    VisibleIndex="4">
                                                    <EditFormSettings Visible="True" Caption="Reference No" VisibleIndex="5" />
                                                    <EditFormCaptionStyle Wrap="False">
                                                    </EditFormCaptionStyle>
                                                    <CellStyle CssClass="gridcellleft">
                                                    </CellStyle>
                                                    <PropertiesTextEdit>
                                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="Text" SetFocusOnError="True">
                                                            <RequiredField ErrorText="Required" IsRequired="True" />
                                                        </ValidationSettings>
                                                    </PropertiesTextEdit>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewCommandColumn VisibleIndex="5">
                                                    <HeaderTemplate>
                                                        <a href="javascript:void(0);" onclick="grid.AddNewRow();"><span style="color: #000099; text-decoration: underline">Add New</span> </a>
                                                    </HeaderTemplate>
                                                    <EditButton Visible="True">
                                                    </EditButton>
                                                    <DeleteButton Visible="True">
                                                    </DeleteButton>
                                                </dxe:GridViewCommandColumn>
                                            </Columns>

                                            <Settings ShowFooter="True" ShowStatusBar="hidden" ShowTitlePanel="True" />
                                            <SettingsText PopupEditFormCaption="Add/Modify Lodging" ConfirmDelete="Confirm delete?" />
                                            <SettingsPager NumericButtonCount="20" PageSize="20" ShowSeparators="True">
                                            </SettingsPager>
                                            <SettingsEditing Mode="PopupEditForm" PopupEditFormHeight="350px" PopupEditFormHorizontalAlign="Center"
                                                PopupEditFormModal="True" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="700px"
                                                EditFormColumnCount="1" />
                                            <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True" />
                                            <Styles>
                                                <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                </Header>
                                                <LoadingPanel ImageSpacing="10px">
                                                </LoadingPanel>
                                            </Styles>
                                            <StylesEditors>
                                                <ProgressBar Height="25px">
                                                </ProgressBar>
                                            </StylesEditors>
                                            <Templates>
                                                <EditForm>
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td style="width: 25%"></td>
                                                            <td style="width: 50%">
                                                                <controls>
                                                   <dxe:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormEditors" ColumnID="" ID="Editors">
                                                   </dxe:ASPxGridViewTemplateReplacement>                                                           
                                                 </controls>
                                                                <div style="text-align: right; padding: 2px 2px 2px 2px">
                                                                    <dxe:ASPxGridViewTemplateReplacement ID="UpdateButton" ReplacementType="EditFormUpdateButton"
                                                                        runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                                    <dxe:ASPxGridViewTemplateReplacement ID="CancelButton" ReplacementType="EditFormCancelButton"
                                                                        runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                                </div>
                                                            </td>
                                                            <td style="width: 25%"></td>
                                                        </tr>
                                                    </table>
                                                </EditForm>

                                            </Templates>
                                        </dxe:ASPxGridView>
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Fooding" Text="Fooding">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Business Processing" Text="Business Processing">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Others" Text="Others">
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
                    </dxe:ASPxPageControl>
                </td>
            </tr>
            <tr>
                <td style="height: 8px">
                    <table style="width: 100%;">
                        <tr>
                            <td align="right" style="width: 843px">
                                <asp:SqlDataSource ID="SqlLodging" runat="server"
                                    SelectCommand="SELECT [expnd_internalId], [expnd_empId], [expnd_compId], [expnd_branchId], [expnd_activityId],  [expnd_currencyType],expnd_LodgingType,expnd_LodgingDateFrom,convert(varchar(11),expnd_LodgingDateFrom,113) as expnd_LodgingDateFrom1,expnd_LodgingDateTo,convert(varchar(11),expnd_LodgingDateTo,113) as expnd_LodgingDateTo1,expnd_LodgingExpnseAmount,expnd_LodgingRefNo, [CreateDate],[CreateUser], [LastModifyUser], [LastModifyDate] FROM [tbl_trans_SExpenditure] where expnd_expenceType='Lodging' and expnd_empId=@expnd_empId"
                                    InsertCommand="Expnd_SLodging_Insert" InsertCommandType="StoredProcedure" DeleteCommand="DELETE FROM [tbl_trans_SExpenditure] WHERE [expnd_internalId] = @expnd_internalId"
                                    UpdateCommand="Expnd_SLodging_Update" UpdateCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="expnd_empId" Type="string" SessionField="SalesID" />
                                    </SelectParameters>
                                    <DeleteParameters>
                                        <asp:Parameter Name="expnd_internalId" Type="Int32" />
                                    </DeleteParameters>
                                    <InsertParameters>
                                        <asp:SessionParameter Name="expnd_empId" Type="string" SessionField="SalesID" />
                                        <%-- <asp:Parameter Name="expnd_compId" Type="String" />--%>
                                        <asp:SessionParameter Name="expnd_branchId" Type="string" SessionField="userbranchID" />
                                        <%--   <asp:Parameter Name="expnd_activityId" Type="String" />--%>
                                        <asp:Parameter Name="expnd_currencyType" Type="String" />
                                        <asp:Parameter Name="expnd_LodgingType" Type="string" />
                                        <asp:Parameter Name="expnd_LodgingDateFrom" Type="datetime" />
                                        <asp:Parameter Name="expnd_LodgingDateTo" Type="datetime" />
                                        <asp:Parameter Name="expnd_LodgingExpnseAmount" Type="decimal" />
                                        <asp:Parameter Name="expnd_LodgingRefNo" Type="String" />
                                        <asp:SessionParameter Name="CreateUser" Type="string" SessionField="userid" />
                                    </InsertParameters>
                                    <UpdateParameters>
                                        <asp:Parameter Name="expnd_internalId" Type="Int32" />
                                        <asp:SessionParameter Name="expnd_empId" Type="string" SessionField="SalesID" />
                                        <%-- <asp:Parameter Name="expnd_compId" Type="String" />--%>
                                        <asp:SessionParameter Name="expnd_branchId" Type="string" SessionField="userbranchID" />
                                        <%--   <asp:Parameter Name="expnd_activityId" Type="String" />--%>
                                        <asp:Parameter Name="expnd_currencyType" Type="String" />
                                        <asp:Parameter Name="expnd_LodgingType" Type="string" />
                                        <asp:Parameter Name="expnd_LodgingDateFrom" Type="datetime" />
                                        <asp:Parameter Name="expnd_LodgingDateTo" Type="datetime" />
                                        <asp:Parameter Name="expnd_LodgingExpnseAmount" Type="decimal" />
                                        <asp:Parameter Name="expnd_LodgingRefNo" Type="String" />
                                        <asp:SessionParameter Name="CreateUser" Type="string" SessionField="userid" />
                                    </UpdateParameters>
                                </asp:SqlDataSource>
                                <asp:SqlDataSource ID="SqlCurrency" runat="server" 
                                    SelectCommand="SELECT [curr_id], [curr_symbol] FROM [tbl_currency]"></asp:SqlDataSource>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
