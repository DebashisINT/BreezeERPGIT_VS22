<%@ Page Title="Sales Document" Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.management_SalesDocument" CodeBehind="SalesDocument.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script lang="javascript" type="text/javascript">
        function Show() {
            var url = "frmAddDocuments.aspx?id=SalesDocument.aspx&id1=Lead";
            popup.SetContentUrl(url);

            popup.Show();

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="TableMain100">
        <dxe:ASPxGridView ID="EmployeeDocumentGrid" runat="server" AutoGenerateColumns="False"
            ClientInstanceName="gridDocument" KeyFieldName="Id" Width="100%" Font-Size="12px"
            OnRowDeleting="EmployeeDocumentGrid_RowDeleting">
            <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True" />
            <Styles>
                <LoadingPanel ImageSpacing="10px">
                </LoadingPanel>
                <Header ImageSpacing="5px" SortingImageSpacing="5px">
                </Header>
            </Styles>
            <SettingsPager NumericButtonCount="20" PageSize="20" ShowSeparators="True">
            </SettingsPager>
            <Columns>
                <dxe:GridViewDataTextColumn FieldName="Id" ReadOnly="True" VisibleIndex="0" Visible="False">
                    <EditFormSettings Visible="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn FieldName="Type" VisibleIndex="0" Caption="DocumentType"
                    Width="25%">
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn FieldName="FileName" VisibleIndex="1" Caption="DocumentName"
                    Width="25%">
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn FieldName="Src" VisibleIndex="2" Visible="False">
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn FieldName="FilePath" ReadOnly="True" VisibleIndex="2"
                    Caption="Document Physical Location" Width="25%">
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataHyperLinkColumn Caption="View" FieldName="Src" VisibleIndex="3"
                    Width="15%">
                    <DataItemTemplate>
                        <a href='viewImage.aspx?id=<%#Eval("Src") %>' target="_blank">View</a>
                    </DataItemTemplate>
                </dxe:GridViewDataHyperLinkColumn>
                <dxe:GridViewCommandColumn VisibleIndex="4" ShowDeleteButton="True">
                    <HeaderTemplate>
                        <a href="javascript:void(0);" onclick="Show();">
                            <span style="color: #000099; text-decoration: underline">Add New</span>
                        </a>
                    </HeaderTemplate>
                </dxe:GridViewCommandColumn>
            </Columns>

            <SettingsEditing Mode="PopupEditForm" PopupEditFormHeight="250px" PopupEditFormHorizontalAlign="Center"
                PopupEditFormModal="True" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="500px"
                EditFormColumnCount="1" />
            <SettingsText PopupEditFormCaption="Add/Modify Family Relationship" ConfirmDelete="Confirm delete?" />
            <Settings ShowGroupPanel="True" ShowFooter="True" ShowStatusBar="Hidden" ShowTitlePanel="True" />
        </dxe:ASPxGridView>
        <dxe:ASPxPopupControl ID="ASPXPopupControl" runat="server" ContentUrl="frmAddDocuments.aspx"
            CloseAction="CloseButton" Top="100" Left="400" ClientInstanceName="popup" Height="500px"
            Width="430px" HeaderText="Add Document">
            <ContentCollection>
                <dxe:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>

    </div>
</asp:Content>
