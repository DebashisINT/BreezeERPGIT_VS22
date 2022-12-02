<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.ToolsUtilities.management_utilities_frmShowForm" CodeBehind="frmShowForm.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <script type="text/javascript">
        function OnAddButtonClick() {
            var flg = document.getElementById("TextBox1").value;
            var url = 'frmAddForms.aspx?id=' + flg;
            //OnMoreInfoClick(url, "Add New File", '700px', '370px', "Y");
            window.location.href = url;
        }
        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }
        function callback() {
            grid.PerformCallback();
        }
        function callheight(obj) {
            //height();
            // parent.CallMessage();
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Forms & Notices
                <asp:Label ID="lblP" runat="server"
                    Text="Label"></asp:Label></h3>
        </div>
    </div>
    <div class="form_main">
        <table class="TableMain100">
            <tr>
                <td style="vertical-align: top; padding-left: 30px;">
                    <div class="crossBtn"><a href="../ProjectMainPage.aspx"><i class="fa fa-times"></i></a></div>
                </td>
            </tr>
            <tr>
                <td style="text-align: left; vertical-align: top">
                    <table>
                        <tr>
                            <td id="ShowFilter">
                                <%--<a href="javascript:ShowHideFilter('s');" class="btn btn-success"><span >Show Filter</span></a>--%>
                                <a href="javascript:void(0);" onclick="OnAddButtonClick();" class="btn btn-primary"><span>Add New</span> </a>
                            </td>
                            <td id="Td1">
                                <%--<a href="javascript:ShowHideFilter('All');" class="btn btn-primary"><span>All Records</span></a>--%>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <dxe:ASPxGridView ID="grdDocuments" runat="server" AutoGenerateColumns="False" OnRowCommand="grdDocuments_RowCommand"
                        KeyFieldName="formID" Width="100%" ClientInstanceName="grid"
                        OnCustomCallback="grdDocuments_CustomCallback">
                        <%--OnRowDeleting="grdDocuments_RowDeleting" --%>
                        <SettingsSearchPanel Visible="True" />
                        <Settings ShowGroupedColumns="True" ShowGroupPanel="True" ShowFilterRow="true" ShowFilterRowMenu="true" />
                        <Columns>
                            <dxe:GridViewDataTextColumn FieldName="formID" ReadOnly="True" VisibleIndex="0" Visible="false">
                                <CellStyle HorizontalAlign="Left">
                                </CellStyle>
                                <HeaderStyle HorizontalAlign="Center" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="FormName" ReadOnly="True" VisibleIndex="0">
                                <CellStyle HorizontalAlign="Left">
                                </CellStyle>
                                <HeaderStyle HorizontalAlign="Center" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="FilePath" VisibleIndex="1" Visible="false">
                                <CellStyle HorizontalAlign="Left">
                                </CellStyle>
                                <HeaderStyle HorizontalAlign="Center" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Description" VisibleIndex="1">
                                <CellStyle HorizontalAlign="Left">
                                </CellStyle>
                                <HeaderStyle HorizontalAlign="Center" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Width="6%" FieldName="formID" ReadOnly="True"
                                VisibleIndex="5">
                                <DataItemTemplate>
                                    <a href='<%= Page.ResolveUrl("~/OMS/Management/ViewFile.aspx")%>?id=<%#Eval("FilePath") %>' target="_blank" title="View" class="pad" style="text-decoration: none;">
                                        <img src="/OMS/images/show.png" /></a>
                                    <asp:LinkButton ID="btn_delete" runat="server" OnClientClick="return confirm('Are you sure you want to delete?');" CommandArgument='<%# Container.KeyValue %>' CommandName="delete"> 
                                         <img src="/assests/images/Delete.png" />
                                    </asp:LinkButton>
                                </DataItemTemplate>
                                <EditFormSettings Visible="False" />
                                <CellStyle HorizontalAlign="Center">
                                </CellStyle>
                                <HeaderStyle HorizontalAlign="Center" />
                                <HeaderTemplate>
                                    Actions
                                </HeaderTemplate>
                                <Settings AllowAutoFilter="False" />
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                        <SettingsBehavior AllowFocusedRow="false" ColumnResizeMode="NextColumn" ConfirmDelete="true" />
                        <SettingsCommandButton>
                            <EditButton Image-Url="/assests/images/Edit.png" ButtonType="Image" Image-AlternateText="Edit" Styles-Style-CssClass="pad">
                            </EditButton>
                            <DeleteButton Image-Url="/assests/images/Delete.png" ButtonType="Image" Image-AlternateText="Delete">
                            </DeleteButton>
                            <UpdateButton Text="Update" ButtonType="Button" Styles-Style-CssClass="btn btn-primary btn-xs"></UpdateButton>
                            <CancelButton Text="Cancel" ButtonType="Button" Styles-Style-CssClass="btn btn-danger btn-xs"></CancelButton>
                        </SettingsCommandButton>
                        <%--<Styles>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                            </Header>
                            <FocusedGroupRow CssClass="gridselectrow">
                            </FocusedGroupRow>
                            <FocusedRow CssClass="gridselectrow">
                            </FocusedRow>
                        </Styles>
                        <SettingsPager NumericButtonCount="20" PageSize="20" ShowSeparators="True" AlwaysShowPager="True">
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </SettingsPager>--%>
                        <SettingsEditing Mode="PopupEditForm" PopupEditFormHorizontalAlign="Center" PopupEditFormHeight="600px"
                            PopupEditFormVerticalAlign="TopSides" PopupEditFormWidth="900px" />
                        <SettingsText PopupEditFormCaption="Add/Modify Cash/Bank" ConfirmDelete="Are you sure to Delete this Record!" />
                        <ClientSideEvents EndCallback="function(s, e) {
	callheight(s.cpHeight);
}" />
                    </dxe:ASPxGridView>
                    <%--   <asp:SqlDataSource ID="grddoc" runat="server" 
                            SelectCommand="" DeleteCommand="DELETE FROM [tbl_master_forms] WHERE [frm_id] = @formID">
                            <DeleteParameters>
                                <asp:Parameter Name="formID" Type="int32" />
                            </DeleteParameters>
                            <SelectParameters>
                                <asp:SessionParameter Name="userlist" SessionField="userchildHierarchy" Type="string" />
                            </SelectParameters>
                        </asp:SqlDataSource>--%></td>
            </tr>
            <tr>
                <td style="display: none;">
                    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox></td>
            </tr>
        </table>
    </div>
</asp:Content>
