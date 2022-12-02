<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.Master.management_master_Lead_FamilyMembers" CodeBehind="Lead_FamilyMembers.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
                //document.location.href="Lead_FamilyMembers.aspx"; 
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
    </script>
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Lead Family Members</h3>
        </div>
        <div class="crossBtn"><a href="Lead.aspx"><i class="fa fa-times"></i></a></div>
    </div>
    <div class="form_main">
        <table class="TableMain100">
            <tr>
                <td>
                    <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="5" ClientInstanceName="page">
                        <ContentStyle>
                            <Border BorderColor="#002D96" BorderStyle="Solid" BorderWidth="1px"></Border>
                        </ContentStyle>
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
                                        <a href="javascript:void(0);" onclick="gridFamily.AddNewRow();" class="btn btn-primary"><span>Add New</span></a>
                                        <dxe:ASPxGridView runat="server" ClientInstanceName="gridFamily" KeyFieldName="Id"
                                            AutoGenerateColumns="False" DataSourceID="FamilyMemberData" Width="100%" Font-Size="12px"
                                            ID="FamilyMemberGrid">
                                            <Columns>
                                                <dxe:GridViewDataTextColumn FieldName="Relation" Width="20%" Caption="Relationship"
                                                    VisibleIndex="0">
                                                    <EditFormSettings Visible="False" Caption="RelationShip"></EditFormSettings>
                                                    <CellStyle CssClass="gridcellleft">
                                                    </CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="Name" Width="20%" Caption="Name" VisibleIndex="1">
                                                    <PropertiesTextEdit>
                                                        <ValidationSettings ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom" SetFocusOnError="True">
                                                            <RequiredField IsRequired="True" ErrorText="Name Required"></RequiredField>
                                                        </ValidationSettings>
                                                    </PropertiesTextEdit>
                                                    <EditFormSettings Visible="True" Caption="Name"></EditFormSettings>
                                                    <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                    </EditFormCaptionStyle>
                                                    <CellStyle CssClass="gridcellleft">
                                                    </CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataDateColumn FieldName="DOB" Width="20%" Caption="Date of Birth"
                                                    VisibleIndex="2">
                                                    <PropertiesDateEdit DisplayFormatString="{0:dd MMM yyyy}" UseMaskBehavior="true" EditFormatString="dd-MM-yyyy" EditFormat="Custom">
                                                        <ValidationSettings ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom" SetFocusOnError="True">
                                                            <RequiredField IsRequired="True" ErrorText="Date of Birth Required"></RequiredField>
                                                        </ValidationSettings>
                                                    </PropertiesDateEdit>
                                                    <EditFormSettings Visible="True"></EditFormSettings>
                                                    <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                    </EditFormCaptionStyle>
                                                    <CellStyle CssClass="gridcellleft">
                                                    </CellStyle>
                                                </dxe:GridViewDataDateColumn>
                                                <dxe:GridViewDataTextColumn FieldName="BloodGroup" Width="20%" Caption="Blood Group"
                                                    VisibleIndex="3">
                                                    <EditFormSettings Visible="False" Caption="Category"></EditFormSettings>
                                                    <CellStyle CssClass="gridcellleft">
                                                    </CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataComboBoxColumn FieldName="RID" Caption="Relationship" Visible="False"
                                                    VisibleIndex="0">
                                                    <PropertiesComboBox DataSourceID="SelectRelation" TextField="fam_familyRelationship"
                                                        ValueField="fam_id" ValueType="System.String" EnableIncrementalFiltering="true">
                                                        <ValidationSettings ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom" SetFocusOnError="True">
                                                            <RequiredField IsRequired="True" ErrorText="RelationShip Required"></RequiredField>
                                                        </ValidationSettings>
                                                    </PropertiesComboBox>
                                                    <EditFormSettings Visible="True"></EditFormSettings>
                                                    <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                    </EditFormCaptionStyle>
                                                </dxe:GridViewDataComboBoxColumn>
                                                <dxe:GridViewDataComboBoxColumn FieldName="BloodGroup" Caption="Blood Group" Visible="False"
                                                    VisibleIndex="0">
                                                    <PropertiesComboBox ValueType="System.String" EnableIncrementalFiltering="true">
                                                        <Items>
                                                            <dxe:ListEditItem Text="A+" Value="A+"></dxe:ListEditItem>
                                                            <dxe:ListEditItem Text="A-" Value="A-"></dxe:ListEditItem>
                                                            <dxe:ListEditItem Text="B+" Value="B+"></dxe:ListEditItem>
                                                            <dxe:ListEditItem Text="B-" Value="B-"></dxe:ListEditItem>
                                                             <dxe:ListEditItem Text="AB+" Value="AB+"></dxe:ListEditItem>
                                                            <dxe:ListEditItem Text="AB-" Value="AB-"></dxe:ListEditItem>
                                                            <dxe:ListEditItem Text="O+" Value="O+"></dxe:ListEditItem>
                                                            <dxe:ListEditItem Text="O-" Value="O-"></dxe:ListEditItem>
                                                        </Items>
                                                        <ValidationSettings ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom" SetFocusOnError="True">
                                                            <RequiredField IsRequired="True" ErrorText="Blood Group Required"></RequiredField>
                                                        </ValidationSettings>
                                                    </PropertiesComboBox>
                                                    <EditFormSettings Visible="True"></EditFormSettings>
                                                    <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                    </EditFormCaptionStyle>
                                                </dxe:GridViewDataComboBoxColumn>
                                                <dxe:GridViewDataTextColumn FieldName="User1" Width="20%" Caption="Create User"
                                                    Visible="False" VisibleIndex="3">
                                                    <EditFormSettings Visible="False" Caption="Category"></EditFormSettings>
                                                    <CellStyle CssClass="gridcellleft">
                                                    </CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewCommandColumn VisibleIndex="4" ShowDeleteButton="true" ShowEditButton="true" HeaderStyle-HorizontalAlign="Center">

                                                    <HeaderTemplate>
                                                    </HeaderTemplate>
                                                </dxe:GridViewCommandColumn>
                                            </Columns>
                                            <SettingsCommandButton>


                                                <EditButton Image-Url="/assests/images/Edit.png" ButtonType="Image" Image-AlternateText="Edit">
                                                </EditButton>
                                                <DeleteButton Image-Url="/assests/images/Delete.png" ButtonType="Image" Image-AlternateText="Delete">
                                                </DeleteButton>
                                                <UpdateButton Text="Update" ButtonType="Button" Styles-Style-CssClass="btn btn-primary btn-xs"></UpdateButton>
                                                <CancelButton Text="Cancel" ButtonType="Button" Styles-Style-CssClass="btn btn-danger btn-xs"></CancelButton>
                                            </SettingsCommandButton>
                                            <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True"></SettingsBehavior>
                                            <SettingsPager PageSize="20" NumericButtonCount="20" ShowSeparators="True">
                                                <FirstPageButton Visible="True">
                                                </FirstPageButton>
                                                <LastPageButton Visible="True">
                                                </LastPageButton>
                                            </SettingsPager>
                                            <SettingsEditing Mode="PopupEditForm" PopupEditFormWidth="500px"
                                                PopupEditFormHorizontalAlign="Center" PopupEditFormVerticalAlign="WindowCenter"
                                                PopupEditFormModal="True" EditFormColumnCount="1">
                                            </SettingsEditing>
                                            <Settings ShowStatusBar="Visible" ShowGroupPanel="true"></Settings>
                                            <SettingsText ConfirmDelete="Confirm delete?" PopupEditFormCaption="Add/Modify Family Relationship"></SettingsText>
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
                        <TabStyle Font-Size="12px">
                        </TabStyle>
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
                        <LoadingPanelStyle ImageSpacing="6px">
                        </LoadingPanelStyle>
                    </dxe:ASPxPageControl>
                </td>
            </tr>
        </table>
    </div>
    <asp:SqlDataSource ID="FamilyMemberData" runat="server"
        InsertCommand="insert into tbl_master_contactfamilyrelationship(femrel_relationId,femrel_memberName,femrel_DOB,femrel_bloodgroup,femrel_cntId,femrel_contacttype,CreateDate,CreateUser) values(@RID,@Name,@DOB,@BloodGroup,@insuId,'contact',getdate(),@User1)"
        SelectCommand="select tbl_master_contactFamilyRelationship.femrel_id AS Id, 
                        tbl_master_familyRelationship.fam_familyRelationship AS Relation, tbl_master_contactFamilyRelationship.femrel_memberName AS Name, 
                        tbl_master_contactFamilyRelationship.femrel_bloodGroup AS BloodGroup, cast(tbl_master_contactFamilyRelationship.femrel_DOB as datetime) AS DOB,
                        tbl_master_contactFamilyRelationship.femrel_relationId as RID,
                        tbl_master_contactFamilyRelationship.CreateUser as User1 
                        from tbl_master_contactFamilyRelationship INNER JOIN tbl_master_familyRelationship 
                        ON tbl_master_contactFamilyRelationship.femrel_relationId = tbl_master_familyRelationship.fam_id 
                        where tbl_master_contactFamilyRelationship.femrel_cntId =@insuId 
                        AND tbl_master_contactFamilyRelationship.femrel_contactType = 'contact'"
        DeleteCommand="delete from tbl_master_contactfamilyrelationship where femrel_id=@Id"
        UpdateCommand="update tbl_master_contactfamilyrelationship set femrel_relationId=@RID,femrel_memberName=@Name,femrel_DOB=@DOB,femrel_bloodgroup=@BloodGroup,LastModifyDate=getdate(),LastModifyUser=@User1 where femrel_id=@Id">
        <InsertParameters>
            <asp:Parameter Name="RID" Type="decimal" />
            <asp:Parameter Name="Name" Type="string" />
            <asp:Parameter Name="DOB" Type="dateTime" />
            <asp:Parameter Name="BloodGroup" Type="string" />
            <asp:SessionParameter Name="insuId" SessionField="KeyVal_InternalID" Type="String" />
            <asp:SessionParameter Name="User1" SessionField="userid" Type="Decimal" />
        </InsertParameters>
        <SelectParameters>
            <asp:SessionParameter Name="insuId" SessionField="KeyVal_InternalID" Type="String" />
        </SelectParameters>
        <DeleteParameters>
            <asp:Parameter Name="Id" Type="decimal" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="RID" Type="decimal" />
            <asp:Parameter Name="Name" Type="string" />
            <asp:Parameter Name="DOB" Type="dateTime" />
            <asp:Parameter Name="BloodGroup" Type="string" />
            <asp:SessionParameter Name="User1" SessionField="userid" Type="Decimal" />
            <asp:Parameter Name="Id" Type="decimal" />
        </UpdateParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SelectRelation" runat="server" 
        SelectCommand="select fam_familyRelationship,fam_id from tbl_master_familyrelationship order by fam_familyRelationship"></asp:SqlDataSource>
</asp:Content>
