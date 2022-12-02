<%@ Page Title="Family" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true"
    Inherits="ERP.OMS.Management.Master.management_master_Employee_FamilyMembers" Codebehind="Employee_FamilyMembers.aspx.cs" %>

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
    <%--<title>Family Members</title>--%>
    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="javascript" type="text/javascript">
    function disp_prompt(name)
    {
        if ( name == "tab0")
        {
        //alert(name);
            document.location.href = "Employee_general.aspx?id=" + $("#hdKeyVal").val();
        }
        if ( name == "tab1")
        {
        //alert(name);
        document.location.href="Employee_Correspondence.aspx"; 
        }
        else if ( name == "tab2")
        {
        //alert(name);
            document.location.href = "Employee_Education.aspx";
        }
        else if ( name == "tab3")
        {
            document.location.href = "Employee_Employee.aspx";
        //alert(name);
        //document.location.href="Employee_DPDetails.aspx"; 
        }
        else if ( name == "tab4")
        {
        //alert(name);
            document.location.href = "Employee_Document.aspx";
        }
        else if ( name == "tab5")
        {
        //alert(name);
        //document.location.href="Employee_FamilyMembers.aspx"; 
        }
        else if ( name == "tab6")
        {
        //alert(name);
            document.location.href = "Employee_GroupMember.aspx";
        }
        else if ( name == "tab7")
        {
        //alert(name);
            document.location.href = "Employee_EmployeeCTC.aspx";
        }
        else if ( name == "tab8")
        {
        //alert(name);
            document.location.href = "Employee_BankDetails.aspx";
        }
        else if ( name == "tab9")
        {
        //alert(name);
            document.location.href = "Employee_Remarks.aspx";
        }
        else if ( name == "tab10")
        {
        //alert(name);
       //// document.location.href="Employee_Remarks.aspx"; 
        }
        else if ( name == "tab11")
        {
        //alert(name);
      //  document.location.href="Employee_Education.aspx"; 
        }
        else if ( name == "tab12")
        {
        //alert(name);
        //document.location.href="Employee_Subscription.aspx";
        }


        else if (name == "tab13") {
            //alert(name);
            var keyValue = $("#hdnlanguagespeak").val();
            document.location.href = 'frmLanguageProfi.aspx?id=' + keyValue + '&status=speak';
            //   document.location.href="Employee_Subscription.aspx"; 
        }
        else if (name == "tab14") {
            //alert(name);
            document.location.href = "StatutoryDocumnentDtls.aspx";
        }
        else if (name == "tab15") {
            //alert(name);
            document.location.href = "PFESIDtls.aspx";
        }
        else if (name == "tab16") {
            //alert(name);
            document.location.href = "OtherDetails.aspx";
        }
        else if (name == "tab17") {
            //alert(name);
            document.location.href = "Resignation.aspx";
        }
    }
    </script>
    <style>
        .dxflHALSys.dxflVATSys {
            width:120px !important;
        }
    /*.dxeErrorCell_PlasticBlue.dxeErrorFrame_PlasticBlue {
         display:none !important;
    }*/
    #FamilyMemberGrid_DXPEForm_efnew_DXEFL_DXEditor4_EC , #FamilyMemberGrid_DXPEForm_efnew_DXEFL_DXEditor1_EC {
        position:absolute;
    }
    </style>
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Employee Family Members</h3>
            <div class="crossBtn"><a href="employee.aspx"><i class="fa fa-times"></i></a></div>
        </div>
    </div>
    
        <div class="panel-title">
            <table class="TableMain100">
                <tr>
                    <td style="text-align: center">
                        <asp:Label ID="lblHeader" runat="server" Font-Bold="True" Font-Size="15px" ForeColor="Navy"
                            Width="819px" Height="18px"></asp:Label></td>
                </tr>
                <tr>
                    <td>
                        <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="5" ClientInstanceName="page">
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
                                        </dxe:ContentControl>
                                    </ContentCollection>
                                </dxe:TabPage>
                                                                <dxe:TabPage Name="Education" Text="Education">
                                    <ContentCollection>
                                        <dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
                                    </ContentCollection>
                                </dxe:TabPage>
                                 <dxe:TabPage Name="Employee" Text="Employment">
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
                                <dxe:TabPage Name="Family Members" Text="Family">
                                    <ContentCollection>
                                        <dxe:ContentControl runat="server">
                                             <div style="float:left">
                                                 <% if (rights.CanAdd)
                                                               { %>
                                                            <a href="javascript:void(0);" onclick="gridFamily.AddNewRow();" class="btn btn-primary"><span >Add New</span> </a> <%} %>
                                                   </div>
                                            <dxe:ASPxGridView ID="FamilyMemberGrid" DataSourceID="FamilyMemberData" ClientInstanceName="gridFamily" OnCommandButtonInitialize="FamilyMemberGrid_CommandButtonInitialize"
                                                KeyFieldName="Id" runat="server" AutoGenerateColumns="False" Width="100%" Font-Size="12px" OnStartRowEditing="FamilyMemberGrid_StartRowEditing">
                                                <Columns>
                                                    <dxe:GridViewDataTextColumn FieldName="Relation" Caption="Relationship" VisibleIndex="0"
                                                        Width="20%">
                                                        <EditFormSettings Caption="RelationShip" Visible="False" />
                                                        <CellStyle CssClass="gridcellleft">
                                                        </CellStyle>
                                                         <PropertiesTextEdit MaxLength="50">
                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" SetFocusOnError="True">
                                                                <RequiredField ErrorText="Mandatory" IsRequired="True" />
                                                            </ValidationSettings>
                                                        </PropertiesTextEdit>
                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                        </EditFormCaptionStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="Name" Caption="Name" VisibleIndex="3" Width="20%">
                                                        <EditFormSettings Caption="Name" Visible="True"/>
                                                        <CellStyle CssClass="gridcellleft">
                                                        </CellStyle>
                                                        <PropertiesTextEdit MaxLength="50">
                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" SetFocusOnError="True">
                                                                <RequiredField ErrorText="Mandatory" IsRequired="True" />
                                                            </ValidationSettings>
                                                        </PropertiesTextEdit>
                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                        </EditFormCaptionStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataDateColumn Caption="Date of Birth" FieldName="DOB" VisibleIndex="4"
                                                        Width="20%">
                                                        <PropertiesDateEdit DisplayFormatString="dd-MMMM-yyyy" EditFormat="Custom" UseMaskBehavior="True"
                                                            EditFormatString="dd-MM-yyyy">
                                                        </PropertiesDateEdit>
                                                        <CellStyle CssClass="gridcellleft">
                                                        </CellStyle>
                                                        <EditFormSettings Visible="True" />
                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="middle" Wrap="False">
                                                        </EditFormCaptionStyle>
                                                    </dxe:GridViewDataDateColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="BloodGroup" Caption="Blood Group" VisibleIndex="5"
                                                        Width="20%">
                                                        <EditFormSettings Caption="Category" Visible="False" />
                                                        <CellStyle CssClass="gridcellleft">
                                                        </CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataComboBoxColumn Caption="Relationship" FieldName="RID" Visible="False"
                                                        VisibleIndex="1">
                                                        <PropertiesComboBox DataSourceID="SelectRelation" TextField="fam_familyRelationship"
                                                            ValueField="fam_id" ValueType="System.String">
                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" SetFocusOnError="True">
                                                                <RequiredField ErrorText="Mandatory" IsRequired="True" />
                                                            </ValidationSettings>
                                                        </PropertiesComboBox>
                                                        <EditFormSettings Visible="True" />
                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                        </EditFormCaptionStyle>
                                                    </dxe:GridViewDataComboBoxColumn>
                                                    <dxe:GridViewDataComboBoxColumn Caption="Blood Group" FieldName="BloodGroup" Visible="False"
                                                        VisibleIndex="2" Width="300px">
                                                        <PropertiesComboBox ValueType="System.String">
                                                            <Items>
                                                                <dxe:ListEditItem Text="N/A" Value="N/A">
                                                                </dxe:ListEditItem>
                                                                <dxe:ListEditItem Text="A+" Value="A+">
                                                                </dxe:ListEditItem>
                                                                <dxe:ListEditItem Text="A-" Value="-">
                                                                </dxe:ListEditItem>
                                                                <dxe:ListEditItem Text="B+" Value="B+">
                                                                </dxe:ListEditItem>
                                                                <dxe:ListEditItem Text="B-" Value="B-">
                                                                </dxe:ListEditItem>
                                                                <dxe:ListEditItem Text="AB+" Value="AB+">
                                                                </dxe:ListEditItem>
                                                                <dxe:ListEditItem Text="AB-" Value="AB-">
                                                                </dxe:ListEditItem>
                                                                <dxe:ListEditItem Text="O" Value="O">
                                                                </dxe:ListEditItem>
                                                                <dxe:ListEditItem Text="O+" Value="O+">
                                                                </dxe:ListEditItem>
                                                            </Items>
                                                        </PropertiesComboBox>
                                                        <EditFormSettings Visible="True" />
                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                        </EditFormCaptionStyle>
                                                    </dxe:GridViewDataComboBoxColumn>
                                                    <dxe:GridViewCommandColumn VisibleIndex="6" ShowDeleteButton="true" ShowEditButton="true" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" Width="10%">
                                                      <%--  <DeleteButton Visible="True">
                                                        </DeleteButton>
                                                        <EditButton Visible="True">
                                                        </EditButton>--%>
