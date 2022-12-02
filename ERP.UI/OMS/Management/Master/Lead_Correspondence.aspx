<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.Master.management_Master_Lead_Correspondence" CodeBehind="Lead_Correspondence.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function OnPhoneClick() {
            gridPhone.UpdateEdit();
            //if (gridPhone.GetEditor('phf_phoneNumber').GetValue() == null) {
            //    alert('Phone Number Required');
            //}
            //else {
            //    gridPhone.UpdateEdit();
            //}
        }
        function OnEmailClick() {
            gridEmail.UpdateEdit();
            //if (gridEmail.GetEditor('eml_type').GetValue() == 'Web Site') {
            //    if (gridEmail.GetEditor('eml_website').GetValue() == null)
            //        alert('Url Required');
            //    else
            //        gridEmail.UpdateEdit();
            //}
            //else {
            //    if (gridEmail.GetEditor('eml_email').GetValue() == null)
            //        alert('Email Required');
            //    else
            //        gridEmail.UpdateEdit();
            //}
        }


        function Emailcheck(obj) {
            if (obj == 'c') {
                alert("This emailid has already exists for other contacts.");
            }

        }

    </script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript">
        function OnCountryChanged(cmbCountry) {
            gridAddress.GetEditor("State").PerformCallback(cmbCountry.GetValue().toString());
        }
        function OnStateChanged(cmbState) {
            gridAddress.GetEditor("City").PerformCallback(cmbState.GetValue().toString());
        }
        function OnCityChanged(cmbCity) {
            gridAddress.GetEditor("area").PerformCallback(cmbCity.GetValue().toString());
        }
        function OnChildCall(cmbCity) {
            OnCityChanged(gridAddress.GetEditor("City"));
        }
        function disp_prompt(name) {
            if (name == "tab0") {
                //alert(name);
                document.location.href = "Lead_general.aspx";
            }
            if (name == "tab1") {
                //alert(name);
                //document.location.href="Lead_Correspondence.aspx"; 
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
        function openAreaPage() {
            var left = (screen.width - 300) / 2;
            var top = (screen.height - 250) / 2;
            var cityid = gridAddress.GetEditor("City").GetValue();
            var cityname = gridAddress.GetEditor("City").GetText();
            var URL = 'AddArea_PopUp.aspx?id=' + cityid + '&name=' + cityname + '';
            if (cityid != null) {
                window.open(URL, '50', 'resizable=1,height=100px,width=300px,top=' + top + ',left=' + left + '');
            }
            else {
                alert('Please select a city first!');
                return false;
            }
        }
    </script>
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Lead Correspondence</h3>
        </div>
        <div class="crossBtn"><a href="Lead.aspx"><i class="fa fa-times"></i></a></div>
    </div>
    <div class="form_main">
        <table class="TableMain100">
            <tr>
                <td>
                    <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="1" ClientInstanceName="page">
                        <TabPages>
                            <dxe:TabPage Name="General" Text="General">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="CorresPondence" Text="CorresPondence">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                        <dxe:ASPxPageControl ID="ASPxPageControl2" runat="server" ActiveTabIndex="1" ClientInstanceName="page">
                                            <TabPages>
                                                <dxe:TabPage Text="Address">
                                                    <ContentCollection>
                                                        <dxe:ContentControl runat="server">

                                                            <dxe:ASPxGridView runat="server" ClientInstanceName="gridAddress" KeyFieldName="Id"
                                                                AutoGenerateColumns="False" DataSourceID="Address" Width="100%" Font-Size="12px"
                                                                ID="AddressGrid" OnCellEditorInitialize="AddressGrid_CellEditorInitialize">
                                                                <Columns>
                                                                    <dxe:GridViewDataTextColumn FieldName="Id" Caption="Id" Visible="False" VisibleIndex="0">
                                                                        <EditFormSettings Visible="False"></EditFormSettings>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataComboBoxColumn FieldName="Type" Caption="Address Type" Visible="False"
                                                                        VisibleIndex="0">
                                                                        <PropertiesComboBox ValueType="System.String" EnableIncrementalFiltering="True">
                                                                            <Items>
                                                                                <dxe:ListEditItem Text="Residence" Value="Residence"></dxe:ListEditItem>
                                                                                <dxe:ListEditItem Text="Office" Value="Office"></dxe:ListEditItem>
                                                                                <dxe:ListEditItem Text="Correspondence" Value="Correspondence"></dxe:ListEditItem>
                                                                            </Items>
                                                                        </PropertiesComboBox>
                                                                        <EditFormSettings Visible="True" VisibleIndex="1"></EditFormSettings>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                    </dxe:GridViewDataComboBoxColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="Type" Caption="Type" VisibleIndex="0">
                                                                        <EditFormSettings Visible="False"></EditFormSettings>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="Address1" Caption="Address1" VisibleIndex="1">
                                                                        <EditFormSettings Visible="True" VisibleIndex="2"></EditFormSettings>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="Address2" Caption="Address2" VisibleIndex="2">
                                                                        <EditFormSettings Visible="True" VisibleIndex="3"></EditFormSettings>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="Address3" Caption="Address3" VisibleIndex="3">
                                                                        <EditFormSettings Visible="True" VisibleIndex="4"></EditFormSettings>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="LandMark" Caption="LandMark" VisibleIndex="4">
                                                                        <EditFormSettings Visible="True" VisibleIndex="5"></EditFormSettings>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataComboBoxColumn FieldName="Country" Caption="Country" Visible="False"
                                                                        VisibleIndex="0">
                                                                        <PropertiesComboBox EnableIncrementalFiltering="True" EnableSynchronization="False"
                                                                            DataSourceID="CountrySelect" TextField="Country" ValueField="cou_id" ValueType="System.String">
                                                                            <ClientSideEvents SelectedIndexChanged="function(s, e) { OnCountryChanged(s); }"></ClientSideEvents>
                                                                        </PropertiesComboBox>
                                                                        <EditFormSettings Visible="True" VisibleIndex="6"></EditFormSettings>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                    </dxe:GridViewDataComboBoxColumn>
                                                                    <dxe:GridViewDataComboBoxColumn FieldName="State" Caption="State" Visible="False"
                                                                        VisibleIndex="0">
                                                                        <PropertiesComboBox EnableIncrementalFiltering="True" EnableSynchronization="False"
                                                                            DataSourceID="StateSelect" TextField="State" ValueField="ID" ValueType="System.String">
                                                                            <ClientSideEvents SelectedIndexChanged="function(s, e) { OnStateChanged(s); }"></ClientSideEvents>
                                                                        </PropertiesComboBox>
                                                                        <EditFormSettings Visible="True" VisibleIndex="7"></EditFormSettings>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                    </dxe:GridViewDataComboBoxColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="Country1" Caption="Country" VisibleIndex="5">
                                                                        <EditFormSettings Visible="False"></EditFormSettings>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="State1" Caption="State" VisibleIndex="6">
                                                                        <EditFormSettings Visible="False"></EditFormSettings>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="City1" Caption="City" VisibleIndex="7">
                                                                        <EditFormSettings Visible="False"></EditFormSettings>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataComboBoxColumn FieldName="City" Caption="City" Visible="False"
                                                                        VisibleIndex="0">
                                                                        <PropertiesComboBox EnableIncrementalFiltering="True" EnableSynchronization="False"
                                                                            DataSourceID="SelectCity" TextField="City" ValueField="CityId" ValueType="System.String">
                                                                            <ClientSideEvents SelectedIndexChanged="function(s, e) { OnCityChanged(s); }"></ClientSideEvents>
                                                                        </PropertiesComboBox>
                                                                        <EditFormSettings Visible="True" VisibleIndex="8"></EditFormSettings>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                    </dxe:GridViewDataComboBoxColumn>
                                                                    <dxe:GridViewDataComboBoxColumn FieldName="area" Caption="Area" VisibleIndex="8">
                                                                        <PropertiesComboBox EnableIncrementalFiltering="True" EnableSynchronization="False"
                                                                            DataSourceID="SelectArea" TextField="area_name" ValueField="area_id" ValueType="System.Int32">
                                                                        </PropertiesComboBox>
                                                                        <EditFormSettings Visible="True" VisibleIndex="9" Caption="Area"></EditFormSettings>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                    </dxe:GridViewDataComboBoxColumn>
                                                                    <dxe:GridViewDataHyperLinkColumn Visible="False" VisibleIndex="10">
                                                                        <EditFormSettings Visible="True" VisibleIndex="10"></EditFormSettings>
                                                                        <EditItemTemplate>
                                                                            <a href="#" onclick="javascript:openAreaPage();"><span class="Ecoheadtxt">
                                                                                <strong>Add New Area</strong></span></a>
                                                                        </EditItemTemplate>
                                                                    </dxe:GridViewDataHyperLinkColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="PinCode" Caption="PinCode" VisibleIndex="9">
                                                                        <PropertiesTextEdit>
                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" SetFocusOnError="True">
                                                                                <RegularExpression ErrorText="Enter Valid PinCode" ValidationExpression="[0-9]{6}"></RegularExpression>
                                                                                <RequiredField IsRequired="True" ErrorText="PinCode Can Not Be Blank"></RequiredField>
                                                                            </ValidationSettings>
                                                                        </PropertiesTextEdit>
                                                                        <EditFormSettings Visible="True" VisibleIndex="11"></EditFormSettings>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewCommandColumn VisibleIndex="10" ShowCancelButton="true" ShowEditButton="true" HeaderStyle-HorizontalAlign="Center">
                                                                        <%--  <EditButton Visible="True">
                                                        </EditButton>
                                                        <DeleteButton Visible="True">
                                                        </DeleteButton>--%>
<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                        <HeaderTemplate>
                                                                            <a href="javascript:void(0);" onclick="gridAddress.AddNewRow();"><span>Add New</span> </a>
                                                                        </HeaderTemplate>
                                                                    </dxe:GridViewCommandColumn>
                                                                </Columns>
                                                                <SettingsCommandButton>
                                                                    <EditButton Image-Url="/assests/images/Edit.png" ButtonType="Image" Image-AlternateText="Edit">
<Image AlternateText="Edit" Url="/assests/images/Edit.png"></Image>
                                                                    </EditButton>
                                                                    <UpdateButton Text="Update" ButtonType="Button" Styles-Style-CssClass="btn btn-primary">
<Styles>
<Style CssClass="btn btn-primary"></Style>
</Styles>
                                                                    </UpdateButton>
                                                                    <CancelButton Text="Cancel" ButtonType="Button" Styles-Style-CssClass="btn btn-danger">
<Styles>
<Style CssClass="btn btn-danger"></Style>
</Styles>
                                                                    </CancelButton>
                                                                </SettingsCommandButton>
                                                                <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True"></SettingsBehavior>
                                                                <SettingsPager PageSize="20" NumericButtonCount="20" ShowSeparators="True">
                                                                    <FirstPageButton Visible="True">
                                                                    </FirstPageButton>
                                                                    <LastPageButton Visible="True">
                                                                    </LastPageButton>
                                                                </SettingsPager>
                                                                <SettingsEditing Mode="PopupEditForm" PopupEditFormWidth="700px"
                                                                    PopupEditFormHorizontalAlign="Center" PopupEditFormVerticalAlign="WindowCenter"
                                                                    PopupEditFormModal="True" EditFormColumnCount="1">
                                                                </SettingsEditing>
                                                                <Settings ShowTitlePanel="True" ShowStatusBar="Visible" ShowGroupPanel="true"></Settings>
                                                                <SettingsText ConfirmDelete="Confirm delete?" PopupEditFormCaption="Add/Modify Address"></SettingsText>
                                                                <Styles>
                                                                    <Header SortingImageSpacing="5px" ImageSpacing="5px">
                                                                    </Header>
                                                                    <LoadingPanel ImageSpacing="10px">
                                                                    </LoadingPanel>
                                                                </Styles>
                                                                <Templates>
                                                                    <EditForm>
                                                                        <table style="width: 100%">
                                                                            <tr>
                                                                                <td style="width: 100%">
                                                                                    <dxe:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormEditors" ColumnID="" ID="Editors1"></dxe:ASPxGridViewTemplateReplacement>
                                                                                    <div style="text-align: center; padding: 2px 2px 2px 2px">
                                                                                        <dxe:ASPxGridViewTemplateReplacement ID="UpdateButton1" ReplacementType="EditFormUpdateButton"
                                                                                            runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                                                        <dxe:ASPxGridViewTemplateReplacement ID="CancelButton1" ReplacementType="EditFormCancelButton"
                                                                                            runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </EditForm>
                                                                </Templates>
                                                            </dxe:ASPxGridView>
                                                        </dxe:ContentControl>
                                                    </ContentCollection>
                                                </dxe:TabPage>
                                                <dxe:TabPage Text="Phone">
                                                    <ContentCollection>
                                                        <dxe:ContentControl runat="server">
                                                            <dxe:ASPxGridView runat="server" ClientInstanceName="gridPhone" KeyFieldName="phf_id"
                                                                AutoGenerateColumns="False" DataSourceID="Phone" Width="100%" Font-Size="12px"
                                                                ID="PhoneGrid" OnRowValidating="PhoneGrid_RowValidating">
                                                                <Columns>
                                                                    <dxe:GridViewDataTextColumn FieldName="phf_id" ReadOnly="True" Visible="False"
                                                                        VisibleIndex="0">
                                                                        <EditFormSettings Visible="False"></EditFormSettings>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="phf_cntId" ReadOnly="True" Visible="False"
                                                                        VisibleIndex="0">
                                                                        <EditFormSettings Visible="False"></EditFormSettings>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="phf_entity" ReadOnly="True" Visible="False"
                                                                        VisibleIndex="0">
                                                                        <EditFormSettings Visible="False"></EditFormSettings>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataComboBoxColumn FieldName="phf_type" Caption="Phone Type" Visible="False"
                                                                        VisibleIndex="0">
                                                                        <PropertiesComboBox ValueType="System.String" EnableIncrementalFiltering="True">
                                                                            <ClientSideEvents SelectedIndexChanged="function(s, e) {
	var value = s.GetValue();
    if(value == &quot;Mobile&quot;)
    {
        $('#PhoneGrid_DXPEForm_efnew_DXEFL_1,#PhoneGrid_DXPEForm_efnew_DXEFL_2,#PhoneGrid_DXPEForm_efnew_DXEFL_4').hide();
         gridPhone.GetEditor(&quot;phf_countryCode&quot;).SetVisible(false);
         gridPhone.GetEditor(&quot;phf_areaCode&quot;).SetVisible(false);
         gridPhone.GetEditor(&quot;phf_extension&quot;).SetVisible(false);
        $('#PhoneGrid_DXPEForm_ef0_DXEFL_0_1,#PhoneGrid_DXPEForm_ef0_DXEFL_0_2,#PhoneGrid_DXPEForm_ef0_DXEFL_0_4').hide();
    }
    else
    {
        $('#PhoneGrid_DXPEForm_efnew_DXEFL_1,#PhoneGrid_DXPEForm_efnew_DXEFL_2,#PhoneGrid_DXPEForm_efnew_DXEFL_4').show();
         gridPhone.GetEditor(&quot;phf_countryCode&quot;).SetVisible(true);
         gridPhone.GetEditor(&quot;phf_areaCode&quot;).SetVisible(true);
         gridPhone.GetEditor(&quot;phf_extension&quot;).SetVisible(true);
        $('#PhoneGrid_DXPEForm_ef0_DXEFL_0_1,#PhoneGrid_DXPEForm_ef0_DXEFL_0_2,#PhoneGrid_DXPEForm_ef0_DXEFL_0_4').show();
    }
}"
                                                                                Init="function(s, e) {
	var value = s.GetValue();
    if(value == &quot;Mobile&quot;)
    {
         gridPhone.GetEditor(&quot;phf_countryCode&quot;).SetVisible(false);
         gridPhone.GetEditor(&quot;phf_areaCode&quot;).SetVisible(false);
         gridPhone.GetEditor(&quot;phf_extension&quot;).SetVisible(false);
        $('#PhoneGrid_DXPEForm_efnew_DXEFL_1,#PhoneGrid_DXPEForm_efnew_DXEFL_2,#PhoneGrid_DXPEForm_efnew_DXEFL_4').hide();
        $('#PhoneGrid_DXPEForm_ef0_DXEFL_0_1,#PhoneGrid_DXPEForm_ef0_DXEFL_0_2,#PhoneGrid_DXPEForm_ef0_DXEFL_0_4').hide();
    }
    else
    {
         gridPhone.GetEditor(&quot;phf_countryCode&quot;).SetVisible(true);
         gridPhone.GetEditor(&quot;phf_areaCode&quot;).SetVisible(true);
         gridPhone.GetEditor(&quot;phf_extension&quot;).SetVisible(true);
         $('#PhoneGrid_DXPEForm_efnew_DXEFL_1,#PhoneGrid_DXPEForm_efnew_DXEFL_2,#PhoneGrid_DXPEForm_efnew_DXEFL_4').show();
         $('#PhoneGrid_DXPEForm_ef0_DXEFL_0_1,#PhoneGrid_DXPEForm_ef0_DXEFL_0_2,#PhoneGrid_DXPEForm_ef0_DXEFL_0_4').show();
    }
}"></ClientSideEvents>
                                                                            <Items>
                                                                                <dxe:ListEditItem Text="Residence" Value="Residence"></dxe:ListEditItem>
                                                                                <dxe:ListEditItem Text="Office" Value="Office"></dxe:ListEditItem>
                                                                                <dxe:ListEditItem Text="Correspondence" Value="Correspondence"></dxe:ListEditItem>
                                                                                <dxe:ListEditItem Text="Mobile" Value="Mobile"></dxe:ListEditItem>
                                                                                <dxe:ListEditItem Text="Fax" Value="Fax"></dxe:ListEditItem>
                                                                            </Items>
                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" SetFocusOnError="True">
                                                                                <RequiredField IsRequired="True" ErrorText="Select Phone Type"></RequiredField>
                                                                            </ValidationSettings>
                                                                        </PropertiesComboBox>
                                                                        <EditFormSettings Visible="True"></EditFormSettings>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                    </dxe:GridViewDataComboBoxColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="phf_type" Width="40%" Caption="Type" VisibleIndex="0">
                                                                        <EditFormSettings Visible="False" Caption="Phone Type"></EditFormSettings>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="phf_countryCode" Visible="False" VisibleIndex="1">
                                                                        <PropertiesTextEdit>
                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" SetFocusOnError="True">
                                                                                <RegularExpression ErrorText="Enter Valid CountryCode" ValidationExpression="[0-9]+"></RegularExpression>
                                                                            </ValidationSettings>
                                                                        </PropertiesTextEdit>
                                                                        <EditFormSettings Visible="True" Caption="Country Code"></EditFormSettings>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="phf_areaCode" Visible="False" VisibleIndex="1">
                                                                        <PropertiesTextEdit>
                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" SetFocusOnError="True">
                                                                                <RegularExpression ErrorText="Enter Valid AreaCode" ValidationExpression="[0-9]+"></RegularExpression>
                                                                            </ValidationSettings>
                                                                        </PropertiesTextEdit>
                                                                        <EditFormSettings Visible="True" Caption="Area Code"></EditFormSettings>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="phf_phoneNumber" Caption="Number" Visible="False"
                                                                        VisibleIndex="1">
                                                                        <PropertiesTextEdit>
                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" SetFocusOnError="True">
                                                                                <RequiredField IsRequired="True" ErrorText="Please Enter Phone Number"></RequiredField>
                                                                                <RegularExpression ErrorText="Enter Valid PhoneNumber" ValidationExpression="[0-9]+"></RegularExpression>
                                                                            </ValidationSettings>
                                                                        </PropertiesTextEdit>
                                                                        <EditFormSettings Visible="True"></EditFormSettings>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="Number" Width="40%" Caption="Phone Number"
                                                                        VisibleIndex="1">
                                                                        <PropertiesTextEdit>
                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" SetFocusOnError="True">
                                                                                <RequiredField IsRequired="True" ErrorText="Please Enter Phone Number"></RequiredField>
                                                                                <RegularExpression ErrorText="Enter Valid PhoneNumber" ValidationExpression="[0-9]+"></RegularExpression>
                                                                            </ValidationSettings>
                                                                        </PropertiesTextEdit>
                                                                        <EditFormSettings Visible="False"></EditFormSettings>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="phf_extension" Caption="Extension" Visible="False"
                                                                        VisibleIndex="2">
                                                                        <PropertiesTextEdit>
                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" SetFocusOnError="True">
                                                                                <RegularExpression ErrorText="Enter Valid Extension" ValidationExpression="[0-9]+"></RegularExpression>
                                                                            </ValidationSettings>
                                                                        </PropertiesTextEdit>
                                                                        <EditFormSettings Visible="True"></EditFormSettings>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewCommandColumn VisibleIndex="2" ShowDeleteButton="true" ShowEditButton="true" HeaderStyle-HorizontalAlign="Center">

<HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                                                        <HeaderTemplate>
                                                                            <a href="javascript:void(0);" onclick="gridPhone.AddNewRow();"><span>Add New</span> </a>
                                                                        </HeaderTemplate>
                                                                    </dxe:GridViewCommandColumn>
                                                                </Columns>
                                                                <SettingsCommandButton>


                                                                    <EditButton Image-Url="/assests/images/Edit.png" ButtonType="Image" Image-AlternateText="Edit">
<Image AlternateText="Edit" Url="/assests/images/Edit.png"></Image>
                                                                    </EditButton>
                                                                    <DeleteButton Image-Url="/assests/images/Delete.png" ButtonType="Image" Image-AlternateText="Delete">
<Image AlternateText="Delete" Url="/assests/images/Delete.png"></Image>
                                                                    </DeleteButton>
                                                                    <CancelButton Text="Cancel" ButtonType="Button" Styles-Style-CssClass="btn btn-danger">
<Styles>
<Style CssClass="btn btn-danger"></Style>
</Styles>
                                                                    </CancelButton>
                                                                </SettingsCommandButton>

                                                                <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True"></SettingsBehavior>
                                                                <SettingsPager PageSize="20" NumericButtonCount="20" ShowSeparators="True">
                                                                    <FirstPageButton Visible="True">
                                                                    </FirstPageButton>
                                                                    <LastPageButton Visible="True">
                                                                    </LastPageButton>
                                                                </SettingsPager>
                                                                <SettingsEditing Mode="PopupEditForm" PopupEditFormWidth="700px"
                                                                    PopupEditFormHorizontalAlign="Center" PopupEditFormVerticalAlign="WindowCenter"
                                                                    PopupEditFormModal="True" EditFormColumnCount="1">
                                                                </SettingsEditing>
                                                                <Settings ShowTitlePanel="True" ShowStatusBar="Visible" ShowGroupPanel="true"></Settings>
                                                                <SettingsText ConfirmDelete="Confirm delete?" PopupEditFormCaption="Add/Modify Phone"></SettingsText>
                                                                <Styles>
                                                                    <Header SortingImageSpacing="5px" ImageSpacing="5px">
                                                                    </Header>
                                                                    <LoadingPanel ImageSpacing="10px">
                                                                    </LoadingPanel>
                                                                </Styles>
                                                                <Templates>
                                                                    <EditForm>
                                                                        <table style="width: 100%">
                                                                            <tr>
                                                                                <td style="width: 100%">
                                                                                    <dxe:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormEditors" ColumnID="" ID="Editors2"></dxe:ASPxGridViewTemplateReplacement>
                                                                                    <div style="text-align: center; padding: 2px 2px 2px 2px">
                                                                                        <%--<dxe:ASPxGridViewTemplateReplacement ID="UpdateButton" ReplacementType="EditFormUpdateButton"
                                                                            runat="server">
                                                                        </dxe:ASPxGridViewTemplateReplacement>--%>
                                                                                        <a id="update" href="#" onclick="OnPhoneClick()" class="btn btn-primary" style="color: white">Update</a>
                                                                                        <dxe:ASPxGridViewTemplateReplacement ID="CancelButton2" ReplacementType="EditFormCancelButton"
                                                                                            runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                                                    </div>
                                                                                </td>
                                                                                <%-- <td style="width: 20%"></td>--%>
                                                                            </tr>
                                                                        </table>
                                                                    </EditForm>
                                                                </Templates>
                                                            </dxe:ASPxGridView>
                                                        </dxe:ContentControl>
                                                    </ContentCollection>
                                                </dxe:TabPage>
                                                <dxe:TabPage Text="Email">
                                                    <ContentCollection>
                                                        <dxe:ContentControl runat="server">
                                                            <dxe:ASPxGridView runat="server" ClientInstanceName="gridEmail" KeyFieldName="eml_id"
                                                                AutoGenerateColumns="False" DataSourceID="Email" Width="100%" Font-Size="12px"
                                                                ID="EmailGrid" OnRowValidating="EmailGrid_RowValidating" OnCustomJSProperties="EmailGrid_CustomJSProperties">
                                                                <Columns>
                                                                    <dxe:GridViewDataTextColumn FieldName="eml_id" Visible="False" VisibleIndex="1">
                                                                        <EditFormSettings Visible="False"></EditFormSettings>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataComboBoxColumn FieldName="eml_type" Caption="Email Type" Visible="False"
                                                                        VisibleIndex="0">
                                                                        <PropertiesComboBox ValueType="System.String" EnableIncrementalFiltering="True">
                                                                            <ClientSideEvents SelectedIndexChanged="function(s, e) {
	var value = s.GetValue();
    if(value == &quot;Web Site&quot;)
    {
         $('#EmailGrid_DXPEForm_ef0_DXEFL_0_3').show();
         $('#EmailGrid_DXPEForm_efnew_DXEFL_1,#EmailGrid_DXPEForm_efnew_DXEFL_2').hide();
         $('#EmailGrid_DXPEForm_ef0_DXEFL_0_1').hide();
         gridEmail.GetEditor(&quot;eml_email&quot;).SetVisible(false);
         gridEmail.GetEditor(&quot;eml_ccEmail&quot;).SetVisible(false);
         gridEmail.GetEditor(&quot;eml_website&quot;).SetVisible(true);
         $('#EmailGrid_DXPEForm_ef0_DXEFL_0_2').hide();
    }
    else
    {
         $('#EmailGrid_DXPEForm_ef0_DXEFL_0_3').hide();
         $('#EmailGrid_DXPEForm_efnew_DXEFL_1,#EmailGrid_DXPEForm_efnew_DXEFL_2').show();
         gridEmail.GetEditor(&quot;eml_email&quot;).SetVisible(true);
         gridEmail.GetEditor(&quot;eml_ccEmail&quot;).SetVisible(true);
         gridEmail.GetEditor(&quot;eml_website&quot;).SetVisible(false);
        $('#EmailGrid_DXPEForm_ef0_DXEFL_0_2').show();
    }
}"
                                                                                Init="function(s, e) {
	var value = s.GetValue();
    if(value == &quot;Web Site&quot;)
    {
        $('#EmailGrid_DXPEForm_efnew_DXEFL_3').show();
        $('#EmailGrid_DXPEForm_ef0_DXEFL_0_1,#EmailGrid_DXPEForm_ef0_DXEFL_0_2').hide();
         gridEmail.GetEditor(&quot;eml_email&quot;).SetVisible(false);
         gridEmail.GetEditor(&quot;eml_ccEmail&quot;).SetVisible(false);
         gridEmail.GetEditor(&quot;eml_website&quot;).SetVisible(true);
    }
    else
    {
         $('#EmailGrid_DXPEForm_efnew_DXEFL_3').show();
         $('#EmailGrid_DXPEForm_ef0_DXEFL_0_1,#EmailGrid_DXPEForm_ef0_DXEFL_0_2').show();
         gridEmail.GetEditor(&quot;eml_email&quot;).SetVisible(true);
         gridEmail.GetEditor(&quot;eml_ccEmail&quot;).SetVisible(true);
         gridEmail.GetEditor(&quot;eml_website&quot;).SetVisible(true);
    }
}"></ClientSideEvents>
                                                                            <Items>
                                                                                <dxe:ListEditItem Text="Personal" Value="Personal"></dxe:ListEditItem>
                                                                                <dxe:ListEditItem Text="Official" Value="Official"></dxe:ListEditItem>
                                                                                <dxe:ListEditItem Text="Web Site" Value="Web Site"></dxe:ListEditItem>
                                                                            </Items>
                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" SetFocusOnError="True">
                                                                                <RequiredField IsRequired="True" ErrorText="Select Type"></RequiredField>
                                                                            </ValidationSettings>
                                                                        </PropertiesComboBox>
                                                                        <EditFormSettings Visible="True"></EditFormSettings>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                    </dxe:GridViewDataComboBoxColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="eml_type" Width="27%" Caption="Type" VisibleIndex="0">
                                                                        <EditFormSettings Visible="False" Caption="Email Type"></EditFormSettings>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="eml_email" Caption="Email" VisibleIndex="1">
                                                                        <PropertiesTextEdit  MaxLength="500">
                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" SetFocusOnError="True">
                                                                                <RegularExpression ErrorText="Enetr Valid E-Mail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></RegularExpression>
                                                                            </ValidationSettings>
                                                                        </PropertiesTextEdit>
                                                                        <EditFormSettings Visible="True" Caption="EmailId"></EditFormSettings>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="eml_ccEmail" Visible="False" VisibleIndex="1">
                                                                        <PropertiesTextEdit MaxLength="500">
                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" SetFocusOnError="True">
                                                                                <RegularExpression ErrorText="Enetr Valid CC EMail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></RegularExpression>
                                                                            </ValidationSettings>
                                                                        </PropertiesTextEdit>
                                                                        <EditFormSettings Visible="True" Caption="CC Email"></EditFormSettings>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="eml_website" Caption="WebURL" PropertiesTextEdit-ValidationSettings-ErrorDisplayMode="ImageWithTooltip" VisibleIndex="2">
                                                                        <PropertiesTextEdit MaxLength="50">
                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" SetFocusOnError="True">
                                                                                <RegularExpression ErrorText="Enter Valid Web Address" ValidationExpression="^(http|https|ftp|)\://|[a-zA-Z0-9\-\.]+\.[a-zA-Z](:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&amp;%\$#\=~])*[^\.\,\)\(\s]$"></RegularExpression>
                                                                            </ValidationSettings>
                                                                        </PropertiesTextEdit>
                                                                        <EditFormSettings Visible="True" Caption="WebURL"></EditFormSettings>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewCommandColumn VisibleIndex="3" ShowDeleteButton="true" ShowEditButton="true" HeaderStyle-HorizontalAlign="Center">

<HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                                                        <HeaderTemplate>
                                                                            <a href="javascript:void(0);" onclick="gridEmail.AddNewRow();"><span>Add New</span> </a>
                                                                        </HeaderTemplate>
                                                                    </dxe:GridViewCommandColumn>
                                                                </Columns>
                                                                <SettingsCommandButton>


                                                                    <EditButton Image-Url="/assests/images/Edit.png" ButtonType="Image" Image-AlternateText="Edit">
<Image AlternateText="Edit" Url="/assests/images/Edit.png"></Image>
                                                                    </EditButton>
                                                                    <DeleteButton Image-Url="/assests/images/Delete.png" ButtonType="Image" Image-AlternateText="Delete">
<Image AlternateText="Delete" Url="/assests/images/Delete.png"></Image>
                                                                    </DeleteButton>
                                                                    <UpdateButton Text="Update" ButtonType="Button" Styles-Style-CssClass="btn btn-primary">
<Styles>
<Style CssClass="btn btn-primary"></Style>
</Styles>
                                                                    </UpdateButton>
                                                                    <CancelButton Text="Cancel" ButtonType="Button" Styles-Style-CssClass="btn btn-danger">
<Styles>
<Style CssClass="btn btn-danger"></Style>
</Styles>
                                                                    </CancelButton>
                                                                </SettingsCommandButton>
                                                                <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True"></SettingsBehavior>
                                                                <SettingsPager PageSize="20" NumericButtonCount="20" ShowSeparators="True">
                                                                    <FirstPageButton Visible="True">
                                                                    </FirstPageButton>
                                                                    <LastPageButton Visible="True">
                                                                    </LastPageButton>
                                                                </SettingsPager>
                                                                <SettingsEditing Mode="PopupEditForm" PopupEditFormWidth="700px"
                                                                    PopupEditFormHorizontalAlign="Center" PopupEditFormVerticalAlign="WindowCenter"
                                                                    PopupEditFormModal="True" EditFormColumnCount="1">
                                                                </SettingsEditing>
                                                                <Settings ShowTitlePanel="True" ShowStatusBar="Visible" ShowGroupPanel="true"></Settings>
                                                                <SettingsText ConfirmDelete="Confirm delete?" PopupEditFormCaption="Add/Modify Email"></SettingsText>
                                                                <Styles>
                                                                    <Header SortingImageSpacing="5px" ImageSpacing="5px">
                                                                    </Header>
                                                                    <LoadingPanel ImageSpacing="10px">
                                                                    </LoadingPanel>
                                                                </Styles>
                                                                <Templates>

                                                                    <EditForm>
                                                                        <table style="width: 100%">
                                                                            <tr>
                                                                                <td style="width: 100%">
                                                                                    <dxe:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormEditors" ColumnID="" ID="Editors3"></dxe:ASPxGridViewTemplateReplacement>
                                                                                    <div style="text-align: center; padding: 2px 2px 2px 2px">
                                                                                        <%-- <dxe:ASPxGridViewTemplateReplacement ID="UpdateButton" ReplacementType="EditFormUpdateButton"
                                                                            runat="server">
                                                                        </dxe:ASPxGridViewTemplateReplacement>--%>
                                                                                        <a id="update1" href="#" onclick="OnEmailClick()" class="btn btn-primary" style="color: white">Update</a>
                                                                                        <dxe:ASPxGridViewTemplateReplacement ID="CancelButton3" ReplacementType="EditFormCancelButton"
                                                                                            runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                                                    </div>
                                                                                </td>
                                                                                <td style="width: 25%"></td>
                                                                            </tr>
                                                                        </table>
                                                                    </EditForm>
                                                                </Templates>
                                                                <ClientSideEvents EndCallback="function(s, e) {
	Emailcheck(s.cpHeight);
}" />
                                                            </dxe:ASPxGridView>
                                                        </dxe:ContentControl>
                                                    </ContentCollection>
                                                </dxe:TabPage>
                                            </TabPages>
                                        </dxe:ASPxPageControl>
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Bank Details" Text="Bank Details">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="DP Details" Text="DP Details" Visible="false">
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
                            <dxe:TabPage Name="Family Members" Text="Family Members">
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
                            <dxe:TabPage Name="Group Member" Text="Group Member">
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
                        <TabStyle Font-Size="12px">
                        </TabStyle>
                    </dxe:ASPxPageControl>
                </td>
                <td></td>
            </tr>
        </table>
    </div>
    <asp:SqlDataSource ID="Address" runat="server"
        SelectCommand="select DISTINCT  tbl_master_address.add_id AS Id, tbl_master_address.add_addressType AS Type,
                        tbl_master_address.add_address1 AS Address1,  tbl_master_address.add_address2 AS Address2, 
                        tbl_master_address.add_address3 AS Address3,tbl_master_address.add_landMark AS LandMark, 
                        tbl_master_address.add_country AS Country, 
                        tbl_master_address.add_state AS State,tbl_master_address.add_city AS City,
                        CASE isnull(add_country, '')WHEN '' THEN '' ELSE(SELECT cou_country FROM tbl_master_country WHERE cou_id = add_country) END AS Country1, 
                        CASE isnull(add_state, '') WHEN '' THEN '' ELSE(SELECT state FROM tbl_master_state WHERE id = add_state) END AS State1,                         
                        CASE isnull(add_city, '') WHEN '' THEN '' ELSE(SELECT city_name FROM tbl_master_city WHERE city_id = add_city) END AS City1,
                        CASE isnull(add_area, '') WHEN '' THEN '' Else(select area_name From tbl_master_area Where area_id = add_area) End AS area,
                        tbl_master_address.add_pin AS PinCode, tbl_master_address.add_landMark AS LankMark 
                        from tbl_master_address where add_cntId=@insuId"
        DeleteCommand="contactDelete"
        DeleteCommandType="StoredProcedure" InsertCommand="insert_correspondence" InsertCommandType="StoredProcedure"
        UpdateCommand="update tbl_master_address set add_addressType=@Type,add_address1=@Address1,add_address2=@Address2,add_address3=@Address3,add_city=@City,add_landMark=@LandMark,add_country=@Country,add_state=@State,add_area=@area,add_pin=@PinCode,LastModifyDate=getdate(),LastModifyUser=@CreateUser where add_id=@Id">
        <SelectParameters>
            <asp:SessionParameter Name="insuId" SessionField="KeyVal_InternalID" Type="String" />
        </SelectParameters>
        <DeleteParameters>
            <asp:Parameter Name="Id" Type="decimal" />
            <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="Decimal" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="Type" Type="string" />
            <asp:Parameter Name="Address1" Type="string" />
            <asp:Parameter Name="Address2" Type="string" />
            <asp:Parameter Name="Address3" Type="string" />
            <asp:Parameter Name="City" Type="int32" />
            <asp:Parameter Name="area" Type="int32" />
            <asp:Parameter Name="LandMark" Type="string" />
            <asp:Parameter Name="Country" Type="int32" />
            <asp:Parameter Name="State" Type="int32" />
            <asp:Parameter Name="PinCode" Type="string" />
            <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="Decimal" />
            <asp:Parameter Name="Id" Type="decimal" />
        </UpdateParameters>
        <InsertParameters>
            <asp:SessionParameter Name="insuId" SessionField="KeyVal_InternalID" Type="String" />
            <asp:Parameter Name="Type" Type="string" />
            <asp:SessionParameter Name="contacttype" SessionField="ContactType" Type="string" />
            <asp:Parameter Name="Address1" Type="string" />
            <asp:Parameter Name="Address2" Type="string" />
            <asp:Parameter Name="Address3" Type="string" />
            <asp:Parameter Name="City" Type="int32" />
            <asp:Parameter Name="area" Type="int32" />
            <asp:Parameter Name="LandMark" Type="string" />
            <asp:Parameter Name="Country" Type="int32" />
            <asp:Parameter Name="State" Type="int32" />
            <asp:Parameter Name="PinCode" Type="string" />
            <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="Decimal" />
        </InsertParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="CountrySelect" runat="server"
        SelectCommand="SELECT cou_id, cou_country as Country FROM tbl_master_country order by cou_country"></asp:SqlDataSource>
    <asp:SqlDataSource ID="StateSelect" runat="server" 
        SelectCommand="SELECT s.id as ID,s.state as State from tbl_master_state s where (s.countryId = @State) ORDER BY s.state">
        <SelectParameters>
            <asp:Parameter Name="State" Type="string" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SelectCity" runat="server" 
        SelectCommand="SELECT c.city_id AS CityId, c.city_name AS City FROM tbl_master_city c where c.state_id=@City order by c.city_name">
        <SelectParameters>
            <asp:Parameter Name="City" Type="string" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SelectArea" runat="server"
        SelectCommand="SELECT area_id, area_name from tbl_master_area where (city_id = @Area) ORDER BY area_name">
        <SelectParameters>
            <asp:Parameter Name="Area" Type="string" />
        </SelectParameters>
    </asp:SqlDataSource>
    &nbsp;
        <asp:SqlDataSource ID="Phone" runat="server"
            DeleteCommand="delete from tbl_master_phonefax where phf_id=@phf_id" InsertCommand="insert_correspondence_phone"
            InsertCommandType="StoredProcedure" SelectCommand="select DISTINCT phf_id,phf_cntId,phf_entity,phf_type,phf_countryCode,phf_areaCode,phf_phoneNumber,phf_extension, ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' + ISNULL(phf_phoneNumber, '') +' '+ ISNULL(phf_extension, '') + ISNULL(phf_faxNumber, '') AS Number 
                      from tbl_master_phonefax where phf_cntId=@PhfId"
            UpdateCommand="update tbl_master_phonefax set phf_type=@phf_type,phf_countryCode=@phf_countryCode,phf_areaCode=@phf_areaCode,phf_phoneNumber=@phf_phoneNumber,
                       phf_extension=@phf_extension,LastModifyDate=getdate(),LastModifyUser=@CreateUser where phf_id=@phf_id">
            <SelectParameters>
                <asp:SessionParameter Name="PhfId" SessionField="KeyVal_InternalID" Type="String" />
            </SelectParameters>
            <InsertParameters>
                <asp:SessionParameter Name="PhfId" SessionField="KeyVal_InternalID" Type="String" />
                <asp:Parameter Name="phf_type" Type="string" />
                <asp:SessionParameter Name="contacttype" SessionField="ContactType" Type="string" />
                <asp:Parameter Name="phf_countryCode" Type="string" />
                <asp:Parameter Name="phf_areaCode" Type="string" />
                <asp:Parameter Name="phf_phoneNumber" Type="string" />
                <asp:Parameter Name="phf_extension" Type="string" />
                <asp:Parameter Name="phf_Availablefrom" Type="string" />
                <asp:Parameter Name="phf_AvailableTo" Type="string" />
                <asp:Parameter Name="phf_SMSFacility" Type="string" />
                <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="Decimal" />
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="phf_type" Type="string" />
                <asp:Parameter Name="phf_countryCode" Type="string" />
                <asp:Parameter Name="phf_areaCode" Type="string" />
                <asp:Parameter Name="phf_phoneNumber" Type="string" />
                <asp:Parameter Name="phf_extension" Type="string" />
                <asp:Parameter Name="phf_Availablefrom" Type="string" />
                <asp:Parameter Name="phf_AvailableTo" Type="string" />
                <asp:Parameter Name="phf_SMSFacility" Type="string" />
                <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="Decimal" />
                <asp:Parameter Name="phf_id" Type="decimal" />
            </UpdateParameters>
            <DeleteParameters>
                <asp:Parameter Name="phf_id" Type="decimal" />
            </DeleteParameters>
        </asp:SqlDataSource>
    <asp:SqlDataSource ID="Email" runat="server"
        DeleteCommand="delete from tbl_master_email where eml_id=@eml_id" InsertCommand="insert_correspondence_email"
        InsertCommandType="StoredProcedure" SelectCommand="select eml_id,eml_cntId,eml_entity,eml_type,eml_email,eml_ccEmail,eml_website,CreateDate,CreateUser from tbl_master_email where eml_cntId=@EmlId"
        UpdateCommand="update tbl_master_email set eml_type=@eml_type,eml_email=@eml_email,eml_ccEmail=@eml_ccEmail,eml_website=@eml_website,LastModifyDate=getdate(),LastModifyUser=@CreateUser where eml_id=@eml_id">
        <DeleteParameters>
            <asp:Parameter Name="eml_id" Type="decimal" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="eml_type" Type="string" />
            <asp:Parameter Name="eml_email" Type="string" />
            <asp:Parameter Name="eml_ccEmail" Type="string" />
            <asp:Parameter Name="eml_website" Type="string" />
            <asp:Parameter Name="eml_id" Type="decimal" />
            <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="Decimal" />
        </UpdateParameters>
        <SelectParameters>
            <asp:SessionParameter Name="EmlId" SessionField="KeyVal_InternalID" Type="string" />
        </SelectParameters>
        <InsertParameters>
            <asp:SessionParameter Name="EmlId" SessionField="KeyVal_InternalID" Type="string" />
            <asp:Parameter Name="eml_type" Type="string" />
            <asp:SessionParameter Name="contacttype" SessionField="ContactType" Type="string" />
            <asp:Parameter Name="eml_email" Type="string" />
            <asp:Parameter Name="eml_ccEmail" Type="string" />
            <asp:Parameter Name="eml_website" Type="string" />
            <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="Decimal" />
        </InsertParameters>
    </asp:SqlDataSource>
</asp:Content>
