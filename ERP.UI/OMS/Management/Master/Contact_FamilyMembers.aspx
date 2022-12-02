<%@ Page Language="C#" Title="Family" MasterPageFile="~/OMS/MasterPage/Erp.Master" AutoEventWireup="true"
    Inherits="ERP.OMS.Management.Master.management_master_Contact_FamilyMembers" CodeBehind="Contact_FamilyMembers.aspx.cs" %>

<%--<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe000001" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxTabControl" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxClasses" TagPrefix="dxe" %>
<%@ Register assembly="DevExpress.Web.v10.2" namespace="DevExpress.Web.ASPxTabControl" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v10.2" namespace="DevExpress.Web.ASPxClasses" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v10.2" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v10.2" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<%--    <title>Family Members</title>--%>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="javascript" type="text/javascript">
        $(document).ready(function({}))
            function disp_prompt(name) {
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
                    document.location.href = "Contact_BankDetails.aspx";
                }
                else if (name == "tab3") {
                    //alert(name);
                    // document.location.href="Contact_DPDetails.aspx"; 
                }
                else if (name == "tab4") {
                    //alert(name);
                    document.location.href = "Contact_Document.aspx";
                }
                else if (name == "tab12") {
                    //alert(name);
                    //document.location.href="Contact_FamilyMembers.aspx"; 
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
                    document.location.href = "Contact_Education.aspx";
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
    </script>
    <style>
        /*.abc{position:absolute; }*/
       #FamilyMemberGrid_DXPEForm_ef1_DXEFL_1_DXEditor1_CC,#FamilyMemberGrid_DXPEForm_ef1_DXEFL_1_DXEditor3_CC,
       #FamilyMemberGrid_DXPEForm_efnew_DXEFL_DXEditor3_CC,#FamilyMemberGrid_DXPEForm_efnew_DXEFL_DXEditor1_CC,
       #FamilyMemberGrid_DXPEForm_efnew_DXEFL_DXEditor4,#FamilyMemberGrid_DXPEForm_ef1_DXEFL_1_DXEditor4,#FamilyMemberGrid_DXPEForm_ef1_DXEFL_1_DXEditor1, #FamilyMemberGrid_DXPEForm_ef1_DXEFL_1_DXEditor2,#FamilyMemberGrid_DXPEForm_ef1_DXEFL_1_DXEditor3, #FamilyMemberGrid_DXPEForm_efnew_DXEFL_DXEditor2, #FamilyMemberGrid_DXPEForm_efnew_DXEFL_DXEditor3, #FamilyMemberGrid_DXPEForm_efnew_DXEFL_DXEditor1
        {
            width:300px !important;
        }
       #FamilyMemberGrid_DXPEForm_ef0_DXEFL_0_DXEditor3_EC,
       #FamilyMemberGrid_DXPEForm_ef0_DXEFL_0_DXEditor1_EC {
           position:absolute;
       }
    </style>


          <div class="panel-heading">
        <div class="panel-title">
           <%-- <h3>Contact Bank List</h3>--%>
             <h3>Family Members List </h3>
            <div class="crossBtn"><a href="frmContactMain.aspx?requesttype=<%= Session["Contactrequesttype"] %>""><i class="fa fa-times"></i></a></div>
        </div>

    </div>
    <div class="form_main">
    <table width="100%">
        <tr>
            <td class="EHEADER" style="text-align: center">
                <asp:Label ID="lblName" runat="server" Font-Bold="True"></asp:Label>
            </td>
        </tr>
    </table>
        </div>
    <table class="TableMain100">
        <tr>
            <td>
                <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="12" ClientInstanceName="page">
                    <TabPages>
                        <dxe:TabPage Name="General" Text="General">
                            <ContentCollection>
                                <dxe:ContentControl runat="server">
                                </dxe:ContentControl>
                            </ContentCollection>
                        </dxe:TabPage>
                        <dxe:TabPage Name="CorresPondence" Text="Correspondence">

                            <ContentCollection>
                                <dxe:ContentControl runat="server">
                                </dxe:ContentControl>
                            </ContentCollection>
                        </dxe:TabPage>
                        <dxe:TabPage Name="Bank Details" Text="Bank">

                            <ContentCollection>
                                <dxe:ContentControl runat="server">
                                </dxe:ContentControl>
                            </ContentCollection>
                        </dxe:TabPage>
                        <dxe:TabPage Name="DP Details" Visible="false">
                            <TabTemplate><span style="font-size: x-small">DP</span>&nbsp;<span style="color: Red;">*</span> </TabTemplate>
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

                        <dxe:TabPage Name="Registration" Text="Registration">

                            <ContentCollection>
                                <dxe:ContentControl runat="server">
                                </dxe:ContentControl>
                            </ContentCollection>
                        </dxe:TabPage>
                        <dxe:TabPage Name="Other" Visible="false">
                            <TabTemplate><span style="font-size: x-small">Other</span>&nbsp;<span style="color: Red;">*</span> </TabTemplate>
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
                        <dxe:TabPage Name="Deposit" Text="Deposit" Visible="false">
                            <ContentCollection>
                                <dxe:ContentControl runat="server">
                                </dxe:ContentControl>
                            </ContentCollection>
                        </dxe:TabPage>
                        <dxe:TabPage Name="Remarks" Text="UDF" Visible="false">
                            <ContentCollection>
                                <dxe:ContentControl runat="server">
                                </dxe:ContentControl>
                            </ContentCollection>
                        </dxe:TabPage>
                        <dxe:TabPage Name="Education" Text="Education" Visible="false">
                            <ContentCollection>
                                <dxe:ContentControl runat="server">
                                </dxe:ContentControl>
                            </ContentCollection>
                        </dxe:TabPage>
                        <dxe:TabPage Name="Trad. Prof." Text="Trad.Prof" Visible="false">
                            <%--<TabTemplate ><span style="font-size:x-small">Trad.Prof</span>&nbsp;<span style="color:Red;">*</span> </TabTemplate>--%>
                            <ContentCollection>
                                <dxe:ContentControl runat="server">
                                </dxe:ContentControl>
                            </ContentCollection>
                        </dxe:TabPage>



                        <dxe:TabPage Name="Family Members" Text="Family">
                            <ContentCollection>
                                <dxe:ContentControl runat="server">
                                        <div style="float:left;"> 
                                            <% if(rights.CanAdd)
                                               { %>
                                             <a href="javascript:void(0);" onclick="gridFamily.AddNewRow();" class="btn btn-primary"><span>Add New</span> </a>
                                            <% } %>
                                        </div>
                                    <dxe:ASPxGridView ID="FamilyMemberGrid" DataSourceID="FamilyMemberData" ClientInstanceName="gridFamily"
                                        KeyFieldName="Id" runat="server" AutoGenerateColumns="False" Width="100%" Font-Size="12px" OnCommandButtonInitialize="FamilyMemberGrid_CommandButtonInitialize" OnStartRowEditing="FamilyMemberGrid_StartRowEditing">
                                        <Columns>
                                            <dxe:GridViewDataTextColumn FieldName="Relation" Caption="Relationship" VisibleIndex="0"
                                                Width="20%">
                                                <EditFormSettings Caption="RelationShip" Visible="False" />
                                                <CellStyle CssClass="gridcellleft">
                                                </CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="Name" Caption="Name" VisibleIndex="1" Width="20%">
                                                <EditFormSettings Caption="Name" Visible="True" />
                                                <CellStyle CssClass="gridcellleft">
                                                </CellStyle>
                                                <PropertiesTextEdit>
                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" SetFocusOnError="True"  >
                                                        <RequiredField ErrorText="Mandatory " IsRequired="True"  />
                                                    </ValidationSettings>
                                                </PropertiesTextEdit>
                                                <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                </EditFormCaptionStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataDateColumn Caption="Date of Birth" FieldName="DOB" VisibleIndex="2"
                                                Width="20%">
                                                <PropertiesDateEdit DisplayFormatString="{0:dd MMM yyyy}" EditFormatString="dd-MM-yyyy" EditFormat="Custom" UseMaskBehavior="true">
                                                </PropertiesDateEdit>
                                                <CellStyle CssClass="gridcellleft">
                                                </CellStyle>
                                                <EditFormSettings Visible="True" />
                                                <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                </EditFormCaptionStyle>
                                            </dxe:GridViewDataDateColumn>
                                            <dxe:GridViewDataComboBoxColumn Caption="Relationship" FieldName="RID" Visible="False"
                                                VisibleIndex="0">
                                                <PropertiesComboBox DataSourceID="SelectRelation" TextField="fam_familyRelationship"
                                                    ValueField="fam_id" ValueType="System.String">
                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" SetFocusOnError="True" >
                                                        <RequiredField ErrorText="Mandatory" IsRequired="True"  />
                                                    </ValidationSettings>
                                                </PropertiesComboBox>
                                                <EditFormSettings Visible="True" />
                                                <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                </EditFormCaptionStyle>
                                            </dxe:GridViewDataComboBoxColumn>
                                            <dxe:GridViewDataComboBoxColumn Caption="Blood Group" FieldName="BloodGroup" VisibleIndex="0" >
                                                <PropertiesComboBox ValueType="System.String">
                                                    <Items>
                                                        <dxe:ListEditItem Text="A+" Value="A+"></dxe:ListEditItem>
                                                        <dxe:ListEditItem Text="A-" Value="A-"></dxe:ListEditItem>
                                                        <dxe:ListEditItem Text="B+" Value="B+"></dxe:ListEditItem>
                                                        <dxe:ListEditItem Text="B-" Value="B-"></dxe:ListEditItem>
                                                        <dxe:ListEditItem Text="AB+" Value="AB+"></dxe:ListEditItem>
                                                        <dxe:ListEditItem Text="AB-" Value="AB-"></dxe:ListEditItem>
                                                        <dxe:ListEditItem Text="O" Value="O"></dxe:ListEditItem>
                                                        <dxe:ListEditItem Text="O+" Value="O+"></dxe:ListEditItem>
                                                    </Items>
                                                </PropertiesComboBox>
                                                <EditFormSettings Visible="True" />
                                                <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False" >
                                                </EditFormCaptionStyle>
                                            </dxe:GridViewDataComboBoxColumn>
                                            <dxe:GridViewDataTextColumn FieldName="User1" Caption="Create User" VisibleIndex="3"
                                                Visible="False" Width="20%">
                                                <EditFormSettings Caption="Category" Visible="False" />
                                                <CellStyle CssClass="gridcellleft">
                                                </CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewCommandColumn VisibleIndex="4" ShowDeleteButton="true" ShowEditButton="true" Width="10%" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">

                                                <HeaderTemplate>Actions
                                                   <%-- <a href="javascript:void(0);" onclick="gridFamily.AddNewRow();"><span>Add New</span> </a>--%>
                                                </HeaderTemplate>
                                            </dxe:GridViewCommandColumn>
                                        </Columns>
                                       <SettingsCommandButton>
                                                    <EditButton Image-Url="../../../assests/images/Edit.png" ButtonType="Image" Image-AlternateText="Edit" Styles-Style-CssClass="pad">
                                                    </EditButton>
                                                    <DeleteButton Image-Url="../../../assests/images/Delete.png" ButtonType="Image" Image-AlternateText="Delete">
                                                    </DeleteButton>
                                                    <UpdateButton Text="Save" ButtonType="Button" Styles-Style-CssClass="btn btn-primary"></UpdateButton>
                                                    <CancelButton Text="Cancel" ButtonType="Button" Styles-Style-CssClass="btn btn-danger"></CancelButton>
                                                </SettingsCommandButton>

                                        <Settings ShowGroupPanel="True" ShowStatusBar="Visible" />
                                        <SettingsEditing Mode="PopupEditForm"  PopupEditFormHorizontalAlign="Center"
                                            PopupEditFormModal="True" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="500px"
                                            EditFormColumnCount="1" />
                                        <Styles>
                                            <LoadingPanel ImageSpacing="10px">
                                            </LoadingPanel>
                                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                            </Header>
                                        </Styles>
                                        <SettingsText PopupEditFormCaption="Add Family Relationship" ConfirmDelete="Confirm delete?" />
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
                                                        <td style="width: 5%"></td>
                                                        <td style="width: 90%">
                                                            <controls>
                                                   <dxe:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormEditors" ColumnID="" ID="Editors">
                                                   </dxe:ASPxGridViewTemplateReplacement>                                                           
                                                 </controls>
                                                            <div style="padding: 2px 2px 2px 97px">
                                                                <dxe:ASPxGridViewTemplateReplacement ID="UpdateButton" ReplacementType="EditFormUpdateButton"
                                                                    runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                                <dxe:ASPxGridViewTemplateReplacement ID="CancelButton" ReplacementType="EditFormCancelButton"
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
                                                        <td align="right">
                                                            <table width="200">
                                                                <tr>
                                                                    <td>
                                                                        <dxe:ASPxButton ID="ASPxButton1" runat="server" Text="ADD" ToolTip="Add New Data"
                                                                            Height="18px" Width="88px" AutoPostBack="False" Font-Size="12px">
                                                                            <ClientSideEvents Click="function(s, e) {gridFamily.AddNewRow();}" />
                                                                        </dxe:ASPxButton>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </TitlePanel>
                                        </Templates>
                                    </dxe:ASPxGridView>
                                </dxe:ContentControl>
                            </ContentCollection>
                        </dxe:TabPage>


                        <dxe:TabPage Name="Subscription" Text="Subscription" Visible="false">
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
        </tr>
    </table>
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

