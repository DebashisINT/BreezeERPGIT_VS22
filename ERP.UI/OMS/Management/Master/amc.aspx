<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.Master.management_master_amc" CodeBehind="amc.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%----%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div>

        <script type="text/javascript">
            //function SignOff() {
            //    window.parent.SignOff()
            //}
            //function height() {
            //    if (document.body.scrollHeight <= 500)
            //        window.frameElement.height = '500px';
            //    else
            //        window.frameElement.height = document.body.scrollHeight;
            //    window.frameElement.Width = document.body.scrollWidth;
            //}
            //function is called on changing country
            function OnCountryChanged(cmbCountry) {
                Address.GetEditor("State").PerformCallback(cmbCountry.GetValue().toString());
            }
            function OnStateChanged(cmbState) {
                Address.GetEditor("City").PerformCallback(cmbState.GetValue().toString());
            }
            function ShowHideFilter(obj) {
                grid.PerformCallback(obj);
            }

            function OnContactInfoClick(keyValue, CompName) {
                var url = 'insurance_contactPerson.aspx?id=' + keyValue + '&comp=' + CompName;
                OnMoreInfoClick(url, "AMC Name : " + CompName + "", '940px', '450px', "Y");
            }

            function OnPhoneClick() {
                if (PhoneFax.GetEditor('phf_phoneNumber').GetValue() == null) {
                    alert('Phone Number Required');
                }
                else {
                    PhoneFax.UpdateEdit();
                }
            }
            function OnEmailClick() {
                if (EmailAdd.GetEditor('eml_type').GetValue() == 'Web Site') {
                    if (EmailAdd.GetEditor('eml_website').GetValue() == null)
                        alert('Url Required');
                    else
                        EmailAdd.UpdateEdit();
                }
                else {
                    if (EmailAdd.GetEditor('eml_email').GetValue() == null)
                        alert('Email Required');
                    else
                        EmailAdd.UpdateEdit();
                }
            }
        </script>

        <table class="TableMain100">
            <tr>
                <td class="EHEADER" align="center">
                    <strong><span style="color: #000099">AMC</span></strong></td>
            </tr>
            <tr>
                <td>
                    <table width="100%">
                        <tr>
                            <td style="text-align: left; vertical-align: top">
                                <table>
                                    <tr>
                                        <td id="ShowFilter">
                                            <a href="javascript:ShowHideFilter('s');"><span style="color: #000099; text-decoration: underline">Show Filter</span></a>
                                        </td>
                                        <td id="Td1">
                                            <a href="javascript:ShowHideFilter('All');"><span style="color: #000099; text-decoration: underline">All Records</span></a>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td></td>
                            <td class="gridcellright">
                                <dxe:ASPxComboBox ID="cmbExport" runat="server" AutoPostBack="true" BackColor="Navy"
                                    Font-Bold="False" ForeColor="White" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged"
                                    ValueType="System.Int32" Width="130px">
                                    <Items>
                                        <dxe:ListEditItem Text="Select" Value="0" />
                                        <dxe:ListEditItem Text="PDF" Value="1" />
                                        <dxe:ListEditItem Text="XLS" Value="2" />
                                        <dxe:ListEditItem Text="RTF" Value="3" />
                                        <dxe:ListEditItem Text="CSV" Value="4" />
                                    </Items>
                                    <ButtonStyle BackColor="#C0C0FF" ForeColor="Black">
                                    </ButtonStyle>
                                    <ItemStyle BackColor="Navy" ForeColor="White">
                                        <HoverStyle BackColor="#8080FF" ForeColor="White">
                                        </HoverStyle>
                                    </ItemStyle>
                                    <Border BorderColor="White" />
                                    <DropDownButton Text="Export">
                                    </DropDownButton>
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <dxe:ASPxGridView ID="amcGrid" runat="server" AutoGenerateColumns="False" ClientInstanceName="grid"
                        DataSourceID="amc" KeyFieldName="amc_amcCode" Width="100%" OnHtmlRowCreated="amcGrid_HtmlRowCreated"
                        OnHtmlEditFormCreated="amcGrid_HtmlEditFormCreated" OnCustomCallback="amcGrid_CustomCallback">
                        <Columns>
                            <dxe:GridViewDataTextColumn Visible="False" ReadOnly="True" VisibleIndex="0" FieldName="amc_id">
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="0" FieldName="amc_amcCode">
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="0" FieldName="amc_nameOfMutualFund" Width="80%"
                                Caption="AMCName">
                                <EditFormSettings Visible="True" Caption="Mutual Fund Name"></EditFormSettings>
                                <PropertiesTextEdit Width="300px">
                                    <ValidationSettings ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom" SetFocusOnError="True">
                                        <RequiredField ErrorText="Please Enter Mutual Fund name" IsRequired="True" />
                                    </ValidationSettings>
                                    <Style HorizontalAlign="Left" Wrap="False"></Style>
                                </PropertiesTextEdit>
                                <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                </EditCellStyle>
                                <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right" VerticalAlign="Top">
                                </EditFormCaptionStyle>
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="1" FieldName="amc_sebiRegnNo"
                                Caption="Sebi Regn. No">
                                <EditFormSettings Visible="True"></EditFormSettings>
                                <PropertiesTextEdit Width="300px">
                                    <ValidationSettings ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom" SetFocusOnError="True">
                                        <RequiredField ErrorText="Please Enter Sebi. Regn. No" IsRequired="True" />
                                    </ValidationSettings>
                                </PropertiesTextEdit>
                                <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                </EditCellStyle>
                                <CellStyle HorizontalAlign="Left" Wrap="False">
                                </CellStyle>
                                <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right" VerticalAlign="Top">
                                </EditFormCaptionStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataDateColumn Visible="False" VisibleIndex="1" FieldName="amc_dateOfSetupOfMutualFund"
                                Caption="Date Of Setup Of MF">
                                <PropertiesDateEdit DisplayFormatString="" Width="300px">
                                </PropertiesDateEdit>
                                <EditFormSettings Visible="True"></EditFormSettings>
                                <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                </EditCellStyle>
                                <CellStyle Wrap="False">
                                </CellStyle>
                                <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right" VerticalAlign="Top">
                                </EditFormCaptionStyle>
                            </dxe:GridViewDataDateColumn>
                            <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="1" FieldName="amc_namesOfSponsors"
                                Caption="Name Of Sponcers">
                                <EditFormSettings Visible="True"></EditFormSettings>
                                <PropertiesTextEdit Width="300px">
                                    <ValidationSettings ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom">
                                        <RequiredField ErrorText="" />
                                    </ValidationSettings>
                                </PropertiesTextEdit>
                                <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                </EditCellStyle>
                                <CellStyle Wrap="False">
                                </CellStyle>
                                <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right" VerticalAlign="Top">
                                </EditFormCaptionStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="1" FieldName="amc_nameOfTrusteeCompany"
                                Caption="Name Of Transteet Co.">
                                <EditFormSettings Visible="True"></EditFormSettings>
                                <PropertiesTextEdit Width="300px">
                                    <ValidationSettings ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom">
                                        <RequiredField ErrorText="" />
                                    </ValidationSettings>
                                </PropertiesTextEdit>
                                <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                </EditCellStyle>
                                <CellStyle Wrap="False">
                                </CellStyle>
                                <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right" VerticalAlign="Top">
                                </EditFormCaptionStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="1" FieldName="amc_nameOfAMC"
                                Caption="Name Of AMC">
                                <EditFormSettings Visible="True"></EditFormSettings>
                                <PropertiesTextEdit Width="300px">
                                    <ValidationSettings ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom">
                                        <RequiredField ErrorText="" />
                                    </ValidationSettings>
                                </PropertiesTextEdit>
                                <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                </EditCellStyle>
                                <CellStyle Wrap="False">
                                </CellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                </EditFormCaptionStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataDateColumn Visible="False" VisibleIndex="1" FieldName="amc_dateOfIncoOfAMC"
                                Caption="AMC Incorporation Date">
                                <PropertiesDateEdit DisplayFormatString="" Width="300px">
                                </PropertiesDateEdit>
                                <EditFormSettings Visible="True"></EditFormSettings>
                                <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                </EditCellStyle>
                                <CellStyle Wrap="False">
                                </CellStyle>
                                <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right" VerticalAlign="Top">
                                </EditFormCaptionStyle>
                            </dxe:GridViewDataDateColumn>
                            <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="1" FieldName="amc_nameOfDirectors"
                                Caption="Name Of Directors">
                                <EditFormSettings Visible="True"></EditFormSettings>
                                <PropertiesTextEdit Width="300px">
                                    <ValidationSettings ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom">
                                        <RequiredField ErrorText="" />
                                    </ValidationSettings>
                                </PropertiesTextEdit>
                                <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                </EditCellStyle>
                                <CellStyle Wrap="False">
                                </CellStyle>
                                <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right" VerticalAlign="Top">
                                </EditFormCaptionStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="1" FieldName="amc_nameOfHeadOfOperation"
                                Caption="Head Of Operation">
                                <EditFormSettings Visible="True"></EditFormSettings>
                                <PropertiesTextEdit Width="300px">
                                    <ValidationSettings ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom">
                                        <RequiredField ErrorText="" />
                                    </ValidationSettings>
                                </PropertiesTextEdit>
                                <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                </EditCellStyle>
                                <CellStyle Wrap="False">
                                </CellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                </EditFormCaptionStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewCommandColumn VisibleIndex="1" ShowDeleteButton="True" ShowEditButton="True">
                                <HeaderStyle HorizontalAlign="Center"/>
                                <HeaderTemplate>
                                    <a href="javascript:void(0);" onclick="grid.AddNewRow()">
                                        <span style="color: #000099; text-decoration: underline">Add New</span>
                                    </a>
                                </HeaderTemplate>
                            </dxe:GridViewCommandColumn>
                            <dxe:GridViewDataTextColumn Caption="Cont.Person" VisibleIndex="2">
                                <DataItemTemplate>
                                    <a href="javascript:void(0);" onclick="OnContactInfoClick('<%# Container.KeyValue %>','<%#Eval("amc_nameOfMutualFund") %>')">Show</a>
                                </DataItemTemplate>
                                <CellStyle HorizontalAlign="Left" Wrap="False">
                                </CellStyle>
                                <EditFormSettings Visible="False" />
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                        <Styles>
                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                            </Header>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                        </Styles>
                        <Settings ShowStatusBar="Visible"></Settings>
                        <SettingsText PopupEditFormCaption="Add/Modify AMC" />
                        <SettingsPager NumericButtonCount="20" PageSize="20" AlwaysShowPager="True" ShowSeparators="True">
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </SettingsPager>
                        <SettingsBehavior AllowFocusedRow="True" ConfirmDelete="True" />
                        <Templates>
                            <EditForm>
                                <div style="padding: 4px 4px 3px 4px">
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="width: 10%"></td>
                                            <td style="width: 80%">
                                                <dxe:ASPxPageControl runat="server" ID="pageControl" Width="100%" ActiveTabIndex="0">
                                                    <TabPages>
                                                        <dxe:TabPage Text="General">
                                                            <ContentCollection>
                                                                <dxe:ContentControl runat="server">
                                                                    <table width="100%">
                                                                        <tr>
                                                                            <td style="width: 10%">&nbsp;</td>
                                                                            <td style="width: 80%" align="center">
                                                                                <dxe:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormEditors"
                                                                                    ColumnID="" ID="Editors"></dxe:ASPxGridViewTemplateReplacement>
                                                                            </td>
                                                                            <td style="width: 10%">&nbsp;</td>
                                                                        </tr>
                                                                    </table>
                                                                    <div style="text-align: right; padding: 2px 2px 2px 2px">
                                                                        <dxe:ASPxGridViewTemplateReplacement ID="UpdateButton1" ReplacementType="EditFormUpdateButton"
                                                                            runat="server" ColumnID=""></dxe:ASPxGridViewTemplateReplacement>
                                                                        <dxe:ASPxGridViewTemplateReplacement ID="ASPxGridViewTemplateReplacement2" ReplacementType="EditFormCancelButton"
                                                                            runat="server" ColumnID=""></dxe:ASPxGridViewTemplateReplacement>
                                                                    </div>
                                                                </dxe:ContentControl>
                                                            </ContentCollection>
                                                        </dxe:TabPage>
                                                        <dxe:TabPage Text="Correspondance">
                                                            <ContentCollection>
                                                                <dxe:ContentControl runat="server">
                                                                    <dxe:ASPxPageControl ID="ASPxPageControl2" runat="server" ActiveTabIndex="0" ClientInstanceName="page">
                                                                        <TabPages>
                                                                            <dxe:TabPage Text="Adress">
                                                                                <ContentCollection>
                                                                                    <dxe:ContentControl runat="server">
                                                                                        <dxe:ASPxGridView runat="server" Width="100%" ID="AddressGrid" ClientInstanceName="Address"
                                                                                            DataSourceID="amcAddress" KeyFieldName="add_id" AutoGenerateColumns="False" OnBeforePerformDataSelect="ASPxGridView1_BeforePerformDataSelect"
                                                                                            OnCellEditorInitialize="AddressGrid_CellEditorInitialize">
                                                                                            <SettingsEditing EditFormColumnCount="1" PopupEditFormHorizontalAlign="Center" PopupEditFormModal="True"
                                                                                                PopupEditFormVerticalAlign="bottomsides" PopupEditFormWidth="700px" Mode="PopupEditForm">
                                                                                            </SettingsEditing>
                                                                                            <SettingsText PopupEditFormCaption="Add/Modify  Address"></SettingsText>
                                                                                            <Styles>
                                                                                                <LoadingPanel ImageSpacing="10px">
                                                                                                </LoadingPanel>
                                                                                                <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                                                                </Header>
                                                                                            </Styles>
                                                                                            <Settings ShowStatusBar="Visible" ShowTitlePanel="True"></Settings>
                                                                                            <SettingsBehavior ConfirmDelete="True" ColumnResizeMode="NextColumn"></SettingsBehavior>
                                                                                            <SettingsPager PageSize="20" NumericButtonCount="20">
                                                                                            </SettingsPager>
                                                                                            <Columns>
                                                                                                <dxe:GridViewDataTextColumn Visible="False" ReadOnly="True" VisibleIndex="0" FieldName="add_id">
                                                                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                                                                </dxe:GridViewDataTextColumn>
                                                                                                <dxe:GridViewDataTextColumn Visible="False" ReadOnly="True" VisibleIndex="0" FieldName="add_cntId">
                                                                                                </dxe:GridViewDataTextColumn>
                                                                                                <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="0" FieldName="add_entity">
                                                                                                </dxe:GridViewDataTextColumn>
                                                                                                <dxe:GridViewDataComboBoxColumn FieldName="add_addressType" Caption="Type" VisibleIndex="0">
                                                                                                    <PropertiesComboBox ValueType="System.String" EnableSynchronization="False" EnableIncrementalFiltering="True"
                                                                                                        Width="300px">
                                                                                                        <Items>
                                                                                                            <dxe:ListEditItem Text="N/A" Value="N/A"></dxe:ListEditItem>
                                                                                                            <dxe:ListEditItem Text="Residence" Value="Residence"></dxe:ListEditItem>
                                                                                                            <dxe:ListEditItem Text="Office" Value="Office"></dxe:ListEditItem>
                                                                                                            <dxe:ListEditItem Text="Emergence" Value="Emergence"></dxe:ListEditItem>
                                                                                                            <dxe:ListEditItem Text="Correspondance" Value="Correspondance"></dxe:ListEditItem>
                                                                                                            <dxe:ListEditItem Text="Permanent" Value="Permanent"></dxe:ListEditItem>
                                                                                                            <dxe:ListEditItem Text="Contact Person" Value="Contact Person"></dxe:ListEditItem>
                                                                                                        </Items>
                                                                                                    </PropertiesComboBox>
                                                                                                    <EditFormSettings Visible="True" Caption="Address Type"></EditFormSettings>
                                                                                                    <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                                                                                    </EditCellStyle>
                                                                                                    <CellStyle Wrap="False">
                                                                                                    </CellStyle>
                                                                                                    <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right">
                                                                                                    </EditFormCaptionStyle>
                                                                                                </dxe:GridViewDataComboBoxColumn>
                                                                                                <dxe:GridViewDataMemoColumn Caption="Address1" FieldName="add_address1" VisibleIndex="1"
                                                                                                    Width="15%">
                                                                                                    <PropertiesMemoEdit Width="300px">
                                                                                                        <ValidationSettings ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom" SetFocusOnError="True">
                                                                                                            <RequiredField ErrorText="Please Enter Address1 Field" IsRequired="True" />
                                                                                                        </ValidationSettings>
                                                                                                    </PropertiesMemoEdit>
                                                                                                    <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                                                                                    </EditCellStyle>
                                                                                                    <CellStyle Wrap="False">
                                                                                                    </CellStyle>
                                                                                                    <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right">
                                                                                                    </EditFormCaptionStyle>
                                                                                                    <EditFormSettings Visible="True" />
                                                                                                </dxe:GridViewDataMemoColumn>
                                                                                                <dxe:GridViewDataMemoColumn Caption="Address2" FieldName="add_address2" VisibleIndex="2"
                                                                                                    Width="15%">
                                                                                                    <PropertiesMemoEdit Width="300px">
                                                                                                    </PropertiesMemoEdit>
                                                                                                    <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                                                                                    </EditCellStyle>
                                                                                                    <CellStyle Wrap="False">
                                                                                                    </CellStyle>
                                                                                                    <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right">
                                                                                                    </EditFormCaptionStyle>
                                                                                                    <EditFormSettings Visible="True" />
                                                                                                </dxe:GridViewDataMemoColumn>
                                                                                                <dxe:GridViewDataMemoColumn Caption="Address3" FieldName="add_address3" VisibleIndex="3">
                                                                                                    <PropertiesMemoEdit Width="300px">
                                                                                                    </PropertiesMemoEdit>
                                                                                                    <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                                                                                    </EditCellStyle>
                                                                                                    <CellStyle Wrap="False">
                                                                                                    </CellStyle>
                                                                                                    <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right">
                                                                                                    </EditFormCaptionStyle>
                                                                                                    <EditFormSettings Visible="True" />
                                                                                                </dxe:GridViewDataMemoColumn>
                                                                                                <dxe:GridViewDataComboBoxColumn Caption="Country" FieldName="Country" VisibleIndex="4"
                                                                                                    Visible="False">
                                                                                                    <PropertiesComboBox DataSourceID="CountrySelect" TextField="Country" ValueField="cou_id"
                                                                                                        EnableSynchronization="False" EnableIncrementalFiltering="True" ValueType="System.String"
                                                                                                        Width="300px">
                                                                                                        <ClientSideEvents SelectedIndexChanged="function(s, e) { OnCountryChanged(s); }"></ClientSideEvents>
                                                                                                    </PropertiesComboBox>
                                                                                                    <CellStyle CssClass="gridcellleft" Wrap="False">
                                                                                                    </CellStyle>
                                                                                                    <EditFormSettings Visible="True" />
                                                                                                    <EditFormCaptionStyle VerticalAlign="Top" Wrap="False" HorizontalAlign="Right">
                                                                                                    </EditFormCaptionStyle>
                                                                                                    <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                                                                                    </EditCellStyle>
                                                                                                </dxe:GridViewDataComboBoxColumn>
                                                                                                <dxe:GridViewDataComboBoxColumn Caption="State" FieldName="State" VisibleIndex="5"
                                                                                                    Visible="False">
                                                                                                    <PropertiesComboBox DataSourceID="StateSelect" TextField="State" ValueField="ID"
                                                                                                        EnableSynchronization="False" EnableIncrementalFiltering="True" ValueType="System.String"
                                                                                                        Width="300px">
                                                                                                        <ClientSideEvents SelectedIndexChanged="function(s, e) { OnStateChanged(s); }"></ClientSideEvents>
                                                                                                    </PropertiesComboBox>
                                                                                                    <CellStyle CssClass="gridcellleft" Wrap="False">
                                                                                                    </CellStyle>
                                                                                                    <EditFormSettings Visible="True" />
                                                                                                    <EditFormCaptionStyle VerticalAlign="Top" Wrap="False" HorizontalAlign="Right">
                                                                                                    </EditFormCaptionStyle>
                                                                                                    <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                                                                                    </EditCellStyle>
                                                                                                </dxe:GridViewDataComboBoxColumn>
                                                                                                <dxe:GridViewDataComboBoxColumn Caption="City" FieldName="City" VisibleIndex="6"
                                                                                                    Visible="False">
                                                                                                    <PropertiesComboBox DataSourceID="SelectCity" TextField="City" ValueField="CityId"
                                                                                                        EnableSynchronization="False" EnableIncrementalFiltering="True" ValueType="System.String">
                                                                                                        <ValidationSettings ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom" SetFocusOnError="True">
                                                                                                            <RequiredField ErrorText="City Can Not Be Blank" IsRequired="True" />
                                                                                                        </ValidationSettings>
                                                                                                    </PropertiesComboBox>
                                                                                                    <CellStyle CssClass="gridcellleft">
                                                                                                    </CellStyle>
                                                                                                    <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                                                    </EditFormCaptionStyle>
                                                                                                    <EditFormSettings Visible="True" />
                                                                                                </dxe:GridViewDataComboBoxColumn>
                                                                                                <dxe:GridViewDataTextColumn FieldName="Country1" VisibleIndex="4" Caption="Country">
                                                                                                    <EditFormSettings Visible="False" />
                                                                                                    <CellStyle CssClass="gridcellleft">
                                                                                                    </CellStyle>
                                                                                                    <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                                                    </EditFormCaptionStyle>
                                                                                                </dxe:GridViewDataTextColumn>
                                                                                                <dxe:GridViewDataTextColumn FieldName="State1" VisibleIndex="5" Caption="State">
                                                                                                    <EditFormSettings Visible="False" />
                                                                                                    <CellStyle CssClass="gridcellleft">
                                                                                                    </CellStyle>
                                                                                                    <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                                                    </EditFormCaptionStyle>
                                                                                                </dxe:GridViewDataTextColumn>
                                                                                                <dxe:GridViewDataTextColumn FieldName="City1" VisibleIndex="6" Caption="City">
                                                                                                    <EditFormSettings Visible="False" />
                                                                                                    <CellStyle CssClass="gridcellleft">
                                                                                                    </CellStyle>
                                                                                                    <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                                                    </EditFormCaptionStyle>
                                                                                                </dxe:GridViewDataTextColumn>
                                                                                                <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="add_pin" Caption="Pin/Zip">
                                                                                                    <PropertiesTextEdit Width="300px">
                                                                                                        <ValidationSettings SetFocusOnError="True" ErrorTextPosition="Bottom" ErrorDisplayMode="ImageWithText">
                                                                                                            <RequiredField IsRequired="True" ErrorText="Please Enter Pin No"></RequiredField>
                                                                                                            <RegularExpression ValidationExpression="[0-9]{6}" ErrorText="Please Enter Number" />
                                                                                                        </ValidationSettings>
                                                                                                    </PropertiesTextEdit>
                                                                                                    <EditFormSettings Visible="True"></EditFormSettings>
                                                                                                    <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                                                                                    </EditCellStyle>
                                                                                                    <CellStyle Wrap="False">
                                                                                                    </CellStyle>
                                                                                                    <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right" VerticalAlign="Top">
                                                                                                    </EditFormCaptionStyle>
                                                                                                </dxe:GridViewDataTextColumn>
                                                                                                <dxe:GridViewDataTextColumn VisibleIndex="8" FieldName="add_landMark" Caption="Land Mark">
                                                                                                    <EditFormSettings Visible="True"></EditFormSettings>
                                                                                                    <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                                                                                    </EditCellStyle>
                                                                                                    <CellStyle Wrap="False">
                                                                                                    </CellStyle>
                                                                                                    <PropertiesTextEdit Width="300px">
                                                                                                    </PropertiesTextEdit>
                                                                                                    <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right">
                                                                                                    </EditFormCaptionStyle>
                                                                                                </dxe:GridViewDataTextColumn>
                                                                                                <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="5" FieldName="add_activityId">
                                                                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                                                                </dxe:GridViewDataTextColumn>
                                                                                                <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="5" FieldName="CreateDate">
                                                                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                                                                </dxe:GridViewDataTextColumn>
                                                                                                <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="5" FieldName="CreateUser">
                                                                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                                                                </dxe:GridViewDataTextColumn>
                                                                                                <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="5" FieldName="LastModifyDate">
                                                                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                                                                </dxe:GridViewDataTextColumn>
                                                                                                <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="5" FieldName="LastModifyUser">
                                                                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                                                                </dxe:GridViewDataTextColumn>
                                                                                                <dxe:GridViewCommandColumn VisibleIndex="9" ShowDeleteButton="True" ShowEditButton="True">
                                                                                                    <HeaderTemplate>
                                                                                                        <%if (Session["PageAccess"].ToString().Trim() == "All" || Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "DelAdd")
                                                                                                          { %>
                                                                                                        <a href="javascript:void(0);" onclick="Address.AddNewRow();">
                                                                                                            <span style="color: #000099; text-decoration: underline">Add New</span>
                                                                                                        </a>
                                                                                                        <%} %>
                                                                                                    </HeaderTemplate>
                                                                                                </dxe:GridViewCommandColumn>
                                                                                            </Columns>
                                                                                            <Templates>
                                                                                                <EditForm>
                                                                                                    <table style="width: 100%">
                                                                                                        <tr>
                                                                                                            <td style="width: 30%"></td>
                                                                                                            <td style="width: 40%">
                                                                                                                <controls>
                                    <dxe:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormEditors" ID="AddressForm"></dxe:ASPxGridViewTemplateReplacement>
                                </controls>
                                                                                                                <div style="text-align: right; padding: 2px 2px 2px 2px">
                                                                                                                    <dxe:ASPxGridViewTemplateReplacement ID="UpdateButton2" ReplacementType="EditFormUpdateButton"
                                                                                                                        runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                                                                                    <dxe:ASPxGridViewTemplateReplacement ID="CancelButton2" ReplacementType="EditFormCancelButton"
                                                                                                                        runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                                                                                </div>
                                                                                                            </td>
                                                                                                            <td style="width: 30%"></td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </EditForm>
                                                                                                <TitlePanel>
                                                                                                    <%--<table style="width:100%">
                    <tr>
                         <td align="right">
                            <table width="200">
                                <tr>
                                    <td>
                                    <%if (Session["PageAccess"].ToString().Trim() == "All" || Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "DelAdd")
                                      { %>
                                        <dxe:ASPxButton ID="ASPxButton1" runat="server" Text="ADD" ToolTip="Add New Data"   Height="18px" Width="88px"  AutoPostBack="False" >
                                            <clientsideevents click="function(s, e) {Address.AddNewRow();}" />
                                        </dxe:ASPxButton>
                                        <%} %>
                                    </td>
                                                                        
                                     
                                  </tr>
                              </table>
                          </td>   
                     </tr>
                </table>--%>
                                                                                                </TitlePanel>
                                                                                            </Templates>
                                                                                        </dxe:ASPxGridView>
                                                                                    </dxe:ContentControl>
                                                                                </ContentCollection>
                                                                            </dxe:TabPage>
                                                                            <dxe:TabPage Text="Phone">
                                                                                <ContentCollection>
                                                                                    <dxe:ContentControl runat="server">
                                                                                        <dxe:ASPxGridView runat="server" Width="100%" ID="phoneGrid" ClientInstanceName="PhoneFax"
                                                                                            DataSourceID="emcPhone" KeyFieldName="phf_id" AutoGenerateColumns="False" OnBeforePerformDataSelect="ASPxGridView1_BeforePerformDataSelect" OnRowValidating="PhoneGrid_RowValidating">
                                                                                            <SettingsEditing EditFormColumnCount="1" PopupEditFormHorizontalAlign="Center" PopupEditFormModal="True"
                                                                                                PopupEditFormVerticalAlign="bottomsides" PopupEditFormWidth="600px" Mode="PopupEditForm">
                                                                                            </SettingsEditing>
                                                                                            <SettingsText PopupEditFormCaption="Add/Modify Phone"></SettingsText>
                                                                                            <Styles>
                                                                                                <LoadingPanel ImageSpacing="10px">
                                                                                                </LoadingPanel>
                                                                                                <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                                                                </Header>
                                                                                            </Styles>
                                                                                            <Settings ShowStatusBar="Visible" ShowTitlePanel="True"></Settings>
                                                                                            <SettingsBehavior ConfirmDelete="True" ColumnResizeMode="NextColumn"></SettingsBehavior>
                                                                                            <SettingsPager PageSize="20" NumericButtonCount="20">
                                                                                            </SettingsPager>
                                                                                            <Columns>
                                                                                                <dxe:GridViewDataTextColumn Visible="False" ReadOnly="True" VisibleIndex="1" FieldName="phf_id">
                                                                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                                                                </dxe:GridViewDataTextColumn>
                                                                                                <dxe:GridViewDataTextColumn Visible="False" ReadOnly="True" VisibleIndex="1" FieldName="phf_cntId">
                                                                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                                                                </dxe:GridViewDataTextColumn>
                                                                                                <dxe:GridViewDataTextColumn Visible="False" ReadOnly="True" VisibleIndex="1" FieldName="phf_entity">
                                                                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                                                                </dxe:GridViewDataTextColumn>
                                                                                                <dxe:GridViewDataComboBoxColumn FieldName="phf_type" Caption="Type" VisibleIndex="0">
                                                                                                    <PropertiesComboBox ValueType="System.String" EnableSynchronization="False" EnableIncrementalFiltering="True"
                                                                                                        Width="300px">
                                                                                                        <ClientSideEvents SelectedIndexChanged="function(s, e) {
	var value = s.GetValue();
    if(value == &quot;Mobile&quot;)
    {
         PhoneFax.GetEditor(&quot;phf_countryCode&quot;).SetVisible(false);
         PhoneFax.GetEditor(&quot;phf_areaCode&quot;).SetVisible(false);
         PhoneFax.GetEditor(&quot;phf_extension&quot;).SetVisible(false);
         PhoneFax.GetEditor(&quot;phf_extension&quot;).SetVisible(false);
    }
    else
    {
         PhoneFax.GetEditor(&quot;phf_countryCode&quot;).SetVisible(true);
         PhoneFax.GetEditor(&quot;phf_areaCode&quot;).SetVisible(true);
         PhoneFax.GetEditor(&quot;phf_extension&quot;).SetVisible(true);
         PhoneFax.GetEditor(&quot;phf_extension&quot;).SetVisible(true);
    }
}" />
                                                                                                        <ClientSideEvents Init="function(s, e) {
	var value = s.GetValue();
    if(value == &quot;Mobile&quot;)
    {
         PhoneFax.GetEditor(&quot;phf_countryCode&quot;).SetVisible(false);
         PhoneFax.GetEditor(&quot;phf_areaCode&quot;).SetVisible(false);
         PhoneFax.GetEditor(&quot;phf_extension&quot;).SetVisible(false);
         PhoneFax.GetEditor(&quot;phf_extension&quot;).SetVisible(false);
    }
    else
    {
         PhoneFax.GetEditor(&quot;phf_countryCode&quot;).SetVisible(true);
         PhoneFax.GetEditor(&quot;phf_areaCode&quot;).SetVisible(true);
         PhoneFax.GetEditor(&quot;phf_extension&quot;).SetVisible(true);
         PhoneFax.GetEditor(&quot;phf_extension&quot;).SetVisible(true);
    }
}" />
                                                                                                        <Items>
                                                                                                            <dxe:ListEditItem Text="N/A" Value="N/A"></dxe:ListEditItem>
                                                                                                            <dxe:ListEditItem Text="Residence" Value="Residence"></dxe:ListEditItem>
                                                                                                            <dxe:ListEditItem Text="Work" Value="Work"></dxe:ListEditItem>
                                                                                                            <dxe:ListEditItem Text="Mobile" Value="Mobile"></dxe:ListEditItem>
                                                                                                            <dxe:ListEditItem Text="Fax" Value="Fax"></dxe:ListEditItem>
                                                                                                        </Items>
                                                                                                        <ValidationSettings ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom" SetFocusOnError="True">
                                                                                                            <RequiredField ErrorText="Select Phone Type" IsRequired="True" />
                                                                                                        </ValidationSettings>
                                                                                                    </PropertiesComboBox>
                                                                                                    <EditFormSettings Visible="True" Caption="Phone Type"></EditFormSettings>
                                                                                                    <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                                                                                    </EditCellStyle>
                                                                                                    <CellStyle Wrap="False">
                                                                                                    </CellStyle>
                                                                                                    <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right" VerticalAlign="Top">
                                                                                                    </EditFormCaptionStyle>
                                                                                                </dxe:GridViewDataComboBoxColumn>
                                                                                                <dxe:GridViewDataTextColumn FieldName="phf_countryCode" VisibleIndex="1" Visible="False">
                                                                                                    <EditFormSettings Caption="Country Code" Visible="True" />
                                                                                                    <PropertiesTextEdit>
                                                                                                        <ValidationSettings ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom" SetFocusOnError="True">
                                                                                                            <RegularExpression ErrorText="Enter Valid CountryCode" ValidationExpression="[0-9]+" />
                                                                                                        </ValidationSettings>
                                                                                                    </PropertiesTextEdit>
                                                                                                    <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                                                    </EditFormCaptionStyle>
                                                                                                </dxe:GridViewDataTextColumn>
                                                                                                <dxe:GridViewDataTextColumn FieldName="phf_areaCode" VisibleIndex="1" Visible="False">
                                                                                                    <EditFormSettings Caption="Area Code" Visible="True" />
                                                                                                    <PropertiesTextEdit>
                                                                                                        <ValidationSettings ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom" SetFocusOnError="True">
                                                                                                            <RegularExpression ErrorText="Enter Valid AreaCode" ValidationExpression="[0-9]+" />
                                                                                                        </ValidationSettings>
                                                                                                    </PropertiesTextEdit>
                                                                                                    <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                                                    </EditFormCaptionStyle>
                                                                                                </dxe:GridViewDataTextColumn>
                                                                                                <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="phf_extension" Width="25%"
                                                                                                    Caption="Extension">
                                                                                                    <PropertiesTextEdit Width="300px">
                                                                                                        <ValidationSettings SetFocusOnError="True" ErrorTextPosition="Bottom" ErrorDisplayMode="ImageWithText">
                                                                                                            <RegularExpression ValidationExpression="[0-9]+" ErrorText="Please Enter Number" />
                                                                                                        </ValidationSettings>
                                                                                                    </PropertiesTextEdit>
                                                                                                    <EditFormSettings Visible="True"></EditFormSettings>
                                                                                                    <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                                                                                    </EditCellStyle>
                                                                                                    <CellStyle Wrap="False">
                                                                                                    </CellStyle>
                                                                                                    <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right" VerticalAlign="Top">
                                                                                                    </EditFormCaptionStyle>
                                                                                                </dxe:GridViewDataTextColumn>
                                                                                                <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="phf_phoneNumber" Width="25%"
                                                                                                    Caption="Number">
                                                                                                    <PropertiesTextEdit Width="300px">
                                                                                                        <ValidationSettings SetFocusOnError="True" ErrorTextPosition="Bottom" ErrorDisplayMode="ImageWithText">
                                                                                                            <RequiredField IsRequired="True" ErrorText="Please Enter Phone number"></RequiredField>
                                                                                                            <RegularExpression ValidationExpression="[0-9]+" ErrorText="Please Enter Number" />
                                                                                                        </ValidationSettings>
                                                                                                    </PropertiesTextEdit>
                                                                                                    <EditFormSettings Visible="True"></EditFormSettings>
                                                                                                    <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                                                                                    </EditCellStyle>
                                                                                                    <CellStyle Wrap="False">
                                                                                                    </CellStyle>
                                                                                                    <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right" VerticalAlign="Top">
                                                                                                    </EditFormCaptionStyle>
                                                                                                </dxe:GridViewDataTextColumn>
                                                                                                <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="2" FieldName="phf_faxNumber">
                                                                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                                                                </dxe:GridViewDataTextColumn>
                                                                                                <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="3" FieldName="phf_Availablefrom">
                                                                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                                                                </dxe:GridViewDataTextColumn>
                                                                                                <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="3" FieldName="phf_AvailableTo">
                                                                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                                                                </dxe:GridViewDataTextColumn>
                                                                                                <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="3" FieldName="phf_SMSFacility">
                                                                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                                                                </dxe:GridViewDataTextColumn>
                                                                                                <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="3" FieldName="phf_IsDefault">
                                                                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                                                                </dxe:GridViewDataTextColumn>
                                                                                                <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="3" FieldName="CreateDate">
                                                                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                                                                </dxe:GridViewDataTextColumn>
                                                                                                <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="3" FieldName="CreateUser">
                                                                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                                                                </dxe:GridViewDataTextColumn>
                                                                                                <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="3" FieldName="LastModifyDate">
                                                                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                                                                </dxe:GridViewDataTextColumn>
                                                                                                <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="3" FieldName="LastModifyUser">
                                                                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                                                                </dxe:GridViewDataTextColumn>
                                                                                                <dxe:GridViewCommandColumn VisibleIndex="3" ShowDeleteButton="True" ShowEditButton="True">
                                                                                                    <HeaderTemplate>
                                                                                                        <%if (Session["PageAccess"].ToString().Trim() == "All" || Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "DelAdd")
                                                                                                          { %>
                                                                                                        <a href="javascript:void(0);" onclick="PhoneFax.AddNewRow();">
                                                                                                            <span style="color: #000099; text-decoration: underline">Add New</span>
                                                                                                        </a>
                                                                                                        <%} %>
                                                                                                    </HeaderTemplate>
                                                                                                </dxe:GridViewCommandColumn>
                                                                                            </Columns>
                                                                                            <Templates>
                                                                                                <EditForm>
                                                                                                    <table style="width: 100%">
                                                                                                        <tr>
                                                                                                            <td style="width: 30%"></td>
                                                                                                            <td style="width: 40%">
                                                                                                                <controls>
                                        <dxe:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormEditors" ID="PhoneForm"></dxe:ASPxGridViewTemplateReplacement>
                                    </controls>
                                                                                                                <div style="text-align: right; padding: 2px 2px 2px 2px">
                                                                                                                    <%-- <dxe:ASPxGridViewTemplateReplacement ID="UpdateButton" ReplacementType="EditFormUpdateButton"
                                                                                                        runat="server">
                                                                                                    </dxe:ASPxGridViewTemplateReplacement>--%>
                                                                                                                    <a id="update" href="#" onclick="OnPhoneClick()">Update</a>
                                                                                                                    <dxe:ASPxGridViewTemplateReplacement ID="CancelButton3" ReplacementType="EditFormCancelButton"
                                                                                                                        runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                                                                                </div>
                                                                                                            </td>
                                                                                                            <td style="width: 30%"></td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </EditForm>
                                                                                                <TitlePanel>
                                                                                                    <%--  <table style="width:100%">
                        <tr>
                             <td align="right">
                                <table width="200">
                                    <tr>
                                        <td>
                                            <dxe:ASPxButton ID="ASPxButton2" runat="server" Text="ADD" ToolTip="Add New Data"   Height="18px" Width="88px"  AutoPostBack="False" >
                                                <clientsideevents click="function(s, e) {PhoneFax.AddNewRow();}" />
                                            </dxe:ASPxButton>
                                        </td>
                                                                            
                                         
                                      </tr>
                                  </table>
                              </td>   
                         </tr>
                    </table>--%>
                                                                                                </TitlePanel>
                                                                                            </Templates>
                                                                                        </dxe:ASPxGridView>
                                                                                    </dxe:ContentControl>
                                                                                </ContentCollection>
                                                                            </dxe:TabPage>
                                                                            <dxe:TabPage Text="Email">
                                                                                <ContentCollection>
                                                                                    <dxe:ContentControl runat="server">
                                                                                        <dxe:ASPxGridView runat="server" Width="100%" ID="emailGrid" ClientInstanceName="EmailAdd"
                                                                                            DataSourceID="emcEmail" KeyFieldName="eml_id" AutoGenerateColumns="False" OnBeforePerformDataSelect="ASPxGridView1_BeforePerformDataSelect" OnRowValidating="EmailGrid_RowValidating">
                                                                                            <SettingsEditing EditFormColumnCount="1" PopupEditFormHorizontalAlign="Center" PopupEditFormModal="True"
                                                                                                PopupEditFormVerticalAlign="bottomsides" PopupEditFormWidth="600px" Mode="PopupEditForm">
                                                                                            </SettingsEditing>
                                                                                            <SettingsText PopupEditFormCaption="Add/Modify Email"></SettingsText>
                                                                                            <Styles>
                                                                                                <LoadingPanel ImageSpacing="10px">
                                                                                                </LoadingPanel>
                                                                                                <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                                                                </Header>
                                                                                            </Styles>
                                                                                            <Settings ShowStatusBar="Visible" ShowTitlePanel="True"></Settings>
                                                                                            <SettingsBehavior ConfirmDelete="True" ColumnResizeMode="NextColumn"></SettingsBehavior>
                                                                                            <SettingsPager PageSize="20" NumericButtonCount="20">
                                                                                            </SettingsPager>
                                                                                            <Columns>
                                                                                                <dxe:GridViewDataTextColumn Visible="False" ReadOnly="True" VisibleIndex="0" FieldName="eml_id">
                                                                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                                                                </dxe:GridViewDataTextColumn>
                                                                                                <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="0" FieldName="eml_internalId">
                                                                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                                                                </dxe:GridViewDataTextColumn>
                                                                                                <dxe:GridViewDataTextColumn Visible="False" ReadOnly="True" VisibleIndex="0" FieldName="eml_entity">
                                                                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                                                                </dxe:GridViewDataTextColumn>
                                                                                                <dxe:GridViewDataTextColumn Visible="False" ReadOnly="True" VisibleIndex="0" FieldName="eml_cntId">
                                                                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                                                                </dxe:GridViewDataTextColumn>
                                                                                                <dxe:GridViewDataComboBoxColumn FieldName="eml_type" Caption="Type" VisibleIndex="0">
                                                                                                    <PropertiesComboBox ValueType="System.String" EnableSynchronization="False" EnableIncrementalFiltering="True"
                                                                                                        Width="300px">
                                                                                                        <ClientSideEvents SelectedIndexChanged="function(s, e) {
	var value = s.GetValue();
    if(value == &quot;Web Site&quot;)
    {
         EmailAdd.GetEditor(&quot;eml_email&quot;).SetVisible(false);
         EmailAdd.GetEditor(&quot;eml_ccEmail&quot;).SetVisible(false);
         EmailAdd.GetEditor(&quot;eml_website&quot;).SetVisible(true);
    }
    else
    {
  
         EmailAdd.GetEditor(&quot;eml_email&quot;).SetVisible(true);
         EmailAdd.GetEditor(&quot;eml_ccEmail&quot;).SetVisible(true);
         EmailAdd.GetEditor(&quot;eml_website&quot;).SetVisible(false);
    }
}"
                                                                                                            Init="function(s, e) {
	var value = s.GetValue();
	 if(value == &quot;Web Site&quot;)
    {
         EmailAdd.GetEditor(&quot;eml_email&quot;).SetVisible(false);
         EmailAdd.GetEditor(&quot;eml_ccEmail&quot;).SetVisible(false);
         EmailAdd.GetEditor(&quot;eml_website&quot;).SetVisible(true);
    }
    else
    {
         EmailAdd.GetEditor(&quot;eml_email&quot;).SetVisible(true);
         EmailAdd.GetEditor(&quot;eml_ccEmail&quot;).SetVisible(true);
         EmailAdd.GetEditor(&quot;eml_website&quot;).SetVisible(false);
    }
}"></ClientSideEvents>
                                                                                                        <Items>
                                                                                                            <dxe:ListEditItem Text="N/A" Value="N/A"></dxe:ListEditItem>
                                                                                                            <dxe:ListEditItem Text="Personal" Value="Personal"></dxe:ListEditItem>
                                                                                                            <dxe:ListEditItem Text="Official" Value="Official"></dxe:ListEditItem>
                                                                                                            <dxe:ListEditItem Text="Web Site" Value="Web Site"></dxe:ListEditItem>
                                                                                                        </Items>
                                                                                                        <ValidationSettings ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom" SetFocusOnError="True">
                                                                                                            <RequiredField ErrorText="Select Type" IsRequired="True" />
                                                                                                        </ValidationSettings>
                                                                                                    </PropertiesComboBox>
                                                                                                    <EditFormSettings Visible="True" Caption="Email Type"></EditFormSettings>
                                                                                                    <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                                                                                    </EditCellStyle>
                                                                                                    <CellStyle Wrap="False">
                                                                                                    </CellStyle>
                                                                                                    <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right">
                                                                                                    </EditFormCaptionStyle>
                                                                                                </dxe:GridViewDataComboBoxColumn>
                                                                                                <dxe:GridViewDataTextColumn FieldName="eml_email" Width="40%" Caption="Email" VisibleIndex="1">
                                                                                                    <PropertiesTextEdit Width="300px">
                                                                                                        <ValidationSettings ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom" SetFocusOnError="True">
                                                                                                            <RegularExpression ErrorText="Please Enter Currect Email" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></RegularExpression>
                                                                                                        </ValidationSettings>
                                                                                                    </PropertiesTextEdit>

                                                                                                    <EditFormSettings Visible="True" Caption="Email ID"></EditFormSettings>

                                                                                                    <EditCellStyle HorizontalAlign="Left" Wrap="False"></EditCellStyle>

                                                                                                    <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False"></EditFormCaptionStyle>

                                                                                                    <CellStyle Wrap="False"></CellStyle>
                                                                                                </dxe:GridViewDataTextColumn>
                                                                                                <dxe:GridViewDataTextColumn FieldName="eml_ccEmail" Visible="False" VisibleIndex="2">
                                                                                                    <PropertiesTextEdit Width="300px">
                                                                                                        <ValidationSettings ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom" SetFocusOnError="True">
                                                                                                            <RegularExpression ErrorText="Please Enter Currect CC EmailId" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></RegularExpression>
                                                                                                        </ValidationSettings>
                                                                                                    </PropertiesTextEdit>

                                                                                                    <EditFormSettings Visible="True" Caption="CC Email"></EditFormSettings>

                                                                                                    <EditCellStyle HorizontalAlign="Left" Wrap="False"></EditCellStyle>

                                                                                                    <EditFormCaptionStyle HorizontalAlign="Right" Wrap="False"></EditFormCaptionStyle>

                                                                                                    <CellStyle Wrap="False"></CellStyle>
                                                                                                </dxe:GridViewDataTextColumn>
                                                                                                <dxe:GridViewDataTextColumn FieldName="eml_website" Visible="False" VisibleIndex="2">
                                                                                                    <PropertiesTextEdit Width="300px"></PropertiesTextEdit>

                                                                                                    <EditFormSettings Visible="True" Caption="WebURL"></EditFormSettings>

                                                                                                    <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False"></EditFormCaptionStyle>
                                                                                                </dxe:GridViewDataTextColumn>
                                                                                                <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="2" FieldName="CreateDate">
                                                                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                                                                </dxe:GridViewDataTextColumn>
                                                                                                <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="2" FieldName="CreateUser">
                                                                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                                                                </dxe:GridViewDataTextColumn>
                                                                                                <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="2" FieldName="LastModifyDate">
                                                                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                                                                </dxe:GridViewDataTextColumn>
                                                                                                <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="2" FieldName="LastModifyUser">
                                                                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                                                                </dxe:GridViewDataTextColumn>
                                                                                                <dxe:GridViewCommandColumn VisibleIndex="2" ShowDeleteButton="True" ShowEditButton="True">
                                                                                                    <HeaderTemplate>
                                                                                                        <%if (Session["PageAccess"].ToString().Trim() == "All" || Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "DelAdd")
                                                                                                          { %>
                                                                                                        <a href="javascript:void(0);" onclick="EmailAdd.AddNewRow();">
                                                                                                            <span style="color: #000099; text-decoration: underline">Add New</span>
                                                                                                        </a>
                                                                                                        <%} %>
                                                                                                    </HeaderTemplate>
                                                                                                </dxe:GridViewCommandColumn>
                                                                                            </Columns>
                                                                                            <Templates>
                                                                                                <EditForm>
                                                                                                    <table style="width: 100%">
                                                                                                        <tr>
                                                                                                            <td style="width: 30%"></td>
                                                                                                            <td style="width: 40%">
                                                                                                                <controls>
                                    <dxe:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormEditors" ID="EmailForm"></dxe:ASPxGridViewTemplateReplacement>
                                </controls>
                                                                                                                <div style="text-align: right; padding: 2px 2px 2px 2px">
                                                                                                                    <%-- <dxe:ASPxGridViewTemplateReplacement ID="UpdateButton" ReplacementType="EditFormUpdateButton"
                                                                                                        runat="server">
                                                                                                    </dxe:ASPxGridViewTemplateReplacement>--%>
                                                                                                                    <a id="update1" href="#" onclick="OnEmailClick()">Update</a>
                                                                                                                    <dxe:ASPxGridViewTemplateReplacement ID="CancelButton4" ReplacementType="EditFormCancelButton"
                                                                                                                        runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                                                                                </div>
                                                                                                            </td>
                                                                                                            <td style="width: 30%"></td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </EditForm>
                                                                                                <TitlePanel>
                                                                                                    <%--<table style="width:100%">
                    <tr>
                         <td align="right">
                            <table width="200">
                                <tr>
                                    <td>
                                        <dxe:ASPxButton ID="ASPxButton3" runat="server" Text="ADD" ToolTip="Add New Data"   Height="18px" Width="88px"  AutoPostBack="False" >
                                            <clientsideevents click="function(s, e) {EmailAdd.AddNewRow();}" />
                                        </dxe:ASPxButton>
                                    </td>
                                                                        
                                     
                                  </tr>
                              </table>
                          </td>   
                     </tr>
                </table>--%>
                                                                                                </TitlePanel>
                                                                                            </Templates>
                                                                                        </dxe:ASPxGridView>
                                                                                    </dxe:ContentControl>
                                                                                </ContentCollection>
                                                                            </dxe:TabPage>
                                                                        </TabPages>
                                                                    </dxe:ASPxPageControl>
                                                                </dxe:ContentControl>
                                                            </ContentCollection>
                                                        </dxe:TabPage>
                                                    </TabPages>
                                                    <ContentStyle>
                                                        <Border BorderColor="#002D96" BorderStyle="Solid" BorderWidth="1px" />
                                                    </ContentStyle>
                                                    <LoadingPanelStyle ImageSpacing="6px">
                                                    </LoadingPanelStyle>
                                                </dxe:ASPxPageControl>
                                            </td>
                                            <td style="width: 10%"></td>
                                        </tr>
                                    </table>
                                    &nbsp;
                                </div>
                            </EditForm>
                        </Templates>
                        <SettingsEditing Mode="PopupEditForm" PopupEditFormHorizontalAlign="Center" PopupEditFormModal="True"
                            PopupEditFormVerticalAlign="BottomSides" PopupEditFormWidth="920px" EditFormColumnCount="1" />
                    </dxe:ASPxGridView>
                </td>
            </tr>
        </table>
        <%--============================================================== Master Grid =======================================================--%>
        <%--================================================================ Data Source Of Master Grid ==============================================--%>
        <asp:SqlDataSource ID="amc" runat="server"
            InsertCommand="InsertAMC" InsertCommandType="StoredProcedure" SelectCommand="AMCselect"
            SelectCommandType="StoredProcedure" DeleteCommand="DeleteAmc" DeleteCommandType="StoredProcedure"
            UpdateCommand="UpdateAmc" UpdateCommandType="StoredProcedure">
            <DeleteParameters>
                <asp:Parameter Name="amc_amcCode" Type="String" />
            </DeleteParameters>
            <InsertParameters>
                <asp:Parameter Name="amc_nameOfMutualFund" Type="String" />
                <asp:Parameter Name="amc_sebiRegnNo" Type="String" />
                <asp:Parameter Name="amc_dateOfSetupOfMutualFund" Type="String" />
                <asp:Parameter Name="amc_namesOfSponsors" Type="String" />
                <asp:Parameter Name="amc_nameOfTrusteeCompany" Type="String" />
                <asp:Parameter Name="amc_nameOfAMC" Type="String" />
                <asp:Parameter Name="amc_dateOfIncoOfAMC" Type="String" />
                <asp:Parameter Name="amc_nameOfDirectors" Type="String" />
                <asp:Parameter Name="amc_nameOfHeadOfOperation" Type="String" />
                <asp:SessionParameter Name="CreateUser" Type="Decimal" SessionField="userid" />
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="amc_nameOfMutualFund" Type="String" />
                <asp:Parameter Name="amc_sebiRegnNo" Type="String" />
                <asp:Parameter Name="amc_dateOfSetupOfMutualFund" Type="String" />
                <asp:Parameter Name="amc_namesOfSponsors" Type="String" />
                <asp:Parameter Name="amc_nameOfTrusteeCompany" Type="String" />
                <asp:Parameter Name="amc_nameOfAMC" Type="String" />
                <asp:Parameter Name="amc_dateOfIncoOfAMC" Type="String" />
                <asp:Parameter Name="amc_nameOfDirectors" Type="String" />
                <asp:Parameter Name="amc_nameOfHeadOfOperation" Type="String" />
                <asp:SessionParameter Name="CreateUser" Type="Decimal" SessionField="userid" />
                <asp:Parameter Name="amc_amcCode" Type="String" />
            </UpdateParameters>
        </asp:SqlDataSource>
        <%--================================================================= End Of Master Grid dta Source =========================================--%>
        <%--============================================================== Address Data Source =============================================--%>
        <asp:SqlDataSource runat="server" ID="amcAddress" SelectCommand="select DISTINCT  tbl_master_address.add_id AS add_id,tbl_master_address.add_entity As add_entity,tbl_master_address.add_cntId As add_cntId, tbl_master_address.add_addressType AS add_addressType,
                        tbl_master_address.add_address1 AS add_address1,  tbl_master_address.add_address2 AS add_address2, 
                        tbl_master_address.add_address3 AS add_address3,tbl_master_address.add_landMark AS add_landMark, 
                        tbl_master_address.add_country AS Country, 
                        tbl_master_address.add_state AS State,tbl_master_address.add_pin AS add_pin, 
                        CASE isnull(add_country, '')WHEN '' THEN '0' ELSE(SELECT cou_country FROM tbl_master_country WHERE cou_id = add_country) END AS Country1, 
                        CASE isnull(add_state, '') WHEN '' THEN '0' ELSE(SELECT state FROM tbl_master_state WHERE id = add_state) END AS State1,
                        CASE isnull(add_city, '') WHEN '' THEN '0' ELSE(SELECT city_name FROM tbl_master_city WHERE city_id = add_city) END AS City1, 
                        tbl_master_address.add_city AS City, tbl_master_address.add_pin AS PinCode, tbl_master_address.add_landMark AS LankMark,tbl_master_address.add_activityId AS add_activityId,tbl_master_address.CreateDate AS CreateDate,tbl_master_address.CreateUser AS CreateUser,tbl_master_address.LastModifyDate AS LastModifyDate,tbl_master_address.LastModifyUser AS LastModifyUser 
                        from tbl_master_address where add_cntId=@amc_amcCode"
            OldValuesParameterFormatString="original_{0}"
            ConflictDetection="CompareAllValues" DeleteCommand="DELETE FROM [tbl_master_address] WHERE [add_id] = @original_add_id "
            InsertCommand="INSERT INTO [tbl_master_address] ([add_cntId],[add_entity],[add_addressType], [add_address1], [add_address2], [add_address3], [add_landMark], [add_country], [add_state], [add_city], [add_pin],[CreateDate],[CreateUser]) VALUES (@amc_amcCode,'AMC',@add_addressType, @add_address1, @add_address2, @add_address3, @add_landMark, @Country, @State, @City, @add_pin,getdate(),@CreateUser1)"
            UpdateCommand="UPDATE [tbl_master_address] SET  [add_addressType] = @add_addressType, [add_address1] = @add_address1, [add_address2] = @add_address2, [add_address3] = @add_address3, [add_landMark] = @add_landMark, [add_country] = @Country, [add_state] = @State, [add_city] = @City, [add_pin] = @add_pin,[LastModifyDate]=getdate(),[LastModifyUser]= @CreateUser1 WHERE [add_id] = @original_add_id">
            <InsertParameters>
                <asp:SessionParameter SessionField="KeyVal_InternalID" Type="String" Name="amc_amcCode"></asp:SessionParameter>
                <asp:SessionParameter Name="CreateUser1" Type="Decimal" SessionField="userid" />
                <asp:Parameter Type="String" Name="add_entity"></asp:Parameter>
                <asp:Parameter Type="String" Name="add_addressType"></asp:Parameter>
                <asp:Parameter Type="String" Name="add_address1"></asp:Parameter>
                <asp:Parameter Type="String" Name="add_address2"></asp:Parameter>
                <asp:Parameter Type="String" Name="add_address3"></asp:Parameter>
                <asp:Parameter Type="String" Name="add_landMark"></asp:Parameter>
                <asp:Parameter Name="Country" Type="int32" />
                <asp:Parameter Name="State" Type="int32" />
                <asp:Parameter Name="City" Type="string" />
                <asp:Parameter Type="String" Name="add_pin"></asp:Parameter>
            </InsertParameters>
            <SelectParameters>
                <asp:SessionParameter SessionField="KeyVal_InternalID" Type="String" Name="amc_amcCode"></asp:SessionParameter>
            </SelectParameters>
            <UpdateParameters>
                <asp:Parameter Type="String" Name="add_addressType"></asp:Parameter>
                <asp:Parameter Type="String" Name="add_address1"></asp:Parameter>
                <asp:Parameter Type="String" Name="add_address2"></asp:Parameter>
                <asp:Parameter Type="String" Name="add_address3"></asp:Parameter>
                <asp:Parameter Type="String" Name="add_landMark"></asp:Parameter>
                <asp:Parameter Name="Country" Type="int32" />
                <asp:Parameter Name="State" Type="int32" />
                <asp:Parameter Name="City" Type="string" />
                <asp:Parameter Type="String" Name="add_pin"></asp:Parameter>
                <asp:SessionParameter Name="CreateUser1" Type="Decimal" SessionField="userid" />
            </UpdateParameters>
            <DeleteParameters>
                <asp:Parameter Type="Decimal" Name="original_add_id"></asp:Parameter>
            </DeleteParameters>
        </asp:SqlDataSource>
        <%--====================================================================== End Of address Data Source =======================================--%>
        <%--=========================================================================== Phone Fax Data Source ==========================================--%>
        <asp:SqlDataSource runat="server" ID="emcPhone" SelectCommand="SELECT * FROM [tbl_master_phonefax] where [phf_cntId]=@amc_amcCode"
            OldValuesParameterFormatString="original_{0}" ConflictDetection="CompareAllValues"
            DeleteCommand="DELETE FROM [tbl_master_phonefax] WHERE [phf_id] = @original_phf_id"
            InsertCommand="INSERT INTO [tbl_master_phonefax] ([phf_cntId],[phf_entity],[phf_type], [phf_countryCode], [phf_areaCode], [phf_phoneNumber],[phf_extension],[CreateDate],[CreateUser]) VALUES ( @amc_amcCode,'AMC',@phf_type, @phf_countryCode, @phf_areaCode, @phf_phoneNumber, @phf_extension,getdate(),@CreateUserPhone)"
            UpdateCommand="UPDATE [tbl_master_phonefax] SET [phf_type] = @phf_type, [phf_countryCode] = @phf_countryCode, [phf_areaCode] = @phf_areaCode, [phf_phoneNumber] = @phf_phoneNumber, [phf_extension] = @phf_extension , [LastModifyDate]=getdate(),[LastModifyUser]=@CreateUserPhone WHERE [phf_id] = @original_phf_id">
            <InsertParameters>
                <asp:SessionParameter SessionField="KeyVal_InternalID" Type="String" Name="amc_amcCode"></asp:SessionParameter>
                <asp:SessionParameter Name="CreateUserPhone" Type="Decimal" SessionField="userid" />
                <asp:Parameter Type="String" Name="phf_entity"></asp:Parameter>
                <asp:Parameter Type="String" Name="phf_type"></asp:Parameter>
                <asp:Parameter Type="String" Name="phf_countryCode"></asp:Parameter>
                <asp:Parameter Type="String" Name="phf_areaCode"></asp:Parameter>
                <asp:Parameter Type="String" Name="phf_phoneNumber"></asp:Parameter>
                <asp:Parameter Type="String" Name="phf_extension"></asp:Parameter>
            </InsertParameters>
            <SelectParameters>
                <asp:SessionParameter SessionField="KeyVal_InternalID" Type="String" Name="amc_amcCode"></asp:SessionParameter>
            </SelectParameters>
            <UpdateParameters>
                <asp:SessionParameter Name="CreateUserPhone" Type="Decimal" SessionField="userid" />
                <asp:Parameter Type="String" Name="phf_type"></asp:Parameter>
                <asp:Parameter Type="String" Name="phf_countryCode"></asp:Parameter>
                <asp:Parameter Type="String" Name="phf_areaCode"></asp:Parameter>
                <asp:Parameter Type="String" Name="phf_phoneNumber"></asp:Parameter>
                <asp:Parameter Type="String" Name="phf_extension"></asp:Parameter>
            </UpdateParameters>
            <DeleteParameters>
                <asp:Parameter Type="Decimal" Name="original_phf_id"></asp:Parameter>
            </DeleteParameters>
        </asp:SqlDataSource>
        <%--============================================================================ End Of Phone Fax Data source ===========================================--%>
        <%--================================================================================= Email Data Source ===========================================--%>
        <asp:SqlDataSource runat="server" ID="emcEmail" SelectCommand="SELECT * FROM [tbl_master_email] where [eml_cntId]=@amc_amcCode"
            OldValuesParameterFormatString="original_{0}" ConflictDetection="CompareAllValues"
            DeleteCommand="DELETE FROM [tbl_master_email] WHERE [eml_id] = @original_eml_id"
            InsertCommand="INSERT INTO [tbl_master_email] ( [eml_cntId],[eml_entity],[eml_type], [eml_email], [eml_ccEmail],[CreateDate],[CreateUser]) VALUES ( @amc_amcCode,'AMC',@eml_type, @eml_email, @eml_ccEmail,getdate(),@CreateUserEmail)"
            UpdateCommand="UPDATE [tbl_master_email] SET  [eml_type] = @eml_type, [eml_email] = @eml_email, [eml_ccEmail] = @eml_ccEmail ,[LastModifyDate]=getdate(),[LastModifyUser]=@CreateUserEmail WHERE [eml_id] = @original_eml_id">
            <InsertParameters>
                <asp:SessionParameter SessionField="KeyVal_InternalID" Type="String" Name="amc_amcCode"></asp:SessionParameter>
                <asp:SessionParameter Name="CreateUserEmail" Type="Decimal" SessionField="userid" />
                <asp:Parameter Type="String" Name="eml_entity"></asp:Parameter>
                <asp:Parameter Type="String" Name="eml_type"></asp:Parameter>
                <asp:Parameter Type="String" Name="eml_email"></asp:Parameter>
                <asp:Parameter Type="String" Name="eml_ccEmail"></asp:Parameter>
            </InsertParameters>
            <SelectParameters>
                <asp:SessionParameter SessionField="KeyVal_InternalID" Type="String" Name="amc_amcCode"></asp:SessionParameter>
            </SelectParameters>
            <UpdateParameters>
                <asp:SessionParameter Name="CreateUserEmail" Type="Decimal" SessionField="userid" />
                <asp:Parameter Type="String" Name="eml_type"></asp:Parameter>
                <asp:Parameter Type="String" Name="eml_email"></asp:Parameter>
                <asp:Parameter Type="String" Name="eml_ccEmail"></asp:Parameter>
            </UpdateParameters>
            <DeleteParameters>
                <asp:Parameter Type="Decimal" Name="original_eml_id"></asp:Parameter>
            </DeleteParameters>
        </asp:SqlDataSource>
        <%--================================================================================= End Of Email Dtata Source ===================================--%>
        <%--============================================================= Country data Source ============================================--%>
        <asp:SqlDataSource ID="CountrySelect" runat="server" 
            SelectCommand="SELECT cou_id, cou_country as Country FROM tbl_master_country order by cou_country"></asp:SqlDataSource>
        <%--===============================================================  State Data Source ==============================================--%>
        <asp:SqlDataSource ID="StateSelect" runat="server"
            SelectCommand="SELECT s.id as ID,s.state as State from tbl_master_state s,tbl_master_country cr where (s.countryId = cr.cou_id) and (cr.cou_id = @State) ORDER BY s.state">
            <SelectParameters>
                <asp:Parameter Name="State" Type="string" />
            </SelectParameters>
        </asp:SqlDataSource>
        <%--====================================================================  City Data source ============================================--%>
        <asp:SqlDataSource ID="SelectCity" runat="server" 
            SelectCommand="SELECT c.city_id AS CityId, c.city_name AS City FROM tbl_master_city c where c.state_id=@City order by c.city_name">
            <SelectParameters>
                <asp:Parameter Name="City" Type="int32" />
            </SelectParameters>
        </asp:SqlDataSource>
        <dxe:ASPxGridViewExporter ID="exporter" runat="server">
        </dxe:ASPxGridViewExporter>
    </div>
</asp:Content>

