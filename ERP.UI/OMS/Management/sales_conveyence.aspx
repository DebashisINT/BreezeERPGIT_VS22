<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.management_sales_conveyence" Codebehind="sales_conveyence.aspx.cs" %>


<%--<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe000001" %>
<%@ Register assembly="DevExpress.Web.v15.1" namespace="DevExpress.Web" tagprefix="dx" %>--%>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />

    <script language="javascript" type="text/javascript">
        function disp_prompt(name)
    {
         if ( name == "tab0")
        {
          document.location.href="sales_total.aspx"; 
        }
        if ( name == "tab1")
        {
         //document.location.href="sales_conveyence.aspx"; 
        }
        else if ( name == "tab2")
        {
         document.location.href="sales_travelling.aspx"; 
        }
        else if ( name == "tab3")
        {
         document.location.href="sales_lodging.aspx"; 
        }
        else if ( name == "tab4")
        {
         document.location.href="sales_fooding.aspx"; 
        }
        else if ( name == "tab5")
        {
         document.location.href="sales_businessP.aspx"; 
        }
        else if ( name == "tab6")
        {
         document.location.href="sales_other.aspx"; 
        }
        
    }
    function OnCityChanged(cmbState) 
    {
        grid.GetEditor("expnd_TArea1").PerformCallback(cmbState.GetValue().toString());
    }
    function OnCityChanged1(cmbState1) 
    {
        grid.GetEditor("expnd_TArea2").PerformCallback(cmbState1.GetValue().toString());
    }
    
  function SignOff()
   {
     window.parent.SignOff()
   }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div>
            <table class="TableMain100">
                <tr>
                    <td class="EHEADER" style="text-align: center;">
                        <strong><span style="color: #000099">Conveyence</span></strong>
                    </td>
                </tr>
                <tr>
                    <td>
                        <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="1" ClientInstanceName="page"
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
                                            <dxe:ASPxGridView ID="GridConveyence" runat="server" KeyFieldName="expnd_internalId"
                                                ClientInstanceName="grid" DataSourceID="SqlConveyence" Width="100%" AutoGenerateColumns="False"
                                                OnCellEditorInitialize="GridConveyence_CellEditorInitialize" OnInitNewRow="GridConveyence_InitNewRow">
                                                <Columns>
                                                    <dxe:GridViewDataTextColumn FieldName="expnd_internalId" Visible="False" VisibleIndex="0">
                                                        <EditFormSettings Visible="False" />
                                                        <CellStyle CssClass="gridcellleft">
                                                        </CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="expnd_empId" Visible="False" VisibleIndex="0">
                                                        <EditFormSettings Visible="False" />
                                                        <CellStyle CssClass="gridcellleft">
                                                        </CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="expnd_compId" Visible="False" VisibleIndex="1">
                                                        <EditFormSettings Visible="False" />
                                                        <CellStyle CssClass="gridcellleft">
                                                        </CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="expnd_branchId" Visible="False" VisibleIndex="2">
                                                        <EditFormSettings Visible="False" />
                                                        <CellStyle CssClass="gridcellleft">
                                                        </CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="expnd_activityId" Visible="False" VisibleIndex="3">
                                                        <EditFormSettings Visible="False" />
                                                        <CellStyle CssClass="gridcellleft">
                                                        </CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn Caption="Expense Type" FieldName="expnd_expenceType"
                                                        Visible="False" VisibleIndex="4">
                                                        <EditFormSettings Visible="False" />
                                                        <CellStyle CssClass="gridcellleft">
                                                        </CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataComboBoxColumn Caption="Currency Type" FieldName="expnd_currencyType"
                                                        Visible="False" VisibleIndex="5">
                                                        <PropertiesComboBox DataSourceID="SqlCurrency" TextField="curr_symbol" ValueField="curr_id"
                                                            ValueType="System.String">
                                                        </PropertiesComboBox>
                                                        <EditFormSettings Visible="True" VisibleIndex="0" />
                                                        <EditFormCaptionStyle Wrap="False">
                                                        </EditFormCaptionStyle>
                                                        <CellStyle CssClass="gridcellleft" Wrap="False">
                                                        </CellStyle>
                                                    </dxe:GridViewDataComboBoxColumn>
                                                    <dxe:GridViewDataComboBoxColumn Caption="Mode" FieldName="expnd_convenceMode" VisibleIndex="0">
                                                        <PropertiesComboBox DisplayFormatString="d" ValueType="System.String">
                                                            <Items>
                                                                <dxe:ListEditItem Text="Bus - Public " Value="Bus - Public">
                                                                </dxe:ListEditItem>
                                                                <dxe:ListEditItem Text="Bus - Chartered" Value="Bus - Chartered">
                                                                </dxe:ListEditItem>
                                                                <dxe:ListEditItem Text="Tram " Value="Tram">
                                                                </dxe:ListEditItem>
                                                                <dxe:ListEditItem Text="Taxi " Value="Taxi">
                                                                </dxe:ListEditItem>
                                                                <dxe:ListEditItem Text="Train - Local" Value="Train - Local">
                                                                </dxe:ListEditItem>
                                                                <dxe:ListEditItem Text="Train – Long distance " Value="Train – Long distance">
                                                                </dxe:ListEditItem>
                                                                <dxe:ListEditItem Text="Private Car" Value="Private Car">
                                                                </dxe:ListEditItem>
                                                                <dxe:ListEditItem Text="Auto rickshaw" Value="Auto rickshaw">
                                                                </dxe:ListEditItem>
                                                                <dxe:ListEditItem Text="Rickshaw " Value="Rickshaw">
                                                                </dxe:ListEditItem>
                                                                <dxe:ListEditItem Text="Flight " Value="Flight">
                                                                </dxe:ListEditItem>
                                                                <dxe:ListEditItem Text="Ferry " Value="Ferry">
                                                                </dxe:ListEditItem>
                                                                <dxe:ListEditItem Text="other" Value="other">
                                                                </dxe:ListEditItem>
                                                            </Items>
                                                        </PropertiesComboBox>
                                                        <EditFormSettings Visible="True" VisibleIndex="0" />
                                                        <CellStyle CssClass="gridcellleft">
                                                        </CellStyle>
                                                        <EditFormCaptionStyle Wrap="False">
                                                        </EditFormCaptionStyle>
                                                    </dxe:GridViewDataComboBoxColumn>
                                                    <dxe:GridViewDataTextColumn Caption="Date" FieldName="expnd_dateExpenditure1" VisibleIndex="1">
                                                        <EditFormSettings Visible="False" />
                                                        <CellStyle CssClass="gridcellleft">
                                                        </CellStyle>
                                                        <PropertiesTextEdit>
                                                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="Text" SetFocusOnError="True">
                                                                <RequiredField ErrorText="Required" IsRequired="True" />
                                                            </ValidationSettings>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn Caption="From" FieldName="Scity" VisibleIndex="2">
                                                        <EditFormSettings Visible="False" />
                                                        <CellStyle CssClass="gridcellleft">
                                                        </CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn Caption="To" FieldName="Fcity" VisibleIndex="3">
                                                        <EditFormSettings Visible="False" />
                                                        <CellStyle CssClass="gridcellleft">
                                                        </CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataDateColumn Caption="Expenditure Date" FieldName="expnd_dateExpenditure"
                                                        Visible="False" VisibleIndex="1">
                                                        <PropertiesDateEdit DisplayFormatString="" EditFormat="Custom" EditFormatString="dd-MM-yyyy">
                                                        </PropertiesDateEdit>
                                                        <EditFormSettings Visible="True" VisibleIndex="0" />
                                                        <CellStyle CssClass="gridcellleft">
                                                        </CellStyle>
                                                        <EditFormCaptionStyle Wrap="False">
                                                        </EditFormCaptionStyle>
                                                    </dxe:GridViewDataDateColumn>
                                                    <dxe:GridViewDataComboBoxColumn Caption="From City" FieldName="expnd_TCity1" Visible="False"
                                                        VisibleIndex="2">
                                                        <PropertiesComboBox DataSourceID="CitySelect" EnableIncrementalFiltering="True" EnableSynchronization="False"
                                                            TextField="city_name" ValueField="city_id" ValueType="System.String">
                                                            <ClientSideEvents SelectedIndexChanged="function(s, e) { OnCityChanged(s); }" />
                                                        </PropertiesComboBox>
                                                        <EditFormSettings Visible="True" VisibleIndex="1" />
                                                        <CellStyle CssClass="gridcellleft">
                                                        </CellStyle>
                                                        <EditFormCaptionStyle Wrap="False">
                                                        </EditFormCaptionStyle>
                                                    </dxe:GridViewDataComboBoxColumn>
                                                    <dxe:GridViewDataComboBoxColumn Caption="From Area" FieldName="expnd_TArea1" Visible="False"
                                                        VisibleIndex="3">
                                                        <PropertiesComboBox DataSourceID="areaSelect" TextField="area_name" ValueField="area_id"
                                                            ValueType="System.String">
                                                        </PropertiesComboBox>
                                                        <EditFormSettings Visible="True" VisibleIndex="2" />
                                                        <CellStyle CssClass="gridcellleft">
                                                        </CellStyle>
                                                        <EditFormCaptionStyle Wrap="False">
                                                        </EditFormCaptionStyle>
                                                    </dxe:GridViewDataComboBoxColumn>
                                                    <dxe:GridViewDataComboBoxColumn Caption="To City" FieldName="expnd_TCity2" Visible="False"
                                                        VisibleIndex="4">
                                                        <PropertiesComboBox DataSourceID="CitySelect1" EnableIncrementalFiltering="True"
                                                            EnableSynchronization="False" TextField="city_name" ValueField="city_id" ValueType="System.String">
                                                            <ClientSideEvents SelectedIndexChanged="function(s, e) { OnCityChanged1(s); }" />
                                                        </PropertiesComboBox>
                                                        <EditFormSettings Visible="True" VisibleIndex="3" />
                                                        <CellStyle CssClass="gridcellleft">
                                                        </CellStyle>
                                                        <EditFormCaptionStyle Wrap="False">
                                                        </EditFormCaptionStyle>
                                                    </dxe:GridViewDataComboBoxColumn>
                                                    <dxe:GridViewDataComboBoxColumn Caption="To Area" FieldName="expnd_TArea2" Visible="False"
                                                        VisibleIndex="5">
                                                        <PropertiesComboBox DataSourceID="areaSelect1" TextField="area_name" ValueField="area_id"
                                                            ValueType="System.String">
                                                        </PropertiesComboBox>
                                                        <EditFormSettings Visible="True" VisibleIndex="4" />
                                                        <CellStyle CssClass="gridcellleft">
                                                        </CellStyle>
                                                        <EditFormCaptionStyle Wrap="False">
                                                        </EditFormCaptionStyle>
                                                    </dxe:GridViewDataComboBoxColumn>
                                                    <dxe:GridViewDataTextColumn Caption="Amount" FieldName="expnd_travExpence" VisibleIndex="4">
                                                        <EditFormSettings Visible="True" VisibleIndex="5" />
                                                        <CellStyle CssClass="gridcellleft">
                                                        </CellStyle>
                                                        <EditFormCaptionStyle Wrap="False">
                                                        </EditFormCaptionStyle>
                                                        <PropertiesTextEdit>
                                                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="Text" SetFocusOnError="True">
                                                                <RegularExpression ErrorText="Enter Valid Amount" ValidationExpression="^(\d{0,13}\.\d{0,5}|\d{0,13})$" />
                                                                <RequiredField ErrorText="Required" IsRequired="True" />
                                                            </ValidationSettings>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn Caption="Reference No" FieldName="expnd_TravRefNo"
                                                        VisibleIndex="5">
                                                        <EditFormSettings Visible="True" VisibleIndex="6" />
                                                        <CellStyle CssClass="gridcellleft">
                                                        </CellStyle>
                                                        <EditFormCaptionStyle Wrap="False">
                                                        </EditFormCaptionStyle>
                                                        <PropertiesTextEdit>
                                                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="Text" SetFocusOnError="True">
                                                                <RequiredField ErrorText="Required" IsRequired="True" />
                                                            </ValidationSettings>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn Caption="Start Journey" Visible="False" FieldName="expnd_dtJourney1"
                                                        VisibleIndex="4">
                                                        <EditFormSettings Visible="False" />
                                                        <CellStyle CssClass="gridcellleft">
                                                        </CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataDateColumn Caption="Start Date" FieldName="expnd_dtJourney" Visible="False"
                                                        VisibleIndex="1">
                                                        <PropertiesDateEdit DisplayFormatString="" EditFormat="Custom" EditFormatString="dd-MM-yyyy">
                                                        </PropertiesDateEdit>
                                                        <EditFormSettings Visible="False" VisibleIndex="0" />
                                                        <CellStyle CssClass="gridcellleft">
                                                        </CellStyle>
                                                        <EditFormCaptionStyle Wrap="False">
                                                        </EditFormCaptionStyle>
                                                    </dxe:GridViewDataDateColumn>
                                                    <dxe:GridViewDataTextColumn Caption="Return Journey" FieldName="expnd_dtRetJourney1"
                                                        Visible="False" VisibleIndex="5">
                                                        <EditFormSettings Visible="False" />
                                                        <CellStyle CssClass="gridcellleft">
                                                        </CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataDateColumn Caption="Return Date" FieldName="expnd_dtRetJourney"
                                                        Visible="False" VisibleIndex="1">
                                                        <PropertiesDateEdit DisplayFormatString="" EditFormat="Custom" EditFormatString="dd-MM-yyyy">
                                                        </PropertiesDateEdit>
                                                        <EditFormSettings Visible="False" VisibleIndex="0" />
                                                        <CellStyle CssClass="gridcellleft">
                                                        </CellStyle>
                                                        <EditFormCaptionStyle Wrap="False">
                                                        </EditFormCaptionStyle>
                                                    </dxe:GridViewDataDateColumn>
                                                    <dxe:GridViewCommandColumn VisibleIndex="6" ShowEditButton="True" ShowDeleteButton="True">
                                                        <HeaderTemplate>
                                                            <a href="javascript:void(0);" onclick="grid.AddNewRow();">
                                                                <span style="color: #000099;
                                                                                                                                text-decoration: underline">Add New</span>
                                                            </a>
                                                        </HeaderTemplate>
                                                    </dxe:GridViewCommandColumn>
                                                </Columns>
                                                <StylesEditors>
                                                    <ProgressBar Height="25px">
                                                    </ProgressBar>
                                                </StylesEditors>
                                                <Settings ShowStatusBar="Visible" ShowTitlePanel="True" />
                                                <SettingsEditing Mode="PopupEditForm" PopupEditFormHeight="350px" PopupEditFormHorizontalAlign="Center"
                                                    PopupEditFormModal="True" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="700px"
                                                    EditFormColumnCount="1" />
                                                <Styles>
                                                    <LoadingPanel ImageSpacing="10px">
                                                    </LoadingPanel>
                                                    <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                    </Header>
                                                </Styles>
                                                <SettingsText PopupEditFormCaption="Add/Modify Conveyence" ConfirmDelete="Confirm delete?" />
                                                <SettingsPager NumericButtonCount="20" PageSize="20" ShowSeparators="True">
                                                    <FirstPageButton Visible="True">
                                                    </FirstPageButton>
                                                    <LastPageButton Visible="True">
                                                    </LastPageButton>
                                                </SettingsPager>
                                                <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True" />
                                                <Templates>
                                                    <EditForm>
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td style="width: 25%">
                                                                </td>
                                                                <td style="width: 50%">
                                                                    <controls>
                                                   <dxe:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormEditors" ColumnID="" ID="Editors">
                                                   </dxe:ASPxGridViewTemplateReplacement>                                                           
                                                 </controls>
                                                                    <div style="text-align: right; padding: 2px 2px 2px 2px">
                                                                        <dxe:ASPxGridViewTemplateReplacement ID="UpdateButton" ReplacementType="EditFormUpdateButton"
                                                                            runat="server">
                                                                        </dxe:ASPxGridViewTemplateReplacement>
                                                                        <dxe:ASPxGridViewTemplateReplacement ID="CancelButton" ReplacementType="EditFormCancelButton"
                                                                            runat="server">
                                                                        </dxe:ASPxGridViewTemplateReplacement>
                                                                    </div>
                                                                </td>
                                                                <td style="width: 25%">
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </EditForm>
                                                    <TitlePanel>
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td align="center" style="width: 50%">
                                                                    <span style="color: White; "">Add/Modify Conveyence.</span>
                                                                </td>
                                                                <%-- <td align="right">
                                              <table >
                                                <tr>                                                 
                                                  <td>
                                                    <dxe:ASPxButton ID="ASPxButton1" runat="server" Text="ADD" ToolTip="Add New Data"   Height="18px" Width="88px" AutoPostBack="False" Font-Size="12px">
                                                       <clientsideevents click="function(s, e) {grid.AddNewRow();}" />
                                                    </dxe:ASPxButton>
                                                  </td>                                                                                
                                                </tr>
                                             </table>
                                          </td>   --%>
                                                            </tr>
                                                        </table>
                                                    </TitlePanel>
                                                </Templates>
                                            </dxe:ASPxGridView>
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
                                    <asp:SqlDataSource ID="SqlConveyence" runat="server" 
                                        
                                        DeleteCommand="DELETE FROM [tbl_trans_salesExpenditure] WHERE [expnd_internalId] = @expnd_internalId"
                                        InsertCommand="Expnd_Convenyence_Insert" InsertCommandType="StoredProcedure"
                                        SelectCommand="SELECT [expnd_internalId], [expnd_empId], [expnd_compId], [expnd_branchId], [expnd_activityId], [expnd_expenceType], [expnd_currencyType],expnd_dateExpenditure, convert(varchar(11),expnd_dateExpenditure,113) as expnd_dateExpenditure1, [expnd_TCity1], [expnd_TArea1],((select city_name from tbl_master_city where city_id=tbl_trans_salesExpenditure.expnd_TCity1)+'-'+(select area_name from tbl_master_area where area_id=tbl_trans_salesExpenditure.expnd_TArea1)) as Scity,((select city_name from tbl_master_city where city_id=tbl_trans_salesExpenditure.expnd_TCity2)+'-'+isnull((select area_name from tbl_master_area where area_id=tbl_trans_salesExpenditure.expnd_TArea2),' ')) as Fcity,[expnd_TCity2], [expnd_TArea2],[expnd_travExpence], [expnd_TravRefNo],[expnd_convenceMode],expnd_dtJourney,convert(varchar(11),expnd_dtJourney,113) as expnd_dtJourney1,expnd_dtRetJourney,convert(varchar(11),expnd_dtRetJourney,113) as expnd_dtRetJourney1, [CreateDate],[CreateUser], [LastModifyUser], [LastModifyDate] FROM [tbl_trans_salesExpenditure] where expnd_expenceType='Convenyence' and expnd_empId=@expnd_empId"
                                        UpdateCommand="Expnd_Convenyence_Update" UpdateCommandType="StoredProcedure">
                                        <DeleteParameters>
                                            <asp:Parameter Name="expnd_internalId" Type="Int32" />
                                        </DeleteParameters>
                                        <SelectParameters>
                                            <asp:SessionParameter Name="expnd_empId" Type="string" SessionField="SalesVisitID" />
                                        </SelectParameters>
                                        <UpdateParameters>
                                            <asp:Parameter Name="expnd_internalId" Type="Int32" />
                                            <asp:SessionParameter Name="expnd_empId" Type="string" SessionField="SalesVisitID" />
                                            <asp:Parameter Name="expnd_compId" Type="String" />
                                            <asp:SessionParameter Name="expnd_branchId" Type="string" SessionField="userbranchID" />
                                            <asp:Parameter Name="expnd_activityId" Type="String" />
                                            <asp:Parameter Name="expnd_currencyType" Type="String" />
                                            <asp:Parameter Name="expnd_dateExpenditure" Type="datetime" />
                                            <asp:Parameter Name="expnd_TCity1" Type="int32" />
                                            <asp:Parameter Name="expnd_TArea1" Type="int32" />
                                            <asp:Parameter Name="expnd_TCity2" Type="int32" />
                                            <asp:Parameter Name="expnd_TArea2" Type="int32" />
                                            <asp:Parameter Name="expnd_travExpence" Type="Double" />
                                            <asp:Parameter Name="expnd_TravRefNo" Type="String" />
                                            <asp:Parameter Name="expnd_convenceMode" Type="String" />
                                            <asp:Parameter Name="expnd_dtJourney" Type="datetime" />
                                            <asp:Parameter Name="expnd_dtRetJourney" Type="datetime" />
                                            <asp:SessionParameter Name="LastModifyUser" Type="string" SessionField="userid" />
                                        </UpdateParameters>
                                        <InsertParameters>
                                            <asp:SessionParameter Name="expnd_empId" Type="string" SessionField="SalesVisitID" />
                                            <%-- <asp:Parameter Name="expnd_compId" Type="String" />--%>
                                            <asp:SessionParameter Name="expnd_branchId" Type="string" SessionField="userbranchID" />
                                            <%--   <asp:Parameter Name="expnd_activityId" Type="String" />--%>
                                            <asp:Parameter Name="expnd_currencyType" Type="String" />
                                            <asp:Parameter Name="expnd_dateExpenditure" Type="datetime" />
                                            <asp:Parameter Name="expnd_TCity1" Type="int32" />
                                            <asp:Parameter Name="expnd_TArea1" Type="int32" />
                                            <asp:Parameter Name="expnd_TCity2" Type="int32" />
                                            <asp:Parameter Name="expnd_TArea2" Type="int32" />
                                            <asp:Parameter Name="expnd_travExpence" Type="Double" />
                                            <asp:Parameter Name="expnd_TravRefNo" Type="String" />
                                            <asp:Parameter Name="expnd_convenceMode" Type="String" />
                                            <asp:Parameter Name="expnd_dtJourney" Type="datetime" />
                                            <asp:Parameter Name="expnd_dtRetJourney" Type="datetime" />
                                            <asp:SessionParameter Name="CreateUser" Type="string" SessionField="userid" />
                                        </InsertParameters>
                                    </asp:SqlDataSource>
                                    <asp:SqlDataSource ID="CitySelect" runat="server" 
                                        SelectCommand="SELECT [city_id], [city_name] FROM [tbl_master_city]"></asp:SqlDataSource>
                                    <asp:SqlDataSource ID="areaSelect" runat="server" 
                                        SelectCommand="SELECT [area_id], [area_name] FROM [tbl_master_area] where city_id=@cityid">
                                        <SelectParameters>
                                            <asp:Parameter Name="cityid" Type="string" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                    <asp:SqlDataSource ID="CitySelect1" runat="server" 
                                        SelectCommand="SELECT [city_id], [city_name] FROM [tbl_master_city]"></asp:SqlDataSource>
                                    <asp:SqlDataSource ID="areaSelect1" runat="server" 
                                        SelectCommand="SELECT [area_id], [area_name] FROM [tbl_master_area] where city_id=@cityid">
                                        <SelectParameters>
                                            <asp:Parameter Name="cityid" Type="string" />
                                        </SelectParameters>
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
