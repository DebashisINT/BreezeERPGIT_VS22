<%--================================================== Revision History ==========================================================================
1.0  12-05-2023    V2.0.38    Priti  25888 : Import module required for Consolidated Customer Opening
====================================================== Revision History ======================================================================--%>
<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true"
    CodeBehind="ConsolidatedVendorList.aspx.cs" Inherits="OpeningEntry.ERP.ConsolidatedVendorList" EnableEventValidation="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function OnMoreInfoClick(CustomerId) {
            var url = 'ConsolidatedVendor.aspx?VendorId=' + CustomerId + "&branch=" + $("#ddl_Branch").val();

            window.location.href = url;
        }
        function OnAddButtonClick() {
            var url = 'ConsolidatedVendor.aspx?key=' + 'ADD'+"&branch=" + $("#ddl_Branch").val();;
            window.location.href = url;
        }
        function OnaggedinfoClick(CustomerId) {
            popuptaggeddocument.Show();
            cGridconsolidatetaggedcustomer.PerformCallback('BindComponentGrid' + '~' + CustomerId);
        }
        function TaggedAfterHide(s, e) {
            popuptaggeddocument.Hide();
        }


        $(function () {
            $("#ddl_Branch").on('change', function () {

                if ($("#ddl_Branch").val() == '0') {

                    $("#a_aaddclick").attr('style', 'display:none;')
                }

                else {
                    $("#a_aaddclick").attr('style', 'display:inline-block;')
                }

                cGridconsolidatecustomer.PerformCallback('TemporaryData~' + 0);
            })
        });
        function Callback_EndCallback() {

            // alert('');
            $("#drdExport").val(0);
        }
    </script>
    <%-- Rev 1.0--%>
     <script type="text/javascript">

         function ImportUpdatePopOpen(e) {
             $("#modalimport").modal('show');
         }
         function ViewLogData() {
             cGvLogSearch.Refresh();
         }
         function ShowLogData(haslog) {
             ;
             $('#btnViewLog').click();
         }
     </script>
  <%--  Rev 1.0 End--%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div>
        <div class="panel-heading">
            <div class="panel-title">
                <h3>Consolidated  Vendor</h3>
            </div>
        </div>
        <div class="form_main">
            <div class="clearfix">
                      <% if (rights.CanAdd)
                           { %>
                <a href="javascript:void(0);" id="a_aaddclick" onclick="OnAddButtonClick()" class="btn btn-success btn-radius"><span class="btn-icon"><i class="fa fa-plus" ></i></span><span> <u>A</u>dd New</span> </a>
                  <% } %>
                      

                        <% if (rights.CanExport)
                                               { %>
                <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-radius" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                    <asp:ListItem Value="0">Export to</asp:ListItem>
                    <asp:ListItem Value="1">PDF</asp:ListItem>
                    <asp:ListItem Value="2">XLS</asp:ListItem>
                    <asp:ListItem Value="3">RTF</asp:ListItem>
                    <asp:ListItem Value="4">CSV</asp:ListItem>
                </asp:DropDownList>
         
                  <% } %>

                    <asp:DropDownList ID="ddl_Branch" runat="server" Width="200px" TabIndex="1">
                    </asp:DropDownList>
                  <%-- Rev 1.0--%>
                <asp:LinkButton ID="lnlDownloaderexcel" runat="server" OnClick="lnlDownloaderexcel_Click" CssClass="btn btn-info btn-radius pull-rigth mBot0">Download Format</asp:LinkButton>
                <button type="button" onclick="ImportUpdatePopOpen();" class="btn btn-primary btn-radius">Import(Add/Update)</button>
                <button type="button" class="btn btn-warning btn-radius" data-toggle="modal" data-target="#modalSS" id="btnViewLog" onclick="ViewLogData();">View Log</button>
               <%-- Rev 1.0 End--%>
            </div>
        </div>
        <div class="GridViewArea">
            <dxe:ASPxGridView ID="Grdconsolidatecustomer" runat="server" KeyFieldName="CustomerId" AutoGenerateColumns="False" ClientSideEvents-BeginCallback="Callback_EndCallback"
                Width="100%" ClientInstanceName="cGridconsolidatecustomer" OnDataBinding="GrdConsolidatedCustomer_DataBinding" OnCustomCallback="OpeningGrid_CustomCallback">
                <Columns>
                    <dxe:GridViewDataTextColumn Caption="Vendor Code" FieldName="cnt_UCC"
                        VisibleIndex="0" FixedStyle="Left" Width="40%">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Vendor Name" FieldName="CustomerName"
                        VisibleIndex="1" FixedStyle="Left" Width="40%">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Credit Balance" FieldName="Credit"
                        VisibleIndex="2" FixedStyle="Left" Width="20%">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Debit Balance" FieldName="Debit"
                        VisibleIndex="3" FixedStyle="Left" Width="20%">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Net Amount" FieldName="NetAmt"
                        VisibleIndex="4" FixedStyle="Left" Width="15%">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                
                  
                    
                <dxe:GridViewDataTextColumn Caption="Net Outstanding Amount" FieldName="NetOSAmt"
                        VisibleIndex="5" FixedStyle="Left" Width="15%">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                
                  

                    
                    <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="6" Width="5%">
                        <DataItemTemplate>
                               <% if (rights.CanEdit)
                                  { %>
                            <a href="javascript:void(0);" onclick="OnaggedinfoClick('<%#Eval("CustomerId")%>')" class="pad" title="Tagged Documents">
                                <img src="/assests/images/attachment.png" /></a>

                             <% } %>
                        </DataItemTemplate>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <CellStyle HorizontalAlign="Center"></CellStyle>
                        <HeaderTemplate><span>Tagged Documents</span></HeaderTemplate>
                        <EditFormSettings Visible="False"></EditFormSettings>
                    </dxe:GridViewDataTextColumn>



                    <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="7" Width="5%">
                        <DataItemTemplate>
                               <% if (rights.CanEdit)
                                        { %>
                            <a href="javascript:void(0);" onclick="OnMoreInfoClick('<%#Eval("CustomerId")%>')" class="pad" title="Edit">
                                <img src="/assests/images/Edit.png" /></a>
                               <% } %>
                        </DataItemTemplate>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <CellStyle HorizontalAlign="Center"></CellStyle>
                        <HeaderTemplate><span>Actions</span></HeaderTemplate>
                        <EditFormSettings Visible="False"></EditFormSettings>
                    </dxe:GridViewDataTextColumn>
                </Columns>
                <ClientSideEvents />
                <SettingsPager NumericButtonCount="20" PageSize="10" ShowSeparators="True" Mode="ShowPager">
                    <FirstPageButton Visible="True">
                    </FirstPageButton>
                    <LastPageButton Visible="True">
                    </LastPageButton>
                        <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                </SettingsPager>
                <SettingsSearchPanel Visible="True" />
                <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                <SettingsLoadingPanel Text="Please Wait..." />
                <SettingsPager Position="Bottom" NumericButtonCount="20" PageSize="20" ShowSeparators="True" AlwaysShowPager="True">
                    </SettingsPager>
            </dxe:ASPxGridView>
        </div>
    </div>
    <div style="display: none">
        <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A3" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
    </div>

    
    <dxe:ASPxPopupControl ID="ASPXPopupControl2" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popuptaggeddocument" Height="500px"
        Width="300px" HeaderText="Tagged Documents" Modal="true" AllowResize="true">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">

                <div class="GridViewArea">
            <dxe:ASPxGridView ID="grid_taggeddocuments" runat="server" AutoGenerateColumns="False"
                Width="100%" ClientInstanceName="cGridconsolidatetaggedcustomer" OnDataBinding="GrdConsolidatedtagged_DataBinding" OnCustomCallback="OpeningGrid_CustomCallbacktaggeddoc">
                <columns>
                    <dxe:GridViewDataTextColumn Caption="Document Number" FieldName="Doc_No"
                        VisibleIndex="0" FixedStyle="Left" Width="40%">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                  
 
                </columns>
                <clientsideevents />
                <settingspager numericbuttoncount="20" pagesize="10" showseparators="True" mode="ShowPager">
                    <FirstPageButton Visible="True">
                    </FirstPageButton>
                    <LastPageButton Visible="True">
                    </LastPageButton>
                       <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                </settingspager>
                <settingssearchpanel visible="True" />
                <settings showgrouppanel="True" showstatusbar="Hidden" showhorizontalscrollbar="False" showfilterrow="true" showfilterrowmenu="true" />
                <settingsloadingpanel text="Please Wait..." />
                <settingspager position="Bottom" numericbuttoncount="20" pagesize="20" showseparators="True" alwaysshowpager="True">
                    </settingspager>
            </dxe:ASPxGridView>
        </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>

        <ClientSideEvents CloseUp="TaggedAfterHide" />
    </dxe:ASPxPopupControl>
     <%--  Rev 1.0--%>
      <div class="modal fade" id="modalimport" role="dialog">
        <div class="modal-dialog VerySmall">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Select File to Import (Add/Update)</h4>
                </div>
                <div class="modal-body">

                    <div class="col-md-12">
                        <div id="divproduct">

                            <div>
                                <asp:FileUpload ID="OFDBankSelect" accept=".xls,.xlsx" runat="server" Width="100%" />
                                <div class="pTop10  mTop5">
                                    <asp:Button ID="BtnSaveexcel" runat="server" Text="Import(Add/Update)"  OnClick="BtnSaveexcel_Click" CssClass="btn btn-primary" />
                                </div>
                            </div>
                        </div>

                    </div>
                </div>

            </div>
        </div>
    </div>
      <div class="modal fade" id="modalSS" role="dialog">
        <div class="modal-dialog fullWidth">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Customer Import Log</h4>
                </div>
                <div class="modal-body">

                    <dxe:ASPxGridView ID="GvLogSearch" runat="server" AutoGenerateColumns="False" SettingsBehavior-AllowSort="true"
                        ClientInstanceName="cGvLogSearch" KeyFieldName="LOG_ID" Width="100%" OnDataBinding="GvLogSearch_DataBinding" Settings-VerticalScrollBarMode="Auto" Settings-VerticalScrollableHeight="400">

                        <SettingsBehavior ConfirmDelete="false" ColumnResizeMode="NextColumn" />
                        <Styles>
                            <Header SortingImageSpacing="5px" ImageSpacing="5px"></Header>
                            <FocusedRow HorizontalAlign="Left" VerticalAlign="Top" CssClass="gridselectrow"></FocusedRow>
                            <LoadingPanel ImageSpacing="10px"></LoadingPanel>
                            <FocusedGroupRow CssClass="gridselectrow"></FocusedGroupRow>
                            <Footer CssClass="gridfooter"></Footer>
                        </Styles>
                        <Columns>
                            <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="0" FieldName="LOG_ID" Caption="LogID" SortOrder="Descending">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="CREATEDON" Caption="Date" Width="10%">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                                <PropertiesTextEdit DisplayFormatString="dd/MM/yyyy"></PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="LOG_DOCNO" Caption="Document Number" Width="10%">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                                <PropertiesTextEdit DisplayFormatString="dd/MM/yyyy"></PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="LOG_LOOPNUMBER" Caption="Row Number" Width="13%">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="cnt_firstName" Width="8%" Caption="Vendor Name">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="LOG_FILENAME" Width="14%" Caption="File Name">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                                <PropertiesTextEdit DisplayFormatString="dd/MM/yyyy"></PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="LOG_DESCRIPTION" Caption="Description" Width="10%" Settings-AllowAutoFilter="False">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="LOG_STASTUS" Caption="Status" Width="14%" Settings-AllowAutoFilter="False">
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
     <%-- Rev 1.0 End--%>

</asp:Content>