<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true"
    CodeBehind="ConsolidatedTransportedList.aspx.cs" Inherits="OpeningEntry.ERP.ConsolidatedTransportedList" EnableEventValidation="false" %>




<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function OnMoreInfoClick(CustomerId) {
            var url = 'ConsolidatedTransporter.aspx?TransporterId=' + CustomerId + "&branch=" + $("#ddl_Branch").val();

            window.location.href = url;
        }
        function OnAddButtonClick() {
            var url = 'ConsolidatedTransporter.aspx?key=' + 'ADD' + "&branch=" + $("#ddl_Branch").val();;
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
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div>
        <div class="panel-heading">
            <div class="panel-title">
                <h3>Consolidated  Transporter List</h3>
            </div>
        </div>
        <div class="form_main">
            <div class="clearfix">
                      <% if (rights.CanAdd)
                           { %>
                <a href="javascript:void(0);" id="a_aaddclick" onclick="OnAddButtonClick()" class="btn btn-primary"><span> <u>A</u>dd New</span> </a>
                  <% } %>
                      

                        <% if (rights.CanExport)
                                               { %>
                <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                    <asp:ListItem Value="0">Export to</asp:ListItem>
                    <asp:ListItem Value="1">PDF</asp:ListItem>
                    <asp:ListItem Value="2">XLS</asp:ListItem>
                    <asp:ListItem Value="3">RTF</asp:ListItem>
                    <asp:ListItem Value="4">CSV</asp:ListItem>
                </asp:DropDownList>
         
                  <% } %>

                    <asp:DropDownList ID="ddl_Branch" runat="server" Width="200px" TabIndex="1">
                    </asp:DropDownList>
            </div>
        </div>
        <div class="GridViewArea">
            <dxe:ASPxGridView ID="Grdconsolidatecustomer" runat="server" KeyFieldName="CustomerId" AutoGenerateColumns="False" ClientSideEvents-BeginCallback="Callback_EndCallback"
                Width="100%" ClientInstanceName="cGridconsolidatecustomer" OnDataBinding="GrdConsolidatedCustomer_DataBinding" OnCustomCallback="OpeningGrid_CustomCallback">
                <Columns>
                    <dxe:GridViewDataTextColumn Caption="Transporter Code" FieldName="cnt_UCC"
                        VisibleIndex="0" FixedStyle="Left" Width="40%">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Transporter Name" FieldName="CustomerName"
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
                
                  

                    
        <%--            <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="5" Width="5%">
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
                    </dxe:GridViewDataTextColumn>--%>



                    <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="6" Width="5%">
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


</asp:Content>