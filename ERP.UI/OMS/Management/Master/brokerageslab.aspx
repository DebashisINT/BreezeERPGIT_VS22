<%@ Page Title="Tax Slabs" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.Master.management_master_brokerageslab" CodeBehind="brokerageslab.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script language="javascript" type="text/javascript">

        function ClickOnMoreInfo(keyValue) {
            var url = 'bslab.aspx?id=' + keyValue;
            window.location.href = url;
            //  OnMoreInfoClick(url, "Edit Brokerage Slab Details", '940px', '450px', "Y");
        }

        function OnAddButtonClick() {
            var url = 'bslab.aspx?id=' + 'ADD';
            // OnMoreInfoClick(url, "Add Brokerage Slab Details", '940px', '450px', "Y");
            window.location.href = url;
        }

        function callback() {
            grid.PerformCallback();
        }
        function EndCall(obj) {
            if (grid.cpDelmsg != null)
                alert(grid.cpDelmsg);
        }

        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Tax Slabs</h3>
        </div>
    </div>
    <div class="form_main">
        <table class="TableMain100">
            <tr>
                <td>
                    <table class="TableMain100">
                        <tr>
                            <td style="text-align: left; vertical-align: top">
                                <table>
                                    <tr>
                                        <td id="ShowFilter">
                                            <%--<a href="javascript:ShowHideFilter('s');" class="btn btn-success"><span >Show Filter</span></a>--%>
                                            <% if (rights.CanAdd)
                                               { %>
                                            <a href="javascript:void(0);" onclick="OnAddButtonClick( )" class="btn btn-primary"><span>Add New</span> </a><%} %>
                                           <% if (rights.CanExport)
                                               { %>
                                             <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnChange="if(!AvailableExportOption()){return false;}" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true"  >
                                                <asp:ListItem Value="0">Export to</asp:ListItem>
                                                <asp:ListItem Value="1">PDF</asp:ListItem>
                                                 <asp:ListItem Value="2">XLS</asp:ListItem>
                                                 <asp:ListItem Value="3">RTF</asp:ListItem>
                                                 <asp:ListItem Value="4">CSV</asp:ListItem>
                                            </asp:DropDownList>
                                            <% } %>
                                        </td>
                                        <td id="Td1">
                                            <%--  <a href="javascript:ShowHideFilter('All');" class="btn btn-primary"><span>All Records</span></a>--%>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <%--<td class="gridcellright pull-right">
                                <dxe:ASPxComboBox ID="cmbExport" runat="server" AutoPostBack="true"
                                    Font-Bold="False" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged"
                                    ValueType="System.Int32" Width="130px">
                                    <Items>
                                        <dxe:ListEditItem Text="Select" Value="0" />
                                        <dxe:ListEditItem Text="PDF" Value="1" />
                                        <dxe:ListEditItem Text="XLS" Value="2" />
                                        <dxe:ListEditItem Text="RTF" Value="3" />
                                        <dxe:ListEditItem Text="CSV" Value="4" />
                                    </Items>
                                    <Border BorderColor="Black" />
                                    <DropDownButton Text="Export">
                                    </DropDownButton>
                                </dxe:ASPxComboBox>
                            </td>--%>
                        </tr>
                    </table>
                </td>
                <td></td>
            </tr>
            <tr>
                <td>

                    <%--<tr>
            <td style="" align="right">
                <table>
                    <tr>
                        <td>
                            <dxe:ASPxComboBox ID="cmbExport" runat="server" AutoPostBack="true" BackColor="Navy"
                                Font-Bold="False" ForeColor="White" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged"
                                ValueType="System.Int32" Width="130px">
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
        </tr>--%>


                    <dxe:ASPxGridView ID="BrokerageSlabGrid" runat="server" KeyFieldName="BrokerageSlab_ID"
                        AutoGenerateColumns="False" OnRowDeleting="BrokerageSlabGrid_RowDeleting" DataSourceID="BrokerageSlabDataSource"
                        Width="100%" ClientInstanceName="grid" OnCustomCallback="BrokerageSlabGrid_CustomCallback"
                        OnCustomJSProperties="BrokerageSlabGrid_CustomJSProperties"  OnCommandButtonInitialize="BrokerageSlabGrid_CommandButtonInitialize">
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
                                                <%--                     <tr>
          
                                    <td>
                                        <dxe:ASPxButton ID="ASPxButton2" runat="server" Text="Search" ToolTip="Search" OnClick="btnSearch"  Height="18px" Width="88px"  >
                                        </dxe:ASPxButton>
                                    </td>
                                    <td>
                                        <%if (Session["PageAccess"].ToString().Trim() == "All" || Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "DelAdd"){ %>
                                        <dxe:ASPxButton ID="ASPxButton1" runat="server" Text="ADD" ToolTip="Add New Data"   Height="18px" Width="88px"   AutoPostBack="False">
                                            <clientsideevents click="function(s, e) {OnAddButtonClick();}" />
                                        </dxe:ASPxButton>
                                        <%} %>
                                    </td>
                                 
                                  </tr>--%>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </TitlePanel>
                        </Templates>
                        <SettingsBehavior AllowFocusedRow="False" ConfirmDelete="True" ColumnResizeMode="NextColumn" />
                        <Styles>
                            <Header CssClass="gridheader" SortingImageSpacing="5px" ImageSpacing="5px">
                            </Header>
                            <FocusedRow CssClass="gridselectrow">
                            </FocusedRow>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                            <FocusedGroupRow CssClass="gridselectrow">
                            </FocusedGroupRow>
                        </Styles>
                        <SettingsPager ShowSeparators="True" AlwaysShowPager="True" NumericButtonCount="20"
                            PageSize="20">
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </SettingsPager>
                        <Columns>
                            <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="0" FieldName="BrokerageSlab_ID"
                                Caption="ID">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="0" FieldName="BrokerageSlab_Code" Width="10%"
                                Caption="Code">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="BrokerageSlab_Type" Width="15%"
                                Caption="Type">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="BrokerageSlab_MinRange"
                                Width="15%" Caption="Min">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="BrokerageSlab_MaxRange"
                                Width="15%" Caption="Max">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="BrokerageSlab_FlatRate"
                                Width="10%" Caption="Flat Rate">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="BrokerageSlab_MinCharge"
                                Width="10%" Caption=" Min.Charge">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="BrokerageSlab_Rate" Width="10%"
                                Caption="Rate">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="7" Width="10%" Caption="Details" CellStyle-HorizontalAlign="Center">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <DataItemTemplate>
                                    <% if (rights.CanEdit)
                                       { %>
                                    <a href="javascript:void(0);" onclick="ClickOnMoreInfo('<%# Container.KeyValue %>')" title="More Info">
                                        <img src="../../../assests/images/info.png" />
                                    </a><%} %>
                                </DataItemTemplate>
                                <CellStyle Wrap="False">
                                </CellStyle>
                                <HeaderTemplate>
                                    <span>More Info</span>
                                    <%--    <%if (Session["PageAccess"].ToString().Trim() == "All" || Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "DelAdd")
                      { %>
                    <a href="javascript:void(0);" onclick="OnAddButtonClick( )"><span style="text-decoration: underline">Add New</span> </a>
                    <%} %>--%>
                                </HeaderTemplate>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewCommandColumn VisibleIndex="8" HeaderStyle-HorizontalAlign="Center" Width="50px" CellStyle-HorizontalAlign="Center" ShowDeleteButton="true">
                                <%--  <DeleteButton Visible="True">
                </DeleteButton>--%>
