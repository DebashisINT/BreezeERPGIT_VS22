<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/PopUp.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.Master.management_master_Contact_Education" Codebehind="Contact_Education.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
      
    
    <script language="javascript" type="text/javascript">
       
        function disp_prompt(name) {
            //var ID = document.getElementById(txtID);
            if (name == "tab0") {
                //alert(name);
                document.location.href = "Contact_general.aspx";
            }
            if (name == "tab1") {
                //alert(name);
                document.location.href = "Contact_Correspondence.aspx";
            }
            else if (name == "tab2") {
                //alert(name);
                document.location.href = "Employee_BankDetails.aspx";
            }
            else if (name == "tab3") {
                //alert(name);
                document.location.href = "Contact_DPDetails.aspx";
            }
            else if (name == "tab4") {
                //alert(name);
                document.location.href = "Contact_Document.aspx";
            }
            else if (name == "tab12") {
                //alert(name);
                document.location.href = "Contact_FamilyMembers.aspx";
            }
            else if (name == "tab5") {
                //alert(name);
                document.location.href = "Contact_Registration.aspx";
            }
            else if (name == "tab7") {
                //alert(name);
                document.location.href = "Contact_GroupMember.aspx";
            }
            else if (name == "tab8") {
                //alert(name);
                document.location.href = "Contact_Deposit.aspx";
            }
            else if (name == "tab9") {
                //alert(name);
                document.location.href = "Contact_Remarks.aspx";
            }
            else if (name == "tab10") {
                //alert(name);
                //document.location.href="Contact_Education.aspx"; 
            }
            else if (name == "tab11") {
                //alert(name);
                document.location.href = "contact_brokerage.aspx";
            }
            else if (name == "tab6") {
                //alert(name);
                document.location.href = "contact_other.aspx";
            }
            else if (name == "tab13") {
                document.location.href = "contact_Subscription.aspx";
            }

        }
        function OnCountryChanged(cmbCountry) {
            grid.GetEditor("edu_state").PerformCallback(cmbCountry.GetValue().toString());
        }
        function OnStateChanged(cmbState) {
            grid.GetEditor("edu_city").PerformCallback(cmbState.GetValue().toString());
        }
