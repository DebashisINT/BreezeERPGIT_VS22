<%@ Page Title="Account Group" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.Master.management_master_AccountGroup" CodeBehind="AccountGroup.aspx.cs" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .dxgvEditFormCaption {
            text-align: right !important;
        }
    
        .padding {
            width:100%;
        }
        .padding>tbody>tr>td {
            padding:5px 0px;
        }
        .cnt {
                width: 70%;
                margin: 0 auto;
        }
    </style>
    <script src="Js/AccountGroup.js"></script>
    <script language="javascript" type="text/javascript">
        function UniqueCodeCheck() {

            var uniqueid = '0';
            var id = '<%= Convert.ToString(Session["id"]) %>';

            var uniqueCode = grid.GetEditor('AccountGroupCode').GetValue();

            if ((id != null) && (id != '')) {
                uniqueid = id;
                '<%=Session["id"]=null %>'
            }
            var CheckUniqueCodee = false;
            $.ajax({
                type: "POST",
                url: "AccountGroup.aspx/CheckUniqueCode",
                data: JSON.stringify({ uniqueCode: uniqueCode, uniqueid: uniqueid }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    CheckUniqueCodee = msg.d;
                    if (CheckUniqueCodee == true) {
                        jAlert('Please enter unique short name');
                        grid.GetEditor('AccountGroupCode').SetValue('');
                        grid.GetEditor('AccountGroupCode').Focus();
                    }
                }
            });
        }
        function UniqueCodeCheckPopUp() {

            var uniqueid = '0';
            var id = '<%= Convert.ToString(Session["id"]) %>';

    var uniqueCode = ctxtAccountShortName.GetValue();

    if ((id != null) && (id != '')) {
        uniqueid = id;
        '<%=Session["id"]=null %>'
    }
    var CheckUniqueCodee = false;
    $.ajax({
        type: "POST",
        url: "AccountGroup.aspx/CheckUniqueCode",
        data: JSON.stringify({ uniqueCode: uniqueCode, uniqueid: uniqueid }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            CheckUniqueCodee = msg.d;
            if (CheckUniqueCodee == true) {
                jAlert('Please enter unique short name');
                ctxtAccountShortName.SetValue('');
            }
        }
    });
        }
        function UniqueNameCheck() {

            var uniqueid = '0';
            var id = '<%= Convert.ToString(Session["id"]) %>';

    var uniqueCode = ctxtAccountName.GetValue();

    if ((id != null) && (id != '')) {
        uniqueid = id;
        '<%=Session["id"]=null %>'
    }
    var CheckUniqueCodee = false;
    $.ajax({
        type: "POST",
        url: "AccountGroup.aspx/CheckUniqueName",
        data: JSON.stringify({ uniqueName: uniqueCode, uniqueid: uniqueid }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            CheckUniqueCodee = msg.d;
            if (CheckUniqueCodee == true) {
                jAlert('Please enter unique name');
                grid.GetEditor('AccountGroupName').SetValue('');
                grid.GetEditor('AccountGroupName').Focus();
            }
        }
    });
}


