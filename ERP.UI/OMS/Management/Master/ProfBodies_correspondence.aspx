<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" EnableEventValidation="false"
    Inherits="ERP.OMS.Management.Master.management_master_ProfBodies_correspondence" CodeBehind="ProfBodies_correspondence.aspx.cs" ValidateRequest="false"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function OnCountryChanged(cmbCountry) {
            gridAddress.GetEditor("State").PerformCallback(cmbCountry.GetValue().toString());

            gridAddress.GetEditor("City").PerformCallback('0');
            gridAddress.GetEditor("area").PerformCallback('0');
            gridAddress.GetEditor("PinCode").PerformCallback('0');
        }
        function OnStateChanged(cmbState) {
            gridAddress.GetEditor("City").PerformCallback(cmbState.GetValue().toString());
            gridAddress.GetEditor("area").PerformCallback('0');
            gridAddress.GetEditor("PinCode").PerformCallback('0');
        }
        function OnCityChanged(cmbCity) {
            gridAddress.GetEditor("area").PerformCallback(cmbCity.GetValue().toString());
            gridAddress.GetEditor("PinCode").PerformCallback(cmbCity.GetValue().toString());
        }
        function OnChildCall(cmbCity) {
            OnCityChanged(gridAddress.GetEditor("City"));
        }
        function openAreaPage() {

            var left = (screen.width - 300) / 2;
            var top = (screen.height - 250) / 2;
            var cityid = gridAddress.GetEditor("City").GetValue();
            var cityname = gridAddress.GetEditor("City").GetText();
            var URL = 'AddArea_PopUp.aspx?id=' + cityid + '&name=' + cityname + '';
            //if (cityid != null) {
            //    window.open(URL, '50', 'resizable=1,height=200px,width=500px,top=' + top + ',left=' + left + '');
            //}
            //else {
            //    alert('Please select a city first.');
            //    return false;
            //}
            popupan.SetContentUrl(URL);
            //alert (url);
            popupan.Show();

            //var left = (screen.width - 300) / 2;
            //var top = (screen.height - 250) / 2;
            //var cityid = gridAddress.GetEditor("City").GetValue();
            //var cityname = gridAddress.GetEditor("City").GetText();
            //var URL = 'AddArea_PopUp.aspx?id=' + cityid + '&name=' + cityname + '';
            //if (cityid != null) {
            //    window.open(URL, '50', 'resizable=1,height=120px,width=300px,top=' + top + ',left=' + left + '');
            //}
            //else {
            //    alert('Please select a city first!');
            //    return false;
            //}
        }
        function disp_prompt(name) {
            if (name == "tab0") {
                //alert(name);
                document.location.href = "ProfBodies_general.aspx";
            }
            if (name == "tab1") {
                //alert(name);
                //document.location.href="ProfBodies_correspondence.aspx";         
            }
        }

    </script>
    <style>
        #EmailGrid_DXPEForm_efnew_DXEFL_DXEditor3_EC, #EmailGrid_DXPEForm_efnew_DXEFL_DXEditor4_EC {
            display:none;
        }
        #EmailGrid_DXPEForm_efnew_DXEFL_DXEditor1_EC, #PhoneGrid_DXPEForm_efnew_DXEFL_DXEditor3_EC, #PhoneGrid_DXPEForm_efnew_DXEFL_DXEditor4_EC,
        #PhoneGrid_DXPEForm_efnew_DXEFL_DXEditor5_EC, #PhoneGrid_DXPEForm_efnew_DXEFL_DXEditor6_EC {
            position:absolute;
        }
        #AddressGrid_DXPEForm_efnew_DXEFL_DXEditor3_EC, #AddressGrid_DXPEForm_efnew_DXEFL_DXEditor4_EC, #PhoneGrid_DXPEForm_efnew_DXEFL_DXEditor8_EC
        ,#AddressGrid_DXPEForm_efnew_DXEFL_DXEditor5_EC, #AddressGrid_DXPEForm_efnew_DXEFL_DXEditor6_EC, #AddressGrid_DXPEForm_efnew_DXEFL_DXEditor15_EC {
            position:absolute;
        }
        #AddressGrid_DXPEForm_ef4_DXEFL_4_DXEditor3_EC {
        position:absolute;
        }
        #AddressGrid_DXPEForm_efnew_DXEFL_DXEditor3,#AddressGrid_DXPEForm_efnew_DXEFL_DXEditor4,#AddressGrid_DXPEForm_efnew_DXEFL_DXEditor5,#AddressGrid_DXPEForm_efnew_DXEFL_DXEditor6
        ,#AddressGrid_DXPEForm_efnew_DXEFL_DXEditor15_CC {
        width:93% !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table class="TableMain100">
            <tr>
                <td style="vertical-align: top; padding-left: 30px; padding-bottom: 15px;">
                    <div class="crossBtn"><a href="ProfBodies.aspx"><i class="fa fa-times"></i></a></div>
                </td>
            </tr>
            <tr>
                <td>
                    <dxe:ASPxPopupControl ID="ASPXPopupControl" runat="server" ContentUrl="AddArea_PopUp.aspx"
                                                                    CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popupan" Height="250px"
                                                                    Width="300px" HeaderText="Add New Area" AllowResize="true" ResizingMode="Postponed" Modal="true">
                                                                    <ContentCollection>
                                                                        <dxe:PopupControlContentControl runat="server">
                                                                        </dxe:PopupControlContentControl>
                                                                    </ContentCollection>
                                                                    <HeaderStyle BackColor="Blue" Font-Bold="True" ForeColor="White" />
                                                                </dxe:ASPxPopupControl>

                    <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="1" ClientInstanceName="page"
                        Font-Size="12px">
                        <TabPages>
                            <dxe:TabPage Text="General" Name="General">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Text="Correspondence" Name="CorresPondence">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                        <dxe:ASPxPageControl ID="ASPxPageControl2" runat="server" ActiveTabIndex="0" ClientInstanceName="page">
                                            <TabPages>
                                                <%-- <table class="TableMain100">
                                            <tr>
                                                <td class="gridcellcenter">--%>
                                                <dxe:TabPage Name="Adress" Text="Address">

                                                    <ContentCollection>
                                                        <dxe:ContentControl runat="server">
                                                            <div style="float: left;">
                                                                <%if (Session["PageAccess"].ToString().Trim() == "All" || Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "DelAdd")
                                                                  {
                                                                      if (rights.CanAdd)
                                                                      { %>
                                                                <a href="javascript:void(0);" onclick="gridAddress.AddNewRow();"><span class="btn btn-primary">Add New</span> </a>
                                                                <% }
                                                                      } %>
                                                            </div>
                                                            <dxe:ASPxGridView ID="AddressGrid" runat="server" DataSourceID="Address" ClientInstanceName="gridAddress"
                                                                KeyFieldName="Id" AutoGenerateColumns="False" OnCellEditorInitialize="AddressGrid_CellEditorInitialize"
                                                                Width="100%" Font-Size="12px" OnCommandButtonInitialize="AddressGrid_CommandButtonInitialize" OnStartRowEditing="AddressGrid_StartRowEditing">
                                                                     <ClientSideEvents CustomButtonClick="function(s, e) {
	                                                                       
                                                                         alert(e.buttonID);
                                                                        }" />
                                                                <Columns>
                                                                    <dxe:GridViewDataTextColumn FieldName="Id" Visible="False" VisibleIndex="0" Caption="Id">
                                                                        <EditFormSettings Visible="False" />
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataComboBoxColumn Caption="Address Type" FieldName="Type" Visible="False"
                                                                        VisibleIndex="0" >
                                                                        <PropertiesComboBox ValueType="System.String" width="93%">
                                                                            <Items>
                                                                                <dxe:ListEditItem Text="Residence" Value="Residence"></dxe:ListEditItem>
                                                                                <dxe:ListEditItem Text="Office" Value="Office"></dxe:ListEditItem>
                                                                                <dxe:ListEditItem Text="Correspondence" Value="Correspondence"></dxe:ListEditItem>
                                                                            </Items>
                                                                        </PropertiesComboBox>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <EditFormSettings Visible="True" VisibleIndex="1" />
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                    </dxe:GridViewDataComboBoxColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="Type" VisibleIndex="0" Caption="Type">
                                                                        <EditFormSettings Visible="False" />
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="Address1" VisibleIndex="1" Caption="Address1">
                                                                        <EditFormSettings Visible="True" VisibleIndex="2" />
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <PropertiesTextEdit MaxLength="150">
                                                                            <ValidationSettings>
                                                                                 <RegularExpression ValidationExpression="^[^<>]+$" ErrorText="Invalid Input" />
                                                                            </ValidationSettings>
                                                                        </PropertiesTextEdit>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="Address2" VisibleIndex="2" Caption="Address2">
                                                                        <EditFormSettings Visible="True" VisibleIndex="3" />
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <PropertiesTextEdit MaxLength="150">
                                                                              <ValidationSettings>
                                                                                 <RegularExpression ValidationExpression="^[^<>]+$" ErrorText="Invalid Input" />
                                                                            </ValidationSettings>
                                                                        </PropertiesTextEdit>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="Address3" VisibleIndex="3" Caption="Address3">
                                                                        <EditFormSettings Visible="True" VisibleIndex="4" />
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <PropertiesTextEdit MaxLength="150">
                                                                              <ValidationSettings>
                                                                                 <RegularExpression ValidationExpression="^[^<>]+$" ErrorText="Invalid Input" />
                                                                            </ValidationSettings>
                                                                        </PropertiesTextEdit>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="LandMark" VisibleIndex="4" Caption="Landmark">
                                                                        <EditFormSettings Visible="True" VisibleIndex="5" />
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <PropertiesTextEdit MaxLength="50">
                                                                              <ValidationSettings>
                                                                                 <RegularExpression ValidationExpression="^[^<>]+$" ErrorText="Invalid Input" />
                                                                            </ValidationSettings>
                                                                        </PropertiesTextEdit>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataComboBoxColumn Caption="Country" FieldName="Country" Visible="False"
                                                                        VisibleIndex="0">
                                                                        <PropertiesComboBox DataSourceID="CountrySelect" TextField="Country" ValueField="cou_id"
                                                                            EnableSynchronization="False" EnableIncrementalFiltering="True" ValueType="System.String">
                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" SetFocusOnError="True">
                                                                                <RequiredField ErrorText="Mandatory"  IsRequired="True" />
                                                                            </ValidationSettings>
                                                                            <ClientSideEvents SelectedIndexChanged="function(s, e) { OnCountryChanged(s); }"></ClientSideEvents>
                                                                        </PropertiesComboBox>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <EditFormSettings Visible="True" VisibleIndex="6" />
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                    </dxe:GridViewDataComboBoxColumn>
                                                                    <dxe:GridViewDataComboBoxColumn Caption="State" FieldName="State" Visible="False"
                                                                        VisibleIndex="0">
                                                                        <PropertiesComboBox DataSourceID="StateSelect" TextField="State" ValueField="ID"
                                                                            EnableSynchronization="False" EnableIncrementalFiltering="True" ValueType="System.String">
                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" SetFocusOnError="True">
                                                                                <RequiredField ErrorText="Mandatory"  IsRequired="True" />
                                                                            </ValidationSettings>
                                                                            <ClientSideEvents SelectedIndexChanged="function(s, e) { OnStateChanged(s); }"></ClientSideEvents>
                                                                        </PropertiesComboBox>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <EditFormSettings Visible="True" VisibleIndex="7" />
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                    </dxe:GridViewDataComboBoxColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="Country1" VisibleIndex="5" Caption="Country">
                                                                        <EditFormSettings Visible="False" />
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="State1" VisibleIndex="6" Caption="State">
                                                                        <EditFormSettings Visible="False" />
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="City1" VisibleIndex="8" Caption="City">
                                                                        <EditFormSettings Visible="False" />
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataComboBoxColumn Caption="City" FieldName="City" VisibleIndex="7"
                                                                        Visible="False">
                                                                        <PropertiesComboBox DataSourceID="SelectCity" TextField="City" ValueField="CityId"
                                                                            EnableSynchronization="False" EnableIncrementalFiltering="True" ValueType="System.String">
                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" SetFocusOnError="True">
                                                                                <RequiredField ErrorText="Mandatory"  IsRequired="True" />
                                                                            </ValidationSettings>
                                                                            <ClientSideEvents SelectedIndexChanged="function(s, e) { OnCityChanged(s); }"></ClientSideEvents>
                                                                        </PropertiesComboBox>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                        <EditFormSettings Visible="True" VisibleIndex="8" />
                                                                    </dxe:GridViewDataComboBoxColumn>
                                                                     <dxe:GridViewDataTextColumn Caption="Area" FieldName="add_area" VisibleIndex="7" Visible="true">
                                                                            <EditFormSettings Visible="False" />
                                                                            <CellStyle CssClass="gridcellleft">
                                                                            </CellStyle>
                                                                            <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                            </EditFormCaptionStyle>
                                                                        </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataComboBoxColumn Caption="Area" FieldName="area" visible="false">
                                                                        <PropertiesComboBox ValueType="System.Int32" DataSourceID="SelectArea" EnableSynchronization="False"
                                                                            EnableIncrementalFiltering="True" ValueField="area_id" TextField="area_name" width="93%">
                                                                        </PropertiesComboBox>
                                                                        <EditFormSettings Caption="Area" Visible="True" VisibleIndex="9" />
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                    </dxe:GridViewDataComboBoxColumn>
                                                                    <dxe:GridViewDataHyperLinkColumn Caption="" Visible="false" VisibleIndex="10">
                                                                        <EditFormSettings Visible="true" VisibleIndex="10" />
                                                                        <EditItemTemplate>
                                                                            <a href="javascript:void(0);" onclick="openAreaPage();"><span class="Ecoheadtxt">
                                                                                <strong>Add New Area</strong></span></a>
                                                                        </EditItemTemplate>
                                                                    </dxe:GridViewDataHyperLinkColumn>
                                                                   
                                                                    <%--debjyoti 06-12-2016--%>
                                                                    <%-- <dxe:GridViewDataTextColumn FieldName="PinCode" VisibleIndex="9" Caption="Pincode">
                                                                        <EditFormSettings Visible="True" VisibleIndex="11" />
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <PropertiesTextEdit MaxLength="6">
                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="right" SetFocusOnError="True">
                                                                                <RequiredField ErrorText="Mandatory." IsRequired="True" />
                                                                                <RegularExpression ErrorText="Invalid value." ValidationExpression="[0-9]{6}" />
                                                                            </ValidationSettings>
                                                                        </PropertiesTextEdit>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                    </dxe:GridViewDataTextColumn>--%>

                                                                     <dxe:GridViewDataComboBoxColumn Caption="Pincode" FieldName="PinCode" Visible="False" VisibleIndex="9">
                                                                        <PropertiesComboBox DataSourceID="SelectPin" TextField="pin_code" ValueField="pin_id" Width="100%"
                                                                            EnableSynchronization="False" EnableIncrementalFiltering="True" ValueType="System.String" ClearButton-DisplayMode="Always" ClearButton-ImagePosition="Right">
                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" SetFocusOnError="True">
                                                                                <RequiredField ErrorText="Mandatory"  IsRequired="True" />
                                                                            </ValidationSettings>
                                                                        </PropertiesComboBox>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <EditFormSettings Visible="True" VisibleIndex="8" />
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                    </dxe:GridViewDataComboBoxColumn>


                                                                    <dxe:GridViewDataTextColumn FieldName="PinCode1" VisibleIndex="8" Caption="Pin / Zip">
                                                                        <EditFormSettings Visible="False" />
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                     <%-- end debjyoti 06-12-2016--%>
                                                                    <dxe:GridViewCommandColumn VisibleIndex="10" ShowDeleteButton="true" ShowEditButton="true" Width="100px" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                        <%-- <DeleteButton Visible="True">
                                                                </DeleteButton>
                                                                <EditButton Visible="True">
                                                                </EditButton>--%>
                                                                        <HeaderTemplate>
                                                                            Actions
                                                                    <%--<%if (Session["PageAccess"].ToString().Trim() == "All" || Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "DelAdd")
                                                                      {
                                                                          if (rights.CanAdd)
                                                                          { %>
                                                                    <a href="javascript:void(0);" onclick="gridAddress.AddNewRow();"><span style="text-decoration: underline">Add New</span> </a>
                                                                     <% }
                                                                      } %>--%>
                                                                        </HeaderTemplate>
                                                                    </dxe:GridViewCommandColumn>
                                                                    <dxe:GridViewCommandColumn VisibleIndex="12" Visible="False">
                                                                    </dxe:GridViewCommandColumn>
                                                                </Columns>
                                                                <SettingsCommandButton>

                                                                    <EditButton Image-Url="/assests/images/Edit.png" ButtonType="Image" Image-AlternateText="Modify">
                                                                    </EditButton>
                                                                    <DeleteButton Image-Url="/assests/images/Delete.png" ButtonType="Image" Image-AlternateText="Delete">
                                                                    </DeleteButton>
                                                                    <UpdateButton Text="Save" ButtonType="Button" Styles-Style-CssClass="btn btn-primary "></UpdateButton>
                                                                    <CancelButton Text="Cancel" ButtonType="Button" Styles-Style-CssClass="btn btn-danger">
                                                                         <%-- <clientsideevents onClick="function(s, e) {
	                                                                                 alert('test');
                                                                                }" />--%>
                                                                    </CancelButton>
                                                                </SettingsCommandButton>
                                                                <Settings ShowStatusBar="Visible" ShowTitlePanel="True" ShowGroupPanel="true" />
                                                                <SettingsEditing Mode="PopupEditForm" PopupEditFormHeight="530px" PopupEditFormHorizontalAlign="Center"
                                                                    PopupEditFormModal="True" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="550px"
                                                                    EditFormColumnCount="1" />
                                                                <Styles>
                                                                    <LoadingPanel ImageSpacing="10px">
                                                                    </LoadingPanel>
                                                                    <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                                    </Header>
                                                                </Styles>
                                                                <SettingsText PopupEditFormCaption="Add Address" ConfirmDelete="Confirm delete?" />
                                                                <SettingsPager NumericButtonCount="20" PageSize="20" ShowSeparators="True">
                                                                </SettingsPager>
                                                                <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True" />
                                                                <Templates>
                                                                    <EditForm>
                                                                        <table style="width: 100%">
                                                                            <tr>
                                                                                <td style="width: 5%"></td>
                                                                                <td style="width: 90%">
                                                                                    <controls>
                                                   <dxe:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormEditors" ColumnID="" ID="Editors1">
                                                   </dxe:ASPxGridViewTemplateReplacement>                                                           
                                                 </controls>
                                                                                    <div style="padding: 2px 2px 2px 96px">
                                                                                        <dxe:ASPxGridViewTemplateReplacement ID="UpdateButton2" ReplacementType="EditFormUpdateButton"
                                                                                            runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                                                        <dxe:ASPxGridViewTemplateReplacement ID="CancelButton2" ReplacementType="EditFormCancelButton"
                                                                                            runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                                                    </div>
                                                                                </td>
                                                                                <td style="width: 5%"></td>
                                                                            </tr>
                                                                        </table>
                                                                    </EditForm>
                                                                    <TitlePanel>
                                                                        <table style="width: 100%">
                                                                            <tr>
                                                                                <td></td>
                                                                                <td align="center">
                                                                                    <%--<span class="Ecoheadtxt">Add/Modify Address.</span>--%>
                                                                                </td>

                                                                            </tr>
                                                                        </table>
                                                                    </TitlePanel>
                                                                </Templates>
                                                            </dxe:ASPxGridView>
                                                        </dxe:ContentControl>
                                                    </ContentCollection>
                                                </dxe:TabPage>
                                                <dxe:TabPage Name="Phone" Text="Phone">

                                                    <ContentCollection>
                                                        <dxe:ContentControl runat="server">
                                                            <div style="float: left;">
                                                               
                                                                 <%if (Session["PageAccess"].ToString().Trim() == "All" || Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "DelAdd")
                                                                              {
                                                                                  if (rights.CanAdd)
                                                                                  {%>
                                                                            <a href="javascript:void(0);" onclick="gridPhone.AddNewRow();"><span class="btn btn-primary">Add New</span> </a>
                                                                            <%  }
                                                                      } %>
                                                            </div>
                                                            <dxe:ASPxGridView ID="PhoneGrid" ClientInstanceName="gridPhone" DataSourceID="Phone"
                                                                KeyFieldName="phf_id" runat="server" AutoGenerateColumns="False" Width="100%"
                                                                Font-Size="12px" OnCommandButtonInitialize="PhoneGrid_CommandButtonInitialize" OnStartRowEditing="PhoneGrid_StartRowEditing">
                                                                <Columns>
                                                                    <dxe:GridViewDataTextColumn FieldName="phf_id" ReadOnly="True" VisibleIndex="0"
                                                                        Visible="False">
                                                                        <EditFormSettings Visible="False" />
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="phf_cntId" ReadOnly="True" VisibleIndex="0"
                                                                        Visible="False">
                                                                        <EditFormSettings Visible="False" />
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="phf_entity" ReadOnly="True" VisibleIndex="0"
                                                                        Visible="False">
                                                                        <EditFormSettings Visible="False" />
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataComboBoxColumn Caption="Phone Type" FieldName="phf_type" VisibleIndex="0">
                                                                        <PropertiesComboBox ValueType="System.String">
                                                                            <%--<ClientSideEvents SelectedIndexChanged="function(s, e) {
	var value = s.GetValue();
    if(value == &quot;Mobile&quot;)
    {
        $('#PhoneGrid_DXPEForm_efnew_DXEFL_1,#PhoneGrid_DXPEForm_efnew_DXEFL_2,#PhoneGrid_DXPEForm_efnew_DXEFL_4').hide();
         gridPhone.GetEditor(&quot;phf_countryCode&quot;).SetVisible(false);
         gridPhone.GetEditor(&quot;phf_areaCode&quot;).SetVisible(false);
         gridPhone.GetEditor(&quot;phf_extension&quot;).SetVisible(false);
    }
    else
    {
        $('#PhoneGrid_DXPEForm_efnew_DXEFL_1,#PhoneGrid_DXPEForm_efnew_DXEFL_2,#PhoneGrid_DXPEForm_efnew_DXEFL_4').show();
         gridPhone.GetEditor(&quot;phf_countryCode&quot;).SetVisible(true);
         gridPhone.GetEditor(&quot;phf_areaCode&quot;).SetVisible(true);
         gridPhone.GetEditor(&quot;phf_extension&quot;).SetVisible(true);
    }
}" />--%>
                                                                            <%--Code  Added and Commented By Priti on 28112016 to Enable TextBox--%>
                                                                            <ClientSideEvents SelectedIndexChanged="function(s, e) {
	var value = s.GetValue();
    if(value == &quot;Mobile&quot;)
    {
        
         gridPhone.GetEditor(&quot;phf_countryCode&quot;).SetEnabled(false);
         gridPhone.GetEditor(&quot;phf_countryCode&quot;).SetText('');                                                                           
         gridPhone.GetEditor(&quot;phf_areaCode&quot;).SetEnabled(false);
         gridPhone.GetEditor(&quot;phf_areaCode&quot;).SetText('');
         gridPhone.GetEditor(&quot;phf_extension&quot;).SetEnabled(false);
         gridPhone.GetEditor(&quot;phf_extension&quot;).SetText('');
    }
    else
    {
        
         gridPhone.GetEditor(&quot;phf_countryCode&quot;).SetEnabled(true);
         gridPhone.GetEditor(&quot;phf_areaCode&quot;).SetEnabled(true);
         gridPhone.GetEditor(&quot;phf_extension&quot;).SetEnabled(true);
    }
}" />
                                                                            <%-- <ClientSideEvents Init="function(s, e) {
	var value = s.GetValue();
    if(value == &quot;Mobile&quot;)
    {
        $('#PhoneGrid_DXPEForm_efnew_DXEFL_1,#PhoneGrid_DXPEForm_efnew_DXEFL_2,#PhoneGrid_DXPEForm_efnew_DXEFL_4').hide();
         gridPhone.GetEditor(&quot;phf_countryCode&quot;).SetVisible(false);
         gridPhone.GetEditor(&quot;phf_areaCode&quot;).SetVisible(false);
         gridPhone.GetEditor(&quot;phf_extension&quot;).SetVisible(false);
    }
    else
    {
        $('#PhoneGrid_DXPEForm_efnew_DXEFL_1,#PhoneGrid_DXPEForm_efnew_DXEFL_2,#PhoneGrid_DXPEForm_efnew_DXEFL_4').show();                                                                                
         gridPhone.GetEditor(&quot;phf_countryCode&quot;).SetVisible(true);
         gridPhone.GetEditor(&quot;phf_areaCode&quot;).SetVisible(true);
         gridPhone.GetEditor(&quot;phf_extension&quot;).SetVisible(true);
    }
}" />--%>
                                                                            <%--Code  Added and Commented By Priti on 28112016 to Enable TextBox--%>
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
                                                                            <Items>
                                                                                <dxe:ListEditItem Text="Residence" Value="Residence"></dxe:ListEditItem>
                                                                                <dxe:ListEditItem Text="Office" Value="Office"></dxe:ListEditItem>
                                                                                <dxe:ListEditItem Text="Correspondence" Value="Correspondence"></dxe:ListEditItem>
                                                                                <dxe:ListEditItem Text="Mobile" Value="Mobile"></dxe:ListEditItem>
                                                                                <dxe:ListEditItem Text="Fax" Value="Fax"></dxe:ListEditItem>
                                                                            </Items>
                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="right" SetFocusOnError="True">
                                                                                <RequiredField ErrorText="Mandatory." IsRequired="True" />
                                                                            </ValidationSettings>
                                                                        </PropertiesComboBox>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <EditFormSettings Visible="True" />
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                    </dxe:GridViewDataComboBoxColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="phf_countryCode" VisibleIndex="1" Visible="False">
                                                                        <EditFormSettings Caption="Country Code" Visible="True" />
                                                                        <PropertiesTextEdit MaxLength="5">
                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="right" SetFocusOnError="True">
                                                                                <RegularExpression ErrorText="Enter Valid Country Code." ValidationExpression="[0-9]+" />
                                                                            </ValidationSettings>
                                                                        </PropertiesTextEdit>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="phf_areaCode" VisibleIndex="1" Visible="False">
                                                                        <EditFormSettings Caption="Area Code" Visible="True" />
                                                                        <PropertiesTextEdit MaxLength="5">
                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="right" SetFocusOnError="True">
                                                                                <RegularExpression ErrorText="Enter Valid Area Code" ValidationExpression="[0-9]+" />
                                                                            </ValidationSettings>
                                                                        </PropertiesTextEdit>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="phf_phoneNumber" VisibleIndex="1" Caption="Number"
                                                                        Visible="False">
                                                                        <EditFormSettings Visible="True" />
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <PropertiesTextEdit MaxLength="10">
                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="right" SetFocusOnError="True">
                                                                                <RegularExpression ErrorText="Enter Valid Phone Number." ValidationExpression="[0-9]+" />
                                                                            </ValidationSettings>
                                                                        </PropertiesTextEdit>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="Number" VisibleIndex="1" Caption="Phone Number"
                                                                        Width="40%">
                                                                        <PropertiesTextEdit MaxLength="10">
                                                                        </PropertiesTextEdit>
                                                                        <EditFormSettings Visible="False" />
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="phf_extension" VisibleIndex="2" Caption="Extension"
                                                                        Visible="False">
                                                                        <EditFormSettings Visible="True" />
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <PropertiesTextEdit MaxLength="10">
                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="right" SetFocusOnError="True">
                                                                                <RegularExpression ErrorText="Invalid value." ValidationExpression="[0-9]+" />
                                                                            </ValidationSettings>
                                                                        </PropertiesTextEdit>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewCommandColumn VisibleIndex="2" ShowDeleteButton="true" ShowEditButton="true" Width="100px" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                        <%--<DeleteButton Visible="True">
                                                                </DeleteButton>
                                                                <EditButton Visible="True">
                                                                </EditButton>--%>
                                                                        <HeaderTemplate>
                                                                             Actions
                                                                            <%--<%if (Session["PageAccess"].ToString().Trim() == "All" || Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "DelAdd")
                                                                              {
                                                                                  if (rights.CanAdd)
                                                                                  {%>
                                                                            <a href="javascript:void(0);" onclick="gridPhone.AddNewRow();"><span style="text-decoration: underline">Add New</span> </a>
                                                                            <%  }
                                                                      } %>--%>
                                                                        </HeaderTemplate>
                                                                    </dxe:GridViewCommandColumn>
                                                                </Columns>
                                                                <SettingsCommandButton>

                                                                    <EditButton Image-Url="/assests/images/Edit.png" ButtonType="Image" Image-AlternateText="Modify">
                                                                    </EditButton>
                                                                    <DeleteButton Image-Url="/assests/images/Delete.png" ButtonType="Image" Image-AlternateText="Delete">
                                                                    </DeleteButton>
                                                                    <UpdateButton Text="Save" ButtonType="Button" Styles-Style-CssClass="btn btn-primary "></UpdateButton>
                                                                    <CancelButton Text="Cancel" ButtonType="Button" Styles-Style-CssClass="btn btn-danger"></CancelButton>
                                                                </SettingsCommandButton>
                                                                <Settings ShowStatusBar="Visible" ShowTitlePanel="True" ShowGroupPanel="true" />
                                                                <SettingsEditing Mode="PopupEditForm" PopupEditFormHeight="350px" PopupEditFormHorizontalAlign="Center"
                                                                    PopupEditFormModal="True" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="500px"
                                                                    EditFormColumnCount="1" />
                                                                <Styles>
                                                                    <LoadingPanel ImageSpacing="10px">
                                                                    </LoadingPanel>
                                                                    <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                                    </Header>
                                                                </Styles>
                                                                <SettingsText PopupEditFormCaption="Add Phone" ConfirmDelete="Confirm delete?" />
                                                                <SettingsPager NumericButtonCount="20" PageSize="20" ShowSeparators="True">
                                                                </SettingsPager>
                                                                <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True" />
                                                                <Templates>
                                                                    <EditForm>
                                                                        <table style="width: 100%">
                                                                            <tr>
                                                                                <td style="width: 5%"></td>
                                                                                <td style="width: 90%">
                                                                                    <controls>
                                                <dxe:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormEditors" ColumnID="" ID="Editors2">
                                                </dxe:ASPxGridViewTemplateReplacement>                                                           
                                              </controls>
                                                                                    <div style="padding: 2px 2px 2px  97px">
                                                                                        <dxe:ASPxGridViewTemplateReplacement ID="UpdateButton3" ReplacementType="EditFormUpdateButton"
                                                                                            runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                                                        <dxe:ASPxGridViewTemplateReplacement ID="CancelButton3" ReplacementType="EditFormCancelButton"
                                                                                            runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                                                    </div>
                                                                                </td>
                                                                                <td style="width: 5%"></td>
                                                                            </tr>
                                                                        </table>
                                                                    </EditForm>
                                                                    <TitlePanel>
                                                                        <table style="width: 100%">
                                                                            <tr>
                                                                                <td align="center">
                                                                                   <%-- <span class="Ecoheadtxt">Add/Modify Phone.</span>--%>
                                                                                </td>

                                                                            </tr>
                                                                        </table>
                                                                    </TitlePanel>
                                                                </Templates>
                                                            </dxe:ASPxGridView>
                                                        </dxe:ContentControl>
                                                    </ContentCollection>
                                                </dxe:TabPage>
                                                <dxe:TabPage Name="Email" Text="Email">

                                                    <ContentCollection>
                                                        <dxe:ContentControl runat="server">
                                                            <div style="float: left;">
                                                               
                                                                <%if (Session["PageAccess"].ToString().Trim() == "All" || Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "DelAdd")
                                                                              {
                                                                                  if (rights.CanAdd)
                                                                                  {%>
                                                                            <a href="javascript:void(0);" onclick="gridEmail.AddNewRow();"><span class="btn btn-primary">Add New</span> </a>
                                                                            <%  }
                                                                      }%>
                                                            </div>
                                                            <dxe:ASPxGridView ID="EmailGrid" runat="server" ClientInstanceName="gridEmail"
                                                                DataSourceID="Email" KeyFieldName="eml_id" AutoGenerateColumns="False" Font-Size="12px"  Width="100%"
                                                                OnCommandButtonInitialize="EmailGrid_CommandButtonInitialize" OnStartRowEditing="EmailGrid_StartRowEditing">
                                                                <Columns>
                                                                    <dxe:GridViewDataTextColumn FieldName="eml_id" VisibleIndex="1" Visible="False">
                                                                        <EditFormSettings Visible="False" />
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataComboBoxColumn Caption="Email Type" FieldName="eml_type" Visible="False"
                                                                        VisibleIndex="0">
                                                                        <PropertiesComboBox ValueType="System.String">
                                                                            <%-- <ClientSideEvents SelectedIndexChanged="function(s, e) {
	var value = s.GetValue();
    if(value == &quot;Web Site&quot;)
    {
        $('#EmailGrid_DXPEForm_efnew_DXEFL_3').show();
        $('#EmailGrid_DXPEForm_efnew_DXEFL_2,#EmailGrid_DXPEForm_efnew_DXEFL_1').hide();
         gridEmail.GetEditor(&quot;eml_email&quot;).SetVisible(false);
         gridEmail.GetEditor(&quot;eml_ccEmail&quot;).SetVisible(false);
         gridEmail.GetEditor(&quot;eml_website&quot;).SetVisible(true);
    }
    else
    {
        $('#EmailGrid_DXPEForm_efnew_DXEFL_3').hide();
        $('#EmailGrid_DXPEForm_efnew_DXEFL_2,#EmailGrid_DXPEForm_efnew_DXEFL_1').show();
         gridEmail.GetEditor(&quot;eml_email&quot;).SetVisible(true);
         gridEmail.GetEditor(&quot;eml_ccEmail&quot;).SetVisible(true);
         gridEmail.GetEditor(&quot;eml_website&quot;).SetVisible(false);
    }
}" />--%>
                                                                            <%--Code  Added and Commented By Priti on 25112016 to Enable TextBox--%>
                                                                            <ClientSideEvents SelectedIndexChanged="function(s, e) {
	var value = s.GetValue();
    if(value == &quot;Web Site&quot;)
    {
         gridEmail.GetEditor(&quot;eml_email&quot;).SetEnabled(false);
         gridEmail.GetEditor(&quot;eml_ccEmail&quot;).SetEnabled(false);
         gridEmail.GetEditor(&quot;eml_website&quot;).SetEnabled(true);
         
    }
    else
    {
         gridEmail.GetEditor(&quot;eml_email&quot;).SetEnabled(true);
         gridEmail.GetEditor(&quot;eml_ccEmail&quot;).SetEnabled(true);
         gridEmail.GetEditor(&quot;eml_website&quot;).SetEnabled(false);
    }
     gridEmail.GetEditor(&quot;eml_email&quot;).SetText('');
     gridEmail.GetEditor(&quot;eml_ccEmail&quot;).SetText('');
     gridEmail.GetEditor(&quot;eml_website&quot;).SetText('');
}" />
                                                                            <%--Code  Added and Commented By Priti on 25112016 to Enable TextBox--%>
                                                                            <ClientSideEvents Init="function(s, e) {
	var value = s.GetValue();
    if(value == &quot;Web Site&quot;)
    { 
         gridEmail.GetEditor(&quot;eml_email&quot;).SetEnabled(false);
         gridEmail.GetEditor(&quot;eml_ccEmail&quot;).SetEnabled(false);
         gridEmail.GetEditor(&quot;eml_website&quot;).SetEnabled(true);
    }
    else
    { 
         gridEmail.GetEditor(&quot;eml_email&quot;).SetEnabled(true);
         gridEmail.GetEditor(&quot;eml_ccEmail&quot;).SetEnabled(true);
         gridEmail.GetEditor(&quot;eml_website&quot;).SetEnabled(false);
    }
}" />
                                                                            <%--<ClientSideEvents Init="function(s, e) {
	var value = s.GetValue();
    if(value == &quot;Web Site&quot;)
    {
        $('#EmailGrid_DXPEForm_efnew_DXEFL_3').show();
        $('#EmailGrid_DXPEForm_efnew_DXEFL_2,#EmailGrid_DXPEForm_efnew_DXEFL_1').hide();
         gridEmail.GetEditor(&quot;eml_email&quot;).SetVisible(false);
         gridEmail.GetEditor(&quot;eml_ccEmail&quot;).SetVisible(false);
         gridEmail.GetEditor(&quot;eml_website&quot;).SetVisible(true);
    }
    else
    {
        $('#EmailGrid_DXPEForm_efnew_DXEFL_3').hide();
        $('#EmailGrid_DXPEForm_efnew_DXEFL_2,#EmailGrid_DXPEForm_efnew_DXEFL_1').show();
         gridEmail.GetEditor(&quot;eml_email&quot;).SetVisible(true);
         gridEmail.GetEditor(&quot;eml_ccEmail&quot;).SetVisible(true);
         gridEmail.GetEditor(&quot;eml_website&quot;).SetVisible(false);
    }
}" />--%>
                                                                            <Items>
                                                                                <dxe:ListEditItem Text="Personal" Value="Personal"></dxe:ListEditItem>
                                                                                <dxe:ListEditItem Text="Official" Value="Official"></dxe:ListEditItem>
                                                                                <dxe:ListEditItem Text="Web Site" Value="Web Site"></dxe:ListEditItem>
                                                                            </Items>
                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" SetFocusOnError="True">
                                                                                <RequiredField ErrorText="Mandatory." IsRequired="True" />
                                                                            </ValidationSettings>
                                                                        </PropertiesComboBox>
                                                                        <EditFormSettings Visible="True" />
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                    </dxe:GridViewDataComboBoxColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="eml_type" VisibleIndex="0" Caption="Type"
                                                                       >
                                                                        <EditFormSettings Caption="Email Type" Visible="False" />
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="eml_email" VisibleIndex="1" Caption="Email">
                                                                        <EditFormSettings Caption="Email ID" Visible="True" />
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                        <PropertiesTextEdit MaxLength="50" >
                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Bottom" SetFocusOnError="True">
                                                                                <RegularExpression ErrorText="Enetr Valid Email ID." ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                                                                            </ValidationSettings>
                                                                        </PropertiesTextEdit>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="eml_ccEmail" VisibleIndex="1" Visible="False">
                                                                        <EditFormSettings Caption="CC Email ID" Visible="True" />
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                        <PropertiesTextEdit MaxLength="50" >
                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Bottom" SetFocusOnError="True">
                                                                                <RegularExpression ErrorText="Enter Valid CC Email ID." ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                                                                            </ValidationSettings>
                                                                        </PropertiesTextEdit>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="eml_website" Caption="Website" VisibleIndex="1"
                                                                        Visible="true">
                                                                        <PropertiesTextEdit MaxLength="50">
                                                                        </PropertiesTextEdit>
                                                                        <EditFormSettings Caption="Website" Visible="True" />
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewCommandColumn VisibleIndex="2" ShowDeleteButton="true" ShowEditButton="true" Width="100px" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                        <%--  <DeleteButton Visible="True">
                                                                </DeleteButton>
                                                                <EditButton Visible="True">
                                                                </EditButton>--%>
                                                                        <HeaderTemplate>
                                                                             Actions
                                                                            <%--<%if (Session["PageAccess"].ToString().Trim() == "All" || Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "DelAdd")
                                                                              {
                                                                                  if (rights.CanAdd)
                                                                                  {%>
                                                                            <a href="javascript:void(0);" onclick="gridEmail.AddNewRow();"><span style="text-decoration: underline">Add New</span> </a>
                                                                            <%  }
                                                                      }%>--%>
                                                                        </HeaderTemplate>
                                                                    </dxe:GridViewCommandColumn>
                                                                </Columns>
                                                                <SettingsCommandButton>

                                                                    <EditButton Image-Url="/assests/images/Edit.png" ButtonType="Image" Image-AlternateText="Modify">
                                                                    </EditButton>
                                                                    <DeleteButton Image-Url="/assests/images/Delete.png" ButtonType="Image" Image-AlternateText="Delete">
                                                                    </DeleteButton>
                                                                    <UpdateButton Text="Save" ButtonType="Button" Styles-Style-CssClass="btn btn-primary"></UpdateButton>
                                                                    <CancelButton Text="Cancel" ButtonType="Button" Styles-Style-CssClass="btn btn-danger"></CancelButton>
                                                                </SettingsCommandButton>
                                                                <Settings ShowStatusBar="Visible" ShowTitlePanel="True" ShowGroupPanel="true" />
                                                                <SettingsEditing Mode="PopupEditForm" PopupEditFormHeight="300px" PopupEditFormHorizontalAlign="Center"
                                                                    PopupEditFormModal="True" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="500px"
                                                                    EditFormColumnCount="1" />
                                                                <Styles>
                                                                    <LoadingPanel ImageSpacing="10px">
                                                                    </LoadingPanel>
                                                                    <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                                    </Header>
                                                                </Styles>
                                                                <SettingsText PopupEditFormCaption="Add Email" ConfirmDelete="Confirm delete?" />
                                                                <SettingsPager NumericButtonCount="20" PageSize="20" ShowSeparators="True">
                                                                </SettingsPager>
                                                                <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True" />
                                                                <Templates>
                                                                    <EditForm>
                                                                        <table style="width: 100%">
                                                                            <tr>
                                                                                <td style="width: 5%"></td>
                                                                                <td style="width: 90%">
                                                                                    <controls>
                                                     <dxe:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormEditors" ColumnID="" ID="Editors3">
                                                     </dxe:ASPxGridViewTemplateReplacement>                                                           
                                                 </controls>
                                                                                    <div style="padding: 2px 2px 2px 92px">
                                                                                        <dxe:ASPxGridViewTemplateReplacement ID="UpdateButton1" ReplacementType="EditFormUpdateButton"
                                                                                            runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                                                        <dxe:ASPxGridViewTemplateReplacement ID="CancelButton1" ReplacementType="EditFormCancelButton"
                                                                                            runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                                                    </div>
                                                                                </td>
                                                                                <td style="width: 5%"></td>
                                                                            </tr>
                                                                        </table>
                                                                    </EditForm>
                                                                    <TitlePanel>
                                                                        <table style="width: 100%">
                                                                            <tr>
                                                                                <td align="center">
                                                                                 <%--   <span class="Ecoheadtxt">Add/Modify Email.</span>--%>
                                                                                </td>

                                                                            </tr>
                                                                        </table>
                                                                    </TitlePanel>
                                                                </Templates>
                                                            </dxe:ASPxGridView>
                                                        </dxe:ContentControl>
                                                    </ContentCollection>
                                                </dxe:TabPage>
                                                <%-- </td>
                                            </tr>
                                        </table>--%>
                                            </TabPages>
                                        </dxe:ASPxPageControl>
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                        </TabPages>
                        <ClientSideEvents ActiveTabChanged="function(s, e) {
	                                            var activeTab   = page.GetActiveTab();
	                                            var Tab0 = page.GetTab(0);
	                                            var Tab1 = page.GetTab(1);
	                                            
	                                            if(activeTab == Tab0)
	                                            {
	                                                disp_prompt('tab0');
	                                            }
	                                            if(activeTab == Tab1)
	                                            {
	                                                disp_prompt('tab1');
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
    </div>
    <asp:SqlDataSource ID="Address" runat="server"
        SelectCommand="select DISTINCT  tbl_master_address.add_id AS Id, tbl_master_address.add_addressType AS Type,
                        tbl_master_address.add_address1 AS Address1,  tbl_master_address.add_address2 AS Address2, 
                        tbl_master_address.add_address3 AS Address3,tbl_master_address.add_landMark AS LandMark, 
                        tbl_master_address.add_country AS Country, 
                        tbl_master_address.add_state AS State,tbl_master_address.add_city AS City,
                        CASE add_country WHEN '' THEN '' ELSE(SELECT cou_country FROM tbl_master_country WHERE cou_id = add_country) END AS Country1, 
                        CASE add_state WHEN '' THEN '' ELSE(SELECT state FROM tbl_master_state WHERE id = add_state) END AS State1,
                        CASE add_city WHEN '' THEN '' ELSE(SELECT city_name FROM tbl_master_city WHERE city_id = add_city) END AS City1,
                        CASE add_area WHEN '' THEN '' Else(select area_name From tbl_master_area Where area_id = add_area) End AS add_area, area = CAST(add_area as int),
                            
                        CASE add_pin WHEN '' THEN '' ELSE(SELECT pin_code FROM tbl_master_pinzip WHERE pin_id = add_pin) END AS PinCode1,                  
                        tbl_master_address.add_pin AS PinCode, tbl_master_address.add_landMark AS LankMark 
                        from tbl_master_address where add_cntId=@insuId"
        DeleteCommand="contactDelete"
        DeleteCommandType="StoredProcedure" InsertCommand="insert_correspondence" UpdateCommand="update tbl_master_address set add_addressType=@Type,add_address1=@Address1,add_address2=@Address2,add_address3=@Address3,add_city=@City,add_landMark=@LandMark,add_country=@Country,add_state=@State,add_area=@area,add_pin=@PinCode,LastModifyDate=getdate(),LastModifyUser=@CreateUser where add_id=@Id"
        InsertCommandType="StoredProcedure">
        <SelectParameters>
            <asp:SessionParameter Name="insuId" SessionField="KeyVal_InternalID" Type="String" />
        </SelectParameters>
        <DeleteParameters>
            <asp:Parameter Name="Id" Type="decimal" />
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

              <%--debjyoti 06-12-2016--%>
     <asp:SqlDataSource ID="SelectPin" runat="server" 
        SelectCommand="select pin_id,pin_code from tbl_master_pinzip where city_id=@City order by pin_code">
        <SelectParameters>
            <asp:Parameter Name="City" Type="string" />
        </SelectParameters>
    </asp:SqlDataSource>
    <%--End Debjyoti 06-12-2016--%>

        <asp:SqlDataSource ID="Phone" runat="server"
            DeleteCommand="delete from tbl_master_phonefax where phf_id=@phf_id" InsertCommand="insert_correspondence_phone"
            SelectCommand="select DISTINCT phf_id,phf_cntId,phf_entity,phf_type,phf_countryCode,phf_areaCode,phf_phoneNumber,phf_extension, ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' + ISNULL(phf_phoneNumber, '') +' '+ ISNULL(phf_extension, '') + ISNULL(phf_faxNumber, '') AS Number 
                      from tbl_master_phonefax where phf_cntId=@PhfId"
            UpdateCommand="update tbl_master_phonefax set phf_type=@phf_type,phf_countryCode=@phf_countryCode,phf_areaCode=@phf_areaCode,phf_phoneNumber=@phf_phoneNumber,
                       phf_extension=@phf_extension,LastModifyDate=getdate(),LastModifyUser=@CreateUser where phf_id=@phf_id"
            InsertCommandType="StoredProcedure">
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
