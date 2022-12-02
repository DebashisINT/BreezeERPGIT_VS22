<%@ Page Language="C#" Title="Bank Details" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.Master.management_master_bank" 
    CodeBehind="bank.aspx.cs" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        
        #BankGrid_DXPagerBottom {
            min-width: 100% !important;
        }
    </style>
    <script type="text/javascript">
        function fn_DeleteEmp(keyValue) {
            //var result=confirm('Confirm delete?');
            //if(result)
            //{
            //    grid.PerformCallback('Delete~' + keyValue);
            //}
            jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    grid.PerformCallback('Delete~' + keyValue);
                }
                else {
                    return false;
                }
            });


        }
        //function is called on changing country
        function OnCountryChanged(cmbCountry) {
            State.PerformCallback(cmbCountry.GetValue().toString());
            //jAlert('test'); 
        }
        function OnStateChanged(cmbState) {
            //  Address.GetEditor("City").PerformCallback(cmbState.GetValue().toString());
            City.PerformCallback(cmbState.GetValue().toString());
        }
        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }
        function OnPhoneClick() {
            if (gridPhone.GetEditor('phf_phoneNumber').GetValue() == null) {
                //jAlert('Phone Mandatory.');
                jAlert('Phone Mandatory.');
            }
            else {
                gridPhone.UpdateEdit(); //Amit
            }
        }
        function OnEmailClick() {
            if (gridEmail.GetEditor('eml_type').GetValue() == 'Web Site') {
                if (gridEmail.GetEditor('eml_website').GetValue() == null)
                {
                    //jAlert('Url Mandatory.');
                    jAlert('Url Mandatory.');
                }
                else {
                    gridEmail.UpdateEdit(); //Amit

                }
            }
            else {
                if (gridEmail.GetEditor('eml_email').GetValue() == null)
                    //jAlert('Email Mandatory.');
                    jAlert('Email Mandatory.');
                else {
                    gridEmail.UpdateEdit();  //Amit

                }
            }
        }
        function NonDeletionMsg() {
            //jAlert("Transactiopn exists for this bank. You cannot this record");
            jAlert("Transactiopn exists for this bank. You cannot this record");
        }

        function DeleteRow(keyValue) {
            // alert (keyValue);
            doIt = confirm('Confirm delete?');
            if (doIt) {
                grid.PerformCallback('Delete~' + keyValue);
            }
        }
        function Emailcheck(obj) {
            if (obj == 'N') {
                if (obj != 'B') {
                    //jAlert("Transactions exists for this Bank... Deletion disallowed!!");
                    jAlert("Transactions exists for this Bank... Deletion disallowed!!");
                    obj = 'B';
                }
            }
        }
        function OnAddButtonClick() {
            //alert('a');
            <%--var url = 'Contact_general.aspx?contact_type=' +'<%=Session["Contactrequesttype"]%>' +'&id=ADD';--%>
             var url = 'Bank_general.aspx?id=' + 'ADD';
             //alert(url);
             window.location.href = url;
        }
        function ClickOnMoreInfo(keyValue) {
            //var url = 'Contact_general.aspx?contact_type=' + '<%=Session["Contactrequesttype"]%>' + '&id=' + keyValue;
            var url = 'Bank_general.aspx?id=' + keyValue;
            window.location.href = url;
        }
    </script>
    <style>
        #BankGrid_DXPEForm_ef0_pageControl_0_ASPxPageControl2_0_AddressGrid_0_DXPEForm_0_efnew_0_DXEFL_DXEditor4_EC,
        #BankGrid_DXPEForm_ef0_pageControl_0_ASPxPageControl2_0_AddressGrid_0_DXPEForm_0_efnew_0_DXEFL_DXEditor9_EC,
        #BankGrid_DXPEForm_ef0_pageControl_0_ASPxPageControl2_0_AddressGrid_0_DXPEForm_0_efnew_0_DXEFL_DXEditor13_EC {
            position:absolute;
        }
    #BankGrid_DXPEForm_ef0_pageControl_0_ASPxPageControl2_0_AddressGrid_0_DXPEForm_0_efnew_0_DXEFL_DXEditor13_CC 
    , #BankGrid_DXPEForm_ef0_pageControl_0_ASPxPageControl2_0_AddressGrid_0_DXPEForm_0_efnew_0_DXEFL_DXEditor4_CC{
        padding:0 !important;
}
    div[id^="BankGrid_DXPEForm_tcefnew"], div[id^="BankGrid_DXPEForm_tcef"] {
        background:#fff !important;
    }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Banks </h3>
        </div>
    </div>
    <div class="form_main">
        <table class="TableMain100">
            <tr>
                <td>
                    <div class="SearchArea">
                        <div class="FilterSide pull-left">
                            <% if (rights.CanAdd)
                                               { %>
                         <%--   <a href="javascript:void(0);" onclick="grid.AddNewRow()" class="btn btn-primary"><span style="color: white;">Add New</span></a>--%>
                                <a href="javascript:void(0);" onclick="OnAddButtonClick()" class="btn btn-success btn-radius"><span class="btn-icon"><i class="fa fa-plus" ></i></span><span>Add New</span> </a>
                            <%} %>
                            <% if (rights.CanExport)
                                               { %>
                            <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-radius" OnChange="if(!AvailableExportOption()){return false;}" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true"  >
                                <asp:ListItem Value="0">Export to</asp:ListItem>
                                <asp:ListItem Value="1">PDF</asp:ListItem>
                                    <asp:ListItem Value="2">XLS</asp:ListItem>
                                    <asp:ListItem Value="3">RTF</asp:ListItem>
                                    <asp:ListItem Value="4">CSV</asp:ListItem>
                            </asp:DropDownList>
                            <%} %>
                            <%--<a href="javascript:ShowHideFilter('All');" class="btn btn-primary"><span>All Records</span></a>--%>
                        </div>
                        <%--<div class="ExportSide">
                            <div class="pull-right">
                                <dxe:ASPxComboBox ID="cmbExport" runat="server" AutoPostBack="true"
                                    ForeColor="Black" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged"
                                    ValueType="System.Int32" Width="130px">
                                    <items>
                                        <dxe:ListEditItem Text="Select" Value="0" />
                                        <dxe:ListEditItem Text="PDF" Value="1" />
                                        <dxe:ListEditItem Text="XLS" Value="2" />
                                        <dxe:ListEditItem Text="RTF" Value="3" />
                                        <dxe:ListEditItem Text="CSV" Value="4" />
                                    </items>
                                    <border />
                                    <dropdownbutton text="Export">
                                    </dropdownbutton>
                                </dxe:ASPxComboBox>
                            </div>
                        </div>--%>
                    </div>
                    <%-- <table width="100%">
                        <tr>
                            <td style="text-align: left; vertical-align: top">
                                <table>
                                    <tr>
                                        <td id="ShowFilter">
                                            <a href="javascript:ShowHideFilter('s');"><span style="color: #000099; text-decoration: underline">
                                                Show Filter</span></a>
                                        </td>
                                        <td id="Td1">
                                            <a href="javascript:ShowHideFilter('All');"><span style="color: #000099; text-decoration: underline">
                                                All Records</span></a>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                            </td>
                            <td class="gridcellright" align="right">
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
                    </table>--%>
                </td>
            </tr>
            <tr>
                <td class="relative">
                    <dxe:ASPxGridView ID="BankGrid" ClientInstanceName="grid" runat="server" AutoGenerateColumns="False"
                        DataSourceID="BankDataSource" KeyFieldName="bnk_internalId" OnHtmlRowCreated="BankGrid_HtmlRowCreated" OnStartRowEditing="BankGrid_StartRowEditing"
                        OnHtmlEditFormCreated="BankGrid_HtmlEditFormCreated" OnCustomCallback="BankGrid_CustomCallback"
                        Width="100%" OnRowValidating="BankGrid_RowValidating" OnCustomJSProperties="BankGrid_CustomJSProperties"
                        OnRowCommand="BankGrid_RowCommand" OnRowUpdated="BankGrid_RowUpdated" OnRowUpdating="BankGrid_RowUpdating" OnInitNewRow="BankGrid_InitNewRow"
                        OnCommandButtonInitialize="BankGrid_CommandButtonInitialize"   SettingsDataSecurity-AllowDelete="false" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" >
                        <settingssearchpanel visible="true" delay="5000"/>
                        <templates>
                            <EditForm>
                                <div style="padding: 8px 8px 7px 8px;">
                                    <table style="width: 100%;">
                                        <tr>
                                            
                                            <td style="width: 100%">
                                                <dxe:ASPxPageControl runat="server" ID="pageControl" Width="100%" ActiveTabIndex="0">
                                                    <TabPages>
                                                        <dxe:TabPage Text="General">
                                                            <ContentCollection>
                                                                <dxe:ContentControl runat="server">
                                                                    <table width="100%">
                                                                        <tr>
                                                                            
                                                                            <td style="width: 100%" align="center">
                                                                                <dxe:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormEditors"
                                                                                    ColumnID="" ID="Editors"></dxe:ASPxGridViewTemplateReplacement>
                                                                            </td>
                                                                            
                                                                        </tr>
                                                                    </table>
                                                                    <div style=" font-size: 16px; padding: 2px 2px 2px 88px">
                                                                        <div class="dxbButton" style="display: inline-block; padding: 3px; color: #000">
                                                                            <dxe:ASPxGridViewTemplateReplacement ID="UpdateButton" ReplacementType="EditFormUpdateButton"
                                                                                runat="server" ColumnID=""></dxe:ASPxGridViewTemplateReplacement>
                                                                        </div>
                                                                        <div class="dxbButton" style="display: inline-block; padding: 3px; color: #000">
                                                                            <dxe:ASPxGridViewTemplateReplacement ID="ASPxGridViewTemplateReplacement1" ReplacementType="EditFormCancelButton"
                                                                                runat="server" ColumnID=""></dxe:ASPxGridViewTemplateReplacement>
                                                                        </div>
                                                                    </div>
                                                                </dxe:ContentControl>
                                                            </ContentCollection>
                                                        </dxe:TabPage>
                                                        <dxe:TabPage Text="Correspondance" Name="tabCorrespondence">
                                                            <ContentCollection>
                                                                <dxe:ContentControl runat="server">
                                                                    <dxe:ASPxPageControl ID="ASPxPageControl2" runat="server" ActiveTabIndex="0" ClientInstanceName="page">
                                                                        <TabPages>
                                                                            <dxe:TabPage Text="Address">
                                                                                <ContentCollection>
                                                                                    <dxe:ContentControl runat="server">
                                                                                        <% if (rights.CanAdd)
                                                                                           { %>
                                                                                        <div style=""float:left;"><a href="javascript:void(0);" onclick="Address.AddNewRow();" class="btn btn-primary"><span style="color: white;">Add New</span> </a></div>
                                                                                             <%   } %>
                                                                                        <dxe:ASPxGridView runat="server" ID="AddressGrid" ClientInstanceName="Address"
                                                                                            DataSourceID="BankAddress" KeyFieldName="add_id" AutoGenerateColumns="False"
                                                                                            Width="100%" OnCellEditorInitialize="AddressGrid_CellEditorInitialize" 
                                                                                            OnBeforePerformDataSelect="AddressGrid_BeforePerformDataSelect" 
                                                                                            OnCommandButtonInitialize="AddressGrid_CommandButtonInitialize">
                                                                                            
                                                                                             <Templates>
                                                                                                <EditForm>
                                                                                                    <table style="width: 97%; margin: 8px;">
                                                                                                        <tr>
                                                                                                            <td style="width: 30%"></td>
                                                                                                            <td style="width: 40%">
                                                                                                                <dxe:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormEditors" ID="AddressForm"></dxe:ASPxGridViewTemplateReplacement>
                                                                                                                <div style=" padding: 2px 2px 2px 90px">
                                                                                                                    <div class="dxbButton" style="display: inline-block; padding: 3px">
                                                                                                                        <dxe:ASPxGridViewTemplateReplacement ID="ASPxGridViewTemplateReplacement2" ReplacementType="EditFormUpdateButton"
                                                                                                                            runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                                                                                    </div>
                                                                                                                    <div class="dxbButton" style="display: inline-block; padding: 3px">
                                                                                                                        <dxe:ASPxGridViewTemplateReplacement ID="ASPxGridViewTemplateReplacement3" ReplacementType="EditFormCancelButton"
                                                                                                                            runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                                                                                    </div>
                                                                                                                </div>
                                                                                                            </td>
                                                                                                            <td style="width: 30%"></td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </EditForm>
                                                                                            </Templates>
                                                                                            <SettingsEditing EditFormColumnCount="1" PopupEditFormHorizontalAlign="Center" PopupEditFormModal="True"
                                                                                                PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="600px" Mode="PopupEditForm">
                                                                                            </SettingsEditing>
                                                                                            <SettingsText PopupEditFormCaption="Add/Modify Address"></SettingsText>
                                                                                            <Styles>
                                                                                                <LoadingPanel ImageSpacing="10px">
                                                                                                </LoadingPanel>
                                                                                                <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                                                                </Header>
                                                                                            </Styles>
                                                                                            <Settings ShowStatusBar="Visible" ShowTitlePanel="True" ShowGroupPanel="True"></Settings>
                                                                                            <SettingsBehavior ConfirmDelete="True" ColumnResizeMode="NextColumn"></SettingsBehavior>
                                                                                            <SettingsPager PageSize="20" NumericButtonCount="20">
                                                                                                   
                                                                                            </SettingsPager>


                                                                                            <Columns>
                                                                                                <dxe:GridViewDataTextColumn Visible="False" ReadOnly="True" VisibleIndex="0" FieldName="add_id">
                                                                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                                                                </dxe:GridViewDataTextColumn>
                                                                                                <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="0" FieldName="add_cntId">
                                                                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                                                                </dxe:GridViewDataTextColumn>
                                                                                                <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="0" FieldName="add_entity">
                                                                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                                                                </dxe:GridViewDataTextColumn>
                                                                                                <dxe:GridViewDataComboBoxColumn FieldName="add_addressType" Caption="Type" VisibleIndex="0"
                                                                                                    Width="10%">
                                                                                                    <EditCellStyle Wrap="False" HorizontalAlign="Left">
                                                                                                    </EditCellStyle>
                                                                                                    <PropertiesComboBox Width="299px" ValueType="System.String" EnableSynchronization="False"
                                                                                                        EnableIncrementalFiltering="True">
                                                                                                        <Items>
                                                                                                            <dxe:ListEditItem Value="N/A" Text="N/A"></dxe:ListEditItem>
                                                                                                            <dxe:ListEditItem Value="Residence" Text="Residence"></dxe:ListEditItem>
                                                                                                            <dxe:ListEditItem Value="Office" Text="Office"></dxe:ListEditItem>
                                                                                                            <dxe:ListEditItem Value="Emergence" Text="Emergence"></dxe:ListEditItem>
                                                                                                            <dxe:ListEditItem Value="Correspondance" Text="Correspondance"></dxe:ListEditItem>
                                                                                                            <dxe:ListEditItem Value="Permanent" Text="Permanent"></dxe:ListEditItem>
                                                                                                            <dxe:ListEditItem Value="Contact Person" Text="Contact Person"></dxe:ListEditItem>
                                                                                                        </Items>
                                                                                                    </PropertiesComboBox>
                                                                                                    <CellStyle Wrap="False">
                                                                                                    </CellStyle>
                                                                                                    <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right">
                                                                                                    </EditFormCaptionStyle>
                                                                                                    <EditFormSettings Caption="Address Type" Visible="True"></EditFormSettings>
                                                                                                </dxe:GridViewDataComboBoxColumn>
                                                                                                <dxe:GridViewDataMemoColumn Caption="Address1" FieldName="add_address1" VisibleIndex="1"
                                                                                                    Width="10%" CellStyle-Wrap="True" >
                                                                                                    <PropertiesMemoEdit  Width="299px">
                                                                                                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" SetFocusOnError="True">
                                                                                                            <RequiredField ErrorText="Mandatory." IsRequired="True" />
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
                                                                                                    Width="10%">
                                                                                                    <PropertiesMemoEdit Width="299px" Height="25px">
                                                                                                    </PropertiesMemoEdit>
                                                                                                    <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                                                                                    </EditCellStyle>
                                                                                                    <CellStyle Wrap="False">
                                                                                                    </CellStyle>
                                                                                                    <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right">
                                                                                                    </EditFormCaptionStyle>
                                                                                                    <EditFormSettings Visible="True" />
                                                                                                </dxe:GridViewDataMemoColumn>
                                                                                                <dxe:GridViewDataMemoColumn Caption="Address3" FieldName="add_address3" VisibleIndex="3"
                                                                                                    Width="10%">
                                                                                                    <PropertiesMemoEdit Width="299" Height="25px">
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
                                                                                                        EnableSynchronization="False" EnableIncrementalFiltering="True" ValueType="System.Int32" EnableClientSideAPI="true"
                                                                                                        Width="300px">
                                                                                                        <ClientSideEvents ValueChanged="function(s, e) { OnCountryChanged(s); }"></ClientSideEvents>
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
                                                                                                    <PropertiesComboBox DataSourceID="StateSelect" TextField="State" ValueField="ID" ClientInstanceName="State" EnableClientSideAPI="true"
                                                                                                        EnableSynchronization="False" EnableIncrementalFiltering="True" ValueType="System.String"
                                                                                                        Width="299">
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
                                                                                                    <PropertiesComboBox DataSourceID="SelectCity" TextField="City" ValueField="CityId" ClientInstanceName="City"
                                                                                                        EnableSynchronization="False" EnableIncrementalFiltering="True" ValueType="System.String">
                                                                                                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="right" SetFocusOnError="True">
                                                                                                            <RequiredField ErrorText="Mandatory." IsRequired="True" />
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
                                                                                                <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="add_pin" Width="10%" Caption="Pincode">
                                                                                                    <EditCellStyle Wrap="False" HorizontalAlign="Left">
                                                                                                    </EditCellStyle>
                                                                                                    <CellStyle Wrap="False">
                                                                                                    </CellStyle>
                                                                                                    <PropertiesTextEdit Width="300px">
                                                                                                        <ValidationSettings SetFocusOnError="True" ErrorTextPosition="right" ErrorDisplayMode="ImageWithTooltip">
                                                                                                            <RequiredField IsRequired="True" ErrorText="Mandatory"></RequiredField>
                                                                                                            <RegularExpression ErrorText="Please Enter valid pincode" ValidationExpression="[0-9]{6}" />
                                                                                                        </ValidationSettings>
                                                                                                    </PropertiesTextEdit>
                                                                                                    <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right" VerticalAlign="Top">
                                                                                                    </EditFormCaptionStyle>
                                                                                                    <EditFormSettings Caption="Pincode" Visible="True"></EditFormSettings>
                                                                                                </dxe:GridViewDataTextColumn>
                                                                                                <dxe:GridViewDataTextColumn VisibleIndex="8" FieldName="add_landMark" Caption="Landmark"
                                                                                                    Width="10%">
                                                                                                    <EditCellStyle Wrap="False" HorizontalAlign="Left">
                                                                                                    </EditCellStyle>
                                                                                                    <CellStyle Wrap="False">
                                                                                                    </CellStyle>
                                                                                                    <PropertiesTextEdit Height="10px" Width="300px">
                                                                                                    </PropertiesTextEdit>
                                                                                                    <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right">
                                                                                                    </EditFormCaptionStyle>
                                                                                                    <EditFormSettings Caption="Landmark" Visible="True"></EditFormSettings>
                                                                                                </dxe:GridViewDataTextColumn>
                                                                                                <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="6" FieldName="add_activityId">
                                                                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                                                                </dxe:GridViewDataTextColumn>
                                                                                                <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="6" FieldName="CreateDate">
                                                                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                                                                </dxe:GridViewDataTextColumn>
                                                                                                <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="6" FieldName="CreateUser">
                                                                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                                                                </dxe:GridViewDataTextColumn>
                                                                                                <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="7" FieldName="LastModifyDate">
                                                                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                                                                </dxe:GridViewDataTextColumn>
                                                                                                <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="7" FieldName="LastModifyUser">
                                                                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                                                                </dxe:GridViewDataTextColumn>
                                                                                                <dxe:GridViewCommandColumn VisibleIndex="9" ShowDeleteButton="true" ShowEditButton="true" Width="100px">
                                                                                                    <HeaderTemplate>
                                                                                                        Action
                                                                                                        <%--<a href="javascript:void(0);" onclick="Address.AddNewRow();"><span style="color: white;">Add New</span> </a>--%>
                                                                                                    </HeaderTemplate>
                                                                                                </dxe:GridViewCommandColumn>
                                                                                                 
                                                                                            </Columns>
                                                                                            <SettingsSearchPanel Visible="True" />
                                                                                             <settings showstatusbar="Visible" showgrouppanel="True" showfilterrow="true" showfilterrowmenu="True" />
                                                                                            <SettingsCommandButton>                                                                                               
                                                                                                <EditButton Image-Url="../../../assests/images/Edit.png" ButtonType="Image" Image-AlternateText="Edit" Styles-Style-CssClass="pad">
                                                                                                
                                                                                                </EditButton>
                                                                                                 <DeleteButton Image-Url="../../../assests/images/Delete.png" ButtonType="Image" Image-AlternateText="Delete">
                                                                                                </DeleteButton>
                                                                                                <UpdateButton Text="Save" ButtonType="Button" Styles-Style-CssClass="btn btn-primary"></UpdateButton>
                                                                                                <CancelButton Text="Cancel" ButtonType="Button" Styles-Style-CssClass="btn btn-danger"></CancelButton>
                                                                                            </SettingsCommandButton>

                                                                                        </dxe:ASPxGridView>
                                                                                    </dxe:ContentControl>
                                                                                </ContentCollection>
                                                                            </dxe:TabPage>
                                                                            <dxe:TabPage Text="Phone">
                                                                                <ContentCollection>
                                                                                    <dxe:ContentControl runat="server">
                                                                                        <% if (rights.CanAdd)
                                                                                           { %>
                                                                                        <a href="javascript:void(0);" onclick="gridPhone.AddNewRow();" class="btn btn-primary"><span style="color:white"> Add New</span> </a>
                                                                                        <% } %>
                                                                                        <dxe:ASPxGridView ID="PhoneFaxGrid" ClientInstanceName="gridPhone" runat="server"
                                                                                            AutoGenerateColumns="False" DataSourceID="BankPhone" KeyFieldName="phf_id" Width="100%"
                                                                                            OnBeforePerformDataSelect="AddressGrid_BeforePerformDataSelect" OnCellEditorInitialize="PhoneFaxGrid_CellEditorInitialize" OnRowValidating="PhoneGrid_RowValidating"
                                                                                            OnCommandButtonInitialize="PhoneFaxGrid_CommandButtonInitialize">
                                                                                            <Templates>
                                                                                                <EditForm>
                                                                                                    <table style="width: 97%; margin: 8px;">
                                                                                                        <tr>
                                                                                                            <td style="width: 30%"></td>
                                                                                                            <td style="width: 40%">
                                                                                                                <controls>
                                                                                                 <dxe:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormEditors" ID="PhoneForm"></dxe:ASPxGridViewTemplateReplacement>
                                                                                                </controls>
                                                                                                                <div style=" padding: 2px 2px 2px 94px">
                                                                                                                    <%--<dxe:ASPxGridViewTemplateReplacement ID="UpdateButton" ReplacementType="EditFormUpdateButton"
                                                                                                        runat="server">
                                                                                                    </dxe:ASPxGridViewTemplateReplacement>--%>
                                                                                                                    <button id="update" class="dxbButton_PlasticBlue btn btn-primary  dxbButtonSys dxbTSys" onclick="OnPhoneClick();return false">Update</button>
                                                                                                                    <div class="dxbButton" style="display: inline-block; padding: 3px">
                                                                                                                        <dxe:ASPxGridViewTemplateReplacement ID="ASPxGridViewTemplateReplacement4" ReplacementType="EditFormCancelButton"
                                                                                                                            runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                                                                                    </div>
                                                                                                                </div>
                                                                                                            </td>
                                                                                                            <td style="width: 30%"></td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </EditForm>
                                                                                                <%-- <TitlePanel>
                                                                                    <table style="width: 100%">
                                                                                        <tr>
                                                                                            <td align="right">
                                                                                                <table width="200">
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                            <dxe:ASPxButton ID="ASPxButton1" runat="server" Text="ADD" ToolTip="Add New Data"
                                                                                                                Height="18px" Width="88px" AutoPostBack="False">
                                                                                                                <ClientSideEvents Click="function(s, e) {gridPhone.AddNewRow();}" />
                                                                                                            </dxe:ASPxButton>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </TitlePanel>--%>
                                                                                            </Templates>
                                                                                            <SettingsEditing EditFormColumnCount="1" PopupEditFormHorizontalAlign="Center" PopupEditFormModal="True"
                                                                                                PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="600px" Mode="PopupEditForm">
                                                                                            </SettingsEditing>
                                                                                            <SettingsText PopupEditFormCaption="Add/Modify Phone Fax"></SettingsText>
                                                                                            <Styles>
                                                                                                <LoadingPanel ImageSpacing="10px">
                                                                                                </LoadingPanel>
                                                                                                <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                                                                </Header>
                                                                                            </Styles>
                                                                                            <Settings ShowStatusBar="Visible" ShowTitlePanel="True" ShowGroupPanel="True"></Settings>
                                                                                            <SettingsBehavior ConfirmDelete="True" ColumnResizeMode="NextColumn"></SettingsBehavior>
                                                                                            <SettingsPager PageSize="20" NumericButtonCount="20">
                                                                                            </SettingsPager>
                                                                                            <Columns>
                                                                                                <dxe:GridViewDataTextColumn Visible="False" ReadOnly="True" VisibleIndex="0" FieldName="phf_id">
                                                                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                                                                </dxe:GridViewDataTextColumn>
                                                                                                <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="0" FieldName="phf_cntId">
                                                                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                                                                </dxe:GridViewDataTextColumn>
                                                                                                <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="0" FieldName="phf_entity">
                                                                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                                                                </dxe:GridViewDataTextColumn>
                                                                                                <dxe:GridViewDataComboBoxColumn FieldName="phf_type" Caption="Type" VisibleIndex="0">
                                                                                                    <PropertiesComboBox ValueType="System.String" EnableSynchronization="False" EnableIncrementalFiltering="True"
                                                                                                        Width="300px">
                                                                                                         <%--Code  Added and Commented By Priti on 07122016 to Enable TextBox--%>
                                                                                                        <%--<ClientSideEvents SelectedIndexChanged="function(s, e) {
	                                                                                                var value = s.GetValue();
                                                                                                    if(value == &quot;Mobile&quot;)
                                                                                                    {
                                                                                                         gridPhone.GetEditor(&quot;phf_countryCode&quot;).SetVisible(false);
                                                                                                         gridPhone.GetEditor(&quot;phf_areaCode&quot;).SetVisible(false);
                                                                                                         gridPhone.GetEditor(&quot;phf_extension&quot;).SetVisible(false);
                                                                                                         gridPhone.GetEditor(&quot;phf_extension&quot;).SetVisible(false);
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                         gridPhone.GetEditor(&quot;phf_countryCode&quot;).SetVisible(true);
                                                                                                         gridPhone.GetEditor(&quot;phf_areaCode&quot;).SetVisible(true);
                                                                                                         gridPhone.GetEditor(&quot;phf_extension&quot;).SetVisible(true);
                                                                                                         gridPhone.GetEditor(&quot;phf_extension&quot;).SetVisible(true);
                                                                                                    }
                                                                                                }" />--%>
                                                                                                        <ClientSideEvents SelectedIndexChanged="function(s, e) {
	                                                                                                var value = s.GetValue();
                                                                                                    if(value == &quot;Mobile&quot;)
                                                                                                    {
                                                                                                         gridPhone.GetEditor(&quot;phf_countryCode&quot;).SetEnabled(false);
                                                                                                        
                                                                                                         gridPhone.GetEditor(&quot;phf_areaCode&quot;).SetEnabled(false);
                                                                                                       
                                                                                                         gridPhone.GetEditor(&quot;phf_extension&quot;).SetEnabled(false);

                                                                                                        
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                          
                                                                                                         gridPhone.GetEditor(&quot;phf_countryCode&quot;).SetEnabled(true);
                                                                                                           
                                                                                                         gridPhone.GetEditor(&quot;phf_areaCode&quot;).SetEnabled(true);
                                                                                                         gridPhone.GetEditor(&quot;phf_extension&quot;).SetEnabled(true);
                                                                                                       
                                                                                                    }
                                                                                                }" />
                                                                                                <%--<ClientSideEvents Init="function(s, e) {
	                                                                                                var value = s.GetValue();
                                                                                                    if(value == &quot;Mobile&quot;)
                                                                                                    {
                                                                                                         gridPhone.GetEditor(&quot;phf_countryCode&quot;).SetVisible(false);
                                                                                                         gridPhone.GetEditor(&quot;phf_areaCode&quot;).SetVisible(false);
                                                                                                         gridPhone.GetEditor(&quot;phf_extension&quot;).SetVisible(false);
                                                                                                         gridPhone.GetEditor(&quot;phf_extension&quot;).SetVisible(false);
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                         gridPhone.GetEditor(&quot;phf_countryCode&quot;).SetVisible(true);
                                                                                                         gridPhone.GetEditor(&quot;phf_areaCode&quot;).SetVisible(true);
                                                                                                         gridPhone.GetEditor(&quot;phf_extension&quot;).SetVisible(true);
                                                                                                         gridPhone.GetEditor(&quot;phf_extension&quot;).SetVisible(true);
                                                                                                    }
                                                                                                }" />--%>
                                                                                                        <ClientSideEvents Init="function(s, e) {
	                                                                                                var value = s.GetValue();
                                                                                                    if(value == &quot;Mobile&quot;)
                                                                                                    {
                                                                                                         gridPhone.GetEditor(&quot;phf_countryCode&quot;).SetEnabled(false);
                                                                                                     
                                                                                                         gridPhone.GetEditor(&quot;phf_areaCode&quot;).SetEnabled(false);
                                                                                                        
                                                                                                         gridPhone.GetEditor(&quot;phf_extension&quot;).SetEnabled(false);
                                                                                                       
                                                                                                         
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                                 
                                                                                                         gridPhone.GetEditor(&quot;phf_countryCode&quot;).SetEnabled(true); 
                                                                                                                                                                                                  
                                                                                                         gridPhone.GetEditor(&quot;phf_areaCode&quot;).SetEnabled(true);
                                                                                                         gridPhone.GetEditor(&quot;phf_extension&quot;).SetEnabled(true);
                                                                                                         
                                                                                                    }
                                                                                                }" />
                                                                                                       <%-- ....end......................--%>
                                                                                                        <Items>
                                                                                                            <dxe:ListEditItem Text="N/A" Value="N/A"></dxe:ListEditItem>
                                                                                                            <dxe:ListEditItem Text="Residence" Value="Residence"></dxe:ListEditItem>
                                                                                                            <dxe:ListEditItem Text="Work" Value="Work"></dxe:ListEditItem>
                                                                                                            <dxe:ListEditItem Text="Mobile" Value="Mobile"></dxe:ListEditItem>
                                                                                                            <dxe:ListEditItem Text="Fax" Value="Fax"></dxe:ListEditItem>
                                                                                                        </Items>
                                                                                                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="right" SetFocusOnError="True">
                                                                                                            <RequiredField ErrorText="Mandatory." IsRequired="True" />
                                                                                                        </ValidationSettings>
                                                                                                    </PropertiesComboBox>
                                                                                                    <EditFormSettings Visible="True" Caption="Phone Type"></EditFormSettings>
                                                                                                    <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                                                                                    </EditCellStyle>
                                                                                                    <CellStyle Wrap="False">
                                                                                                    </CellStyle>
                                                                                                    <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right">
                                                                                                    </EditFormCaptionStyle>
                                                                                                </dxe:GridViewDataComboBoxColumn>
                                                                                                <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="1" FieldName="phf_countryCode"
                                                                                                    Width="20%">
                                                                                                    <EditCellStyle Wrap="False" HorizontalAlign="Left">
                                                                                                    </EditCellStyle>
                                                                                                    <CellStyle Wrap="False">
                                                                                                    </CellStyle>
                                                                                                    <PropertiesTextEdit Width="300px">
                                                                                                        <ValidationSettings SetFocusOnError="True" ErrorTextPosition="right" ErrorDisplayMode="ImageWithTooltip">
                                                                                                            <RegularExpression ErrorText="Please Enter Number" ValidationExpression="[0-9]+" />
                                                                                                        </ValidationSettings>
                                                                                                    </PropertiesTextEdit>
                                                                                                    <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right" VerticalAlign="Top">
                                                                                                    </EditFormCaptionStyle>
                                                                                                    <EditFormSettings Caption="Country Code" Visible="True"></EditFormSettings>
                                                                                                </dxe:GridViewDataTextColumn>
                                                                                                <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="1" FieldName="phf_areaCode">
                                                                                                    <EditCellStyle Wrap="False" HorizontalAlign="Left">
                                                                                                    </EditCellStyle>
                                                                                                    <CellStyle Wrap="False">
                                                                                                    </CellStyle>
                                                                                                    <PropertiesTextEdit Width="300px">
                                                                                                        <ValidationSettings SetFocusOnError="True" ErrorTextPosition="right" ErrorDisplayMode="ImageWithTooltip">
                                                                                                            <RegularExpression ErrorText="Please Enter Number" ValidationExpression="[0-9]+" />
                                                                                                        </ValidationSettings>
                                                                                                    </PropertiesTextEdit>
                                                                                                    <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right" VerticalAlign="Top">
                                                                                                    </EditFormCaptionStyle>
                                                                                                    <EditFormSettings Caption="Area Code" Visible="True"></EditFormSettings>
                                                                                                </dxe:GridViewDataTextColumn>
                                                                                                <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="phf_phoneNumber" Width="20%"
                                                                                                    Caption="Number">
                                                                                                    <EditCellStyle Wrap="False" HorizontalAlign="Left">
                                                                                                    </EditCellStyle>
                                                                                                    <CellStyle Wrap="False">
                                                                                                    </CellStyle>
                                                                                                    <PropertiesTextEdit Width="300px">
                                                                                                        <ValidationSettings SetFocusOnError="True" ErrorTextPosition="right" ErrorDisplayMode="ImageWithTooltip">
                                                                                                            <RequiredField IsRequired="True" ErrorText="Mandatory."></RequiredField>
                                                                                                            <RegularExpression ErrorText="Please Enter Number " ValidationExpression="[0-9]+" />
                                                                                                        </ValidationSettings>
                                                                                                    </PropertiesTextEdit>
                                                                                                    <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right" VerticalAlign="Top">
                                                                                                    </EditFormCaptionStyle>
                                                                                                    <EditFormSettings Caption="Number" Visible="True"></EditFormSettings>
                                                                                                </dxe:GridViewDataTextColumn>
                                                                                                <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="2" FieldName="phf_faxNumber">
                                                                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                                                                </dxe:GridViewDataTextColumn>
                                                                                                <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="phf_extension" Width="20%"
                                                                                                    Caption="Extn">
                                                                                                    <EditCellStyle Wrap="False" HorizontalAlign="Left">
                                                                                                    </EditCellStyle>
                                                                                                    <CellStyle Wrap="False">
                                                                                                    </CellStyle>
                                                                                                    <PropertiesTextEdit Width="300px">
                                                                                                        <ValidationSettings SetFocusOnError="True" ErrorTextPosition="right" ErrorDisplayMode="ImageWithTooltip">
                                                                                                            <RegularExpression ErrorText="Please Enter number" ValidationExpression="[0-9]+" />
                                                                                                        </ValidationSettings>
                                                                                                    </PropertiesTextEdit>
                                                                                                    <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right" VerticalAlign="Top">
                                                                                                    </EditFormCaptionStyle>
                                                                                                    <EditFormSettings Caption="Extension" Visible="True"></EditFormSettings>
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
                                                                                                <dxe:GridViewCommandColumn VisibleIndex="3" ShowDeleteButton="true" ShowEditButton="true">
                                                                                                    <HeaderTemplate>
                                                                                                        Actions
                                                                                                    </HeaderTemplate>
                                                                                                </dxe:GridViewCommandColumn>
                                                                                            </Columns>
                                                                                            <SettingsSearchPanel Visible="True" />
                                                                                              <settings showstatusbar="Visible" showgrouppanel="True" showfilterrow="true" showfilterrowmenu="True" />
                                                                                            <SettingsCommandButton>
                                                                                               
                                                                                                <EditButton Image-Url="../../../assests/images/Edit.png" ButtonType="Image" Image-AlternateText="Edit" Styles-Style-CssClass="pad">
                                                                                                </EditButton>
                                                                                                 <DeleteButton Image-Url="../../../assests/images/Delete.png" ButtonType="Image" Image-AlternateText="Delete">
                                                                                                </DeleteButton>
                                                                                                <UpdateButton Text="Save" ButtonType="Button" Styles-Style-CssClass="btn btn-primary"></UpdateButton>
                                                                                                <CancelButton Text="Cancel" ButtonType="Button" Styles-Style-CssClass="btn btn-danger"></CancelButton>
                                                                                            </SettingsCommandButton>
                                                                                        </dxe:ASPxGridView>
                                                                                    </dxe:ContentControl>
                                                                                </ContentCollection>
                                                                            </dxe:TabPage>
                                                                            <dxe:TabPage Text="Email">
                                                                                <ContentCollection>
                                                                                    <dxe:ContentControl runat="server">
                                                                                        <% if (rights.CanAdd)
                                                                                           { %>
                                                                                        <a href="javascript:void(0);" onclick="gridEmail.AddNewRow();" class="btn btn-primary"><span style="color: white;">Add New</span> </a>
                                                                                         <% } %>
                                                                                        <dxe:ASPxGridView ID="EmailGrid" ClientInstanceName="gridEmail" runat="server"
                                                                                            AutoGenerateColumns="False" DataSourceID="BankEmail" KeyFieldName="eml_id" Width="100%"
                                                                                            OnBeforePerformDataSelect="AddressGrid_BeforePerformDataSelect" OnCellEditorInitialize="EmailGrid_CellEditorInitialize" OnRowValidating="EmailGrid_RowValidating"
                                                                                            OnCommandButtonInitialize="EmailGrid_CommandButtonInitialize">
                                                                                            <Templates>
                                                                                                <EditForm>
                                                                                                    <table style="width: 97%; margin: 8px;">
                                                                                                        <tr>
                                                                                                            <td style="width: 30%"></td>
                                                                                                            <td style="width: 40%">
                                                                                                                <dxe:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormEditors" ID="EmailForm"></dxe:ASPxGridViewTemplateReplacement>
                                                                                                                <div style=" padding: 2px 2px 2px 91px">
                                                                                                                    <%--<dxe:ASPxGridViewTemplateReplacement ID="UpdateButton" ReplacementType="EditFormUpdateButton"
                                                                                                        runat="server">
                                                                                                    </dxe:ASPxGridViewTemplateReplacement>--%>
                                                                                                                <%--    <button id="update" class="dxbButton_PlasticBlue btn btn-primary btn-xs dxbButtonSys dxbTSys dxbButtonHover_PlasticBlue" style="color: #000; padding: 3px;" href="#" onclick="OnEmailClick();return false">Update</button>--%>
                                                                                                                    <dxe:ASPxGridViewTemplateReplacement ID="UpdateButton1" ReplacementType="EditFormUpdateButton"
                                                                                                           runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                                                                                     <div class="dxbButton" style="display: inline-block; padding: 3px">
                                                                                                                        <dxe:ASPxGridViewTemplateReplacement ID="CancelButton" ReplacementType="EditFormCancelButton"
                                                                                                                            runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                                                                                    </div>
                                                                                                                </div>
                                                                                                            </td>
                                                                                                            <td style="width: 30%"></td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </EditForm>
                                                                                                <%-- <TitlePanel>
                                                                                    <table style="width: 100%">
                                                                                        <tr>
                                                                                            <td align="right">
                                                                                                <table width="200">
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                            <dxe:ASPxButton ID="ASPxButton1" runat="server" Text="ADD" ToolTip="Add New Data"
                                                                                                                Height="18px" Width="88px" AutoPostBack="False">
                                                                                                                <ClientSideEvents Click="function(s, e) {gridEmail.AddNewRow();}" />
                                                                                                            </dxe:ASPxButton>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </TitlePanel>--%>
                                                                                            </Templates>
                                                                                            <SettingsEditing EditFormColumnCount="1" Mode="PopupEditForm" PopupEditFormHorizontalAlign="Center"
                                                                                                PopupEditFormModal="True" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="600px" />
                                                                                            <SettingsText PopupEditFormCaption="Add/Modify Email" />
                                                                                            <Styles>
                                                                                                <LoadingPanel ImageSpacing="10px">
                                                                                                </LoadingPanel>
                                                                                                <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                                                                </Header>
                                                                                            </Styles>
                                                                                            <Settings ShowStatusBar="Visible" ShowTitlePanel="True" ShowGroupPanel="True" />
                                                                                            <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True" />
                                                                                            <SettingsPager NumericButtonCount="20" PageSize="20">
                                                                                            </SettingsPager>
                                                                                            <Columns>
                                                                                                <dxe:GridViewDataTextColumn Visible="False" ReadOnly="True" VisibleIndex="0" FieldName="eml_id">
                                                                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                                                                </dxe:GridViewDataTextColumn>
                                                                                                <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="0" FieldName="eml_internalId">
                                                                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                                                                </dxe:GridViewDataTextColumn>
                                                                                                <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="0" FieldName="eml_entity">
                                                                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                                                                </dxe:GridViewDataTextColumn>
                                                                                                <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="0" FieldName="eml_cntId">
                                                                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                                                                </dxe:GridViewDataTextColumn>
                                                                                                <dxe:GridViewDataComboBoxColumn FieldName="eml_type" Caption="Type" VisibleIndex="0">
                                                                                                    <PropertiesComboBox ValueType="System.String" EnableSynchronization="False" EnableIncrementalFiltering="True"
                                                                                                        Width="300px">
                                                                                                        <ClientSideEvents SelectedIndexChanged="function(s, e) {
	                                                                                                                                    var value = s.GetValue();
                                                                                                                                        if(value == &quot;Web Site&quot;)
                                                                                                                                        {
                                                                                                                                             gridEmail.GetEditor(&quot;eml_email&quot;).SetVisible(false);
                                                                                                                                             gridEmail.GetEditor(&quot;eml_ccEmail&quot;).SetVisible(false);
                                                                                                                                             gridEmail.GetEditor(&quot;eml_website&quot;).SetVisible(true);
                                                                                                                                        }
                                                                                                                                        else
                                                                                                                                        {
                                                                                                                                             gridEmail.GetEditor(&quot;eml_email&quot;).SetVisible(true);
                                                                                                                                             gridEmail.GetEditor(&quot;eml_ccEmail&quot;).SetVisible(true);
                                                                                                                                             gridEmail.GetEditor(&quot;eml_website&quot;).SetVisible(true);
                                                                                                                                        }
                                                                                                                                    }" />
                                                                                                                                                                                                                                            <ClientSideEvents Init="function(s, e) {
	                                                                                                                                    var value = s.GetValue();
                                                                                                                                        if(value == &quot;Web Site&quot;)
                                                                                                                                        {
                                                                                                                                             gridEmail.GetEditor(&quot;eml_email&quot;).SetVisible(false);
                                                                                                                                             gridEmail.GetEditor(&quot;eml_ccEmail&quot;).SetVisible(false);
                                                                                                                                             gridEmail.GetEditor(&quot;eml_website&quot;).SetVisible(true);
                                                                                                                                        }
                                                                                                                                        else
                                                                                                                                        {
                                                                                                                                             gridEmail.GetEditor(&quot;eml_email&quot;).SetVisible(true);
                                                                                                                                             gridEmail.GetEditor(&quot;eml_ccEmail&quot;).SetVisible(true);
                                                                                                                                             gridEmail.GetEditor(&quot;eml_website&quot;).SetVisible(true);
                                                                                                                                        }
                                                                                                                                    }" />
                                                                                                        <Items>
                                                                                                            <dxe:ListEditItem Text="N/A" Value="N/A"></dxe:ListEditItem>
                                                                                                            <dxe:ListEditItem Text="Personal" Value="Personal"></dxe:ListEditItem>
                                                                                                            <dxe:ListEditItem Text="Official" Value="Official"></dxe:ListEditItem>
                                                                                                            <dxe:ListEditItem Text="Web Site" Value="Web Site"></dxe:ListEditItem>
                                                                                                        </Items>
                                                                                                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="right" SetFocusOnError="True">
                                                                                                            <RequiredField ErrorText="Mandatory." IsRequired="True" />
                                                                                                        </ValidationSettings>
                                                                                                    </PropertiesComboBox>
                                                                                                    <EditFormSettings Visible="True" Caption="Email Type"></EditFormSettings>
                                                                                                    <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                                                                                    </EditCellStyle>
                                                                                                    <CellStyle Wrap="False">
                                                                                                    </CellStyle>
                                                                                                    <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right" VerticalAlign="Top">
                                                                                                    </EditFormCaptionStyle>
                                                                                                </dxe:GridViewDataComboBoxColumn>
                                                                                                <dxe:GridViewDataTextColumn FieldName="eml_email" VisibleIndex="1" Caption="Email">
                                                                                                    <EditFormSettings Caption="Email ID" Visible="True" />
                                                                                                    <CellStyle CssClass="gridcellleft">
                                                                                                    </CellStyle>
                                                                                                    <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                                                    </EditFormCaptionStyle>
                                                                                                    <PropertiesTextEdit>
                                                                                                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="right" SetFocusOnError="True">
                                                                                                            <RegularExpression ErrorText="Enter Valid Email ID." ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                                                                                                        </ValidationSettings>
                                                                                                    </PropertiesTextEdit>
                                                                                                </dxe:GridViewDataTextColumn>
                                                                                                <dxe:GridViewDataTextColumn FieldName="eml_ccEmail" VisibleIndex="1" Visible="False">
                                                                                                    <EditFormSettings Caption="CC Email" Visible="True" />
                                                                                                    <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                                                    </EditFormCaptionStyle>
                                                                                                    <PropertiesTextEdit>
                                                                                                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="right" SetFocusOnError="True">
                                                                                                            <RegularExpression ErrorText="Enter Valid CC Email ID." ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                                                                                                        </ValidationSettings>
                                                                                                    </PropertiesTextEdit>
                                                                                                </dxe:GridViewDataTextColumn>
                                                                                                <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="2" FieldName="eml_website">
                                                                                                    <EditFormSettings Visible="True" Caption="Web Site"></EditFormSettings>
                                                                                                    <PropertiesTextEdit Width="300px">
                                                                                                    </PropertiesTextEdit>
                                                                                                    <EditFormCaptionStyle HorizontalAlign="Right" Wrap="False">
                                                                                                    </EditFormCaptionStyle>
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
                                                                                                <dxe:GridViewCommandColumn VisibleIndex="2" ShowDeleteButton="true" ShowEditButton="true">
                                                                                                    <HeaderTemplate>
                                                                                                        Actions
                                                                                                    </HeaderTemplate>
                                                                                                </dxe:GridViewCommandColumn>
                                                                                            </Columns>
                                                                                            <SettingsSearchPanel Visible="True" />
                                                                                             <settings showstatusbar="Visible" showgrouppanel="True" showfilterrow="true" showfilterrowmenu="True" />
                                                                                            <SettingsCommandButton>
                                                                                               
                                                                                                <EditButton Image-Url="../../../assests/images/Edit.png" ButtonType="Image" Image-AlternateText="Edit" Styles-Style-CssClass="pad">
                                                                                                </EditButton>
                                                                                                 <DeleteButton Image-Url="../../../assests/images/Delete.png" ButtonType="Image" Image-AlternateText="Delete">
                                                                                                </DeleteButton>
                                                                                                <UpdateButton Text="Save" ButtonType="Button" Styles-Style-CssClass="btn btn-primary"></UpdateButton>
                                                                                                <CancelButton Text="Cancel" ButtonType="Button" Styles-Style-CssClass="btn btn-danger "></CancelButton>
                                                                                            </SettingsCommandButton>
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
                                            
                                        </tr>
                                    </table>
                                </div>
                            </EditForm>
                        </templates>
                        <settingsbehavior columnresizemode="NextColumn" confirmdelete="True" allowfocusedrow="false" />
                        <styles>
                            <%--   <Header SortingImageSpacing="5px" BackColor="#95C9FD" ImageSpacing="5px">
                            </Header>
                            <Cell CssClass="gridcellleft">
                            </Cell>
                            <FocusedRow CssClass="gridselectrow">
                            </FocusedRow>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                            <FocusedGroupRow CssClass="gridselectrow">
                            </FocusedGroupRow>
                            <GroupRow BackColor="#95C9FD">
                            </GroupRow>--%>
                        </styles>
                        <settingspager numericbuttoncount="15" pagesize="20" showseparators="True">
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </settingspager>
                        <columns>
                            <dxe:GridViewDataTextColumn Visible="False" ReadOnly="True" VisibleIndex="0" FieldName="bnk_id">
                                <EditFormSettings Visible="False"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="0" FieldName="bnk_internalId">
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="0" FieldName="bnk_bankName" Caption="Bank Name">
                                <EditCellStyle Wrap="False" HorizontalAlign="Left">
                                </EditCellStyle>
                                <PropertiesTextEdit Width="300px" MaxLength="200">
                                    <ValidationSettings SetFocusOnError="True" ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="right">
                                        <RequiredField IsRequired="True" ErrorText="Mandatory."></RequiredField>
                                    </ValidationSettings>
                                </PropertiesTextEdit>
                                <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right" VerticalAlign="Top">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="True"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="bnk_branchName" Caption="Branch">
                                <EditCellStyle Wrap="False" HorizontalAlign="Left">
                                </EditCellStyle>
                                <PropertiesTextEdit Width="300px" MaxLength="200">
                                </PropertiesTextEdit>
                                <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right" VerticalAlign="Top">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="True"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="bnk_micrno" Caption="MICR Code">
                                <EditCellStyle HorizontalAlign="Left">
                                </EditCellStyle>
                                <PropertiesTextEdit Width="300px" MaxLength="9">
                                </PropertiesTextEdit>
                                <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right" VerticalAlign="Top">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="True"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="bnk_IFSCCode" Caption="IFSC Code">
                                <EditCellStyle HorizontalAlign="Left">
                                </EditCellStyle>
                                <PropertiesTextEdit Width="300px" MaxLength="11">
                                </PropertiesTextEdit>
                                <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right" VerticalAlign="Top">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="True"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="bnk_NEFTCode" Caption="NEFT Code">
                                <EditCellStyle HorizontalAlign="Left">
                                </EditCellStyle>
                                <PropertiesTextEdit Width="300px" MaxLength="11">
                                </PropertiesTextEdit>
                                <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right" VerticalAlign="Top">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="True"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="bnk_RTGSCode" Caption="RTGS Code">
                                <EditCellStyle HorizontalAlign="Left">
                                </EditCellStyle>
                                <PropertiesTextEdit Width="300px" MaxLength="11">
                                </PropertiesTextEdit>
                                <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right" VerticalAlign="Top">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="True"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>


                            <%-- Mantis Issue Number #16918 --%>
                            <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="bnk_AcNo" Caption="Account No.">
                                <EditCellStyle HorizontalAlign="Left">
                                </EditCellStyle>
                                <PropertiesTextEdit Width="300px" MaxLength="11">
                                </PropertiesTextEdit>
                                <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right" VerticalAlign="Top">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="True"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="bnk_SwiftCode" Caption="SWIFT Code">
                                <EditCellStyle HorizontalAlign="Left">
                                </EditCellStyle>
                                <PropertiesTextEdit Width="300px" MaxLength="11">
                                </PropertiesTextEdit>
                                <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right" VerticalAlign="Top">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="True"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="bnk_Remarks" Caption="Remarks">
                                <EditCellStyle HorizontalAlign="Left">
                                </EditCellStyle>
                                <PropertiesTextEdit Width="300px" MaxLength="11">
                                </PropertiesTextEdit>
                                <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right" VerticalAlign="Top">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="True"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <%--Mantis Issue 0023983--%>
                             <dxe:GridViewDataTextColumn VisibleIndex="8" FieldName="bnk_Active_View" Caption="Status">
                                <EditCellStyle HorizontalAlign="Left">
                                </EditCellStyle>
                                <PropertiesTextEdit Width="300px" MaxLength="11">
                                </PropertiesTextEdit>
                                <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right" VerticalAlign="Top">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="True"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <%--End of Mantis Issue 0023983--%>


                              <%--End Mantis Number #16918--%>
                          <%--  code Add and Commented by Priti On 26122016 to add new page bank_general.aspx and Bank_correspondence--%>
                           <%-- <dxe:GridViewCommandColumn Caption="Actions" VisibleIndex="5" ShowEditButton="true" ShowDeleteButton="true" Width="70px">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <HeaderTemplate>
                                    Actions
                                </HeaderTemplate>
                                <CellStyle HorizontalAlign="Center"></CellStyle>
                            </dxe:GridViewCommandColumn>--%>
                           <%-- ...end...--%>
                            <%--<dxe:GridViewDataTextColumn VisibleIndex="6" Width="50px">
                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                <DataItemTemplate>
                                    <asp:Panel ID="pnlDelete" runat="server" Visible='<%#IsDeletable(Eval("bnk_internalId")) %>'>
                                        <a href="javascript:void(0);" onclick="DeleteRow('<%# Container.KeyValue %>')">
                                            <img src="../../../assests/images/Delete.png" alt="Delete"></a>
                                    </asp:Panel>
                                </DataItemTemplate>
                                <CellStyle Wrap="False">
                                </CellStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>A
                            </dxe:GridViewDataTextColumn>--%>
                              <%--  code Add and Commented by Priti On 26122016 to add new page bank_general.aspx and Bank_correspondence--%>
                             <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="11" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" Width="14%" >

                                 <CellStyle HorizontalAlign="Center"></CellStyle>
                                <HeaderTemplate>Actions</HeaderTemplate>
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <DataItemTemplate>
                                     <a href="javascript:void(0);" onclick="ClickOnMoreInfo('<%# Eval("bnk_id") %>')" title=" More Info" class="pad"  style="text-decoration: none;">
                                        <img src="../../../assests/images/info.png" />
                                         </a>
                                    <a href="javascript:void(0);" onclick="fn_DeleteEmp('<%#Eval("bnk_internalId") %>')" title="Delete">
                                    <img src="../../../assests/images/Delete.png" />
                                        </a>
                                </DataItemTemplate>
                                 <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                              <%-- ...end...--%>
                        </columns>
                        <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                        <settingspager pagesize="50">
                            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="50,100,150,200"/>
                              </settingspager>
                        <settingsbehavior confirmdelete="True" />
                        <settingsediting editformcolumncount="1" Mode="PopupEditForm" popupeditformhorizontalalign="Center" UseFormLayout="true" 
                          popupeditformverticalalign="WindowCenter" popupeditformwidth="1020px"  />
                        <settingstext popupeditformcaption="Add/Modify Bank Details" confirmdelete="Confirm delete?" />
                        <SettingsSearchPanel Visible="True" />
                        <settings showstatusbar="Visible" showgrouppanel="True" showfilterrow="true" showfilterrowmenu="True" />
                        <settingscommandbutton>
                           
                            <EditButton Image-Url="../../../assests/images/Edit.png" ButtonType="Image" Image-AlternateText="Edit" Styles-Style-CssClass="pad">
                            </EditButton>
                             <DeleteButton Image-Url="../../../assests/images/Delete.png" ButtonType="Image" Image-AlternateText="Delete">
                            </DeleteButton>
                            <UpdateButton Text="Save" ButtonType="Button" Styles-Style-CssClass="btn btn-primary "></UpdateButton>
                            <CancelButton Text="Cancel" ButtonType="Button" Styles-Style-CssClass="btn btn-danger"></CancelButton>
                        </settingscommandbutton>
                        <clientsideevents endcallback="function(s, e) {
	Emailcheck(s.cpHeight);
}"></clientsideevents>
                    </dxe:ASPxGridView>
                </td>
            </tr>
        </table>
        <%--===================================================== Master Grid =====================================================================--%>
        <%--========================================================== End Of Master Grid =====================================================--%>
        <%--======================================================== Master Grid DataSource ======================================================--%>
        <asp:SqlDataSource ID="BankDataSource" runat="server" 
            InsertCommand="BankInsert" InsertCommandType="StoredProcedure" SelectCommand="BankSelect"
            SelectCommandType="StoredProcedure" UpdateCommand="BankUpdate" UpdateCommandType="StoredProcedure"
            OnUpdated="BankDataSource_Updated" OnUpdating="BankDataSource_Updating" DeleteCommand="BankDelete" DeleteCommandType="StoredProcedure">
            <DeleteParameters>
                <asp:Parameter Name="bnk_internalId" Type="string" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="bnk_bankName" Type="String" />
                <asp:Parameter Name="bnk_micrno" Type="String" />
                <asp:Parameter Name="bnk_IFSCCode" Type="String" />
                <asp:Parameter Name="bnk_branchName" Type="String" />
                <asp:SessionParameter Name="CreateUser" Type="Decimal" SessionField="userid" />
                <asp:Parameter Name="bnk_internalId" Type="string" />
                <asp:Parameter Name="bnk_NEFTCode" Type="String" />
                <asp:Parameter Name="bnk_RTGSCode" Type="String" />
                <%-- Mantis Issue Number #16918 --%>
                <asp:Parameter Name="bnk_AcNo" Type="String" />
                <asp:Parameter Name="bnk_SwiftCode" Type="String" />
                <asp:Parameter Name="bnk_Remarks" Type="String" />
                <%-- End Mantis Issue Number #16918 --%>
                <%-- Mantis Issue 0023983 --%>
                <asp:Parameter Name="bnk_Active" Type="Boolean" />
                <%-- End of Mantis Issue 0023983  --%>
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="bnk_bankName" Type="String" />
                <asp:Parameter Name="bnk_micrno" Type="String" />
                <asp:Parameter Name="bnk_IFSCCode" Type="String" />
                <asp:Parameter Name="bnk_branchName" Type="String" />
                <asp:SessionParameter Name="CreateUser" Type="Decimal" SessionField="userid" />
                <asp:Parameter Name="bnk_NEFTCode" Type="String" />
                <asp:Parameter Name="bnk_RTGSCode" Type="String" />
                <%-- Mantis Issue Number #16918 --%>
                <asp:Parameter Name="bnk_AcNo" Type="String" />
                <asp:Parameter Name="bnk_SwiftCode" Type="String" />
                <asp:Parameter Name="bnk_Remarks" Type="String" />
                <%-- End Mantis Issue Number #16918 --%>
                <%-- Mantis Issue 0023983 --%>
                <asp:Parameter Name="bnk_Active" Type="Boolean" />
                <%-- End of Mantis Issue 0023983  --%>
            </InsertParameters>
        </asp:SqlDataSource>
        <%--========================================================= End of Master Grid Datasource=================================================--%>
        <%--========================================================== Address Data Source ========================================--%>
        <asp:SqlDataSource runat="server" ID="BankAddress" SelectCommand="select DISTINCT  tbl_master_address.add_id AS add_id,tbl_master_address.add_entity As add_entity,tbl_master_address.add_cntId As add_cntId, tbl_master_address.add_addressType AS add_addressType,
                        tbl_master_address.add_address1 AS add_address1,  tbl_master_address.add_address2 AS add_address2, 
                        tbl_master_address.add_address3 AS add_address3,tbl_master_address.add_landMark AS add_landMark, 
                        tbl_master_address.add_country AS Country, 
                        tbl_master_address.add_state AS State,tbl_master_address.add_pin AS add_pin, 
                        CASE isnull(add_country, '')WHEN '' THEN '0' ELSE(SELECT cou_country FROM tbl_master_country WHERE cou_id = add_country) END AS Country1, 
                        CASE isnull(add_state, '') WHEN '' THEN '0' ELSE(SELECT state FROM tbl_master_state WHERE id = add_state) END AS State1,
                        CASE isnull(add_city, '') WHEN '' THEN '0' ELSE(SELECT city_name FROM tbl_master_city WHERE city_id = add_city) END AS City1, 
                        tbl_master_address.add_city AS City, tbl_master_address.add_pin AS PinCode, tbl_master_address.add_landMark AS LankMark,tbl_master_address.add_activityId AS add_activityId,tbl_master_address.CreateDate AS CreateDate,tbl_master_address.CreateUser AS CreateUser,tbl_master_address.LastModifyDate AS LastModifyDate,tbl_master_address.LastModifyUser AS LastModifyUser 
                        from tbl_master_address where add_cntId=@bnk_internalId"
            OldValuesParameterFormatString="original_{0}"
            ConflictDetection="CompareAllValues" DeleteCommand="DELETE FROM [tbl_master_address] WHERE [add_id] = @original_add_id"
            InsertCommand="INSERT INTO [tbl_master_address] ([add_cntId],[add_entity],[add_addressType], [add_address1], [add_address2], [add_address3], [add_landMark], [add_country], [add_state], [add_city], [add_pin],[CreateDate],[CreateUser]) VALUES ( @bnk_internalId,'Bank',@add_addressType, @add_address1, @add_address2, @add_address3, @add_landMark, @Country, @State, @City, @add_pin,getdate(),@CreateUserAddress)"
           UpdateCommand="UPDATE [tbl_master_address] SET  [add_addressType] = @add_addressType, [add_address1] = @add_address1, [add_address2] = @add_address2, [add_address3] = @add_address3, [add_landMark] = @add_landMark, [add_country] = @Country, [add_state] = @State, [add_city] = @City, [add_pin] = @add_pin,[LastModifyDate]=getdate(),[LastModifyUser]=@CreateUserAddress  WHERE [add_id] = @original_add_id">
            <InsertParameters>
                <asp:SessionParameter SessionField="KeyVal_InternalID" Type="String" Name="bnk_internalId"></asp:SessionParameter>
                <asp:SessionParameter Name="CreateUserAddress" Type="Decimal" SessionField="userid" />
                <asp:Parameter Name="add_entity" Type="String" />
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
                <asp:SessionParameter SessionField="KeyVal_InternalID" Type="String" Name="bnk_internalId"></asp:SessionParameter>
            </SelectParameters>
            <UpdateParameters>
                <asp:SessionParameter Name="CreateUserAddress" Type="Decimal" SessionField="userid" />
                <asp:Parameter Type="String" Name="add_addressType"></asp:Parameter>
                <asp:Parameter Type="String" Name="add_address1"></asp:Parameter>
                <asp:Parameter Type="String" Name="add_address2"></asp:Parameter>
                <asp:Parameter Type="String" Name="add_address3"></asp:Parameter>
                <asp:Parameter Type="String" Name="add_landMark"></asp:Parameter>
                <asp:Parameter Name="Country" Type="int32" />
                <asp:Parameter Name="State" Type="int32" />
                <asp:Parameter Name="City" Type="string" />
                <asp:Parameter Type="String" Name="add_pin"></asp:Parameter>
            </UpdateParameters>
            <DeleteParameters>
                <asp:Parameter Type="Decimal" Name="original_add_id"></asp:Parameter>
            </DeleteParameters>
        </asp:SqlDataSource>
        <%--============================================================== End of Address DataSource ====================================--%>
        <%--============================================================ Phone Fax DataSource ===========================================--%>
        <asp:SqlDataSource ID="BankPhone" runat="server" ConflictDetection="CompareAllValues"
             DeleteCommand="DELETE FROM [tbl_master_phonefax] WHERE [phf_id] = @original_phf_id"
            InsertCommand="INSERT INTO [tbl_master_phonefax] ([phf_cntId],[phf_entity],[phf_type], [phf_countryCode], [phf_areaCode], [phf_phoneNumber], [phf_extension],[CreateDate],[CreateUser]) VALUES (@bnk_internalId,'Bank',@phf_type, @phf_countryCode, @phf_areaCode, @phf_phoneNumber, @phf_extension,getdate(),@CreateUserPhone)"
            OldValuesParameterFormatString="original_{0}" SelectCommand="SELECT * FROM [tbl_master_phonefax] where [phf_cntId]=@bnk_internalId"
            UpdateCommand="UPDATE [tbl_master_phonefax] SET  [phf_type] = @phf_type, [phf_countryCode] = @phf_countryCode, [phf_areaCode] = @phf_areaCode, [phf_phoneNumber] = @phf_phoneNumber, [phf_extension] = @phf_extension, [LastModifyDate]=getdate(),[LastModifyUser]=@CreateUserPhone WHERE [phf_id] = @original_phf_id"
            OnDeleted="BankPhone_Deleted">
            <DeleteParameters>
                <asp:Parameter Name="original_phf_id" Type="Decimal" />
            </DeleteParameters>
            <SelectParameters>
                <asp:SessionParameter SessionField="KeyVal_InternalID" Type="String" Name="bnk_internalId"></asp:SessionParameter>
            </SelectParameters>
            <UpdateParameters>
                <asp:Parameter Name="phf_type" Type="String" />
                <asp:Parameter Name="phf_countryCode" Type="String" />
                <asp:Parameter Name="phf_areaCode" Type="String" />
                <asp:Parameter Name="phf_phoneNumber" Type="String" />
                <asp:Parameter Name="phf_extension" Type="String" />
                <asp:SessionParameter Name="CreateUserPhone" Type="Decimal" SessionField="userid" />
            </UpdateParameters>
            <InsertParameters>
                <asp:SessionParameter SessionField="KeyVal_InternalID" Type="String" Name="bnk_internalId"></asp:SessionParameter>
                <asp:SessionParameter Name="CreateUserPhone" Type="Decimal" SessionField="userid" />
                <asp:Parameter Name="phf_entity" Type="String" />
                <asp:Parameter Name="phf_type" Type="String" />
                <asp:Parameter Name="phf_countryCode" Type="String" />
                <asp:Parameter Name="phf_areaCode" Type="String" />
                <asp:Parameter Name="phf_phoneNumber" Type="String" />
                <asp:Parameter Name="phf_extension" Type="String" />
            </InsertParameters>
        </asp:SqlDataSource>
        <%--================================================================ End of Phone Fax Data Source ========================================--%>
        <%--=================================================================== Email Data Source =============================================--%>
        <asp:SqlDataSource ID="BankEmail" runat="server" ConflictDetection="CompareAllValues"
            DeleteCommand="DELETE FROM [tbl_master_email] WHERE [eml_id] = @original_eml_id"
            InsertCommand="INSERT INTO [tbl_master_email] ([eml_cntId],[eml_entity],[eml_type], [eml_email], [eml_website],[CreateDate],[CreateUser]) VALUES (@bnk_internalId,'Bank',@eml_type, @eml_email, @eml_website,getdate(),@CreateUserEmail)"
            OldValuesParameterFormatString="original_{0}" SelectCommand="SELECT * FROM [tbl_master_email] where [eml_cntId]=@bnk_internalId "
            UpdateCommand="UPDATE [tbl_master_email] SET [eml_type] = @eml_type, [eml_email] = @eml_email, [eml_website] = @eml_website, [LastModifyDate]=getdate(),[LastModifyUser]=@CreateUserEmail WHERE [eml_id] = @original_eml_id">
            <DeleteParameters>
                <asp:Parameter Name="original_eml_id" Type="Decimal" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="eml_type" Type="String" />
                <asp:Parameter Name="eml_email" Type="String" />
                <asp:Parameter Name="eml_website" Type="String" />
                <asp:SessionParameter Name="CreateUserEmail" Type="Decimal" SessionField="userid" />
            </UpdateParameters>
            <SelectParameters>
                <asp:SessionParameter SessionField="KeyVal_InternalID" Type="String" Name="bnk_internalId"></asp:SessionParameter>
            </SelectParameters>
            <InsertParameters>
                <asp:SessionParameter SessionField="KeyVal_InternalID" Type="String" Name="bnk_internalId"></asp:SessionParameter>
                <asp:SessionParameter Name="CreateUserEmail" Type="Decimal" SessionField="userid" />
                <asp:Parameter Name="eml_internalId" Type="String" />
                <asp:Parameter Name="eml_entity" Type="String" />
                <asp:Parameter Name="eml_cntId" Type="String" />
                <asp:Parameter Name="eml_type" Type="String" />
                <asp:Parameter Name="eml_email" Type="String" />
                <asp:Parameter Name="eml_website" Type="String" />
                <asp:Parameter Name="CreateDate" Type="String" />
                <asp:Parameter Name="CreateUser" Type="Decimal" />
                <asp:Parameter Name="LastModifyDate" Type="String" />
                <asp:Parameter Name="LastModifyUser" Type="Decimal" />
            </InsertParameters>
        </asp:SqlDataSource>
        <%--================================================================  End Of Email Data source  ======================================--%>
        <%--=================================== Country datasource ===================================================--%>
        <asp:SqlDataSource ID="CountrySelect" runat="server" 
            SelectCommand="SELECT cou_id, cou_country as Country FROM tbl_master_country order by cou_country"></asp:SqlDataSource>
        <%--=================================== State datasource ===================================================--%>
        <asp:SqlDataSource ID="StateSelect" runat="server"
            SelectCommand="SELECT s.id as ID,s.state as State from tbl_master_state s,tbl_master_country cr where (s.countryId = cr.cou_id) and (cr.cou_id = @State) ORDER BY s.state">
            <SelectParameters>
                <asp:Parameter Name="State" Type="string" />
            </SelectParameters>
        </asp:SqlDataSource>
        <%--=================================== City Datasource ===================================================--%>
        <asp:SqlDataSource ID="SelectCity" runat="server" 
            SelectCommand="SELECT c.city_id AS CityId, c.city_name AS City FROM tbl_master_city c where c.state_id=@City order by c.city_name">
            <SelectParameters>
                <asp:Parameter Name="City" Type="string" />
            </SelectParameters>
        </asp:SqlDataSource>
        <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
    </div>
</asp:Content>