function UniqueNameCheckpopUp() {

    var uniqueid = '0';
    var id = '<%= Convert.ToString(Session["id"]) %>';

    var uniqueCode = ctxtAccountName.GetValue();

    if ((id != null) && (id != '')) {
        uniqueid = id;
        '<%=Session["id"]=null %>'
    }
    var CheckUniqueCodee = false;
    $.ajax({
        type: "POST",
        url: "AccountGroup.aspx/CheckUniqueName",
        data: JSON.stringify({ uniqueName: uniqueCode, uniqueid: uniqueid }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            CheckUniqueCodee = msg.d;
            if (CheckUniqueCodee == true) {
                jAlert('Please enter unique name');
                ctxtAccountName.SetValue('');
            }
        }
    });
}
     </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Account Group</h3>
        </div>
    </div>
    <div class="form_main">
        <div class="SearchArea">
            <div class="FilterSide">
                <div style="float: left; padding-right: 5px;">
                    <% if (rights.CanAdd)
                       { %>
                    <a href="javascript:void(0);" onclick="AddNewRow()" class="btn btn-success btn-radius"><span class="btn-icon"><i class="fa fa-plus" ></i></span><span>Add New</span> </a>

                    <% } %>
                </div>

                <div class="pull-left">
                    <% if (rights.CanExport)
                       { %>
                    <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-radius" OnChange="if(!AvailableExportOption()){return false;}" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Value="0">Export to</asp:ListItem>
                        <asp:ListItem Value="1">PDF</asp:ListItem>
                        <asp:ListItem Value="2">XLS</asp:ListItem>
                        <asp:ListItem Value="3">RTF</asp:ListItem>
                        <asp:ListItem Value="4">CSV</asp:ListItem>

                    </asp:DropDownList>
                    <% } %>
                </div>
            </div>

        </div>
        <div class="clear"></div>
        <div class="relative clearfix">
        <%--#a20102016 - 0011279--%>
        <dxe:ASPxGridView ID="AccountGroup" runat="server" ClientInstanceName="grid" AutoGenerateColumns="False"
            DataSourceID="SqlDsAccountGroup" Width="100%" KeyFieldName="AccountGroupID" OnHtmlEditFormCreated="AccountGroup_HtmlEditFormCreated"
            OnCellEditorInitialize="AccountGroup_CellEditorInitialize" OnRowValidating="AccountGroup_OnRowValidating" OnInitNewRow="AccountGroup_InitNewRow"
            OnRowUpdating="AccountGroup_OnRowUpdating" OnCustomCallback="AccountGroup_CustomCallback" OnStartRowEditing="AccountGroup_StartRowEditing" OnParseValue="AccountGroup_ParseValue"
            OnCustomJSProperties="AccountGroup_CustomJSProperties" OnRowDeleting="AccountGroup_RowDeleting" OnCommandButtonInitialize="AccountGroup_CommandButtonInitialize" Settings-VerticalScrollableHeight="280" Settings-VerticalScrollBarMode="Auto">
            <ClientSideEvents EndCallback="function(s, e) {
	  EndCall(s.cpInsertError);
}" />

            <%--            <Templates>