<HeaderStyle HorizontalAlign="Center"></HeaderStyle>

<CellStyle HorizontalAlign="Center"></CellStyle>
                                <HeaderTemplate>
                                    <span>Actions</span>

                                </HeaderTemplate>
                            </dxe:GridViewCommandColumn>
                        </Columns>
                        <SettingsCommandButton>

                            <DeleteButton Image-Url="/assests/images/Delete.png" ButtonType="Image" Image-AlternateText="Delete">
<Image AlternateText="Delete" Url="/assests/images/Delete.png"></Image>
                            </DeleteButton>

                        </SettingsCommandButton>
                        <SettingsEditing Mode="PopupEditForm" PopupEditFormHorizontalAlign="Center" PopupEditFormModal="True"
                            PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="900px" EditFormColumnCount="3" />
                        <SettingsText PopupEditFormCaption="Add/ Modify Employee" />
                        <SettingsSearchPanel Visible="True" />
                        <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="True" ShowFilterRowMenu="True" />
                    </dxe:ASPxGridView>

                </td>
            </tr>
        </table>
        <asp:SqlDataSource ID="BrokerageSlabDataSource" runat="server"
            DeleteCommand="delete from Master_BrokerageSlab where BrokerageSlab_ID=@BrokerageSlab_ID">
            <DeleteParameters>
                <asp:Parameter Name="BrokerageSlab_ID" Type="String" />
            </DeleteParameters>
        </asp:SqlDataSource>
        <br /> 
        <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="true" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
    </div>
</asp:Content>
