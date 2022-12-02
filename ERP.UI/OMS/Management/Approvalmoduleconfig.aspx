<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="Approvalmoduleconfig.aspx.cs" Inherits="ERP.OMS.Management.Approvalmoduleconfig" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

     <dxe:ASPxGridView ID="grid" ClientInstanceName="grid" runat="server" OnBatchUpdate="grid_BatchUpdate" KeyFieldName="CustomerId" OnCellEditorInitialize="grid_CellEditorInitialize">
           <Columns>
               <dxe:GridViewCommandColumn ShowNewButtonInHeader="true" ShowDeleteButton="true"></dxe:GridViewCommandColumn>
                <dxe:GridViewDataTextColumn FieldName="moduleid" VisibleIndex="0" Visible="false" Caption="Entries">
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn FieldName="acid" VisibleIndex="1" Visible="false" Caption="acid">
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn Caption="Active" VisibleIndex="2" FieldName="active">
                    <DataItemTemplate>
                        <dxe:ASPxCheckBox ID="chkDetail" runat="server">
                        </dxe:ASPxCheckBox>
                     
                    </DataItemTemplate>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn FieldName="modulename" Visible="true" VisibleIndex="3" Caption="Entries">
                </dxe:GridViewDataTextColumn>
              <dxe:GridViewDataComboBoxColumn Caption="1st Level User" FieldName="level1userid" VisibleIndex="4" Width="220px">
                    <PropertiesComboBox ValueField="LevelID" ClientInstanceName="cCmblevel1" TextField="LevelName" EnableCallbackMode="true">
                        <%-- <ValidationSettings RequiredField-IsRequired="true" Display="None"></ValidationSettings>--%>
                        <ClientSideEvents SelectedIndexChanged="function(s, e) { OnCmblevel1SelectedIndexChanged(s); }" />
                    </PropertiesComboBox>
                </dxe:GridViewDataComboBoxColumn>

                <dxe:GridViewDataComboBoxColumn Caption="2nd Level User" FieldName="level2userid" VisibleIndex="5" Width="220px">
                    <PropertiesComboBox ValueField="LevelID" ClientInstanceName="cCmblevel2" TextField="LevelName" EnableCallbackMode="true">
                        <%-- <ValidationSettings RequiredField-IsRequired="true" Display="None"></ValidationSettings>--%>
                         <ClientSideEvents SelectedIndexChanged="function(s, e) { OnCmblevel2SelectedIndexChanged(s); }" />
                    </PropertiesComboBox>
                </dxe:GridViewDataComboBoxColumn>

                  <dxe:GridViewDataComboBoxColumn Caption="3rd Level User" FieldName="level3userid" VisibleIndex="6" Width="220px">
                    <PropertiesComboBox ValueField="LevelID" ClientInstanceName="cCmblevel3" TextField="LevelName" EnableCallbackMode="true">
                        <%-- <ValidationSettings RequiredField-IsRequired="true" Display="None"></ValidationSettings>--%>
                         <ClientSideEvents SelectedIndexChanged="function(s, e) { OnCmblevel3SelectedIndexChanged(s); }" />
                    </PropertiesComboBox>
                </dxe:GridViewDataComboBoxColumn>
              
           </Columns>
           <ClientSideEvents/>
           <SettingsEditing Mode="Batch">
               <BatchEditSettings ShowConfirmOnLosingChanges="true" EditMode="Cell" />
           </SettingsEditing>
       </dxe:ASPxGridView>
</asp:Content>