<%--                <EditForm>
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 10%"></td>
                            <td style="width: 90%">
                                <controls>
                                <dxe:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormEditors"  ID="Editors">
                                </dxe:ASPxGridViewTemplateReplacement>                                                           
                            </controls>
                                <div style="text-align: left; padding: 2px 2px 2px 102px; width: 90%">
                                    <dxe:ASPxGridViewTemplateReplacement ID="UpdateButton" ReplacementType="EditFormUpdateButton" class="btn btn-primary"
                                        runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                    <dxe:ASPxGridViewTemplateReplacement ID="CancelButton" ReplacementType="EditFormCancelButton"
                                        runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                </div>
                            </td>
                            <%--<td style="width: 25%"></td>--
                        </tr>
                    </table>
                </EditForm>--
            </Templates>--%>
            <SettingsBehavior ConfirmDelete="True" AllowFocusedRow="true" />
            <SettingsPager NumericButtonCount="20" PageSize="20" AlwaysShowPager="True" ShowSeparators="True">
                <FirstPageButton Visible="True">
                </FirstPageButton>
                <LastPageButton Visible="True">
                </LastPageButton>
            </SettingsPager>
            <%-- <Styles>
                <Header CssClass="gridheader" SortingImageSpacing="5px" ImageSpacing="5px">
                </Header>
                <FocusedRow CssClass="gridselectrow">
                </FocusedRow>
                <LoadingPanel ImageSpacing="10px">
                </LoadingPanel>
                <FocusedGroupRow CssClass="gridselectrow">
                </FocusedGroupRow>
            </Styles>--%>
            <%--    <SettingsPager NumericButtonCount="20" PageSize="20" AlwaysShowPager="True" ShowSeparators="True">
                <FirstPageButton Visible="True">
                </FirstPageButton>
                <LastPageButton Visible="True">
                </LastPageButton>
            </SettingsPager>--%>
            <Columns>
                <dxe:GridViewDataComboBoxColumn Visible="False" FieldName="AccountGroupType" Caption="Account Type"
                    VisibleIndex="1" Width="50px">
                    <PropertiesComboBox ValueType="System.String" Width="200px">
                        <Items>
                            <dxe:ListEditItem Value="Asset" Text="Asset"></dxe:ListEditItem>
                            <dxe:ListEditItem Value="Liability" Text="Liability"></dxe:ListEditItem>
                            <dxe:ListEditItem Value="Income" Text="Income"></dxe:ListEditItem>
                            <dxe:ListEditItem Value="Expense" Text="Expense"></dxe:ListEditItem>
                            <%--#ag18102016 - 0011279--%>
                        </Items>
                        <%--<ClientSideEvents SelectedIndexChanged="function(s,e){AddAccountType()}" />--%>
                    </PropertiesComboBox>
                    <CellStyle CssClass="gridcellleft">
                    </CellStyle>
                    <EditFormSettings Visible="True" VisibleIndex="1"></EditFormSettings>
                </dxe:GridViewDataComboBoxColumn>

                <dxe:GridViewDataComboBoxColumn Visible="False" FieldName="AccountType" Caption="Type"
                    VisibleIndex="1" Width="50px">
                    <PropertiesComboBox Width="200px" ClientInstanceName="cAccountType">
                        <Items>
                        </Items>

                    </PropertiesComboBox>
                    <CellStyle CssClass="gridcellleft">
                    </CellStyle>
                    <EditFormSettings Visible="True" VisibleIndex="1"></EditFormSettings>
                </dxe:GridViewDataComboBoxColumn>

                <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="3" FieldName="AccountGroupSequence"
                    Caption="Sequence">
                    <CellStyle CssClass="gridcellleft">
                    </CellStyle>
                    <PropertiesTextEdit Width="200px" MaxLength="5">

                        <ValidationSettings SetFocusOnError="True" ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right">
                            <RequiredField IsRequired="True" ErrorText="Mandatory"></RequiredField>
                            <%--#ag18102016 - 0011279--%>
                        </ValidationSettings>
                    </PropertiesTextEdit>
                    <EditFormSettings Visible="True" VisibleIndex="2"></EditFormSettings>

                </dxe:GridViewDataTextColumn>


                <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="3" FieldName="AccountGroupSchedule"
                    Caption="Schedule">
                    <CellStyle CssClass="gridcellleft">
                    </CellStyle>
                    <PropertiesTextEdit Width="200px" MaxLength="10">

                        <ValidationSettings SetFocusOnError="True" ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right">
                            <RequiredField IsRequired="True" ErrorText="Mandatory"></RequiredField>
                            <%--#ag18102016 - 0011279--%>
                        </ValidationSettings>
                    </PropertiesTextEdit>
                    <EditFormSettings Visible="True" VisibleIndex="2"></EditFormSettings>

                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="3" FieldName="AccountGroupCode"
                    Caption="Short Name">
                    <CellStyle CssClass="gridcellleft">
                    </CellStyle>
                    <PropertiesTextEdit Width="200px" MaxLength="50">

                        <ValidationSettings SetFocusOnError="True" ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right">
                            <RequiredField IsRequired="True" ErrorText="Mandatory"></RequiredField>
                            <%--#ag18102016 - 0011279--%>
                        </ValidationSettings>
                        <ClientSideEvents TextChanged="function(s,e){UniqueCodeCheck()}" />
                    </PropertiesTextEdit>
                    <EditFormSettings Visible="True" VisibleIndex="2"></EditFormSettings>

                </dxe:GridViewDataTextColumn>






                <dxe:GridViewDataTextColumn VisibleIndex="0" FieldName="AccountGroupName" Caption="Name">
                    <CellStyle CssClass="gridcellleft">
                    </CellStyle>
                    <PropertiesTextEdit Width="200px" MaxLength="50">
                        <ValidationSettings SetFocusOnError="True" ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right">
                            <RequiredField IsRequired="True" ErrorText="Mandatory"></RequiredField>
                            <%--#ag18102016 - 0011279--%>
                        </ValidationSettings>
                        <ClientSideEvents TextChanged="function(s,e){UniqueNameCheck()}" />
                    </PropertiesTextEdit>
                    <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right">
                    </EditFormCaptionStyle>
                    <EditFormSettings Visible="True" VisibleIndex="3"></EditFormSettings>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataComboBoxColumn FieldName="AccountGroupParentID" Caption="Parent ID"
                    VisibleIndex="2">
                    <PropertiesComboBox ValueType="System.Int64" TextField="ParentIDWithName" ValueField="AccountGroupParentID" Width="200px">
                        <%--  <ClientSideEvents SelectedIndexChanged="function(s,e){OnParentChanged(s); }" ></ClientSideEvents>--%>
                    </PropertiesComboBox>
                    <Settings FilterMode="DisplayText"></Settings>
                    <CellStyle CssClass="gridcellleft">
                    </CellStyle>
                    <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right" VerticalAlign="Top">
                    </EditFormCaptionStyle>
                    <EditFormSettings Visible="True" VisibleIndex="4"></EditFormSettings>
                </dxe:GridViewDataComboBoxColumn>
                

                <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="17" Width="1">
                    <DataItemTemplate>
                        <div class='floatedBtnArea'>
                        <% if (rights.CanEdit)
                           { %>
                        <a href="javascript:void(0);" onclick="EditForm('<%# Container.KeyValue %>')" class="" title="" style='<%#Eval("IsSystemGroup")%>'> 
                            <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Edit</span></a>
                        <% } %>

                        <% if (rights.CanDelete )
                           { %>
                        <a href="javascript:void(0);" onclick="OnClickDelete('<%# Container.KeyValue %>')" class="" title="" style='<%#Eval("IsSystemGroup")%>'>
                           <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>Delete</span></a><%} %>
                        </div>
                    </DataItemTemplate>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <CellStyle HorizontalAlign="Center"></CellStyle>
                    <HeaderTemplate><span></span></HeaderTemplate>
                    <EditFormSettings Visible="False"></EditFormSettings>
                </dxe:GridViewDataTextColumn>
            </Columns>
            <ClientSideEvents RowClick="gridRowclick" />
            <SettingsEditing Mode="PopupEditForm" PopupEditFormHorizontalAlign="WindowCenter"
                PopupEditFormModal="True" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="500px"
                EditFormColumnCount="1" />
            <SettingsText PopupEditFormCaption="Add/Modify Account Group" ConfirmDelete="Confirm Delete?" />
            <SettingsSearchPanel Visible="True" />
            <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="True" />
        </dxe:ASPxGridView>
        <asp:SqlDataSource ID="SqlDsAccountGroup" runat="server"
            DeleteCommand="DELETE FROM [Master_AccountGroup] WHERE [AccountGroup_ReferenceID] = @AccountGroupID"
            InsertCommand="InsertInAccountGroup"
            InsertCommandType="StoredProcedure" SelectCommand="SELECT a.[AccountGroup_ReferenceID] as AccountGroupID,a.[AccountGroup_Type] as AccountGroupType ,a.[AccountGroup_Code] as AccountGroupCode ,a.[AccountGroup_Name] as AccountGroupName,a.[AccountGroup_ParentGroupID]as AccountGroupParentID1,b.AccountGroup_Name as AccountGroupParentID,isnull(A.AccountType,'None') as AccountType,isnull(A.AccountGroupSequence,0) as AccountGroupSequence,isnull(A.AccountGroupSchedule,'') as AccountGroupSchedule,
            Case when a.IsSystemGroup=1 then 'visibility: hidden;' else 'visibility: visible;' end IsSystemGroup  FROM [Master_AccountGroup] AS a LEFT OUTER JOIN [Master_AccountGroup] AS B on a.AccountGroup_ParentGroupID=b.AccountGroup_ReferenceID order by AccountGroupID Desc"
            UpdateCommand="UPDATE [Master_AccountGroup] SET [AccountGroup_Type] = @AccountGroupType, AccountGroup_Code = @AccountGroupCode, AccountGroup_Name = @AccountGroupName, [AccountGroup_ParentGroupID] = @AccountGroupParentID,AccountType=@AccountType,AccountGroupSequence=cast(@AccountGroupSequence as bigint),AccountGroupSchedule=@AccountGroupSchedule WHERE [AccountGroup_ReferenceID] = @AccountGroupID">
            <DeleteParameters>
                <asp:Parameter Name="AccountGroupID" Type="int32" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="AccountGroupID" Type="int32" />
                <asp:Parameter Name="AccountGroupType" Type="String" />
                <asp:Parameter Name="AccountGroupCode" Type="String" />
                <asp:Parameter Name="AccountGroupName" Type="String" />
                <asp:Parameter Name="AccountGroupParentID" Type="Int32" />
                <asp:Parameter Name="AccountType" Type="String" />
                <asp:Parameter Name="AccountGroupSequence" Type="String" />
                <asp:Parameter Name="AccountGroupSchedule" Type="String" />
                <asp:Parameter Name="CreateUser" Type="Int32" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="AccountGroupType" Type="String" />
                <asp:Parameter Name="AccountGroupCode" Type="String" />
                <asp:Parameter Name="AccountGroupName" Type="String" />
                <asp:Parameter Name="AccountGroupParentID" Type="Int32" />
                <asp:Parameter Name="AccountType" Type="String" DefaultValue="" />
                <asp:Parameter Name="AccountGroupSequence" Type="String" DefaultValue="" />
                <asp:Parameter Name="AccountGroupSchedule" Type="String" DefaultValue="" />
                <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="int32" />
            </InsertParameters>
        </asp:SqlDataSource>
        </div>


    </div>
    <div class="PopUpArea">
        <dxe:ASPxPopupControl ID="AddEditPopUp" runat="server" ClientInstanceName="cAddEditPopUp"
            Width="600px" HeaderText="Add/Edit Account Group" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">

            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <dxe:ASPxCallbackPanel runat="server" ID="SelectPanel" ClientInstanceName="cSelectPanel" OnCallback="SelectPanel_Callback" CssClass="cnt">
                        <ClientSideEvents EndCallback="function(s, e) {SelectPanel_EndCallBack();}" />
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <table class="padding">
                                    <tr>
                                        <td><div>Group Type</div></td>
                                        <td>
                                            <div>
                                                <dxe:ASPxComboBox ID="cmbGroup" ClientInstanceName="ccmbGroup" runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True">
                                                    <ClientSideEvents SelectedIndexChanged="function(s,e){AddAccountType()}" />
                                                    <Items>
                                                        <dxe:ListEditItem Value="Asset" Text="Asset"></dxe:ListEditItem>
                                                        <dxe:ListEditItem Value="Liability" Text="Liability"></dxe:ListEditItem>
                                                        <dxe:ListEditItem Value="Income" Text="Income"></dxe:ListEditItem>
                                                        <dxe:ListEditItem Value="Expense" Text="Expense"></dxe:ListEditItem>
                                                        <%--#ag18102016 - 0011279--%>
                                                    </Items>

                                                </dxe:ASPxComboBox>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr style="display:none">
                                        <td><div>Group Type</div></td>
                                        <td>
                                            <div>
                                                <dxe:ASPxComboBox ID="cmbGroupType" ClientInstanceName="ccmbGroupType" runat="server" Width="100%" OnCallback="cmbGroupType_Callback">
                                                    <ClientSideEvents SelectedIndexChanged="TypeChange" />

                                                    <ValidationSettings RequiredField-IsRequired="true" Display="None"></ValidationSettings>
                                                </dxe:ASPxComboBox>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><div>Group Sequence</div></td>
                                        <td>
                                            <div>
                                                <dxe:ASPxTextBox ID="txtGroupSequence" ClientInstanceName="ctxtGroupSequence" runat="server" ValueType="System.Int32" Width="100%">
                                                     <MaskSettings Mask="&lt;0..999999999&gt;" AllowMouseWheel="false" />
                                                    <ValidationSettings RequiredField-IsRequired="true" Display="None"></ValidationSettings>
                                                </dxe:ASPxTextBox>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><div>Schedule No.</div></td>
                                        <td>
                                            <div>
                                                <dxe:ASPxTextBox ID="txtGroupSchedule" ClientEnabled="true" ClientInstanceName="ctxtGroupSchedule" runat="server" MaskType="RegEx" Mask="[1-9]{2}" Width="100%">
                                                    <ValidationSettings RequiredField-IsRequired="true" Display="None"></ValidationSettings>
                                                </dxe:ASPxTextBox>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><div>Short Name</div></td>
                                        <td>
                                            <div>
                                                <dxe:ASPxTextBox ID="txtAccountShortName" ClientInstanceName="ctxtAccountShortName" runat="server" ValueType="System.String" Width="100%">
                                                    <ClientSideEvents TextChanged="function(s,e){UniqueCodeCheckPopUp()}" />
                                                    <ValidationSettings RequiredField-IsRequired="true" Display="None"></ValidationSettings>
                                                </dxe:ASPxTextBox>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><div>Name</div></td>
                                        <td>
                                            <div>
                                                <dxe:ASPxTextBox ID="txtAccountName" ClientInstanceName="ctxtAccountName" runat="server" ValueType="System.String" Width="100%">
                                                    <ClientSideEvents TextChanged="function(s,e){UniqueNameCheckpopUp()}" />
                                                    <ValidationSettings RequiredField-IsRequired="true" Display="None"></ValidationSettings>
                                                </dxe:ASPxTextBox>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><div>Parent Id</div></td>
                                        <td>
                                            <div>
                                                <dxe:ASPxComboBox ID="cmbParentId" ClientInstanceName="ccmbParentId" runat="server" Width="100%"
                                                    OnCallback="cmbParentId_Callback">
                                                    <ValidationSettings RequiredField-IsRequired="true" Display="None"></ValidationSettings>
                                                    <ClientSideEvents SelectedIndexChanged="cmbParentId_Change" />
                                                </dxe:ASPxComboBox>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <div>
                                                <dxe:ASPxButton ID="btnSave" ClientInstanceName="cbtnSave" runat="server" AutoPostBack="False" Text="Save" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                    <ClientSideEvents Click="function(s, e) {return PerformCallToGridBind(); }" />

                                                </dxe:ASPxButton>
                                                <dxe:ASPxButton ID="btnCancel" ClientInstanceName="cbtnCancel" runat="server" AutoPostBack="False" Text="Cancel" CssClass="btn btn-danger" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                    <ClientSideEvents Click="function(s, e) {return CloseWindow(); }" />

                                                </dxe:ASPxButton>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                               
                                
                                <div style="padding-top:15px;">
                                    
                                </div>

                            </dxe:PanelContent>
                        </PanelCollection>
                    </dxe:ASPxCallbackPanel>
                </dxe:PopupControlContentControl>
            </ContentCollection>

        </dxe:ASPxPopupControl>
    </div>
    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>
    <asp:HiddenField ID="CallStatus" runat="server" EnableViewState="true" />
    <asp:HiddenField ID="GroupId" runat="server" EnableViewState="true" />

    <asp:SqlDataSource ID="SqlDs1AccountGroupParentID" runat="server" ConflictDetection="CompareAllValues"
         SelectCommand=""></asp:SqlDataSource>
</asp:Content>


