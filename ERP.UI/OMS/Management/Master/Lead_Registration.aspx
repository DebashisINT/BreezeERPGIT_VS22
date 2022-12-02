<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.Master.management_Master_Lead_Registration" CodeBehind="Lead_Registration.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="/OMS/CSS/dhtmlgoodies_calendar.css?random=20051112"
        media="screen" />
    <script type="text/javascript" src="/assests/js/dhtmlgoodies_calendar.js?random=20060118"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script language="javascript" type="text/javascript">
        function disp_prompt(name) {
            if (name == "tab0") {
                //alert(name);
                document.location.href = "Lead_general.aspx";
            }
            if (name == "tab1") {
                //alert(name);
                document.location.href = "Lead_Correspondence.aspx";
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
                //document.location.href="Lead_Registration.aspx"; 
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
        function OnCompanyChanged(cmbCountry) {
            exchange.GetEditor("crg_exchange1").PerformCallback(cmbCountry.GetValue().toString());
        }

    </script>

    <div class="panel-heading">
        <div class="panel-title">
            <h3>Lead Registration</h3>
        </div>
        <div class="crossBtn"><a href="Lead.aspx"><i class="fa fa-times"></i></a></div>
    </div>
    <div class="form_main">
        <table class="TableMain100">
            <tr>
                <td>
                    <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="6" ClientInstanceName="page"
                        Font-Size="12px">
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
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Bank Details" Text="Bank Details">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="DP Details" Text="DP Details"  Visible="false">
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
                                        <table width="100%" border="1">
                                            <tbody>
                                                <tr>
                                                    <td>
                                                        <dxe:ASPxGridView runat="server" ClientInstanceName="grid" KeyFieldName="crg_id"
                                                            AutoGenerateColumns="False" DataSourceID="Sqlstatutory" Width="100%" ID="gridRegisStatutory"
                                                            OnRowValidating="gridRegisStatutory_RowValidating" OnInitNewRow="gridRegisStatutory_InitNewRow">
                                                            <Columns>
                                                                <dxe:GridViewDataTextColumn FieldName="crg_id" ReadOnly="True" Visible="False"
                                                                    VisibleIndex="0">
                                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn FieldName="crg_cntId" Visible="False" VisibleIndex="0">
                                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataComboBoxColumn FieldName="crg_type" Caption="Type" VisibleIndex="0">
                                                                    <PropertiesComboBox EnableIncrementalFiltering="True" EnableSynchronization="False"
                                                                        ValueType="System.String">
                                                                        <ClientSideEvents SelectedIndexChanged="function(s, e) {
	                                                                var value = s.GetValue();
                                                                    if(value == &quot;Pancard&quot;)
                                                                    {
                                                                        grid.GetEditor(&quot;crg_registrationAuthority&quot;).SetClientVisible(true);
                                                                        grid.GetEditor(&quot;crg_place&quot;).SetClientVisible(true);
                                                                        grid.GetEditor(&quot;crg_Date&quot;).SetClientVisible(true);
                                                                        grid.GetEditor(&quot;crg_validDate&quot;).SetClientVisible(true);
                                                                        grid.GetEditor(&quot;crg_verify&quot;).SetClientVisible(true);
                                                                    }
                                                                    else
                                                                    {
                                                                        if(value == &quot;Other&quot;)
                                                                        {
                                                                            grid.GetEditor(&quot;crg_registrationAuthority&quot;).SetClientVisible(true);
                                                                            grid.GetEditor(&quot;crg_place&quot;).SetClientVisible(true);
                                                                            grid.GetEditor(&quot;crg_Date&quot;).SetClientVisible(true);
                                                                            grid.GetEditor(&quot;crg_validDate&quot;).SetClientVisible(true);
                                                                            grid.GetEditor(&quot;crg_verify&quot;).SetClientVisible(true);
                                                                        }
                                                                        else
                                                                        {
                                                                            grid.GetEditor(&quot;crg_registrationAuthority&quot;).SetClientVisible(true);
                                                                            grid.GetEditor(&quot;crg_place&quot;).SetClientVisible(true);
                                                                            grid.GetEditor(&quot;crg_Date&quot;).SetClientVisible(true);
                                                                            grid.GetEditor(&quot;crg_validDate&quot;).SetClientVisible(true);
                                                                            grid.GetEditor(&quot;crg_verify&quot;).SetClientVisible(true);
                                                                        }
                                                                    }
                                                                }"></ClientSideEvents>
                                                                        <Items>
                                                                            <dxe:ListEditItem Text="Pan Card" Value="Pancard"></dxe:ListEditItem>
                                                                            <dxe:ListEditItem Text="Passport" Value="Passport"></dxe:ListEditItem>
                                                                            <dxe:ListEditItem Text="Ration Card" Value="RationCard"></dxe:ListEditItem>
                                                                            <dxe:ListEditItem Text="Driving License" Value="DrivingLicense"></dxe:ListEditItem>
                                                                            <dxe:ListEditItem Text="Voter Id" Value="VoterId"></dxe:ListEditItem>
                                                                            <dxe:ListEditItem Text="Social Security Number" Value="SSNumber"></dxe:ListEditItem>
                                                                            <dxe:ListEditItem Text="Other" Value="Other"></dxe:ListEditItem>
                                                                        </Items>
                                                                    </PropertiesComboBox>
                                                                    <EditFormSettings Visible="True" VisibleIndex="0" Caption="Type"></EditFormSettings>
                                                                    <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                                                    </EditFormCaptionStyle>
                                                                    <CellStyle CssClass="gridcellleft">
                                                                    </CellStyle>
                                                                </dxe:GridViewDataComboBoxColumn>
                                                                <dxe:GridViewDataTextColumn FieldName="crg_Number" Caption="Number" VisibleIndex="1">
                                                                    <EditFormSettings Visible="True" VisibleIndex="1" Caption="Number"></EditFormSettings>
                                                                    <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                                                    </EditFormCaptionStyle>
                                                                    <CellStyle CssClass="gridcellleft">
                                                                    </CellStyle>
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn FieldName="crg_registrationAuthority" Caption="Registration Authority Name"
                                                                    Visible="False" VisibleIndex="6">
                                                                    <EditFormSettings Visible="True" VisibleIndex="6" Caption="Registration Authority Name"></EditFormSettings>
                                                                    <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                                                    </EditFormCaptionStyle>
                                                                    <CellStyle CssClass="gridcellleft">
                                                                    </CellStyle>
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn FieldName="crg_place" Caption="Place of Issue" VisibleIndex="2">
                                                                    <EditFormSettings Visible="True" VisibleIndex="3" Caption="Place of Issue"></EditFormSettings>
                                                                    <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                                                    </EditFormCaptionStyle>
                                                                    <CellStyle CssClass="gridcellleft">
                                                                    </CellStyle>
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataDateColumn FieldName="crg_Date" Caption="Date of Issue" Visible="False">
                                                                    <PropertiesDateEdit EditFormatString="dd MM yyyy" EditFormat="Custom" UseMaskBehavior="True">
                                                                    </PropertiesDateEdit>
                                                                    <EditFormSettings Visible="True" VisibleIndex="4" Caption="Date of Issue"></EditFormSettings>
                                                                    <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                                                    </EditFormCaptionStyle>
                                                                    <CellStyle CssClass="gridcellleft">
                                                                    </CellStyle>
                                                                </dxe:GridViewDataDateColumn>
                                                                <dxe:GridViewDataTextColumn FieldName="crg_Date1" Caption="Date of Issue" VisibleIndex="3">
                                                                    <EditFormSettings Visible="False" Caption="Date of Issue"></EditFormSettings>
                                                                    <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                                                    </EditFormCaptionStyle>
                                                                    <CellStyle CssClass="gridcellleft">
                                                                    </CellStyle>
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataDateColumn FieldName="crg_validDate" Caption="Valid Untill" Visible="False">
                                                                    <PropertiesDateEdit DisplayFormatString="{0:dd MMM yyyy}" EditFormatString="dd-MM-yyyy" EditFormat="Custom" UseMaskBehavior="True">
                                                                    </PropertiesDateEdit>
                                                                    <EditFormSettings Visible="True" VisibleIndex="5" Caption="Valid Untill"></EditFormSettings>
                                                                    <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                                                    </EditFormCaptionStyle>
                                                                    <CellStyle CssClass="gridcellleft">
                                                                    </CellStyle>
                                                                </dxe:GridViewDataDateColumn>
                                                                <dxe:GridViewDataTextColumn FieldName="crg_validDate1" Caption="Valid Untill" VisibleIndex="4">
                                                                    <EditFormSettings Visible="False" Caption="Valid Untill"></EditFormSettings>
                                                                    <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                                                    </EditFormCaptionStyle>

                                                                    <CellStyle CssClass="gridcellleft">
                                                                    </CellStyle>
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataComboBoxColumn FieldName="crg_verify" Caption="Verify" Visible="False">
                                                                    <PropertiesComboBox EnableIncrementalFiltering="True" EnableSynchronization="False"
                                                                        ValueType="System.String">
                                                                        <Items>
                                                                            <dxe:ListEditItem Text="Verified" Value="Verified"></dxe:ListEditItem>
                                                                            <dxe:ListEditItem Text="Not Verified" Value="Not Verified"></dxe:ListEditItem>
                                                                        </Items>
                                                                    </PropertiesComboBox>
                                                                    <EditFormSettings Visible="True" VisibleIndex="7" Caption="Verify"></EditFormSettings>
                                                                    <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                                                    </EditFormCaptionStyle>
                                                                    <CellStyle CssClass="gridcellleft">
                                                                    </CellStyle>
                                                                </dxe:GridViewDataComboBoxColumn>
                                                                <dxe:GridViewCommandColumn VisibleIndex="5" ShowDeleteButton="true" ShowEditButton="true" HeaderStyle-HorizontalAlign="Center">

                                                                    <HeaderTemplate>
                                                                        <a href="javascript:void(0);" onclick="grid.AddNewRow();"><span>Add New</span> </a>
                                                                    </HeaderTemplate>
                                                                </dxe:GridViewCommandColumn>
                                                            </Columns>
                                                            <SettingsCommandButton>

                                                                
                                                                    <EditButton Image-Url="/assests/images/Edit.png" ButtonType="Image" Image-AlternateText="Edit">
                                                                    </EditButton>
                                                                 <DeleteButton Image-Url="/assests/images/Delete.png" ButtonType="Image" Image-AlternateText="Delete">
                                                                    </DeleteButton>
                                                                    <UpdateButton Text="Update" ButtonType="Button" Styles-Style-CssClass="btn btn-primary"></UpdateButton>
                                                                    <CancelButton Text="Cancel" ButtonType="Button" Styles-Style-CssClass="btn btn-danger"></CancelButton>
                                                            </SettingsCommandButton>
                                                            <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True"></SettingsBehavior>
                                                            <SettingsPager PageSize="20" AlwaysShowPager="True" NumericButtonCount="20" ShowSeparators="True">
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
                                                            <SettingsText ConfirmDelete="Confirm delete?" PopupEditFormCaption="Add/Modify Registration"></SettingsText>
                                                            <Styles>
                                                                <Header SortingImageSpacing="5px" ImageSpacing="5px">
                                                                </Header>
                                                                <LoadingPanel ImageSpacing="10px">
                                                                </LoadingPanel>
                                                            </Styles>
                                                            <StylesEditors>
                                                                <ProgressBar Height="25px">
                                                                </ProgressBar>
                                                            </StylesEditors>
                                                            <Templates>
                                                                <TitlePanel>
                                                                    <table style="width: 100%">
                                                                        <tr>
                                                                            <td align="center">
                                                                                <table width="200">
                                                                                    <tr>
                                                                                        <td align="center" style="width: 50%">
                                                                                            <span style="color: black;">Statutory Registrations.</span>
                                                                                        </td>
                                                                                        <%--   <td>
                                                                                <dxe:ASPxButton ID="ASPxButton1" runat="server" Text="ADD" ToolTip="Add New Data"   Height="18px" Width="88px" AutoPostBack="False">
                                                                                    <clientsideevents click="function(s, e) {grid.AddNewRow();}" />
                                                                                </dxe:ASPxButton>
                                                                            </td>--%>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </TitlePanel>
                                                                <EditForm>
                                                                    <table style="width: 100%">
                                                                        <tr>
                                                                            <td style="width: 25%"></td>
                                                                            <td style="width: 50%">
                                                                                <controls>
                                                                        <dxe:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormEditors" ColumnID="" ID="Editors1">
                                                                        </dxe:ASPxGridViewTemplateReplacement>                                                           
                                                                    </controls>
                                                                                <div style="text-align: right; padding: 2px 2px 2px 2px">
                                                                                    <dxe:ASPxGridViewTemplateReplacement ID="UpdateButton1" ReplacementType="EditFormUpdateButton"
                                                                                        runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                                                    <dxe:ASPxGridViewTemplateReplacement ID="CancelButton1" ReplacementType="EditFormCancelButton"
                                                                                        runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                                                </div>
                                                                            </td>
                                                                            <td style="width: 25%"></td>
                                                                        </tr>
                                                                    </table>
                                                                </EditForm>
                                                            </Templates>
                                                        </dxe:ASPxGridView>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <dxe:ASPxGridView runat="server" ClientInstanceName="exchange" KeyFieldName="crg_internalId"
                                                            AutoGenerateColumns="False" DataSourceID="SqlExchange" Width="100%" ID="gridExchange"
                                                            OnRowValidating="gridExchange_RowValidating" OnInitNewRow="gridExchange_InitNewRow"
                                                            OnCellEditorInitialize="gridExchange_CellEditorInitialize">
                                                            <Columns>
                                                                <dxe:GridViewDataTextColumn FieldName="crg_internalId" Visible="False">
                                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn FieldName="crg_cntID" Visible="False">
                                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataComboBoxColumn FieldName="crg_company1" Caption="Company" VisibleIndex="0">
                                                                    <PropertiesComboBox EnableIncrementalFiltering="True" EnableSynchronization="False"
                                                                        DataSourceID="SqlComp" TextField="cmp_Name" ValueField="cmp_internalid" ValueType="System.String">
                                                                        <ClientSideEvents SelectedIndexChanged="function(s, e) { OnCompanyChanged(s); }"></ClientSideEvents>
                                                                    </PropertiesComboBox>
                                                                    <EditFormSettings Visible="True" VisibleIndex="0"></EditFormSettings>
                                                                    <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                                                    </EditFormCaptionStyle>
                                                                    <CellStyle CssClass="gridcellleft">
                                                                    </CellStyle>
                                                                </dxe:GridViewDataComboBoxColumn>
                                                                <dxe:GridViewDataComboBoxColumn FieldName="crg_exchange1" Visible="False">
                                                                    <PropertiesComboBox EnableIncrementalFiltering="True" EnableSynchronization="False"
                                                                        DataSourceID="SqlExchangeSegment" TextField="name" ValueField="exch_internalId"
                                                                        ValueType="System.String">
                                                                    </PropertiesComboBox>
                                                                    <EditFormSettings Visible="True" VisibleIndex="1" Caption="Exchange Segment"></EditFormSettings>
                                                                    <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                                                    </EditFormCaptionStyle>
                                                                    <CellStyle CssClass="gridcellleft">
                                                                    </CellStyle>
                                                                </dxe:GridViewDataComboBoxColumn>
                                                                <dxe:GridViewDataTextColumn FieldName="crg_exchange" Caption="Exchange Segment"
                                                                    VisibleIndex="1">
                                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn FieldName="crg_company" Visible="False">
                                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn FieldName="crg_tcode" Caption="UCC" VisibleIndex="2">
                                                                    <EditFormSettings Visible="True" VisibleIndex="2" Caption="Client Id(UCC)"></EditFormSettings>
                                                                    <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                                                    </EditFormCaptionStyle>
                                                                    <CellStyle CssClass="gridcellleft">
                                                                    </CellStyle>
                                                                    <PropertiesTextEdit MaxLength="10">
                                                                    </PropertiesTextEdit>
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn FieldName="crg_regisDate1" ReadOnly="True" Caption="Regn. Date"
                                                                    VisibleIndex="3">
                                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataDateColumn FieldName="crg_regisDate" Caption="Registration Date" Visible="False" VisibleIndex="3">
                                                                    <PropertiesDateEdit DisplayFormatString="{0:dd MMM yyyy}" EditFormatString="dd-MM-yyyy" EditFormat="Custom" UseMaskBehavior="True"></PropertiesDateEdit>

                                                                    <EditFormSettings Visible="True" VisibleIndex="3" Caption="Registration Date"></EditFormSettings>
                                                                    <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                                                    </EditFormCaptionStyle>
                                                                    <CellStyle CssClass="gridcellleft">
                                                                    </CellStyle>
                                                                </dxe:GridViewDataDateColumn>
                                                                <dxe:GridViewDataDateColumn FieldName="crg_businessCmmDate" Caption="Business Comm. Date" VisibleIndex="4">
                                                                    <PropertiesDateEdit DisplayFormatString="{0:dd MMM yyyy}" EditFormatString="dd-MM-yyyy" EditFormat="Custom" UseMaskBehavior="True"></PropertiesDateEdit>

                                                                    <EditFormSettings Visible="True" VisibleIndex="4" Caption="Business Comm. Date"></EditFormSettings>
                                                                    <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                                                    </EditFormCaptionStyle>
                                                                    <CellStyle CssClass="gridcellleft">
                                                                    </CellStyle>
                                                                </dxe:GridViewDataDateColumn>
                                                                <dxe:GridViewDataDateColumn FieldName="crg_suspensionDate" Caption="Suspension Date" VisibleIndex="5">
                                                                    <PropertiesDateEdit DisplayFormatString="{0:dd MMM yyyy}" EditFormatString="dd-MM-yyyy" EditFormat="Custom" UseMaskBehavior="True"></PropertiesDateEdit>
                                                                    <EditFormSettings Visible="True" VisibleIndex="5" Caption="Suspension Date"></EditFormSettings>
                                                                    <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                                                    </EditFormCaptionStyle>
                                                                    <CellStyle CssClass="gridcellleft">
                                                                    </CellStyle>
                                                                </dxe:GridViewDataDateColumn>
                                                                <dxe:GridViewDataMemoColumn FieldName="crg_reasonforSuspension" Caption="Reason For Suspension" Visible="False"
                                                                    VisibleIndex="5">
                                                                    <EditFormSettings Visible="True" VisibleIndex="6" Caption="Reason For Suspension"></EditFormSettings>
                                                                    <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                                                    </EditFormCaptionStyle>
                                                                    <CellStyle CssClass="gridcellleft">
                                                                    </CellStyle>
                                                                </dxe:GridViewDataMemoColumn>
                                                                <dxe:GridViewDataTextColumn FieldName="CreateUser" Visible="False">
                                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewCommandColumn VisibleIndex="6" ShowDeleteButton="true" ShowEditButton="true" HeaderStyle-HorizontalAlign="Center">

                                                                    <HeaderTemplate>
                                                                        <a href="javascript:void(0);" onclick="exchange.AddNewRow();"><span>Add New</span> </a>
                                                                    </HeaderTemplate>
                                                                </dxe:GridViewCommandColumn>
                                                            </Columns>
                                                            <SettingsCommandButton>

                                                               
                                                                    <EditButton Image-Url="/assests/images/Edit.png" ButtonType="Image" Image-AlternateText="Edit">
                                                                    </EditButton>
                                                                  <DeleteButton Image-Url="/assests/images/Delete.png" ButtonType="Image" Image-AlternateText="Delete">
                                                                    </DeleteButton>
                                                                    <UpdateButton Text="Update" ButtonType="Button" Styles-Style-CssClass="btn btn-primary"></UpdateButton>
                                                                    <CancelButton Text="Cancel" ButtonType="Button" Styles-Style-CssClass="btn btn-danger"></CancelButton>
                                                            </SettingsCommandButton>
                                                            <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True"></SettingsBehavior>
                                                            <SettingsPager PageSize="20" AlwaysShowPager="True" NumericButtonCount="20" ShowSeparators="True">
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
                                                            <SettingsText ConfirmDelete="Confirm delete?" PopupEditFormCaption="Add/Modify Exchange"></SettingsText>
                                                            <Styles>
                                                                <Header SortingImageSpacing="5px" ImageSpacing="5px">
                                                                </Header>
                                                                <LoadingPanel ImageSpacing="10px">
                                                                </LoadingPanel>
                                                            </Styles>
                                                            <StylesEditors>
                                                                <ProgressBar Height="25px">
                                                                </ProgressBar>
                                                            </StylesEditors>
                                                            <Templates>
                                                                <TitlePanel>
                                                                    <table style="width: 100%">
                                                                        <tr>
                                                                            <td align="center">
                                                                                <table width="200">
                                                                                    <tr>
                                                                                        <td align="center" style="width: 50%">
                                                                                            <span style="color: black;">Exchange/DP Segment Registrations</span>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </TitlePanel>
                                                                <EditForm>
                                                                    <table style="width: 100%">
                                                                        <tr>
                                                                            <td style="width: 25%"></td>
                                                                            <td style="width: 50%">
                                                                                <controls>
                                                                        <dxe:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormEditors" ColumnID="" ID="Editors2">
                                                                        </dxe:ASPxGridViewTemplateReplacement>                                                           
                                                                    </controls>
                                                                                <div style="text-align: right; padding: 2px 2px 2px 2px">
                                                                                    <dxe:ASPxGridViewTemplateReplacement ID="UpdateButton2" ReplacementType="EditFormUpdateButton"
                                                                                        runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                                                    <dxe:ASPxGridViewTemplateReplacement ID="CancelButton2" ReplacementType="EditFormCancelButton"
                                                                                        runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                                                </div>
                                                                            </td>
                                                                            <td style="width: 25%"></td>
                                                                        </tr>
                                                                    </table>
                                                                </EditForm>
                                                            </Templates>
                                                        </dxe:ASPxGridView>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <dxe:ASPxGridView runat="server" ClientInstanceName="membership" KeyFieldName="crg_internalid"
                                                            AutoGenerateColumns="False" DataSourceID="Sqlmembership" Width="100%" ID="gridMembership">
                                                            <Columns>
                                                                <dxe:GridViewDataTextColumn FieldName="crg_internalid" ReadOnly="True" Visible="False"
                                                                    VisibleIndex="0">
                                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn FieldName="crg_cntId" Visible="False" VisibleIndex="0">
                                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn FieldName="crg_memNumber" Caption="Membership Number"
                                                                    VisibleIndex="1">
                                                                    <EditFormSettings Visible="True" VisibleIndex="1"></EditFormSettings>
                                                                    <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                                                    </EditFormCaptionStyle>
                                                                    <CellStyle CssClass="gridcellleft">
                                                                    </CellStyle>
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn FieldName="crg_memtype" Caption="Membership Type" VisibleIndex="2">
                                                                    <EditFormSettings Visible="True" VisibleIndex="2"></EditFormSettings>
                                                                    <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                                                    </EditFormCaptionStyle>
                                                                    <CellStyle CssClass="gridcellleft">
                                                                    </CellStyle>
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataComboBoxColumn FieldName="crg_idprof" Caption="Professional Association"
                                                                    VisibleIndex="0">
                                                                    <PropertiesComboBox EnableIncrementalFiltering="True" EnableSynchronization="False"
                                                                        DataSourceID="SqlProfessional" TextField="prof_name" ValueField="prof_id" ValueType="System.String">
                                                                    </PropertiesComboBox>
                                                                    <EditFormSettings Visible="True" VisibleIndex="0" Caption="Professional Association"></EditFormSettings>
                                                                    <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                                                    </EditFormCaptionStyle>
                                                                    <CellStyle CssClass="gridcellleft">
                                                                    </CellStyle>
                                                                </dxe:GridViewDataComboBoxColumn>
                                                                <dxe:GridViewDataComboBoxColumn FieldName="crg_validityType" Caption="Validity Type"
                                                                    VisibleIndex="3">
                                                                    <PropertiesComboBox EnableIncrementalFiltering="True" EnableSynchronization="False"
                                                                        ValueType="System.String">
                                                                        <ClientSideEvents SelectedIndexChanged="function(s, e) {
	                                                                var value = s.GetValue();
                                                                    if(value == &quot;Limited&quot;)
                                                                    {
                                                                        membership.GetEditor(&quot;crg_memExpDate&quot;).SetClientVisible(true);
                                                                    }
                                                                    else
                                                                    {
                                                                        membership.GetEditor(&quot;crg_memExpDate&quot;).SetClientVisible(false);
                                                                    }
                                                                }"></ClientSideEvents>
                                                                        <Items>
                                                                            <dxe:ListEditItem Text="Limited" Value="Limited"></dxe:ListEditItem>
                                                                            <dxe:ListEditItem Text="Lifetime" Value="Lifetime"></dxe:ListEditItem>
                                                                        </Items>
                                                                    </PropertiesComboBox>
                                                                    <EditFormSettings Visible="True" VisibleIndex="3" Caption="Validity Type"></EditFormSettings>
                                                                    <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                                                    </EditFormCaptionStyle>
                                                                    <CellStyle CssClass="gridcellleft">
                                                                    </CellStyle>
                                                                </dxe:GridViewDataComboBoxColumn>
                                                                <dxe:GridViewDataDateColumn FieldName="crg_memExpDate" Caption="Membership Expiry Date"
                                                                    VisibleIndex="5">
                                                                    <PropertiesDateEdit DisplayFormatString="{0:dd MMM yyyy}" EditFormatString="dd-MM-yyyy" EditFormat="Custom" UseMaskBehavior="True"></PropertiesDateEdit>
                                                                    <EditFormSettings Visible="True" VisibleIndex="4" Caption="Membership Expiry Date"></EditFormSettings>
                                                                    <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                                                    </EditFormCaptionStyle>
                                                                    <CellStyle CssClass="gridcellleft">
                                                                    </CellStyle>
                                                                </dxe:GridViewDataDateColumn>
                                                                <dxe:GridViewDataTextColumn FieldName="crg_notes" Caption="Notes" VisibleIndex="5">
                                                                    <EditFormSettings Visible="True" VisibleIndex="5" Caption="Notes"></EditFormSettings>
                                                                    <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                                                    </EditFormCaptionStyle>
                                                                    <CellStyle CssClass="gridcellleft">
                                                                    </CellStyle>
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewCommandColumn VisibleIndex="6" ShowDeleteButton="true" ShowEditButton="true" HeaderStyle-HorizontalAlign="Center">

                                                                    <HeaderTemplate>
                                                                        <a href="javascript:void(0);" onclick="membership.AddNewRow();"><span>Add New</span> </a>
                                                                    </HeaderTemplate>
                                                                </dxe:GridViewCommandColumn>
                                                            </Columns>
                                                            <SettingsCommandButton>

                                                               
                                                                    <EditButton Image-Url="/assests/images/Edit.png" ButtonType="Image" Image-AlternateText="Edit">
                                                                    </EditButton>
                                                                  <DeleteButton Image-Url="/assests/images/Delete.png" ButtonType="Image" Image-AlternateText="Delete">
                                                                    </DeleteButton>
                                                                    <UpdateButton Text="Update" ButtonType="Button" Styles-Style-CssClass="btn btn-primary"></UpdateButton>
                                                                    <CancelButton Text="Cancel" ButtonType="Button" Styles-Style-CssClass="btn btn-danger"></CancelButton>
                                                            </SettingsCommandButton>
                                                            <SettingsBehavior ConfirmDelete="True"></SettingsBehavior>
                                                            <SettingsPager PageSize="20" AlwaysShowPager="True" NumericButtonCount="20" ShowSeparators="True">
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
                                                            <SettingsText ConfirmDelete="Confirm delete?" PopupEditFormCaption="Add/Modify Membership"></SettingsText>
                                                            <Styles>
                                                                <Header SortingImageSpacing="5px" ImageSpacing="5px">
                                                                </Header>
                                                                <LoadingPanel ImageSpacing="10px">
                                                                </LoadingPanel>
                                                            </Styles>
                                                            <StylesEditors>
                                                                <ProgressBar Height="25px">
                                                                </ProgressBar>
                                                            </StylesEditors>
                                                            <Templates>
                                                                <TitlePanel>
                                                                    <table style="width: 100%">
                                                                        <tr>
                                                                            <td align="center">
                                                                                <table width="200">
                                                                                    <tr>
                                                                                        <td align="center" style="width: 50%">
                                                                                            <span style="color: black;">Other Memberships</span>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </TitlePanel>
                                                                <EditForm>
                                                                    <table style="width: 100%">
                                                                        <tr>
                                                                            <td style="width: 25%"></td>
                                                                            <td style="width: 50%">
                                                                                <controls>
                                                                        <dxe:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormEditors" ColumnID="" ID="Editors3">
                                                                        </dxe:ASPxGridViewTemplateReplacement>                                                           
                                                                    </controls>
                                                                                <div style="text-align: right; padding: 2px 2px 2px 2px">
                                                                                    <dxe:ASPxGridViewTemplateReplacement ID="UpdateButton3" ReplacementType="EditFormUpdateButton"
                                                                                        runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                                                    <dxe:ASPxGridViewTemplateReplacement ID="CancelButton3" ReplacementType="EditFormCancelButton"
                                                                                        runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                                                </div>
                                                                            </td>
                                                                            <td style="width: 25%"></td>
                                                                        </tr>
                                                                    </table>
                                                                </EditForm>
                                                            </Templates>
                                                        </dxe:ASPxGridView>
                                                        <asp:SqlDataSource runat="server"
                                                            DeleteCommand="DELETE FROM [tbl_master_contactMembership] WHERE [crg_internalid] = @crg_internalid"
                                                            InsertCommand="INSERT INTO [tbl_master_contactMembership] ([crg_cntId],[crg_idprof], [crg_memNumber], [crg_memtype], [crg_validityType], [crg_memExpDate], [crg_notes], [CreateUser], [CreateDate]) VALUES (@crg_cntId,@crg_idprof, @crg_memNumber, @crg_memtype, @crg_validityType, @crg_memExpDate, @crg_notes, @CreateUser, getdate())"
                                                            SelectCommand="SELECT *,convert(varchar(11),crg_memExpDate,113) as crg_memExpDate1 FROM [tbl_master_contactMembership] where crg_cntId=@crg_cntId"
                                                            UpdateCommand="UPDATE [tbl_master_contactMembership] SET  [crg_idprof] = @crg_idprof, [crg_memNumber] = @crg_memNumber, [crg_memtype] = @crg_memtype, [crg_validityType] = @crg_validityType, [crg_memExpDate] = @crg_memExpDate, [crg_notes] = @crg_notes,  [LastModifyUser] = @LastModifyUser, [LastModifyDate] = getdate() WHERE [crg_internalid] = @crg_internalid"
                                                            ID="Sqlmembership">
                                                            <DeleteParameters>
                                                                <asp:Parameter Name="crg_internalid" Type="Int32"></asp:Parameter>
                                                            </DeleteParameters>
                                                            <InsertParameters>
                                                                <asp:SessionParameter SessionField="KeyVal_InternalID" Name="crg_cntId" Type="String"></asp:SessionParameter>
                                                                <asp:Parameter Name="crg_idprof" Type="String"></asp:Parameter>
                                                                <asp:Parameter Name="crg_memNumber" Type="String"></asp:Parameter>
                                                                <asp:Parameter Name="crg_memtype" Type="String"></asp:Parameter>
                                                                <asp:Parameter Name="crg_validityType" Type="String"></asp:Parameter>
                                                                <asp:Parameter Name="crg_memExpDate" Type="DateTime"></asp:Parameter>
                                                                <asp:Parameter Name="crg_notes" Type="String"></asp:Parameter>
                                                                <asp:SessionParameter SessionField="userid" Name="CreateUser" Type="Int32"></asp:SessionParameter>
                                                            </InsertParameters>
                                                            <SelectParameters>
                                                                <asp:SessionParameter SessionField="KeyVal_InternalID" Name="crg_cntId" Type="String"></asp:SessionParameter>
                                                            </SelectParameters>
                                                            <UpdateParameters>
                                                                <asp:Parameter Name="crg_idprof" Type="String"></asp:Parameter>
                                                                <asp:Parameter Name="crg_memNumber" Type="String"></asp:Parameter>
                                                                <asp:Parameter Name="crg_memtype" Type="String"></asp:Parameter>
                                                                <asp:Parameter Name="crg_validityType" Type="String"></asp:Parameter>
                                                                <asp:Parameter Name="crg_memExpDate" Type="DateTime"></asp:Parameter>
                                                                <asp:Parameter Name="crg_notes" Type="String"></asp:Parameter>
                                                                <asp:SessionParameter SessionField="userid" Name="LastModifyUser" Type="Int32"></asp:SessionParameter>
                                                                <asp:Parameter Name="crg_internalid" Type="Int32"></asp:Parameter>
                                                            </UpdateParameters>
                                                        </asp:SqlDataSource>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
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
                    </dxe:ASPxPageControl>
                </td>
            </tr>
        </table>
        <asp:SqlDataSource ID="Sqlstatutory" runat="server"
            DeleteCommand="DELETE FROM [tbl_master_contactRegistration] WHERE [crg_id] = @crg_id"
            InsertCommand="INSERT INTO [tbl_master_contactRegistration] ([crg_cntId], [crg_type], [crg_Number], [crg_registrationAuthority], [crg_place], [crg_Date], [crg_validDate], [CreateDate], [CreateUser],[crg_contactType]) VALUES (@crg_cntId, @crg_type, @crg_Number, @crg_registrationAuthority, @crg_place, @crg_Date, @crg_validDate, getdate(), @CreateUser,'contact')"
            SelectCommand="SELECT [crg_id], [crg_cntId], [crg_type], [crg_Number], [crg_registrationAuthority], [crg_place], [crg_Date],convert(varchar(11),crg_Date,113) as crg_Date1, [crg_validDate],convert(varchar(11),crg_validDate,113) as crg_validDate1,[crg_verify], [CreateDate], [CreateUser], [LastModifyDate], [LastModifyUser] FROM [tbl_master_contactRegistration] where crg_cntId=@crg_cntId"
            UpdateCommand="UPDATE [tbl_master_contactRegistration] SET  [crg_type] = @crg_type, [crg_Number] = @crg_Number, [crg_registrationAuthority] = @crg_registrationAuthority, [crg_place] = @crg_place,[crg_contactType]='contact', [crg_Date] = @crg_Date, [crg_validDate] = @crg_validDate, [LastModifyDate] = getdate(), [LastModifyUser] = @LastModifyUser WHERE [crg_id] = @crg_id">
            <DeleteParameters>
                <asp:Parameter Name="crg_id" Type="Decimal" />
            </DeleteParameters>
            <SelectParameters>
                <asp:SessionParameter Name="crg_cntId" SessionField="KeyVal_InternalID" Type="string" />
            </SelectParameters>
            <InsertParameters>
                <asp:SessionParameter Name="crg_cntId" SessionField="KeyVal_InternalID" Type="string" />
                <asp:Parameter Name="crg_type" Type="String" />
                <asp:Parameter Name="crg_Number" Type="String" />
                <asp:Parameter Name="crg_registrationAuthority" Type="String" />
                <asp:Parameter Name="crg_place" Type="String" />
                <asp:Parameter Type="datetime" Name="crg_Date" />
                <asp:Parameter Type="datetime" Name="crg_validDate" />
                <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="Decimal" />
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="crg_type" Type="String" />
                <asp:Parameter Name="crg_Number" Type="String" />
                <asp:Parameter Name="crg_registrationAuthority" Type="String" />
                <asp:Parameter Name="crg_place" Type="String" />
                <asp:Parameter Type="datetime" Name="crg_Date" />
                <asp:Parameter Type="datetime" Name="crg_validDate" />
                <asp:SessionParameter Name="LastModifyUser" SessionField="userid" Type="Decimal" />
                <asp:Parameter Name="crg_id" Type="Decimal" />
            </UpdateParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlExchange" runat="server"
            SelectCommand="select ce.crg_internalId as crg_internalId,ce.crg_cntID as crg_cntID,ce.crg_exchange as crg_exchange1,ce.crg_company as crg_company1,case crg_company when '0' then 'N/A' else (select cmp_name from tbl_master_company where cmp_internalId=ce.crg_company) end as crg_company,ce.crg_exchange as crg_exchange,ce.crg_tcode as crg_tcode,convert(varchar(11),ce.crg_regisDate,113) as crg_regisDate1,ce.crg_regisDate as crg_regisDate,convert(varchar(11),ce.crg_businessCmmDate,113) as crg_businessCmmDate1,ce.crg_businessCmmDate as crg_businessCmmDate,convert(varchar(11),ce.crg_suspensionDate,113) as crg_suspensionDate1,ce.crg_suspensionDate as crg_suspensionDate,ce.crg_reasonforSuspension as crg_reasonforSuspension,ce.CreateUser as CreateUser from tbl_master_contactExchange ce where crg_cntID=@crg_cntID"
            InsertCommand="insertContactExchange" InsertCommandType="StoredProcedure" UpdateCommand="updateContactExchange"
            UpdateCommandType="StoredProcedure" DeleteCommand="delete from tbl_master_contactExchange where crg_internalId=@crg_internalId">
            <SelectParameters>
                <asp:SessionParameter Name="crg_cntID" SessionField="KeyVal_InternalID" Type="string" />
            </SelectParameters>
            <DeleteParameters>
                <asp:Parameter Name="crg_internalId" Type="string" />
            </DeleteParameters>
            <InsertParameters>
                <asp:SessionParameter Name="crg_cntID" SessionField="KeyVal_InternalID" Type="string" />
                <asp:Parameter Name="crg_company1" Type="string" />
                <asp:Parameter Name="crg_exchange1" Type="string" />
                <asp:Parameter Name="crg_regisDate" Type="datetime" />
                <asp:Parameter Name="crg_businessCmmDate" Type="datetime" />
                <asp:Parameter Name="crg_suspensionDate" Type="datetime" />
                <asp:Parameter Name="crg_reasonforSuspension" Type="string" />
                <asp:Parameter Name="crg_SubBrokerFranchiseeID" Type="string" />
                   <asp:Parameter Name="crg_Dealer" Type="string" />
                 <asp:Parameter Name="crg_AccountClosureDate" Type="string" />
                 <asp:Parameter Name="crg_FrontOfficeBranchCode" Type="string" />
                 <asp:Parameter Name="crg_FrontOfficeGroupCode" Type="string" />
                 <asp:Parameter Name="crg_ParticipantSchemeCode" Type="string" />
                   <asp:Parameter Name="crg_ClearingBankCode" Type="string" />
                 <asp:Parameter Name="crg_SchemeCode" Type="string" />
                <asp:Parameter Name="crg_STTPattern" Type="string" />
                 <asp:Parameter Name="SttWap" Type="string" />
                 <asp:Parameter Name="MapinSwapSebi" Type="string" />
                 <asp:Parameter Name="crg_exchangesegment" Type="string" />
            
                <asp:Parameter Name="crg_tcode" Type="string" />
                <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="int32" />
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="crg_internalId" Type="string" />
                <asp:SessionParameter Name="crg_cntID" SessionField="KeyVal_InternalID" Type="string" />
                <asp:Parameter Name="crg_company1" Type="string" />
                <asp:Parameter Name="crg_exchange1" Type="string" />
                <asp:Parameter Name="crg_regisDate" Type="datetime" />
                <asp:Parameter Name="crg_businessCmmDate" Type="datetime" />
                <asp:Parameter Name="crg_suspensionDate" Type="datetime" />
                <asp:Parameter Name="crg_reasonforSuspension" Type="string" />
                <asp:Parameter Name="crg_SubBrokerFranchiseeID" Type="string" />
                  <asp:Parameter Name="crg_Dealer" Type="string" />
                 <asp:Parameter Name="crg_AccountClosureDate" Type="string" />
                 <asp:Parameter Name="crg_FrontOfficeBranchCode" Type="string" />
                 <asp:Parameter Name="crg_FrontOfficeGroupCode" Type="string" />
                 <asp:Parameter Name="crg_ParticipantSchemeCode" Type="string" />
                   <asp:Parameter Name="crg_ClearingBankCode" Type="string" />
                 <asp:Parameter Name="crg_SchemeCode" Type="string" />
                <asp:Parameter Name="crg_STTPattern" Type="string" />
                 <asp:Parameter Name="SttWap" Type="string" />
                 <asp:Parameter Name="MapinSwapSebi" Type="string" />
                 <asp:Parameter Name="crg_exchangesegment" Type="string" />
            
                <asp:Parameter Name="crg_tcode" Type="string" />
                <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="int32" />
            </UpdateParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlComp" runat="server" 
            SelectCommand="SELECT [cmp_internalid], [cmp_Name] FROM [tbl_master_company]"></asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlExchangeSegment" runat="server"
            SelectCommand="select ((select distinct exh_shortName from tbl_master_exchange where tbl_master_companyExchange.exch_exchId=exh_cntId)+' - ' + exch_segmentId) as exch_internalId ,((select distinct exh_shortName from tbl_master_exchange where tbl_master_companyExchange.exch_exchId=exh_cntId)+' - ' +exch_segmentId) as name from tbl_master_companyExchange where exch_compId=@crg_exchange1 union all select (dp_depository+ ' - '+dp_dpid) as exch_internalId ,(dp_depository+ ' - '+dp_dpid) as name from tbl_master_companyDp where dp_companyId=@crg_exchange1">
            <SelectParameters>
                <asp:Parameter Name="crg_exchange1" Type="string" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlProfessional" runat="server" 
            SelectCommand="SELECT [prof_id], [prof_name] FROM [tbl_master_profTechBodies]"></asp:SqlDataSource>
    </div>
</asp:Content>

