<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="EmployeeSyncList.aspx.cs" Inherits="ERP.OMS.Management.Master.EmployeeSyncList" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function gridRowclick(s, e) {
            $('#GrdEmployee').find('tr').removeClass('rowActive');
            $('.floatedBtnArea').removeClass('insideGrid');
            $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea').addClass('insideGrid');
            $(s.GetRow(e.visibleIndex)).addClass('rowActive');
            setTimeout(function () {
                var lists = $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a');
                $.each(lists, function (index, value) {
                    setTimeout(function () {
                        console.log(value);
                        $(value).css({ 'opacity': '1' });
                    }, 100);
                });
            }, 200);
        }

        function ViewLogData() {
            cGvJvSearch.Refresh();
            //cGrdEmployee.UnselectRows();
        }

        function AfterSync() {
            //cGrdEmployee.Refresh();

            //ViewLogData();
            jAlert("Customer sync successfully.", "alert", function () {
                location.href = "EmployeeSyncList.aspx";
            })
        }

        function AfterSegmentSync() {
            jAlert("Segment sync successfully.", "alert", function () {
                location.href = "EmployeeSyncList.aspx";
            })
        }

        function ViewSegmentLogData() {
            cSegmentGrid.Refresh();
            //cGrdEmployee.UnselectRows();
        }

    </script>

    <script>
        function ClickOnViewDetails(values) {
            $("#hdnCustomerID").val(values);
            cSegmentStatusGrid.Refresh();
            $('#modalSegmentStatus').modal("toggle");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Sync Customer to FSM</h3>
        </div>
    </div>
    <div class="form_main">

        <div class="clearfix pTop10">
            <div class="">
                <table>
                    <tr>
                        <td>
                            <asp:LinkButton ID="lnlDownloaderexcel" runat="server" CssClass="btn btn-info btn-radius pull-right mBot0" OnClick="lnlDownloaderexcel_Click">
                                <i class="fa fa-refresh" aria-hidden="true"></i>Sync Customer</asp:LinkButton>
                        </td>
                        <td>
                            <%--<% if (rights.CanExport)
                               { %> OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" --%>
                            <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-radius" AutoPostBack="true">
                                <asp:ListItem Value="0">Export to</asp:ListItem>
                                <asp:ListItem Value="1">PDF</asp:ListItem>
                                <asp:ListItem Value="2">XLS</asp:ListItem>
                                <asp:ListItem Value="3">RTF</asp:ListItem>
                                <asp:ListItem Value="4">CSV</asp:ListItem>
                            </asp:DropDownList>
                            <%--   <% } %>--%>                            
                        </td>
                        <td>
                            <button type="button" class="btn btn-warning btn-radius" data-toggle="modal" data-target="#modalSS" id="btnViewLog" onclick="ViewLogData();">View Log</button>
                        </td>
                        <td>
                            <asp:LinkButton ID="lnlSegmentSync" runat="server" CssClass="btn btn-info btn-radius pull-right mBot0" OnClick="lnlSegmentSync_Click">
                                <i class="fa fa-refresh" aria-hidden="true"></i>Sync Segment</asp:LinkButton>
                        </td>
                        <td>
                            <button type="button" class="btn btn-warning btn-radius" data-toggle="modal" data-target="#modalSegment" id="btnSegmentViewLog" onclick="ViewSegmentLogData();">Segment View Log</button>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="col-md-5">
            </div>
        </div>
        <table class="TableMain100">
            <tr>
                <td colspan="2" class="relative">
                    <dxe:ASPxGridView ID="GrdEmployee" runat="server" KeyFieldName="cnt_Id" AutoGenerateColumns="False" Settings-VerticalScrollableHeight="250" Settings-VerticalScrollBarMode="Auto"
                        Width="100%" ClientInstanceName="cGrdEmployee" SettingsBehavior-AllowFocusedRow="true" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false"
                        SettingsDataSecurity-AllowDelete="false" DataSourceID="EntityServerModeDataSource" OnHtmlDataCellPrepared="GrdEmployee_HtmlDataCellPrepared"
                        Settings-HorizontalScrollBarMode="Visible">
                        <SettingsSearchPanel Visible="True" Delay="5000" />
                        <%--<ClientSideEvents EndCallback="function(s, e) {GridEmployee_EndCallBack();}" />--%>
                        <SettingsBehavior AllowFocusedRow="true" ConfirmDelete="True" ColumnResizeMode="NextColumn" />
                        <Styles>
                            <Header SortingImageSpacing="5px" ImageSpacing="5px">
                            </Header>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                            <Row Wrap="true">
                            </Row>
                        </Styles>

                        <Columns>
                            <dxe:GridViewDataTextColumn Caption="Code" Visible="False" FieldName="cnt_Id"
                                VisibleIndex="0" FixedStyle="Left">
                                <PropertiesTextEdit DisplayFormatInEditMode="True">
                                </PropertiesTextEdit>
                                <CellStyle CssClass="gridcellleft" Wrap="False">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="1" Width="60" Caption=" " FixedStyle="Left" />

                            <dxe:GridViewDataTextColumn Caption="Unique ID" FieldName="Code"
                                VisibleIndex="2" Width="250px">
                                <CellStyle CssClass="gridcellleft" Wrap="False">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Caption="Name" FieldName="Name"
                                VisibleIndex="3" Width="350px">
                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Caption="Branch" FieldName="BranchName"
                                VisibleIndex="4" Width="150px">
                                <CellStyle CssClass="gridcellleft" Wrap="False">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="phf_phoneNumber" Caption="Phone Number" Width="110px">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="city_name" Caption="District" Width="150px">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="Activetype" Caption="Status" Width="100px">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn FieldName="SyncStatus" Caption="Sync Status" Width="8%" VisibleIndex="8" HeaderStyle-CssClass="colDisable">
                            </dxe:GridViewDataTextColumn>

                            <%--  <dxe:GridViewCommandColumn VisibleIndex="22" Width="180px" Caption="Actions" ButtonType="Image" HeaderStyle-HorizontalAlign="Center">
                                <CustomButtons>
                                     <a href="javascript:void(0);" onclick="ClickOnViewDetails('<%# Container.KeyValue %>')" id="a_ViewDetails" class="pad" title="View Segment Status" >
                                    <span class='ico editColor'><i class='fa fa-eye dat' aria-hidden='true'></i></span><span class='hidden-xs'>View Segment Status</span></a>
                                </CustomButtons>
                            </dxe:GridViewCommandColumn>--%>
                            <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="16" Width="1px">
                                <DataItemTemplate>
                                    <div class="floatedIcons">
                                        <div class='floatedBtnArea'>

                                            <a href="javascript:void(0);" onclick="ClickOnViewDetails('<%# Container.KeyValue %>')" id="a_ViewDetails" class="pad" title="View Segment Status">
                                                <span class='ico editColor'><i class='fa fa-eye dat' aria-hidden='true'></i></span><span class='hidden-xs'>View Segment Status</span></a>
                                        </div>
                                    </div>
                                </DataItemTemplate>
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                <HeaderTemplate><span>Actions</span></HeaderTemplate>
                                <EditFormSettings Visible="False"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>


                            <%--   <dxe:GridViewDataDateColumn Caption="Joining On" FieldName="DOJ"
                                VisibleIndex="7" Width="100px" ReadOnly="True">
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataDateColumn>

                            <dxe:GridViewDataDateColumn Caption="Date Of Confirmation" FieldName="Date_Of_Confirmation"
                                VisibleIndex="8" Width="100px" ReadOnly="True">
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataDateColumn>

                            <dxe:GridViewDataTextColumn Caption="Report To" FieldName="ReportTo"
                                VisibleIndex="9" Width="150px">
                                <CellStyle CssClass="gridcellleft" Wrap="False">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Caption="Designation" FieldName="Designation"
                                VisibleIndex="6" Width="150px">
                                <CellStyle CssClass="gridcellleft" Wrap="False">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>--%>


                            <%-- <dxe:GridViewCommandColumn Visible="False" ShowDeleteButton="true" VisibleIndex="16">
                                <DeleteButton Visible="True" Text="Delete">
                                </DeleteButton>
                            </dxe:GridViewCommandColumn>--%>
                        </Columns>
                        <ClientSideEvents RowClick="gridRowclick" />
                        <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                        <SettingsPager PageSize="10" ShowSeparators="True">
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                        </SettingsPager>
                        <%-- <SettingsCommandButton>
                            <DeleteButton ButtonType="Image" Image-Url="/assests/images/Delete.png">
                            </DeleteButton>
                        </SettingsCommandButton>--%>
                        <%--   <SettingsEditing Mode="PopupEditForm" PopupEditFormHorizontalAlign="Center" PopupEditFormModal="True"
                            PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="900px" EditFormColumnCount="3" />
                        <SettingsText PopupEditFormCaption="Add/ Modify Employee" ConfirmDelete="Are you sure to delete?" />--%>

                        <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                        <SettingsLoadingPanel Text="Please Wait..." />
                    </dxe:ASPxGridView>
                    <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
                        ContextTypeName="ServicveManagementDataClassesDataContext" TableName="V_STB_MoneyReceiptList" />
                </td>
            </tr>
        </table>
        <br />
        <asp:HiddenField ID="hdn_GridBindOrNotBind" runat="server" />
        <%--<asp:Button ID="BtnForExportEvent" runat="server" OnClick="cmbExport_SelectedIndexChanged" BackColor="#DDECFE" BorderStyle="None" Visible="false" />--%>
        <dxe:ASPxGridViewExporter ID="exporter" runat="server" GridViewID="GrdEmployee" Landscape="true" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>

    </div>



    <div class="modal fade" id="modalSS" role="dialog">
        <div class="modal-dialog fullWidth">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Customer Sync Log</h4>
                </div>
                <div class="modal-body">

                    <dxe:ASPxGridView ID="GvJvSearch" runat="server" AutoGenerateColumns="False" SettingsBehavior-AllowSort="true"
                        ClientInstanceName="cGvJvSearch" KeyFieldName="row_num" Width="100%" OnDataBinding="GvJvSearch_DataBinding" Settings-VerticalScrollBarMode="Auto" Settings-VerticalScrollableHeight="400">

                        <SettingsBehavior ConfirmDelete="false" ColumnResizeMode="NextColumn" />
                        <Styles>
                            <Header SortingImageSpacing="5px" ImageSpacing="5px"></Header>
                            <FocusedRow HorizontalAlign="Left" VerticalAlign="Top" CssClass="gridselectrow"></FocusedRow>
                            <LoadingPanel ImageSpacing="10px"></LoadingPanel>
                            <FocusedGroupRow CssClass="gridselectrow"></FocusedGroupRow>
                            <Footer CssClass="gridfooter"></Footer>
                        </Styles>
                        <Columns>
                            <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="0" FieldName="row_num" Caption="LogID" SortOrder="Descending">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="Customer_Name" Caption="Customer Name" Width="10%">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="Customer_Address" Caption="Customer Address" Width="13%">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="Customer_Phone" Width="10%" Caption="Customer Phone">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="USER_NAME" Width="10%" Caption="Sync By">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="SyncOn" Caption="Sync On" Width="12%">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                                <PropertiesTextEdit DisplayFormatString="dd/MM/yyyy hh:mm:ss"></PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="SyncStatus" Caption="Status" Width="8%" Settings-AllowAutoFilter="False">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="SyncFailedReason" Caption="Sync Failed Reason" Width="15%" Settings-AllowAutoFilter="False">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>

                        </Columns>
                        <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />
                        <SettingsSearchPanel Visible="false" />
                        <SettingsPager NumericButtonCount="200" PageSize="200" ShowSeparators="True" Mode="ShowPager">
                            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="200,400,600" />
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </SettingsPager>
                    </dxe:ASPxGridView>

                </div>

            </div>
        </div>
    </div>


    <div class="modal fade" id="modalSegment" role="dialog">
        <div class="modal-dialog fullWidth">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Segment Sync Log</h4>
                </div>
                <div class="modal-body">

                    <dxe:ASPxGridView ID="SegmentGrid" runat="server" AutoGenerateColumns="False" SettingsBehavior-AllowSort="true"
                        ClientInstanceName="cSegmentGrid" KeyFieldName="row_num" Width="100%" OnDataBinding="SegmentGrid_DataBinding" Settings-VerticalScrollBarMode="Auto" Settings-VerticalScrollableHeight="400">

                        <SettingsBehavior ConfirmDelete="false" ColumnResizeMode="NextColumn" />
                        <Styles>
                            <Header SortingImageSpacing="5px" ImageSpacing="5px"></Header>
                            <FocusedRow HorizontalAlign="Left" VerticalAlign="Top" CssClass="gridselectrow"></FocusedRow>
                            <LoadingPanel ImageSpacing="10px"></LoadingPanel>
                            <FocusedGroupRow CssClass="gridselectrow"></FocusedGroupRow>
                            <Footer CssClass="gridfooter"></Footer>
                        </Styles>
                        <Columns>
                            <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="0" FieldName="row_num" Caption="LogID" SortOrder="Descending">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="CustomerName" Caption="Customer Name" Width="10%">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="Segment_Name" Caption="Segment Name" Width="10%">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="SegemntCode" Caption="Segment Code" Width="10%">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="Segment_Address" Caption="Segment Address" Width="13%">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="Segment_Phone" Width="10%" Caption="Segment Phone">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="USER_NAME" Width="10%" Caption="Sync By">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="SyncOn" Caption="Sync On" Width="12%">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                                <PropertiesTextEdit DisplayFormatString="dd/MM/yyyy hh:mm:ss"></PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="SyncStatus" Caption="Status" Width="8%" Settings-AllowAutoFilter="False">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="8" FieldName="SyncFailedReason" Caption="Sync Failed Reason" Width="15%" Settings-AllowAutoFilter="False">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>

                        </Columns>
                        <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />
                        <SettingsSearchPanel Visible="false" />
                        <SettingsPager NumericButtonCount="200" PageSize="200" ShowSeparators="True" Mode="ShowPager">
                            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="200,400,600" />
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </SettingsPager>
                    </dxe:ASPxGridView>

                </div>

            </div>
        </div>
    </div>


    <div class="modal fade" id="modalSegmentStatus" role="dialog">
        <div class="modal-dialog fullWidth">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Segment Sync Status</h4>
                </div>
                <div class="modal-body">
                    <asp:HiddenField ID="hdnCustomerID" runat="server" />
                    <dxe:ASPxGridView ID="SegmentStatusGrid" runat="server" AutoGenerateColumns="False" SettingsBehavior-AllowSort="true"
                        ClientInstanceName="cSegmentStatusGrid" KeyFieldName="ID" Width="100%" OnDataBinding="SegmentStatusGrid_DataBinding"
                        Settings-VerticalScrollBarMode="Auto" Settings-VerticalScrollableHeight="400" OnHtmlDataCellPrepared="SegmentStatusGrid_HtmlDataCellPrepared">

                        <SettingsBehavior ConfirmDelete="false" ColumnResizeMode="NextColumn" />
                        <Styles>
                            <Header SortingImageSpacing="5px" ImageSpacing="5px"></Header>
                            <FocusedRow HorizontalAlign="Left" VerticalAlign="Top" CssClass="gridselectrow"></FocusedRow>
                            <LoadingPanel ImageSpacing="10px"></LoadingPanel>
                            <FocusedGroupRow CssClass="gridselectrow"></FocusedGroupRow>
                            <Footer CssClass="gridfooter"></Footer>
                        </Styles>
                        <Columns>
                            <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="0" FieldName="row_num" Caption="LogID" SortOrder="Descending">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>

                          <%--  <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="CustomerName" Caption="Customer Name" Width="10%">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>--%>

                            <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="Segment_Name" Caption="Segment Name" Width="10%">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="SegemntCode" Caption="Segment Code" Width="10%">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="Segment_Address" Caption="Segment Address" Width="13%">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="Segment_Phone" Width="10%" Caption="Segment Phone">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn FieldName="SyncStatus" Caption="Sync Status" Width="8%" VisibleIndex="5" HeaderStyle-CssClass="colDisable">
                            </dxe:GridViewDataTextColumn>

                        </Columns>
                        <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />
                        <SettingsSearchPanel Visible="false" />
                        <SettingsPager NumericButtonCount="200" PageSize="200" ShowSeparators="True" Mode="ShowPager">
                            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="200,400,600" />
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </SettingsPager>
                    </dxe:ASPxGridView>

                </div>

            </div>
        </div>
    </div>
</asp:Content>
