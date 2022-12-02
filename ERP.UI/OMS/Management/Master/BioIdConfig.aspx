<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="BioIdConfig.aspx.cs" Inherits="ERP.OMS.Management.Master.BioIdConfig" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

     <div class="panel-heading">
        <div class="panel-title">
            <h3>Employees Biometric Configuration</h3>
        </div>
    </div>
    <div class="form_main">
        
      <dxe:ASPxGridView ID="GrdEmployee" runat="server" KeyFieldName="cnt_id" AutoGenerateColumns="False"
                        Width="100%" ClientInstanceName="cGrdEmployee"   SettingsBehavior-AllowFocusedRow="true" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false">
                        <SettingsSearchPanel Visible="True" Delay="5000" />
                              <SettingsBehavior AllowFocusedRow="true"  ColumnResizeMode="NextColumn" />
                        <Styles>
                            <Header SortingImageSpacing="5px" ImageSpacing="5px">
                            </Header>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                            <Row Wrap="true">
                            </Row>
                           
                        </Styles>

                        <Columns>
                            <dxe:GridViewDataTextColumn Caption="Code" Visible="False" FieldName="ContactID"
                                VisibleIndex="0" FixedStyle="Left">
                                <PropertiesTextEdit DisplayFormatInEditMode="True">
                                </PropertiesTextEdit>
                                <CellStyle CssClass="gridcellleft" Wrap="False">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Caption="Name" FieldName="Name"
                                VisibleIndex="2" FixedStyle="Left">
                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataDateColumn Caption="Joining On" FieldName="DOJ"
                                VisibleIndex="7" Width="100px" ReadOnly="True">
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataDateColumn>

                            <dxe:GridViewDataTextColumn Caption="Department" FieldName="Department"
                                VisibleIndex="5" Width="120px">
                                <CellStyle CssClass="gridcellleft" Wrap="False">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Caption="Branch" FieldName="BranchName"
                                VisibleIndex="4" Width="75px">
                                <CellStyle CssClass="gridcellleft" Wrap="False">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Caption="CTC" FieldName="CTC"
                                VisibleIndex="6" Width="75px" Visible="false">
                                <CellStyle CssClass="gridcellleft" Wrap="False" HorizontalAlign="Left">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Caption="Report To" FieldName="ReportTo"
                                VisibleIndex="8" Width="150px">
                                <CellStyle CssClass="gridcellleft" Wrap="False">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Caption="Designation" FieldName="Designation"
                                VisibleIndex="6" Width="150px">
                                <CellStyle CssClass="gridcellleft" Wrap="False">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Company" FieldName="Company"
                                VisibleIndex="3" Width="150px">
                                <CellStyle CssClass="gridcellleft" Wrap="False">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewCommandColumn Visible="False" ShowDeleteButton="true" VisibleIndex="16">
                                
                          
                            </dxe:GridViewCommandColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="17" Width="12%">
                                <DataItemTemplate>
                                     
                                    <a href="javascript:void(0);" onclick="OnContactInfoClick('<%#Eval("ContactID") %>','<%#Eval("Name") %>')" title="show contact person" class="pad">
                                        <img src="../../../assests/images/show.png" style="padding-right: 8px" />
                                    </a> 
                                </DataItemTemplate>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />

                                <HeaderTemplate><span>Actions</span></HeaderTemplate>

                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Caption="Employee Code" FieldName="Code"
                                VisibleIndex="1" FixedStyle="Left" Width="150px">
                                <CellStyle CssClass="gridcellleft" Wrap="False">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>

                        </Columns>
                        <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                        <SettingsPager  PageSize="10" ShowSeparators="True">
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                             <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200"/>
                        </SettingsPager>
                        <SettingsCommandButton>
                            <DeleteButton ButtonType="Image" Image-Url="/assests/images/Delete.png">
                            </DeleteButton>
                        </SettingsCommandButton>
                        <SettingsEditing Mode="PopupEditForm" PopupEditFormHorizontalAlign="Center" PopupEditFormModal="True"
                            PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="900px" EditFormColumnCount="3" />
                        <SettingsText PopupEditFormCaption="Add/ Modify Employee" ConfirmDelete="Are you sure to delete?" />
                  
                        <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                        <SettingsLoadingPanel Text="Please Wait..." />
                    </dxe:ASPxGridView>


        </div>
</asp:Content>