<HeaderStyle HorizontalAlign="Center"></HeaderStyle>

<CellStyle HorizontalAlign="Center"></CellStyle>
                                                        <HeaderTemplate>Actions
                                                           <%-- <%if (Session["PageAccess"].ToString().Trim() == "All" || Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "DelAdd")
                                                                          { %>
                                                            <a href="javascript:void(0);" onclick="gridFamily.AddNewRow();"><span >Add New</span> </a>
                                                            <%} %>--%>
                                                        </HeaderTemplate>
                                                    </dxe:GridViewCommandColumn>
                                                </Columns>
                                                  <SettingsCommandButton>
                                                                    <EditButton Image-Url="../../../assests/images/Edit.png" ButtonType="Image" Image-AlternateText="Edit" Styles-Style-CssClass="pad">
                                                                        <Image AlternateText="Edit" Url="../../../assests/images/Edit.png"></Image>

                                                                        <Styles>
                                                                            <Style CssClass="pad"></Style>
                                                                        </Styles>
                                                                    </EditButton>
                                                                    <DeleteButton Image-Url="../../../assests/images/Delete.png" ButtonType="Image" Image-AlternateText="Delete">
                                                                        <Image AlternateText="Delete" Url="../../../assests/images/Delete.png"></Image>
                                                                    </DeleteButton>
                                                                    <UpdateButton Text="Save" ButtonType="Button" Styles-Style-CssClass="btn btn-primary">
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
                                              <Settings ShowGroupPanel="True" ShowStatusBar="Visible" />
                                                <SettingsEditing Mode="PopupEditForm"   PopupEditFormHorizontalAlign="WindowCenter" 
                                                    PopupEditFormModal="True" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="500px"
                                                    EditFormColumnCount="1" />
                                                <Styles>
                                                    <LoadingPanel ImageSpacing="10px">
                                                    </LoadingPanel>
                                                    <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                    </Header>
                                                </Styles>
                                                <SettingsText PopupEditFormCaption="Add Family Relationship" ConfirmDelete="Confirm delete?" />
                                                <SettingsPager NumericButtonCount="20" PageSize="20">
                                                </SettingsPager>
                                                <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True" />
                                                <Templates>
                                                    <EditForm>
                                                        <table style="width: 90%">
                                                            <tr>
                                                                
                                                                <td>
                                                                    <controls>
                                                   <dxe:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormEditors" ColumnID="" ID="Editors">
                                                   </dxe:ASPxGridViewTemplateReplacement>                                                           
                                                 </controls>
                                                                    <div style="text-align: left; padding: 2px 2px 2px 139px">
                                                                        <dxe:ASPxGridViewTemplateReplacement ID="UpdateButton" ReplacementType="EditFormUpdateButton"
                                                                            runat="server">
                                                                        </dxe:ASPxGridViewTemplateReplacement>
                                                                        <dxe:ASPxGridViewTemplateReplacement ID="CancelButton" ReplacementType="EditFormCancelButton"
                                                                            runat="server">
                                                                        </dxe:ASPxGridViewTemplateReplacement>
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

                                 <dxe:TabPage Name="Group Member" Text="Group">
                                    <ContentCollection>
                                        <dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
                                    </ContentCollection>
                                </dxe:TabPage>
                               
                                <dxe:TabPage Name="Employee CTC" Text="CTC">
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
                                 <dxe:TabPage Name="Remarks" Text="UDF" Visible="false">
                                    <ContentCollection>
                                        <dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
                                    </ContentCollection>
                                </dxe:TabPage>
                                <dxe:TabPage Name="DP Details" Text="DP" Visible="false">
                                    <ContentCollection>
                                        <dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
                                    </ContentCollection>
                                </dxe:TabPage>
                               
                                <dxe:TabPage Name="Registration" Text="Registration" Visible="false">
                                    <ContentCollection>
                                        <dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
                                    </ContentCollection>
                                </dxe:TabPage>
                               
                               

                                     <dxe:TabPage Name="Subscription" Text="Subscriptions" Visible="false">
                                    <ContentCollection>
                                        <dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
                                    </ContentCollection>
                                </dxe:TabPage>
                                  <dxe:TabPage Name="Language" Text="Language" >
                                    <ContentCollection>
                                        <dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
                                    </ContentCollection>
                                </dxe:TabPage>
                               <dxe:TabPage Name="StautoryDocumentDetails" Text="Statutory">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                                <dxe:TabPage Name="PFESI" Text="PF/ESI">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                                 <dxe:TabPage Name="othrdtls" Text="Other Details">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                                <dxe:TabPage Name="RESIGNATION" Text="Resignation">
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
                                                var Tab14 = page.GetTab(14);
                                                var Tab13 = page.GetTab(13);    
                                                var Tab15 = page.GetTab(15);
                                                var Tab16 = page.GetTab(16);
                                                var Tab17 = page.GetTab(17);
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
                                else if(activeTab == Tab14)
	                                            {
	                                                disp_prompt('tab14');
	                                            }
                                 else if(activeTab == Tab15)
	                                            {
	                                                disp_prompt('tab15');
	                                            }
                                 else if(activeTab == Tab16)
	                                            {
	                                                disp_prompt('tab16');
	                                            }
                                else if(activeTab == Tab17)
	                                            {
	                                                disp_prompt('tab17');
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
        </div>
        <asp:SqlDataSource ID="FamilyMemberData" runat="server" 
            InsertCommand="insert into tbl_master_contactfamilyrelationship(femrel_relationId,femrel_memberName,femrel_DOB,femrel_bloodgroup,femrel_cntId,femrel_contacttype,CreateDate,CreateUser) values(@RID,@Name,@DOB,@BloodGroup,@insuId,'contact',getdate(),@User1)"
            SelectCommand="select tbl_master_contactFamilyRelationship.femrel_id AS Id, 
                        tbl_master_familyRelationship.fam_familyRelationship AS Relation, tbl_master_contactFamilyRelationship.femrel_memberName AS Name, 
                        tbl_master_contactFamilyRelationship.femrel_bloodGroup AS BloodGroup, CAST(tbl_master_contactFamilyRelationship.femrel_DOB AS DATETIME) AS DOB,
                        tbl_master_contactFamilyRelationship.femrel_relationId as RID,
                        tbl_master_contactFamilyRelationship.CreateUser as User1 
                        from tbl_master_contactFamilyRelationship INNER JOIN tbl_master_familyRelationship 
                        ON tbl_master_contactFamilyRelationship.femrel_relationId = tbl_master_familyRelationship.fam_id 
                        where tbl_master_contactFamilyRelationship.femrel_cntId =@insuId 
                        AND tbl_master_contactFamilyRelationship.femrel_contactType = 'contact'"
            DeleteCommand="INSERT INTO tbl_master_contactFamilyRelationship_Log(femrel_id, femrel_cntId, femrel_contactType, femrel_memberName, femrel_relationId, femrel_DOB, femrel_bloodGroup, CreateDate, CreateUser, LastModifyDate, LastModifyUser, LogModifyDate, LogModifyUser, LogStatus) SELECT *,getdate(),@User1,'D' FROM tbl_master_contactFamilyRelationship WHERE  femrel_id=@Id delete from tbl_master_contactfamilyrelationship where femrel_id=@Id"
            UpdateCommand="INSERT INTO tbl_master_contactFamilyRelationship_Log(femrel_id, femrel_cntId, femrel_contactType, femrel_memberName, femrel_relationId, femrel_DOB, femrel_bloodGroup, CreateDate, CreateUser, LastModifyDate, LastModifyUser, LogModifyDate, LogModifyUser, LogStatus) SELECT *,getdate(),@User1,'M' FROM tbl_master_contactFamilyRelationship WHERE  femrel_id=@Id update tbl_master_contactfamilyrelationship set femrel_relationId=@RID,femrel_memberName=@Name,femrel_DOB=@DOB,femrel_bloodgroup=@BloodGroup,LastModifyDate=getdate(),LastModifyUser=@User1 where femrel_id=@Id">
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
                <asp:SessionParameter Name="User1" SessionField="userid" Type="Decimal" />
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
            SelectCommand="select fam_familyRelationship,fam_id from tbl_master_familyrelationship order by fam_familyRelationship">
        </asp:SqlDataSource>
    <asp:HiddenField ID="hdKeyVal" runat="server" />
   </asp:Content>