</script>
</asp:Content>
   
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
    <table width="100%">
        <tr>
           <td class="EHEADER" style="text-align:center">
                 <asp:Label ID="lblName" runat="server" Font-Bold="True"></asp:Label>
           </td>
       </tr>
    </table>  
    <table class="TableMain100">
          <tr>
            <td>
                <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="10" ClientInstanceName="page">
                
                <TabPages>
                    <dxe:TabPage  Name="General">
                    <TabTemplate ><span style="font-size:x-small">General</span>&nbsp;<span style="color:Red;">*</span> </TabTemplate>
                    <ContentCollection>
                        <dxe:ContentControl runat="server">
                        </dxe:ContentControl>
                        </ContentCollection>
                    </dxe:TabPage>
                    <dxe:TabPage  Name="CorresPondence">
                    <TabTemplate ><span style="font-size:x-small">CorresPondence</span>&nbsp;<span style="color:Red;">*</span> </TabTemplate>
                    <ContentCollection>
                        <dxe:ContentControl runat="server">
                        </dxe:ContentControl>
                        </ContentCollection>
                        </dxe:TabPage>
                    <dxe:TabPage Name="Bank Details">
                    <TabTemplate ><span style="font-size:x-small">Bank</span>&nbsp;<span style="color:Red;">*</span> </TabTemplate>
                        <ContentCollection>
                            <dxe:ContentControl runat="server">
                            </dxe:ContentControl>
                        </ContentCollection>
                    </dxe:TabPage>
                    <dxe:TabPage Name="DP Details" Visible="false" >
                    <TabTemplate ><span style="font-size:x-small">DP</span>&nbsp;<span style="color:Red;">*</span> </TabTemplate>
                        <ContentCollection>
                            <dxe:ContentControl runat="server">
                            </dxe:ContentControl>
                        </ContentCollection>
                    </dxe:TabPage>
                    <dxe:TabPage Name="Documents" >
                    <TabTemplate ><span style="font-size:x-small">Documents</span>&nbsp;<span style="color:Red;">*</span> </TabTemplate>
                        <ContentCollection>
                            <dxe:ContentControl runat="server">
                            </dxe:ContentControl>
                        </ContentCollection>
                    </dxe:TabPage>
                    
                    
                    
                    <dxe:TabPage Name="Registration">
                    <TabTemplate ><span style="font-size:x-small">Registration</span>&nbsp;<span style="color:Red;">*</span> </TabTemplate>
                        <ContentCollection>
                            <dxe:ContentControl runat="server">
                            </dxe:ContentControl>
                        </ContentCollection>
                    </dxe:TabPage>
                    
                    <dxe:TabPage Name="Other">
                    <TabTemplate ><span style="font-size:x-small">Other</span>&nbsp;<span style="color:Red;">*</span> </TabTemplate>
                        <ContentCollection>
                            <dxe:ContentControl runat="server">
                            </dxe:ContentControl>
                        </ContentCollection>
                    </dxe:TabPage>
                    <dxe:TabPage Name="Group Member" Text="Group">
                        <ContentCollection>
                            <dxe:ContentControl runat="server">
                            </dxe:ContentControl>
                        </ContentCollection>
                    </dxe:TabPage>
                    <dxe:TabPage Name="Deposit" Text="Deposit">
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
                     <dxe:TabPage Name="Education" Text="Education">
                        <ContentCollection>
                            <dxe:ContentControl runat="server">
                                <dxe:ASPxGridView ID="gridEducation" runat="server" ClientInstanceName="grid" AutoGenerateColumns="False" DataSourceID="sqleducation" KeyFieldName="edu_id" Width="100%" OnCellEditorInitialize="gridEducation_CellEditorInitialize1" OnInitNewRow="gridEducation_InitNewRow">
                                    <Columns>
                                        <dxe:GridViewDataTextColumn FieldName="edu_id" ReadOnly="True" VisibleIndex="0" Visible="False">
                                            <EditFormSettings Visible="False" />
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn FieldName="edu_internalId" VisibleIndex="0" Visible="False">
                                            <EditFormSettings Visible="False" />
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataComboBoxColumn Caption="Degree" FieldName="edu_degree" VisibleIndex="0">
                                            <PropertiesComboBox DataSourceID="sqlDegree" ValueField="edu_id" TextField="edu_education" EnableIncrementalFiltering="True" ValueType="System.String" >
                                      
                                                
                                                 </PropertiesComboBox>
                                            <EditFormSettings Visible="True" VisibleIndex="1" />
                                            <CellStyle CssClass="gridcellleft">
                                            </CellStyle>
                                            <EditFormCaptionStyle Wrap="False">
                                            </EditFormCaptionStyle>
                                        </dxe:GridViewDataComboBoxColumn>
                                        <dxe:GridViewDataTextColumn FieldName="edu_instName" VisibleIndex="1" Caption="Institute Name">
                                            <EditFormSettings Visible="True" Caption="Institute Name" VisibleIndex="2" />
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
                                        <dxe:GridViewDataComboBoxColumn Caption="Country" FieldName="edu_country" VisibleIndex="2" Visible="false">
                                            <PropertiesComboBox DataSourceID="SqlCountry" ValueField="cou_id" TextField="cou_country" ValueType="System.String" EnableIncrementalFiltering="True" EnableSynchronization="False">
                                                <ClientSideEvents  SelectedIndexChanged="function(s, e) { OnCountryChanged(s); }"></ClientSideEvents>
                                            </PropertiesComboBox>
                                            <EditFormSettings Visible="True" VisibleIndex="3" />
                                            <EditFormCaptionStyle Wrap="False">
                                            </EditFormCaptionStyle>
                                        </dxe:GridViewDataComboBoxColumn>
                                        <dxe:GridViewDataComboBoxColumn Caption="State" FieldName="edu_state" VisibleIndex="3" Visible="false">
                                            <PropertiesComboBox DataSourceID="StateSelect" ValueField="ID" TextField="State" ValueType="System.String" EnableIncrementalFiltering="True" EnableSynchronization="False">
                                                <ClientSideEvents SelectedIndexChanged="function(s, e) { OnStateChanged(s); }"></ClientSideEvents>
                                            </PropertiesComboBox>
                                            <EditFormSettings Visible="True" VisibleIndex="4" />
                                            <EditFormCaptionStyle Wrap="False">
                                            </EditFormCaptionStyle>
                                        </dxe:GridViewDataComboBoxColumn>
                                        <dxe:GridViewDataComboBoxColumn Caption="City" FieldName="edu_city" VisibleIndex="4">
                                            <PropertiesComboBox DataSourceID="SelectCity" ValueField="CityId" TextField="City" ValueType="System.String" EnableIncrementalFiltering="True" EnableSynchronization="False">
                                            </PropertiesComboBox>
                                            <EditFormSettings Visible="True" VisibleIndex="5" />
                                            <EditFormCaptionStyle Wrap="False">
                                            </EditFormCaptionStyle>
                                        </dxe:GridViewDataComboBoxColumn>
                                        <dxe:GridViewDataDateColumn Caption="From" FieldName="edu_courseFrom" Visible="false" VisibleIndex="5">
                                            <PropertiesDateEdit EditFormat="Custom" EditFormatString="dd MM yyyy" UseMaskBehavior="true">
                                                <ValidationSettings Display="Dynamic" ErrorDisplayMode="Text" SetFocusOnError="True">
                                                    <RequiredField ErrorText="Required" IsRequired="True" />
                                                </ValidationSettings>
                                            </PropertiesDateEdit>
                                            <EditFormSettings Visible="True" VisibleIndex="6" />
                                            <EditFormCaptionStyle Wrap="False">
                                            </EditFormCaptionStyle>
                                        </dxe:GridViewDataDateColumn>
                                        <dxe:GridViewDataDateColumn Caption="To" FieldName="edu_courseuntil"  Visible="false" VisibleIndex="6">
                                            <PropertiesDateEdit EditFormat="Custom" EditFormatString="dd MM yyyy" UseMaskBehavior="true">
                                                <ValidationSettings Display="Dynamic" ErrorDisplayMode="Text" SetFocusOnError="True">
                                                    <RequiredField ErrorText="Required" IsRequired="True" />
                                                </ValidationSettings>
                                            </PropertiesDateEdit>
                                            <EditFormSettings Visible="True" VisibleIndex="7" />
                                            <EditFormCaptionStyle Wrap="False">
                                            </EditFormCaptionStyle>
                                        </dxe:GridViewDataDateColumn>
                                        <dxe:GridViewDataComboBoxColumn Caption="Result" FieldName="edu_courseResult" VisibleIndex="7">
                                            <PropertiesComboBox ValueType="System.String" EnableIncrementalFiltering="True">
                                                <Items>
                                                    <dxe:ListEditItem Text="Passed" Value="Passed">
                                                    </dxe:ListEditItem>
                                                    <dxe:ListEditItem Text="Failed" Value="Failed">
                                                    </dxe:ListEditItem>
                                                    <dxe:ListEditItem Text="Discontinued" Value="Discontinued">
                                                    </dxe:ListEditItem>
                                                    <dxe:ListEditItem Text="Passed with distinction" Value="Passed with distinction">
                                                    </dxe:ListEditItem>
                                                 </Items>
                                            </PropertiesComboBox>
                                            <EditFormSettings Visible="True" VisibleIndex="8" />
                                             <CellStyle CssClass="gridcellleft">
                                            </CellStyle>
                                            <EditFormCaptionStyle Wrap="False">
                                            </EditFormCaptionStyle>
                                        </dxe:GridViewDataComboBoxColumn>
                                        <dxe:GridViewDataTextColumn FieldName="edu_percentage" VisibleIndex="8" Caption="Percentage">
                                            <EditFormSettings Visible="true" Caption="Percentage" VisibleIndex="9" />
                                             <CellStyle CssClass="gridcellleft">
                                            </CellStyle>
                                            <EditFormCaptionStyle Wrap="False">
                                            </EditFormCaptionStyle>
                                            <PropertiesTextEdit>
                                                <ValidationSettings Display="Dynamic" ErrorDisplayMode="Text" SetFocusOnError="True">
                                                    <RegularExpression ErrorText="Valid Percentage" ValidationExpression="^(\d{0,13}\.\d{0,5}|\d{0,13})$" />
                                                    <RequiredField ErrorText="Required" IsRequired="True" />
                                                </ValidationSettings>
                                            </PropertiesTextEdit>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn FieldName="edu_grade" VisibleIndex="9" Caption="Grade">
                                            <EditFormSettings Visible="True" Caption="Grade" VisibleIndex="10" />
                                             <CellStyle CssClass="gridcellleft">
                                            </CellStyle>
                                            <EditFormCaptionStyle Wrap="False">
                                            </EditFormCaptionStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataDateColumn Caption="Passing Month & Year" Visible="false" FieldName="edu_month_year"
                                            VisibleIndex="10">
                                            <PropertiesDateEdit DisplayFormatString="" EditFormat="Custom" EditFormatString="MM yyyy" UseMaskBehavior="true">
                                            </PropertiesDateEdit>
                                            <EditFormSettings Visible="True" VisibleIndex="10" />
                                            <EditFormCaptionStyle Wrap="False">
                                            </EditFormCaptionStyle>
                                        </dxe:GridViewDataDateColumn>
                                        <dxe:GridViewDataTextColumn FieldName="createuser" VisibleIndex="11" Visible="False">
                                            <EditFormSettings Visible="False" />
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataDateColumn FieldName="createdate" VisibleIndex="11" Visible="False">
                                            <EditFormSettings Visible="False" />
                                        </dxe:GridViewDataDateColumn>
                                        <dxe:GridViewDataTextColumn FieldName="lastmodifyuser" VisibleIndex="11" Visible="False">
                                            <EditFormSettings Visible="False" />
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataDateColumn FieldName="lastmodifydate" VisibleIndex="11" Visible="False">
                                            <EditFormSettings Visible="False" />
                                        </dxe:GridViewDataDateColumn>
                                         <dxe:GridViewDataTextColumn FieldName="edu_courseFrom1" VisibleIndex="5" Caption="From">
                                            <EditFormSettings Visible="false" />
                                             <CellStyle CssClass="gridcellleft">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                         <dxe:GridViewDataTextColumn FieldName="edu_courseuntil1" VisibleIndex="6" Caption="To">
                                            <EditFormSettings Visible="false" />
                                             <CellStyle CssClass="gridcellleft">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                         <dxe:GridViewDataTextColumn FieldName="edu_country1" VisibleIndex="2"  Caption="Country">
                                            <EditFormSettings Visible="false" />
                                             <CellStyle CssClass="gridcellleft">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                         <dxe:GridViewDataTextColumn FieldName="edu_state1"  VisibleIndex="3" Caption="State">
                                            <EditFormSettings Visible="false" />
                                             <CellStyle CssClass="gridcellleft">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn FieldName="edu_month_year1"  VisibleIndex="10" Caption="Passing Month & Year">
                                            <EditFormSettings Visible="false" />
                                             <CellStyle CssClass="gridcellleft">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewCommandColumn VisibleIndex="11" ShowDeleteButton="true" ShowEditButton="true">
                                           
                                             <HeaderTemplate>
                                                            <a href="javascript:void(0);" onclick="grid.AddNewRow();"><span >Add New</span> </a>
                                                        </HeaderTemplate>
                                        </dxe:GridViewCommandColumn>
                                    </Columns>
                                    
  								<SettingsCommandButton>
                                                                      <DeleteButton  Text="Delete">
                                                                        </DeleteButton>
                                                                       <EditButton  Text="Edit">
                                                                        </EditButton>
                                                                </SettingsCommandButton>
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
                                    <Settings ShowFooter="True" ShowStatusBar="Visible" ShowTitlePanel="True" ShowGroupButtons="False" />
                                    <SettingsEditing Mode="PopupEditForm" PopupEditFormHeight="400px" PopupEditFormHorizontalAlign="Center"
                                        PopupEditFormModal="True" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="700px" EditFormColumnCount="1" />
                                    <Styles>
                                     <LoadingPanel ImageSpacing="10px">
                                     </LoadingPanel>
                                     <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                    </Header>
                                     </Styles>
                                        <SettingsText PopupEditFormCaption="Add/Modify Education" ConfirmDelete="Confirm delete?" Title="Add Address" />
                                        <SettingsPager NumericButtonCount="20" PageSize="20">
                                        </SettingsPager>
                                        <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True" />
                                        <Templates>
                                          <EditForm>
                                             <table style="width:100%">
                                               <tr><td style="width:25%"></td>
                                                   <td style="width:50%">
                                                     <controls>
                                                       <dxe:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormEditors" ColumnID="" ID="Editors">
                                                       </dxe:ASPxGridViewTemplateReplacement>                                                           
                                                     </controls>
                                                     <div style="text-align:right; padding:2px 2px 2px 2px">
                                                       <dxe:ASPxGridViewTemplateReplacement ID="UpdateButton" ReplacementType="EditFormUpdateButton" runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                       <dxe:ASPxGridViewTemplateReplacement ID="CancelButton" ReplacementType="EditFormCancelButton" runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                     </div>
                                                   </td>
                                                   <td style="width:25%"></td>
                                               </tr>
                                            </table>
                                        </EditForm>
                                        <TitlePanel>
                                            <table style="width:100%">
                                              <tr>
                                                <td align="center" style="width:50%">
                                                    <span style="color:White; font-weight:bold; font-size:12px; text-align:center" >Add/Modify Education.</span>
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
                    <dxe:TabPage Name="Trad. Prof." Text="Trad.Prof">
                    <%--<TabTemplate ><span style="font-size:x-small">Trad.Prof</span>&nbsp;<span style="color:Red;">*</span> </TabTemplate>--%>
                        <ContentCollection>
                            <dxe:ContentControl runat="server">
                            </dxe:ContentControl>
                        </ContentCollection>
                    </dxe:TabPage>
                    
                    <dxe:TabPage Name="Family Members" Text="Family">
                        <ContentCollection>
                            <dxe:ContentControl runat="server">
                            </dxe:ContentControl>
                        </ContentCollection>
                    </dxe:TabPage>
                    
                    
                    <dxe:TabPage Name="Subscription" Text="Subscription">
                        <ContentCollection>
                            <dxe:ContentControl runat="server">
                            </dxe:ContentControl>
                        </ContentCollection>
                    </dxe:TabPage>
                    
                    </TabPages>
                    <ClientSideEvents  ActiveTabChanged="function(s, e) {
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
	                                            var Tab9 = page.GetTab(9);
	                                            var Tab10 = page.GetTab(10);
	                                            var Tab11 = page.GetTab(11);
	                                            var Tab12 = page.GetTab(12);
	                                             var Tab13=page.GetTab(13);
	                                            
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
	                                            else if(activeTab == Tab9)
	                                            {
	                                                disp_prompt('tab9');
	                                            }
	                                            else if(activeTab == Tab10)
	                                            {
	                                                disp_prompt('tab10');
	                                            }
	                                            else if(activeTab == Tab11)
	                                            {
	                                                disp_prompt('tab11');
	                                            }
	                                            else if(activeTab == Tab12)
	                                            {
	                                                disp_prompt('tab12');
	                                            }
	                                            else if(activeTab == Tab13)
	                                            {
	                                               disp_prompt('tab13');
	                                            }
	                                           
	                                            }">
                    </ClientSideEvents>
                    <ContentStyle>
                        <Border BorderColor="#002D96" BorderStyle="Solid" BorderWidth="1px" />
                    </ContentStyle>
                    <LoadingPanelStyle ImageSpacing="6px">
                    </LoadingPanelStyle>
                    <TabStyle Font-Size="12px">
                    </TabStyle>
                </dxe:ASPxPageControl>
              </td>
            </tr>
            <tr>
            <td>
            </td>
            </tr>
        </table>
        <asp:SqlDataSource ID="sqleducation" runat="server" 
                                    DeleteCommand="DELETE FROM [tbl_master_educationProfessional] WHERE [edu_id] = @edu_id"
                                    InsertCommand="EmployeeEducationInsert" InsertCommandType="StoredProcedure"
                                    SelectCommand="select ep.edu_id as edu_id,ep.edu_internalId as edu_internalId,ep.edu_instName as edu_instName,ep.edu_courseFrom as edu_courseFrom,case edu_degree when '' then 'N/A' else(select edu_education from tbl_master_education where ep.edu_degree=edu_id) end as edu_degree,ep.edu_courseuntil as edu_courseuntil,ep.edu_courseResult as edu_courseResult,ep.edu_percentage as edu_percentage,ep.edu_grade as edu_grade,ep.edu_month_year as edu_month_year,ep.createuser as createuser,ep.createdate as createdate,ep.lastmodifyuser as lastmodifyuser,ep.lastmodifydate as lastmodifydate,case edu_state when  '0' then 'N/A' else (select state from tbl_master_state where ep.edu_state=id) end as edu_state1,case edu_city when null then '0' else(select city_name from tbl_master_city where ep.edu_city=city_id) end as edu_city,case edu_country when '0' then 'N/A' else(select cou_country from tbl_master_country where ep.edu_country=cou_id) end as edu_country1,convert(varchar(11),ep.edu_courseFrom,113) as edu_courseFrom1,convert(varchar(11),ep.edu_courseuntil,113) as edu_courseuntil1,ep.edu_country as edu_country,ep.edu_state as edu_state,Right(Convert(VARCHAR(11),ep.edu_month_year,113),8) as  edu_month_year1 from tbl_master_educationProfessional ep where ep.edu_internalId=@edu_internalId" 
                                    UpdateCommand="UPDATE [tbl_master_educationProfessional] SET [edu_degree] = @edu_degree, [edu_instName] = @edu_instName, [edu_country] = @edu_country, [edu_state] = @edu_state, [edu_city] = @edu_city, [edu_courseFrom] = @edu_courseFrom, [edu_courseuntil] = @edu_courseuntil, [edu_courseResult] = @edu_courseResult, [edu_percentage] = @edu_percentage, [edu_grade] = @edu_grade, [edu_month_year] = @edu_month_year,  [lastmodifyuser] = @lastmodifyuser, [lastmodifydate] = getdate() WHERE [edu_id] = @edu_id">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="edu_internalId" SessionField="KeyVal_InternalID" Type="String" />
                                    </SelectParameters>
                                    <DeleteParameters>
                                        <asp:Parameter Name="edu_id" Type="Int32" />
                                    </DeleteParameters>
                                    <InsertParameters>
                                        <asp:SessionParameter Name="edu_internalId" SessionField="KeyVal_InternalID" Type="String" />
                                        <asp:Parameter Name="edu_degree" Type="String" />
                                        <asp:Parameter Name="edu_instName" Type="String" />
                                        <asp:Parameter Name="edu_country" Type="Int32" />
                                        <asp:Parameter Name="edu_state" Type="Int32" />
                                        <asp:Parameter Name="edu_city" Type="Int32" />
                                        <asp:Parameter Type="datetime" Name="edu_courseFrom" />
                                        <asp:Parameter Type="datetime" Name="edu_courseuntil" />
                                        <asp:Parameter Name="edu_courseResult" Type="String" />
                                        <asp:Parameter Name="edu_percentage" Type="string" />
                                        <asp:Parameter Name="edu_grade" Type="String" />
                                        <asp:Parameter Name="edu_month_year" Type="String" />
                                        <asp:SessionParameter Name="createuser" SessionField="userid" Type="Int32" />
                                    </InsertParameters>
                                    <UpdateParameters>
                                        <asp:Parameter Name="edu_degree" Type="String" />
                                        <asp:Parameter Name="edu_instName" Type="String" />
                                        <asp:Parameter Name="edu_country" Type="Int32" />
                                        <asp:Parameter Name="edu_state" Type="Int32" />
                                        <asp:Parameter Name="edu_city" Type="Int32" />
                                        <asp:Parameter Type="datetime" Name="edu_courseFrom" />
                                        <asp:Parameter Type="datetime" Name="edu_courseuntil" />
                                        <asp:Parameter Name="edu_courseResult" Type="String" />
                                        <asp:Parameter Name="edu_percentage" Type="string" />
                                        <asp:Parameter Name="edu_grade" Type="String" />
                                        <asp:Parameter Name="edu_month_year" Type="String" />
                                        <asp:SessionParameter Name="lastmodifyuser" SessionField="userid" Type="Decimal" />
                                        <asp:Parameter Type="datetime" Name="lastmodifydate" />
                                        <asp:Parameter Name="edu_id" Type="Int32" />
                                    </UpdateParameters>
                                </asp:SqlDataSource>
        <asp:SqlDataSource ID="sqlDegree" runat="server"  SelectCommand="SELECT [edu_id], [edu_education] FROM [tbl_master_education] order by edu_education"></asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlCountry" runat="server"  SelectCommand="SELECT [cou_id], [cou_country] FROM [tbl_master_country] order by cou_country"></asp:SqlDataSource>
        <asp:SqlDataSource ID="StateSelect" runat="server" 
            SelectCommand="SELECT s.id as ID,s.state as State from tbl_master_state s where (s.countryId = @State) ORDER BY s.state">
            <SelectParameters>
                <asp:Parameter Name="State" Type="string"/>
            </SelectParameters>
        </asp:SqlDataSource>
         <asp:SqlDataSource ID="SelectCity" runat="server" 
            SelectCommand="SELECT c.city_id AS CityId, c.city_name AS City FROM tbl_master_city c where c.state_id=@City order by c.city_name">
             <SelectParameters>
                <asp:Parameter Name="City" Type="string"/>
            </SelectParameters>
        </asp:SqlDataSource>
    </div>   
    
   
  </asp:Content>

