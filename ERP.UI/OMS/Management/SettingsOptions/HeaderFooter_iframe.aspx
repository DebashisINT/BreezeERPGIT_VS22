<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.SettingsOptions.management_SettingsOptions_HeaderFooter_iframe" CodeBehind="HeaderFooter_iframe.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--

    --%>

    <script type="text/javascript">
        function ClickOnMoreInfo(keyValue) {
            //alert(keyValue);
            var url = 'HeaderFooterDetails.aspx?id=' + keyValue;
            //OnMoreInfoClick(url, "Show Items", '960px', '530px', "Y")
            popup.SetContentUrl(url);
            popup.Show();
        }

        function CustomDelete(s, e) {
            alert(s);
        }

        function OnAddButtonClick() {
            //alert(keyValue);
            var url = 'HeaderFooterDetails.aspx?id=Add';
            //OnMoreInfoClick(url, "Show Items", '960px', '530px', "Y")
            popup.SetContentUrl(url);
            popup.Show();

        }

        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }
        
        function callback() {
            grid.PerformCallback();
        }
        function EndCall(obj) {         
        }
        function OnCustomButtonClick(s, e) {
            if (e.buttonID == 'cusDel') { 
                grid.DeleteRow(e.visibleIndex);
            }
            
            if (e.buttonID == 'cusEdit') {
                ClickOnMoreInfo(grid.keys[e.visibleIndex]);
            }
        }
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Header/Footer</h3>
        </div>
    </div>
    <div class="form_main">
        <table class="TableMain100" style="width: 100%">
            <%--<tr>
                <td class="EHEADER" style="text-align: center;">
                    <strong><span style="color: #000099">Header/Footer</span></strong>
                </td>
            </tr>--%>
            <tr>
                <td>
                    <table width="100%">
                        <tr>
                            <td style="text-align: left; vertical-align: top; height: 37px;">
                                <table>
                                    <tr>
                                        <td id="ShowFilter">
                                  <% if (rights.CanAdd)
                                   { %>
                                <a href="javascript:void(0);" onclick="javascript:OnAddButtonClick();" class="btn btn-primary">Add New</a>
                                 <% } %>
                                   
                                            <% if (rights.CanExport)
                                               { %>
                                             <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true"  >
                                    <asp:ListItem Value="0">Export to</asp:ListItem>
                                    <asp:ListItem Value="1">PDF</asp:ListItem>
                                     <asp:ListItem Value="2">XLS</asp:ListItem>
                                     <asp:ListItem Value="3">RTF</asp:ListItem>
                                     <asp:ListItem Value="4">CSV</asp:ListItem>
                        
                                </asp:DropDownList> 
                                               <% } %>
                                   
                            </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="height: 37px"></td>
                            <td style="text-align: right; vertical-align: top; height: 37px;" class="pull-right">
                               <%-- <dxe:ASPxComboBox ID="cmbExport" runat="server" AutoPostBack="true" 
                                    Font-Bold="False" ForeColor="black" ValueType="System.Int32" Width="130px">
                                    <Items>
                                        <dxe:ListEditItem Text="Select" Value="0" />
                                        <dxe:ListEditItem Text="PDF" Value="1" />
                                        <dxe:ListEditItem Text="XLS" Value="2" />
                                        <dxe:ListEditItem Text="RTF" Value="3" />
                                        <dxe:ListEditItem Text="CSV" Value="4" />
                                    </Items>
                                    <ButtonStyle>
                                    </ButtonStyle>
                                    <ItemStyle>
                                        <HoverStyle >
                                        </HoverStyle>
                                    </ItemStyle>
                                    <Border BorderColor="black" />
                                    <DropDownButton Text="Export">
                                    </DropDownButton>
                                </dxe:ASPxComboBox>--%>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <dxe:ASPxGridView ID="HeaderFooterGrid" runat="server" Width="100%" AutoGenerateColumns="False"
                        ClientInstanceName="grid" DataSourceID="HeaderFooter" KeyFieldName="HeaderFooter_ID"
                        OnAutoFilterCellEditorInitialize="HeaderFooterGrid_AutoFilterCellEditorInitialize"
                        OnCellEditorInitialize="HeaderFooterGrid_CellEditorInitialize" OnCustomCallback="HeaderFooterGrid_CustomCallback"
                        OnHtmlEditFormCreated="HeaderFooterGrid_HtmlEditFormCreated" OnHtmlRowCreated="HeaderFooterGrid_HtmlRowCreated"
                        OnInitNewRow="HeaderFooterGrid_InitNewRow" OnRowValidating="HeaderFooterGrid_RowValidating">
                        <SettingsBehavior AllowFocusedRow="True" ConfirmDelete="True" />
                        <Styles>
                            <Header SortingImageSpacing="5px" ImageSpacing="5px"></Header>

                            <FocusedGroupRow CssClass="gridselectrow"></FocusedGroupRow>

                            <FocusedRow CssClass="gridselectrow"></FocusedRow>

                            <LoadingPanel ImageSpacing="10px"></LoadingPanel>
                        </Styles>
                        <SettingsPager AlwaysShowPager="True" NumericButtonCount="20" PageSize="20" ShowSeparators="True">
                            <FirstPageButton Visible="True"></FirstPageButton>

                            <LastPageButton Visible="True"></LastPageButton>
                        </SettingsPager>
                        <SettingsEditing EditFormColumnCount="1" PopupEditFormVerticalAlign="WindowCenter"
                            PopupEditFormWidth="800px" Mode="EditForm" PopupEditFormHeight="400px" PopupEditFormHorizontalAlign="LeftSides"
                            PopupEditFormModal="True" />
                        <SettingsText ConfirmDelete="Are You Sure To Delete This Record ???" />
                        <ClientSideEvents EndCallback="function(s, e) {
	                              EndCall(s.cpEND);
                            }"  CustomButtonClick="OnCustomButtonClick" ></ClientSideEvents>
                        <Columns>
                            <dxe:GridViewDataTextColumn FieldName="HeaderFooter_ID" Visible="False" VisibleIndex="0">
                                <EditFormSettings Visible="False"></EditFormSettings>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="DropDownList1" runat="server">
                                    </asp:DropDownList>



                                </EditItemTemplate>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataComboBoxColumn FieldName="HeaderFooter_Type" Caption="Type" VisibleIndex="0">
                                <PropertiesComboBox ValueType="System.Char" Width="40%">
                                    <Items>
                                        <dxe:ListEditItem Text="Header" Value="H"></dxe:ListEditItem>
                                        <dxe:ListEditItem Text="Footer" Value="F"></dxe:ListEditItem>
                                    </Items>

                                    <ValidationSettings CausesValidation="True" EnableCustomValidation="True"></ValidationSettings>
                                </PropertiesComboBox>

                                <EditFormSettings ColumnSpan="2" Visible="True"></EditFormSettings>
                            </dxe:GridViewDataComboBoxColumn>
                            <dxe:GridViewDataTextColumn FieldName="HeaderFooter_ShortName" Width="400px" Caption="Name" VisibleIndex="1">
                                <PropertiesTextEdit Width="650px">
                                    <ValidationSettings ErrorDisplayMode="ImageWithText" CausesValidation="True" ErrorText="Name cannot be left blank" SetFocusOnError="True" ValidationGroup="a">
                                        <RequiredField IsRequired="True" ErrorText="Name cannot be left blank"></RequiredField>
                                    </ValidationSettings>
                                </PropertiesTextEdit>

                                <EditFormSettings ColumnSpan="2"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataMemoColumn FieldName="HeaderFooter_Content" Visible="false" Caption="Content" VisibleIndex="2">
                                <PropertiesMemoEdit Height="400px" EncodeHtml="False" EnableDefaultAppearance="False">
                                    <Style BackColor="White">
                                        <Border BorderColor="Black" > </Border >
                                    </Style>
                                </PropertiesMemoEdit>

                                <EditFormSettings Visible="True"></EditFormSettings>
                                <DataItemTemplate>
                                    <a href="javascript:void(0);" onclick="ClickOnMoreInfo('<%# Container.KeyValue %>')">
                                        <u>MoreInfo... </u></a>



                                </DataItemTemplate>

                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>

                                <CellStyle Wrap="True"></CellStyle>
                            </dxe:GridViewDataMemoColumn>
                            <dxe:GridViewCommandColumn VisibleIndex="3" ShowDeleteButton="false">
                                <%--<DeleteButton Visible="True"></DeleteButton>--%>
                                <CustomButtons>
                                    <dxe:GridViewCommandColumnCustomButton Text=" " ID="cusDel" Image-Url="../../../assests/images/Delete.png"></dxe:GridViewCommandColumnCustomButton>
                                    <dxe:GridViewCommandColumnCustomButton Text=" " ID="cusEdit" Image-Url="../../../assests/images/Edit.png"></dxe:GridViewCommandColumnCustomButton>
                                </CustomButtons>
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <HeaderTemplate>
                                    <%if (Session["PageAccess"].ToString().Trim() == "All" || Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "DelAdd")
                                      { %>
                                    <%--<a href="javascript:void(0);" onclick="grid.AddNewRow()"><span style="color: #000099;
                                            text-decoration: underline">Add New Content</span> </a>--%>
                                  <%--  <a href="javascript:void(0);" onclick="OnAddButtonClick()">
                                        <u>Add New</u></a>--%>
                                    <%} %>
                                    Actions
                                </HeaderTemplate>
                               
                            </dxe:GridViewCommandColumn>
                              
                        </Columns>
                         
                        <SettingsCommandButton>
                            <DeleteButton Text="Delete"></DeleteButton>
                        </SettingsCommandButton>
                        <Settings ShowGroupPanel="True" ShowStatusBar="Visible" />
                        <StylesEditors>
                            <ProgressBar Height="25px"></ProgressBar>
                        </StylesEditors>
                    </dxe:ASPxGridView>
                     <dxe:ASPxPopupControl ID="ASPXPopupControl" runat="server" ContentUrl="HeaderFooterDetails.aspx"
                                            CloseAction="CloseButton" ClientInstanceName="popup" Height="466px" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                                            Width="900px" HeaderText="Add/Modify Item" AllowResize="true" ResizingMode="Postponed" Modal="true">
                                            <ContentCollection>
                                                <dxe:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                                                </dxe:PopupControlContentControl>
                                            </ContentCollection>
                                            <HeaderStyle BackColor="Blue" Font-Bold="True" ForeColor="White" />
                                        </dxe:ASPxPopupControl>
                     <dxe:ASPxGridViewExporter ID="exporter" runat="server">
        </dxe:ASPxGridViewExporter>
                    <asp:SqlDataSource ID="HeaderFooter" runat="server" 
                        SelectCommand="SELECT HeaderFooter_ID, HeaderFooter_Type, HeaderFooter_ShortName, HeaderFooter_Content FROM [Master_HeaderFooter] ORDER BY HeaderFooter_ID"
                        DeleteCommand="Delete From [Master_HeaderFooter] where HeaderFooter_ID=@HeaderFooter_ID"
                        InsertCommand="INSERT INTO Master_HeaderFooter(HeaderFooter_Type, HeaderFooter_ShortName, HeaderFooter_Content) VALUES (@HeaderFooter_Type, @HeaderFooter_ShortName, @HeaderFooter_Content)"
                        UpdateCommand="Update [Master_HeaderFooter] set [HeaderFooter_ShortName]=@HeaderFooter_ShortName,[HeaderFooter_Content]=@HeaderFooter_Content where HeaderFooter_ID=@HeaderFooter_ID">
                        <DeleteParameters>
                            <asp:Parameter Name="HeaderFooter_ID" />
                        </DeleteParameters>
                        <UpdateParameters>
                            <asp:Parameter Name="HeaderFooter_ShortName" />
                            <asp:Parameter Name="HeaderFooter_Content" />
                            <asp:Parameter Name="HeaderFooter_ID" />
                        </UpdateParameters>
                        <InsertParameters>
                            <asp:Parameter Name="HeaderFooter_Type" />
                            <asp:Parameter Name="HeaderFooter_ShortName" />
                            <asp:Parameter Name="HeaderFooter_Content" />
                        </InsertParameters>
                    </asp:SqlDataSource>
                    &nbsp;&nbsp;
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
