<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.Master.management_master_ChargeSlab" CodeBehind="ChargeSlab.aspx.cs" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="javascript" type="text/javascript">
        function callback() {
            grid.PerformCallback();
        }
        function EndCall(obj) {
        }
        function ClickOnMoreInfo(keyValue) {

            var url = 'slab.aspx?id=' + keyValue;
            OnMoreInfoClick(url, "Edit Charge Slab Details", '940px', '450px', "Y");

        }

        function OnAddButtonClick() {
            var url = 'slab.aspx?id=' + 'ADD';
            OnMoreInfoClick(url, "Add Charge Slab Details", '940px', '450px', "Y");

        }
    </script>
    <table class="TableMain100">
        <tr>
            <td class="EHEADER" style="text-align: center;">
                <strong><span style="color: #000099">Chage Slab</span></strong>
            </td>
        </tr>
        <tr>
            <td style="" align="right">
                <table>
                    <tr>

                        <td>
                            <dxe:ASPxComboBox ID="cmbExport" runat="server" AutoPostBack="true" BackColor="Navy" Font-Bold="False" ForeColor="White" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" ValueType="System.Int32" Width="130px">
                                <Items>
                                    <dxe:ListEditItem Text="Select" Value="0" />
                                    <dxe:ListEditItem Text="PDF" Value="1" />
                                    <dxe:ListEditItem Text="XLS" Value="2" />
                                    <dxe:ListEditItem Text="RTF" Value="3" />
                                    <dxe:ListEditItem Text="CSV" Value="4" />
                                </Items>
                                <ButtonStyle BackColor="#C0C0FF" ForeColor="Black">
                                </ButtonStyle>
                                <ItemStyle BackColor="Navy" ForeColor="White">
                                    <HoverStyle BackColor="#8080FF" ForeColor="White">
                                    </HoverStyle>
                                </ItemStyle>
                                <Border BorderColor="White" />
                                <DropDownButton Text="Export">
                                </DropDownButton>
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

    <dxe:ASPxGridView ID="SlabGrid" runat="server" KeyFieldName="OtherChargeSlab_ID"
        AutoGenerateColumns="False" OnRowDeleting="SlabGrid_RowDeleting" DataSourceID="SlabDataSource" Width="100%" ClientInstanceName="grid" OnCustomCallback="SlabGrid_CustomCallback" OnCustomJSProperties="SlabGrid_CustomJSProperties">
        <ClientSideEvents EndCallback="function(s, e) {
	 EndCall(s.cpEND);

}" />
        <Templates>
            <EditForm>
            </EditForm>
            <TitlePanel>
                <table style="width: 100%">
                    <tr>
                        <td align="right">
                            <table width="200px">
                                <tr>

                                    <%--        <td>
                                        <dxe:ASPxButton ID="ASPxButton2" runat="server" Text="Search" ToolTip="Search" OnClick="btnSearch"  Height="18px" Width="88px" >
                                        </dxe:ASPxButton>
                                    </td>
                                    <td>
                                        <%if (Session["PageAccess"].ToString().Trim() == "All" || Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "DelAdd"){ %>
                                        <dxe:ASPxButton ID="ASPxButton1" runat="server" Text="ADD" ToolTip="Add New Data"   Height="18px" Width="88px"  AutoPostBack="False">
                                            <clientsideevents click="function(s, e) {OnAddButtonClick();}" />
                                        </dxe:ASPxButton>
                                        <%} %>
                                    </td>--%>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>


            </TitlePanel>
        </Templates>
        <SettingsBehavior AllowFocusedRow="True" ConfirmDelete="True" ColumnResizeMode="NextColumn" />
        <Styles>
            <Header CssClass="gridheader" SortingImageSpacing="5px" ImageSpacing="5px"></Header>

            <FocusedRow CssClass="gridselectrow"></FocusedRow>

            <LoadingPanel ImageSpacing="10px"></LoadingPanel>

            <FocusedGroupRow CssClass="gridselectrow"></FocusedGroupRow>
        </Styles>
        <SettingsPager ShowSeparators="True" AlwaysShowPager="True" NumericButtonCount="20" PageSize="20">
            <FirstPageButton Visible="True"></FirstPageButton>

            <LastPageButton Visible="True"></LastPageButton>
        </SettingsPager>
        <Columns>
            <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="0" FieldName="OtherChargeSlab_ID" Caption="ID">
                <CellStyle CssClass="gridcellleft"></CellStyle>

                <EditFormCaptionStyle HorizontalAlign="Right"></EditFormCaptionStyle>

                <EditFormSettings Visible="False"></EditFormSettings>
            </dxe:GridViewDataTextColumn>
            <dxe:GridViewDataTextColumn VisibleIndex="0" FieldName="OtherChargeSlab_Code" Width="10%" Caption="Code">
                <CellStyle CssClass="gridcellleft"></CellStyle>

                <EditFormCaptionStyle HorizontalAlign="Right"></EditFormCaptionStyle>

                <EditFormSettings Visible="False"></EditFormSettings>
            </dxe:GridViewDataTextColumn>
            <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="OtherChargeSlab_Type" Width="15%" Caption="Type">
                <CellStyle CssClass="gridcellleft"></CellStyle>

                <EditFormCaptionStyle HorizontalAlign="Right"></EditFormCaptionStyle>

                <EditFormSettings Visible="False"></EditFormSettings>
            </dxe:GridViewDataTextColumn>
            <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="OtherChargeSlab_MinRange" Width="15%" Caption="Min">
                <CellStyle CssClass="gridcellleft"></CellStyle>

                <EditFormCaptionStyle HorizontalAlign="Right"></EditFormCaptionStyle>

                <EditFormSettings Visible="False"></EditFormSettings>
            </dxe:GridViewDataTextColumn>
            <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="OtherChargeSlab_MaxRange" Width="15%" Caption="Max">
                <CellStyle CssClass="gridcellleft"></CellStyle>

                <EditFormCaptionStyle HorizontalAlign="Right"></EditFormCaptionStyle>

                <EditFormSettings Visible="False"></EditFormSettings>
            </dxe:GridViewDataTextColumn>
            <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="OtherChargeSlab_FlatRate" Width="10%" Caption="Flat Rate">
                <CellStyle CssClass="gridcellleft"></CellStyle>

                <EditFormCaptionStyle HorizontalAlign="Right"></EditFormCaptionStyle>

                <EditFormSettings Visible="False"></EditFormSettings>
            </dxe:GridViewDataTextColumn>
            <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="OtherChargeSlab_MinCharge" Width="10%" Caption="Min. Charge">
                <CellStyle CssClass="gridcellleft"></CellStyle>

                <EditFormCaptionStyle HorizontalAlign="Right"></EditFormCaptionStyle>

                <EditFormSettings Visible="False"></EditFormSettings>
            </dxe:GridViewDataTextColumn>
            <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="OtherChargeSlab_Rate" Width="10%" Caption="Rate">
                <CellStyle CssClass="gridcellleft"></CellStyle>

                <EditFormCaptionStyle HorizontalAlign="Right"></EditFormCaptionStyle>

                <EditFormSettings Visible="False"></EditFormSettings>
            </dxe:GridViewDataTextColumn>
            <dxe:GridViewDataTextColumn VisibleIndex="7" Width="10%" Caption="Details">
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <DataItemTemplate>
                    <a href="javascript:void(0);" onclick="ClickOnMoreInfo('<%# Container.KeyValue %>')">More Info...</a>

                </DataItemTemplate>

                <CellStyle Wrap="False"></CellStyle>
                <HeaderTemplate>
                    <%if (Session["PageAccess"].ToString().Trim() == "All" || Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "DelAdd")
                      { %>
                    <a href="javascript:void(0);" onclick="OnAddButtonClick( )">
                        <span style="color: #000099; text-decoration: underline">Add New</span>
                    </a>
                    <%} %>
                </HeaderTemplate>

                <EditFormSettings Visible="False"></EditFormSettings>
            </dxe:GridViewDataTextColumn>
            <dxe:GridViewCommandColumn VisibleIndex="8" Caption="Delete" ShowDeleteButton="True"/>
        </Columns>
        <SettingsEditing Mode="PopupEditForm" PopupEditFormHorizontalAlign="Center" PopupEditFormModal="True"
            PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="900px" EditFormColumnCount="3" />
        <SettingsText PopupEditFormCaption="Add/ Modify Employee" GroupPanel="Other Charge Slabs" />
        <Settings ShowGroupPanel="True" ShowStatusBar="Visible" />
    </dxe:ASPxGridView>
    <asp:SqlDataSource ID="SlabDataSource" runat="server"
        DeleteCommand="delete from Master_OtherChargeSlab where OtherChargeSlab_ID=@OtherChargeSlab_ID ">
        <DeleteParameters>
            <asp:Parameter Name="OtherChargeSlab_ID" Type="String" />

        </DeleteParameters>
    </asp:SqlDataSource>

    <br />
    <dxe:ASPxGridViewExporter ID="exporter" runat="server">
    </dxe:ASPxGridViewExporter>
</asp:Content>
